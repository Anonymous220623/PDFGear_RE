// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.BorderElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Converter;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Controls;

public class BorderElement
{
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof (CornerRadius), typeof (BorderElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(), FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty CircularProperty = DependencyProperty.RegisterAttached("Circular", typeof (bool), typeof (BorderElement), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(BorderElement.OnCircularChanged)));

  public static void SetCornerRadius(DependencyObject element, CornerRadius value)
  {
    element.SetValue(BorderElement.CornerRadiusProperty, (object) value);
  }

  public static CornerRadius GetCornerRadius(DependencyObject element)
  {
    return (CornerRadius) element.GetValue(BorderElement.CornerRadiusProperty);
  }

  private static void OnCircularChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Border target))
      return;
    if ((bool) e.NewValue)
    {
      MultiBinding binding = new MultiBinding()
      {
        Converter = (IMultiValueConverter) new BorderCircularConverter()
      };
      binding.Bindings.Add((BindingBase) new Binding(FrameworkElement.ActualWidthProperty.Name)
      {
        Source = (object) target
      });
      binding.Bindings.Add((BindingBase) new Binding(FrameworkElement.ActualHeightProperty.Name)
      {
        Source = (object) target
      });
      target.SetBinding(Border.CornerRadiusProperty, (BindingBase) binding);
    }
    else
    {
      BindingOperations.ClearBinding((DependencyObject) target, FrameworkElement.ActualWidthProperty);
      BindingOperations.ClearBinding((DependencyObject) target, FrameworkElement.ActualHeightProperty);
      BindingOperations.ClearBinding((DependencyObject) target, Border.CornerRadiusProperty);
    }
  }

  public static void SetCircular(DependencyObject element, bool value)
  {
    element.SetValue(BorderElement.CircularProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetCircular(DependencyObject element)
  {
    return (bool) element.GetValue(BorderElement.CircularProperty);
  }
}
