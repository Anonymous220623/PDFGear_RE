// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.ControlBehavior
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Controls;

public class ControlBehavior : VisualStateBehavior
{
  protected internal override Type TargetType => typeof (Control);

  protected override void OnAttach(Control control)
  {
    control.Loaded += (RoutedEventHandler) ((sender, e) => this.UpdateState(control, false));
    VisualStateBehavior.AddValueChanged(UIElement.IsKeyboardFocusWithinProperty, typeof (Control), (object) control, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void OnDetach(Control control)
  {
    VisualStateBehavior.RemoveValueChanged(UIElement.IsKeyboardFocusWithinProperty, typeof (Control), (object) control, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void UpdateStateHandler(object o, EventArgs e)
  {
    if (!(o is Control control))
      throw new InvalidOperationException("This should never be used on anything other than a control.");
    this.UpdateState(control, true);
  }

  protected override void UpdateState(Control control, bool useTransitions)
  {
    if (control.IsKeyboardFocusWithin)
      Syncfusion.Windows.VisualStateManager.GoToState(control, "Focused", useTransitions);
    else
      Syncfusion.Windows.VisualStateManager.GoToState(control, "Unfocused", useTransitions);
  }
}
