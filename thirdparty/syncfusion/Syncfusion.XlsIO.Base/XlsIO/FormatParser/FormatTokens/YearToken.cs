// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.YearToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class YearToken : FormatTokenBase
{
  private const string DEF_FORMAT_SHORT = "00";
  private static readonly Regex YearRegex = new Regex("[yY]+", RegexOptions.Compiled);

  public override int TryParse(string strFormat, int iIndex)
  {
    return this.TryParseRegex(YearToken.YearRegex, strFormat, iIndex);
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    int year = DateTime.FromOADate(value).Year;
    return this.m_strFormat.Length > 2 ? year.ToString() : (year % 100).ToString("00");
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Year;
}
