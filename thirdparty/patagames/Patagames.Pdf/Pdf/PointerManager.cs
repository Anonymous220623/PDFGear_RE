// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.PointerManager
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;
using System;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf;

internal class PointerManager
{
  private static Dictionary<IntPtr, IPointerManagerItem> _items = new Dictionary<IntPtr, IPointerManagerItem>();
  private static object _sync = new object();

  public static void Add(IPointerManagerItem item)
  {
    if (item.Key == IntPtr.Zero)
      throw new NullReferenceException(Error.err0001);
    lock (PointerManager._sync)
    {
      if (PointerManager._items.ContainsKey(item.Key))
        return;
      PointerManager._items.Add(item.Key, item);
    }
  }

  public static void Remove(IPointerManagerItem item) => PointerManager.Remove(item.Key);

  public static void Remove(IntPtr key)
  {
    IPointerManagerItem pointerManagerItem = (IPointerManagerItem) null;
    lock (PointerManager._sync)
    {
      if (PointerManager._items.ContainsKey(key))
      {
        pointerManagerItem = PointerManager._items[key];
        PointerManager._items.Remove(key);
      }
    }
    if (pointerManagerItem == null)
      return;
    (pointerManagerItem as IDisposable).Dispose();
  }

  public static void Clear()
  {
    foreach (IPointerManagerItem pointerManagerItem in PointerManager._items.Values)
      (pointerManagerItem as IDisposable).Dispose();
    PointerManager._items.Clear();
  }

  public static IPointerManagerItem GetAt(IntPtr key)
  {
    lock (PointerManager._sync)
      return PointerManager._items.ContainsKey(key) ? PointerManager._items[key] : (IPointerManagerItem) null;
  }
}
