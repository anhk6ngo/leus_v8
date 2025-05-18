namespace LeUs.Infrastructure.Context.Configurations;

public class CCustomerConfiguration : IEntityTypeConfiguration<CCustomer>
{
    public void Configure(EntityTypeBuilder<CCustomer> builder)
    {
        builder.Property(p => p.Code).HasMaxLength(50);
        builder.Property(p => p.Name).HasMaxLength(255);
        builder.Property(p => p.Email).HasMaxLength(255);
        builder.Property(p => p.ContactPerson).HasMaxLength(255);
        builder.Property(p => p.Address).HasMaxLength(500);
        builder.Property(p => p.Phone).HasMaxLength(50);
        builder.Property(p => p.TaxNo).HasMaxLength(50);
        builder.Property(p => p.BankAccount).HasMaxLength(50);
        builder.Property(p => p.BankName).HasMaxLength(255);
        builder.Property(p => p.SignContract).HasColumnType("timestamp(6)");
        builder.HasIndex(u => u.Email);
    }
}