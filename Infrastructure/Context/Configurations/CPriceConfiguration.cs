namespace LeUs.Infrastructure.Context.Configurations;

public class CPriceConfiguration : IEntityTypeConfiguration<CPrice>
{
    public void Configure(EntityTypeBuilder<CPrice> builder)
    {
        builder.ToTable("CPrice");
        builder.Property(p => p.FromDate).HasColumnType("timestamp(6)");
        builder.Property(p => p.ToDate).HasColumnType("timestamp(6)");
        builder.Property(p => p.ServiceId).HasMaxLength(36);
        builder.Property(p => p.PriceCode).HasMaxLength(50);
        builder.Property(p => p.PriceName).HasMaxLength(255);
        builder.Property(p => p.Currency).HasMaxLength(3);
        builder.Property(p => p.CustomerId).HasMaxLength(2000);
        builder.OwnsMany(s => s.Zones, u => { u.ToJson(); });
    }
}