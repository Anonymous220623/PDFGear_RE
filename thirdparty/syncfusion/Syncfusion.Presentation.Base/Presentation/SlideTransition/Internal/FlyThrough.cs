// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideTransition.Internal.FlyThrough
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.SlideTransition.Internal;

internal class FlyThrough
{
  private bool _hasBounce;
  private TransitionInOutDirectionType _direction;

  internal bool hasBounce
  {
    get => this._hasBounce;
    set => this._hasBounce = value;
  }

  internal TransitionInOutDirectionType Direction
  {
    get => this._direction;
    set => this._direction = value;
  }
}
