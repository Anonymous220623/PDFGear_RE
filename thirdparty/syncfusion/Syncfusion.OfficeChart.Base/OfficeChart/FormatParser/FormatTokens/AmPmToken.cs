// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatTokens.AmPmToken
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Globalization;
using System.Threading;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser.FormatTokens;

internal class AmPmToken : FormatTokenBase
{
  private const string DefaultStart2 = "tt";
  private const string DEF_START = "AM/PM";
  private const int DEF_AMPM_EDGE = 12;
  private const string DEF_AM = "AM";
  private const string DEF_PM = "PM";
  private static readonly int DEF_LENGTH = "AM/PM".Length;

  public override int TryParse(string strFormat, int iIndex)
  {
    int num = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num == 0)
      throw new ArgumentException("strFormat - string cannot be empty.");
    if (iIndex < 0 || iIndex > num - 1)
      throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than format length - 1.");
    if (string.Compare(strFormat, iIndex, "AM/PM", 0, AmPmToken.DEF_LENGTH, StringComparison.CurrentCultureIgnoreCase) == 0)
    {
      this.m_strFormat = "AM/PM";
      iIndex += AmPmToken.DEF_LENGTH;
    }
    return iIndex;
  }

  public override string ApplyFormat(
    ref double value,
    bool bShowHiddenSymbols,
    CultureInfo culture,
    FormatSection section)
  {
    return DateTime.FromOADate(value).Hour <= 12 ? culture.DateTimeFormat.AMDesignator : culture.DateTimeFormat.PMDesignator;
  }

  public override string ApplyFormat(string value, bool bShowHiddenSymbols)
  {
    throw new NotSupportedException();
  }

  internal static string CheckAndApplyAMPM(string format)
  {
    if (format == null)
      throw new ArgumentNullException(format);
    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
    int iIndex1 = new HourToken().TryParse(format, 0);
    int iIndex2 = new MinuteToken().TryParse(format, iIndex1);
    int num = new SecondToken().TryParse(format, iIndex2);
    return (iIndex1 != 0 || iIndex2 != 0 || num != 0) && format.Contains("tt") && currentCulture.DateTimeFormat.ShortTimePattern.Contains("tt") ? format.Replace("tt", "AM/PM") : format;
  }

  public override TokenType TokenType => TokenType.AmPm;
}
