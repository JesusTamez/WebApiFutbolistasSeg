using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiFutbolistasSeg.Migrations
{
    /// <inheritdoc />
    public partial class FutbolistaEquipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipos_Futbolistas_FutbolistaId",
                table: "Equipos");

            migrationBuilder.DropIndex(
                name: "IX_Equipos_FutbolistaId",
                table: "Equipos");

            migrationBuilder.DropColumn(
                name: "FutbolistaId",
                table: "Equipos");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Futbolistas",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Equipos",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FutbolistaEquipo",
                columns: table => new
                {
                    FutbolistaId = table.Column<int>(type: "int", nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FutbolistaEquipo", x => new { x.FutbolistaId, x.EquipoId });
                    table.ForeignKey(
                        name: "FK_FutbolistaEquipo_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FutbolistaEquipo_Futbolistas_FutbolistaId",
                        column: x => x.FutbolistaId,
                        principalTable: "Futbolistas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FutbolistaEquipo_EquipoId",
                table: "FutbolistaEquipo",
                column: "EquipoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FutbolistaEquipo");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Futbolistas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Equipos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<int>(
                name: "FutbolistaId",
                table: "Equipos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_FutbolistaId",
                table: "Equipos",
                column: "FutbolistaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipos_Futbolistas_FutbolistaId",
                table: "Equipos",
                column: "FutbolistaId",
                principalTable: "Futbolistas",
                principalColumn: "Id");
        }
    }
}
