using Microsoft.EntityFrameworkCore;

namespace YrLawyerWeb.Models
{
    public class YrLawyerContext : DbContext
    {
        public YrLawyerContext(DbContextOptions<YrLawyerContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ClientService> ClientServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientService>()
                .HasOne(cs => cs.Client)
                .WithMany(c => c.ClientServices)
                .HasForeignKey(cs => cs.ClientId);

            modelBuilder.Entity<ClientService>()
                .HasOne(cs => cs.Service)
                .WithMany(s => s.ClientServices)
                .HasForeignKey(cs => cs.ServiceId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Client)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.ClientId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Service)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(f => f.ServiceId);
        }
    }
}
