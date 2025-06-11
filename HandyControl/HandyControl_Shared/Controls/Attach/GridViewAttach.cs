// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.GridViewAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class GridViewAttach
{
  public static readonly DependencyProperty ColumnHeaderHeightProperty = DependencyProperty.RegisterAttached("ColumnHeaderHeight", typeof (double), typeof (GridViewAttach), new PropertyMetadata(ValueBoxes.Double0Box));

  public static void SetColumnHeaderHeight(DependencyObject element, double value)
  {
    element.SetValue(GridViewAttach.ColumnHeaderHeightProperty, (object) value);
  }

  public static double GetColumnHeaderHeight(DependencyObject element)
  {
    return (double) element.GetValue(GridViewAttach.ColumnHeaderHeightProperty);
  }
}
