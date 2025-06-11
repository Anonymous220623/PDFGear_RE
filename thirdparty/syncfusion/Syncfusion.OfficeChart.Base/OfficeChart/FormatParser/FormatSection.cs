// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatSection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.FormatParser.FormatTokens;
using Syncfusion.OfficeChart.Implementation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser;

internal class FormatSection : CommonObject
{
  private const int DEF_NOTFOUND_INDEX = -1;
  private const string DEF_THOUSAND_SEPARATOR = ",";
  private const string DEF_MINUS = "-";
  private const int DEF_ROUNDOFF_DIGIT = 9;
  private static readonly object[] DEF_POSSIBLE_TOKENS = new object[12]
  {
    (object) new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[5]
    {
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Unknown,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.String,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.ReservedPlace,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Character,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Color
    },
    (object) OfficeFormatType.Unknown,
    (object) new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[2]
    {
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.General,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Culture
    },
    (object) OfficeFormatType.General,
    (object) new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[9]
    {
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Unknown,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.String,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.ReservedPlace,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Character,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Color,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Condition,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Text,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Asterix,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Culture
    },
    (object) OfficeFormatType.Text,
    (object) new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[16 /*0x10*/]
    {
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Unknown,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.String,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.ReservedPlace,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Character,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Color,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Condition,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.SignificantDigit,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.InsignificantDigit,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.PlaceReservedDigit,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Percent,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Scientific,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.ThousandsSeparator,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.DecimalPoint,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Asterix,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Fraction,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Culture
    },
    (object) OfficeFormatType.Number,
    (object) new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[17]
    {
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Unknown,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Day,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.String,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.ReservedPlace,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Character,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Color,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Condition,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.SignificantDigit,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.InsignificantDigit,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.PlaceReservedDigit,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Percent,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Scientific,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.ThousandsSeparator,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.DecimalPoint,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Asterix,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Fraction,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Culture
    },
    (object) OfficeFormatType.Number,
    (object) new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[21]
    {
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Unknown,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Hour,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Hour24,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Minute,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.MinuteTotal,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Second,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.SecondTotal,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Year,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Month,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Day,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.String,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.ReservedPlace,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Character,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.AmPm,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Color,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Condition,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.SignificantDigit,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.DecimalPoint,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Asterix,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Fraction,
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Culture
    },
    (object) OfficeFormatType.DateTime
  };
  private static readonly Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[] DEF_BREAK_HOUR = new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[1]
  {
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Minute
  };
  private static readonly Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[] DEF_BREAK_SECOND = new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[5]
  {
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Minute,
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Hour,
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Day,
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Month,
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Year
  };
  private static readonly Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[] DEF_MILLISECONDTOKENS = new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[1]
  {
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.SignificantDigit
  };
  private static readonly Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[] NotTimeTokens = new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[3]
  {
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Day,
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Month,
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Year
  };
  private static readonly Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[] NotDateTokens = new Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[4]
  {
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Hour,
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Minute,
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Second,
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.AmPm
  };
  private List<FormatTokenBase> m_arrTokens;
  private bool m_bFormatPrepared;
  private int m_iDecimalPointPos = -1;
  private int m_iScientificPos = -1;
  private int m_iLastDigit = -1;
  private bool m_bLastGroups;
  private int m_iNumberOfFractionDigits;
  private int m_iNumberOfIntegerDigits;
  private int m_iFractionPos = -1;
  private bool m_bFraction;
  private int m_iFractionStart;
  private int m_iFractionEnd;
  private int m_iDenumaratorLen;
  private int m_iIntegerEnd = -1;
  private int m_iDecimalEnd = -1;
  private ConditionToken m_condition;
  private CultureToken m_culture;
  private OfficeFormatType m_formatType;
  private bool m_bGroupDigits;
  private bool m_bMultiplePoints;
  private bool m_bUseSystemDateFormat;

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
    if (this.FormatType == OfficeFormatType.Number)
    {
      this.m_iNumberOfFractionDigits = this.CalculateFractionDigits();
      this.m_iNumberOfIntegerDigits = this.CalculateIntegerDigits();
      this.LocateFractionParts();
    }
    else if (this.FormatType == OfficeFormatType.DateTime)
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
    }
    if (this.m_iDecimalPointPos > 0)
      this.m_iIntegerEnd = this.m_iDecimalPointPos - 1;
    if (this.FormatType == OfficeFormatType.Number)
      this.m_bGroupDigits = this.CheckGroupDigits();
    this.m_bFormatPrepared = true;
    if (this.m_arrTokens.Count <= 0 || !(this.m_arrTokens[0] is CultureToken arrToken))
      return;
    this.m_bUseSystemDateFormat = arrToken.UseSystemSettings;
  }

  private bool CheckGroupDigits()
  {
    int index1 = 0;
    for (int index2 = this.m_iIntegerEnd - 1; index1 <= index2; ++index1)
    {
      if (this[index1].TokenType == Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.ThousandsSeparator && this[index1 - 1] is DigitToken && this[index1 + 1] is DigitToken)
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
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Minute:
          this.CheckMinuteToken(num);
          break;
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.AmPm:
          HourToken correspondingHourSection = this.FindCorrespondingHourSection(num);
          if (correspondingHourSection != null)
          {
            correspondingHourSection.IsAmPm = true;
            break;
          }
          break;
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Condition:
          this.m_condition = this.m_condition == null ? (ConditionToken) formatTokenBase : throw new FormatException("More than one condition was found in the section.");
          break;
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.SignificantDigit:
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.InsignificantDigit:
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.PlaceReservedDigit:
          DigitToken digitToken = (DigitToken) formatTokenBase;
          if (!flag)
          {
            digitToken.IsLastDigit = true;
            flag = true;
            break;
          }
          break;
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Scientific:
          this.AssignPosition(ref this.m_iScientificPos, num);
          break;
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.DecimalPoint:
          if (this.m_iDecimalPointPos < 0)
          {
            this.AssignPosition(ref this.m_iDecimalPointPos, num);
            break;
          }
          this.m_bMultiplePoints = true;
          break;
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Fraction:
          if (this.m_iFractionPos < 0)
          {
            this.m_iFractionPos = num;
            this.m_bFraction = true;
            break;
          }
          this.m_bFraction = false;
          break;
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Culture:
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
      if (correspondingHourSection.TokenType == Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Hour)
        return (HourToken) correspondingHourSection;
    }
    while (index1 != index);
    return (HourToken) null;
  }

  public string ApplyFormat(double value) => this.ApplyFormat(value, false);

  public string ApplyFormat(string value) => this.ApplyFormat(value, false);

  public string ApplyFormat(double value, bool bShowReservedSymbols)
  {
    this.PrepareFormat();
    this.PrepareValue(ref value, bShowReservedSymbols);
    if (this.m_bUseSystemDateFormat)
      return DateTime.FromOADate(value).ToLongDateString();
    int count = this.Count;
    double dFraction = 0.0;
    double y1 = value != 0.0 ? Math.Floor(Math.Log10(Math.Abs(value))) : value;
    double num1 = value / Math.Pow(10.0, y1);
    if (this.m_iScientificPos > 0)
    {
      int y2 = this.m_iNumberOfIntegerDigits - 1;
      double num2 = num1 * Math.Pow(10.0, (double) y2);
      y1 -= (double) y2;
      value = num2;
    }
    bool bAddNegativeSign = value < 0.0;
    if (this.m_formatType == OfficeFormatType.Number && !this.m_bFraction)
    {
      value = FormatSection.SplitValue(value, out dFraction);
      double num3 = Math.Pow(10.0, (double) this.m_iNumberOfFractionDigits);
      dFraction *= num3;
      dFraction = FormatSection.Round(dFraction);
      if (dFraction >= num3)
      {
        dFraction -= num3;
        if (bAddNegativeSign)
          --value;
        else
          ++value;
      }
    }
    this.PrepareDigits(0, this.m_arrTokens.Count, false);
    if (value == 0.0)
      bAddNegativeSign &= dFraction > 0.0;
    string s = this.ApplyFormat(value, bShowReservedSymbols, 0, this.m_iIntegerEnd, false, this.m_bGroupDigits, bAddNegativeSign);
    if (this.m_iDecimalPointPos > 0)
    {
      int startPos = this.m_iDecimalPointPos + 1;
      int length = dFraction.ToString().Length;
      if (length < this.DecimalNumber && dFraction != 0.0)
        this.PrepareDigits(startPos, startPos + (this.DecimalNumber - length), true);
      s += this.ApplyFormat(dFraction, bShowReservedSymbols, this.m_iDecimalPointPos, this.m_iDecimalEnd, false);
    }
    if (this.m_iScientificPos > 0)
      s += this.ApplyFormat(y1, bShowReservedSymbols, this.m_iDecimalEnd + 1, count - 1, false, false, y1 < 0.0);
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
        long numerator = (long) fraction.Numerator;
        long denumerator = (long) fraction.Denumerator;
        if (s == "1 ")
          s = " ";
        s = s + this.ApplyFormat((double) numerator, bShowReservedSymbols, this.m_iIntegerEnd + 1, this.m_iFractionPos, false) + this.ApplyFormat((double) denumerator, bShowReservedSymbols, this.m_iFractionPos + 1, this.m_iFractionEnd, false) + this.ApplyFormat(0.0, bShowReservedSymbols, this.m_iFractionEnd + 1, count - 1, false);
      }
    }
    Decimal result;
    if (this.m_formatType == OfficeFormatType.General && Decimal.TryParse(s, out result) && s.Length > 9)
    {
      result = Math.Round(result, 9);
      s = result.ToString();
    }
    return s;
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
    if (this.m_formatType != OfficeFormatType.Text && this.m_formatType != OfficeFormatType.Unknown && this.m_formatType != OfficeFormatType.General || this.m_bUseSystemDateFormat)
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
        if (strValue == culture.DateTimeFormat.DateSeparator && arrToken.ApplyFormat(ref num4) != culture.DateTimeFormat.DateSeparator)
          strValue = arrToken.ApplyFormat(ref num4);
        if (strValue != null)
          this.AddToBuilder(builder, bForward, strValue);
      }
    }
    if (num2 >= 0 && bAddNegativeSign)
      builder.Insert(builder.Length - num2, "-");
    return builder.ToString();
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
      if (this[index].TokenType == Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Percent)
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
    if (this.FormatType != OfficeFormatType.Number)
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

  public bool CheckCondition(double value)
  {
    return this.HasCondition && this.m_condition.CheckCondition(value);
  }

  private void DetectFormatType()
  {
    this.m_formatType = OfficeFormatType.Unknown;
    int index = 0;
    for (int length = FormatSection.DEF_POSSIBLE_TOKENS.Length; index < length; index += 2)
    {
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[] arrPossibleTokens = (Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[]) FormatSection.DEF_POSSIBLE_TOKENS[index];
      OfficeFormatType officeFormatType = (OfficeFormatType) FormatSection.DEF_POSSIBLE_TOKENS[index + 1];
      if ((officeFormatType != OfficeFormatType.Number || !this.m_bMultiplePoints) && this.CheckTokenTypes(arrPossibleTokens))
      {
        this.m_formatType = officeFormatType;
        break;
      }
    }
  }

  private bool CheckTokenTypes(Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[] arrPossibleTokens)
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
    if (formatTokenBase.TokenType != Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Minute)
      throw new ArgumentException("Wrong token type.");
    int num;
    if (this.FindTimeToken(iTokenIndex - 1, FormatSection.DEF_BREAK_HOUR, false, Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Hour, Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Hour24) == -1)
      num = this.FindTimeToken(iTokenIndex + 1, FormatSection.DEF_BREAK_SECOND, true, Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Second, Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.SecondTotal) != -1 ? 1 : 0;
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
    Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[] arrBreakTypes,
    bool bForward,
    params Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[] arrTypes)
  {
    int count = this.Count;
    int num = bForward ? 1 : -1;
    for (; iTokenIndex >= 0 && iTokenIndex < count; iTokenIndex += num)
    {
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType tokenType = this[iTokenIndex].TokenType;
      if (Array.IndexOf<Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType>(arrBreakTypes, tokenType) == -1)
      {
        if (Array.IndexOf<Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType>(arrTypes, tokenType) != -1)
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
      if (this[index1].TokenType == Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.DecimalPoint)
      {
        int index2 = index1;
        string empty = string.Empty;
        for (++index1; index1 < count1 && Array.IndexOf<Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType>(FormatSection.DEF_MILLISECONDTOKENS, this[index1].TokenType) != -1; ++index1)
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
      if (formatTokenBase.TokenType == Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Second)
        ((SecondToken) formatTokenBase).RoundValue = false;
    }
  }

  internal bool IsTimeFormat
  {
    get
    {
      if (this.FormatType != OfficeFormatType.DateTime)
        return false;
      bool isTimeFormat = true;
      foreach (FormatTokenBase arrToken in this.m_arrTokens)
      {
        if (Array.IndexOf<Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType>(FormatSection.NotTimeTokens, arrToken.TokenType) >= 0)
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
      if (this.FormatType != OfficeFormatType.DateTime)
        return false;
      bool isDateFormat = true;
      foreach (FormatTokenBase arrToken in this.m_arrTokens)
      {
        if (Array.IndexOf<Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType>(FormatSection.NotDateTokens, arrToken.TokenType) >= 0)
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

  public OfficeFormatType FormatType
  {
    get
    {
      if (this.m_formatType == OfficeFormatType.Unknown)
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

  public bool IsThousandSeparator => this.m_bGroupDigits;

  public int DecimalNumber => this.m_iNumberOfFractionDigits;

  private static bool ContainsIn(Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType[] arrPossibleTokens, Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType token)
  {
    if (arrPossibleTokens == null)
      throw new ArgumentNullException(nameof (arrPossibleTokens));
    int index1 = 0;
    int index2 = arrPossibleTokens.Length - 1;
    while (index2 != index1)
    {
      int index3 = (index2 + index1) / 2;
      Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType arrPossibleToken = arrPossibleTokens[index3];
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
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Condition:
          formatSection.m_condition = (ConditionToken) formatTokenBase;
          break;
        case Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Culture:
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
