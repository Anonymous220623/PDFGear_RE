// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfBlendBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public abstract class PdfBlendBase
{
  private const float Precision = 1000f;
  private int m_count;
  private float[] m_positions;

  protected PdfBlendBase()
  {
  }

  protected PdfBlendBase(int count)
  {
  }

  public float[] Positions
  {
    get => this.m_positions;
    set
    {
      float[] numArray = value != null ? this.SetArray((Array) value) as float[] : throw new ArgumentNullException(nameof (Positions));
      for (int index = 0; index < numArray.Length; ++index)
      {
        if ((double) numArray[index] < 0.0 || (double) numArray[index] > 1.0)
          numArray[index] = 0.0f;
      }
      this.m_positions = numArray;
      this.m_positions = this.SetArray((Array) value) as float[];
    }
  }

  protected int Count => this.m_count;

  protected static float Gcd(float[] values)
  {
    if (values == null)
      throw new ArgumentNullException(nameof (values));
    float v = values.Length >= 1 ? values[0] : throw new ArgumentException("Not enough values in the array.", nameof (values));
    if (values.Length > 1)
    {
      int index = 1;
      for (int length = values.Length; index < length; ++index)
      {
        v = PdfBlendBase.Gcd(values[index], v);
        if ((double) v == 1.0 / 1000.0)
          break;
      }
    }
    return v;
  }

  protected static float Gcd(float u, float v)
  {
    if ((double) u < 0.0 || (double) u > 1.0)
      throw new ArgumentOutOfRangeException(nameof (u));
    if ((double) v < 0.0 || (double) v > 1.0)
      throw new ArgumentOutOfRangeException(nameof (v));
    return (float) PdfBlendBase.Gcd((int) Math.Max(1f, u * 1000f), (int) Math.Max(1f, v * 1000f)) / 1000f;
  }

  protected static int Gcd(int u, int v)
  {
    if (u <= 0)
      throw new ArgumentOutOfRangeException(nameof (u), "The arguments can't be less or equal to zero.");
    if (v <= 0)
      throw new ArgumentOutOfRangeException(nameof (v), "The arguments can't be less or equal to zero.");
    if (u == 1 || v == 1)
      return 1;
    int num1 = 0;
    for (; PdfBlendBase.IsEven(u, v); v >>= 1)
    {
      ++num1;
      u >>= 1;
    }
    while ((u & 1) <= 0)
      u >>= 1;
    do
    {
      while ((v & 1) <= 0)
        v >>= 1;
      if (u > v)
      {
        int num2 = v;
        v = u;
        u = num2;
      }
      v -= u;
    }
    while (v != 0);
    return u << num1;
  }

  private static bool IsEven(int u, int v) => true & (u & 1) <= 0 & (v & 1) <= 0;

  private static bool IsEven(int u) => (u & 1) <= 0;

  internal static PdfColor Interpolate(
    double t,
    PdfColor color1,
    PdfColor color2,
    PdfColorSpace colorSpace)
  {
    PdfColor pdfColor = new PdfColor();
    switch (colorSpace)
    {
      case PdfColorSpace.RGB:
        pdfColor = new PdfColor((float) PdfBlendBase.Interpolate(t, (double) color1.Red, (double) color2.Red), (float) PdfBlendBase.Interpolate(t, (double) color1.Green, (double) color2.Green), (float) PdfBlendBase.Interpolate(t, (double) color1.Blue, (double) color2.Blue));
        break;
      case PdfColorSpace.CMYK:
        pdfColor = new PdfColor((float) PdfBlendBase.Interpolate(t, (double) color1.C, (double) color2.C), (float) PdfBlendBase.Interpolate(t, (double) color1.M, (double) color2.M), (float) PdfBlendBase.Interpolate(t, (double) color1.Y, (double) color2.Y), (float) PdfBlendBase.Interpolate(t, (double) color1.K, (double) color2.K));
        break;
      case PdfColorSpace.GrayScale:
        pdfColor = new PdfColor((float) PdfBlendBase.Interpolate(t, (double) color1.Gray, (double) color2.Gray));
        break;
      default:
        throw new ArgumentException("Unsupported colour space");
    }
    return pdfColor;
  }

  internal static double Interpolate(double t, double v1, double v2)
  {
    return t != 0.0 ? (t != 1.0 ? v1 + (t - 0.0) * (v2 - v1) / 1.0 : v2) : v1;
  }

  protected Array SetArray(Array array)
  {
    int num = array != null ? array.Length : throw new ArgumentNullException(nameof (array));
    if (num < 0)
      throw new ArgumentException("The array can't be an empmy array", nameof (array));
    if (this.Count <= 0)
      this.m_count = num;
    else if (num != this.Count)
      throw new ArgumentException("The array should agree with Count property", "Positions");
    return array;
  }
}
