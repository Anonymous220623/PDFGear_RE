// Decompiled with JetBrains decompiler
// Type: NLog.Internal.Fakeables.AppEnvironmentWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Xml;

#nullable disable
namespace NLog.Internal.Fakeables;

internal class AppEnvironmentWrapper : IAppEnvironment, IFileSystem
{
  private const string UnknownProcessName = "<unknown>";
  private string _entryAssemblyLocation;
  private string _entryAssemblyFileName;
  private string _currentProcessFilePath;
  private string _currentProcessBaseName;
  private int? _currentProcessId;

  public string EntryAssemblyLocation
  {
    get
    {
      return this._entryAssemblyLocation ?? (this._entryAssemblyLocation = AppEnvironmentWrapper.LookupEntryAssemblyLocation());
    }
  }

  public string EntryAssemblyFileName
  {
    get
    {
      return this._entryAssemblyFileName ?? (this._entryAssemblyFileName = AppEnvironmentWrapper.LookupEntryAssemblyFileName());
    }
  }

  public string CurrentProcessFilePath
  {
    get
    {
      return this._currentProcessFilePath ?? (this._currentProcessFilePath = AppEnvironmentWrapper.LookupCurrentProcessFilePathWithFallback());
    }
  }

  public string CurrentProcessBaseName
  {
    get
    {
      return this._currentProcessBaseName ?? (this._currentProcessBaseName = string.IsNullOrEmpty(this.CurrentProcessFilePath) ? "<unknown>" : Path.GetFileNameWithoutExtension(this.CurrentProcessFilePath));
    }
  }

  public int CurrentProcessId
  {
    get
    {
      return this._currentProcessId ?? (this._currentProcessId = new int?(AppEnvironmentWrapper.LookupCurrentProcessIdWithFallback())).Value;
    }
  }

  public string AppDomainBaseDirectory => this.AppDomain.BaseDirectory;

  public string AppDomainConfigurationFile => this.AppDomain.ConfigurationFile;

  public IEnumerable<string> PrivateBinPath => this.AppDomain.PrivateBinPath;

  public string UserTempFilePath => Path.GetTempPath();

  public IAppDomain AppDomain { get; internal set; }

  public AppEnvironmentWrapper(IAppDomain appDomain) => this.AppDomain = appDomain;

  public bool FileExists(string path) => File.Exists(path);

  public XmlReader LoadXmlFile(string path) => XmlReader.Create(path);

  private static string LookupEntryAssemblyLocation()
  {
    return AssemblyHelpers.GetAssemblyFileLocation(Assembly.GetEntryAssembly());
  }

  private static string LookupEntryAssemblyFileName()
  {
    try
    {
      return Path.GetFileName(Assembly.GetEntryAssembly()?.Location ?? string.Empty);
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return string.Empty;
      throw;
    }
  }

  private static string LookupCurrentProcessFilePathWithFallback()
  {
    try
    {
      return AppEnvironmentWrapper.LookupCurrentProcessFilePath() ?? AppEnvironmentWrapper.LookupCurrentProcessFilePathNative();
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return AppEnvironmentWrapper.LookupCurrentProcessFilePathNative();
      throw;
    }
  }

  private static string LookupCurrentProcessFilePath()
  {
    try
    {
      return Process.GetCurrentProcess()?.MainModule.FileName;
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return (string) null;
      throw;
    }
  }

  private static int LookupCurrentProcessIdWithFallback()
  {
    try
    {
      return AppEnvironmentWrapper.LookupCurrentProcessId() ?? AppEnvironmentWrapper.LookupCurrentProcessIdNative();
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return AppEnvironmentWrapper.LookupCurrentProcessIdNative();
      throw;
    }
  }

  private static int? LookupCurrentProcessId()
  {
    try
    {
      return Process.GetCurrentProcess()?.Id;
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return new int?();
      throw;
    }
  }

  private static string LookupCurrentProcessFilePathNative()
  {
    try
    {
      return !PlatformDetector.IsWin32 ? string.Empty : AppEnvironmentWrapper.LookupCurrentProcessFilePathWin32();
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return string.Empty;
      throw;
    }
  }

  [SecuritySafeCritical]
  private static string LookupCurrentProcessFilePathWin32()
  {
    try
    {
      StringBuilder lpFilename = new StringBuilder(512 /*0x0200*/);
      if (NativeMethods.GetModuleFileName(IntPtr.Zero, lpFilename, lpFilename.Capacity) == 0U)
        throw new InvalidOperationException("Cannot determine program name.");
      return lpFilename.ToString();
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return string.Empty;
      throw;
    }
  }

  private static int LookupCurrentProcessIdNative()
  {
    try
    {
      return !PlatformDetector.IsWin32 ? 0 : AppEnvironmentWrapper.LookupCurrentProcessIdWin32();
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return 0;
      throw;
    }
  }

  [SecuritySafeCritical]
  private static int LookupCurrentProcessIdWin32()
  {
    try
    {
      return NativeMethods.GetCurrentProcessId();
    }
    catch (Exception ex)
    {
      if (!ex.MustBeRethrownImmediately())
        return 0;
      throw;
    }
  }
}
