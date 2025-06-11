// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.DxfImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Interfaces;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders;

public class DxfImpl
{
  private BordersCollection m_borders;
  private FillImpl m_fill;
  private FontImpl m_font;
  private FormatImpl m_format;
  private ColorObject m_verticalColorObject = new ColorObject(ExcelKnownColors.Black);
  private ColorObject m_horizontalColorObject = new ColorObject(ExcelKnownColors.Black);
  private ExcelLineStyle m_verticalBorderStyle;
  private ExcelLineStyle m_horizontalBorderStyle;
  private bool m_isVerticalBorderModified;
  private bool m_isHorizontalBorderModified;
  private byte m_btIndent;
  private ushort m_readingOrder;
  private ushort m_rotation;
  private bool m_bShrinkToFit;
  private bool m_bWrapText;
  private ExcelHAlign m_hAlignment;
  private ExcelVAlign m_vAlignment;
  private bool m_bLocked = true;
  private bool m_bHidden;
  private bool m_bHasAlignment;
  private bool m_bHasProtection;

  public FormatImpl FormatRecord
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  public FillImpl Fill
  {
    get => this.m_fill;
    set => this.m_fill = value;
  }

  public FontImpl Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  public BordersCollection Borders
  {
    get => this.m_borders;
    set => this.m_borders = value;
  }

  internal ColorObject VerticalColorObject
  {
    get => this.m_verticalColorObject;
    set => this.m_verticalColorObject = value;
  }

  internal ColorObject HorizontalColorObject
  {
    get => this.m_horizontalColorObject;
    set => this.m_horizontalColorObject = value;
  }

  internal ExcelLineStyle VerticalBorderStyle
  {
    get => this.m_verticalBorderStyle;
    set => this.m_verticalBorderStyle = value;
  }

  internal ExcelLineStyle HorizontalBorderStyle
  {
    get => this.m_horizontalBorderStyle;
    set => this.m_horizontalBorderStyle = value;
  }

  internal bool IsHorizontalBorderModified
  {
    get => this.m_isHorizontalBorderModified;
    set => this.m_isHorizontalBorderModified = value;
  }

  internal bool IsVerticalBorderModified
  {
    get => this.m_isVerticalBorderModified;
    set => this.m_isVerticalBorderModified = value;
  }

  internal ExcelHAlign HAlignmentType
  {
    get => this.m_hAlignment;
    set => this.m_hAlignment = value;
  }

  internal ExcelVAlign VAlignmentType
  {
    get => this.m_vAlignment;
    set => this.m_vAlignment = value;
  }

  internal bool WrapText
  {
    get => this.m_bWrapText;
    set => this.m_bWrapText = value;
  }

  internal byte Indent
  {
    get => this.m_btIndent;
    set => this.m_btIndent = value;
  }

  internal bool ShrinkToFit
  {
    get => this.m_bShrinkToFit;
    set => this.m_bShrinkToFit = value;
  }

  internal ushort ReadingOrder
  {
    get => this.m_readingOrder;
    set
    {
      this.m_readingOrder = value <= (ushort) 3 ? value : throw new ArgumentOutOfRangeException("Reading Order");
    }
  }

  internal ushort Rotation
  {
    get => this.m_rotation;
    set
    {
      this.m_rotation = value <= (ushort) byte.MaxValue ? value : throw new ArgumentOutOfRangeException(nameof (Rotation));
    }
  }

  internal bool IsLocked
  {
    get => this.m_bLocked;
    set => this.m_bLocked = value;
  }

  internal bool IsHidden
  {
    get => this.m_bHidden;
    set => this.m_bHidden = value;
  }

  internal bool HasAlignment
  {
    get => this.m_bHasAlignment;
    set => this.m_bHasAlignment = value;
  }

  internal bool HasProtection
  {
    get => this.m_bHasProtection;
    set => this.m_bHasProtection = value;
  }

  internal void FillCondition(IInternalConditionalFormat conFormat)
  {
    if (this.m_format != null)
      conFormat.NumberFormat = this.m_format.FormatString;
    if (this.m_fill != null)
    {
      conFormat.FillPattern = this.m_fill.Pattern;
      if (!this.m_fill.IsDxfPatternNone)
      {
        conFormat.BackColorObject.CopyFrom(this.m_fill.PatternColorObject, true);
        conFormat.ColorObject.CopyFrom(this.m_fill.ColorObject, true);
      }
      conFormat.IsPatternFormatPresent = true;
    }
    if (this.m_font != null)
    {
      if (this.m_font.Color != (ExcelKnownColors) 32767 /*0x7FFF*/)
        conFormat.FontColorObject.CopyFrom(this.m_font.ColorObject, true);
      conFormat.IsBold = this.m_font.Bold;
      conFormat.IsItalic = this.m_font.Italic;
      conFormat.IsStrikeThrough = this.m_font.Strikethrough;
      conFormat.IsSubScript = this.m_font.Subscript;
      conFormat.IsSuperScript = this.m_font.Superscript;
      conFormat.Underline = this.m_font.Underline;
    }
    if (this.m_borders == null)
      return;
    BorderSettingsHolder border1 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeBottom];
    if (border1 != null)
    {
      conFormat.BottomBorderColorObject.CopyFrom(border1.ColorObject, true);
      conFormat.BottomBorderStyle = border1.LineStyle;
    }
    BorderSettingsHolder border2 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeLeft];
    if (border2 != null)
    {
      conFormat.LeftBorderColorObject.CopyFrom(border2.ColorObject, true);
      conFormat.LeftBorderStyle = border2.LineStyle;
    }
    BorderSettingsHolder border3 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeRight];
    if (border3 != null)
    {
      conFormat.RightBorderColorObject.CopyFrom(border3.ColorObject, true);
      conFormat.RightBorderStyle = border3.LineStyle;
    }
    BorderSettingsHolder border4 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeTop];
    if (border4 == null)
      return;
    conFormat.TopBorderColorObject.CopyFrom(border4.ColorObject, true);
    conFormat.TopBorderStyle = border4.LineStyle;
  }

  internal void FillTableStyle(TableStyleElement tableStyleElement)
  {
    if (this.m_fill != null)
    {
      tableStyleElement.PatternStyle = this.m_fill.Pattern;
      tableStyleElement.BackColorObject.CopyFrom(this.m_fill.PatternColorObject, true);
      tableStyleElement.ColorObject.CopyFrom(this.m_fill.ColorObject, true);
      tableStyleElement.IsPatternFormatPresent = true;
    }
    if (this.m_font != null)
    {
      if (this.m_font.Color != (ExcelKnownColors) 32767 /*0x7FFF*/)
        tableStyleElement.FontColorObject.CopyFrom(this.m_font.ColorObject, true);
      tableStyleElement.Bold = this.m_font.Bold;
      tableStyleElement.Italic = this.m_font.Italic;
      tableStyleElement.StrikeThrough = this.m_font.Strikethrough;
      tableStyleElement.Underline = this.m_font.Underline;
    }
    if (this.m_borders == null)
      return;
    BorderSettingsHolder border1 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeBottom];
    if (border1 != null)
    {
      tableStyleElement.BottomBorderColorObject.CopyFrom(border1.ColorObject, true);
      tableStyleElement.BottomBorderStyle = border1.LineStyle;
    }
    BorderSettingsHolder border2 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeLeft];
    if (border2 != null)
    {
      tableStyleElement.LeftBorderColorObject.CopyFrom(border2.ColorObject, true);
      tableStyleElement.LeftBorderStyle = border2.LineStyle;
    }
    BorderSettingsHolder border3 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeRight];
    if (border3 != null)
    {
      tableStyleElement.RightBorderColorObject.CopyFrom(border3.ColorObject, true);
      tableStyleElement.RightBorderStyle = border3.LineStyle;
    }
    BorderSettingsHolder border4 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeTop];
    if (border4 != null)
    {
      tableStyleElement.TopBorderColorObject.CopyFrom(border4.ColorObject, true);
      tableStyleElement.TopBorderStyle = border4.LineStyle;
    }
    if (this.m_isVerticalBorderModified)
    {
      tableStyleElement.VerticalBorderColorObject = this.m_verticalColorObject;
      tableStyleElement.VerticalBorderStyle = this.m_verticalBorderStyle;
    }
    if (!this.m_isHorizontalBorderModified)
      return;
    tableStyleElement.HorizontalBorderColorObject = this.m_horizontalColorObject;
    tableStyleElement.HorizontalBorderStyle = this.m_horizontalBorderStyle;
  }

  internal void FillPivotCellFormat(IInternalPivotCellFormat pivotCellFormat)
  {
    if (this.m_bHasAlignment)
    {
      pivotCellFormat.HorizontalAlignment = this.m_hAlignment;
      pivotCellFormat.VerticalAlignment = this.m_vAlignment;
      pivotCellFormat.WrapText = this.m_bWrapText;
      pivotCellFormat.ShrinkToFit = this.m_bShrinkToFit;
      pivotCellFormat.IndentLevel = (int) this.m_btIndent;
      pivotCellFormat.ReadingOrder = (ExcelReadingOrderType) this.m_readingOrder;
      pivotCellFormat.Rotation = (int) this.m_rotation;
      pivotCellFormat.IncludeAlignment = this.m_bHasAlignment;
    }
    if (this.m_bHasAlignment)
    {
      pivotCellFormat.Locked = this.m_bLocked;
      pivotCellFormat.FormulaHidden = this.m_bHidden;
      pivotCellFormat.IncludeProtection = this.m_bHasProtection;
    }
    if (this.m_format != null)
      pivotCellFormat.NumberFormat = this.m_format.FormatString;
    if (this.m_fill != null)
    {
      pivotCellFormat.PatternStyle = this.m_fill.Pattern;
      pivotCellFormat.BackColorObject.CopyFrom(this.m_fill.PatternColorObject, true);
      pivotCellFormat.ColorObject.CopyFrom(this.m_fill.ColorObject, true);
      pivotCellFormat.IsPatternFormatPresent = true;
    }
    if (this.m_font != null)
    {
      if (this.m_font.Color != (ExcelKnownColors) 32767 /*0x7FFF*/)
      {
        pivotCellFormat.FontColorObject.CopyFrom(this.m_font.ColorObject, true);
        pivotCellFormat.IsFontColorPresent = true;
        pivotCellFormat.IsFontFormatPresent = true;
      }
      pivotCellFormat.Bold = this.m_font.Bold;
      pivotCellFormat.Italic = this.m_font.Italic;
      pivotCellFormat.StrikeThrough = this.m_font.Strikethrough;
      pivotCellFormat.Underline = this.m_font.Underline;
      pivotCellFormat.FontSize = this.m_font.Size;
    }
    if (this.m_borders == null)
      return;
    BorderSettingsHolder border1 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeBottom];
    if (border1 != null)
    {
      pivotCellFormat.BottomBorderColorObject.CopyFrom(border1.ColorObject, true);
      pivotCellFormat.BottomBorderStyle = border1.LineStyle;
    }
    BorderSettingsHolder border2 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeLeft];
    if (border2 != null)
    {
      pivotCellFormat.LeftBorderColorObject.CopyFrom(border2.ColorObject, true);
      pivotCellFormat.LeftBorderStyle = border2.LineStyle;
    }
    BorderSettingsHolder border3 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeRight];
    if (border3 != null)
    {
      pivotCellFormat.RightBorderColorObject.CopyFrom(border3.ColorObject, true);
      pivotCellFormat.RightBorderStyle = border3.LineStyle;
    }
    BorderSettingsHolder border4 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.EdgeTop];
    if (border4 != null)
    {
      pivotCellFormat.TopBorderColorObject.CopyFrom(border4.ColorObject, true);
      pivotCellFormat.TopBorderStyle = border4.LineStyle;
    }
    BorderSettingsHolder border5 = (BorderSettingsHolder) this.m_borders[ExcelBordersIndex.DiagonalUp];
    if (border5 != null && border5.ShowDiagonalLine)
    {
      pivotCellFormat.DiagonalBorderColorObject.CopyFrom(border5.ColorObject, true);
      pivotCellFormat.DiagonalBorderStyle = border5.LineStyle;
    }
    if (this.m_isVerticalBorderModified)
    {
      pivotCellFormat.VerticalBorderColorObject.CopyFrom(this.m_verticalColorObject, true);
      pivotCellFormat.VerticalBorderStyle = this.m_verticalBorderStyle;
    }
    if (!this.m_isHorizontalBorderModified)
      return;
    pivotCellFormat.HorizontalBorderColorObject.CopyFrom(this.m_horizontalColorObject, true);
    pivotCellFormat.HorizontalBorderStyle = this.m_horizontalBorderStyle;
  }

  internal void FillSorting(ISortField sortField)
  {
    if (this.m_fill != null)
      sortField.Color = ColorExtension.FromArgb(this.m_fill.ColorObject.Value);
    if (this.m_font == null)
      return;
    sortField.Color = ColorExtension.FromArgb(this.m_font.ColorObject.Value);
  }

  public DxfImpl Clone(WorkbookImpl book)
  {
    DxfImpl dxfImpl = (DxfImpl) this.MemberwiseClone();
    if (this.m_borders != null)
      dxfImpl.m_borders = (BordersCollection) this.m_borders.Clone((object) book);
    if (this.m_fill != null)
      dxfImpl.m_fill = this.m_fill.Clone();
    if (this.m_font != null)
      dxfImpl.m_font = this.m_font.Clone((object) book);
    if (this.m_format != null)
      dxfImpl.m_format = (FormatImpl) this.m_format.Clone((object) book.InnerFormats);
    return dxfImpl;
  }
}
