// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.IconHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace HandyControl.Tools;

internal static class IconHelper
{
  private static Size SmallIconSize;
  private static Size IconSize;
  private static int SystemBitDepth;

  [SecurityCritical]
  [SecuritySafeCritical]
  public static void GetIconHandlesFromImageSource(
    ImageSource image,
    out IconHandle largeIconHandle,
    out IconHandle smallIconHandle)
  {
    IconHelper.EnsureSystemMetrics();
    largeIconHandle = IconHelper.CreateIconHandleFromImageSource(image, IconHelper.IconSize);
    smallIconHandle = IconHelper.CreateIconHandleFromImageSource(image, IconHelper.SmallIconSize);
  }

  [SecurityCritical]
  public static IconHandle CreateIconHandleFromImageSource(ImageSource image, Size size)
  {
    IconHelper.EnsureSystemMetrics();
    bool flag = false;
    if ((image is BitmapFrame sourceBitmapFrame ? sourceBitmapFrame.Decoder?.Frames : (ReadOnlyCollection<BitmapFrame>) null) != null)
    {
      sourceBitmapFrame = IconHelper.GetBestMatch(sourceBitmapFrame.Decoder.Frames, size);
      flag = sourceBitmapFrame.Decoder is IconBitmapDecoder || sourceBitmapFrame.PixelWidth == (int) size.Width && sourceBitmapFrame.PixelHeight == (int) size.Height;
      image = (ImageSource) sourceBitmapFrame;
    }
    if (!flag)
      sourceBitmapFrame = BitmapFrame.Create(IconHelper.GenerateBitmapSource(image, size));
    return IconHelper.CreateIconHandleFromBitmapFrame(sourceBitmapFrame);
  }

  [SecurityCritical]
  private static IconHandle CreateIconHandleFromBitmapFrame(BitmapFrame sourceBitmapFrame)
  {
    BitmapSource source = (BitmapSource) sourceBitmapFrame;
    if (source.Format != PixelFormats.Bgra32 && source.Format != PixelFormats.Pbgra32)
      source = (BitmapSource) new FormatConvertedBitmap(source, PixelFormats.Bgra32, (BitmapPalette) null, 0.0);
    int pixelWidth = source.PixelWidth;
    int pixelHeight = source.PixelHeight;
    int stride = (source.Format.BitsPerPixel * pixelWidth + 31 /*0x1F*/) / 32 /*0x20*/ * 4;
    byte[] numArray = new byte[stride * pixelHeight];
    source.CopyPixels((Array) numArray, stride, 0);
    return IconHelper.CreateIconCursor(numArray, pixelWidth, pixelHeight, 0, 0, true);
  }

  [SecurityCritical]
  internal static IconHandle CreateIconCursor(
    byte[] colorArray,
    int width,
    int height,
    int xHotspot,
    int yHotspot,
    bool isIcon)
  {
    BitmapHandle bitmapHandle1 = (BitmapHandle) null;
    BitmapHandle bitmapHandle2 = (BitmapHandle) null;
    try
    {
      InteropValues.BITMAPINFO bitmapInfo = new InteropValues.BITMAPINFO(width, -height, (short) 32 /*0x20*/)
      {
        biCompression = 0
      };
      IntPtr zero = IntPtr.Zero;
      bitmapHandle1 = InteropMethods.CreateDIBSection(new HandleRef((object) null, IntPtr.Zero), ref bitmapInfo, 0, ref zero, (SafeFileMappingHandle) null, 0);
      if (bitmapHandle1.IsInvalid || zero == IntPtr.Zero)
        return IconHandle.GetInvalidIcon();
      Marshal.Copy(colorArray, 0, zero, colorArray.Length);
      byte[] maskArray = IconHelper.GenerateMaskArray(width, height, colorArray);
      bitmapHandle2 = InteropMethods.CreateBitmap(width, height, 1, 1, maskArray);
      if (bitmapHandle2.IsInvalid)
        return IconHandle.GetInvalidIcon();
      return InteropMethods.CreateIconIndirect(new InteropValues.ICONINFO()
      {
        fIcon = isIcon,
        xHotspot = xHotspot,
        yHotspot = yHotspot,
        hbmMask = bitmapHandle2,
        hbmColor = bitmapHandle1
      });
    }
    finally
    {
      bitmapHandle1?.Dispose();
      bitmapHandle2?.Dispose();
    }
  }

  private static byte[] GenerateMaskArray(int width, int height, byte[] colorArray)
  {
    int num1 = width * height;
    int num2 = IconHelper.AlignToBytes((double) width, 2) / 8;
    byte[] maskArray = new byte[num2 * height];
    for (int index = 0; index < num1; ++index)
    {
      int num3 = index % width;
      int num4 = index / width;
      int num5 = num3 / 8;
      byte num6 = (byte) (128 /*0x80*/ >> num3 % 8);
      if (colorArray[index * 4 + 3] == (byte) 0)
        maskArray[num5 + num2 * num4] |= num6;
      else
        maskArray[num5 + num2 * num4] &= ~num6;
      if (num3 == width - 1 && width == 8)
        maskArray[1 + num2 * num4] = byte.MaxValue;
    }
    return maskArray;
  }

  internal static int AlignToBytes(double original, int nBytesCount)
  {
    int num = 8 << nBytesCount - 1;
    return ((int) Math.Ceiling(original) + (num - 1)) / num * num;
  }

  private static BitmapSource GenerateBitmapSource(ImageSource img, Size renderSize)
  {
    Rect rectangle = new Rect(0.0, 0.0, renderSize.Width, renderSize.Height);
    double num1 = renderSize.Width / renderSize.Height;
    double num2 = img.Width / img.Height;
    if (img.Width <= renderSize.Width && img.Height <= renderSize.Height)
      rectangle = new Rect((renderSize.Width - img.Width) / 2.0, (renderSize.Height - img.Height) / 2.0, img.Width, img.Height);
    else if (num1 > num2)
    {
      double width = img.Width / img.Height * renderSize.Width;
      rectangle = new Rect((renderSize.Width - width) / 2.0, 0.0, width, renderSize.Height);
    }
    else if (num1 < num2)
    {
      double height = img.Height / img.Width * renderSize.Height;
      rectangle = new Rect(0.0, (renderSize.Height - height) / 2.0, renderSize.Width, height);
    }
    DrawingVisual drawingVisual = new DrawingVisual();
    DrawingContext drawingContext = drawingVisual.RenderOpen();
    drawingContext.DrawImage(img, rectangle);
    drawingContext.Close();
    RenderTargetBitmap bitmapSource = new RenderTargetBitmap((int) renderSize.Width, (int) renderSize.Height, 96.0, 96.0, PixelFormats.Pbgra32);
    bitmapSource.Render((Visual) drawingVisual);
    return (BitmapSource) bitmapSource;
  }

  private static BitmapFrame GetBestMatch(ReadOnlyCollection<BitmapFrame> frames, Size size)
  {
    int num1 = int.MaxValue;
    int num2 = 0;
    int index1 = 0;
    bool flag = frames[0].Decoder is IconBitmapDecoder;
    for (int index2 = 0; index2 < frames.Count && num1 != 0; ++index2)
    {
      PixelFormat format;
      int bitsPerPixel;
      if (!flag)
      {
        format = frames[index2].Format;
        bitsPerPixel = format.BitsPerPixel;
      }
      else
      {
        format = frames[index2].Thumbnail.Format;
        bitsPerPixel = format.BitsPerPixel;
      }
      int bpp = bitsPerPixel;
      if (bpp == 0)
        bpp = 8;
      int num3 = IconHelper.MatchImage(frames[index2], size, bpp);
      if (num3 < num1)
      {
        index1 = index2;
        num2 = bpp;
        num1 = num3;
      }
      else if (num3 == num1 && num2 < bpp)
      {
        index1 = index2;
        num2 = bpp;
      }
    }
    return frames[index1];
  }

  private static int MatchImage(BitmapFrame frame, Size size, int bpp)
  {
    return 2 * IconHelper.MyAbs(bpp, IconHelper.SystemBitDepth, false) + IconHelper.MyAbs(frame.PixelWidth, (int) size.Width, true) + IconHelper.MyAbs(frame.PixelHeight, (int) size.Height, true);
  }

  private static int MyAbs(int valueHave, int valueWant, bool fPunish)
  {
    int num = valueHave - valueWant;
    if (num < 0)
      num = (fPunish ? -2 : -1) * num;
    return num;
  }

  [SecurityCritical]
  [SecuritySafeCritical]
  private static void EnsureSystemMetrics()
  {
    if (IconHelper.SystemBitDepth != 0)
      return;
    HandleRef hDC = new HandleRef((object) null, InteropMethods.GetDC(new HandleRef()));
    try
    {
      int num = InteropMethods.GetDeviceCaps(hDC, 12) * InteropMethods.GetDeviceCaps(hDC, 14);
      if (num == 8)
        num = 4;
      int systemMetrics1 = InteropMethods.GetSystemMetrics(InteropValues.SM.CXSMICON);
      int systemMetrics2 = InteropMethods.GetSystemMetrics(InteropValues.SM.CYSMICON);
      int systemMetrics3 = InteropMethods.GetSystemMetrics(InteropValues.SM.CXICON);
      int systemMetrics4 = InteropMethods.GetSystemMetrics(InteropValues.SM.CYICON);
      IconHelper.SmallIconSize = new Size((double) systemMetrics1, (double) systemMetrics2);
      IconHelper.IconSize = new Size((double) systemMetrics3, (double) systemMetrics4);
      IconHelper.SystemBitDepth = num;
    }
    finally
    {
      InteropMethods.ReleaseDC(new HandleRef(), hDC);
    }
  }

  [SecurityCritical]
  [SecuritySafeCritical]
  public static void GetDefaultIconHandles(
    out IconHandle largeIconHandle,
    out IconHandle smallIconHandle)
  {
    largeIconHandle = (IconHandle) null;
    smallIconHandle = (IconHandle) null;
    SecurityHelper.DemandUIWindowPermission();
    InteropMethods.ExtractIconEx(InteropMethods.GetModuleFileName(new HandleRef()), 0, out largeIconHandle, out smallIconHandle, 1);
  }
}
