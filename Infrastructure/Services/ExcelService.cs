using System.Drawing;
using LeUs.Application.Dtos.Gps;
using Leus.Application.Features.Catalog.Queries;
using Leus.Application.Features.Data.Commands;
using SharedExtension.Constants;
using CAddress = LeUs.Application.Dtos.Gps.CAddress;

namespace Leus.Infrastructure.Services;

public class ExcelService(
    IUnitOfWork<Guid, PortalContext> unitOfWork,
    IWebHostEnvironment host,
    IMediator mediator,
    IValidator<CShipmentDto> valshipment)
    : IExcelService
{
    public async Task<List<UploadResult>> UpLoad(UploadCommandRequest request)
    {
        var result = new List<UploadResult>();
        try
        {
            foreach (var file in request.Files!)
            {
                var memStream = new MemoryStream(file.Data!)
                {
                    Position = 0
                };
                using var package = new ExcelPackage(memStream);
                var worksheet = package.Workbook.Worksheets[1];
                var blnResult = request.TypeUpload switch
                {
                    1 => await AddShipments(worksheet, $"{request.UserId}"),
                    2 => await AddPriceData(worksheet),
                    _ => false
                };
                result.Add(new UploadResult()
                {
                    Uploaded = blnResult,
                    FileName = file.FileName + (blnResult ? " success" : "fail")
                });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return result;
    }

    private async Task<bool> AddShipments(ExcelWorksheet worksheet, string customerId)
    {
        var services = await mediator.Send(new GetAllServiceQuery());
        var lstDetail = new List<CShipmentDto>();
        var rowCount = worksheet.Dimension.Rows;
        for (var row = 3; row <= rowCount; row++)
        {
            var newItem = new CShipmentDto()
            {
                ReferenceId2 = $"{worksheet.Cells[$"A{row}"].Value}",
                ReferenceId3 = $"{worksheet.Cells[$"B{row}"].Value}",
                TrackingNo = $"{worksheet.Cells[$"C{row}"].Value}",
                EntryPoint = $"{worksheet.Cells[$"D{row}"].Value}",
                ServiceCode = $"{worksheet.Cells[$"E{row}"].Value}",
                SignatureRequired = $"{worksheet.Cells[$"F{row}"].Value}",
                DutyType = $"{worksheet.Cells[$"G{row}"].Value}",
                BoxQty = $"{worksheet.Cells[$"H{row}"].Value}",
                DimensionUnit = $"{worksheet.Cells[$"I{row}"].Value}",
                Boxes =
                [
                    new()
                    {
                        BoxQty = $"{worksheet.Cells[$"H{row}"].Value}".ConvertToInt(),
                        Length = $"{worksheet.Cells[$"J{row}"].Value}".ConvertToDouble(),
                        Width = $"{worksheet.Cells[$"K{row}"].Value}".ConvertToDouble(),
                        Height = $"{worksheet.Cells[$"L{row}"].Value}".ConvertToDouble(),
                        Weight = $"{worksheet.Cells[$"N{row}"].Value}".ConvertToDouble(),
                    }
                ],
                WeightUnit = $"{worksheet.Cells[$"M{row}"].Value}",
                Shipper = new CAddress()
                {
                    Name = $"{worksheet.Cells[$"O{row}"].Value}",
                    Company = $"{worksheet.Cells[$"P{row}"].Value}",
                    AddressLine1 = $"{worksheet.Cells[$"Q{row}"].Value}",
                    AddressLine2 = $"{worksheet.Cells[$"R{row}"].Value}",
                    City = $"{worksheet.Cells[$"S{row}"].Value}",
                    State = $"{worksheet.Cells[$"T{row}"].Value}",
                    Zip = $"{worksheet.Cells[$"U{row}"].Value}",
                    CountryCode = $"{worksheet.Cells[$"V{row}"].Value}",
                    Phone = $"{worksheet.Cells[$"W{row}"].Value}",
                    IdNo = $"{worksheet.Cells[$"X{row}"].Value}",
                    TaxNo = $"{worksheet.Cells[$"Y{row}"].Value}",
                },
                Consignee = new CAddress()
                {
                    Name = $"{worksheet.Cells[$"Z{row}"].Value}",
                    Company = $"{worksheet.Cells[$"AA{row}"].Value}",
                    AddressLine1 = $"{worksheet.Cells[$"AB{row}"].Value}",
                    AddressLine2 = $"{worksheet.Cells[$"AC{row}"].Value}",
                    City = $"{worksheet.Cells[$"AD{row}"].Value}",
                    State = $"{worksheet.Cells[$"AE{row}"].Value}",
                    Zip = $"{worksheet.Cells[$"AF{row}"].Value}",
                    CountryCode = $"{worksheet.Cells[$"AG{row}"].Value}",
                    Phone = $"{worksheet.Cells[$"AH{row}"].Value}",
                    Email = $"{worksheet.Cells[$"AI{row}"].Value}",
                    IdNo = $"{worksheet.Cells[$"AJ{row}"].Value}",
                    TaxNo = $"{worksheet.Cells[$"AK{row}"].Value}",
                },
                CustomsCurrency = $"{worksheet.Cells[$"AL{row}"].Value}",
                Customs = []
            };
            newItem.UnitType = $"{newItem.WeightUnit}".ToLower() switch
            {
                "kg" => 2,
                "oz" => 1,
                _ => 0
            };
            var tmpData = $"{worksheet.Cells[$"AN{row}"].Value}";
            if (tmpData.NotIsNullOrEmpty())
            {
                newItem.Customs.Add(new CCustom()
                {
                    LocalDescription = $"{worksheet.Cells[$"AM{row}"].Value}",
                    DestDescription = $"{worksheet.Cells[$"AN{row}"].Value}",
                    HsCode = $"{worksheet.Cells[$"AO{row}"].Value}",
                    OriginCountryCode = $"{worksheet.Cells[$"AP{row}"].Value}",
                    UnitValue = $"{worksheet.Cells[$"AQ{row}"].Value}".ConvertToDouble(),
                    UnitWeight = $"{worksheet.Cells[$"AR{row}"].Value}".ConvertToDouble(),
                    Qty = $"{worksheet.Cells[$"AS{row}"].Value}".ConvertToInt(),
                });
            }

            tmpData = $"{worksheet.Cells[$"AU{row}"].Value}";
            if (tmpData.NotIsNullOrEmpty())
            {
                newItem.Customs.Add(new CCustom()
                {
                    LocalDescription = $"{worksheet.Cells[$"AT{row}"].Value}",
                    DestDescription = $"{worksheet.Cells[$"AU{row}"].Value}",
                    HsCode = $"{worksheet.Cells[$"AV{row}"].Value}",
                    OriginCountryCode = $"{worksheet.Cells[$"AW{row}"].Value}",
                    UnitValue = $"{worksheet.Cells[$"AX{row}"].Value}".ConvertToDouble(),
                    UnitWeight = $"{worksheet.Cells[$"AY{row}"].Value}".ConvertToDouble(),
                    Qty = $"{worksheet.Cells[$"AZ{row}"].Value}".ConvertToInt(),
                });
            }

            tmpData = $"{worksheet.Cells[$"BB{row}"].Value}";
            if (tmpData.NotIsNullOrEmpty())
            {
                newItem.Customs.Add(new CCustom()
                {
                    LocalDescription = $"{worksheet.Cells[$"BA{row}"].Value}",
                    DestDescription = $"{worksheet.Cells[$"BB{row}"].Value}",
                    HsCode = $"{worksheet.Cells[$"BC{row}"].Value}",
                    OriginCountryCode = $"{worksheet.Cells[$"BD{row}"].Value}",
                    UnitValue = $"{worksheet.Cells[$"BE{row}"].Value}".ConvertToDouble(),
                    UnitWeight = $"{worksheet.Cells[$"BF{row}"].Value}".ConvertToDouble(),
                    Qty = $"{worksheet.Cells[$"BG{row}"].Value}".ConvertToInt(),
                });
            }

            tmpData = $"{worksheet.Cells[$"BI{row}"].Value}";
            if (tmpData.NotIsNullOrEmpty())
            {
                newItem.Customs.Add(new CCustom()
                {
                    LocalDescription = $"{worksheet.Cells[$"BH{row}"].Value}",
                    DestDescription = $"{worksheet.Cells[$"BI{row}"].Value}",
                    HsCode = $"{worksheet.Cells[$"BJ{row}"].Value}",
                    OriginCountryCode = $"{worksheet.Cells[$"BK{row}"].Value}",
                    UnitValue = $"{worksheet.Cells[$"BL{row}"].Value}".ConvertToDouble(),
                    UnitWeight = $"{worksheet.Cells[$"BM{row}"].Value}".ConvertToDouble(),
                    Qty = $"{worksheet.Cells[$"BN{row}"].Value}".ConvertToInt(),
                });
            }

            var oFindSer = services.FirstOrDefault(w => w.ServiceCode == newItem.ServiceCode);
            if (oFindSer is null) continue;
            newItem.Weight = newItem.Boxes!.Sum(s => s.Weight);
            newItem.ApiName = oFindSer.ApiName;
            newItem.ServiceId = $"{oFindSer.Id}";
            newItem.CustomerId = customerId;
            if (newItem.Consignee.Email.IsNullOrEmpty())
            {
                newItem.Consignee.Email = null;
            }

            var valResult = await valshipment.ValidateAsync(newItem);
            if (valResult.IsValid)
            {
                lstDetail.Add(newItem);
            }
        }

        if (lstDetail.Count == 0) return false;

        foreach (var item in lstDetail)
        {
            await mediator.Send(new AddEditShipmentCommand()
            {
                Request = new AddEditDataRequest<CShipmentDto>()
                {
                    Data = item,
                    Action = ActionCommandType.Add
                }
            });
        }

        return true;
    }

    public async Task<DownloadFileContent> DownloadPrice(CPriceDto data)
    {
        var rootPath = host.WebRootPath;
        var fileName = "TemplatePrice.xlsx";
        var fileInfo = new FileInfo(Path.Combine(rootPath, "TemplateExcel", fileName));
        using var pck = new ExcelPackage(fileInfo);
        var worksheet = pck.Workbook.Worksheets[1];
        var tmpPriceDetails = data.Details?.OrderBy(o => o.GoodType).ToList();
        worksheet.Cells["B1"].Value = data.Id.ToString();
        worksheet.Cells["B2"].Value = data.PriceCode;
        worksheet.Cells["B3"].Value = data.PriceName;
        if (data.Zones is { Count: > 0 })
        {
            var iRow = 9;
            var iCol = 7;
            foreach (var item in data.Zones)
            {
                worksheet.Cells[3, iCol].Value = item.Country;
                worksheet.Cells[5, iCol].Value = item.Zone;
                iCol++;
            }

            foreach (var item in tmpPriceDetails!.OrderBy(o => o.Min).ToList())
            {
                worksheet.Cells[$"A{iRow}"].Value = item.ServiceCode;
                worksheet.Cells[$"B{iRow}"].Value = item.GoodType;
                worksheet.Cells[$"C{iRow}"].Value = item.PriceType;
                worksheet.Cells[$"D{iRow}"].Value = item.ChargeWeightType;
                worksheet.Cells[$"E{iRow}"].Value = item.Min;
                worksheet.Cells[$"F{iRow}"].Value = item.Max;
                iCol = 7;
                var aPrice = $"{item.Price}".SplitExt(" ");
                foreach (var price in aPrice)
                {
                    worksheet.Cells[iRow, iCol].Value = $"{price}".ConvertToDouble();
                    iCol++;
                }

                iRow++;
            }
        }

        await Task.Delay(200);
        return new DownloadFileContent()
        {
            content = pck.GetAsByteArray(),
        };
    }

    public DownloadFileContent DownloadShipment(List<CShipmentReport> data, bool isAcc, List<CCustomerDto> customers)
    {
        using var pck = new ExcelPackage();
        var ws = pck.Workbook.Worksheets.Add("Shipment");
        var iRow = 1;
        ws.Cells[$"A{iRow}"].Value = "Date";
        ws.Cells[$"B{iRow}"].Value = "Ref";
        ws.Cells[$"C{iRow}"].Value = "Ref 2";
        ws.Cells[$"D{iRow}"].Value = "Ref 3";
        ws.Cells[$"E{iRow}"].Value = "Service";
        ws.Cells[$"F{iRow}"].Value = "Entry Point";
        ws.Cells[$"G{iRow}"].Value = "Name";
        ws.Cells[$"H{iRow}"].Value = "Address";
        ws.Cells[$"I{iRow}"].Value = "Address 1";
        ws.Cells[$"J{iRow}"].Value = "State";
        ws.Cells[$"K{iRow}"].Value = "Zip";
        ws.Cells[$"L{iRow}"].Value = "City";
        ws.Cells[$"M{iRow}"].Value = "Country";
        ws.Cells[$"N{iRow}"].Value = "Weight";
        ws.Cells[$"O{iRow}"].Value = "Weight Unit";
        ws.Cells[$"P{iRow}"].Value = "Zone";
        ws.Cells[$"Q{iRow}"].Value = "Tracking";
        if (isAcc)
        {
            ws.Cells[$"R{iRow}"].Value = "Customer";
            ws.Cells[$"S{iRow}"].Value = "Price";
            ws.Cells[$"T{iRow}"].Value = "Cost";
        }
        else
        {
            ws.Cells[$"R{iRow}"].Value = "Price";
        }

        iRow = 2;
        foreach (var item in data)
        {
            ws.Cells[$"A{iRow}"].Value = item.CreatedOn.ToDmy();
            ws.Cells[$"B{iRow}"].Value = item.ReferenceId;
            ws.Cells[$"C{iRow}"].Value = item.ReferenceId2;
            ws.Cells[$"D{iRow}"].Value = item.ReferenceId3;
            ws.Cells[$"E{iRow}"].Value = item.ServiceCode;
            ws.Cells[$"F{iRow}"].Value = item.EntryPoint;
            ws.Cells[$"G{iRow}"].Value = item.Consignee?.Name;
            ws.Cells[$"H{iRow}"].Value = item.Consignee?.AddressLine1;
            ws.Cells[$"I{iRow}"].Value = item.Consignee?.AddressLine2;
            ws.Cells[$"J{iRow}"].Value = item.Consignee?.State;
            ws.Cells[$"K{iRow}"].Value = item.Consignee?.Zip;
            ws.Cells[$"L{iRow}"].Value = item.Consignee?.City;
            ws.Cells[$"M{iRow}"].Value = item.Consignee?.CountryCode;
            ws.Cells[$"N{iRow}"].Value = item.ChargeWeight;
            ws.Cells[$"O{iRow}"].Value = item.WeightUnit;
            ws.Cells[$"P{iRow}"].Value = item.ZonePrice;
            var rng = ws.Cells[$"Q{iRow}"];
            if ($"{item.ServiceCode}".Contains("usps", StringComparison.OrdinalIgnoreCase) &&
                item.TrackIds.NotIsNullOrEmpty())
            {
                rng.Hyperlink =
                    new Uri(
                        $"https://tools.usps.com/go/TrackConfirmAction?tRef=fullpage&tLc=2&text28777=&tLabels={item.TrackIds}",
                        UriKind.Absolute);
                rng.Style.Font.UnderLine = true;
                rng.Style.Font.Color.SetColor(Color.Blue);
            }

            rng.Value = item.TrackIds;
            if (isAcc)
            {
                ws.Cells[$"O{iRow}"].Value = customers.FirstOrDefault(w => w.Id.ToString() == item.CustomerId)?.Name;
                ws.Cells[$"S{iRow}"].Value = item.Cost;
            }
            ws.Cells[$"R{iRow}"].Value = item.Price;

            iRow++;
        }

        return new DownloadFileContent()
        {
            fileType = MimeTypes.TextXlsx,
            code = $"Shipment_{new Random().Next(10000)}",
            content = pck.GetAsByteArray()
        };
    }

    public async Task<DownloadFileContent> DownloadTemplate(GetReportRequest request)
    {
        await Task.Delay(100);
        var rootPath = host.WebRootPath;
        var fileInfo = new FileInfo(Path.Combine(rootPath, "TemplateExcel", "TemplateShipment.xlsx"));
        using var pck = new ExcelPackage(fileInfo);
        return new DownloadFileContent()
        {
            fileType = MimeTypes.TextXlsx,
            code = "TemplateShipment",
            content = pck.GetAsByteArray()
        };
    }

    private async Task<bool> AddPriceData(ExcelWorksheet worksheet)
    {
        var blnSaveAll = false;
        var lstDetail = new List<CPriceDetail>();
        var id = $"{worksheet.Cells["B1"].Value}".ToGuid();
        var dicCountry = new List<PriceZone>();
        for (var i = 1; i <= 50; i++)
        {
            var sData = $"{worksheet.Cells[3, i + 6].Value}";
            if (sData.IsNullOrEmpty()) break;
            dicCountry.Add(new PriceZone()
            {
                Country = sData,
                Zone = $"{worksheet.Cells[5, i + 6].Value}"
            });
        }

        if (dicCountry is { Count: 0 }) return false;
        var rowCount = worksheet.Dimension.Rows;
        for (var row = 9; row <= rowCount; row++)
        {
            var goodType = Convert.ToInt32(worksheet.Cells[$"B{row}"].Value ?? 0);
            var priceType = Convert.ToInt32(worksheet.Cells[$"C{row}"].Value ?? 0);
            var newData = new CPriceDetail()
            {
                ServiceCode = $"{worksheet.Cells[$"A{row}"].Value}",
                CPriceId = Guid.Empty,
                GoodType = goodType,
                PriceType = priceType,
                ChargeWeightType = Convert.ToInt32(worksheet.Cells[$"D{row}"].Value ?? 0),
                Min = Convert.ToInt32(worksheet.Cells[$"E{row}"].Value ?? 0),
                Max = Convert.ToDouble(worksheet.Cells[$"F{row}"].Value ?? 0).ToRound(2)
            };
            for (var i = 1; i <= dicCountry.Count; i++)
            {
                var uPrice = Convert.ToDouble(worksheet.Cells[row, i + 6].Value ?? 0).ToRound(2);
                newData.Price += $"{uPrice} ";
            }

            if (newData.Min + newData.Max > 0) lstDetail.Add(newData);
        }

        if (lstDetail.Count == 0) return false;
        try
        {
            var oFind = await unitOfWork.RepositoryNew<CPrice>().Entities.Include(u => u.Details)
                .FirstOrDefaultAsync(w => w.Id == id);
            if (oFind == null) return false;
            oFind.Zones = dicCountry;
            oFind.Details ??= [];
            oFind.Details.Clear();
            oFind.Details.AddRange(lstDetail);
            await unitOfWork.RepositoryNew<CPrice>().UpdateAsync(oFind);
            await unitOfWork.CommitAndRemoveCache(CancellationToken.None, Caches.GetAllPriceCacheKey);
            blnSaveAll = true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return blnSaveAll;
    }
}