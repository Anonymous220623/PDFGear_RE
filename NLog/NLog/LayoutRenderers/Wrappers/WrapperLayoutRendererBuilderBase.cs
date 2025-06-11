// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.WrapperLayoutRendererBuilderBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

public abstract class WrapperLayoutRendererBuilderBase : WrapperLayoutRendererBase
{
  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    using (AppendBuilderCreator appendBuilderCreator = new AppendBuilderCreator(builder, true))
    {
      this.RenderFormattedMessage(logEvent, appendBuilderCreator.Builder);
      this.TransformFormattedMesssage(logEvent, appendBuilderCreator.Builder);
    }
  }

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected virtual void TransformFormattedMesssage(LogEventInfo logEvent, StringBuilder target)
  {
    this.TransformFormattedMesssage(target);
  }

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected abstract void TransformFormattedMesssage(StringBuilder target);

  [Obsolete("Inherit from WrapperLayoutRendererBase and override RenderInnerAndTransform() instead. Marked obsolete in NLog 4.6")]
  protected virtual void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
  {
    this.Inner.RenderAppendBuilder(logEvent, target);
  }

  protected sealed override string Transform(string text)
  {
    throw new NotSupportedException("Use TransformFormattedMesssage");
  }

  protected sealed override string RenderInner(LogEventInfo logEvent)
  {
    throw new NotSupportedException("Use RenderFormattedMessage");
  }
}
