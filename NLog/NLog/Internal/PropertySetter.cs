// Decompiled with JetBrains decompiler
// Type: NLog.Internal.PropertySetter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Reflection;

#nullable disable
namespace NLog.Internal;

internal class PropertySetter
{
  private readonly Type _objectType;
  private readonly PropertyInfo _propertyInfo;
  private ReflectionHelpers.LateBoundMethodSingle _propertySetter;

  public static PropertySetter CreatePropertySetter(Type objectType, string propertyName)
  {
    PropertyInfo property;
    return PropertySetter.TryGetProperty(objectType, propertyName, out property) ? new PropertySetter(objectType, property) : (PropertySetter) null;
  }

  private PropertySetter(Type objectType, PropertyInfo propertyInfo)
  {
    this._objectType = objectType;
    this._propertyInfo = propertyInfo;
  }

  public bool SetPropertyValue(object instance, object value)
  {
    if (instance == null)
      throw new ArgumentNullException(nameof (instance));
    if (this._propertySetter != null)
    {
      object obj = this._propertySetter(instance, value);
    }
    else
    {
      MethodInfo setMethod = this._propertyInfo.GetSetMethod();
      setMethod.Invoke(instance, new object[1]{ value });
      this._propertySetter = ReflectionHelpers.CreateLateBoundMethodSingle(setMethod);
    }
    return true;
  }

  private static bool TryGetProperty(
    Type objectType,
    string propertyName,
    out PropertyInfo property)
  {
    if (objectType == (Type) null)
      throw new ArgumentNullException(nameof (objectType));
    property = propertyName != null ? objectType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public) : throw new ArgumentNullException(nameof (propertyName));
    if (property == (PropertyInfo) null)
    {
      InternalLogger.Warn<string, string>("Cannot set {0} on type {1}, property not found", propertyName, objectType.FullName);
      return false;
    }
    if (property.CanWrite)
      return true;
    InternalLogger.Warn<string, string>("Cannot set {0} on type {1}, property not settable", propertyName, objectType.FullName);
    return false;
  }
}
