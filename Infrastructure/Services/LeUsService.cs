using System.IO.Compression;
using FirstMile;
using LeUs.Application.Dtos.Gps;
using LeUs.Application.Dtos.UnitedBridge;
using Leus.Application.Features.Catalog.Queries;
using Leus.Application.Features.Data.Commands;
using LeUs.Application.Features.Data.Commands;
using Leus.Application.Features.Data.Queries;

namespace LeUs.Infrastructure.Services;

public class LeUsService(
    IGpsService gpsService,
    IFirstMileService firstMileService,
    IUnitedBrideService unitedBrideService,
    IMediator mediator,
    IHttpClientFactory clientFactory) : ILeUsService
{
    public async Task<List<CResult<string>>> CreateShipment(List<CShipmentDto> shipments,
        string? userId)
    {
        var services = await mediator.Send(new GetAllServiceQuery());
        var results = new List<CResult<string>>();
        var oUps = new List<CShipment>();
        var oHis = new List<CHistoryLabel>();
        foreach (var item in shipments)
        {
            var dAmount = item.Price.PlusNumber(item.Remote)
                .PlusNumber(item.ExtraLongFee).PlusNumber(item.OverLimitFee)
                .PlusNumber(item.ExcessVolumeFee);
            if ((item.Price ?? 0) == 0)
            {
                results.Add(new CResult<string>()
                {
                    Success = false,
                    ErrorMessage = "The shipment don't have price",
                    RequestId = item.ReferenceId
                });
                continue;
            }

            var oSer = services.FirstOrDefault(x => x.ServiceCode == item.ServiceCode1);
            if (oSer == null)
            {
                results.Add(new CResult<string>
                {
                    Success = false,
                    ErrorMessage = "Service not found",
                    RequestId = item.ReferenceId,
                });
                continue;
            }
            //Check balance and reduce amount before generate label
            var rqBalance = new BalanceRequest()
            {
                UserId = userId,
                Amount = dAmount,
                Action = ActionCommandType.GetData
            };
            var blnCheck = await FuncBalance(rqBalance);
            if (!blnCheck)
            {
                results.Add(new CResult<string>()
                {
                    Success = false,
                    ErrorMessage = "Balance is insufficient. Please top-up and try again",
                    RequestId = item.ReferenceId
                });
                break;
            }
            var newHis = new CHistoryLabel()
            {
                ReferenceId = item.ReferenceId,
                Request = item.ConvertObjectToString()
            };
            var blnFailure = false;
            var sTrackingId = "";
            switch (oSer.ApiName)
            {
                case ApiName.Gps:
                    var rqG = item.Adapt<GpsCreateShipmentRequest>();
                    var resG = await gpsService.CreateShipment(rqG);
                    newHis.Response = resG.ConvertObjectToString();
                    if (resG.Success == true)
                    {
                        sTrackingId = resG.Data?.TrackingNo;
                        oUps.Add(new CShipment
                        {
                            Id = item.Id,
                            ShipmentId = resG.Data?.ShipmentId,
                            Cost = resG.Data?.Fees?.Sum(s => s.Amount) ?? 0,
                            TrackIds = resG.Data?.TrackingNo,
                        });
                    }
                    else
                    {
                        blnFailure = true;
                    }
                    results.Add(new CResult<string>()
                    {
                        Success = resG.Success,
                        RequestId = item.ReferenceId,
                        TrackingId = sTrackingId,
                        ErrorMessage = resG.ErrorMessage,
                    });
                    break;
                case ApiName.FirstMile:
                    var rqF = TransformHelper.ToFirstMile(item);
                    var resF = await firstMileService.CreateLabel(rqF);
                    newHis.Response = resF.ConvertObjectToString();
                    if (resF.Errors is null or { Length: 0 })
                    {
                        var aLabels = resF.PackageData;
                        var newItem = new CShipment
                        {
                            Id = item.Id,
                            Labels = aLabels.Select(s => new LabelDetail()
                            {
                                Label = s.LabelImageBase64,
                                TrackignNo = s.TrackingNumber
                            }).ToList(),
                            Cost = (double)aLabels.Sum(s => s.RateInfo.Cost),
                            TrackIds = aLabels.Select(s => s.TrackingNumber).Aggregate((a, b) => $"{a},{b}")
                        };
                        oUps.Add(newItem);
                        results.Add(new CResult<string>()
                        {
                            Success = true,
                            RequestId = item.ReferenceId,
                            TrackingId = newItem.TrackIds,
                            ErrorMessage = "The label is created",
                        });
                    }
                    else
                    {
                        blnFailure = true;
                        results.Add(new CResult<string>()
                        {
                            Success = false,
                            RequestId = item.ReferenceId,
                            ErrorMessage = resF.Errors.Aggregate((a, b) => $"{a} {b}"),
                        });
                    }

                    break;
                case ApiName.UnitedBridge:
                    var rqU = TransformHelper.ToUnitedBridge(item);
                    var resU = await unitedBrideService.CreateLabel(rqU);
                    newHis.Response = resU.ConvertObjectToString();
                    if (resU.zone > -1)
                    {
                        var newLabel = new LabelDetail()
                        {
                            TrackignNo = resU.tracking_number
                        };
                        var spdfLink = resU.postage_url;
                        if (spdfLink.NotIsNullOrEmpty())
                        {
                            var clientLabel = new HttpClient();
                            using var httpResponseImage = await clientLabel.GetAsync(spdfLink);
                            using var ms = new MemoryStream();
                            await httpResponseImage.Content.CopyToAsync(ms);
                            var bytes = ms.ToArray();
                            newLabel.Label = Convert.ToBase64String(bytes);
                        }

                        oUps.Add(new CShipment
                        {
                            Id = item.Id,
                            Labels = [newLabel],
                            Cost = resU.rate?.total,
                            TrackIds = resU.tracking_number,
                        });
                        blnFailure = true;
                        results.Add(new CResult<string>()
                        {
                            Success = true,
                            TrackingId = resU.tracking_number,
                            RequestId = item.ReferenceId,
                            ErrorMessage = "The label is created",
                        });
                    }
                    else
                    {
                        results.Add(new CResult<string>()
                        {
                            Success = false,
                            RequestId = item.ReferenceId,
                            ErrorMessage = resU.service,
                        });
                    }
                    break;
            }
            oHis.Add(newHis);
            if (!blnFailure) continue;
            //Increase balance if generate label failure
            rqBalance = new BalanceRequest()
            {
                UserId = userId,
                Amount = dAmount,
                Action = ActionCommandType.Add
            };
            await FuncBalance(rqBalance);
        }

        if (oUps is { Count: > 0 })
        {
            await mediator.Send(new UpdateLabelShipmentCommand()
            {
                Data = oUps,
                History = oHis
            });
        }
        return results;
    }

    public async Task<CResult<DownloadFileContent>> CreateShipment(CShipmentDto item, List<CServiceDto> services,
        string? userId)
    {
        var result = new CResult<DownloadFileContent>()
        {
            RequestId = item.ReferenceId
        };
        var oUps = new List<CShipment>();
        var oHis = new List<CHistoryLabel>();
        var dAmount = item.Price.PlusNumber(item.Remote)
            .PlusNumber(item.ExtraLongFee).PlusNumber(item.OverLimitFee)
            .PlusNumber(item.ExcessVolumeFee);
        if ((item.Price ?? 0) == 0)
        {
            result.Success = false;
            result.ErrorMessage = "The shipment don't have price";
            return result;
        }

        var oSer = services.FirstOrDefault(x => x.ServiceCode == item.ServiceCode1);
        if (oSer == null)
        {
            result.Success = false;
            result.ErrorMessage = "Service not found";
            return result;
        }
        //Check balance and reduce amount before generate label
        var rqBalance = new BalanceRequest()
        {
            UserId = userId,
            Amount = dAmount,
            Action = ActionCommandType.GetData
        };
        var blnCheck = await FuncBalance(rqBalance);
        if (!blnCheck)
        {
            result.Success = false;
            result.ErrorMessage = "Balance is insufficient. Please top-up and try again";
            return result;
        }
        var blnFailure = false;
        var newHis = new CHistoryLabel()
        {
            ReferenceId = item.ReferenceId,
            Request = item.ConvertObjectToString()
        };
        var sTrackingId = "";
        switch (oSer.ApiName)
        {
            case ApiName.Gps:
                var rqG = item.Adapt<GpsCreateShipmentRequest>();
                var resG = await gpsService.CreateShipment(rqG);
                newHis.Response = resG.ConvertObjectToString();
                if (resG.Success == true)
                {
                    sTrackingId = resG.Data?.TrackingNo;
                    var newItem = new CShipment
                    {
                        Id = item.Id,
                        ShipmentId = resG.Data?.ShipmentId,
                        Cost = resG.Data?.Fees?.Sum(s => s.Amount) ?? 0,
                        TrackIds = resG.Data?.TrackingNo,
                    };
                    //Get label if the processing is successful
                    var gpsResult = await gpsService.GetLabel(new GpsLabelRequest()
                    {
                        ShipmentId = newItem.ShipmentId
                    });
                    if (gpsResult is { Success: true, Data.Count: > 0 })
                    {
                        newItem.Labels ??= [];
                        newItem.TrackIds = "";
                        foreach (var itemLabel in gpsResult.Data!)
                        {
                            if (itemLabel.LabelBase64S != null)
                            {
                                newItem.Labels.AddRange(itemLabel.LabelBase64S);
                            }
                            else
                            {
                                newItem.Labels.Add(new LabelDetail()
                                {
                                    Label = itemLabel.LabelBase64,
                                    TrackignNo = itemLabel.TrackingNo,
                                });
                            }

                            newItem.TrackIds += itemLabel.TrackingNo + ", ";
                        }
                    }
                    else
                    {
                        blnFailure = true;
                    }
                    oUps.Add(newItem);
                }
                result.Success = resG.Success;
                result.TrackingId = sTrackingId;
                result.ErrorMessage = resG.ErrorMessage;
                break;
            case ApiName.FirstMile:
                var rqF = TransformHelper.ToFirstMile(item);
                var resF = await firstMileService.CreateLabel(rqF);
                newHis.Response = resF.ConvertObjectToString();
                if (resF.Errors is null or { Length: 0 })
                {
                    var aLabels = resF.PackageData;
                    var newItem = new CShipment
                    {
                        Id = item.Id,
                        Labels = aLabels.Select(s => new LabelDetail()
                        {
                            Label = s.LabelImageBase64,
                            TrackignNo = s.TrackingNumber
                        }).ToList(),
                        Cost = (double)aLabels.Sum(s => s.RateInfo.Cost),
                        TrackIds = aLabels.Select(s => s.TrackingNumber).Aggregate((a, b) => $"{a},{b}")
                    };
                    oUps.Add(newItem);
                    result.Success = true;
                    result.TrackingId = newItem.TrackIds;
                    result.ErrorMessage = "The label is created";
                }
                else
                {
                    blnFailure = true;
                    result.Success = false;
                    result.ErrorMessage = resF.Errors.Aggregate((a, b) => $"{a} {b}");
                }

                break;
            case ApiName.UnitedBridge:
                var rqU = TransformHelper.ToUnitedBridge(item);
                var resU = await unitedBrideService.CreateLabel(rqU);
                newHis.Response = resU.ConvertObjectToString();
                if (resU.zone > -1)
                {
                    var newLabel = new LabelDetail()
                    {
                        TrackignNo = resU.tracking_number
                    };
                    var spdfLink = resU.postage_url;
                    if (spdfLink.NotIsNullOrEmpty())
                    {
                        var clientLabel = new HttpClient();
                        using var httpResponseImage = await clientLabel.GetAsync(spdfLink);
                        using var ms = new MemoryStream();
                        await httpResponseImage.Content.CopyToAsync(ms);
                        var bytes = ms.ToArray();
                        newLabel.Label = Convert.ToBase64String(bytes);
                    }

                    oUps.Add(new CShipment
                    {
                        Id = item.Id,
                        Labels = [newLabel],
                        Cost = resU.rate?.total,
                        TrackIds = resU.tracking_number,
                    });
                    result.Success = true;
                    result.ErrorMessage = "The label is created";
                    result.TrackingId = resU.tracking_number;
                }
                else
                {
                    blnFailure = true;
                    result.Success = false;
                    result.ErrorMessage = resU.service;
                }

                break;
        }

        if (blnFailure)
        {
            //Increase balance if generate label failure
            rqBalance = new BalanceRequest()
            {
                UserId = userId,
                Amount = dAmount,
                Action = ActionCommandType.Add
            };
            await FuncBalance(rqBalance);
        }
        
        oHis.Add(newHis);
        if (result.Success == false || oUps is { Count: 0 })
        {
            return result;
        }

        await mediator.Send(new UpdateLabelShipmentCommand()
        {
            Data = oUps,
            History = oHis
        });
        result.Data = new DownloadFileContent();
        var lstLabels = oUps.SelectMany(s => s.Labels!).ToList().ToList();
        if (lstLabels.Count == 1)
        {
            result.Data.code = $"{lstLabels[0].TrackignNo}.pdf";
            result.Data.content = Convert.FromBase64String($"{lstLabels[0].Label}");
        }
        else
        {
            using var compressedFileStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
            {
                foreach (var iLabel in lstLabels)
                {
                    var zipEntry = zipArchive.CreateEntry($"{iLabel.TrackignNo}.pdf");
                    var bContent = Convert.FromBase64String($"{iLabel.Label}");
                    using var originalFileStream = new MemoryStream(bContent);
                    await using var zipEntryStream = zipEntry.Open();
                    await originalFileStream.CopyToAsync(zipEntryStream);
                }
            }
            result.Data.code = $"{DateTime.UtcNow.Ticks}.zip";
            result.Data.fileType = "application/octet-stream";
            result.Data.content = compressedFileStream.ToArray();
        }
        return result;
    }

    public async Task<List<CResult<string>>> CancelShipment(List<string> input, string? userId)
    {
        var dDeadline = DateTime.UtcNow.AddDays(-14);
        var results = new List<CResult<string>>();
        var oUps = new List<CShipment>();
        var oHis = new List<CHistoryLabel>();
        var shipments = await mediator.Send(new GetAllShipmentByRefQuery()
        {
            RefIds = input,
            UserId = userId,
            GetLabel = false
        });
        var dAddAmount = 0.0;
        foreach (var item in shipments)
        {
            var blnSuccess = false;
            var dAmount = item.Price.PlusNumber(item.Remote)
                .PlusNumber(item.ExtraLongFee).PlusNumber(item.OverLimitFee)
                .PlusNumber(item.ExcessVolumeFee);
            if (dAmount == 0)
            {
                results.Add(new CResult<string>()
                {
                    Success = false,
                    ErrorMessage = "The shipment don't have price",
                    RequestId = item.ReferenceId
                });
                continue;
            }

            if (item.ShipmentStatus != 2)
            {
                results.Add(new CResult<string>()
                {
                    Success = false,
                    ErrorMessage = "The shipment don't have label",
                    RequestId = item.ReferenceId
                });
                continue;
            }

            if (item.CreateLabelDate != null && item.CreateLabelDate < dDeadline)
            {
                results.Add(new CResult<string>()
                {
                    Success = false,
                    ErrorMessage = "The shipment cannot be cancelled due over 14 days",
                    RequestId = item.ReferenceId
                });
                continue;
            }

            var apiName = item.ApiName1 ?? item.ApiName;
            var newHis = new CHistoryLabel()
            {
                ReferenceId = item.ReferenceId
            };
            switch (apiName)
            {
                case ApiName.Gps:
                    var rqG = new GpsTrackRequest()
                    {
                        ShipmentId = item.ShipmentId
                    };
                    var resG = await gpsService.CancelShipment(rqG);
                    newHis.Response = resG.ConvertObjectToString();
                    if (resG.Success == true)
                    {
                        blnSuccess = true;
                    }

                    results.Add(new CResult<string>()
                    {
                        Success = resG.Success,
                        RequestId = item.ReferenceId,
                        ErrorMessage = resG.ErrorMessage,
                    });
                    break;
                case ApiName.FirstMile:
                    var rqF = new CancelDomesticLabelRequest()
                    {
                        TrackingNumber = item.TrackIds
                    };
                    var resF = await firstMileService.CancelLabel(rqF);
                    newHis.Response = resF.ConvertObjectToString();
                    if (resF.Successful)
                    {
                        blnSuccess = true;
                    }

                    results.Add(new CResult<string>()
                    {
                        Success = resF.Successful,
                        RequestId = item.ReferenceId,
                        ErrorMessage = resF.ErrorMessages is { Length: > 0 }
                            ? resF.ErrorMessages.Aggregate((a, b) => $"{a} {b}")
                            : "The processing is error.",
                    });
                    break;
                case ApiName.UnitedBridge:
                    var rqU = new UVoidRequest()
                    {
                        tracking_number = item.TrackIds
                    };
                    var resU = await unitedBrideService.CancelLabel(rqU);
                    newHis.Response = resU.ConvertObjectToString();
                    if ($"{resU.text}".Contains("request accepted"))
                    {
                        blnSuccess = true;
                    }

                    results.Add(new CResult<string>()
                    {
                        Success = blnSuccess,
                        RequestId = item.ReferenceId,
                        ErrorMessage = resU.text,
                    });
                    break;
            }

            oHis.Add(newHis);
            if (!blnSuccess) continue;
            dAddAmount += dAmount;
            oUps.Add(item);
        }

        if (oUps is { Count: 0 }) return results;
        var rqBalance = new BalanceRequest()
        {
            UserId = userId,
            Amount = dAddAmount,
            Action = ActionCommandType.Add
        };
        await FuncBalance(rqBalance);
        await mediator.Send(new UpdateLabelShipmentCommand()
        {
            Data = oUps,
            History = oHis,
            Action = ActionCommandType.Delete
        });
        return results;
    }

    public async Task<DownloadFileContent> GetLabel(List<string> input, string? userId)
    {
        var result = new DownloadFileContent();
        var lstUpdate = new List<CShipment>();
        var shipments = await mediator.Send(new GetAllShipmentByRefQuery()
        {
            RefIds = input,
            UserId = userId
        });
        var lstLabels = new List<LabelDetail>();
        if (shipments is not { Count: > 0 }) return result;
        foreach (var item in shipments)
        {
            switch (item.ApiName1)
            {
                case "gps":
                    if (item.Labels is null || item.Labels.Count == 0)
                    {
                        var gpsResult = await gpsService.GetLabel(new GpsLabelRequest()
                        {
                            ShipmentId = item.ShipmentId
                        });
                        if (gpsResult is { Success: true, Data.Count: > 0 })
                        {
                            item.Labels ??= [];
                            item.TrackIds = "";
                            foreach (var itemLabel in gpsResult.Data!)
                            {
                                if (itemLabel.LabelBase64S != null)
                                {
                                    item.Labels.AddRange(itemLabel.LabelBase64S);
                                }
                                else
                                {
                                    item.Labels.Add(new LabelDetail()
                                    {
                                        Label = itemLabel.LabelBase64,
                                        TrackignNo = itemLabel.TrackingNo,
                                    });
                                }

                                item.TrackIds += itemLabel.TrackingNo + ", ";
                            }

                            lstUpdate.Add(item);
                        }
                    }

                    lstLabels.AddRange(item.Labels!);
                    break;
                default:
                    if (item.Labels is { Count: > 0 })
                    {
                        lstLabels.AddRange(item.Labels!);
                    }

                    break;
            }
        }

        if (lstUpdate is { Count: > 0 })
        {
            await mediator.Send(new UpdateLabelShipmentCommand()
            {
                Data = lstUpdate,
                Action = ActionCommandType.UpdateLabel
            });
        }

        if (lstLabels is { Count: 0 }) return result;
        if (lstLabels.Count == 1)
        {
            result.code = $"{lstLabels[0].TrackignNo}.pdf";
            result.content = Convert.FromBase64String($"{lstLabels[0].Label}");
        }
        else
        {
            using var compressedFileStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
            {
                foreach (var item in lstLabels)
                {
                    var zipEntry = zipArchive.CreateEntry($"{item.TrackignNo}.pdf");
                    var bContent = Convert.FromBase64String($"{item.Label}");
                    using var originalFileStream = new MemoryStream(bContent);
                    await using var zipEntryStream = zipEntry.Open();
                    await originalFileStream.CopyToAsync(zipEntryStream);
                }
            }

            result.code = $"{DateTime.UtcNow.Ticks}.zip";
            result.fileType = "application/octet-stream";
            result.content = compressedFileStream.ToArray();
        }

        return result;
    }

    public async Task<CTrackingResponse> GetTrack(List<string> input, string? userId)
    {
        var result = new CTrackingResponse();
        var shipments = await mediator.Send(new GetAllShipmentByRefQuery()
        {
            RefIds = input,
            UserId = userId
        });
        foreach (var item in shipments)
        {
            switch (item.ApiName1)
            {
                case "gps":
                    var oRes = gpsService.Tracking(new GpsTrackRequest()
                    {
                        TrackingNo = item.TrackIds
                    });
                    result.TrackingNumber = item.TrackIds;
                    result.ReferenceId = item.ReferenceId;
                    if (oRes.Result is { Success: true, Data.Traces.Count: > 0 })
                    {
                        result.Events = oRes.Result.Data!.Traces!.Select(s => new CTrackingEvent()
                        {
                            Date = s.Time,
                            Status = s.Status,
                            Description = s.Description,
                            Location = s.Location
                        }).ToList();
                    }

                    break;
                case ApiName.FirstMile:
                    result = await firstMileService.GetTracking(item.TrackIds);
                    result.ReferenceId = item.ReferenceId;
                    break;
                default:
                    result = await unitedBrideService.GetTrack(item.TrackIds);
                    result.ReferenceId = item.ReferenceId;
                    break;
            }
        }

        return result;
    }

    public async Task<int> GetZone(string from, string to)
    {
        var client = clientFactory.CreateClient("uspsZone");
        var tmpTo = to.SplitExt("-")[0];
        var endpoint =
            $"DomesticZoneChart/GetZone?origin={from}&destination={tmpTo}&shippingDate={DateTime.Today.ToShortDateString()}";
        using var httpResponse = await client.GetAsync(endpoint);
        if (httpResponse.StatusCode != HttpStatusCode.OK) return 0;
        var sContent = await httpResponse.Content.ReadAsStringAsync();
        var oZoneInfo = sContent.ToObject<CZoneInfo>();
        if (oZoneInfo.ZoneInformation.IsNullOrEmpty()) return 0;
        var iIndex = $"{oZoneInfo.ZoneInformation}".IndexOf($".", StringComparison.Ordinal);
        if (iIndex < 0) return 0;
        sContent = $"{oZoneInfo.ZoneInformation}"[..iIndex];
        var aData = sContent.SplitExt().Reverse();
        return $"{aData.FirstOrDefault()}".ConvertToInt();
    }

    public async Task<bool> FuncBalance(BalanceRequest input)
    {
        return await mediator.Send(new FuncBalanceCommand()
        {
            Input = input
        });
    }

    public async Task<double> GetRate(CShipmentDto input)
    {
        var dRate = 0.0;
        switch (input.ApiName)
        {
            case ApiName.FirstMile:
                var rqRate = TransformHelper.ToFirstMileRate(input);
                var rsRate = await firstMileService.GetRate(rqRate);
                dRate = (double)rsRate.Cost;
                break;
            default:
                break;
        }

        return dRate;
    }
}