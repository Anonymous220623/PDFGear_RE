// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ShadowData
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

internal class ShadowData : ICloneable
{
  private ushort m_ShadowOuterPresets;
  private ushort m_ShadowInnerPresets;
  private ushort m_ShadowPrespectivePresets;
  private ushort m_BevelTop;
  private ushort m_BevelBottom;
  private ChartImpl m_chartObject;
  private ushort m_Material;
  private ushort m_Lighting;

  public Office2007ChartPresetsOuter ShadowOuterPresets
  {
    get => (Office2007ChartPresetsOuter) this.m_ShadowOuterPresets;
    set => this.m_ShadowOuterPresets = (ushort) value;
  }

  public Office2007ChartPresetsInner ShadowInnerPresets
  {
    get => (Office2007ChartPresetsInner) this.m_ShadowInnerPresets;
    set => this.m_ShadowInnerPresets = (ushort) value;
  }

  public Office2007ChartPresetsPerspective ShadowPrespectivePresets
  {
    get => (Office2007ChartPresetsPerspective) this.m_ShadowPrespectivePresets;
    set => this.m_ShadowPrespectivePresets = (ushort) value;
  }

  public Office2007ChartMaterialProperties Material
  {
    get => (Office2007ChartMaterialProperties) this.m_Material;
    set => this.m_Material = (ushort) value;
  }

  public Office2007ChartLightingProperties Lighting
  {
    get => (Office2007ChartLightingProperties) this.m_Lighting;
    set => this.m_Lighting = (ushort) value;
  }

  public Office2007ChartBevelProperties BevelTop
  {
    get => (Office2007ChartBevelProperties) this.m_BevelTop;
    set => this.m_BevelTop = (ushort) value;
  }

  public Office2007ChartBevelProperties BevelBottom
  {
    get => (Office2007ChartBevelProperties) this.m_BevelBottom;
    set => this.m_BevelBottom = (ushort) value;
  }

  public object Clone() => this.MemberwiseClone();

  internal ChartImpl ChartObject
  {
    get => this.m_chartObject;
    set => this.m_chartObject = value;
  }
}
