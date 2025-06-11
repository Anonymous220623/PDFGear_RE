// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExtendedFormatImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ExtendedFormatImpl : 
  CommonObject,
  IInternalExtendedFormat,
  IExtendedFormat,
  IParentApplication,
  IComparable,
  ICloneable,
  IXFIndex,
  ICloneParent,
  IDisposable
{
  public const int DEF_NO_PARENT_INDEX = 4095 /*0x0FFF*/;
  public const int TopToBottomRotation = 255 /*0xFF*/;
  public const int MaxTintValue = 32767 /*0x7FFF*/;
  internal ushort FONTBOLD = 700;
  internal ushort FONTNORMAL = 400;
  private uint m_cfApplied;
  private ExtendedFormatRecord m_extFormat;
  private ExtendedXFRecord m_xfExt;
  private WorkbookImpl m_book;
  private int m_iXFIndex;
  private ShapeFillImpl m_gradient;
  private ColorObject m_color;
  private ColorObject m_patternColor;
  private ColorObject m_topBorderColor;
  private ColorObject m_bottomBorderColor;
  private ColorObject m_leftBorderColor;
  private ColorObject m_rightBorderColor;
  private ColorObject m_diagonalBorderColor;
  private bool m_hasBorder;
  private bool m_pivotButton;
  private bool isVerticalText;
  internal bool m_bisValid;
  private int m_cfPriority;
  private bool m_bIsPivotFormat;

  public int FontIndex
  {
    get => !this.IncludeFont ? (int) this.ParentRecord.FontIndex : (int) this.m_extFormat.FontIndex;
    set
    {
      if (this.FontIndex == value)
        return;
      this.IncludeFont = true;
      this.m_extFormat.FontIndex = (ushort) value;
      this.SetChanged();
    }
  }

  public int XFormatIndex => this.Index;

  public int NumberFormatIndex
  {
    get
    {
      return !this.IncludeNumberFormat ? (int) this.ParentRecord.FormatIndex : (int) this.m_extFormat.FormatIndex;
    }
    set
    {
      if (!this.m_book.InnerFormats.Contains(value))
        throw new ArgumentOutOfRangeException("Unknown format index");
      if (this.NumberFormatIndex != value)
      {
        this.IncludeNumberFormat = true;
        this.m_extFormat.FormatIndex = (ushort) value;
      }
      this.SetChanged();
    }
  }

  public ExcelPattern FillPattern
  {
    get
    {
      return !this.IncludePatterns ? (ExcelPattern) this.ParentRecord.AdtlFillPattern : (ExcelPattern) this.m_extFormat.AdtlFillPattern;
    }
    set
    {
      if (this.FillPattern == value)
        return;
      this.IncludePatterns = true;
      if (value == ExcelPattern.None)
      {
        this.ColorIndex = ExcelKnownColors.None;
        this.PatternColorIndex = ExcelKnownColors.BlackCustom;
      }
      this.m_extFormat.AdtlFillPattern = (ushort) value;
      this.SetChanged();
    }
  }

  public ExcelKnownColors FillBackground
  {
    get => this.ColorIndex;
    set => this.ColorIndex = value;
  }

  public Color FillBackgroundRGB
  {
    get => this.Color;
    set => this.Color = value;
  }

  public ExcelKnownColors FillForeground
  {
    get => this.PatternColorIndex;
    set => this.PatternColorIndex = value;
  }

  public Color FillForegroundRGB
  {
    get => this.PatternColor;
    set => this.PatternColor = value;
  }

  public ExcelHAlign HorizontalAlignment
  {
    get => (this.IncludeAlignment ? this.m_extFormat : this.ParentRecord).HAlignmentType;
    set
    {
      if (this.HorizontalAlignment == value)
        return;
      this.IncludeAlignment = true;
      this.m_extFormat.HAlignmentType = value;
      if (value == ExcelHAlign.HAlignCenterAcrossSelection || value == ExcelHAlign.HAlignFill)
        this.m_extFormat.Rotation = (ushort) 0;
      this.SetChanged();
    }
  }

  public int IndentLevel
  {
    get => (int) this.m_extFormat.Indent;
    set
    {
      if (this.IndentLevel == value)
        return;
      if (value > this.m_book.MaxIndent)
        throw new ArgumentOutOfRangeException(nameof (IndentLevel));
      this.IncludeAlignment = true;
      this.m_extFormat.Indent = (byte) value;
      if (this.HorizontalAlignment == ExcelHAlign.HAlignGeneral)
        this.HorizontalAlignment = ExcelHAlign.HAlignLeft;
      if (value != 0)
        this.m_extFormat.Rotation = (ushort) 0;
      this.SetChanged();
    }
  }

  public bool FormulaHidden
  {
    get => this.m_extFormat.IsHidden;
    set
    {
      this.IncludeProtection = true;
      this.m_extFormat.IsHidden = value;
      this.SetChanged();
    }
  }

  public bool Locked
  {
    get => this.m_extFormat.IsLocked;
    set
    {
      if (this.Locked == value)
        return;
      this.IncludeProtection = true;
      this.m_extFormat.IsLocked = value;
      this.SetChanged();
    }
  }

  public bool JustifyLast
  {
    get => this.m_extFormat.JustifyLast;
    set
    {
      this.m_extFormat.JustifyLast = value;
      this.SetChanged();
    }
  }

  public string NumberFormat
  {
    get => this.NumberFormatObject.FormatString;
    set
    {
      this.NumberFormatIndex = (int) (ushort) this.m_book.InnerFormats.FindOrCreateFormat(value);
      this.SetChanged();
    }
  }

  public string NumberFormatLocal
  {
    get => this.NumberFormat;
    set => this.NumberFormat = value;
  }

  public INumberFormat NumberFormatSettings
  {
    get
    {
      return this.m_book.InnerFormats[this.NumberFormatIndex] != null ? (INumberFormat) this.m_book.InnerFormats[this.NumberFormatIndex] : (INumberFormat) this.m_book.InnerFormats[this.NumberFormat];
    }
  }

  public bool ShrinkToFit
  {
    get => this.m_extFormat.ShrinkToFit;
    set
    {
      if (value == this.ShrinkToFit)
        return;
      this.IncludeAlignment = true;
      this.m_extFormat.ShrinkToFit = value;
      this.SetChanged();
    }
  }

  public bool WrapText
  {
    get => this.m_extFormat.WrapText;
    set
    {
      if (this.WrapText == value)
        return;
      this.IncludeAlignment = true;
      this.m_extFormat.WrapText = value;
      if (this.Rotation == 0)
        this.m_extFormat.isWrappedFirst = true;
      this.SetChanged();
    }
  }

  public ExcelVAlign VerticalAlignment
  {
    get => (this.IncludeAlignment ? this.m_extFormat : this.ParentRecord).VAlignmentType;
    set
    {
      if (this.VerticalAlignment == value)
        return;
      this.IncludeAlignment = true;
      this.m_extFormat.VAlignmentType = value;
      this.SetChanged();
    }
  }

  public bool IncludeAlignment
  {
    get
    {
      bool notParentAlignment = this.m_extFormat.IsNotParentAlignment;
      return !this.HasParent ? !notParentAlignment : notParentAlignment;
    }
    set
    {
      if (this.HasParent)
      {
        if (this.IncludeAlignment == value)
          return;
        if (value && !this.m_book.Loading)
          this.m_extFormat.CopyAlignment(this.ParentRecord);
        this.m_extFormat.IsNotParentAlignment = value;
        this.SetChanged();
      }
      else
      {
        this.m_extFormat.IsNotParentAlignment = !value;
        this.SetChanged();
      }
    }
  }

  public bool IncludeBorder
  {
    get
    {
      bool isNotParentBorder = this.m_extFormat.IsNotParentBorder;
      return !this.HasParent ? !isNotParentBorder : isNotParentBorder;
    }
    set
    {
      if (this.HasParent)
      {
        if (this.IncludeBorder == value)
          return;
        if (value && !this.m_book.Loading)
          this.CopyBorders(this.ParentFormat);
        this.m_extFormat.IsNotParentBorder = value;
        this.SetChanged();
      }
      else
      {
        this.m_extFormat.IsNotParentBorder = !value;
        this.SetChanged();
      }
    }
  }

  public bool IncludeFont
  {
    get
    {
      bool isNotParentFont = this.m_extFormat.IsNotParentFont;
      return !this.HasParent ? !isNotParentFont : isNotParentFont;
    }
    set
    {
      if (this.HasParent)
      {
        if (this.IncludeFont == value)
          return;
        if (value && !this.m_book.Loading)
          this.m_extFormat.FontIndex = this.ParentRecord.FontIndex;
        this.m_extFormat.IsNotParentFont = value;
        this.SetChanged();
      }
      else
      {
        this.m_extFormat.IsNotParentFont = !value;
        this.SetChanged();
      }
    }
  }

  public bool IncludeNumberFormat
  {
    get
    {
      bool isNotParentFormat = this.m_extFormat.IsNotParentFormat;
      return !this.HasParent ? !isNotParentFormat : isNotParentFormat;
    }
    set
    {
      if (this.HasParent)
      {
        if (this.IncludeNumberFormat == value)
          return;
        if (value && !this.m_book.Loading)
          this.m_extFormat.FormatIndex = this.ParentRecord.FormatIndex;
        this.m_extFormat.IsNotParentFormat = value;
        this.SetChanged();
      }
      else
      {
        this.m_extFormat.IsNotParentFormat = !value;
        this.SetChanged();
      }
    }
  }

  public bool IncludePatterns
  {
    get
    {
      bool notParentPattern = this.m_extFormat.IsNotParentPattern;
      return !this.HasParent ? !notParentPattern : notParentPattern;
    }
    set
    {
      if (this.HasParent)
      {
        if (this.IncludePatterns == value)
          return;
        if (value && !this.m_book.Loading)
          this.CopyPatterns(this.ParentFormat);
        this.m_extFormat.IsNotParentPattern = value;
        this.SetChanged();
      }
      else
      {
        this.m_extFormat.IsNotParentPattern = !value;
        this.SetChanged();
      }
    }
  }

  public bool IncludeProtection
  {
    get
    {
      bool parentCellOptions = this.m_extFormat.IsNotParentCellOptions;
      return !this.HasParent ? !parentCellOptions : parentCellOptions;
    }
    set
    {
      if (this.HasParent)
      {
        if (this.IncludeProtection == value)
          return;
        if (value && !this.m_book.Loading)
          this.m_extFormat.CopyProtection(this.ParentRecord);
        this.m_extFormat.IsNotParentCellOptions = value;
        this.SetChanged();
      }
      else
      {
        this.m_extFormat.IsNotParentCellOptions = !value;
        this.SetChanged();
      }
    }
  }

  public virtual IFont Font => this.m_book.InnerFonts[this.FontIndex];

  public IBorders Borders
  {
    get
    {
      return (IBorders) new BordersCollection(this.Application, (object) this.m_book, (IInternalExtendedFormat) this);
    }
  }

  public bool IsFirstSymbolApostrophe
  {
    get => this.m_extFormat._123Prefix;
    set
    {
      this.m_extFormat._123Prefix = value;
      this.SetChanged();
    }
  }

  public ExcelKnownColors PatternColorIndex
  {
    get => this.PatternColorObject.GetIndexed((IWorkbook) this.m_book);
    set
    {
      if (value == this.PatternColorIndex)
        return;
      this.IncludePatterns = true;
      this.PatternColorObject.SetIndexed(value);
      this.m_extFormat.AdtlFillPattern |= (ushort) 1;
      this.SetChanged();
    }
  }

  public Color PatternColor
  {
    get => this.PatternColorObject.GetRGB((IWorkbook) this.m_book);
    set
    {
      this.IncludePatterns = true;
      this.PatternColorObject.SetRGB(value, (IWorkbook) this.m_book);
      if (this.m_extFormat.AdtlFillPattern == (ushort) 0)
        this.m_extFormat.AdtlFillPattern |= (ushort) 1;
      this.SetChanged();
    }
  }

  public ColorObject PatternColorObject
  {
    get => (this.IncludePatterns ? this : this.ParentFormat).InnerPatternColor;
  }

  public ExcelKnownColors ColorIndex
  {
    get => this.ColorObject.GetIndexed((IWorkbook) this.m_book);
    set
    {
      if (value == this.ColorIndex)
        return;
      this.IncludePatterns = true;
      this.ColorObject.SetIndexed(value, true, this.m_book);
      if (this.m_extFormat.AdtlFillPattern == (ushort) 0)
        this.m_extFormat.AdtlFillPattern = (ushort) 1;
      this.SetChanged();
    }
  }

  public Color Color
  {
    get => this.ColorObject.GetRGB((IWorkbook) this.m_book);
    set
    {
      this.IncludePatterns = true;
      this.ColorObject.SetRGB(value, (IWorkbook) this.m_book);
      if (this.m_extFormat.AdtlFillPattern == (ushort) 0)
        this.m_extFormat.AdtlFillPattern = (ushort) 1;
      this.SetChanged();
    }
  }

  public ColorObject ColorObject => (this.IncludePatterns ? this : this.ParentFormat).InnerColor;

  public bool IsModified
  {
    get
    {
      bool isModified = false;
      if (this.HasParent)
        isModified = this.IncludeAlignment || this.IncludeBorder || this.IncludeFont || this.IncludeNumberFormat || this.IncludePatterns || this.IncludeProtection;
      return isModified;
    }
  }

  private bool CompareProperties(ExtendedFormatImpl parent) => throw new NotImplementedException();

  public List<ExtendedProperty> Properties
  {
    get => this.m_xfExt.Properties;
    set
    {
      this.m_xfExt.Properties = value;
      this.m_xfExt.PropertyCount = (ushort) this.m_xfExt.Properties.Count;
    }
  }

  public ExcelReadingOrderType ReadingOrder
  {
    get => (ExcelReadingOrderType) this.m_extFormat.ReadingOrder;
    set
    {
      this.m_extFormat.ReadingOrder = (ushort) value;
      this.SetChanged();
    }
  }

  public int Rotation
  {
    get => (int) this.m_extFormat.Rotation;
    set
    {
      if (value == this.Rotation)
        return;
      this.IncludeAlignment = true;
      this.m_extFormat.Rotation = this.HorizontalAlignment == ExcelHAlign.HAlignCenterAcrossSelection || this.HorizontalAlignment == ExcelHAlign.HAlignFill ? (ushort) 0 : (ushort) value;
      if (value != 0)
        this.m_extFormat.Indent = (byte) 0;
      if (!this.WrapText)
        this.m_extFormat.isRotatedFirst = true;
      if (value == (int) byte.MaxValue)
        this.isVerticalText = true;
      this.SetChanged();
    }
  }

  internal bool IsVerticalText => this.isVerticalText;

  [CLSCompliant(false)]
  public ExtendedFormatRecord.TXFType XFType
  {
    get => this.m_extFormat.XFType;
    set
    {
      this.m_extFormat.XFType = value;
      this.SetChanged();
    }
  }

  internal int CFPriority
  {
    get => this.m_cfPriority;
    set => this.m_cfPriority = value;
  }

  internal uint CFApplied
  {
    get => this.m_cfApplied;
    set => this.m_cfApplied = value;
  }

  public IGradient Gradient
  {
    get => (IGradient) this.m_gradient;
    set => this.m_gradient = (ShapeFillImpl) value;
  }

  internal int Index
  {
    get => this.m_iXFIndex;
    set => this.m_iXFIndex = value;
  }

  [CLSCompliant(false)]
  public ExtendedFormatRecord Record
  {
    get => this.m_extFormat;
    protected set => this.m_extFormat = value != null ? value : throw new ArgumentNullException();
  }

  [CLSCompliant(false)]
  public ExtendedXFRecord XFRecord
  {
    get => this.m_xfExt;
    protected set => this.m_xfExt = value != null ? value : throw new ArgumentNullException();
  }

  internal int ParentIndex
  {
    get => (int) this.m_extFormat.ParentIndex;
    set => this.m_extFormat.ParentIndex = (ushort) value;
  }

  public WorkbookImpl Workbook => this.m_book;

  protected internal ExtendedFormatsCollection ParentCollection => this.m_book.InnerExtFormats;

  public ColorObject BottomBorderColor
  {
    get => (this.IncludeBorder ? this : this.ParentFormat).InnerBottomBorderColor;
  }

  public ColorObject TopBorderColor
  {
    get => (this.IncludeBorder ? this : this.ParentFormat).InnerTopBorderColor;
  }

  public ColorObject LeftBorderColor
  {
    get => (this.IncludeBorder ? this : this.ParentFormat).InnerLeftBorderColor;
  }

  public ColorObject RightBorderColor
  {
    get => (this.IncludeBorder ? this : this.ParentFormat).InnerRightBorderColor;
  }

  public ColorObject DiagonalBorderColor
  {
    get => (this.IncludeBorder ? this : this.ParentFormat).InnerDiagonalBorderColor;
  }

  public ExcelLineStyle LeftBorderLineStyle
  {
    get => (this.IncludeBorder ? this.m_extFormat : this.ParentRecord).BorderLeft;
    set
    {
      if (this.LeftBorderLineStyle == value)
        return;
      this.IncludeBorder = true;
      this.m_extFormat.SetWorkbook(this.m_book);
      this.m_extFormat.BorderLeft = value;
      if (value == ExcelLineStyle.None)
      {
        this.CheckAndUpdateHasBorder(this.m_extFormat);
        this.LeftBorderColor.SetIndexed(ExcelKnownColors.BlackCustom, false);
      }
      else
        this.m_hasBorder = true;
      this.LeftBorderColor.Normalize();
      this.SetChanged();
    }
  }

  public ExcelLineStyle RightBorderLineStyle
  {
    get => (this.IncludeBorder ? this.m_extFormat : this.ParentRecord).BorderRight;
    set
    {
      if (this.RightBorderLineStyle == value)
        return;
      this.IncludeBorder = true;
      this.m_extFormat.SetWorkbook(this.m_book);
      this.m_extFormat.BorderRight = value;
      if (value == ExcelLineStyle.None)
      {
        this.CheckAndUpdateHasBorder(this.m_extFormat);
        this.RightBorderColor.SetIndexed(ExcelKnownColors.BlackCustom, false);
      }
      else
        this.m_hasBorder = true;
      this.RightBorderColor.Normalize();
      this.SetChanged();
    }
  }

  public ExcelLineStyle TopBorderLineStyle
  {
    get => (this.IncludeBorder ? this.m_extFormat : this.ParentRecord).BorderTop;
    set
    {
      if (this.TopBorderLineStyle == value)
        return;
      this.IncludeBorder = true;
      this.m_extFormat.SetWorkbook(this.m_book);
      this.m_extFormat.BorderTop = value;
      if (value == ExcelLineStyle.None)
      {
        this.CheckAndUpdateHasBorder(this.m_extFormat);
        this.TopBorderColor.SetIndexed(ExcelKnownColors.BlackCustom, false);
      }
      else
        this.m_hasBorder = true;
      this.TopBorderColor.Normalize();
      this.SetChanged();
    }
  }

  public ExcelLineStyle BottomBorderLineStyle
  {
    get => (this.IncludeBorder ? this.m_extFormat : this.ParentRecord).BorderBottom;
    set
    {
      if (this.BottomBorderLineStyle == value)
        return;
      this.IncludeBorder = true;
      this.m_extFormat.SetWorkbook(this.m_book);
      this.m_extFormat.BorderBottom = value;
      if (value == ExcelLineStyle.None)
      {
        this.CheckAndUpdateHasBorder(this.m_extFormat);
        this.BottomBorderColor.SetIndexed(ExcelKnownColors.BlackCustom, false);
      }
      else
        this.m_hasBorder = true;
      this.BottomBorderColor.Normalize();
      this.SetChanged();
    }
  }

  public ExcelLineStyle DiagonalUpBorderLineStyle
  {
    get
    {
      return (ExcelLineStyle) (this.IncludeBorder ? this.m_extFormat : this.ParentRecord).DiagonalLineStyle;
    }
    set
    {
      if (this.DiagonalUpBorderLineStyle != value)
      {
        this.IncludeBorder = true;
        this.m_extFormat.DiagonalLineStyle = (ushort) value;
        if (value == ExcelLineStyle.None)
        {
          this.CheckAndUpdateHasBorder(this.m_extFormat);
          this.DiagonalBorderColor.SetIndexed(ExcelKnownColors.BlackCustom, true);
        }
        else
          this.m_hasBorder = true;
        this.DiagonalBorderColor.Normalize();
        this.SetChanged();
      }
      if (this.m_extFormat.DiagonalFromBottomLeft)
        return;
      this.IncludeBorder = true;
      this.m_extFormat.DiagonalFromBottomLeft = true;
      this.SetChanged();
    }
  }

  public ExcelLineStyle DiagonalDownBorderLineStyle
  {
    get
    {
      if (!this.IncludeBorder)
      {
        ExtendedFormatRecord parentRecord = this.ParentRecord;
      }
      return (ExcelLineStyle) this.m_extFormat.DiagonalLineStyle;
    }
    set
    {
      if (this.DiagonalDownBorderLineStyle != value)
      {
        this.IncludeBorder = true;
        this.m_extFormat.DiagonalLineStyle = (ushort) value;
        if (value == ExcelLineStyle.None)
        {
          this.CheckAndUpdateHasBorder(this.m_extFormat);
          this.DiagonalBorderColor.SetIndexed(ExcelKnownColors.BlackCustom, true);
        }
        else
          this.m_hasBorder = true;
        this.DiagonalBorderColor.Normalize();
        this.SetChanged();
      }
      if (this.m_extFormat.DiagonalFromTopLeft)
        return;
      this.IncludeBorder = true;
      this.m_extFormat.DiagonalFromTopLeft = true;
      this.SetChanged();
    }
  }

  public bool DiagonalUpVisible
  {
    get => (this.IncludeBorder ? this.m_extFormat : this.ParentRecord).DiagonalFromBottomLeft;
    set
    {
      if (this.DiagonalUpVisible == value)
        return;
      this.IncludeBorder = true;
      this.m_extFormat.DiagonalFromBottomLeft = value;
      this.SetChanged();
    }
  }

  public bool DiagonalDownVisible
  {
    get => (this.IncludeBorder ? this.m_extFormat : this.ParentRecord).DiagonalFromTopLeft;
    set
    {
      if (this.DiagonalDownVisible == value)
        return;
      this.IncludeBorder = true;
      this.m_extFormat.DiagonalFromTopLeft = value;
      this.SetChanged();
    }
  }

  public bool HasParent => this.ParentIndex != this.m_book.MaxXFCount;

  public bool IsDefaultColor => this.ColorIndex == ExcelKnownColors.None;

  public bool IsDefaultPatternColor => this.PatternColorIndex == ExcelKnownColors.BlackCustom;

  private ExtendedFormatRecord ParentRecord
  {
    get
    {
      return !this.HasParent ? this.m_extFormat : ((ExtendedFormatImpl) this.m_book.GetExtFormat(this.ParentIndex)).Record;
    }
  }

  private ExtendedFormatImpl ParentFormat
  {
    get => !this.HasParent ? this : (ExtendedFormatImpl) this.m_book.GetExtFormat(this.ParentIndex);
  }

  public FormatImpl NumberFormatObject
  {
    get
    {
      if (this.m_book.InnerFormats.Count > 14 && !this.m_book.InnerFormats.Contains(this.NumberFormatIndex))
        this.NumberFormatIndex = 14;
      return this.m_book.InnerFormats[this.NumberFormatIndex];
    }
  }

  public bool HasBorder
  {
    get => this.m_hasBorder;
    set => this.m_hasBorder = value;
  }

  internal bool PivotButton
  {
    get => this.m_pivotButton;
    set => this.m_pivotButton = value;
  }

  internal bool IsPivotFormat
  {
    get => this.m_bIsPivotFormat;
    set => this.m_bIsPivotFormat = value;
  }

  protected ColorObject InnerColor => this.m_color;

  protected ColorObject InnerPatternColor => this.m_patternColor;

  protected ColorObject InnerTopBorderColor => this.m_topBorderColor;

  protected ColorObject InnerBottomBorderColor => this.m_bottomBorderColor;

  protected ColorObject InnerLeftBorderColor => this.m_leftBorderColor;

  protected ColorObject InnerRightBorderColor => this.m_rightBorderColor;

  protected ColorObject InnerDiagonalBorderColor => this.m_diagonalBorderColor;

  internal void SetChanged() => this.m_book.Saved = false;

  public void CopyTo(ExtendedFormatImpl twin)
  {
    if (twin == null)
      throw new ArgumentNullException(nameof (twin));
    twin.m_book = this.m_book;
    this.m_extFormat.CopyTo(twin.m_extFormat);
    this.m_xfExt.CopyTo(twin.m_xfExt);
  }

  public ExtendedFormatImpl CreateChildFormat() => this.CreateChildFormat(true);

  public ExtendedFormatImpl CreateChildFormat(bool bRegister)
  {
    ExtendedFormatImpl childFormat = this;
    if (childFormat.Record.XFType == ExtendedFormatRecord.TXFType.XF_CELL)
    {
      ExtendedFormatImpl formatWithoutRegister = this.m_book.CreateExtFormatWithoutRegister((IExtendedFormat) childFormat);
      formatWithoutRegister.Record.XFType = ExtendedFormatRecord.TXFType.XF_STYLE;
      formatWithoutRegister.ParentIndex = this.Index;
      childFormat = this.UpdateIncludeProperties(this.Index, formatWithoutRegister);
      if (bRegister)
        childFormat = this.m_book.RegisterExtFormat(childFormat);
    }
    return childFormat;
  }

  private ExtendedFormatImpl UpdateIncludeProperties(int index, ExtendedFormatImpl format)
  {
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    ExtendedFormatImpl extendedFormatImpl = innerExtFormats[index];
    ExtendedFormatRecord record1 = extendedFormatImpl.Record;
    ExtendedFormatRecord record2 = format.Record;
    IBorders borders1 = innerExtFormats[this.m_book.DefaultXFIndex].Borders;
    IBorders borders2 = extendedFormatImpl.Borders;
    if (!borders1.Equals((object) borders2))
      record2.IsNotParentBorder = true;
    if (record1.FontIndex > (ushort) 0)
      record2.IsNotParentFont = true;
    if (record1.FormatIndex > (ushort) 0)
      record2.IsNotParentFormat = true;
    if (record1.FillIndex > (ushort) 0)
      record2.IsNotParentPattern = true;
    (borders1 as BordersCollection).Dispose();
    (borders2 as BordersCollection).Dispose();
    return format;
  }

  public ExtendedFormatImpl CreateChildFormat(ExtendedFormatImpl oldFormat)
  {
    ExtendedFormatRecord.TXFType xfType = this.Record.XFType;
    ExtendedFormatImpl childFormat = this.CreateChildFormat();
    if (xfType == ExtendedFormatRecord.TXFType.XF_CELL)
    {
      ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) childFormat.Clone();
      bool flag = false;
      if (!this.IncludeAlignment)
      {
        ExtendedFormatImpl.CopyAlignment(oldFormat, extendedFormatImpl, false);
        extendedFormatImpl.IncludeAlignment = true;
        flag = true;
      }
      if (!this.IncludeBorder)
      {
        ExtendedFormatImpl.CopyBorders(oldFormat, extendedFormatImpl, false);
        extendedFormatImpl.IncludeBorder = true;
        flag = true;
      }
      if (!this.IncludeFont)
      {
        ExtendedFormatImpl.CopyFont(oldFormat, extendedFormatImpl, false);
        extendedFormatImpl.IncludeFont = true;
        flag = true;
      }
      if (!this.IncludeNumberFormat)
      {
        ExtendedFormatImpl.CopyFormat(oldFormat, extendedFormatImpl, false);
        extendedFormatImpl.IncludeNumberFormat = true;
        flag = true;
      }
      if (!this.IncludePatterns)
      {
        ExtendedFormatImpl.CopyPatterns(oldFormat, extendedFormatImpl, false);
        extendedFormatImpl.IncludePatterns = true;
        flag = true;
      }
      if (!this.IncludeProtection)
      {
        ExtendedFormatImpl.CopyProtection(oldFormat, extendedFormatImpl, false);
        extendedFormatImpl.IncludeProtection = true;
        flag = true;
      }
      if (flag)
        childFormat = this.m_book.InnerExtFormats.Add(extendedFormatImpl);
      else
        extendedFormatImpl.Dispose();
    }
    return childFormat;
  }

  public void SynchronizeWithParent()
  {
    ExtendedFormatRecord parentRecord = this.ParentRecord;
    if (!this.IncludeAlignment)
      this.m_extFormat.CopyAlignment(parentRecord);
    if (this.IncludeBorder)
      this.CopyBorders(this.ParentFormat);
    if (this.IncludeFont)
      this.m_extFormat.FontIndex = parentRecord.FontIndex;
    if (this.IncludeNumberFormat)
      this.m_extFormat.FormatIndex = parentRecord.FormatIndex;
    if (this.IncludePatterns)
      this.CopyPatterns(this.ParentFormat);
    if (this.IncludeProtection)
      return;
    this.m_extFormat.CopyProtection(parentRecord);
  }

  private void CopyBorders(ExtendedFormatImpl source)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    this.m_extFormat.CopyBorders(source.m_extFormat);
    this.m_topBorderColor.CopyFrom(source.m_topBorderColor, false);
    this.m_bottomBorderColor.CopyFrom(source.m_bottomBorderColor, false);
    this.m_leftBorderColor.CopyFrom(source.m_leftBorderColor, false);
    this.m_rightBorderColor.CopyFrom(source.m_rightBorderColor, false);
    this.m_diagonalBorderColor.CopyFrom(source.m_diagonalBorderColor, false);
  }

  private void CopyPatterns(ExtendedFormatImpl source)
  {
    if (source == null)
      throw new ArgumentNullException(nameof (source));
    this.m_extFormat.CopyPatterns(this.m_extFormat);
    if (this.m_color == (ColorObject) null && this.m_book.Loading)
      return;
    this.m_color.CopyFrom(source.m_color, false);
    this.m_patternColor.CopyFrom(source.m_patternColor, false);
  }

  public void BeginUpdate()
  {
  }

  public void EndUpdate()
  {
  }

  public ExtendedFormatImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
    this.m_extFormat = (ExtendedFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedFormat);
    this.m_xfExt = (ExtendedXFRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExtendedXFRecord);
    this.Parse(this.m_extFormat, this.m_xfExt);
  }

  private ExtendedFormatImpl(IApplication application, object parent, BiffReader reader)
    : this(application, parent)
  {
    this.Parse(reader);
  }

  [CLSCompliant(false)]
  public ExtendedFormatImpl(
    IApplication application,
    object parent,
    BiffRecordRaw[] data,
    int position)
    : this(application, parent)
  {
    this.Parse((IList<BiffRecordRaw>) data, position);
  }

  public ExtendedFormatImpl(
    IApplication application,
    object parent,
    List<BiffRecordRaw> data,
    int position)
    : this(application, parent)
  {
    this.Parse((IList<BiffRecordRaw>) data, position);
  }

  [CLSCompliant(false)]
  public ExtendedFormatImpl(
    IApplication application,
    object parent,
    ExtendedFormatRecord format,
    ExtendedXFRecord xfExt)
    : this(application, parent, format, xfExt, true)
  {
  }

  [CLSCompliant(false)]
  public ExtendedFormatImpl(
    IApplication application,
    object parent,
    ExtendedFormatRecord format,
    ExtendedXFRecord xfext,
    bool bInitializeColors)
    : base(application, parent)
  {
    this.FindParents();
    this.Parse(format, xfext, bInitializeColors);
  }

  private void FindParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }

  [CLSCompliant(false)]
  protected void Parse(BiffReader reader)
  {
  }

  [CLSCompliant(false)]
  protected void Parse(IList<BiffRecordRaw> data, int position)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (position < 0 || position > data.Count - 1)
      throw new ArgumentOutOfRangeException(nameof (position), "Value cannot be less than 0 and greater than data.Length - 1.");
    this.Parse((ExtendedFormatRecord) data[position], (ExtendedXFRecord) data[position]);
  }

  [CLSCompliant(false)]
  protected void Parse(ExtendedFormatRecord format, ExtendedXFRecord xfExt)
  {
    this.Parse(format, xfExt, true);
  }

  [CLSCompliant(false)]
  protected void Parse(
    ExtendedFormatRecord format,
    ExtendedXFRecord xfExt,
    bool isInitializeColors)
  {
    this.m_extFormat = format;
    this.m_xfExt = xfExt;
    if (this.m_book.InnerFonts.Count == 0)
      this.m_extFormat.FontIndex = (ushort) 0;
    if ((int) this.m_extFormat.FontIndex > this.m_book.InnerFonts.Count)
      throw new ApplicationException("Extended Format record FontIndex field has wrong value");
    this.Index = (int) (ushort) this.m_book.InnerExtFormats.Count + this.m_book.StyleArrayCount;
    if (isInitializeColors || !this.m_book.Loading || this.m_book.Version != ExcelVersion.Excel97to2003 || this.IncludePatterns || !this.HasParent)
    {
      this.InitializeColors();
      this.CopyColors(xfExt);
    }
    this.CheckAndUpdateHasBorder(format);
  }

  public void UpdateFromParent()
  {
    ExtendedFormatImpl parentFormat = this.ParentFormat;
    if (this.m_book.Version != ExcelVersion.Excel97to2003 || this.IncludePatterns || !this.HasParent)
      return;
    if ((int) (ushort) parentFormat.FillBackground != (int) this.m_extFormat.FillBackground || (int) (ushort) parentFormat.FillForeground != (int) this.m_extFormat.FillForeground)
    {
      ushort fillForeground = this.m_extFormat.FillForeground;
      ushort fillBackground = this.m_extFormat.FillBackground;
      this.IncludePatterns = true;
      this.m_extFormat.FillForeground = fillForeground;
      this.m_extFormat.FillBackground = fillBackground;
    }
    this.InitializeColors();
  }

  public void UpdateFromCurrentExtendedFormat(ExtendedFormatImpl CurrXF)
  {
    ShapeFillImpl shapeFillImpl = (ShapeFillImpl) null;
    ExtendedFormatImpl extendedFormatImpl1 = CurrXF != null ? CurrXF : throw new ArgumentNullException("CurrentXF");
    ExtendedFormatRecord format = (ExtendedFormatRecord) extendedFormatImpl1.Record.Clone();
    ExtendedXFRecord xfExt = extendedFormatImpl1.XFRecord.CloneObject();
    ExtendedFormatImpl extendedFormatImpl2 = CurrXF;
    CurrXF = new ExtendedFormatImpl(this.Application, (object) this, format, xfExt);
    CurrXF.ColorObject.CopyFrom(extendedFormatImpl2.ColorObject, false);
    CurrXF.PatternColorObject.CopyFrom(extendedFormatImpl2.PatternColorObject, false);
    CurrXF.Gradient = (IGradient) shapeFillImpl;
    CurrXF.BottomBorderColor.CopyFrom(extendedFormatImpl2.BottomBorderColor, false);
    CurrXF.TopBorderColor.CopyFrom(extendedFormatImpl2.TopBorderColor, false);
    CurrXF.LeftBorderColor.CopyFrom(extendedFormatImpl2.LeftBorderColor, false);
    CurrXF.RightBorderColor.CopyFrom(extendedFormatImpl2.RightBorderColor, false);
    CurrXF.DiagonalBorderColor.CopyFrom(extendedFormatImpl2.DiagonalBorderColor, false);
    CurrXF.Font.RGBColor = extendedFormatImpl2.Font.RGBColor;
    CurrXF.IndentLevel = extendedFormatImpl2.IndentLevel;
  }

  public void UpdateFromCurrentExtendedFormatNew(ExtendedFormatImpl CurrXF, bool isCellStyle)
  {
    ExtendedFormatImpl extendedFormatImpl = CurrXF != null ? CurrXF : throw new ArgumentNullException("CurrentXF");
    this.m_extFormat = (ExtendedFormatRecord) extendedFormatImpl.m_extFormat.Clone();
    this.m_color.CopyFrom(extendedFormatImpl.ColorObject, false);
    this.m_patternColor.CopyFrom(extendedFormatImpl.PatternColorObject, false);
    if (extendedFormatImpl.m_gradient != null)
      this.m_gradient = extendedFormatImpl.m_gradient;
    this.m_bottomBorderColor.CopyFrom(extendedFormatImpl.BottomBorderColor, false);
    this.m_topBorderColor.CopyFrom(extendedFormatImpl.TopBorderColor, false);
    this.m_leftBorderColor.CopyFrom(extendedFormatImpl.LeftBorderColor, false);
    this.m_rightBorderColor.CopyFrom(extendedFormatImpl.RightBorderColor, false);
    this.m_diagonalBorderColor.CopyFrom(extendedFormatImpl.DiagonalBorderColor, false);
    if (!isCellStyle)
      return;
    this.Font.RGBColor = extendedFormatImpl.Font.RGBColor;
    this.IndentLevel = extendedFormatImpl.IndentLevel;
  }

  protected void InitializeColors()
  {
    this.m_color = new ColorObject((ExcelKnownColors) this.m_extFormat.FillBackground);
    this.m_color.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateColor);
    this.m_patternColor = new ColorObject((ExcelKnownColors) this.m_extFormat.FillForeground);
    this.m_patternColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdatePatternColor);
    this.m_topBorderColor = new ColorObject((ExcelKnownColors) this.m_extFormat.TopBorderPaletteIndex);
    this.m_topBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateTopBorderColor);
    this.m_bottomBorderColor = new ColorObject((ExcelKnownColors) this.m_extFormat.BottomBorderPaletteIndex);
    this.m_bottomBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateBottomBorderColor);
    this.m_leftBorderColor = new ColorObject((ExcelKnownColors) this.m_extFormat.LeftBorderPaletteIndex);
    this.m_leftBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateLeftBorderColor);
    this.m_rightBorderColor = new ColorObject((ExcelKnownColors) this.m_extFormat.RightBorderPaletteIndex);
    this.m_rightBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateRightBorderColor);
    this.m_diagonalBorderColor = new ColorObject((ExcelKnownColors) this.m_extFormat.DiagonalLineColor);
    this.m_diagonalBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateDiagonalBorderColor);
  }

  internal void UpdateColor()
  {
    this.m_extFormat.FillBackground = (ushort) this.m_color.GetIndexed((IWorkbook) this.m_book);
  }

  internal void UpdatePatternColor()
  {
    this.m_extFormat.FillForeground = (ushort) this.m_patternColor.GetIndexed((IWorkbook) this.m_book);
  }

  internal void UpdateTopBorderColor()
  {
    this.m_extFormat.TopBorderPaletteIndex = (ushort) this.m_topBorderColor.GetIndexed((IWorkbook) this.m_book);
  }

  internal void UpdateBottomBorderColor()
  {
    this.m_extFormat.BottomBorderPaletteIndex = (ushort) this.m_bottomBorderColor.GetIndexed((IWorkbook) this.m_book);
  }

  internal void UpdateLeftBorderColor()
  {
    this.m_extFormat.LeftBorderPaletteIndex = (ushort) this.m_leftBorderColor.GetIndexed((IWorkbook) this.m_book);
  }

  internal void UpdateRightBorderColor()
  {
    this.m_extFormat.RightBorderPaletteIndex = (ushort) this.m_rightBorderColor.GetIndexed((IWorkbook) this.m_book);
  }

  internal void UpdateDiagonalBorderColor()
  {
    this.m_extFormat.DiagonalLineColor = (ushort) this.m_diagonalBorderColor.GetIndexed((IWorkbook) this.m_book);
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records, uint[] crcCache)
  {
    ExtendedFormatRecord record = (ExtendedFormatRecord) this.m_extFormat.Clone();
    record.FillBackground = (ushort) this.ColorIndex;
    record.FillForeground = (ushort) this.PatternColorIndex;
    this.CheckAndCorrectFormatRecord(record);
    records.Add((IBiffStorage) record);
    byte[] data = record.Data;
    if (this.m_book.IsCRCSucceed)
      return;
    this.m_book.crcValue = this.m_book.CalculateCRC(this.m_book.crcValue, data, crcCache);
  }

  [CLSCompliant(false)]
  protected void CheckAndCorrectFormatRecord(ExtendedFormatRecord record)
  {
    if (this.ParentIndex != 0)
      return;
    ExtendedFormatRecord extFormat = ((ExtendedFormatImpl) this.m_book.GetExtFormat(0)).m_extFormat;
    if (!record.IsNotParentAlignment)
      record.CopyAlignment(extFormat);
    if (!record.IsNotParentBorder)
      record.CopyBorders(extFormat);
    if (!record.IsNotParentCellOptions)
      record.CopyProtection(extFormat);
    if (!record.IsNotParentFont)
      record.FontIndex = extFormat.FontIndex;
    if (!record.IsNotParentFormat)
      record.FormatIndex = extFormat.FormatIndex;
    if (record.IsNotParentPattern)
      return;
    record.CopyPatterns(extFormat);
  }

  [CLSCompliant(false)]
  public void SerializeXFormat(OffsetArrayList records)
  {
    if (this.m_xfExt == null)
      return;
    ExtendedXFRecord extendedXfRecord = this.m_xfExt.CloneObject();
    records.Add((IBiffStorage) extendedXfRecord);
  }

  private void CopyColors(ExtendedXFRecord xfExt)
  {
    if (xfExt.Properties.Count <= 0)
      return;
    for (int index = 0; index < xfExt.Properties.Count; ++index)
    {
      ExtendedProperty property = xfExt.Properties[index];
      Color argb = this.m_book.ConvertRGBAToARGB(this.m_book.UIntToColor(property.ColorValue));
      ColorType colorType = property.ColorType;
      switch (colorType)
      {
        case ColorType.RGB:
        case ColorType.Theme:
          switch (property.Type)
          {
            case CellPropertyExtensionType.ForeColor:
              if (colorType == ColorType.Theme)
              {
                property.Tint /= (double) short.MaxValue;
                this.ColorObject.SetTheme((int) property.ColorValue, (IWorkbook) this.Workbook, property.Tint);
                continue;
              }
              if (this.FillPattern == ExcelPattern.Solid)
              {
                this.ColorObject.ColorType = property.ColorType;
                this.Color = argb;
                continue;
              }
              this.PatternColorObject.ColorType = property.ColorType;
              this.PatternColor = argb;
              continue;
            case CellPropertyExtensionType.BackColor:
              if (colorType == ColorType.Theme)
              {
                property.Tint /= (double) short.MaxValue;
                this.ColorObject.SetTheme((int) property.ColorValue, (IWorkbook) this.Workbook, property.Tint);
                continue;
              }
              if (this.FillPattern == ExcelPattern.Solid)
              {
                this.PatternColorObject.ColorType = property.ColorType;
                this.PatternColor = argb;
                continue;
              }
              this.ColorObject.ColorType = property.ColorType;
              this.Color = argb;
              continue;
            case CellPropertyExtensionType.TopBorderColor:
              if (colorType == ColorType.RGB)
              {
                this.Borders[ExcelBordersIndex.EdgeTop].ColorObject.ColorType = property.ColorType;
                this.Borders[ExcelBordersIndex.EdgeTop].ColorRGB = argb;
                continue;
              }
              continue;
            case CellPropertyExtensionType.BottomBorderColor:
              if (colorType == ColorType.RGB)
              {
                this.Borders[ExcelBordersIndex.EdgeBottom].ColorObject.ColorType = property.ColorType;
                this.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = argb;
                continue;
              }
              continue;
            case CellPropertyExtensionType.LeftBorderColor:
              if (colorType == ColorType.RGB)
              {
                this.Borders[ExcelBordersIndex.EdgeLeft].ColorObject.ColorType = property.ColorType;
                this.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB = argb;
                continue;
              }
              continue;
            case CellPropertyExtensionType.RightBorderColor:
              if (colorType == ColorType.RGB)
              {
                this.Borders[ExcelBordersIndex.EdgeRight].ColorObject.ColorType = property.ColorType;
                this.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = argb;
                continue;
              }
              continue;
            case CellPropertyExtensionType.DiagonalCellBorder:
              if (colorType == ColorType.RGB)
              {
                this.Borders[ExcelBordersIndex.DiagonalUp].ColorObject.ColorType = property.ColorType;
                this.Borders[ExcelBordersIndex.DiagonalUp].ColorRGB = argb;
                this.Borders[ExcelBordersIndex.DiagonalDown].ColorObject.ColorType = property.ColorType;
                this.Borders[ExcelBordersIndex.DiagonalDown].ColorRGB = argb;
                continue;
              }
              continue;
            case CellPropertyExtensionType.TextColor:
              if (colorType == ColorType.RGB)
              {
                this.Font.RGBColor = argb;
                continue;
              }
              continue;
            case CellPropertyExtensionType.TextIndentationLevel:
              if (colorType != ColorType.Theme)
              {
                this.IndentLevel = (int) property.Indent;
                continue;
              }
              continue;
            default:
              continue;
          }
        default:
          if (property.Indent <= (ushort) 15)
            break;
          goto case ColorType.RGB;
      }
    }
  }

  public int CompareTo(object obj)
  {
    return obj is ExtendedFormatImpl ? this.CompareTo((ExtendedFormatImpl) obj) : throw new ArgumentException("Can only compare types with the same type", nameof (obj));
  }

  public int CompareTo(ExtendedFormatImpl twin)
  {
    this.CheckAndCorrectFormatRecord(this.m_extFormat);
    twin.CheckAndCorrectFormatRecord(twin.m_extFormat);
    byte[] data1 = this.m_extFormat.Data;
    byte[] data2 = twin.m_extFormat.Data;
    int index1 = 0;
    for (int index2 = Math.Min(data1.Length, data2.Length); index1 < index2; ++index1)
    {
      int num;
      if ((num = data1[index1].CompareTo(data2[index1])) != 0)
        return num;
    }
    return data1.Length - data2.Length;
  }

  public int CompareToWithoutIndex(ExtendedFormatImpl twin)
  {
    int num = this.m_gradient == null ? (twin.Gradient == null ? 0 : 1) : this.m_gradient.CompareTo(twin.Gradient);
    if (num == 0)
      num = !(this.m_color == twin.m_color) || !(this.m_patternColor == twin.m_patternColor) || this.PivotButton != twin.PivotButton ? 1 : 0;
    if (num == 0 && this.m_book.InnerFormats.Contains(this.NumberFormatIndex) && this.m_book.InnerFormats.Contains(twin.NumberFormatIndex))
      num = this.NumberFormat == twin.NumberFormat ? 0 : 1;
    return num != 0 || this.m_extFormat.CompareTo(twin.m_extFormat) != 0 ? 1 : 0;
  }

  public override int GetHashCode()
  {
    return this.m_gradient == null ? this.m_extFormat.GetHashCode() : this.m_extFormat.GetHashCode() ^ this.m_gradient.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    return obj is ExtendedFormatImpl twin && this.CompareToWithoutIndex(twin) == 0;
  }

  public static void CopyFromTo(
    ExtendedFormatImpl childFormat,
    ExtendedFormatImpl parentFormat,
    bool bSetFlag)
  {
    ExtendedFormatImpl.CopyAlignment(childFormat, parentFormat, bSetFlag);
    ExtendedFormatImpl.CopyBorders(childFormat, parentFormat, bSetFlag);
    ExtendedFormatImpl.CopyFont(childFormat, parentFormat, bSetFlag);
    ExtendedFormatImpl.CopyFormat(childFormat, parentFormat, bSetFlag);
    ExtendedFormatImpl.CopyPatterns(childFormat, parentFormat, bSetFlag);
    ExtendedFormatImpl.CopyProtection(childFormat, parentFormat, bSetFlag);
  }

  private static void CopyAlignment(
    ExtendedFormatImpl childFormat,
    ExtendedFormatImpl parentFormat,
    bool bSetFlag)
  {
    parentFormat.VerticalAlignment = childFormat.VerticalAlignment;
    parentFormat.HorizontalAlignment = childFormat.HorizontalAlignment;
    parentFormat.WrapText = childFormat.WrapText;
    parentFormat.IndentLevel = childFormat.IndentLevel;
    parentFormat.Rotation = childFormat.Rotation;
    parentFormat.ReadingOrder = childFormat.ReadingOrder;
    if (!bSetFlag)
      return;
    childFormat.IncludeAlignment = false;
  }

  private static void CopyBorders(
    ExtendedFormatImpl childFormat,
    ExtendedFormatImpl parentFormat,
    bool bSetFlag)
  {
    parentFormat.Record.BorderBottom = childFormat.Record.BorderBottom;
    parentFormat.Record.BorderLeft = childFormat.Record.BorderLeft;
    parentFormat.Record.BorderRight = childFormat.Record.BorderRight;
    parentFormat.Record.BorderTop = childFormat.Record.BorderTop;
    parentFormat.Record.DiagonalFromBottomLeft = childFormat.Record.DiagonalFromBottomLeft;
    parentFormat.Record.DiagonalFromTopLeft = childFormat.Record.DiagonalFromTopLeft;
    parentFormat.Record.DiagonalLineStyle = childFormat.Record.DiagonalLineStyle;
    parentFormat.TopBorderColor.CopyFrom(childFormat.TopBorderColor, true);
    parentFormat.BottomBorderColor.CopyFrom(childFormat.BottomBorderColor, true);
    parentFormat.LeftBorderColor.CopyFrom(childFormat.LeftBorderColor, true);
    parentFormat.RightBorderColor.CopyFrom(childFormat.RightBorderColor, true);
    parentFormat.DiagonalBorderColor.CopyFrom(childFormat.DiagonalBorderColor, true);
    if (!bSetFlag)
      return;
    childFormat.IncludeBorder = false;
  }

  private static void CopyFont(
    ExtendedFormatImpl childFormat,
    ExtendedFormatImpl parentFormat,
    bool bSetFlag)
  {
    parentFormat.FontIndex = childFormat.FontIndex;
    if (!bSetFlag)
      return;
    childFormat.IncludeFont = false;
  }

  private static void CopyFormat(
    ExtendedFormatImpl childFormat,
    ExtendedFormatImpl parentFormat,
    bool bSetFlag)
  {
    parentFormat.NumberFormat = childFormat.NumberFormat;
    if (!bSetFlag)
      return;
    childFormat.IncludeNumberFormat = false;
  }

  private static void CopyPatterns(
    ExtendedFormatImpl childFormat,
    ExtendedFormatImpl parentFormat,
    bool bSetFlag)
  {
    parentFormat.ColorObject.CopyFrom(childFormat.ColorObject, true);
    parentFormat.PatternColorObject.CopyFrom(childFormat.PatternColorObject, true);
    parentFormat.FillPattern = childFormat.FillPattern;
    if (!bSetFlag)
      return;
    childFormat.IncludePatterns = false;
  }

  private static void CopyProtection(
    ExtendedFormatImpl childFormat,
    ExtendedFormatImpl parentFormat,
    bool bSetFlag)
  {
    parentFormat.FormulaHidden = childFormat.FormulaHidden;
    parentFormat.Locked = childFormat.Locked;
    if (!bSetFlag)
      return;
    childFormat.IncludeProtection = false;
  }

  protected internal void CopyColorsFrom(ExtendedFormatImpl format)
  {
    this.m_color.CopyFrom(format.m_color, false);
    this.m_patternColor.CopyFrom(format.m_patternColor, false);
    this.m_topBorderColor.CopyFrom(format.m_topBorderColor, false);
    this.m_bottomBorderColor.CopyFrom(format.m_bottomBorderColor, false);
    this.m_leftBorderColor.CopyFrom(format.m_leftBorderColor, false);
    this.m_rightBorderColor.CopyFrom(format.m_rightBorderColor, false);
    this.m_diagonalBorderColor.CopyFrom(format.m_diagonalBorderColor, false);
  }

  public object Clone() => (object) this.TypedClone((object) this);

  public ExtendedFormatImpl TypedClone(object parent)
  {
    ExtendedFormatImpl parent1 = this.MemberwiseClone() as ExtendedFormatImpl;
    parent1.m_extFormat = this.m_extFormat.Clone() as ExtendedFormatRecord;
    parent1.m_xfExt = this.m_xfExt.CloneObject();
    if (parent != parent1.Parent)
    {
      parent1.SetParent(parent);
      parent1.FindParents();
    }
    if (parent1.m_gradient != null)
      parent1.m_gradient = this.m_gradient.Clone((object) parent1);
    if (this.ParentIndex == this.m_book.MaxXFCount)
      parent1.ParentIndex = parent1.m_book.MaxXFCount;
    parent1.InitializeColors();
    parent1.m_color.CopyFrom(this.m_color, false);
    parent1.m_patternColor.CopyFrom(this.m_patternColor, false);
    parent1.m_topBorderColor.CopyFrom(this.m_topBorderColor, false);
    parent1.m_bottomBorderColor.CopyFrom(this.m_bottomBorderColor, false);
    parent1.m_leftBorderColor.CopyFrom(this.m_leftBorderColor, false);
    parent1.m_rightBorderColor.CopyFrom(this.m_rightBorderColor, false);
    parent1.m_diagonalBorderColor.CopyFrom(this.m_diagonalBorderColor, false);
    return parent1;
  }

  object ICloneParent.Clone(object parent) => (object) this.TypedClone(parent);

  public void Clear()
  {
    this.m_gradient = (ShapeFillImpl) null;
    this.m_xfExt = (ExtendedXFRecord) null;
    this.m_extFormat = (ExtendedFormatRecord) null;
    this.m_color = (ColorObject) null;
    this.m_patternColor = (ColorObject) null;
    this.m_topBorderColor = (ColorObject) null;
    this.m_bottomBorderColor = (ColorObject) null;
    this.m_leftBorderColor = (ColorObject) null;
    this.m_rightBorderColor = (ColorObject) null;
    this.m_diagonalBorderColor = (ColorObject) null;
    this.Dispose();
  }

  void IDisposable.Dispose() => GC.SuppressFinalize((object) this);

  internal void ClearAll()
  {
    if (this.m_gradient != null)
      this.m_gradient.Clear();
    if (this.m_color != (ColorObject) null)
      this.m_color.Dispose();
    if (this.m_patternColor != (ColorObject) null)
      this.m_patternColor.Dispose();
    if (this.m_topBorderColor != (ColorObject) null)
      this.m_topBorderColor.Dispose();
    if (this.m_bottomBorderColor != (ColorObject) null)
      this.m_bottomBorderColor.Dispose();
    if (this.m_leftBorderColor != (ColorObject) null)
      this.m_leftBorderColor.Dispose();
    if (this.m_rightBorderColor != (ColorObject) null)
      this.m_rightBorderColor.Dispose();
    if (this.m_diagonalBorderColor != (ColorObject) null)
      this.m_diagonalBorderColor.Dispose();
    if (this.m_extFormat != null)
      this.m_extFormat = (ExtendedFormatRecord) null;
    this.Clear();
  }

  private void CheckAndUpdateHasBorder(ExtendedFormatRecord record)
  {
    if ((record.BorderBottom | record.BorderLeft | record.BorderTop | record.BorderRight) > ExcelLineStyle.None || record.DiagonalLineStyle > (ushort) 0)
      this.m_hasBorder = true;
    else
      this.m_hasBorder = false;
  }
}
