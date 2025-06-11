// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.DataValidationCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class DataValidationCollection : CollectionBaseEx<DataValidationImpl>
{
  private DValRecord m_dvalRecord;
  private DataValidationTable m_parentTable;
  private Dictionary<DVRecord, DataValidationImpl> m_hashRecords = new Dictionary<DVRecord, DataValidationImpl>();
  private System.Collections.Generic.List<BiffRecordRaw> m_arrStorage;
  private bool m_bIsDelay;

  public DataValidationCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  [CLSCompliant(false)]
  public DataValidationCollection(IApplication application, object parent, DValRecord dval)
    : this(application, parent)
  {
    this.m_dvalRecord = (DValRecord) dval.Clone();
  }

  public DataValidationCollection(
    IApplication application,
    object parent,
    System.Collections.Generic.List<BiffRecordRaw> arrRecords,
    ref int iOffset)
    : this(application, parent)
  {
    this.Parse(arrRecords, ref iOffset, false);
  }

  private void SetParents()
  {
    this.m_parentTable = this.FindParent(typeof (DataValidationTable)) as DataValidationTable;
    if (this.m_parentTable == null)
      throw new ArgumentNullException("Can't find parent table.");
  }

  public DataValidationImpl Add(DataValidationImpl dv)
  {
    if (this.m_bIsDelay)
    {
      int iOffset = 0;
      this.UpdateRecords(this.m_arrStorage, ref iOffset, this.m_arrStorage.Count);
    }
    DVRecord dvRecord = dv.DVRecord;
    if (this.m_hashRecords.ContainsKey(dvRecord))
    {
      DataValidationImpl hashRecord = this.m_hashRecords[dvRecord];
      if (hashRecord != dv)
        hashRecord.AddRange(dv);
      return hashRecord;
    }
    this.m_hashRecords.Add(dvRecord, dv);
    base.Add(dv);
    return dv;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_bIsDelay)
    {
      if ((long) this.m_arrStorage.Count != (long) this.m_dvalRecord.DVNumber)
        throw new ApplicationException("Cannot find data validation entries.");
      records.Add((IBiffStorage) this.m_dvalRecord);
      records.AddList((IList) this.m_arrStorage);
    }
    else
    {
      if (this.Count == 0)
        return;
      this.m_dvalRecord.DVNumber = (uint) this.Count;
      records.Add((IBiffStorage) this.m_dvalRecord);
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        this.List[index].Serialize(records);
    }
  }

  private void Parse(System.Collections.Generic.List<BiffRecordRaw> arrRecords, ref int iOffset, bool bIsParse)
  {
    if (arrRecords == null)
      throw new ArgumentNullException(nameof (arrRecords));
    if (iOffset < 0 || iOffset > arrRecords.Count)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value cannot be less than 0 or greater than arrRecords.Count.");
    this.m_dvalRecord = arrRecords[iOffset] as DValRecord;
    ++iOffset;
    int num = this.m_dvalRecord != null ? (int) this.m_dvalRecord.DVNumber : throw new ArgumentNullException("Can't find DVal record at the specified position.");
    if (bIsParse)
    {
      this.UpdateRecords(arrRecords, ref iOffset, num);
    }
    else
    {
      this.m_arrStorage = new System.Collections.Generic.List<BiffRecordRaw>(num);
      this.m_arrStorage.AddRange((IEnumerable<BiffRecordRaw>) arrRecords.GetRange(iOffset, num));
      this.m_bIsDelay = true;
    }
  }

  public void Remove(DataValidationImpl dv)
  {
    if (this.m_bIsDelay)
    {
      int iOffset = 0;
      this.UpdateRecords(this.m_arrStorage, ref iOffset, this.m_arrStorage.Count);
    }
    if (this.List.IndexOf(dv) < 0)
      return;
    base.Remove(dv);
    this.m_hashRecords.Remove(dv.DVRecord);
  }

  public void Remove(Rectangle[] rectangles)
  {
    if (this.Count <= 0)
      return;
    for (int index = this.Count - 1; index >= 0; --index)
      this[index].RemoveRange(rectangles);
  }

  public override object Clone(object parent)
  {
    DataValidationCollection validationCollection = (DataValidationCollection) base.Clone(parent);
    validationCollection.m_dvalRecord = (DValRecord) CloneUtils.CloneCloneable((ICloneable) this.m_dvalRecord);
    if (this.m_bIsDelay)
    {
      validationCollection.m_bIsDelay = this.m_bIsDelay;
      validationCollection.m_arrStorage = CloneUtils.CloneCloneable(this.m_arrStorage);
    }
    else
    {
      System.Collections.Generic.List<DataValidationImpl> innerList = validationCollection.InnerList;
      int index = 0;
      for (int count = innerList.Count; index < count; ++index)
      {
        DataValidationImpl dataValidationImpl = innerList[index];
        validationCollection.m_hashRecords.Add(dataValidationImpl.DVRecord, dataValidationImpl);
      }
    }
    return (object) validationCollection;
  }

  public DataValidationImpl FindByCellIndex(long cellIndex)
  {
    System.Collections.Generic.List<DataValidationImpl> innerList = this.InnerList;
    if (this.m_bIsDelay)
    {
      int iOffset = 0;
      this.UpdateRecords(this.m_arrStorage, ref iOffset, this.m_arrStorage.Count);
    }
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      DataValidationImpl byCellIndex = innerList[index];
      if (byCellIndex.ContainsCell(cellIndex))
        return byCellIndex;
    }
    return (DataValidationImpl) null;
  }

  public void UpdateNamedRangeIndexes(int[] arrNewIndex)
  {
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    if (this.m_bIsDelay)
    {
      FormulaUtil formulaUtil = this.Workbook.FormulaUtil;
      int index = 0;
      for (int count = this.m_arrStorage.Count; index < count; ++index)
      {
        DVRecord dvRecord = (DVRecord) this.m_arrStorage[index];
        formulaUtil.UpdateNameIndex(dvRecord.FirstFormulaTokens, arrNewIndex);
        formulaUtil.UpdateNameIndex(dvRecord.SecondFormulaTokens, arrNewIndex);
      }
    }
    this.m_hashRecords.Clear();
    int index1 = 0;
    for (int count = this.Count; index1 < count; ++index1)
    {
      DataValidationImpl inner = this.InnerList[index1];
      inner.UpdateNamedRangeIndexes(arrNewIndex);
      if (!this.m_hashRecords.ContainsKey(inner.DVRecord))
        this.m_hashRecords.Add(inner.DVRecord, inner);
    }
  }

  public void UpdateNamedRangeIndexes(IDictionary<int, int> dicNewIndex)
  {
    if (dicNewIndex == null)
      throw new ArgumentNullException(nameof (dicNewIndex));
    if (this.m_bIsDelay)
    {
      FormulaUtil formulaUtil = this.Workbook.FormulaUtil;
      int index = 0;
      for (int count = this.m_arrStorage.Count; index < count; ++index)
      {
        DVRecord dvRecord = (DVRecord) this.m_arrStorage[index];
        formulaUtil.UpdateNameIndex(dvRecord.FirstFormulaTokens, dicNewIndex);
        formulaUtil.UpdateNameIndex(dvRecord.SecondFormulaTokens, dicNewIndex);
      }
    }
    else
    {
      this.m_hashRecords.Clear();
      int index = 0;
      for (int count = this.Count; index < count; ++index)
      {
        DataValidationImpl inner = this.InnerList[index];
        inner.UpdateNamedRangeIndexes(dicNewIndex);
        this.m_hashRecords.Add(inner.DVRecord, inner);
      }
    }
  }

  [CLSCompliant(false)]
  public DataValidationImpl AddDVRecord(DVRecord dv)
  {
    if (dv == null)
      throw new ArgumentNullException(nameof (dv));
    if (this.m_bIsDelay)
    {
      int iOffset = 0;
      this.UpdateRecords(this.m_arrStorage, ref iOffset, this.m_arrStorage.Count);
    }
    return this.AddLocalRecord(dv);
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    System.Collections.Generic.List<DataValidationImpl> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    System.Collections.Generic.List<DataValidationImpl> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  internal void UpdateRecords(System.Collections.Generic.List<BiffRecordRaw> arrRecords, ref int iOffset, int iCount)
  {
    if (arrRecords == null)
      throw new ArgumentNullException(nameof (arrRecords));
    if (arrRecords.Count > iCount + iOffset)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    uint num = 0;
    while ((long) num < (long) iCount)
    {
      if (!(arrRecords[iOffset] is DVRecord arrRecord))
        throw new ArgumentNullException("Not enough DVRecords");
      this.AddLocalRecord(arrRecord);
      ++num;
      ++iOffset;
    }
    this.m_arrStorage = (System.Collections.Generic.List<BiffRecordRaw>) null;
    this.m_bIsDelay = false;
  }

  private DataValidationImpl AddLocalRecord(DVRecord dv)
  {
    if (dv == null)
      throw new ArgumentNullException(nameof (dv));
    if (this.m_hashRecords.ContainsKey(dv))
      return this.m_hashRecords[dv];
    DataValidationImpl dataValidationImpl = this.AppImplementation.CreateDataValidationImpl(this, dv);
    this.m_hashRecords.Add(dv, dataValidationImpl);
    base.Add(dataValidationImpl);
    return dataValidationImpl;
  }

  public System.Collections.Generic.List<BiffRecordRaw> DataValidations => this.m_arrStorage;

  public int PromptBoxHPosition
  {
    get => this.m_dvalRecord.PromtBoxHPos;
    set => this.m_dvalRecord.PromtBoxHPos = value;
  }

  public int PromptBoxVPosition
  {
    get => this.m_dvalRecord.PromtBoxVPos;
    set => this.m_dvalRecord.PromtBoxVPos = value;
  }

  public bool IsPromptBoxVisible
  {
    get => this.m_dvalRecord.IsPromtBoxVisible;
    set => this.m_dvalRecord.IsPromtBoxVisible = value;
  }

  public bool IsPromptBoxPositionFixed
  {
    get => this.m_dvalRecord.IsPromtBoxPosFixed;
    set => this.m_dvalRecord.IsPromtBoxPosFixed = value;
  }

  public WorkbookImpl Workbook => this.m_parentTable.Workbook;

  public WorksheetImpl Worksheet => this.m_parentTable.Worksheet;

  public DataValidationTable ParentTable => this.m_parentTable;

  [CLSCompliant(false)]
  public DValRecord Record => this.m_dvalRecord;

  public new DataValidationImpl this[int index]
  {
    get
    {
      return index >= 0 && index <= this.Count ? this.List[index] : throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 or greater than Count.");
    }
  }

  public int ShapesCount
  {
    get
    {
      int shapesCount = 0;
      if (!this.m_bIsDelay)
      {
        int index = 0;
        for (int count = this.Count; index < count; ++index)
          shapesCount += this[index].ShapesCount;
      }
      else
      {
        int index = 0;
        for (int count = this.m_arrStorage.Count; index < count; ++index)
        {
          if (this.m_arrStorage[index] is DVRecord dvRecord)
            shapesCount += (int) dvRecord.AddrListSize;
        }
      }
      return shapesCount;
    }
  }

  internal Dictionary<DVRecord, DataValidationImpl> HashRecords => this.m_hashRecords;

  internal void AddFrom(
    DataValidationCollection dvCollection,
    int iSourceRow,
    int iSourceColumn,
    int iDestRow,
    int iDestColumn,
    int iRowCount,
    int iColumnCount)
  {
    int iRowDelta = iDestRow - iSourceRow;
    int iColumnDelta = iDestColumn - iSourceColumn;
    this.DelayedParse();
    dvCollection.DelayedParse();
    foreach (KeyValuePair<DVRecord, DataValidationImpl> hashRecord in dvCollection.m_hashRecords)
    {
      DataValidationImpl dv = hashRecord.Value.Clone(this, iSourceRow, iSourceColumn, iRowDelta, iColumnDelta, iRowCount, iColumnCount);
      if (dv.DVRanges.Length > 0)
        this.Add(dv);
    }
  }

  private void DelayedParse()
  {
    if (!this.m_bIsDelay)
      return;
    int iOffset = 0;
    this.UpdateRecords(this.m_arrStorage, ref iOffset, this.m_arrStorage.Count);
  }
}
