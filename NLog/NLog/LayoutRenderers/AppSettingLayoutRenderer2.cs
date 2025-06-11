// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.AppSettingLayoutRenderer2
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("appsetting")]
[ThreadAgnostic]
[ThreadSafe]
public sealed class AppSettingLayoutRenderer2 : LayoutRenderer, IStringValueRenderer
{
  private string _connectionStringName;

  [RequiredParameter]
  [DefaultParameter]
  public string Item { get; set; }

  [Obsolete("Allows easier conversion from NLog.Extended. Instead use Item-property")]
  public string Name
  {
    get => this.Item;
    set => this.Item = value;
  }

  public string Default { get; set; }

  internal IConfigurationManager2 ConfigurationManager { get; set; } = (IConfigurationManager2) new ConfigurationManager2();

  protected override void InitializeLayoutRenderer()
  {
    string str1 = "ConnectionStrings.";
    string str2 = this.Item;
    this._connectionStringName = (str2 != null ? (str2.TrimStart().StartsWith(str1, StringComparison.OrdinalIgnoreCase) ? 1 : 0) : 0) != 0 ? this.Item.TrimStart().Substring(str1.Length) : (string) null;
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(this.GetStringValue());
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent) => this.GetStringValue();

  private string GetStringValue()
  {
    if (string.IsNullOrEmpty(this.Item))
      return this.Default;
    string str = this._connectionStringName != null ? this.ConfigurationManager.LookupConnectionString(this._connectionStringName)?.ConnectionString : this.ConfigurationManager.AppSettings[this.Item];
    if (str == null && this.Default != null)
      str = this.Default;
    return str ?? string.Empty;
  }
}
