// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.GlowDrawingContext
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;

#nullable disable
namespace HandyControl.Data;

internal class GlowDrawingContext : DisposableObject
{
  private readonly GlowBitmap _windowBitmap;
  internal InteropValues.BLENDFUNCTION Blend;

  internal GlowDrawingContext(int width, int height)
  {
    this.ScreenDC = InteropMethods.GetDC(IntPtr.Zero);
    if (this.ScreenDC == IntPtr.Zero)
      return;
    this.WindowDC = InteropMethods.CreateCompatibleDC(this.ScreenDC);
    if (this.WindowDC == IntPtr.Zero)
      return;
    this.BackgroundDC = InteropMethods.CreateCompatibleDC(this.ScreenDC);
    if (this.BackgroundDC == IntPtr.Zero)
      return;
    this.Blend.BlendOp = (byte) 0;
    this.Blend.BlendFlags = (byte) 0;
    this.Blend.SourceConstantAlpha = byte.MaxValue;
    this.Blend.AlphaFormat = (byte) 1;
    this._windowBitmap = new GlowBitmap(this.ScreenDC, width, height);
    InteropMethods.SelectObject(this.WindowDC, this._windowBitmap.Handle);
  }

  internal bool IsInitialized
  {
    get
    {
      return this.ScreenDC != IntPtr.Zero && this.WindowDC != IntPtr.Zero && this.BackgroundDC != IntPtr.Zero && this._windowBitmap != null;
    }
  }

  internal IntPtr ScreenDC { get; }

  internal IntPtr WindowDC { get; }

  internal IntPtr BackgroundDC { get; }

  internal int Width => this._windowBitmap.Width;

  internal int Height => this._windowBitmap.Height;

  protected override void DisposeManagedResources() => this._windowBitmap.Dispose();

  protected override void DisposeNativeResources()
  {
    if (this.ScreenDC != IntPtr.Zero)
      InteropMethods.ReleaseDC(IntPtr.Zero, this.ScreenDC);
    if (this.WindowDC != IntPtr.Zero)
      InteropMethods.DeleteDC(this.WindowDC);
    if (!(this.BackgroundDC != IntPtr.Zero))
      return;
    InteropMethods.DeleteDC(this.BackgroundDC);
  }
}
