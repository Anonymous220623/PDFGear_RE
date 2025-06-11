// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.LinearGradient
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class LinearGradient
{
  private short m_angle;
  private byte m_bFlags = 2;

  internal bool AnglePositive
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal short Angle
  {
    get => this.m_angle;
    set => this.m_angle = value;
  }

  internal bool IsAngleDefined
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal bool Scaled
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal LinearGradient()
  {
  }

  internal LinearGradient Clone() => (LinearGradient) this.MemberwiseClone();
}
