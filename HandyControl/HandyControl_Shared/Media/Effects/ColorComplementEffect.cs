// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Effects.ColorComplementEffect
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows.Media.Effects;

#nullable disable
namespace HandyControl.Media.Effects;

public class ColorComplementEffect : EffectBase
{
  private static readonly PixelShader Shader = new PixelShader()
  {
    UriSource = new Uri("pack://application:,,,/HandyControl;component/Resources/Effects/ColorComplementEffect.ps")
  };

  public ColorComplementEffect()
  {
    this.PixelShader = ColorComplementEffect.Shader;
    this.UpdateShaderValue(EffectBase.InputProperty);
  }
}
