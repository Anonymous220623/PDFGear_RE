// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.PathGradient
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class PathGradient
{
  private GradientShadeType m_pathShade;
  private short m_bottomOffset;
  private short m_leftOffset;
  private short m_rightOffset;
  private short m_topOffset;

  internal GradientShadeType PathShade
  {
    get => this.m_pathShade;
    set => this.m_pathShade = value;
  }

  internal short BottomOffset
  {
    get => this.m_bottomOffset;
    set => this.m_bottomOffset = value;
  }

  internal short LeftOffset
  {
    get => this.m_leftOffset;
    set => this.m_leftOffset = value;
  }

  internal short RightOffset
  {
    get => this.m_rightOffset;
    set => this.m_rightOffset = value;
  }

  internal short TopOffset
  {
    get => this.m_topOffset;
    set => this.m_topOffset = value;
  }

  internal PathGradient()
  {
  }

  internal PathGradient Clone() => (PathGradient) this.MemberwiseClone();
}
