using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MathApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Axioms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remarks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Axioms", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "FormulaTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InferenceArgumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceArgumentTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InferenceAssumptionDissolutionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceAssumptionDissolutionTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Theorems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsProved = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theorems", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AxiomPropositions",
                columns: table => new
                {
                    AxiomId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FormulaId = table.Column<long>(type: "bigint", nullable: false),
                    Remarks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AxiomPropositions", x => new { x.AxiomId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_AxiomPropositions_Axioms_AxiomId",
                        column: x => x.AxiomId,
                        principalTable: "Axioms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AxiomPropositions_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SymbolTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormulaTypeId = table.Column<long>(type: "bigint", nullable: false),
                    IsQuantifier = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymbolTypes_FormulaTypes_FormulaTypeId",
                        column: x => x.FormulaTypeId,
                        principalTable: "FormulaTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inferences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAssumptionAdd = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsBasic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TheoremId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inferences_Theorems_TheoremId",
                        column: x => x.TheoremId,
                        principalTable: "Theorems",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Proofs",
                columns: table => new
                {
                    TheoremId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proofs", x => new { x.TheoremId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_Proofs_Theorems_TheoremId",
                        column: x => x.TheoremId,
                        principalTable: "Theorems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TheoremAssumptions",
                columns: table => new
                {
                    TheoremId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FormulaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheoremAssumptions", x => new { x.TheoremId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_TheoremAssumptions_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TheoremAssumptions_Theorems_TheoremId",
                        column: x => x.TheoremId,
                        principalTable: "Theorems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TheoremConclusions",
                columns: table => new
                {
                    TheoremId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FormulaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheoremConclusions", x => new { x.TheoremId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_TheoremConclusions_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TheoremConclusions_Theorems_TheoremId",
                        column: x => x.TheoremId,
                        principalTable: "Theorems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Character = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SymbolTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Arity = table.Column<int>(type: "int", nullable: true),
                    ArityFormulaTypeId = table.Column<long>(type: "bigint", nullable: true),
                    Meaning = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Symbols_FormulaTypes_ArityFormulaTypeId",
                        column: x => x.ArityFormulaTypeId,
                        principalTable: "FormulaTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Symbols_SymbolTypes_SymbolTypeId",
                        column: x => x.SymbolTypeId,
                        principalTable: "SymbolTypes",
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
                    InferenceAssumptionDissolutionTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceAssumptions", x => new { x.InferenceId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptions_InferenceAssumptionDissolutionTypes_Inf~",
                        column: x => x.InferenceAssumptionDissolutionTypeId,
                        principalTable: "InferenceAssumptionDissolutionTypes",
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

            migrationBuilder.CreateTable(
                name: "ProofInferences",
                columns: table => new
                {
                    TheoremId = table.Column<long>(type: "bigint", nullable: false),
                    ProofSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    ConclusionFormulaId = table.Column<long>(type: "bigint", nullable: false),
                    NextProofInferenceSerialNo = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofInferences", x => new { x.TheoremId, x.ProofSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_ProofInferences_Formulas_ConclusionFormulaId",
                        column: x => x.ConclusionFormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofInferences_Inferences_InferenceId",
                        column: x => x.InferenceId,
                        principalTable: "Inferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofInferences_ProofInferences_TheoremId_ProofSerialNo_Next~",
                        columns: x => new { x.TheoremId, x.ProofSerialNo, x.NextProofInferenceSerialNo },
                        principalTable: "ProofInferences",
                        principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_ProofInferences_Proofs_TheoremId_ProofSerialNo",
                        columns: x => new { x.TheoremId, x.ProofSerialNo },
                        principalTable: "Proofs",
                        principalColumns: new[] { "TheoremId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
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
                name: "InferenceArguments",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    InferenceArgumentTypeId = table.Column<int>(type: "int", nullable: false),
                    VariableSymbolId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceArguments", x => new { x.InferenceId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceArguments_InferenceArgumentTypes_InferenceArgumentT~",
                        column: x => x.InferenceArgumentTypeId,
                        principalTable: "InferenceArgumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceArguments_Inferences_InferenceId",
                        column: x => x.InferenceId,
                        principalTable: "Inferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceArguments_Symbols_VariableSymbolId",
                        column: x => x.VariableSymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProofAssumptions",
                columns: table => new
                {
                    TheoremId = table.Column<long>(type: "bigint", nullable: false),
                    ProofSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FormulaId = table.Column<long>(type: "bigint", nullable: false),
                    AddedProofInferenceSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    LastUsedProofInferenceSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    DissolutedProofInferenceSerialNo = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofAssumptions", x => new { x.TheoremId, x.ProofSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_ProofAssumptions_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Add~",
                        columns: x => new { x.TheoremId, x.ProofSerialNo, x.AddedProofInferenceSerialNo },
                        principalTable: "ProofInferences",
                        principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Dis~",
                        columns: x => new { x.TheoremId, x.ProofSerialNo, x.DissolutedProofInferenceSerialNo },
                        principalTable: "ProofInferences",
                        principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Las~",
                        columns: x => new { x.TheoremId, x.ProofSerialNo, x.LastUsedProofInferenceSerialNo },
                        principalTable: "ProofInferences",
                        principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofAssumptions_Proofs_TheoremId_ProofSerialNo",
                        columns: x => new { x.TheoremId, x.ProofSerialNo },
                        principalTable: "Proofs",
                        principalColumns: new[] { "TheoremId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProofInferenceArguments",
                columns: table => new
                {
                    TheoremId = table.Column<long>(type: "bigint", nullable: false),
                    ProofSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    ProofInferenceSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FormulaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofInferenceArguments", x => new { x.TheoremId, x.ProofSerialNo, x.ProofInferenceSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_ProofInferenceArguments_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofInferenceArguments_ProofInferences_TheoremId_ProofSeria~",
                        columns: x => new { x.TheoremId, x.ProofSerialNo, x.ProofInferenceSerialNo },
                        principalTable: "ProofInferences",
                        principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofInferenceArguments_TheoremAssumptions_TheoremId_SerialNo",
                        columns: x => new { x.TheoremId, x.SerialNo },
                        principalTable: "TheoremAssumptions",
                        principalColumns: new[] { "TheoremId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormulaChains",
                columns: table => new
                {
                    FormulaId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FromFormulaStringSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    ToFormulaStringSerialNo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaChains", x => new { x.FormulaId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_FormulaChains_FormulaStrings_FormulaId_FromFormulaStringSeri~",
                        columns: x => new { x.FormulaId, x.FromFormulaStringSerialNo },
                        principalTable: "FormulaStrings",
                        principalColumns: new[] { "FormulaId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulaChains_FormulaStrings_FormulaId_ToFormulaStringSerial~",
                        columns: x => new { x.FormulaId, x.ToFormulaStringSerialNo },
                        principalTable: "FormulaStrings",
                        principalColumns: new[] { "FormulaId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulaChains_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InferenceArgumentConstraints",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    InferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    ConstraintDestinationInferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: false),
                    IsConstraintPredissolvedAssumption = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceArgumentConstraints", x => new { x.InferenceId, x.InferenceArgumentSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceArgumentConstraints_InferenceArguments_InferenceId_~",
                        columns: x => new { x.InferenceId, x.ConstraintDestinationInferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceArgumentConstraints_InferenceArguments_InferenceId~1",
                        columns: x => new { x.InferenceId, x.InferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    InferenceAssumptionSerialNo = table.Column<int>(type: "int", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    SymbolId = table.Column<long>(type: "bigint", nullable: true),
                    BoundInferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: true),
                    InferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: true),
                    SubstitutionInferenceArgumentFromSerialNo = table.Column<int>(type: "int", nullable: true),
                    SubstitutionInferenceArgumentToSerialNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceAssumptionDissolutableAssumptionFormulas", x => new { x.InferenceId, x.InferenceAssumptionSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_InferenceA~",
                        columns: x => new { x.InferenceId, x.BoundInferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~1",
                        columns: x => new { x.InferenceId, x.InferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~2",
                        columns: x => new { x.InferenceId, x.InferenceAssumptionSerialNo },
                        principalTable: "InferenceAssumptions",
                        principalColumns: new[] { "InferenceId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~3",
                        columns: x => new { x.InferenceId, x.SubstitutionInferenceArgumentFromSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~4",
                        columns: x => new { x.InferenceId, x.SubstitutionInferenceArgumentToSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Symbols_Sy~",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InferenceAssumptionFormulas",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    InferenceAssumptionSerialNo = table.Column<int>(type: "int", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    SymbolId = table.Column<long>(type: "bigint", nullable: true),
                    BoundInferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: true),
                    InferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: true),
                    SubstitutionInferenceArgumentFromSerialNo = table.Column<int>(type: "int", nullable: true),
                    SubstitutionInferenceArgumentToSerialNo = table.Column<int>(type: "int", nullable: true),
                    FormulaId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceAssumptionFormulas", x => new { x.InferenceId, x.InferenceAssumptionSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionFormulas_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_B~",
                        columns: x => new { x.InferenceId, x.BoundInferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_I~",
                        columns: x => new { x.InferenceId, x.InferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_S~",
                        columns: x => new { x.InferenceId, x.SubstitutionInferenceArgumentFromSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_~1",
                        columns: x => new { x.InferenceId, x.SubstitutionInferenceArgumentToSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionFormulas_InferenceAssumptions_InferenceId~",
                        columns: x => new { x.InferenceId, x.InferenceAssumptionSerialNo },
                        principalTable: "InferenceAssumptions",
                        principalColumns: new[] { "InferenceId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionFormulas_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InferenceConclusionFormulas",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    SymbolId = table.Column<long>(type: "bigint", nullable: true),
                    BoundInferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: true),
                    InferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: true),
                    SubstitutionInferenceArgumentFromSerialNo = table.Column<int>(type: "int", nullable: true),
                    SubstitutionInferenceArgumentToSerialNo = table.Column<int>(type: "int", nullable: true),
                    FormulaId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceConclusionFormulas", x => new { x.InferenceId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceConclusionFormulas_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InferenceConclusionFormulas_InferenceArguments_InferenceId_B~",
                        columns: x => new { x.InferenceId, x.BoundInferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceConclusionFormulas_InferenceArguments_InferenceId_I~",
                        columns: x => new { x.InferenceId, x.InferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceConclusionFormulas_InferenceArguments_InferenceId_S~",
                        columns: x => new { x.InferenceId, x.SubstitutionInferenceArgumentFromSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceConclusionFormulas_InferenceArguments_InferenceId_~1",
                        columns: x => new { x.InferenceId, x.SubstitutionInferenceArgumentToSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceConclusionFormulas_Inferences_InferenceId",
                        column: x => x.InferenceId,
                        principalTable: "Inferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceConclusionFormulas_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "FormulaTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Term" },
                    { 2L, "Proposition" }
                });

            migrationBuilder.InsertData(
                table: "InferenceArgumentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "term" },
                    { 2, "proposition" },
                    { 3, "free variable" }
                });

            migrationBuilder.InsertData(
                table: "InferenceAssumptionDissolutionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "none" },
                    { 2, "required" },
                    { 3, "necessary" }
                });

            migrationBuilder.InsertData(
                table: "SymbolTypes",
                columns: new[] { "Id", "FormulaTypeId", "IsQuantifier", "Name" },
                values: new object[,]
                {
                    { 1L, 1L, false, "free variable" },
                    { 2L, 1L, false, "bound variable" },
                    { 3L, 2L, false, "proposition variable" },
                    { 4L, 1L, false, "function" },
                    { 5L, 2L, false, "predicate" },
                    { 6L, 2L, false, "logic" },
                    { 7L, 1L, true, "term quantifier" },
                    { 8L, 2L, true, "proposition quantifier" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AxiomPropositions_FormulaId",
                table: "AxiomPropositions",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaChains_FormulaId_FromFormulaStringSerialNo",
                table: "FormulaChains",
                columns: new[] { "FormulaId", "FromFormulaStringSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormulaChains_FormulaId_ToFormulaStringSerialNo",
                table: "FormulaChains",
                columns: new[] { "FormulaId", "ToFormulaStringSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStrings_SymbolId",
                table: "FormulaStrings",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceArgumentConstraints_InferenceId_ConstraintDestinati~",
                table: "InferenceArgumentConstraints",
                columns: new[] { "InferenceId", "ConstraintDestinationInferenceArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceArguments_InferenceArgumentTypeId",
                table: "InferenceArguments",
                column: "InferenceArgumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceArguments_VariableSymbolId",
                table: "InferenceArguments",
                column: "VariableSymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormulas_Inference~1",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormulas_Inference~2",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormulas_Inference~3",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormulas_InferenceI~",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormulas_SymbolId",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_FormulaId",
                table: "InferenceAssumptionFormulas",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_InferenceId_BoundInferenceArgume~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_InferenceId_InferenceArgumentSer~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_InferenceId_SubstitutionInferen~1",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_InferenceId_SubstitutionInferenc~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_SymbolId",
                table: "InferenceAssumptionFormulas",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptions_InferenceAssumptionDissolutionTypeId",
                table: "InferenceAssumptions",
                column: "InferenceAssumptionDissolutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_FormulaId",
                table: "InferenceConclusionFormulas",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_InferenceId_BoundInferenceArgume~",
                table: "InferenceConclusionFormulas",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_InferenceId_InferenceArgumentSer~",
                table: "InferenceConclusionFormulas",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_InferenceId_SubstitutionInferen~1",
                table: "InferenceConclusionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_InferenceId_SubstitutionInferenc~",
                table: "InferenceConclusionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_SymbolId",
                table: "InferenceConclusionFormulas",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Inferences_TheoremId",
                table: "Inferences",
                column: "TheoremId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProofAssumptions_FormulaId",
                table: "ProofAssumptions",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofAssumptions_TheoremId_ProofSerialNo_AddedProofInference~",
                table: "ProofAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "AddedProofInferenceSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProofAssumptions_TheoremId_ProofSerialNo_DissolutedProofInfe~",
                table: "ProofAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "DissolutedProofInferenceSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProofAssumptions_TheoremId_ProofSerialNo_LastUsedProofInfere~",
                table: "ProofAssumptions",
                columns: new[] { "TheoremId", "ProofSerialNo", "LastUsedProofInferenceSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferenceArguments_FormulaId",
                table: "ProofInferenceArguments",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferenceArguments_TheoremId_SerialNo",
                table: "ProofInferenceArguments",
                columns: new[] { "TheoremId", "SerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferences_ConclusionFormulaId",
                table: "ProofInferences",
                column: "ConclusionFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferences_InferenceId",
                table: "ProofInferences",
                column: "InferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferences_TheoremId_ProofSerialNo_NextProofInferenceSe~",
                table: "ProofInferences",
                columns: new[] { "TheoremId", "ProofSerialNo", "NextProofInferenceSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_ArityFormulaTypeId",
                table: "Symbols",
                column: "ArityFormulaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Character_SymbolTypeId",
                table: "Symbols",
                columns: new[] { "Character", "SymbolTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_SymbolTypeId",
                table: "Symbols",
                column: "SymbolTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SymbolTypes_FormulaTypeId",
                table: "SymbolTypes",
                column: "FormulaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TheoremAssumptions_FormulaId",
                table: "TheoremAssumptions",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_TheoremConclusions_FormulaId",
                table: "TheoremConclusions",
                column: "FormulaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AxiomPropositions");

            migrationBuilder.DropTable(
                name: "FormulaChains");

            migrationBuilder.DropTable(
                name: "InferenceArgumentConstraints");

            migrationBuilder.DropTable(
                name: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropTable(
                name: "InferenceAssumptionFormulas");

            migrationBuilder.DropTable(
                name: "InferenceConclusionFormulas");

            migrationBuilder.DropTable(
                name: "ProofAssumptions");

            migrationBuilder.DropTable(
                name: "ProofInferenceArguments");

            migrationBuilder.DropTable(
                name: "TheoremConclusions");

            migrationBuilder.DropTable(
                name: "Axioms");

            migrationBuilder.DropTable(
                name: "FormulaStrings");

            migrationBuilder.DropTable(
                name: "InferenceAssumptions");

            migrationBuilder.DropTable(
                name: "InferenceArguments");

            migrationBuilder.DropTable(
                name: "ProofInferences");

            migrationBuilder.DropTable(
                name: "TheoremAssumptions");

            migrationBuilder.DropTable(
                name: "InferenceAssumptionDissolutionTypes");

            migrationBuilder.DropTable(
                name: "InferenceArgumentTypes");

            migrationBuilder.DropTable(
                name: "Symbols");

            migrationBuilder.DropTable(
                name: "Inferences");

            migrationBuilder.DropTable(
                name: "Proofs");

            migrationBuilder.DropTable(
                name: "Formulas");

            migrationBuilder.DropTable(
                name: "SymbolTypes");

            migrationBuilder.DropTable(
                name: "Theorems");

            migrationBuilder.DropTable(
                name: "FormulaTypes");
        }
    }
}
