namespace LeUs.Installers;

public class MapsterInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        // TypeAdapterConfig<CShipment, CShipmentReport>.NewConfig()
        //     .Map(des => des.Box,
        //         src => src.Boxes != null
        //             ? src.Boxes.Select(s => $"{s.Length}x{s.Width}x{s.Height} {s.Weight}").ToList().FirstOrDefault()
        //             : "");
    }
}