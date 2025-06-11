// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IInterior
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IInterior
{
  OfficeKnownColors PatternColorIndex { get; set; }

  Color PatternColor { get; set; }

  OfficeKnownColors ColorIndex { get; set; }

  Color Color { get; set; }

  IGradient Gradient { get; }

  OfficePattern FillPattern { get; set; }
}
