// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PropertyItem
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class PropertyItem : ListBoxItem
{
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (object), typeof (PropertyItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof (DisplayName), typeof (string), typeof (PropertyItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(nameof (PropertyName), typeof (string), typeof (PropertyItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(nameof (PropertyType), typeof (Type), typeof (PropertyItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty PropertyTypeNameProperty = DependencyProperty.Register(nameof (PropertyTypeName), typeof (string), typeof (PropertyItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof (Description), typeof (string), typeof (PropertyItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof (IsReadOnly), typeof (bool), typeof (PropertyItem), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(nameof (DefaultValue), typeof (object), typeof (PropertyItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty CategoryProperty = DependencyProperty.Register(nameof (Category), typeof (string), typeof (PropertyItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(nameof (Editor), typeof (PropertyEditorBase), typeof (PropertyItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty EditorElementProperty = DependencyProperty.Register(nameof (EditorElement), typeof (FrameworkElement), typeof (PropertyItem), new PropertyMetadata((object) null));
  public static readonly DependencyProperty IsExpandedEnabledProperty = DependencyProperty.Register(nameof (IsExpandedEnabled), typeof (bool), typeof (PropertyItem), new PropertyMetadata(ValueBoxes.FalseBox));

  public object Value
  {
    get => this.GetValue(PropertyItem.ValueProperty);
    set => this.SetValue(PropertyItem.ValueProperty, value);
  }

  public string DisplayName
  {
    get => (string) this.GetValue(PropertyItem.DisplayNameProperty);
    set => this.SetValue(PropertyItem.DisplayNameProperty, (object) value);
  }

  public string PropertyName
  {
    get => (string) this.GetValue(PropertyItem.PropertyNameProperty);
    set => this.SetValue(PropertyItem.PropertyNameProperty, (object) value);
  }

  public Type PropertyType
  {
    get => (Type) this.GetValue(PropertyItem.PropertyTypeProperty);
    set => this.SetValue(PropertyItem.PropertyTypeProperty, (object) value);
  }

  public string PropertyTypeName
  {
    get => (string) this.GetValue(PropertyItem.PropertyTypeNameProperty);
    set => this.SetValue(PropertyItem.PropertyTypeNameProperty, (object) value);
  }

  public string Description
  {
    get => (string) this.GetValue(PropertyItem.DescriptionProperty);
    set => this.SetValue(PropertyItem.DescriptionProperty, (object) value);
  }

  public bool IsReadOnly
  {
    get => (bool) this.GetValue(PropertyItem.IsReadOnlyProperty);
    set => this.SetValue(PropertyItem.IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
  }

  public object DefaultValue
  {
    get => this.GetValue(PropertyItem.DefaultValueProperty);
    set => this.SetValue(PropertyItem.DefaultValueProperty, value);
  }

  public string Category
  {
    get => (string) this.GetValue(PropertyItem.CategoryProperty);
    set => this.SetValue(PropertyItem.CategoryProperty, (object) value);
  }

  public PropertyEditorBase Editor
  {
    get => (PropertyEditorBase) this.GetValue(PropertyItem.EditorProperty);
    set => this.SetValue(PropertyItem.EditorProperty, (object) value);
  }

  public FrameworkElement EditorElement
  {
    get => (FrameworkElement) this.GetValue(PropertyItem.EditorElementProperty);
    set => this.SetValue(PropertyItem.EditorElementProperty, (object) value);
  }

  public bool IsExpandedEnabled
  {
    get => (bool) this.GetValue(PropertyItem.IsExpandedEnabledProperty);
    set => this.SetValue(PropertyItem.IsExpandedEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  public PropertyDescriptor PropertyDescriptor { get; set; }

  public virtual void InitElement()
  {
    if (this.Editor == null)
      return;
    this.EditorElement = this.Editor.CreateElement(this);
    this.Editor.CreateBinding(this, (DependencyObject) this.EditorElement);
  }
}
