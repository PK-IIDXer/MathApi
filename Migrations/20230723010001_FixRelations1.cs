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
                name: "FK_InferenceArguments_Symbols_PropositionVariableSymbolId",
                table: "InferenceArguments");

            migrationBuilder.RenameColumn(
                name: "PropositionVariableSymbolId",
                table: "InferenceArguments",
                newName: "VariableSymbolId");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceArguments_PropositionVariableSymbolId",
                table: "InferenceArguments",
                newName: "IX_InferenceArguments_VariableSymbolId");

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceArguments_Symbols_VariableSymbolId",
                table: "InferenceArguments",
                column: "VariableSymbolId",
                principalTable: "Symbols",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InferenceArguments_Symbols_VariableSymbolId",
                table: "InferenceArguments");

            migrationBuilder.RenameColumn(
                name: "VariableSymbolId",
                table: "InferenceArguments",
                newName: "PropositionVariableSymbolId");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceArguments_VariableSymbolId",
                table: "InferenceArguments",
                newName: "IX_InferenceArguments_PropositionVariableSymbolId");

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceArguments_Symbols_PropositionVariableSymbolId",
                table: "InferenceArguments",
                column: "PropositionVariableSymbolId",
                principalTable: "Symbols",
                principalColumn: "Id");
        }
    }
}
