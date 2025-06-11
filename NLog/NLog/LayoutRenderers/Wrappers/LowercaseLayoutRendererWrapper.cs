// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.LowercaseLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("lowercase")]
[AmbientProperty("Lowercase")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public sealed class LowercaseLayoutRendererWrapper : WrapperLayoutRendererBuilderBase
{
  public LowercaseLayoutRendererWrapper()
  {
    this.Culture = CultureInfo.InvariantCulture;
    this.Lowercase = true;
  }

  [DefaultValue(true)]
  public bool Lowercase { get; set; }

  public CultureInfo Culture { get; set; }

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    this.Inner.RenderAppendBuilder(logEvent, builder);
    if (!this.Lowercase || builder.Length <= orgLength)
      return;
    this.TransformToLowerCase(builder, orgLength);
  }

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected override void TransformFormattedMesssage(StringBuilder target)
  {
  }

  private void TransformToLowerCase(StringBuilder target, int startPos)
  {
    CultureInfo culture = this.Culture;
    for (int index = startPos; index < target.Length; ++index)
      target[index] = char.ToLower(target[index], culture);
  }
}
