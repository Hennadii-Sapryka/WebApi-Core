namespace WebApi.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public int FeedBack { get; set; }
        public int? ElectricianId { get; set; }
        public Electrician? Electrician { get; set; }

    }
}
