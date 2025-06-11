// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TileRectangle
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class TileRectangle
{
  private float m_bottomOffset;
  private float m_leftOffset;
  private float m_rightOffset;
  private float m_topOffset;
  private byte m_flag;

  internal float BottomOffset
  {
    get => this.m_bottomOffset;
    set => this.m_bottomOffset = value;
  }

  internal float LeftOffset
  {
    get => this.m_leftOffset;
    set => this.m_leftOffset = value;
  }

  internal float RightOffset
  {
    get => this.m_rightOffset;
    set => this.m_rightOffset = value;
  }

  internal float TopOffset
  {
    get => this.m_topOffset;
    set => this.m_topOffset = value;
  }

  internal bool HasAttributes
  {
    get => ((int) this.m_flag & 1) != 0;
    set => this.m_flag = (byte) ((int) this.m_flag & 254 | (value ? 1 : 0));
  }

  internal TileRectangle()
  {
  }

  internal TileRectangle Clone() => (TileRectangle) this.MemberwiseClone();
}
