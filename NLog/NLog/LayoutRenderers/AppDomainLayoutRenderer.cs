// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.AppDomainLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal.Fakeables;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("appdomain")]
[AppDomainFixedOutput]
[ThreadAgnostic]
[ThreadSafe]
public class AppDomainLayoutRenderer : LayoutRenderer
{
  private const string ShortFormat = "{0:00}";
  private const string LongFormat = "{0:0000}:{1}";
  private const string LongFormatCode = "Long";
  private const string ShortFormatCode = "Short";
  private readonly IAppDomain _currentDomain;
  private string _assemblyName;

  public AppDomainLayoutRenderer()
    : this(LogFactory.CurrentAppDomain)
  {
  }

  public AppDomainLayoutRenderer(IAppDomain currentDomain)
  {
    this._currentDomain = currentDomain;
    this.Format = "Long";
  }

  [DefaultParameter]
  [DefaultValue("Long")]
  public string Format { get; set; }

  protected override void InitializeLayoutRenderer()
  {
    this._assemblyName = (string) null;
    base.InitializeLayoutRenderer();
  }

  protected override void CloseLayoutRenderer()
  {
    this._assemblyName = (string) null;
    base.CloseLayoutRenderer();
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    if (this._assemblyName == null)
      this._assemblyName = string.Format(AppDomainLayoutRenderer.GetFormattingString(this.Format), (object) this._currentDomain.Id, (object) this._currentDomain.FriendlyName);
    builder.Append(this._assemblyName);
  }

  private static string GetFormattingString(string format)
  {
    return !format.Equals("Long", StringComparison.OrdinalIgnoreCase) ? (!format.Equals("Short", StringComparison.OrdinalIgnoreCase) ? format : "{0:00}") : "{0:0000}:{1}";
  }
}
