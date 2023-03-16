using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recon.Migrations
{
    /// <inheritdoc />
    public partial class removeLunchTIme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LunchEndTime",
                table: "WorkTimeUsers");

            migrationBuilder.DropColumn(
                name: "LunchStartTime",
                table: "WorkTimeUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LunchEndTime",
                table: "WorkTimeUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LunchStartTime",
                table: "WorkTimeUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
