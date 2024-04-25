namespace fitnessAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
    }
}
