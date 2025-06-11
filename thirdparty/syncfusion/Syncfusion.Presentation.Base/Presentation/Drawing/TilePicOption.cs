// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.TilePicOption
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class TilePicOption
{
  private int _offsetX;
  private int _offsetY;
  private int _scaleX;
  private int _scaleY;
  private MirrorType _mirrorType;
  private RectangleAlignmentType _rectangleAlignmentType;

  internal TilePicOption()
  {
    this._scaleX = 100000;
    this._scaleY = 100000;
    this._rectangleAlignmentType = RectangleAlignmentType.TopLeft;
  }

  internal RectangleAlignmentType AlignmentType
  {
    get => this._rectangleAlignmentType;
    set => this._rectangleAlignmentType = value;
  }

  internal MirrorType MirrorType
  {
    get => this._mirrorType;
    set => this._mirrorType = value;
  }

  internal double OffsetX
  {
    get => (double) this._offsetX / 12700.0;
    set => this._offsetX = (int) (value * 12700.0);
  }

  internal double OffsetY
  {
    get => (double) this._offsetY / 12700.0;
    set => this._offsetY = (int) (value * 12700.0);
  }

  internal double ScaleX
  {
    get => (double) this._scaleX / 1000.0;
    set => this._scaleX = (int) (value * 1000.0);
  }

  internal double ScaleY
  {
    get => (double) this._scaleY / 1000.0;
    set => this._scaleY = (int) (value * 1000.0);
  }

  internal void Copy(TilePicOption tilePicOption)
  {
    this._offsetX = tilePicOption._offsetX;
    this._scaleX = tilePicOption._scaleX;
    this._offsetY = tilePicOption._offsetY;
    this._scaleY = tilePicOption._scaleY;
    this._mirrorType = tilePicOption._mirrorType;
    this._rectangleAlignmentType = tilePicOption._rectangleAlignmentType;
  }

  public TilePicOption Clone() => (TilePicOption) this.MemberwiseClone();
}
