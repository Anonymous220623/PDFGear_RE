// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IFont
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IFont : IParentApplication, IOptimizedUpdate
{
  bool Bold { get; set; }

  ExcelKnownColors Color { get; set; }

  System.Drawing.Color RGBColor { get; set; }

  bool Italic { get; set; }

  bool MacOSOutlineFont { get; set; }

  bool MacOSShadow { get; set; }

  double Size { get; set; }

  bool Strikethrough { get; set; }

  bool Subscript { get; set; }

  bool Superscript { get; set; }

  ExcelUnderline Underline { get; set; }

  string FontName { get; set; }

  ExcelFontVertialAlignment VerticalAlignment { get; set; }

  bool IsAutoColor { get; }

  Font GenerateNativeFont();
}
