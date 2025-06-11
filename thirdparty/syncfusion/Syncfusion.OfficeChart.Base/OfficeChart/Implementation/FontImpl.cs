// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.FontImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.XmlReaders;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class FontImpl : 
  CommonObject,
  ICloneable,
  IComparable,
  IInternalFont,
  IOfficeFont,
  IParentApplication,
  IOptimizedUpdate,
  ICloneParent,
  IDisposable
{
  internal const ushort FONTBOLD = 700;
  internal const ushort FONTNORMAL = 400;
  private const int DEF_INCORRECT_INDEX = -1;
  internal const int DEF_BAD_INDEX = 4;
  private const float DEF_SMALL_BOLD_FONT_MULTIPLIER = 1.15f;
  private const float DEF_BOLD_FONT_MULTIPLIER = 1.07f;
  private const int DEF_INDEX = 64 /*0x40*/;
  private static readonly CharacterRange[] characterRanges = new CharacterRange[2]
  {
    new CharacterRange(0, 2),
    new CharacterRange(1, 1)
  };
  private FontRecord m_font;
  private WorkbookImpl m_book;
  private int m_index = -1;
  private byte m_btCharSet = 1;
  private System.Drawing.Font m_fontNative;
  private ChartColor m_color;
  private string m_strLanguage;
  private string m_scheme;
  private int m_family;
  private bool m_hasLatin;
  private bool m_hasComplexScripts;
  private bool m_hasEastAsianFont;
  private string m_actualFont;
  private string[] m_arrItalicFonts = new string[1]
  {
    "Brush Script MT"
  };
  internal TextSettings m_textSettings;
  private Dictionary<string, Stream> _preservedElements;
  private Excel2007CommentHAlign m_paraAlign = Excel2007CommentHAlign.l;
  private bool m_bHasParaAlign;
  internal bool showFontName = true;

  public bool Bold
  {
    get => this.m_font.BoldWeight >= (ushort) 700;
    set
    {
      if (!value)
      {
        if (this.m_font.IsItalic)
        {
          System.Drawing.Font font1 = new System.Drawing.Font(this.m_font.FontName, 10f, FontStyle.Italic);
        }
        else
        {
          System.Drawing.Font font2 = new System.Drawing.Font(this.m_font.FontName, 10f, FontStyle.Regular);
        }
      }
      if (value == this.Bold)
        return;
      this.m_font.BoldWeight = value ? (ushort) 700 : (ushort) 400;
      this.SetChanged();
    }
  }

  public OfficeKnownColors Color
  {
    get => this.m_color.GetIndexed((IWorkbook) this.m_book);
    set => this.m_color.SetIndexed(value);
  }

  public System.Drawing.Color RGBColor
  {
    get => this.m_color.GetRGB((IWorkbook) this.m_book);
    set => this.m_color.SetRGB(value, (IWorkbook) this.m_book);
  }

  public bool Italic
  {
    get => this.m_font.IsItalic;
    set
    {
      if (!value)
      {
        System.Drawing.Font font1 = new System.Drawing.Font(this.m_font.FontName, 10f, FontStyle.Bold);
      }
      else
      {
        System.Drawing.Font font2 = new System.Drawing.Font(this.m_font.FontName, 10f, this.GetSupportedFontStyle(this.m_font.FontName));
      }
      if (this.m_font.IsItalic == value)
        return;
      this.m_font.IsItalic = value;
      this.SetChanged();
    }
  }

  public bool MacOSOutlineFont
  {
    get => this.m_font.IsMacOutline;
    set
    {
      this.m_font.IsMacOutline = value;
      this.SetChanged();
    }
  }

  public bool MacOSShadow
  {
    get => this.m_font.IsMacShadow;
    set
    {
      this.m_font.IsMacShadow = value;
      this.SetChanged();
    }
  }

  public double Size
  {
    get => (double) this.m_font.FontHeight / 20.0;
    set
    {
      if (value < 1.0 || value > 409.0)
        throw new ArgumentOutOfRangeException("Font.Size", "Font.Size out of range. Size must be less then 409 and greater than 1.");
      if ((int) this.m_font.FontHeight == (int) (ushort) (value * 20.0))
        return;
      this.m_font.FontHeight = (ushort) (value * 20.0);
      this.SetChanged();
    }
  }

  public bool Strikethrough
  {
    get => this.m_font.IsStrikeout;
    set
    {
      this.m_font.IsStrikeout = value;
      this.SetChanged();
    }
  }

  public bool Subscript
  {
    get => this.m_font.SuperSubscript == OfficeFontVerticalAlignment.Subscript;
    set
    {
      if (value == this.Subscript)
        return;
      if (value)
      {
        this.m_font.SuperSubscript = OfficeFontVerticalAlignment.Subscript;
        if (this.BaseLine >= 0)
          this.BaseLine = -25000;
      }
      else if (this.m_font.SuperSubscript == OfficeFontVerticalAlignment.Subscript)
      {
        this.m_font.SuperSubscript = OfficeFontVerticalAlignment.Baseline;
        if (this.BaseLine < 0)
          this.BaseLine = 0;
      }
      this.SetChanged();
    }
  }

  public bool Superscript
  {
    get => this.m_font.SuperSubscript == OfficeFontVerticalAlignment.Superscript;
    set
    {
      if (value == this.Superscript)
        return;
      if (value)
      {
        this.m_font.SuperSubscript = OfficeFontVerticalAlignment.Superscript;
        if (this.BaseLine <= 0)
          this.BaseLine = 30000;
      }
      else if (this.m_font.SuperSubscript == OfficeFontVerticalAlignment.Superscript)
      {
        this.m_font.SuperSubscript = OfficeFontVerticalAlignment.Baseline;
        if (this.BaseLine > 0)
          this.BaseLine = 0;
      }
      this.SetChanged();
    }
  }

  public OfficeUnderline Underline
  {
    get => this.m_font.Underline;
    set
    {
      this.m_font.Underline = value;
      this.SetChanged();
    }
  }

  public string FontName
  {
    get => this.m_font.FontName;
    set
    {
      if (!(value != this.m_font.FontName))
        return;
      this.m_font.FontName = value;
      this.SetChanged();
      switch (this.GetSupportedFontStyle(this.m_font.FontName))
      {
        case FontStyle.Bold:
          this.m_font.BoldWeight = (ushort) 700;
          this.SetChanged();
          break;
        case FontStyle.Italic:
          this.m_font.IsItalic = true;
          this.SetChanged();
          break;
      }
    }
  }

  public OfficeFontVerticalAlignment VerticalAlignment
  {
    get => this.m_font.SuperSubscript;
    set => this.m_font.SuperSubscript = value;
  }

  public bool IsAutoColor => false;

  internal int BaseLine
  {
    get => this.m_font.Baseline;
    set => this.m_font.Baseline = value;
  }

  [CLSCompliant(false)]
  public FontRecord Record => this.m_font;

  internal WorkbookImpl ParentWorkbook
  {
    get
    {
      if (this.m_book == null)
        this.m_book = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
      return this.m_book;
    }
  }

  internal int Index
  {
    get => this.m_index;
    set
    {
      if (this.m_index == value)
        return;
      this.m_index = value;
    }
  }

  public byte CharSet
  {
    get => this.m_font.Charset;
    set => this.m_font.Charset = value;
  }

  internal Dictionary<string, Stream> PreservedElements
  {
    get => this._preservedElements ?? (this._preservedElements = new Dictionary<string, Stream>());
  }

  internal byte Family
  {
    get => this.m_font.Family;
    set => this.m_font.Family = value;
  }

  public ChartColor ColorObject => this.m_color;

  public string Language
  {
    get => this.m_strLanguage;
    set => this.m_strLanguage = value;
  }

  internal bool HasLatin
  {
    get => this.m_hasLatin;
    set => this.m_hasLatin = value;
  }

  internal bool HasComplexScripts
  {
    get => this.m_hasComplexScripts;
    set => this.m_hasComplexScripts = value;
  }

  internal bool HasEastAsianFont
  {
    get => this.m_hasEastAsianFont;
    set => this.m_hasEastAsianFont = value;
  }

  internal Excel2007CommentHAlign ParaAlign
  {
    get => this.m_paraAlign;
    set => this.m_paraAlign = value;
  }

  internal bool HasParagrapAlign
  {
    get => this.m_bHasParaAlign;
    set => this.m_bHasParaAlign = value;
  }

  public FontImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_font = (FontRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Font);
    this.m_font.FontName = this.AppImplementation.StandardFont;
    this.m_font.FontHeight = (ushort) FontImpl.SizeInTwips(this.AppImplementation.StandardFontSize);
    this.InitializeColor();
    this.InitializeParent();
  }

  [CLSCompliant(false)]
  public FontImpl(IApplication application, object parent, BiffReader reader)
    : this(application, parent)
  {
    this.Parse(reader);
  }

  [CLSCompliant(false)]
  public FontImpl(IApplication application, object parent, FontRecord record)
    : this(application, parent)
  {
    this.m_font = record;
    this.UpdateColor();
  }

  [CLSCompliant(false)]
  public FontImpl(IApplication application, object parent, FontImpl font)
    : this(application, parent)
  {
    this.m_font = font.Record;
    if (font != null)
      this.m_color = font.ColorObject;
    this.UpdateColor();
  }

  public FontImpl(IOfficeFont baseFont)
    : this(baseFont is FontImpl ? (baseFont as FontImpl).ParentWorkbook.Application : (baseFont as FontWrapper).Workbook.Application, baseFont.Parent)
  {
    switch (baseFont)
    {
      case FontImpl _:
        this.m_font = (FontRecord) ((FontImpl) baseFont).Record.Clone();
        break;
      case FontWrapper _:
        this.m_font = (FontRecord) ((FontWrapper) baseFont).Wrapped.Record.Clone();
        break;
      default:
        throw new ArgumentException("baseFont must be FontImpl or FontWrapper class instance");
    }
    this.UpdateColor();
  }

  public FontImpl(IApplication application, object parent, System.Drawing.Font nativeFont)
    : this(application, parent)
  {
    this.Parse(nativeFont);
  }

  private void InitializeColor()
  {
    this.m_color = new ChartColor((OfficeKnownColors) this.m_font.PaletteColorIndex);
    this.m_color.AfterChange += new ChartColor.AfterChangeHandler(this.UpdateRecord);
  }

  internal void UpdateRecord()
  {
    this.m_font.PaletteColorIndex = (ushort) this.m_color.GetIndexed((IWorkbook) this.m_book);
    this.SetChanged();
  }

  private void UpdateColor()
  {
    if (this.m_color.ColorType == ColorType.RGB || this.m_color.ColorType == ColorType.Theme)
      this.m_color.SetRGB(this.m_color.GetRGB((IWorkbook) this.m_book));
    else
      this.m_color.SetIndexed((OfficeKnownColors) this.m_font.PaletteColorIndex);
  }

  private void InitializeParent()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentException("Cannot find parent workbook.");
  }

  private void Parse(BiffReader reader)
  {
    BiffRecordRaw biffRecordRaw = !reader.IsEOF ? reader.GetRecord() : throw new ApplicationException("Reached end of stream. Font object cannot be initialized.");
    this.m_font = biffRecordRaw.TypeCode == TBIFFRecord.Font ? (FontRecord) biffRecordRaw : throw new ApplicationException("Record extracted from stream is not a Font Record");
    this.UpdateColor();
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records) => records.Add((IBiffStorage) this.m_font);

  public void CopyTo(FontImpl twin)
  {
    this.m_font.CopyTo((BiffRecordRaw) twin.m_font);
    twin.CharSet = this.CharSet;
  }

  public void SetChanged()
  {
    this.ParentWorkbook.Saved = false;
    this.m_fontNative = (System.Drawing.Font) null;
  }

  public System.Drawing.Font GenerateNativeFont()
  {
    if (this.m_fontNative == null)
      this.m_fontNative = this.GenerateNativeFont((float) this.Size);
    return this.m_fontNative;
  }

  public System.Drawing.Font GenerateNativeFont(float size)
  {
    FontStyle style = FontStyle.Regular;
    switch (this.GetSupportedFontStyle(this.m_font.FontName))
    {
      case FontStyle.Bold:
        this.Bold = true;
        break;
      case FontStyle.Italic:
        this.Italic = true;
        break;
    }
    if (this.Bold)
      style |= FontStyle.Bold;
    if (this.Italic)
      style |= FontStyle.Italic;
    if (this.Strikethrough)
      style |= FontStyle.Strikeout;
    if (this.Underline != OfficeUnderline.None)
      style |= FontStyle.Underline;
    if (Array.IndexOf<string>(this.m_arrItalicFonts, this.FontName) >= 0)
      style |= FontStyle.Italic;
    return new System.Drawing.Font(this.FontName, size, style, GraphicsUnit.Point, this.m_btCharSet);
  }

  public void Parse(System.Drawing.Font nativeFont)
  {
    this.FontName = nativeFont != null ? nativeFont.Name : throw new ArgumentNullException(nameof (nativeFont));
    this.Size = (double) (int) nativeFont.Size;
    this.Strikethrough = nativeFont.Strikeout;
    this.Bold = nativeFont.Bold;
    this.Italic = nativeFont.Italic;
    this.Underline = nativeFont.Underline ? OfficeUnderline.Single : OfficeUnderline.None;
    this.UpdateColor();
  }

  public SizeF MeasureString(string strValue)
  {
    System.Drawing.Size size = this.AppImplementation.MeasureString(strValue, this, new SizeF((float) int.MaxValue, (float) int.MaxValue)).ToSize();
    return new SizeF((float) size.Width, (float) (size.Height - 1));
  }

  public SizeF MeasureStringSpecial(string strValue) => new SizeF(0.0f, 0.0f);

  public SizeF MeasureCharacter(char value) => new SizeF(0.0f, 0.0f);

  private void RaiseIndexChangedEvent(ValueChangedEventArgs args)
  {
    if (this.IndexChanged == null)
      return;
    this.IndexChanged((object) this, args);
  }

  public FontImpl TypedClone()
  {
    FontImpl fontImpl = this.MemberwiseClone() as FontImpl;
    fontImpl.m_font = this.m_font.Clone() as FontRecord;
    return fontImpl;
  }

  public object Clone() => (object) this.TypedClone();

  public FontImpl Clone(object parent)
  {
    FontImpl fontImpl = new FontImpl(this.Application, parent);
    fontImpl.m_bIsDisposed = this.m_bIsDisposed;
    fontImpl.m_index = -1;
    fontImpl.m_font = (FontRecord) this.m_font.Clone();
    fontImpl.m_color = !(this.m_color != (ChartColor) null) || this.m_color.ColorType == ColorType.Indexed ? new ChartColor((OfficeKnownColors) fontImpl.m_font.PaletteColorIndex) : this.m_color.Clone();
    fontImpl.m_color.AfterChange += new ChartColor.AfterChangeHandler(this.UpdateRecord);
    return fontImpl;
  }

  public static int SizeInTwips(double fontSize) => (int) (fontSize * 20.0);

  public static double SizeInPoints(int twipsSize) => (double) twipsSize / 20.0;

  public static int UpdateFontIndexes(
    int iOldIndex,
    Dictionary<int, int> dicNewIndexes,
    OfficeParseOptions options)
  {
    int num = iOldIndex;
    dicNewIndexes?.TryGetValue(iOldIndex, out num);
    return num;
  }

  public FontStyle GetSupportedFontStyle(string fontName)
  {
    FontStyle[] fontStyleArray = new FontStyle[3]
    {
      FontStyle.Regular,
      FontStyle.Bold,
      FontStyle.Italic
    };
    for (int index = 0; index < fontStyleArray.Length; ++index)
    {
      try
      {
        System.Drawing.Font font = new System.Drawing.Font(this.FontName, 12f, fontStyleArray[index]);
        return fontStyleArray[index];
      }
      catch (Exception ex)
      {
      }
    }
    return FontStyle.Regular;
  }

  internal event ValueChangedEventHandler IndexChanged;

  public override bool Equals(object obj)
  {
    return obj is FontImpl fontImpl && this.GetHashCode() == fontImpl.GetHashCode() && fontImpl.m_font.Equals((object) this.m_font) && (int) this.m_btCharSet == (int) fontImpl.m_btCharSet && this.m_color == fontImpl.m_color;
  }

  public override int GetHashCode() => this.m_font.GetHashCode();

  public int CompareTo(object obj)
  {
    int num = obj is FontImpl fontImpl ? this.m_font.CompareTo(fontImpl.m_font) : throw new ArgumentNullException("font");
    if (num == 0)
      num = (int) this.m_btCharSet - (int) fontImpl.m_btCharSet;
    if (num == 0)
      num = this.m_color == fontImpl.m_color ? 0 : 1;
    return num;
  }

  int IInternalFont.Index => this.Index;

  public FontImpl Font => this;

  internal string ActualFontName
  {
    get => this.m_actualFont;
    set => this.m_actualFont = value;
  }

  public void BeginUpdate()
  {
  }

  public void EndUpdate()
  {
  }

  object ICloneParent.Clone(object parent) => (object) this.Clone(parent);

  void IDisposable.Dispose() => GC.SuppressFinalize((object) this);

  internal void Clear()
  {
    this.IndexChanged = (ValueChangedEventHandler) null;
    if (this.m_fontNative != null)
      this.m_fontNative.Dispose();
    if (this.m_color != (ChartColor) null)
      this.m_color.Dispose();
    this.m_font = (FontRecord) null;
    this.m_color = (ChartColor) null;
    this.m_fontNative = (System.Drawing.Font) null;
    if (this._preservedElements != null)
    {
      foreach (KeyValuePair<string, Stream> preservedElement in this._preservedElements)
        preservedElement.Value.Dispose();
      this._preservedElements.Clear();
      this._preservedElements = (Dictionary<string, Stream>) null;
    }
    this.Dispose();
  }
}
