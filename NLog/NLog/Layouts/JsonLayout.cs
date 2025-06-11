// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.JsonLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.MessageTemplates;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.Layouts;

[Layout("JsonLayout")]
[NLog.Config.ThreadAgnostic]
[NLog.Config.ThreadSafe]
public class JsonLayout : Layout
{
  private JsonLayout.LimitRecursionJsonConvert _jsonConverter;
  private IValueFormatter _valueFormatter;
  private bool? _escapeForwardSlashInternal;

  private JsonLayout.LimitRecursionJsonConvert JsonConverter
  {
    get
    {
      return this._jsonConverter ?? (this._jsonConverter = new JsonLayout.LimitRecursionJsonConvert(ConfigurationItemFactory.Default.JsonConverter, this.MaxRecursionLimit, this.EscapeForwardSlash));
    }
    set => this._jsonConverter = value;
  }

  private IValueFormatter ValueFormatter
  {
    get
    {
      return this._valueFormatter ?? (this._valueFormatter = ConfigurationItemFactory.Default.ValueFormatter);
    }
    set => this._valueFormatter = value;
  }

  public JsonLayout()
  {
    this.Attributes = (IList<JsonAttribute>) new List<JsonAttribute>();
    this.RenderEmptyObject = true;
    this.IncludeAllProperties = false;
    this.ExcludeProperties = (ISet<string>) new HashSet<string>();
    this.MaxRecursionLimit = 0;
  }

  [ArrayParameter(typeof (JsonAttribute), "attribute")]
  public IList<JsonAttribute> Attributes { get; private set; }

  [DefaultValue(false)]
  public bool SuppressSpaces { get; set; }

  [DefaultValue(true)]
  public bool RenderEmptyObject { get; set; }

  [DefaultValue(false)]
  public bool IncludeGdc { get; set; }

  [DefaultValue(false)]
  public bool IncludeMdc { get; set; }

  [DefaultValue(false)]
  public bool IncludeMdlc { get; set; }

  [DefaultValue(false)]
  public bool IncludeAllProperties { get; set; }

  [DefaultValue(false)]
  public bool ExcludeEmptyProperties { get; set; }

  public ISet<string> ExcludeProperties { get; set; }

  [DefaultValue(0)]
  public int MaxRecursionLimit { get; set; }

  [DefaultValue(true)]
  public bool EscapeForwardSlash
  {
    get => this._escapeForwardSlashInternal ?? true;
    set => this._escapeForwardSlashInternal = new bool?(value);
  }

  protected override void InitializeLayout()
  {
    base.InitializeLayout();
    if (this.IncludeMdc)
      this.ThreadAgnostic = false;
    if (this.IncludeMdlc)
      this.ThreadAgnostic = false;
    if (this.IncludeAllProperties)
      this.MutableUnsafe = true;
    if (!this._escapeForwardSlashInternal.HasValue)
      return;
    IList<JsonAttribute> attributes = this.Attributes;
    if ((attributes != null ? (attributes.Count > 0 ? 1 : 0) : 0) == 0)
      return;
    foreach (JsonAttribute attribute in (IEnumerable<JsonAttribute>) this.Attributes)
    {
      if (!attribute.LayoutWrapper.EscapeForwardSlashInternal.HasValue)
        attribute.LayoutWrapper.EscapeForwardSlashInternal = new bool?(this._escapeForwardSlashInternal.Value);
    }
  }

  protected override void CloseLayout()
  {
    this.JsonConverter = (JsonLayout.LimitRecursionJsonConvert) null;
    this.ValueFormatter = (IValueFormatter) null;
    base.CloseLayout();
  }

  internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
  {
    this.PrecalculateBuilderInternal(logEvent, target);
  }

  protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
  {
    int length = target.Length;
    this.RenderJsonFormattedMessage(logEvent, target);
    if (target.Length != length || !this.RenderEmptyObject)
      return;
    target.Append(this.SuppressSpaces ? "{}" : "{  }");
  }

  protected override string GetFormattedMessage(LogEventInfo logEvent)
  {
    return this.RenderAllocateBuilder(logEvent);
  }

  private void RenderJsonFormattedMessage(LogEventInfo logEvent, StringBuilder sb)
  {
    int length1 = sb.Length;
    for (int index = 0; index < this.Attributes.Count; ++index)
    {
      JsonAttribute attribute = this.Attributes[index];
      int length2 = sb.Length;
      if (!this.RenderAppendJsonPropertyValue(attribute, logEvent, sb, sb.Length == length1))
        sb.Length = length2;
    }
    if (this.IncludeGdc)
    {
      foreach (string name in (IEnumerable<string>) GlobalDiagnosticsContext.GetNames())
      {
        if (!string.IsNullOrEmpty(name))
        {
          object propertyValue = GlobalDiagnosticsContext.GetObject(name);
          this.AppendJsonPropertyValue(name, propertyValue, (string) null, (IFormatProvider) null, CaptureType.Unknown, sb, sb.Length == length1);
        }
      }
    }
    if (this.IncludeMdc)
    {
      foreach (string name in (IEnumerable<string>) MappedDiagnosticsContext.GetNames())
      {
        if (!string.IsNullOrEmpty(name))
        {
          object propertyValue = MappedDiagnosticsContext.GetObject(name);
          this.AppendJsonPropertyValue(name, propertyValue, (string) null, (IFormatProvider) null, CaptureType.Unknown, sb, sb.Length == length1);
        }
      }
    }
    if (this.IncludeMdlc)
    {
      foreach (string name in (IEnumerable<string>) MappedDiagnosticsLogicalContext.GetNames())
      {
        if (!string.IsNullOrEmpty(name))
        {
          object propertyValue = MappedDiagnosticsLogicalContext.GetObject(name);
          this.AppendJsonPropertyValue(name, propertyValue, (string) null, (IFormatProvider) null, CaptureType.Unknown, sb, sb.Length == length1);
        }
      }
    }
    if (this.IncludeAllProperties && logEvent.HasProperties)
    {
      foreach (MessageTemplateParameter templateParameter in (IEnumerable<MessageTemplateParameter>) logEvent.CreateOrUpdatePropertiesInternal())
      {
        if (!string.IsNullOrEmpty(templateParameter.Name) && !this.ExcludeProperties.Contains(templateParameter.Name))
          this.AppendJsonPropertyValue(templateParameter.Name, templateParameter.Value, templateParameter.Format, logEvent.FormatProvider, templateParameter.CaptureType, sb, sb.Length == length1);
      }
    }
    if (sb.Length <= length1)
      return;
    this.CompleteJsonMessage(sb);
  }

  private void BeginJsonProperty(
    StringBuilder sb,
    string propName,
    bool beginJsonMessage,
    bool ensureStringEscape)
  {
    if (beginJsonMessage)
      sb.Append(this.SuppressSpaces ? "{\"" : "{ \"");
    else
      sb.Append(this.SuppressSpaces ? ",\"" : ", \"");
    if (ensureStringEscape)
      DefaultJsonSerializer.AppendStringEscape(sb, propName, false, false);
    else
      sb.Append(propName);
    sb.Append(this.SuppressSpaces ? "\":" : "\": ");
  }

  private void CompleteJsonMessage(StringBuilder sb) => sb.Append(this.SuppressSpaces ? "}" : " }");

  private void AppendJsonPropertyValue(
    string propName,
    object propertyValue,
    string format,
    IFormatProvider formatProvider,
    CaptureType captureType,
    StringBuilder sb,
    bool beginJsonMessage)
  {
    if (this.ExcludeEmptyProperties && propertyValue == null)
      return;
    int length1 = sb.Length;
    this.BeginJsonProperty(sb, propName, beginJsonMessage, true);
    if (this.MaxRecursionLimit <= 1 && captureType == CaptureType.Serialize)
    {
      if (!this.JsonConverter.SerializeObjectNoLimit(propertyValue, sb))
      {
        sb.Length = length1;
        return;
      }
    }
    else if (captureType == CaptureType.Stringify)
    {
      int length2 = sb.Length;
      this.ValueFormatter.FormatValue(propertyValue, format, captureType, formatProvider, sb);
      JsonLayout.PerformJsonEscapeIfNeeded(sb, length2, this.EscapeForwardSlash);
    }
    else if (!this.JsonConverter.SerializeObject(propertyValue, sb))
    {
      sb.Length = length1;
      return;
    }
    if (!this.ExcludeEmptyProperties || sb[sb.Length - 1] != '"' || sb[sb.Length - 2] != '"')
      return;
    sb.Length = length1;
  }

  private static void PerformJsonEscapeIfNeeded(
    StringBuilder sb,
    int valueStart,
    bool escapeForwardSlash)
  {
    if (sb.Length - valueStart <= 2)
      return;
    for (int index = valueStart + 1; index < sb.Length - 1; ++index)
    {
      if (DefaultJsonSerializer.RequiresJsonEscape(sb[index], false, escapeForwardSlash))
      {
        string text = sb.ToString(valueStart + 1, sb.Length - valueStart - 2);
        sb.Length = valueStart;
        sb.Append('"');
        DefaultJsonSerializer.AppendStringEscape(sb, text, false, escapeForwardSlash);
        sb.Append('"');
        break;
      }
    }
  }

  private bool RenderAppendJsonPropertyValue(
    JsonAttribute attrib,
    LogEventInfo logEvent,
    StringBuilder sb,
    bool beginJsonMessage)
  {
    this.BeginJsonProperty(sb, attrib.Name, beginJsonMessage, false);
    if (attrib.Encode)
      sb.Append('"');
    int length = sb.Length;
    attrib.LayoutWrapper.RenderAppendBuilder(logEvent, sb);
    if (!attrib.IncludeEmptyValue && length == sb.Length)
      return false;
    if (attrib.Encode)
      sb.Append('"');
    return true;
  }

  public override string ToString()
  {
    return this.ToStringWithNestedItems<JsonAttribute>(this.Attributes, (Func<JsonAttribute, string>) (a => $"{a.Name}-{a.Layout?.ToString()}"));
  }

  private class LimitRecursionJsonConvert : IJsonConverter
  {
    private readonly IJsonConverter _converter;
    private readonly DefaultJsonSerializer _serializer;
    private readonly JsonSerializeOptions _serializerOptions;

    public LimitRecursionJsonConvert(
      IJsonConverter converter,
      int maxRecursionLimit,
      bool escapeForwardSlash)
    {
      this._converter = converter;
      this._serializer = converter as DefaultJsonSerializer;
      this._serializerOptions = new JsonSerializeOptions()
      {
        MaxRecursionLimit = Math.Max(0, maxRecursionLimit),
        EscapeForwardSlash = escapeForwardSlash
      };
    }

    public bool SerializeObject(object value, StringBuilder builder)
    {
      return this._serializer != null ? this._serializer.SerializeObject(value, builder, this._serializerOptions) : this._converter.SerializeObject(value, builder);
    }

    public bool SerializeObjectNoLimit(object value, StringBuilder builder)
    {
      return this._converter.SerializeObject(value, builder);
    }
  }
}
