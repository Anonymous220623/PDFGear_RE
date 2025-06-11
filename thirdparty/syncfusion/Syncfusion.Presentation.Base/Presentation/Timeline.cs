// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Timeline
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

internal class Timeline : IAnimationTimeline
{
  private BaseSlide _baseSlide;
  private ISequence mainSequence;
  private ISequences interactiveSequence;

  internal Timeline(BaseSlide baseSlide)
  {
    this._baseSlide = baseSlide;
    this.mainSequence = (ISequence) new Sequence(baseSlide);
    this.interactiveSequence = (ISequences) new Sequences(baseSlide);
  }

  public ISequence MainSequence
  {
    get
    {
      this._baseSlide.Timing.TimeNodeList = (List<object>) null;
      return this.mainSequence;
    }
  }

  public ISequences InteractiveSequences
  {
    get
    {
      this._baseSlide.Timing.TimeNodeList = (List<object>) null;
      return this.interactiveSequence;
    }
  }

  internal ISequence GetMainSequence() => this.mainSequence;

  internal ISequences GetInteractiveSequences() => this.interactiveSequence;

  internal Timeline Clone(BaseSlide newBaseSlide)
  {
    Timeline timeline = (Timeline) this.MemberwiseClone();
    timeline._baseSlide = newBaseSlide;
    timeline.mainSequence = (ISequence) (this.mainSequence as Sequence).Clone(newBaseSlide);
    timeline.interactiveSequence = (ISequences) (this.interactiveSequence as Sequences).Clone(newBaseSlide);
    return timeline;
  }
}
