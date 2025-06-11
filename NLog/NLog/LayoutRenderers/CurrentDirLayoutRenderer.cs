// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.CurrentDirLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("currentdir")]
[ThreadAgnostic]
[ThreadSafe]
public class CurrentDirLayoutRenderer : LayoutRenderer, IStringValueRenderer
{
  public string File { get; set; }

  public string Dir { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(this.GetStringValue());
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent) => this.GetStringValue();

  private string GetStringValue()
  {
    return PathHelpers.CombinePaths(Directory.GetCurrentDirectory(), this.Dir, this.File);
  }
}
