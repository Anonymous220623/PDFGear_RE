// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Effects.ContrastEffect
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Media.Effects;

#nullable disable
namespace HandyControl.Media.Effects;

public class ContrastEffect : EffectBase
{
  private static readonly PixelShader Shader;
  public static readonly DependencyProperty ContrastProperty = DependencyProperty.Register(nameof (Contrast), typeof (double), typeof (ContrastEffect), new PropertyMetadata(ValueBoxes.Double1Box, ShaderEffect.PixelShaderConstantCallback(0)));

  static ContrastEffect()
  {
    ContrastEffect.Shader = new PixelShader()
    {
      UriSource = new Uri("pack://application:,,,/HandyControl;component/Resources/Effects/ContrastEffect.ps")
    };
  }

  public ContrastEffect()
  {
    this.PixelShader = ContrastEffect.Shader;
    this.UpdateShaderValue(EffectBase.InputProperty);
    this.UpdateShaderValue(ContrastEffect.ContrastProperty);
  }

  public double Contrast
  {
    get => (double) this.GetValue(ContrastEffect.ContrastProperty);
    set => this.SetValue(ContrastEffect.ContrastProperty, (object) value);
  }
}
