// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingConfigurationReloadedEventArgs
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Config;

public class LoggingConfigurationReloadedEventArgs : EventArgs
{
  public LoggingConfigurationReloadedEventArgs(bool succeeded) => this.Succeeded = succeeded;

  public LoggingConfigurationReloadedEventArgs(bool succeeded, Exception exception)
  {
    this.Succeeded = succeeded;
    this.Exception = exception;
  }

  public bool Succeeded { get; private set; }

  public Exception Exception { get; private set; }
}
