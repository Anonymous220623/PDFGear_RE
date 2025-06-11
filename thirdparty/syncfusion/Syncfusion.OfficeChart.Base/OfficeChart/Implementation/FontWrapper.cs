// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.FontWrapper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using Syncfusion.OfficeChart.Interfaces;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class FontWrapper : 
  CommonWrapper,
  IInternalFont,
  IOfficeFont,
  IParentApplication,
  IOptimizedUpdate
{
  private FontImpl m_font;
  private int m_charSet;
  private bool m_bReadOnly;
  private bool m_bRaiseEvents = true;
  private bool m_bDirectAccess;
  private ChartColor m_fontColor;
  private bool m_bIsAutoColor = true;
  private IRange m_range;

  public FontWrapper()
  {
    this.m_fontColor = new ChartColor(ColorExtension.Black);
    this.m_fontColor.AfterChange += new ChartColor.AfterChangeHandler(this.ColorObjectUpdate);
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
      if (this.Range != null)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public OfficeKnownColors Color
  {
    get => this.m_font.Color;
    set
    {
      if (value == this.Color)
        return;
      this.BeginUpdate();
      this.m_fontColor.SetIndexed(value);
      this.m_bIsAutoColor = false;
      if (this.Range != null)
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
      if (this.Range != null)
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
      if (this.Range != null)
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
      this.BeginUpdate();
      this.m_font.Size = value;
      if (this.Range != null)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
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
      if (this.Range != null)
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
      if (this.Range != null)
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
      if (this.Range != null)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  public OfficeUnderline Underline
  {
    get => this.m_font.Underline;
    set
    {
      if (value == this.Underline)
        return;
      this.BeginUpdate();
      this.m_font.Underline = value;
      if (this.Range != null)
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
      if (this.Range != null)
        (this.Range.RichText as RichTextString).UpdateRTF(this.Range, this.m_font);
      this.EndUpdate();
    }
  }

  internal int CharSet
  {
    get => this.m_charSet;
    set => this.m_charSet = value;
  }

  public OfficeFontVerticalAlignment VerticalAlignment
  {
    get => this.m_font.VerticalAlignment;
    set
    {
      if (value == this.VerticalAlignment)
        return;
      this.BeginUpdate();
      this.m_font.VerticalAlignment = value;
      if (this.Range != null)
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
    fontWrapper.m_font = ((FontImpl) book.InnerFonts[num]).Clone((object) book);
    fontWrapper.m_bIsAutoColor = this.m_bIsAutoColor;
    fontWrapper.m_font.Index = num;
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

  public ChartColor ColorObject => this.m_fontColor;

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
        this.m_font = (FontImpl) this.Workbook.CreateFont((IOfficeFont) this.m_font, false);
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
      this.m_font = (FontImpl) workbook.AddFont((IOfficeFont) this.m_font);
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
    this.m_fontColor = (ChartColor) null;
  }
}
