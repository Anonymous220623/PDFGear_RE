// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.MenuAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class MenuAttach
{
  public static readonly DependencyProperty PopupVerticalOffsetProperty = DependencyProperty.RegisterAttached("PopupVerticalOffset", typeof (double), typeof (MenuAttach), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty PopupHorizontalOffsetProperty = DependencyProperty.RegisterAttached("PopupHorizontalOffset", typeof (double), typeof (MenuAttach), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.RegisterAttached("ItemPadding", typeof (Thickness), typeof (MenuAttach), new PropertyMetadata((object) new Thickness()));

  public static void SetPopupVerticalOffset(DependencyObject element, double value)
  {
    element.SetValue(MenuAttach.PopupVerticalOffsetProperty, (object) value);
  }

  public static double GetPopupVerticalOffset(DependencyObject element)
  {
    return (double) element.GetValue(MenuAttach.PopupVerticalOffsetProperty);
  }

  public static void SetPopupHorizontalOffset(DependencyObject element, double value)
  {
    element.SetValue(MenuAttach.PopupHorizontalOffsetProperty, (object) value);
  }

  public static double GetPopupHorizontalOffset(DependencyObject element)
  {
    return (double) element.GetValue(MenuAttach.PopupHorizontalOffsetProperty);
  }

  public static void SetItemPadding(DependencyObject element, Thickness value)
  {
    element.SetValue(MenuAttach.ItemPaddingProperty, (object) value);
  }

  public static Thickness GetItemPadding(DependencyObject element)
  {
    return (Thickness) element.GetValue(MenuAttach.ItemPaddingProperty);
  }
}
