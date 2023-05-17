using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public  string? WebSite { get; set; }
        public  string? ServicesOfBrands { get; set; }
        public  string? Fee { get; set; }
        public  string? FeePerHour { get; set; }
        public  string? RadiusMaxToWork { get; set; }
        public  string? SpecialTerms { get; set; }

        public  bool IsApproved { get; set; } = false;
        public  bool IsOwnBusiness { get; set; } = false;
        public  bool IsTechnicians { get; set; } = false;
        public  string? Role { get; set; }
        public string? PasswordHash { get; set; }
        [NotMapped]
        public ICollection<string>? Skills { get; set; }
        [NotMapped]
        public ICollection<string>? Feedbacks { get; set; } = new List<string>();
        [NotMapped]
        public ICollection<Location>? Locations { get; set; } = new List<Location>();
    }
}
