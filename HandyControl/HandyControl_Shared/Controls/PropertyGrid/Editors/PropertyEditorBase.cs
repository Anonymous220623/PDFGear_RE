// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PropertyEditorBase
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Controls;

public abstract class PropertyEditorBase : DependencyObject
{
  public abstract FrameworkElement CreateElement(PropertyItem propertyItem);

  public virtual void CreateBinding(PropertyItem propertyItem, DependencyObject element)
  {
    BindingOperations.SetBinding(element, this.GetDependencyProperty(), (BindingBase) new Binding(propertyItem.PropertyName ?? "")
    {
      Source = propertyItem.Value,
      Mode = this.GetBindingMode(propertyItem),
      UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyItem),
      Converter = this.GetConverter(propertyItem)
    });
  }

  public abstract DependencyProperty GetDependencyProperty();

  public virtual BindingMode GetBindingMode(PropertyItem propertyItem)
  {
    return !propertyItem.IsReadOnly ? BindingMode.TwoWay : BindingMode.OneWay;
  }

  public virtual UpdateSourceTrigger GetUpdateSourceTrigger(PropertyItem propertyItem)
  {
    return UpdateSourceTrigger.PropertyChanged;
  }

  protected virtual IValueConverter GetConverter(PropertyItem propertyItem)
  {
    return (IValueConverter) null;
  }
}
