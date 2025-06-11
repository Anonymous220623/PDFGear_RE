// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IThreeDFormat
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IThreeDFormat
{
  Office2007ChartBevelProperties BevelTop { get; set; }

  Office2007ChartBevelProperties BevelBottom { get; set; }

  Office2007ChartMaterialProperties Material { get; set; }

  Office2007ChartLightingProperties Lighting { get; set; }

  int BevelTopHeight { get; set; }

  int BevelTopWidth { get; set; }

  int BevelBottomHeight { get; set; }

  int BevelBottomWidth { get; set; }
}
