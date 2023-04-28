namespace WebApi.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string? NameOfSkill { get; set; }
        public int? ElectricianId { get; set; }
        public Electrician? Electrician{ get; set; }

    }
}
