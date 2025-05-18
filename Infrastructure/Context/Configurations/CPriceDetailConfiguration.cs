namespace LeUs.Infrastructure.Context.Configurations;

public class CPriceDetailConfiguration : IEntityTypeConfiguration<CPriceDetail>
{
    public void Configure(EntityTypeBuilder<CPriceDetail> builder)
    {
        builder.Property(p => p.Price).HasMaxLength(2000);
        builder.Property(p => p.ServiceCode).HasMaxLength(30);
    }
}