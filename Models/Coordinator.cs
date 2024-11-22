namespace CMCSApp1.Models
{
    public class Coordinator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }
}
