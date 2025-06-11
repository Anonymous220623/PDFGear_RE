// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.CalendarSelectionChangedEventArgs
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Controls;

internal class CalendarSelectionChangedEventArgs(
  RoutedEvent eventId,
  IList removedItems,
  IList addedItems) : SelectionChangedEventArgs(eventId, removedItems, addedItems)
{
  protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
  {
    if (genericHandler is EventHandler<SelectionChangedEventArgs> eventHandler)
      eventHandler(genericTarget, (SelectionChangedEventArgs) this);
    else
      base.InvokeEventHandler(genericHandler, genericTarget);
  }
}
