// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.DateStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class DateStyle : TimeStyle
{
  private bool m_automaticOrder;
  private DateBase m_day;
  private DateBase m_dayOfWeek;
  private DateBase m_era;
  private Month m_month;
  private DateBase m_quarter;
  private DateBase m_weekOfYear;
  private DateBase m_year;

  internal bool AutomaticOrder
  {
    get => this.m_automaticOrder;
    set => this.m_automaticOrder = value;
  }

  internal DateBase Day
  {
    get => this.m_day;
    set => this.m_day = value;
  }

  internal DateBase DayOfWeek
  {
    get => this.m_dayOfWeek;
    set => this.m_dayOfWeek = value;
  }

  internal DateBase Era
  {
    get => this.m_era;
    set => this.m_era = value;
  }

  internal Month Month
  {
    get => this.m_month;
    set => this.m_month = value;
  }

  internal DateBase Quarter
  {
    get => this.m_quarter;
    set => this.m_quarter = value;
  }

  internal DateBase WeekOfYear
  {
    get => this.m_weekOfYear;
    set => this.m_weekOfYear = value;
  }

  internal DateBase Year
  {
    get => this.m_year;
    set => this.m_year = value;
  }

  internal void Dispose()
  {
    if (this.m_day != null)
      this.m_day = (DateBase) null;
    if (this.m_dayOfWeek != null)
      this.m_dayOfWeek = (DateBase) null;
    if (this.m_era != null)
      this.m_era = (DateBase) null;
    if (this.m_month != null)
      this.m_month = (Month) null;
    if (this.m_quarter != null)
      this.m_quarter = (DateBase) null;
    if (this.m_weekOfYear != null)
      this.m_weekOfYear = (DateBase) null;
    if (this.m_year != null)
      this.m_year = (DateBase) null;
    this.Dispose1();
  }
}
