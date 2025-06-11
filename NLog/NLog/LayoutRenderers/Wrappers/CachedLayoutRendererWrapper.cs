// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.Wrappers.CachedLayoutRendererWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.ComponentModel;

#nullable disable
namespace NLog.LayoutRenderers.Wrappers;

[LayoutRenderer("cached")]
[AmbientProperty("Cached")]
[AmbientProperty("ClearCache")]
[AmbientProperty("CachedSeconds")]
[ThreadAgnostic]
[ThreadSafe]
public sealed class CachedLayoutRendererWrapper : WrapperLayoutRendererBase, IStringValueRenderer
{
  private readonly object _lockObject = new object();
  private string _cachedValue;
  private string _renderedCacheKey;
  private DateTime _cachedValueExpires;
  private TimeSpan? _cachedValueTimeout;

  public CachedLayoutRendererWrapper()
  {
    this.Cached = true;
    this.ClearCache = CachedLayoutRendererWrapper.ClearCacheOption.OnInit | CachedLayoutRendererWrapper.ClearCacheOption.OnClose;
  }

  [DefaultValue(true)]
  public bool Cached { get; set; }

  public CachedLayoutRendererWrapper.ClearCacheOption ClearCache { get; set; }

  public Layout CacheKey { get; set; }

  public int CachedSeconds
  {
    get
    {
      ref TimeSpan? local = ref this._cachedValueTimeout;
      return local.HasValue ? (int) local.GetValueOrDefault().TotalSeconds : 0;
    }
    set
    {
      this._cachedValueTimeout = new TimeSpan?(TimeSpan.FromSeconds((double) value));
      TimeSpan? cachedValueTimeout = this._cachedValueTimeout;
      TimeSpan zero = TimeSpan.Zero;
      if ((cachedValueTimeout.HasValue ? (cachedValueTimeout.GetValueOrDefault() > zero ? 1 : 0) : 0) == 0)
        return;
      this.Cached = true;
    }
  }

  protected override void InitializeLayoutRenderer()
  {
    base.InitializeLayoutRenderer();
    if ((this.ClearCache & CachedLayoutRendererWrapper.ClearCacheOption.OnInit) != CachedLayoutRendererWrapper.ClearCacheOption.OnInit)
      return;
    this._cachedValue = (string) null;
  }

  protected override void CloseLayoutRenderer()
  {
    base.CloseLayoutRenderer();
    if ((this.ClearCache & CachedLayoutRendererWrapper.ClearCacheOption.OnClose) != CachedLayoutRendererWrapper.ClearCacheOption.OnClose)
      return;
    this._cachedValue = (string) null;
  }

  protected override string Transform(string text) => text;

  protected override string RenderInner(LogEventInfo logEvent)
  {
    if (!this.Cached)
      return base.RenderInner(logEvent);
    string newCacheKey = this.CacheKey?.Render(logEvent) ?? string.Empty;
    string str = this.LookupValidCachedValue(logEvent, newCacheKey);
    if (str == null)
    {
      lock (this._lockObject)
      {
        str = this.LookupValidCachedValue(logEvent, newCacheKey);
        if (str == null)
        {
          this._cachedValue = str = base.RenderInner(logEvent);
          this._renderedCacheKey = newCacheKey;
          if (this._cachedValueTimeout.HasValue)
            this._cachedValueExpires = logEvent.TimeStamp + this._cachedValueTimeout.Value;
        }
      }
    }
    return str;
  }

  private string LookupValidCachedValue(LogEventInfo logEvent, string newCacheKey)
  {
    if (this._renderedCacheKey != newCacheKey)
      return (string) null;
    return this._cachedValueTimeout.HasValue && logEvent.TimeStamp > this._cachedValueExpires ? (string) null : this._cachedValue;
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return !this.Cached ? (string) null : this.RenderInner(logEvent);
  }

  [Flags]
  public enum ClearCacheOption
  {
    None = 0,
    OnInit = 1,
    OnClose = 2,
  }
}
