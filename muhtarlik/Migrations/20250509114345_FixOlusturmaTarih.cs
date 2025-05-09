using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace muhtarlik.Migrations
{
    /// <inheritdoc />
    public partial class FixOlusturmaTarih : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OlusturmaTarih",
                table: "Muhtars",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OlusturmaTarih",
                table: "Muhtars",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
