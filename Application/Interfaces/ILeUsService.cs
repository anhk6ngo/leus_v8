﻿using LeUs.Application.Dtos.Gps;

namespace LeUs.Application.interfaces;

public interface ILeUsService : IManager
{
    public Task<List<CResult<string>>> CreateShipment(List<CShipmentDto> shipments, string? userId);
    public Task<List<CResult<string>>> CreateShipment(List<string> refIds, string? userId);
    public Task<CResult<DownloadFileContent>> CreateShipment(CShipmentDto item, List<CServiceDto> services, string? userId);
    public Task<List<CResult<string>>> CancelShipment(List<string> input, string? userId);
    public Task<DownloadFileContent> GetLabel(List<string> input, string? userId, int typeFileName = 0);
    public Task<CTrackingResponse> GetTrack(List<string> input, string? userId);
    public Task<int> GetZone(string from, string to);
    public Task<bool> FuncBalance(BalanceRequest input);
    public Task<double> GetRate(CShipmentDto input);
}