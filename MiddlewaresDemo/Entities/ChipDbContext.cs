using Microsoft.EntityFrameworkCore;

namespace MiddlewaresDemo.Entities
{
    public class ChipDbContext:DbContext
    {
        public ChipDbContext(DbContextOptions<ChipDbContext> options) : base(options)
        {
        }
    }
}
