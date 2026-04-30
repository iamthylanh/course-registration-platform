using Microsoft.EntityFrameworkCore;
using HTDangKyKhoaHocOnline.Models;
namespace HTDangKyKhoaHocOnline.Data
{
    public class SystemDBContext : DbContext
    {
        public SystemDBContext(DbContextOptions<SystemDBContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
    }
}
