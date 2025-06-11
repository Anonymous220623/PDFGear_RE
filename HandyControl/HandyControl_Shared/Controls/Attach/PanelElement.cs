// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.PanelElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Interactivity;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class PanelElement
{
  public static readonly DependencyProperty FluidMoveBehaviorProperty = DependencyProperty.RegisterAttached("FluidMoveBehavior", typeof (FluidMoveBehavior), typeof (PanelElement), new PropertyMetadata((object) null, new PropertyChangedCallback(PanelElement.OnFluidMoveBehaviorChanged)));
  private static readonly DependencyProperty TempFluidMoveBehaviorProperty = DependencyProperty.RegisterAttached("TempFluidMoveBehavior", typeof (FluidMoveBehavior), typeof (PanelElement), new PropertyMetadata((object) null));

  private static void OnFluidMoveBehaviorChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Panel element))
      return;
    BehaviorCollection behaviors = Interaction.GetBehaviors((DependencyObject) element);
    FluidMoveBehavior fluidMoveBehavior = PanelElement.GetTempFluidMoveBehavior((DependencyObject) element);
    behaviors.ItemRemoved((Behavior) fluidMoveBehavior);
    element.SetCurrentValue(PanelElement.TempFluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
    if (!(e.NewValue is FluidMoveBehavior newValue))
      return;
    behaviors.ItemAdded((Behavior) newValue);
    PanelElement.SetTempFluidMoveBehavior((DependencyObject) element, newValue);
  }

  public static void SetFluidMoveBehavior(DependencyObject element, FluidMoveBehavior value)
  {
    element.SetValue(PanelElement.FluidMoveBehaviorProperty, (object) value);
  }

  public static FluidMoveBehavior GetFluidMoveBehavior(DependencyObject element)
  {
    return (FluidMoveBehavior) element.GetValue(PanelElement.FluidMoveBehaviorProperty);
  }

  private static void SetTempFluidMoveBehavior(DependencyObject element, FluidMoveBehavior value)
  {
    element.SetValue(PanelElement.TempFluidMoveBehaviorProperty, (object) value);
  }

  private static FluidMoveBehavior GetTempFluidMoveBehavior(DependencyObject element)
  {
    return (FluidMoveBehavior) element.GetValue(PanelElement.TempFluidMoveBehaviorProperty);
  }
}
