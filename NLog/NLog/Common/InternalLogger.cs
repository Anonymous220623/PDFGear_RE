// Decompiled with JetBrains decompiler
// Type: NLog.Common.InternalLogger
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Internal;
using NLog.Targets;
using NLog.Time;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;

#nullable disable
namespace NLog.Common;

public static class InternalLogger
{
  private static readonly object LockObject = new object();
  private static string _logFile;
  private static NLog.LogLevel _logLevel;

  public static bool IsTraceEnabled => InternalLogger.IsLogLevelEnabled(NLog.LogLevel.Trace);

  public static bool IsDebugEnabled => InternalLogger.IsLogLevelEnabled(NLog.LogLevel.Debug);

  public static bool IsInfoEnabled => InternalLogger.IsLogLevelEnabled(NLog.LogLevel.Info);

  public static bool IsWarnEnabled => InternalLogger.IsLogLevelEnabled(NLog.LogLevel.Warn);

  public static bool IsErrorEnabled => InternalLogger.IsLogLevelEnabled(NLog.LogLevel.Error);

  public static bool IsFatalEnabled => InternalLogger.IsLogLevelEnabled(NLog.LogLevel.Fatal);

  [StringFormatMethod("message")]
  public static void Trace([Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Trace, message, args);
  }

  public static void Trace([Localizable(false)] string message)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Trace, message, (object[]) null);
  }

  public static void Trace([Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsTraceEnabled)
      return;
    InternalLogger.Write((Exception) null, NLog.LogLevel.Trace, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Trace(Exception ex, [Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Trace, message, args);
  }

  [StringFormatMethod("message")]
  public static void Trace<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
  {
    if (!InternalLogger.IsTraceEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Trace, message, (object) arg0);
  }

  [StringFormatMethod("message")]
  public static void Trace<TArgument1, TArgument2>(
    [Localizable(false)] string message,
    TArgument1 arg0,
    TArgument2 arg1)
  {
    if (!InternalLogger.IsTraceEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Trace, message, (object) arg0, (object) arg1);
  }

  [StringFormatMethod("message")]
  public static void Trace<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 arg0,
    TArgument2 arg1,
    TArgument3 arg2)
  {
    if (!InternalLogger.IsTraceEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Trace, message, (object) arg0, (object) arg1, (object) arg2);
  }

  public static void Trace(Exception ex, [Localizable(false)] string message)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Trace, message, (object[]) null);
  }

  public static void Trace(Exception ex, [Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsTraceEnabled)
      return;
    InternalLogger.Write(ex, NLog.LogLevel.Trace, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Debug([Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Debug, message, args);
  }

  public static void Debug([Localizable(false)] string message)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Debug, message, (object[]) null);
  }

  public static void Debug([Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsDebugEnabled)
      return;
    InternalLogger.Write((Exception) null, NLog.LogLevel.Debug, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Debug(Exception ex, [Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Debug, message, args);
  }

  [StringFormatMethod("message")]
  public static void Debug<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
  {
    if (!InternalLogger.IsDebugEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Debug, message, (object) arg0);
  }

  [StringFormatMethod("message")]
  public static void Debug<TArgument1, TArgument2>(
    [Localizable(false)] string message,
    TArgument1 arg0,
    TArgument2 arg1)
  {
    if (!InternalLogger.IsDebugEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Debug, message, (object) arg0, (object) arg1);
  }

  [StringFormatMethod("message")]
  public static void Debug<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 arg0,
    TArgument2 arg1,
    TArgument3 arg2)
  {
    if (!InternalLogger.IsDebugEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Debug, message, (object) arg0, (object) arg1, (object) arg2);
  }

  public static void Debug(Exception ex, [Localizable(false)] string message)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Debug, message, (object[]) null);
  }

  public static void Debug(Exception ex, [Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsDebugEnabled)
      return;
    InternalLogger.Write(ex, NLog.LogLevel.Debug, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Info([Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Info, message, args);
  }

  public static void Info([Localizable(false)] string message)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Info, message, (object[]) null);
  }

  public static void Info([Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsInfoEnabled)
      return;
    InternalLogger.Write((Exception) null, NLog.LogLevel.Info, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Info(Exception ex, [Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Info, message, args);
  }

  [StringFormatMethod("message")]
  public static void Info<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
  {
    if (!InternalLogger.IsInfoEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Info, message, (object) arg0);
  }

  [StringFormatMethod("message")]
  public static void Info<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 arg0, TArgument2 arg1)
  {
    if (!InternalLogger.IsInfoEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Info, message, (object) arg0, (object) arg1);
  }

  [StringFormatMethod("message")]
  public static void Info<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 arg0,
    TArgument2 arg1,
    TArgument3 arg2)
  {
    if (!InternalLogger.IsInfoEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Info, message, (object) arg0, (object) arg1, (object) arg2);
  }

  public static void Info(Exception ex, [Localizable(false)] string message)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Info, message, (object[]) null);
  }

  public static void Info(Exception ex, [Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsInfoEnabled)
      return;
    InternalLogger.Write(ex, NLog.LogLevel.Info, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Warn([Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Warn, message, args);
  }

  public static void Warn([Localizable(false)] string message)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Warn, message, (object[]) null);
  }

  public static void Warn([Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsWarnEnabled)
      return;
    InternalLogger.Write((Exception) null, NLog.LogLevel.Warn, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Warn(Exception ex, [Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Warn, message, args);
  }

  [StringFormatMethod("message")]
  public static void Warn<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
  {
    if (!InternalLogger.IsWarnEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Warn, message, (object) arg0);
  }

  [StringFormatMethod("message")]
  public static void Warn<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 arg0, TArgument2 arg1)
  {
    if (!InternalLogger.IsWarnEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Warn, message, (object) arg0, (object) arg1);
  }

  [StringFormatMethod("message")]
  public static void Warn<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 arg0,
    TArgument2 arg1,
    TArgument3 arg2)
  {
    if (!InternalLogger.IsWarnEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Warn, message, (object) arg0, (object) arg1, (object) arg2);
  }

  public static void Warn(Exception ex, [Localizable(false)] string message)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Warn, message, (object[]) null);
  }

  public static void Warn(Exception ex, [Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsWarnEnabled)
      return;
    InternalLogger.Write(ex, NLog.LogLevel.Warn, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Error([Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Error, message, args);
  }

  public static void Error([Localizable(false)] string message)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Error, message, (object[]) null);
  }

  public static void Error([Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsErrorEnabled)
      return;
    InternalLogger.Write((Exception) null, NLog.LogLevel.Error, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Error(Exception ex, [Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Error, message, args);
  }

  [StringFormatMethod("message")]
  public static void Error<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
  {
    if (!InternalLogger.IsErrorEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Error, message, (object) arg0);
  }

  [StringFormatMethod("message")]
  public static void Error<TArgument1, TArgument2>(
    [Localizable(false)] string message,
    TArgument1 arg0,
    TArgument2 arg1)
  {
    if (!InternalLogger.IsErrorEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Error, message, (object) arg0, (object) arg1);
  }

  [StringFormatMethod("message")]
  public static void Error<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 arg0,
    TArgument2 arg1,
    TArgument3 arg2)
  {
    if (!InternalLogger.IsErrorEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Error, message, (object) arg0, (object) arg1, (object) arg2);
  }

  public static void Error(Exception ex, [Localizable(false)] string message)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Error, message, (object[]) null);
  }

  public static void Error(Exception ex, [Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsErrorEnabled)
      return;
    InternalLogger.Write(ex, NLog.LogLevel.Error, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Fatal([Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Fatal, message, args);
  }

  public static void Fatal([Localizable(false)] string message)
  {
    InternalLogger.Write((Exception) null, NLog.LogLevel.Fatal, message, (object[]) null);
  }

  public static void Fatal([Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsFatalEnabled)
      return;
    InternalLogger.Write((Exception) null, NLog.LogLevel.Fatal, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Fatal(Exception ex, [Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Fatal, message, args);
  }

  [StringFormatMethod("message")]
  public static void Fatal<TArgument1>([Localizable(false)] string message, TArgument1 arg0)
  {
    if (!InternalLogger.IsFatalEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Fatal, message, (object) arg0);
  }

  [StringFormatMethod("message")]
  public static void Fatal<TArgument1, TArgument2>(
    [Localizable(false)] string message,
    TArgument1 arg0,
    TArgument2 arg1)
  {
    if (!InternalLogger.IsFatalEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Fatal, message, (object) arg0, (object) arg1);
  }

  [StringFormatMethod("message")]
  public static void Fatal<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 arg0,
    TArgument2 arg1,
    TArgument3 arg2)
  {
    if (!InternalLogger.IsFatalEnabled)
      return;
    InternalLogger.Log((Exception) null, NLog.LogLevel.Fatal, message, (object) arg0, (object) arg1, (object) arg2);
  }

  public static void Fatal(Exception ex, [Localizable(false)] string message)
  {
    InternalLogger.Write(ex, NLog.LogLevel.Fatal, message, (object[]) null);
  }

  public static void Fatal(Exception ex, [Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsFatalEnabled)
      return;
    InternalLogger.Write(ex, NLog.LogLevel.Fatal, messageFunc(), (object[]) null);
  }

  static InternalLogger() => InternalLogger.Reset();

  public static void Reset()
  {
    InternalLogger.LogToConsole = InternalLogger.GetSetting<bool>("nlog.internalLogToConsole", "NLOG_INTERNAL_LOG_TO_CONSOLE", false);
    InternalLogger.LogToConsoleError = InternalLogger.GetSetting<bool>("nlog.internalLogToConsoleError", "NLOG_INTERNAL_LOG_TO_CONSOLE_ERROR", false);
    InternalLogger.LogLevel = InternalLogger.GetSetting("nlog.internalLogLevel", "NLOG_INTERNAL_LOG_LEVEL", NLog.LogLevel.Info);
    InternalLogger.LogFile = InternalLogger.GetSetting<string>("nlog.internalLogFile", "NLOG_INTERNAL_LOG_FILE", string.Empty);
    InternalLogger.LogToTrace = InternalLogger.GetSetting<bool>("nlog.internalLogToTrace", "NLOG_INTERNAL_LOG_TO_TRACE", false);
    InternalLogger.IncludeTimestamp = InternalLogger.GetSetting<bool>("nlog.internalLogIncludeTimestamp", "NLOG_INTERNAL_INCLUDE_TIMESTAMP", true);
    InternalLogger.Info("NLog internal logger initialized.");
    InternalLogger.ExceptionThrowWhenWriting = false;
    InternalLogger.LogWriter = (TextWriter) null;
    InternalLogger.LogMessageReceived = (EventHandler<InternalLoggerMessageEventArgs>) null;
  }

  public static NLog.LogLevel LogLevel
  {
    get => InternalLogger._logLevel;
    set
    {
      NLog.LogLevel logLevel = value;
      if ((object) logLevel == null)
        logLevel = NLog.LogLevel.Info;
      InternalLogger._logLevel = logLevel;
    }
  }

  public static bool LogToConsole { get; set; }

  public static bool LogToConsoleError { get; set; }

  public static bool LogToTrace { get; set; }

  public static string LogFile
  {
    get => InternalLogger._logFile;
    set
    {
      InternalLogger._logFile = value;
      if (string.IsNullOrEmpty(InternalLogger._logFile))
        return;
      InternalLogger.CreateDirectoriesIfNeeded(InternalLogger._logFile);
    }
  }

  public static TextWriter LogWriter { get; set; }

  public static event EventHandler<InternalLoggerMessageEventArgs> LogMessageReceived;

  public static bool IncludeTimestamp { get; set; }

  internal static bool ExceptionThrowWhenWriting { get; private set; }

  [StringFormatMethod("message")]
  public static void Log(NLog.LogLevel level, [Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write((Exception) null, level, message, args);
  }

  public static void Log(NLog.LogLevel level, [Localizable(false)] string message)
  {
    InternalLogger.Write((Exception) null, level, message, (object[]) null);
  }

  public static void Log(NLog.LogLevel level, [Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsLogLevelEnabled(level))
      return;
    InternalLogger.Write((Exception) null, level, messageFunc(), (object[]) null);
  }

  public static void Log(Exception ex, NLog.LogLevel level, [Localizable(false)] Func<string> messageFunc)
  {
    if (!InternalLogger.IsLogLevelEnabled(level))
      return;
    InternalLogger.Write(ex, level, messageFunc(), (object[]) null);
  }

  [StringFormatMethod("message")]
  public static void Log(Exception ex, NLog.LogLevel level, [Localizable(false)] string message, params object[] args)
  {
    InternalLogger.Write(ex, level, message, args);
  }

  public static void Log(Exception ex, NLog.LogLevel level, [Localizable(false)] string message)
  {
    InternalLogger.Write(ex, level, message, (object[]) null);
  }

  private static void Write([CanBeNull] Exception ex, NLog.LogLevel level, string message, [CanBeNull] object[] args)
  {
    if (!InternalLogger.IsLogLevelEnabled(level) || InternalLogger.IsSeriousException(ex))
      return;
    bool flag1 = InternalLogger.HasActiveLoggersWithLine();
    bool flag2 = InternalLogger.HasEventListeners();
    if (!flag1 && !flag2)
      return;
    try
    {
      string fullMessage = InternalLogger.CreateFullMessage(message, args);
      if (flag1)
        InternalLogger.WriteLogLine(ex, level, fullMessage);
      if (!flag2)
        return;
      IInternalLoggerContext internalLoggerContext = args == null || args.Length == 0 ? (IInternalLoggerContext) null : args[0] as IInternalLoggerContext;
      EventHandler<InternalLoggerMessageEventArgs> logMessageReceived = InternalLogger.LogMessageReceived;
      if (logMessageReceived != null)
        logMessageReceived((object) null, new InternalLoggerMessageEventArgs(fullMessage, level, ex, internalLoggerContext?.GetType(), internalLoggerContext?.Name));
      if (ex == null)
        return;
      ex.MarkAsLoggedToInternalLogger();
    }
    catch (Exception ex1)
    {
      InternalLogger.ExceptionThrowWhenWriting = true;
      if (!ex1.MustBeRethrownImmediately())
        return;
      throw;
    }
  }

  private static void WriteLogLine(Exception ex, NLog.LogLevel level, string message)
  {
    try
    {
      string logLine = InternalLogger.CreateLogLine(ex, level, message);
      InternalLogger.WriteToLogFile(logLine);
      InternalLogger.WriteToTextWriter(logLine);
      InternalLogger.WriteToConsole(logLine);
      InternalLogger.WriteToErrorConsole(logLine);
      InternalLogger.WriteToTrace(logLine);
      if (ex == null)
        return;
      ex.MarkAsLoggedToInternalLogger();
    }
    catch (Exception ex1)
    {
      InternalLogger.ExceptionThrowWhenWriting = true;
      if (!ex1.MustBeRethrownImmediately())
        return;
      throw;
    }
  }

  private static string CreateLogLine([CanBeNull] Exception ex, NLog.LogLevel level, string fullMessage)
  {
    return InternalLogger.IncludeTimestamp ? $"{TimeSource.Current.Time.ToString("yyyy-MM-dd HH:mm:ss.ffff", (IFormatProvider) CultureInfo.InvariantCulture)} {level.ToString()} {fullMessage}{(ex != null ? " Exception: " : "")}{ex?.ToString() ?? ""}" : $"{level.ToString()} {fullMessage}{(ex != null ? " Exception: " : "")}{ex?.ToString() ?? ""}";
  }

  private static string CreateFullMessage(string message, object[] args)
  {
    return args != null ? string.Format((IFormatProvider) CultureInfo.InvariantCulture, message, args) : message;
  }

  private static bool IsSeriousException(Exception exception)
  {
    return exception != null && exception.MustBeRethrownImmediately();
  }

  private static bool IsLogLevelEnabled(NLog.LogLevel logLevel)
  {
    return (object) InternalLogger._logLevel != (object) NLog.LogLevel.Off && logLevel >= InternalLogger._logLevel;
  }

  internal static bool HasActiveLoggers()
  {
    return InternalLogger.HasActiveLoggersWithLine() || InternalLogger.HasEventListeners();
  }

  private static bool HasEventListeners() => InternalLogger.LogMessageReceived != null;

  internal static bool HasActiveLoggersWithLine()
  {
    return !string.IsNullOrEmpty(InternalLogger.LogFile) || InternalLogger.LogToConsole || InternalLogger.LogToConsoleError || InternalLogger.LogToTrace || InternalLogger.LogWriter != null;
  }

  private static void WriteToLogFile(string message)
  {
    string logFile = InternalLogger.LogFile;
    if (string.IsNullOrEmpty(logFile))
      return;
    lock (InternalLogger.LockObject)
    {
      using (StreamWriter streamWriter = File.AppendText(logFile))
        streamWriter.WriteLine(message);
    }
  }

  private static void WriteToTextWriter(string message)
  {
    TextWriter logWriter = InternalLogger.LogWriter;
    if (logWriter == null)
      return;
    lock (InternalLogger.LockObject)
      logWriter.WriteLine(message);
  }

  private static void WriteToConsole(string message)
  {
    if (!InternalLogger.LogToConsole)
      return;
    ConsoleTargetHelper.WriteLineThreadSafe(Console.Out, message);
  }

  private static void WriteToErrorConsole(string message)
  {
    if (!InternalLogger.LogToConsoleError)
      return;
    ConsoleTargetHelper.WriteLineThreadSafe(Console.Error, message);
  }

  private static void WriteToTrace(string message)
  {
    if (!InternalLogger.LogToTrace)
      return;
    System.Diagnostics.Trace.WriteLine(message, "NLog");
  }

  public static void LogAssemblyVersion(Assembly assembly)
  {
    try
    {
      FileVersionInfo versionInfo = !string.IsNullOrEmpty(assembly.Location) ? FileVersionInfo.GetVersionInfo(assembly.Location) : (FileVersionInfo) null;
      InternalLogger.Info("{0}. File version: {1}. Product version: {2}. GlobalAssemblyCache: {3}", (object) assembly.FullName, (object) versionInfo?.FileVersion, (object) versionInfo?.ProductVersion, (object) assembly.GlobalAssemblyCache);
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]
      {
        (object) assembly.FullName
      };
      InternalLogger.Error(ex, "Error logging version of assembly {0}.", objArray);
    }
  }

  private static string GetAppSettings(string configName)
  {
    try
    {
      return System.Configuration.ConfigurationManager.AppSettings[configName];
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
    }
    return (string) null;
  }

  private static string GetSettingString(string configName, string envName)
  {
    try
    {
      string appSettings = InternalLogger.GetAppSettings(configName);
      if (appSettings != null)
        return appSettings;
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
    }
    try
    {
      string environmentVariable = EnvironmentHelper.GetSafeEnvironmentVariable(envName);
      if (!string.IsNullOrEmpty(environmentVariable))
        return environmentVariable;
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
    }
    return (string) null;
  }

  private static NLog.LogLevel GetSetting(string configName, string envName, NLog.LogLevel defaultValue)
  {
    string settingString = InternalLogger.GetSettingString(configName, envName);
    if (settingString == null)
      return defaultValue;
    try
    {
      return NLog.LogLevel.FromString(settingString);
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return defaultValue;
      throw;
    }
  }

  private static T GetSetting<T>(string configName, string envName, T defaultValue)
  {
    string settingString = InternalLogger.GetSettingString(configName, envName);
    if (settingString == null)
      return defaultValue;
    try
    {
      return (T) Convert.ChangeType((object) settingString, typeof (T), (IFormatProvider) CultureInfo.InvariantCulture);
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return defaultValue;
      throw;
    }
  }

  private static void CreateDirectoriesIfNeeded(string filename)
  {
    try
    {
      if (InternalLogger.LogLevel == NLog.LogLevel.Off)
        return;
      string directoryName = Path.GetDirectoryName(filename);
      if (string.IsNullOrEmpty(directoryName))
        return;
      Directory.CreateDirectory(directoryName);
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "Cannot create needed directories to '{0}'.", (object) filename);
      if (!ex.MustBeRethrownImmediately())
        return;
      throw;
    }
  }
}
