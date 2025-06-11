// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.GradientStopImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

public class GradientStopImpl
{
  internal const int Size = 12;
  private ColorObject m_color;
  private int m_iPosiiton;
  private int m_iTransparency;
  private int m_iTint = -1;
  private int m_iShade = -1;
  private ShapeFillImpl m_parentFill;

  internal ShapeFillImpl ParentFill
  {
    get => this.m_parentFill;
    set => this.m_parentFill = value;
  }

  public ColorObject ColorObject => this.m_color;

  public int Position
  {
    get => this.m_iPosiiton;
    set
    {
      this.m_iPosiiton = value;
      if (this.m_parentFill == null)
        return;
      this.m_parentFill.ChangeVisible();
    }
  }

  public int Transparency
  {
    get => this.m_iTransparency;
    set
    {
      this.m_iTransparency = value;
      if (this.m_parentFill == null)
        return;
      this.m_parentFill.ChangeVisible();
    }
  }

  public int Tint
  {
    get => this.m_iTint;
    set
    {
      this.m_iTint = value;
      if (this.m_parentFill == null)
        return;
      this.m_parentFill.ChangeVisible();
    }
  }

  public int Shade
  {
    get => this.m_iShade;
    set
    {
      this.m_iShade = value;
      if (this.m_parentFill == null)
        return;
      this.m_parentFill.ChangeVisible();
    }
  }

  public GradientStopImpl(ColorObject color, int position, int transparency)
    : this(color, position, transparency, -1, -1)
  {
  }

  public GradientStopImpl(ColorObject color, int position, int transparency, int tint, int shade)
  {
    this.m_color = color;
    this.m_iPosiiton = position;
    this.m_iTransparency = transparency;
    this.m_iTint = tint;
    this.m_iShade = shade;
  }

  public GradientStopImpl(byte[] data, int offset)
  {
    this.m_iPosiiton = data != null ? BitConverter.ToInt32(data, offset) : throw new ArgumentNullException(nameof (data));
    offset += 4;
    this.m_color = new ColorObject(ColorExtension.FromArgb(BitConverter.ToInt32(data, offset)));
    offset += 4;
    this.m_iTransparency = BitConverter.ToInt32(data, offset);
  }

  internal void Serialize(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    byte[] bytes1 = BitConverter.GetBytes(this.m_iPosiiton);
    stream.Write(bytes1, 0, bytes1.Length);
    byte[] bytes2 = BitConverter.GetBytes(this.m_color.Value);
    stream.Write(bytes2, 0, bytes2.Length);
    byte[] bytes3 = BitConverter.GetBytes(this.m_iTransparency);
    stream.Write(bytes3, 0, bytes3.Length);
  }

  internal GradientStopImpl Clone()
  {
    GradientStopImpl gradientStopImpl = (GradientStopImpl) this.MemberwiseClone();
    gradientStopImpl.m_color = new ColorObject(ExcelKnownColors.None);
    gradientStopImpl.m_color.CopyFrom(this.m_color, false);
    return gradientStopImpl;
  }

  internal bool EqualsWithoutTransparency(GradientStopImpl stop)
  {
    return stop != null && stop.m_color == this.m_color && stop.m_iPosiiton == this.m_iPosiiton && stop.m_iShade == this.m_iShade && stop.m_iTint == this.m_iTint;
  }

  internal void Dispose()
  {
    this.m_color.Dispose();
    this.m_color = (ColorObject) null;
  }
}
