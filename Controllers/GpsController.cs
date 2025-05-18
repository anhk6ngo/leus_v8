using LeUs.Application.Dtos.Gps;
using Microsoft.AspNetCore.Mvc;

namespace LeUs.Controllers;

public class GpsController(IGpsService gpsService) : AnonymousApiWithHideDocController
{
    [HttpGet("get-service")]
    public async Task<IActionResult> GetService()
    {
        var result = await gpsService.GetServices();
        return Ok(result);
    }

    [HttpGet("get-entry-point")]
    public async Task<IActionResult> GetEntryPoint()
    {
        var result = await gpsService.GetEntryPoints();
        return Ok(result);
    }

    [HttpGet("get-sensitive-attribute")]
    public async Task<IActionResult> GetSensitiveAttribute()
    {
        var result = await gpsService.GetSensitiveAttributes();
        return Ok(result);
    }
    
    [HttpPost("get-rate")]
    public async Task<IActionResult> GetRate(GpsRateRequest input)
    {
        var result = await gpsService.GetRates(input);
        return Ok(result);
    }
    [HttpPost("get-shipment")]
    public async Task<IActionResult> GetRate(GpsTrackRequest input)
    {
        var result = await gpsService.GetShipment(input);
        return Ok(result);
    }
    [HttpPost("get-abnormal")]
    public async Task<IActionResult> GetAbnormal(GpsAbnormalRequest input)
    {
        var result = await gpsService.GetAbnormal(input);
        return Ok(result);
    }
}