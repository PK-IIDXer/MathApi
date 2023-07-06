using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFormulaTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Formulas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Meaning = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formulas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormulaStrings",
                columns: table => new
                {
                    FormulaId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    SymbolId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaStrings", x => new { x.FormulaId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_FormulaStrings_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulaStrings_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormulasChain",
                columns: table => new
                {
                    FormulaId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FromFormulaStringSerialNo = table.Column<long>(type: "bigint", nullable: true),
                    ToFormulaStringSerialNo = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulasChain", x => new { x.FormulaId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_FormulasChain_FormulaStrings_FromFormulaStringFormulaId_From~",
                        columns: x => new { x.FormulaId, x.FromFormulaStringSerialNo },
                        principalTable: "FormulaStrings",
                        principalColumns: new[] { "FormulaId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_FormulasChain_FormulaStrings_ToFormulaStringFormulaId_ToForm~",
                        columns: x => new { x.FormulaId, x.ToFormulaStringSerialNo },
                        principalTable: "FormulaStrings",
                        principalColumns: new[] { "FormulaId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_FormulasChain_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FormulasChain_FromFormulaStringFormulaId_FromFormulaStringSe~",
                table: "FormulasChain",
                columns: new[] { "FormulaId", "FromFormulaStringSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_FormulasChain_ToFormulaStringFormulaId_ToFormulaStringSerial~",
                table: "FormulasChain",
                columns: new[] { "FormulaId", "ToFormulaStringSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStrings_SymbolId",
                table: "FormulaStrings",
                column: "SymbolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormulasChain");

            migrationBuilder.DropTable(
                name: "FormulaStrings");

            migrationBuilder.DropTable(
                name: "Formulas");
        }
    }
}
