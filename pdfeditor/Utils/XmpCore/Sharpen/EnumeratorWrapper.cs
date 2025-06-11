// Decompiled with JetBrains decompiler
// Type: Sharpen.EnumeratorWrapper`1
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Collections.Generic;

#nullable disable
namespace Sharpen;

public sealed class EnumeratorWrapper<T> : Iterator<T>
{
  private readonly object _collection;
  private IEnumerator<T> _e;
  private T _lastVal;
  private bool _more;
  private bool _copied;

  public EnumeratorWrapper(object collection, IEnumerator<T> e)
  {
    this._e = e;
    this._collection = collection;
    this._more = e.MoveNext();
  }

  public override bool HasNext() => this._more;

  public override T Next()
  {
    if (!this._more)
      throw new InvalidOperationException();
    this._lastVal = this._e.Current;
    this._more = this._e.MoveNext();
    return this._lastVal;
  }

  public override void Remove()
  {
    if (!(this._collection is ICollection<T> collection))
      throw new NotSupportedException();
    if (this._more && !this._copied)
    {
      List<T> objList = new List<T>();
      do
      {
        objList.Add(this._e.Current);
      }
      while (this._e.MoveNext());
      this._e = (IEnumerator<T>) objList.GetEnumerator();
      this._e.MoveNext();
      this._copied = true;
    }
    collection.Remove(this._lastVal);
  }
}
