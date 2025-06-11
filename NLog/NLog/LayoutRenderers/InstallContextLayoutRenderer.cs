// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.InstallContextLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("install-context")]
[ThreadSafe]
public class InstallContextLayoutRenderer : LayoutRenderer
{
  [RequiredParameter]
  [DefaultParameter]
  public string Parameter { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    object obj = this.GetValue(logEvent);
    if (obj == null)
      return;
    IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
    builder.Append(Convert.ToString(obj, formatProvider));
  }

  private object GetValue(LogEventInfo logEvent)
  {
    object obj;
    return logEvent.Properties.TryGetValue((object) this.Parameter, out obj) ? obj : (object) null;
  }
}
