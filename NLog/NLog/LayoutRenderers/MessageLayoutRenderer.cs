// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.MessageLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("message")]
[ThreadAgnostic]
[ThreadSafe]
public class MessageLayoutRenderer : LayoutRenderer, IStringValueRenderer
{
  public MessageLayoutRenderer() => this.ExceptionSeparator = EnvironmentHelper.NewLine;

  public bool WithException { get; set; }

  public string ExceptionSeparator { get; set; }

  public bool Raw { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    int num;
    if (logEvent.Exception != null && this.WithException)
    {
      object[] parameters = logEvent.Parameters;
      if ((parameters != null ? (parameters.Length == 1 ? 1 : 0) : 0) != 0 && logEvent.Parameters[0] == logEvent.Exception)
      {
        num = logEvent.Message == "{0}" ? 1 : 0;
        goto label_4;
      }
    }
    num = 0;
label_4:
    bool flag = num != 0;
    if (this.Raw)
      builder.Append(logEvent.Message);
    else if (!flag)
    {
      if (logEvent.MessageFormatter == LogMessageTemplateFormatter.DefaultAutoSingleTarget.MessageFormatter)
        logEvent.AppendFormattedMessage((ILogMessageFormatter) LogMessageTemplateFormatter.DefaultAutoSingleTarget, builder);
      else
        builder.Append(logEvent.FormattedMessage);
    }
    if (!this.WithException || logEvent.Exception == null)
      return;
    Exception exception1 = logEvent.Exception;
    if (logEvent.Exception is AggregateException exception2)
    {
      AggregateException aggregateException = exception2.Flatten();
      exception1 = aggregateException.InnerExceptions.Count == 1 ? aggregateException.InnerExceptions[0] : (Exception) aggregateException;
    }
    if (!flag)
      builder.Append(this.ExceptionSeparator);
    builder.Append(exception1.ToString());
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return this.WithException ? (string) null : (this.Raw ? logEvent.Message : logEvent.FormattedMessage) ?? string.Empty;
  }
}
