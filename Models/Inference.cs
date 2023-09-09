using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MathApi.Models;

/// <summary>
/// 推論規則
/// </summary>
public class Inference
{
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long Id { get; set; }
  /// <summary>
  /// 推論規則名称
  /// </summary>
  public string Name { get; set;} = "";

  public List<InferenceArgument> Arguments { get; set; } = new();
  public List<InferenceAssumption> Assumptions { get; set; } = new();
  public List<InferenceConclusion> Conclusions { get; set; } = new();
  public List<InferenceFormulaStructArgumentMapping> FormulaStructArgumentMappings { get; set; } = new();

  public List<ProofInference>? ProofInferences { get; }

  public Theorem? Theorem { get; }
  public long? TheoremId { get; set; }

  public struct InferenceResult
  {
    public List<AssumptionResult> AssumptionFormulas { get; set; }
    public List<Formula> ConclusionFormulas { get; set; }
    public struct AssumptionResult
    {
      public Formula Formula { get; set; }
      public Formula? Dissolutable { get; set; }
      public bool IsDissoluteForce { get; set; }
    }
  }

  public struct InferenceStructResult
  {
    public List<AssumptionStructResult> AssumptionFormulaStructs { get; set; }
    public List<FormulaStruct> ConclusionFormulaStructs { get; set; }
    public struct AssumptionStructResult
    {
      public FormulaStruct FormulaStruct { get; set; }
      public FormulaStruct? DissolutableStruct { get; set; }
      public bool IsDissoluteForce { get; set; }
    }
  }

  public InferenceResult Apply(List<Formula> args)
  {
    var result = new InferenceResult();

    foreach (var asmp in Assumptions)
    {
      var assumption = new InferenceResult.AssumptionResult();

      if (asmp.FormulaStruct == null)
        throw new ArgumentException("Include Inference.Assumptions.FormulaStruct");

      var asmpArg = new List<Formula>();
      var mappings = asmp.FormulaStructArgumentMappings
                         .Where(fsam => fsam.FormulaStructId == asmp.FormulaStructId)
                         .OrderBy(fsam => fsam.FormulaStructArgumentSerialNo);
      foreach (var mapping in mappings)
      {
        asmpArg.Add(args[mapping.InferenceArgumentSerialNo]);
      }
      assumption.Formula = asmp.FormulaStruct.Apply(asmpArg);

      if (asmp.DissolutableAssumption == null)
      {
        result.AssumptionFormulas.Add(assumption);
        continue;
      }

      var disArg = new List<Formula>();
      var disMappings = asmp.DissolutableAssumption.FormulaStructArgumentMappings
                            .Where(fsam => fsam.FormulaStructId == asmp.DissolutableAssumption.FormulaStructId)
                            .OrderBy(fsam => fsam.FormulaStructArgumentSerialNo);
      foreach (var mapping in disMappings)
      {
        disArg.Add(args[mapping.InferenceArgumentSerialNo]);
      }
      if (asmp.DissolutableAssumption.FormulaStruct == null)
        throw new ArgumentException("Include Inference.Assumptions.DissolutableAssumption.FormulaStruct");
      assumption.Dissolutable = asmp.DissolutableAssumption.FormulaStruct.Apply(asmpArg);
      assumption.IsDissoluteForce = asmp.DissolutableAssumption.IsForce;
      // TODO: 不要？
      // if (asmp.DissolutableAssumption.IsForce)
      // {
      //   if (assumption.Dissolutable == null)
      //     throw new ArgumentException("There is no Dissolutable in spite of IsForce");
      // }

      result.AssumptionFormulas.Add(assumption);
    }

    foreach (var con in Conclusions)
    {
      var conArgs = new List<Formula>();
      var conMappings = con.FormulaStructArgumentMappings
                           .Where(fsam => fsam.FormulaStructId == con.FormulaStructId)
                           .OrderBy(fsam => fsam.FormulaStructArgumentSerialNo);
      foreach (var mapping in conMappings)
      {
        conArgs.Add(args[mapping.InferenceArgumentSerialNo]);
      }
      if (con.FormulaStruct == null)
        throw new ArgumentException("Include Inference.Conclusions.FormulaStruct");

      result.ConclusionFormulas.Add(con.FormulaStruct.Apply(conArgs));
    }

    return result;
  }

  public InferenceStructResult Apply(List<FormulaStruct> args)
  {
    var result = new InferenceStructResult();

    foreach (var asmp in Assumptions)
    {
      var assumption = new InferenceStructResult.AssumptionStructResult();

      if (asmp.FormulaStruct == null)
        throw new ArgumentException("Include Inference.Assumptions.FormulaStruct");

      var asmpArg = new List<FormulaStruct>();
      var mappings = asmp.FormulaStructArgumentMappings
                         .Where(fsam => fsam.FormulaStructId == asmp.FormulaStructId)
                         .OrderBy(fsam => fsam.FormulaStructArgumentSerialNo);
      foreach (var mapping in mappings)
      {
        asmpArg.Add(args[mapping.InferenceArgumentSerialNo]);
      }
      assumption.FormulaStruct = asmp.FormulaStruct.Apply(asmpArg);

      if (asmp.DissolutableAssumption == null)
      {
        result.AssumptionFormulaStructs.Add(assumption);
        continue;
      }

      var disArg = new List<FormulaStruct>();
      var disMappings = asmp.DissolutableAssumption.FormulaStructArgumentMappings
                            .Where(fsam => fsam.FormulaStructId == asmp.DissolutableAssumption.FormulaStructId)
                            .OrderBy(fsam => fsam.FormulaStructArgumentSerialNo);
      foreach (var mapping in disMappings)
      {
        disArg.Add(args[mapping.InferenceArgumentSerialNo]);
      }
      if (asmp.DissolutableAssumption.FormulaStruct == null)
        throw new ArgumentException("Include Inference.Assumptions.DissolutableAssumption.FormulaStruct");
      assumption.DissolutableStruct = asmp.DissolutableAssumption.FormulaStruct.Apply(asmpArg);
      assumption.IsDissoluteForce = asmp.DissolutableAssumption.IsForce;
      // TODO: 不要？
      // if (asmp.DissolutableAssumption.IsForce)
      // {
      //   if (assumption.DissolutableStruct == null)
      //     throw new ArgumentException("There is no DissolutableStruct in spite of IsForce");
      // }

      result.AssumptionFormulaStructs.Add(assumption);
    }

    foreach (var con in Conclusions)
    {
      var conArgs = new List<FormulaStruct>();
      var conMappings = con.FormulaStructArgumentMappings
                           .Where(fsam => fsam.FormulaStructId == con.FormulaStructId)
                           .OrderBy(fsam => fsam.FormulaStructArgumentSerialNo);
      foreach (var mapping in conMappings)
      {
        conArgs.Add(args[mapping.InferenceArgumentSerialNo]);
      }
      if (con.FormulaStruct == null)
        throw new ArgumentException("Include Inference.Conclusions.FormulaStruct");

      result.ConclusionFormulaStructs.Add(con.FormulaStruct.Apply(conArgs));
    }

    return result;
  }
}
