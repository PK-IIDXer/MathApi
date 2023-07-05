using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

public class MathDbContext : DbContext
{
  // 接続文字列
  readonly string connectionString = "server=192.168.33.12;user=math;password=math@123;database=mathematics";
  // MySQLのバージョン
  readonly MySqlServerVersion serverVersion = new (new Version(8, 0, 32));
  // DBコンテキストの設定
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseMySql(connectionString, serverVersion);

  public DbSet<FormulaType>? FormulaTypes { get; set; }
  public DbSet<SymbolType>? SymbolTypes { get; set; }
  public DbSet<Symbol>? Symbols { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<FormulaType>().HasData(
      new FormulaType { Id = 1, Name = "Term" },
      new FormulaType { Id = 2, Name = "Proposition" }
    );

    modelBuilder.Entity<SymbolType>().HasData(
      new { Id = (long)1, Name = "free variable", FormulaTypeId = (long)1 },
      new { Id = (long)2, Name = "bound variable", FormulaTypeId = (long)1 },
      new { Id = (long)3, Name = "proposition variable", FormulaTypeId = (long)2 },
      new { Id = (long)4, Name = "constant", FormulaTypeId = (long)1 },
      new { Id = (long)5, Name = "function", FormulaTypeId = (long)1 },
      new { Id = (long)6, Name = "predicate", FormulaTypeId = (long)2 },
      new { Id = (long)7, Name = "logic", FormulaTypeId = (long)2 },
      new { Id = (long)8, Name = "term quantifier", FormulaTypeId = (long)1 },
      new { Id = (long)9, Name = "proposition quantifier", FormulaTypeId = (long)2 }
    );
  }
}