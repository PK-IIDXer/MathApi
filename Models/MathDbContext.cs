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

  public DbSet<Axiom> Axioms { get; set; } = null!;
  public DbSet<AxiomProposition> AxiomPropositions { get; set; } = null!;
  public DbSet<Formula> Formulas { get; set; } = null!;
  public DbSet<FormulaChain> FormulaChains { get; set;} = null!;
  public DbSet<FormulaLabel> FormulaLabels { get; set;} = null!;
  public DbSet<FormulaLabelType> FormulaLabelTypes { get; set;} = null!;
  public DbSet<FormulaString> FormulaStrings { get; set; } = null!;
  public DbSet<FormulaStruct> FormulaStructs { get; set; } = null!;
  public DbSet<FormulaStructArgument> FormulaStructArguments { get; set; } = null!;
  public DbSet<FormulaStructString> FormulaStructStrings { get; set; } = null!;
  public DbSet<FormulaStructStringSubstitution> FormulaStructStringSubstitutions { get; set; } = null!;
  public DbSet<FormulaType> FormulaTypes { get; set; } = null!;
  public DbSet<Inference> Inferences { get; set; } = null!;
  public DbSet<InferenceArgument> InferenceArguments { get; set; } = null!;
  public DbSet<InferenceArgumentConstraint> InferenceArgumentConstraints{ get; set; } = null!;
  public DbSet<InferenceAssumption> InferenceAssumptions { get; set; } = null!;
  public DbSet<InferenceAssumptionDissolutableAssumption> InferenceAssumptionDissolutableAssumptions { get; set; } = null!;
  public DbSet<InferenceConclusion> InferenceConclusions { get; set; } = null!;
  public DbSet<InferenceFormulaStructArgumentMapping> InferenceFormulaStructArgumentMappings { get; set; } = null!;
  public DbSet<Proof> Proofs { get; set; } = null!;
  public DbSet<ProofAssumption> ProofAssumptions { get; set; } = null!;
  public DbSet<ProofInference> ProofInferences { get; set; } = null!;
  public DbSet<ProofInferenceArgument> ProofInferenceArguments { get; set; } = null!;
  public DbSet<Symbol> Symbols { get; set; } = null!;
  public DbSet<SymbolType> SymbolTypes { get; set; } = null!;
  public DbSet<Theorem> Theorems { get; set; } = null!;
  public DbSet<TheoremAssumption> TheoremAssumptions { get; set; } = null!;
  public DbSet<TheoremConclusion> TheoremConclusions { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Axiom>().Navigation(a => a.AxiomPropositions).AutoInclude();
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
        nestedBuilder.Navigation(ap => ap.Axiom).AutoInclude();
        nestedBuilder.Navigation(ap => ap.Formula).AutoInclude();
      }
    );
    modelBuilder.Entity<Formula>(
      nestedBuilder => {
        nestedBuilder.Navigation(f => f.FormulaStrings).AutoInclude();
        nestedBuilder.Navigation(f => f.FormulaChains).AutoInclude();
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
        nestedBuilder.Navigation(fc => fc.Formula).AutoInclude();
        nestedBuilder.Navigation(fc => fc.FromFormulaString).AutoInclude();
        nestedBuilder.Navigation(fc => fc.ToFormulaString).AutoInclude();
      }
    );
    modelBuilder.Entity<FormulaLabel>(
      nestedBuilder => {
        nestedBuilder.HasKey(fl => new { fl.Id });
        nestedBuilder.HasOne(fl => fl.Type)
                     .WithMany(flt => flt.FormulaLabels)
                     .HasPrincipalKey(flt => new { flt.Id })
                     .HasForeignKey(fl => new { fl.TypeId });
        nestedBuilder.Navigation(fl => fl.Type);
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
        nestedBuilder.Navigation(fs => fs.Formula).AutoInclude();
        nestedBuilder.Navigation(fs => fs.Symbol).AutoInclude();
      }
    );
    modelBuilder.Entity<FormulaStruct>(
      nestedBuilder => {
        nestedBuilder.Navigation(fs => fs.Arguments).AutoInclude();
        nestedBuilder.Navigation(fs => fs.Strings).AutoInclude();
      }
    );
    modelBuilder.Entity<FormulaStructArgument>(
      nestedBuilder => {
        nestedBuilder.HasKey(fsa => new { fsa.FormulaStructId, fsa.SerialNo });
        nestedBuilder.HasOne(fsa => fsa.FormulaStruct)
                     .WithMany(fs => fs.Arguments)
                     .HasPrincipalKey(fs => new { fs.Id })
                     .HasForeignKey(fsa => new { fsa.FormulaStructId });
        nestedBuilder.HasOne(fsa => fsa.Label)
                     .WithMany(l => l.FormulaStructArguments)
                     .HasPrincipalKey(l => new { l.Id })
                     .HasForeignKey(fsa => new { fsa.LabelId });
        nestedBuilder.Navigation(fsa => fsa.FormulaStruct).AutoInclude();
        nestedBuilder.Navigation(fsa => fsa.Label).AutoInclude();
      }
    );
    modelBuilder.Entity<FormulaStructString>(
      nestedBuilder => {
        nestedBuilder.HasKey(fss => new { fss.FormulaStructId, fss.SerialNo });
        nestedBuilder.HasOne(fss => fss.FormulaStruct)
                     .WithMany(fs => fs.Strings)
                     .HasPrincipalKey(fs => new { fs.Id })
                     .HasForeignKey(fss => new { fss.FormulaStructId });
        nestedBuilder.HasOne(fss => fss.Symbol)
                     .WithMany(s => s.FormulaStructStrings)
                     .HasPrincipalKey(s => new { s.Id })
                     .HasForeignKey(fss => new { fss.SymbolId });
        nestedBuilder.HasOne(fss => fss.BoundArgument)
                     .WithMany(fsa => fsa.StringsToBoundArgument)
                     .HasPrincipalKey(fsa => new { fsa.FormulaStructId, fsa.SerialNo })
                     .HasForeignKey(fss => new { fss.FormulaStructId, fss.BoundArgumentSerialNo });
        nestedBuilder.HasOne(fss => fss.Argument)
                     .WithMany(fsa => fsa.Strings)
                     .HasPrincipalKey(fsa => new { fsa.FormulaStructId, fsa.SerialNo })
                     .HasForeignKey(fss => new { fss.FormulaStructId, fss.ArgumentSerialNo });
        nestedBuilder.Navigation(fss => fss.FormulaStruct).AutoInclude();
        nestedBuilder.Navigation(fss => fss.Symbol).AutoInclude();
        nestedBuilder.Navigation(fss => fss.BoundArgument).AutoInclude();
        nestedBuilder.Navigation(fss => fss.Argument).AutoInclude();
        nestedBuilder.Navigation(fss => fss.Substitutions).AutoInclude();
      }
    );
    modelBuilder.Entity<FormulaStructStringSubstitution>(
      nestedBuilder => {
        nestedBuilder.HasKey(fsss => new { fsss.FormulaStructId, fsss.FormulaStructStringSerialNo, fsss.SerialNo });
        nestedBuilder.HasOne(fsss => fsss.FormulaStructString)
                     .WithMany(fss => fss.Substitutions)
                     .HasPrincipalKey(fss => new { fss.FormulaStructId, fss.SerialNo })
                     .HasForeignKey(fsss => new { fsss.FormulaStructId, fsss.FormulaStructStringSerialNo });
        nestedBuilder.HasOne(fsss => fsss.ArgumentFrom)
                     .WithMany(fsa => fsa.StringsToSubstitutionArgumentFrom)
                     .HasPrincipalKey(fsa => new { fsa.FormulaStructId, fsa.SerialNo })
                     .HasForeignKey(fss => new { fss.FormulaStructId, fss.ArgumentFromSerialNo });
        nestedBuilder.HasOne(fss => fss.ArgumentTo)
                     .WithMany(fsa => fsa.StringsToSubstitutionArgumentTo)
                     .HasPrincipalKey(fsa => new { fsa.FormulaStructId, fsa.SerialNo })
                     .HasForeignKey(fss => new { fss.FormulaStructId, fss.ArgumentToSerialNo });
        nestedBuilder.Navigation(fsss => fsss.FormulaStructString).AutoInclude();
        nestedBuilder.Navigation(fsss => fsss.ArgumentFrom).AutoInclude();
        nestedBuilder.Navigation(fsss => fsss.ArgumentTo).AutoInclude();
      }
    );
    modelBuilder.Entity<Inference>(
      nestedBuilder => {
        nestedBuilder.Navigation(i => i.Arguments).AutoInclude();
        nestedBuilder.Navigation(i => i.Assumptions).AutoInclude();
        nestedBuilder.Navigation(i => i.Conclusions).AutoInclude();
        nestedBuilder.Navigation(i => i.FormulaStructArgumentMappings).AutoInclude();
      }
    );
    modelBuilder.Entity<InferenceArgument>(
      nestedBuilder => {
        nestedBuilder.HasKey(ia => new { ia.InferenceId, ia.SerialNo });
        nestedBuilder.HasOne(ia => ia.Inference)
                     .WithMany(i => i.Arguments)
                     .HasPrincipalKey(i => new { i.Id })
                     .HasForeignKey(ia => new { ia.InferenceId });
        nestedBuilder.HasOne(ia => ia.FormulaLabel)
                     .WithOne(fl => fl.InferenceArgument)
                     .HasPrincipalKey<FormulaLabel>(fl => new { fl.Id })
                     .HasForeignKey<InferenceArgument>(ia => new { ia.FormulaLabelId });
        nestedBuilder.Navigation(ia => ia.Inference).AutoInclude();
        nestedBuilder.Navigation(ia => ia.FormulaLabel).AutoInclude();
        nestedBuilder.Navigation(ia => ia.InferenceArgumentConstraints).AutoInclude();
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
                     .WithMany(ia => ia.InferenceArgumentConstraintDestinations)
                     .HasPrincipalKey(ia => new { ia.InferenceId, ia.SerialNo })
                     .HasForeignKey(iac => new { iac.InferenceId, iac.ConstraintDestinationInferenceArgumentSerialNo });
        nestedBuilder.Navigation(iac => iac.InferenceArgument).AutoInclude();
        nestedBuilder.Navigation(iac => iac.ConstraintDestinationInferenceArgument).AutoInclude();
      }
    );
    modelBuilder.Entity<InferenceAssumption>(
      nestedBuilder => {
        nestedBuilder.HasKey(ia => new { ia.InferenceId, ia.SerialNo });
        nestedBuilder.HasOne(ia => ia.Inference)
                     .WithMany(i => i.Assumptions)
                     .HasPrincipalKey(i => new { i.Id })
                     .HasForeignKey(ia => new { ia.InferenceId });
        nestedBuilder.HasOne(ia => ia.FormulaStruct)
                     .WithMany(fs => fs.InferenceAssumptions)
                     .HasPrincipalKey(fs => new { fs.Id })
                     .HasForeignKey(ia => new { ia.FormulaStructId });
        nestedBuilder.HasMany(ia => ia.FormulaStructArgumentMappings)
                     .WithOne(fsam => fsam.InferenceAssumption)
                     .HasPrincipalKey(ia => new { ia.InferenceId, ia.FormulaStructArgumentMappingSerialNo })
                     .HasForeignKey(fsam => new { fsam.InferenceId, fsam.SerialNo });
        nestedBuilder.Navigation(ia => ia.Inference).AutoInclude();
        nestedBuilder.Navigation(ia => ia.FormulaStruct).AutoInclude();
        nestedBuilder.Navigation(ia => ia.FormulaStructArgumentMappings).AutoInclude();
        nestedBuilder.Navigation(ia => ia.DissolutableAssumption).AutoInclude();
      }
    );
    modelBuilder.Entity<InferenceAssumptionDissolutableAssumption>(
      nestedBuilder => {
        nestedBuilder.HasKey(iada => new { iada.InferenceId, iada.InferenceAssumptionSerialNo });
        nestedBuilder.HasOne(iada => iada.InferenceAssumption)
                     .WithOne(ia => ia.DissolutableAssumption)
                     .HasPrincipalKey<InferenceAssumption>(ia => new { ia.InferenceId, ia.SerialNo })
                     .HasForeignKey<InferenceAssumptionDissolutableAssumption>(iada => new { iada.InferenceId, iada.InferenceAssumptionSerialNo });
        nestedBuilder.HasOne(iada => iada.FormulaStruct)
                     .WithMany(fs => fs.InferenceAssumptionDissolutableAssumptions)
                     .HasPrincipalKey(fs => new { fs.Id })
                     .HasForeignKey(iada => new { iada.FormulaStructId });
        nestedBuilder.HasMany(iada => iada.FormulaStructArgumentMappings)
                     .WithOne(fsam => fsam.InferenceAssumptionDissolutableAssumption)
                     .HasPrincipalKey(iada => new { iada.InferenceId, iada.FormulaStructArgumentMappingSerialNo })
                     .HasForeignKey(fsam => new { fsam.InferenceId, fsam.SerialNo });
        nestedBuilder.Navigation(iada => iada.InferenceAssumption).AutoInclude();
        nestedBuilder.Navigation(iada => iada.FormulaStruct).AutoInclude();
        nestedBuilder.Navigation(iada => iada.FormulaStructArgumentMappings).AutoInclude();
      }
    );
    modelBuilder.Entity<InferenceConclusion>(
      nestedBuilder => {
        nestedBuilder.HasKey(ic => new { ic.InferenceId });
        nestedBuilder.HasOne(ic => ic.Inference)
                     .WithMany(i => i.Conclusions)
                     .HasPrincipalKey(i => new { i.Id })
                     .HasForeignKey(ic => new { ic.InferenceId });
        nestedBuilder.HasOne(ic => ic.FormulaStruct)
                     .WithMany(fs => fs.InferenceConclusions)
                     .HasPrincipalKey(fs => new { fs.Id })
                     .HasForeignKey(ic => new { ic.FormulaStructId });
        nestedBuilder.HasMany(ic => ic.FormulaStructArgumentMappings)
                     .WithOne(fsam => fsam.InferenceConclusion)
                     .HasForeignKey(fsam => new { fsam.InferenceId, fsam.SerialNo })
                     .HasPrincipalKey(ic => new { ic.InferenceId, ic.FormulaStructArgumentMappingSerialNo });
        nestedBuilder.Navigation(ic => ic.Inference).AutoInclude();
        nestedBuilder.Navigation(ic => ic.FormulaStruct).AutoInclude();
        nestedBuilder.Navigation(ic => ic.FormulaStructArgumentMappings).AutoInclude();
      }
    );
    modelBuilder.Entity<InferenceFormulaStructArgumentMapping>(
      nestedBuilder => {
        nestedBuilder.HasKey(ifsam => new { ifsam.InferenceId, ifsam.SerialNo });
        nestedBuilder.HasOne(ifsam => ifsam.Inference)
                     .WithMany(i => i.FormulaStructArgumentMappings)
                     .HasPrincipalKey(i => new { i.Id })
                     .HasForeignKey(ifsam => new { ifsam.InferenceId });
        nestedBuilder.HasOne(ifsam => ifsam.FormulaStructArgument)
                     .WithMany(fsa => fsa.InferenceFormulaStructArgumentMappings)
                     .HasPrincipalKey(fsa => new { fsa.FormulaStructId, fsa.SerialNo })
                     .HasForeignKey(ifsam => new { ifsam.FormulaStructId, ifsam.FormulaStructArgumentSerialNo });
        nestedBuilder.HasOne(ifsam => ifsam.InferenceArgument)
                     .WithMany(ia => ia.InferenceFormulaStructArgumentMappings)
                     .HasPrincipalKey(ia => new { ia.InferenceId, ia.SerialNo })
                     .HasForeignKey(ifsam => new { ifsam.InferenceId, ifsam.InferenceArgumentSerialNo });
        nestedBuilder.Navigation(ifsam => ifsam.Inference).AutoInclude();
        nestedBuilder.Navigation(ifsam => ifsam.FormulaStructArgument).AutoInclude();
        nestedBuilder.Navigation(ifsam => ifsam.InferenceArgument).AutoInclude();
      }
    );
    modelBuilder.Entity<Proof>(
      nestedBuilder => {
        nestedBuilder.HasKey(p => new { p.TheoremId, p.SerialNo });
        nestedBuilder.HasOne(p => p.Theorem)
                     .WithMany(t => t.Proofs)
                     .HasPrincipalKey(t => new { t.Id })
                     .HasForeignKey(p => new { p.TheoremId });
        nestedBuilder.Navigation(p => p.Theorem).AutoInclude();
        nestedBuilder.Navigation(p => p.Inferences).AutoInclude();
        nestedBuilder.Navigation(p => p.Assumptions).AutoInclude();
      }
    );
    modelBuilder.Entity<ProofAssumption>(
      nestedBuilder => {
        nestedBuilder.HasKey(pa => new { pa.TheoremId, pa.ProofSerialNo, pa.SerialNo });
        nestedBuilder.HasOne(pa => pa.Proof)
                     .WithMany(p => p.Assumptions)
                     .HasPrincipalKey(p => new { p.TheoremId, p.SerialNo })
                     .HasForeignKey(pa => new { pa.TheoremId, pa.ProofSerialNo });
        nestedBuilder.HasOne(pa => pa.AddedProofInference)
                     .WithOne(pi => pi.AddingProofInference)
                     .HasPrincipalKey<ProofInference>(pi => new { pi.TheoremId, pi.ProofSerialNo, pi.SerialNo })
                     .HasForeignKey<ProofAssumption>(pa => new { pa.TheoremId, pa.ProofSerialNo, pa.AddedProofInferenceSerialNo });
        nestedBuilder.HasOne(pa => pa.DissolutedProofInference)
                     .WithOne(pi => pi.DissolutingAssumption)
                     .HasPrincipalKey<ProofInference>(pi => new { pi.TheoremId, pi.ProofSerialNo, pi.SerialNo })
                     .HasForeignKey<ProofAssumption>(pa => new { pa.TheoremId, pa.ProofSerialNo, pa.DissolutedProofInferenceSerialNo });
        nestedBuilder.Navigation(pa => pa.Proof).AutoInclude();
        nestedBuilder.Navigation(pa => pa.AddedProofInference).AutoInclude();
        nestedBuilder.Navigation(pa => pa.DissolutedProofInference).AutoInclude();
      }
    );
    modelBuilder.Entity<ProofInference>(
      nestedBuilder => {
        nestedBuilder.HasKey(pi => new { pi.TheoremId, pi.ProofSerialNo, pi.SerialNo });
        nestedBuilder.HasOne(pi => pi.Proof)
                     .WithMany(p => p.Inferences)
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
        nestedBuilder.HasOne(pi => pi.ConclusionFormulaStruct)
                     .WithMany(fs => fs.ProofInferences)
                     .HasPrincipalKey(fs => new { fs.Id })
                     .HasForeignKey(pi => new { pi.ConclusionFormulaStructId });
        nestedBuilder.HasOne(pi => pi.ProofAssumption)
                     .WithMany(pa => pa.ProofInferences)
                     .HasPrincipalKey(pa => new { pa.TheoremId, pa.ProofSerialNo, pa.SerialNo })
                     .HasForeignKey(pi => new { pi.TheoremId, pi.ProofSerialNo, pi.ProofAssumptionSerialNo });
        nestedBuilder.Navigation(pi => pi.Proof).AutoInclude();
        nestedBuilder.Navigation(pi => pi.Inference).AutoInclude();
        nestedBuilder.Navigation(pi => pi.ConclusionFormula).AutoInclude();
        nestedBuilder.Navigation(pi => pi.ConclusionFormulaStruct).AutoInclude();
        nestedBuilder.Navigation(pi => pi.ProofInferenceArguments).AutoInclude();
        nestedBuilder.Navigation(pi => pi.ProofAssumption).AutoInclude();
      }
    );
    modelBuilder.Entity<ProofInferenceArgument>(
      nestedBuilder => {
        nestedBuilder.HasKey(pia => new { pia.TheoremId, pia.ProofSerialNo, pia.ProofInferenceSerialNo, pia.SerialNo });
        nestedBuilder.HasOne(pia => pia.ProofInference)
                     .WithMany(pi => pi.ProofInferenceArguments)
                     .HasPrincipalKey(pi => new { pi.TheoremId, pi.ProofSerialNo, pi.SerialNo })
                     .HasForeignKey(pia => new { pia.TheoremId, pia.ProofSerialNo, pia.ProofInferenceSerialNo });
        nestedBuilder.HasOne(pia => pia.Formula)
                     .WithMany(f => f.ProofInferenceArguments)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(pia => new { pia.FormulaId });
        nestedBuilder.HasOne(pia => pia.FormulaStruct)
                     .WithMany(fs => fs.ProofInferenceArguments)
                     .HasPrincipalKey(fs => new { fs.Id })
                     .HasForeignKey(pia => new { pia.FormulaStructId });
        nestedBuilder.Navigation(pia => pia.ProofInference).AutoInclude();
        nestedBuilder.Navigation(pia => pia.Formula).AutoInclude();
        nestedBuilder.Navigation(pia => pia.FormulaStruct).AutoInclude();
      }
    );
    modelBuilder.Entity<Symbol>(
      nestedBuilder => {
        nestedBuilder.HasOne(s => s.Type)
                     .WithMany(st => st.Symbols)
                     .HasPrincipalKey(st => new { st.Id })
                     .HasForeignKey(s => new { s.TypeId });
        nestedBuilder.HasOne(s => s.ArityFormulaType)
                     .WithMany(f => f.Symbols)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(s => new { s.ArityFormulaTypeId });
        nestedBuilder.Navigation(s => s.Type).AutoInclude();
        nestedBuilder.Navigation(s => s.ArityFormulaType).AutoInclude();
      }
    );
    modelBuilder.Entity<SymbolType>(
      nestedBuilder => {
        nestedBuilder.HasOne(st => st.FormulaType)
                     .WithMany(ft => ft.SymbolTypes)
                     .HasPrincipalKey(ft => new { ft.Id })
                     .HasForeignKey(st => new { st.FormulaTypeId });
        nestedBuilder.Navigation(st => st.FormulaType).AutoInclude();
      }
    );
    modelBuilder.Entity<Theorem>(
      nestedBuilder => {
        nestedBuilder.HasOne(t => t.Inference)
                     .WithOne(i => i.Theorem)
                     .HasPrincipalKey<Inference>(i => i.TheoremId)
                     .HasForeignKey<Theorem>(t => t.InferenceId);
        nestedBuilder.Navigation(t => t.Assumptions).AutoInclude();
        nestedBuilder.Navigation(t => t.Conclusions).AutoInclude();
        nestedBuilder.Navigation(t => t.Inference).AutoInclude();
      }
    );
    modelBuilder.Entity<TheoremAssumption>(
      nestedBuilder => {
        nestedBuilder.HasKey(ta => new { ta.TheoremId, ta.SerialNo });
        nestedBuilder.HasOne(ta => ta.Theorem)
                     .WithMany(t => t.Assumptions)
                     .HasPrincipalKey(t => new { t.Id })
                     .HasForeignKey(ta => new { ta.TheoremId });
        nestedBuilder.HasOne(ta => ta.Formula)
                     .WithMany(f => f.TheoremAssumptions)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(ta => new { ta.FormulaId });
        nestedBuilder.Navigation(ta => ta.Theorem).AutoInclude();
        nestedBuilder.Navigation(ta => ta.Formula).AutoInclude();
      }
    );
    modelBuilder.Entity<TheoremConclusion>(
      nestedBuilder => {
        nestedBuilder.HasKey(tc => new { tc.TheoremId, tc.SerialNo });
        nestedBuilder.HasOne(tc => tc.Theorem)
                     .WithMany(t => t.Conclusions)
                     .HasPrincipalKey(t => new { t.Id })
                     .HasForeignKey(tc => new { tc.TheoremId });
        nestedBuilder.HasOne(tc => tc.Formula)
                     .WithMany(f => f.TheoremConclusions)
                     .HasPrincipalKey(f => new { f.Id })
                     .HasForeignKey(tc => new { tc.FormulaId });
        nestedBuilder.Navigation(tc => tc.Theorem).AutoInclude();
        nestedBuilder.Navigation(tc => tc.Formula).AutoInclude();
      }
    );

    modelBuilder.Entity<FormulaType>().HasData(
      new { Id = Const.FormulaTypeEnum.Term, Name = "Term" },
      new { Id = Const.FormulaTypeEnum.Proposition, Name = "Proposition" }
    );

    modelBuilder.Entity<SymbolType>().HasData(
      new { Id = Const.SymbolTypeEnum.FreeVariable, Name = "free variable", FormulaTypeId = Const.FormulaTypeEnum.Term, IsQuantifier = false },
      new { Id = Const.SymbolTypeEnum.BoundVariable, Name = "bound variable", FormulaTypeId = Const.FormulaTypeEnum.Term, IsQuantifier = false },
      new { Id = Const.SymbolTypeEnum.Function, Name = "function", FormulaTypeId = Const.FormulaTypeEnum.Term, IsQuantifier = false },
      new { Id = Const.SymbolTypeEnum.Predicate, Name = "predicate", FormulaTypeId = Const.FormulaTypeEnum.Proposition, IsQuantifier = false },
      new { Id = Const.SymbolTypeEnum.Logic, Name = "logic", FormulaTypeId = Const.FormulaTypeEnum.Proposition, IsQuantifier = false },
      new { Id = Const.SymbolTypeEnum.TermQuantifier, Name = "term quantifier", FormulaTypeId = Const.FormulaTypeEnum.Term, IsQuantifier = true },
      new { Id = Const.SymbolTypeEnum.PropositionQuantifier, Name = "proposition quantifier", FormulaTypeId = Const.FormulaTypeEnum.Proposition, IsQuantifier = true }
    );

    modelBuilder.Entity<FormulaLabelType>().HasData(
      new { Id = Const.FormulaLabelTypeEnum.Term, Name = "Term" },
      new { Id = Const.FormulaLabelTypeEnum.Proposition, Name = "Proposition" },
      new { Id = Const.FormulaLabelTypeEnum.FreeVariable, Name = "Free Variable" }
    );
  }
}