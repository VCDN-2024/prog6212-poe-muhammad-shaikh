using CMCSApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCSApp1.Models
{
    public class CMCSContext : DbContext
    {
        public CMCSContext(DbContextOptions<CMCSContext> options) : base(options) { }

        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<SupportingDocument> SupportingDocuments { get; set; }
    }
}
