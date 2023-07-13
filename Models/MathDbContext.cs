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
  public DbSet<FormulaChain> FormulaChains { get; set;} = null!;
  public DbSet<Inference> Inferences { get; set; } = null!;
  public DbSet<InferenceArgumentType> InferenceArgumentTypes { get; set; } = null!;
  public DbSet<InferenceArgument> InferenceArguments { get; set; } = null!;
  public DbSet<InferenceArgumentConstraint> InferenceArgumentConstraints{ get; set; } = null!;
  public DbSet<InferenceAssumption> InferenceAssumptions { get; set; } = null!;
  public DbSet<InferenceAssumptionDissolutionType> InferenceAssumptionDissolutionTypes { get; set; } = null!;
  public DbSet<InferenceAssumptionFormula> InferenceAssumptionFormulas { get; set; } = null!;
  public DbSet<InferenceAssumptionDissolutableAssumptionFormula> InferenceAssumptionDissolutableAssumptionFormula { get; set; } = null!;
  public DbSet<InferenceConclusionFormula> InferenceConclusionFormulas { get; set; } = null!;
  public DbSet<Axiom> Axioms { get; set; } = null!;
  public DbSet<AxiomProposition> AxiomPropositions { get; set; } = null!;
  public DbSet<Theorem> Theorems { get; set; } = null!;
  public DbSet<TheoremAssumption> TheoremAssumption { get; set; } = null!;
  public DbSet<TheoremConclusion> TheoremConclusions { get; set; } = null!;
  public DbSet<ProofHead> ProofHeads { get; set; } = null!;
  public DbSet<Proof> Proofs { get; set; } = null!;
  public DbSet<ProofArgument> ProofArguments { get; set; } = null!;
  public DbSet<ProofAssumption> ProofAssumptions { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<AxiomProposition>(
      nestedBuilder => {
        nestedBuilder.HasKey(ap => new { ap.AxiomId, ap.SerialNo });
        nestedBuilder.HasOne(p => p.Formula)
                     .WithMany(f => f.AxiomPropositions)
                     .HasForeignKey(p => p.FormulaId);
      }
    );
    modelBuilder.Entity<FormulaString>()
      .HasKey(fs => new { fs.FormulaId, fs.SerialNo });
    modelBuilder.Entity<FormulaChain>(
      nestedBuilder => {
        nestedBuilder.HasKey(fc => new { fc.FormulaId, fc.SerialNo});
        nestedBuilder.HasOne(fc => fc.FromFormulaString)
                     .WithOne(fs => fs.FormulaChainAtFrom)
                     .HasPrincipalKey<FormulaString>(fs => new { fs.FormulaId, fs.SerialNo })
                     .HasForeignKey<FormulaChain>(fc => new { fc.FormulaId, fc.FromFormulaStringSerialNo });
        nestedBuilder.HasOne(fc => fc.ToFormulaString)
                     .WithOne(fs => fs.FormulaChainAtTo)
                     .HasPrincipalKey<FormulaString>(fs => new { fs.FormulaId, fs.SerialNo })
                     .HasForeignKey<FormulaChain>(fc => new { fc.FormulaId, fc.ToFormulaStringSerialNo });
      }
    );
    modelBuilder.Entity<InferenceArgument>()
      .HasKey(ia => new { ia.InferenceId, ia.SerialNo });
    modelBuilder.Entity<InferenceArgumentConstraint>(
      nestedBuilder => {
        nestedBuilder.HasKey(iac => new { iac.InferenceId, iac.InferenceArgumentSerialNo, iac.SerialNo });
        nestedBuilder.HasOne(iac => iac.InferenceArgument)
                     .WithMany(ia => ia.InferenceArgumentConstraints)
                     .HasPrincipalKey(ia => new { ia.InferenceId, ia.SerialNo })
                     .HasForeignKey(iac => new { iac.InferenceId, iac.InferenceArgumentSerialNo });
        nestedBuilder.HasOne(iac => iac.ConstraintDestinationInferenceArgument)
                     .WithMany(ia => ia.InferenceArgumentConstraintDistinations)
                     .HasPrincipalKey(ia => new { ia.InferenceId, ia.SerialNo })
                     .HasForeignKey(iac => new { iac.InferenceId, iac.ConstraintDestinationInferenceArgumentSerialNo });
      }
    );
    modelBuilder.Entity<InferenceAssumption>()
      .HasKey(ia => new { ia.InferenceId, ia.SerialNo });
    modelBuilder.Entity<InferenceAssumptionDissolutableAssumptionFormula>(
      nestedBuilder => {
        nestedBuilder.HasKey(iadaf => new { iadaf.InferenceId, iadaf.InferenceAssumptionSerialNo, iadaf.SerialNo });
        nestedBuilder.HasOne(iadaf => iadaf.InferenceAssumption)
                     .WithMany(ia => ia.InferenceAssumptionDissolutableAssumptionFormulas)
                     .HasPrincipalKey(e => new { e.InferenceId, e.SerialNo })
                     .HasForeignKey(e => new { e.InferenceId, e.InferenceAssumptionSerialNo });
        nestedBuilder.HasOne(iadaf => iadaf.BoundInferenceArgument)
                     .WithOne(iag => iag.InferenceAssumptionDissolutableAssumptionFormulaToBound)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceAssumptionDissolutableAssumptionFormula>(
                       iadaf => new { iadaf.InferenceId, iadaf.BoundInferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(iadaf => iadaf.InferenceArgument)
                     .WithOne(iag => iag.InferenceAssumptionDissolutableAssumptionFormula)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceAssumptionDissolutableAssumptionFormula>(
                       iadaf => new { iadaf.InferenceId, iadaf.InferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(iadaf => iadaf.SubstitutionInferenceArgumentFrom)
                     .WithOne(iag => iag.InferenceAssumptionDissolutableAssumptionFormulaToSubstitutionInferenceArgumentFrom)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceAssumptionDissolutableAssumptionFormula>(
                       iadaf => new { iadaf.InferenceId, iadaf.SubstitutionInferenceArgumentFromSerialNo }
                     );
        nestedBuilder.HasOne(iadaf => iadaf.SubstitutionInferenceArgumentTo)
                     .WithOne(iag => iag.InferenceAssumptionDissolutableAssumptionFormulaToSubstitutionInferenceArgumentTo)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceAssumptionDissolutableAssumptionFormula>(
                       iadaf => new { iadaf.InferenceId, iadaf.SubstitutionInferenceArgumentToSerialNo }
                     );
      }
    );
    modelBuilder.Entity<InferenceAssumptionFormula>(
      nestedBuilder => {
        nestedBuilder.HasKey(iaf => new { iaf.InferenceId, iaf.InferenceAssumptionSerialNo, iaf.SerialNo });
        nestedBuilder.HasOne(iadaf => iadaf.InferenceAssumption)
                     .WithMany(ia => ia.InferenceAssumptionFormulas)
                     .HasPrincipalKey(e => new { e.InferenceId, e.SerialNo })
                     .HasForeignKey(e => new { e.InferenceId, e.InferenceAssumptionSerialNo });
        nestedBuilder.HasOne(iaf => iaf.BoundInferenceArgument)
                     .WithOne(iag => iag.InferenceAssumptionFormulaToBound)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceAssumptionFormula>(
                       iaf => new { iaf.InferenceId, iaf.BoundInferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(iaf => iaf.InferenceArgument)
                     .WithOne(iag => iag.InferenceAssumptionFormula)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceAssumptionFormula>(
                       iaf => new { iaf.InferenceId, iaf.InferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(iaf => iaf.SubstitutionInferenceArgumentFrom)
                     .WithOne(iag => iag.InferenceAssumptionFormulaToSubstitutionInferenceArgumentFrom)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceAssumptionFormula>(
                       iag => new { iag.InferenceId, iag.SubstitutionInferenceArgumentFromSerialNo }
                     );
        nestedBuilder.HasOne(iaf => iaf.SubstitutionInferenceArgumentTo)
                     .WithOne(iag => iag.InferenceAssumptionFormulaToSubstitutionInferenceArgumentTo)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceAssumptionFormula>(
                       iaf => new { iaf.InferenceId, iaf.SubstitutionInferenceArgumentToSerialNo }
                     );
      }
    );
    modelBuilder.Entity<InferenceConclusionFormula>(
      nestedBuilder => {
        nestedBuilder.HasKey(icf => new { icf.InferenceId, icf.SerialNo });
        nestedBuilder.HasOne(icf => icf.BoundInferenceArgument)
                     .WithOne(iag => iag.InferenceConclusionFormulaToBound)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceConclusionFormula>(
                       icf => new { icf.InferenceId, icf.BoundInferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(icf => icf.InferenceArgument)
                     .WithOne(iag => iag.InferenceConclusionFormula)
                     .HasPrincipalKey<InferenceArgument>(
                       icf => new { icf.InferenceId, icf.SerialNo }
                     )
                     .HasForeignKey<InferenceConclusionFormula>(
                       iag => new { iag.InferenceId, iag.InferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(icf => icf.SubstitutionInferenceArgumentFrom)
                     .WithOne(iag => iag.InferenceConclusionFormulaToSubstitutionInferenceArgumentFrom)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceConclusionFormula>(
                       icf => new { icf.InferenceId, icf.SubstitutionInferenceArgumentFromSerialNo }
                     );
        nestedBuilder.HasOne(icf => icf.SubstitutionInferenceArgumentTo)
                     .WithOne(iag => iag.InferenceConclusionFormulaToSubstitutionInferenceArgumentTo)
                     .HasPrincipalKey<InferenceArgument>(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey<InferenceConclusionFormula>(
                       icf => new { icf.InferenceId, icf.SubstitutionInferenceArgumentToSerialNo }
                     );
      }
    );
    modelBuilder.Entity<Proof>(
      nestedBuilder => {
        nestedBuilder.HasKey(p => new { p.ProofHeadId, p.ProofHeadSerialNo, p.SerialNo });
        nestedBuilder.HasOne(p => p.ConclusionFormula)
                     .WithOne(f => f.Proof)
                     .HasForeignKey<Proof>(p => p.ConclusionFormulaId);
        nestedBuilder.HasOne(p => p.Inference)
                     .WithOne(i => i.Proof)
                     .HasForeignKey<Proof>(p => p.InferenceId);
      }
    );
    modelBuilder.Entity<ProofArgument>(
      nestedBuilder => {
        nestedBuilder.HasKey(pa => new { pa.ProofHeadId, pa.ProofHeadSerialNo, pa.ProofSerialNo, pa.SerialNo });
        nestedBuilder.HasOne(pa => pa.AxiomProposition)
                     .WithMany(ap => ap.ProofArguments)
                     .HasPrincipalKey(ap => new { ap.AxiomId, ap.SerialNo })
                     .HasForeignKey(pa => new { pa.AxiomId, pa.AxiomPropositionSerialNo });
      }
    );
    modelBuilder.Entity<Symbol>()
      .HasOne(s => s.ArityFormulaType)
      .WithMany(f => f.Symbols)
      .HasForeignKey(s => s.ArityFormulaTypeId);
    modelBuilder.Entity<TheoremAssumption>()
      .HasKey(ta => new { ta.TheoremId, ta.SerialNo });
    modelBuilder.Entity<TheoremConclusion>()
      .HasKey(tc => new { tc.TheoremId, tc.SerialNo });

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

    modelBuilder.Entity<InferenceArgumentType>().HasData(
      new { Id = 1, Name = "term" },
      new { Id = 2, Name = "proposition" },
      new { Id = 3, Name = "free variable" }
    );

    modelBuilder.Entity<InferenceAssumptionDissolutionType>().HasData(
      new { Id = 1, Name = "none" },
      new { Id = 2, Name = "required" },
      new { Id = 3, Name = "necessary" }
    );
  }
}