// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.TrimWhiteSpaceLayoutRendererWrapper
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

[LayoutRenderer("trim-whitespace")]
[AmbientProperty("TrimWhiteSpace")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class TrimWhiteSpaceLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
{
  public TrimWhiteSpaceLayoutRendererWrapper() => this.TrimWhiteSpace = true;

  [DefaultValue(true)]
  public bool TrimWhiteSpace { get; set; }

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    this.Inner.RenderAppendBuilder(logEvent, builder);
    if (!this.TrimWhiteSpace || builder.Length <= orgLength)
      return;
    TrimWhiteSpaceLayoutRendererWrapper.TransformTrimWhiteSpaces(builder, orgLength);
  }

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected override void TransformFormattedMesssage(StringBuilder target)
  {
  }

  private static void TransformTrimWhiteSpaces(StringBuilder builder, int startPos)
  {
    builder.TrimRight(startPos);
    if (builder.Length <= startPos || !char.IsWhiteSpace(builder[startPos]))
      return;
    string str = builder.ToString(startPos, builder.Length - startPos);
    builder.Length = startPos;
    builder.Append(str.Trim());
  }
}
