// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.AllEventPropertiesLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("all-event-properties")]
[ThreadAgnostic]
[ThreadSafe]
[MutableUnsafe]
public class AllEventPropertiesLayoutRenderer : LayoutRenderer
{
  private string _format;
  private string _beforeKey;
  private string _afterKey;
  private string _afterValue;
  private static string[] CallerInformationAttributeNames = new string[3]
  {
    "CallerMemberName",
    "CallerFilePath",
    "CallerLineNumber"
  };

  public AllEventPropertiesLayoutRenderer()
  {
    this.Separator = ", ";
    this.Format = "[key]=[value]";
    this.Exclude = (ISet<string>) new HashSet<string>((IEnumerable<string>) AllEventPropertiesLayoutRenderer.CallerInformationAttributeNames, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  }

  public string Separator { get; set; }

  [DefaultValue(false)]
  public bool IncludeEmptyValues { get; set; }

  public ISet<string> Exclude { get; set; }

  [Obsolete("Instead use the Exclude-property. Marked obsolete on NLog 4.6.8")]
  [DefaultValue(false)]
  public bool IncludeCallerInformation
  {
    get
    {
      ISet<string> exclude = this.Exclude;
      return exclude == null || !exclude.Contains(AllEventPropertiesLayoutRenderer.CallerInformationAttributeNames[0]);
    }
    set
    {
      if (!value)
      {
        if (this.Exclude == null)
        {
          this.Exclude = (ISet<string>) new HashSet<string>((IEnumerable<string>) AllEventPropertiesLayoutRenderer.CallerInformationAttributeNames, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        }
        else
        {
          foreach (string informationAttributeName in AllEventPropertiesLayoutRenderer.CallerInformationAttributeNames)
            this.Exclude.Add(informationAttributeName);
        }
      }
      else
      {
        ISet<string> exclude = this.Exclude;
        if ((exclude != null ? (exclude.Count > 0 ? 1 : 0) : 0) == 0)
          return;
        foreach (string informationAttributeName in AllEventPropertiesLayoutRenderer.CallerInformationAttributeNames)
          this.Exclude.Remove(informationAttributeName);
      }
    }
  }

  public string Format
  {
    get => this._format;
    set
    {
      if (!value.Contains("[key]"))
        throw new ArgumentException("Invalid format: [key] placeholder is missing.");
      this._format = value.Contains("[value]") ? value : throw new ArgumentException("Invalid format: [value] placeholder is missing.");
      string[] strArray = this._format.Split(new string[2]
      {
        "[key]",
        "[value]"
      }, StringSplitOptions.None);
      if (strArray.Length == 3)
      {
        this._beforeKey = strArray[0];
        this._afterKey = strArray[1];
        this._afterValue = strArray[2];
      }
      else
      {
        this._beforeKey = (string) null;
        this._afterKey = (string) null;
        this._afterValue = (string) null;
      }
    }
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    if (!logEvent.HasProperties)
      return;
    IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
    bool flag1 = this.CheckForExclude(logEvent);
    bool flag2 = this._beforeKey == null || this._afterKey == null || this._afterValue == null;
    bool flag3 = true;
    foreach (KeyValuePair<object, object> property in (IEnumerable<KeyValuePair<object, object>>) logEvent.Properties)
    {
      if ((this.IncludeEmptyValues || !AllEventPropertiesLayoutRenderer.IsEmptyPropertyValue(property.Value)) && (!flag1 || !(property.Key is string key) || !this.Exclude.Contains(key)))
      {
        if (!flag3)
          builder.Append(this.Separator);
        flag3 = false;
        if (flag2)
        {
          string newValue1 = Convert.ToString(property.Key, formatProvider);
          string newValue2 = Convert.ToString(property.Value, formatProvider);
          string str = this.Format.Replace("[key]", newValue1).Replace("[value]", newValue2);
          builder.Append(str);
        }
        else
        {
          builder.Append(this._beforeKey);
          builder.AppendFormattedValue(property.Key, (string) null, formatProvider);
          builder.Append(this._afterKey);
          builder.AppendFormattedValue(property.Value, (string) null, formatProvider);
          builder.Append(this._afterValue);
        }
      }
    }
  }

  private bool CheckForExclude(LogEventInfo logEvent)
  {
    ISet<string> exclude = this.Exclude;
    bool flag = exclude != null && exclude.Count > 0;
    if (flag)
      flag = logEvent.CallSiteInformation != null || this.Exclude.Count != AllEventPropertiesLayoutRenderer.CallerInformationAttributeNames.Length || !this.Exclude.Contains(AllEventPropertiesLayoutRenderer.CallerInformationAttributeNames[0]);
    return flag;
  }

  private static bool IsEmptyPropertyValue(object value)
  {
    return value is string str ? string.IsNullOrEmpty(str) : value == null;
  }
}
