// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.LeftLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("left")]
[AmbientProperty("Truncate")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class LeftLayoutRendererWrapper : WrapperLayoutRendererBase
{
  [RequiredParameter]
  public int Length { get; set; }

  public int Truncate
  {
    get => this.Length;
    set => this.Length = value;
  }

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
    builder.Length = orgLength + this.Length;
  }

  protected override string Transform(string text) => throw new NotSupportedException();
}
