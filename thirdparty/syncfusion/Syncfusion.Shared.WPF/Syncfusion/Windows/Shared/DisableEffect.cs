// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DisableEffect
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class DisableEffect : ShaderEffect
{
  public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty(nameof (Input), typeof (DisableEffect), 0);
  public static readonly DependencyProperty FilterColorProperty = DependencyProperty.Register(nameof (FilterColor), typeof (Color), typeof (DisableEffect), (PropertyMetadata) new UIPropertyMetadata((object) Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), ShaderEffect.PixelShaderConstantCallback(0)));

  public DisableEffect()
  {
    PixelShader pixelShader = new PixelShader();
    int num = (bool) DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof (FrameworkElement)).Metadata.DefaultValue ? 1 : 0;
    pixelShader.UriSource = new Uri("/Syncfusion.Shared.WPF;component/Utils/Effects/Disable.ps", UriKind.RelativeOrAbsolute);
    this.PixelShader = pixelShader;
    this.UpdateShaderValue(DisableEffect.InputProperty);
    this.UpdateShaderValue(DisableEffect.FilterColorProperty);
  }

  public Brush Input
  {
    get => (Brush) this.GetValue(DisableEffect.InputProperty);
    set => this.SetValue(DisableEffect.InputProperty, (object) value);
  }

  public Color FilterColor
  {
    get => (Color) this.GetValue(DisableEffect.FilterColorProperty);
    set => this.SetValue(DisableEffect.FilterColorProperty, (object) value);
  }
}
