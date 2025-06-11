// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.MinuteToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class MinuteToken : FormatTokenBase
{
  private const string DEF_FORMAT_LONG = "00";
  private const double DEF_OLE_DOUBLE = 2958465.9999999884;
  private const double DEF_MAX_DOUBLE = 2958466.0;
  private static readonly Regex MinuteRegex = new Regex("[mM]{1,2}", RegexOptions.Compiled);

  public override int TryParse(string strFormat, int iIndex)
  {
    return this.TryParseRegex(MinuteToken.MinuteRegex, strFormat, iIndex);
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    DateTime dateTime = DateTime.FromOADate(value);
    int minute = dateTime.Minute;
    int second = dateTime.Second;
    if (dateTime.Millisecond >= 500 && second == 59)
    {
      if (!section.IsMilliSecondFormatValue)
        ++minute;
      else
        section.IsMilliSecondFormatValue = false;
    }
    return this.m_strFormat.Length > 1 ? minute.ToString("00") : minute.ToString();
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.Minute;

  protected override void OnFormatChange()
  {
    base.OnFormatChange();
    this.m_strFormat = this.m_strFormat.ToLower();
  }
}
