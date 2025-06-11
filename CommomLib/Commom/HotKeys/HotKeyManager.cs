// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.HotKeyManager
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;

#nullable disable
namespace CommomLib.Commom.HotKeys;

public static class HotKeyManager
{
  private static ConcurrentDictionary<string, HotKeyModel> models = new ConcurrentDictionary<string, HotKeyModel>();
  private static List<string> names = new List<string>();

  public static IReadOnlyList<string> Names
  {
    get => (IReadOnlyList<string>) HotKeyManager.names.ToArray();
  }

  public static bool Remove(string name)
  {
    lock (HotKeyManager.models)
    {
      if (!HotKeyManager.models.TryRemove(name, out HotKeyModel _))
        return false;
      HotKeyManager.names.Remove(name);
      return true;
    }
  }

  public static HotKeyModel GetOrCreate(string name)
  {
    return HotKeyManager.GetOrCreate(name, new HotKeyItem(), (Action<HotKeyModel>) null);
  }

  public static HotKeyModel GetOrCreate(string name, HotKeyItem defaultValue)
  {
    return HotKeyManager.GetOrCreate(name, defaultValue, (Action<HotKeyModel>) null);
  }

  public static HotKeyModel GetOrCreate(string name, Action<HotKeyModel> configAction)
  {
    return HotKeyManager.GetOrCreate(name, new HotKeyItem(), configAction);
  }

  public static HotKeyModel GetOrCreate(
    string name,
    HotKeyItem defaultValue,
    Action<HotKeyModel> configAction)
  {
    if (!HotKeyListener.ValidKeyName(name))
      return (HotKeyModel) null;
    HotKeyModel hotKeyModel;
    lock (HotKeyManager.models)
    {
      if (HotKeyManager.models.TryGetValue(name, out hotKeyModel))
        return hotKeyModel;
      hotKeyModel = new HotKeyModel(name, defaultValue);
      HotKeyManager.models[name] = hotKeyModel;
      HotKeyManager.names.Add(name);
    }
    if (hotKeyModel != null && configAction != null)
      configAction(hotKeyModel);
    return hotKeyModel;
  }

  public static void ResetAllKeysToDefault()
  {
    lock (HotKeyManager.models)
    {
      HotKeyListener.Clear();
      foreach (HotKeyModel hotKeyModel in (IEnumerable<HotKeyModel>) HotKeyManager.models.Values)
        hotKeyModel.ResetToDefault();
      try
      {
        SqliteUtils.WaitForInit().Wait();
        using (SQLiteConnection connectionUnsafe = SqliteUtils.GetConnectionUnsafe())
        {
          using (SQLiteCommand sqLiteCommand = new SQLiteCommand("DELETE FROM configs WHERE key LIKE 'HOTKEY_%'", connectionUnsafe))
            sqLiteCommand.ExecuteReader();
        }
      }
      catch
      {
      }
    }
  }

  internal static bool ContainsKey(string name) => HotKeyManager.models.ContainsKey(name);

  internal static bool IsHotKeyModelEnabled(string name, bool isRepeat)
  {
    HotKeyModel hotKeyModel;
    return HotKeyManager.models.TryGetValue(name, out hotKeyModel) && hotKeyModel.IsHotKeyModelEnabled(isRepeat);
  }
}
