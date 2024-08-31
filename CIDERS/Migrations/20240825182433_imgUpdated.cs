using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIDERS.Migrations
{
    /// <inheritdoc />
    public partial class imgUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imgProfile",
                table: "ApiEmployee",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imgProfile",
                table: "ApiEmployee");
        }
    }
}
