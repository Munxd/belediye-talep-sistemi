using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BelediyeTalepSistemi.Migrations
{
    /// <inheritdoc />
    public partial class PersonelMudurlukVeTalepAktiflik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AktifMi",
                table: "Talepler",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MudurlukId",
                table: "ApplicationUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_MudurlukId",
                table: "ApplicationUsers",
                column: "MudurlukId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Mudurlukler_MudurlukId",
                table: "ApplicationUsers",
                column: "MudurlukId",
                principalTable: "Mudurlukler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Mudurlukler_MudurlukId",
                table: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUsers_MudurlukId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "AktifMi",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "MudurlukId",
                table: "ApplicationUsers");
        }
    }
}
