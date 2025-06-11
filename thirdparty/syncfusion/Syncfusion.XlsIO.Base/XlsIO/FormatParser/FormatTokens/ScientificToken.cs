// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.ScientificToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class ScientificToken : FormatTokenBase
{
  private const string DEF_SHORT_FORM = "E";
  private static readonly string[] PossibleFormats = new string[2]
  {
    "E+",
    "E-"
  };
  private int m_iFormatIndex = -1;

  public override int TryParse(string strFormat, int iIndex)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty");
    if (iIndex < 0 || iIndex > num - 1)
      throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    this.m_iFormatIndex = this.FindString(ScientificToken.PossibleFormats, strFormat, iIndex, false);
    return this.m_iFormatIndex < 0 ? iIndex : iIndex + ScientificToken.PossibleFormats[this.m_iFormatIndex].Length;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return this.m_iFormatIndex == 0 && value >= 0.0 ? ScientificToken.PossibleFormats[0] : "E";
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Scientific;
}
