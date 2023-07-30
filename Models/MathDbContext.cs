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
  public DbSet<Proof> Proofs { get; set; } = null!;
  public DbSet<ProofInference> ProofInferences { get; set; } = null!;
  public DbSet<ProofInferenceArgument> ProofInferenceArguments { get; set; } = null!;
  public DbSet<ProofAssumption> ProofInferenceAssumptions { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<AxiomProposition>(
      nestedBuilder => {
        nestedBuilder.HasKey(ap => new { ap.AxiomId, ap.SerialNo });
        nestedBuilder.HasOne(ap => ap.Axiom)
                     .WithMany(a => a.AxiomPropositions)
                     .HasPrincipalKey(a => new { a.Id })
                     .HasForeignKey(ap => new { ap.AxiomId });
        nestedBuilder.HasOne(p => p.Formula)
                     .WithMany(f => f.AxiomPropositions)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(p => new { p.FormulaId });
      }
    );
    modelBuilder.Entity<FormulaString>(
      nestedBuilder => {
        nestedBuilder.HasKey(fs => new { fs.FormulaId, fs.SerialNo });
        nestedBuilder.HasOne(fs => fs.Formula)
                     .WithMany(f => f.FormulaStrings)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(fs => new { fs.FormulaId });
        nestedBuilder.HasOne(fs => fs.Symbol)
                     .WithMany(f => f.FormulaStrings)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(fs => new { fs.SymbolId });
      }
    );
    modelBuilder.Entity<FormulaChain>(
      nestedBuilder => {
        nestedBuilder.HasKey(fc => new { fc.FormulaId, fc.SerialNo});
        nestedBuilder.HasOne(fc => fc.Formula)
                     .WithMany(f => f.FormulaChains)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(fc => new { fc.FormulaId });
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
    modelBuilder.Entity<Inference>()
                .HasOne(i => i.Theorem)
                .WithOne(t => t.Inference)
                .HasPrincipalKey<Theorem>(t => new { t.Id })
                .HasForeignKey<Inference>(i => new { i.TheoremId });
    modelBuilder.Entity<InferenceArgument>(
      nestedBuilder => {
        nestedBuilder.HasKey(ia => new { ia.InferenceId, ia.SerialNo });
        nestedBuilder.HasOne(ia => ia.Inference)
                     .WithMany(i => i.InferenceArguments)
                     .HasPrincipalKey(i => new { i.Id })
                     .HasForeignKey(ia => new { ia.InferenceId });
        nestedBuilder.HasOne(ia => ia.InferenceArgumentType)
                     .WithMany(iat => iat.InferenceArguments)
                     .HasPrincipalKey(iat => new { iat.Id })
                     .HasForeignKey(ia => new { ia.InferenceArgumentTypeId });
        nestedBuilder.HasOne(ia => ia.VariableSymbol)
                     .WithMany(s => s.InferenceArguments)
                     .HasPrincipalKey(s => new { s.Id })
                     .HasForeignKey(ia => new { ia.VariableSymbolId });
      }
    );
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
    modelBuilder.Entity<InferenceAssumption>(
      nestedBuilder => {
        nestedBuilder.HasKey(ia => new { ia.InferenceId, ia.SerialNo });
        nestedBuilder.HasOne(ia => ia.Inference)
                     .WithMany(i => i.InferenceAssumptions)
                     .HasPrincipalKey(i => new { i.Id })
                     .HasForeignKey(ia => new { ia.InferenceId });
        nestedBuilder.HasOne(ia => ia.InferenceAssumptionDissolutionType)
                     .WithMany(iadt => iadt.InferenceAssumptions)
                     .HasPrincipalKey(iadt => new { iadt.Id })
                     .HasForeignKey(ia => new { ia.InferenceAssumptionDissolutionTypeId });
      }
    );
    modelBuilder.Entity<InferenceAssumptionDissolutableAssumptionFormula>(
      nestedBuilder => {
        nestedBuilder.HasKey(iadaf => new { iadaf.InferenceId, iadaf.InferenceAssumptionSerialNo, iadaf.SerialNo });
        nestedBuilder.HasOne(iadaf => iadaf.InferenceAssumption)
                     .WithMany(ia => ia.InferenceAssumptionDissolutableAssumptionFormulas)
                     .HasPrincipalKey(ia => new { ia.InferenceId, ia.SerialNo })
                     .HasForeignKey(iadaf => new { iadaf.InferenceId, iadaf.InferenceAssumptionSerialNo });
        nestedBuilder.HasOne(iadaf => iadaf.Symbol)
                     .WithMany(s => s.InferenceAssumptionDissolutableAssumptionFormulas)
                     .HasPrincipalKey(s => new { s.Id })
                     .HasForeignKey(iadaf => new { iadaf.SymbolId });
        nestedBuilder.HasOne(iadaf => iadaf.BoundInferenceArgument)
                     .WithMany(iag => iag.InferenceAssumptionDissolutableAssumptionFormulasToBound)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       iadaf => new { iadaf.InferenceId, iadaf.BoundInferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(iadaf => iadaf.InferenceArgument)
                     .WithMany(iag => iag.InferenceAssumptionDissolutableAssumptionFormulas)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       iadaf => new { iadaf.InferenceId, iadaf.InferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(iadaf => iadaf.SubstitutionInferenceArgumentFrom)
                     .WithMany(iag => iag.InferenceAssumptionDissolutableAssumptionFormulasToSubstitutionInferenceArgumentFrom)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       iadaf => new { iadaf.InferenceId, iadaf.SubstitutionInferenceArgumentFromSerialNo }
                     );
        nestedBuilder.HasOne(iadaf => iadaf.SubstitutionInferenceArgumentTo)
                     .WithMany(iag => iag.InferenceAssumptionDissolutableAssumptionFormulasToSubstitutionInferenceArgumentTo)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       iadaf => new { iadaf.InferenceId, iadaf.SubstitutionInferenceArgumentToSerialNo }
                     );
      }
    );
    modelBuilder.Entity<InferenceAssumptionFormula>(
      nestedBuilder => {
        nestedBuilder.HasKey(iaf => new { iaf.InferenceId, iaf.InferenceAssumptionSerialNo, iaf.SerialNo });
        nestedBuilder.HasOne(iaf => iaf.InferenceAssumption)
                     .WithMany(ia => ia.InferenceAssumptionFormulas)
                     .HasPrincipalKey(e => new { e.InferenceId, e.SerialNo })
                     .HasForeignKey(e => new { e.InferenceId, e.InferenceAssumptionSerialNo });
        nestedBuilder.HasOne(iaf => iaf.Symbol)
                     .WithMany(s => s.InferenceAssumptionFormulas)
                     .HasPrincipalKey(s => new { s.Id })
                     .HasForeignKey(iaf => new { iaf.SymbolId });
        nestedBuilder.HasOne(iaf => iaf.BoundInferenceArgument)
                     .WithMany(iag => iag.InferenceAssumptionFormulasToBound)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       iaf => new { iaf.InferenceId, iaf.BoundInferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(iaf => iaf.InferenceArgument)
                     .WithMany(iag => iag.InferenceAssumptionFormulas)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       iaf => new { iaf.InferenceId, iaf.InferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(iaf => iaf.SubstitutionInferenceArgumentFrom)
                     .WithMany(iag => iag.InferenceAssumptionFormulasToSubstitutionInferenceArgumentFrom)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       iag => new { iag.InferenceId, iag.SubstitutionInferenceArgumentFromSerialNo }
                     );
        nestedBuilder.HasOne(iaf => iaf.SubstitutionInferenceArgumentTo)
                     .WithMany(iag => iag.InferenceAssumptionFormulasToSubstitutionInferenceArgumentTo)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       iaf => new { iaf.InferenceId, iaf.SubstitutionInferenceArgumentToSerialNo }
                     );
        nestedBuilder.HasOne(iaf => iaf.Formula)
                     .WithMany(f => f.InferenceAssumptionFormulas)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(iaf => new { iaf.FormulaId });
      }
    );
    modelBuilder.Entity<InferenceConclusionFormula>(
      nestedBuilder => {
        nestedBuilder.HasKey(icf => new { icf.InferenceId, icf.SerialNo });
        nestedBuilder.HasOne(icf => icf.Inference)
                     .WithMany(i => i.InferenceConclusionFormulas)
                     .HasPrincipalKey(i => new { i.Id })
                     .HasForeignKey(icf => new { icf.InferenceId });
        nestedBuilder.HasOne(iac => iac.Symbol)
                     .WithMany(s => s.InferenceConclusionFormulas)
                     .HasPrincipalKey(s => new { s.Id })
                     .HasForeignKey(iac => new { iac.SymbolId });
        nestedBuilder.HasOne(icf => icf.BoundInferenceArgument)
                     .WithMany(iag => iag.InferenceConclusionFormulasToBound)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       icf => new { icf.InferenceId, icf.BoundInferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(icf => icf.InferenceArgument)
                     .WithMany(iag => iag.InferenceConclusionFormulas)
                     .HasPrincipalKey(
                       icf => new { icf.InferenceId, icf.SerialNo }
                     )
                     .HasForeignKey(
                       iag => new { iag.InferenceId, iag.InferenceArgumentSerialNo }
                     );
        nestedBuilder.HasOne(icf => icf.SubstitutionInferenceArgumentFrom)
                     .WithMany(iag => iag.InferenceConclusionFormulasToSubstitutionInferenceArgumentFrom)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       icf => new { icf.InferenceId, icf.SubstitutionInferenceArgumentFromSerialNo }
                     );
        nestedBuilder.HasOne(icf => icf.SubstitutionInferenceArgumentTo)
                     .WithMany(iag => iag.InferenceConclusionFormulasToSubstitutionInferenceArgumentTo)
                     .HasPrincipalKey(
                       iag => new { iag.InferenceId, iag.SerialNo }
                     )
                     .HasForeignKey(
                       icf => new { icf.InferenceId, icf.SubstitutionInferenceArgumentToSerialNo }
                     );
        nestedBuilder.HasOne(ict => ict.Formula)
                     .WithMany(f => f.InferenceConclusionFormulas)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(ict => new { ict.FormulaId });
      }
    );
    modelBuilder.Entity<Proof>(
      nestedBuilder => {
        nestedBuilder.HasKey(p => new { p.TheoremId, p.SerialNo });
        nestedBuilder.HasOne(p => p.Theorem)
                     .WithMany(t => t.Proofs)
                     .HasPrincipalKey(t => new { t.Id })
                     .HasForeignKey(p => new { p.TheoremId });
      }
    );
    modelBuilder.Entity<ProofInference>(
      nestedBuilder => {
        nestedBuilder.HasKey(pi => new { pi.TheoremId, pi.ProofSerialNo, pi.SerialNo });
        nestedBuilder.HasOne(pi => pi.Proof)
                     .WithMany(p => p.ProofInferences)
                     .HasPrincipalKey(p => new { p.TheoremId, p.SerialNo })
                     .HasForeignKey(pi => new { pi.TheoremId, pi.ProofSerialNo });
        nestedBuilder.HasOne(pi => pi.Inference)
                     .WithMany(i => i.ProofInferences)
                     .HasPrincipalKey(i => new { i.Id })
                     .HasForeignKey(pi => new { pi.InferenceId });
        nestedBuilder.HasOne(pi => pi.ConclusionFormula)
                     .WithMany(f => f.ProofInferences)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(pi => new { pi.ConclusionFormulaId });
        nestedBuilder.HasMany(ppi => ppi.PreviousProofInferences)
                     .WithOne(npi => npi.NextProofInference)
                     .HasPrincipalKey(npi => new { npi.TheoremId, npi.ProofSerialNo, npi.SerialNo })
                     .HasForeignKey(ppi => new { ppi.TheoremId, ppi.ProofSerialNo, ppi.NextProofInferenceSerialNo });
      }
    );
    modelBuilder.Entity<ProofInferenceArgument>(
      nestedBuilder => {
        nestedBuilder.HasKey(pia => new { pia.TheoremId, pia.ProofSerialNo, pia.ProofInferenceSerialNo, pia.SerialNo });
        nestedBuilder.HasOne(pia => pia.ProofInference)
                     .WithMany(pi => pi.ProofInferenceArguments)
                     .HasPrincipalKey(pi => new { pi.TheoremId, pi.ProofSerialNo, pi.SerialNo })
                     .HasForeignKey(pia => new { pia.TheoremId, pia.ProofSerialNo, pia.ProofInferenceSerialNo });
        nestedBuilder.HasOne(pia => pia.AxiomProposition)
                     .WithMany(ap => ap.ProofArguments)
                     .HasPrincipalKey(ap => new { ap.AxiomId, ap.SerialNo })
                     .HasForeignKey(pa => new { pa.AxiomId, pa.AxiomPropositionSerialNo });
        nestedBuilder.HasOne(pia => pia.TheoremAssumption)
                     .WithMany(ta => ta.ProofInferenceArguments)
                     .HasPrincipalKey(ta => new { ta.TheoremId, ta.SerialNo })
                     .HasForeignKey(pia => new { pia.TheoremAssumptionTheoremId, pia.TheoremAssumptionSerialNo });
        nestedBuilder.HasOne(pia => pia.Formula)
                     .WithMany(f => f.ProofInferenceArguments)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(pia => new { pia.FormulaId });
      }
    );
    modelBuilder.Entity<ProofAssumption>(
      nestedBuilder => {
        nestedBuilder.HasKey(pa => new { pa.TheoremId, pa.ProofSerialNo, pa.SerialNo });
        nestedBuilder.HasOne(pa => pa.Proof)
                     .WithMany(p => p.ProofAssumptions)
                     .HasPrincipalKey(p => new { p.TheoremId, p.SerialNo })
                     .HasForeignKey(pa => new { pa.TheoremId, pa.ProofSerialNo });
        nestedBuilder.HasOne(pa => pa.Formula)
                     .WithMany(f => f.ProofAssumptions)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(pa => new { pa.FormulaId });
        nestedBuilder.HasOne(pa => pa.AddedProofInference)
                     .WithOne(pi => pi.AddingProofInference)
                     .HasPrincipalKey<ProofInference>(pi => new { pi.TheoremId, pi.ProofSerialNo, pi.SerialNo })
                     .HasForeignKey<ProofAssumption>(pa => new { pa.TheoremId, pa.ProofSerialNo, pa.AddedProofInferenceSerialNo });
        nestedBuilder.HasOne(pa => pa.LastUsedProofInference)
                     .WithOne(pi => pi.LastUsingProofInference)
                     .HasPrincipalKey<ProofInference>(pi => new { pi.TheoremId, pi.ProofSerialNo, pi.SerialNo })
                     .HasForeignKey<ProofAssumption>(pa => new { pa.TheoremId, pa.ProofSerialNo, pa.LastUsedProofInferenceSerialNo });
        nestedBuilder.HasOne(pa => pa.DissolutedProofInference)
                     .WithOne(pi => pi.DissolutingAssumption)
                     .HasPrincipalKey<ProofInference>(pi => new { pi.TheoremId, pi.ProofSerialNo, pi.SerialNo })
                     .HasForeignKey<ProofAssumption>(pa => new { pa.TheoremId, pa.ProofSerialNo, pa.DissolutedProofInferenceSerialNo });
      }
    );
    modelBuilder.Entity<Symbol>(
      nestedBuilder => {
        nestedBuilder.HasOne(s => s.SymbolType)
                     .WithMany(st => st.Symbols)
                     .HasPrincipalKey(st => new { st.Id })
                     .HasForeignKey(s => new { s.SymbolTypeId });
        nestedBuilder.HasOne(s => s.ArityFormulaType)
                     .WithMany(f => f.Symbols)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(s => new { s.ArityFormulaTypeId });
        nestedBuilder.HasOne(s => s.ArityFormulaType)
                     .WithMany(ft => ft.Symbols)
                     .HasPrincipalKey(ft => new { ft.Id })
                     .HasForeignKey(s => new { s.ArityFormulaTypeId });
      }
    );
    modelBuilder.Entity<SymbolType>(
      nestedBuilder => {
        nestedBuilder.HasOne(st => st.FormulaType)
                     .WithMany(ft => ft.SymbolTypes)
                     .HasPrincipalKey(ft => new { ft.Id })
                     .HasForeignKey(st => new { st.FormulaTypeId });
      }
    );
    modelBuilder.Entity<TheoremAssumption>(
      nestedBuilder => {
        nestedBuilder.HasKey(ta => new { ta.TheoremId, ta.SerialNo });
        nestedBuilder.HasOne(ta => ta.Theorem)
                     .WithMany(t => t.TheoremAssumptions)
                     .HasPrincipalKey(t => new { t.Id })
                     .HasForeignKey(ta => new { ta.TheoremId });
        nestedBuilder.HasOne(ta => ta.Formula)
                     .WithMany(f => f.TheoremAssumptions)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(ta => new { ta.FormulaId });
      }
    );
    modelBuilder.Entity<TheoremConclusion>(
      nestedBuilder => {
        nestedBuilder.HasKey(tc => new { tc.TheoremId, tc.SerialNo });
        nestedBuilder.HasOne(tc => tc.Theorem)
                     .WithMany(t => t.TheoremConclusions)
                     .HasPrincipalKey(t => new { t.Id })
                     .HasForeignKey(tc => new { tc.TheoremId });
        nestedBuilder.HasOne(tc => tc.Formula)
                     .WithMany(f => f.TheoremConclusions)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(tc => new { tc.FormulaId });
      }
    );

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