// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.TextBoxBaseBehavior
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Syncfusion.Windows.Controls;

public class TextBoxBaseBehavior : ControlBehavior
{
  protected internal override Type TargetType => typeof (TextBoxBase);

  protected override void OnAttach(Control control)
  {
    base.OnAttach(control);
    TextBoxBase instance = (TextBoxBase) control;
    Type targetType = typeof (TextBoxBase);
    VisualStateBehavior.AddValueChanged(UIElement.IsMouseOverProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
    VisualStateBehavior.AddValueChanged(UIElement.IsEnabledProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
    VisualStateBehavior.AddValueChanged(TextBoxBase.IsReadOnlyProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void OnDetach(Control control)
  {
    base.OnDetach(control);
    TextBoxBase instance = (TextBoxBase) control;
    Type targetType = typeof (TextBoxBase);
    VisualStateBehavior.RemoveValueChanged(UIElement.IsMouseOverProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
    VisualStateBehavior.RemoveValueChanged(UIElement.IsEnabledProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
    VisualStateBehavior.RemoveValueChanged(TextBoxBase.IsReadOnlyProperty, targetType, (object) instance, new EventHandler(((VisualStateBehavior) this).UpdateStateHandler));
  }

  protected override void UpdateState(Control control, bool useTransitions)
  {
    TextBoxBase textBoxBase = (TextBoxBase) control;
    if (!textBoxBase.IsEnabled)
      Syncfusion.Windows.VisualStateManager.GoToState((Control) textBoxBase, "Disabled", useTransitions);
    else if (textBoxBase.IsReadOnly)
      Syncfusion.Windows.VisualStateManager.GoToState((Control) textBoxBase, "ReadOnly", useTransitions);
    else if (textBoxBase.IsMouseOver)
      Syncfusion.Windows.VisualStateManager.GoToState((Control) textBoxBase, "MouseOver", useTransitions);
    else
      Syncfusion.Windows.VisualStateManager.GoToState((Control) textBoxBase, "Normal", useTransitions);
    base.UpdateState(control, useTransitions);
  }
}
