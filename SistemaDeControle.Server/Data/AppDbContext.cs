using Microsoft.EntityFrameworkCore;
using sistemadecontrole.Server.Models;

namespace sistemadecontrole.Server.Data
{
    public class AppDbContext : DbContext //Herda de DbContext para interagir com o BD
    {
        //Constructor onde permite a injecao de dependencia via Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) //Options define a config do BD
        {
        }

        //Representa as tabelas do BD
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<VinculoProjeto> Vinculos { get; set; }

        //configurar detalhes dos relacionamentos entre tabelas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamento 1:N - Projeto -> Coordenador (Usuario)
            modelBuilder.Entity<Projeto>()
                .HasOne(p => p.Coordenador)
                .WithMany(u => u.ProjetosCoordenados)
                .HasForeignKey(p => p.CoordenadorId)
                .OnDelete(DeleteBehavior.Restrict);

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
