// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts.ChartFillObjectGetterAny
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Interfaces;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;

internal class ChartFillObjectGetterAny : IChartFillObjectGetter
{
  private ChartBorderImpl m_border;
  private ChartInteriorImpl m_interior;
  private IInternalFill m_fill;
  private ShadowImpl m_shadow;
  private ThreeDFormatImpl m_threeD;

  public ChartFillObjectGetterAny(
    ChartBorderImpl border,
    ChartInteriorImpl interior,
    IInternalFill fill,
    ShadowImpl shadow,
    ThreeDFormatImpl three_d)
  {
    this.m_border = border;
    this.m_interior = interior;
    this.m_fill = fill;
    this.m_shadow = shadow;
    this.m_threeD = three_d;
  }

  public ChartBorderImpl Border => this.m_border;

  public ChartInteriorImpl Interior => this.m_interior;

  public IInternalFill Fill => this.m_fill;

  public ShadowImpl Shadow => this.m_shadow;

  public ThreeDFormatImpl ThreeD => this.m_threeD;
}
