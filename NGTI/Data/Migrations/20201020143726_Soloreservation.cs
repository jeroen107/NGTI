using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NGTI.Data.Migrations
{
    public partial class Soloreservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoloReservations",
                columns: table => new
                {
                    IdSoloReservation = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    TableId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoloReservations", x => x.IdSoloReservation);
                    table.ForeignKey(
                        name: "FK_SoloReservations_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoloReservations_TableId",
                table: "SoloReservations",
                column: "TableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoloReservations");
        }
    }
}
