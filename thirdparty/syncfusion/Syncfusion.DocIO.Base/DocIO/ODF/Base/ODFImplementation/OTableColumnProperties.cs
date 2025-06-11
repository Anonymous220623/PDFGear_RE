// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OTableColumnProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OTableColumnProperties
{
  private double m_columnWidth;
  private bool m_useOptimalColumnWidth;

  internal double ColumnWidth
  {
    get => this.m_columnWidth;
    set => this.m_columnWidth = value;
  }

  internal bool UseOptimalColumnWidth
  {
    get => this.m_useOptimalColumnWidth;
    set => this.m_useOptimalColumnWidth = value;
  }

  public override bool Equals(object obj)
  {
    return obj is OTableColumnProperties columnProperties && this.m_columnWidth == columnProperties.ColumnWidth;
  }
}
