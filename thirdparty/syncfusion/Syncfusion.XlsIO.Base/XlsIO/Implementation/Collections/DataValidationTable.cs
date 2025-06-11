// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.DataValidationTable
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class DataValidationTable : CollectionBaseEx<DataValidationCollection>
{
  private WorksheetImpl m_worksheet;
  private Dictionary<DValRecord, DataValidationCollection> m_hashDVals = new Dictionary<DValRecord, DataValidationCollection>();

  public DataValidationTable(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public DataValidationTable(
    IApplication application,
    object parent,
    System.Collections.Generic.List<BiffRecordRaw> arrRecords,
    ref int iOffset)
    : this(application, parent)
  {
    this.Parse(arrRecords, ref iOffset);
  }

  private void SetParents()
  {
    this.m_worksheet = this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;
    if (this.m_worksheet == null)
      throw new ArgumentNullException("Can't find parent worksheet.");
  }

  public void Parse(System.Collections.Generic.List<BiffRecordRaw> arrRecords, ref int iOffset)
  {
    int num = arrRecords != null ? arrRecords.Count : throw new ArgumentNullException(nameof (arrRecords));
    if (iOffset < 0 || iOffset > num)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value cannot be less than 0 or greater than arrRecords.Count.");
    while (arrRecords[iOffset] is DValRecord arrRecord)
    {
      DataValidationCollection validationCollection = new DataValidationCollection(this.Application, (object) this, arrRecords, ref iOffset);
      base.Add(validationCollection);
      this.m_hashDVals[arrRecord] = validationCollection;
      if (iOffset >= num)
        break;
    }
  }

  public DataValidationCollection Add(DataValidationCollection dval)
  {
    DValRecord record = dval.Record;
    if (this.m_hashDVals.ContainsKey(record))
      return this.m_hashDVals[record];
    this.m_hashDVals.Add(record, dval);
    base.Add(dval);
    return dval;
  }

  [CLSCompliant(false)]
  public DataValidationCollection Add(DValRecord dval)
  {
    if (this.m_hashDVals.ContainsKey(dval))
      return this.m_hashDVals[dval];
    int i = 0;
    for (int count = this.Count; i < count; ++i)
    {
      if (base[i].Worksheet.Index == this.Worksheet.Index)
      {
        DataValidationCollection validationCollection = base[i];
        this.m_hashDVals.Add(dval, validationCollection);
        return validationCollection;
      }
    }
    DataValidationCollection validationCollection1 = new DataValidationCollection(this.Application, (object) this, dval);
    this.m_hashDVals.Add(dval, validationCollection1);
    base.Add(validationCollection1);
    return validationCollection1;
  }

  public override object Clone(object parent)
  {
    DataValidationTable dataValidationTable = (DataValidationTable) base.Clone(parent);
    System.Collections.Generic.List<DataValidationCollection> innerList = dataValidationTable.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      DataValidationCollection validationCollection = innerList[index];
      dataValidationTable.m_hashDVals.Add(validationCollection.Record, validationCollection);
    }
    return (object) dataValidationTable;
  }

  public DataValidationImpl FindDataValidation(long cellIndex)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      DataValidationImpl byCellIndex = this[index].FindByCellIndex(cellIndex);
      if (byCellIndex != null)
        return byCellIndex;
    }
    return (DataValidationImpl) null;
  }

  public DataValidationImpl FindDataValidation(int row, int column)
  {
    return this.FindDataValidation(RangeImpl.GetCellIndex(column, row));
  }

  public void UpdateNamedRangeIndexes(int[] arrNewIndex)
  {
    if (arrNewIndex == null)
      throw new ArgumentNullException(nameof (arrNewIndex));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].UpdateNamedRangeIndexes(arrNewIndex);
  }

  public void UpdateNamedRangeIndexes(IDictionary<int, int> dicNewIndex)
  {
    if (dicNewIndex == null)
      throw new ArgumentNullException(nameof (dicNewIndex));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.InnerList[index].UpdateNamedRangeIndexes(dicNewIndex);
  }

  public void Remove(Rectangle[] rectangles)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this[index].Remove(rectangles);
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    System.Collections.Generic.List<DataValidationCollection> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    System.Collections.Generic.List<DataValidationCollection> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      innerList[index].UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  internal void CopyMoveTo(
    DataValidationTable destDataValidation,
    int iSourceRow,
    int iSourceColumn,
    int iDestRow,
    int iDestColumn,
    int iRowCount,
    int iColumnCount,
    bool isMove)
  {
    if (destDataValidation == null)
      throw new ArgumentNullException(nameof (destDataValidation));
    if (destDataValidation == this)
    {
      DataValidationTable destDataValidation1 = new DataValidationTable(this.Application, this.Parent);
      this.CopyMoveTo(destDataValidation1, iSourceRow, iSourceColumn, iDestRow, iDestColumn, iRowCount, iColumnCount, isMove);
      destDataValidation1.CopyMoveTo(this, iDestRow, iDestColumn, iDestRow, iDestColumn, iRowCount, iColumnCount, isMove);
    }
    else
    {
      Rectangle[] rectangles1 = new Rectangle[1]
      {
        new Rectangle(iSourceColumn - 1, iSourceRow - 1, iColumnCount - 1, iRowCount - 1)
      };
      Rectangle[] rectangles2 = new Rectangle[1]
      {
        new Rectangle(iDestColumn - 1, iDestRow - 1, iColumnCount - 1, iRowCount - 1)
      };
      if (this.m_hashDVals.Count > 0)
        destDataValidation.Remove(rectangles2);
      foreach (DataValidationCollection dvCollection in this.m_hashDVals.Values)
      {
        destDataValidation.Add(dvCollection, iSourceRow, iSourceColumn, iDestRow, iDestColumn, iRowCount, iColumnCount);
        if (isMove)
          dvCollection.Remove(rectangles1);
      }
    }
  }

  private void Add(
    DataValidationCollection dvCollection,
    int iSourceRow,
    int iSourceColumn,
    int iDestRow,
    int iDestColumn,
    int iRowCount,
    int iColumnCount)
  {
    DValRecord dvalRecord = (DValRecord) dvCollection.Record.Clone();
    bool flag = false;
    DataValidationCollection dval;
    if (!this.m_hashDVals.TryGetValue(dvalRecord, out dval))
    {
      dval = new DataValidationCollection(this.Application, (object) this, dvalRecord);
      flag = true;
    }
    dval.AddFrom(dvCollection, iSourceRow, iSourceColumn, iDestRow, iDestColumn, iRowCount, iColumnCount);
    if (!flag || dval.Count <= 0)
      return;
    if (this.m_hashDVals.Count == 0)
      this.Add(dval);
    Dictionary<DVRecord, DataValidationImpl> dictionary = new Dictionary<DVRecord, DataValidationImpl>((IDictionary<DVRecord, DataValidationImpl>) dval.HashRecords);
    DValRecord key = this.GetFirstKeyValuePair(this.m_hashDVals).Key;
    foreach (KeyValuePair<DVRecord, DataValidationImpl> keyValuePair in dictionary)
      this.m_hashDVals[key].Add(keyValuePair.Value);
  }

  private KeyValuePair<DValRecord, DataValidationCollection> GetFirstKeyValuePair(
    Dictionary<DValRecord, DataValidationCollection> collection)
  {
    KeyValuePair<DValRecord, DataValidationCollection> firstKeyValuePair = new KeyValuePair<DValRecord, DataValidationCollection>();
    using (Dictionary<DValRecord, DataValidationCollection>.Enumerator enumerator = collection.GetEnumerator())
    {
      if (enumerator.MoveNext())
        firstKeyValuePair = enumerator.Current;
    }
    return firstKeyValuePair;
  }

  protected override void OnClearComplete() => this.m_hashDVals.Clear();

  public WorksheetImpl Worksheet => this.m_worksheet;

  public WorkbookImpl Workbook => this.m_worksheet.ParentWorkbook;

  public new DataValidationCollection this[int index]
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
      int index = 0;
      for (int count = this.Count; index < count; ++index)
        shapesCount += this[index].ShapesCount;
      return shapesCount;
    }
  }
}
