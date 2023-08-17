namespace MathApi.Models;

public class FormulaLabel
{
  public int Id { get; set; }
  public string Text { get; set; } = "";
  public FormulaLabelType? Type { get; set; }
  public Const.FormulaLabelType TypeId { get; set; }

  public List<FormulaStructArgument> FormulaStructArguments { get; } = new();
}
