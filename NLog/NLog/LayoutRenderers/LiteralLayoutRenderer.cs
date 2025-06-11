// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.LiteralLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("literal")]
[ThreadAgnostic]
[ThreadSafe]
[AppDomainFixedOutput]
public class LiteralLayoutRenderer : LayoutRenderer
{
  public LiteralLayoutRenderer()
  {
  }

  public LiteralLayoutRenderer(string text) => this.Text = text;

  public string Text { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(this.Text);
  }
}
