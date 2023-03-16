using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recon.Migrations
{
    /// <inheritdoc />
    public partial class removePhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Person");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Phone",
                table: "Person",
                type: "bit",
                nullable: true);
        }
    }
}
