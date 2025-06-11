// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.ListObjectManager`2
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

internal class ListObjectManager<TKey, TValue>
{
  private Dictionary<TKey, TValue> _map = new Dictionary<TKey, TValue>();

  public int Count => this._map.Count;

  public TValue GetByIndex(int index)
  {
    int num = 0;
    foreach (TValue byIndex in this._map.Values)
    {
      if (num++ == index)
        return byIndex;
    }
    return default (TValue);
  }

  public void Add(TKey key, TValue value)
  {
    if (this.Contains(key))
      return;
    this._map.Add(key, value);
  }

  public TValue Get(TKey key) => this.Contains(key) ? this._map[key] : default (TValue);

  public bool Contains(TKey key) => this._map.ContainsKey(key);

  public bool Remove(TKey key) => this._map.Remove(key);

  public void Clear() => this._map.Clear();
}
