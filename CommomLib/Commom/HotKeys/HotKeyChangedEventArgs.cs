// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.HotKeyChangedEventArgs
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;

#nullable disable
namespace CommomLib.Commom.HotKeys;

public class HotKeyChangedEventArgs : EventArgs
{
  public HotKeyChangedEventArgs(HotKeyChangedAction action, string name, HotKeyItem hotKey)
  {
    this.Action = action;
    this.Name = name;
    this.HotKey = hotKey;
  }

  public HotKeyChangedAction Action { get; }

  public string Name { get; }

  public HotKeyItem HotKey { get; }
}
