// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotValueCollections
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotValueCollections
{
  private IRange m_strImmediateRowHeader;
  private IRange m_strImmediateColumnHeader;
  private CellType m_pivotCellType;
  private string m_strValue;
  private PivotTableParts m_PivotTablePartStyle;
  private ExtendedFormatImpl m_XF;

  public PivotTableParts PivotTablePartStyle
  {
    get => this.m_PivotTablePartStyle;
    set => this.m_PivotTablePartStyle = value;
  }

  public IRange ImmediateRowHeader
  {
    get => this.m_strImmediateRowHeader;
    set => this.m_strImmediateRowHeader = value;
  }

  public IRange ImmediateColumnHeader
  {
    get => this.m_strImmediateColumnHeader;
    set => this.m_strImmediateColumnHeader = value;
  }

  public CellType PivotCellType
  {
    get => this.m_pivotCellType;
    set => this.m_pivotCellType = value;
  }

  public string Value
  {
    get => this.m_strValue;
    set => this.m_strValue = value;
  }

  public ExtendedFormatImpl XF
  {
    get => this.m_XF;
    set => this.m_XF = value;
  }

  public PivotValueCollections()
  {
    this.PivotCellType = CellType.None;
    this.ImmediateRowHeader = (IRange) null;
    this.ImmediateColumnHeader = (IRange) null;
    this.Value = (string) null;
  }
}
