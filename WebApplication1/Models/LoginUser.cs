namespace WebApi.Models
{
    public class LoginUser
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; } = null;
        public string? PasswordHash { get; set; }
    }
}
