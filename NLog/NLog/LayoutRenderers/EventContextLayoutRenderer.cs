// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.EventContextLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("event-context")]
[MutableUnsafe]
[Obsolete("Use EventPropertiesLayoutRenderer class instead. Marked obsolete on NLog 2.0")]
public class EventContextLayoutRenderer : LayoutRenderer
{
  [RequiredParameter]
  [DefaultParameter]
  public string Item { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    object obj;
    if (!logEvent.HasProperties || !logEvent.Properties.TryGetValue((object) this.Item, out obj))
      return;
    IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
    builder.Append(Convert.ToString(obj, formatProvider));
  }
}
