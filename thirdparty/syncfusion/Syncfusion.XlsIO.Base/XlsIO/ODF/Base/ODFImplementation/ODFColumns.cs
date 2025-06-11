// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.ODFColumns
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

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
