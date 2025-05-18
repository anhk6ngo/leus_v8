using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeUs.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "caddresss",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Company = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AddressLine1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AddressLine2 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    County = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Zip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PhoneNumberExt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IdNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaxNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaxNoType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaxNoIssuerCountryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_caddresss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ccountrys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    CountryName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Continent = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ccountrys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ccustomers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ContactPerson = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaxNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BankAccount = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BankName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    SignContract = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ccustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cprices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PriceName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CustomerId = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    FromDate = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    ToDate = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true),
                    IsPercent = table.Column<bool>(type: "boolean", nullable: false),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    UnitType = table.Column<int>(type: "integer", nullable: false),
                    Ratio = table.Column<int>(type: "integer", nullable: false),
                    MaxCubic = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(255)", nullable: true),
                    Zones = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cprices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cservices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceCode = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    ServiceName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    HasApi = table.Column<bool>(type: "boolean", nullable: false),
                    ApiName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsHide = table.Column<bool>(type: "boolean", nullable: false),
                    ServiceType = table.Column<int>(type: "integer", nullable: false),
                    ServiceNo = table.Column<int>(type: "integer", nullable: false),
                    GoodType = table.Column<int>(type: "integer", nullable: false),
                    UseLocation = table.Column<bool>(type: "boolean", nullable: false),
                    Numerator = table.Column<int>(type: "integer", nullable: false),
                    UnitType = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cservices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cshipmentss",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SyncGetLabel = table.Column<bool>(type: "boolean", nullable: false),
                    ReferenceId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ShipmentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ApiName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ApiName1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UnitType = table.Column<int>(type: "integer", nullable: false),
                    TrackIds = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ZonePrice = table.Column<int>(type: "integer", nullable: false),
                    ReferenceId2 = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ReferenceId3 = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    TrackingNo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    EntryPoint = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ServiceCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ServiceCode1 = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ClearanceType = table.Column<int>(type: "integer", nullable: false),
                    DutyType = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    DimensionUnit = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    FbaCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    FbaShipmentId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FbaPoId = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    WeightUnit = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    CustomsCurrency = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    BoxQty = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    SignatureRequired = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    PackageType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    BatteryType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    PromotionCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    PromotionAmount = table.Column<double>(type: "double precision", nullable: true),
                    CustomerId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    PriceCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    Cost = table.Column<double>(type: "double precision", nullable: true),
                    Remote = table.Column<double>(type: "double precision", nullable: true),
                    CancelFee = table.Column<double>(type: "double precision", nullable: true),
                    CreateLabelDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ChargeWeight = table.Column<double>(type: "double precision", nullable: true),
                    ShipmentStatus = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(255)", nullable: true),
                    Boxes = table.Column<string>(type: "jsonb", nullable: true),
                    Cod = table.Column<string>(type: "jsonb", nullable: true),
                    Consignee = table.Column<string>(type: "jsonb", nullable: true),
                    Customs = table.Column<string>(type: "jsonb", nullable: true),
                    Labels = table.Column<string>(type: "jsonb", nullable: true),
                    PackageCustomerReferences = table.Column<string>(type: "jsonb", nullable: true),
                    Products = table.Column<string>(type: "jsonb", nullable: true),
                    Shipper = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cshipmentss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cstoreaddresss",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Company = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AddressLine1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AddressLine2 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    County = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Zip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PhoneNumberExt = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IdNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaxNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaxNoType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TaxNoIssuerCountryCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ServiceCode = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cstoreaddresss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ctopups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    RequestAmount = table.Column<double>(type: "double precision", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ApproveAmount = table.Column<double>(type: "double precision", nullable: true),
                    ApproveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AccNote = table.Column<string>(type: "text", nullable: true),
                    TransactionId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    GateWay = table.Column<int>(type: "integer", nullable: false),
                    IsDeposit = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "character varying(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ctopups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userbalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    DepositAmount = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userbalances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cpricedetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CPriceId = table.Column<Guid>(type: "uuid", nullable: false),
                    GoodType = table.Column<int>(type: "integer", nullable: false),
                    PriceType = table.Column<int>(type: "integer", nullable: false),
                    Min = table.Column<int>(type: "integer", nullable: false),
                    Max = table.Column<double>(type: "double precision", nullable: false),
                    Price = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ServiceCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ChargeWeightType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cpricedetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cpricedetails_cprices_CPriceId",
                        column: x => x.CPriceId,
                        principalTable: "cprices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ccustomers_Email",
                table: "ccustomers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_cpricedetails_CPriceId",
                table: "cpricedetails",
                column: "CPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_cshipmentss_ReferenceId",
                table: "cshipmentss",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_UserIndex",
                table: "cshipmentss",
                columns: new[] { "IsActive", "CreatedOn", "CreatedBy" });

            migrationBuilder.CreateIndex(
                name: "IX_ctopups_IsActive_RequestDate_Status_UserId",
                table: "ctopups",
                columns: new[] { "IsActive", "RequestDate", "Status", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "caddresss");

            migrationBuilder.DropTable(
                name: "ccountrys");

            migrationBuilder.DropTable(
                name: "ccustomers");

            migrationBuilder.DropTable(
                name: "cpricedetails");

            migrationBuilder.DropTable(
                name: "cservices");

            migrationBuilder.DropTable(
                name: "cshipmentss");

            migrationBuilder.DropTable(
                name: "cstoreaddresss");

            migrationBuilder.DropTable(
                name: "ctopups");

            migrationBuilder.DropTable(
                name: "userbalances");

            migrationBuilder.DropTable(
                name: "cprices");
        }
    }
}
