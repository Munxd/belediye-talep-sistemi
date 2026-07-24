using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BelediyeTalepSistemi.Migrations
{
    /// <inheritdoc />
    public partial class TalepKonumFotografAlanlari : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcikAdres",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Boylam",
                table: "Talepler",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Enlem",
                table: "Talepler",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotografYolu",
                table: "Talepler",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcikAdres",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "Boylam",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "Enlem",
                table: "Talepler");

            migrationBuilder.DropColumn(
                name: "FotografYolu",
                table: "Talepler");
        }
    }
}
