// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.SecondToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class SecondToken : FormatTokenBase
{
  private const string DEF_FORMAT_LONG = "00";
  private const int DEF_MILLISECOND_HALF = 500;
  private const double DEF_OLE_DOUBLE = 2958465.9999999884;
  private const double DEF_MAX_DOUBLE = 2958466.0;
  private static readonly Regex SecondRegex = new Regex("[sS]+", RegexOptions.Compiled);
  private bool m_bRound = true;

  public override int TryParse(string strFormat, int iIndex)
  {
    return this.TryParseRegex(SecondToken.SecondRegex, strFormat, iIndex);
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    DateTime dateTime = DateTime.FromOADate(value);
    int second = dateTime.Second;
    int millisecond = dateTime.Millisecond;
    if (this.m_bRound && millisecond >= 500)
      ++second;
    return this.m_strFormat.Length > 1 ? second.ToString("00") : second.ToString();
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Second;

  public bool RoundValue
  {
    get => this.m_bRound;
    set => this.m_bRound = value;
  }
}
