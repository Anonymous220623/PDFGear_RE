// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.BehaviorCollection
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;

#nullable disable
namespace HandyControl.Interactivity;

public sealed class BehaviorCollection : AttachableCollection<Behavior>
{
  internal BehaviorCollection()
  {
  }

  protected override void OnAttached()
  {
    foreach (Behavior behavior in (FreezableCollection<Behavior>) this)
      behavior.Attach(this.AssociatedObject);
  }

  protected override void OnDetaching()
  {
    foreach (Behavior behavior in (FreezableCollection<Behavior>) this)
      behavior.Detach();
  }

  internal override void ItemAdded(Behavior item)
  {
    if (item == null || this.AssociatedObject == null)
      return;
    item.Attach(this.AssociatedObject);
  }

  internal override void ItemRemoved(Behavior item)
  {
    if (((IAttachedObject) item)?.AssociatedObject == null)
      return;
    item.Detach();
  }

  protected override Freezable CreateInstanceCore() => (Freezable) new BehaviorCollection();
}
