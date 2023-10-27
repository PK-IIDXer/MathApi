namespace MathApi.Models;
using System.Text.Json.Serialization;

public class FormulaLabel
{
  public int Id { get; set; }
  public string Text { get; set; } = "";
  [JsonIgnore]
  public FormulaLabelType? Type { get; set; }
  public Const.FormulaLabelTypeEnum TypeId { get; set; }

  public List<FormulaStructArgument> FormulaStructArguments { get; } = new();
  public List<InferenceArgument> InferenceArguments { get; } = new();
}
