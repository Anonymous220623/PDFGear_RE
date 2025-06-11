// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ThreadNameLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("threadname")]
[ThreadSafe]
public class ThreadNameLayoutRenderer : LayoutRenderer, IStringValueRenderer
{
  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(ThreadNameLayoutRenderer.GetStringValue());
  }

  private static string GetStringValue() => Thread.CurrentThread.Name;

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return ThreadNameLayoutRenderer.GetStringValue();
  }
}
