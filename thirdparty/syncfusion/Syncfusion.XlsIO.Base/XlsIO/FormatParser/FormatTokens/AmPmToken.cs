// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatTokens.AmPmToken
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Globalization;
using System.Threading;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser.FormatTokens;

public class AmPmToken : FormatTokenBase
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
    int hour = DateTime.FromOADate(value).Hour;
    string amDesignator = culture.DateTimeFormat.AMDesignator;
    string pmDesignator = culture.DateTimeFormat.PMDesignator;
    return hour >= 12 ? (pmDesignator != string.Empty ? pmDesignator : "PM") : (amDesignator != string.Empty ? amDesignator : "AM");
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
    new SecondToken().TryParse(format, iIndex2);
    return format;
  }

  public override TokenType TokenType => TokenType.AmPm;
}
