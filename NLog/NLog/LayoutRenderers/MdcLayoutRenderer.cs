// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.MdcLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("mdc")]
[ThreadSafe]
public class MdcLayoutRenderer : LayoutRenderer, IStringValueRenderer
{
  [RequiredParameter]
  [DefaultParameter]
  public string Item { get; set; }

  public string Format { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    object obj = this.GetValue();
    IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
    builder.AppendFormattedValue(obj, this.Format, formatProvider);
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return this.GetStringValue(logEvent);
  }

  private string GetStringValue(LogEventInfo logEvent)
  {
    return this.Format != "@" ? FormatHelper.TryFormatToString(this.GetValue(), this.Format, this.GetFormatProvider(logEvent)) : (string) null;
  }

  private object GetValue() => MappedDiagnosticsContext.GetObject(this.Item);
}
