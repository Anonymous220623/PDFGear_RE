// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.OTableRow
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

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
