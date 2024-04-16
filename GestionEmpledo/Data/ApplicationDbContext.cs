using Microsoft.EntityFrameworkCore;
using GestionEmpledo.Models;

namespace GestionEmpledo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones adicionales de tu modelo, como relaciones, restricciones, etc.
        }
    }
}
