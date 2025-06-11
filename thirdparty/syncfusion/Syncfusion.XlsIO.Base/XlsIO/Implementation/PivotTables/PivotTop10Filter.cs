// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotTop10Filter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotTop10Filter
{
  private double m_dFilterValue;
  private double m_dValue;
  private bool m_bPercent;
  private bool m_bTop = true;

  public double FilterValue
  {
    get => this.m_dFilterValue;
    set => this.m_dFilterValue = value;
  }

  public double Value
  {
    get => this.m_dValue;
    set => this.m_dValue = value;
  }

  public bool IsPercent
  {
    get => this.m_bPercent;
    set => this.m_bPercent = value;
  }

  public bool IsTop
  {
    get => this.m_bTop;
    set => this.m_bTop = value;
  }
}
