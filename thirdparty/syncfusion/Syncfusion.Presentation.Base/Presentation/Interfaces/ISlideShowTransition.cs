// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Interfaces.ISlideShowTransition
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideTransition;

#nullable disable
namespace Syncfusion.Presentation.Interfaces;

public interface ISlideShowTransition
{
  bool TriggerOnClick { get; set; }

  bool TriggerOnTimeDelay { get; set; }

  float TimeDelay { get; set; }

  float Duration { get; set; }

  TransitionEffect TransitionEffect { get; set; }

  TransitionEffectOption TransitionEffectOption { get; set; }

  TransitionSpeed Speed { get; set; }
}
