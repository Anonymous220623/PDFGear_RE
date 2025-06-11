// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.BoxAndWhiskerSerieFormat
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal struct BoxAndWhiskerSerieFormat
{
  private byte m_options;

  internal bool ShowMeanLine
  {
    get => ((int) this.m_options & 1) == 1;
    set
    {
      if (value)
        this.m_options |= (byte) 1;
      else
        this.m_options &= (byte) 254;
    }
  }

  internal bool ShowMeanMarkers
  {
    get => ((int) this.m_options & 2) == 2;
    set
    {
      if (value)
        this.m_options |= (byte) 2;
      else
        this.m_options &= (byte) 253;
    }
  }

  internal bool ShowInnerPoints
  {
    get => ((int) this.m_options & 4) == 4;
    set
    {
      if (value)
        this.m_options |= (byte) 4;
      else
        this.m_options &= (byte) 251;
    }
  }

  internal bool ShowOutlierPoints
  {
    get => ((int) this.m_options & 8) == 8;
    set
    {
      if (value)
        this.m_options |= (byte) 8;
      else
        this.m_options &= (byte) 247;
    }
  }

  internal QuartileCalculation QuartileCalculationType
  {
    get
    {
      return ((int) this.m_options & 16 /*0x10*/) != 16 /*0x10*/ ? QuartileCalculation.ExclusiveMedian : QuartileCalculation.InclusiveMedian;
    }
    set
    {
      if (value == QuartileCalculation.InclusiveMedian)
        this.m_options |= (byte) 16 /*0x10*/;
      else
        this.m_options &= (byte) 239;
    }
  }

  internal byte Options
  {
    get => this.m_options;
    set => this.m_options = value;
  }
}
