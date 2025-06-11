// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.StoppedEventArgs
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio.Wave;

public class StoppedEventArgs : EventArgs
{
  private readonly Exception exception;

  public StoppedEventArgs(Exception exception = null) => this.exception = exception;

  public Exception Exception => this.exception;
}
