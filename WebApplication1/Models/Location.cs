namespace WebApi.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public User? User { get; set; }

    }
}