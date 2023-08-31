using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

/// <summary>
/// 推論規則引数
/// </summary>
/// <remarks>
/// 推論規則を適用する際にアサインする命題・項・自由変数・束縛変数を定義する。
/// 自由変数への項の代入操作が必要な場合(∃除去等)、その代入元・代入先も指定する。
/// </remarks>
[PrimaryKey(nameof(InferenceId), nameof(SerialNo))]
public class InferenceArgument
{
  public Inference Inference { get; } = new();
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  /// <summary>
  /// 推論規則引数連番
  /// </summary>
  public int SerialNo { get; set; }
  /// <summary>
  /// 論理式ラベル
  /// </summary>
  public FormulaLabel? FormulaLabel { get; set; }
  /// <summary>
  /// 論理式ラベルID
  /// </summary>
  public int FormulaLabelId { get; set; }

  public List<InferenceArgumentConstraint> InferenceArgumentConstraints { get; set; } = new();
  public List<InferenceArgumentConstraint> InferenceArgumentConstraintDistinations { get; } = new();
  public List<InferenceFormulaStructArgumentMapping> InferenceFormulaStructArgumentMappings { get; } = new();
}
