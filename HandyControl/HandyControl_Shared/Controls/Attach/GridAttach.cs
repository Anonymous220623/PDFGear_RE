// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.GridAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class GridAttach
{
  public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached("Name", typeof (string), typeof (GridAttach), new PropertyMetadata((object) null));
  public static readonly DependencyProperty RowNameProperty = DependencyProperty.RegisterAttached("RowName", typeof (string), typeof (GridAttach), new PropertyMetadata((object) null, new PropertyChangedCallback(GridAttach.OnRowNameChanged)));
  public static readonly DependencyProperty ColumnNameProperty = DependencyProperty.RegisterAttached("ColumnName", typeof (string), typeof (GridAttach), new PropertyMetadata((object) null, new PropertyChangedCallback(GridAttach.OnColumnNameChanged)));

  public static void SetName(DependencyObject element, string value)
  {
    element.SetValue(GridAttach.NameProperty, (object) value);
  }

  public static string GetName(DependencyObject element)
  {
    return (string) element.GetValue(GridAttach.NameProperty);
  }

  private static void OnRowNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FrameworkElement element) || !(e.NewValue is string newValue) || string.IsNullOrEmpty(newValue) || !(element.Parent is Grid parent))
      return;
    for (int index = 0; index < parent.RowDefinitions.Count; ++index)
    {
      string name = GridAttach.GetName((DependencyObject) parent.RowDefinitions[index]);
      if (!string.IsNullOrEmpty(name) && name.Equals(newValue, StringComparison.Ordinal))
      {
        Grid.SetRow((UIElement) element, index);
        break;
      }
    }
  }

  public static void SetRowName(DependencyObject element, string value)
  {
    element.SetValue(GridAttach.RowNameProperty, (object) value);
  }

  public static string GetRowName(DependencyObject element)
  {
    return (string) element.GetValue(GridAttach.RowNameProperty);
  }

  private static void OnColumnNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FrameworkElement element) || !(e.NewValue is string newValue) || string.IsNullOrEmpty(newValue) || !(element.Parent is Grid parent))
      return;
    for (int index = 0; index < parent.ColumnDefinitions.Count; ++index)
    {
      string name = GridAttach.GetName((DependencyObject) parent.ColumnDefinitions[index]);
      if (!string.IsNullOrEmpty(name) && name.Equals(newValue, StringComparison.Ordinal))
      {
        Grid.SetColumn((UIElement) element, index);
        break;
      }
    }
  }

  public static void SetColumnName(DependencyObject element, string value)
  {
    element.SetValue(GridAttach.ColumnNameProperty, (object) value);
  }

  public static string GetColumnName(DependencyObject element)
  {
    return (string) element.GetValue(GridAttach.ColumnNameProperty);
  }
}
