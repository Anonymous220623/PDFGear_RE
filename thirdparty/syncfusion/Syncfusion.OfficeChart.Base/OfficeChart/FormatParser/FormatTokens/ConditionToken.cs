// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.ConditionToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class ConditionToken : InBracketToken
{
  private static readonly string[] CompareOperationStrings = new string[6]
  {
    "=",
    "<>",
    ">=",
    "<=",
    "<",
    ">"
  };
  private double m_dCompareNumber;
  private ConditionToken.CompareOperation m_operation;

  public override int TryParse(string strFormat, int iStartIndex, int iIndex, int iEndIndex)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty.");
    if (iIndex < 0 || iIndex > num - 1)
      throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    int index = this.FindString(ConditionToken.CompareOperationStrings, strFormat, iIndex, false);
    if (index < 0)
      return iStartIndex;
    string compareOperationString = ConditionToken.CompareOperationStrings[index];
    iIndex += compareOperationString.Length;
    this.m_operation = (ConditionToken.CompareOperation) (index + 1);
    double result;
    if (!double.TryParse(strFormat.Substring(iIndex, iEndIndex - iIndex), NumberStyles.Any, (IFormatProvider) null, out result))
      return iStartIndex;
    this.m_dCompareNumber = result;
    return iEndIndex + 1;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return string.Empty;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Condition;

  public bool CheckCondition(double value)
  {
    switch (this.m_operation)
    {
      case ConditionToken.CompareOperation.Equal:
        return value == this.m_dCompareNumber;
      case ConditionToken.CompareOperation.NotEqual:
        return value != this.m_dCompareNumber;
      case ConditionToken.CompareOperation.GreaterEqual:
        return value >= this.m_dCompareNumber;
      case ConditionToken.CompareOperation.LessEqual:
        return value <= this.m_dCompareNumber;
      case ConditionToken.CompareOperation.Less:
        return value < this.m_dCompareNumber;
      case ConditionToken.CompareOperation.Greater:
        return value > this.m_dCompareNumber;
      default:
        throw new ArgumentOutOfRangeException("Compare operation");
    }
  }

  private enum CompareOperation
  {
    None,
    Equal,
    NotEqual,
    GreaterEqual,
    LessEqual,
    Less,
    Greater,
  }
}
