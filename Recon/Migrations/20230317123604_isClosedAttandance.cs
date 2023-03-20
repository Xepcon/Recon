using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recon.Migrations
{
    /// <inheritdoc />
    public partial class isClosedAttandance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isClosed",
                table: "Attendances",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isClosed",
                table: "Attendances");
        }
    }
}
