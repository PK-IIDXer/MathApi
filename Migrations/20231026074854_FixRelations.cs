using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathApi.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InferenceFormulaStructArgumentMappings_InferenceAssumptionDi~",
                table: "InferenceFormulaStructArgumentMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceFormulaStructArgumentMappings_InferenceAssumptions_~",
                table: "InferenceFormulaStructArgumentMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceFormulaStructArgumentMappings_InferenceConclusions_~",
                table: "InferenceFormulaStructArgumentMappings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_InferenceFormulaStructArgumentMappings_InferenceAssumptionDi~",
                table: "InferenceFormulaStructArgumentMappings",
                columns: new[] { "InferenceId", "SerialNo" },
                principalTable: "InferenceAssumptionDissolutableAssumptions",
                principalColumns: new[] { "InferenceId", "FormulaStructArgumentMappingSerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceFormulaStructArgumentMappings_InferenceAssumptions_~",
                table: "InferenceFormulaStructArgumentMappings",
                columns: new[] { "InferenceId", "SerialNo" },
                principalTable: "InferenceAssumptions",
                principalColumns: new[] { "InferenceId", "FormulaStructArgumentMappingSerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceFormulaStructArgumentMappings_InferenceConclusions_~",
                table: "InferenceFormulaStructArgumentMappings",
                columns: new[] { "InferenceId", "SerialNo" },
                principalTable: "InferenceConclusions",
                principalColumns: new[] { "InferenceId", "FormulaStructArgumentMappingSerialNo" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
