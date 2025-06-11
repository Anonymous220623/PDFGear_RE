// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.MonthToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class MonthToken : FormatTokenBase
{
  private const string DEF_FORMAT_SHORT = "00";
  private const int DEF_FULL_NAME_LENGTH = 4;
  private const int DEF_SHORT_NAME_LENGTH = 3;
  private const int DEF_LONG_NUMBER_LENGTH = 3;
  private const string DEF_LONG_NUMBER_FORMAT = "00";
  private static readonly Regex MonthRegex = new Regex("[Mm]{3,}", RegexOptions.Compiled);

  public override int TryParse(string strFormat, int iIndex)
  {
    return this.TryParseRegex(MonthToken.MonthRegex, strFormat, iIndex);
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return DateTime.FromOADate(value).ToString(" " + this.m_strFormat, (IFormatProvider) culture).Substring(1);
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Month;

  protected override void OnFormatChange()
  {
    base.OnFormatChange();
    this.m_strFormat = this.m_strFormat.ToUpper();
  }
}
