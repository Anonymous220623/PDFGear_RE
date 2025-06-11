// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Effects.BrightnessEffect
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Media.Effects;

#nullable disable
namespace HandyControl.Media.Effects;

public class BrightnessEffect : EffectBase
{
  private static readonly PixelShader Shader;
  public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register(nameof (Brightness), typeof (double), typeof (BrightnessEffect), new PropertyMetadata(ValueBoxes.Double1Box, ShaderEffect.PixelShaderConstantCallback(0)));

  static BrightnessEffect()
  {
    BrightnessEffect.Shader = new PixelShader()
    {
      UriSource = new Uri("pack://application:,,,/HandyControl;component/Resources/Effects/BrightnessEffect.ps")
    };
  }

  public BrightnessEffect()
  {
    this.PixelShader = BrightnessEffect.Shader;
    this.UpdateShaderValue(EffectBase.InputProperty);
    this.UpdateShaderValue(BrightnessEffect.BrightnessProperty);
  }

  public double Brightness
  {
    get => (double) this.GetValue(BrightnessEffect.BrightnessProperty);
    set => this.SetValue(BrightnessEffect.BrightnessProperty, (object) value);
  }
}
