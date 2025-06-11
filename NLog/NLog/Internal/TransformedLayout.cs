// Decompiled with JetBrains decompiler
// Type: NLog.Internal.TransformedLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Config;
using NLog.Layouts;
using System;

#nullable disable
namespace NLog.Internal;

internal class TransformedLayout : IRenderable, ISupportsInitialize
{
  private readonly string _fixedValue;
  [CanBeNull]
  private readonly Func<Layout, LogEventInfo, string> _renderLogEvent;
  private readonly Func<string, string> _transformation;

  private TransformedLayout(
    [NotNull] Layout layout,
    [NotNull] Func<string, string> transformation,
    [CanBeNull] Func<Layout, LogEventInfo, string> renderLogEvent)
  {
    this._transformation = transformation ?? throw new ArgumentNullException(nameof (transformation));
    this._renderLogEvent = renderLogEvent;
    this.Layout = layout ?? throw new ArgumentNullException(nameof (layout));
    if (!(layout is SimpleLayout simpleLayout) || !simpleLayout.IsFixedText)
      return;
    this._fixedValue = transformation(simpleLayout.FixedText);
  }

  public Layout Layout { get; }

  public string Render(LogEventInfo logEvent)
  {
    return this._fixedValue != null ? this._fixedValue : this._transformation(this._renderLogEvent != null ? this._renderLogEvent(this.Layout, logEvent) : this.Layout.Render(logEvent));
  }

  public static TransformedLayout Create(
    [CanBeNull] Layout layout,
    [NotNull] Func<string, string> transformation,
    [CanBeNull] Func<Layout, LogEventInfo, string> renderLogEvent)
  {
    if (transformation == null)
      throw new ArgumentNullException(nameof (transformation));
    return layout == null ? (TransformedLayout) null : new TransformedLayout(layout, transformation, renderLogEvent);
  }

  public void Initialize(LoggingConfiguration configuration)
  {
    this.Layout.Initialize(configuration);
  }

  public void Close() => this.Layout.Close();
}
