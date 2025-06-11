// Decompiled with JetBrains decompiler
// Type: NLog.Common.InternalLoggerMessageEventArgs
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using System;

#nullable disable
namespace NLog.Common;

public sealed class InternalLoggerMessageEventArgs : EventArgs
{
  public string Message { get; }

  public NLog.LogLevel Level { get; }

  [CanBeNull]
  public Exception Exception { get; }

  [CanBeNull]
  public Type SenderType { get; }

  [CanBeNull]
  public string SenderName { get; }

  internal InternalLoggerMessageEventArgs(
    string message,
    NLog.LogLevel level,
    [CanBeNull] Exception exception,
    [CanBeNull] Type senderType,
    [CanBeNull] string senderName)
  {
    this.Message = message;
    this.Level = level;
    this.Exception = exception;
    this.SenderType = senderType;
    this.SenderName = senderName;
  }
}
