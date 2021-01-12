using Microsoft.EntityFrameworkCore.Migrations;

namespace NGTI.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupReservations_Tables_TableId",
                table: "GroupReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_SoloReservations_Tables_TableId",
                table: "SoloReservations");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropIndex(
                name: "IX_SoloReservations_TableId",
                table: "SoloReservations");

            migrationBuilder.DropIndex(
                name: "IX_GroupReservations_TableId",
                table: "GroupReservations");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "SoloReservations");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "GroupReservations");

            migrationBuilder.AddColumn<string>(
                name: "Seat",
                table: "SoloReservations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seat",
                table: "GroupReservations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seat",
                table: "SoloReservations");

            migrationBuilder.DropColumn(
                name: "Seat",
                table: "GroupReservations");

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "SoloReservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "GroupReservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.TableId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoloReservations_TableId",
                table: "SoloReservations",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupReservations_TableId",
                table: "GroupReservations",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupReservations_Tables_TableId",
                table: "GroupReservations",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "TableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoloReservations_Tables_TableId",
                table: "SoloReservations",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "TableId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
