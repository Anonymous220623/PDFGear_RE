// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.HPageBreaksCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class HPageBreaksCollection : 
  CollectionBaseEx<IHPageBreak>,
  IHPageBreaks,
  IEnumerable,
  IBiffStorage
{
  private WorkbookImpl m_book;

  public IHPageBreak this[IRange location]
  {
    get
    {
      int pageBreakIndex = this.GetPageBreakIndex(location);
      return pageBreakIndex < 0 ? (IHPageBreak) null : this.List[pageBreakIndex];
    }
  }

  public IHPageBreak Add(IRange location)
  {
    if (location == null)
      throw new ArgumentNullException(nameof (location));
    HPageBreakImpl pageBreak = ((RangeImpl) location).IsSingleCell ? new HPageBreakImpl(this.Application, (object) this, location) : throw new ArgumentException("Location should be single cell.");
    this.Add(pageBreak);
    return (IHPageBreak) pageBreak;
  }

  public IHPageBreak Remove(IRange location)
  {
    IHPageBreak hpageBreak = (IHPageBreak) null;
    int pageBreakIndex = this.GetPageBreakIndex(location);
    if (pageBreakIndex >= 0)
    {
      hpageBreak = this.List[pageBreakIndex];
      this.RemoveAt(pageBreakIndex);
    }
    return hpageBreak;
  }

  public IHPageBreak GetPageBreak(int iRow)
  {
    IHPageBreak pageBreak = (IHPageBreak) null;
    int i = 0;
    for (int count = this.Count; i < count; ++i)
    {
      IHPageBreak hpageBreak = this[i];
      if (hpageBreak.Location.Row == iRow)
      {
        pageBreak = hpageBreak;
        break;
      }
    }
    return pageBreak;
  }

  internal int GetPageBreakIndex(IRange location)
  {
    int pageBreakIndex = -1;
    int row = location.Row;
    int column = location.Column;
    int i = 0;
    for (int count = this.Count; i < count; ++i)
    {
      IRange location1 = this[i].Location;
      if (location1.Row == row && location1.Column == column)
      {
        pageBreakIndex = i;
        break;
      }
    }
    return pageBreakIndex;
  }

  internal void InsertRows(int rowIndex, int totalRows)
  {
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      HPageBreakImpl inner = (HPageBreakImpl) this.InnerList[index];
      if (inner.Row >= rowIndex)
        inner.Row += totalRows;
    }
  }

  internal void DeleteRows(int rowIndex, int totalRow)
  {
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      HPageBreakImpl inner = (HPageBreakImpl) this.InnerList[index];
      if (inner.Row >= rowIndex)
      {
        if (rowIndex + totalRow > inner.Row)
        {
          this.InnerList.RemoveAt(index--);
        }
        else
        {
          int num = inner.Row - totalRow;
          if (num < rowIndex)
            num = rowIndex;
          inner.Row = num;
        }
      }
    }
  }

  public new void Clear() => base.Clear();

  public int ManualBreakCount
  {
    get
    {
      int manualBreakCount = 0;
      foreach (HPageBreakImpl hpageBreakImpl in (IEnumerable<IHPageBreak>) this.List)
      {
        if (hpageBreakImpl.Type == ExcelPageBreak.PageBreakManual)
          ++manualBreakCount;
      }
      return manualBreakCount;
    }
  }

  public HPageBreaksCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
  }

  private void FindParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentOutOfRangeException("parent");
  }

  [CLSCompliant(false)]
  public void Parse(HorizontalPageBreaksRecord record)
  {
    HorizontalPageBreaksRecord.THPageBreak[] thPageBreakArray = record != null ? record.PageBreaks : throw new ArgumentNullException(nameof (record));
    int index = 0;
    for (int length = thPageBreakArray.Length; index < length; ++index)
    {
      HorizontalPageBreaksRecord.THPageBreak pagebreak = thPageBreakArray[index];
      if ((int) pagebreak.EndColumn > this.m_book.MaxColumnCount - 1)
        pagebreak.EndColumn = (ushort) (this.m_book.MaxColumnCount - 1);
      if ((int) pagebreak.StartColumn < this.m_book.MaxColumnCount)
        this.Add((IHPageBreak) new HPageBreakImpl(this.Application, (object) this, pagebreak));
    }
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    HorizontalPageBreaksRecord pageBreaksRecord = this.PrepareRecord();
    if (pageBreaksRecord == null)
      return;
    records.Add((IBiffStorage) pageBreaksRecord);
  }

  private HorizontalPageBreaksRecord PrepareRecord()
  {
    int count = this.Count;
    if (count == 0)
      return (HorizontalPageBreaksRecord) null;
    HorizontalPageBreaksRecord.THPageBreak[] thPageBreakArray = new HorizontalPageBreaksRecord.THPageBreak[count];
    System.Collections.Generic.List<IHPageBreak> innerList = this.InnerList;
    for (int index = 0; index < count; ++index)
    {
      HPageBreakImpl hpageBreakImpl = innerList[index] as HPageBreakImpl;
      thPageBreakArray[index] = hpageBreakImpl.HPageBreak;
    }
    HorizontalPageBreaksRecord record = (HorizontalPageBreaksRecord) BiffRecordFactory.GetRecord(TBIFFRecord.HorizontalPageBreaks);
    record.PageBreaks = thPageBreakArray;
    return record;
  }

  public override object Clone(object parent)
  {
    HPageBreaksCollection breaksCollection = (HPageBreaksCollection) base.Clone(parent);
    breaksCollection.FindParents();
    return (object) breaksCollection;
  }

  public void Add(HPageBreakImpl pageBreak)
  {
    if (pageBreak == null)
      throw new ArgumentNullException(nameof (pageBreak));
    if (this.GetPageBreakIndex(pageBreak.Location) >= 0)
      return;
    this.Add((IHPageBreak) pageBreak);
  }

  public void ChangeToExcel97to03Version()
  {
    System.Collections.Generic.List<HPageBreakImpl> hpageBreakImplList = new System.Collections.Generic.List<HPageBreakImpl>();
    foreach (HPageBreakImpl hpageBreakImpl in (IEnumerable<IHPageBreak>) this.List)
    {
      WorkbookImpl workbook = (WorkbookImpl) ((PageSetupImpl) this.Parent).Worksheet.Workbook;
      HorizontalPageBreaksRecord.THPageBreak hpageBreak = hpageBreakImpl.HPageBreak;
      if ((int) hpageBreak.Row > workbook.MaxRowCount)
      {
        hpageBreakImplList.Add(hpageBreakImpl);
      }
      else
      {
        if ((int) hpageBreak.StartColumn > workbook.MaxColumnCount - 1)
          hpageBreak.StartColumn = (ushort) (workbook.MaxColumnCount - 1);
        if ((int) hpageBreak.EndColumn > workbook.MaxColumnCount - 1)
          hpageBreak.EndColumn = (ushort) (workbook.MaxColumnCount - 1);
      }
    }
    foreach (IHPageBreak hpageBreak in hpageBreakImplList)
      this.Remove(hpageBreak);
  }

  public TBIFFRecord TypeCode => TBIFFRecord.Unknown;

  public int RecordCode => 0;

  public bool NeedDataArray => false;

  public long StreamPos
  {
    get => -1;
    set
    {
    }
  }

  public int GetStoreSize(ExcelVersion version)
  {
    int count = this.Count;
    return count <= 0 ? -4 : 6 * count + 2;
  }

  public int FillStream(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition)
  {
    HorizontalPageBreaksRecord pageBreaksRecord = this.PrepareRecord();
    return pageBreaksRecord == null ? 0 : pageBreaksRecord.FillStream(writer, provider, encryptor, streamPosition);
  }
}
