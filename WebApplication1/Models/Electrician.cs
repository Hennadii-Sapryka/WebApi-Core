using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Electrician
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }    
        public string? Email { get; set; }
        public string? WebSite { get; set; }
        [Column("Brands")]
        public string? ServicesOfBrends { get; set; }
        public string? Fee { get; set; }
        public string? FeePerHour { get; set; }
        public string? RadiusMaxToWork { get; set; }
        public string? SpecialTerms { get; set; }

        public bool IsAproved { get; set; } = false;
        public bool IsOwnShop { get; set; } = false;
        public string? Role { get; set; }
        [NotMapped]
        public List<string> ScillsList { get; set; } = new();
        [NotMapped]
        public List<string> Feedbacks { get; set; } = new();
    }
}
