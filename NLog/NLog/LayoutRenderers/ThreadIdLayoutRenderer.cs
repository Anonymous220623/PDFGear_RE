// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ThreadIdLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("threadid")]
[ThreadSafe]
public class ThreadIdLayoutRenderer : LayoutRenderer
{
  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.AppendInvariant(ThreadIdLayoutRenderer.GetValue());
  }

  private static int GetValue() => AsyncHelpers.GetManagedThreadId();
}
