// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.NoRawValueLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("norawvalue")]
[AmbientProperty("NoRawValue")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class NoRawValueLayoutRendererWrapper : WrapperLayoutRendererBase
{
  [DefaultValue(true)]
  public bool NoRawValue { get; set; } = true;

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    this.Inner?.RenderAppendBuilder(logEvent, builder);
  }

  protected override string Transform(string text) => throw new NotSupportedException();
}
