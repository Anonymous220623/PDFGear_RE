// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.HSVBackgroundBrushExtension
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[MarkupExtensionReturnType(typeof (Brush))]
[DesignTimeVisible(false)]
public class HSVBackgroundBrushExtension : MarkupExtension
{
  private static ColorToHSVBackgroundConverter m_converter = new ColorToHSVBackgroundConverter();
  private string m_key;

  public HSVBackgroundBrushExtension(string key) => this.m_key = key;

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    return (object) new MultiBinding()
    {
      Converter = (IMultiValueConverter) HSVBackgroundBrushExtension.m_converter,
      ConverterParameter = (object) this.m_key,
      Bindings = {
        (BindingBase) new Binding()
        {
          RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (ColorEdit), 1),
          Path = new PropertyPath((object) ColorEdit.HSVProperty)
        },
        (BindingBase) new Binding()
        {
          RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (ColorEdit), 1),
          Path = new PropertyPath((object) ColorEdit.HProperty)
        },
        (BindingBase) new Binding()
        {
          RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (ColorEdit), 1),
          Path = new PropertyPath((object) ColorEdit.SProperty)
        },
        (BindingBase) new Binding()
        {
          RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (ColorEdit), 1),
          Path = new PropertyPath((object) ColorEdit.VProperty)
        }
      }
    };
  }
}
