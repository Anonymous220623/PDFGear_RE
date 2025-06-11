// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideTransition.Internal.Reveal
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.SlideTransition.Internal;

internal class Reveal
{
  private bool _throwBlack;
  private TransitionLeftRightDirectionType _direction;

  internal bool ThrowBlack
  {
    get => this._throwBlack;
    set => this._throwBlack = value;
  }

  internal TransitionLeftRightDirectionType Direction
  {
    get => this._direction;
    set => this._direction = value;
  }
}
