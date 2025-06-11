// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.FontsCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class FontsCollection : CollectionBaseEx<FontImpl>
{
  private WorkbookImpl m_book;
  private Dictionary<FontImpl, FontImpl> m_hashFonts = new Dictionary<FontImpl, FontImpl>();

  public FontsCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public IOfficeFont this[int index]
  {
    get
    {
      if (index < 0 || index >= this.InnerList.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is out of range.");
      return (IOfficeFont) this.InnerList[index];
    }
  }

  [CLSCompliant(false)]
  public FontImpl Add(FontImpl font, FontRecord record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    return (FontImpl) this.Add((IOfficeFont) this.AppImplementation.CreateFont((object) this, font));
  }

  public IOfficeFont Add(IOfficeFont font)
  {
    if (!(font is FontImpl fontImpl))
      throw new ArgumentException("Can't add font.");
    if (fontImpl.HasParagrapAlign)
    {
      this.ForceAdd(fontImpl);
      return (IOfficeFont) fontImpl;
    }
    if (this.m_hashFonts.ContainsKey(fontImpl))
      return (IOfficeFont) this.m_hashFonts[fontImpl];
    this.ForceAdd(fontImpl);
    return (IOfficeFont) fontImpl;
  }

  public void InsertDefaultFonts()
  {
    FontRecord record = (FontRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Font);
    record.FontName = this.AppImplementation.StandardFont;
    record.FontHeight = (ushort) FontImpl.SizeInTwips(this.AppImplementation.StandardFontSize);
    this.ForceAdd(this.AppImplementation.CreateFont((object) this, record));
    FontRecord font1 = (FontRecord) record.Clone();
    this.ForceAdd(this.AppImplementation.CreateFont((object) this, font1));
    FontRecord font2 = (FontRecord) font1.Clone();
    this.ForceAdd(this.AppImplementation.CreateFont((object) this, font2));
    this.ForceAdd(this.AppImplementation.CreateFont((object) this, (FontRecord) font2.Clone()));
    this.InnerList.Add(this.InnerList[0]);
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }

  public void ForceAdd(FontImpl font)
  {
    if (this.InnerList.Count == 4)
      this.InnerList.Add(this.InnerList[0]);
    font.Index = this.InnerList.Count;
    this.InnerList.Add(font);
    if (this.m_hashFonts.ContainsKey(font))
      return;
    this.m_hashFonts.Add(font, font);
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    int index = 0;
    for (int count = this.InnerList.Count; index < count; ++index)
    {
      if (index != 4)
        this.InnerList[index].Serialize(records);
    }
  }

  public new bool Contains(FontImpl font) => this.m_hashFonts.ContainsKey(font);

  public Dictionary<int, int> AddRange(FontsCollection arrFonts)
  {
    if (arrFonts == null)
      throw new ArgumentNullException(nameof (arrFonts));
    if (arrFonts == this)
      return (Dictionary<int, int>) null;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    int num1 = 0;
    for (int count = arrFonts.Count; num1 < count; ++num1)
    {
      if (num1 == 4)
      {
        dictionary.Add(num1, num1);
      }
      else
      {
        int num2 = this.AddCopy(arrFonts[num1] as FontImpl);
        dictionary.Add(num1, num2);
      }
    }
    return dictionary;
  }

  public Dictionary<int, int> AddRange(ICollection<int> colFonts, FontsCollection sourceFonts)
  {
    if (colFonts == null)
      throw new ArgumentNullException(nameof (colFonts));
    if (sourceFonts == null)
      throw new ArgumentNullException(nameof (sourceFonts));
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    foreach (int colFont in (IEnumerable<int>) colFonts)
    {
      FontImpl sourceFont = (FontImpl) sourceFonts[colFont];
      int index = sourceFont.Index;
      int num = this.AddCopy(sourceFont);
      dictionary.Add(index, num);
    }
    return dictionary;
  }

  private int AddCopy(FontImpl font)
  {
    font = font != null ? this.Add(font, font.Record) : throw new ArgumentNullException(nameof (font));
    return font.Index;
  }

  protected override void OnClearComplete() => this.m_hashFonts.Clear();

  public FontsCollection Clone(WorkbookImpl parent)
  {
    FontsCollection parent1 = parent != null ? new FontsCollection(this.Application, (object) parent) : throw new ArgumentNullException(nameof (parent));
    System.Collections.Generic.List<FontImpl> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      FontImpl font = innerList[index].Clone((object) parent1);
      if (index != 4)
        parent1.ForceAdd(font);
    }
    return parent1;
  }

  internal void Dispose()
  {
    foreach (FontImpl inner in this.InnerList)
      inner.Clear();
    this.InnerList.Clear();
  }
}
