// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.GifImageInfo
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Controls;
using HandyControl.Tools;
using System;

#nullable disable
namespace HandyControl.Data;

public class GifImageInfo
{
  private const int PropertyTagFrameDelay = 20736;
  private int _frame;
  private readonly int[] _frameDelay;

  public int FrameCount { get; }

  internal GifImage Image { get; }

  public bool Animated { get; }

  public EventHandler FrameChangedHandler { get; set; }

  internal int FrameTimer { get; set; }

  public bool FrameDirty { get; private set; }

  public int Frame
  {
    get => this._frame;
    set
    {
      if (this._frame == value)
        return;
      if (value < 0 || value >= this.FrameCount)
        throw new ArgumentException("InvalidFrame");
      if (!this.Animated)
        return;
      this._frame = value;
      this.FrameDirty = true;
      this.OnFrameChanged(EventArgs.Empty);
    }
  }

  public GifImageInfo(GifImage image)
  {
    this.Image = image;
    this.Animated = ImageAnimator.CanAnimate(image);
    if (this.Animated)
    {
      this.FrameCount = image.GetFrameCount(GifFrameDimension.Time);
      GifPropertyItem propertyItem = image.GetPropertyItem(20736);
      if (propertyItem != null)
      {
        byte[] numArray = propertyItem.Value;
        this._frameDelay = new int[this.FrameCount];
        for (int index = 0; index < this.FrameCount; ++index)
          this._frameDelay[index] = (int) numArray[index * 4] + 256 /*0x0100*/ * (int) numArray[index * 4 + 1] + 65536 /*0x010000*/ * (int) numArray[index * 4 + 2] + 16777216 /*0x01000000*/ * (int) numArray[index * 4 + 3];
      }
    }
    else
      this.FrameCount = 1;
    if (this._frameDelay != null)
      return;
    this._frameDelay = new int[this.FrameCount];
  }

  public int FrameDelay(int frame) => this._frameDelay[frame];

  internal void UpdateFrame()
  {
    if (!this.FrameDirty)
      return;
    this.Image.SelectActiveFrame(GifFrameDimension.Time, this.Frame);
    this.FrameDirty = false;
  }

  protected void OnFrameChanged(EventArgs e)
  {
    EventHandler frameChangedHandler = this.FrameChangedHandler;
    if (frameChangedHandler == null)
      return;
    frameChangedHandler((object) this.Image, e);
  }
}
