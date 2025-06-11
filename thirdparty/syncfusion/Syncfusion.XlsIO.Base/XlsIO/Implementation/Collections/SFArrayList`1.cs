// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.SFArrayList`1
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class SFArrayList<T> : List<T>, ICloneable where T : class
{
  public SFArrayList()
  {
  }

  public SFArrayList(ICollection<T> c)
    : base((IEnumerable<T>) c)
  {
  }

  public object Clone()
  {
    SFArrayList<T> sfArrayList = new SFArrayList<T>();
    foreach (T obj1 in (List<T>) this)
    {
      T obj2 = (object) obj1 is ICloneable ? (T) ((ICloneable) (object) obj1).Clone() : obj1;
      sfArrayList.Add(obj2);
    }
    return (object) sfArrayList;
  }

  public object Clone(object parent)
  {
    SFArrayList<T> sfArrayList = new SFArrayList<T>();
    foreach (T obj1 in (List<T>) this)
    {
      T obj2 = obj1;
      if (obj2 is ICloneParent cloneParent)
        obj2 = (T) cloneParent.Clone(parent);
      sfArrayList.Add(obj2);
    }
    return (object) sfArrayList;
  }

  public new T this[int index]
  {
    get => index < this.Count && index >= 0 ? base[index] : default (T);
    set
    {
      this.EnsureCount(index + 1);
      base[index] = value;
    }
  }

  public void EnsureCount(int value)
  {
    int count = this.Count;
    if (count >= value)
      return;
    this.AddRange((IEnumerable<T>) new T[value - count]);
  }
}
