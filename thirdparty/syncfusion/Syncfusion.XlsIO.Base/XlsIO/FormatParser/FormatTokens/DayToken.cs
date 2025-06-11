// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.DayToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class DayToken : FormatTokenBase
{
  private static readonly Regex DayRegex = new Regex("[Dd]+", RegexOptions.Compiled);
  private string m_strFormatLower;

  public override int TryParse(string strFormat, int iIndex)
  {
    int regex = this.TryParseRegex(DayToken.DayRegex, strFormat, iIndex);
    if (regex != iIndex)
      this.m_strFormatLower = this.m_strFormat.ToLower();
    return regex;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    double d = value;
    if (this.m_strFormatLower.Length > 2 && d < 60.0 && section.FormatType == ExcelFormatType.DateTime)
      --d;
    if (section.FormatType == ExcelFormatType.Number && d <= 31.0 && this.m_strFormatLower != "dddd" && this.m_strFormatLower != "ddd")
      ++d;
    return DateTime.FromOADate(d).ToString(" " + this.m_strFormatLower, (IFormatProvider) culture).Substring(1);
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Day;
}
