// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts.ChartFillObjectGetter
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Interfaces;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;

internal class ChartFillObjectGetter : IChartFillObjectGetter
{
  private ChartSerieDataFormatImpl m_parentFormat;

  public ChartFillObjectGetter(ChartSerieDataFormatImpl dataFormat)
  {
    this.m_parentFormat = dataFormat != null ? dataFormat : throw new ArgumentNullException(nameof (dataFormat));
  }

  public ChartBorderImpl Border
  {
    get
    {
      this.m_parentFormat.HasLineProperties = true;
      return this.m_parentFormat.LineProperties as ChartBorderImpl;
    }
  }

  public ChartInteriorImpl Interior
  {
    get
    {
      this.m_parentFormat.HasInterior = true;
      return this.m_parentFormat.Interior as ChartInteriorImpl;
    }
  }

  public IInternalFill Fill
  {
    get
    {
      this.m_parentFormat.HasInterior = true;
      return this.m_parentFormat.Fill as IInternalFill;
    }
  }

  public ShadowImpl Shadow => this.m_parentFormat.Shadow as ShadowImpl;

  public ThreeDFormatImpl ThreeD => this.m_parentFormat.ThreeD as ThreeDFormatImpl;
}
