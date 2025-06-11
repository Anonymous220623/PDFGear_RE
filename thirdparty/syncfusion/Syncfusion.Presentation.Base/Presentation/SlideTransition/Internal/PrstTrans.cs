// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideTransition.Internal.PrstTrans
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.SlideTransition.Internal;

internal class PrstTrans
{
  private bool _invX;
  private bool _invY;
  private string _presetTransition;

  internal bool InvX
  {
    get => this._invX;
    set => this._invX = value;
  }

  internal bool InvY
  {
    get => this._invY;
    set => this._invY = value;
  }

  internal string PresetTransition
  {
    get => this._presetTransition;
    set => this._presetTransition = value;
  }
}
