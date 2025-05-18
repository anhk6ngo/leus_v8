namespace LeUs.Domain.Catalog;

public class CService : AuditableEntityNew<Guid>, ICService
{
    public string? ServiceCode { get; set; }
    public string? ServiceName { get; set; }
    public bool HasApi { get; set; }
    public string? ApiName { get; set; }
    public bool IsHide { get; set; }
    public int ServiceType { get; set; }
    public int ServiceNo { get; set; }
    public int GoodType { get; set; }
    public bool UseLocation { get; set; }
    public int Numerator { get; set; }
    public int UnitType { get; set; }
    public bool UseCubic { get; set; }
}