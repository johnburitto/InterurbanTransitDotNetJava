using Flight.API.Entities;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flight.API.Configurations
{
    public class FlightRouteConfiguration : IEntityTypeConfiguration<FlightRoute>
    {
        public void Configure(EntityTypeBuilder<FlightRoute> builder)
        {
            builder.Property(route => route.Id)
                   .UseIdentityColumn()
            .IsRequired();

            builder.Property(route => route.FromCity)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(route => route.ToCity)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(route => route.DepartureTime)
                   .HasColumnType("time")
                   .IsRequired();

            builder.Property(route => route.ArrivalTime)
                   .HasColumnType("time")
                   .IsRequired();

            builder.Property(route => route.CreatedAt)
                   .IsRequired();

            builder.Property(route => route.UpdatedAt)
                   .IsRequired();
        }
    }
}
