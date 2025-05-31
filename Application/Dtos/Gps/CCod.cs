using System.ComponentModel.DataAnnotations;

namespace LeUs.Application.Dtos.Gps;

public class CCod
{
    [Description("COD currency")]
    [MaxLength(33)]
    public string? Currency { get; set; }

    [Description("COD amount")] public double Amount { get; set; } = 0;
}