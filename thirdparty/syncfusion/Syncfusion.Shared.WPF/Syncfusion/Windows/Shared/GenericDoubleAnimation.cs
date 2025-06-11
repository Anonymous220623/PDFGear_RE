// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.GenericDoubleAnimation
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class GenericDoubleAnimation : DoubleAnimationBase
{
  public static readonly DependencyProperty FromProperty = DependencyProperty.Register(nameof (From), typeof (double), typeof (GenericDoubleAnimation), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty ToProperty = DependencyProperty.Register(nameof (To), typeof (double), typeof (GenericDoubleAnimation), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty StepValueProviderProperty = DependencyProperty.Register(nameof (StepValueProvider), typeof (IDoubleAnimationStepValueProvider), typeof (GenericDoubleAnimation), new PropertyMetadata((PropertyChangedCallback) null));

  public double From
  {
    get => (double) this.GetValue(GenericDoubleAnimation.FromProperty);
    set => this.SetValue(GenericDoubleAnimation.FromProperty, (object) value);
  }

  public double To
  {
    get => (double) this.GetValue(GenericDoubleAnimation.ToProperty);
    set => this.SetValue(GenericDoubleAnimation.ToProperty, (object) value);
  }

  public IDoubleAnimationStepValueProvider StepValueProvider
  {
    get
    {
      return (IDoubleAnimationStepValueProvider) this.GetValue(GenericDoubleAnimation.StepValueProviderProperty);
    }
    set => this.SetValue(GenericDoubleAnimation.StepValueProviderProperty, (object) value);
  }

  protected override double GetCurrentValueCore(
    double startValue,
    double targetValue,
    AnimationClock clock)
  {
    try
    {
      if (this.StepValueProvider != null)
        return this.StepValueProvider.GetAnimationStepValue(clock.CurrentTime.Value.TotalSeconds, this.From, this.To - this.From, this.Duration.TimeSpan.TotalSeconds);
    }
    catch
    {
    }
    return this.From;
  }

  protected override Freezable CreateInstanceCore() => (Freezable) new GenericDoubleAnimation();
}
