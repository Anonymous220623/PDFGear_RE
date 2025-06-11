// Decompiled with JetBrains decompiler
// Type: NLog.MappedDiagnosticsContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog;

public static class MappedDiagnosticsContext
{
  private static readonly object DataSlot = ThreadLocalStorageHelper.AllocateDataSlot();

  private static IDictionary<string, object> GetThreadDictionary(bool create = true)
  {
    return (IDictionary<string, object>) ThreadLocalStorageHelper.GetDataForSlot<Dictionary<string, object>>(MappedDiagnosticsContext.DataSlot, create);
  }

  public static IDisposable SetScoped(string item, string value)
  {
    MappedDiagnosticsContext.Set(item, value);
    return (IDisposable) new MappedDiagnosticsContext.ItemRemover(item);
  }

  public static IDisposable SetScoped(string item, object value)
  {
    MappedDiagnosticsContext.Set(item, value);
    return (IDisposable) new MappedDiagnosticsContext.ItemRemover(item);
  }

  public static void Set(string item, string value)
  {
    MappedDiagnosticsContext.GetThreadDictionary()[item] = (object) value;
  }

  public static void Set(string item, object value)
  {
    MappedDiagnosticsContext.GetThreadDictionary()[item] = value;
  }

  public static string Get(string item)
  {
    return MappedDiagnosticsContext.Get(item, (IFormatProvider) null);
  }

  public static string Get(string item, IFormatProvider formatProvider)
  {
    return FormatHelper.ConvertToString(MappedDiagnosticsContext.GetObject(item), formatProvider);
  }

  public static object GetObject(string item)
  {
    IDictionary<string, object> threadDictionary = MappedDiagnosticsContext.GetThreadDictionary(false);
    object obj;
    return threadDictionary != null && threadDictionary.TryGetValue(item, out obj) ? obj : (object) null;
  }

  public static ICollection<string> GetNames()
  {
    return MappedDiagnosticsContext.GetThreadDictionary(false)?.Keys ?? (ICollection<string>) ArrayHelper.Empty<string>();
  }

  public static bool Contains(string item)
  {
    IDictionary<string, object> threadDictionary = MappedDiagnosticsContext.GetThreadDictionary(false);
    return threadDictionary != null && threadDictionary.ContainsKey(item);
  }

  public static void Remove(string item)
  {
    MappedDiagnosticsContext.GetThreadDictionary().Remove(item);
  }

  public static void Clear() => MappedDiagnosticsContext.GetThreadDictionary(false)?.Clear();

  private sealed class ItemRemover : IDisposable
  {
    private readonly string _item;
    private bool _disposed;

    public ItemRemover(string item) => this._item = item;

    public void Dispose()
    {
      if (this._disposed)
        return;
      this._disposed = true;
      MappedDiagnosticsContext.Remove(this._item);
    }
  }
}
