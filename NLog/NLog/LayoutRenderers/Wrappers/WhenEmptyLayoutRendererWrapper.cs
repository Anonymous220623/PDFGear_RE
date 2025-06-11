// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.WhenEmptyLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("whenEmpty")]
[AmbientProperty("WhenEmpty")]
[ThreadAgnostic]
[ThreadSafe]
public sealed class WhenEmptyLayoutRendererWrapper : 
  WrapperLayoutRendererBuilderBase,
  IRawValue,
  IStringValueRenderer
{
  private bool _skipStringValueRenderer;

  [RequiredParameter]
  public Layout WhenEmpty { get; set; }

  protected override void InitializeLayoutRenderer()
  {
    base.InitializeLayoutRenderer();
    this.WhenEmpty?.Initialize(this.LoggingConfiguration);
    this._skipStringValueRenderer = !this.TryGetStringValue(out SimpleLayout _, out SimpleLayout _);
  }

  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    this.Inner.RenderAppendBuilder(logEvent, builder);
    if (builder.Length > orgLength)
      return;
    this.WhenEmpty.RenderAppendBuilder(logEvent, builder);
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    if (this._skipStringValueRenderer)
      return (string) null;
    SimpleLayout innerLayout;
    SimpleLayout whenEmptyLayout;
    if (this.TryGetStringValue(out innerLayout, out whenEmptyLayout))
    {
      string str = innerLayout.Render(logEvent);
      return !string.IsNullOrEmpty(str) ? str : whenEmptyLayout.Render(logEvent);
    }
    this._skipStringValueRenderer = true;
    return (string) null;
  }

  private bool TryGetStringValue(out SimpleLayout innerLayout, out SimpleLayout whenEmptyLayout)
  {
    whenEmptyLayout = this.WhenEmpty as SimpleLayout;
    innerLayout = this.Inner as SimpleLayout;
    return WhenEmptyLayoutRendererWrapper.IsStringLayout(innerLayout) && WhenEmptyLayoutRendererWrapper.IsStringLayout(whenEmptyLayout);
  }

  private static bool IsStringLayout(SimpleLayout innerLayout)
  {
    if (innerLayout == null)
      return false;
    return innerLayout.IsFixedText || innerLayout.IsSimpleStringText;
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    object rawValue;
    if (this.Inner.TryGetRawValue(logEvent, out rawValue))
    {
      if (rawValue != null && !rawValue.Equals((object) string.Empty))
      {
        value = rawValue;
        return true;
      }
    }
    else if (!string.IsNullOrEmpty(this.Inner.Render(logEvent)))
    {
      value = (object) null;
      return false;
    }
    return this.WhenEmpty.TryGetRawValue(logEvent, out value);
  }

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected override void TransformFormattedMesssage(StringBuilder target)
  {
  }
}
