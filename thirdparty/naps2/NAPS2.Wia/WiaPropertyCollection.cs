// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaPropertyCollection
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using NAPS2.Wia.Native;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace NAPS2.Wia;

public class WiaPropertyCollection : NativeWiaObject, IEnumerable<WiaProperty>, IEnumerable
{
  private readonly Dictionary<int, WiaProperty> _propertyDict;

  protected internal WiaPropertyCollection(WiaVersion version, IntPtr propertyStorageHandle)
    : base(version, propertyStorageHandle)
  {
    this._propertyDict = new Dictionary<int, WiaProperty>();
    WiaException.Check(NativeWiaMethods.EnumerateProperties(this.Handle, (NativeWiaMethods.EnumPropertyCallback) ((id, name, type) => this._propertyDict.Add(id, new WiaProperty(this.Handle, id, name, type)))));
  }

  public WiaProperty this[int propId] => this._propertyDict[propId];

  public WiaProperty? GetOrNull(int propId)
  {
    return !this._propertyDict.ContainsKey(propId) ? (WiaProperty) null : this._propertyDict[propId];
  }

  public IEnumerator<WiaProperty> GetEnumerator()
  {
    return (IEnumerator<WiaProperty>) this._propertyDict.Values.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
