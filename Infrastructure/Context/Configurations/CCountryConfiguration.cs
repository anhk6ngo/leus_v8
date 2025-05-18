namespace LeUs.Infrastructure.Context.Configurations;

public class CCountryConfiguration : IEntityTypeConfiguration<CCountry>
{
    public void Configure(EntityTypeBuilder<CCountry> builder)
    {
        builder.Property(p => p.CountryCode).HasMaxLength(2);
        builder.Property(p => p.CountryName).HasMaxLength(255);
    }
}