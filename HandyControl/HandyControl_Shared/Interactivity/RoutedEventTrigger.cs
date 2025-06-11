// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.RoutedEventTrigger
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;

#nullable disable
namespace HandyControl.Interactivity;

public class RoutedEventTrigger : EventTriggerBase<DependencyObject>
{
  public RoutedEvent RoutedEvent { get; set; }

  protected override void OnAttached()
  {
    Behavior associatedObject1 = this.AssociatedObject as Behavior;
    FrameworkElement associatedObject2 = this.AssociatedObject as FrameworkElement;
    if (associatedObject1 != null)
      associatedObject2 = ((IAttachedObject) associatedObject1).AssociatedObject as FrameworkElement;
    if (associatedObject2 == null)
      throw new ArgumentException();
    if (this.RoutedEvent == null)
      return;
    associatedObject2.AddHandler(this.RoutedEvent, (Delegate) new RoutedEventHandler(this.OnRoutedEvent));
  }

  private void OnRoutedEvent(object sender, RoutedEventArgs args) => this.OnEvent((EventArgs) args);

  protected override string GetEventName() => this.RoutedEvent.Name;
}
