namespace MathApi.Models;

/// <summary>
/// 論理式ラベル種類
/// </summary>
/// <remarks>
/// 項、命題、自由変数の３種類
/// </remarks>
public class FormulaLabelType
{
  /// <summary>
  /// 論理式ラベル種類ID
  /// </summary>
  public Const.FormulaLabelType Id { get; set; }
  /// <summary>
  /// 論理式ラベル種類名称
  /// </summary>
  public string Name { get; set; } = "";
  
  public List<FormulaLabel> FormulaLabels { get; } = new();
}