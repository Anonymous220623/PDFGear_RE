// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.GlowFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class GlowFormat
{
  private Color m_color;
  private float m_radius;
  private float m_transparency;
  private ShapeBase m_shape;
  private byte m_flags;

  internal Color Color
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  internal float Radius
  {
    get => this.m_radius;
    set => this.m_radius = value;
  }

  internal float Transparency
  {
    get => this.m_transparency;
    set => this.m_transparency = value;
  }

  internal bool IsInlineColor
  {
    get => ((int) this.m_flags & 1) != 0;
    set => this.m_flags = (byte) ((int) this.m_flags & 254 | (value ? 1 : 0));
  }

  internal bool IsInlineRadius
  {
    get => ((int) this.m_flags & 2) >> 1 != 0;
    set => this.m_flags = (byte) ((int) this.m_flags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsInlineTransparency
  {
    get => ((int) this.m_flags & 4) >> 2 != 0;
    set => this.m_flags = (byte) ((int) this.m_flags & 251 | (value ? 1 : 0) << 2);
  }

  internal GlowFormat(ShapeBase shape) => this.m_shape = shape;

  internal void Close()
  {
    if (this.m_shape == null)
      return;
    this.m_shape = (ShapeBase) null;
  }

  internal GlowFormat Clone() => (GlowFormat) this.MemberwiseClone();
}
