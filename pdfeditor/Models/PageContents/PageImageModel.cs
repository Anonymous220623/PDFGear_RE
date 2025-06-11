// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.PageContents.PageImageModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Buffers;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace pdfeditor.Models.PageContents;

public class PageImageModel
{
  public int ImagePageIndex { get; set; }

  public ImageMatrix Matrix { get; set; }

  public int ImageIndex { get; set; }

  public Image Image { get; set; }

  public float Height { get; set; }

  public float Width { get; set; }

  public int imageHeight { get; set; }

  public int imageWidth { get; set; }

  public PdfTypeBase sMaskRef { get; set; }

  public PageImageModel(PdfImageObject pdfImageObject, int pageindex)
  {
    this.ImagePageIndex = pageindex;
    if (pdfImageObject.Container == null || pdfImageObject.Container.Form != null)
      throw new ArgumentException((string) null, nameof (pdfImageObject));
    this.ImageIndex = pdfImageObject.Container.IndexOf((PdfPageObject) pdfImageObject);
    this.Matrix = new ImageMatrix(pdfImageObject.Matrix.a, pdfImageObject.Matrix.b, pdfImageObject.Matrix.c, pdfImageObject.Matrix.d, pdfImageObject.Matrix.e, pdfImageObject.Matrix.f);
    using (MemoryStream memoryStream = new MemoryStream())
    {
      pdfImageObject.Bitmap.Image.Save((Stream) memoryStream, ImageFormat.Png);
      memoryStream.Position = 0L;
      this.Image = Image.FromStream((Stream) memoryStream);
    }
    if (pdfImageObject.Stream == null || !pdfImageObject.Stream.Dictionary.ContainsKey("SMask") || !pdfImageObject.Stream.Dictionary["SMask"].Is<PdfTypeStream>())
      return;
    this.sMaskRef = (PdfTypeBase) pdfImageObject.Stream.Dictionary["SMask"].As<PdfTypeStream>();
  }

  private static unsafe Bitmap CreateSoftMaskBitmap(
    PdfTypeStream sMaskStream,
    out string colorSpace)
  {
    int intValue1 = sMaskStream.Dictionary["Width"].As<PdfTypeNumber>().IntValue;
    int intValue2 = sMaskStream.Dictionary["Height"].As<PdfTypeNumber>().IntValue;
    int intValue3 = sMaskStream.Dictionary["BitsPerComponent"].As<PdfTypeNumber>().IntValue;
    colorSpace = sMaskStream.Dictionary["ColorSpace"].As<PdfTypeName>().Value;
    if (!string.IsNullOrEmpty(sMaskStream.Dictionary.ContainsKey("Filter") ? sMaskStream.Dictionary["Filter"].As<PdfTypeName>().Value : (string) null))
      return (Bitmap) null;
    try
    {
      using (MemoryHandle memoryHandle = new Memory<byte>(sMaskStream.DecodedData).Pin())
        return new Bitmap(intValue1, intValue2, intValue1 * intValue3 / 8, PixelFormat.Format8bppIndexed, new IntPtr(memoryHandle.Pointer));
    }
    catch
    {
    }
    return (Bitmap) null;
  }

  private static PdfBitmap CreateFXDIB8bppMask(Bitmap bitmap)
  {
    BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
    try
    {
      return new PdfBitmap(bitmapdata.Width, bitmapdata.Height, BitmapFormats.FXDIB_8bppMask, bitmapdata.Scan0, bitmapdata.Stride);
    }
    finally
    {
      bitmap.UnlockBits(bitmapdata);
    }
  }

  private static PdfTypeStream ConvertBitmapToPdfTypeStream(Bitmap bitmap, string color)
  {
    int width = bitmap.Width;
    int height = bitmap.Height;
    PixelFormat pixelFormat = bitmap.PixelFormat;
    if (Image.GetPixelFormatSize(pixelFormat) != 8 || pixelFormat != PixelFormat.Format8bppIndexed)
      throw new ArgumentException("error!");
    PdfTypeStream pdfTypeStream = PdfTypeStream.Create();
    pdfTypeStream.InitEmpty();
    pdfTypeStream.Dictionary.Add("Width", (PdfTypeBase) PdfTypeNumber.Create(width));
    pdfTypeStream.Dictionary.Add("Height", (PdfTypeBase) PdfTypeNumber.Create(height));
    pdfTypeStream.Dictionary.Add("BitsPerComponent", (PdfTypeBase) PdfTypeNumber.Create(8));
    pdfTypeStream.Dictionary.Add("ColorSpace", (PdfTypeBase) PdfTypeName.Create(color));
    BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, pixelFormat);
    try
    {
      int length = Math.Abs(bitmapdata.Stride) * height;
      byte[] numArray = new byte[length];
      Marshal.Copy(bitmapdata.Scan0, numArray, 0, length);
      pdfTypeStream.SetContent(numArray, true);
    }
    finally
    {
      bitmap.UnlockBits(bitmapdata);
    }
    return pdfTypeStream;
  }
}
