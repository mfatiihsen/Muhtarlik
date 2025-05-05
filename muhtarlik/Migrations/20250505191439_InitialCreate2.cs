using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace muhtarlik.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iletisim_Vatandaslar_VatandasId",
                table: "Iletisim");

            migrationBuilder.DropIndex(
                name: "IX_Basvurular_VatandasId",
                table: "Basvurular");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Iletisim",
                table: "Iletisim");

            migrationBuilder.RenameTable(
                name: "Iletisim",
                newName: "Iletisimler");

            migrationBuilder.RenameIndex(
                name: "IX_Iletisim_VatandasId",
                table: "Iletisimler",
                newName: "IX_Iletisimler_VatandasId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Iletisimler",
                table: "Iletisimler",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Basvurular_VatandasId",
                table: "Basvurular",
                column: "VatandasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Iletisimler_Vatandaslar_VatandasId",
                table: "Iletisimler",
                column: "VatandasId",
                principalTable: "Vatandaslar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Iletisimler_Vatandaslar_VatandasId",
                table: "Iletisimler");

            migrationBuilder.DropIndex(
                name: "IX_Basvurular_VatandasId",
                table: "Basvurular");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Iletisimler",
                table: "Iletisimler");

            migrationBuilder.RenameTable(
                name: "Iletisimler",
                newName: "Iletisim");

            migrationBuilder.RenameIndex(
                name: "IX_Iletisimler_VatandasId",
                table: "Iletisim",
                newName: "IX_Iletisim_VatandasId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Iletisim",
                table: "Iletisim",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Basvurular_VatandasId",
                table: "Basvurular",
                column: "VatandasId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Iletisim_Vatandaslar_VatandasId",
                table: "Iletisim",
                column: "VatandasId",
                principalTable: "Vatandaslar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
