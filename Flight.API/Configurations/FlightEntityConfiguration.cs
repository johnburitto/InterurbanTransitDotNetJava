using Flight.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flight.API.Configurations
{
    public class FlightEntityConfiguration : IEntityTypeConfiguration<FlightEntity>
    {
        public void Configure(EntityTypeBuilder<FlightEntity> builder)
        {
            builder.Property(flight => flight.Id)
                   .UseIdentityColumn()
                   .IsRequired();

            builder.Property(flight => flight.StartDay)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(flight => flight.EndDay)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(flight => flight.CreatedAt)
                   .IsRequired();

            builder.Property(flight => flight.UpdatedAt)
                   .IsRequired();

            builder.HasOne(flight => flight.Driver)
                   .WithMany(driver => driver.Flights)
                   .HasForeignKey(flight => flight.DriverId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Flights_DriverId");            
            
            builder.HasOne(flight => flight.Transport)
                   .WithMany(transport => transport.Flights)
                   .HasForeignKey(flight => flight.TransportId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Flights_TransportId");            
            
            builder.HasOne(flight => flight.Route)
                   .WithMany(route => route.Flights)
                   .HasForeignKey(flight => flight.RouteId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Flights_RouteId");
        }
    }
}
