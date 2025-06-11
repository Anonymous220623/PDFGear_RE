// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ProcessNameLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal.Fakeables;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("processname")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public class ProcessNameLayoutRenderer : LayoutRenderer
{
  private readonly string _processFilePath;
  private readonly string _processBaseName;

  [DefaultValue(false)]
  public bool FullName { get; set; }

  public ProcessNameLayoutRenderer()
    : this(LogFactory.DefaultAppEnvironment)
  {
  }

  internal ProcessNameLayoutRenderer(IAppEnvironment appEnvironment)
  {
    this._processFilePath = appEnvironment.CurrentProcessFilePath;
    this._processBaseName = appEnvironment.CurrentProcessBaseName;
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    string str = this.FullName ? this._processFilePath : this._processBaseName;
    builder.Append(str);
  }
}
