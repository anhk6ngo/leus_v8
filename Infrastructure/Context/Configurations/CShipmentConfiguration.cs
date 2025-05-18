namespace LeUs.Infrastructure.Context.Configurations;

public class CShipmentConfiguration : IEntityTypeConfiguration<CShipment>
{
    public void Configure(EntityTypeBuilder<CShipment> builder)
    {
        builder.Property(p => p.ReferenceId).HasMaxLength(30);
        builder.Property(p => p.ReferenceId2).HasMaxLength(30);
        builder.Property(p => p.ReferenceId3).HasMaxLength(30);
        builder.Property(p => p.TrackingNo).HasMaxLength(30);
        builder.Property(p => p.EntryPoint).HasMaxLength(30);
        builder.Property(p => p.ServiceCode).HasMaxLength(30);
        builder.Property(p => p.ServiceCode1).HasMaxLength(30);
        builder.Property(p => p.DutyType).HasMaxLength(3);
        builder.Property(p => p.DimensionUnit).HasMaxLength(4);
        builder.Property(p => p.FbaCode).HasMaxLength(10);
        builder.Property(p => p.FbaShipmentId).HasMaxLength(20);
        builder.Property(p => p.FbaPoId).HasMaxLength(20);
        builder.Property(p => p.WeightUnit).HasMaxLength(30);
        builder.Property(p => p.CustomsCurrency).HasMaxLength(30);
        builder.Property(p => p.BoxQty).HasMaxLength(30);
        builder.Property(p => p.SignatureRequired).HasMaxLength(30);
        builder.Property(p => p.PackageType).HasMaxLength(30);
        builder.Property(p => p.BatteryType).HasMaxLength(30);
        builder.Property(p => p.CustomerId).HasMaxLength(36);
        builder.Property(p => p.PromotionCode).HasMaxLength(30);
        builder.Property(p => p.PriceCode).HasMaxLength(50);
        builder.Property(p => p.ApiName).HasMaxLength(50);
        builder.Property(p => p.ApiName1).HasMaxLength(50);
        builder.Property(p => p.ShipmentId).HasMaxLength(50);
        builder.Property(p => p.TrackIds).HasMaxLength(255);
        builder.ToTable("cshipments");
        builder.OwnsOne(s => s.Cod, u =>
        {
            u.ToTable("cshipments");
            u.ToJson();
        });
        builder.OwnsOne(s => s.PackageCustomerReferences, u =>
        {
            u.ToTable("cshipments");
            u.ToJson();
        });
        builder.OwnsOne(s => s.Shipper, u =>
        {
            u.ToTable("cshipments");
            u.ToJson();
        });
        builder.OwnsOne(s => s.Consignee, u =>
        {
            u.ToTable("cshipments");
            u.ToJson();
        });
        builder.OwnsMany(s => s.Products, u =>
        {
            u.ToTable("cshipments");
            u.ToJson();
        });
        builder.OwnsMany(s => s.Customs, u =>
        {
            u.ToTable("cshipments");
            u.ToJson();
        });
        builder.OwnsMany(s => s.Boxes, u =>
        {
            u.ToTable("cshipments");
            u.ToJson();
            u.OwnsMany(update => update.Products, u1 =>
            {
                
                u1.ToTable("cshipments");
            });
        });
        builder.OwnsMany(s => s.Labels, u =>
        {
            u.ToTable("cshipments");
            u.ToJson();
        });
        builder.HasIndex(i => i.ReferenceId);
        builder.HasIndex(u => new { u.IsActive, u.CreatedOn, u.CreatedBy }, "IX_Shipment_UserIndex");
    }
}