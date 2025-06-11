// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.TriggerActionCollection
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;

#nullable disable
namespace HandyControl.Interactivity;

public class TriggerActionCollection : AttachableCollection<TriggerAction>
{
  internal TriggerActionCollection()
  {
  }

  protected override Freezable CreateInstanceCore() => (Freezable) new TriggerActionCollection();

  internal override void ItemAdded(TriggerAction item)
  {
    if (item.IsHosted)
      throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerActionMultipleTimesExceptionMessage);
    if (this.AssociatedObject != null)
      item.Attach(this.AssociatedObject);
    item.IsHosted = true;
  }

  internal override void ItemRemoved(TriggerAction item)
  {
    if (((IAttachedObject) item).AssociatedObject != null)
      item.Detach();
    item.IsHosted = false;
  }

  protected override void OnAttached()
  {
    foreach (TriggerAction triggerAction in (FreezableCollection<TriggerAction>) this)
      triggerAction.Attach(this.AssociatedObject);
  }

  protected override void OnDetaching()
  {
    foreach (TriggerAction triggerAction in (FreezableCollection<TriggerAction>) this)
      triggerAction.Detach();
  }
}
