// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.MergedCellInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal struct MergedCellInfo
{
  private TableSpan m_tableSpan;
  private int m_rowSpan;
  private int m_colspan;
  private bool m_isFirst;
  private long m_firstCellIndex;

  internal TableSpan TableSpan
  {
    get => this.m_tableSpan;
    set => this.m_tableSpan = value;
  }

  internal int RowSpan
  {
    get => this.m_rowSpan;
    set => this.m_rowSpan = value;
  }

  internal int ColSpan
  {
    get => this.m_colspan;
    set => this.m_colspan = value;
  }

  internal bool IsFirst
  {
    get => this.m_isFirst;
    set => this.m_isFirst = value;
  }

  internal long FirstCellIndex
  {
    get => this.m_firstCellIndex;
    set => this.m_firstCellIndex = value;
  }

  internal MergedCellInfo Clone() => (MergedCellInfo) this.MemberwiseClone();
}
