// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.TextRegion
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal class TextRegion
{
  private float m_y;
  private float m_height;

  public float Y
  {
    get => this.m_y;
    set => this.m_y = value;
  }

  public float Height
  {
    get => this.m_height;
    set
    {
      this.m_height = (double) value >= 0.0 ? value : throw new ArgumentOutOfRangeException(nameof (value), (object) value, "Height can not be less 0");
    }
  }

  public TextRegion()
  {
  }

  public TextRegion(float y, float height)
  {
    if ((double) height < 0.0)
      throw new ArgumentOutOfRangeException(nameof (height), (object) height, "Value can not be less 0");
    this.Y = y;
    this.Height = height;
  }

  public static TextRegion Union(TextRegion region1, TextRegion region2)
  {
    if (region1 == null)
      throw new ArgumentNullException(nameof (region1));
    if (region2 == null)
      throw new ArgumentNullException(nameof (region2));
    if (!region1.IntersectsWith(region2))
      throw new ArgumentException("The specified regions don't intersect");
    TextRegion textRegion = new TextRegion();
    float num1 = Math.Min(region1.Y, region2.Y);
    float num2 = Math.Max(region1.Y + region1.Height, region2.Y + region2.Height) - num1;
    textRegion.Y = num1;
    textRegion.Height = num2;
    return textRegion;
  }

  public bool IntersectsWith(TextRegion region)
  {
    if (region == null)
      throw new ArgumentNullException(nameof (region));
    return new RectangleF(0.0f, this.Y, 1f, this.Height).IntersectsWith(new RectangleF(0.0f, region.Y, 1f, region.Height));
  }
}
