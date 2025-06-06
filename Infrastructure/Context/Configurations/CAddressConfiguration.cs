﻿namespace LeUs.Infrastructure.Context.Configurations;

public class CAddressConfiguration : IEntityTypeConfiguration<CAddress>
{
    public void Configure(EntityTypeBuilder<CAddress> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(50);
        builder.Property(p => p.Company).HasMaxLength(50);
        builder.Property(p => p.AddressLine1).HasMaxLength(50);
        builder.Property(p => p.AddressLine2).HasMaxLength(50);
        builder.Property(p => p.County).HasMaxLength(50);
        builder.Property(p => p.City).HasMaxLength(50);
        builder.Property(p => p.State).HasMaxLength(50);
        builder.Property(p => p.CountryCode).HasMaxLength(2);
        builder.Property(p => p.Zip).HasMaxLength(50);
        builder.Property(p => p.Phone).HasMaxLength(50);
        builder.Property(p => p.PhoneNumberExt).HasMaxLength(50);
        builder.Property(p => p.Email).HasMaxLength(50);
        builder.Property(p => p.IdNo).HasMaxLength(50);
        builder.Property(p => p.TaxNo).HasMaxLength(50);
        builder.Property(p => p.TaxNoType).HasMaxLength(50);
        builder.Property(p => p.TaxNoIssuerCountryCode).HasMaxLength(50);
    }
}