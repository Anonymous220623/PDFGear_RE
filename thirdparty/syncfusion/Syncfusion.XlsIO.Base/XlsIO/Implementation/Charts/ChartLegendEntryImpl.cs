// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartLegendEntryImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartLegendEntryImpl : CommonObject, IChartLegendEntry
{
  private ChartLegendxnRecord m_legendXN;
  private ChartTextAreaImpl m_text;
  private ChartLegendEntriesColl m_legendEnties;
  private int m_index;

  public ChartLegendEntryImpl(IApplication application, object parent, int iIndex)
    : base(application, parent)
  {
    this.m_legendXN = (ChartLegendxnRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartLegendxn);
    this.m_text = new ChartTextAreaImpl(application, (object) this);
    this.m_index = iIndex;
    this.SetParents();
  }

  public ChartLegendEntryImpl(
    IApplication application,
    object parent,
    int iIndex,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    this.m_index = iIndex;
    this.SetParents();
    this.Parse(data, ref iPos);
  }

  public void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartLegendxn);
    this.m_legendXN = (ChartLegendxnRecord) data[iPos];
    ++iPos;
    if (data[iPos].TypeCode != TBIFFRecord.Begin)
      return;
    ++iPos;
    int num = 1;
    while (num != 0)
    {
      switch (data[iPos].TypeCode)
      {
        case TBIFFRecord.ChartText:
          this.m_text = new ChartTextAreaImpl(this.Application, (object) this);
          iPos = this.m_text.Parse(data, iPos) - 1;
          break;
        case TBIFFRecord.Begin:
          iPos = BiffRecordRaw.SkipBeginEndBlock(data, iPos);
          break;
        case TBIFFRecord.End:
          --num;
          break;
      }
      ++iPos;
    }
  }

  public void SetParents()
  {
    this.m_legendEnties = (ChartLegendEntriesColl) this.FindParent(typeof (ChartLegendEntriesColl));
    if (this.m_legendEnties == null)
      throw new ArgumentNullException("cannot find parent object");
  }

  [CLSCompliant(false)]
  public void Serialize(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentException(nameof (records));
    if (!this.IsFormatted && !this.IsDeleted)
      return;
    records.Add((IBiffStorage) this.m_legendXN.Clone());
    if (this.m_text == null)
      return;
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    this.m_text.Serialize(records, true);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  public bool IsDeleted
  {
    get => this.m_legendXN.IsDeleted;
    set
    {
      if (this.IsDeleted == value)
        return;
      if (value)
        this.IsFormatted = !value;
      this.m_legendXN.IsDeleted = value;
    }
  }

  public bool IsFormatted
  {
    get => this.m_legendXN.IsFormatted;
    set
    {
      if (value == this.IsFormatted)
        return;
      if (value)
      {
        this.m_text = new ChartTextAreaImpl(this.Application, (object) this);
        this.m_legendXN.IsDeleted = false;
      }
      this.m_legendXN.IsFormatted = value;
    }
  }

  public IChartTextArea TextArea
  {
    get
    {
      if (this.FindParent(typeof (WorkbookImpl)) is WorkbookImpl parent && !parent.Loading)
        this.m_legendXN.IsDeleted = false;
      this.m_legendXN.IsFormatted = true;
      return (IChartTextArea) this.m_text;
    }
  }

  public int LegendEntityIndex
  {
    get => (int) this.m_legendXN.LegendEntityIndex;
    set => this.m_legendXN.LegendEntityIndex = (ushort) value;
  }

  public int Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  public void Clear() => this.IsFormatted = false;

  public void Delete() => this.IsDeleted = true;

  public ChartLegendEntryImpl Clone(
    object parent,
    Dictionary<int, int> dicIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartLegendEntryImpl parent1 = (ChartLegendEntryImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    if (this.m_text != null)
      parent1.m_text = (ChartTextAreaImpl) this.m_text.Clone((object) parent1, dicIndexes, dicNewSheetNames);
    parent1.m_legendXN = (ChartLegendxnRecord) CloneUtils.CloneCloneable((ICloneable) this.m_legendXN);
    return parent1;
  }
}
