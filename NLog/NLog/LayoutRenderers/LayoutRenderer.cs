// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.LayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[NLogConfigurationItem]
public abstract class LayoutRenderer : ISupportsInitialize, IRenderable, IDisposable
{
  private const int MaxInitialRenderBufferLength = 16384 /*0x4000*/;
  private int _maxRenderedLength;
  private bool _isInitialized;

  protected LoggingConfiguration LoggingConfiguration { get; private set; }

  public override string ToString()
  {
    LayoutRendererAttribute customAttribute = this.GetType().GetCustomAttribute<LayoutRendererAttribute>();
    return customAttribute != null ? $"Layout Renderer: ${{{customAttribute.Name}}}" : this.GetType().Name;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public string Render(LogEventInfo logEvent)
  {
    int capacity = this._maxRenderedLength;
    if (capacity > 16384 /*0x4000*/)
      capacity = 16384 /*0x4000*/;
    StringBuilder builder = new StringBuilder(capacity);
    this.RenderAppendBuilder(logEvent, builder);
    if (builder.Length > this._maxRenderedLength)
      this._maxRenderedLength = builder.Length;
    return builder.ToString();
  }

  void ISupportsInitialize.Initialize(LoggingConfiguration configuration)
  {
    this.Initialize(configuration);
  }

  void ISupportsInitialize.Close() => this.Close();

  internal void Initialize(LoggingConfiguration configuration)
  {
    if (this.LoggingConfiguration == null)
      this.LoggingConfiguration = configuration;
    if (this._isInitialized)
      return;
    this._isInitialized = true;
    PropertyHelper.CheckRequiredParameters((object) this);
    this.InitializeLayoutRenderer();
  }

  internal void Close()
  {
    if (!this._isInitialized)
      return;
    this.LoggingConfiguration = (LoggingConfiguration) null;
    this._isInitialized = false;
    this.CloseLayoutRenderer();
  }

  internal void RenderAppendBuilder(LogEventInfo logEvent, StringBuilder builder)
  {
    if (!this._isInitialized)
    {
      this._isInitialized = true;
      this.InitializeLayoutRenderer();
    }
    try
    {
      this.Append(builder, logEvent);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "Exception in layout renderer.");
      if (!ex.MustBeRethrown())
        return;
      throw;
    }
  }

  protected abstract void Append(StringBuilder builder, LogEventInfo logEvent);

  protected virtual void InitializeLayoutRenderer()
  {
  }

  protected virtual void CloseLayoutRenderer()
  {
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.Close();
  }

  protected IFormatProvider GetFormatProvider(LogEventInfo logEvent, IFormatProvider layoutCulture = null)
  {
    IFormatProvider formatProvider1 = logEvent.FormatProvider;
    if (formatProvider1 != null)
      return formatProvider1;
    IFormatProvider formatProvider2 = layoutCulture;
    if (formatProvider2 != null)
      return formatProvider2;
    LoggingConfiguration loggingConfiguration = this.LoggingConfiguration;
    return loggingConfiguration == null ? (IFormatProvider) null : (IFormatProvider) loggingConfiguration.DefaultCultureInfo;
  }

  protected CultureInfo GetCulture(LogEventInfo logEvent, CultureInfo layoutCulture = null)
  {
    if (logEvent.FormatProvider is CultureInfo formatProvider)
      return formatProvider;
    CultureInfo culture = layoutCulture;
    if (culture != null)
      return culture;
    return this.LoggingConfiguration?.DefaultCultureInfo;
  }

  public static void Register<T>(string name) where T : LayoutRenderer
  {
    Type layoutRendererType = typeof (T);
    LayoutRenderer.Register(name, layoutRendererType);
  }

  public static void Register(string name, Type layoutRendererType)
  {
    ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition(name, layoutRendererType);
  }

  public static void Register(string name, Func<LogEventInfo, object> func)
  {
    LayoutRenderer.Register(name, (Func<LogEventInfo, LoggingConfiguration, object>) ((info, configuration) => func(info)));
  }

  public static void Register(
    string name,
    Func<LogEventInfo, LoggingConfiguration, object> func)
  {
    LayoutRenderer.Register(new FuncLayoutRenderer(name, func));
  }

  public static void Register(FuncLayoutRenderer layoutRenderer)
  {
    ConfigurationItemFactory.Default.GetLayoutRenderers().RegisterFuncLayout(layoutRenderer.LayoutRendererName, layoutRenderer);
  }
}
