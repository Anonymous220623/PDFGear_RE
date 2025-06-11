// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IBorder
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IBorder : IParentApplication
{
  OfficeKnownColors Color { get; set; }

  ChartColor ColorObject { get; }

  System.Drawing.Color ColorRGB { get; set; }

  OfficeLineStyle LineStyle { get; set; }

  bool ShowDiagonalLine { get; set; }
}
