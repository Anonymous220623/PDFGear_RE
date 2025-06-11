// Decompiled with JetBrains decompiler
// Type: NLog.Internal.LoggerConfiguration
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Internal;

internal class LoggerConfiguration
{
  private readonly TargetWithFilterChain[] _targetsByLevel;

  public LoggerConfiguration(TargetWithFilterChain[] targetsByLevel, bool exceptionLoggingOldStyle)
  {
    this._targetsByLevel = targetsByLevel;
    this.ExceptionLoggingOldStyle = exceptionLoggingOldStyle;
  }

  [Obsolete("This property marked obsolete before v4.3.11 and it will be removed in NLog 5.")]
  public bool ExceptionLoggingOldStyle { get; }

  public TargetWithFilterChain GetTargetsForLevel(NLog.LogLevel level)
  {
    return level == NLog.LogLevel.Off ? (TargetWithFilterChain) null : this._targetsByLevel[level.Ordinal];
  }
}
