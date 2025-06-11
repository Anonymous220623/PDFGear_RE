// Decompiled with JetBrains decompiler
// Type: NLog.Internal.LogMessageTemplateFormatter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.MessageTemplates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.Internal;

internal sealed class LogMessageTemplateFormatter : ILogMessageFormatter
{
  public static readonly LogMessageTemplateFormatter DefaultAuto = new LogMessageTemplateFormatter(false, false);
  public static readonly LogMessageTemplateFormatter Default = new LogMessageTemplateFormatter(true, false);
  public static readonly LogMessageTemplateFormatter DefaultAutoSingleTarget = new LogMessageTemplateFormatter(false, true);
  private static readonly StringBuilderPool _builderPool = new StringBuilderPool(Environment.ProcessorCount * 2);
  private readonly bool _forceTemplateRenderer;
  private readonly bool _singleTargetOnly;

  private LogMessageTemplateFormatter(bool forceTemplateRenderer, bool singleTargetOnly)
  {
    this._forceTemplateRenderer = forceTemplateRenderer;
    this._singleTargetOnly = singleTargetOnly;
    this.MessageFormatter = new LogMessageFormatter(this.FormatMessage);
  }

  public LogMessageFormatter MessageFormatter { get; }

  public bool HasProperties(LogEventInfo logEvent)
  {
    if (!this.HasParameters(logEvent))
      return false;
    if (this._singleTargetOnly)
    {
      TemplateEnumerator templateEnumerator = new TemplateEnumerator(logEvent.Message);
      if (templateEnumerator.MoveNext() && templateEnumerator.Current.MaybePositionalTemplate)
        return false;
    }
    return true;
  }

  private bool HasParameters(LogEventInfo logEvent)
  {
    return logEvent.Parameters != null && !string.IsNullOrEmpty(logEvent.Message) && logEvent.Parameters.Length != 0;
  }

  public void AppendFormattedMessage(LogEventInfo logEvent, StringBuilder builder)
  {
    if (!this.HasParameters(logEvent))
      builder.Append(logEvent.Message ?? string.Empty);
    else
      logEvent.Message.Render(logEvent.FormatProvider ?? (IFormatProvider) CultureInfo.CurrentCulture, logEvent.Parameters, this._forceTemplateRenderer, builder, out IList<MessageTemplateParameter> _);
  }

  public string FormatMessage(LogEventInfo logEvent)
  {
    if (!this.HasParameters(logEvent))
      return logEvent.Message;
    using (StringBuilderPool.ItemHolder itemHolder = LogMessageTemplateFormatter._builderPool.Acquire())
    {
      this.AppendToBuilder(logEvent, itemHolder.Item);
      return itemHolder.Item.ToString();
    }
  }

  private void AppendToBuilder(LogEventInfo logEvent, StringBuilder builder)
  {
    IList<MessageTemplateParameter> messageTemplateParameters;
    logEvent.Message.Render(logEvent.FormatProvider ?? (IFormatProvider) CultureInfo.CurrentCulture, logEvent.Parameters, this._forceTemplateRenderer, builder, out messageTemplateParameters);
    logEvent.CreateOrUpdatePropertiesInternal(false, (IList<MessageTemplateParameter>) ((object) messageTemplateParameters ?? (object) ArrayHelper.Empty<MessageTemplateParameter>()));
  }
}
