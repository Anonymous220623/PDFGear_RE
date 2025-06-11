// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.StyleImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.XmlReaders;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class StyleImpl : 
  ExtendedFormatWrapper,
  IStyle,
  IExtendedFormat,
  IParentApplication,
  IOptimizedUpdate,
  IComparable,
  INamedObject
{
  private const int Excel2007StylesStart = 10;
  public const int DEF_LESS = -1;
  public const int DEF_EQUAL = 0;
  public const int DEF_LARGER = 1;
  private const int RowLevelStyleIndex = 1;
  private const int ColumnLevelStyleIndex = 2;
  private StyleRecord m_style;
  private StyleExtRecord m_styleExt;
  private bool m_bNotCompareName;

  public new bool HasBorder => throw new ArgumentException("No need to implement");

  public StyleImpl(WorkbookImpl book)
    : base(book)
  {
    this.m_style = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    this.SetFormatIndex((int) this.m_style.ExtendedFormatIndex);
  }

  [CLSCompliant(false)]
  public StyleImpl(WorkbookImpl book, StyleRecord style)
    : base(book)
  {
    this.m_style = style;
    this.SetFormatIndex((int) this.m_style.ExtendedFormatIndex);
    if (!style.IsBuildInStyle || style.BuildInOrNameLen != (byte) 0)
      return;
    this.m_font.IsDirectAccess = true;
  }

  public StyleImpl(WorkbookImpl book, string strName)
    : this(book, strName, (StyleImpl) null)
  {
  }

  public StyleImpl(WorkbookImpl book, string strName, StyleImpl baseStyle)
    : this(book, strName, baseStyle, false)
  {
  }

  public StyleImpl(WorkbookImpl book, string strName, StyleImpl baseStyle, bool bIsBuiltIn)
    : this(book)
  {
    this.m_style = baseStyle == null ? (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style) : baseStyle.m_style.Clone() as StyleRecord;
    int index = -1;
    if (bIsBuiltIn)
    {
      index = Array.IndexOf<string>(this.DefaultStyleNames, strName);
      this.m_style.BuildInOrNameLen = index >= 0 ? (byte) index : throw new ArgumentOutOfRangeException("name", "Can't find built in name");
    }
    else
      this.m_style.StyleName = strName;
    this.m_style.IsBuildInStyle = bIsBuiltIn;
    ExtendedFormatImpl extendedFormatImpl = baseStyle != null ? (ExtendedFormatImpl) this.m_book.CreateExtFormat((IExtendedFormat) baseStyle.Wrapped, true) : (ExtendedFormatImpl) this.m_book.CreateExtFormat(true);
    extendedFormatImpl.ParentIndex = this.m_book.MaxXFCount;
    extendedFormatImpl.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
    this.m_style.ExtendedFormatIndex = (ushort) extendedFormatImpl.Index;
    this.m_style.IsBuildInStyle = bIsBuiltIn;
    this.SetFormatIndex(extendedFormatImpl.Index);
    if (!bIsBuiltIn || this.m_book.Version == OfficeVersion.Excel97to2003 || index < 10)
      return;
    this.CopyDefaultStyleSettings(index);
  }

  private void CopyDefaultStyleSettings(int index)
  {
    StyleImpl.StyleSettings styleSettings = (this.Application as ApplicationImpl).BuiltInStyleInfo[index];
    FillImpl fill = styleSettings.Fill;
    if (fill != null)
      Excel2007Parser.CopyFillSettings(fill, this.m_xFormat);
    StyleImpl.FontSettings font = styleSettings.Font;
    if (font != null)
      this.CopyFontSettings(font, this.m_font);
    StyleImpl.BorderSettings borders = styleSettings.Borders;
    if (borders == null)
      return;
    this.CopyBordersSettings(borders, this.m_xFormat);
  }

  private void CopyBordersSettings(StyleImpl.BorderSettings borders, ExtendedFormatImpl m_xFormat)
  {
    ChartColor borderColor = borders.BorderColor;
    if (borders.Left != OfficeLineStyle.None)
    {
      m_xFormat.LeftBorderLineStyle = borders.Left;
      if (borderColor != (ChartColor) null)
        m_xFormat.LeftBorderColor.CopyFrom(borderColor, true);
    }
    if (borders.Right != OfficeLineStyle.None)
    {
      m_xFormat.RightBorderLineStyle = borders.Right;
      if (borderColor != (ChartColor) null)
        m_xFormat.RightBorderColor.CopyFrom(borderColor, true);
    }
    if (borders.Top != OfficeLineStyle.None)
    {
      m_xFormat.TopBorderLineStyle = borders.Top;
      if (borderColor != (ChartColor) null)
        m_xFormat.TopBorderColor.CopyFrom(borderColor, true);
    }
    if (borders.Bottom == OfficeLineStyle.None)
      return;
    m_xFormat.BottomBorderLineStyle = borders.Bottom;
    if (!(borderColor != (ChartColor) null))
      return;
    m_xFormat.BottomBorderColor.CopyFrom(borderColor, true);
  }

  private void CopyFontSettings(StyleImpl.FontSettings font, FontWrapper m_font)
  {
    ChartColor color = font.Color;
    m_font.BeginUpdate();
    if (color != (ChartColor) null)
      m_font.ColorObject.CopyFrom(color, true);
    m_font.Size = (double) font.Size;
    m_font.Italic = font.Italic;
    m_font.Bold = font.Bold;
    string name = font.Name;
    if (name != null)
      m_font.FontName = name;
    m_font.EndUpdate();
  }

  private string[] DefaultStyleNames => this.m_book.AppImplementation.DefaultStyleNames;

  public new bool BuiltIn => this.m_style.IsBuildInStyle;

  public new string Name
  {
    get
    {
      string name = (string) null;
      bool flag = !this.BuiltIn;
      if (!flag)
      {
        int buildInOrNameLen = (int) this.m_style.BuildInOrNameLen;
        name = this.DefaultStyleNames[buildInOrNameLen];
        if (buildInOrNameLen == 1 || buildInOrNameLen == 2)
          name += ((int) this.m_style.OutlineStyleLevel + 1).ToString();
        flag = name == null || name.Length == 0;
      }
      if (flag && this.m_style.StyleName != null)
        name = this.m_style.StyleName;
      return name;
    }
  }

  internal string StyleNameCache => this.Record.StyleNameCache;

  public new bool IsInitialized
  {
    get
    {
      string defaultStyleName = this.DefaultStyleNames[0];
      return !(this.Name == defaultStyleName) && !StylesCollection.CompareStyles((IStyle) this, this.m_book.Styles[defaultStyleName]);
    }
  }

  public int Index => this.m_xFormat.Index;

  public bool NotCompareNames
  {
    get => this.m_bNotCompareName;
    set => this.m_bNotCompareName = value;
  }

  [CLSCompliant(false)]
  public StyleRecord Record
  {
    get
    {
      this.UpdateStyleRecord();
      return this.m_style;
    }
  }

  public bool IsBuiltInCustomized
  {
    get => this.Record.IsBuiltIncustomized;
    set => this.Record.IsBuiltIncustomized = value;
  }

  internal bool IsAsciiConverted
  {
    get => this.Record.IsAsciiConverted;
    set => this.Record.IsAsciiConverted = value;
  }

  internal StyleExtRecord StyleExt
  {
    get => this.m_styleExt;
    set => this.m_styleExt = value;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_style);
  }

  public void UpdateStyleRecord()
  {
    this.m_style.ExtendedFormatIndex = (ushort) this.m_xFormat.Index;
  }

  public override void EndUpdate()
  {
    base.EndUpdate();
    if (this.BeginCallsCount == 0 && this.AfterChange != null)
      this.AfterChange((object) this, EventArgs.Empty);
    List<int> childXfs = this.FindChildXFs();
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int index1 = 0;
    for (int count = childXfs.Count; index1 < count; ++index1)
    {
      int index2 = childXfs[index1];
      innerExtFormats[index2].SynchronizeWithParent();
    }
  }

  public override void BeginUpdate()
  {
    if (this.BeginCallsCount == 0 && this.BeforeChange != null)
      this.BeforeChange((object) this, EventArgs.Empty);
    base.BeginUpdate();
  }

  public override object Clone(object parent)
  {
    StyleImpl styleImpl = (StyleImpl) base.Clone(parent);
    styleImpl.m_style = (StyleRecord) CloneUtils.CloneCloneable((ICloneable) this.m_style);
    return (object) styleImpl;
  }

  private List<int> FindChildXFs()
  {
    List<int> childXfs = new List<int>();
    ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
    int index1 = this.Index;
    int index2 = 0;
    for (int count = innerExtFormats.Count; index2 < count; ++index2)
    {
      if (innerExtFormats[index2].ParentIndex == index1)
        childXfs.Add(index2);
    }
    return childXfs;
  }

  public event EventHandler BeforeChange;

  public event EventHandler AfterChange;

  public int CompareTo(object obj)
  {
    if (!(obj is StyleImpl styleImpl))
      return 1;
    int num = this.m_font.Wrapped.CompareTo((object) styleImpl.m_font.Wrapped);
    if (num != 0)
      return num;
    int withoutIndex = this.m_xFormat.CompareToWithoutIndex(styleImpl.m_xFormat);
    if (withoutIndex != 0 || this.m_bNotCompareName || styleImpl.m_bNotCompareName)
      return withoutIndex;
    withoutIndex = this.Name.CompareTo(styleImpl.Name);
    return withoutIndex != 0 ? withoutIndex : withoutIndex;
  }

  internal new void Dispose()
  {
    this.AfterChange = (EventHandler) null;
    this.BeforeChange = (EventHandler) null;
    this.m_style = (StyleRecord) null;
    base.Dispose();
  }

  internal class StyleSettings
  {
    public FillImpl Fill;
    public StyleImpl.FontSettings Font;
    public StyleImpl.BorderSettings Borders;

    public StyleSettings(FillImpl fill, StyleImpl.FontSettings font)
      : this(fill, font, (StyleImpl.BorderSettings) null)
    {
    }

    public StyleSettings(
      FillImpl fill,
      StyleImpl.FontSettings font,
      StyleImpl.BorderSettings borders)
    {
      this.Fill = fill;
      this.Font = font;
      this.Borders = borders;
    }

    internal void Clear()
    {
      if (this.Fill != null)
        this.Fill.Dispose();
      if (this.Font != null)
        this.Font.Dispose();
      if (this.Borders != null)
        this.Borders.Dispose();
      this.Fill = (FillImpl) null;
      this.Font = (StyleImpl.FontSettings) null;
      this.Borders = (StyleImpl.BorderSettings) null;
    }
  }

  internal class FontSettings
  {
    public ChartColor Color;
    public int Size;
    public bool Bold;
    public bool Italic;
    public string Name;

    public FontSettings(ChartColor color)
      : this(color, 11)
    {
    }

    public FontSettings(ChartColor color, int size)
      : this(color, size, FontStyle.Regular)
    {
    }

    public FontSettings(ChartColor color, FontStyle fontStyle)
      : this(color, 11, fontStyle)
    {
    }

    public FontSettings(ChartColor color, int size, FontStyle fontStyle)
      : this(color, size, fontStyle, (string) null)
    {
    }

    public FontSettings(ChartColor color, int size, FontStyle fontStyle, string name)
    {
      this.Color = color;
      this.Size = size;
      this.Bold = (fontStyle & FontStyle.Bold) != FontStyle.Regular;
      this.Italic = (fontStyle & FontStyle.Italic) != FontStyle.Regular;
      this.Name = name;
    }

    internal void Dispose()
    {
      this.Color.Dispose();
      this.Color = (ChartColor) null;
    }
  }

  internal class BorderSettings
  {
    public ChartColor BorderColor;
    public OfficeLineStyle Left;
    public OfficeLineStyle Right;
    public OfficeLineStyle Top;
    public OfficeLineStyle Bottom;

    public BorderSettings(ChartColor color, OfficeLineStyle lineStyle)
    {
      this.BorderColor = color;
      this.Left = this.Right = this.Top = this.Bottom = lineStyle;
    }

    public BorderSettings(
      ChartColor color,
      OfficeLineStyle left,
      OfficeLineStyle right,
      OfficeLineStyle top,
      OfficeLineStyle bottom)
    {
      this.BorderColor = color;
      this.Left = left;
      this.Right = right;
      this.Top = top;
      this.Bottom = bottom;
    }

    internal void Dispose()
    {
      this.BorderColor.Dispose();
      this.BorderColor = (ChartColor) null;
    }
  }

  [Flags]
  private enum StyleOptions
  {
    None = 0,
    UpdateStyleXF = 1,
    Temporary = 2,
  }
}
