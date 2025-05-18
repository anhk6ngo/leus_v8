using LeUs.Application.Dtos.Gps;

namespace LeUs.Application.Interfaces.Data;

public interface ICStoreLabel
{
    public string? ShipmentId { get; set; }
    public string? ServiceCode1 { get; set; }
    public string? ApiName1 { get; set; } 
    public List<LabelDetail>? Labels { get; set; }
}