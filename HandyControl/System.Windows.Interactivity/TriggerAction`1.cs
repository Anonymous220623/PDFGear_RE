// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.TriggerAction`1
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;

#nullable disable
namespace HandyControl.Interactivity;

public abstract class TriggerAction<T> : TriggerAction where T : DependencyObject
{
  protected TriggerAction()
    : base(typeof (T))
  {
  }

  protected T AssociatedObject => (T) base.AssociatedObject;

  protected sealed override Type AssociatedObjectTypeConstraint
  {
    get => base.AssociatedObjectTypeConstraint;
  }
}
