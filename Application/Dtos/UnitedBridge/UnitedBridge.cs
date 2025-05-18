namespace LeUs.Application.Dtos.UnitedBridge;

public class URateRequest
{
    public UAddress? sender { get; set; }
    public UAddress? recipient { get; set; }
    public UDimensions? dimensions { get; set; }
    public string? dimensions_unit { get; set; } = "inch";
    public double weight { get; set; } = 0;
    public string? weight_unit { get; set; } = "lb";
    public string? service { get; set; }
    public string? package { get; set; } = "Parcel";
    public string? confirmation { get; set; }
    public double? insurance { get; set; }
    public List<UCustomForm>? customs_form { get; set; } = [];
}

public class UBuyRequest : URateRequest
{
    public UMetaData? metadata { get; set; }
    public string? unique_order { get; set; }
    public string? image_format { get; set; } = "pdf";
}

public class UVoidRequest
{
    public string? tracking_number { get; set; }
}

public class UVoidResponse
{
    public string? text { get; set; }
}

public class TrackData
{
    public string? tracking_number { get; set; }
}

public class UMetaData
{
    public string? reference1 { get; set; }
    public string? reference2 { get; set; }
}

public class UAddress
{
    public string? name { get; set; }
    public string? address1 { get; set; }
    public string? address2 { get; set; }
    public string? city { get; set; }
    public string? state { get; set; }
    public string? postal_code { get; set; }
    public string? country { get; set; }
    public string? phone { get; set; }
}

public class UDimensions
{
    public double length { get; set; } = 0;
    public double width { get; set; } = 0;
    public double height { get; set; } = 0;
}

public class URateResponse
{
    public double account_balance { get; set; }
    public string? currency { get; set; }
    public URate? rate { get; set; }
    public double weight { get; set; } = 0;
    public string? weight_unit { get; set; }
    public int zone { get; set; } = 0;
    public string? service { get; set; }
    public string? package { get; set; }
}

public class UBuyReponse : URateResponse
{
    public string? tracking_number { get; set; }
    public string? postage_url { get; set; }
}

public class URate
{
    public double total { get; set; } = 0;
    public double shipment_rate { get; set; } = 0;
    public double other_rate { get; set; } = 0;
    public List<UOtherDetail>? other_detail { get; set; }
}

public class UOtherDetail
{
    public string? type { get; set; }
    public double rate { get; set; } = 0;
}

public class UTrackEventRequest
{
    public string? tracking_number { get; set; }
    public string? event_code { get; set; }
    public string? event_timestamp { get; set; }
}

public class UTrackEventResponse
{
    public string? status { get; set; }
    public string? location { get; set; }
    public string? description { get; set; }
    public string? timestamp { get; set; }
}

public class UCustomForm
{
    public string? description { get; set; }
    public int quantity { get; set; }
    public string? hts_code { get; set; }
    public string? origin_country_code { get; set; }
    public double? value { get; set; }
}

#region Api End Points

public static class UnitedBridgeEndPoint
{
    public static string? Rate { get; } = "api/labels/rate/";
    public static string? Buy { get; } = "api/labels/buy/";
    public static string? Void { get; } = "api/labels/void/";
    public static string? Manifest { get; } = "api/labels/manifest/";
    public static string? Track { get; } = "api/labels/track/";
    public static string? TrackEvents { get; } = "api/labels/tracking_events/";
}

#endregion