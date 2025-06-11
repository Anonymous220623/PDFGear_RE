// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.FontWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class FontWrapper : CommonWrapper, IInternalFont, IFont, IParentApplication, IOptimizedUpdate
{
  private FontImpl m_font;
  private int m_charSet;
  private bool m_bReadOnly;
  private bool m_bRaiseEvents = true;
  private bool m_bDirectAccess;
  private ColorObject m_fontColor;
  private bool m_bIsAutoColor = true;
  private IRange m_range;

  public FontWrapper()
  {
    this.m_fontColor = new ColorObject(ColorExtension.Black);
    this.m_fontColor.AfterChange += new ColorObject.AfterChangeHandler(this.ColorObjectUpdate);
  }

  public FontWrapper(FontImpl font)
    : this()
  {
    this.m_font = font != null ? font : throw new ArgumentNullException(nameof (font));
    this.m_fontColor.CopyFrom(font.ColorObject, false);
  }

  public FontWrapper(FontImpl font, bool bReadOnly, bool bRaiseEvents)
    : this(font)
  {
    this.m_bReadOnly = bReadOnly;
    this.m_bRaiseEvents = bRaiseEvents;
  }

  public bool Bold
  {
    get => this.m_font.Bold;
    set
    {
      if (value == this.Bold)
        return;
      this.BeginUpdate();
      this.m_font.Bold = value;
      if (this.Range != null && this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public ExcelKnownColors Color
  {
    get => this.m_font.Color;
    set
    {
      if (value == this.Color)
        return;
      this.BeginUpdate();
      if (this.Workbook.OwnPalette)
        this.m_fontColor.SetIndexed(new ColorObject(value).GetRGB((IWorkbook) this.Workbook), this.Workbook, value, true);
      else
        this.m_fontColor.SetIndexed(value);
      this.m_bIsAutoColor = false;
      if (this.Range != null && this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public System.Drawing.Color RGBColor
  {
    get => this.m_font.RGBColor;
    set
    {
      if (!(value != this.RGBColor))
        return;
      this.BeginUpdate();
      this.m_fontColor.SetRGB(value);
      this.m_bIsAutoColor = false;
      if (this.Range != null && this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public bool Italic
  {
    get => this.m_font.Italic;
    set
    {
      if (value == this.Italic)
        return;
      this.BeginUpdate();
      this.m_font.Italic = value;
      if (this.Range != null && this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public bool MacOSOutlineFont
  {
    get => this.m_font.MacOSOutlineFont;
    set
    {
      if (value == this.MacOSOutlineFont)
        return;
      this.BeginUpdate();
      this.m_font.MacOSOutlineFont = value;
      this.EndUpdate();
    }
  }

  public bool MacOSShadow
  {
    get => this.m_font.MacOSShadow;
    set
    {
      if (value == this.MacOSShadow)
        return;
      this.BeginUpdate();
      this.m_font.MacOSShadow = value;
      this.EndUpdate();
    }
  }

  public double Size
  {
    get => this.m_font.Size;
    set
    {
      if (value == this.Size)
        return;
      double num = this.Size;
      this.BeginUpdate();
      this.m_font.Size = value;
      this.EndUpdate();
      if (this.Range == null)
        return;
      if (this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      if (this.Application.SkipAutoFitRow)
        return;
      ExtendedFormatsCollection innerExtFormats = this.Workbook.InnerExtFormats;
      RowStorage row = WorksheetHelper.GetOrCreateRow(this.Range.Worksheet as IInternalWorksheet, this.Range.Row - 1, true);
      if (row != null && (row.IsBadFontHeight || this.Range.IsMerged))
        return;
      bool flag = true;
      RowStorageEnumerator storageEnumerator = new RowStorageEnumerator(row, (this.Range.Worksheet as WorksheetImpl).RecordExtractor);
      if (num < value)
        num = value;
      while (storageEnumerator.MoveNext())
      {
        if (storageEnumerator.ColumnIndex + 1 != this.Range.Column && num <= innerExtFormats[storageEnumerator.XFIndex].Font.Size)
        {
          flag = false;
          break;
        }
      }
      if (!flag)
        return;
      this.Range.AutofitRows();
    }
  }

  public bool Strikethrough
  {
    get => this.m_font.Strikethrough;
    set
    {
      if (value == this.Strikethrough)
        return;
      this.BeginUpdate();
      this.m_font.Strikethrough = value;
      if (this.Range != null && this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public int Baseline
  {
    get => this.m_font.BaseLine;
    set => this.m_font.BaseLine = value;
  }

  public bool Subscript
  {
    get => this.m_font.Subscript;
    set
    {
      if (value == this.Subscript)
        return;
      this.BeginUpdate();
      this.m_font.Subscript = value;
      if (this.Range != null && this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public bool Superscript
  {
    get => this.m_font.Superscript;
    set
    {
      if (value == this.Superscript)
        return;
      this.BeginUpdate();
      this.m_font.Superscript = value;
      if (this.Range != null && this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public ExcelUnderline Underline
  {
    get => this.m_font.Underline;
    set
    {
      if (value == this.Underline)
        return;
      this.BeginUpdate();
      this.m_font.Underline = value;
      if (this.Range != null && this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public string FontName
  {
    get => this.m_font.FontName;
    set
    {
      if (!(value != this.FontName))
        return;
      this.BeginUpdate();
      this.m_font.FontName = value;
      if (this.Range != null && this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  internal int CharSet
  {
    get => this.m_charSet;
    set => this.m_charSet = value;
  }

  public ExcelFontVertialAlignment VerticalAlignment
  {
    get => this.m_font.VerticalAlignment;
    set
    {
      if (value == this.VerticalAlignment)
        return;
      this.BeginUpdate();
      this.m_font.VerticalAlignment = value;
      if (this.m_font.VerticalAlignment == ExcelFontVertialAlignment.Superscript)
      {
        this.Superscript = true;
        this.Baseline = 30;
      }
      else if (this.m_font.VerticalAlignment == ExcelFontVertialAlignment.Subscript)
      {
        this.Subscript = true;
        this.Baseline = -25;
      }
      if (this.Range != null && this.Range.HasRichText)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public System.Drawing.Font GenerateNativeFont() => this.m_font.GenerateNativeFont();

  public bool IsAutoColor
  {
    get => this.m_bIsAutoColor;
    set => this.m_bIsAutoColor = value;
  }

  public IApplication Application => this.m_font.Application;

  public object Parent => this.m_font.Parent;

  public void ColorObjectUpdate()
  {
    this.BeginUpdate();
    this.m_font.ColorObject.CopyFrom(this.m_fontColor, true);
    this.EndUpdate();
  }

  public FontWrapper Clone(WorkbookImpl book, object parent, IDictionary dicFontIndexes)
  {
    FontWrapper fontWrapper = new FontWrapper();
    int num = this.m_font.Index;
    if (dicFontIndexes != null)
      num = (int) dicFontIndexes[(object) num];
    fontWrapper.m_bReadOnly = this.m_bReadOnly;
    fontWrapper.m_font = (FontImpl) book.InnerFonts[num];
    fontWrapper.m_bIsAutoColor = this.m_bIsAutoColor;
    return fontWrapper;
  }

  public event EventHandler AfterChangeEvent;

  public int FontIndex => this.m_font.Index;

  public FontImpl Wrapped
  {
    get => this.m_font;
    set => this.m_font = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  public bool IsReadOnly
  {
    get => this.m_bReadOnly;
    set
    {
      this.m_bReadOnly = value || !this.m_bReadOnly ? value : throw new ArgumentOutOfRangeException("Can't change this property for read-only fonts");
    }
  }

  public WorkbookImpl Workbook => this.m_font.ParentWorkbook;

  public bool IsDirectAccess
  {
    get => this.m_bDirectAccess;
    set => this.m_bDirectAccess = value;
  }

  public ColorObject ColorObject => this.m_fontColor;

  internal IRange Range
  {
    get => this.m_range;
    set => this.m_range = value;
  }

  public int Index => this.m_font.Index;

  public FontImpl Font => this.m_font;

  public override void BeginUpdate()
  {
    if (this.BeginCallsCount == 0)
    {
      if (this.m_bReadOnly)
        throw new ReadOnlyException();
      if (!this.m_bRaiseEvents)
        return;
      if (!this.m_bDirectAccess)
      {
        this.m_font = (FontImpl) this.Workbook.CreateFont((IFont) this.m_font, false);
        this.m_font.CanDispose = true;
      }
    }
    base.BeginUpdate();
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount != 0 || !this.m_bRaiseEvents)
      return;
    WorkbookImpl workbook = this.Workbook;
    if (!this.m_bDirectAccess)
    {
      this.m_font = (FontImpl) workbook.AddFont((IFont) this.m_font);
      this.m_font.CanDispose = false;
    }
    workbook.SetChanged();
    if (this.AfterChangeEvent == null)
      return;
    this.AfterChangeEvent((object) this, EventArgs.Empty);
  }

  internal void InvokeAfterChange()
  {
    if (this.AfterChangeEvent == null)
      return;
    this.AfterChangeEvent((object) this, EventArgs.Empty);
  }

  internal void Dispose()
  {
    this.AfterChangeEvent = (EventHandler) null;
    this.m_fontColor.Dispose();
    this.m_font.Clear();
    this.m_font = (FontImpl) null;
    this.m_fontColor = (ColorObject) null;
  }

  public override bool Equals(object obj)
  {
    return obj is FontWrapper fontWrapper && this.m_charSet == fontWrapper.m_charSet && this.m_font.Equals((object) fontWrapper.m_font) && this.m_fontColor.Equals((object) fontWrapper.m_fontColor);
  }
}
