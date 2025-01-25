using CMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMS.Infractructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Hall> Halls { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Genre)
            .WithMany(g => g.Movies)
            .HasForeignKey(m => m.GenreId);

        modelBuilder.Entity<Session>()
            .HasOne(s => s.Movie)
            .WithMany(m => m.Sessions)
            .HasForeignKey(s => s.MovieId);

        modelBuilder.Entity<Session>()
            .HasOne(s => s.Hall)
            .WithMany(h => h.Sessions)
            .HasForeignKey(s => s.HallId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Session)
            .WithMany(s => s.Tickets)
            .HasForeignKey(t => t.SessionId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Payment)
            .WithMany()
            .HasForeignKey(t => t.PaymentId);
	
		modelBuilder.Entity<Role>().HasData(
     		new Role { RoleId = 1, RoleName = "Admin" },
     		new Role { RoleId = 2, RoleName = "User" }
 		);
    }
}
