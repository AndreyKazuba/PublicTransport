using PublicTransport.Data.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace PublicTransport.Data
{
    public class PublicTransportDbContext : DbContext
    {
        public PublicTransportDbContext() { }

        public PublicTransportDbContext(DbContextOptions<PublicTransportDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=PublicTransport;Trusted_Connection=True;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Transport> Transports { get; set; }
    }
}
