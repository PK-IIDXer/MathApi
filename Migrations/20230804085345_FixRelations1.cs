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
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceAr~",
                table: "InferenceAssumptionDissolutableAssumptionFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceAs~",
                table: "InferenceAssumptionDissolutableAssumptionFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceA~1",
                table: "InferenceAssumptionDissolutableAssumptionFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceA~2",
                table: "InferenceAssumptionDissolutableAssumptionFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceA~3",
                table: "InferenceAssumptionDissolutableAssumptionFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_Symbols_Sym~",
                table: "InferenceAssumptionDissolutableAssumptionFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferenceArguments_AxiomPropositions_AxiomPropositionAx~",
                table: "ProofInferenceArguments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferenceArguments_TheoremAssumption_TheoremId_SerialNo",
                table: "ProofInferenceArguments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferenceAssumptions_Formulas_FormulaId",
                table: "ProofInferenceAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferenceAssumptions_ProofInferences_TheoremId_ProofSer~",
                table: "ProofInferenceAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferenceAssumptions_ProofInferences_TheoremId_ProofSe~1",
                table: "ProofInferenceAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferenceAssumptions_ProofInferences_TheoremId_ProofSe~2",
                table: "ProofInferenceAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferenceAssumptions_Proofs_TheoremId_ProofSerialNo",
                table: "ProofInferenceAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TheoremAssumption_Formulas_FormulaId",
                table: "TheoremAssumption");

            migrationBuilder.DropForeignKey(
                name: "FK_TheoremAssumption_Theorems_TheoremId",
                table: "TheoremAssumption");

            migrationBuilder.DropIndex(
                name: "IX_ProofInferenceArguments_AxiomPropositionAxiomId_AxiomProposi~",
                table: "ProofInferenceArguments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TheoremAssumption",
                table: "TheoremAssumption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProofInferenceAssumptions",
                table: "ProofInferenceAssumptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InferenceAssumptionDissolutableAssumptionFormula",
                table: "InferenceAssumptionDissolutableAssumptionFormula");

            migrationBuilder.DropColumn(
                name: "AxiomPropositionAxiomId",
                table: "ProofInferenceArguments");

            migrationBuilder.DropColumn(
                name: "AxiomPropositionSerialNo",
                table: "ProofInferenceArguments");

            migrationBuilder.RenameTable(
                name: "TheoremAssumption",
                newName: "TheoremAssumptions");

            migrationBuilder.RenameTable(
                name: "ProofInferenceAssumptions",
                newName: "ProofAssumptions");

            migrationBuilder.RenameTable(
                name: "InferenceAssumptionDissolutableAssumptionFormula",
                newName: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.RenameIndex(
                name: "IX_TheoremAssumption_FormulaId",
                table: "TheoremAssumptions",
                newName: "IX_TheoremAssumptions_FormulaId");

            migrationBuilder.RenameIndex(
                name: "IX_ProofInferenceAssumptions_TheoremId_ProofSerialNo_LastUsedPr~",
                table: "ProofAssumptions",
                newName: "IX_ProofAssumptions_TheoremId_ProofSerialNo_LastUsedProofInfere~");

            migrationBuilder.RenameIndex(
                name: "IX_ProofInferenceAssumptions_TheoremId_ProofSerialNo_Dissoluted~",
                table: "ProofAssumptions",
                newName: "IX_ProofAssumptions_TheoremId_ProofSerialNo_DissolutedProofInfe~");

            migrationBuilder.RenameIndex(
                name: "IX_ProofInferenceAssumptions_TheoremId_ProofSerialNo_AddedProof~",
                table: "ProofAssumptions",
                newName: "IX_ProofAssumptions_TheoremId_ProofSerialNo_AddedProofInference~");

            migrationBuilder.RenameIndex(
                name: "IX_ProofInferenceAssumptions_FormulaId",
                table: "ProofAssumptions",
                newName: "IX_ProofAssumptions_FormulaId");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormula_SymbolId",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                newName: "IX_InferenceAssumptionDissolutableAssumptionFormulas_SymbolId");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceId~",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                newName: "IX_InferenceAssumptionDissolutableAssumptionFormulas_InferenceI~");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceI~3",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                newName: "IX_InferenceAssumptionDissolutableAssumptionFormulas_Inference~3");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceI~2",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                newName: "IX_InferenceAssumptionDissolutableAssumptionFormulas_Inference~2");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceI~1",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                newName: "IX_InferenceAssumptionDissolutableAssumptionFormulas_Inference~1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TheoremAssumptions",
                table: "TheoremAssumptions",
                columns: new[] { "TheoremId", "SerialNo" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProofAssumptions",
                table: "ProofAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_InferenceAssumptionDissolutableAssumptionFormulas",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "InferenceAssumptionSerialNo", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_InferenceA~",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~1",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~2",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "InferenceAssumptionSerialNo" },
                principalTable: "InferenceAssumptions",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~3",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~4",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Symbols_Sy~",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofAssumptions_Formulas_FormulaId",
                table: "ProofAssumptions",
                column: "FormulaId",
                principalTable: "Formulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Add~",
                table: "ProofAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "AddedProofInferenceSerialNo" },
                principalTable: "ProofInferences",
                principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Dis~",
                table: "ProofAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "DissolutedProofInferenceSerialNo" },
                principalTable: "ProofInferences",
                principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Las~",
                table: "ProofAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "LastUsedProofInferenceSerialNo" },
                principalTable: "ProofInferences",
                principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofAssumptions_Proofs_TheoremId_ProofSerialNo",
                table: "ProofAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo" },
                principalTable: "Proofs",
                principalColumns: new[] { "TheoremId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofInferenceArguments_TheoremAssumptions_TheoremId_SerialNo",
                table: "ProofInferenceArguments",
                columns: new[] { "TheoremId", "SerialNo" },
                principalTable: "TheoremAssumptions",
                principalColumns: new[] { "TheoremId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TheoremAssumptions_Formulas_FormulaId",
                table: "TheoremAssumptions",
                column: "FormulaId",
                principalTable: "Formulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TheoremAssumptions_Theorems_TheoremId",
                table: "TheoremAssumptions",
                column: "TheoremId",
                principalTable: "Theorems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_InferenceA~",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~1",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~2",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~3",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~4",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Symbols_Sy~",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofAssumptions_Formulas_FormulaId",
                table: "ProofAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Add~",
                table: "ProofAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Dis~",
                table: "ProofAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Las~",
                table: "ProofAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofAssumptions_Proofs_TheoremId_ProofSerialNo",
                table: "ProofAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferenceArguments_TheoremAssumptions_TheoremId_SerialNo",
                table: "ProofInferenceArguments");

            migrationBuilder.DropForeignKey(
                name: "FK_TheoremAssumptions_Formulas_FormulaId",
                table: "TheoremAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TheoremAssumptions_Theorems_TheoremId",
                table: "TheoremAssumptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TheoremAssumptions",
                table: "TheoremAssumptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProofAssumptions",
                table: "ProofAssumptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InferenceAssumptionDissolutableAssumptionFormulas",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.RenameTable(
                name: "TheoremAssumptions",
                newName: "TheoremAssumption");

            migrationBuilder.RenameTable(
                name: "ProofAssumptions",
                newName: "ProofInferenceAssumptions");

            migrationBuilder.RenameTable(
                name: "InferenceAssumptionDissolutableAssumptionFormulas",
                newName: "InferenceAssumptionDissolutableAssumptionFormula");

            migrationBuilder.RenameIndex(
                name: "IX_TheoremAssumptions_FormulaId",
                table: "TheoremAssumption",
                newName: "IX_TheoremAssumption_FormulaId");

            migrationBuilder.RenameIndex(
                name: "IX_ProofAssumptions_TheoremId_ProofSerialNo_LastUsedProofInfere~",
                table: "ProofInferenceAssumptions",
                newName: "IX_ProofInferenceAssumptions_TheoremId_ProofSerialNo_LastUsedPr~");

            migrationBuilder.RenameIndex(
                name: "IX_ProofAssumptions_TheoremId_ProofSerialNo_DissolutedProofInfe~",
                table: "ProofInferenceAssumptions",
                newName: "IX_ProofInferenceAssumptions_TheoremId_ProofSerialNo_Dissoluted~");

            migrationBuilder.RenameIndex(
                name: "IX_ProofAssumptions_TheoremId_ProofSerialNo_AddedProofInference~",
                table: "ProofInferenceAssumptions",
                newName: "IX_ProofInferenceAssumptions_TheoremId_ProofSerialNo_AddedProof~");

            migrationBuilder.RenameIndex(
                name: "IX_ProofAssumptions_FormulaId",
                table: "ProofInferenceAssumptions",
                newName: "IX_ProofInferenceAssumptions_FormulaId");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormulas_SymbolId",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                newName: "IX_InferenceAssumptionDissolutableAssumptionFormula_SymbolId");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormulas_InferenceI~",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                newName: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceId~");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormulas_Inference~3",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                newName: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceI~3");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormulas_Inference~2",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                newName: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceI~2");

            migrationBuilder.RenameIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormulas_Inference~1",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                newName: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceI~1");

            migrationBuilder.AddColumn<long>(
                name: "AxiomPropositionAxiomId",
                table: "ProofInferenceArguments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AxiomPropositionSerialNo",
                table: "ProofInferenceArguments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TheoremAssumption",
                table: "TheoremAssumption",
                columns: new[] { "TheoremId", "SerialNo" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProofInferenceAssumptions",
                table: "ProofInferenceAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_InferenceAssumptionDissolutableAssumptionFormula",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                columns: new[] { "InferenceId", "InferenceAssumptionSerialNo", "SerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferenceArguments_AxiomPropositionAxiomId_AxiomProposi~",
                table: "ProofInferenceArguments",
                columns: new[] { "AxiomPropositionAxiomId", "AxiomPropositionSerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceAr~",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceAs~",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                columns: new[] { "InferenceId", "InferenceAssumptionSerialNo" },
                principalTable: "InferenceAssumptions",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceA~1",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceA~2",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceA~3",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormula_Symbols_Sym~",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofInferenceArguments_AxiomPropositions_AxiomPropositionAx~",
                table: "ProofInferenceArguments",
                columns: new[] { "AxiomPropositionAxiomId", "AxiomPropositionSerialNo" },
                principalTable: "AxiomPropositions",
                principalColumns: new[] { "AxiomId", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProofInferenceArguments_TheoremAssumption_TheoremId_SerialNo",
                table: "ProofInferenceArguments",
                columns: new[] { "TheoremId", "SerialNo" },
                principalTable: "TheoremAssumption",
                principalColumns: new[] { "TheoremId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofInferenceAssumptions_Formulas_FormulaId",
                table: "ProofInferenceAssumptions",
                column: "FormulaId",
                principalTable: "Formulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofInferenceAssumptions_ProofInferences_TheoremId_ProofSer~",
                table: "ProofInferenceAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "AddedProofInferenceSerialNo" },
                principalTable: "ProofInferences",
                principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofInferenceAssumptions_ProofInferences_TheoremId_ProofSe~1",
                table: "ProofInferenceAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "DissolutedProofInferenceSerialNo" },
                principalTable: "ProofInferences",
                principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProofInferenceAssumptions_ProofInferences_TheoremId_ProofSe~2",
                table: "ProofInferenceAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "LastUsedProofInferenceSerialNo" },
                principalTable: "ProofInferences",
                principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProofInferenceAssumptions_Proofs_TheoremId_ProofSerialNo",
                table: "ProofInferenceAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo" },
                principalTable: "Proofs",
                principalColumns: new[] { "TheoremId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TheoremAssumption_Formulas_FormulaId",
                table: "TheoremAssumption",
                column: "FormulaId",
                principalTable: "Formulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TheoremAssumption_Theorems_TheoremId",
                table: "TheoremAssumption",
                column: "TheoremId",
                principalTable: "Theorems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
