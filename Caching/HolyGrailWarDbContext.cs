using Microsoft.EntityFrameworkCore;

namespace HolyGrailWarModel
{
    public class HolyGrailWarDbContext : DbContext
    {
        public HolyGrailWarDbContext(DbContextOptions<HolyGrailWarDbContext> options) : base(options)
        {
        }

        public DbSet<Master> Masters { get; set; }
    }

    public class Master
    {
        public int MasterId { get; set; }
        public string Name { get; set; }
    }
}
