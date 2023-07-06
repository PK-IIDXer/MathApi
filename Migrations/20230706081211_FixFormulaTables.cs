using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathApi.Migrations
{
    /// <inheritdoc />
    public partial class FixFormulaTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormulasChain_FormulaStrings_FromFormulaStringFormulaId_From~",
                table: "FormulasChain");

            migrationBuilder.DropForeignKey(
                name: "FK_FormulasChain_FormulaStrings_ToFormulaStringFormulaId_ToForm~",
                table: "FormulasChain");

            migrationBuilder.AlterColumn<long>(
                name: "ToFormulaStringSerialNo",
                table: "FormulasChain",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FromFormulaStringSerialNo",
                table: "FormulasChain",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FormulasChain_FormulaStrings_FormulaId_FromFormulaStringSeri~",
                table: "FormulasChain",
                columns: new[] { "FormulaId", "FromFormulaStringSerialNo" },
                principalTable: "FormulaStrings",
                principalColumns: new[] { "FormulaId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormulasChain_FormulaStrings_FormulaId_ToFormulaStringSerial~",
                table: "FormulasChain",
                columns: new[] { "FormulaId", "ToFormulaStringSerialNo" },
                principalTable: "FormulaStrings",
                principalColumns: new[] { "FormulaId", "SerialNo" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormulasChain_FormulaStrings_FromFormulaStringFormulaId_From~",
                table: "FormulasChain");

            migrationBuilder.DropForeignKey(
                name: "FK_FormulasChain_FormulaStrings_ToFormulaStringFormulaId_ToForm~",
                table: "FormulasChain");

            migrationBuilder.AlterColumn<long>(
                name: "ToFormulaStringSerialNo",
                table: "FormulasChain",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "FromFormulaStringSerialNo",
                table: "FormulasChain",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_FormulasChain_FormulaStrings_FormulaId_FromFormulaStringSeri~",
                table: "FormulasChain",
                columns: new[] { "FormulaId", "FromFormulaStringSerialNo" },
                principalTable: "FormulaStrings",
                principalColumns: new[] { "FormulaId", "SerialNo" });

            migrationBuilder.AddForeignKey(
                name: "FK_FormulasChain_FormulaStrings_FormulaId_ToFormulaStringSerial~",
                table: "FormulasChain",
                columns: new[] { "FormulaId", "ToFormulaStringSerialNo" },
                principalTable: "FormulaStrings",
                principalColumns: new[] { "FormulaId", "SerialNo" });
        }
    }
}
