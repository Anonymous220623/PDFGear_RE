// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ReusableObjectCreator`1
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Internal;

internal class ReusableObjectCreator<T> where T : class
{
  protected T _reusableObject;
  private readonly Action<T> _clearObject;
  private readonly Func<int, T> _createObject;
  private readonly int _initialCapacity;
  public readonly ReusableObjectCreator<T>.LockOject None;

  protected ReusableObjectCreator(
    int initialCapacity,
    Func<int, T> createObject,
    Action<T> clearObject)
  {
    this._reusableObject = createObject(initialCapacity);
    this._clearObject = clearObject;
    this._createObject = createObject;
    this._initialCapacity = initialCapacity;
  }

  public ReusableObjectCreator<T>.LockOject Allocate()
  {
    T reusableObject = this._reusableObject ?? this._createObject(this._initialCapacity);
    this._reusableObject = default (T);
    return new ReusableObjectCreator<T>.LockOject(this, reusableObject);
  }

  private void Deallocate(T reusableObject)
  {
    this._clearObject(reusableObject);
    this._reusableObject = reusableObject;
  }

  public struct LockOject(ReusableObjectCreator<T> owner, T reusableObject) : IDisposable
  {
    public readonly T Result = reusableObject;
    private readonly ReusableObjectCreator<T> _owner = owner;

    public void Dispose() => this._owner?.Deallocate(this.Result);
  }
}
