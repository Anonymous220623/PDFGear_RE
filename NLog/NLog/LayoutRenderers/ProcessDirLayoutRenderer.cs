// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ProcessDirLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.Internal.Fakeables;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("processdir")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public class ProcessDirLayoutRenderer : LayoutRenderer
{
  private readonly string _processDir;

  public string File { get; set; }

  public string Dir { get; set; }

  public ProcessDirLayoutRenderer()
    : this(LogFactory.DefaultAppEnvironment)
  {
  }

  internal ProcessDirLayoutRenderer(IAppEnvironment appEnvironment)
  {
    this._processDir = Path.GetDirectoryName(appEnvironment.CurrentProcessFilePath);
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    string str = PathHelpers.CombinePaths(this._processDir, this.Dir, this.File);
    builder.Append(str);
  }
}
