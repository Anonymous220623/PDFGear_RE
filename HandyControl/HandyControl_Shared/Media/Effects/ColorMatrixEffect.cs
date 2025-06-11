// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Effects.ColorMatrixEffect
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Media.Effects;

#nullable disable
namespace HandyControl.Media.Effects;

public class ColorMatrixEffect : EffectBase
{
  private static readonly PixelShader Shader;
  public static readonly DependencyProperty M11Property = DependencyProperty.Register(nameof (M11), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double1Box, ShaderEffect.PixelShaderConstantCallback(0)));
  public static readonly DependencyProperty M21Property = DependencyProperty.Register(nameof (M21), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(1)));
  public static readonly DependencyProperty M31Property = DependencyProperty.Register(nameof (M31), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(2)));
  public static readonly DependencyProperty M41Property = DependencyProperty.Register(nameof (M41), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(3)));
  public static readonly DependencyProperty M51Property = DependencyProperty.Register(nameof (M51), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(4)));
  public static readonly DependencyProperty M12Property = DependencyProperty.Register(nameof (M12), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(5)));
  public static readonly DependencyProperty M22Property = DependencyProperty.Register(nameof (M22), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double1Box, ShaderEffect.PixelShaderConstantCallback(6)));
  public static readonly DependencyProperty M32Property = DependencyProperty.Register(nameof (M32), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(7)));
  public static readonly DependencyProperty M42Property = DependencyProperty.Register(nameof (M42), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(8)));
  public static readonly DependencyProperty M52Property = DependencyProperty.Register(nameof (M52), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(9)));
  public static readonly DependencyProperty M13Property = DependencyProperty.Register(nameof (M13), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(10)));
  public static readonly DependencyProperty M23Property = DependencyProperty.Register(nameof (M23), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(11)));
  public static readonly DependencyProperty M33Property = DependencyProperty.Register(nameof (M33), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double1Box, ShaderEffect.PixelShaderConstantCallback(12)));
  public static readonly DependencyProperty M43Property = DependencyProperty.Register(nameof (M43), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(13)));
  public static readonly DependencyProperty M53Property = DependencyProperty.Register(nameof (M53), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(14)));
  public static readonly DependencyProperty M14Property = DependencyProperty.Register(nameof (M14), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(15)));
  public static readonly DependencyProperty M24Property = DependencyProperty.Register(nameof (M24), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(16 /*0x10*/)));
  public static readonly DependencyProperty M34Property = DependencyProperty.Register(nameof (M34), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(17)));
  public static readonly DependencyProperty M44Property = DependencyProperty.Register(nameof (M44), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double1Box, ShaderEffect.PixelShaderConstantCallback(18)));
  public static readonly DependencyProperty M54Property = DependencyProperty.Register(nameof (M54), typeof (double), typeof (ColorMatrixEffect), new PropertyMetadata(ValueBoxes.Double0Box, ShaderEffect.PixelShaderConstantCallback(19)));

  static ColorMatrixEffect()
  {
    ColorMatrixEffect.Shader = new PixelShader()
    {
      UriSource = new Uri("pack://application:,,,/HandyControl;component/Resources/Effects/ColorMatrixEffect.ps")
    };
  }

  public ColorMatrixEffect()
  {
    this.PixelShader = ColorMatrixEffect.Shader;
    this.UpdateShaderValue(EffectBase.InputProperty);
    this.UpdateShaderValue(ColorMatrixEffect.M11Property);
    this.UpdateShaderValue(ColorMatrixEffect.M21Property);
    this.UpdateShaderValue(ColorMatrixEffect.M31Property);
    this.UpdateShaderValue(ColorMatrixEffect.M41Property);
    this.UpdateShaderValue(ColorMatrixEffect.M51Property);
    this.UpdateShaderValue(ColorMatrixEffect.M12Property);
    this.UpdateShaderValue(ColorMatrixEffect.M22Property);
    this.UpdateShaderValue(ColorMatrixEffect.M32Property);
    this.UpdateShaderValue(ColorMatrixEffect.M42Property);
    this.UpdateShaderValue(ColorMatrixEffect.M52Property);
    this.UpdateShaderValue(ColorMatrixEffect.M13Property);
    this.UpdateShaderValue(ColorMatrixEffect.M23Property);
    this.UpdateShaderValue(ColorMatrixEffect.M33Property);
    this.UpdateShaderValue(ColorMatrixEffect.M43Property);
    this.UpdateShaderValue(ColorMatrixEffect.M53Property);
    this.UpdateShaderValue(ColorMatrixEffect.M14Property);
    this.UpdateShaderValue(ColorMatrixEffect.M24Property);
    this.UpdateShaderValue(ColorMatrixEffect.M34Property);
    this.UpdateShaderValue(ColorMatrixEffect.M44Property);
    this.UpdateShaderValue(ColorMatrixEffect.M54Property);
  }

  public double M11
  {
    get => (double) this.GetValue(ColorMatrixEffect.M11Property);
    set => this.SetValue(ColorMatrixEffect.M11Property, (object) value);
  }

  public double M21
  {
    get => (double) this.GetValue(ColorMatrixEffect.M21Property);
    set => this.SetValue(ColorMatrixEffect.M21Property, (object) value);
  }

  public double M31
  {
    get => (double) this.GetValue(ColorMatrixEffect.M31Property);
    set => this.SetValue(ColorMatrixEffect.M31Property, (object) value);
  }

  public double M41
  {
    get => (double) this.GetValue(ColorMatrixEffect.M41Property);
    set => this.SetValue(ColorMatrixEffect.M41Property, (object) value);
  }

  public double M51
  {
    get => (double) this.GetValue(ColorMatrixEffect.M51Property);
    set => this.SetValue(ColorMatrixEffect.M51Property, (object) value);
  }

  public double M12
  {
    get => (double) this.GetValue(ColorMatrixEffect.M12Property);
    set => this.SetValue(ColorMatrixEffect.M12Property, (object) value);
  }

  public double M22
  {
    get => (double) this.GetValue(ColorMatrixEffect.M22Property);
    set => this.SetValue(ColorMatrixEffect.M22Property, (object) value);
  }

  public double M32
  {
    get => (double) this.GetValue(ColorMatrixEffect.M32Property);
    set => this.SetValue(ColorMatrixEffect.M32Property, (object) value);
  }

  public double M42
  {
    get => (double) this.GetValue(ColorMatrixEffect.M42Property);
    set => this.SetValue(ColorMatrixEffect.M42Property, (object) value);
  }

  public double M52
  {
    get => (double) this.GetValue(ColorMatrixEffect.M52Property);
    set => this.SetValue(ColorMatrixEffect.M52Property, (object) value);
  }

  public double M13
  {
    get => (double) this.GetValue(ColorMatrixEffect.M13Property);
    set => this.SetValue(ColorMatrixEffect.M13Property, (object) value);
  }

  public double M23
  {
    get => (double) this.GetValue(ColorMatrixEffect.M23Property);
    set => this.SetValue(ColorMatrixEffect.M23Property, (object) value);
  }

  public double M33
  {
    get => (double) this.GetValue(ColorMatrixEffect.M33Property);
    set => this.SetValue(ColorMatrixEffect.M33Property, (object) value);
  }

  public double M43
  {
    get => (double) this.GetValue(ColorMatrixEffect.M43Property);
    set => this.SetValue(ColorMatrixEffect.M43Property, (object) value);
  }

  public double M53
  {
    get => (double) this.GetValue(ColorMatrixEffect.M53Property);
    set => this.SetValue(ColorMatrixEffect.M53Property, (object) value);
  }

  public double M14
  {
    get => (double) this.GetValue(ColorMatrixEffect.M14Property);
    set => this.SetValue(ColorMatrixEffect.M14Property, (object) value);
  }

  public double M24
  {
    get => (double) this.GetValue(ColorMatrixEffect.M24Property);
    set => this.SetValue(ColorMatrixEffect.M24Property, (object) value);
  }

  public double M34
  {
    get => (double) this.GetValue(ColorMatrixEffect.M34Property);
    set => this.SetValue(ColorMatrixEffect.M34Property, (object) value);
  }

  public double M44
  {
    get => (double) this.GetValue(ColorMatrixEffect.M44Property);
    set => this.SetValue(ColorMatrixEffect.M44Property, (object) value);
  }

  public double M54
  {
    get => (double) this.GetValue(ColorMatrixEffect.M54Property);
    set => this.SetValue(ColorMatrixEffect.M54Property, (object) value);
  }
}
