namespace WebApplication1.Models
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public IEnumerable<User> userList  { get; set; }
    }
}
