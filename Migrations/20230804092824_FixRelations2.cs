using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathApi.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_InferenceA~",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~1",
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
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_B~",
                table: "InferenceAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_I~",
                table: "InferenceAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_S~",
                table: "InferenceAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_~1",
                table: "InferenceAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionFormulas_Symbols_SymbolId",
                table: "InferenceAssumptionFormulas");

            migrationBuilder.AlterColumn<long>(
                name: "SymbolId",
                table: "InferenceAssumptionFormulas",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "SubstitutionInferenceArgumentToSerialNo",
                table: "InferenceAssumptionFormulas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubstitutionInferenceArgumentFromSerialNo",
                table: "InferenceAssumptionFormulas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "InferenceArgumentSerialNo",
                table: "InferenceAssumptionFormulas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BoundInferenceArgumentSerialNo",
                table: "InferenceAssumptionFormulas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "SymbolId",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "SubstitutionInferenceArgumentToSerialNo",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubstitutionInferenceArgumentFromSerialNo",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "InferenceArgumentSerialNo",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BoundInferenceArgumentSerialNo",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_InferenceA~",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~1",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~3",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~4",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Symbols_Sy~",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_B~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_I~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_S~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_~1",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionFormulas_Symbols_SymbolId",
                table: "InferenceAssumptionFormulas",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id");
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
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~3",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Inference~4",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionDissolutableAssumptionFormulas_Symbols_Sy~",
                table: "InferenceAssumptionDissolutableAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_B~",
                table: "InferenceAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_I~",
                table: "InferenceAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_S~",
                table: "InferenceAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_~1",
                table: "InferenceAssumptionFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceAssumptionFormulas_Symbols_SymbolId",
                table: "InferenceAssumptionFormulas");

            migrationBuilder.AlterColumn<long>(
                name: "SymbolId",
                table: "InferenceAssumptionFormulas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubstitutionInferenceArgumentToSerialNo",
                table: "InferenceAssumptionFormulas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubstitutionInferenceArgumentFromSerialNo",
                table: "InferenceAssumptionFormulas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InferenceArgumentSerialNo",
                table: "InferenceAssumptionFormulas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BoundInferenceArgumentSerialNo",
                table: "InferenceAssumptionFormulas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SymbolId",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubstitutionInferenceArgumentToSerialNo",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SubstitutionInferenceArgumentFromSerialNo",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InferenceArgumentSerialNo",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BoundInferenceArgumentSerialNo",
                table: "InferenceAssumptionDissolutableAssumptionFormulas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_B~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "BoundInferenceArgumentSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_I~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "InferenceArgumentSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_S~",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentFromSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionFormulas_InferenceArguments_InferenceId_~1",
                table: "InferenceAssumptionFormulas",
                columns: new[] { "InferenceId", "SubstitutionInferenceArgumentToSerialNo" },
                principalTable: "InferenceArguments",
                principalColumns: new[] { "InferenceId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceAssumptionFormulas_Symbols_SymbolId",
                table: "InferenceAssumptionFormulas",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
