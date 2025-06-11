// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.LevelLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("level")]
[ThreadAgnostic]
[ThreadSafe]
public class LevelLayoutRenderer : LayoutRenderer, IRawValue, IStringValueRenderer
{
  [DefaultValue(LevelFormat.Name)]
  public LevelFormat Format { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    LogLevel logLevel = LevelLayoutRenderer.GetValue(logEvent);
    switch (this.Format)
    {
      case LevelFormat.Name:
        builder.Append(logLevel.ToString());
        break;
      case LevelFormat.FirstCharacter:
        builder.Append(logLevel.ToString()[0]);
        break;
      case LevelFormat.Ordinal:
        builder.AppendInvariant(logLevel.Ordinal);
        break;
      case LevelFormat.FullName:
        if (logLevel == LogLevel.Info)
        {
          builder.Append("Information");
          break;
        }
        if (logLevel == LogLevel.Warn)
        {
          builder.Append("Warning");
          break;
        }
        builder.Append(logLevel.ToString());
        break;
    }
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) LevelLayoutRenderer.GetValue(logEvent);
    return true;
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return this.Format != LevelFormat.Name ? (string) null : LevelLayoutRenderer.GetValue(logEvent).ToString();
  }

  private static LogLevel GetValue(LogEventInfo logEvent) => logEvent.Level;
}
