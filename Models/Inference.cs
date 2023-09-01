using MathApi.Commons;

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
  /// <summary>
  /// 仮定を追加するかどうか
  /// </summary>
  public bool IsAssumptionAdd { get; set; }

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

  // public InferenceResult Apply(
  //   long nextProofInferenceSerialNo,
  //   List<ProofInference> prevProofInferences,
  //   List<ProofAssumption> proofAssumptions,
  //   List<ProofInferenceArgument> args)
  // {
  //   InferenceResult result = new();

  //   // 基本的なバリデーション
  //   ValidateProofInference(args);

  //   if (prevProofInferences.Count != Assumptions.Count)
  //     throw new ArgumentException("Inference assumption count mismatch");

  //   // 未解消仮定の解消
  //   foreach (var ia in Assumptions)
  //   {
  //     var willDissolutable = ia.DissolutableAssumption.FormulaStruct;

  //     // 仮定論理式の存在確認
  //     var assumptionFormula = ia.FormulaStruct;
  //     var checkedFlag = false;
  //     // 解消フラグ（解消必須の仮定が存在しない場合エラーとする処理に使用
  //     bool dissoluteFlag = false;
  //     foreach (var prevPi in prevProofInferences)
  //     {
  //       if (prevPi.ConclusionFormula?.Equals(assumptionFormula) ?? throw new Exception())
  //       {
  //         // TODO: tree noの更新。ここではできないかも（_contextの取得が）
  //         result.UpdatedProofInferences.Add(prevPi);
  //         checkedFlag = true;
  //       }
  //       if (!checkedFlag)
  //         continue;

  //       // 仮定の解消
  //       // TODO: 考え方の齟齬：あれば解消。なければ「強制」フラグをみて、強制の場合はエラー。
  //       if (willDissolutable == null)
  //         continue;
  //       foreach (var pa in proofAssumptions.Where(p => prevPi.TreeFrom <= p.AddedProofInferenceSerialNo && p.AddedProofInferenceSerialNo <= prevPi.TreeTo))
  //       {
  //         if (pa.DissolutedProofInference != null)
  //           continue;

  //         if (willDissolutable.Equals(pa.Formula ?? throw new Exception()))
  //         {
  //           pa.DissolutedProofInferenceSerialNo = nextProofInferenceSerialNo;
  //           pa.LastUsedProofInferenceSerialNo = nextProofInferenceSerialNo;
  //           dissoluteFlag = true;

  //           result.UpdatedProofAssumptions.Add(pa);
  //           break;
  //         }
  //       }
  //       // 解消必須の仮定が存在しない場合エラー
  //       if (ia.DissolutionTypeId == (int)Const.InferenceAssumptionDissolutionType.Required
  //         && dissoluteFlag == false)
  //       {
  //         throw new ArgumentException("dissolution assumption is required but is not entered.");
  //       }

  //       if (checkedFlag)
  //         break;
  //     }
  //     if (!checkedFlag)
  //       throw new ArgumentException("Assumption mismatch");

  //     // 解消必須の仮定が存在しない場合エラー
  //     if (ia.DissolutionTypeId == (int)Const.InferenceAssumptionDissolutionType.Required
  //       && dissoluteFlag == false)
  //     {
  //       throw new ArgumentException("dissolution assumption is required but is not entered.");
  //     }
  //   }

  //   // 推論規則制約のチェック
  //   ValidateInferenceConstraint(proofAssumptions, args);

  //   // 更新対象の返却
  //   result.ConclusionFormula = CreateConclusionFormula(args);
  //   if (IsAssumptionAdd)
  //   {
  //     result.AddedProofAssumption = new ProofAssumption()
  //     {
  //       Formula = args[0].Formula,
  //       FormulaId = args[0].FormulaId,
  //       AddedProofInferenceSerialNo = nextProofInferenceSerialNo,
  //       LastUsedProofInferenceSerialNo = nextProofInferenceSerialNo
  //     };
  //   }

  //   return result;
  // }

  // private void ValidateProofInference(List<ProofInferenceArgument> args)
  // {
  //   if (args.Count != Arguments.Count)
  //     throw new ArgumentException("args count mismatch");

  //   if (args.Count == 0)
  //     return;

  //   for (var i = 0; i < args.Count; i++)
  //   {
  //     if (Arguments[i].InferenceArgumentTypeId == (int)Const.InferenceArgumentType.Term)
  //     {
  //       if (args[i].Formula?.FormulaTypeId != (long)Const.FormulaType.Term)
  //         throw new ArgumentException("argument formula mismatch");
  //     }
  //     if (Arguments[i].InferenceArgumentTypeId == (int)Const.InferenceArgumentType.Proposition)
  //     {
  //       if (args[i].Formula?.FormulaTypeId != (long)Const.FormulaType.Proposition)
  //         throw new ArgumentException("argument formula mismatch");
  //     }
  //     if (Arguments[i].InferenceArgumentTypeId == (int)Const.InferenceArgumentType.FreeVariable)
  //     {
  //       if (!args[i].Formula?.IsFreeVariable ?? throw new Exception())
  //         throw new ArgumentException("argument formula mismatch");
  //     }
  //   }

  //   // 結論に関するバリデーション
  //   if (InferenceConclusionFormulas.Count == 0)
  //     throw new ArgumentException("InferenceConclusionFormulas is null.");

  //   if (InferenceConclusionFormulas[0].SymbolId != null)
  //   {
  //     var firstSymbol = InferenceConclusionFormulas[0].Symbol;
  //     if (firstSymbol?.IsQuantifier ?? false)
  //     {
  //       if (InferenceConclusionFormulas.Count != 3)
  //         throw new ArgumentException("InferenceConclusionFormulas should be 3-formulas if first symbol is quantifier");
  //     }
  //   }

  //   foreach (var inferenceAssumption in Assumptions)
  //   {
  //     // 仮定に関するバリデーション
  //     var assumptionFormulas = inferenceAssumption.InferenceAssumptionFormulas;
  //     if (assumptionFormulas.Count == 0)
  //       throw new ArgumentException("InferenceAssumptionFormulas is null.");
  //     if (assumptionFormulas[0].SymbolId != null)
  //     {
  //       var firstSymbol = assumptionFormulas[0].Symbol;
  //       if (firstSymbol?.IsQuantifier ?? false)
  //       {
  //         if (assumptionFormulas.Count != 3)
  //           throw new ArgumentException("InferenceAssumptionFormulas should be 3-formulas if first symbol is quantifier");
  //       }
  //     }

  //     // 解消可能仮定に関するバリデーション
  //     var dissolutableAssumptionFormulas = inferenceAssumption.InferenceAssumptionDissolutableAssumptionFormulas;
  //     if (dissolutableAssumptionFormulas == null)
  //       return;
  //     if (dissolutableAssumptionFormulas.Count == 0)
  //       throw new ArgumentException("InferenceAssumptionDissolutableAssumptionFormulas is null.");
  //     if (dissolutableAssumptionFormulas[0].SymbolId != null)
  //     {
  //       var firstSymbol = dissolutableAssumptionFormulas[0].Symbol;
  //       if (firstSymbol?.IsQuantifier ?? false)
  //       {
  //         if (dissolutableAssumptionFormulas.Count != 3)
  //           throw new ArgumentException("InferenceAssumptionDissolutableAssumptionFormulas should be 3-formulas if first symbol is quantifier");
  //       }
  //     }
  //   }
  // }

  // private void ValidateInferenceConstraint(List<ProofAssumption> proofAssumptions, List<ProofInferenceArgument> args)
  // {
  //   foreach (var ia in Arguments)
  //   {
  //     if (ia.InferenceArgumentConstraints == null)
  //       continue;

  //     var arg = args.Find(a => a.SerialNo == ia.SerialNo)
  //       ?? throw new ArgumentException("Proof Argument and Inference Argument are mismatch");

  //     foreach (var iac in ia.InferenceArgumentConstraints)
  //     {
  //       // 推論規則制約が存在する推論規則引数には、自由変数のみが許可される
  //       if (!arg.Formula?.IsFreeVariable ?? throw new Exception())
  //         throw new ArgumentException("only free variable is arrowed, where has Inference argument constraints");

  //       var trgInfArg = args.Find(a => a.SerialNo == iac.ConstraintDestinationInferenceArgumentSerialNo)
  //         ?? throw new ArgumentException("Proof Argument and Inference Argument are mismatch");

  //       if (trgInfArg.Formula?.HasSymbol(arg.Formula?.FormulaStrings[0].Symbol ?? throw new Exception()) ?? throw new Exception())
  //         throw new ArgumentException("Inference Argument Constraint Error");

  //       // 未解消仮定への制約チェック
  //       if (iac.IsConstraintPredissolvedAssumption)
  //       {
  //         foreach (var lastUsedProofInference in proofAssumptions.Where(pa => pa.LastUsedProofInferenceSerialNo == arg.ProofInference.SerialNo))
  //         {
  //           if (lastUsedProofInference.Formula?.HasSymbol(arg.Formula.FormulaStrings[0].Symbol) ?? throw new Exception())
  //             throw new ArgumentException("Inference Argument Constraint Error");
  //         }
  //       }
  //     }
  //   }
  // }

  // /// <summary>
  // /// 結論論理式を作成。
  // /// ※以下のインクルードが必要
  // /// ・InferenceConclusionFormulas
  // /// ・InferenceConclusionFormulas.Symbol
  // /// ・args(引数).Formula
  // /// ・args(引数).Formula.FormulaStrings
  // /// ・args(引数).Formula.FormulaStrings.Symbol
  // /// ・args(引数).Formula.FormulaChains
  // /// </summary>
  // public Formula CreateConclusionFormula(List<ProofInferenceArgument> args)
  // {
  //   return Util.CreateProofInferenceFormula(InferenceConclusionFormulas.Select(icf => icf as IInferenceFormula).ToList(), args);
  // }
}
