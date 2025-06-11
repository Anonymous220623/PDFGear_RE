// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.GlowBitmap
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace HandyControl.Data;

internal class GlowBitmap : DisposableObject
{
  internal const int GlowBitmapPartCount = 16 /*0x10*/;
  private const int BytesPerPixelBgra32 = 4;
  private static readonly GlowBitmap.CachedBitmapInfo[] _transparencyMasks = new GlowBitmap.CachedBitmapInfo[16 /*0x10*/];
  private readonly InteropValues.BITMAPINFO _bitmapInfo;
  private readonly IntPtr _pbits;

  internal GlowBitmap(IntPtr hdcScreen, int width, int height)
  {
    this._bitmapInfo.biSize = Marshal.SizeOf(typeof (InteropValues.BITMAPINFOHEADER));
    this._bitmapInfo.biPlanes = (short) 1;
    this._bitmapInfo.biBitCount = (short) 32 /*0x20*/;
    this._bitmapInfo.biCompression = 0;
    this._bitmapInfo.biXPelsPerMeter = 0;
    this._bitmapInfo.biYPelsPerMeter = 0;
    this._bitmapInfo.biWidth = width;
    this._bitmapInfo.biHeight = -height;
    this.Handle = InteropMethods.CreateDIBSection(hdcScreen, ref this._bitmapInfo, 0U, out this._pbits, IntPtr.Zero, 0U);
  }

  internal IntPtr Handle { get; }

  internal IntPtr DIBits => this._pbits;

  internal int Width => this._bitmapInfo.biWidth;

  internal int Height => -this._bitmapInfo.biHeight;

  protected override void DisposeNativeResources() => InteropMethods.DeleteObject(this.Handle);

  private static byte PremultiplyAlpha(byte channel, byte alpha)
  {
    return (byte) ((double) ((int) channel * (int) alpha) / (double) byte.MaxValue);
  }

  internal static GlowBitmap Create(
    GlowDrawingContext drawingContext,
    GlowBitmapPart bitmapPart,
    Color color)
  {
    GlowBitmap.CachedBitmapInfo alphaMask = GlowBitmap.GetOrCreateAlphaMask(bitmapPart);
    GlowBitmap glowBitmap = new GlowBitmap(drawingContext.ScreenDC, alphaMask.Width, alphaMask.Height);
    for (int ofs = 0; ofs < alphaMask.DIBits.Length; ofs += 4)
    {
      byte diBit = alphaMask.DIBits[ofs + 3];
      byte val1 = GlowBitmap.PremultiplyAlpha(color.R, diBit);
      byte val2 = GlowBitmap.PremultiplyAlpha(color.G, diBit);
      byte val3 = GlowBitmap.PremultiplyAlpha(color.B, diBit);
      Marshal.WriteByte(glowBitmap.DIBits, ofs, val3);
      Marshal.WriteByte(glowBitmap.DIBits, ofs + 1, val2);
      Marshal.WriteByte(glowBitmap.DIBits, ofs + 2, val1);
      Marshal.WriteByte(glowBitmap.DIBits, ofs + 3, diBit);
    }
    return glowBitmap;
  }

  private static GlowBitmap.CachedBitmapInfo GetOrCreateAlphaMask(GlowBitmapPart bitmapPart)
  {
    if (GlowBitmap._transparencyMasks[(int) bitmapPart] == null)
    {
      BitmapImage bitmapImage = new BitmapImage(new Uri($"pack://application:,,,/HandyControl;Component/Resources/Images/GlowWindow/{bitmapPart}.png"));
      byte[] numArray = new byte[4 * bitmapImage.PixelWidth * bitmapImage.PixelHeight];
      int stride = 4 * bitmapImage.PixelWidth;
      bitmapImage.CopyPixels((Array) numArray, stride, 0);
      bitmapImage.Freeze();
      GlowBitmap._transparencyMasks[(int) bitmapPart] = new GlowBitmap.CachedBitmapInfo(numArray, bitmapImage.PixelWidth, bitmapImage.PixelHeight);
    }
    return GlowBitmap._transparencyMasks[(int) bitmapPart];
  }

  private sealed class CachedBitmapInfo
  {
    internal readonly byte[] DIBits;
    internal readonly int Height;
    internal readonly int Width;

    internal CachedBitmapInfo(byte[] diBits, int width, int height)
    {
      this.Width = width;
      this.Height = height;
      this.DIBits = diBits;
    }
  }
}
