using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

/// <summary>
/// 推論規則引数制約
/// </summary>
/// <remarks>
/// 自由変数に対する制約を定義するテーブル。
/// 1つの推論規則引数に対して複数のレコードが対応する。
/// 推論規則引数制約の1レコードは、「制約先推論規則引数連番」に制約先の推論規則引数(論理式)の連番を指定し、
/// その論理式に当該自由変数が含まれている場合、エラーとする。
/// </remarks>
[PrimaryKey(nameof(InferenceId), nameof(InferenceArgumentSerialNo), nameof(SerialNo))]
public class InferenceArgumentConstraint
{
  public InferenceArgument InferenceArgument { get; } = new();
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  /// <summary>
  /// 推論規則引数連番
  /// </summary>
  /// <remarks>
  /// 関連先の推論規則引数は自由変数以外指定できないものとする（バリデーションする）
  /// </remarks>
  public int InferenceArgumentSerialNo { get; set; }
  /// <summary>
  /// 推論規則引数制約連番
  /// </summary>
  public int SerialNo { get; set; }
  public InferenceArgument ConstraintDestinationInferenceArgument { get; } = new();
  /// <summary>
  /// 制約先推論規則引数連番
  /// </summary>
  public int ConstraintDestinationInferenceArgumentSerialNo { get; set; }
  /// <summary>
  /// 未解消の仮定に
  /// </summary>
  public bool IsConstraintPredissolvedAssumption { get; set; }
}
