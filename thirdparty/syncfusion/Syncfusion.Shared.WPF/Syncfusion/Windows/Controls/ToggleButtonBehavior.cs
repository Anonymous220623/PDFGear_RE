// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.ToggleButtonBehavior
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Syncfusion.Windows.Controls;

public class ToggleButtonBehavior : ButtonBaseBehavior
{
  protected internal override Type TargetType => typeof (ToggleButton);

  protected override void OnAttach(Control control)
  {
    base.OnAttach(control);
    ToggleButton instance = (ToggleButton) control;
    Type targetType = typeof (ToggleButton);
    VisualStateBehavior.AddValueChanged(ToggleButton.IsCheckedProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void OnDetach(Control control)
  {
    base.OnDetach(control);
    ToggleButton instance = (ToggleButton) control;
    Type targetType = typeof (ToggleButton);
    VisualStateBehavior.RemoveValueChanged(ToggleButton.IsCheckedProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void UpdateState(Control control, bool useTransitions)
  {
    ToggleButton toggleButton = (ToggleButton) control;
    if (!toggleButton.IsChecked.HasValue)
      VisualStateManager.GoToState((Control) toggleButton, "Indeterminate", useTransitions);
    else if (toggleButton.IsChecked.Value)
      VisualStateManager.GoToState((Control) toggleButton, "Checked", useTransitions);
    else
      VisualStateManager.GoToState((Control) toggleButton, "Unchecked", useTransitions);
    base.UpdateState(control, useTransitions);
  }
}
