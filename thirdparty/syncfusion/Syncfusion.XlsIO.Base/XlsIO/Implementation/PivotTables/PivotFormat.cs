// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotFormat
{
  private PivotArea m_pivotArea;
  private IPivotCellFormat m_pivotCellFormat;
  private PivotFormats m_parent;
  private PivotTableImpl m_pivotTable;

  internal PivotArea PivotArea
  {
    get => this.m_pivotArea;
    set => this.m_pivotArea = value;
  }

  internal IPivotCellFormat PivotCellFormat
  {
    get => this.m_pivotCellFormat;
    set => this.m_pivotCellFormat = value;
  }

  internal PivotTableImpl PivotTable => this.m_pivotTable;

  internal PivotFormat(PivotTableImpl pivotTable)
  {
    this.m_pivotTable = pivotTable;
    this.m_parent = pivotTable.PivotFormats;
    this.m_pivotArea = new PivotArea(pivotTable);
    this.m_pivotArea.FieldIndex = -1;
  }

  internal object Clone(PivotFormats parent)
  {
    PivotFormat pivotFormat = (PivotFormat) this.MemberwiseClone();
    pivotFormat.m_parent = parent;
    pivotFormat.m_pivotTable = parent.Parent;
    if (this.m_pivotArea != null)
      pivotFormat.m_pivotArea = this.m_pivotArea.Clone((object) pivotFormat) as PivotArea;
    if (this.m_pivotCellFormat != null)
      pivotFormat.PivotCellFormat = (IPivotCellFormat) (this.m_pivotCellFormat as Syncfusion.XlsIO.Implementation.PivotCellFormat).Clone(pivotFormat);
    return (object) pivotFormat;
  }
}
