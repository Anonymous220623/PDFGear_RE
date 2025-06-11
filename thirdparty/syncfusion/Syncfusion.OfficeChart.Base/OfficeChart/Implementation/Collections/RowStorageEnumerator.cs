// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.RowStorageEnumerator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class RowStorageEnumerator : IEnumerator
{
  private RowStorage m_rowStorage;
  private int m_iOffset = -1;
  private RecordExtractor m_recordExtractor;

  private RowStorageEnumerator()
  {
  }

  public RowStorageEnumerator(RowStorage row, RecordExtractor recordExtractor)
  {
    if (row == null)
      throw new ArgumentNullException(nameof (row));
    if (recordExtractor == null)
      throw new ArgumentNullException(nameof (recordExtractor));
    this.m_rowStorage = row;
    this.m_recordExtractor = recordExtractor;
  }

  public void Reset() => this.m_iOffset = -1;

  public bool MoveNext()
  {
    bool flag;
    if (this.m_rowStorage.UsedSize == 0)
      flag = false;
    else if (this.m_iOffset == -1)
    {
      this.m_iOffset = 0;
      flag = true;
    }
    else
    {
      int num = this.m_rowStorage.MoveNextCell(this.m_iOffset);
      if (num == this.m_rowStorage.UsedSize)
      {
        this.m_iOffset = -1;
        flag = false;
      }
      else
      {
        this.m_iOffset = num;
        flag = true;
      }
    }
    return flag;
  }

  public object Current
  {
    get
    {
      return (object) this.m_recordExtractor.GetRecord(this.m_rowStorage.Provider, this.m_iOffset, this.m_rowStorage.Version);
    }
  }

  [CLSCompliant(false)]
  public ArrayRecord GetArrayRecord()
  {
    return this.m_iOffset != -1 ? this.m_rowStorage.GetArrayRecordByOffset(this.m_iOffset) : throw new InvalidOperationException("Enumerator pointer is not set to an object instance");
  }

  public string GetFormulaStringValue()
  {
    return this.m_iOffset != -1 ? this.m_rowStorage.GetFormulaStringValueByOffset(this.m_iOffset) : throw new InvalidOperationException("Enumerator pointer is not set to an object instance");
  }

  public int RowIndex
  {
    get
    {
      return this.m_iOffset != -1 ? this.m_rowStorage.GetRow(this.m_iOffset) : throw new InvalidOperationException("Enumerator pointer is not set to an object instance");
    }
  }

  public int ColumnIndex
  {
    get
    {
      return this.m_iOffset != -1 ? this.m_rowStorage.GetColumn(this.m_iOffset) : throw new InvalidOperationException("Enumerator pointer is not set to an object instance");
    }
  }

  public int XFIndex
  {
    get
    {
      return this.m_iOffset != -1 ? (int) this.m_rowStorage.GetXFIndex(this.m_iOffset, false) : throw new InvalidOperationException("Enumerator pointer is not set to an object instance");
    }
  }
}
