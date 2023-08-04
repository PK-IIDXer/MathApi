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
  public Inference Inference { get; } = new();
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  public Formula ConclusionFormula { get; set; } = new();
  /// <summary>
  /// 結論論理式ID
  /// </summary>
  public long ConclusionFormulaId { get; set; }

  public List<ProofInference>? PreviousProofInferences { get; set; }
  public ProofInference? NextProofInference { get; set; }
  /// <summary>
  /// 後続証明推論連番
  /// </summary>
  /// <remarks>
  /// 当該証明推論の結果を使用する証明推論を指定する
  /// </remarks>
  public long? NextProofInferenceSerialNo { get; set; }

  public List<ProofInferenceArgument>? ProofInferenceArguments { get; set; }

  public ProofAssumption? AddingProofInference { get; }
  public ProofAssumption? LastUsingProofInference { get; }
  public ProofAssumption? DissolutingAssumption { get; }
}
