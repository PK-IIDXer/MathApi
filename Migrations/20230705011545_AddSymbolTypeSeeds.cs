using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MathApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSymbolTypeSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SymbolTypes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "SymbolTypes",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "SymbolTypes",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "SymbolTypes",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "SymbolTypes",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "SymbolTypes",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "SymbolTypes",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "SymbolTypes",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "SymbolTypes",
                keyColumn: "Id",
                keyValue: 9L);
        }
    }
}
