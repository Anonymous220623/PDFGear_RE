// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OTableCellProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OTableCellProperties : BorderProperties
{
  internal const byte RotationAngleKey = 0;
  internal const byte WrapKey = 1;
  internal const byte BorderLineWidthKey = 2;
  internal const byte BorderLineWidthTopKey = 3;
  internal const byte BorderLineWidthBottomKey = 4;
  internal const byte BorderLineWidthLeftKey = 5;
  internal const byte BorderLineWidthRightKey = 6;
  internal const byte ShrinkToFitKey = 7;
  internal const byte BackColorKey = 8;
  internal const byte VerticalAlignKey = 9;
  internal const byte PaddingRightKey = 10;
  internal const byte paddingLeftKey = 11;
  internal const byte PaddingBottomKey = 12;
  internal const byte PaddingTopKey = 13;
  internal const byte RepeatContentKey = 14;
  internal const byte DirectionKey = 15;
  private int m_rotationAngle;
  private bool m_wrap;
  private float m_borderLineWidth;
  private float m_borderLineWidthTop;
  private float m_borderLineWidthBottom;
  private float m_borderLineWidthLeft;
  private float m_borderLineWidthRight;
  private bool m_shrinkToFit;
  private Syncfusion.DocIO.ODF.Base.VerticalAlign? m_verticalAlign;
  private Color m_backColor;
  private float m_paddingTop;
  private float m_paddingBottom;
  private float m_paddingLeft;
  private float m_paddingRight;
  private bool m_repeatContent;
  private PageOrder m_direction;
  internal ushort tableCellFlags;

  internal int RotationAngle
  {
    get => this.m_rotationAngle;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 65534 | 1);
      this.m_rotationAngle = value;
    }
  }

  internal bool Wrap
  {
    get => this.m_wrap;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 65533 | 2);
      this.m_wrap = value;
    }
  }

  internal float BorderLineWidth
  {
    get => this.m_borderLineWidth;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 65531 | 4);
      this.m_borderLineWidth = value;
    }
  }

  internal float BorderLineWidthTop
  {
    get => this.m_borderLineWidthTop;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 65527 | 8);
      this.m_borderLineWidthTop = value;
    }
  }

  internal float BorderLineWidthBottom
  {
    get => this.m_borderLineWidthBottom;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 65519 | 16 /*0x10*/);
      this.m_borderLineWidthBottom = value;
    }
  }

  internal float BorderLineWidthLeft
  {
    get => this.m_borderLineWidthLeft;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 65503 | 32 /*0x20*/);
      this.m_borderLineWidthLeft = value;
    }
  }

  internal float BorderLineWidthRight
  {
    get => this.m_borderLineWidthRight;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 65471 | 64 /*0x40*/);
      this.m_borderLineWidthRight = value;
    }
  }

  internal bool ShrinkToFit
  {
    get => this.m_shrinkToFit;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 65407 | 128 /*0x80*/);
      this.m_shrinkToFit = value;
    }
  }

  internal Color BackColor
  {
    get => this.m_backColor;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 65279 | 256 /*0x0100*/);
      this.m_backColor = value;
    }
  }

  internal Syncfusion.DocIO.ODF.Base.VerticalAlign? VerticalAlign
  {
    get => this.m_verticalAlign;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 65023 | 512 /*0x0200*/);
      this.m_verticalAlign = value;
    }
  }

  internal float PaddingRight
  {
    get => this.m_paddingRight;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 64511 | 1024 /*0x0400*/);
      this.m_paddingRight = value;
    }
  }

  internal float PaddingLeft
  {
    get => this.m_paddingLeft;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 63487 | 2048 /*0x0800*/);
      this.m_paddingLeft = value;
    }
  }

  internal float PaddingBottom
  {
    get => this.m_paddingBottom;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 61439 /*0xEFFF*/ | 4096 /*0x1000*/);
      this.m_paddingBottom = value;
    }
  }

  internal float PaddingTop
  {
    get => this.m_paddingTop;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 57343 /*0xDFFF*/ | 8192 /*0x2000*/);
      this.m_paddingTop = value;
    }
  }

  internal bool RepeatContent
  {
    get => this.m_repeatContent;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & 49151 /*0xBFFF*/ | 16384 /*0x4000*/);
      this.m_repeatContent = value;
    }
  }

  internal PageOrder Direction
  {
    get => this.m_direction;
    set
    {
      this.tableCellFlags = (ushort) ((int) this.tableCellFlags & (int) short.MaxValue | 32768 /*0x8000*/);
      this.m_direction = value;
    }
  }

  internal bool HasKey(int propertyKey, int flagname)
  {
    return (flagname & (int) (ushort) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0;
  }

  public override bool Equals(object obj)
  {
    OTableCellProperties otableCellProperties = obj as OTableCellProperties;
    bool flag = false;
    if (otableCellProperties == null)
      return false;
    if (this.HasKey(0, (int) this.tableCellFlags) && otableCellProperties.HasKey(0, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.RotationAngle.Equals(otableCellProperties.RotationAngle);
      if (!flag)
        return flag;
    }
    if (this.HasKey(1, (int) this.tableCellFlags) && otableCellProperties.HasKey(1, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.Wrap.Equals(otableCellProperties.Wrap);
      if (!flag)
        return flag;
    }
    if (this.HasKey(9, (int) this.tableCellFlags) && otableCellProperties.HasKey(9, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.VerticalAlign.Equals((object) otableCellProperties.VerticalAlign);
      if (!flag)
        return flag;
    }
    if (this.HasKey(8, (int) this.tableCellFlags) && otableCellProperties.HasKey(8, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.BackColor.Equals((object) otableCellProperties.BackColor);
      if (!flag)
        return flag;
    }
    if (this.HasKey(7, (int) this.tableCellFlags) && otableCellProperties.HasKey(7, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.BackColor.Equals((object) otableCellProperties.BackColor);
      if (!flag)
        return flag;
    }
    if (this.HasKey(0, (int) this.borderFlags) && otableCellProperties.HasKey(0, (int) otableCellProperties.borderFlags))
    {
      flag = this.Border.Equals((object) otableCellProperties.Border);
      if (!flag)
        return flag;
    }
    if (this.HasKey(1, (int) this.borderFlags) && otableCellProperties.HasKey(1, (int) otableCellProperties.borderFlags))
    {
      flag = this.BorderTop.Equals((object) otableCellProperties.BorderTop);
      if (!flag)
        return flag;
    }
    if (this.HasKey(2, (int) this.borderFlags) && otableCellProperties.HasKey(2, (int) otableCellProperties.borderFlags))
    {
      flag = this.BorderBottom.Equals((object) otableCellProperties.BorderBottom);
      if (!flag)
        return flag;
    }
    if (this.HasKey(3, (int) this.borderFlags) && otableCellProperties.HasKey(3, (int) otableCellProperties.borderFlags))
    {
      flag = this.BorderLeft.Equals((object) otableCellProperties.BorderLeft);
      if (!flag)
        return flag;
    }
    if (this.HasKey(4, (int) this.borderFlags) && otableCellProperties.HasKey(4, (int) otableCellProperties.borderFlags))
    {
      flag = this.BorderRight.Equals((object) otableCellProperties.BorderRight);
      if (!flag)
        return flag;
    }
    if (this.HasKey(11, (int) this.tableCellFlags) && otableCellProperties.HasKey(11, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.PaddingLeft.Equals(otableCellProperties.PaddingLeft);
      if (!flag)
        return flag;
    }
    if (this.HasKey(10, (int) this.tableCellFlags) && otableCellProperties.HasKey(10, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.PaddingRight.Equals(otableCellProperties.PaddingRight);
      if (!flag)
        return flag;
    }
    if (this.HasKey(13, (int) this.tableCellFlags) && otableCellProperties.HasKey(13, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.PaddingTop.Equals(otableCellProperties.PaddingTop);
      if (!flag)
        return flag;
    }
    if (this.HasKey(12, (int) this.tableCellFlags) && otableCellProperties.HasKey(12, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.PaddingBottom.Equals(otableCellProperties.PaddingBottom);
      if (!flag)
        return flag;
    }
    if (this.HasKey(14, (int) this.tableCellFlags) && otableCellProperties.HasKey(14, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.RepeatContent.Equals(otableCellProperties.RepeatContent);
      if (!flag)
        return flag;
    }
    if (this.HasKey(15, (int) this.tableCellFlags) && otableCellProperties.HasKey(15, (int) otableCellProperties.tableCellFlags))
    {
      flag = this.Direction.Equals((object) otableCellProperties.Direction);
      if (!flag)
        return flag;
    }
    return flag;
  }
}
