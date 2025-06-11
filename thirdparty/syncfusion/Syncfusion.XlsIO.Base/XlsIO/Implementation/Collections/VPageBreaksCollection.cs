// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.VPageBreaksCollection
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

public class VPageBreaksCollection : 
  CollectionBaseEx<IVPageBreak>,
  IVPageBreaks,
  IEnumerable,
  IBiffStorage
{
  private WorkbookImpl m_book;

  public IVPageBreak this[IRange location] => this.GetPageBreak(location.Column);

  public IVPageBreak Add(IRange location)
  {
    if (location == null)
      throw new ArgumentNullException(nameof (location));
    IVPageBreak vpageBreak = ((RangeImpl) location).IsSingleCell ? (IVPageBreak) new VPageBreakImpl(this.Application, (object) this, location) : throw new ArgumentException("Location should be single cell.");
    this.Add(vpageBreak);
    return vpageBreak;
  }

  public IVPageBreak Remove(IRange location)
  {
    int pageBreakIndex = this.GetPageBreakIndex(location);
    IVPageBreak vpageBreak = (IVPageBreak) null;
    if (pageBreakIndex >= 0)
    {
      vpageBreak = this.List[pageBreakIndex];
      this.RemoveAt(pageBreakIndex);
    }
    return vpageBreak;
  }

  public IVPageBreak GetPageBreak(int iColumn)
  {
    IVPageBreak pageBreak = (IVPageBreak) null;
    int i = 0;
    for (int count = this.Count; i < count; ++i)
    {
      IVPageBreak vpageBreak = this[i];
      if (vpageBreak.Location.Column == iColumn)
      {
        pageBreak = vpageBreak;
        break;
      }
    }
    return pageBreak;
  }

  private int GetPageBreakIndex(IRange location)
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

  public new void Clear() => base.Clear();

  public int ManualBreakCount
  {
    get
    {
      int manualBreakCount = 0;
      foreach (VPageBreakImpl vpageBreakImpl in (IEnumerable<IVPageBreak>) this.List)
      {
        if (vpageBreakImpl.Type == ExcelPageBreak.PageBreakManual)
          ++manualBreakCount;
      }
      return manualBreakCount;
    }
  }

  internal void InsertColumns(int columnIndex, int iColumnCount)
  {
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      VPageBreakImpl inner = (VPageBreakImpl) this.InnerList[index];
      if (inner.Column >= columnIndex)
        inner.Column += iColumnCount;
    }
  }

  internal void DeleteColumns(int columnIndex, int columnCount)
  {
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      VPageBreakImpl inner = (VPageBreakImpl) this.InnerList[index];
      if (inner.Column >= columnIndex)
      {
        if (columnIndex + columnCount > inner.Column)
        {
          this.InnerList.RemoveAt(index--);
        }
        else
        {
          int num = inner.Column - columnCount;
          if (num < columnIndex)
            num = columnIndex;
          inner.Column = num;
        }
      }
    }
  }

  public VPageBreaksCollection(IApplication application, object parent)
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
  public void Parse(VerticalPageBreaksRecord record)
  {
    VerticalPageBreaksRecord.TVPageBreak[] tvPageBreakArray = record != null ? record.PageBreaks : throw new ArgumentNullException(nameof (record));
    int index = 0;
    for (int length = tvPageBreakArray.Length; index < length; ++index)
      this.Add((IVPageBreak) new VPageBreakImpl(this.Application, (object) this, tvPageBreakArray[index]));
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    VerticalPageBreaksRecord pageBreaksRecord = this.PrepareRecord();
    if (pageBreaksRecord == null)
      return;
    records.Add((IBiffStorage) pageBreaksRecord);
  }

  private VerticalPageBreaksRecord PrepareRecord()
  {
    int count = this.Count;
    if (count == 0)
      return (VerticalPageBreaksRecord) null;
    VerticalPageBreaksRecord.TVPageBreak[] tvPageBreakArray = new VerticalPageBreaksRecord.TVPageBreak[count];
    System.Collections.Generic.List<IVPageBreak> innerList = this.InnerList;
    for (int index = 0; index < count; ++index)
    {
      VPageBreakImpl vpageBreakImpl = innerList[index] as VPageBreakImpl;
      tvPageBreakArray[index] = vpageBreakImpl.VPageBreak;
    }
    VerticalPageBreaksRecord record = (VerticalPageBreaksRecord) BiffRecordFactory.GetRecord(TBIFFRecord.VerticalPageBreaks);
    record.PageBreaks = tvPageBreakArray;
    return record;
  }

  public override object Clone(object parent)
  {
    VPageBreaksCollection breaksCollection = (VPageBreaksCollection) base.Clone(parent);
    breaksCollection.FindParents();
    return (object) breaksCollection;
  }

  public void Add(VPageBreakImpl pageBreak)
  {
    if (pageBreak == null)
      throw new ArgumentNullException(nameof (pageBreak));
    if (this.GetPageBreakIndex(pageBreak.Location) >= 0)
      return;
    this.Add((IVPageBreak) pageBreak);
  }

  public void ChangeToExcel97to03Version()
  {
    System.Collections.Generic.List<VPageBreakImpl> vpageBreakImplList = new System.Collections.Generic.List<VPageBreakImpl>();
    foreach (VPageBreakImpl vpageBreakImpl in (IEnumerable<IVPageBreak>) this.List)
    {
      WorkbookImpl workbook = (WorkbookImpl) ((PageSetupImpl) this.Parent).Worksheet.Workbook;
      VerticalPageBreaksRecord.TVPageBreak vpageBreak = vpageBreakImpl.VPageBreak;
      if ((int) vpageBreak.Column > workbook.MaxColumnCount)
      {
        vpageBreakImplList.Add(vpageBreakImpl);
      }
      else
      {
        if ((long) vpageBreak.StartRow > (long) (workbook.MaxRowCount - 1))
          vpageBreak.StartRow = (uint) (ushort) (workbook.MaxRowCount - 1);
        if ((long) vpageBreak.EndRow > (long) (workbook.MaxRowCount - 1))
          vpageBreak.EndRow = (uint) (ushort) (workbook.MaxRowCount - 1);
      }
    }
    foreach (IVPageBreak vpageBreak in vpageBreakImplList)
      this.Remove(vpageBreak);
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
    VerticalPageBreaksRecord pageBreaksRecord = this.PrepareRecord();
    return pageBreaksRecord == null ? 0 : pageBreaksRecord.FillStream(writer, provider, encryptor, streamPosition);
  }
}
