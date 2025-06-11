// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ProcessIdLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.Internal.Fakeables;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("processid")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public class ProcessIdLayoutRenderer : LayoutRenderer, IRawValue
{
  private readonly int _processId;

  public ProcessIdLayoutRenderer()
    : this(LogFactory.DefaultAppEnvironment)
  {
  }

  internal ProcessIdLayoutRenderer(IAppEnvironment appEnvironment)
  {
    this._processId = appEnvironment.CurrentProcessId;
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.AppendInvariant(this._processId);
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) this._processId;
    return true;
  }
}
