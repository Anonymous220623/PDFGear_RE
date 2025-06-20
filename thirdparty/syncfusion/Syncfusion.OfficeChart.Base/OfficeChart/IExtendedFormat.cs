﻿// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IExtendedFormat
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IExtendedFormat : IParentApplication
{
  IBorders Borders { get; }

  [Obsolete("Use ColorIndex instead of this property.")]
  OfficeKnownColors FillBackground { get; set; }

  [Obsolete("Use Color instead of this property.")]
  Color FillBackgroundRGB { get; set; }

  [Obsolete("Use PatternColorIndex instead of this property.")]
  OfficeKnownColors FillForeground { get; set; }

  [Obsolete("Use PatternColor instead of this property.")]
  Color FillForegroundRGB { get; set; }

  OfficePattern FillPattern { get; set; }

  IOfficeFont Font { get; }

  bool FormulaHidden { get; set; }

  OfficeHAlign HorizontalAlignment { get; set; }

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

  OfficeReadingOrderType ReadingOrder { get; set; }

  int Rotation { get; set; }

  bool ShrinkToFit { get; set; }

  OfficeVAlign VerticalAlignment { get; set; }

  bool WrapText { get; set; }

  OfficeKnownColors PatternColorIndex { get; set; }

  Color PatternColor { get; set; }

  OfficeKnownColors ColorIndex { get; set; }

  Color Color { get; set; }

  bool IsModified { get; }

  bool HasBorder { get; }
}
