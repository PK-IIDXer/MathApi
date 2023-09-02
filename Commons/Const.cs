namespace MathApi.Const;

/// <summary>
/// 文字タイプ
/// </summary>
public enum SymbolType : int {
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
public enum FormulaType : int {
  Term = 1,
  Proposition = 2
}

/// <summary>
/// 論理式構成タイプ
/// </summary>
public enum FormulaLabelType : int {
  Term = 1,
  Proposition = 2,
  FreeVariable = 3
}

/// <summary>
/// 推論規則仮定解消タイプ
/// </summary>
public enum InferenceAssumptionDissolutionType : int {
  None = 1,
  Required = 2,
  Necessary = 3
}

/// <summary>
/// 基礎的な記号ID
/// </summary>
public enum BasicSymbol : long {
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