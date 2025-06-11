// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.Layout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

#nullable disable
namespace NLog.Layouts;

[NLogConfigurationItem]
public abstract class Layout : ISupportsInitialize, IRenderable
{
  internal bool IsInitialized;
  private bool _scannedForObjects;
  private const int MaxInitialRenderBufferLength = 16384 /*0x4000*/;
  private int _maxRenderedLength;

  internal bool ThreadAgnostic { get; set; }

  internal bool ThreadSafe { get; set; }

  internal bool MutableUnsafe { get; set; }

  internal StackTraceUsage StackTraceUsage { get; private set; }

  protected LoggingConfiguration LoggingConfiguration { get; private set; }

  public static implicit operator Layout([Localizable(false)] string text)
  {
    return Layout.FromString(text, ConfigurationItemFactory.Default);
  }

  public static Layout FromString(string layoutText)
  {
    return Layout.FromString(layoutText, ConfigurationItemFactory.Default);
  }

  public static Layout FromString(
    string layoutText,
    ConfigurationItemFactory configurationItemFactory)
  {
    return (Layout) new SimpleLayout(layoutText, configurationItemFactory);
  }

  public static Layout FromString(string layoutText, bool throwConfigExceptions)
  {
    try
    {
      return (Layout) new SimpleLayout(layoutText, ConfigurationItemFactory.Default, new bool?(throwConfigExceptions));
    }
    catch (NLogConfigurationException ex)
    {
      throw;
    }
    catch (Exception ex)
    {
      if (throwConfigExceptions && !ex.MustBeRethrownImmediately())
        throw new NLogConfigurationException("Invalid Layout: " + layoutText, ex);
      throw;
    }
  }

  public static Layout FromMethod(
    Func<LogEventInfo, object> layoutMethod,
    LayoutRenderOptions options = LayoutRenderOptions.None)
  {
    if (layoutMethod == null)
      throw new ArgumentNullException(nameof (layoutMethod));
    string name = $"{layoutMethod.Method?.DeclaringType?.ToString()}.{layoutMethod.Method?.Name}";
    FuncLayoutRenderer funcLayoutRenderer = Layout.CreateFuncLayoutRenderer(layoutMethod, options, name);
    return (Layout) new SimpleLayout((LayoutRenderer[]) new FuncLayoutRenderer[1]
    {
      funcLayoutRenderer
    }, funcLayoutRenderer.LayoutRendererName, ConfigurationItemFactory.Default);
  }

  private static FuncLayoutRenderer CreateFuncLayoutRenderer(
    Func<LogEventInfo, object> layoutMethod,
    LayoutRenderOptions options,
    string name)
  {
    if ((options & LayoutRenderOptions.ThreadAgnostic) == LayoutRenderOptions.ThreadAgnostic)
      return (FuncLayoutRenderer) new FuncThreadAgnosticLayoutRenderer(name, (Func<LogEventInfo, LoggingConfiguration, object>) ((l, c) => layoutMethod(l)));
    return (options & LayoutRenderOptions.ThreadSafe) != LayoutRenderOptions.None ? (FuncLayoutRenderer) new FuncThreadSafeLayoutRenderer(name, (Func<LogEventInfo, LoggingConfiguration, object>) ((l, c) => layoutMethod(l))) : new FuncLayoutRenderer(name, (Func<LogEventInfo, LoggingConfiguration, object>) ((l, c) => layoutMethod(l)));
  }

  public virtual void Precalculate(LogEventInfo logEvent)
  {
    if (this.ThreadAgnostic && !this.MutableUnsafe)
      return;
    this.Render(logEvent);
  }

  public string Render(LogEventInfo logEvent)
  {
    if (!this.IsInitialized)
      this.Initialize(this.LoggingConfiguration);
    object obj;
    if ((!this.ThreadAgnostic || this.MutableUnsafe) && logEvent.TryGetCachedLayoutValue(this, out obj))
      return obj?.ToString() ?? string.Empty;
    string str = this.GetFormattedMessage(logEvent) ?? string.Empty;
    if (!this.ThreadAgnostic || this.MutableUnsafe)
      logEvent.AddCachedLayoutValue(this, (object) str);
    return str;
  }

  internal virtual void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
  {
    this.Precalculate(logEvent);
  }

  internal void RenderAppendBuilder(
    LogEventInfo logEvent,
    StringBuilder target,
    bool cacheLayoutResult = false)
  {
    if (!this.IsInitialized)
      this.Initialize(this.LoggingConfiguration);
    object obj;
    if ((!this.ThreadAgnostic || this.MutableUnsafe) && logEvent.TryGetCachedLayoutValue(this, out obj))
    {
      target.Append(obj?.ToString() ?? string.Empty);
    }
    else
    {
      cacheLayoutResult = cacheLayoutResult && !this.ThreadAgnostic;
      using (AppendBuilderCreator appendBuilderCreator = new AppendBuilderCreator(target, cacheLayoutResult))
      {
        this.RenderFormattedMessage(logEvent, appendBuilderCreator.Builder);
        if (!cacheLayoutResult)
          return;
        logEvent.AddCachedLayoutValue(this, (object) appendBuilderCreator.Builder.ToString());
      }
    }
  }

  internal string RenderAllocateBuilder(LogEventInfo logEvent, StringBuilder reusableBuilder = null)
  {
    int capacity = this._maxRenderedLength;
    if (capacity > 16384 /*0x4000*/)
      capacity = 16384 /*0x4000*/;
    StringBuilder target = reusableBuilder ?? new StringBuilder(capacity);
    this.RenderFormattedMessage(logEvent, target);
    if (target.Length > this._maxRenderedLength)
      this._maxRenderedLength = target.Length;
    return target.ToString();
  }

  protected virtual void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
  {
    target.Append(this.GetFormattedMessage(logEvent) ?? string.Empty);
  }

  void ISupportsInitialize.Initialize(LoggingConfiguration configuration)
  {
    this.Initialize(configuration);
  }

  void ISupportsInitialize.Close() => this.Close();

  internal void Initialize(LoggingConfiguration configuration)
  {
    if (this.IsInitialized)
      return;
    this.LoggingConfiguration = configuration;
    this.IsInitialized = true;
    this._scannedForObjects = false;
    PropertyHelper.CheckRequiredParameters((object) this);
    this.InitializeLayout();
    if (this._scannedForObjects)
      return;
    InternalLogger.Debug<Type>("{0} Initialized Layout done but not scanned for objects", this.GetType());
    this.PerformObjectScanning();
  }

  internal void PerformObjectScanning()
  {
    List<IRenderable> reachableObjects = ObjectGraphScanner.FindReachableObjects<IRenderable>(true, (object) this);
    HashSet<Type> source = new HashSet<Type>(reachableObjects.Select<IRenderable, Type>((Func<IRenderable, Type>) (o => o.GetType())));
    source.Remove(typeof (SimpleLayout));
    source.Remove(typeof (LiteralLayoutRenderer));
    this.ThreadAgnostic = source.All<Type>((Func<Type, bool>) (t => t.IsDefined(typeof (ThreadAgnosticAttribute), true)));
    this.ThreadSafe = source.All<Type>((Func<Type, bool>) (t => t.IsDefined(typeof (ThreadSafeAttribute), true)));
    this.MutableUnsafe = source.Any<Type>((Func<Type, bool>) (t => t.IsDefined(typeof (MutableUnsafeAttribute), true)));
    if ((this.ThreadAgnostic || !this.MutableUnsafe) && reachableObjects.Count > 1 && source.Count > 0)
    {
      foreach (Layout layout in reachableObjects.OfType<Layout>())
      {
        if (layout != this)
        {
          layout.Initialize(this.LoggingConfiguration);
          this.ThreadAgnostic = layout.ThreadAgnostic && this.ThreadAgnostic;
          this.MutableUnsafe = layout.MutableUnsafe || this.MutableUnsafe;
        }
      }
    }
    this.StackTraceUsage = StackTraceUsage.None;
    this.StackTraceUsage = reachableObjects.OfType<IUsesStackTrace>().DefaultIfEmpty<IUsesStackTrace>().Max<IUsesStackTrace, StackTraceUsage>((Func<IUsesStackTrace, StackTraceUsage>) (item => item == null ? StackTraceUsage.None : item.StackTraceUsage));
    this._scannedForObjects = true;
  }

  internal void Close()
  {
    if (!this.IsInitialized)
      return;
    this.LoggingConfiguration = (LoggingConfiguration) null;
    this.IsInitialized = false;
    this.CloseLayout();
  }

  protected virtual void InitializeLayout() => this.PerformObjectScanning();

  protected virtual void CloseLayout()
  {
  }

  protected abstract string GetFormattedMessage(LogEventInfo logEvent);

  public static void Register<T>(string name) where T : Layout
  {
    Type layoutType = typeof (T);
    Layout.Register(name, layoutType);
  }

  public static void Register(string name, Type layoutType)
  {
    ConfigurationItemFactory.Default.Layouts.RegisterDefinition(name, layoutType);
  }

  internal void PrecalculateBuilderInternal(LogEventInfo logEvent, StringBuilder target)
  {
    if (this.ThreadAgnostic && !this.MutableUnsafe)
      return;
    this.RenderAppendBuilder(logEvent, target, true);
  }

  internal string ToStringWithNestedItems<T>(IList<T> nestedItems, Func<T, string> nextItemToString)
  {
    if (nestedItems == null || nestedItems.Count <= 0)
      return this.ToString();
    string[] array = nestedItems.Select<T, string>((Func<T, string>) (c => nextItemToString(c))).ToArray<string>();
    return $"{this.GetType().Name}={string.Join("|", array)}";
  }

  internal virtual bool TryGetRawValue(LogEventInfo logEvent, out object rawValue)
  {
    rawValue = (object) null;
    return false;
  }
}
