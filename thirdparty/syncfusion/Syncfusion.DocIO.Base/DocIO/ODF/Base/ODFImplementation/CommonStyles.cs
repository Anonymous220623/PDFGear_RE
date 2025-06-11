// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.CommonStyles
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class CommonStyles
{
  private List<NumberStyle> m_numbeStyles;
  private List<CurrencyStyle> m_currencyStyles;
  private List<PercentageStyle> m_percentageStyles;
  private List<DateStyle> m_dateStyles;
  private List<TimeStyle> m_timeStyles;
  private List<BooleanStyle> m_booleanStyles;
  private List<TextStyle> m_textStyles;
  private List<DefaultStyle> m_defaultStyles;
  private ODFStyleCollection m_odfStyles;

  internal List<NumberStyle> NumbeStyles
  {
    get
    {
      if (this.m_numbeStyles == null)
        this.m_numbeStyles = new List<NumberStyle>();
      return this.m_numbeStyles;
    }
    set => this.m_numbeStyles = value;
  }

  internal List<CurrencyStyle> CurrencyStyles
  {
    get
    {
      if (this.m_currencyStyles == null)
        this.m_currencyStyles = new List<CurrencyStyle>();
      return this.m_currencyStyles;
    }
    set => this.m_currencyStyles = value;
  }

  internal List<PercentageStyle> PercentageStyles
  {
    get
    {
      if (this.m_percentageStyles == null)
        this.m_percentageStyles = new List<PercentageStyle>();
      return this.m_percentageStyles;
    }
    set => this.m_percentageStyles = value;
  }

  internal List<DateStyle> DateStyles
  {
    get
    {
      if (this.m_dateStyles == null)
        this.m_dateStyles = new List<DateStyle>();
      return this.m_dateStyles;
    }
    set => this.m_dateStyles = value;
  }

  internal List<TimeStyle> TimeStyles
  {
    get
    {
      if (this.m_timeStyles == null)
        this.m_timeStyles = new List<TimeStyle>();
      return this.m_timeStyles;
    }
    set => this.m_timeStyles = value;
  }

  internal List<BooleanStyle> BooleanStyles
  {
    get
    {
      if (this.m_booleanStyles == null)
        this.m_booleanStyles = new List<BooleanStyle>();
      return this.m_booleanStyles;
    }
    set => this.m_booleanStyles = value;
  }

  internal List<TextStyle> TextStyles
  {
    get
    {
      if (this.m_textStyles == null)
        this.m_textStyles = new List<TextStyle>();
      return this.m_textStyles;
    }
    set => this.m_textStyles = value;
  }

  internal List<DefaultStyle> DefaultStyles
  {
    get
    {
      if (this.m_defaultStyles == null)
        this.m_defaultStyles = new List<DefaultStyle>();
      return this.m_defaultStyles;
    }
    set => this.m_defaultStyles = value;
  }

  internal ODFStyleCollection OdfStyles
  {
    get
    {
      if (this.m_odfStyles == null)
        this.m_odfStyles = new ODFStyleCollection();
      return this.m_odfStyles;
    }
    set => this.m_odfStyles = value;
  }
}
