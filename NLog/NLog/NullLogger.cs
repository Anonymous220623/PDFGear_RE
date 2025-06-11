// Decompiled with JetBrains decompiler
// Type: NLog.NullLogger
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;

#nullable disable
namespace NLog;

public sealed class NullLogger : Logger
{
  public NullLogger(LogFactory factory)
  {
    if (factory == null)
      throw new ArgumentNullException(nameof (factory));
    TargetWithFilterChain[] targetsByLevel = new TargetWithFilterChain[LogLevel.MaxLevel.Ordinal + 1];
    this.Initialize(string.Empty, new LoggerConfiguration(targetsByLevel, false), factory);
  }
}
