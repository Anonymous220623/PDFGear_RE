// Decompiled with JetBrains decompiler
// Type: Tesseract.TesseractDrawingExtensions
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;

#nullable disable
namespace Tesseract;

public static class TesseractDrawingExtensions
{
  public static Page Process(this TesseractEngine engine, Bitmap image, PageSegMode? pageSegMode = null)
  {
    return engine.Process(image, new Rect(0, 0, image.Width, image.Height), pageSegMode);
  }

  public static Page Process(
    this TesseractEngine engine,
    Bitmap image,
    string inputName,
    PageSegMode? pageSegMode = null)
  {
    return engine.Process(image, inputName, new Rect(0, 0, image.Width, image.Height), pageSegMode);
  }

  public static Page Process(
    this TesseractEngine engine,
    Bitmap image,
    Rect region,
    PageSegMode? pageSegMode = null)
  {
    return engine.Process(image, (string) null, region, pageSegMode);
  }

  public static Page Process(
    this TesseractEngine engine,
    Bitmap image,
    string inputName,
    Rect region,
    PageSegMode? pageSegMode = null)
  {
    Pix pix = PixConverter.ToPix(image);
    Page page = engine.Process(pix, inputName, region, pageSegMode);
    TesseractEngine.PageDisposalHandle pageDisposalHandle = new TesseractEngine.PageDisposalHandle(page, pix);
    return page;
  }

  public static Color ToColor(this PixColor color)
  {
    return Color.FromArgb((int) color.Alpha, (int) color.Red, (int) color.Green, (int) color.Blue);
  }

  public static PixColor ToPixColor(this Color color)
  {
    return new PixColor(color.R, color.G, color.B, color.A);
  }

  public static int GetBPP(this Bitmap bitmap)
  {
    switch (bitmap.PixelFormat)
    {
      case PixelFormat.Format16bppRgb555:
      case PixelFormat.Format16bppRgb565:
      case PixelFormat.Format16bppArgb1555:
      case PixelFormat.Format16bppGrayScale:
        return 16 /*0x10*/;
      case PixelFormat.Format24bppRgb:
        return 24;
      case PixelFormat.Format32bppRgb:
      case PixelFormat.Format32bppPArgb:
      case PixelFormat.Format32bppArgb:
        return 32 /*0x20*/;
      case PixelFormat.Format1bppIndexed:
        return 1;
      case PixelFormat.Format4bppIndexed:
        return 4;
      case PixelFormat.Format8bppIndexed:
        return 8;
      case PixelFormat.Format48bppRgb:
        return 48 /*0x30*/;
      case PixelFormat.Format64bppPArgb:
      case PixelFormat.Format64bppArgb:
        return 64 /*0x40*/;
      default:
        throw new ArgumentException($"The bitmap's pixel format of {bitmap.PixelFormat} was not recognised.", nameof (bitmap));
    }
  }
}
