// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SwitchPropertyEditor
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System.Windows;
using System.Windows.Controls.Primitives;

#nullable disable
namespace HandyControl.Controls;

public class SwitchPropertyEditor : PropertyEditorBase
{
  public override FrameworkElement CreateElement(PropertyItem propertyItem)
  {
    ToggleButton element = new ToggleButton();
    element.Style = ResourceHelper.GetResourceInternal<Style>("ToggleButtonSwitch");
    element.HorizontalAlignment = HorizontalAlignment.Left;
    element.IsEnabled = !propertyItem.IsReadOnly;
    return (FrameworkElement) element;
  }

  public override DependencyProperty GetDependencyProperty() => ToggleButton.IsCheckedProperty;
}
