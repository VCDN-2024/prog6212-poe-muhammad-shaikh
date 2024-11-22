using System.Collections.Generic;

namespace CMCSApp1.Models
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Claim> Claims { get; set; } = new List<Claim>(); // Add a navigation property for claims
    }
}
