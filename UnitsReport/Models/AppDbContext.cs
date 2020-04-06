using Microsoft.EntityFrameworkCore;

namespace UnitsReport.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbQuery<UnitIntervalData> UnitIntervalData { get; set; }
    }
}
