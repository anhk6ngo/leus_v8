using LeUs.Application.Dtos.UnitedBridge;

namespace LeUs.Application.Interfaces;

public interface IUnitedBrideService : IManager
{
    public Task<URateResponse> GetRate(URateRequest request);
    public Task<UBuyReponse> CreateLabel(UBuyRequest request);
    public Task<UVoidResponse> CancelLabel(UVoidRequest request);
    public Task<CTrackingResponse> GetTrack(string? trackingNumber);
    public Task<UVoidResponse> TrackEvents(UTrackEventRequest request);
}