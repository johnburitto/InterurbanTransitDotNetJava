using Flight.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flight.API.Configurations
{
    public class TransportConfiguration : IEntityTypeConfiguration<Transport>
    {
        public void Configure(EntityTypeBuilder<Transport> builder)
        {
            builder.Property(transport => transport.Id)
                   .UseIdentityColumn()
                   .IsRequired();

            builder.Property(transport => transport.Barnd)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(transport => transport.Model)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(transport => transport.Category)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(transport => transport.ReleaseDate)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(transport => transport.CreatedAt)
                   .IsRequired();

            builder.Property(transport => transport.UpdatedAt)
                   .IsRequired();
        }
    }
}
