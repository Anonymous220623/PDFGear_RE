// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OTableRow
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OTableRow : OTableColumn
{
  private List<OTableCell> m_cells;

  internal List<OTableCell> Cells
  {
    get
    {
      if (this.m_cells == null)
        this.m_cells = new List<OTableCell>();
      return this.m_cells;
    }
    set => this.m_cells = value;
  }

  internal void Dispose()
  {
    if (this.m_cells == null)
      return;
    foreach (OTableCell cell in this.m_cells)
      cell.Dispose();
    this.m_cells.Clear();
    this.m_cells = (List<OTableCell>) null;
  }
}
