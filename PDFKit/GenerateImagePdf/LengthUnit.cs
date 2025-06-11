// Decompiled with JetBrains decompiler
// Type: PDFKit.GenerateImagePdf.LengthUnit
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

#nullable disable
namespace PDFKit.GenerateImagePdf;

public struct LengthUnit
{
  private float centimeter;
  private float inch;
  private float pixel;

  public float Centimeter => this.centimeter;

  public float Inch => this.inch;

  public float Pixel => this.pixel;

  public static LengthUnit FromCentimeter(float centimeter)
  {
    return new LengthUnit()
    {
      centimeter = centimeter,
      inch = centimeter / 2.54f,
      pixel = (float) ((double) centimeter / 2.5399999618530273 * 72.0)
    };
  }

  public static LengthUnit FromInch(float inch)
  {
    return new LengthUnit()
    {
      centimeter = inch * 2.54f,
      inch = inch,
      pixel = inch * 72f
    };
  }

  public static LengthUnit FromPixel(float pixel)
  {
    return new LengthUnit()
    {
      centimeter = (float) ((double) pixel / 72.0 * 2.5399999618530273),
      inch = pixel / 72f,
      pixel = pixel
    };
  }
}
