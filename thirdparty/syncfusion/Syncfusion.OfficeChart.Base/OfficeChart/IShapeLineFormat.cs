// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IShapeLineFormat
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IShapeLineFormat
{
  double Weight { get; set; }

  Color ForeColor { get; set; }

  Color BackColor { get; set; }

  OfficeKnownColors ForeColorIndex { get; set; }

  OfficeKnownColors BackColorIndex { get; set; }

  OfficeShapeArrowStyle BeginArrowHeadStyle { get; set; }

  OfficeShapeArrowStyle EndArrowHeadStyle { get; set; }

  OfficeShapeArrowLength BeginArrowheadLength { get; set; }

  OfficeShapeArrowLength EndArrowheadLength { get; set; }

  OfficeShapeArrowWidth BeginArrowheadWidth { get; set; }

  OfficeShapeArrowWidth EndArrowheadWidth { get; set; }

  OfficeShapeDashLineStyle DashStyle { get; set; }

  OfficeShapeLineStyle Style { get; set; }

  double Transparency { get; set; }

  bool Visible { get; set; }

  OfficeGradientPattern Pattern { get; set; }

  bool HasPattern { get; set; }
}
