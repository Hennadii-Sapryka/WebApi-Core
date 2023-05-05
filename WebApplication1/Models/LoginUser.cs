namespace WebApi.Models
{
    public class LoginUser
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; } = null;
        public string? Password { get; set; }
        public string? Role { get;set; }
    }
}
