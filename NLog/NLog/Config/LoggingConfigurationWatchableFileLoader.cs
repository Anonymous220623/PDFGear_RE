// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingConfigurationWatchableFileLoader
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using NLog.Internal.Fakeables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

#nullable disable
namespace NLog.Config;

internal class LoggingConfigurationWatchableFileLoader(IAppEnvironment appEnvironment) : 
  LoggingConfigurationFileLoader(appEnvironment)
{
  private const int ReconfigAfterFileChangedTimeout = 1000;
  private readonly object _lockObject = new object();
  private Timer _reloadTimer;
  private MultiFileWatcher _watcher;
  private bool _isDisposing;
  private LogFactory _logFactory;

  public override LoggingConfiguration Load(LogFactory logFactory, string filename = null)
  {
    if (string.IsNullOrEmpty(filename))
    {
      LoggingConfiguration loggingConfiguration = this.TryLoadFromAppConfig();
      if (loggingConfiguration != null)
        return loggingConfiguration;
    }
    return base.Load(logFactory, filename);
  }

  public override void Activated(LogFactory logFactory, LoggingConfiguration config)
  {
    this._logFactory = logFactory;
    this.TryUnwatchConfigFile();
    if (config == null)
      return;
    this.TryWachtingConfigFile(config);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      this._isDisposing = true;
      if (this._watcher != null)
      {
        this._watcher.FileChanged -= new FileSystemEventHandler(this.ConfigFileChanged);
        this._watcher.StopWatching();
      }
      Timer reloadTimer = this._reloadTimer;
      if (reloadTimer != null)
      {
        this._reloadTimer = (Timer) null;
        reloadTimer.WaitForDispose(TimeSpan.Zero);
      }
      this._watcher?.Dispose();
    }
    base.Dispose(disposing);
  }

  private LoggingConfiguration TryLoadFromAppConfig()
  {
    try
    {
      return XmlLoggingConfiguration.AppConfig;
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrown())
        return (LoggingConfiguration) null;
      throw;
    }
  }

  internal void ReloadConfigOnTimer(object state)
  {
    if (this._reloadTimer == null && this._isDisposing)
      return;
    LoggingConfiguration loggingConfiguration1 = (LoggingConfiguration) state;
    InternalLogger.Info("Reloading configuration...");
    lock (this._lockObject)
    {
      if (this._isDisposing)
        return;
      Timer reloadTimer = this._reloadTimer;
      if (reloadTimer != null)
      {
        this._reloadTimer = (Timer) null;
        reloadTimer.WaitForDispose(TimeSpan.Zero);
      }
    }
    lock (this._logFactory._syncRoot)
    {
      LoggingConfiguration loggingConfiguration2;
      try
      {
        if (this._logFactory._config != loggingConfiguration1)
        {
          InternalLogger.Warn("NLog Config changed in between. Not reloading.");
          return;
        }
        loggingConfiguration2 = loggingConfiguration1.ReloadNewConfig();
        if (loggingConfiguration2 == null)
          return;
        if (loggingConfiguration2 == loggingConfiguration1)
          return;
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
        InternalLogger.Warn(ex, "NLog configuration failed to reload");
        this._logFactory?.NotifyConfigurationReloaded(new LoggingConfigurationReloadedEventArgs(false, ex));
        return;
      }
      try
      {
        this.TryUnwatchConfigFile();
        this._logFactory.Configuration = loggingConfiguration2;
        this._logFactory?.NotifyConfigurationReloaded(new LoggingConfigurationReloadedEventArgs(true));
      }
      catch (Exception ex)
      {
        if (ex.MustBeRethrownImmediately())
          throw;
        InternalLogger.Warn(ex, "NLog configuration reloaded, failed to be assigned");
        this._watcher.Watch(loggingConfiguration1.FileNamesToWatch);
        this._logFactory?.NotifyConfigurationReloaded(new LoggingConfigurationReloadedEventArgs(false, ex));
      }
    }
  }

  private void ConfigFileChanged(object sender, EventArgs args)
  {
    InternalLogger.Info<int>("Configuration file change detected! Reloading in {0}ms...", 1000);
    lock (this._lockObject)
    {
      if (this._isDisposing)
        return;
      if (this._reloadTimer == null)
      {
        LoggingConfiguration config = this._logFactory._config;
        if (config == null)
          return;
        this._reloadTimer = new Timer(new TimerCallback(this.ReloadConfigOnTimer), (object) config, 1000, -1);
      }
      else
        this._reloadTimer.Change(1000, -1);
    }
  }

  private void TryWachtingConfigFile(LoggingConfiguration config)
  {
    try
    {
      IEnumerable<string> fileNamesToWatch = config.FileNamesToWatch;
      List<string> list = fileNamesToWatch != null ? fileNamesToWatch.ToList<string>() : (List<string>) null;
      // ISSUE: explicit non-virtual call
      if (list == null || __nonvirtual (list.Count) <= 0)
        return;
      if (this._watcher == null)
      {
        this._watcher = new MultiFileWatcher();
        this._watcher.FileChanged += new FileSystemEventHandler(this.ConfigFileChanged);
      }
      this._watcher.Watch((IEnumerable<string>) list);
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrown())
        throw;
      object[] objArray = new object[1]
      {
        (object) string.Join(",", this._logFactory._config.FileNamesToWatch.ToArray<string>())
      };
      InternalLogger.Warn(ex, "Cannot start file watching: {0}", objArray);
    }
  }

  private void TryUnwatchConfigFile()
  {
    try
    {
      this._watcher?.StopWatching();
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "Cannot stop file watching.");
      if (!ex.MustBeRethrown())
        return;
      throw;
    }
  }
}
