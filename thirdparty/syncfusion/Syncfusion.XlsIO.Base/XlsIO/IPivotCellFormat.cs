// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IPivotCellFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IPivotCellFormat
{
  ExcelKnownColors BackColor { get; set; }

  Color BackColorRGB { get; set; }

  ExcelPattern PatternStyle { get; set; }

  Color PatternColorRGB { get; set; }

  ExcelKnownColors PatternColor { get; set; }

  ExcelKnownColors FontColor { get; set; }

  Color FontColorRGB { get; set; }

  double FontSize { get; set; }

  string FontName { get; set; }

  bool Bold { get; set; }

  bool Italic { get; set; }

  ExcelUnderline Underline { get; set; }

  bool StrikeThrough { get; set; }

  ExcelKnownColors TopBorderColor { get; set; }

  Color TopBorderColorRGB { get; set; }

  ExcelLineStyle TopBorderStyle { get; set; }

  ExcelKnownColors HorizontalBorderColor { get; set; }

  Color HorizontalBorderColorRGB { get; set; }

  ExcelLineStyle HorizontalBorderStyle { get; set; }

  ExcelKnownColors VerticalBorderColor { get; set; }

  Color VerticalBorderColorRGB { get; set; }

  ExcelLineStyle VerticalBorderStyle { get; set; }

  ExcelKnownColors BottomBorderColor { get; set; }

  Color BottomBorderColorRGB { get; set; }

  ExcelLineStyle BottomBorderStyle { get; set; }

  ExcelKnownColors RightBorderColor { get; set; }

  Color RightBorderColorRGB { get; set; }

  ExcelLineStyle RightBorderStyle { get; set; }

  ExcelKnownColors LeftBorderColor { get; set; }

  Color LeftBorderColorRGB { get; set; }

  ExcelLineStyle LeftBorderStyle { get; set; }

  ExcelHAlign HorizontalAlignment { get; set; }

  int IndentLevel { get; set; }

  ExcelVAlign VerticalAlignment { get; set; }

  ExcelKnownColors DiagonalBorderColor { get; set; }

  Color DiagonalBorderColorRGB { get; set; }

  ExcelLineStyle DiagonalBorderStyle { get; set; }

  ExcelReadingOrderType ReadingOrder { get; set; }

  bool WrapText { get; set; }

  bool ShrinkToFit { get; set; }

  bool Locked { get; set; }

  bool FormulaHidden { get; set; }

  int Rotation { get; set; }

  ushort NumberFormatIndex { get; set; }

  string NumberFormat { get; set; }
}
