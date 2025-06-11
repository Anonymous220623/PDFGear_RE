// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.SFTable
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class SFTable : ICloneable
{
  private int m_iRowCount;
  private int m_iColumnCount;
  private SFArrayList<object> m_arrRows;
  private int m_iCellCount;

  public SFTable(int iRowCount, int iColumnCount)
  {
    this.m_iRowCount = iRowCount;
    this.m_iColumnCount = iColumnCount;
  }

  protected SFTable(SFTable data, bool clone)
  {
    this.m_iRowCount = data.m_iRowCount;
    this.m_iColumnCount = data.m_iColumnCount;
    if (data.m_arrRows == null || !clone)
      return;
    this.m_iCellCount = data.m_iCellCount;
    this.m_arrRows = (SFArrayList<object>) data.m_arrRows.Clone();
  }

  protected SFTable(SFTable data, bool clone, object parent)
  {
    this.m_iRowCount = data.m_iRowCount;
    this.m_iColumnCount = data.m_iColumnCount;
    if (data.m_arrRows == null || !clone)
      return;
    this.m_iCellCount = data.m_iCellCount;
    this.m_arrRows = (SFArrayList<object>) data.m_arrRows.Clone(parent);
  }

  public virtual object Clone() => (object) new SFTable(this, true);

  public virtual object Clone(object parent) => (object) new SFTable(this, true, parent);

  public SFArrayList<object> Rows
  {
    get
    {
      if (this.m_arrRows == null)
        this.m_arrRows = new SFArrayList<object>();
      return this.m_arrRows;
    }
  }

  public int RowCount => this.m_iRowCount;

  public int ColCount => this.m_iColumnCount;

  public int CellCount => this.m_iCellCount;

  public void Clear() => this.m_arrRows = (SFArrayList<object>) null;

  public virtual SFArrayList<object> CreateCellCollection() => new SFArrayList<object>();

  public bool Contains(int rowIndex, int colIndex)
  {
    return rowIndex >= 0 && rowIndex < this.m_iRowCount && colIndex >= 0 && colIndex < this.m_iColumnCount && this.Rows[rowIndex] is SFArrayList<object> row && row[colIndex] != null;
  }

  public object this[int rowIndex, int colIndex]
  {
    get
    {
      if (rowIndex >= this.m_iRowCount || rowIndex < 0 || colIndex >= this.m_iColumnCount || colIndex < 0)
        return (object) null;
      return this.Rows[rowIndex] is SFArrayList<object> row ? row[colIndex] : (object) null;
    }
    set
    {
      if (rowIndex >= this.m_iRowCount || rowIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (rowIndex));
      if (colIndex >= this.m_iColumnCount || colIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (colIndex));
      SFArrayList<object> rows = this.Rows;
      if (!(rows[rowIndex] is SFArrayList<object> cellCollection))
      {
        if (value == null)
          return;
        rows[rowIndex] = (object) (cellCollection = this.CreateCellCollection());
      }
      if (cellCollection[colIndex] != null)
      {
        if (value == null)
          --this.m_iCellCount;
      }
      else if (value != null)
        ++this.m_iCellCount;
      cellCollection[colIndex] = value;
    }
  }
}
