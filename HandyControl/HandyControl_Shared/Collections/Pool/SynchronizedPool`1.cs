// Decompiled with JetBrains decompiler
// Type: HandyControl.Collections.SynchronizedPool`1
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

#nullable disable
namespace HandyControl.Collections;

public class SynchronizedPool<T>(int maxPoolSize) : SimplePool<T>(maxPoolSize)
{
  private readonly object _lockObj = new object();

  public override T Acquire()
  {
    lock (this._lockObj)
      return base.Acquire();
  }

  public override bool Release(T element)
  {
    lock (this._lockObj)
      return base.Release(element);
  }
}
