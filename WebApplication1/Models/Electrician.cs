namespace WebApi.Models
{
    public class Electrician
    {
        public int Id { get; set; }
        public string? Name { get; set; }    
        public string? Email { get; set; }
        public string? WebSite { get; set; }
        public string? ServicesOfBrends { get; set; }
        public string? Fee { get; set; }
        public string? FeePerHour { get; set; }
        public string? RadiusMaxToWork { get; set; }
        public string? SpecialTerms { get; set; }

        public bool IsAproved { get; set; } = false;
        public bool IsOwnShop { get; set; } = false;

        public List<Skill>? ScillsList { get; set; }

       public List<Feedback>? Feedbacks { get; set; }
    }
}
