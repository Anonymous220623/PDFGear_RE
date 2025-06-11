// Decompiled with JetBrains decompiler
// Type: Tesseract.PixConverter
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System.Drawing;

#nullable disable
namespace Tesseract;

public static class PixConverter
{
  private static readonly BitmapToPixConverter bitmapConverter = new BitmapToPixConverter();
  private static readonly PixToBitmapConverter pixConverter = new PixToBitmapConverter();

  public static Bitmap ToBitmap(Pix pix) => PixConverter.pixConverter.Convert(pix);

  public static Pix ToPix(Bitmap img) => PixConverter.bitmapConverter.Convert(img);
}
