// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.Behavior
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Interactivity;

public abstract class Behavior : Animatable, IAttachedObject
{
  private DependencyObject _associatedObject;
  private readonly Type _associatedType;

  internal Behavior(Type associatedType) => this._associatedType = associatedType;

  protected DependencyObject AssociatedObject
  {
    get
    {
      this.ReadPreamble();
      return this._associatedObject;
    }
  }

  protected Type AssociatedType
  {
    get
    {
      this.ReadPreamble();
      return this._associatedType;
    }
  }

  public void Attach(DependencyObject dependencyObject)
  {
    if (object.Equals((object) dependencyObject, (object) this.AssociatedObject))
      return;
    if (this.AssociatedObject != null)
      throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorMultipleTimesExceptionMessage);
    if (dependencyObject != null && !this.AssociatedType.IsInstanceOfType((object) dependencyObject))
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, ExceptionStringTable.TypeConstraintViolatedExceptionMessage, new object[3]
      {
        (object) this.GetType().Name,
        (object) dependencyObject.GetType().Name,
        (object) this.AssociatedType.Name
      }));
    this.WritePreamble();
    this._associatedObject = dependencyObject;
    this.WritePostscript();
    this.OnAssociatedObjectChanged();
    this.OnAttached();
  }

  public void Detach()
  {
    this.OnDetaching();
    this.WritePreamble();
    this._associatedObject = (DependencyObject) null;
    this.WritePostscript();
    this.OnAssociatedObjectChanged();
  }

  DependencyObject IAttachedObject.AssociatedObject => this.AssociatedObject;

  internal event EventHandler AssociatedObjectChanged;

  protected override Freezable CreateInstanceCore()
  {
    return (Freezable) Activator.CreateInstance(this.GetType());
  }

  private void OnAssociatedObjectChanged()
  {
    EventHandler associatedObjectChanged = this.AssociatedObjectChanged;
    if (associatedObjectChanged == null)
      return;
    associatedObjectChanged((object) this, new EventArgs());
  }

  protected virtual void OnAttached()
  {
  }

  protected virtual void OnDetaching()
  {
  }
}
