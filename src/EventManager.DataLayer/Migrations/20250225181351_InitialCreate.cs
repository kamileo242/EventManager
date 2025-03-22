using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManager.Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adres",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    miasto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ulica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numer_domu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    wydarzenia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adres", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "uzytkownik",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    imie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nazwisko = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uzytkownik", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "uzytkownik_wydarzenie",
                columns: table => new
                {
                    id_uzytkownika = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_wydarzenia = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    wplacona_kwota = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    data_dolaczenia = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uzytkownik_wydarzenie", x => new { x.id_uzytkownika, x.id_wydarzenia });
                });

            migrationBuilder.CreateTable(
                name: "wydarzenie",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nazwa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    data_rozpoczecia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_zakonczenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_adresu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    maksymalna_liczba_uczestnikow = table.Column<int>(type: "int", nullable: false),
                    koszt = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wydarzenie", x => x.id);
                    table.ForeignKey(
                        name: "FK_wydarzenie_adres_id_adresu",
                        column: x => x.id_adresu,
                        principalTable: "adres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_wydarzenie_id_adresu",
                table: "wydarzenie",
                column: "id_adresu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "uzytkownik");

            migrationBuilder.DropTable(
                name: "uzytkownik_wydarzenie");

            migrationBuilder.DropTable(
                name: "wydarzenie");

            migrationBuilder.DropTable(
                name: "adres");
        }
    }
}
