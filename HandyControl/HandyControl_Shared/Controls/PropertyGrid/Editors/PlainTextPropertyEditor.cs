// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PlainTextPropertyEditor
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class PlainTextPropertyEditor : PropertyEditorBase
{
  public override FrameworkElement CreateElement(PropertyItem propertyItem)
  {
    System.Windows.Controls.TextBox element = new System.Windows.Controls.TextBox();
    element.IsReadOnly = propertyItem.IsReadOnly;
    return (FrameworkElement) element;
  }

  public override DependencyProperty GetDependencyProperty() => System.Windows.Controls.TextBox.TextProperty;
}
