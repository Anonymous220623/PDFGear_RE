// Decompiled with JetBrains decompiler
// Type: NLog.Layouts.SimpleLayout
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

#nullable disable
namespace NLog.Layouts;

[Layout("SimpleLayout")]
[NLog.Config.ThreadAgnostic]
[NLog.Config.ThreadSafe]
[AppDomainFixedOutput]
public class SimpleLayout : Layout, IUsesStackTrace
{
  private string _fixedText;
  private string _layoutText;
  private IRawValue _rawValueRenderer;
  private IStringValueRenderer _stringValueRenderer;
  private readonly ConfigurationItemFactory _configurationItemFactory;

  public SimpleLayout()
    : this(string.Empty)
  {
  }

  public SimpleLayout(string txt)
    : this(txt, ConfigurationItemFactory.Default)
  {
  }

  public SimpleLayout(string txt, ConfigurationItemFactory configurationItemFactory)
    : this(txt, configurationItemFactory, new bool?())
  {
  }

  internal SimpleLayout(
    string txt,
    ConfigurationItemFactory configurationItemFactory,
    bool? throwConfigExceptions)
  {
    this._configurationItemFactory = configurationItemFactory;
    this.SetLayoutText(txt, throwConfigExceptions);
  }

  internal SimpleLayout(
    LayoutRenderer[] renderers,
    string text,
    ConfigurationItemFactory configurationItemFactory)
  {
    this._configurationItemFactory = configurationItemFactory;
    this.OriginalText = text;
    this.SetRenderers(renderers, text);
  }

  public string OriginalText { get; private set; }

  public string Text
  {
    get => this._layoutText;
    set => this.SetLayoutText(value);
  }

  private void SetLayoutText(string value, bool? throwConfigExceptions = null)
  {
    this.OriginalText = value;
    LayoutRenderer[] renderers;
    string text;
    if (value == null)
    {
      renderers = ArrayHelper.Empty<LayoutRenderer>();
      text = string.Empty;
    }
    else
      renderers = LayoutParser.CompileLayout(this._configurationItemFactory, new SimpleStringReader(value), throwConfigExceptions, false, out text);
    this.SetRenderers(renderers, text);
  }

  public bool IsFixedText => this._fixedText != null;

  public string FixedText => this._fixedText;

  internal bool IsSimpleStringText => this._stringValueRenderer != null;

  public ReadOnlyCollection<LayoutRenderer> Renderers { get; private set; }

  public new StackTraceUsage StackTraceUsage => base.StackTraceUsage;

  public static implicit operator SimpleLayout(string text)
  {
    return text == null ? (SimpleLayout) null : new SimpleLayout(text);
  }

  public static string Escape(string text) => text.Replace("${", "${literal:text=${}");

  public static string Evaluate(string text, LogEventInfo logEvent)
  {
    return new SimpleLayout(text).Render(logEvent);
  }

  public static string Evaluate(string text)
  {
    return SimpleLayout.Evaluate(text, LogEventInfo.CreateNullEvent());
  }

  public override string ToString()
  {
    if (string.IsNullOrEmpty(this.Text))
    {
      ReadOnlyCollection<LayoutRenderer> renderers = this.Renderers;
      // ISSUE: explicit non-virtual call
      if ((renderers != null ? (__nonvirtual (renderers.Count) > 0 ? 1 : 0) : 0) != 0)
        return this.ToStringWithNestedItems<LayoutRenderer>((IList<LayoutRenderer>) this.Renderers, (Func<LayoutRenderer, string>) (r => r.ToString()));
    }
    return $"'{this.Text}'";
  }

  internal void SetRenderers(LayoutRenderer[] renderers, string text)
  {
    this.Renderers = new ReadOnlyCollection<LayoutRenderer>((IList<LayoutRenderer>) renderers);
    this._fixedText = (string) null;
    this._rawValueRenderer = (IRawValue) null;
    this._stringValueRenderer = (IStringValueRenderer) null;
    if (this.Renderers.Count == 0)
      this._fixedText = string.Empty;
    else if (this.Renderers.Count == 1)
    {
      if (this.Renderers[0] is LiteralLayoutRenderer renderer)
      {
        this._fixedText = renderer.Text;
      }
      else
      {
        if (this.Renderers[0] is IRawValue renderer1)
          this._rawValueRenderer = renderer1;
        if (this.Renderers[0] is IStringValueRenderer renderer2)
          this._stringValueRenderer = renderer2;
      }
    }
    this._layoutText = text;
    if (this.LoggingConfiguration == null)
      return;
    this.PerformObjectScanning();
  }

  protected override void InitializeLayout()
  {
    for (int index = 0; index < this.Renderers.Count; ++index)
    {
      LayoutRenderer renderer = this.Renderers[index];
      try
      {
        renderer.Initialize(this.LoggingConfiguration);
      }
      catch (Exception ex)
      {
        if (InternalLogger.IsWarnEnabled || InternalLogger.IsErrorEnabled)
          InternalLogger.Warn(ex, "Exception in '{0}.InitializeLayout()'", (object) renderer.GetType().FullName);
        if (ex.MustBeRethrown())
          throw;
      }
    }
    base.InitializeLayout();
  }

  public override void Precalculate(LogEventInfo logEvent)
  {
    if (this._rawValueRenderer != null)
    {
      try
      {
        if (!this.IsInitialized)
          this.Initialize(this.LoggingConfiguration);
        if (this.ThreadAgnostic)
        {
          if (this.MutableUnsafe)
          {
            object obj;
            if (this._rawValueRenderer.TryGetRawValue(logEvent, out obj))
            {
              if (obj != null)
              {
                if (Convert.GetTypeCode(obj) != TypeCode.Object)
                  return;
                if (obj.GetType().IsValueType())
                  return;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        if (InternalLogger.IsWarnEnabled || InternalLogger.IsErrorEnabled)
          InternalLogger.Warn(ex, "Exception in precalculate using '{0}.TryGetRawValue()'", (object) this._rawValueRenderer?.GetType());
        if (ex.MustBeRethrown())
          throw;
      }
    }
    base.Precalculate(logEvent);
  }

  internal override void PrecalculateBuilder(LogEventInfo logEvent, StringBuilder target)
  {
    this.PrecalculateBuilderInternal(logEvent, target);
  }

  internal override bool TryGetRawValue(LogEventInfo logEvent, out object rawValue)
  {
    if (this._rawValueRenderer != null)
    {
      try
      {
        if (!this.IsInitialized)
          this.Initialize(this.LoggingConfiguration);
        if (this.ThreadAgnostic && !this.MutableUnsafe || !logEvent.TryGetCachedLayoutValue((Layout) this, out object _))
          return this._rawValueRenderer.TryGetRawValue(logEvent, out rawValue);
        rawValue = (object) null;
        return false;
      }
      catch (Exception ex)
      {
        if (InternalLogger.IsWarnEnabled || InternalLogger.IsErrorEnabled)
          InternalLogger.Warn(ex, "Exception in TryGetRawValue using '{0}.TryGetRawValue()'", (object) this._rawValueRenderer?.GetType());
        if (ex.MustBeRethrown())
          throw;
      }
    }
    rawValue = (object) null;
    return false;
  }

  protected override string GetFormattedMessage(LogEventInfo logEvent)
  {
    if (this.IsFixedText)
      return this._fixedText;
    if (this._stringValueRenderer != null)
    {
      try
      {
        string formattedString = this._stringValueRenderer.GetFormattedString(logEvent);
        if (formattedString != null)
          return formattedString;
        this._stringValueRenderer = (IStringValueRenderer) null;
      }
      catch (Exception ex)
      {
        if (InternalLogger.IsWarnEnabled || InternalLogger.IsErrorEnabled)
          InternalLogger.Warn(ex, "Exception in '{0}.Append()'", (object) this._stringValueRenderer?.GetType().FullName);
        if (ex.MustBeRethrown())
          throw;
      }
    }
    return this.RenderAllocateBuilder(logEvent);
  }

  private void RenderAllRenderers(LogEventInfo logEvent, StringBuilder target)
  {
    for (int index = 0; index < this.Renderers.Count; ++index)
    {
      LayoutRenderer renderer = this.Renderers[index];
      try
      {
        renderer.RenderAppendBuilder(logEvent, target);
      }
      catch (Exception ex)
      {
        if (InternalLogger.IsWarnEnabled || InternalLogger.IsErrorEnabled)
          InternalLogger.Warn(ex, "Exception in '{0}.Append()'", (object) renderer.GetType().FullName);
        if (ex.MustBeRethrown())
          throw;
      }
    }
  }

  protected override void RenderFormattedMessage(LogEventInfo logEvent, StringBuilder target)
  {
    if (this.IsFixedText)
      target.Append(this._fixedText);
    else
      this.RenderAllRenderers(logEvent, target);
  }
}
