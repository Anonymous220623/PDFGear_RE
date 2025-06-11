// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Margins
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public sealed class Margins
{
  private int m_iLeft;
  private int m_iRight;
  private int m_iTop;
  private int m_iBottom;

  public int All
  {
    get => !this.IsAll ? 0 : this.m_iLeft;
    set
    {
      if (this.IsAll || this.m_iLeft == value)
        return;
      this.m_iLeft = this.m_iRight = this.m_iTop = this.m_iBottom = value;
    }
  }

  public int Left
  {
    get => this.m_iLeft;
    set
    {
      if (value == this.m_iLeft)
        return;
      this.m_iLeft = value;
    }
  }

  public int Right
  {
    get => this.m_iRight;
    set
    {
      if (value == this.m_iRight)
        return;
      this.m_iRight = value;
    }
  }

  public int Top
  {
    get => this.m_iTop;
    set
    {
      if (value == this.m_iTop)
        return;
      this.m_iTop = value;
    }
  }

  public int Bottom
  {
    get => this.m_iBottom;
    set
    {
      if (value == this.m_iBottom)
        return;
      this.m_iBottom = value;
    }
  }

  private bool IsAll
  {
    get
    {
      return this.m_iLeft == this.m_iRight && this.m_iRight == this.m_iTop && this.m_iTop == this.m_iBottom;
    }
  }

  public Margins()
  {
  }

  public Margins(int left, int top, int right, int bottom)
  {
    this.m_iLeft = left;
    this.m_iTop = top;
    this.m_iRight = right;
    this.m_iBottom = bottom;
  }
}
