using Microsoft.EntityFrameworkCore;
using SistemaDeControle.Server.Models;

namespace SistemaDeControle.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<VinculoProjeto> Vinculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define a chave primária composta
            modelBuilder.Entity<VinculoProjeto>()
                .HasKey(v => new { v.ProjetoId, v.UsuarioId });

            // Relacionamento N:N com dados extras - Usuario <-> Projeto via VinculoProjeto
            modelBuilder.Entity<VinculoProjeto>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.ProjetosParticipando)
                .HasForeignKey(v => v.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VinculoProjeto>()
                .HasOne(v => v.Projeto)
                .WithMany(p => p.Vinculos)
                .HasForeignKey(v => v.ProjetoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
