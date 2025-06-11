// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.TriggerBase
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Interactivity;

[ContentProperty("Actions")]
public abstract class TriggerBase : Animatable, IAttachedObject
{
  private static readonly DependencyPropertyKey ActionsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (Actions), typeof (TriggerActionCollection), typeof (TriggerBase), (PropertyMetadata) new FrameworkPropertyMetadata());
  public static readonly DependencyProperty ActionsProperty = TriggerBase.ActionsPropertyKey.DependencyProperty;
  private DependencyObject _associatedObject;
  private readonly Type _associatedObjectTypeConstraint;

  internal TriggerBase(Type associatedObjectTypeConstraint)
  {
    this._associatedObjectTypeConstraint = associatedObjectTypeConstraint;
    TriggerActionCollection actionCollection = new TriggerActionCollection();
    this.SetValue(TriggerBase.ActionsPropertyKey, (object) actionCollection);
  }

  public TriggerActionCollection Actions
  {
    get => (TriggerActionCollection) this.GetValue(TriggerBase.ActionsProperty);
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

  public void Attach(DependencyObject dependencyObject)
  {
    if (object.Equals((object) dependencyObject, (object) this.AssociatedObject))
      return;
    if (this.AssociatedObject != null)
      throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerMultipleTimesExceptionMessage);
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
    this.Actions.Attach(dependencyObject);
    this.OnAttached();
  }

  public void Detach()
  {
    this.OnDetaching();
    this.WritePreamble();
    this._associatedObject = (DependencyObject) null;
    this.WritePostscript();
    this.Actions.Detach();
  }

  DependencyObject IAttachedObject.AssociatedObject => this.AssociatedObject;

  public event EventHandler<PreviewInvokeEventArgs> PreviewInvoke;

  protected override Freezable CreateInstanceCore()
  {
    return (Freezable) Activator.CreateInstance(this.GetType());
  }

  protected void InvokeActions(object parameter)
  {
    if (this.PreviewInvoke != null)
    {
      PreviewInvokeEventArgs e = new PreviewInvokeEventArgs();
      this.PreviewInvoke((object) this, e);
      if (e.Cancelling)
        return;
    }
    foreach (TriggerAction action in (FreezableCollection<TriggerAction>) this.Actions)
      action.CallInvoke(parameter);
  }

  protected virtual void OnAttached()
  {
  }

  protected virtual void OnDetaching()
  {
  }
}
