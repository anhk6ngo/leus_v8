using System.Net;
using System.Text;
using LeUs.Application.Dtos.Gps;
using Microsoft.Extensions.Options;

namespace LeUs.Infrastructure.Services;

public class GpsService(IOptions<ApiSetting> settings, IHttpClientFactory clientFactory) : IGpsService
{
    public string? LogContent { get; set; }

    public async Task<CResult<List<GpsDataService>>> GetServices()
    {
        var client = GenerateClient("", "tms.service.query");
        var sData = new StringContent("", Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
        return sDataResponse.ToObject<CResult<List<GpsDataService>>>();
    }

    public async Task<CResult<List<GpsEntryPoint>>> GetEntryPoints()
    {
        var client = GenerateClient("", "tms.entrypoint.query");
        var sData = new StringContent("", Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
        return sDataResponse.ToObject<CResult<List<GpsEntryPoint>>>();
    }

    public async Task<CResult<List<GpsSensitiveAttribute>>> GetSensitiveAttributes()
    {
        var client  = GenerateClient("", "tms.sensitiveattributes.query");
        var sData = new StringContent("", Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
        return sDataResponse.ToObject<CResult<List<GpsSensitiveAttribute>>>();
    }
    public async Task<CResult<List<GpsRateResponse>>> GetRates(GpsRateRequest input)
    {
        var sObject = input.ConvertObjectToString(false);
        var client =GenerateClient(sObject, "tms.rate.query");
        var sData = new StringContent(sObject, Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
        return sDataResponse.ToObject<CResult<List<GpsRateResponse>>>();
    }

    public async Task<CResult<object>> GetAbnormal(GpsAbnormalRequest input)
    {
        var sObject = input.ConvertObjectToString(false);
        var client =GenerateClient(sObject, "tms.rate.query");
        var sData = new StringContent(sObject, Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
        return sDataResponse.ToObject<CResult<object>>();
    }

    public async Task<CResult<GpsTrackResponse>> Tracking(GpsTrackRequest input)
    {
        var sObject = input.ConvertObjectToString(false);
        var client =GenerateClient(sObject, "tms.shipment.track");
        var sData = new StringContent(sObject, Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
        return sDataResponse.ToObject<CResult<GpsTrackResponse>>();
    }

    public async Task<CResult<List<string>>> GetTrackingNo(GpsTrackRequest input)
    {
        var sObject = input.ConvertObjectToString(false);
        var client =GenerateClient(sObject, "tms.shipment.getTrackingNo");
        var sData = new StringContent(sObject, Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
        return sDataResponse.ToObject<CResult<List<string>>>();
    }

    public async Task<CResult<List<GpsLabelResponse>>> GetLabel(GpsLabelRequest input)
    {
        var sObject = input.ConvertObjectToString(false);
        var client =GenerateClient(sObject, "tms.shipment.getLabel");
        var sData = new StringContent(sObject, Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
        return sDataResponse.ToObject<CResult<List<GpsLabelResponse>>>();
    }

    public async Task<CResult<object>> CancelShipment(GpsTrackRequest input)
    {
        var sObject = input.ConvertObjectToString(false);
        var client =GenerateClient(sObject, "tms.shipment.cancel");
        var sData = new StringContent(sObject, Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        LogContent = await httpResponse.Content.ReadAsStringAsync();
        return LogContent.ToObject<CResult<object>>();
    }

    public async Task<CResult<GpsGetShipmentResponse>> GetShipment(GpsTrackRequest input)
    {
        var sObject = input.ConvertObjectToString(false);
        var client =GenerateClient(sObject, "tms.shipment.get");
        var sData = new StringContent(sObject, Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
        return sDataResponse.ToObject<CResult<GpsGetShipmentResponse>>();
    }

    public async Task<CResult<GpsCreateShipmentResponse>> CreateShipment(GpsCreateShipmentRequest input)
    {
        var sObject = input.ConvertObjectToString(false);
        var client =GenerateClient(sObject, "tms.shipment.create");
        var sData = new StringContent(sObject, Encoding.UTF8, "text/plain");
        using var httpResponse = await client.PostAsync("", sData);
        LogContent = await httpResponse.Content.ReadAsStringAsync();
        return LogContent.ToObject<CResult<GpsCreateShipmentResponse>>();
    }

    private HttpClient GenerateClient(string data, string method)
    {
        var client = clientFactory.CreateClient("Gps");
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var appId = settings.Value.AppId;
        var appKey = settings.Value.AppKey;
        var appSecret = settings.Value.AppSecret;
        var sign =
            $"appId={appId}&appKey={appKey}&language=en&method={method}&timestamp={timestamp}{data}{appSecret}"
                .CreateMd5(option:"x2");
        client.DefaultRequestHeaders.Add("appId", appId);
        client.DefaultRequestHeaders.Add("appKey", appKey);
        client.DefaultRequestHeaders.Add("language", "en");
        client.DefaultRequestHeaders.Add("method", method);
        client.DefaultRequestHeaders.Add("timestamp", timestamp.ToString());
        client.DefaultRequestHeaders.Add("sign", sign);
        return client;
    }
}
