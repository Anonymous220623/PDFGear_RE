// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IConditionalFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IConditionalFormat : IParentApplication, IOptimizedUpdate
{
  ExcelCFType FormatType { get; set; }

  CFTimePeriods TimePeriodType { get; set; }

  ExcelComparisonOperator Operator { get; set; }

  bool IsBold { get; set; }

  bool IsItalic { get; set; }

  ExcelKnownColors FontColor { get; set; }

  System.Drawing.Color FontColorRGB { get; set; }

  ExcelUnderline Underline { get; set; }

  bool IsStrikeThrough { get; set; }

  ExcelKnownColors LeftBorderColor { get; set; }

  System.Drawing.Color LeftBorderColorRGB { get; set; }

  ExcelLineStyle LeftBorderStyle { get; set; }

  ExcelKnownColors RightBorderColor { get; set; }

  System.Drawing.Color RightBorderColorRGB { get; set; }

  ExcelLineStyle RightBorderStyle { get; set; }

  ExcelKnownColors TopBorderColor { get; set; }

  System.Drawing.Color TopBorderColorRGB { get; set; }

  ExcelLineStyle TopBorderStyle { get; set; }

  ExcelKnownColors BottomBorderColor { get; set; }

  System.Drawing.Color BottomBorderColorRGB { get; set; }

  ExcelLineStyle BottomBorderStyle { get; set; }

  string FirstFormula { get; set; }

  string FirstFormulaR1C1 { get; set; }

  string SecondFormula { get; set; }

  string SecondFormulaR1C1 { get; set; }

  ExcelKnownColors Color { get; set; }

  System.Drawing.Color ColorRGB { get; set; }

  ExcelKnownColors BackColor { get; set; }

  System.Drawing.Color BackColorRGB { get; set; }

  ExcelPattern FillPattern { get; set; }

  bool IsSuperScript { get; set; }

  bool IsSubScript { get; set; }

  bool IsFontFormatPresent { get; set; }

  bool IsBorderFormatPresent { get; set; }

  bool IsPatternFormatPresent { get; set; }

  bool IsFontColorPresent { get; set; }

  bool IsPatternColorPresent { get; set; }

  bool IsBackgroundColorPresent { get; set; }

  bool IsLeftBorderModified { get; set; }

  bool IsRightBorderModified { get; set; }

  bool IsTopBorderModified { get; set; }

  bool IsBottomBorderModified { get; set; }

  IDataBar DataBar { get; }

  IIconSet IconSet { get; }

  IColorScale ColorScale { get; }

  string NumberFormat { get; set; }

  bool StopIfTrue { get; set; }

  string Text { get; set; }

  ITopBottom TopBottom { get; }

  IAboveBelowAverage AboveBelowAverage { get; }
}
