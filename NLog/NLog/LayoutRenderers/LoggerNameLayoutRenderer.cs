// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.LoggerNameLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("logger")]
[ThreadAgnostic]
[ThreadSafe]
public class LoggerNameLayoutRenderer : LayoutRenderer, IStringValueRenderer
{
  [DefaultValue(false)]
  public bool ShortName { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    if (this.ShortName)
    {
      int lastDotForShortName = this.TryGetLastDotForShortName(logEvent);
      if (lastDotForShortName >= 0)
      {
        builder.Append(logEvent.LoggerName, lastDotForShortName + 1, logEvent.LoggerName.Length - lastDotForShortName - 1);
        return;
      }
    }
    builder.Append(logEvent.LoggerName);
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    if (this.ShortName)
    {
      int lastDotForShortName = this.TryGetLastDotForShortName(logEvent);
      if (lastDotForShortName >= 0)
        return logEvent.LoggerName.Substring(lastDotForShortName + 1);
    }
    return logEvent.LoggerName ?? string.Empty;
  }

  private int TryGetLastDotForShortName(LogEventInfo logEvent)
  {
    string loggerName = logEvent.LoggerName;
    return loggerName == null ? -1 : loggerName.LastIndexOf('.');
  }
}
