using fitnessAPI.Entities;
using fitnessAPI.Helpers;
using fitnessAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
	//This is to generate the Default UI of Swagger Documentation
	swagger.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "JWT Token Authentication API",
		Description = ".NET 8 Web API"
	});
	// To Enable authorization using Swagger (JWT)
	swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
	});
	swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						  new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "Bearer"
								}
							},
							new string[] {}

					}
				});
});

builder.Services.AddDbContext<UsersContext>(options =>
{
    // MySQL adatforrás hozzáadása
    options.UseMySql(builder.Configuration.GetConnectionString("Local"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Local")));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
