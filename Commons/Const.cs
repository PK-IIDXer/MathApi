namespace MathApi.Const;

/// <summary>
/// 文字タイプ
/// </summary>
public enum SymbolTypeEnum : int {
  FreeVariable = 1,
  BoundVariable = 2,
  Function = 3,
  Predicate = 4,
  Logic = 5,
  TermQuantifier = 6,
  PropositionQuantifier = 7
}

/// <summary>
/// 論理式タイプ
/// </summary>
public enum FormulaTypeEnum : int {
  Term = 1,
  Proposition = 2
}

/// <summary>
/// 論理式構成タイプ
/// </summary>
public enum FormulaLabelTypeEnum : int {
  Term = 1,
  Proposition = 2,
  FreeVariable = 3
}

/// <summary>
/// 基礎的な記号ID
/// </summary>
public enum BasicSymbolEnum : long {
  BoundVariable = 0,
  Equals = 1,
  Contradiction = 2,
  Denial = 3,
  And = 4,
  Or = 5,
  Implication = 6,
  Forall = 7,
  Exists = 8
}