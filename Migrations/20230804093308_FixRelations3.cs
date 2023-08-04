using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathApi.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InferenceConclusionFormulas_Symbols_SymbolId",
                table: "InferenceConclusionFormulas");

            migrationBuilder.AlterColumn<long>(
                name: "SymbolId",
                table: "InferenceConclusionFormulas",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceConclusionFormulas_Symbols_SymbolId",
                table: "InferenceConclusionFormulas",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InferenceConclusionFormulas_Symbols_SymbolId",
                table: "InferenceConclusionFormulas");

            migrationBuilder.AlterColumn<long>(
                name: "SymbolId",
                table: "InferenceConclusionFormulas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceConclusionFormulas_Symbols_SymbolId",
                table: "InferenceConclusionFormulas",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
