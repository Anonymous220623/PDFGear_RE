// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideTransition.Internal.AlternateContent
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.SlideTransition.Internal;

internal class AlternateContent
{
  private Choice _choice;
  private FallBack _fallBack;

  internal Choice Choice
  {
    get => this._choice;
    set => this._choice = value;
  }

  internal FallBack FallBack
  {
    get => this._fallBack;
    set => this._fallBack = value;
  }
}
