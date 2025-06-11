// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.ODFColumns
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class ODFColumns
{
  private int m_columnCount;
  private int m_columnGap;

  public int ColumnGap
  {
    get => this.m_columnGap;
    set => this.m_columnGap = value;
  }

  internal int ColumnCount
  {
    get => this.m_columnCount;
    set => this.m_columnCount = value;
  }
}
