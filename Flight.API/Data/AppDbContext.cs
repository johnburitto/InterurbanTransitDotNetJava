using Flight.API.Configurations;
using Flight.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flight.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<FlightRoute> Routes { get; set; }
        public DbSet<FlightEntity> Flights { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new DriverConfiguration());
            modelBuilder.ApplyConfiguration(new FlightEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FlightRouteConfiguration());
            modelBuilder.ApplyConfiguration(new TransportConfiguration());
        }
    }
}
