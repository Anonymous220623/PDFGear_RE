// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.RecordTableEnumerator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class RecordTableEnumerator : IDictionaryEnumerator, IEnumerator
{
  private CellRecordCollection m_table;
  private int m_iRow = -1;
  private RecordTable m_sfTable;
  private int m_iOffset;

  private RecordTableEnumerator()
  {
  }

  public RecordTableEnumerator(CellRecordCollection table)
  {
    this.m_table = table != null ? table : throw new ArgumentNullException(nameof (table));
    this.m_sfTable = table.Table;
  }

  public void Reset()
  {
    this.m_iRow = this.m_sfTable.FirstRow - 1;
    this.m_iOffset = 0;
    this.MoveNextRow();
  }

  public object Current
  {
    get
    {
      if (this.m_iRow < 0)
        return (object) null;
      RowStorage row = this.m_sfTable.Rows[this.m_iRow];
      if (row == null)
        return (object) new DictionaryEntry((object) null, (object) null);
      ICellPositionFormat recordAtOffset = (ICellPositionFormat) row.GetRecordAtOffset(this.m_iOffset);
      return (object) new DictionaryEntry((object) RangeImpl.GetCellIndex(recordAtOffset.Column + 1, recordAtOffset.Row + 1), (object) recordAtOffset);
    }
  }

  public bool MoveNext()
  {
    int lastRow = this.m_sfTable.LastRow;
    if (this.m_iRow < 0)
    {
      this.m_iRow = this.m_sfTable.FirstRow;
      this.m_iOffset = 0;
      if (this.m_iRow < 0 || this.m_iRow > lastRow)
        return false;
      for (RowStorage row = this.m_sfTable.Rows[this.m_iRow]; (row == null || row.UsedSize <= 0) && this.m_iRow <= lastRow; row = this.m_sfTable.Rows[this.m_iRow])
        ++this.m_iRow;
      return true;
    }
    RowStorage row1 = this.m_sfTable.Rows[this.m_iRow];
    if (row1 == null)
      return false;
    this.m_iOffset = row1.MoveNextCell(this.m_iOffset);
    return this.m_iOffset < row1.UsedSize || this.MoveNextRow();
  }

  private bool MoveNextRow()
  {
    int lastRow = this.m_sfTable.LastRow;
    ArrayListEx rows = this.m_sfTable.Rows;
    for (++this.m_iRow; this.m_iRow <= lastRow; ++this.m_iRow)
    {
      RowStorage rowStorage = rows[this.m_iRow];
      if (rowStorage != null && rowStorage.UsedSize > 0)
      {
        this.m_iOffset = 0;
        return true;
      }
    }
    return false;
  }

  public object Key => this.Entry.Key;

  public object Value => this.Entry.Value;

  public DictionaryEntry Entry
  {
    get
    {
      if (this.m_iRow < 0)
        throw new InvalidOperationException();
      ICellPositionFormat recordAtOffset = (ICellPositionFormat) this.m_sfTable.Rows[this.m_iRow].GetRecordAtOffset(this.m_iOffset);
      return new DictionaryEntry((object) RangeImpl.GetCellIndex(recordAtOffset.Column + 1, recordAtOffset.Row + 1), (object) recordAtOffset);
    }
  }
}
