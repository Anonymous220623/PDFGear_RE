// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.TimeStyle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class TimeStyle : DataStyle
{
  private string m_ampm;
  private TimeBase m_hours;
  private TimeBase m_minutes;
  private Seconds m_seconds;

  internal string AMPM
  {
    get => this.m_ampm;
    set => this.m_ampm = value;
  }

  internal TimeBase Hours
  {
    get => this.m_hours;
    set => this.m_hours = value;
  }

  internal TimeBase Minutes
  {
    get => this.m_minutes;
    set => this.m_minutes = value;
  }

  internal Seconds Seconds
  {
    get => this.m_seconds;
    set => this.m_seconds = value;
  }

  internal TimeStyle()
  {
  }
}
