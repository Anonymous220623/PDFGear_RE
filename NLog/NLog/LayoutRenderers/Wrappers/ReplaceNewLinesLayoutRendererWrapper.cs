// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.ReplaceNewLinesLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("replace-newlines")]
[AmbientProperty("ReplaceNewLines")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class ReplaceNewLinesLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
{
  private const string WindowsNewLine = "\r\n";
  private const string UnixNewLine = "\n";

  public ReplaceNewLinesLayoutRendererWrapper() => this.Replacement = " ";

  [DefaultValue(" ")]
  public string Replacement { get; set; }

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    this.Inner.RenderAppendBuilder(logEvent, builder);
    if (builder.Length <= orgLength || builder.IndexOf('\n', orgLength) < 0)
      return;
    string str = builder.ToString(orgLength, builder.Length - orgLength).Replace("\r\n", this.Replacement).Replace("\n", this.Replacement);
    builder.Length = orgLength;
    builder.Append(str);
  }

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected override void TransformFormattedMesssage(StringBuilder target)
  {
  }
}
