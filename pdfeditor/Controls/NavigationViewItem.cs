// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.NavigationViewItem
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls;

public class NavigationViewItem : ListBoxItem
{
  public static readonly RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent("ItemClicked", RoutingStrategy.Bubble, typeof (EventHandler<NavigationViewItemClickEventArgs>), typeof (NavigationViewItem));

  static NavigationViewItem()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (NavigationViewItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (NavigationViewItem)));
  }

  public event EventHandler<NavigationViewItemClickEventArgs> ItemClicked
  {
    add => this.AddHandler(NavigationViewItem.ItemClickedEvent, (Delegate) value);
    remove => this.RemoveHandler(NavigationViewItem.ItemClickedEvent, (Delegate) value);
  }

  protected virtual void OnItemClickedEvent(DependencyObject element, object item)
  {
    this.RaiseEvent((RoutedEventArgs) new NavigationViewItemClickEventArgs(item, NavigationViewItem.ItemClickedEvent, (object) this));
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.Content != null)
      this.OnItemClickedEvent((DependencyObject) this, this.Content);
    base.OnMouseLeftButtonDown(e);
  }
}
