// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.MenuItemAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Extension;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class MenuItemAttach
{
  public static readonly DependencyProperty GroupNameProperty = DependencyProperty.RegisterAttached("GroupName", typeof (string), typeof (MenuItemAttach), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(MenuItemAttach.OnGroupNameChanged)));

  [AttachedPropertyBrowsableForType(typeof (MenuItem))]
  public static string GetGroupName(DependencyObject obj)
  {
    return (string) obj.GetValue(MenuItemAttach.GroupNameProperty);
  }

  [AttachedPropertyBrowsableForType(typeof (MenuItem))]
  public static void SetGroupName(DependencyObject obj, string value)
  {
    obj.SetValue(MenuItemAttach.GroupNameProperty, (object) value);
  }

  private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is MenuItem menuItem))
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    menuItem.Checked -= MenuItemAttach.\u003C\u003EO.\u003C0\u003E__MenuItem_Checked ?? (MenuItemAttach.\u003C\u003EO.\u003C0\u003E__MenuItem_Checked = new RoutedEventHandler(MenuItemAttach.MenuItem_Checked));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    menuItem.Click -= MenuItemAttach.\u003C\u003EO.\u003C1\u003E__MenuItem_Click ?? (MenuItemAttach.\u003C\u003EO.\u003C1\u003E__MenuItem_Click = new RoutedEventHandler(MenuItemAttach.MenuItem_Click));
    if (string.IsNullOrWhiteSpace(e.NewValue.ToString()))
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    menuItem.Checked += MenuItemAttach.\u003C\u003EO.\u003C0\u003E__MenuItem_Checked ?? (MenuItemAttach.\u003C\u003EO.\u003C0\u003E__MenuItem_Checked = new RoutedEventHandler(MenuItemAttach.MenuItem_Checked));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    menuItem.Click += MenuItemAttach.\u003C\u003EO.\u003C1\u003E__MenuItem_Click ?? (MenuItemAttach.\u003C\u003EO.\u003C1\u003E__MenuItem_Click = new RoutedEventHandler(MenuItemAttach.MenuItem_Click));
  }

  private static void MenuItem_Checked(object sender, RoutedEventArgs e)
  {
    MenuItem menuItem = sender as MenuItem;
    if (menuItem == null || !(menuItem.Parent is MenuItem parent))
      return;
    string groupName = MenuItemAttach.GetGroupName((DependencyObject) menuItem);
    parent.Items.OfType<MenuItem>().Where<MenuItem>((Func<MenuItem, bool>) (item => item != menuItem && item.IsCheckable && string.Equals(MenuItemAttach.GetGroupName((DependencyObject) item), groupName))).Do<MenuItem>((Action<MenuItem>) (item => item.IsChecked = false));
  }

  private static void MenuItem_Click(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is MenuItem originalSource) || originalSource.IsChecked)
      return;
    originalSource.IsChecked = true;
  }
}
