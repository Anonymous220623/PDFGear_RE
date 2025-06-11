// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Reflection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class Reflection
{
  private const byte BlurRadiusKey = 0;
  private const byte DirectionKey = 1;
  private const byte DistanceKey = 2;
  private const byte TranparencyKey = 3;
  private const byte StartPositionKey = 4;
  private const byte EndOpacityKey = 5;
  private const byte SizeKey = 6;
  private const byte FadeDirectionKey = 7;
  private const byte RotateWithShapekey = 8;
  private const byte HorizontalSkewkey = 9;
  private const byte VerticalSkewKey = 10;
  private const byte HorizontalRatioKey = 11;
  private const byte VerticalRatioKey = 12;
  private const byte AlignmentKey = 13;
  private float m_blurRadius;
  private int m_direction;
  private float m_distance;
  private float m_startOpacity;
  private float m_startPosition;
  private float m_endOpacity;
  private float m_endPosition;
  private int m_fadeDirection;
  private bool m_rotWithShape;
  private int m_horizontalSkew;
  private int m_verticalSkew;
  private float m_horizontalRatio;
  private float m_verticalRatio;
  private TextureAlignment m_refAlign;
  private ushort m_flag;
  private ShapeBase m_shape;

  internal float Blur
  {
    get => this.m_blurRadius;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65534 | 1);
      this.m_blurRadius = value;
    }
  }

  internal int Direction
  {
    get => this.m_direction;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65533 | 2);
      this.m_direction = value;
    }
  }

  internal float Offset
  {
    get => this.m_distance;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65531 | 4);
      this.m_distance = value;
    }
  }

  internal float Transparency
  {
    get => this.m_startOpacity;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65527 | 8);
      this.m_startOpacity = value;
    }
  }

  internal float StartPosition
  {
    get => this.m_startPosition;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65519 | 16 /*0x10*/);
      this.m_startPosition = value;
    }
  }

  internal float EndOpacity
  {
    get => this.m_endOpacity;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65503 | 32 /*0x20*/);
      this.m_endOpacity = value;
    }
  }

  internal float Size
  {
    get => this.m_endPosition;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65471 | 64 /*0x40*/);
      this.m_endPosition = value;
    }
  }

  internal int FadeDirection
  {
    get => this.m_fadeDirection;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65407 | 128 /*0x80*/);
      this.m_fadeDirection = value;
    }
  }

  internal int HorizontalSkew
  {
    get => this.m_horizontalSkew;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65279 | 256 /*0x0100*/);
      this.m_horizontalSkew = value;
    }
  }

  internal int VerticalSkew
  {
    get => this.m_verticalSkew;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65023 | 512 /*0x0200*/);
      this.m_verticalSkew = value;
    }
  }

  internal float HorizontalRatio
  {
    get => this.m_horizontalRatio;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 64511 | 1024 /*0x0400*/);
      this.m_horizontalRatio = value;
    }
  }

  internal float VerticalRatio
  {
    get => this.m_verticalRatio;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 63487 | 2048 /*0x0800*/);
      this.m_verticalRatio = value;
    }
  }

  internal bool RotateWithShape
  {
    get => this.m_rotWithShape;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 61439 /*0xEFFF*/ | 4096 /*0x1000*/);
      this.m_rotWithShape = value;
    }
  }

  internal TextureAlignment Alignment
  {
    get => this.m_refAlign;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 57343 /*0xDFFF*/ | 8192 /*0x2000*/);
      this.m_refAlign = value;
    }
  }

  internal Reflection(ShapeBase shape) => this.m_shape = shape;

  internal bool HasKey(int propertyKey)
  {
    return ((int) this.m_flag & (int) (ushort) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0;
  }

  internal void Close()
  {
    if (this.m_shape == null)
      return;
    this.m_shape = (ShapeBase) null;
  }

  internal Reflection Clone() => (Reflection) this.MemberwiseClone();
}
