// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.EventPropertiesLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("event-properties")]
[ThreadAgnostic]
[ThreadSafe]
[MutableUnsafe]
public class EventPropertiesLayoutRenderer : LayoutRenderer, IRawValue, IStringValueRenderer
{
  private readonly ObjectPropertyHelper _objectPropertyHelper = new ObjectPropertyHelper();

  [RequiredParameter]
  [DefaultParameter]
  public string Item { get; set; }

  public string Format { get; set; }

  public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

  public string ObjectPath
  {
    get => this._objectPropertyHelper.ObjectPath;
    set => this._objectPropertyHelper.ObjectPath = value;
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    object obj;
    if (!this.TryGetValue(logEvent, out obj))
      return;
    IFormatProvider formatProvider = this.GetFormatProvider(logEvent, (IFormatProvider) this.Culture);
    builder.AppendFormattedValue(obj, this.Format, formatProvider);
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    this.TryGetValue(logEvent, out value);
    return true;
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return this.GetStringValue(logEvent);
  }

  private bool TryGetValue(LogEventInfo logEvent, out object value)
  {
    value = (object) null;
    if (!logEvent.HasProperties || !logEvent.Properties.TryGetValue((object) this.Item, out value))
      return false;
    if (this.ObjectPath != null)
    {
      object foundValue;
      value = !this._objectPropertyHelper.TryGetObjectProperty(value, out foundValue) ? (object) null : foundValue;
    }
    return true;
  }

  private string GetStringValue(LogEventInfo logEvent)
  {
    if (!(this.Format != "@"))
      return (string) null;
    object obj;
    return this.TryGetValue(logEvent, out obj) ? FormatHelper.TryFormatToString(obj, this.Format, this.GetFormatProvider(logEvent, (IFormatProvider) this.Culture)) : string.Empty;
  }
}
