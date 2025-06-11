// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ShadowData
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Charts;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

public class ShadowData : ICloneable
{
  private readonly ExcelChartType[] WARMMATTE_CHARTS = new ExcelChartType[35]
  {
    ExcelChartType.Pie_3D,
    ExcelChartType.Pie_Exploded_3D,
    ExcelChartType.Bar_Clustered_3D,
    ExcelChartType.Bar_Stacked_3D,
    ExcelChartType.Bar_Stacked_100_3D,
    ExcelChartType.Column_Clustered_3D,
    ExcelChartType.Column_Stacked_3D,
    ExcelChartType.Column_Stacked_100,
    ExcelChartType.Column_Stacked_100_3D,
    ExcelChartType.Column_3D,
    ExcelChartType.Cone_Bar_Clustered,
    ExcelChartType.Cone_Bar_Stacked,
    ExcelChartType.Cone_Bar_Stacked_100,
    ExcelChartType.Cone_Stacked,
    ExcelChartType.Cone_Clustered,
    ExcelChartType.Cone_Clustered_3D,
    ExcelChartType.Cone_Stacked_100,
    ExcelChartType.Cylinder_Bar_Stacked,
    ExcelChartType.Cylinder_Bar_Clustered,
    ExcelChartType.Cylinder_Bar_Stacked_100,
    ExcelChartType.Cylinder_Clustered,
    ExcelChartType.Cylinder_Clustered_3D,
    ExcelChartType.Cylinder_Stacked,
    ExcelChartType.Cylinder_Stacked_100,
    ExcelChartType.Pyramid_Bar_Clustered,
    ExcelChartType.Pyramid_Bar_Stacked,
    ExcelChartType.Pyramid_Bar_Stacked_100,
    ExcelChartType.Pyramid_Clustered,
    ExcelChartType.Pyramid_Clustered_3D,
    ExcelChartType.Pyramid_Stacked,
    ExcelChartType.Pyramid_Stacked_100,
    ExcelChartType.Area_3D,
    ExcelChartType.Area_Stacked_3D,
    ExcelChartType.Area_Stacked_100_3D,
    ExcelChartType.Line_3D
  };
  private ushort m_ShadowOuterPresets;
  private ushort m_ShadowInnerPresets;
  private ushort m_ShadowPrespectivePresets;
  private ushort m_BevelTop;
  private ushort m_BevelBottom;
  private ushort m_Material;
  private ushort m_Lighting;
  private ChartImpl m_chartObject;

  public ShadowData() => this.m_Material = (ushort) 0;

  internal ShadowData(ThreeDFormatImpl parent)
    : this()
  {
    this.m_chartObject = parent.FindParent(typeof (ChartImpl)) as ChartImpl;
  }

  public Excel2007ChartPresetsOuter ShadowOuterPresets
  {
    get => (Excel2007ChartPresetsOuter) this.m_ShadowOuterPresets;
    set => this.m_ShadowOuterPresets = (ushort) value;
  }

  public Excel2007ChartPresetsInner ShadowInnerPresets
  {
    get => (Excel2007ChartPresetsInner) this.m_ShadowInnerPresets;
    set => this.m_ShadowInnerPresets = (ushort) value;
  }

  public Excel2007ChartPresetsPrespective ShadowPrespectivePresets
  {
    get => (Excel2007ChartPresetsPrespective) this.m_ShadowPrespectivePresets;
    set => this.m_ShadowPrespectivePresets = (ushort) value;
  }

  public Excel2007ChartMaterialProperties Material
  {
    get
    {
      return this.m_chartObject != null && !(this.m_chartObject.FindParent(typeof (WorkbookImpl)) as WorkbookImpl).Saving && Array.IndexOf<ExcelChartType>(this.WARMMATTE_CHARTS, this.m_chartObject.ChartType) > -1 && this.m_Material == (ushort) 0 ? Excel2007ChartMaterialProperties.WarmMatte : (Excel2007ChartMaterialProperties) this.m_Material;
    }
    set => this.m_Material = (ushort) value;
  }

  public Excel2007ChartLightingProperties Lighting
  {
    get => (Excel2007ChartLightingProperties) this.m_Lighting;
    set => this.m_Lighting = (ushort) value;
  }

  public Excel2007ChartBevelProperties BevelTop
  {
    get => (Excel2007ChartBevelProperties) this.m_BevelTop;
    set => this.m_BevelTop = (ushort) value;
  }

  public Excel2007ChartBevelProperties BevelBottom
  {
    get => (Excel2007ChartBevelProperties) this.m_BevelBottom;
    set => this.m_BevelBottom = (ushort) value;
  }

  internal ChartImpl ChartObject
  {
    get => this.m_chartObject;
    set => this.m_chartObject = value;
  }

  internal Excel2007ChartMaterialProperties GetMaterial()
  {
    return (Excel2007ChartMaterialProperties) this.m_Material;
  }

  public object Clone() => this.MemberwiseClone();
}
