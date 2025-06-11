// Decompiled with JetBrains decompiler
// Type: NLog.Internal.StringBuilderPool
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.Internal;

internal class StringBuilderPool
{
  private StringBuilder _fastPool;
  private readonly StringBuilder[] _slowPool;
  private readonly int _maxBuilderCapacity;

  public StringBuilderPool(int poolCapacity, int initialBuilderCapacity = 1024 /*0x0400*/, int maxBuilderCapacity = 524288 /*0x080000*/)
  {
    this._fastPool = new StringBuilder(10 * initialBuilderCapacity);
    this._slowPool = new StringBuilder[poolCapacity];
    for (int index = 0; index < this._slowPool.Length; ++index)
      this._slowPool[index] = new StringBuilder(initialBuilderCapacity);
    this._maxBuilderCapacity = maxBuilderCapacity;
  }

  public StringBuilderPool.ItemHolder Acquire()
  {
    StringBuilder fastPool = this._fastPool;
    if (fastPool != null && fastPool == Interlocked.CompareExchange<StringBuilder>(ref this._fastPool, (StringBuilder) null, fastPool))
      return new StringBuilderPool.ItemHolder(fastPool, this, -1);
    for (int poolIndex = 0; poolIndex < this._slowPool.Length; ++poolIndex)
    {
      StringBuilder stringBuilder = this._slowPool[poolIndex];
      if (stringBuilder != null && stringBuilder == Interlocked.CompareExchange<StringBuilder>(ref this._slowPool[poolIndex], (StringBuilder) null, stringBuilder))
        return new StringBuilderPool.ItemHolder(stringBuilder, this, poolIndex);
    }
    return new StringBuilderPool.ItemHolder(new StringBuilder(), (StringBuilderPool) null, 0);
  }

  private void Release(StringBuilder stringBuilder, int poolIndex)
  {
    if (stringBuilder.Length > this._maxBuilderCapacity)
    {
      int num = poolIndex == -1 ? this._maxBuilderCapacity * 10 : this._maxBuilderCapacity;
      if (stringBuilder.Length > num)
      {
        stringBuilder.Remove(0, stringBuilder.Length - 1);
        if (stringBuilder.Capacity > num)
          stringBuilder = new StringBuilder(num / 2);
      }
    }
    stringBuilder.ClearBuilder();
    if (poolIndex == -1)
      this._fastPool = stringBuilder;
    else
      this._slowPool[poolIndex] = stringBuilder;
  }

  public struct ItemHolder(StringBuilder stringBuilder, StringBuilderPool owner, int poolIndex) : 
    IDisposable
  {
    public readonly StringBuilder Item = stringBuilder;
    private readonly StringBuilderPool _owner = owner;
    private readonly int _poolIndex = poolIndex;

    public void Dispose()
    {
      if (this._owner == null)
        return;
      this._owner.Release(this.Item, this._poolIndex);
    }
  }
}
