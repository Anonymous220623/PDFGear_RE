// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridColumnCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGridColumnCollection : IEnumerable
{
  private PdfGrid m_grid;
  private List<PdfGridColumn> m_columns = new List<PdfGridColumn>();
  private float m_width = float.MinValue;
  private float m_previousCellsCount;

  public PdfGridColumn this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? this.m_columns[index] : throw new IndexOutOfRangeException();
    }
  }

  public int Count => this.m_columns.Count;

  internal float Width
  {
    get
    {
      if ((double) this.m_width == -3.4028234663852886E+38)
        this.m_width = this.MeasureColumnsWidth();
      if ((double) this.m_grid.InitialWidth != 0.0 && (double) this.m_width != (double) this.m_grid.InitialWidth && !this.m_grid.Style.AllowHorizontalOverflow)
      {
        this.m_width = this.m_grid.InitialWidth;
        this.m_grid.IsPageWidth = true;
      }
      return this.m_width;
    }
  }

  public PdfGridColumnCollection(PdfGrid grid)
  {
    this.m_grid = grid;
    this.m_columns = new List<PdfGridColumn>();
  }

  internal void Clear() => this.m_columns.Clear();

  public PdfGridColumn Add()
  {
    PdfGridColumn pdfGridColumn = new PdfGridColumn(this.m_grid);
    this.m_columns.Add(pdfGridColumn);
    return pdfGridColumn;
  }

  public void Add(int count)
  {
    for (int index = 0; index < count; ++index)
    {
      this.m_columns.Add(new PdfGridColumn(this.m_grid));
      foreach (PdfGridRow row in (List<PdfGridRow>) this.m_grid.Rows)
        row.Cells.Add(new PdfGridCell()
        {
          Value = (object) ""
        });
    }
  }

  public void Add(PdfGridColumn column)
  {
    if (column == null)
      throw new ArgumentNullException(nameof (column));
    this.m_columns.Add(column);
  }

  internal void AddColumns(int count)
  {
    if ((double) this.m_previousCellsCount == (double) count)
      return;
    for (int index = count - 1; index < count; ++index)
      this.m_columns.Add(new PdfGridColumn(this.m_grid));
    this.m_previousCellsCount = (float) count;
  }

  internal float MeasureColumnsWidth()
  {
    float num = 0.0f;
    this.m_grid.MeasureColumnsWidth();
    int index = 0;
    for (int count = this.m_columns.Count; index < count; ++index)
      num += this.m_columns[index].Width;
    return num;
  }

  internal float[] GetDefaultWidths(float totalWidth)
  {
    float[] defaultWidths = new float[this.Count];
    int count = this.Count;
    for (int index = 0; index < this.Count; ++index)
    {
      if (this.m_grid.IsPageWidth && (double) totalWidth >= 0.0 && !this.m_columns[index].isCustomWidth)
      {
        this.m_columns[index].Width = float.MinValue;
      }
      else
      {
        defaultWidths[index] = this.m_columns[index].Width;
        if ((double) this.m_columns[index].Width > 0.0 && this.m_columns[index].isCustomWidth)
        {
          totalWidth -= this.m_columns[index].Width;
          --count;
        }
        else
          defaultWidths[index] = float.MinValue;
      }
    }
    for (int index = 0; index < this.Count; ++index)
    {
      float num = totalWidth / (float) count;
      if ((double) defaultWidths[index] <= 0.0)
        defaultWidths[index] = num;
    }
    return defaultWidths;
  }

  public IEnumerator GetEnumerator()
  {
    return (IEnumerator) new PdfGridColumnCollection.PdfGridColumnEnumerator(this);
  }

  private struct PdfGridColumnEnumerator : IEnumerator
  {
    private PdfGridColumnCollection m_columnCollection;
    private int m_currentIndex;

    internal PdfGridColumnEnumerator(PdfGridColumnCollection columnCollection)
    {
      this.m_columnCollection = columnCollection != null ? columnCollection : throw new ArgumentNullException(nameof (columnCollection));
      this.m_currentIndex = -1;
    }

    public object Current
    {
      get
      {
        this.CheckIndex();
        return (object) this.m_columnCollection[this.m_currentIndex];
      }
    }

    public bool MoveNext()
    {
      ++this.m_currentIndex;
      return this.m_currentIndex < this.m_columnCollection.Count;
    }

    public void Reset() => this.m_currentIndex = -1;

    private void CheckIndex()
    {
      if (this.m_currentIndex < 0 || this.m_currentIndex >= this.m_columnCollection.Count)
        throw new IndexOutOfRangeException();
    }
  }
}
