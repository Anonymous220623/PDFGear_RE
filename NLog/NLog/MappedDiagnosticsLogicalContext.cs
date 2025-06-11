// Decompiled with JetBrains decompiler
// Type: NLog.MappedDiagnosticsLogicalContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Threading;

#nullable disable
namespace NLog;

public static class MappedDiagnosticsLogicalContext
{
  private const string LogicalThreadDictionaryKey = "NLog.AsyncableMappedDiagnosticsContext";
  private static readonly IDictionary<string, object> EmptyDefaultDictionary = (IDictionary<string, object>) new SortHelpers.ReadOnlySingleBucketDictionary<string, object>();

  private static IDictionary<string, object> GetLogicalThreadDictionary(
    bool clone = false,
    int initialCapacity = 0)
  {
    Dictionary<string, object> newValue1 = MappedDiagnosticsLogicalContext.GetThreadLocal();
    if (newValue1 == null)
    {
      if (!clone)
        return MappedDiagnosticsLogicalContext.EmptyDefaultDictionary;
      newValue1 = new Dictionary<string, object>(initialCapacity);
      MappedDiagnosticsLogicalContext.SetThreadLocal(newValue1);
    }
    else if (clone)
    {
      Dictionary<string, object> newValue2 = new Dictionary<string, object>(newValue1.Count + initialCapacity);
      foreach (KeyValuePair<string, object> keyValuePair in newValue1)
        newValue2[keyValuePair.Key] = keyValuePair.Value;
      MappedDiagnosticsLogicalContext.SetThreadLocal(newValue2);
      return (IDictionary<string, object>) newValue2;
    }
    return (IDictionary<string, object>) newValue1;
  }

  public static string Get(string item)
  {
    return MappedDiagnosticsLogicalContext.Get(item, (IFormatProvider) null);
  }

  public static string Get(string item, IFormatProvider formatProvider)
  {
    return FormatHelper.ConvertToString(MappedDiagnosticsLogicalContext.GetObject(item), formatProvider);
  }

  public static object GetObject(string item)
  {
    object obj;
    if (!MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary().TryGetValue(item, out obj))
      return (object) null;
    return obj is ObjectHandleSerializer handleSerializer ? handleSerializer.Unwrap() : obj;
  }

  public static IDisposable SetScoped(string item, string value)
  {
    return MappedDiagnosticsLogicalContext.SetScoped<string>(item, value);
  }

  public static IDisposable SetScoped(string item, object value)
  {
    return MappedDiagnosticsLogicalContext.SetScoped<object>(item, value);
  }

  public static IDisposable SetScoped<T>(string item, T value)
  {
    IDictionary<string, object> threadDictionary = MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary(true, 1);
    bool wasEmpty = threadDictionary.Count == 0;
    MappedDiagnosticsLogicalContext.SetItemValue<T>(item, value, threadDictionary);
    return (IDisposable) new MappedDiagnosticsLogicalContext.ItemRemover(item, wasEmpty);
  }

  public static IDisposable SetScoped(IReadOnlyList<KeyValuePair<string, object>> items)
  {
    if (items == null || items.Count <= 0)
      return (IDisposable) null;
    IDictionary<string, object> threadDictionary = MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary(true, items.Count);
    bool wasEmpty = threadDictionary.Count == 0;
    for (int index = 0; index < items.Count; ++index)
    {
      KeyValuePair<string, object> keyValuePair = items[index];
      string key = keyValuePair.Key;
      keyValuePair = items[index];
      object obj = keyValuePair.Value;
      IDictionary<string, object> logicalContext = threadDictionary;
      MappedDiagnosticsLogicalContext.SetItemValue<object>(key, obj, logicalContext);
    }
    return (IDisposable) new MappedDiagnosticsLogicalContext.ItemRemover(items, wasEmpty);
  }

  public static void Set(string item, string value)
  {
    MappedDiagnosticsLogicalContext.Set<string>(item, value);
  }

  public static void Set(string item, object value)
  {
    MappedDiagnosticsLogicalContext.Set<object>(item, value);
  }

  public static void Set<T>(string item, T value)
  {
    IDictionary<string, object> threadDictionary = MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary(true, 1);
    MappedDiagnosticsLogicalContext.SetItemValue<T>(item, value, threadDictionary);
  }

  private static void SetItemValue<T>(
    string item,
    T value,
    IDictionary<string, object> logicalContext)
  {
    if (typeof (T).IsValueType || Convert.GetTypeCode((object) value) != TypeCode.Object)
      logicalContext[item] = (object) value;
    else
      logicalContext[item] = (object) new ObjectHandleSerializer((object) value);
  }

  public static ICollection<string> GetNames()
  {
    return MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary().Keys;
  }

  public static bool Contains(string item)
  {
    return MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary().ContainsKey(item);
  }

  public static void Remove(string item)
  {
    MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary(true).Remove(item);
  }

  public static void Clear() => MappedDiagnosticsLogicalContext.Clear(true);

  public static void Clear(bool free)
  {
    if (free)
      MappedDiagnosticsLogicalContext.SetThreadLocal((Dictionary<string, object>) null);
    else
      MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary(true).Clear();
  }

  private static void SetThreadLocal(Dictionary<string, object> newValue)
  {
    if (newValue == null)
      CallContext.FreeNamedDataSlot("NLog.AsyncableMappedDiagnosticsContext");
    else
      CallContext.LogicalSetData("NLog.AsyncableMappedDiagnosticsContext", (object) newValue);
  }

  private static Dictionary<string, object> GetThreadLocal()
  {
    return CallContext.LogicalGetData("NLog.AsyncableMappedDiagnosticsContext") as Dictionary<string, object>;
  }

  private sealed class ItemRemover : IDisposable
  {
    private readonly string _item1;
    private readonly string _item2;
    private readonly string _item3;
    private readonly string[] _itemArray;
    private int _disposed;
    private readonly bool _wasEmpty;

    public ItemRemover(string item, bool wasEmpty)
    {
      this._item1 = item;
      this._wasEmpty = wasEmpty;
    }

    public ItemRemover(IReadOnlyList<KeyValuePair<string, object>> items, bool wasEmpty)
    {
      int count = items.Count;
      if (count > 2)
      {
        this._item1 = items[0].Key;
        this._item2 = items[1].Key;
        this._item3 = items[2].Key;
        for (int index = 3; index < count; ++index)
        {
          this._itemArray = this._itemArray ?? new string[count - 3];
          this._itemArray[index - 3] = items[index].Key;
        }
      }
      else if (count > 1)
      {
        this._item1 = items[0].Key;
        this._item2 = items[1].Key;
      }
      else
        this._item1 = items[0].Key;
      this._wasEmpty = wasEmpty;
    }

    public void Dispose()
    {
      if (Interlocked.Exchange(ref this._disposed, 1) != 0)
        return;
      if (this._wasEmpty && this.RemoveScopeWillClearContext())
      {
        MappedDiagnosticsLogicalContext.Clear(true);
      }
      else
      {
        IDictionary<string, object> threadDictionary = MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary(true);
        threadDictionary.Remove(this._item1);
        if (this._item2 == null)
          return;
        threadDictionary.Remove(this._item2);
        if (this._item3 != null)
          threadDictionary.Remove(this._item3);
        if (this._itemArray == null)
          return;
        for (int index = 0; index < this._itemArray.Length; ++index)
        {
          if (this._itemArray[index] != null)
            threadDictionary.Remove(this._itemArray[index]);
        }
      }
    }

    private bool RemoveScopeWillClearContext()
    {
      if (this._itemArray == null)
      {
        IDictionary<string, object> threadDictionary = MappedDiagnosticsLogicalContext.GetLogicalThreadDictionary();
        if (threadDictionary.Count == 1 && this._item2 == null && threadDictionary.ContainsKey(this._item1) || threadDictionary.Count == 2 && this._item2 != null && this._item3 == null && threadDictionary.ContainsKey(this._item1) && threadDictionary.ContainsKey(this._item2) && !this._item1.Equals(this._item2) || threadDictionary.Count == 3 && this._item3 != null && threadDictionary.ContainsKey(this._item1) && threadDictionary.ContainsKey(this._item2) && threadDictionary.ContainsKey(this._item3) && !this._item1.Equals(this._item2) && !this._item1.Equals(this._item3) && !this._item2.Equals(this._item3))
          return true;
      }
      return false;
    }

    public override string ToString() => this._item1?.ToString() ?? base.ToString();
  }
}
