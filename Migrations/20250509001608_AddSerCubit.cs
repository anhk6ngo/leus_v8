using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeUs.Migrations
{
    /// <inheritdoc />
    public partial class AddSerCubit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseCubic",
                table: "cservices",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "chistorylabels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Response = table.Column<string>(type: "text", nullable: true),
                    ReferenceId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    Request = table.Column<string>(type: "text", nullable: true),
                    CratedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chistorylabels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chistorylabels_ReferenceId",
                table: "chistorylabels",
                column: "ReferenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chistorylabels");

            migrationBuilder.DropColumn(
                name: "UseCubic",
                table: "cservices");
        }
    }
}
