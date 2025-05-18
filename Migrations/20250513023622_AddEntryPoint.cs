using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeUs.Migrations
{
    /// <inheritdoc />
    public partial class AddEntryPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntryPoint",
                table: "cstoreaddresss",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryPoint",
                table: "cstoreaddresss");
        }
    }
}
