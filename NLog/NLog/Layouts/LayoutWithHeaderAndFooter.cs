// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.LayoutWithHeaderAndFooter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System.Text;

#nullable disable
namespace NLog.Layouts;

[Layout("LayoutWithHeaderAndFooter")]
[NLog.Config.ThreadAgnostic]
[NLog.Config.ThreadSafe]
[AppDomainFixedOutput]
public class LayoutWithHeaderAndFooter : Layout
{
  public Layout Layout { get; set; }

  public Layout Header { get; set; }

  public Layout Footer { get; set; }

  protected override string GetFormattedMessage(LogEventInfo logEvent)
  {
    return this.Layout.Render(logEvent);
  }

  protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
  {
    this.Layout.RenderAppendBuilder(logEvent, target);
  }
}
