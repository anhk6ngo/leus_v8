
namespace LeUs.Infrastructure.Context.Configurations;

public class CHistoryLabelConfiguration : IEntityTypeConfiguration<CHistoryLabel>
{
    public void Configure(EntityTypeBuilder<CHistoryLabel> builder)
    {
        builder.Property(p => p.ReferenceId).HasMaxLength(36);
        builder.Property(p => p.Request).HasColumnType("text");
        builder.Property(p => p.Response).HasColumnType("text");
        builder.HasIndex(u => new { u.ReferenceId});
    }
}