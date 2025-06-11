// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.Charts.ChartFillObjectGetter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Interfaces;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;

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
