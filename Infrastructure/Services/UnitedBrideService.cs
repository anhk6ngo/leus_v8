using LeUs.Application.Dtos.UnitedBridge;

namespace LeUs.Infrastructure.Services;

public class UnitedBrideService(IOptions<ApiSetting> options, IHttpClientFactory clientFactory) : IUnitedBrideService
{
    public async Task<URateResponse> GetRate(URateRequest request)
    {
       
        var result = new URateResponse();
        try
        {
            var client = GenerateClient();
            var endpoint = UnitedBridgeEndPoint.Rate;
            var sObject = request.ConvertObjectToString(false);
            var sData = new StringContent(sObject, Encoding.UTF8, "application/json");
            using var httpResponse = await client.PostAsync(endpoint, sData);
            var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return sDataResponse.ToObject<URateResponse>();
            }
            result.zone = -1;
            result.service = sDataResponse;
            return result;
        }
        catch (Exception e)
        {
            result.zone = -1;
            result.service = e.Message;
        }
        return result;
    }

    public async Task<UBuyReponse> CreateLabel(UBuyRequest request)
    {
        var result = new UBuyReponse();
        try
        {
            var client = GenerateClient();
            var endpoint = UnitedBridgeEndPoint.Buy;
            var sObject = request.ConvertObjectToString(false);
            var sData = new StringContent(sObject, Encoding.UTF8, "application/json");
            using var httpResponse = await client.PostAsync(endpoint, sData);
            var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return sDataResponse.ToObject<UBuyReponse>();
            }
            result.zone = -1;
            result.service = sDataResponse;
            return result;
        }
        catch (Exception e)
        {
            result.zone = -1;
            result.service = e.Message;
        }
        return result;
    }

    public async Task<UVoidResponse> CancelLabel(UVoidRequest request)
    {
        var result = new UVoidResponse();
        try
        {
            var client = GenerateClient();
            var endpoint = UnitedBridgeEndPoint.Void;
            var sObject = request.ConvertObjectToString(false);
            var sData = new StringContent(sObject, Encoding.UTF8, "application/json");
            using var httpResponse = await client.PostAsync(endpoint, sData);
            var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return sDataResponse.ToObject<UVoidResponse>();
            }
            result.text = sDataResponse;
            return result;
        }
        catch (Exception e)
        {
            result.text = e.Message;
        }
        return result;
    }

    public async Task<CTrackingResponse> GetTrack(string? trackingNumber)
    {
        var result = new CTrackingResponse()
        {
            TrackingNumber = trackingNumber
        };
        try
        {
            var client = GenerateClient();
            var endpoint = UnitedBridgeEndPoint.Track;
            var rq = new UVoidRequest()
            {
                tracking_number = trackingNumber
            };
            var sObject = rq.ConvertObjectToString(false);
            var sData = new StringContent(sObject, Encoding.UTF8, "application/json");
            using var httpResponse = await client.PostAsync(endpoint, sData);
            var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var events = sDataResponse.ToObject<List<UTrackEventResponse>>();
                result.Events = events.Select(s=>new CTrackingEvent()
                {
                    Date = s.timestamp,
                    Status = s.status,
                    Location = s.location,
                    Description = s.description
                }).ToList();
            }
        }
        catch
        {
            return result;
        }
        return result;
    }

    public async Task<UVoidResponse> TrackEvents(UTrackEventRequest request)
    {
        var result = new UVoidResponse();
        try
        {
            var client = GenerateClient();
            var endpoint = UnitedBridgeEndPoint.TrackEvents;
            var sObject = request.ConvertObjectToString(false);
            var sData = new StringContent(sObject, Encoding.UTF8, "application/json");
            using var httpResponse = await client.PostAsync(endpoint, sData);
            var sDataResponse = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return sDataResponse.ToObject<UVoidResponse>();
            }
            result.text = sDataResponse;
            return result;
        }
        catch (Exception e)
        {
            result.text = e.Message;
        }
        return result;
    }

    private HttpClient GenerateClient()
    {
        var client = clientFactory.CreateClient("UnitedBridge");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.Value.AppSecret2}");
        return client;
    }
}