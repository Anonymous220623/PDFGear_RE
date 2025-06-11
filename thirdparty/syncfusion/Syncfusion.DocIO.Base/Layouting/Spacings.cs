// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.Spacings
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.Layouting;

internal class Spacings
{
  private float m_left;
  private float m_top;
  private float m_right;
  private float m_bottom;

  public float Left
  {
    get => this.m_left;
    set => this.m_left = value;
  }

  public float Top
  {
    get => this.m_top;
    set => this.m_top = value;
  }

  public float Right
  {
    get => this.m_right;
    set => this.m_right = value;
  }

  public float Bottom
  {
    get => this.m_bottom;
    set => this.m_bottom = value;
  }
}
