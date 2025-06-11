// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingConfigurationFileLoader
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using NLog.Internal.Fakeables;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Xml;

#nullable disable
namespace NLog.Config;

internal class LoggingConfigurationFileLoader : ILoggingConfigurationLoader, IDisposable
{
  private readonly IAppEnvironment _appEnvironment;

  public LoggingConfigurationFileLoader(IAppEnvironment appEnvironment)
  {
    this._appEnvironment = appEnvironment;
  }

  public virtual LoggingConfiguration Load(LogFactory logFactory, string filename = null)
  {
    if (string.IsNullOrEmpty(filename) || FilePathLayout.DetectFilePathKind(filename) == FilePathKind.Relative)
      return this.TryLoadFromFilePaths(logFactory, filename);
    LoggingConfiguration config;
    return this.TryLoadLoggingConfiguration(logFactory, filename, out config) ? config : (LoggingConfiguration) null;
  }

  public virtual void Activated(LogFactory logFactory, LoggingConfiguration config)
  {
  }

  private LoggingConfiguration TryLoadFromFilePaths(LogFactory logFactory, string filename)
  {
    foreach (string candidateConfigFilePath in logFactory.GetCandidateConfigFilePaths(filename))
    {
      LoggingConfiguration config;
      if (this.TryLoadLoggingConfiguration(logFactory, candidateConfigFilePath, out config))
        return config;
    }
    return (LoggingConfiguration) null;
  }

  private bool TryLoadLoggingConfiguration(
    LogFactory logFactory,
    string configFile,
    out LoggingConfiguration config)
  {
    try
    {
      if (this._appEnvironment.FileExists(configFile))
      {
        config = this.LoadXmlLoggingConfigurationFile(logFactory, configFile);
        return true;
      }
    }
    catch (IOException ex)
    {
      object[] objArray = new object[1]
      {
        (object) configFile
      };
      InternalLogger.Warn((Exception) ex, "Skipping invalid config file location: {0}", objArray);
    }
    catch (UnauthorizedAccessException ex)
    {
      object[] objArray = new object[1]
      {
        (object) configFile
      };
      InternalLogger.Warn((Exception) ex, "Skipping inaccessible config file location: {0}", objArray);
    }
    catch (SecurityException ex)
    {
      object[] objArray = new object[1]
      {
        (object) configFile
      };
      InternalLogger.Warn((Exception) ex, "Skipping inaccessible config file location: {0}", objArray);
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "Failed loading from config file location: {0}", (object) configFile);
      if (((int) logFactory.ThrowConfigExceptions ?? (logFactory.ThrowExceptions ? 1 : 0)) != 0)
        throw;
      if (ex.MustBeRethrown())
        throw;
    }
    config = (LoggingConfiguration) null;
    return false;
  }

  private LoggingConfiguration LoadXmlLoggingConfigurationFile(
    LogFactory logFactory,
    string configFile)
  {
    InternalLogger.Debug<string>("Loading config from {0}", configFile);
    using (XmlReader xmlReader = this._appEnvironment.LoadXmlFile(configFile))
      return this.LoadXmlLoggingConfiguration(xmlReader, configFile, logFactory);
  }

  private LoggingConfiguration LoadXmlLoggingConfiguration(
    XmlReader xmlReader,
    string configFile,
    LogFactory logFactory)
  {
    try
    {
      XmlLoggingConfiguration loggingConfiguration = new XmlLoggingConfiguration(xmlReader, configFile, logFactory);
      bool? initializeSucceeded = loggingConfiguration.InitializeSucceeded;
      bool flag = true;
      if (!(initializeSucceeded.GetValueOrDefault() == flag & initializeSucceeded.HasValue))
      {
        bool autoReload;
        if (this.ThrowXmlConfigExceptions(configFile, xmlReader, logFactory, out autoReload))
        {
          using (XmlReader reader = this._appEnvironment.LoadXmlFile(configFile))
            loggingConfiguration = new XmlLoggingConfiguration(reader, configFile, logFactory);
        }
        else if (autoReload && !loggingConfiguration.AutoReload)
          return LoggingConfigurationFileLoader.CreateEmptyDefaultConfig(configFile, logFactory, autoReload);
      }
      return (LoggingConfiguration) loggingConfiguration;
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately() || ex.MustBeRethrown() || ((int) logFactory.ThrowConfigExceptions ?? (logFactory.ThrowExceptions ? 1 : 0)) != 0)
        throw;
      bool autoReload;
      if (!this.ThrowXmlConfigExceptions(configFile, xmlReader, logFactory, out autoReload))
        return LoggingConfigurationFileLoader.CreateEmptyDefaultConfig(configFile, logFactory, autoReload);
      throw;
    }
  }

  private static LoggingConfiguration CreateEmptyDefaultConfig(
    string configFile,
    LogFactory logFactory,
    bool autoReload)
  {
    return (LoggingConfiguration) new XmlLoggingConfiguration($"<nlog autoReload='{autoReload}'></nlog>", configFile, logFactory);
  }

  private bool ThrowXmlConfigExceptions(
    string configFile,
    XmlReader xmlReader,
    LogFactory logFactory,
    out bool autoReload)
  {
    autoReload = false;
    try
    {
      if (string.IsNullOrEmpty(configFile))
        return false;
      string fileContent = File.ReadAllText(configFile);
      if (xmlReader.ReadState == ReadState.Error)
      {
        if (LoggingConfigurationFileLoader.ScanForBooleanParameter(fileContent, "throwExceptions", true))
        {
          logFactory.ThrowExceptions = true;
          return true;
        }
        if (LoggingConfigurationFileLoader.ScanForBooleanParameter(fileContent, "throwConfigExceptions", true))
        {
          logFactory.ThrowConfigExceptions = new bool?(true);
          return true;
        }
      }
      if (LoggingConfigurationFileLoader.ScanForBooleanParameter(fileContent, nameof (autoReload), true))
        autoReload = true;
      return false;
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]
      {
        (object) configFile
      };
      InternalLogger.Error(ex, "Failed to scan content of config file: {0}", objArray);
      return false;
    }
  }

  private static bool ScanForBooleanParameter(
    string fileContent,
    string parameterName,
    bool parameterValue)
  {
    return fileContent.IndexOf($"{parameterName}=\"{parameterValue}", StringComparison.OrdinalIgnoreCase) >= 0 || fileContent.IndexOf($"{parameterName}='{parameterValue}", StringComparison.OrdinalIgnoreCase) >= 0;
  }

  public IEnumerable<string> GetDefaultCandidateConfigFilePaths(string filename = null)
  {
    string nlogConfigFile = filename ?? "NLog.config";
    string baseDirectory = PathHelpers.TrimDirectorySeparators(this._appEnvironment.AppDomainBaseDirectory);
    if (!string.IsNullOrEmpty(baseDirectory))
      yield return Path.Combine(baseDirectory, nlogConfigFile);
    string nLogConfigFileLowerCase = nlogConfigFile.ToLower();
    bool platformFileSystemCaseInsensitive = nlogConfigFile == nLogConfigFileLowerCase || PlatformDetector.IsWin32;
    if (!platformFileSystemCaseInsensitive && !string.IsNullOrEmpty(baseDirectory))
      yield return Path.Combine(baseDirectory, nLogConfigFileLowerCase);
    string entryAssemblyLocation = PathHelpers.TrimDirectorySeparators(this._appEnvironment.EntryAssemblyLocation);
    if (!string.IsNullOrEmpty(entryAssemblyLocation) && !string.Equals(entryAssemblyLocation, baseDirectory, StringComparison.OrdinalIgnoreCase))
    {
      yield return Path.Combine(entryAssemblyLocation, nlogConfigFile);
      if (!platformFileSystemCaseInsensitive)
        yield return Path.Combine(entryAssemblyLocation, nLogConfigFileLowerCase);
    }
    if (string.IsNullOrEmpty(baseDirectory))
    {
      yield return nlogConfigFile;
      if (!platformFileSystemCaseInsensitive)
        yield return nLogConfigFileLowerCase;
    }
    if (filename == null)
    {
      foreach (string specificNlogLocation in this.GetAppSpecificNLogLocations(baseDirectory, entryAssemblyLocation))
        yield return specificNlogLocation;
    }
    foreach (string pathNlogLocation in this.GetPrivateBinPathNLogLocations(baseDirectory, nlogConfigFile, platformFileSystemCaseInsensitive ? nLogConfigFileLowerCase : string.Empty))
      yield return pathNlogLocation;
    string str = filename != null ? (string) null : LoggingConfigurationFileLoader.LookupNLogAssemblyLocation();
    if (str != null)
      yield return str + ".nlog";
  }

  private static string LookupNLogAssemblyLocation()
  {
    Assembly assembly = typeof (LogFactory).GetAssembly();
    string location = assembly?.Location;
    return !string.IsNullOrEmpty(location) && !assembly.GlobalAssemblyCache ? location : (string) null;
  }

  public IEnumerable<string> GetAppSpecificNLogLocations(
    string baseDirectory,
    string entryAssemblyLocation)
  {
    string configurationFile = this._appEnvironment.AppDomainConfigurationFile;
    if (!StringHelpers.IsNullOrWhiteSpace(configurationFile))
    {
      yield return Path.ChangeExtension(configurationFile, ".nlog");
      if (configurationFile.Contains(".vshost."))
        yield return Path.ChangeExtension(configurationFile.Replace(".vshost.", "."), ".nlog");
    }
  }

  private IEnumerable<string> GetPrivateBinPathNLogLocations(
    string baseDirectory,
    string nlogConfigFile,
    string nLogConfigFileLowerCase)
  {
    IEnumerable<string> privateBinPath = this._appEnvironment.PrivateBinPath;
    if (privateBinPath != null)
    {
      foreach (string path1 in privateBinPath)
      {
        string path = PathHelpers.TrimDirectorySeparators(path1);
        if (!StringHelpers.IsNullOrWhiteSpace(path) && !string.Equals(path, baseDirectory, StringComparison.OrdinalIgnoreCase))
        {
          yield return Path.Combine(path, nlogConfigFile);
          if (!string.IsNullOrEmpty(nLogConfigFileLowerCase))
            yield return Path.Combine(path, nLogConfigFileLowerCase);
        }
        path = (string) null;
      }
    }
  }

  protected virtual void Dispose(bool disposing)
  {
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }
}
