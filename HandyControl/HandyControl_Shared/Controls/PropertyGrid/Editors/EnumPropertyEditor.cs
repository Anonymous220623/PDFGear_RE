// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.EnumPropertyEditor
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls.Primitives;

#nullable disable
namespace HandyControl.Controls;

public class EnumPropertyEditor : PropertyEditorBase
{
  public override FrameworkElement CreateElement(PropertyItem propertyItem)
  {
    System.Windows.Controls.ComboBox element = new System.Windows.Controls.ComboBox();
    element.IsEnabled = !propertyItem.IsReadOnly;
    element.ItemsSource = (IEnumerable) Enum.GetValues(propertyItem.PropertyType);
    return (FrameworkElement) element;
  }

  public override DependencyProperty GetDependencyProperty() => Selector.SelectedValueProperty;
}
