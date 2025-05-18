namespace LeUs.Application.Dtos.Catalog;

public class CServiceDto : AggregateRoot<Guid>, ICService
{
    public string? ServiceCode { get; set; }
    public string? ServiceName { get; set; }
    public bool HasApi { get; set; } = true;
    public string? ApiName { get; set; }
    public bool IsHide { get; set; } = false;
    public int ServiceType { get; set; } = 0;
    public int ServiceNo { get; set; } = 0;
    public int GoodType { get; set; } = 0;
    public bool UseLocation { get; set; } = false;
    public int Numerator { get; set; } = 139;
    public int UnitType { get; set; } = 0;
    public bool UseCubic { get; set; } = false;
}