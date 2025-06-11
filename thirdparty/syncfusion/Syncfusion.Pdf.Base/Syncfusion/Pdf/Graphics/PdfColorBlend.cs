// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfColorBlend
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Functions;
using Syncfusion.Pdf.Primitives;
using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public sealed class PdfColorBlend : PdfBlendBase
{
  private PdfColor[] m_colors;
  private PdfBrush m_brush;

  public PdfColorBlend()
  {
  }

  public PdfColorBlend(int count)
    : base(count)
  {
  }

  internal PdfColorBlend(PdfBrush brush) => this.m_brush = brush;

  public PdfColor[] Colors
  {
    get => this.m_colors;
    set
    {
      this.m_colors = value != null ? this.SetArray((Array) value) as PdfColor[] : throw new ArgumentNullException(nameof (Colors));
    }
  }

  internal PdfFunction GetFunction(PdfColorSpace colorSpace)
  {
    float[] domain = new float[2]{ 0.0f, 1f };
    int colorComponentsCount = PdfColorBlend.GetColorComponentsCount(colorSpace);
    int maxComponentValue = this.GetMaxComponentValue(colorSpace);
    float[] numArray = PdfColorBlend.SetRange(colorComponentsCount, (float) maxComponentValue);
    PdfSampledFunction function1 = (PdfSampledFunction) null;
    if (this.m_brush == null)
    {
      int[] sizes = new int[1];
      float step = 1f;
      int sampleCount;
      if (this.Positions.Length == 2)
      {
        sampleCount = 2;
      }
      else
      {
        float num = PdfBlendBase.Gcd(this.GetIntervals(this.Positions));
        step = num;
        sampleCount = (int) (1.0 / (double) num) + 1;
      }
      sizes[0] = sampleCount;
      byte[] samplesValues = this.GetSamplesValues(colorSpace, sampleCount, maxComponentValue, step);
      return (PdfFunction) new PdfSampledFunction(domain, numArray, sizes, samplesValues);
    }
    if (this.m_brush is PdfLinearGradientBrush || this.m_brush is PdfRadialGradientBrush)
    {
      PdfLinearGradientBrush brush1 = this.m_brush as PdfLinearGradientBrush;
      PdfRadialGradientBrush brush2 = this.m_brush as PdfRadialGradientBrush;
      if (brush1 != null && brush1.Extend == PdfExtend.Both || brush2 != null)
      {
        PdfStitchingFunction function2 = new PdfStitchingFunction();
        PdfArray pdfArray = new PdfArray();
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        for (int index = 1; index < this.Positions.Length; ++index)
        {
          PdfExponentialInterpolationFunction wrapper = new PdfExponentialInterpolationFunction(true);
          wrapper.Domain = new PdfArray(new float[2]
          {
            0.0f,
            1f
          });
          wrapper.Range = new PdfArray(numArray);
          float[] array1 = new float[3]
          {
            this.Colors[index - 1].Red,
            this.Colors[index - 1].Green,
            this.Colors[index - 1].Blue
          };
          float[] array2 = new float[3]
          {
            this.Colors[index].Red,
            this.Colors[index].Green,
            this.Colors[index].Blue
          };
          wrapper.Dictionary["FunctionType"] = (IPdfPrimitive) new PdfNumber(2);
          wrapper.Dictionary["N"] = (IPdfPrimitive) new PdfNumber(1);
          wrapper.Dictionary["C0"] = (IPdfPrimitive) new PdfArray(array1);
          wrapper.Dictionary["C1"] = (IPdfPrimitive) new PdfArray(array2);
          if (index > 1)
          {
            stringBuilder1.Append(' ');
            stringBuilder2.Append(' ');
          }
          if (index < this.Positions.Length - 1)
            stringBuilder1.Append(this.Positions[index]);
          if (brush1 != null)
            stringBuilder2.Append("0 1");
          else if (brush2 != null)
            stringBuilder2.Append("1 0");
          PdfReferenceHolder element = new PdfReferenceHolder((IPdfWrapper) wrapper);
          pdfArray.Add((IPdfPrimitive) element);
        }
        float[] array3 = new float[stringBuilder2.ToString().Split(new char[1]
        {
          ' '
        }, StringSplitOptions.RemoveEmptyEntries).Length];
        float[] array4 = new float[stringBuilder1.ToString().Split(new char[1]
        {
          ' '
        }, StringSplitOptions.RemoveEmptyEntries).Length];
        for (int index = 0; index < array3.Length; ++index)
          array3[index] = float.Parse(stringBuilder2.ToString().Split(new char[1]
          {
            ' '
          }, StringSplitOptions.RemoveEmptyEntries)[index]);
        for (int index = 0; index < array4.Length; ++index)
          array4[index] = float.Parse(stringBuilder1.ToString().Split(new char[1]
          {
            ' '
          }, StringSplitOptions.RemoveEmptyEntries)[index]);
        function2.Dictionary["Bounds"] = (IPdfPrimitive) new PdfArray(array4);
        function2.Dictionary["Encode"] = (IPdfPrimitive) new PdfArray(array3);
        if (brush2 != null)
          function2.Range = new PdfArray(numArray);
        function2.Domain = new PdfArray(new float[2]
        {
          0.0f,
          1f
        });
        function2.Dictionary["Functions"] = (IPdfPrimitive) pdfArray;
        function2.Dictionary["FunctionType"] = (IPdfPrimitive) new PdfNumber(3);
        return (PdfFunction) function2;
      }
      if (brush1 != null)
        brush1.Extend = PdfExtend.Both;
    }
    return (PdfFunction) function1;
  }

  internal PdfColorBlend CloneColorBlend()
  {
    PdfColorBlend pdfColorBlend = this.MemberwiseClone() as PdfColorBlend;
    if (this.m_colors != null)
      pdfColorBlend.Colors = this.m_colors.Clone() as PdfColor[];
    if (this.Positions != null)
      pdfColorBlend.Positions = this.Positions.Clone() as float[];
    return pdfColorBlend;
  }

  private static float[] SetRange(int colourComponents, float maxValue)
  {
    float[] numArray = new float[colourComponents * 2];
    for (int index = 0; index < colourComponents; ++index)
    {
      numArray[index * 2] = 0.0f;
      numArray[index * 2 + 1] = 1f;
    }
    return numArray;
  }

  private static int GetColorComponentsCount(PdfColorSpace colorSpace)
  {
    switch (colorSpace)
    {
      case PdfColorSpace.RGB:
        return 3;
      case PdfColorSpace.CMYK:
        return 4;
      case PdfColorSpace.GrayScale:
        return 1;
      default:
        throw new ArgumentException("Unsupported color space: " + (object) colorSpace, nameof (colorSpace));
    }
  }

  private byte[] GetSamplesValues(
    PdfColorSpace colorSpace,
    int sampleCount,
    int maxComponentValue,
    float step)
  {
    switch (colorSpace)
    {
      case PdfColorSpace.RGB:
        return this.GetRgbSamples(sampleCount, maxComponentValue, step);
      case PdfColorSpace.CMYK:
        return this.GetCmykSamples(sampleCount, maxComponentValue, step);
      case PdfColorSpace.GrayScale:
        return this.GetGrayscaleSamples(sampleCount, maxComponentValue, step);
      default:
        throw new ArgumentException("Unsupported color space: " + (object) colorSpace, nameof (colorSpace));
    }
  }

  private byte[] GetGrayscaleSamples(int sampleCount, int maxComponentValue, float step)
  {
    byte[] grayscaleSamples = new byte[sampleCount * 2];
    for (int index1 = 0; index1 < sampleCount; ++index1)
    {
      PdfColor nextColor = this.GetNextColor(index1, step, PdfColorSpace.GrayScale);
      int index2 = index1 * 2;
      byte[] bytes = BitConverter.GetBytes((short) ((double) nextColor.Gray * (double) maxComponentValue));
      grayscaleSamples[index2] = bytes[0];
      grayscaleSamples[index2 + 1] = bytes[1];
    }
    return grayscaleSamples;
  }

  private byte[] GetCmykSamples(int sampleCount, int maxComponentValue, float step)
  {
    byte[] cmykSamples = new byte[sampleCount * 4];
    for (int index1 = 0; index1 < sampleCount; ++index1)
    {
      PdfColor nextColor = this.GetNextColor(index1, step, PdfColorSpace.CMYK);
      int index2 = index1 * 4;
      cmykSamples[index2] = (byte) ((double) nextColor.C * (double) maxComponentValue);
      cmykSamples[index2 + 1] = (byte) ((double) nextColor.M * (double) maxComponentValue);
      cmykSamples[index2 + 2] = (byte) ((double) nextColor.Y * (double) maxComponentValue);
      cmykSamples[index2 + 3] = (byte) ((double) nextColor.K * (double) maxComponentValue);
    }
    return cmykSamples;
  }

  private byte[] GetRgbSamples(int sampleCount, int maxComponentValue, float step)
  {
    byte[] rgbSamples = new byte[sampleCount * 3];
    for (int index1 = 0; index1 < sampleCount; ++index1)
    {
      PdfColor nextColor = this.GetNextColor(index1, step, PdfColorSpace.RGB);
      int index2 = index1 * 3;
      rgbSamples[index2] = nextColor.R;
      rgbSamples[index2 + 1] = nextColor.G;
      rgbSamples[index2 + 2] = nextColor.B;
    }
    return rgbSamples;
  }

  private PdfColor GetNextColor(int index, float step, PdfColorSpace colorSpace)
  {
    float position1 = step * (float) index;
    int indexLow;
    int indexHi;
    this.GetIndices(position1, out indexLow, out indexHi);
    PdfColor nextColor;
    if (indexLow == indexHi)
    {
      nextColor = this.m_colors[indexLow];
    }
    else
    {
      float position2 = this.Positions[indexLow];
      float position3 = this.Positions[indexHi];
      PdfColor color1 = this.m_colors[indexLow];
      PdfColor color2 = this.m_colors[indexHi];
      nextColor = PdfBlendBase.Interpolate(((double) position1 - (double) position2) / ((double) position3 - (double) position2), color1, color2, colorSpace);
    }
    return nextColor;
  }

  private void GetIndices(float position, out int indexLow, out int indexHi)
  {
    float[] positions = this.Positions;
    indexLow = 0;
    indexHi = 0;
    for (int index = 0; index < this.m_colors.Length; ++index)
    {
      float num = positions[index];
      if ((double) num == (double) position)
      {
        indexLow = indexHi = index;
        break;
      }
      if ((double) num > (double) position)
      {
        indexHi = index;
        break;
      }
      indexLow = index;
      indexHi = index;
    }
  }

  private int GetMaxComponentValue(PdfColorSpace colorSpace)
  {
    switch (colorSpace)
    {
      case PdfColorSpace.RGB:
      case PdfColorSpace.CMYK:
        return (int) byte.MaxValue;
      case PdfColorSpace.GrayScale:
        return (int) ushort.MaxValue;
      default:
        throw new ArgumentException("Unsupported color space: " + (object) colorSpace, nameof (colorSpace));
    }
  }

  private float[] GetIntervals(float[] positions)
  {
    int length = positions.Length;
    float[] intervals = new float[length - 1];
    float num = positions[0];
    for (int index = 1; index < length; ++index)
    {
      float position = positions[index];
      intervals[index - 1] = position - num;
      num = position;
    }
    return intervals;
  }
}
