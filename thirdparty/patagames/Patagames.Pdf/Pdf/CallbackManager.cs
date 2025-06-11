// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.CallbackManager
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf;

internal class CallbackManager
{
  private static object _sync = new object();
  private static object _sync2 = new object();
  private static Dictionary<Guid, object> _list = new Dictionary<Guid, object>();
  private static Dictionary<IntPtr, Dictionary<int, object>> _list2 = new Dictionary<IntPtr, Dictionary<int, object>>();

  public static T Get<T>(Guid key)
  {
    lock (CallbackManager._sync)
      return CallbackManager._list.ContainsKey(key) ? (T) CallbackManager._list[key] : default (T);
  }

  public static void Set<T>(Guid key, T val)
  {
    if (key == new Guid())
      throw new ArgumentNullException();
    lock (CallbackManager._sync)
    {
      if (!CallbackManager._list.ContainsKey(key))
        CallbackManager._list.Add(key, (object) val);
      else
        CallbackManager._list[key] = (object) val;
    }
  }

  public static void Remove(Guid key)
  {
    lock (CallbackManager._sync)
    {
      if (!CallbackManager._list.ContainsKey(key))
        return;
      CallbackManager._list.Remove(key);
    }
  }

  public static TVal Get<TVal>(IntPtr key, int idx)
  {
    lock (CallbackManager._sync2)
    {
      if (!CallbackManager._list2.ContainsKey(key))
        return default (TVal);
      Dictionary<int, object> dictionary = CallbackManager._list2[key];
      return !dictionary.ContainsKey(idx) ? default (TVal) : (TVal) dictionary[idx];
    }
  }

  public static void Set<TVal>(IntPtr key, int idx, TVal val) where TVal : class
  {
    if (key == IntPtr.Zero)
      return;
    lock (CallbackManager._sync)
    {
      if (!CallbackManager._list2.ContainsKey(key))
        CallbackManager._list2.Add(key, new Dictionary<int, object>());
      Dictionary<int, object> dictionary = CallbackManager._list2[key];
      if (!dictionary.ContainsKey(idx) && (object) val != null)
      {
        dictionary.Add(idx, (object) val);
      }
      else
      {
        if (!dictionary.ContainsKey(idx) && (object) val == null)
          return;
        if (dictionary.ContainsKey(idx) && (object) val != null)
        {
          dictionary[idx] = (object) val;
        }
        else
        {
          if (!dictionary.ContainsKey(idx) || (object) val != null)
            return;
          dictionary.Remove(idx);
        }
      }
    }
  }

  public static void Remove(IntPtr key)
  {
    lock (CallbackManager._sync)
    {
      if (!CallbackManager._list2.ContainsKey(key))
        return;
      CallbackManager._list2.Remove(key);
    }
  }

  public static void Remove(IntPtr key, int idx)
  {
    lock (CallbackManager._sync)
    {
      if (!CallbackManager._list2.ContainsKey(key))
        return;
      if (CallbackManager._list2[key].ContainsKey(idx))
        CallbackManager._list2[key].Remove(idx);
      if (CallbackManager._list2[key].Count != 0)
        return;
      CallbackManager._list2.Remove(key);
    }
  }
}
