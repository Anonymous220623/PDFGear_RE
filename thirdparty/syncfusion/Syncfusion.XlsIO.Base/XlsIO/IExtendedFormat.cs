// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IExtendedFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IExtendedFormat : IParentApplication
{
  IBorders Borders { get; }

  [Obsolete("Use ColorIndex instead of this property.")]
  ExcelKnownColors FillBackground { get; set; }

  [Obsolete("Use Color instead of this property.")]
  Color FillBackgroundRGB { get; set; }

  [Obsolete("Use PatternColorIndex instead of this property.")]
  ExcelKnownColors FillForeground { get; set; }

  [Obsolete("Use PatternColor instead of this property.")]
  Color FillForegroundRGB { get; set; }

  ExcelPattern FillPattern { get; set; }

  IFont Font { get; }

  bool FormulaHidden { get; set; }

  ExcelHAlign HorizontalAlignment { get; set; }

  bool IncludeAlignment { get; set; }

  bool IncludeBorder { get; set; }

  bool IncludeFont { get; set; }

  bool IncludeNumberFormat { get; set; }

  bool IncludePatterns { get; set; }

  bool IncludeProtection { get; set; }

  int IndentLevel { get; set; }

  bool IsFirstSymbolApostrophe { get; set; }

  bool Locked { get; set; }

  bool JustifyLast { get; set; }

  string NumberFormat { get; set; }

  int NumberFormatIndex { get; set; }

  string NumberFormatLocal { get; set; }

  INumberFormat NumberFormatSettings { get; }

  ExcelReadingOrderType ReadingOrder { get; set; }

  int Rotation { get; set; }

  bool ShrinkToFit { get; set; }

  ExcelVAlign VerticalAlignment { get; set; }

  bool WrapText { get; set; }

  ExcelKnownColors PatternColorIndex { get; set; }

  Color PatternColor { get; set; }

  ExcelKnownColors ColorIndex { get; set; }

  Color Color { get; set; }

  bool IsModified { get; }

  bool HasBorder { get; }
}
