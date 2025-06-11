// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IInternalPivotCellFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;

#nullable disable
namespace Syncfusion.XlsIO;

internal interface IInternalPivotCellFormat : IPivotCellFormat
{
  ColorObject ColorObject { get; }

  ColorObject BackColorObject { get; }

  ColorObject TopBorderColorObject { get; }

  ColorObject BottomBorderColorObject { get; }

  ColorObject LeftBorderColorObject { get; }

  ColorObject RightBorderColorObject { get; }

  ColorObject HorizontalBorderColorObject { get; }

  ColorObject VerticalBorderColorObject { get; }

  ColorObject DiagonalBorderColorObject { get; }

  ColorObject FontColorObject { get; }

  bool IsNumberFormatPresent { get; set; }

  bool IncludeAlignment { get; set; }

  bool IncludeProtection { get; set; }

  bool IsBackgroundColorPresent { get; set; }

  bool IsBorderFormatPresent { get; set; }

  bool IsFontColorPresent { get; set; }

  bool IsTopBorderModified { get; set; }

  bool IsBottomBorderModified { get; set; }

  bool IsRightBorderModified { get; set; }

  bool IsLeftBorderModified { get; set; }

  bool IsVerticalBorderModified { get; set; }

  bool IsHorizontalBorderModified { get; set; }

  bool IsDiagonalBorderModified { get; set; }

  bool IsFontFormatPresent { get; set; }

  bool IsPatternColorModified { get; set; }

  bool IsPatternFormatPresent { get; set; }
}
