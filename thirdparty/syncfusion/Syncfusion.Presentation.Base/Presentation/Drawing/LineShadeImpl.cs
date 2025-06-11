// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.LineShadeImpl
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class LineShadeImpl
{
  private int _angle;
  private bool _isScaled;

  internal int Angle
  {
    get => this._angle;
    set => this._angle = value;
  }

  internal bool IsScaled
  {
    get => this._isScaled;
    set => this._isScaled = value;
  }

  public LineShadeImpl Clone() => (LineShadeImpl) this.MemberwiseClone();
}
