// Decompiled with JetBrains decompiler
// Type: CommomLib.AppTheme.ActualThemeChangedEventArgs
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;

#nullable disable
namespace CommomLib.AppTheme;

public class ActualThemeChangedEventArgs : EventArgs
{
  internal ActualThemeChangedEventArgs(string oldTheme, string newTheme)
  {
    this.OldTheme = oldTheme;
    this.NewTheme = newTheme;
  }

  public string OldTheme { get; }

  public string NewTheme { get; }
}
