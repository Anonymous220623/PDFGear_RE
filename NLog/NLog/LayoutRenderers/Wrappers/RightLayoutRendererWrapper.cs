// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.RightLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("right")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class RightLayoutRendererWrapper : WrapperLayoutRendererBase
{
  [RequiredParameter]
  public int Length { get; set; }

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    if (this.Length <= 0)
      return;
    this.Inner.RenderAppendBuilder(logEvent, builder);
    if (builder.Length - orgLength <= this.Length)
      return;
    string str = builder.ToString(builder.Length - this.Length, this.Length);
    builder.Length = orgLength;
    builder.Append(str);
  }

  protected override string Transform(string text) => throw new NotSupportedException();
}
