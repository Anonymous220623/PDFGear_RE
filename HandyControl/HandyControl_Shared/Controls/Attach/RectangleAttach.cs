// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.RectangleAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Converter;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Controls;

public class RectangleAttach
{
  public static readonly DependencyProperty CircularProperty = DependencyProperty.RegisterAttached("Circular", typeof (bool), typeof (RectangleAttach), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(RectangleAttach.OnCircularChanged)));

  private static void OnCircularChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Rectangle target))
      return;
    if ((bool) e.NewValue)
    {
      MultiBinding binding = new MultiBinding()
      {
        Converter = (IMultiValueConverter) new RectangleCircularConverter()
      };
      binding.Bindings.Add((BindingBase) new Binding(FrameworkElement.ActualWidthProperty.Name)
      {
        Source = (object) target
      });
      binding.Bindings.Add((BindingBase) new Binding(FrameworkElement.ActualHeightProperty.Name)
      {
        Source = (object) target
      });
      target.SetBinding(Rectangle.RadiusXProperty, (BindingBase) binding);
      target.SetBinding(Rectangle.RadiusYProperty, (BindingBase) binding);
    }
    else
    {
      BindingOperations.ClearBinding((DependencyObject) target, FrameworkElement.ActualWidthProperty);
      BindingOperations.ClearBinding((DependencyObject) target, FrameworkElement.ActualHeightProperty);
      BindingOperations.ClearBinding((DependencyObject) target, Rectangle.RadiusXProperty);
      BindingOperations.ClearBinding((DependencyObject) target, Rectangle.RadiusYProperty);
    }
  }

  public static void SetCircular(DependencyObject element, bool value)
  {
    element.SetValue(RectangleAttach.CircularProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetCircular(DependencyObject element)
  {
    return (bool) element.GetValue(RectangleAttach.CircularProperty);
  }
}
