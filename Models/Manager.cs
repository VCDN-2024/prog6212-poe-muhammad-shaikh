namespace CMCSApp1.Models
{
    public class Manager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Claim> Claims { get; set; }
    }
}
