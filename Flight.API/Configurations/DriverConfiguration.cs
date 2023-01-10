using Flight.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flight.API.Configurations
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.Property(driver => driver.Id)
                   .UseIdentityColumn()
                   .IsRequired();

            builder.Property(driver => driver.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(driver => driver.Experience)
                   .IsRequired();

            builder.Property(driver => driver.Category)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(driver => driver.DateOfBirth)
                   .HasColumnType("date")
                   .IsRequired();

            builder.Property(driver => driver.CreatedAt)
                   .IsRequired();

            builder.Property(driver => driver.UpdatedAt)
                   .IsRequired();
        }
    }
}
