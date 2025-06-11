// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideSize
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using System;

#nullable disable
namespace Syncfusion.Presentation;

internal class SlideSize : ISlideSize
{
  private int _cx;
  private int _cy;
  private SlideSizeType _type;
  private SlideOrientation _slideOrientation;

  internal SlideSize()
  {
    this._type = SlideSizeType.Custom;
    this._cx = 12192000;
    this._cy = 6858000;
  }

  public SlideSize(SlideSize slideSize)
  {
    this._cx = slideSize._cx;
    this._cy = slideSize._cy;
  }

  public SlideOrientation SlideOrientation
  {
    get
    {
      this._slideOrientation = this.Width <= this.Height ? SlideOrientation.Portrait : SlideOrientation.Landscape;
      return this._slideOrientation;
    }
    set
    {
      this._slideOrientation = value;
      switch (value)
      {
        case SlideOrientation.Landscape:
          this.SetSize();
          break;
        case SlideOrientation.Portrait:
          this.SetSize();
          this.SetSize(this._cy, this._cx);
          break;
      }
    }
  }

  public SlideSizeType Type
  {
    get => this._type = this.GetTypeFromSize();
    set
    {
      this._type = value;
      this.SetSize();
    }
  }

  private SlideSizeType GetTypeFromSize()
  {
    if (this._cx == 12801600 && this._cy == 9601200)
      return SlideSizeType.A3Paper;
    if (this._cx == 9906000 && this._cy == 6858000)
      return SlideSizeType.A4Paper;
    if (this._cx == 10826750 && this._cy == 8120063)
      return SlideSizeType.B4IsoPaper;
    if (this._cx == 7169150 && this._cy == 5376863)
      return SlideSizeType.B5IsoPaper;
    if (this._cx == 7315200 && this._cy == 914400)
      return SlideSizeType.Banner;
    if ((this._cx == 12179300 || this._cx == 12178894) && this._cy == 9134475)
      return SlideSizeType.Ledger;
    if (this._cx == 9144000 && this._cy == 6858000 && this._type == SlideSizeType.LetterPaper)
      return SlideSizeType.LetterPaper;
    if (this._cx == 9144000 && this._cy == 6858000)
      return SlideSizeType.OnScreen;
    if (this._cx == 9144000 && this._cy == 5715000)
      return SlideSizeType.OnScreen16X10;
    if (this._cx == 9144000 && this._cy == 5143500)
      return SlideSizeType.OnScreen16X9;
    if (this._cx == 9144000 && this._cy == 6858000)
      return SlideSizeType.Overhead;
    return this._cx == 10287000 && this._cy == 6858000 ? SlideSizeType.Slide35Mm : SlideSizeType.Custom;
  }

  public double Width
  {
    get => Helper.EmuToPoint(this._cx);
    set
    {
      if (this._type != SlideSizeType.Custom)
        throw new ArgumentException("Width can be modified only if the slide size type is set to Custom");
      this._cx = Helper.PointToEmu(value);
    }
  }

  public double Height
  {
    get => Helper.EmuToPoint(this._cy);
    set
    {
      if (this._type != SlideSizeType.Custom)
        throw new ArgumentException("Height can be modified only if the slide size type is set to Custom");
      this._cy = Helper.PointToEmu(value);
    }
  }

  internal void SetSize(int cx, int cy)
  {
    this._cx = cx;
    this._cy = cy;
  }

  internal int GetCx() => this._cx;

  internal int GetCy() => this._cy;

  internal void SetSize()
  {
    switch (this._type)
    {
      case SlideSizeType.OnScreen:
        this._cx = 9144000;
        this._cy = 6858000;
        break;
      case SlideSizeType.LetterPaper:
        this._cx = 9144000;
        this._cy = 6858000;
        break;
      case SlideSizeType.A4Paper:
        this._cx = 9906000;
        this._cy = 6858000;
        break;
      case SlideSizeType.Slide35Mm:
        this._cx = 10287000;
        this._cy = 6858000;
        break;
      case SlideSizeType.Overhead:
        this._cx = 9144000;
        this._cy = 6858000;
        break;
      case SlideSizeType.Banner:
        this._cx = 7315200;
        this._cy = 914400;
        break;
      case SlideSizeType.Custom:
        this._cx = 9525000;
        this._cy = 7239000;
        break;
      case SlideSizeType.Ledger:
        this._cx = 12178894;
        this._cy = 9134475;
        break;
      case SlideSizeType.A3Paper:
        this._cx = 12801600;
        this._cy = 9601200;
        break;
      case SlideSizeType.B4IsoPaper:
        this._cx = 10826750;
        this._cy = 8120063;
        break;
      case SlideSizeType.B5IsoPaper:
        this._cx = 7169150;
        this._cy = 5376863;
        break;
      case SlideSizeType.OnScreen16X9:
        this._cx = 9144000;
        this._cy = 5143500;
        break;
      case SlideSizeType.OnScreen16X10:
        this._cx = 9144000;
        this._cy = 5715000;
        break;
    }
  }

  public SlideSize Clone() => new SlideSize(this);
}
