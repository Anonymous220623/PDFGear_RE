// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.EnvironmentUserLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("environment-user")]
[ThreadSafe]
public class EnvironmentUserLayoutRenderer : LayoutRenderer, IStringValueRenderer
{
  public EnvironmentUserLayoutRenderer()
  {
    this.UserName = true;
    this.Domain = false;
  }

  [DefaultValue(true)]
  public bool UserName { get; set; }

  [DefaultValue(false)]
  public bool Domain { get; set; }

  [DefaultValue("UserUnknown")]
  public string DefaultUser { get; set; } = "UserUnknown";

  [DefaultValue("DomainUnknown")]
  public string DefaultDomain { get; set; } = "DomainUnknown";

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(this.GetStringValue());
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent) => this.GetStringValue();

  private string GetStringValue()
  {
    return this.UserName ? (!this.Domain ? this.GetUserName() : $"{this.GetDomainName()}\\{this.GetUserName()}") : (!this.Domain ? string.Empty : this.GetDomainName());
  }

  private string GetUserName()
  {
    return this.GetValueSafe((Func<string>) (() => Environment.UserName), this.DefaultUser);
  }

  private string GetDomainName()
  {
    return this.GetValueSafe((Func<string>) (() => Environment.UserDomainName), this.DefaultDomain);
  }

  private string GetValueSafe(Func<string> getValue, string defaultValue)
  {
    try
    {
      string str = getValue();
      return string.IsNullOrEmpty(str) ? defaultValue ?? string.Empty : str;
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]
      {
        (object) defaultValue
      };
      InternalLogger.Warn(ex, "Failed to lookup Environment-User. Fallback value={0}", objArray);
      return defaultValue ?? string.Empty;
    }
  }
}
