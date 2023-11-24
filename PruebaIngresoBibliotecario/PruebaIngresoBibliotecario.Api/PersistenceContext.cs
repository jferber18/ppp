using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PruebaIngresoBibliotecario.Api.Models;
using System.Threading.Tasks;

namespace PruebaIngresoBibliotecario.Infrastructure
{
    public class PersistenceContext : DbContext
    {

        private readonly IConfiguration Config;

        public PersistenceContext(DbContextOptions<PersistenceContext> options, IConfiguration config) : base(options)
        {
            Config = config;
        }

        public async Task CommitAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }

        public DbSet<PrestamoModel> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Restrinciones Prestamo
            modelBuilder.Entity<PrestamoModel>(x => x.Property(x => x.Id).IsRequired());
            modelBuilder.Entity<PrestamoModel>(x => x.Property(x => x.IdentificacionUsuario).IsRequired().HasMaxLength(10));
            modelBuilder.Entity<PrestamoModel>(x => x.Property(x => x.TipoUsuario).IsRequired());

            modelBuilder.HasDefaultSchema(Config.GetValue<string>("SchemaName"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
