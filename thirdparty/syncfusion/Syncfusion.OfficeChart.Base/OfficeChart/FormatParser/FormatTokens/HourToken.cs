// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.HourToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class HourToken : FormatTokenBase
{
  private const string DEF_FORMAT_LONG = "00";
  private static readonly Regex HourRegex = new Regex("[hH]+", RegexOptions.Compiled);
  private bool m_bAmPm;

  public override int TryParse(string strFormat, int iIndex)
  {
    return this.TryParseRegex(HourToken.HourRegex, strFormat, iIndex);
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    int hour = DateTime.FromOADate(value).Hour;
    if (this.IsAmPm && hour > 12)
      hour -= 12;
    return this.m_strFormat.Length > 1 ? hour.ToString("00") : hour.ToString();
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Hour;

  public bool IsAmPm
  {
    get => this.m_bAmPm;
    set => this.m_bAmPm = value;
  }
}
