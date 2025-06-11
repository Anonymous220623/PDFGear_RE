// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.BaseDirLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Internal.Fakeables;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("basedir")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public class BaseDirLayoutRenderer : LayoutRenderer
{
  private readonly string _baseDir;
  private string _processDir;
  private readonly IAppEnvironment _appEnvironment;

  public bool ProcessDir { get; set; }

  public bool FixTempDir { get; set; }

  public BaseDirLayoutRenderer()
    : this(LogFactory.DefaultAppEnvironment)
  {
  }

  public BaseDirLayoutRenderer(IAppDomain appDomain)
  {
    this._baseDir = appDomain.BaseDirectory;
    this._appEnvironment = LogFactory.DefaultAppEnvironment;
  }

  internal BaseDirLayoutRenderer(IAppEnvironment appEnvironment)
  {
    this._baseDir = appEnvironment.AppDomainBaseDirectory;
    this._appEnvironment = appEnvironment;
  }

  public string File { get; set; }

  public string Dir { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    string path = this._baseDir;
    if (this.ProcessDir)
      path = this._processDir ?? (this._processDir = this.GetProcessDir());
    else if (this.FixTempDir)
      path = this._processDir ?? (this._processDir = this.GetFixedTempBaseDir(this._baseDir));
    if (path == null)
      return;
    string str = PathHelpers.CombinePaths(path, this.Dir, this.File);
    builder.Append(str);
  }

  private string GetFixedTempBaseDir(string baseDir)
  {
    try
    {
      string userTempFilePath = this._appEnvironment.UserTempFilePath;
      if (PathHelpers.IsTempDir(baseDir, userTempFilePath))
      {
        string processDir = this.GetProcessDir();
        if (!string.IsNullOrEmpty(processDir) && !PathHelpers.IsTempDir(processDir, userTempFilePath))
          return processDir;
      }
      return baseDir;
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "BaseDir LayoutRenderer unexpected exception");
      return baseDir;
    }
  }

  private string GetProcessDir()
  {
    return Path.GetDirectoryName(this._appEnvironment.CurrentProcessFilePath);
  }
}
