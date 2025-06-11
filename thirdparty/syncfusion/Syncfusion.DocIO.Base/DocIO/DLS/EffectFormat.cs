// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.EffectFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class EffectFormat
{
  private ShadowFormat m_shadowFormat;
  private GlowFormat m_glowFormat;
  private ThreeDFormat m_threeDFormat;
  private Reflection m_reflection;
  private float m_softEdgeRadius;
  private ShapeBase m_shape;
  private ChildShape m_childShape;
  private GroupShape m_groupShape;
  private byte m_flagA;
  internal Dictionary<string, Stream> m_docxProps = new Dictionary<string, Stream>();

  internal bool IsShadowEffect
  {
    get => ((int) this.m_flagA & 1) != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 254 | (value ? 1 : 0));
  }

  internal bool IsGlowEffect
  {
    get => ((int) this.m_flagA & 2) >> 1 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 253 | (value ? 1 : 0) << 1);
  }

  internal bool IsReflection
  {
    get => ((int) this.m_flagA & 4) >> 2 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsSoftEdge
  {
    get => ((int) this.m_flagA & 8) >> 3 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 247 | (value ? 1 : 0) << 3);
  }

  internal bool NoSoftEdges
  {
    get => ((int) this.m_flagA & 16 /*0x10*/) >> 4 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 239 | (value ? 1 : 0) << 4);
  }

  internal bool IsSceneProperties
  {
    get => ((int) this.m_flagA & 32 /*0x20*/) >> 5 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 223 | (value ? 1 : 0) << 5);
  }

  internal bool IsShapeProperties
  {
    get => ((int) this.m_flagA & 64 /*0x40*/) >> 6 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 191 | (value ? 1 : 0) << 6);
  }

  internal bool IsEffectListItem
  {
    get => ((int) this.m_flagA & 128 /*0x80*/) >> 7 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
  }

  internal ShadowFormat ShadowFormat
  {
    get
    {
      if (this.m_shadowFormat == null)
        this.m_shadowFormat = new ShadowFormat(this.m_shape);
      return this.m_shadowFormat;
    }
    set => this.m_shadowFormat = value;
  }

  internal GlowFormat GlowFormat
  {
    get
    {
      if (this.m_glowFormat == null)
        this.m_glowFormat = new GlowFormat(this.m_shape);
      return this.m_glowFormat;
    }
    set => this.m_glowFormat = value;
  }

  internal ThreeDFormat ThreeDFormat
  {
    get
    {
      if (this.m_threeDFormat == null)
        this.m_threeDFormat = new ThreeDFormat(this.m_shape);
      return this.m_threeDFormat;
    }
    set => this.m_threeDFormat = value;
  }

  internal Reflection ReflectionFormat
  {
    get
    {
      if (this.m_reflection == null)
        this.m_reflection = new Reflection(this.m_shape);
      return this.m_reflection;
    }
    set => this.m_reflection = value;
  }

  internal float SoftEdgeRadius
  {
    get => this.m_softEdgeRadius;
    set
    {
      if ((double) value == 0.0)
        this.NoSoftEdges = true;
      this.m_softEdgeRadius = value;
    }
  }

  internal Dictionary<string, Stream> DocxProps
  {
    get
    {
      if (this.m_docxProps == null)
        this.m_docxProps = new Dictionary<string, Stream>();
      return this.m_docxProps;
    }
  }

  internal EffectFormat(Shape shape)
  {
    this.m_shape = (ShapeBase) shape;
    this.m_docxProps = new Dictionary<string, Stream>();
  }

  internal EffectFormat(ChildShape shape)
  {
    this.m_childShape = shape;
    this.m_docxProps = new Dictionary<string, Stream>();
  }

  internal EffectFormat(GroupShape shape)
  {
    this.m_groupShape = shape;
    this.m_docxProps = new Dictionary<string, Stream>();
  }

  internal void Close()
  {
    if (this.m_shadowFormat != null)
    {
      this.m_shadowFormat.Close();
      this.m_shadowFormat = (ShadowFormat) null;
    }
    if (this.m_glowFormat != null)
    {
      this.m_glowFormat.Close();
      this.m_glowFormat = (GlowFormat) null;
    }
    if (this.m_reflection != null)
    {
      this.m_reflection.Close();
      this.m_reflection = (Reflection) null;
    }
    if (this.m_threeDFormat != null)
    {
      this.m_threeDFormat.Close();
      this.m_threeDFormat = (ThreeDFormat) null;
    }
    if (this.m_docxProps.Count <= 0)
      return;
    this.m_docxProps.Clear();
    this.m_docxProps = (Dictionary<string, Stream>) null;
  }
}
