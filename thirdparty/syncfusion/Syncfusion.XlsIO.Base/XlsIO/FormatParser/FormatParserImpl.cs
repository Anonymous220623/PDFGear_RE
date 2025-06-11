// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.FormatParser.FormatParserImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.FormatParser.FormatTokens;
using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.FormatParser;

public class FormatParserImpl : CommonObject
{
  internal const string DEF_EXPONENTIAL = "E";
  private const string DEF_HASH = "#";
  private List<FormatTokenBase> m_arrFormatTokens = new List<FormatTokenBase>();
  internal static readonly Regex NumberFormatRegex = new Regex("\\[(DBNUM[1-4]{1}|GB[1-4]{1})\\]", RegexOptions.IgnoreCase);

  public FormatParserImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_arrFormatTokens.Add((FormatTokenBase) new GeneralToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new StringToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new ReservedPlaceToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new DollarToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new CharacterToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new YearToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new MonthToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new DayToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new HourToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new Hour24Token());
    this.m_arrFormatTokens.Add((FormatTokenBase) new MinuteToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new MinuteTotalToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new SecondToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new SecondTotalToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new AmPmToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new SectionSeparatorToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new ColorToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new ConditionToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new TextToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new SignificantDigitToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new InsignificantDigitToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new PlaceReservedDigitToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new PercentToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new DecimalPointToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new ThousandsSeparatorToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new AsterixToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new ScientificToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new FractionToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new CultureToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new UnknownToken());
  }

  public FormatSectionCollection Parse(string strFormat)
  {
    if (strFormat == null)
      throw new ArgumentNullException(nameof (strFormat));
    strFormat = FormatParserImpl.NumberFormatRegex.IsMatch(strFormat) ? FormatParserImpl.NumberFormatRegex.Replace(strFormat, string.Empty) : strFormat;
    int length = strFormat.Length;
    if (length == 0)
      throw new ArgumentException("strFormat - string cannot be empty");
    List<FormatTokenBase> arrTokens = new List<FormatTokenBase>();
    int iIndex = 0;
    while (iIndex < length)
    {
      int index = 0;
      for (int count = this.m_arrFormatTokens.Count; index < count; ++index)
      {
        FormatTokenBase arrFormatToken = this.m_arrFormatTokens[index];
        int num = arrFormatToken.TryParse(strFormat, iIndex);
        if (num > iIndex)
        {
          FormatTokenBase formatTokenBase = (FormatTokenBase) arrFormatToken.Clone();
          iIndex = num;
          arrTokens.Add(formatTokenBase);
          break;
        }
      }
    }
    return new FormatSectionCollection(this.Application, (object) this, arrTokens);
  }

  internal void Clear()
  {
    if (this.m_arrFormatTokens != null)
      this.m_arrFormatTokens.Clear();
    this.m_arrFormatTokens = (List<FormatTokenBase>) null;
  }
}
