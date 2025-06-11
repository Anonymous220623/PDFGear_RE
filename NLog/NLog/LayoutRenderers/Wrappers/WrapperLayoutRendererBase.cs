// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.WrapperLayoutRendererBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

public abstract class WrapperLayoutRendererBase : LayoutRenderer
{
  [DefaultParameter]
  public Layout Inner { get; set; }

  protected override void InitializeLayoutRenderer()
  {
    base.InitializeLayoutRenderer();
    this.Inner?.Initialize(this.LoggingConfiguration);
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    if (this.Inner == null)
    {
      InternalLogger.Warn<WrapperLayoutRendererBase>("{0} has no configured Inner-Layout, so skipping", this);
    }
    else
    {
      int length = builder.Length;
      try
      {
        this.RenderInnerAndTransform(logEvent, builder, length);
      }
      catch
      {
        builder.Length = length;
        throw;
      }
    }
  }

  protected virtual void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    string text = this.RenderInner(logEvent);
    builder.Append(this.Transform(logEvent, text));
  }

  protected virtual string Transform(LogEventInfo logEvent, string text) => this.Transform(text);

  protected abstract string Transform(string text);

  protected virtual string RenderInner(LogEventInfo logEvent)
  {
    return this.Inner?.Render(logEvent) ?? string.Empty;
  }
}
