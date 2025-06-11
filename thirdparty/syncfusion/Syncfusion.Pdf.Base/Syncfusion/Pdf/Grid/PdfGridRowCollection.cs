// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Grid.PdfGridRowCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Grid;

public class PdfGridRowCollection : List<PdfGridRow>
{
  private PdfGrid m_grid;

  internal PdfGridRowCollection(PdfGrid grid) => this.m_grid = grid;

  public PdfGridRow Add()
  {
    PdfGridRow row = new PdfGridRow(this.m_grid);
    this.Add(row);
    return row;
  }

  public new void Add(PdfGridRow row)
  {
    row.Style = this.m_grid.Style as PdfGridRowStyle;
    if (row.Cells.Count == 0)
    {
      for (int index = 0; index < this.m_grid.Columns.Count; ++index)
        row.Cells.Add(new PdfGridCell());
    }
    base.Add(row);
  }

  public void SetSpan(int rowIndex, int cellIndex, int rowSpan, int colSpan)
  {
    if (rowIndex > this.m_grid.Rows.Count)
      throw new IndexOutOfRangeException(nameof (rowIndex));
    if (cellIndex > this.m_grid.Columns.Count)
      throw new IndexOutOfRangeException(nameof (cellIndex));
    this.m_grid.Rows[rowIndex].Cells[cellIndex].RowSpan = rowSpan;
    this.m_grid.Rows[rowIndex].Cells[cellIndex].ColumnSpan = colSpan;
  }

  public void ApplyStyle(PdfGridStyleBase style)
  {
    switch (style)
    {
      case PdfGridCellStyle _:
        using (List<PdfGridRow>.Enumerator enumerator = this.m_grid.Rows.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            foreach (PdfGridCell cell in enumerator.Current.Cells)
              cell.Style = style.Clone() as PdfGridCellStyle;
          }
          break;
        }
      case PdfGridRowStyle _:
        using (List<PdfGridRow>.Enumerator enumerator = this.m_grid.Rows.GetEnumerator())
        {
          while (enumerator.MoveNext())
            enumerator.Current.Style = style as PdfGridRowStyle;
          break;
        }
      case PdfGridStyle _:
        this.m_grid.Style = style as PdfGridStyle;
        break;
    }
  }
}
