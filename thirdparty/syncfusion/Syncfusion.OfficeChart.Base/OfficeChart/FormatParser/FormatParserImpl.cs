// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatParserImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.FormatParser.FormatTokens;
using Syncfusion.OfficeChart.Implementation;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser;

internal class FormatParserImpl : CommonObject
{
  internal const string DEF_EXPONENTIAL = "E";
  private const string DEF_HASH = "#";
  private List<FormatTokenBase> m_arrFormatTokens = new List<FormatTokenBase>();

  public FormatParserImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_arrFormatTokens.Add((FormatTokenBase) new GeneralToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new StringToken());
    this.m_arrFormatTokens.Add((FormatTokenBase) new ReservedPlaceToken());
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
    int num1 = strFormat != null ? strFormat.Length : throw new ArgumentNullException(nameof (strFormat));
    if (num1 == 0)
      throw new ArgumentException("strFormat - string cannot be empty");
    List<FormatTokenBase> arrTokens = new List<FormatTokenBase>();
    int iIndex = 0;
    while (iIndex < num1)
    {
      int index = 0;
      for (int count = this.m_arrFormatTokens.Count; index < count; ++index)
      {
        FormatTokenBase arrFormatToken = this.m_arrFormatTokens[index];
        int num2 = arrFormatToken.TryParse(strFormat, iIndex);
        if (num2 > iIndex)
        {
          FormatTokenBase formatTokenBase = (FormatTokenBase) arrFormatToken.Clone();
          if (formatTokenBase.TokenType == Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Section && arrTokens.Count == 0)
            arrTokens.Add(this.m_arrFormatTokens[17]);
          else if (arrTokens.Count > 0 && arrTokens[arrTokens.Count - 1].TokenType == Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Section && formatTokenBase.TokenType == Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Section)
            arrTokens.Add(this.m_arrFormatTokens[17]);
          iIndex = num2;
          arrTokens.Add(formatTokenBase);
          break;
        }
      }
    }
    if (arrTokens.Count > 0 && arrTokens[arrTokens.Count - 1].TokenType == Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Section)
      arrTokens.Add(this.m_arrFormatTokens[17]);
    return new FormatSectionCollection(this.Application, (object) this, arrTokens);
  }

  internal void Clear()
  {
    if (this.m_arrFormatTokens != null)
      this.m_arrFormatTokens.Clear();
    this.m_arrFormatTokens = (List<FormatTokenBase>) null;
  }
}
