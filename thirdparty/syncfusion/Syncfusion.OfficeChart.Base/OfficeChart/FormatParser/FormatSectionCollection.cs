// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.FormatParser.FormatSectionCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.FormatParser.FormatTokens;
using Syncfusion.OfficeChart.Implementation.Collections;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.FormatParser;

internal class FormatSectionCollection : CollectionBaseEx<FormatSection>
{
  private const string DEF_TWO_MANY_SECTIONS_MESSAGE = "Two many sections in format.";
  private const int DEF_CONDITION_MAX_COUNT = 3;
  private const int DEF_NONCONDITION_MAX_COUNT = 4;
  private const int DEF_POSITIVE_SECTION = 0;
  private const int DEF_NEGATIVE_SECTION = 1;
  private const int DEF_ZERO_SECTION = 2;
  private const int DEF_TEXT_SECTION = 3;
  private bool m_bConditionalFormat;

  private FormatSectionCollection(IApplication application, object parent)
    : base(application, parent)
  {
  }

  public FormatSectionCollection(
    IApplication application,
    object parent,
    System.Collections.Generic.List<FormatTokenBase> arrTokens)
    : base(application, parent)
  {
    if (arrTokens == null)
      throw new ArgumentNullException(nameof (arrTokens));
    this.Parse(arrTokens);
  }

  public OfficeFormatType GetFormatType(double value)
  {
    return (this.GetSection(value) ?? throw new FormatException("Can't find required format section.")).FormatType;
  }

  public OfficeFormatType GetFormatType(string value)
  {
    return !this.HasDateTimeFormat() ? this.GetSection(3).FormatType : OfficeFormatType.DateTime;
  }

  private void Parse(System.Collections.Generic.List<FormatTokenBase> arrTokens)
  {
    if (arrTokens == null)
      throw new ArgumentNullException(nameof (arrTokens));
    System.Collections.Generic.List<FormatTokenBase> arrTokens1 = new System.Collections.Generic.List<FormatTokenBase>();
    int index = 0;
    for (int count = arrTokens.Count; index < count; ++index)
    {
      FormatTokenBase arrToken = arrTokens[index];
      if (arrToken.TokenType == Syncfusion.OfficeChart.FormatParser.FormatTokens.TokenType.Section)
      {
        this.InnerList.Add(new FormatSection(this.Application, (object) this, arrTokens1));
        arrTokens1 = new System.Collections.Generic.List<FormatTokenBase>();
      }
      else
        arrTokens1.Add(arrToken);
    }
    this.InnerList.Add(new FormatSection(this.Application, (object) this, arrTokens1));
    if (this[0].HasCondition)
    {
      int num = 0;
      for (int i = 0; i < this.Count; ++i)
      {
        if (this[i].HasCondition)
          ++num;
      }
      if (num > 3)
        throw new FormatException("Two many sections in format.");
      this.m_bConditionalFormat = true;
    }
    else if (this.Count > 4)
      throw new FormatException("Two many sections in format.");
  }

  public string ApplyFormat(double value, bool bShowReservedSymbols)
  {
    FormatSection section = this.GetSection(value);
    if (section == null)
      throw new FormatException("Can't locate correct section.");
    if (!this.m_bConditionalFormat && value < 0.0 && this.Count > 1)
      value = -value;
    return section.ApplyFormat(value, bShowReservedSymbols);
  }

  public string ApplyFormat(string value, bool bShowReservedSymbols)
  {
    FormatSection textSection = this.GetTextSection();
    return textSection == null ? value : textSection.ApplyFormat(value, bShowReservedSymbols);
  }

  private FormatSection GetSection(int iSectionIndex) => this[iSectionIndex % this.Count];

  private FormatSection GetSection(double value)
  {
    FormatSection section = (FormatSection) null;
    if (this.m_bConditionalFormat)
    {
      int count = this.Count;
      for (int i = 0; i < count; ++i)
      {
        FormatSection formatSection = this[i];
        bool hasCondition = formatSection.HasCondition;
        if (!hasCondition || hasCondition && formatSection.CheckCondition(value))
        {
          section = formatSection;
          break;
        }
      }
    }
    else
      section = value <= 0.0 ? (value >= 0.0 ? this.GetZeroSection() : this.GetSection(1)) : this.GetSection(0);
    return section;
  }

  private FormatSection GetZeroSection()
  {
    if (this.m_bConditionalFormat)
      throw new NotSupportedException("This method is not supported for number formats with conditions.");
    int num = this.InnerList.Count - 1;
    FormatSection zeroSection;
    if (num < 2)
      zeroSection = this[0];
    else if (num > 2)
    {
      zeroSection = this[2];
    }
    else
    {
      zeroSection = this[2];
      if (zeroSection.FormatType == OfficeFormatType.Text)
        zeroSection = this[0];
    }
    return zeroSection;
  }

  private FormatSection GetTextSection()
  {
    FormatSection textSection = (FormatSection) null;
    if (!this.m_bConditionalFormat)
    {
      int i = this.InnerList.Count - 1;
      if (i >= 3)
      {
        textSection = this[3];
      }
      else
      {
        textSection = this[i];
        if (textSection.FormatType != OfficeFormatType.Text)
          textSection = this[0];
      }
    }
    return textSection;
  }

  internal bool IsTimeFormat(double value) => value >= 0.0 && this.GetSection(value).IsTimeFormat;

  internal bool IsDateFormat(double value) => value >= 0.0 && this.GetSection(value).IsDateFormat;

  private bool HasDateTimeFormat()
  {
    foreach (FormatSection formatSection in (CollectionBase<FormatSection>) this)
    {
      if (formatSection.FormatType == OfficeFormatType.DateTime)
        return true;
    }
    return false;
  }

  public override object Clone(object parent)
  {
    FormatSectionCollection parent1 = parent != null ? new FormatSectionCollection(this.Application, parent) : throw new ArgumentNullException(nameof (parent));
    System.Collections.Generic.List<FormatSection> innerList1 = this.InnerList;
    System.Collections.Generic.List<FormatSection> innerList2 = parent1.InnerList;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      FormatSection formatSection = (FormatSection) innerList1[index].Clone((object) parent1);
      innerList2.Add(formatSection);
    }
    return (object) parent1;
  }

  internal void Dispose()
  {
    int count = this.InnerList.Count;
    for (int index = 0; index < count; ++index)
    {
      this.InnerList[index].Clear();
      this.InnerList[index] = (FormatSection) null;
    }
  }
}
