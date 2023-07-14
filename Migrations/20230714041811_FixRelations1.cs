using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathApi.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptions_InferenceAssumptionDissolutionTypes_Inf~",
                table: "InferenceAssumptions");

            migrationBuilder.DropIndex(
                name: "IX_InferenceAssumptions_InferenceAssumptionDissolutionTypeId",
                table: "InferenceAssumptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptions_InferenceAssumptionDissolutionTypeId",
                table: "InferenceAssumptions",
                column: "InferenceAssumptionDissolutionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptions_InferenceAssumptionDissolutionTypes_Inf~",
                table: "InferenceAssumptions",
                column: "InferenceAssumptionDissolutionTypeId",
                principalTable: "InferenceAssumptionDissolutionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
