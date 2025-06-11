// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IShapeLineFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IShapeLineFormat
{
  double Weight { get; set; }

  Color ForeColor { get; set; }

  Color BackColor { get; set; }

  ExcelKnownColors ForeColorIndex { get; set; }

  ExcelKnownColors BackColorIndex { get; set; }

  ExcelShapeArrowStyle BeginArrowHeadStyle { get; set; }

  ExcelShapeArrowStyle EndArrowHeadStyle { get; set; }

  ExcelShapeArrowLength BeginArrowheadLength { get; set; }

  ExcelShapeArrowLength EndArrowheadLength { get; set; }

  ExcelShapeArrowWidth BeginArrowheadWidth { get; set; }

  ExcelShapeArrowWidth EndArrowheadWidth { get; set; }

  ExcelShapeDashLineStyle DashStyle { get; set; }

  ExcelShapeLineStyle Style { get; set; }

  double Transparency { get; set; }

  bool Visible { get; set; }

  ExcelGradientPattern Pattern { get; set; }

  bool HasPattern { get; set; }
}
