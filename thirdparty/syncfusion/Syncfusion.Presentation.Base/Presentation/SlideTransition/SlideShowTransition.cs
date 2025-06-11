// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideTransition.SlideShowTransition
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Interfaces;
using Syncfusion.Presentation.Resource;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.SlideTransition.Internal;
using System;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.SlideTransition;

internal class SlideShowTransition : ISlideShowTransition
{
  private bool _triggerOnClick = true;
  private bool _triggerOnTimeDelay;
  private float _TimeDelay;
  private float _duration;
  private TransitionEffect _transitionEffect;
  private TransitionEffectOption _transitionEffectOption;
  private TransitionSpeed _transitionSpeed;
  private BaseSlide _baseSlide;

  internal SlideShowTransition(BaseSlide baseSlide) => this._baseSlide = baseSlide;

  public bool TriggerOnClick
  {
    get => this._triggerOnClick;
    set => this._triggerOnClick = value;
  }

  public bool TriggerOnTimeDelay
  {
    get => this._triggerOnTimeDelay;
    set
    {
      this._triggerOnTimeDelay = value;
      if (!this._baseSlide.IsAlternateContent)
        return;
      this._baseSlide.InternalTransition.AlternateContent.Choice.Transition.IsAdvanceOnTime = this._triggerOnTimeDelay;
      if (this._baseSlide.InternalTransition.AlternateContent.FallBack == null)
        return;
      this._baseSlide.InternalTransition.AlternateContent.FallBack.Transition.IsAdvanceOnTime = this._triggerOnTimeDelay;
    }
  }

  public float TimeDelay
  {
    get => this._TimeDelay;
    set
    {
      this._TimeDelay = (double) value <= 86399.0 && (double) value >= 0.0 ? value : throw new Exception("Floating point value out of range. Value is not in the valid range of 0 to 86399.");
      this.TriggerOnTimeDelay = true;
    }
  }

  public float Duration
  {
    get => this._duration;
    set
    {
      this._duration = (double) value < 60.0 && (double) value >= 0.0 ? value : throw new Exception("Value should be with in the range of 0 to 59");
    }
  }

  public TransitionEffect TransitionEffect
  {
    get => this._transitionEffect;
    set
    {
      if (value == TransitionEffect.None)
      {
        this._transitionEffect = value;
        if (this._transitionEffect == TransitionEffect.None)
          return;
        this._transitionEffectOption = TransitionEffectOption.None;
        this._baseSlide.InternalTransition.AlternateContent = (AlternateContent) null;
        this._baseSlide.InternalTransition.Transition = (TransitionInternal) null;
      }
      else if (this._baseSlide.InternalTransition.AlternateContent != null)
      {
        if (this._baseSlide.InternalTransition.AlternateContent.Choice.Transition.SlideShowTransition.TransitionEffect == TransitionEffect.None)
          this._transitionEffect = value;
        else if (this._baseSlide.InternalTransition.AlternateContent.FallBack != null)
        {
          if (this._baseSlide.InternalTransition.AlternateContent.FallBack.Transition.SlideShowTransition.TransitionEffect == TransitionEffect.None)
          {
            this._transitionEffect = value;
          }
          else
          {
            this._transitionEffect = value;
            this.ModifySlideTransition(this._baseSlide, this.TransitionEffect, TransitionEffectOption.None);
          }
        }
        else
          this._transitionEffect = value;
      }
      else if (this._baseSlide.InternalTransition.Transition != null)
      {
        this._transitionEffect = value;
      }
      else
      {
        this._transitionEffect = value;
        this.ModifySlideTransition(this._baseSlide, this.TransitionEffect, TransitionEffectOption.None);
      }
    }
  }

  public TransitionEffectOption TransitionEffectOption
  {
    get => this._transitionEffectOption;
    set
    {
      if (this.TransitionEffect != TransitionEffect.None)
      {
        if (this._baseSlide.InternalTransition.AlternateContent != null)
        {
          if (this._baseSlide.InternalTransition.AlternateContent.Choice.Transition.SlideShowTransition.TransitionEffect == TransitionEffect.None)
            this._transitionEffectOption = value;
          else if (this._baseSlide.InternalTransition.AlternateContent.FallBack != null)
          {
            if (this._baseSlide.InternalTransition.AlternateContent.FallBack.Transition.SlideShowTransition.TransitionEffect == TransitionEffect.None)
              this._transitionEffectOption = value;
            else if (SlideTransitionConstant.IsValidTransitionSubtype(this._baseSlide.SlideTransition.TransitionEffect, value))
            {
              this._transitionEffectOption = value;
              this.ModifySlideTransition(this._baseSlide, this._baseSlide.SlideTransition.TransitionEffect, value);
            }
            else
              throw new Exception($"{(object) value} subtype is not valid for the {(object) this._baseSlide.SlideTransition.TransitionEffect} effect");
          }
          else
            this._transitionEffectOption = value;
        }
        else
        {
          if (this._baseSlide.InternalTransition.Transition == null)
            return;
          this._transitionEffectOption = value;
        }
      }
      else
        this._transitionEffectOption = value;
    }
  }

  public TransitionSpeed Speed
  {
    get => this._transitionSpeed;
    set
    {
      this._transitionSpeed = value;
      if (this.Speed == TransitionSpeed.Fast)
        this._baseSlide.SlideTransition.Duration = 0.5f;
      else if (this.Speed == TransitionSpeed.Medium)
      {
        this._baseSlide.SlideTransition.Duration = 0.75f;
      }
      else
      {
        if (this.Speed != TransitionSpeed.Slow)
          return;
        this._baseSlide.SlideTransition.Duration = 1f;
      }
    }
  }

  internal void ModifySlideTransition(
    BaseSlide slide,
    TransitionEffect effect,
    TransitionEffectOption subType)
  {
    Parser.GetSlideTransitionInternalDOM(XmlReader.Create(ResourceManager.GetStreamFromResource("TransitionEffects", ".xml")), effect, subType, slide);
    slide.IsSlideTransition = true;
    slide.IsAlternateContent = true;
  }
}
