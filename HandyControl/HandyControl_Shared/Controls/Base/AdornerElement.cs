// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.AdornerElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public abstract class AdornerElement : Control, IDisposable
{
  public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(nameof (Target), typeof (FrameworkElement), typeof (AdornerElement), new PropertyMetadata((object) null, new PropertyChangedCallback(AdornerElement.OnTargetChanged)));
  public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached("Instance", typeof (AdornerElement), typeof (AdornerElement), new PropertyMetadata((object) null, new PropertyChangedCallback(AdornerElement.OnInstanceChanged)));
  public static readonly DependencyProperty IsInstanceProperty = DependencyProperty.RegisterAttached("IsInstance", typeof (bool), typeof (AdornerElement), new PropertyMetadata(ValueBoxes.TrueBox));

  protected FrameworkElement ElementTarget { get; set; }

  private static void OnTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    AdornerElement adornerElement = (AdornerElement) d;
    adornerElement.OnTargetChanged(adornerElement.ElementTarget, false);
    adornerElement.OnTargetChanged((FrameworkElement) e.NewValue, true);
  }

  [Bindable(true)]
  [Category("Layout")]
  [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
  public FrameworkElement Target
  {
    get => (FrameworkElement) this.GetValue(AdornerElement.TargetProperty);
    set => this.SetValue(AdornerElement.TargetProperty, (object) value);
  }

  private static void OnInstanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FrameworkElement target))
      return;
    ((AdornerElement) e.NewValue).OnInstanceChanged(target);
  }

  protected virtual void OnInstanceChanged(FrameworkElement target) => this.Target = target;

  public static void SetInstance(DependencyObject element, AdornerElement value)
  {
    element.SetValue(AdornerElement.InstanceProperty, (object) value);
  }

  public static AdornerElement GetInstance(DependencyObject element)
  {
    return (AdornerElement) element.GetValue(AdornerElement.InstanceProperty);
  }

  public static void SetIsInstance(DependencyObject element, bool value)
  {
    element.SetValue(AdornerElement.IsInstanceProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetIsInstance(DependencyObject element)
  {
    return (bool) element.GetValue(AdornerElement.IsInstanceProperty);
  }

  protected virtual void OnTargetChanged(FrameworkElement element, bool isNew)
  {
    if (element == null)
      return;
    if (!isNew)
    {
      element.Unloaded -= new RoutedEventHandler(this.TargetElement_Unloaded);
      this.ElementTarget = (FrameworkElement) null;
    }
    else
    {
      element.Unloaded += new RoutedEventHandler(this.TargetElement_Unloaded);
      this.ElementTarget = element;
    }
  }

  private void TargetElement_Unloaded(object sender, RoutedEventArgs e)
  {
    if (!(sender is FrameworkElement frameworkElement))
      return;
    frameworkElement.Unloaded -= new RoutedEventHandler(this.TargetElement_Unloaded);
    this.Dispose();
  }

  protected abstract void Dispose();

  void IDisposable.Dispose() => this.Dispose();
}
