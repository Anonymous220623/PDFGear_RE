// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.FuncLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

public class FuncLayoutRenderer : LayoutRenderer, IStringValueRenderer
{
  private readonly Func<LogEventInfo, LoggingConfiguration, object> _renderMethod;

  protected FuncLayoutRenderer(string layoutRendererName)
  {
    this.LayoutRendererName = layoutRendererName;
  }

  public FuncLayoutRenderer(
    string layoutRendererName,
    Func<LogEventInfo, LoggingConfiguration, object> renderMethod)
  {
    this._renderMethod = renderMethod ?? throw new ArgumentNullException(nameof (renderMethod));
    this.LayoutRendererName = layoutRendererName;
  }

  public string LayoutRendererName { get; set; }

  public Func<LogEventInfo, LoggingConfiguration, object> RenderMethod => this._renderMethod;

  public string Format { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    object obj = this.RenderValue(logEvent);
    IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
    builder.AppendFormattedValue(obj, this.Format, formatProvider);
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return this.GetStringValue(logEvent);
  }

  private string GetStringValue(LogEventInfo logEvent)
  {
    return this.Format != "@" ? FormatHelper.TryFormatToString(this.RenderValue(logEvent), this.Format, this.GetFormatProvider(logEvent)) : (string) null;
  }

  protected virtual object RenderValue(LogEventInfo logEvent)
  {
    Func<LogEventInfo, LoggingConfiguration, object> renderMethod = this._renderMethod;
    return renderMethod == null ? (object) null : renderMethod(logEvent, this.LoggingConfiguration);
  }
}
