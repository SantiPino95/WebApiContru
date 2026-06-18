using Microsoft.EntityFrameworkCore;
using MiWebApi.Models;

namespace MiWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Obra> Obras { get; set; }
    }
}
