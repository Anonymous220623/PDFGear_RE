// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Effects.GrayScaleEffect
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Media.Effects;

#nullable disable
namespace HandyControl.Media.Effects;

public class GrayScaleEffect : EffectBase
{
  private static readonly PixelShader Shader;
  public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof (Scale), typeof (double), typeof (GrayScaleEffect), new PropertyMetadata(ValueBoxes.Double1Box, ShaderEffect.PixelShaderConstantCallback(0)));

  static GrayScaleEffect()
  {
    GrayScaleEffect.Shader = new PixelShader()
    {
      UriSource = new Uri("pack://application:,,,/HandyControl;component/Resources/Effects/GrayScaleEffect.ps")
    };
  }

  public GrayScaleEffect()
  {
    this.PixelShader = GrayScaleEffect.Shader;
    this.UpdateShaderValue(EffectBase.InputProperty);
    this.UpdateShaderValue(GrayScaleEffect.ScaleProperty);
  }

  public double Scale
  {
    get => (double) this.GetValue(GrayScaleEffect.ScaleProperty);
    set => this.SetValue(GrayScaleEffect.ScaleProperty, (object) value);
  }
}
