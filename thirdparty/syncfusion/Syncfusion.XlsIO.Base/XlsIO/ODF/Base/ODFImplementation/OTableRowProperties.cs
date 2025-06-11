// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.OTableRowProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class OTableRowProperties
{
  private double m_rowHeight;
  private bool m_useOptimalRowHeight;

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

  public override bool Equals(object obj)
  {
    return obj is OTableRowProperties otableRowProperties && this.m_rowHeight == otableRowProperties.RowHeight;
  }
}
