// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.WindowLifetimeListenerEventArgs
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Windows;
using System.Windows.Interop;

#nullable disable
namespace CommomLib.Commom;

public class WindowLifetimeListenerEventArgs : EventArgs
{
  public HwndSource HwndSource { get; internal set; }

  public Window Window { get; internal set; }
}
