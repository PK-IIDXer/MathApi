using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathApi.Migrations
{
    /// <inheritdoc />
    public partial class AddInferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inferences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConclusionFormulaId = table.Column<long>(type: "bigint", nullable: false),
                    HasBoundingVariable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasAssignmentOperation = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inferences_Formulas_ConclusionFormulaId",
                        column: x => x.ConclusionFormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InferenceAssumptions",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    FormulaId = table.Column<long>(type: "bigint", nullable: false),
                    IsDissolutionAssumption = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DissolutionAssumptionFormulaId = table.Column<long>(type: "bigint", nullable: false),
                    IsBoundVariableRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasAssignmentOperation = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceAssumptions", x => new { x.InferenceId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptions_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceAssumptions_Inferences_InferenceId",
                        column: x => x.InferenceId,
                        principalTable: "Inferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptions_FormulaId",
                table: "InferenceAssumptions",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Inferences_ConclusionFormulaId",
                table: "Inferences",
                column: "ConclusionFormulaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InferenceAssumptions");

            migrationBuilder.DropTable(
                name: "Inferences");
        }
    }
}
