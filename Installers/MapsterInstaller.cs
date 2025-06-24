namespace LeUs.Installers;

public class MapsterInstaller : IInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        // TypeAdapterConfig<CShipment, CShipmentDto>.NewConfig()
        //     .Map(des => des.TotalTime,
        //         src => (src.ShipmentStatus != 2
        //             ? 0
        //             : (src.LastModifiedOn!.Value - src.CreateLabelDate!.Value).TotalSeconds));
    }
}