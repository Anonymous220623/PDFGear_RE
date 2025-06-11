// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.DayToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class DayToken : FormatTokenBase
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
    return DateTime.FromOADate(value).ToString(" " + this.m_strFormatLower, (IFormatProvider) culture).Substring(1);
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Day;
}
