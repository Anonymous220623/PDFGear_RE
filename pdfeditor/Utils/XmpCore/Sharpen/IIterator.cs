// Decompiled with JetBrains decompiler
// Type: Sharpen.Iterator`1
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Sharpen;

public abstract class Iterator<T> : IEnumerator<T>, IDisposable, IEnumerator, IIterator
{
  private T _lastValue;

  object IIterator.Next() => (object) this.Next();

  public abstract bool HasNext();

  public abstract T Next();

  public abstract void Remove();

  bool IEnumerator.MoveNext()
  {
    if (!this.HasNext())
      return false;
    this._lastValue = this.Next();
    return true;
  }

  void IEnumerator.Reset() => throw new NotImplementedException();

  void IDisposable.Dispose()
  {
  }

  T IEnumerator<T>.Current => this._lastValue;

  object IEnumerator.Current => (object) this._lastValue;
}
