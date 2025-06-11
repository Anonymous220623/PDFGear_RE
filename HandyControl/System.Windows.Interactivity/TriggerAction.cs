// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.TriggerAction
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Interactivity;

[DefaultTrigger(typeof (UIElement), typeof (EventTrigger), "MouseLeftButtonDown")]
[DefaultTrigger(typeof (ButtonBase), typeof (EventTrigger), "Click")]
public abstract class TriggerAction : Animatable, IAttachedObject
{
  public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(nameof (IsEnabled), typeof (bool), typeof (TriggerAction), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.TrueBox));
  private readonly Type _associatedObjectTypeConstraint;
  private DependencyObject _associatedObject;
  private bool _isHosted;

  internal TriggerAction(Type associatedObjectTypeConstraint)
  {
    this._associatedObjectTypeConstraint = associatedObjectTypeConstraint;
  }

  protected DependencyObject AssociatedObject
  {
    get
    {
      this.ReadPreamble();
      return this._associatedObject;
    }
  }

  protected virtual Type AssociatedObjectTypeConstraint
  {
    get
    {
      this.ReadPreamble();
      return this._associatedObjectTypeConstraint;
    }
  }

  public bool IsEnabled
  {
    get => (bool) this.GetValue(TriggerAction.IsEnabledProperty);
    set => this.SetValue(TriggerAction.IsEnabledProperty, ValueBoxes.BooleanBox(value));
  }

  internal bool IsHosted
  {
    get
    {
      this.ReadPreamble();
      return this._isHosted;
    }
    set
    {
      this.WritePreamble();
      this._isHosted = value;
      this.WritePostscript();
    }
  }

  public void Attach(DependencyObject dependencyObject)
  {
    if (object.Equals((object) dependencyObject, (object) this.AssociatedObject))
      return;
    if (this.AssociatedObject != null)
      throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
    if (dependencyObject != null && !this.AssociatedObjectTypeConstraint.IsInstanceOfType((object) dependencyObject))
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.TypeConstraintViolatedExceptionMessage, new object[3]
      {
        (object) this.GetType().Name,
        (object) dependencyObject.GetType().Name,
        (object) this.AssociatedObjectTypeConstraint.Name
      }));
    this.WritePreamble();
    this._associatedObject = dependencyObject;
    this.WritePostscript();
    this.OnAttached();
  }

  public void Detach()
  {
    this.OnDetaching();
    this.WritePreamble();
    this._associatedObject = (DependencyObject) null;
    this.WritePostscript();
  }

  DependencyObject IAttachedObject.AssociatedObject => this.AssociatedObject;

  internal void CallInvoke(object parameter)
  {
    if (!this.IsEnabled)
      return;
    this.Invoke(parameter);
  }

  protected override Freezable CreateInstanceCore()
  {
    return (Freezable) Activator.CreateInstance(this.GetType());
  }

  protected abstract void Invoke(object parameter);

  protected virtual void OnAttached()
  {
  }

  protected virtual void OnDetaching()
  {
  }
}
