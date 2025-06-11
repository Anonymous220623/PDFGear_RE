// Decompiled with JetBrains decompiler
// Type: NLog.Config.InstallationContext
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;

#nullable disable
namespace NLog.Config;

public sealed class InstallationContext : IDisposable
{
  private static readonly Dictionary<NLog.LogLevel, ConsoleColor> LogLevel2ConsoleColor = new Dictionary<NLog.LogLevel, ConsoleColor>()
  {
    {
      NLog.LogLevel.Trace,
      ConsoleColor.DarkGray
    },
    {
      NLog.LogLevel.Debug,
      ConsoleColor.Gray
    },
    {
      NLog.LogLevel.Info,
      ConsoleColor.White
    },
    {
      NLog.LogLevel.Warn,
      ConsoleColor.Yellow
    },
    {
      NLog.LogLevel.Error,
      ConsoleColor.Red
    },
    {
      NLog.LogLevel.Fatal,
      ConsoleColor.DarkRed
    }
  };

  public InstallationContext()
    : this(TextWriter.Null)
  {
  }

  public InstallationContext(TextWriter logOutput)
  {
    this.LogOutput = logOutput;
    this.Parameters = (IDictionary<string, string>) new Dictionary<string, string>();
    this.LogLevel = NLog.LogLevel.Info;
    this.ThrowExceptions = false;
  }

  public NLog.LogLevel LogLevel { get; set; }

  public bool IgnoreFailures { get; set; }

  public bool ThrowExceptions { get; set; }

  public IDictionary<string, string> Parameters { get; private set; }

  public TextWriter LogOutput { get; set; }

  public void Trace([Localizable(false)] string message, params object[] arguments)
  {
    this.Log(NLog.LogLevel.Trace, message, arguments);
  }

  public void Debug([Localizable(false)] string message, params object[] arguments)
  {
    this.Log(NLog.LogLevel.Debug, message, arguments);
  }

  public void Info([Localizable(false)] string message, params object[] arguments)
  {
    this.Log(NLog.LogLevel.Info, message, arguments);
  }

  public void Warning([Localizable(false)] string message, params object[] arguments)
  {
    this.Log(NLog.LogLevel.Warn, message, arguments);
  }

  public void Error([Localizable(false)] string message, params object[] arguments)
  {
    this.Log(NLog.LogLevel.Error, message, arguments);
  }

  public void Dispose()
  {
    if (this.LogOutput == null)
      return;
    this.LogOutput.Close();
    this.LogOutput = (TextWriter) null;
  }

  public LogEventInfo CreateLogEvent()
  {
    LogEventInfo nullEvent = LogEventInfo.CreateNullEvent();
    foreach (KeyValuePair<string, string> parameter in (IEnumerable<KeyValuePair<string, string>>) this.Parameters)
      nullEvent.Properties.Add((object) parameter.Key, (object) parameter.Value);
    return nullEvent;
  }

  private void Log(NLog.LogLevel logLevel, [Localizable(false)] string message, object[] arguments)
  {
    if (!(logLevel >= this.LogLevel))
      return;
    if (arguments != null && arguments.Length != 0)
      message = string.Format((IFormatProvider) CultureInfo.InvariantCulture, message, arguments);
    ConsoleColor foregroundColor = Console.ForegroundColor;
    Console.ForegroundColor = InstallationContext.LogLevel2ConsoleColor[logLevel];
    try
    {
      this.LogOutput.WriteLine(message);
    }
    finally
    {
      Console.ForegroundColor = foregroundColor;
    }
  }
}
