// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.WhenLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Conditions;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("when")]
[AmbientProperty("When")]
[ThreadAgnostic]
[ThreadSafe]
public sealed class WhenLayoutRendererWrapper : WrapperLayoutRendererBuilderBase, IRawValue
{
  [RequiredParameter]
  public ConditionExpression When { get; set; }

  public Layout Else { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    int length = builder.Length;
    try
    {
      if (this.ShouldRenderInner(logEvent))
        this.Inner?.RenderAppendBuilder(logEvent, builder);
      else
        this.Else?.RenderAppendBuilder(logEvent, builder);
    }
    catch
    {
      builder.Length = length;
      throw;
    }
  }

  private bool ShouldRenderInner(LogEventInfo logEvent)
  {
    return this.When == null || true.Equals(this.When.Evaluate(logEvent));
  }

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected override void TransformFormattedMesssage(StringBuilder target)
  {
  }

  public bool TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    return this.ShouldRenderInner(logEvent) ? WhenLayoutRendererWrapper.TryGetRawValueFromLayout(logEvent, this.Inner, out value) : WhenLayoutRendererWrapper.TryGetRawValueFromLayout(logEvent, this.Else, out value);
  }

  private static bool TryGetRawValueFromLayout(
    LogEventInfo logEvent,
    Layout layout,
    out object value)
  {
    if (layout != null)
      return layout.TryGetRawValue(logEvent, out value);
    value = (object) null;
    return false;
  }
}
