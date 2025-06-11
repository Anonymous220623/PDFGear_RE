// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.MilliSecondToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class MilliSecondToken : FormatTokenBase
{
  private const string DEF_FORMAT_LONG = "000";
  private const string DEF_DOT = ".";
  private const double DEF_OLE_DOUBLE = 2958465.9999999884;
  private const double DEF_MAX_DOUBLE = 2958466.0;
  private static readonly int DEF_MAX_LEN = "000".Length;

  public override int TryParse(string strFormat, int iIndex) => throw new NotImplementedException();

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    int num = DateTime.FromOADate(value).Millisecond;
    int length = this.m_strFormat.Length;
    string empty = string.Empty;
    string str = string.Empty;
    string format;
    if (length < MilliSecondToken.DEF_MAX_LEN)
    {
      int y = MilliSecondToken.DEF_MAX_LEN - length;
      num = (int) FormatSection.Round((double) num / Math.Pow(10.0, (double) y));
      format = this.m_strFormat.Substring(1, length - 1);
    }
    else
    {
      format = "000";
      str = this.m_strFormat.Substring(MilliSecondToken.DEF_MAX_LEN);
    }
    return $".{num.ToString(format)}{str}";
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols) => string.Empty;

  public override TokenType TokenType => TokenType.MilliSecond;
}
