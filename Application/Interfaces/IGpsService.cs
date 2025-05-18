using LeUs.Application.Dtos.Gps;

namespace LeUs.Application.Interfaces;

public interface IGpsService: IManager
{
    public Task<CResult<List<GpsDataService>>> GetServices();
    public Task<CResult<List<GpsEntryPoint>>> GetEntryPoints();
    public Task<CResult<List<GpsSensitiveAttribute>>> GetSensitiveAttributes();
    public Task<CResult<List<GpsRateResponse>>> GetRates(GpsRateRequest input);
    public Task<CResult<object>> GetAbnormal(GpsAbnormalRequest input);
    public Task<CResult<GpsTrackResponse>> Tracking(GpsTrackRequest input);
    public Task<CResult<List<string>>> GetTrackingNo(GpsTrackRequest input);
    public Task<CResult<List<GpsLabelResponse>>> GetLabel(GpsLabelRequest input);
    public Task<CResult<object>> CancelShipment(GpsTrackRequest input);
    public Task<CResult<GpsGetShipmentResponse>> GetShipment(GpsTrackRequest input);
    public Task<CResult<GpsCreateShipmentResponse>> CreateShipment(GpsCreateShipmentRequest input);
}
