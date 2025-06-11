// Decompiled with JetBrains decompiler
// Type: HandyControl.Collections.SimplePool`1
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Collections;

public class SimplePool<T> : IPool<T>
{
  private readonly object[] _pool;
  private int _poolSize;

  public SimplePool(int maxPoolSize)
  {
    this._pool = maxPoolSize > 0 ? new object[maxPoolSize] : throw new ArgumentException("The max pool size must be > 0");
  }

  public virtual T Acquire()
  {
    if (this._poolSize <= 0)
      return default (T);
    int index = this._poolSize - 1;
    T obj = (T) this._pool[index];
    this._pool[index] = (object) null;
    --this._poolSize;
    return obj;
  }

  public virtual bool Release(T instance)
  {
    if (this.IsInPool(instance))
      throw new Exception("Already in the pool!");
    if (this._poolSize >= this._pool.Length)
      return false;
    this._pool[this._poolSize] = (object) instance;
    ++this._poolSize;
    return true;
  }

  private bool IsInPool(T instance)
  {
    for (int index = 0; index < this._poolSize; ++index)
    {
      if (object.Equals(this._pool[index], (object) instance))
        return true;
    }
    return false;
  }
}
