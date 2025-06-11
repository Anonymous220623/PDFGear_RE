// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.VisualStateBehaviorFactory
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Controls;

internal class VisualStateBehaviorFactory : TypeHandlerFactory<VisualStateBehavior>
{
  [ThreadStatic]
  private static VisualStateBehaviorFactory _instance;
  [ThreadStatic]
  private static bool _registeredKnownTypes;

  internal static VisualStateBehaviorFactory Instance
  {
    get
    {
      if (VisualStateBehaviorFactory._instance == null)
        VisualStateBehaviorFactory._instance = new VisualStateBehaviorFactory();
      return VisualStateBehaviorFactory._instance;
    }
  }

  private VisualStateBehaviorFactory()
  {
  }

  internal static void AttachBehavior(Control control)
  {
    if (DependencyPropertyHelper.GetValueSource((DependencyObject) control, VisualStateBehavior.VisualStateBehaviorProperty).BaseValueSource != BaseValueSource.Default)
      return;
    if (!VisualStateBehaviorFactory._registeredKnownTypes)
    {
      VisualStateBehaviorFactory._registeredKnownTypes = true;
      VisualStateBehaviorFactory.RegisterControlBehavior((VisualStateBehavior) new ButtonBaseBehavior());
      VisualStateBehaviorFactory.RegisterControlBehavior((VisualStateBehavior) new ToggleButtonBehavior());
      VisualStateBehaviorFactory.RegisterControlBehavior((VisualStateBehavior) new ListBoxItemBehavior());
      VisualStateBehaviorFactory.RegisterControlBehavior((VisualStateBehavior) new TextBoxBaseBehavior());
      VisualStateBehaviorFactory.RegisterControlBehavior((VisualStateBehavior) new ProgressBarBehavior());
    }
    VisualStateBehavior handler = VisualStateBehaviorFactory.Instance.GetHandler(control.GetType());
    if (handler == null)
      return;
    VisualStateBehavior.SetVisualStateBehavior((DependencyObject) control, handler);
  }

  internal static void RegisterControlBehavior(VisualStateBehavior behavior)
  {
    VisualStateBehaviorFactory.Instance.RegisterHandler(behavior);
  }

  protected override Type GetBaseType(VisualStateBehavior behavior) => behavior.TargetType;
}
