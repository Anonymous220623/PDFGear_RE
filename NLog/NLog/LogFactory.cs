// Decompiled with JetBrains decompiler
// Type: NLog.LogFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Common;
using NLog.Config;
using NLog.Filters;
using NLog.Internal;
using NLog.Internal.Fakeables;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog;

public class LogFactory : IDisposable
{
  private static readonly TimeSpan DefaultFlushTimeout = TimeSpan.FromSeconds(15.0);
  private static IAppDomain currentAppDomain;
  private static AppEnvironmentWrapper defaultAppEnvironment;
  internal readonly object _syncRoot = new object();
  internal LoggingConfiguration _config;
  private IAppEnvironment _currentAppEnvironment;
  private LogLevel _globalThreshold = LogLevel.MinLevel;
  private bool _configLoaded;
  private int _logsEnabled;
  private readonly LogFactory.LoggerCache _loggerCache = new LogFactory.LoggerCache();
  private List<string> _candidateConfigFilePaths;
  private readonly ILoggingConfigurationLoader _configLoader;
  private bool _autoShutdown = true;
  private bool _isDisposing;

  public event EventHandler<LoggingConfigurationChangedEventArgs> ConfigurationChanged;

  public event EventHandler<LoggingConfigurationReloadedEventArgs> ConfigurationReloaded;

  private static event EventHandler<EventArgs> LoggerShutdown;

  static LogFactory() => LogFactory.RegisterEvents(LogFactory.CurrentAppDomain);

  public LogFactory()
    : this((ILoggingConfigurationLoader) new LoggingConfigurationWatchableFileLoader(LogFactory.DefaultAppEnvironment))
  {
  }

  public LogFactory(LoggingConfiguration config)
    : this()
  {
    this.Configuration = config;
  }

  internal LogFactory(ILoggingConfigurationLoader configLoader, IAppEnvironment appEnvironment = null)
  {
    this._configLoader = configLoader;
    this._currentAppEnvironment = appEnvironment;
    LogFactory.LoggerShutdown += new EventHandler<EventArgs>(this.OnStopLogging);
  }

  public static IAppDomain CurrentAppDomain
  {
    get => LogFactory.currentAppDomain ?? LogFactory.DefaultAppEnvironment.AppDomain;
    set
    {
      LogFactory.UnregisterEvents(LogFactory.currentAppDomain);
      LogFactory.UnregisterEvents(value);
      LogFactory.RegisterEvents(value);
      LogFactory.currentAppDomain = value;
      if (value == null || LogFactory.defaultAppEnvironment == null)
        return;
      LogFactory.defaultAppEnvironment.AppDomain = value;
    }
  }

  internal static IAppEnvironment DefaultAppEnvironment
  {
    get
    {
      AppEnvironmentWrapper defaultAppEnvironment = LogFactory.defaultAppEnvironment;
      if (defaultAppEnvironment != null)
        return (IAppEnvironment) defaultAppEnvironment;
      IAppDomain appDomain = LogFactory.currentAppDomain ?? (LogFactory.currentAppDomain = (IAppDomain) new AppDomainWrapper(AppDomain.CurrentDomain));
      return (IAppEnvironment) (LogFactory.defaultAppEnvironment = new AppEnvironmentWrapper(appDomain));
    }
  }

  internal IAppEnvironment CurrentAppEnvironment
  {
    get => this._currentAppEnvironment ?? LogFactory.DefaultAppEnvironment;
    set => this._currentAppEnvironment = value;
  }

  public bool ThrowExceptions { get; set; }

  public bool? ThrowConfigExceptions { get; set; }

  public bool KeepVariablesOnReload { get; set; }

  public bool AutoShutdown
  {
    get => this._autoShutdown;
    set
    {
      if (value == this._autoShutdown)
        return;
      this._autoShutdown = value;
      LogFactory.LoggerShutdown -= new EventHandler<EventArgs>(this.OnStopLogging);
      if (!value)
        return;
      LogFactory.LoggerShutdown += new EventHandler<EventArgs>(this.OnStopLogging);
    }
  }

  public LoggingConfiguration Configuration
  {
    get
    {
      if (this._configLoaded)
        return this._config;
      lock (this._syncRoot)
      {
        if (this._configLoaded || this._isDisposing)
          return this._config;
        LoggingConfiguration loggingConfiguration = this._configLoader.Load(this);
        if (loggingConfiguration != null)
        {
          try
          {
            this._config = loggingConfiguration;
            this._configLoader.Activated(this, this._config);
            this._config.Dump();
            this.ReconfigExistingLoggers();
            LogFactory.LogConfigurationInitialized();
          }
          finally
          {
            this._configLoaded = true;
          }
        }
        return this._config;
      }
    }
    set
    {
      lock (this._syncRoot)
      {
        LoggingConfiguration config = this._config;
        if (config != null)
        {
          InternalLogger.Info("Closing old configuration.");
          this.Flush();
          config.Close();
        }
        this._config = value;
        if (this._config == null)
        {
          this._configLoaded = false;
          this._configLoader.Activated(this, this._config);
        }
        else
        {
          try
          {
            this._configLoader.Activated(this, this._config);
            this._config.Dump();
            this.ReconfigExistingLoggers();
          }
          finally
          {
            this._configLoaded = true;
          }
        }
        this.OnConfigurationChanged(new LoggingConfigurationChangedEventArgs(value, config));
      }
    }
  }

  public LogLevel GlobalThreshold
  {
    get => this._globalThreshold;
    set
    {
      lock (this._syncRoot)
      {
        this._globalThreshold = value;
        this.ReconfigExistingLoggers();
      }
    }
  }

  [CanBeNull]
  public CultureInfo DefaultCultureInfo => this.Configuration?.DefaultCultureInfo;

  internal static void LogConfigurationInitialized()
  {
    InternalLogger.Info("Configuration initialized.");
    try
    {
      InternalLogger.LogAssemblyVersion(typeof (ILogger).GetAssembly());
    }
    catch (SecurityException ex)
    {
      InternalLogger.Debug((Exception) ex, "Not running in full trust");
    }
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public ISetupBuilder Setup() => (ISetupBuilder) new SetupBuilder(this);

  public LogFactory Setup(Action<ISetupBuilder> setupBuilder)
  {
    if (setupBuilder == null)
      throw new ArgumentNullException(nameof (setupBuilder));
    setupBuilder((ISetupBuilder) new SetupBuilder(this));
    return this;
  }

  public Logger CreateNullLogger() => (Logger) new NullLogger(this);

  [MethodImpl(MethodImplOptions.NoInlining)]
  public Logger GetCurrentClassLogger()
  {
    return this.GetLogger(StackTraceUsageUtils.GetClassFullName(new StackFrame(1, false)));
  }

  [MethodImpl(MethodImplOptions.NoInlining)]
  public T GetCurrentClassLogger<T>() where T : Logger
  {
    return (T) this.GetLogger(StackTraceUsageUtils.GetClassFullName(new StackFrame(1, false)), typeof (T));
  }

  [MethodImpl(MethodImplOptions.NoInlining)]
  public Logger GetCurrentClassLogger(Type loggerType)
  {
    return this.GetLoggerThreadSafe(StackTraceUsageUtils.GetClassFullName(new StackFrame(1, false)), loggerType);
  }

  public Logger GetLogger(string name) => this.GetLoggerThreadSafe(name, Logger.DefaultLoggerType);

  public T GetLogger<T>(string name) where T : Logger
  {
    return (T) this.GetLoggerThreadSafe(name, typeof (T));
  }

  public Logger GetLogger(string name, Type loggerType)
  {
    return this.GetLoggerThreadSafe(name, loggerType);
  }

  public void ReconfigExistingLoggers()
  {
    List<Logger> loggers;
    lock (this._syncRoot)
    {
      this._config?.InitializeAll();
      loggers = this._loggerCache.GetLoggers();
    }
    foreach (Logger logger in loggers)
      logger.SetConfiguration(this.GetConfigurationForLogger(logger.Name, this._config));
  }

  public void Flush() => this.Flush(LogFactory.DefaultFlushTimeout);

  public void Flush(TimeSpan timeout) => this.FlushInternal(timeout, (AsyncContinuation) null);

  public void Flush(int timeoutMilliseconds)
  {
    this.Flush(TimeSpan.FromMilliseconds((double) timeoutMilliseconds));
  }

  public void Flush(AsyncContinuation asyncContinuation)
  {
    this.Flush(asyncContinuation, TimeSpan.MaxValue);
  }

  public void Flush(AsyncContinuation asyncContinuation, int timeoutMilliseconds)
  {
    this.Flush(asyncContinuation, TimeSpan.FromMilliseconds((double) timeoutMilliseconds));
  }

  public void Flush(AsyncContinuation asyncContinuation, TimeSpan timeout)
  {
    this.FlushInternal(timeout, asyncContinuation ?? (AsyncContinuation) (ex => { }));
  }

  private void FlushInternal(TimeSpan timeout, AsyncContinuation asyncContinuation)
  {
    try
    {
      InternalLogger.Debug<double>("LogFactory Flush with timeout={0} secs", timeout.TotalSeconds);
      LoggingConfiguration config;
      lock (this._syncRoot)
        config = this._config;
      if (config != null)
      {
        if (asyncContinuation != null)
          LogFactory.FlushAllTargetsAsync(config, asyncContinuation, new TimeSpan?(timeout));
        else if (!LogFactory.FlushAllTargetsSync(config, timeout, this.ThrowExceptions))
          ;
      }
      else
      {
        if (asyncContinuation == null)
          return;
        asyncContinuation((Exception) null);
      }
    }
    catch (Exception ex)
    {
      if (this.ThrowExceptions)
        throw;
      InternalLogger.Error(ex, "Error with flush.");
      if (asyncContinuation == null)
        return;
      asyncContinuation(ex);
    }
  }

  private static AsyncContinuation FlushAllTargetsAsync(
    LoggingConfiguration loggingConfiguration,
    AsyncContinuation asyncContinuation,
    TimeSpan? asyncTimeout)
  {
    List<Target> allTargetsToFlush = loggingConfiguration.GetAllTargetsToFlush();
    HashSet<Target> pendingTargets = new HashSet<Target>((IEnumerable<Target>) allTargetsToFlush, (IEqualityComparer<Target>) SingleItemOptimizedHashSet<Target>.ReferenceEqualityComparer.Default);
    AsynchronousAction<Target> action = (AsynchronousAction<Target>) ((target, cont) => target.Flush((AsyncContinuation) (ex =>
    {
      if (ex != null)
        InternalLogger.Warn(ex, "Flush failed for target {0}(Name={1})", (object) target.GetType(), (object) target.Name);
      lock (pendingTargets)
        pendingTargets.Remove(target);
      cont(ex);
    })));
    AsyncContinuation asyncContinuation1 = (AsyncContinuation) (ex =>
    {
      lock (pendingTargets)
      {
        foreach (Target target in pendingTargets)
          InternalLogger.Debug<Type, string>("Flush timeout for target {0}(Name={1})", target.GetType(), target.Name);
        pendingTargets.Clear();
      }
      if (ex != null)
        InternalLogger.Warn(ex, "Flush completed with errors");
      else
        InternalLogger.Debug("Flush completed");
      asyncContinuation(ex);
    });
    AsyncContinuation asyncContinuation2 = !asyncTimeout.HasValue ? AsyncHelpers.PreventMultipleCalls(asyncContinuation1) : AsyncHelpers.WithTimeout(asyncContinuation1, asyncTimeout.Value);
    InternalLogger.Trace<int>("Flushing all {0} targets...", allTargetsToFlush.Count);
    AsyncHelpers.ForEachItemInParallel<Target>((IEnumerable<Target>) allTargetsToFlush, asyncContinuation2, action);
    return asyncContinuation2;
  }

  private static bool FlushAllTargetsSync(
    LoggingConfiguration oldConfig,
    TimeSpan timeout,
    bool throwExceptions)
  {
    Exception lastException = (Exception) null;
    try
    {
      ManualResetEvent flushCompletedEvent = new ManualResetEvent(false);
      AsyncContinuation asyncContinuation = LogFactory.FlushAllTargetsAsync(oldConfig, (AsyncContinuation) (ex =>
      {
        if (ex != null)
          lastException = ex;
        flushCompletedEvent.Set();
      }), new TimeSpan?());
      if (!flushCompletedEvent.WaitOne(timeout))
        asyncContinuation((Exception) new TimeoutException($"Timeout when flushing all targets, after waiting {timeout.TotalSeconds} seconds."));
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      if (throwExceptions)
        throw new NLogRuntimeException("Asynchronous exception has occurred.", ex);
      InternalLogger.Error(ex, "Error with flush.");
      return false;
    }
    if (lastException == null)
      return true;
    if (throwExceptions)
      throw new NLogRuntimeException("Asynchronous exception has occurred.", lastException);
    return false;
  }

  [Obsolete("Use SuspendLogging() instead. Marked obsolete on NLog 4.0")]
  public IDisposable DisableLogging() => this.SuspendLogging();

  [Obsolete("Use ResumeLogging() instead. Marked obsolete on NLog 4.0")]
  public void EnableLogging() => this.ResumeLogging();

  public IDisposable SuspendLogging()
  {
    lock (this._syncRoot)
    {
      --this._logsEnabled;
      if (this._logsEnabled == -1)
        this.ReconfigExistingLoggers();
    }
    return (IDisposable) new LogFactory.LogEnabler(this);
  }

  public void ResumeLogging()
  {
    lock (this._syncRoot)
    {
      ++this._logsEnabled;
      if (this._logsEnabled != 0)
        return;
      this.ReconfigExistingLoggers();
    }
  }

  public bool IsLoggingEnabled() => this._logsEnabled >= 0;

  protected virtual void OnConfigurationChanged(LoggingConfigurationChangedEventArgs e)
  {
    EventHandler<LoggingConfigurationChangedEventArgs> configurationChanged = this.ConfigurationChanged;
    if (configurationChanged == null)
      return;
    configurationChanged((object) this, e);
  }

  protected virtual void OnConfigurationReloaded(LoggingConfigurationReloadedEventArgs e)
  {
    EventHandler<LoggingConfigurationReloadedEventArgs> configurationReloaded = this.ConfigurationReloaded;
    if (configurationReloaded == null)
      return;
    configurationReloaded((object) this, e);
  }

  internal void NotifyConfigurationReloaded(LoggingConfigurationReloadedEventArgs eventArgs)
  {
    this.OnConfigurationReloaded(eventArgs);
  }

  private bool GetTargetsByLevelForLogger(
    string name,
    List<LoggingRule> loggingRules,
    TargetWithFilterChain[] targetsByLevel,
    TargetWithFilterChain[] lastTargetsByLevel,
    bool[] suppressedLevels)
  {
    bool byLevelForLogger = false;
    foreach (LoggingRule loggingRule in loggingRules)
    {
      if (loggingRule.NameMatches(name))
      {
        for (int ordinal = 0; ordinal <= LogLevel.MaxLevel.Ordinal; ++ordinal)
        {
          if (ordinal >= this.GlobalThreshold.Ordinal && !suppressedLevels[ordinal] && loggingRule.IsLoggingEnabledForLevel(LogLevel.FromOrdinal(ordinal)))
          {
            if (loggingRule.Final)
              suppressedLevels[ordinal] = true;
            foreach (Target target in loggingRule.GetTargetsThreadSafe())
            {
              byLevelForLogger = true;
              IList<Filter> filters = loggingRule.Filters;
              int defaultFilterResult = (int) loggingRule.DefaultFilterResult;
              TargetWithFilterChain targetWithFilterChain = new TargetWithFilterChain(target, filters, (FilterResult) defaultFilterResult);
              if (lastTargetsByLevel[ordinal] != null)
                lastTargetsByLevel[ordinal].NextInChain = targetWithFilterChain;
              else
                targetsByLevel[ordinal] = targetWithFilterChain;
              lastTargetsByLevel[ordinal] = targetWithFilterChain;
            }
          }
        }
        if (loggingRule.ChildRules.Count != 0)
          byLevelForLogger = this.GetTargetsByLevelForLogger(name, loggingRule.GetChildRulesThreadSafe(), targetsByLevel, lastTargetsByLevel, suppressedLevels) | byLevelForLogger;
      }
    }
    for (int index = 0; index <= LogLevel.MaxLevel.Ordinal; ++index)
    {
      TargetWithFilterChain targetWithFilterChain = targetsByLevel[index];
      if (targetWithFilterChain != null)
      {
        int num = (int) targetWithFilterChain.PrecalculateStackTraceUsage();
      }
    }
    return byLevelForLogger;
  }

  internal LoggerConfiguration GetConfigurationForLogger(
    string name,
    LoggingConfiguration configuration)
  {
    TargetWithFilterChain[] targetsByLevel = new TargetWithFilterChain[LogLevel.MaxLevel.Ordinal + 1];
    TargetWithFilterChain[] lastTargetsByLevel = new TargetWithFilterChain[LogLevel.MaxLevel.Ordinal + 1];
    bool[] suppressedLevels = new bool[LogLevel.MaxLevel.Ordinal + 1];
    bool flag = false;
    if (configuration != null && this.IsLoggingEnabled())
    {
      List<LoggingRule> loggingRulesThreadSafe = configuration.GetLoggingRulesThreadSafe();
      flag = this.GetTargetsByLevelForLogger(name, loggingRulesThreadSafe, targetsByLevel, lastTargetsByLevel, suppressedLevels);
    }
    if (InternalLogger.IsDebugEnabled)
    {
      if (flag)
      {
        InternalLogger.Debug<string>("Targets for {0} by level:", name);
        for (int ordinal = 0; ordinal <= LogLevel.MaxLevel.Ordinal; ++ordinal)
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0} =>", new object[1]
          {
            (object) LogLevel.FromOrdinal(ordinal)
          });
          for (TargetWithFilterChain nextInChain = targetsByLevel[ordinal]; nextInChain != null; nextInChain = nextInChain.NextInChain)
          {
            stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, " {0}", new object[1]
            {
              (object) nextInChain.Target.Name
            });
            if (nextInChain.FilterChain.Count > 0)
              stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, " ({0} filters)", new object[1]
              {
                (object) nextInChain.FilterChain.Count
              });
          }
          InternalLogger.Debug(stringBuilder.ToString());
        }
      }
      else
        InternalLogger.Debug<string>("Targets not configured for logger: {0}", name);
    }
    bool exceptionLoggingOldStyle = configuration != null && configuration.ExceptionLoggingOldStyle;
    return new LoggerConfiguration(targetsByLevel, exceptionLoggingOldStyle);
  }

  private void Close(TimeSpan flushTimeout)
  {
    if (this._isDisposing)
      return;
    this._isDisposing = true;
    LogFactory.LoggerShutdown -= new EventHandler<EventArgs>(this.OnStopLogging);
    this.ConfigurationReloaded = (EventHandler<LoggingConfigurationReloadedEventArgs>) null;
    if (Monitor.TryEnter(this._syncRoot, 500))
    {
      try
      {
        this._configLoader.Dispose();
        LoggingConfiguration config = this._config;
        if (this._configLoaded)
        {
          if (config != null)
            this.CloseOldConfig(flushTimeout, config);
        }
      }
      finally
      {
        Monitor.Exit(this._syncRoot);
      }
    }
    this.ConfigurationChanged = (EventHandler<LoggingConfigurationChangedEventArgs>) null;
  }

  private void CloseOldConfig(TimeSpan flushTimeout, LoggingConfiguration oldConfig)
  {
    try
    {
      bool flag = true;
      if (flushTimeout != TimeSpan.Zero && !PlatformDetector.IsMono && !PlatformDetector.IsUnix)
        flag = LogFactory.FlushAllTargetsSync(oldConfig, flushTimeout, false);
      this._config = (LoggingConfiguration) null;
      this.ReconfigExistingLoggers();
      if (!flag)
      {
        InternalLogger.Warn("Target flush timeout. One or more targets did not complete flush operation, skipping target close.");
      }
      else
      {
        oldConfig.Close();
        this.OnConfigurationChanged(new LoggingConfigurationChangedEventArgs((LoggingConfiguration) null, oldConfig));
      }
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "Error with close.");
    }
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.Close(TimeSpan.Zero);
  }

  public void Shutdown()
  {
    InternalLogger.Info("Shutdown() called. Logger closing...");
    if (!this._isDisposing && this._configLoaded)
    {
      lock (this._syncRoot)
      {
        if (this._isDisposing || !this._configLoaded)
          return;
        this.Configuration = (LoggingConfiguration) null;
        this._configLoaded = true;
        this.ReconfigExistingLoggers();
      }
    }
    InternalLogger.Info("Logger has been closed down.");
  }

  public IEnumerable<string> GetCandidateConfigFilePaths()
  {
    return this._candidateConfigFilePaths != null ? (IEnumerable<string>) this._candidateConfigFilePaths.AsReadOnly() : this._configLoader.GetDefaultCandidateConfigFilePaths();
  }

  internal IEnumerable<string> GetCandidateConfigFilePaths(string filename)
  {
    return this._candidateConfigFilePaths != null ? this.GetCandidateConfigFilePaths() : this._configLoader.GetDefaultCandidateConfigFilePaths(string.IsNullOrEmpty(filename) ? (string) null : filename);
  }

  public void SetCandidateConfigFilePaths(IEnumerable<string> filePaths)
  {
    this._candidateConfigFilePaths = new List<string>();
    if (filePaths == null)
      return;
    this._candidateConfigFilePaths.AddRange(filePaths);
  }

  public void ResetCandidateConfigFilePath()
  {
    this._candidateConfigFilePaths = (List<string>) null;
  }

  private Logger GetLoggerThreadSafe(string name, Type loggerType)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name), "Name of logger cannot be null");
    LogFactory.LoggerCacheKey cacheKey;
    ref LogFactory.LoggerCacheKey local = ref cacheKey;
    string name1 = name;
    Type concreteType = loggerType;
    if ((object) concreteType == null)
      concreteType = typeof (Logger);
    local = new LogFactory.LoggerCacheKey(name1, concreteType);
    lock (this._syncRoot)
    {
      Logger loggerThreadSafe = this._loggerCache.Retrieve(cacheKey);
      if (loggerThreadSafe != null)
        return loggerThreadSafe;
      Logger logger = this.CreateNewLogger(cacheKey.ConcreteType);
      if (logger == null)
      {
        cacheKey = new LogFactory.LoggerCacheKey(cacheKey.Name, typeof (Logger));
        logger = new Logger();
      }
      logger.Initialize(name, this.GetConfigurationForLogger(name, this.Configuration), this);
      this._loggerCache.InsertOrUpdate(cacheKey, logger);
      return logger;
    }
  }

  internal Logger CreateNewLogger(Type loggerType)
  {
    Logger newLogger;
    if (loggerType != (Type) null)
    {
      if (loggerType != typeof (Logger))
      {
        try
        {
          newLogger = this.CreateCustomLoggerType(loggerType);
          goto label_7;
        }
        catch (Exception ex)
        {
          InternalLogger.Error(ex, "GetLogger / GetCurrentClassLogger. Cannot create instance of type '{0}'. It should have an default contructor.", (object) loggerType);
          if (ex.MustBeRethrown())
            throw;
          newLogger = (Logger) null;
          goto label_7;
        }
      }
    }
    newLogger = new Logger();
label_7:
    return newLogger;
  }

  private Logger CreateCustomLoggerType(Type customLoggerType)
  {
    if (customLoggerType.IsStaticClass())
    {
      string message = $"GetLogger / GetCurrentClassLogger is '{customLoggerType}' as loggerType is static class and should instead inherit from Logger";
      InternalLogger.Error(message);
      if (this.ThrowExceptions)
        throw new NLogRuntimeException(message);
      return (Logger) null;
    }
    if (FactoryHelper.CreateInstance(customLoggerType) is Logger instance)
      return instance;
    string message1 = $"GetLogger / GetCurrentClassLogger got '{customLoggerType}' as loggerType doesn't inherit from Logger";
    InternalLogger.Error(message1);
    if (this.ThrowExceptions)
      throw new NLogRuntimeException(message1);
    return (Logger) null;
  }

  public LogFactory LoadConfiguration(string configFile)
  {
    return this.LoadConfiguration(configFile, false);
  }

  internal LogFactory LoadConfiguration(string configFile, bool optional)
  {
    string fileName = string.IsNullOrEmpty(configFile) ? "NLog.config" : configFile;
    if (optional && string.Equals(fileName.Trim(), "NLog.config", StringComparison.OrdinalIgnoreCase) && this._config != null)
      return this;
    LoggingConfiguration loggingConfiguration = this._configLoader.Load(this, configFile);
    if (loggingConfiguration == null)
    {
      if (!optional)
        throw new FileNotFoundException(this.CreateFileNotFoundMessage(configFile), fileName);
      return this;
    }
    this.Configuration = loggingConfiguration;
    return this;
  }

  private string CreateFileNotFoundMessage(string configFile)
  {
    StringBuilder stringBuilder = new StringBuilder("Failed to load NLog LoggingConfiguration.");
    try
    {
      HashSet<string> stringSet = new HashSet<string>(this._configLoader.GetDefaultCandidateConfigFilePaths(configFile));
      stringBuilder.AppendLine(" Searched the following locations:");
      foreach (string str in stringSet)
      {
        stringBuilder.Append("- ");
        stringBuilder.AppendLine(str);
      }
    }
    catch (Exception ex)
    {
      InternalLogger.Debug<Exception>("Failed to GetDefaultCandidateConfigFilePaths in CreateFileNotFoundMessage: {0}", ex);
    }
    return stringBuilder.ToString();
  }

  internal void ResetLoggerCache() => this._loggerCache.Reset();

  private static void RegisterEvents(IAppDomain appDomain)
  {
    if (appDomain == null)
      return;
    try
    {
      appDomain.ProcessExit += new EventHandler<EventArgs>(LogFactory.OnLoggerShutdown);
      appDomain.DomainUnload += new EventHandler<EventArgs>(LogFactory.OnLoggerShutdown);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "Error setting up termination events.");
      if (!ex.MustBeRethrown())
        return;
      throw;
    }
  }

  private static void UnregisterEvents(IAppDomain appDomain)
  {
    if (appDomain == null)
      return;
    appDomain.DomainUnload -= new EventHandler<EventArgs>(LogFactory.OnLoggerShutdown);
    appDomain.ProcessExit -= new EventHandler<EventArgs>(LogFactory.OnLoggerShutdown);
  }

  private static void OnLoggerShutdown(object sender, EventArgs args)
  {
    try
    {
      EventHandler<EventArgs> loggerShutdown = LogFactory.LoggerShutdown;
      if (loggerShutdown == null)
        return;
      loggerShutdown(sender, args);
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      InternalLogger.Error(ex, "LogFactory failed to shut down properly.");
    }
    finally
    {
      LogFactory.LoggerShutdown = (EventHandler<EventArgs>) null;
      if (LogFactory.currentAppDomain != null)
        LogFactory.CurrentAppDomain = (IAppDomain) null;
    }
  }

  private void OnStopLogging(object sender, EventArgs args)
  {
    try
    {
      InternalLogger.Info("AppDomain Shutting down. Logger closing...");
      this.Close(TimeSpan.FromMilliseconds(1500.0));
      InternalLogger.Info("Logger has been shut down.");
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      InternalLogger.Error(ex, "Logger failed to shut down properly.");
    }
  }

  private struct LoggerCacheKey(string name, Type concreteType) : 
    IEquatable<LogFactory.LoggerCacheKey>
  {
    public readonly string Name = name;
    public readonly Type ConcreteType = concreteType;

    public override int GetHashCode() => this.ConcreteType.GetHashCode() ^ this.Name.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is LogFactory.LoggerCacheKey other && this.Equals(other);
    }

    public bool Equals(LogFactory.LoggerCacheKey other)
    {
      return this.ConcreteType == other.ConcreteType && string.Equals(other.Name, this.Name, StringComparison.Ordinal);
    }
  }

  private class LoggerCache
  {
    private readonly Dictionary<LogFactory.LoggerCacheKey, WeakReference> _loggerCache = new Dictionary<LogFactory.LoggerCacheKey, WeakReference>();

    public void InsertOrUpdate(LogFactory.LoggerCacheKey cacheKey, Logger logger)
    {
      this._loggerCache[cacheKey] = new WeakReference((object) logger);
    }

    public Logger Retrieve(LogFactory.LoggerCacheKey cacheKey)
    {
      WeakReference weakReference;
      return this._loggerCache.TryGetValue(cacheKey, out weakReference) ? weakReference.Target as Logger : (Logger) null;
    }

    public List<Logger> GetLoggers()
    {
      List<Logger> loggers = new List<Logger>(this._loggerCache.Count);
      foreach (WeakReference weakReference in this._loggerCache.Values)
      {
        if (weakReference.Target is Logger target)
          loggers.Add(target);
      }
      return loggers;
    }

    public void Reset() => this._loggerCache.Clear();
  }

  private class LogEnabler : IDisposable
  {
    private readonly LogFactory _factory;

    public LogEnabler(LogFactory factory) => this._factory = factory;

    void IDisposable.Dispose() => this._factory.ResumeLogging();
  }
}
