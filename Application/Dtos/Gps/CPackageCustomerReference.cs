namespace LeUs.Application.Dtos.Gps;
public class CPackageCustomerReference
{
    [Description("Refenrece Number")]
    public string? CustomerRefenrece { get; set; }
    [Description("Department Number")]
    public string? DepartmentNumber { get; set; }
    [Description("Invoice Number")]
    public string? InvoiceNumber { get; set; }
    [Description("Po Number")]
    public string? PoNumber { get; set; }
}