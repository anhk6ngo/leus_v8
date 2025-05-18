using FirstMile;

namespace LeUs.Application.Interfaces;

public interface IFirstMileService : IManager
{
    public Task<DomesticRateResponse> GetRate(DomesticRateRequest request);
    public Task<DomesticRatesResponse> GetRates(DomesticRatesRequest request);
    public Task<DhlLabelResponse> CreateLabel(DhlLabelRequest request);
    public Task<DhlLabelResponse> ReprintLabel(DhlReprintRequest request);
    public Task<CancelDomesticLabelResponse> CancelLabel(CancelDomesticLabelRequest request);
    public Task<CTrackingResponse> GetTracking(string? trackingNumber);
}