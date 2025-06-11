// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.VariableLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("var")]
[ThreadSafe]
public class VariableLayoutRenderer : LayoutRenderer
{
  [RequiredParameter]
  [DefaultParameter]
  public string Name { get; set; }

  public string Default { get; set; }

  protected override void InitializeLayoutRenderer()
  {
    SimpleLayout layout;
    if (this.TryGetLayout(out layout) && layout != null)
    {
      layout.Initialize(this.LoggingConfiguration);
      if (!layout.ThreadSafe)
        InternalLogger.Warn<string, SimpleLayout>("${{var={0}}} should be declared as <variable name=\"var_{0}\" value=\"...\" /> and used like this ${{var_{0}}}. Because of unsafe Layout={1}", this.Name, layout);
    }
    base.InitializeLayoutRenderer();
  }

  private bool TryGetLayout(out SimpleLayout layout)
  {
    layout = (SimpleLayout) null;
    if (this.Name == null)
      return false;
    LoggingConfiguration loggingConfiguration = this.LoggingConfiguration;
    if (loggingConfiguration == null)
      return false;
    bool? nullable = loggingConfiguration.Variables?.TryGetValue(this.Name, out layout);
    bool flag = true;
    return nullable.GetValueOrDefault() == flag & nullable.HasValue;
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    if (this.Name == null)
      return;
    SimpleLayout layout;
    if (this.TryGetLayout(out layout))
    {
      layout?.RenderAppendBuilder(logEvent, builder);
    }
    else
    {
      if (this.Default == null)
        return;
      builder.Append(this.Default);
    }
  }
}
