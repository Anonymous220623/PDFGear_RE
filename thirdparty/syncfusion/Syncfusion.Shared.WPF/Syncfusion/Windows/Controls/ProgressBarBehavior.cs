// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.ProgressBarBehavior
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Controls;

public class ProgressBarBehavior : ControlBehavior
{
  protected internal override Type TargetType => typeof (ProgressBar);

  protected override void OnAttach(Control control)
  {
    base.OnAttach(control);
    ProgressBar instance = (ProgressBar) control;
    Type targetType = typeof (ProgressBar);
    VisualStateBehavior.AddValueChanged(ProgressBar.IsIndeterminateProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void OnDetach(Control control)
  {
    base.OnDetach(control);
    ProgressBar instance = (ProgressBar) control;
    Type targetType = typeof (ProgressBar);
    VisualStateBehavior.RemoveValueChanged(ProgressBar.IsIndeterminateProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void UpdateState(Control control, bool useTransitions)
  {
    ProgressBar progressBar = (ProgressBar) control;
    if (!progressBar.IsIndeterminate)
      VisualStateManager.GoToState((Control) progressBar, "Determinate", useTransitions);
    else
      VisualStateManager.GoToState((Control) progressBar, "Indeterminate", useTransitions);
    base.UpdateState(control, useTransitions);
  }
}
