using GameDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace GameDashboard.Data;

public class GameDashboardContext : DbContext
{
    public DbSet<Jogo> Jogos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=gamedashboard.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Jogo>(entity =>
        {
            entity.HasKey(j => j.Id);
            entity.Property(j => j.Nome).IsRequired().HasMaxLength(300);
            entity.Property(j => j.Slug).HasMaxLength(300);
            entity.HasIndex(j => j.Slug).IsUnique();
        });
    }
}