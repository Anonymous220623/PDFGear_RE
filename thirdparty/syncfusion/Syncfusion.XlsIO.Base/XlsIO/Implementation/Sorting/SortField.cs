// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Sorting.SortField
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Sorting;

internal class SortField : ISortField
{
  private int m_iKey;
  private SortOn m_sortOn;
  private OrderBy m_Order;
  private Color m_color;
  private SortFields m_parent;

  public int Key
  {
    get => this.m_iKey;
    set => this.m_iKey = value;
  }

  public SortOn SortOn
  {
    get => this.m_sortOn;
    set => this.m_sortOn = value;
  }

  public OrderBy Order
  {
    get => this.m_Order;
    set => this.m_Order = value;
  }

  public Color Color
  {
    get => this.m_color;
    set
    {
      int byKey = this.m_parent.FindByKey(this.Key);
      if (byKey != -1 && (int) this.m_parent[byKey].Color.R == (int) value.R && (int) this.m_parent[byKey].Color.G == (int) value.G && (int) this.m_parent[byKey].Color.B == (int) value.B)
        this.m_parent.RemoveLast(this.Key);
      else
        this.m_color = value;
    }
  }

  public SortField(SortFields parent) => this.m_parent = parent;

  public void SetPriority(int priority) => this.m_parent.SetPriority(this, priority);
}
