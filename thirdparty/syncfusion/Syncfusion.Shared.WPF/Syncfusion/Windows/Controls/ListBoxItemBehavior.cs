// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.ListBoxItemBehavior
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Controls;

public class ListBoxItemBehavior : ControlBehavior
{
  protected internal override Type TargetType => typeof (ListBoxItem);

  protected override void OnAttach(Control control)
  {
    base.OnAttach(control);
    ListBoxItem instance = (ListBoxItem) control;
    Type targetType = typeof (ListBoxItem);
    VisualStateBehavior.AddValueChanged(UIElement.IsMouseOverProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
    VisualStateBehavior.AddValueChanged(ListBoxItem.IsSelectedProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void OnDetach(Control control)
  {
    base.OnDetach(control);
    ListBoxItem instance = (ListBoxItem) control;
    Type targetType = typeof (ListBoxItem);
    VisualStateBehavior.RemoveValueChanged(UIElement.IsMouseOverProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
    VisualStateBehavior.RemoveValueChanged(ListBoxItem.IsSelectedProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void UpdateState(Control control, bool useTransitions)
  {
    ListBoxItem listBoxItem = (ListBoxItem) control;
    if (listBoxItem.IsMouseOver)
      Syncfusion.Windows.VisualStateManager.GoToState((Control) listBoxItem, "MouseOver", useTransitions);
    else
      Syncfusion.Windows.VisualStateManager.GoToState((Control) listBoxItem, "Normal", useTransitions);
    if (listBoxItem.IsSelected)
      Syncfusion.Windows.VisualStateManager.GoToState((Control) listBoxItem, "Selected", useTransitions);
    else
      Syncfusion.Windows.VisualStateManager.GoToState((Control) listBoxItem, "Unselected", useTransitions);
    base.UpdateState(control, useTransitions);
  }
}
