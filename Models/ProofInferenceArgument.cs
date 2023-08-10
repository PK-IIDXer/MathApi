using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

/// <summary>
/// 証明推論引数
/// </summary>
[PrimaryKey(nameof(TheoremId), nameof(ProofSerialNo), nameof(ProofInferenceSerialNo), nameof(SerialNo))]
public class ProofInferenceArgument
{
  public ProofInference ProofInference { get; } = new();
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
  public long ProofInferenceSerialNo { get; set; }
  /// <summary>
  /// 証明推論引数連番
  /// </summary>
  public long SerialNo { get; set; }
  public Formula? Formula { get; }
  /// <summary>
  /// 論理式ID
  /// </summary>
  public long FormulaId { get; set; }
}
