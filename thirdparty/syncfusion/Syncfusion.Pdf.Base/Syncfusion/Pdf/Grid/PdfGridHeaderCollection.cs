// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridHeaderCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGridHeaderCollection : IEnumerable
{
  internal PdfGrid m_grid;
  private List<PdfGridRow> m_rows = new List<PdfGridRow>();

  public PdfGridRow this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? this.m_rows[index] : throw new IndexOutOfRangeException();
    }
  }

  public int Count => this.m_rows.Count;

  public PdfGridHeaderCollection(PdfGrid grid)
  {
    this.m_grid = grid;
    this.m_rows = new List<PdfGridRow>();
  }

  internal void Add(PdfGridRow row)
  {
    row.IsHeaderRow = true;
    this.m_rows.Add(row);
  }

  public PdfGridRow[] Add(int count)
  {
    for (int index1 = 0; index1 < count; ++index1)
    {
      PdfGridRow pdfGridRow = new PdfGridRow(this.m_grid);
      for (int index2 = 0; index2 < this.m_grid.Columns.Count; ++index2)
        pdfGridRow.Cells.Add(new PdfGridCell());
      pdfGridRow.IsHeaderRow = true;
      this.m_rows.Add(pdfGridRow);
    }
    return this.m_rows.ToArray();
  }

  public void Clear() => this.m_rows.Clear();

  public void ApplyStyle(PdfGridStyleBase style)
  {
    switch (style)
    {
      case PdfGridCellStyle _:
        IEnumerator enumerator1 = this.m_grid.Headers.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
          {
            foreach (PdfGridCell cell in ((PdfGridRow) enumerator1.Current).Cells)
              cell.Style = style as PdfGridCellStyle;
          }
          break;
        }
        finally
        {
          if (enumerator1 is IDisposable disposable)
            disposable.Dispose();
        }
      case PdfGridRowStyle _:
        IEnumerator enumerator2 = this.m_grid.Headers.GetEnumerator();
        try
        {
          while (enumerator2.MoveNext())
            ((PdfGridRow) enumerator2.Current).Style = style as PdfGridRowStyle;
          break;
        }
        finally
        {
          if (enumerator2 is IDisposable disposable)
            disposable.Dispose();
        }
    }
  }

  internal int IndexOf(PdfGridRow row) => this.m_rows.IndexOf(row);

  public IEnumerator GetEnumerator()
  {
    return (IEnumerator) new PdfGridHeaderCollection.PdfGridHeaderRowEnumerator(this);
  }

  private struct PdfGridHeaderRowEnumerator : IEnumerator
  {
    private PdfGridHeaderCollection m_headerRowCollection;
    private int m_currentIndex;

    internal PdfGridHeaderRowEnumerator(PdfGridHeaderCollection rowCollection)
    {
      this.m_headerRowCollection = rowCollection != null ? rowCollection : throw new ArgumentNullException(nameof (rowCollection));
      this.m_currentIndex = -1;
    }

    public object Current
    {
      get
      {
        this.CheckIndex();
        return (object) this.m_headerRowCollection[this.m_currentIndex];
      }
    }

    public bool MoveNext()
    {
      ++this.m_currentIndex;
      return this.m_currentIndex < this.m_headerRowCollection.Count;
    }

    public void Reset() => this.m_currentIndex = -1;

    private void CheckIndex()
    {
      if (this.m_currentIndex < 0 || this.m_currentIndex >= this.m_headerRowCollection.Count)
        throw new IndexOutOfRangeException();
    }
  }
}
