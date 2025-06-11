// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.SequenceIdLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("sequenceid")]
[ThreadAgnostic]
[ThreadSafe]
public class SequenceIdLayoutRenderer : LayoutRenderer, IRawValue
{
  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.AppendInvariant(SequenceIdLayoutRenderer.GetValue(logEvent));
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) SequenceIdLayoutRenderer.GetValue(logEvent);
    return true;
  }

  private static int GetValue(LogEventInfo logEvent) => logEvent.SequenceID;
}
