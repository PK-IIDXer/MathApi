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
                name: "Inferences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAssumptionAdd = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inferences", x => x.Id);
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
                    FormulaTypeId = table.Column<long>(type: "bigint", nullable: false)
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
                name: "InferenceArguments",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    InferenceArgumentTypeId = table.Column<int>(type: "int", nullable: false)
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
                name: "ProofHeads",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    TheoremId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofHeads", x => new { x.Id, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_ProofHeads_Theorems_TheoremId",
                        column: x => x.TheoremId,
                        principalTable: "Theorems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TheoremAssumption",
                columns: table => new
                {
                    TheoremId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FormulaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheoremAssumption", x => new { x.TheoremId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_TheoremAssumption_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TheoremAssumption_Theorems_TheoremId",
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
                name: "Proofs",
                columns: table => new
                {
                    ProofHeadId = table.Column<long>(type: "bigint", nullable: false),
                    ProofHeadSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    NextProofSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    ConclusionFormulaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proofs", x => new { x.ProofHeadId, x.ProofHeadSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_Proofs_Formulas_ConclusionFormulaId",
                        column: x => x.ConclusionFormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Proofs_Inferences_InferenceId",
                        column: x => x.InferenceId,
                        principalTable: "Inferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Proofs_ProofHeads_ProofHeadId_ProofHeadSerialNo",
                        columns: x => new { x.ProofHeadId, x.ProofHeadSerialNo },
                        principalTable: "ProofHeads",
                        principalColumns: new[] { "Id", "SerialNo" },
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
                name: "InferenceAssumptionDissolutableAssumptionFormula",
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
                    table.PrimaryKey("PK_InferenceAssumptionDissolutableAssumptionFormula", x => new { x.InferenceId, x.InferenceAssumptionSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceAr~",
                        columns: x => new { x.InferenceId, x.BoundInferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceAs~",
                        columns: x => new { x.InferenceId, x.InferenceAssumptionSerialNo },
                        principalTable: "InferenceAssumptions",
                        principalColumns: new[] { "InferenceId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceA~1",
                        columns: x => new { x.InferenceId, x.InferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceA~2",
                        columns: x => new { x.InferenceId, x.SubstitutionInferenceArgumentFromSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormula_InferenceA~3",
                        columns: x => new { x.InferenceId, x.SubstitutionInferenceArgumentToSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptionFormula_Symbols_Sym~",
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
                    SubstitutionInferenceArgumentToSerialNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceAssumptionFormulas", x => new { x.InferenceId, x.InferenceAssumptionSerialNo, x.SerialNo });
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
                    SubstitutionInferenceArgumentToSerialNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceConclusionFormulas", x => new { x.InferenceId, x.SerialNo });
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

            migrationBuilder.CreateTable(
                name: "ProofArguments",
                columns: table => new
                {
                    ProofHeadId = table.Column<long>(type: "bigint", nullable: false),
                    ProofHeadSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    ProofSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    AxiomId = table.Column<long>(type: "bigint", nullable: true),
                    AxiomPropositionSerialNo = table.Column<long>(type: "bigint", nullable: true),
                    TheoremAssumptionTheoremId = table.Column<long>(type: "bigint", nullable: true),
                    TheoremAssumptionSerialNo = table.Column<long>(type: "bigint", nullable: true),
                    FormulaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofArguments", x => new { x.ProofHeadId, x.ProofHeadSerialNo, x.ProofSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_ProofArguments_AxiomPropositions_AxiomId_AxiomPropositionSer~",
                        columns: x => new { x.AxiomId, x.AxiomPropositionSerialNo },
                        principalTable: "AxiomPropositions",
                        principalColumns: new[] { "AxiomId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_ProofArguments_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofArguments_Proofs_ProofHeadId_ProofHeadSerialNo_SerialNo",
                        columns: x => new { x.ProofHeadId, x.ProofHeadSerialNo, x.SerialNo },
                        principalTable: "Proofs",
                        principalColumns: new[] { "ProofHeadId", "ProofHeadSerialNo", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofArguments_TheoremAssumption_TheoremAssumptionTheoremId_~",
                        columns: x => new { x.TheoremAssumptionTheoremId, x.TheoremAssumptionSerialNo },
                        principalTable: "TheoremAssumption",
                        principalColumns: new[] { "TheoremId", "SerialNo" });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProofAssumptions",
                columns: table => new
                {
                    ProofHeadId = table.Column<long>(type: "bigint", nullable: false),
                    ProofHeadSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    ProofSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FormulaId = table.Column<long>(type: "bigint", nullable: false),
                    DissolutedProofSerialNo = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofAssumptions", x => new { x.ProofHeadId, x.ProofHeadSerialNo, x.ProofSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_ProofAssumptions_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofAssumptions_Proofs_ProofHeadId_ProofHeadSerialNo_Serial~",
                        columns: x => new { x.ProofHeadId, x.ProofHeadSerialNo, x.SerialNo },
                        principalTable: "Proofs",
                        principalColumns: new[] { "ProofHeadId", "ProofHeadSerialNo", "SerialNo" },
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
                columns: new[] { "Id", "FormulaTypeId", "Name" },
                values: new object[,]
                {
                    { 1L, 1L, "free variable" },
                    { 2L, 1L, "bound variable" },
                    { 3L, 2L, "proposition variable" },
                    { 4L, 1L, "constant" },
                    { 5L, 1L, "function" },
                    { 6L, 2L, "predicate" },
                    { 7L, 2L, "logic" },
                    { 8L, 1L, "term quantifier" },
                    { 9L, 2L, "proposition quantifier" }
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
                name: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceI~1",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceI~2",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceI~3",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormula_InferenceId~",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptionFormula_SymbolId",
                table: "InferenceAssumptionDissolutableAssumptionFormula",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_InferenceId_BoundInferenceArgume~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_InferenceId_InferenceArgumentSer~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_InferenceId_SubstitutionInferen~1",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_InferenceId_SubstitutionInferenc~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionFormulas_SymbolId",
                table: "InferenceAssumptionFormulas",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptions_InferenceAssumptionDissolutionTypeId",
                table: "InferenceAssumptions",
                column: "InferenceAssumptionDissolutionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_InferenceId_BoundInferenceArgume~",
                table: "InferenceConclusionFormulas",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_InferenceId_InferenceArgumentSer~",
                table: "InferenceConclusionFormulas",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_InferenceId_SubstitutionInferen~1",
                table: "InferenceConclusionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_InferenceId_SubstitutionInferenc~",
                table: "InferenceConclusionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusionFormulas_SymbolId",
                table: "InferenceConclusionFormulas",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofArguments_AxiomId_AxiomPropositionSerialNo",
                table: "ProofArguments",
                columns: new[] { "AxiomId", "AxiomPropositionSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_ProofArguments_FormulaId",
                table: "ProofArguments",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofArguments_ProofHeadId_ProofHeadSerialNo_SerialNo",
                table: "ProofArguments",
                columns: new[] { "ProofHeadId", "ProofHeadSerialNo", "SerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_ProofArguments_TheoremAssumptionTheoremId_TheoremAssumptionS~",
                table: "ProofArguments",
                columns: new[] { "TheoremAssumptionTheoremId", "TheoremAssumptionSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_ProofAssumptions_FormulaId",
                table: "ProofAssumptions",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofAssumptions_ProofHeadId_ProofHeadSerialNo_SerialNo",
                table: "ProofAssumptions",
                columns: new[] { "ProofHeadId", "ProofHeadSerialNo", "SerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_ProofHeads_TheoremId",
                table: "ProofHeads",
                column: "TheoremId");

            migrationBuilder.CreateIndex(
                name: "IX_Proofs_ConclusionFormulaId",
                table: "Proofs",
                column: "ConclusionFormulaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proofs_InferenceId",
                table: "Proofs",
                column: "InferenceId",
                unique: true);

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
                name: "IX_TheoremAssumption_FormulaId",
                table: "TheoremAssumption",
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
                name: "FormulaChains");

            migrationBuilder.DropTable(
                name: "InferenceArgumentConstraints");

            migrationBuilder.DropTable(
                name: "InferenceAssumptionDissolutableAssumptionFormula");

            migrationBuilder.DropTable(
                name: "InferenceAssumptionFormulas");

            migrationBuilder.DropTable(
                name: "InferenceConclusionFormulas");

            migrationBuilder.DropTable(
                name: "ProofArguments");

            migrationBuilder.DropTable(
                name: "ProofAssumptions");

            migrationBuilder.DropTable(
                name: "TheoremConclusions");

            migrationBuilder.DropTable(
                name: "FormulaStrings");

            migrationBuilder.DropTable(
                name: "InferenceAssumptions");

            migrationBuilder.DropTable(
                name: "InferenceArguments");

            migrationBuilder.DropTable(
                name: "AxiomPropositions");

            migrationBuilder.DropTable(
                name: "TheoremAssumption");

            migrationBuilder.DropTable(
                name: "Proofs");

            migrationBuilder.DropTable(
                name: "Symbols");

            migrationBuilder.DropTable(
                name: "InferenceAssumptionDissolutionTypes");

            migrationBuilder.DropTable(
                name: "InferenceArgumentTypes");

            migrationBuilder.DropTable(
                name: "Axioms");

            migrationBuilder.DropTable(
                name: "Formulas");

            migrationBuilder.DropTable(
                name: "Inferences");

            migrationBuilder.DropTable(
                name: "ProofHeads");

            migrationBuilder.DropTable(
                name: "SymbolTypes");

            migrationBuilder.DropTable(
                name: "Theorems");

            migrationBuilder.DropTable(
                name: "FormulaTypes");
        }
    }
}
