// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeFont
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeFont : IParentApplication, IOptimizedUpdate
{
  bool Bold { get; set; }

  OfficeKnownColors Color { get; set; }

  System.Drawing.Color RGBColor { get; set; }

  bool Italic { get; set; }

  bool MacOSOutlineFont { get; set; }

  bool MacOSShadow { get; set; }

  double Size { get; set; }

  bool Strikethrough { get; set; }

  bool Subscript { get; set; }

  bool Superscript { get; set; }

  OfficeUnderline Underline { get; set; }

  string FontName { get; set; }

  OfficeFontVerticalAlignment VerticalAlignment { get; set; }

  bool IsAutoColor { get; }

  Font GenerateNativeFont();
}
