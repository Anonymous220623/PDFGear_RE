// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.NLogDirLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.IO;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("nlogdir")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public class NLogDirLayoutRenderer : LayoutRenderer
{
  private string _nlogCombinedPath;

  static NLogDirLayoutRenderer()
  {
    Assembly assembly = typeof (LogManager).GetAssembly();
    NLogDirLayoutRenderer.NLogDir = Path.GetDirectoryName(!string.IsNullOrEmpty(assembly.Location) ? assembly.Location : new Uri(assembly.CodeBase).LocalPath);
  }

  public string File { get; set; }

  public string Dir { get; set; }

  private static string NLogDir { get; set; }

  protected override void InitializeLayoutRenderer()
  {
    this._nlogCombinedPath = (string) null;
    base.InitializeLayoutRenderer();
  }

  protected override void CloseLayoutRenderer()
  {
    this._nlogCombinedPath = (string) null;
    base.CloseLayoutRenderer();
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    string str = this._nlogCombinedPath ?? (this._nlogCombinedPath = PathHelpers.CombinePaths(NLogDirLayoutRenderer.NLogDir, this.Dir, this.File));
    builder.Append(str);
  }
}
