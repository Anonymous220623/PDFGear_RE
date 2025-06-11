// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.HotKeyInvokedEventArgs
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;

#nullable disable
namespace CommomLib.Commom.HotKeys;

public class HotKeyInvokedEventArgs : EventArgs
{
  public HotKeyInvokedEventArgs(string hotKeyName, bool isRepeat)
  {
    this.HotKeyName = hotKeyName;
    this.IsRepeat = isRepeat;
  }

  public string HotKeyName { get; }

  public bool IsRepeat { get; }

  public bool Handled { get; set; }
}
