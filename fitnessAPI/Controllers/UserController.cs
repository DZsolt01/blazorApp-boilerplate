using fitnessAPI.Entities;
using fitnessAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fitnessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UsersContext _usersContext;

        public UserController(UsersContext userContext)
        {
            _usersContext = userContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if(_usersContext.Users == null)
            {
                return NotFound();
            }
            return await _usersContext.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            if (_usersContext.Users == null)
            {
                return NotFound();
            }
            var user = await _usersContext.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _usersContext.Users.Add(user);
            await _usersContext.SaveChangesAsync();
            return CreatedAtAction(nameof(User), new {id = user.Id}, user);
        }
    }
}
