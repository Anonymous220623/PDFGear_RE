// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.DollarToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class DollarToken : FormatTokenBase
{
  private const char DEF_START = '[';
  private const char DEF_END = ']';
  private const char DEF_DOLLAR = '$';
  private const char DEF_HYPEN = '-';

  public override int TryParse(string strFormat, int iIndex)
  {
    int num1 = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num1 == 0)
      throw new ArgumentException("strFormat - string cannot be empty.");
    int startIndex = iIndex >= 0 && iIndex <= num1 - 1 ? iIndex : throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    if (strFormat[iIndex] != '[')
      return iIndex;
    ++iIndex;
    int num2 = strFormat.IndexOf(']', iIndex);
    if (num2 < iIndex)
      return startIndex;
    int num3 = strFormat.IndexOf('$', iIndex);
    if (num3 != -1 && num3 == iIndex)
    {
      this.m_strFormat = strFormat.Substring(startIndex, num2 - startIndex + 1);
      if (!this.m_strFormat.Contains('-'.ToString()))
        return num2 + 1;
    }
    return startIndex;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    string str = this.m_strFormat.Remove(0, 1).Remove(0, 1);
    return str.Remove(str.Length - 1, 1);
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => value;

  public override TokenType TokenType => TokenType.Dollar;
}
