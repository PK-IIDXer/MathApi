using MathApi.Models;

namespace MathApi.Controllers;

public class FormulaLabelDto
{
  public int Id { get; set; }
  public string Text { get; set; } = "";
  public Const.FormulaLabelTypeEnum TypeId { get; set; }

  public FormulaLabel CreateModel()
  {
    return new FormulaLabel
    {
      Id = Id,
      Text = Text,
      TypeId = TypeId
    };
  }
}
