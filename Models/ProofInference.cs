using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

/// <summary>
/// 証明推論
/// </summary>
[PrimaryKey(nameof(TheoremId), nameof(ProofSerialNo), nameof(SerialNo))]
public class ProofInference
{
  public Proof Proof { get; } = new();
  /// <summary>
  /// 定理ID
  /// </summary>
  public long TheoremId { get; set; }
  /// <summary>
  /// 証明連番
  /// </summary>
  public long ProofSerialNo { get; set; }
  /// <summary>
  /// 証明推論連番
  /// </summary>
  public long SerialNo { get; set; }
  public Inference? Inference { get; }
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  public Formula? ConclusionFormula { get; set; }
  /// <summary>
  /// 結論論理式ID
  /// </summary>
  public long? ConclusionFormulaId { get; set; }
  public FormulaStruct? ConclusionFormulaStruct { get; set; }
  /// <summary>
  /// 結論論理式構成ID
  /// </summary>
  public long? ConclusionFormulaStructId { get; set; }
  /// <summary>
  /// 証明推論木From
  /// </summary>
  public long TreeFrom { get; set; }
  /// <summary>
  /// 証明推論木To
  /// </summary>
  public long TreeTo { get; set; }

  public List<ProofInferenceArgument>? ProofInferenceArguments { get; set; }

  public ProofAssumption? AddingProofInference { get; }
  public ProofAssumption? DissolutingAssumption { get; }
}
