// Decompiled with JetBrains decompiler
// Type: NLog.LogManager
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog;

public static class LogManager
{
  internal static readonly LogFactory factory = new LogFactory();
  private static ICollection<Assembly> _hiddenAssemblies;
  private static readonly object lockObject = new object();

  public static LogFactory LogFactory => LogManager.factory;

  public static event EventHandler<LoggingConfigurationChangedEventArgs> ConfigurationChanged
  {
    add => LogManager.factory.ConfigurationChanged += value;
    remove => LogManager.factory.ConfigurationChanged -= value;
  }

  public static event EventHandler<LoggingConfigurationReloadedEventArgs> ConfigurationReloaded
  {
    add => LogManager.factory.ConfigurationReloaded += value;
    remove => LogManager.factory.ConfigurationReloaded -= value;
  }

  public static bool ThrowExceptions
  {
    get => LogManager.factory.ThrowExceptions;
    set => LogManager.factory.ThrowExceptions = value;
  }

  public static bool? ThrowConfigExceptions
  {
    get => LogManager.factory.ThrowConfigExceptions;
    set => LogManager.factory.ThrowConfigExceptions = value;
  }

  public static bool KeepVariablesOnReload
  {
    get => LogManager.factory.KeepVariablesOnReload;
    set => LogManager.factory.KeepVariablesOnReload = value;
  }

  public static bool AutoShutdown
  {
    get => LogManager.factory.AutoShutdown;
    set => LogManager.factory.AutoShutdown = value;
  }

  public static LoggingConfiguration Configuration
  {
    get => LogManager.factory.Configuration;
    set => LogManager.factory.Configuration = value;
  }

  public static ISetupBuilder Setup() => LogManager.LogFactory.Setup();

  public static LogFactory Setup(Action<ISetupBuilder> setupBuilder)
  {
    return LogManager.LogFactory.Setup(setupBuilder);
  }

  public static LogFactory LoadConfiguration(string configFile)
  {
    LogManager.factory.LoadConfiguration(configFile);
    return LogManager.factory;
  }

  public static LogLevel GlobalThreshold
  {
    get => LogManager.factory.GlobalThreshold;
    set => LogManager.factory.GlobalThreshold = value;
  }

  [Obsolete("Use Configuration.DefaultCultureInfo property instead. Marked obsolete before v4.3.11")]
  public static LogManager.GetCultureInfo DefaultCultureInfo
  {
    get
    {
      return (LogManager.GetCultureInfo) (() => LogManager.factory.DefaultCultureInfo ?? CultureInfo.CurrentCulture);
    }
    set
    {
      throw new NotSupportedException("Setting the DefaultCultureInfo delegate is no longer supported. Use the Configuration.DefaultCultureInfo property to change the default CultureInfo.");
    }
  }

  [CLSCompliant(false)]
  [MethodImpl(MethodImplOptions.NoInlining)]
  public static Logger GetCurrentClassLogger()
  {
    return LogManager.factory.GetLogger(StackTraceUsageUtils.GetClassFullName());
  }

  internal static bool IsHiddenAssembly(Assembly assembly)
  {
    return LogManager._hiddenAssemblies != null && LogManager._hiddenAssemblies.Contains(assembly);
  }

  [MethodImpl(MethodImplOptions.NoInlining)]
  public static void AddHiddenAssembly(Assembly assembly)
  {
    lock (LogManager.lockObject)
    {
      if (LogManager._hiddenAssemblies != null && LogManager._hiddenAssemblies.Contains(assembly))
        return;
      LogManager._hiddenAssemblies = (ICollection<Assembly>) new HashSet<Assembly>((IEnumerable<Assembly>) LogManager._hiddenAssemblies ?? Enumerable.Empty<Assembly>())
      {
        assembly
      };
    }
    InternalLogger.Trace<string>("Assembly '{0}' will be hidden in callsite stacktrace", assembly?.FullName);
  }

  [CLSCompliant(false)]
  [MethodImpl(MethodImplOptions.NoInlining)]
  public static Logger GetCurrentClassLogger(Type loggerType)
  {
    return LogManager.factory.GetLogger(StackTraceUsageUtils.GetClassFullName(), loggerType);
  }

  [CLSCompliant(false)]
  public static Logger CreateNullLogger() => LogManager.factory.CreateNullLogger();

  [CLSCompliant(false)]
  public static Logger GetLogger(string name) => LogManager.factory.GetLogger(name);

  [CLSCompliant(false)]
  public static Logger GetLogger(string name, Type loggerType)
  {
    return LogManager.factory.GetLogger(name, loggerType);
  }

  public static void ReconfigExistingLoggers() => LogManager.factory.ReconfigExistingLoggers();

  public static void Flush() => LogManager.factory.Flush();

  public static void Flush(TimeSpan timeout) => LogManager.factory.Flush(timeout);

  public static void Flush(int timeoutMilliseconds)
  {
    LogManager.factory.Flush(timeoutMilliseconds);
  }

  public static void Flush(AsyncContinuation asyncContinuation)
  {
    LogManager.factory.Flush(asyncContinuation);
  }

  public static void Flush(AsyncContinuation asyncContinuation, TimeSpan timeout)
  {
    LogManager.factory.Flush(asyncContinuation, timeout);
  }

  public static void Flush(AsyncContinuation asyncContinuation, int timeoutMilliseconds)
  {
    LogManager.factory.Flush(asyncContinuation, timeoutMilliseconds);
  }

  public static IDisposable DisableLogging() => LogManager.factory.SuspendLogging();

  public static void EnableLogging() => LogManager.factory.ResumeLogging();

  public static bool IsLoggingEnabled() => LogManager.factory.IsLoggingEnabled();

  public static void Shutdown() => LogManager.factory.Shutdown();

  [Obsolete("Marked obsolete before v4.3.11")]
  public delegate CultureInfo GetCultureInfo();
}
