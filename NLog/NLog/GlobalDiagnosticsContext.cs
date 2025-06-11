// Decompiled with JetBrains decompiler
// Type: NLog.GlobalDiagnosticsContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog;

public static class GlobalDiagnosticsContext
{
  private static readonly object _lockObject = new object();
  private static Dictionary<string, object> _dict = new Dictionary<string, object>();
  private static Dictionary<string, object> _dictReadOnly;

  public static void Set(string item, string value)
  {
    GlobalDiagnosticsContext.Set(item, (object) value);
  }

  public static void Set(string item, object value)
  {
    lock (GlobalDiagnosticsContext._lockObject)
      GlobalDiagnosticsContext.GetWritableDict(GlobalDiagnosticsContext._dictReadOnly != null && !GlobalDiagnosticsContext._dict.ContainsKey(item))[item] = value;
  }

  public static string Get(string item)
  {
    return GlobalDiagnosticsContext.Get(item, (IFormatProvider) null);
  }

  public static string Get(string item, IFormatProvider formatProvider)
  {
    return FormatHelper.ConvertToString(GlobalDiagnosticsContext.GetObject(item), formatProvider);
  }

  public static object GetObject(string item)
  {
    object obj;
    GlobalDiagnosticsContext.GetReadOnlyDict().TryGetValue(item, out obj);
    return obj;
  }

  public static ICollection<string> GetNames()
  {
    return (ICollection<string>) GlobalDiagnosticsContext.GetReadOnlyDict().Keys;
  }

  public static bool Contains(string item)
  {
    return GlobalDiagnosticsContext.GetReadOnlyDict().ContainsKey(item);
  }

  public static void Remove(string item)
  {
    lock (GlobalDiagnosticsContext._lockObject)
      GlobalDiagnosticsContext.GetWritableDict(GlobalDiagnosticsContext._dictReadOnly != null && GlobalDiagnosticsContext._dict.ContainsKey(item)).Remove(item);
  }

  public static void Clear()
  {
    lock (GlobalDiagnosticsContext._lockObject)
      GlobalDiagnosticsContext.GetWritableDict(GlobalDiagnosticsContext._dictReadOnly != null && GlobalDiagnosticsContext._dict.Count > 0, true).Clear();
  }

  private static Dictionary<string, object> GetReadOnlyDict()
  {
    Dictionary<string, object> readOnlyDict = GlobalDiagnosticsContext._dictReadOnly;
    if (readOnlyDict == null)
    {
      lock (GlobalDiagnosticsContext._lockObject)
        readOnlyDict = GlobalDiagnosticsContext._dictReadOnly = GlobalDiagnosticsContext._dict;
    }
    return readOnlyDict;
  }

  private static Dictionary<string, object> GetWritableDict(
    bool requireCopyOnWrite,
    bool clearDictionary = false)
  {
    if (requireCopyOnWrite)
    {
      GlobalDiagnosticsContext._dict = GlobalDiagnosticsContext.CopyDictionaryOnWrite(clearDictionary);
      GlobalDiagnosticsContext._dictReadOnly = (Dictionary<string, object>) null;
    }
    return GlobalDiagnosticsContext._dict;
  }

  private static Dictionary<string, object> CopyDictionaryOnWrite(bool clearDictionary)
  {
    Dictionary<string, object> dictionary = new Dictionary<string, object>(clearDictionary ? 0 : GlobalDiagnosticsContext._dict.Count + 1);
    if (!clearDictionary)
    {
      foreach (KeyValuePair<string, object> keyValuePair in GlobalDiagnosticsContext._dict)
        dictionary[keyValuePair.Key] = keyValuePair.Value;
    }
    return dictionary;
  }
}
