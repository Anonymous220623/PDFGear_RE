// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.HotKeyListener
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.Commom.HotKeys;

internal static class HotKeyListener
{
  private static bool hasEvent;
  private static ConcurrentDictionary<HotKeyItem, string> hotKeyNames = new ConcurrentDictionary<HotKeyItem, string>();

  private static Dispatcher Dispatcher
  {
    get
    {
      Dispatcher dispatcher = Application.Current.Dispatcher;
      return dispatcher != null && !dispatcher.HasShutdownStarted ? dispatcher : (Dispatcher) null;
    }
  }

  private static IKeyboardEventListener KeyboardEventListener
  {
    get => CommomLib.Commom.HotKeys.KeyboardEventListener.HookKeyEventListener;
  }

  public static bool Set(HotKeyItem hotKeyItem, string name)
  {
    if (!HotKeyListener.ValidKeyName(name) || !hotKeyItem.IsSupported)
      return false;
    HotKeyListener.VerifyAccess();
    bool flag = false;
    if (HotKeyListener.hotKeyNames.GetOrAdd(hotKeyItem, name) == name)
    {
      EventHandler<HotKeyChangedEventArgs> hotKeyChanged = HotKeyListener.HotKeyChanged;
      if (hotKeyChanged != null)
        hotKeyChanged((object) null, new HotKeyChangedEventArgs(HotKeyChangedAction.Set, name, hotKeyItem));
      flag = true;
    }
    if (!HotKeyListener.hasEvent && HotKeyListener.hotKeyNames.Count > 0)
    {
      lock (HotKeyListener.hotKeyNames)
      {
        if (!HotKeyListener.hasEvent)
        {
          if (HotKeyListener.hotKeyNames.Count > 0)
          {
            HotKeyListener.KeyboardEventListener.KeyDown -= new CommomLib.Commom.HotKeys.KeyboardEventListener.KeyDownEventHandler(HotKeyListener.OnKeyDown);
            HotKeyListener.KeyboardEventListener.KeyDown += new CommomLib.Commom.HotKeys.KeyboardEventListener.KeyDownEventHandler(HotKeyListener.OnKeyDown);
            HotKeyListener.hasEvent = true;
          }
        }
      }
    }
    return flag;
  }

  public static bool Unset(HotKeyItem hotKeyItem, string name)
  {
    if (!HotKeyListener.ValidKeyName(name))
      return false;
    HotKeyListener.VerifyAccess();
    bool flag = false;
    lock (HotKeyListener.hotKeyNames)
    {
      string name1;
      if (HotKeyListener.hotKeyNames.TryGetValue(hotKeyItem, out name1) && name == name1 && HotKeyListener.hotKeyNames.TryRemove(hotKeyItem, out name1))
      {
        EventHandler<HotKeyChangedEventArgs> hotKeyChanged = HotKeyListener.HotKeyChanged;
        if (hotKeyChanged != null)
          hotKeyChanged((object) null, new HotKeyChangedEventArgs(HotKeyChangedAction.Unset, name1, hotKeyItem));
      }
      if (HotKeyListener.hasEvent)
      {
        if (HotKeyListener.hotKeyNames.Count == 0)
        {
          HotKeyListener.KeyboardEventListener.KeyDown -= new CommomLib.Commom.HotKeys.KeyboardEventListener.KeyDownEventHandler(HotKeyListener.OnKeyDown);
          HotKeyListener.hasEvent = false;
        }
      }
    }
    return flag;
  }

  public static bool Unset(string name)
  {
    if (!HotKeyListener.ValidKeyName(name))
      return false;
    HotKeyListener.VerifyAccess();
    bool flag = false;
    lock (HotKeyListener.hotKeyNames)
    {
      KeyValuePair<HotKeyItem, string>[] array = HotKeyListener.hotKeyNames.ToArray();
      for (int index = 0; index < array.Length && !flag; ++index)
      {
        string name1 = array[index].Value;
        if (name1 == name)
        {
          flag = true;
          HotKeyListener.hotKeyNames.TryRemove(array[index].Key, out string _);
          HotKeyItem key = array[index].Key;
          EventHandler<HotKeyChangedEventArgs> hotKeyChanged = HotKeyListener.HotKeyChanged;
          if (hotKeyChanged != null)
            hotKeyChanged((object) null, new HotKeyChangedEventArgs(HotKeyChangedAction.Unset, name1, key));
        }
      }
      if (HotKeyListener.hasEvent)
      {
        if (HotKeyListener.hotKeyNames.Count == 0)
        {
          HotKeyListener.KeyboardEventListener.KeyDown -= new CommomLib.Commom.HotKeys.KeyboardEventListener.KeyDownEventHandler(HotKeyListener.OnKeyDown);
          HotKeyListener.hasEvent = false;
        }
      }
    }
    return flag;
  }

  public static bool Unset(HotKeyItem hotKey, out string name)
  {
    HotKeyListener.VerifyAccess();
    bool flag = false;
    lock (HotKeyListener.hotKeyNames)
    {
      if (HotKeyListener.hotKeyNames.TryRemove(hotKey, out name))
      {
        flag = true;
        EventHandler<HotKeyChangedEventArgs> hotKeyChanged = HotKeyListener.HotKeyChanged;
        if (hotKeyChanged != null)
          hotKeyChanged((object) null, new HotKeyChangedEventArgs(HotKeyChangedAction.Unset, name, hotKey));
      }
    }
    if (HotKeyListener.hasEvent && HotKeyListener.hotKeyNames.Count == 0)
    {
      HotKeyListener.KeyboardEventListener.KeyDown -= new CommomLib.Commom.HotKeys.KeyboardEventListener.KeyDownEventHandler(HotKeyListener.OnKeyDown);
      HotKeyListener.hasEvent = false;
    }
    return flag;
  }

  public static void Clear()
  {
    HotKeyListener.VerifyAccess();
    lock (HotKeyListener.hotKeyNames)
    {
      HotKeyListener.hotKeyNames.Clear();
      if (HotKeyListener.hasEvent)
      {
        HotKeyListener.KeyboardEventListener.KeyDown -= new CommomLib.Commom.HotKeys.KeyboardEventListener.KeyDownEventHandler(HotKeyListener.OnKeyDown);
        HotKeyListener.hasEvent = false;
      }
      EventHandler<HotKeyChangedEventArgs> hotKeyChanged = HotKeyListener.HotKeyChanged;
      if (hotKeyChanged == null)
        return;
      hotKeyChanged((object) null, new HotKeyChangedEventArgs(HotKeyChangedAction.Clear, string.Empty, new HotKeyItem()));
    }
  }

  public static bool TryGetHotKey(string name, out HotKeyItem hotKey)
  {
    hotKey = new HotKeyItem();
    if (!HotKeyListener.ValidKeyName(name))
      return false;
    KeyValuePair<HotKeyItem, string>[] array = HotKeyListener.hotKeyNames.ToArray();
    for (int index = 0; index < array.Length; ++index)
    {
      if (array[index].Value == name)
      {
        hotKey = array[index].Key;
        return true;
      }
    }
    return false;
  }

  private static void VerifyAccess()
  {
    (HotKeyListener.Dispatcher ?? throw new InvalidOperationException("Dispatcher")).VerifyAccess();
  }

  private static void OnKeyDown(object sender, CommomLib.Commom.HotKeys.KeyboardEventListener.KeyDownEventArgs e)
  {
    HotKeyItem key = new HotKeyItem(e.VirtualKey, e.Modifier);
    string name;
    if (!HotKeyListener.hotKeyNames.TryGetValue(key, out name))
      return;
    EventHandler<HotKeyInvokedEventArgs> handler = HotKeyListener.HotKeyInvoked;
    if (handler == null)
      return;
    Dispatcher dispatcher = HotKeyListener.Dispatcher;
    if (dispatcher == null)
      return;
    bool repeat = e.PrevKeyPressed;
    if (dispatcher.CheckAccess())
    {
      HotKeyInvokedEventArgs e1 = new HotKeyInvokedEventArgs(name, repeat);
      handler((object) null, e1);
      e.Handled = e1.Handled;
    }
    else
    {
      bool handled = false;
      dispatcher.InvokeAsync((Action) (() =>
      {
        try
        {
          HotKeyInvokedEventArgs e2 = new HotKeyInvokedEventArgs(name, repeat);
          handler((object) null, e2);
          handled = e2.Handled;
        }
        finally
        {
          lock (e)
            Monitor.Pulse((object) e);
        }
      }), DispatcherPriority.Send);
      lock (e)
        Monitor.Wait((object) e, 200);
      e.Handled = handled;
    }
  }

  public static event EventHandler<HotKeyInvokedEventArgs> HotKeyInvoked;

  public static event EventHandler<HotKeyChangedEventArgs> HotKeyChanged;

  internal static bool ValidKeyName(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      return false;
    for (int index = 0; index < name.Length; ++index)
    {
      if (char.IsWhiteSpace(name[index]) || name[index] == ',')
        return false;
    }
    return true;
  }

  internal static string[] SplitKeyNameGroup(string nameGroup)
  {
    if (string.IsNullOrEmpty(nameGroup))
      return Array.Empty<string>();
    string[] array = ((IEnumerable<string>) nameGroup.Split(',', ' ')).Select<string, string>((Func<string, string>) (c => c.Trim())).Where<string>((Func<string, bool>) (c => !string.IsNullOrEmpty(c))).ToArray<string>();
    return array.Length != 0 ? array : Array.Empty<string>();
  }
}
