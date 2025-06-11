// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DatabaseObjectPropertyInfo
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace NLog.Targets;

[NLogConfigurationItem]
public class DatabaseObjectPropertyInfo
{
  private DatabaseObjectPropertyInfo.PropertySetterCacheItem _propertySetter;

  [RequiredParameter]
  public string Name { get; set; }

  [RequiredParameter]
  public Layout Layout { get; set; }

  [DefaultValue(typeof (string))]
  public Type PropertyType { get; set; } = typeof (string);

  [DefaultValue(null)]
  public string Format { get; set; }

  [DefaultValue(null)]
  public CultureInfo Culture { get; set; }

  internal bool SetPropertyValue(object dbObject, object propertyValue)
  {
    Type type = dbObject.GetType();
    DatabaseObjectPropertyInfo.PropertySetterCacheItem propertySetterCacheItem = this._propertySetter;
    if (!propertySetterCacheItem.Equals(this.Name, type))
    {
      PropertySetter propertySetter = PropertySetter.CreatePropertySetter(type, this.Name);
      propertySetterCacheItem = new DatabaseObjectPropertyInfo.PropertySetterCacheItem(this.Name, type, propertySetter);
      this._propertySetter = propertySetterCacheItem;
    }
    PropertySetter propertySetter1 = propertySetterCacheItem.PropertySetter;
    return propertySetter1 != null && propertySetter1.SetPropertyValue(dbObject, propertyValue);
  }

  private struct PropertySetterCacheItem(
    string propertyName,
    Type objectType,
    PropertySetter propertySetter)
  {
    public string PropertyName { get; } = propertyName;

    public Type ObjectType { get; } = objectType;

    public PropertySetter PropertySetter { get; } = propertySetter;

    public bool Equals(string propertyName, Type objectType)
    {
      return this.PropertyName == propertyName && this.ObjectType == objectType;
    }
  }
}
