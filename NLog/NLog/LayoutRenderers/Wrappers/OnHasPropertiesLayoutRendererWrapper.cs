// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.OnHasPropertiesLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("onhasproperties")]
[ThreadAgnostic]
[ThreadSafe]
public sealed class OnHasPropertiesLayoutRendererWrapper : WrapperLayoutRendererBase
{
  protected override void RenderInnerAndTransform(
    LogEventInfo logEvent,
    StringBuilder builder,
    int orgLength)
  {
    if (!logEvent.HasProperties)
      return;
    this.Inner.RenderAppendBuilder(logEvent, builder);
  }

  protected override string Transform(string text) => text;
}
