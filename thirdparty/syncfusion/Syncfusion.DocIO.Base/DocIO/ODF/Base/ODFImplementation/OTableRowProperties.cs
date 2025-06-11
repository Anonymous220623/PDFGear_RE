// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OTableRowProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OTableRowProperties
{
  private double m_rowHeight;
  private bool m_useOptimalRowHeight;
  private bool m_isBreakAcrossPages;
  private bool m_isHeaderRow;

  internal double RowHeight
  {
    get => this.m_rowHeight;
    set => this.m_rowHeight = value;
  }

  internal bool UseOptimalRowHeight
  {
    get => this.m_useOptimalRowHeight;
    set => this.m_useOptimalRowHeight = value;
  }

  internal bool IsBreakAcrossPages
  {
    get => this.m_isBreakAcrossPages;
    set => this.m_isBreakAcrossPages = value;
  }

  internal bool IsHeaderRow
  {
    get => this.m_isHeaderRow;
    set => this.m_isHeaderRow = value;
  }

  public override bool Equals(object obj)
  {
    return obj is OTableRowProperties otableRowProperties && this.m_rowHeight == otableRowProperties.RowHeight;
  }
}
