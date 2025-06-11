// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.GuidLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("guid")]
[ThreadSafe]
[ThreadAgnostic]
public class GuidLayoutRenderer : LayoutRenderer, IRawValue, IStringValueRenderer
{
  [DefaultValue("N")]
  public string Format { get; set; } = "N";

  [DefaultValue(false)]
  public bool GeneratedFromLogEvent { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(this.GetStringValue(logEvent));
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) this.GetValue(logEvent);
    return true;
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return this.GetStringValue(logEvent);
  }

  private string GetStringValue(LogEventInfo logEvent)
  {
    return this.GetValue(logEvent).ToString(this.Format);
  }

  private Guid GetValue(LogEventInfo logEvent)
  {
    Guid guid;
    if (this.GeneratedFromLogEvent)
    {
      int hashCode = logEvent.GetHashCode();
      short b = (short) (hashCode >> 16 /*0x10*/ & (int) ushort.MaxValue);
      short c = (short) (hashCode & (int) ushort.MaxValue);
      long ticks = LogEventInfo.ZeroDate.Ticks;
      byte d = (byte) ((ulong) (ticks >> 56) & (ulong) byte.MaxValue);
      byte e = (byte) ((ulong) (ticks >> 48 /*0x30*/) & (ulong) byte.MaxValue);
      byte f = (byte) ((ulong) (ticks >> 40) & (ulong) byte.MaxValue);
      byte g = (byte) ((ulong) (ticks >> 32 /*0x20*/) & (ulong) byte.MaxValue);
      byte h = (byte) ((ulong) (ticks >> 24) & (ulong) byte.MaxValue);
      byte i = (byte) ((ulong) (ticks >> 16 /*0x10*/) & (ulong) byte.MaxValue);
      byte j = (byte) ((ulong) (ticks >> 8) & (ulong) byte.MaxValue);
      byte k = (byte) ((ulong) ticks & (ulong) byte.MaxValue);
      guid = new Guid(logEvent.SequenceID, b, c, d, e, f, g, h, i, j, k);
    }
    else
      guid = Guid.NewGuid();
    return guid;
  }
}
