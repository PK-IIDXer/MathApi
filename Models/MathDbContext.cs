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
  {
    optionsBuilder.UseMySql(connectionString, serverVersion);
    optionsBuilder.LogTo(Console.WriteLine).EnableSensitiveDataLogging();
  }

  public DbSet<FormulaType> FormulaTypes { get; set; } = null!;
  public DbSet<SymbolType> SymbolTypes { get; set; } = null!;
  public DbSet<Symbol> Symbols { get; set; } = null!;
  public DbSet<Formula> Formulas { get; set; } = null!;
  public DbSet<FormulaString> FormulaStrings { get; set; } = null!;
  public DbSet<FormulaChain> FormulasChain { get; set;} = null!;
  public DbSet<Inference> Inferences { get; set; } = null!;
  public DbSet<InferenceAssumption> InferenceAssumptions { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<FormulaType>().HasData(
      new { Id = 1L, Name = "Term" },
      new { Id = 2L, Name = "Proposition" }
    );

    modelBuilder.Entity<SymbolType>().HasData(
      new { Id = 1L, Name = "free variable", FormulaTypeId = 1L },
      new { Id = 2L, Name = "bound variable", FormulaTypeId = 1L },
      new { Id = 3L, Name = "proposition variable", FormulaTypeId = 2L },
      new { Id = 4L, Name = "constant", FormulaTypeId = 1L },
      new { Id = 5L, Name = "function", FormulaTypeId = 1L },
      new { Id = 6L, Name = "predicate", FormulaTypeId = 2L },
      new { Id = 7L, Name = "logic", FormulaTypeId = 2L },
      new { Id = 8L, Name = "term quantifier", FormulaTypeId = 1L },
      new { Id = 9L, Name = "proposition quantifier", FormulaTypeId = 2L }
    );

    modelBuilder.Entity<FormulaChain>()
      .HasOne<FormulaString>()
      .WithMany()
      .HasForeignKey(chain => new { chain.FormulaId, chain.FromFormulaStringSerialNo });

    modelBuilder.Entity<FormulaChain>()
      .HasOne<FormulaString>()
      .WithMany()
      .HasForeignKey(chain => new { chain.FormulaId, chain.ToFormulaStringSerialNo });
  }
}