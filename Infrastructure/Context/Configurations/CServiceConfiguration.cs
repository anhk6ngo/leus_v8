namespace LeUs.Infrastructure.Context.Configurations;

public class CServiceConfiguration : IEntityTypeConfiguration<CService>
{
    public void Configure(EntityTypeBuilder<CService> builder)
    {
        builder.Property(p => p.ServiceCode).HasMaxLength(36);
        builder.Property(p => p.ServiceName).HasMaxLength(255);
        builder.Property(p => p.ApiName).HasMaxLength(255);
    }
}