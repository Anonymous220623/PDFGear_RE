// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ShadowFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ShadowFormat
{
  internal const byte ShadowOffsetXKey = 0;
  internal const byte ShadowOffsetYKey = 1;
  internal const byte ShadowOffset2XKey = 2;
  internal const byte ShadowOffset2YKey = 3;
  internal const byte OriginXKey = 4;
  internal const byte OriginYKey = 5;
  internal const byte ShadowPerspectiveMatrixKey = 6;
  internal const byte HorizontalSkewAngleKey = 7;
  internal const byte VerticalSkewAngleKey = 8;
  internal const byte HorizontalScalingFactorKey = 9;
  internal const byte VerticalScalingFactorKey = 10;
  internal const byte RotateWithShapeKey = 11;
  internal const byte DirectionKey = 12;
  internal const byte DistanceKey = 13;
  internal const byte BlurKey = 14;
  internal const byte AlignmentKey = 15;
  internal const byte ColorKey = 16 /*0x10*/;
  internal const byte Color2Key = 17;
  internal const byte NameKey = 18;
  internal const byte ShadowTypeKey = 19;
  internal const byte VisibleKey = 20;
  internal const byte TransparencyKey = 21;
  internal const byte ObscuredKey = 22;
  private float m_shadowOffsetX;
  private float m_nonchoiceshadowOffsetX;
  private float m_nonchoiceshadowOffsetY;
  private float m_shadowOffsetY;
  private float m_shadowOffset2X;
  private float m_shadowOffset2Y;
  private float m_originX;
  private float m_originY;
  private string m_shadowPerspectiveMatrix;
  private short m_horizontalSkewAngle;
  private short m_verticalSkewAngle;
  private double m_horizontalScalingFactor;
  private double m_verticalScalingFactor;
  private bool m_rotateWithShape;
  private double m_direction;
  private double m_distance;
  private double m_blurRadius;
  private ShadowAlignment m_align;
  private Color m_color;
  private Color m_backColor;
  private string m_name;
  private ShadowType m_shadowType;
  private bool m_visible;
  private float m_transparency;
  private bool m_obscured;
  private ShapeBase m_shape;
  private ushort m_flagsA;
  private byte m_flagsB;
  internal string m_type;

  internal float ShadowOffsetX
  {
    get => this.m_shadowOffsetX;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 65534 | 1);
      this.m_shadowOffsetX = value;
    }
  }

  internal float NonChoiceShadowOffsetX
  {
    get => this.m_nonchoiceshadowOffsetX;
    set => this.m_nonchoiceshadowOffsetX = value;
  }

  internal float ShadowOffsetY
  {
    get => this.m_shadowOffsetY;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 65533 | 2);
      this.m_shadowOffsetY = value;
    }
  }

  internal float NonChoiceShadowOffsetY
  {
    get => this.m_nonchoiceshadowOffsetY;
    set => this.m_nonchoiceshadowOffsetY = value;
  }

  internal float ShadowOffset2X
  {
    get => this.m_shadowOffset2X;
    set
    {
      this.m_flagsA = (ushort) (byte) ((int) this.m_flagsA & 65531 | 4);
      this.m_shadowOffset2X = value;
    }
  }

  internal float ShadowOffset2Y
  {
    get => this.m_shadowOffset2Y;
    set
    {
      this.m_flagsA = (ushort) (byte) ((int) this.m_flagsA & 65527 | 8);
      this.m_shadowOffset2Y = value;
    }
  }

  internal float OriginX
  {
    get => this.m_originX;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 65519 | 16 /*0x10*/);
      this.m_originX = value;
    }
  }

  internal float OriginY
  {
    get => this.m_originY;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 65503 | 32 /*0x20*/);
      this.m_originY = value;
    }
  }

  internal string ShadowPerspectiveMatrix
  {
    get => this.m_shadowPerspectiveMatrix;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 65471 | 64 /*0x40*/);
      this.m_shadowPerspectiveMatrix = value;
    }
  }

  internal short HorizontalSkewAngle
  {
    get => this.m_horizontalSkewAngle;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 65407 | 128 /*0x80*/);
      this.m_horizontalSkewAngle = value;
    }
  }

  internal short VerticalSkewAngle
  {
    get => this.m_verticalSkewAngle;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 65279 | 256 /*0x0100*/);
      this.m_verticalSkewAngle = value;
    }
  }

  internal double HorizontalScalingFactor
  {
    get => this.m_horizontalScalingFactor;
    set
    {
      if (value < -5400000.0 || value > 5400000.0)
        throw new ArgumentOutOfRangeException("Horizontal Skew angle must be between -90 and 90 degrees");
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 65023 | 512 /*0x0200*/);
      this.m_horizontalScalingFactor = value;
    }
  }

  internal double VerticalScalingFactor
  {
    get => this.m_verticalScalingFactor;
    set
    {
      if (value < -5400000.0 || value > 5400000.0)
        throw new ArgumentOutOfRangeException("Vertical Skew angle must be between -90 and 90 degrees");
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 64511 | 1024 /*0x0400*/);
      this.m_verticalScalingFactor = value;
    }
  }

  internal bool RotateWithShape
  {
    get => this.m_rotateWithShape;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 63487 | 2048 /*0x0800*/);
      this.m_rotateWithShape = value;
    }
  }

  internal double Direction
  {
    get => this.m_direction;
    set
    {
      if (value < 0.0 || value > 21600000.0)
        throw new ArgumentOutOfRangeException("Angle must be between 0 and 360");
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 61439 /*0xEFFF*/ | 4096 /*0x1000*/);
      this.m_direction = value;
    }
  }

  internal double Distance
  {
    get => this.m_distance;
    set
    {
      if (value < 0.0 || value > (double) int.MaxValue)
        throw new ArgumentOutOfRangeException("Distance must be between 0 and 100");
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 57343 /*0xDFFF*/ | 8192 /*0x2000*/);
      this.m_distance = value;
    }
  }

  internal double Blur
  {
    get => this.m_blurRadius;
    set
    {
      if (value < 0.0 || value > (double) int.MaxValue)
        throw new ArgumentOutOfRangeException("Blur radius value must be between 0 and 100");
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 49151 /*0xBFFF*/ | 16384 /*0x4000*/);
      this.m_blurRadius = value;
    }
  }

  internal ShadowAlignment Alignment
  {
    get => this.m_align;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & (int) short.MaxValue | 32768 /*0x8000*/);
      this.m_align = value;
    }
  }

  internal Color Color
  {
    get => this.m_color;
    set
    {
      this.m_flagsB = (byte) ((int) this.m_flagsB & 254 | 1);
      this.m_color = value;
    }
  }

  internal Color Color2
  {
    get => this.m_backColor;
    set
    {
      this.m_flagsB = (byte) ((int) this.m_flagsB & 253 | 2);
      this.m_backColor = value;
    }
  }

  internal string Name
  {
    get => this.m_name;
    set
    {
      this.m_flagsB = (byte) ((int) this.m_flagsB & 251 | 4);
      this.m_name = value;
    }
  }

  internal ShadowType ShadowType
  {
    get => this.m_shadowType;
    set
    {
      this.m_flagsB = (byte) ((int) this.m_flagsB & 247 | 8);
      this.m_shadowType = value;
    }
  }

  internal bool Visible
  {
    get => this.m_visible;
    set
    {
      this.m_flagsB = (byte) ((int) this.m_flagsB & 239 | 16 /*0x10*/);
      this.m_visible = value;
    }
  }

  internal float Transparency
  {
    get => this.m_transparency;
    set
    {
      this.m_flagsB = (byte) ((int) this.m_flagsB & 223 | 32 /*0x20*/);
      this.m_transparency = value;
    }
  }

  internal bool Obscured
  {
    get => this.m_obscured;
    set
    {
      this.m_flagsB = (byte) ((int) this.m_flagsB & 191 | 64 /*0x40*/);
      this.m_obscured = value;
    }
  }

  internal ShadowFormat(ShapeBase shape)
  {
    this.m_shape = shape;
    this.m_color = Color.White;
    this.m_shadowOffsetX = 25400f;
    this.m_shadowOffsetY = 25400f;
    this.m_shadowOffset2X = -25400f;
    this.m_shadowOffset2Y = -25400f;
    this.m_visible = true;
    this.m_horizontalScalingFactor = 100.0;
    this.m_verticalScalingFactor = 100.0;
  }

  internal void Close()
  {
    if (this.m_shape == null)
      return;
    this.m_shape = (ShapeBase) null;
  }

  internal bool HasKey(int propertyKey)
  {
    return propertyKey < 16 /*0x10*/ ? ((int) this.m_flagsA & (int) (ushort) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0 : ((int) this.m_flagsB & (int) (byte) Math.Pow(2.0, (double) (propertyKey - 16 /*0x10*/))) >> propertyKey - 16 /*0x10*/ != 0;
  }

  internal ShadowFormat Clone() => (ShadowFormat) this.MemberwiseClone();
}
