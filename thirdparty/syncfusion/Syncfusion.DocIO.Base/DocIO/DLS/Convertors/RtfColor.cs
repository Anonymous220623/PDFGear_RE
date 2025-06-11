// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.RtfColor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

internal class RtfColor
{
  private int m_redN;
  private int m_greenN;
  private int m_blueN;

  internal int RedN
  {
    get => this.m_redN;
    set => this.m_redN = value;
  }

  internal int GreenN
  {
    get => this.m_greenN;
    set => this.m_greenN = value;
  }

  internal int BlueN
  {
    get => this.m_blueN;
    set => this.m_blueN = value;
  }
}
