// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.ButtonBaseBehavior
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Syncfusion.Windows.Controls;

public class ButtonBaseBehavior : ControlBehavior
{
  protected internal override Type TargetType => typeof (ButtonBase);

  protected override void OnAttach(Control control)
  {
    base.OnAttach(control);
    ButtonBase instance = (ButtonBase) control;
    Type targetType = typeof (ButtonBase);
    VisualStateBehavior.AddValueChanged(UIElement.IsMouseOverProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
    VisualStateBehavior.AddValueChanged(UIElement.IsEnabledProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
    VisualStateBehavior.AddValueChanged(ButtonBase.IsPressedProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void OnDetach(Control control)
  {
    base.OnDetach(control);
    ButtonBase instance = (ButtonBase) control;
    Type targetType = typeof (ButtonBase);
    VisualStateBehavior.RemoveValueChanged(UIElement.IsMouseOverProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
    VisualStateBehavior.RemoveValueChanged(UIElement.IsEnabledProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
    VisualStateBehavior.RemoveValueChanged(ButtonBase.IsPressedProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void UpdateState(Control control, bool useTransitions)
  {
    ButtonBase buttonBase = (ButtonBase) control;
    if (!buttonBase.IsEnabled)
      Syncfusion.Windows.VisualStateManager.GoToState((Control) buttonBase, "Disabled", useTransitions);
    else if (buttonBase.IsPressed)
      Syncfusion.Windows.VisualStateManager.GoToState((Control) buttonBase, "Pressed", useTransitions);
    else if (buttonBase.IsMouseOver)
      Syncfusion.Windows.VisualStateManager.GoToState((Control) buttonBase, "MouseOver", useTransitions);
    else
      Syncfusion.Windows.VisualStateManager.GoToState((Control) buttonBase, "Normal", useTransitions);
    base.UpdateState(control, useTransitions);
  }
}
