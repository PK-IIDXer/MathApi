namespace MathApi.Controllers;

public class ProofDto
{
  /// <summary>
  /// 証明対象の定理ＩＤ
  /// </summary>
  public long TheoremId { get; set; }
  /// <summary>
  /// 当証明のProofSerialNo
  /// </summary>
  public long ProofSerialNo { get; set; }
  /// <summary>
  /// 証明に追加する推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  /// <summary>
  /// 当InferenceIdの推論規則の引数にアサインする論理式
  /// </summary>
  public List<InferenceArgumentFormulaDto> InferenceArguments { get; set; } = new();
  /// <summary>
  /// 当InferenceIdの推論規則の仮定にアサインする論理式を導いたProofInferenceSerialNo
  /// </summary>
  public List<long> ProofInferenceSerialNos { get; set; } = new();
  public long? ProofAssumptionSerialNo { get; set; }

  public class InferenceArgumentFormulaDto
  {
    public long SerialNo { get; set; }
    public long? FormulaId { get; set; }
    public long? FormulaStructId { get; set; }
  }
}