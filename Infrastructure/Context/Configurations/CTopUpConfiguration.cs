using CTopUp = LeUs.Domain.Data.CTopUp;

namespace LeUs.Infrastructure.Context.Configurations;

public class CTopUpConfiguration : IEntityTypeConfiguration<CTopUp>
{
    public void Configure(EntityTypeBuilder<CTopUp> builder)
    {
        builder.Property(p => p.UserId).HasMaxLength(36);
        builder.Property(p => p.Currency).HasMaxLength(3);
        builder.Property(p => p.Note).HasMaxLength(500);
        builder.Property(p => p.TransactionId).HasMaxLength(255);
        builder.HasIndex(u => new { u.IsActive, u.RequestDate, u.Status, u.UserId });
    }
}