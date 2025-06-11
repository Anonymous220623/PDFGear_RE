// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExtendedFormatWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ExtendedFormatWrapper : 
  CommonWrapper,
  IInternalExtendedFormat,
  IXFIndex,
  IStyle,
  IExtendedFormat,
  IParentApplication,
  IOptimizedUpdate,
  ICloneParent
{
  protected internal ExtendedFormatImpl m_xFormat;
  protected WorkbookImpl m_book;
  protected FontWrapper m_font;
  private BordersCollection m_borders;
  private InteriorWrapper m_interior;

  public ExtendedFormatWrapper(WorkbookImpl book) => this.m_book = book;

  public ExtendedFormatWrapper(WorkbookImpl book, int iXFIndex)
    : this(book)
  {
    this.SetFormatIndex(iXFIndex);
  }

  public void ChangeFillPattern()
  {
    switch (this.m_xFormat.FillPattern)
    {
      case ExcelPattern.None:
      case ExcelPattern.Gradient:
        this.m_xFormat.FillPattern = ExcelPattern.Solid;
        this.m_xFormat.Gradient = (IGradient) null;
        break;
    }
  }

  public void SetFormatIndex(int index)
  {
    if (this.m_xFormat != null && this.m_xFormat.Index == index && !this.m_book.m_bisUnusedXFRemoved)
      return;
    this.m_xFormat = index <= this.m_book.InnerExtFormats.Count ? this.m_book.InnerExtFormats[index] : this.m_book.InnerExtFormats[this.m_book.DefaultXFIndex];
    if (this.m_book.InnerFonts.Count <= this.m_xFormat.FontIndex)
      this.m_xFormat.FontIndex = 0;
    int fontIndex = this.m_xFormat.FontIndex;
    if (this.m_book.InnerFonts.Count == 0)
      this.m_book.InnerFonts.Add(this.m_book.CreateFont((IFont) null, false));
    FontImpl innerFont = this.m_book.InnerFonts[fontIndex] as FontImpl;
    if (this.m_font == null)
    {
      this.m_font = new FontWrapper();
      this.m_font.AfterChangeEvent += new EventHandler(this.WrappedFontAfterChangeEvent);
    }
    this.m_font.Wrapped = innerFont;
  }

  public void UpdateFont() => this.m_font.Wrapped = (FontImpl) this.m_xFormat.Font;

  protected virtual void SetParents(object parent)
  {
    this.m_book = CommonObject.FindParent(parent, typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Workbook", "Can't find parent workbook");
  }

  protected void SetChanged() => this.m_book.SetChanged();

  private void WrappedFontAfterChangeEvent(object sender, EventArgs e)
  {
    this.FontIndex = this.m_font.FontIndex;
  }

  private void WrappedInteriorAfterChangeEvent(object sender, EventArgs e)
  {
    this.BeginUpdate();
    this.m_xFormat = this.m_interior.Wrapped;
    this.EndUpdate();
  }

  protected void OnNumberFormatChange()
  {
    if (this.NumberFormatChanged == null)
      return;
    this.NumberFormatChanged((object) this, EventArgs.Empty);
  }

  public override object Clone(object parent)
  {
    ExtendedFormatWrapper extendedFormatWrapper = (ExtendedFormatWrapper) base.Clone(parent);
    extendedFormatWrapper.m_book = CommonObject.FindParent(parent, typeof (WorkbookImpl)) as WorkbookImpl;
    if (extendedFormatWrapper.m_book == null)
      throw new ArgumentOutOfRangeException(nameof (parent), "Can't find parent workbook.");
    extendedFormatWrapper.m_borders = (BordersCollection) null;
    extendedFormatWrapper.m_xFormat = (ExtendedFormatImpl) null;
    extendedFormatWrapper.m_font = (FontWrapper) null;
    extendedFormatWrapper.SetFormatIndex(this.m_xFormat.Index);
    return (object) extendedFormatWrapper;
  }

  protected virtual void BeforeRead()
  {
  }

  private IStyle GetStyle()
  {
    return (IStyle) this.m_book.InnerStyles.GetByXFIndex(this.m_xFormat.HasParent ? this.m_xFormat.ParentIndex : this.m_xFormat.XFormatIndex);
  }

  public WorkbookImpl Workbook => this.m_book;

  public ExcelPattern FillPattern
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.FillPattern;
    }
    set
    {
      if (this.FillPattern == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.FillPattern = value;
      this.EndUpdate();
    }
  }

  public int XFormatIndex
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.XFormatIndex;
    }
  }

  public ExcelKnownColors FillBackground
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.FillBackground;
    }
    set
    {
      if (this.FillBackground == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.FillBackground = value;
      this.ChangeFillPattern();
      this.EndUpdate();
    }
  }

  public Color FillBackgroundRGB
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.FillBackgroundRGB;
    }
    set
    {
      if (!(this.FillBackgroundRGB != value))
        return;
      this.BeginUpdate();
      this.m_xFormat.FillBackgroundRGB = value;
      this.ChangeFillPattern();
      this.EndUpdate();
    }
  }

  public ExcelKnownColors FillForeground
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.FillForeground;
    }
    set
    {
      if (this.FillForeground == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.FillForeground = value;
      this.ChangeFillPattern();
      this.EndUpdate();
    }
  }

  public Color FillForegroundRGB
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.FillForegroundRGB;
    }
    set
    {
      if (!(this.FillForegroundRGB != value))
        return;
      this.BeginUpdate();
      this.m_xFormat.FillForegroundRGB = value;
      this.ChangeFillPattern();
      this.EndUpdate();
    }
  }

  public int NumberFormatIndex
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.NumberFormatIndex;
    }
    set
    {
      if (this.NumberFormatIndex == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.NumberFormatIndex = value;
      this.EndUpdate();
      this.OnNumberFormatChange();
    }
  }

  public ExcelHAlign HorizontalAlignment
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.HorizontalAlignment;
    }
    set
    {
      if (this.HorizontalAlignment == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.HorizontalAlignment = value;
      this.EndUpdate();
    }
  }

  public bool IncludeAlignment
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.IncludeAlignment;
    }
    set
    {
      if (this.IncludeAlignment == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.IncludeAlignment = value;
      this.EndUpdate();
    }
  }

  public bool IncludeBorder
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.IncludeBorder;
    }
    set
    {
      if (this.IncludeBorder == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.IncludeBorder = value;
      this.EndUpdate();
    }
  }

  public bool IncludeFont
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.IncludeFont;
    }
    set
    {
      if (this.IncludeFont == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.IncludeFont = value;
      this.EndUpdate();
    }
  }

  public bool IncludeNumberFormat
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.IncludeNumberFormat;
    }
    set
    {
      if (this.IncludeNumberFormat == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.IncludeNumberFormat = value;
      this.EndUpdate();
    }
  }

  public bool IncludePatterns
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.IncludePatterns;
    }
    set
    {
      if (this.IncludePatterns == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.IncludePatterns = value;
      this.EndUpdate();
    }
  }

  public bool IncludeProtection
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.IncludeProtection;
    }
    set
    {
      if (this.IncludeProtection == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.IncludeProtection = value;
      this.EndUpdate();
    }
  }

  public int IndentLevel
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.IndentLevel;
    }
    set
    {
      if (this.IndentLevel == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.IndentLevel = value;
      this.EndUpdate();
    }
  }

  public bool FormulaHidden
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.FormulaHidden;
    }
    set
    {
      if (this.FormulaHidden == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.FormulaHidden = value;
      this.EndUpdate();
    }
  }

  public bool Locked
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.Locked;
    }
    set
    {
      if (this.Locked == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.Locked = value;
      this.EndUpdate();
    }
  }

  public bool JustifyLast
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.JustifyLast;
    }
    set
    {
      if (this.JustifyLast == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.JustifyLast = value;
      this.EndUpdate();
    }
  }

  public string NumberFormat
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.NumberFormat;
    }
    set
    {
      if (!(this.NumberFormat != value))
        return;
      this.BeginUpdate();
      this.m_xFormat.NumberFormat = value;
      this.EndUpdate();
      this.OnNumberFormatChange();
    }
  }

  public string NumberFormatLocal
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.NumberFormatLocal;
    }
    set
    {
      if (!(this.NumberFormatLocal != value))
        return;
      this.BeginUpdate();
      this.m_xFormat.NumberFormatLocal = value;
      this.EndUpdate();
      this.OnNumberFormatChange();
    }
  }

  public INumberFormat NumberFormatSettings
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.NumberFormatSettings;
    }
  }

  public ExcelReadingOrderType ReadingOrder
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.ReadingOrder;
    }
    set
    {
      if (this.ReadingOrder == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.ReadingOrder = value;
      this.EndUpdate();
    }
  }

  public int Rotation
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.Rotation;
    }
    set
    {
      if (this.Rotation == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.Rotation = value;
      this.EndUpdate();
    }
  }

  public bool ShrinkToFit
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.ShrinkToFit;
    }
    set
    {
      if (this.ShrinkToFit == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.ShrinkToFit = value;
      this.EndUpdate();
    }
  }

  public ExcelVAlign VerticalAlignment
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.VerticalAlignment;
    }
    set
    {
      if (this.VerticalAlignment == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.VerticalAlignment = value;
      this.EndUpdate();
    }
  }

  public bool WrapText
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.WrapText;
    }
    set
    {
      if (this.WrapText == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.WrapText = value;
      this.EndUpdate();
    }
  }

  public IFont Font
  {
    get
    {
      this.BeforeRead();
      return (IFont) this.m_font;
    }
  }

  public IBorders Borders
  {
    get
    {
      this.BeforeRead();
      if (this.m_borders == null)
        this.m_borders = new BordersCollection(this.Application, (object) this, (IInternalExtendedFormat) this);
      return (IBorders) this.m_borders;
    }
  }

  public bool IsFirstSymbolApostrophe
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.IsFirstSymbolApostrophe;
    }
    set
    {
      if (this.IsFirstSymbolApostrophe == value)
        return;
      if (!this.m_book.Loading)
        this.BeginUpdate();
      this.m_xFormat.IsFirstSymbolApostrophe = value;
      if (this.m_book.Loading)
        return;
      this.EndUpdate();
    }
  }

  public ExcelKnownColors PatternColorIndex
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.PatternColorIndex;
    }
    set
    {
      if (this.PatternColorIndex == value && this.FillPattern != ExcelPattern.Gradient)
        return;
      this.BeginUpdate();
      this.m_xFormat.PatternColorIndex = value;
      this.ChangeFillPattern();
      this.EndUpdate();
    }
  }

  public Color PatternColor
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.PatternColor;
    }
    set
    {
      if (!(this.PatternColor != value) && this.PatternColorIndex != ExcelKnownColors.BlackCustom)
        return;
      this.BeginUpdate();
      this.m_xFormat.PatternColor = value;
      this.ChangeFillPattern();
      this.EndUpdate();
    }
  }

  public ExcelKnownColors ColorIndex
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.ColorIndex;
    }
    set
    {
      if (this.FillPattern != ExcelPattern.Gradient && this.ColorIndex == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.ColorIndex = value;
      this.ChangeFillPattern();
      this.EndUpdate();
    }
  }

  public Color Color
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.Color;
    }
    set
    {
      if (!(this.Color != value) && this.ColorIndex != ExcelKnownColors.None)
        return;
      this.BeginUpdate();
      this.m_xFormat.Color = value;
      this.ChangeFillPattern();
      this.EndUpdate();
    }
  }

  public IInterior Interior
  {
    get
    {
      if (this.m_interior == null)
      {
        this.m_interior = new InteriorWrapper(this.m_xFormat);
        this.m_interior.AfterChangeEvent += new EventHandler(this.WrappedInteriorAfterChangeEvent);
      }
      this.BeforeRead();
      return (IInterior) this.m_interior;
    }
  }

  public bool IsModified
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.IsModified;
    }
  }

  public int FontIndex
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.FontIndex;
    }
    set
    {
      if (this.FontIndex == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.FontIndex = value;
      this.EndUpdate();
    }
  }

  public ExtendedFormatImpl Wrapped
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat;
    }
  }

  public bool HasBorder => this.m_xFormat.HasBorder;

  public virtual ColorObject BottomBorderColor
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.BottomBorderColor;
    }
  }

  public virtual ColorObject TopBorderColor
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.TopBorderColor;
    }
  }

  public virtual ColorObject LeftBorderColor
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.LeftBorderColor;
    }
  }

  public virtual ColorObject RightBorderColor
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.RightBorderColor;
    }
  }

  public ColorObject DiagonalBorderColor
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.DiagonalBorderColor;
    }
  }

  public virtual ExcelLineStyle LeftBorderLineStyle
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.LeftBorderLineStyle;
    }
    set
    {
      this.BeginUpdate();
      this.m_xFormat.LeftBorderLineStyle = value;
      this.EndUpdate();
    }
  }

  public virtual ExcelLineStyle RightBorderLineStyle
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.RightBorderLineStyle;
    }
    set
    {
      this.BeginUpdate();
      this.m_xFormat.RightBorderLineStyle = value;
      this.EndUpdate();
    }
  }

  public virtual ExcelLineStyle TopBorderLineStyle
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.TopBorderLineStyle;
    }
    set
    {
      this.BeginUpdate();
      this.m_xFormat.TopBorderLineStyle = value;
      this.EndUpdate();
    }
  }

  public virtual ExcelLineStyle BottomBorderLineStyle
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.BottomBorderLineStyle;
    }
    set
    {
      this.BeginUpdate();
      this.m_xFormat.BottomBorderLineStyle = value;
      this.EndUpdate();
    }
  }

  public ExcelLineStyle DiagonalUpBorderLineStyle
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.DiagonalUpBorderLineStyle;
    }
    set
    {
      bool flag = false;
      if (this.DiagonalUpBorderLineStyle != value)
      {
        this.BeginUpdate();
        this.m_xFormat.DiagonalUpBorderLineStyle = value;
        flag = true;
      }
      if (!this.m_xFormat.DiagonalUpVisible && value != ExcelLineStyle.None)
      {
        if (!flag)
          this.BeginUpdate();
        this.m_xFormat.DiagonalUpVisible = true;
        flag = true;
      }
      if (!flag)
        return;
      this.EndUpdate();
    }
  }

  public ExcelLineStyle DiagonalDownBorderLineStyle
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.DiagonalDownBorderLineStyle;
    }
    set
    {
      bool flag = false;
      if (this.DiagonalDownBorderLineStyle != value)
      {
        this.BeginUpdate();
        this.m_xFormat.DiagonalDownBorderLineStyle = value;
        flag = true;
      }
      if (!this.m_xFormat.DiagonalDownVisible)
      {
        if (!flag)
          this.BeginUpdate();
        this.m_xFormat.DiagonalDownVisible = true;
        flag = true;
      }
      if (!flag)
        return;
      this.EndUpdate();
    }
  }

  public bool DiagonalUpVisible
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.DiagonalUpVisible;
    }
    set
    {
      if (this.DiagonalUpVisible == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.DiagonalUpVisible = value;
      this.EndUpdate();
    }
  }

  public bool DiagonalDownVisible
  {
    get
    {
      this.BeforeRead();
      return this.m_xFormat.DiagonalDownVisible;
    }
    set
    {
      if (this.DiagonalDownVisible == value)
        return;
      this.BeginUpdate();
      this.m_xFormat.DiagonalDownVisible = value;
      this.EndUpdate();
    }
  }

  public event EventHandler NumberFormatChanged;

  public IApplication Application => this.m_xFormat.Application;

  public object Parent => this.m_xFormat.Parent;

  public bool BuiltIn => this.GetStyle().BuiltIn;

  public string Name
  {
    get
    {
      this.BeforeRead();
      StyleImpl styleImpl = this.m_book.InnerStyles.GetByXFIndex((int) this.m_xFormat.Record.ParentIndex);
      if (styleImpl == null)
      {
        styleImpl = this.m_book.InnerStyles["Normal"] as StyleImpl;
        this.m_xFormat.ParentIndex = (int) (ushort) styleImpl.Index;
      }
      return styleImpl.Name;
    }
  }

  public bool IsInitialized
  {
    get
    {
      this.BeforeRead();
      return !StylesCollection.CompareStyles((IStyle) this, this.m_book.Styles[this.m_book.AppImplementation.DefaultStyleNames[0]]);
    }
  }

  public override void BeginUpdate() => base.BeginUpdate();

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount != 0)
      return;
    this.SetChanged();
  }

  internal void Dispose()
  {
    this.NumberFormatChanged = (EventHandler) null;
    this.m_xFormat.ClearAll();
    this.m_font.Dispose();
    if (this.m_borders != null)
      this.m_borders.Clear();
    if (this.m_interior == null)
      return;
    this.m_interior.Dispose();
  }
}
