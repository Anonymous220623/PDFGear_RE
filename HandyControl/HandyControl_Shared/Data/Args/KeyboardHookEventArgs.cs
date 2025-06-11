// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.KeyboardHookEventArgs
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Data;

public class KeyboardHookEventArgs : EventArgs
{
  public bool IsSystemKey { get; }

  public Key Key { get; }

  public KeyboardHookEventArgs(int virtualKey, bool isSystemKey)
  {
    this.IsSystemKey = isSystemKey;
    this.Key = KeyInterop.KeyFromVirtualKey(virtualKey);
  }
}
