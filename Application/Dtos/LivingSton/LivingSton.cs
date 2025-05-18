namespace LeUs.Application.Dtos.LivingSton;

public class LCreateProjectRequest
{
    public string? ProjectName { get; set; }
    public bool IsOutputACERoad { get; set; }
    public bool IsOutputACEAirACAS { get; set; }
    public bool IsOutputACEAirAMS { get; set; }
    public bool IsOutputEntry { get; set; }
    public bool IsOutputEntryFormal { get; set; }
    public bool IsOutputEntryInformal { get; set; }
    public bool IsOutputProforma { get; set; }
    public bool IsOutputInbond { get; set; }
    public bool IsOutputACIShipment { get; set; }
    public bool IsOutputACIHouseBill { get; set; }
    public bool IgnoreBadAddress { get; set; }
    public List<LProjectLine>? ProjectLines { get; set; }
}

public class LProjectLine
{
    public string? MasterIssuerCode { get; set; }
    public string? MasterBillNumber { get; set; }
    public string? HouseIssuerCode { get; set; }
    public string? HouseBillNum { get; set; }
    public string? ModeTrans { get; set; }
    public string? ShipperRefNum { get; set; }
    public string? EntryNumber { get; set; }
    public string? PortOfEntry { get; set; }
    public string? PortOfUnlading { get; set; }
    public string? PortofLoading { get; set; }
    public string? ImportingCarrierCode { get; set; }
    public string? ImportingCarrierName { get; set; }
    public string? FIRMSCode { get; set; }
    public string? ArrivalDate { get; set; }
    public string? ExportDate { get; set; }
    public string? ArrivalAirport { get; set; }
    public string? CargoTerminalOperator { get; set; }
    public string? ACEAirTripTypeKey { get; set; }
    public string? OriginAirport { get; set; }
    public string? PortofLoadingType { get; set; }
    public string? VoyageOrFlightNumber { get; set; }
    public string? ImportingCarrierTaxID { get; set; }
    public string? ImportingCarrierCountry { get; set; }
    public string? InBondNumber { get; set; }
    public string? InbondEntryType { get; set; }
    public string? CarrierCode { get; set; }
    public string? MarksAndNumbers { get; set; }
    public string? StateOfDestination { get; set; }
    public string? CrossingPort { get; set; }
    public string? PortOfDestination { get; set; }
    public string? CustomsValue { get; set; }
    public string? ITNumber { get; set; }
    public string? ShipperName { get; set; }
    public string? ShipperStreetAddress { get; set; }
    public string? ShipperStreetAddress2 { get; set; }
    public string? ShipperStreetAddress3 { get; set; }
    public string? ShipperCity { get; set; }
    public string? ShipperStateOrProvince { get; set; }
    public string? ShipperPostalCode { get; set; }
    public string? ShipperCountry { get; set; }
    public string? ShipperTelephone { get; set; }
    public string? ShipperEmail { get; set; }
    public string? ConsigneeName { get; set; }
    public string? ConsigneeStreetAddress { get; set; }
    public string? ConsigneeStreetAddress2 { get; set; }
    public string? ConsigneeStreetAddress3 { get; set; }
    public string? ConsigneeCity { get; set; }
    public string? ConsigneePostalCode { get; set; }
    public string? ConsigneeStateOrProvince { get; set; }
    public string? ConsigneeCountry { get; set; }
    public string? ConsigneeTelephone { get; set; }
    public string? ConsigneeEmail { get; set; }
    public string? ImporterNumber { get; set; }
    public string? ImporterName { get; set; }
    public string? ImporterStreetAddress { get; set; }
    public string? ImporterCity { get; set; }
    public string? ImporterStateOrProvince { get; set; }
    public string? ImporterPostalCode { get; set; }
    public string? ImporterCountry { get; set; }
    public string? ManufacturerName { get; set; }
    public string? ManufacturerStreetAddress { get; set; }
    public string? ManufacturerCity { get; set; }
    public string? ManufacturerPostalCode { get; set; }
    public string? ManufacturerCountry { get; set; }
    public string? ManufacturerStateOrProvince { get; set; }
    public string? ManufacturerID { get; set; }
    public string? SellerName { get; set; }
    public string? SellerStreetAddress { get; set; }
    public string? SellerCity { get; set; }
    public string? SellerPostalCode { get; set; }
    public string? SellerCountry { get; set; }
    public string? SellerStateOrProvince { get; set; }
    public string? LastMileProvider { get; set; }
    public string? LastMileServiceCode { get; set; }
    public bool IsResidential { get; set; }
    public string? LabelSize { get; set; }
    public bool GenerateNewLabel { get; set; }
    public double InsuredValue { get; set; }
    public bool IsECCF { get; set; }
    public List<LProjectLineItem>? ProjectLineItems { get; set; }
}

public class LProjectLineItem
{
    public string? Description { get; set; }
    public string? Weight { get; set; }
    public string? WeightCode { get; set; }
    public string? CustomsValue { get; set; }
    public string? ValueForeign { get; set; }
    public string? MarksAndNumbers { get; set; }
    public string? ContainerNumber { get; set; }
    public string? NotifyPartyName { get; set; }
    public string? NotifyPartyStreetAddress { get; set; }
    public string? NotifyPartyStreetAddress2 { get; set; }
    public string? NotifyPartyStreetAddress3 { get; set; }
    public string? NotifyPartyCity { get; set; }
    public string? NotifyPartyStateOrProvince { get; set; }
    public string? NotifyPartyPostalCode { get; set; }
    public string? NotifyPartyCountry { get; set; }
    public string? NotifyPartyTelephone { get; set; }
    public string? ProductPartNumber { get; set; }
    public string? HTSNumber1 { get; set; }
    public string? HTSNumber1QTY1 { get; set; }
    public string? HTSNumber1QTY2 { get; set; }
    public string? HTSNumber1QTY3 { get; set; }
    public string? HTSNumber1UOM1 { get; set; }
    public string? HTSNumber1UOM2 { get; set; }
    public string? HTSNumber1UOM3 { get; set; }
    public bool IsDisclaim { get; set; }
    public string? Pieces { get; set; }
    public string? CurrencyCode { get; set; }
    public string? PgaQty1 { get; set; }
    public string? PgaUom1 { get; set; }
    public string? PgaQty2 { get; set; }
    public string? PgaUom2 { get; set; }
    public string? PgaQty3 { get; set; }
    public string? PgaUom3 { get; set; }
    public string? PgaQty4 { get; set; }
    public string? PgaUom4 { get; set; }
    public string? PgaQty5 { get; set; }
    public string? PgaUom5 { get; set; }
    public string? PgaQty6 { get; set; }
    public string? PgaUom6 { get; set; }
    public string? BoxCEntryPoint { get; set; }
    public string? PackageUOM { get; set; }
    public string? CountryOfOrigin { get; set; }
}


public class LCreateProjectResponse
{
    public LProjectResponse? Project { get; set; }
    public string? ProcessingStatus { get; set; }
    public List<LValidation>? Validations { get; set; }
}

public class LCreateProjectLineResponse
{
    public LProjectLineResponse? Project { get; set; }
    public string? ProcessingStatus { get; set; }
    public List<LValidation>? Validations { get; set; }
}
public class LProjectResponse
{
    public int ProjectID { get; set; }
    public string? ProjectName { get; set; }
    public bool IsOutputACERoad { get; set; }
    public bool IsOutputACEAirACAS { get; set; }
    public bool IsOutputACEAirAMS { get; set; }
    public bool IsOutputEntry { get; set; }
    public bool IsOutputEntryFormal { get; set; }
    public bool IsOutputEntryInformal { get; set; }
    public bool IsOutputProforma { get; set; }
    public bool IsOutputInbond { get; set; }
    public bool IsOutputACIShipment { get; set; }
    public bool IsOutputACIHouseBill { get; set; }
    public int ProfileID { get; set; }
    public string? CreatedBy { get; set; }
    public string? CreatedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public string? ModifiedOn { get; set; }
    public bool IsDeleted { get; set; }
    public bool IgnoreBadAddress { get; set; }
    public List<LProjectLineResponse>? ProjectLines { get; set; }
}

public class LProjectLineResponse
{
    public int ProjectLineID { get; set; }
    public string? CustomerOrderNumber { get; set; }
    public bool Success { get; set; }
    public string? TrackType { get; set; }
    public string? Remark { get; set; }
    public string? AgentNumber { get; set; }
    public string? WayBillNumber { get; set; }
    public string? TrackingNumber { get; set; }
    public string? TrackingSentOn { get; set; }
    public string? LabelSentOn { get; set; }
    public bool IsDeleted { get; set; }
    public List<LProjectLineItemResponse>? ProjectLineItems { get; set; }
    public string? MasterIssuerCode { get; set; }
    public string? MasterBillNumber { get; set; }
    public string? HouseIssuerCode { get; set; }
    public string? HouseBillNum { get; set; }
    public string? ModeTrans { get; set; }
    public string? ShipperRefNum { get; set; }
    public string? EntryNumber { get; set; }
    public string? PortOfEntry { get; set; }
    public string? PortOfUnlading { get; set; }
    public string? PortofLoading { get; set; }
    public string? ImportingCarrierCode { get; set; }
    public string? ImportingCarrierName { get; set; }
    public string? FIRMSCode { get; set; }
    public string? ArrivalDate { get; set; }
    public string? ExportDate { get; set; }
    public string? ArrivalAirport { get; set; }
    public string? CargoTerminalOperator { get; set; }
    public string? ACEAirTripTypeKey { get; set; }
    public string? OriginAirport { get; set; }
    public string? PortofLoadingType { get; set; }
    public string? VoyageOrFlightNumber { get; set; }
    public string? ImportingCarrierTaxID { get; set; }
    public string? ImportingCarrierCountry { get; set; }
    public string? InBondNumber { get; set; }
    public string? InbondEntryType { get; set; }
    public string? CarrierCode { get; set; }
    public string? MarksAndNumbers { get; set; }
    public string? StateOfDestination { get; set; }
    public string? CrossingPort { get; set; }
    public string? PortOfDestination { get; set; }
    public string? CustomsValue { get; set; }
    public string? ITNumber { get; set; }
    public string? ShipperName { get; set; }
    public string? ShipperStreetAddress { get; set; }
    public string? ShipperStreetAddress2 { get; set; }
    public string? ShipperStreetAddress3 { get; set; }
    public string? ShipperCity { get; set; }
    public string? ShipperStateOrProvince { get; set; }
    public string? ShipperPostalCode { get; set; }
    public string? ShipperCountry { get; set; }
    public string? ShipperTelephone { get; set; }
    public string? ShipperEmail { get; set; }
    public string? ConsigneeName { get; set; }
    public string? ConsigneeStreetAddress { get; set; }
    public string? ConsigneeStreetAddress2 { get; set; }
    public string? ConsigneeStreetAddress3 { get; set; }
    public string? ConsigneeCity { get; set; }
    public string? ConsigneePostalCode { get; set; }
    public string? ConsigneeStateOrProvince { get; set; }
    public string? ConsigneeCountry { get; set; }
    public string? ConsigneeTelephone { get; set; }
    public string? ConsigneeEmail { get; set; }
    public string? ImporterNumber { get; set; }
    public string? ImporterName { get; set; }
    public string? ImporterStreetAddress { get; set; }
    public string? ImporterCity { get; set; }
    public string? ImporterStateOrProvince { get; set; }
    public string? ImporterPostalCode { get; set; }
    public string? ImporterCountry { get; set; }
    public string? ManufacturerName { get; set; }
    public string? ManufacturerStreetAddress { get; set; }
    public string? ManufacturerCity { get; set; }
    public string? ManufacturerPostalCode { get; set; }
    public string? ManufacturerCountry { get; set; }
    public string? ManufacturerStateOrProvince { get; set; }
    public string? ManufacturerID { get; set; }
    public string? SellerName { get; set; }
    public string? SellerStreetAddress { get; set; }
    public string? SellerCity { get; set; }
    public string? SellerPostalCode { get; set; }
    public string? SellerCountry { get; set; }
    public string? SellerStateOrProvince { get; set; }
    public string? LastMileProvider { get; set; }
    public string? LastMileServiceCode { get; set; }
    public bool IsResidential { get; set; }
    public string? LabelSize { get; set; }
    public bool GenerateNewLabel { get; set; }
    public double InsuredValue { get; set; }
    public bool IsECCF { get; set; }
}

public class LProjectLineItemResponse
{
    public int ProjectLineItemID { get; set; }
    public string? Description { get; set; }
    public string? Weight { get; set; }
    public string? WeightCode { get; set; }
    public string? CustomsValue { get; set; }
    public string? ValueForeign { get; set; }
    public string? MarksAndNumbers { get; set; }
    public string? ContainerNumber { get; set; }
    public string? NotifyPartyName { get; set; }
    public string? NotifyPartyStreetAddress { get; set; }
    public string? NotifyPartyStreetAddress2 { get; set; }
    public string? NotifyPartyStreetAddress3 { get; set; }
    public string? NotifyPartyCity { get; set; }
    public string? NotifyPartyStateOrProvince { get; set; }
    public string? NotifyPartyPostalCode { get; set; }
    public string? NotifyPartyCountry { get; set; }
    public string? NotifyPartyTelephone { get; set; }
    public string? ProductPartNumber { get; set; }
    public string? HTSNumber1 { get; set; }
    public string? HTSNumber1QTY1 { get; set; }
    public string? HTSNumber1QTY2 { get; set; }
    public string? HTSNumber1QTY3 { get; set; }
    public string? HTSNumber1UOM1 { get; set; }
    public string? HTSNumber1UOM2 { get; set; }
    public string? HTSNumber1UOM3 { get; set; }
    public bool IsDisclaim { get; set; }
    public string? Pieces { get; set; }
    public string? CurrencyCode { get; set; }
    public string? PgaQty1 { get; set; }
    public string? PgaUom1 { get; set; }
    public string? PgaQty2 { get; set; }
    public string? PgaUom2 { get; set; }
    public string? PgaQty3 { get; set; }
    public string? PgaUom3 { get; set; }
    public string? PgaQty4 { get; set; }
    public string? PgaUom4 { get; set; }
    public string? PgaQty5 { get; set; }
    public string? PgaUom5 { get; set; }
    public string? PgaQty6 { get; set; }
    public string? PgaUom6 { get; set; }
    public string? BoxCEntryPoint { get; set; }
    public string? PackageUOM { get; set; }
    public string? CountryOfOrigin { get; set; }
}

public class LValidation
{
    public int ProjectLineItemID { get; set; }
    public string? ProjectLineID { get; set; }
    public string? FieldName { get; set; }
    public string? ValidationKey { get; set; }
    public string? Message { get; set; }
}

