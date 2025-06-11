// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.TransitionEffect
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

#nullable disable
namespace HandyControl.Interactivity;

public abstract class TransitionEffect : ShaderEffect
{
  public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(nameof (Input), typeof (TransitionEffect), 0, SamplingMode.NearestNeighbor);
  public static readonly DependencyProperty OldImageProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(nameof (OldImage), typeof (TransitionEffect), 1, SamplingMode.NearestNeighbor);
  public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof (Progress), typeof (double), typeof (TransitionEffect), new PropertyMetadata((object) 0.0, ShaderEffect.PixelShaderConstantCallback(0)));

  protected TransitionEffect()
  {
    this.UpdateShaderValue(TransitionEffect.InputProperty);
    this.UpdateShaderValue(TransitionEffect.OldImageProperty);
    this.UpdateShaderValue(TransitionEffect.ProgressProperty);
  }

  public Brush Input
  {
    get => (Brush) this.GetValue(TransitionEffect.InputProperty);
    set => this.SetValue(TransitionEffect.InputProperty, (object) value);
  }

  public Brush OldImage
  {
    get => (Brush) this.GetValue(TransitionEffect.OldImageProperty);
    set => this.SetValue(TransitionEffect.OldImageProperty, (object) value);
  }

  public double Progress
  {
    get => (double) this.GetValue(TransitionEffect.ProgressProperty);
    set => this.SetValue(TransitionEffect.ProgressProperty, (object) value);
  }

  public TransitionEffect CloneCurrentValue() => (TransitionEffect) base.CloneCurrentValue();

  protected abstract TransitionEffect DeepCopy();
}
