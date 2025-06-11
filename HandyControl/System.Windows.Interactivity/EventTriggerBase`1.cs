// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.EventTriggerBase`1
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

#nullable disable
namespace HandyControl.Interactivity;

public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
{
  protected EventTriggerBase()
    : base(typeof (T))
  {
  }

  protected virtual void OnSourceChanged(T oldSource, T newSource)
  {
  }

  internal sealed override void OnSourceChangedImpl(object oldSource, object newSource)
  {
    base.OnSourceChangedImpl(oldSource, newSource);
    this.OnSourceChanged(oldSource as T, newSource as T);
  }

  public T Source => (T) base.Source;
}
