// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.TriggerCollection
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;

#nullable disable
namespace HandyControl.Interactivity;

public sealed class TriggerCollection : AttachableCollection<TriggerBase>
{
  internal TriggerCollection()
  {
  }

  protected override void OnAttached()
  {
    foreach (TriggerBase triggerBase in (FreezableCollection<TriggerBase>) this)
      triggerBase.Attach(this.AssociatedObject);
  }

  protected override void OnDetaching()
  {
    foreach (TriggerBase triggerBase in (FreezableCollection<TriggerBase>) this)
      triggerBase.Detach();
  }

  internal override void ItemAdded(TriggerBase item)
  {
    if (this.AssociatedObject == null)
      return;
    item.Attach(this.AssociatedObject);
  }

  internal override void ItemRemoved(TriggerBase item)
  {
    if (((IAttachedObject) item).AssociatedObject == null)
      return;
    item.Detach();
  }

  protected override Freezable CreateInstanceCore() => (Freezable) new TriggerCollection();
}
