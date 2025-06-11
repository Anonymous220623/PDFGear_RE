// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ScRGBColorExtension
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Tools;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class ScRGBColorExtension : MarkupExtension
{
  private static RGBToStringConverter m_converter = new RGBToStringConverter();
  private string m_key;

  public ScRGBColorExtension(string key) => this.m_key = key;

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    return (object) new MultiBinding()
    {
      ConverterParameter = (object) this.m_key,
      Converter = (IMultiValueConverter) ScRGBColorExtension.m_converter,
      UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
      Bindings = {
        (BindingBase) new Binding()
        {
          RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (ColorEdit), 1),
          Path = new PropertyPath((object) ColorEdit.ColorProperty)
        },
        (BindingBase) new Binding()
        {
          RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (ColorEdit), 1),
          Path = new PropertyPath((object) ColorEdit.IsScRGBColorProperty)
        },
        (BindingBase) new Binding()
        {
          RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof (ColorEdit), 1),
          Path = new PropertyPath((object) ColorEdit.VisualizationStyleProperty)
        }
      }
    };
  }
}
