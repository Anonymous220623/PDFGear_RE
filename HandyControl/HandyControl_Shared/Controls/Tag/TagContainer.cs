// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TagContainer
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Extension;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class TagContainer : ItemsControl
{
  public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.RegisterAttached("ShowCloseButton", typeof (bool), typeof (TagContainer), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));

  public TagContainer()
  {
    this.AddHandler(HandyControl.Controls.Tag.ClosedEvent, (Delegate) new RoutedEventHandler(this.Tag_OnClosed));
  }

  public static void SetShowCloseButton(DependencyObject element, bool value)
  {
    element.SetValue(TagContainer.ShowCloseButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetShowCloseButton(DependencyObject element)
  {
    return (bool) element.GetValue(TagContainer.ShowCloseButtonProperty);
  }

  private void Tag_OnClosed(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is HandyControl.Controls.Tag originalSource))
      return;
    originalSource.Hide();
    if (this.ItemsSource == null)
    {
      this.Items.Remove((object) originalSource);
    }
    else
    {
      object obj = this.ItemContainerGenerator.ItemFromContainer((DependencyObject) originalSource);
      this.GetActualList()?.Remove(obj);
      this.Items.Refresh();
    }
  }

  internal IList GetActualList()
  {
    return this.ItemsSource == null ? (IList) this.Items : this.ItemsSource as IList;
  }

  protected override DependencyObject GetContainerForItemOverride() => (DependencyObject) new HandyControl.Controls.Tag();

  protected override bool IsItemItsOwnContainerOverride(object item) => item is HandyControl.Controls.Tag;
}
