// Decompiled with JetBrains decompiler
// Type: NLog.Internal.Fakeables.AppDomainWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NLog.Internal.Fakeables;

public class AppDomainWrapper : IAppDomain
{
  private readonly AppDomain _currentAppDomain;

  public AppDomainWrapper(AppDomain appDomain)
  {
    this._currentAppDomain = appDomain;
    try
    {
      this.BaseDirectory = AppDomainWrapper.LookupBaseDirectory(appDomain) ?? string.Empty;
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "AppDomain.BaseDirectory Failed");
      this.BaseDirectory = string.Empty;
    }
    try
    {
      this.ConfigurationFile = AppDomainWrapper.LookupConfigurationFile(appDomain);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "AppDomain.SetupInformation.ConfigurationFile Failed");
      this.ConfigurationFile = string.Empty;
    }
    try
    {
      this.PrivateBinPath = (IEnumerable<string>) AppDomainWrapper.LookupPrivateBinPath(appDomain);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "AppDomain.SetupInformation.PrivateBinPath Failed");
      this.PrivateBinPath = (IEnumerable<string>) ArrayHelper.Empty<string>();
    }
    this.FriendlyName = appDomain.FriendlyName;
    this.Id = appDomain.Id;
  }

  private static string LookupBaseDirectory(AppDomain appDomain) => appDomain.BaseDirectory;

  private static string LookupConfigurationFile(AppDomain appDomain)
  {
    return appDomain.SetupInformation.ConfigurationFile;
  }

  private static string[] LookupPrivateBinPath(AppDomain appDomain)
  {
    string privateBinPath = appDomain.SetupInformation.PrivateBinPath;
    return !string.IsNullOrEmpty(privateBinPath) ? privateBinPath.SplitAndTrimTokens(';') : ArrayHelper.Empty<string>();
  }

  public static AppDomainWrapper CurrentDomain => new AppDomainWrapper(AppDomain.CurrentDomain);

  public string BaseDirectory { get; private set; }

  public string ConfigurationFile { get; private set; }

  public IEnumerable<string> PrivateBinPath { get; private set; }

  public string FriendlyName { get; private set; }

  public int Id { get; private set; }

  public IEnumerable<Assembly> GetAssemblies()
  {
    return this._currentAppDomain != null ? (IEnumerable<Assembly>) this._currentAppDomain.GetAssemblies() : (IEnumerable<Assembly>) ArrayHelper.Empty<Assembly>();
  }

  public event EventHandler<EventArgs> ProcessExit
  {
    add
    {
      if (this.processExitEvent == null && this._currentAppDomain != null)
        this._currentAppDomain.ProcessExit += new EventHandler(this.OnProcessExit);
      this.processExitEvent += value;
    }
    remove
    {
      this.processExitEvent -= value;
      if (this.processExitEvent != null || this._currentAppDomain == null)
        return;
      this._currentAppDomain.ProcessExit -= new EventHandler(this.OnProcessExit);
    }
  }

  private event EventHandler<EventArgs> processExitEvent;

  public event EventHandler<EventArgs> DomainUnload
  {
    add
    {
      if (this.domainUnloadEvent == null && this._currentAppDomain != null)
        this._currentAppDomain.DomainUnload += new EventHandler(this.OnDomainUnload);
      this.domainUnloadEvent += value;
    }
    remove
    {
      this.domainUnloadEvent -= value;
      if (this.domainUnloadEvent != null || this._currentAppDomain == null)
        return;
      this._currentAppDomain.DomainUnload -= new EventHandler(this.OnDomainUnload);
    }
  }

  private event EventHandler<EventArgs> domainUnloadEvent;

  private void OnDomainUnload(object sender, EventArgs e)
  {
    EventHandler<EventArgs> domainUnloadEvent = this.domainUnloadEvent;
    if (domainUnloadEvent == null)
      return;
    domainUnloadEvent(sender, e);
  }

  private void OnProcessExit(object sender, EventArgs eventArgs)
  {
    EventHandler<EventArgs> processExitEvent = this.processExitEvent;
    if (processExitEvent == null)
      return;
    processExitEvent(sender, eventArgs);
  }
}
