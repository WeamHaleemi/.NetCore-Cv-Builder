namespace WebApplication1.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nationality nationality { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public List<Skill> skills { get; set; }=new List<Skill>();
        public DateTime BirthDate { get; set; }
        public int Grade { get; set; }

    }
}
