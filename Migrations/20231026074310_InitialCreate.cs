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
                name: "FormulaLabelTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaLabelTypes", x => x.Id);
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
                name: "FormulaStructs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Meaning = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaStructs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormulaTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaTypes", x => x.Id);
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
                    TheoremId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inferences", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormulaLabels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaLabels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormulaLabels_FormulaLabelTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FormulaLabelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AxiomPropositions",
                columns: table => new
                {
                    AxiomId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FormulaId = table.Column<long>(type: "bigint", nullable: false),
                    Meaning = table.Column<string>(type: "longtext", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FormulaTypeId = table.Column<int>(type: "int", nullable: false),
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
                name: "InferenceAssumptions",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    FormulaStructId = table.Column<long>(type: "bigint", nullable: false),
                    FormulaStructArgumentMappingSerialNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceAssumptions", x => new { x.InferenceId, x.SerialNo });
                    table.UniqueConstraint("AK_InferenceAssumptions_InferenceId_FormulaStructArgumentMappin~", x => new { x.InferenceId, x.FormulaStructArgumentMappingSerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptions_FormulaStructs_FormulaStructId",
                        column: x => x.FormulaStructId,
                        principalTable: "FormulaStructs",
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
                name: "InferenceConclusions",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    FormulaStructId = table.Column<long>(type: "bigint", nullable: false),
                    FormulaStructArgumentMappingSerialNo = table.Column<int>(type: "int", nullable: false),
                    AddAssumption = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceConclusions", x => x.InferenceId);
                    table.UniqueConstraint("AK_InferenceConclusions_InferenceId_FormulaStructArgumentMappin~", x => new { x.InferenceId, x.FormulaStructArgumentMappingSerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceConclusions_FormulaStructs_FormulaStructId",
                        column: x => x.FormulaStructId,
                        principalTable: "FormulaStructs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceConclusions_Inferences_InferenceId",
                        column: x => x.InferenceId,
                        principalTable: "Inferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    IsProved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    InferenceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theorems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Theorems_Inferences_InferenceId",
                        column: x => x.InferenceId,
                        principalTable: "Inferences",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormulaStructArguments",
                columns: table => new
                {
                    FormulaStructId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    LabelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaStructArguments", x => new { x.FormulaStructId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_FormulaStructArguments_FormulaLabels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "FormulaLabels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulaStructArguments_FormulaStructs_FormulaStructId",
                        column: x => x.FormulaStructId,
                        principalTable: "FormulaStructs",
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
                    FormulaLabelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceArguments", x => new { x.InferenceId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceArguments_FormulaLabels_FormulaLabelId",
                        column: x => x.FormulaLabelId,
                        principalTable: "FormulaLabels",
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
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Character = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Arity = table.Column<int>(type: "int", nullable: true),
                    ArityFormulaTypeId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_Symbols_SymbolTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "SymbolTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InferenceAssumptionDissolutableAssumptions",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    InferenceAssumptionSerialNo = table.Column<int>(type: "int", nullable: false),
                    FormulaStructId = table.Column<long>(type: "bigint", nullable: false),
                    FormulaStructArgumentMappingSerialNo = table.Column<int>(type: "int", nullable: false),
                    IsForce = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceAssumptionDissolutableAssumptions", x => new { x.InferenceId, x.InferenceAssumptionSerialNo });
                    table.UniqueConstraint("AK_InferenceAssumptionDissolutableAssumptions_InferenceId_Formu~", x => new { x.InferenceId, x.FormulaStructArgumentMappingSerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptions_FormulaStructs_Fo~",
                        column: x => x.FormulaStructId,
                        principalTable: "FormulaStructs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceAssumptionDissolutableAssumptions_InferenceAssumpti~",
                        columns: x => new { x.InferenceId, x.InferenceAssumptionSerialNo },
                        principalTable: "InferenceAssumptions",
                        principalColumns: new[] { "InferenceId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
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
                name: "InferenceArgumentConstraints",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    InferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    ConstraintDestinationInferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: true),
                    AssumptionSerialNoForConstraintToAllPredissolvedAssumption = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceArgumentConstraints", x => new { x.InferenceId, x.InferenceArgumentSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceArgumentConstraints_InferenceArguments_InferenceId_~",
                        columns: x => new { x.InferenceId, x.ConstraintDestinationInferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceArgumentConstraints_InferenceArguments_InferenceId~1",
                        columns: x => new { x.InferenceId, x.InferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceArgumentConstraints_InferenceAssumptions_InferenceI~",
                        columns: x => new { x.InferenceId, x.AssumptionSerialNoForConstraintToAllPredissolvedAssumption },
                        principalTable: "InferenceAssumptions",
                        principalColumns: new[] { "InferenceId", "SerialNo" });
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
                name: "FormulaStructStrings",
                columns: table => new
                {
                    FormulaStructId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    SymbolId = table.Column<long>(type: "bigint", nullable: true),
                    BoundArgumentSerialNo = table.Column<int>(type: "int", nullable: true),
                    ArgumentSerialNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaStructStrings", x => new { x.FormulaStructId, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_FormulaStructStrings_FormulaStructArguments_FormulaStructId_~",
                        columns: x => new { x.FormulaStructId, x.ArgumentSerialNo },
                        principalTable: "FormulaStructArguments",
                        principalColumns: new[] { "FormulaStructId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_FormulaStructStrings_FormulaStructArguments_FormulaStructId~1",
                        columns: x => new { x.FormulaStructId, x.BoundArgumentSerialNo },
                        principalTable: "FormulaStructArguments",
                        principalColumns: new[] { "FormulaStructId", "SerialNo" });
                    table.ForeignKey(
                        name: "FK_FormulaStructStrings_FormulaStructs_FormulaStructId",
                        column: x => x.FormulaStructId,
                        principalTable: "FormulaStructs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulaStructStrings_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InferenceFormulaStructArgumentMappings",
                columns: table => new
                {
                    InferenceId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    FormulaStructId = table.Column<long>(type: "bigint", nullable: false),
                    FormulaStructArgumentSerialNo = table.Column<int>(type: "int", nullable: false),
                    InferenceArgumentSerialNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceFormulaStructArgumentMappings", x => new { x.InferenceId, x.SerialNo, x.FormulaStructId, x.FormulaStructArgumentSerialNo });
                    table.ForeignKey(
                        name: "FK_InferenceFormulaStructArgumentMappings_FormulaStructArgument~",
                        columns: x => new { x.FormulaStructId, x.FormulaStructArgumentSerialNo },
                        principalTable: "FormulaStructArguments",
                        principalColumns: new[] { "FormulaStructId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceFormulaStructArgumentMappings_InferenceArguments_In~",
                        columns: x => new { x.InferenceId, x.InferenceArgumentSerialNo },
                        principalTable: "InferenceArguments",
                        principalColumns: new[] { "InferenceId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceFormulaStructArgumentMappings_InferenceAssumptionDi~",
                        columns: x => new { x.InferenceId, x.SerialNo },
                        principalTable: "InferenceAssumptionDissolutableAssumptions",
                        principalColumns: new[] { "InferenceId", "FormulaStructArgumentMappingSerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceFormulaStructArgumentMappings_InferenceAssumptions_~",
                        columns: x => new { x.InferenceId, x.SerialNo },
                        principalTable: "InferenceAssumptions",
                        principalColumns: new[] { "InferenceId", "FormulaStructArgumentMappingSerialNo" });
                    table.ForeignKey(
                        name: "FK_InferenceFormulaStructArgumentMappings_InferenceConclusions_~",
                        columns: x => new { x.InferenceId, x.SerialNo },
                        principalTable: "InferenceConclusions",
                        principalColumns: new[] { "InferenceId", "FormulaStructArgumentMappingSerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceFormulaStructArgumentMappings_Inferences_InferenceId",
                        column: x => x.InferenceId,
                        principalTable: "Inferences",
                        principalColumn: "Id",
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
                name: "FormulaStructStringSubstitutions",
                columns: table => new
                {
                    FormulaStructId = table.Column<long>(type: "bigint", nullable: false),
                    FormulaStructStringSerialNo = table.Column<int>(type: "int", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    ArgumentFromSerialNo = table.Column<int>(type: "int", nullable: false),
                    ArgumentToSerialNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaStructStringSubstitutions", x => new { x.FormulaStructId, x.FormulaStructStringSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_FormulaStructStringSubstitutions_FormulaStructArguments_Form~",
                        columns: x => new { x.FormulaStructId, x.ArgumentFromSerialNo },
                        principalTable: "FormulaStructArguments",
                        principalColumns: new[] { "FormulaStructId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulaStructStringSubstitutions_FormulaStructArguments_For~1",
                        columns: x => new { x.FormulaStructId, x.ArgumentToSerialNo },
                        principalTable: "FormulaStructArguments",
                        principalColumns: new[] { "FormulaStructId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormulaStructStringSubstitutions_FormulaStructStrings_Formul~",
                        columns: x => new { x.FormulaStructId, x.FormulaStructStringSerialNo },
                        principalTable: "FormulaStructStrings",
                        principalColumns: new[] { "FormulaStructId", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProofAssumptions",
                columns: table => new
                {
                    TheoremId = table.Column<long>(type: "bigint", nullable: false),
                    ProofSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    AddedProofInferenceSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    DissolutedProofInferenceSerialNo = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofAssumptions", x => new { x.TheoremId, x.ProofSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_ProofAssumptions_Proofs_TheoremId_ProofSerialNo",
                        columns: x => new { x.TheoremId, x.ProofSerialNo },
                        principalTable: "Proofs",
                        principalColumns: new[] { "TheoremId", "SerialNo" },
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
                    ConclusionFormulaId = table.Column<long>(type: "bigint", nullable: true),
                    ConclusionFormulaStructId = table.Column<long>(type: "bigint", nullable: true),
                    ProofAssumptionSerialNo = table.Column<long>(type: "bigint", nullable: true),
                    TreeFrom = table.Column<long>(type: "bigint", nullable: false),
                    TreeTo = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofInferences", x => new { x.TheoremId, x.ProofSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_ProofInferences_FormulaStructs_ConclusionFormulaStructId",
                        column: x => x.ConclusionFormulaStructId,
                        principalTable: "FormulaStructs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProofInferences_Formulas_ConclusionFormulaId",
                        column: x => x.ConclusionFormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProofInferences_Inferences_InferenceId",
                        column: x => x.InferenceId,
                        principalTable: "Inferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProofInferences_ProofAssumptions_TheoremId_ProofSerialNo_Pro~",
                        columns: x => new { x.TheoremId, x.ProofSerialNo, x.ProofAssumptionSerialNo },
                        principalTable: "ProofAssumptions",
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
                name: "ProofInferenceArguments",
                columns: table => new
                {
                    TheoremId = table.Column<long>(type: "bigint", nullable: false),
                    ProofSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    ProofInferenceSerialNo = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<long>(type: "bigint", nullable: false),
                    FormulaId = table.Column<long>(type: "bigint", nullable: true),
                    FormulaStructId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofInferenceArguments", x => new { x.TheoremId, x.ProofSerialNo, x.ProofInferenceSerialNo, x.SerialNo });
                    table.ForeignKey(
                        name: "FK_ProofInferenceArguments_FormulaStructs_FormulaStructId",
                        column: x => x.FormulaStructId,
                        principalTable: "FormulaStructs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProofInferenceArguments_Formulas_FormulaId",
                        column: x => x.FormulaId,
                        principalTable: "Formulas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProofInferenceArguments_ProofInferences_TheoremId_ProofSeria~",
                        columns: x => new { x.TheoremId, x.ProofSerialNo, x.ProofInferenceSerialNo },
                        principalTable: "ProofInferences",
                        principalColumns: new[] { "TheoremId", "ProofSerialNo", "SerialNo" },
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "FormulaLabelTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Term" },
                    { 2, "Proposition" },
                    { 3, "Free Variable" }
                });

            migrationBuilder.InsertData(
                table: "FormulaTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Term" },
                    { 2, "Proposition" }
                });

            migrationBuilder.InsertData(
                table: "SymbolTypes",
                columns: new[] { "Id", "FormulaTypeId", "IsQuantifier", "Name" },
                values: new object[,]
                {
                    { 1, 1, false, "free variable" },
                    { 2, 1, false, "bound variable" },
                    { 3, 1, false, "function" },
                    { 4, 2, false, "predicate" },
                    { 5, 2, false, "logic" },
                    { 6, 1, true, "term quantifier" },
                    { 7, 2, true, "proposition quantifier" }
                });

            migrationBuilder.InsertData(
                table: "Symbols",
                columns: new[] { "Id", "Arity", "ArityFormulaTypeId", "Character", "Meaning", "TypeId" },
                values: new object[,]
                {
                    { 1L, null, null, "□", "bound variable", 2 },
                    { 2L, 2, 1, "=", "equals", 4 },
                    { 3L, 0, 2, "⊥", "contradiction", 5 },
                    { 4L, 1, 2, "￢", "not", 5 },
                    { 5L, 2, 2, "∧", "and", 5 },
                    { 6L, 2, 2, "∨", "or", 5 },
                    { 7L, 2, 2, "⇒", "imply", 5 },
                    { 8L, 1, 2, "∀", "forall", 7 },
                    { 9L, 1, 2, "∃", "exists", 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AxiomPropositions_FormulaId",
                table: "AxiomPropositions",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaChains_FormulaId_FromFormulaStringSerialNo",
                table: "FormulaChains",
                columns: new[] { "FormulaId", "FromFormulaStringSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_FormulaChains_FormulaId_ToFormulaStringSerialNo",
                table: "FormulaChains",
                columns: new[] { "FormulaId", "ToFormulaStringSerialNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormulaLabels_TypeId",
                table: "FormulaLabels",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStrings_SymbolId",
                table: "FormulaStrings",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStructArguments_LabelId",
                table: "FormulaStructArguments",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStructStrings_FormulaStructId_ArgumentSerialNo",
                table: "FormulaStructStrings",
                columns: new[] { "FormulaStructId", "ArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStructStrings_FormulaStructId_BoundArgumentSerialNo",
                table: "FormulaStructStrings",
                columns: new[] { "FormulaStructId", "BoundArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStructStrings_SymbolId",
                table: "FormulaStructStrings",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStructStringSubstitutions_FormulaStructId_ArgumentFro~",
                table: "FormulaStructStringSubstitutions",
                columns: new[] { "FormulaStructId", "ArgumentFromSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStructStringSubstitutions_FormulaStructId_ArgumentToS~",
                table: "FormulaStructStringSubstitutions",
                columns: new[] { "FormulaStructId", "ArgumentToSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceArgumentConstraints_InferenceId_AssumptionSerialNoF~",
                table: "InferenceArgumentConstraints",
                columns: new[] { "InferenceId", "AssumptionSerialNoForConstraintToAllPredissolvedAssumption" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceArgumentConstraints_InferenceId_ConstraintDestinati~",
                table: "InferenceArgumentConstraints",
                columns: new[] { "InferenceId", "ConstraintDestinationInferenceArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceArguments_FormulaLabelId",
                table: "InferenceArguments",
                column: "FormulaLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptionDissolutableAssumptions_FormulaStructId",
                table: "InferenceAssumptionDissolutableAssumptions",
                column: "FormulaStructId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceAssumptions_FormulaStructId",
                table: "InferenceAssumptions",
                column: "FormulaStructId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceConclusions_FormulaStructId",
                table: "InferenceConclusions",
                column: "FormulaStructId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceFormulaStructArgumentMappings_FormulaStructId_Formu~",
                table: "InferenceFormulaStructArgumentMappings",
                columns: new[] { "FormulaStructId", "FormulaStructArgumentSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceFormulaStructArgumentMappings_InferenceId_Inference~",
                table: "InferenceFormulaStructArgumentMappings",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" });

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
                name: "IX_ProofInferenceArguments_FormulaId",
                table: "ProofInferenceArguments",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferenceArguments_FormulaStructId",
                table: "ProofInferenceArguments",
                column: "FormulaStructId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferences_ConclusionFormulaId",
                table: "ProofInferences",
                column: "ConclusionFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferences_ConclusionFormulaStructId",
                table: "ProofInferences",
                column: "ConclusionFormulaStructId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferences_InferenceId",
                table: "ProofInferences",
                column: "InferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProofInferences_TheoremId_ProofSerialNo_ProofAssumptionSeria~",
                table: "ProofInferences",
                columns: new[] { "TheoremId", "ProofSerialNo", "ProofAssumptionSerialNo" });

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_ArityFormulaTypeId",
                table: "Symbols",
                column: "ArityFormulaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Character_TypeId",
                table: "Symbols",
                columns: new[] { "Character", "TypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_TypeId",
                table: "Symbols",
                column: "TypeId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Theorems_InferenceId",
                table: "Theorems",
                column: "InferenceId",
                unique: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferences_Formulas_ConclusionFormulaId",
                table: "ProofInferences");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferences_FormulaStructs_ConclusionFormulaStructId",
                table: "ProofInferences");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofInferences_Inferences_InferenceId",
                table: "ProofInferences");

            migrationBuilder.DropForeignKey(
                name: "FK_Theorems_Inferences_InferenceId",
                table: "Theorems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Add~",
                table: "ProofAssumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProofAssumptions_ProofInferences_TheoremId_ProofSerialNo_Dis~",
                table: "ProofAssumptions");

            migrationBuilder.DropTable(
                name: "AxiomPropositions");

            migrationBuilder.DropTable(
                name: "FormulaChains");

            migrationBuilder.DropTable(
                name: "FormulaStructStringSubstitutions");

            migrationBuilder.DropTable(
                name: "InferenceArgumentConstraints");

            migrationBuilder.DropTable(
                name: "InferenceFormulaStructArgumentMappings");

            migrationBuilder.DropTable(
                name: "ProofInferenceArguments");

            migrationBuilder.DropTable(
                name: "TheoremAssumptions");

            migrationBuilder.DropTable(
                name: "TheoremConclusions");

            migrationBuilder.DropTable(
                name: "Axioms");

            migrationBuilder.DropTable(
                name: "FormulaStrings");

            migrationBuilder.DropTable(
                name: "FormulaStructStrings");

            migrationBuilder.DropTable(
                name: "InferenceArguments");

            migrationBuilder.DropTable(
                name: "InferenceAssumptionDissolutableAssumptions");

            migrationBuilder.DropTable(
                name: "InferenceConclusions");

            migrationBuilder.DropTable(
                name: "FormulaStructArguments");

            migrationBuilder.DropTable(
                name: "Symbols");

            migrationBuilder.DropTable(
                name: "InferenceAssumptions");

            migrationBuilder.DropTable(
                name: "FormulaLabels");

            migrationBuilder.DropTable(
                name: "SymbolTypes");

            migrationBuilder.DropTable(
                name: "FormulaLabelTypes");

            migrationBuilder.DropTable(
                name: "FormulaTypes");

            migrationBuilder.DropTable(
                name: "Formulas");

            migrationBuilder.DropTable(
                name: "FormulaStructs");

            migrationBuilder.DropTable(
                name: "Inferences");

            migrationBuilder.DropTable(
                name: "ProofInferences");

            migrationBuilder.DropTable(
                name: "ProofAssumptions");

            migrationBuilder.DropTable(
                name: "Proofs");

            migrationBuilder.DropTable(
                name: "Theorems");
        }
    }
}
