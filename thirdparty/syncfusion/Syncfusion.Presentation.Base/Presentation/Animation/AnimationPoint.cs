// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.AnimationPoint
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Animation;

internal class AnimationPoint : IAnimationPoint
{
  private string _formula = "";
  private object _value;
  private float _time = float.NaN;

  public AnimationPoint()
  {
  }

  public AnimationPoint(float time, object value, string formula)
  {
    this._formula = formula;
    this._value = value;
    this._time = time;
  }

  public string Formula
  {
    get => this._formula;
    set => this._formula = value;
  }

  public object Value
  {
    get => this._value;
    set => this._value = value;
  }

  public float Time
  {
    get => this._time;
    set => this._time = value;
  }

  internal AnimationPoint Clone() => (AnimationPoint) this.MemberwiseClone();
}
