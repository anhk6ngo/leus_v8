using FirstMile;
using LeUs.Application.Dtos.Gps;
using Microsoft.Extensions.Options;

namespace LeUs.Infrastructure.Services;

public class FirstMileService(IOptions<ApiSetting> options) : IFirstMileService
{
    private Credentials Credentials { get; set; } = new()
    {
        MailerId = $"{options.Value.AppId1}".ConvertToInt(),
        Username = $"{options.Value.AppKey1}",
        Password = $"{options.Value.AppSecret1}",
        TechPartnerId = "A45C3BDA-6A6F-412D-A07E-E4D1054CBCBE"
    };

    private DhlWebApiClient client { get; set; } = new(DhlWebApiClient.EndpointConfiguration.DhlApiSecure);

    public async Task<DomesticRateResponse> GetRate(DomesticRateRequest request)
    {
        request.UserCredentials = Credentials;
        return await client.GetRateAsync(request);
    }

    public async Task<DomesticRatesResponse> GetRates(DomesticRatesRequest request)
    {
        request.UserCredentials = Credentials;
        return await client.GetRatesAsync(request);
    }

    public async Task<DhlLabelResponse> CreateLabel(DhlLabelRequest request)
    {
        request.UserCredentials = Credentials;
        return await client.GetLabelAsync(request);
    }

    public async Task<DhlLabelResponse> ReprintLabel(DhlReprintRequest request)
    {
        request.UserCredentials = Credentials;
        return await client.GetReprintAsync(request);
    }

    public async Task<CancelDomesticLabelResponse> CancelLabel(CancelDomesticLabelRequest request)
    {
        request.UserCredentials = Credentials;
        return await client.CancelDomesticLabelAsync(request);
    }

    public async Task<CTrackingResponse> GetTracking(string? trackingNumber)
    {
        var request = new TrackingRequest()
        {
            TrackingNumber = trackingNumber
        };
        var result = new CTrackingResponse()
        {
            TrackingNumber = trackingNumber
        };
        var response = await client.GetTrackingInfoAsync(request);
        if (response.Events is { Length: > 0 })
        {
            result.Events = response.Events.Select(s=>new CTrackingEvent()
            {
                Date = $"{s.EventDatetime}",
                Description = s.EventDescription,
                Location = $"{s.EventLocation.City} {s.EventLocation.Region} {s.EventLocation.CountryCode}",
                Status = s.EventCodeAsString
            }).ToList();
        }
        return result;
    }
}