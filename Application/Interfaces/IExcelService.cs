namespace LeUs.Application.Interfaces;

public interface IExcelService: IManager
{
    Task<List<UploadResult>> UpLoad(UploadCommandRequest request);
    Task<DownloadFileContent> DownloadPrice(CPriceDto data);
    DownloadFileContent DownloadShipment(List<CShipmentReport> data, bool isAcc, List<CCustomerDto> customers);
    Task<DownloadFileContent> DownloadTemplate(GetReportRequest request);
    
}