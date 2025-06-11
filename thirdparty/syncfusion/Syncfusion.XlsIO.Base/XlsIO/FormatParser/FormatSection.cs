// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatSection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.FormatParser.FormatTokens;
using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser;

public class FormatSection : CommonObject
{
  private const int DEF_NOTFOUND_INDEX = -1;
  private const string DEF_THOUSAND_SEPARATOR = ",";
  private const string DEF_MINUS = "-";
  private const int DEF_ROUNDOFF_DIGIT = 9;
  private const char DEF_FRACTION_TOKEN = '/';
  private const int DEF_MONTHTOKEN_LENGTH = 5;
  private static readonly object[] DEF_POSSIBLE_TOKENS = new object[12]
  {
    (object) new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[5]
    {
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Unknown,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.String,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.ReservedPlace,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Character,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Color
    },
    (object) ExcelFormatType.Unknown,
    (object) new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[2]
    {
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.General,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Culture
    },
    (object) ExcelFormatType.General,
    (object) new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[9]
    {
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Unknown,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.String,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.ReservedPlace,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Character,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Color,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Condition,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Text,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Asterix,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Culture
    },
    (object) ExcelFormatType.Text,
    (object) new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[17]
    {
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Unknown,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.String,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.ReservedPlace,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Character,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Color,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Condition,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.SignificantDigit,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.InsignificantDigit,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.PlaceReservedDigit,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Percent,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Scientific,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.ThousandsSeparator,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.DecimalPoint,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Asterix,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Fraction,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Culture,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Dollar
    },
    (object) ExcelFormatType.Number,
    (object) new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[18]
    {
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Unknown,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Day,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.String,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.ReservedPlace,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Character,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Color,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Condition,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.SignificantDigit,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.InsignificantDigit,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.PlaceReservedDigit,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Percent,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Scientific,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.ThousandsSeparator,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.DecimalPoint,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Asterix,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Fraction,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Culture,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Dollar
    },
    (object) ExcelFormatType.Number,
    (object) new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[21]
    {
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Unknown,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Hour,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Hour24,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Minute,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.MinuteTotal,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Second,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.SecondTotal,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Year,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Month,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Day,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.String,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.ReservedPlace,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Character,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.AmPm,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Color,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Condition,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.SignificantDigit,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.DecimalPoint,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Asterix,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Fraction,
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Culture
    },
    (object) ExcelFormatType.DateTime
  };
  private static readonly Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[] DEF_BREAK_HOUR = new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[1]
  {
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Minute
  };
  private static readonly Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[] DEF_BREAK_SECOND = new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[5]
  {
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Minute,
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Hour,
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Day,
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Month,
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Year
  };
  private static readonly Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[] DEF_MILLISECONDTOKENS = new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[1]
  {
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.SignificantDigit
  };
  private static readonly Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[] NotTimeTokens = new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[3]
  {
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Day,
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Month,
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Year
  };
  private static readonly Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[] NotDateTokens = new Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[4]
  {
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Hour,
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Minute,
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Second,
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.AmPm
  };
  private List<FormatTokenBase> m_arrTokens;
  private bool m_bFormatPrepared;
  private int m_iDecimalPointPos = -1;
  private int m_iScientificPos = -1;
  private int m_iLastDigit = -1;
  private bool m_bLastGroups;
  private int m_iNumberOfFractionDigits;
  private int m_iNumberOfIntegerDigits;
  private int m_significantDigits;
  private int m_exponentDigits;
  private int m_iFractionPos = -1;
  private bool m_bFraction;
  private int m_iFractionStart;
  private int m_iFractionEnd;
  private int m_iDenumaratorLen;
  private int m_iNumneratorLen;
  private int m_fractionBase = -1;
  private int m_iIntegerEnd = -1;
  private int m_iDecimalEnd = -1;
  private ConditionToken m_condition;
  private CultureToken m_culture;
  private ExcelFormatType m_formatType;
  private bool m_bGroupDigits;
  private bool m_bMultiplePoints;
  private bool m_bUseSystemDateFormat;
  private bool m_bUseSystemTimeFormat;
  private bool m_isMilliSecondFormatValue;

  private FormatSection(IApplication application, object parent)
    : base(application, parent)
  {
  }

  public FormatSection(IApplication application, object parent, List<FormatTokenBase> arrTokens)
    : base(application, parent)
  {
    this.m_arrTokens = arrTokens != null ? new List<FormatTokenBase>((IEnumerable<FormatTokenBase>) arrTokens) : throw new ArgumentNullException(nameof (arrTokens));
    this.PrepareFormat();
  }

  public void PrepareFormat()
  {
    if (this.m_bFormatPrepared)
      return;
    this.PreparePositions();
    this.m_iLastDigit = this.LocateLastFractionDigit();
    this.m_bLastGroups = this.LocateLastGroups(this.m_iLastDigit + 1);
    if (this.FormatType == ExcelFormatType.Number)
    {
      this.m_iNumberOfFractionDigits = this.CalculateFractionDigits();
      this.m_iNumberOfIntegerDigits = this.CalculateIntegerDigits();
      this.m_significantDigits = this.CalculateODSIntegers();
      if (this.m_iScientificPos > 0)
        this.m_exponentDigits = this.CalculateSignificantDigits(this.m_iScientificPos, this.Count - 1);
      this.LocateFractionParts();
    }
    else if (this.FormatType == ExcelFormatType.DateTime)
    {
      this.SetRoundSeconds();
      this.m_iDecimalPointPos = -1;
      this.m_bFraction = false;
    }
    else
    {
      this.m_iNumberOfFractionDigits = -1;
      this.m_iNumberOfIntegerDigits = -1;
    }
    int count = this.Count;
    this.m_iDecimalEnd = count - 1;
    this.m_iIntegerEnd = count - 1;
    if (this.m_iScientificPos > 0)
      this.m_iIntegerEnd = this.m_iDecimalEnd = this.m_iScientificPos - 1;
    else if (this.m_bFraction)
    {
      this.LocateFractionParts();
      this.m_iIntegerEnd = this.m_iFractionStart - 1;
      this.m_iNumneratorLen = this.m_iFractionPos - (this.m_iIntegerEnd + 1);
      int.TryParse(this.GetFractionBase(), out this.m_fractionBase);
    }
    if (this.m_iDecimalPointPos > 0)
      this.m_iIntegerEnd = this.m_iDecimalPointPos - 1;
    if (this.FormatType == ExcelFormatType.Number)
      this.m_bGroupDigits = this.CheckGroupDigits();
    this.m_bFormatPrepared = true;
    if (this.m_arrTokens.Count <= 0 || !(this.m_arrTokens[0] is CultureToken arrToken))
      return;
    this.m_bUseSystemDateFormat = arrToken.UseSystemSettings;
    this.m_bUseSystemTimeFormat = arrToken.UseSystemTimeSettings;
  }

  private bool CheckGroupDigits()
  {
    int index1 = 0;
    for (int index2 = this.m_iIntegerEnd - 1; index1 <= index2; ++index1)
    {
      if (this[index1].TokenType == Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.ThousandsSeparator && this[index1 - 1] is DigitToken && this[index1 + 1] is DigitToken)
        return true;
    }
    return false;
  }

  private void PreparePositions()
  {
    bool flag = false;
    this.m_bMultiplePoints = false;
    int num = 0;
    for (int count = this.Count; num < count; ++num)
    {
      FormatTokenBase formatTokenBase = this[num];
      switch (formatTokenBase.TokenType)
      {
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Minute:
          this.CheckMinuteToken(num);
          break;
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.AmPm:
          HourToken correspondingHourSection = this.FindCorrespondingHourSection(num);
          if (correspondingHourSection != null)
          {
            correspondingHourSection.IsAmPm = true;
            break;
          }
          break;
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Condition:
          this.m_condition = this.m_condition == null ? (ConditionToken) formatTokenBase : throw new FormatException("More than one condition was found in the section.");
          break;
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.SignificantDigit:
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.InsignificantDigit:
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.PlaceReservedDigit:
          DigitToken digitToken = (DigitToken) formatTokenBase;
          if (!flag)
          {
            digitToken.IsLastDigit = true;
            flag = true;
            break;
          }
          break;
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Scientific:
          this.AssignPosition(ref this.m_iScientificPos, num);
          break;
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.DecimalPoint:
          if (this.m_iDecimalPointPos < 0)
          {
            this.AssignPosition(ref this.m_iDecimalPointPos, num);
            break;
          }
          this.m_bMultiplePoints = true;
          break;
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Fraction:
          if (this.m_iFractionPos < 0)
          {
            this.m_iFractionPos = num;
            this.m_bFraction = true;
            break;
          }
          this.m_bFraction = false;
          break;
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Culture:
          this.m_culture = this.m_culture == null ? (CultureToken) formatTokenBase : throw new FormatException("More than one culture information was found in the section");
          break;
      }
    }
    this.PrepareInsignificantDigits();
  }

  private void PrepareInsignificantDigits()
  {
    if (this.m_iDecimalPointPos < 0)
      return;
    for (int index = this.Count - 1; index > this.m_iDecimalPointPos; --index)
    {
      if (this.m_arrTokens[index] is DigitToken arrToken)
      {
        if (!(arrToken is InsignificantDigitToken insignificantDigitToken))
          break;
        insignificantDigitToken.HideIfZero = true;
      }
    }
  }

  public HourToken FindCorrespondingHourSection(int index)
  {
    int index1 = index;
    do
    {
      --index1;
      if (index1 < 0)
        index1 += this.Count;
      FormatTokenBase correspondingHourSection = this[index1];
      if (correspondingHourSection.TokenType == Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Hour)
        return (HourToken) correspondingHourSection;
    }
    while (index1 != index);
    return (HourToken) null;
  }

  public string ApplyFormat(double value) => this.ApplyFormat(value, false);

  public string ApplyFormat(string value) => this.ApplyFormat(value, false);

  private string UpdateDayofWeek(double value)
  {
    DateTime dateTime = DateTime.FromOADate(value);
    string str = dateTime.ToLongDateString();
    if (value <= 60.0)
    {
      string oldValue = dateTime.DayOfWeek.ToString();
      if (str.Contains(oldValue))
        str = str.Replace(oldValue, DateTime.FromOADate(value - 1.0).DayOfWeek.ToString());
    }
    return str;
  }

  public string ApplyFormat(double value, bool bShowReservedSymbols)
  {
    return this.ApplyFormat(value, bShowReservedSymbols, (RangeImpl) null);
  }

  internal string ApplyFormat(double value, bool bShowReservedSymbols, RangeImpl cell)
  {
    this.PrepareFormat();
    this.PrepareValue(ref value, bShowReservedSymbols);
    if (this.m_bUseSystemDateFormat)
      return this.UpdateDayofWeek(value);
    if (this.m_bUseSystemTimeFormat)
      return WorksheetImpl.ConvertSecondsMinutesToHours(DateTime.FromOADate(value).ToLongTimeString(), value);
    int count = this.Count;
    double dFraction = 0.0;
    double y1 = value != 0.0 ? Math.Floor(Math.Log10(Math.Abs(value))) : value;
    double num1 = value / Math.Pow(10.0, y1);
    if (this.m_iScientificPos > 0)
    {
      int y2 = this.m_iNumberOfIntegerDigits - 1;
      int num2 = value.ToString().IndexOf(this.Culture.NumberFormat.CurrencyDecimalSeparator);
      if (this.m_iNumberOfIntegerDigits > num2 && num2 > 0)
        y2 = num2 - 1;
      double num3 = num1 * Math.Pow(10.0, (double) y2);
      y1 -= (double) y2;
      value = num3;
    }
    if (this.m_formatType == ExcelFormatType.DateTime)
      value = Math.Round(value, 10);
    bool bAddNegativeSign = value < 0.0;
    if (this.m_formatType == ExcelFormatType.Number && !this.m_bFraction)
    {
      value = FormatSection.SplitValue(value, out dFraction);
      double fractionSize = Math.Pow(10.0, (double) this.m_iNumberOfFractionDigits);
      dFraction *= fractionSize;
      dFraction = this.Round(dFraction, value, fractionSize);
      if (dFraction >= fractionSize)
      {
        dFraction -= fractionSize;
        if (bAddNegativeSign)
          --value;
        else
          ++value;
      }
    }
    this.PrepareDigits(0, this.m_arrTokens.Count, false);
    if (value == 0.0)
      bAddNegativeSign &= dFraction > 0.0;
    string str1 = WorksheetImpl.ConvertSecondsMinutesToHours(this.ApplyFormat(value, bShowReservedSymbols, 0, this.m_iIntegerEnd, false, this.m_bGroupDigits, bAddNegativeSign), value);
    if (this.m_iDecimalPointPos > 0)
    {
      int startPos = this.m_iDecimalPointPos + 1;
      int length = dFraction.ToString().Length;
      if (length < this.DecimalNumber && dFraction != 0.0)
        this.PrepareDigits(startPos, startPos + (this.DecimalNumber - length), true);
      str1 += this.ApplyFormat(dFraction, bShowReservedSymbols, this.m_iDecimalPointPos, this.m_iDecimalEnd, false);
    }
    if (this.m_iScientificPos > 0)
      str1 += this.ApplyFormat(y1, bShowReservedSymbols, this.m_iDecimalEnd + 1, count - 1, false, false, y1 < 0.0);
    else if (this.m_bFraction)
    {
      dFraction = value;
      if (this.IsAnyDigit(0, this.m_iIntegerEnd))
      {
        dFraction -= value > 0.0 ? Math.Floor(value) : Math.Ceiling(value);
        dFraction = Math.Abs(dFraction);
      }
      if (dFraction != 0.0)
      {
        Fraction fraction = Fraction.ConvertToFraction(dFraction, this.m_iDenumaratorLen);
        string fractionBase = this.GetFractionBase();
        if (fractionBase != string.Empty)
        {
          double result = 0.0;
          if (double.TryParse(fractionBase, out result) && result != 0.0)
            fraction = new Fraction(Math.Round(fraction.Numerator * result / fraction.Denumerator), result);
        }
        long numerator = (long) fraction.Numerator;
        long denumerator = (long) fraction.Denumerator;
        str1 = numerator > 0L || !(str1 == " ") ? (value < 1.0 ? " " : ((int) value).ToString() + " ") : "0";
        if (numerator != 0L)
        {
          string str2 = str1 + this.ApplyFormat((double) numerator, bShowReservedSymbols, this.m_iIntegerEnd + 1, this.m_iFractionPos, false);
          string strResult = this.ApplyFormat((double) denumerator, bShowReservedSymbols, this.m_iFractionPos + 1, this.m_iFractionEnd, false);
          if (denumerator.ToString().Length < this.m_iDenumaratorLen)
            strResult = this.UpdateFormat(strResult);
          str1 = str2 + strResult + this.ApplyFormat(0.0, bShowReservedSymbols, this.m_iFractionEnd + 1, count - 1, false);
        }
      }
      else
        str1 = this.FillFractionFormat(str1 + this.ApplyFormat(0.0, bShowReservedSymbols, this.m_iFractionStart, this.m_iFractionEnd, false)).Replace('/', ' ');
    }
    if (this.m_formatType == ExcelFormatType.General || this.m_formatType == ExcelFormatType.Text)
    {
      if (cell != null)
      {
        str1 = this.ApplyRoundOffNumber(str1, cell);
      }
      else
      {
        string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        Decimal result;
        if (str1.Contains(decimalSeparator) && Decimal.TryParse(str1, out result) && str1.Length > 9)
        {
          result = Math.Round(result, 9);
          str1 = result.ToString().TrimEnd('0');
        }
      }
    }
    return str1;
  }

  internal string ApplyRoundOffNumber(string strResult, RangeImpl cell)
  {
    int num1 = 11;
    int digits = 9;
    int num2 = 0;
    if (cell != null)
    {
      if (cell.IsMerged)
      {
        foreach (RangeImpl rangeImpl in (IEnumerable<IRange>) cell.MergeArea)
          num2 += cell.Worksheet.GetColumnWidthInPixels(rangeImpl.Column);
      }
      else
        num2 = cell.Worksheet.GetColumnWidthInPixels(cell.Column);
      if (!strResult.Contains("E"))
      {
        Font nativeFont = cell.CellStyle.Font.GenerateNativeFont();
        double textWidth1 = cell.GetTextWidth(strResult, nativeFont);
        string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        if (strResult.Contains(decimalSeparator))
        {
          int length = strResult.IndexOf(decimalSeparator);
          string displayText = strResult.Substring(0, length);
          if (displayText.Length > num1)
            strResult = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:0.##E+00}", (object) Convert.ToDouble(strResult));
          double textWidth2 = cell.GetTextWidth(displayText, nativeFont);
          if (displayText.Length <= num1)
          {
            while (textWidth1 > (double) num2 && textWidth1 > textWidth2)
            {
              strResult = Math.Round(double.Parse(strResult), digits, MidpointRounding.AwayFromZero).ToString().TrimEnd('0');
              textWidth1 = cell.GetTextWidth(strResult, nativeFont);
              --digits;
            }
            if (strResult.Contains("-"))
              num1 = 12;
            while (strResult.Length > num1)
            {
              strResult = Math.Round(double.Parse(strResult), digits, MidpointRounding.AwayFromZero).ToString().TrimEnd('0');
              --digits;
            }
          }
        }
      }
    }
    return strResult;
  }

  private string GetFractionBase()
  {
    string empty = string.Empty;
    for (int digitGroupStart = this.GetDigitGroupStart(this.m_iFractionEnd, false); digitGroupStart <= this.m_iFractionEnd; ++digitGroupStart)
    {
      if (this.m_arrTokens[digitGroupStart] is SignificantDigitToken)
        empty += this.m_arrTokens[digitGroupStart].Format;
    }
    return empty;
  }

  private string UpdateFormat(string strResult)
  {
    string[] strArray = strResult.Split(new char[1]{ ' ' }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length <= 0)
      return strResult;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(strArray[strArray.Length - 1]);
    int length = stringBuilder.Length;
    for (int index = 0; index < strResult.Length - length; ++index)
      stringBuilder.Append(" ");
    return stringBuilder.ToString();
  }

  private string FillFractionFormat(string strResult)
  {
    if (this.IsSignificantDigit(this.m_iFractionStart, this.m_iFractionEnd))
    {
      StringBuilder stringBuilder = new StringBuilder(strResult);
      for (int index = strResult.IndexOf('/') + 1; index < strResult.Length; ++index)
      {
        if (char.IsDigit(strResult[index]))
          stringBuilder[index] = ' ';
      }
      strResult = stringBuilder.ToString();
    }
    return strResult;
  }

  private double Round(double value, double dValue, double fractionSize)
  {
    bool flag = value >= 0.0;
    double num1 = flag ? Math.Floor(value) : Math.Ceiling(value);
    int num2 = Math.Sign(value);
    double num3 = flag ? value - num1 : num1 - value;
    double num4 = ((num1 + 0.5) / fractionSize + dValue - (num1 / fractionSize + dValue)) * fractionSize;
    if (num4 > 0.5)
      num4 = 0.5;
    if (num3 >= num4)
      num1 += (double) num2;
    return num1;
  }

  private void PrepareDigits(int startPos, int count, bool IsCenterDigit)
  {
    for (int index = startPos; index < count; ++index)
    {
      if (this.m_arrTokens[index] is DigitToken arrToken)
        arrToken.IsCenterDigit = IsCenterDigit;
    }
  }

  public string ApplyFormat(string value, bool bShowReservedSymbols)
  {
    this.PrepareFormat();
    if (this.m_formatType != ExcelFormatType.Text && this.m_formatType != ExcelFormatType.Unknown && this.m_formatType != ExcelFormatType.General || this.m_bUseSystemDateFormat)
      return value;
    int count = this.m_arrTokens.Count;
    string str1 = string.Empty;
    if (count > 1)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = count - 1; index >= 0; --index)
      {
        string str2 = this.m_arrTokens[index].ApplyFormat(value, bShowReservedSymbols);
        stringBuilder.Insert(0, str2);
      }
      str1 = stringBuilder.ToString();
    }
    else if (count == 1)
      str1 = this.m_arrTokens[0].ApplyFormat(value, bShowReservedSymbols);
    return str1;
  }

  private void AssignPosition(ref int iToAssign, int iCurrentPos)
  {
    if (iToAssign >= 0)
      throw new FormatException();
    iToAssign = iCurrentPos;
  }

  private string ApplyFormat(
    double value,
    bool bShowReservedSymbols,
    int iStartToken,
    int iEndToken,
    bool bForward)
  {
    return this.ApplyFormat(value, bShowReservedSymbols, iStartToken, iEndToken, bForward, false, false);
  }

  private string ApplyFormat(
    double value,
    bool bShowReservedSymbols,
    int iStartToken,
    int iEndToken,
    bool bForward,
    bool bGroupDigits,
    bool bAddNegativeSign)
  {
    StringBuilder builder = new StringBuilder();
    int num1 = bForward ? 1 : -1;
    int iStart = bForward ? iStartToken : iEndToken;
    int iEndToken1 = bForward ? iEndToken : iStartToken;
    int iDigitCounter = 0;
    CultureInfo culture = this.Culture;
    int num2 = -1;
    double num3 = value;
    for (int index = iStart; this.CheckCondition(iEndToken1, bForward, index); index += num1)
    {
      FormatTokenBase arrToken = this.m_arrTokens[index];
      if (arrToken is DigitToken digit)
      {
        digit.OriginalValue = num3;
        iDigitCounter = this.ApplyDigit(digit, index, iStart, ref value, iDigitCounter, builder, bForward, bShowReservedSymbols, bGroupDigits);
        if (bForward)
        {
          if (bAddNegativeSign)
          {
            this.AddToBuilder(builder, bForward, "-");
            bAddNegativeSign = false;
          }
        }
        else
          num2 = builder.Length;
      }
      else
      {
        double num4 = num3;
        string strValue = arrToken.ApplyFormat(ref num4, bShowReservedSymbols, culture, this);
        if (arrToken is MonthToken && arrToken.Format.Length == 5)
          strValue = strValue.Substring(0, 1);
        if (arrToken is MilliSecondToken)
        {
          int num5 = int.Parse(strValue.Remove(0, 1));
          if (arrToken.Format == "0" && num5 >= 5)
            this.IsMilliSecondFormatValue = true;
          else if (arrToken.Format == "00" && num5 >= 50)
            this.IsMilliSecondFormatValue = true;
          else if (arrToken.Format == "000" && num5 >= 500)
            this.IsMilliSecondFormatValue = true;
        }
        if (strValue != null)
          this.AddToBuilder(builder, bForward, strValue);
      }
    }
    if (num2 >= 0 && bAddNegativeSign)
    {
      if (this.IsScientific)
        builder.Insert(builder.Length - num2, "-");
      else
        this.AddToBuilder(builder, bForward, "-");
    }
    return this.UpdateThousandSeparator(builder).ToString();
  }

  private StringBuilder UpdateThousandSeparator(StringBuilder builder)
  {
    string oldValue = "E+";
    string str1 = builder.ToString();
    string str2 = str1.Replace(oldValue, "");
    int length = str2.Length;
    if (this.IsScientific && str1.Contains(oldValue) && length > 3)
    {
      for (int index = 0; index <= this.m_iIntegerEnd; ++index)
      {
        if (this.m_arrTokens[index] is ThousandsSeparatorToken)
        {
          int num = 1;
          for (; length > 1; --length)
          {
            if (num % 3 == 0)
              str2 = str2.Insert(length - 1, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
            ++num;
          }
          string str3 = oldValue + str2;
          builder.Length = 0;
          builder.Append(str3);
          break;
        }
      }
    }
    return builder;
  }

  private int ApplyDigit(
    DigitToken digit,
    int iIndex,
    int iStart,
    ref double value,
    int iDigitCounter,
    StringBuilder builder,
    bool bForward,
    bool bShowHiddenSymbols,
    bool bGroupDigits)
  {
    if (digit == null)
      throw new ArgumentNullException(nameof (digit));
    CultureInfo culture = this.Culture;
    if (digit.IsLastDigit)
    {
      bool flag = value < 0.0;
      value = Math.Abs(value);
      do
      {
        string strTokenResult = digit.ApplyFormat(ref value, bShowHiddenSymbols, culture, this);
        iDigitCounter = this.ApplySingleDigit(iIndex, iStart, iDigitCounter, strTokenResult, builder, bForward, bGroupDigits);
      }
      while (value >= 1.0);
      if (flag)
        value = -value;
    }
    else
    {
      string strTokenResult = digit.ApplyFormat(ref value, bShowHiddenSymbols, culture, this);
      iDigitCounter = this.ApplySingleDigit(iIndex, iStart, iDigitCounter, strTokenResult, builder, bForward, bGroupDigits);
    }
    return iDigitCounter;
  }

  private int ApplySingleDigit(
    int iIndex,
    int iStart,
    int iDigitCounter,
    string strTokenResult,
    StringBuilder builder,
    bool bForward,
    bool bGroupDigits)
  {
    if (strTokenResult == null)
      throw new ArgumentNullException(nameof (strTokenResult));
    ++iDigitCounter;
    if (bGroupDigits && strTokenResult.Length > 0 && iIndex != iStart && iDigitCounter == 4)
    {
      this.AddToBuilder(builder, bForward, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator);
      iDigitCounter = 1;
    }
    this.AddToBuilder(builder, bForward, strTokenResult);
    return iDigitCounter;
  }

  private void AddToBuilder(StringBuilder builder, bool bForward, string strValue)
  {
    if (builder == null)
      throw new ArgumentNullException(nameof (builder));
    if (strValue == null)
      throw new ArgumentNullException(nameof (strValue));
    if (bForward)
      builder.Append(strValue);
    else
      builder.Insert(0, strValue);
  }

  private bool CheckCondition(int iEndToken, bool bForward, int iPos)
  {
    return !bForward ? iPos >= iEndToken : iPos <= iEndToken;
  }

  private int LocateLastFractionDigit()
  {
    int num = -1;
    int count = this.Count;
    for (int index = this.m_iScientificPos > 0 ? this.m_iScientificPos - 1 : count - 1; index >= 0; --index)
    {
      if (this[index] is DigitToken)
      {
        int iDecimalPointPos = this.m_iDecimalPointPos;
        num = index;
        break;
      }
    }
    if (this.m_iScientificPos > 0)
    {
      for (int iScientificPos = this.m_iScientificPos; iScientificPos < count; ++iScientificPos)
      {
        if (this[iScientificPos] is DigitToken digitToken)
        {
          digitToken.IsLastDigit = true;
          break;
        }
      }
    }
    return num;
  }

  private bool LocateLastGroups(int iStartIndex)
  {
    iStartIndex = Math.Max(this.m_iDecimalPointPos, iStartIndex);
    iStartIndex = Math.Max(0, iStartIndex);
    int count = this.Count;
    if (iStartIndex >= count)
      return false;
    ThousandsSeparatorToken arrToken = this.m_arrTokens[iStartIndex] as ThousandsSeparatorToken;
    bool flag = arrToken != null;
    for (; arrToken != null; arrToken = this.m_arrTokens[iStartIndex] as ThousandsSeparatorToken)
    {
      arrToken.IsAfterDigits = true;
      ++iStartIndex;
      if (iStartIndex >= count)
        break;
    }
    return flag;
  }

  private void ApplyLastGroups(ref double value, bool bShowReservedSymbols)
  {
    if (!this.m_bLastGroups)
      return;
    int index = this.m_iLastDigit + 1;
    for (int count = this.Count; index < count && this.m_arrTokens[index] is ThousandsSeparatorToken arrToken; ++index)
      value = arrToken.PreprocessValue(value);
  }

  private void PrepareValue(ref double value, bool bShowReservedSymbols)
  {
    this.ApplyLastGroups(ref value, bShowReservedSymbols);
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      if (this[index].TokenType == Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Percent)
        value *= 100.0;
    }
  }

  private int CalculateFractionDigits()
  {
    int num = 0;
    return this.m_iDecimalPointPos > 0 ? this.CalculateDigits(this.m_iDecimalPointPos, this.m_iLastDigit) : num;
  }

  private int CalculateIntegerDigits()
  {
    int iEndIndex = this.Count - 1;
    if (this.m_iDecimalPointPos > 0)
      iEndIndex = this.m_iDecimalPointPos;
    else if (this.m_iScientificPos > 0)
      iEndIndex = this.m_iScientificPos;
    return this.CalculateDigits(0, iEndIndex);
  }

  private int CalculateODSIntegers()
  {
    int iEndIndex = this.Count - 1;
    if (this.m_iDecimalPointPos > 0)
      iEndIndex = this.m_iDecimalPointPos;
    else if (this.m_iScientificPos > 0)
      iEndIndex = this.m_iScientificPos;
    else if (this.m_bFraction)
      iEndIndex = this.m_iFractionPos - 1;
    return this.CalculateSignificantDigits(0, iEndIndex);
  }

  private int CalculateSignificantDigits(int iStartIndex, int iEndIndex)
  {
    int count = this.Count;
    if (iStartIndex < 0 || iStartIndex > count)
      throw new ArgumentOutOfRangeException(nameof (iStartIndex), "Value cannot be less than 0 and greater than iCount.");
    if (iEndIndex < 0 || iEndIndex > count)
      throw new ArgumentOutOfRangeException(nameof (iEndIndex), "Value cannot be less than 0 and greater than iCount.");
    int significantDigits = 0;
    for (int index = iStartIndex; index <= iEndIndex; ++index)
    {
      if (this.m_arrTokens[index] is SignificantDigitToken)
        ++significantDigits;
    }
    return significantDigits;
  }

  private int CalculateDigits(int iStartIndex, int iEndIndex)
  {
    int count = this.Count;
    if (iStartIndex < 0 || iStartIndex > count)
      throw new ArgumentOutOfRangeException(nameof (iStartIndex), "Value cannot be less than 0 and greater than iCount.");
    if (iEndIndex < 0 || iEndIndex > count)
      throw new ArgumentOutOfRangeException(nameof (iEndIndex), "Value cannot be less than 0 and greater than iCount.");
    int digits = 0;
    for (int index = iStartIndex; index <= iEndIndex; ++index)
    {
      if (this.m_arrTokens[index] is DigitToken)
        ++digits;
    }
    return digits;
  }

  private void LocateFractionParts()
  {
    if (!this.m_bFraction)
      return;
    this.m_iFractionStart = this.GetDigitGroupStart(this.m_iFractionPos, false);
    this.m_iFractionEnd = this.GetDigitGroupStart(this.m_iFractionPos, true);
    this.m_iDenumaratorLen = this.m_iFractionEnd - this.GetDigitGroupStart(this.m_iFractionEnd, false) + 1;
    if (this.FormatType != ExcelFormatType.Number)
      return;
    if (this.m_iFractionStart < 0)
      throw new ArgumentException("Can't locate fraction digits");
    if (!(this.m_arrTokens[this.m_iFractionStart] is DigitToken))
      return;
    ((DigitToken) this.m_arrTokens[this.m_iFractionStart]).IsLastDigit = true;
  }

  private int GetDigitGroupStart(int iStartPos, bool bForward)
  {
    int num = bForward ? 1 : -1;
    int index1 = iStartPos;
    int count = this.Count;
    bool flag = false;
    for (; index1 >= 0 && index1 < count; index1 += num)
    {
      if (this.m_arrTokens[index1] is DigitToken || this.m_arrTokens[index1] is UnknownToken && this.m_arrTokens[index1].Format != " ")
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return -1;
    int index2 = index1 + num;
    while (index2 >= 0 && index2 < count && (this.m_arrTokens[index2] is DigitToken || this.m_arrTokens[index2] is UnknownToken && this.m_arrTokens[index2].Format != " "))
      index2 += num;
    return index2 - num;
  }

  private bool IsAnyDigit(int iStartIndex, int iEndIndex)
  {
    int count = this.Count;
    iStartIndex = Math.Max(iStartIndex, 0);
    iEndIndex = Math.Min(iEndIndex, count - 1);
    for (int index = iStartIndex; index < iEndIndex; ++index)
    {
      if (this.m_arrTokens[index] is DigitToken)
        return true;
    }
    return false;
  }

  private bool IsSignificantDigit(int iStartIndex, int iEndIndex)
  {
    int count = this.Count;
    iStartIndex = Math.Max(iStartIndex, 0);
    iEndIndex = Math.Min(iEndIndex, count - 1);
    for (int index = iStartIndex; index <= iEndIndex; ++index)
    {
      if (this.m_arrTokens[index] is SignificantDigitToken)
        return true;
    }
    return false;
  }

  public bool CheckCondition(double value)
  {
    return this.HasCondition && this.m_condition.CheckCondition(value);
  }

  private void DetectFormatType()
  {
    this.m_formatType = ExcelFormatType.Unknown;
    int index = 0;
    for (int length = FormatSection.DEF_POSSIBLE_TOKENS.Length; index < length; index += 2)
    {
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[] arrPossibleTokens = (Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[]) FormatSection.DEF_POSSIBLE_TOKENS[index];
      ExcelFormatType excelFormatType = (ExcelFormatType) FormatSection.DEF_POSSIBLE_TOKENS[index + 1];
      if ((excelFormatType != ExcelFormatType.Number || !this.m_bMultiplePoints) && this.CheckTokenTypes(arrPossibleTokens))
      {
        this.m_formatType = excelFormatType;
        break;
      }
    }
  }

  private bool CheckTokenTypes(Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[] arrPossibleTokens)
  {
    if (arrPossibleTokens == null)
      throw new ArgumentNullException(nameof (arrPossibleTokens));
    int count = this.Count;
    if (count == 0 && arrPossibleTokens.Length == 0)
      return true;
    int index1 = 0;
    for (int index2 = count; index1 < index2; ++index1)
    {
      FormatTokenBase formatTokenBase = this[index1];
      if (!FormatSection.ContainsIn(arrPossibleTokens, formatTokenBase.TokenType))
        return false;
    }
    return true;
  }

  private void CheckMinuteToken(int iTokenIndex)
  {
    FormatTokenBase formatTokenBase = this[iTokenIndex];
    if (formatTokenBase.TokenType != Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Minute)
      throw new ArgumentException("Wrong token type.");
    int num;
    if (this.FindTimeToken(iTokenIndex - 1, FormatSection.DEF_BREAK_HOUR, false, Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Hour, Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Hour24) == -1)
      num = this.FindTimeToken(iTokenIndex + 1, FormatSection.DEF_BREAK_SECOND, true, Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Second, Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.SecondTotal) != -1 ? 1 : 0;
    else
      num = 1;
    if (num != 0)
      return;
    MonthToken monthToken = new MonthToken();
    monthToken.Format = formatTokenBase.Format;
    this.m_arrTokens[iTokenIndex] = (FormatTokenBase) monthToken;
  }

  private int FindTimeToken(
    int iTokenIndex,
    Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[] arrBreakTypes,
    bool bForward,
    params Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[] arrTypes)
  {
    int count = this.Count;
    int num = bForward ? 1 : -1;
    for (; iTokenIndex >= 0 && iTokenIndex < count; iTokenIndex += num)
    {
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType tokenType = this[iTokenIndex].TokenType;
      if (Array.IndexOf<Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType>(arrBreakTypes, tokenType) == -1)
      {
        if (Array.IndexOf<Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType>(arrTypes, tokenType) != -1)
          return iTokenIndex;
      }
      else
        break;
    }
    return -1;
  }

  private void SetRoundSeconds()
  {
    bool flag = true;
    int count1 = this.Count;
    for (int index1 = 0; index1 < count1; ++index1)
    {
      if (this[index1].TokenType == Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.DecimalPoint)
      {
        int index2 = index1;
        string empty = string.Empty;
        for (++index1; index1 < count1 && Array.IndexOf<Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType>(FormatSection.DEF_MILLISECONDTOKENS, this[index1].TokenType) != -1; ++index1)
          empty += this[index1].Format;
        if (index1 != index2 + 1)
        {
          MilliSecondToken milliSecondToken = new MilliSecondToken();
          milliSecondToken.Format = empty;
          int count2 = index1 - index2;
          this.m_arrTokens.RemoveRange(index2, count2);
          this.m_arrTokens.Insert(index2, (FormatTokenBase) milliSecondToken);
          count1 -= count2 - 1;
          flag = false;
        }
      }
    }
    if (flag)
      return;
    for (int index = 0; index < count1; ++index)
    {
      FormatTokenBase formatTokenBase = this[index];
      if (formatTokenBase.TokenType == Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Second)
        ((SecondToken) formatTokenBase).RoundValue = false;
    }
  }

  internal bool IsTimeFormat
  {
    get
    {
      if (this.FormatType != ExcelFormatType.DateTime)
        return false;
      bool isTimeFormat = true;
      foreach (FormatTokenBase arrToken in this.m_arrTokens)
      {
        if (Array.IndexOf<Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType>(FormatSection.NotTimeTokens, arrToken.TokenType) >= 0)
        {
          isTimeFormat = false;
          break;
        }
      }
      return isTimeFormat;
    }
  }

  internal bool IsDateFormat
  {
    get
    {
      if (this.FormatType != ExcelFormatType.DateTime)
        return false;
      bool isDateFormat = true;
      foreach (FormatTokenBase arrToken in this.m_arrTokens)
      {
        if (Array.IndexOf<Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType>(FormatSection.NotDateTokens, arrToken.TokenType) >= 0)
        {
          isDateFormat = false;
          break;
        }
      }
      return isDateFormat;
    }
  }

  public FormatTokenBase this[int index]
  {
    get
    {
      return index >= 0 && index <= this.Count - 1 ? this.m_arrTokens[index] : throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than Count - 1.");
    }
  }

  public int Count => this.m_arrTokens.Count;

  public bool HasCondition => this.m_condition != null;

  public ExcelFormatType FormatType
  {
    get
    {
      if (this.m_formatType == ExcelFormatType.Unknown)
        this.DetectFormatType();
      return this.m_formatType;
    }
  }

  public CultureInfo Culture
  {
    get => this.m_culture == null ? CultureInfo.CurrentCulture : this.m_culture.Culture;
  }

  public bool IsFraction => this.m_bFraction;

  public bool IsScientific => this.m_iScientificPos >= 0;

  internal bool IsMilliSecondFormatValue
  {
    get => this.m_isMilliSecondFormatValue;
    set => this.m_isMilliSecondFormatValue = value;
  }

  public bool IsThousandSeparator => this.m_bGroupDigits;

  public int DecimalNumber => this.m_iNumberOfFractionDigits;

  internal int NumeratorLen
  {
    get => this.m_iNumneratorLen;
    set => this.m_iNumneratorLen = value;
  }

  internal int DenumaratorLen => this.m_iDenumaratorLen;

  internal int FractionBase => this.m_fractionBase;

  internal int SignificantDigits => this.m_significantDigits;

  internal int ExponentDigits => this.m_exponentDigits;

  private static bool ContainsIn(Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType[] arrPossibleTokens, Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType token)
  {
    if (arrPossibleTokens == null)
      throw new ArgumentNullException(nameof (arrPossibleTokens));
    int index1 = 0;
    int index2 = arrPossibleTokens.Length - 1;
    while (index2 != index1)
    {
      int index3 = (index2 + index1) / 2;
      Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType arrPossibleToken = arrPossibleTokens[index3];
      if (arrPossibleToken >= token)
      {
        if (index2 != index3)
          index2 = index3;
        else
          break;
      }
      else if (arrPossibleToken < token)
      {
        if (index1 != index3)
          index1 = index3;
        else
          break;
      }
    }
    return arrPossibleTokens[index1] == token || arrPossibleTokens[index2] == token;
  }

  private static double SplitValue(double value, out double dFraction)
  {
    bool flag = value > 0.0;
    int length = value.ToString().Length;
    dFraction = Math.Abs(value - (flag ? Math.Floor(value) : Math.Ceiling(value)));
    double num = Math.Abs(value) - dFraction;
    int digits = length - num.ToString().Length + 1;
    if (digits < length && digits < 15)
      dFraction = Math.Round(dFraction, digits);
    return !flag ? -num : num;
  }

  internal static double Round(double value)
  {
    bool flag = value >= 0.0;
    double num1 = flag ? Math.Floor(value) : Math.Ceiling(value);
    int num2 = Math.Sign(value);
    if ((flag ? value - num1 : num1 - value) >= 1.0 / 2.0)
      num1 += (double) num2;
    return num1;
  }

  public object Clone(object parent)
  {
    FormatSection formatSection = (FormatSection) this.MemberwiseClone();
    formatSection.SetParent(parent);
    formatSection.m_arrTokens = new List<FormatTokenBase>(this.m_arrTokens.Count);
    int index = 0;
    for (int count = this.m_arrTokens.Count; index < count; ++index)
    {
      FormatTokenBase formatTokenBase = (FormatTokenBase) this.m_arrTokens[index].Clone();
      formatSection.m_arrTokens.Add(formatTokenBase);
      switch (formatTokenBase.TokenType)
      {
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Condition:
          formatSection.m_condition = (ConditionToken) formatTokenBase;
          break;
        case Syncfusion.XlsIO.FormatParser.FormatTokens.TokenType.Culture:
          formatSection.m_culture = (CultureToken) formatTokenBase;
          break;
      }
    }
    return (object) formatSection;
  }

  internal void Clear()
  {
    this.m_arrTokens.Clear();
    this.m_arrTokens = (List<FormatTokenBase>) null;
    this.Dispose();
  }
}
