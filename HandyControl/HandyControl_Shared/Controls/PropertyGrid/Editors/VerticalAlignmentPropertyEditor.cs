// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.VerticalAlignmentPropertyEditor
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace HandyControl.Controls;

public class VerticalAlignmentPropertyEditor : PropertyEditorBase
{
  public override FrameworkElement CreateElement(PropertyItem propertyItem)
  {
    System.Windows.Controls.ComboBox element = new System.Windows.Controls.ComboBox();
    element.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("ComboBoxCapsule");
    element.ItemsSource = (IEnumerable) Enum.GetValues(propertyItem.PropertyType);
    element.ItemTemplateSelector = HandyControl.Tools.ResourceHelper.GetResourceInternal<DataTemplateSelector>("VerticalAlignmentPathTemplateSelector");
    element.HorizontalAlignment = HorizontalAlignment.Left;
    return (FrameworkElement) element;
  }

  public override DependencyProperty GetDependencyProperty() => Selector.SelectedValueProperty;
}
