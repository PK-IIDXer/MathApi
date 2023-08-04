using Microsoft.EntityFrameworkCore;

namespace MathApi.Models;

/// <summary>
/// 推論規則仮定論理式構成要素
/// </summary>
/// <remarks>
/// 基本的には論理式の構成手続き(MathApi.Util.ProceedFormulaConstructionを参照)の引数を指定する。
/// ただし、論理式(ProceedFormulaConstructionメソッドのarguments引数)が指定された場合、
/// その自由変数に対する項の代入操作が指定できるよう「代入元・代入先推論規則引数連番」をもつ。
/// </remarks>
[PrimaryKey(nameof(InferenceId), nameof(InferenceAssumptionSerialNo), nameof(SerialNo))]
public class InferenceAssumptionFormula
{
  public InferenceAssumption InferenceAssumption { get; } = new();
  /// <summary>
  /// 推論規則ID
  /// </summary>
  public long InferenceId { get; set; }
  /// <summary>
  /// 推論規則仮定連番
  /// </summary>
  public int InferenceAssumptionSerialNo { get; set; }
  /// <summary>
  /// 推論規則仮定論理式構成要素連番
  /// </summary>
  public int SerialNo { get; set; }
  public Symbol Symbol { get; } = new();
  /// <summary>
  /// 記号ID
  /// </summary>
  public long? SymbolId { get; set; }
  public InferenceArgument BoundInferenceArgument { get; } = new();
  /// <summary>
  /// 束縛変数推論規則引数連番
  /// </summary>
  /// <remarks>
  /// 記号IDが量化記号の場合、それが束縛する変数を指定する
  /// </remarks>
  public int? BoundInferenceArgumentSerialNo { get; set; }
  public InferenceArgument InferenceArgument { get; } = new();
  /// <summary>
  /// 推論規則引数連番
  /// </summary>
  public int? InferenceArgumentSerialNo { get; set; }
  public InferenceArgument SubstitutionInferenceArgumentFrom { get; } = new();
  /// <summary>
  /// 代入元推論規則引数連番
  /// </summary>
  public int? SubstitutionInferenceArgumentFromSerialNo { get; set; }
  public InferenceArgument SubstitutionInferenceArgumentTo { get; } = new();
  /// <summary>
  /// 代入先推論規則引数連番
  /// </summary>
  public int? SubstitutionInferenceArgumentToSerialNo { get; set; }

  public Formula? Formula { get; }
  /// <summary>
  /// 論理式ID
  /// </summary>
  /// <remarks>
  /// IsBasic=falseの場合に、論理式によって推論規則を定義する際に使用する
  /// </remarks>
  public long? FormulaId { get; set; }
}