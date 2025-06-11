// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.Interaction
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;

#nullable disable
namespace HandyControl.Interactivity;

public static class Interaction
{
  private static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("ShadowTriggers", typeof (TriggerCollection), typeof (Interaction), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(Interaction.OnTriggersChanged)));
  private static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("ShadowBehaviors", typeof (BehaviorCollection), typeof (Interaction), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(Interaction.OnBehaviorsChanged)));

  internal static bool ShouldRunInDesignMode { get; set; }

  public static TriggerCollection GetTriggers(DependencyObject obj)
  {
    TriggerCollection triggers = (TriggerCollection) obj.GetValue(Interaction.TriggersProperty);
    if (triggers == null)
    {
      triggers = new TriggerCollection();
      obj.SetValue(Interaction.TriggersProperty, (object) triggers);
    }
    return triggers;
  }

  public static BehaviorCollection GetBehaviors(DependencyObject obj)
  {
    BehaviorCollection behaviors = (BehaviorCollection) obj.GetValue(Interaction.BehaviorsProperty);
    if (behaviors == null)
    {
      behaviors = new BehaviorCollection();
      obj.SetValue(Interaction.BehaviorsProperty, (object) behaviors);
    }
    return behaviors;
  }

  private static void OnBehaviorsChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    BehaviorCollection oldValue = (BehaviorCollection) args.OldValue;
    BehaviorCollection newValue = (BehaviorCollection) args.NewValue;
    if (object.Equals((object) oldValue, (object) newValue))
      return;
    if (oldValue?.AssociatedObject != null)
      oldValue.Detach();
    if (newValue == null || obj == null)
      return;
    if (newValue.AssociatedObject != null)
      throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorCollectionMultipleTimesExceptionMessage);
    newValue.Attach(obj);
  }

  private static void OnTriggersChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    TriggerCollection oldValue = args.OldValue as TriggerCollection;
    TriggerCollection newValue = args.NewValue as TriggerCollection;
    if (object.Equals((object) oldValue, (object) newValue))
      return;
    if (oldValue?.AssociatedObject != null)
      oldValue.Detach();
    if (newValue == null || obj == null)
      return;
    if (newValue.AssociatedObject != null)
      throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerCollectionMultipleTimesExceptionMessage);
    newValue.Attach(obj);
  }

  internal static bool IsElementLoaded(FrameworkElement element) => element.IsLoaded;
}
