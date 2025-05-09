using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace muhtarlik.Migrations
{
    /// <inheritdoc />
    public partial class MuhtarGuncelle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "Muhtars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "Muhtars");
        }
    }
}
