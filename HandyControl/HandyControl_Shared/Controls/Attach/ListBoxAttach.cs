// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ListBoxAttach
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class ListBoxAttach
{
  public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof (IList), typeof (ListBoxAttach), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ListBoxAttach.OnSelectedItemsChanged)));
  internal static readonly DependencyProperty InternalActionProperty = DependencyProperty.RegisterAttached("InternalAction", typeof (bool), typeof (ListBoxAttach), new PropertyMetadata(ValueBoxes.FalseBox));

  public static void SetSelectedItems(DependencyObject element, IList value)
  {
    element.SetValue(ListBoxAttach.SelectedItemsProperty, (object) value);
  }

  public static IList GetSelectedItems(DependencyObject element)
  {
    return (IList) element.GetValue(ListBoxAttach.SelectedItemsProperty);
  }

  internal static void SetInternalAction(DependencyObject element, bool value)
  {
    element.SetValue(ListBoxAttach.InternalActionProperty, ValueBoxes.BooleanBox(value));
  }

  internal static bool GetInternalAction(DependencyObject element)
  {
    return (bool) element.GetValue(ListBoxAttach.InternalActionProperty);
  }

  private static void OnSelectedItemsChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ListBox element) || ListBoxAttach.GetInternalAction((DependencyObject) element))
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.SelectionChanged -= ListBoxAttach.\u003C\u003EO.\u003C0\u003E__OnListBoxSelectionChanged ?? (ListBoxAttach.\u003C\u003EO.\u003C0\u003E__OnListBoxSelectionChanged = new SelectionChangedEventHandler(ListBoxAttach.OnListBoxSelectionChanged));
    element.SelectedItems.Clear();
    if (e.NewValue is IList newValue)
    {
      foreach (object obj in (IEnumerable) newValue)
        element.SelectedItems.Add(obj);
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    element.SelectionChanged += ListBoxAttach.\u003C\u003EO.\u003C0\u003E__OnListBoxSelectionChanged ?? (ListBoxAttach.\u003C\u003EO.\u003C0\u003E__OnListBoxSelectionChanged = new SelectionChangedEventHandler(ListBoxAttach.OnListBoxSelectionChanged));
  }

  private static void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (!(sender is ListBox element))
      return;
    ListBoxAttach.SetInternalAction((DependencyObject) element, true);
    ListBoxAttach.SetSelectedItems((DependencyObject) element, (IList) element.SelectedItems.Cast<object>().ToArray<object>());
    ListBoxAttach.SetInternalAction((DependencyObject) element, false);
  }
}
