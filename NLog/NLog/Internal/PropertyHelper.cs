// Decompiled with JetBrains decompiler
// Type: NLog.Internal.PropertyHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Conditions;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.Internal;

internal static class PropertyHelper
{
  private static Dictionary<Type, Dictionary<string, PropertyInfo>> parameterInfoCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
  private static Dictionary<Type, Func<string, ConfigurationItemFactory, object>> DefaultPropertyConversionMapper = PropertyHelper.BuildPropertyConversionMapper();
  private static readonly RequiredParameterAttribute _requiredParameterAttribute = new RequiredParameterAttribute();
  private static readonly ArrayParameterAttribute _arrayParameterAttribute = new ArrayParameterAttribute((Type) null, string.Empty);
  private static readonly DefaultValueAttribute _defaultValueAttribute = new DefaultValueAttribute(string.Empty);
  private static readonly AdvancedAttribute _advancedAttribute = new AdvancedAttribute();
  private static readonly DefaultParameterAttribute _defaultParameterAttribute = new DefaultParameterAttribute();
  private static readonly NLogConfigurationIgnorePropertyAttribute _ignorePropertyAttribute = new NLogConfigurationIgnorePropertyAttribute();
  private static readonly FlagsAttribute _flagsAttribute = new FlagsAttribute();

  private static Dictionary<Type, Func<string, ConfigurationItemFactory, object>> BuildPropertyConversionMapper()
  {
    return new Dictionary<Type, Func<string, ConfigurationItemFactory, object>>()
    {
      {
        typeof (Layout),
        new Func<string, ConfigurationItemFactory, object>(PropertyHelper.TryParseLayoutValue)
      },
      {
        typeof (SimpleLayout),
        new Func<string, ConfigurationItemFactory, object>(PropertyHelper.TryParseLayoutValue)
      },
      {
        typeof (ConditionExpression),
        new Func<string, ConfigurationItemFactory, object>(PropertyHelper.TryParseConditionValue)
      },
      {
        typeof (Encoding),
        new Func<string, ConfigurationItemFactory, object>(PropertyHelper.TryParseEncodingValue)
      },
      {
        typeof (string),
        (Func<string, ConfigurationItemFactory, object>) ((stringvalue, factory) => (object) stringvalue)
      },
      {
        typeof (int),
        (Func<string, ConfigurationItemFactory, object>) ((stringvalue, factory) => Convert.ChangeType((object) stringvalue.Trim(), TypeCode.Int32, (IFormatProvider) CultureInfo.InvariantCulture))
      },
      {
        typeof (bool),
        (Func<string, ConfigurationItemFactory, object>) ((stringvalue, factory) => Convert.ChangeType((object) stringvalue.Trim(), TypeCode.Boolean, (IFormatProvider) CultureInfo.InvariantCulture))
      },
      {
        typeof (CultureInfo),
        (Func<string, ConfigurationItemFactory, object>) ((stringvalue, factory) => (object) new CultureInfo(stringvalue.Trim()))
      },
      {
        typeof (Type),
        (Func<string, ConfigurationItemFactory, object>) ((stringvalue, factory) => (object) Type.GetType(stringvalue.Trim(), true))
      },
      {
        typeof (LineEndingMode),
        (Func<string, ConfigurationItemFactory, object>) ((stringvalue, factory) => (object) LineEndingMode.FromString(stringvalue.Trim()))
      },
      {
        typeof (Uri),
        (Func<string, ConfigurationItemFactory, object>) ((stringvalue, factory) => (object) new Uri(stringvalue.Trim()))
      }
    };
  }

  internal static void SetPropertyFromString(
    object obj,
    string propertyName,
    string value,
    ConfigurationItemFactory configurationItemFactory)
  {
    Type type1 = obj.GetType();
    InternalLogger.Debug<Type, string, string>("Setting '{0}.{1}' to '{2}'", type1, propertyName, value);
    PropertyInfo result;
    if (!PropertyHelper.TryGetPropertyInfo(type1, propertyName, out result))
      throw new NotSupportedException($"Parameter {propertyName} not supported on {type1.Name}");
    try
    {
      Type propertyType = result.PropertyType;
      object obj1;
      if (!PropertyHelper.TryNLogSpecificConversion(propertyType, value, configurationItemFactory, out obj1))
      {
        if (result.IsDefined(PropertyHelper._arrayParameterAttribute.GetType(), false))
          throw new NotSupportedException($"Parameter {propertyName} of {type1.Name} is an array and cannot be assigned a scalar value.");
        Type type2 = Nullable.GetUnderlyingType(propertyType);
        if ((object) type2 == null)
          type2 = propertyType;
        Type type3 = type2;
        if (!PropertyHelper.TryGetEnumValue(type3, value, out obj1, true) && !PropertyHelper.TryImplicitConversion(type3, value, out obj1) && !PropertyHelper.TryFlatListConversion(obj, result, value, configurationItemFactory, out obj1) && !PropertyHelper.TryTypeConverterConversion(type3, value, out obj1))
          obj1 = Convert.ChangeType((object) value, type3, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      result.SetValue(obj, obj1, (object[]) null);
    }
    catch (TargetInvocationException ex)
    {
      throw new NLogConfigurationException($"Error when setting property '{result.Name}' on {type1.Name}", ex.InnerException);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "Error when setting property '{0}' on '{1}'", (object) result.Name, (object) type1);
      if (!ex.MustBeRethrownImmediately())
        throw new NLogConfigurationException($"Error when setting property '{result.Name}' on {type1.Name}", ex);
      throw;
    }
  }

  internal static bool TryGetPropertyInfo(object obj, string propertyName, out PropertyInfo result)
  {
    return PropertyHelper.TryGetPropertyInfo(obj.GetType(), propertyName, out result);
  }

  internal static Type GetArrayItemType(PropertyInfo propInfo)
  {
    return propInfo.GetCustomAttribute<ArrayParameterAttribute>()?.ItemType;
  }

  internal static bool IsConfigurationItemType(Type type)
  {
    if (type == (Type) null || PropertyHelper.IsSimplePropertyType(type))
      return false;
    return typeof (LayoutRenderer).IsAssignableFrom(type) || typeof (Layout).IsAssignableFrom(type) || PropertyHelper.TryLookupConfigItemProperties(type) != null;
  }

  internal static Dictionary<string, PropertyInfo> GetAllConfigItemProperties(Type type)
  {
    return PropertyHelper.TryLookupConfigItemProperties(type) ?? new Dictionary<string, PropertyInfo>();
  }

  private static Dictionary<string, PropertyInfo> TryLookupConfigItemProperties(Type type)
  {
    lock (PropertyHelper.parameterInfoCache)
    {
      Dictionary<string, PropertyInfo> result;
      if (!PropertyHelper.parameterInfoCache.TryGetValue(type, out result))
        PropertyHelper.parameterInfoCache[type] = !PropertyHelper.TryCreatePropertyInfoDictionary(type, out result) ? (Dictionary<string, PropertyInfo>) null : result;
      return result;
    }
  }

  internal static void CheckRequiredParameters(object o)
  {
    foreach (KeyValuePair<string, PropertyInfo> configItemProperty in PropertyHelper.GetAllConfigItemProperties(o.GetType()))
    {
      PropertyInfo propertyInfo = configItemProperty.Value;
      if (propertyInfo.IsDefined(PropertyHelper._requiredParameterAttribute.GetType(), false) && propertyInfo.GetValue(o, (object[]) null) == null)
        throw new NLogConfigurationException($"Required parameter '{propertyInfo.Name}' on '{o}' was not specified.");
    }
  }

  internal static bool IsSimplePropertyType(Type type)
  {
    return Type.GetTypeCode(type) != TypeCode.Object || type == typeof (CultureInfo) || type == typeof (Type) || type == typeof (Encoding);
  }

  private static bool TryImplicitConversion(Type resultType, string value, out object result)
  {
    try
    {
      if (PropertyHelper.IsSimplePropertyType(resultType))
      {
        result = (object) null;
        return false;
      }
      MethodInfo method = resultType.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[1]
      {
        value.GetType()
      }, (ParameterModifier[]) null);
      if (method == (MethodInfo) null || !resultType.IsAssignableFrom(method.ReturnType))
      {
        result = (object) null;
        return false;
      }
      result = method.Invoke((object) null, new object[1]
      {
        (object) value
      });
      return true;
    }
    catch (Exception ex)
    {
      object[] objArray = new object[2]
      {
        (object) value,
        (object) resultType
      };
      InternalLogger.Warn(ex, "Implicit Conversion Failed of {0} to {1}", objArray);
    }
    result = (object) null;
    return false;
  }

  private static bool TryNLogSpecificConversion(
    Type propertyType,
    string value,
    ConfigurationItemFactory configurationItemFactory,
    out object newValue)
  {
    Func<string, ConfigurationItemFactory, object> func;
    if (PropertyHelper.DefaultPropertyConversionMapper.TryGetValue(propertyType, out func))
    {
      newValue = func(value, configurationItemFactory);
      return true;
    }
    newValue = (object) null;
    return false;
  }

  private static bool TryGetEnumValue(
    Type resultType,
    string value,
    out object result,
    bool flagsEnumAllowed)
  {
    if (!resultType.IsEnum())
    {
      result = (object) null;
      return false;
    }
    if (flagsEnumAllowed && resultType.IsDefined(PropertyHelper._flagsAttribute.GetType(), false))
    {
      ulong num = 0;
      foreach (string splitAndTrimToken in value.SplitAndTrimTokens(','))
      {
        FieldInfo field = resultType.GetField(splitAndTrimToken, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
        if (field == (FieldInfo) null)
          throw new NLogConfigurationException($"Invalid enumeration value '{value}'.");
        num |= Convert.ToUInt64(field.GetValue((object) null), (IFormatProvider) CultureInfo.InvariantCulture);
      }
      result = Convert.ChangeType((object) num, Enum.GetUnderlyingType(resultType), (IFormatProvider) CultureInfo.InvariantCulture);
      result = Enum.ToObject(resultType, result);
      return true;
    }
    FieldInfo field1 = resultType.GetField(value, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
    result = !(field1 == (FieldInfo) null) ? field1.GetValue((object) null) : throw new NLogConfigurationException($"Invalid enumeration value '{value}'.");
    return true;
  }

  private static object TryParseEncodingValue(
    string stringValue,
    ConfigurationItemFactory configurationItemFactory)
  {
    stringValue = stringValue.Trim();
    if (string.Equals(stringValue, "UTF8", StringComparison.OrdinalIgnoreCase))
      stringValue = Encoding.UTF8.WebName;
    return (object) Encoding.GetEncoding(stringValue);
  }

  private static object TryParseLayoutValue(
    string stringValue,
    ConfigurationItemFactory configurationItemFactory)
  {
    return (object) new SimpleLayout(stringValue, configurationItemFactory);
  }

  private static object TryParseConditionValue(
    string stringValue,
    ConfigurationItemFactory configurationItemFactory)
  {
    return (object) ConditionParser.ParseExpression(stringValue, configurationItemFactory);
  }

  private static bool TryFlatListConversion(
    object obj,
    PropertyInfo propInfo,
    string valueRaw,
    ConfigurationItemFactory configurationItemFactory,
    out object newValue)
  {
    object collectionObject;
    MethodInfo collectionAddMethod;
    Type collectionItemType;
    if (propInfo.PropertyType.IsGenericType() && PropertyHelper.TryCreateCollectionObject(obj, propInfo, valueRaw, out collectionObject, out collectionAddMethod, out collectionItemType))
    {
      foreach (string str in valueRaw.SplitQuoted(',', '\'', '\\'))
      {
        if (!PropertyHelper.TryGetEnumValue(collectionItemType, str, out newValue, false) && !PropertyHelper.TryNLogSpecificConversion(collectionItemType, str, configurationItemFactory, out newValue) && !PropertyHelper.TryImplicitConversion(collectionItemType, str, out newValue) && !PropertyHelper.TryTypeConverterConversion(collectionItemType, str, out newValue))
          newValue = Convert.ChangeType((object) str, collectionItemType, (IFormatProvider) CultureInfo.InvariantCulture);
        collectionAddMethod.Invoke(collectionObject, new object[1]
        {
          newValue
        });
      }
      newValue = collectionObject;
      return true;
    }
    newValue = (object) null;
    return false;
  }

  private static bool TryCreateCollectionObject(
    object obj,
    PropertyInfo propInfo,
    string valueRaw,
    out object collectionObject,
    out MethodInfo collectionAddMethod,
    out Type collectionItemType)
  {
    Type propertyType = propInfo.PropertyType;
    Type genericTypeDefinition = propertyType.GetGenericTypeDefinition();
    bool flag = genericTypeDefinition == typeof (ISet<>) || genericTypeDefinition == typeof (HashSet<>);
    if (flag || genericTypeDefinition == typeof (List<>) || genericTypeDefinition == typeof (IList<>) || genericTypeDefinition == typeof (IEnumerable<>))
    {
      object hashSetComparer = flag ? PropertyHelper.ExtractHashSetComparer(obj, propInfo) : (object) null;
      collectionItemType = propertyType.GetGenericArguments()[0];
      collectionObject = PropertyHelper.CreateCollectionObjectInstance(flag ? typeof (HashSet<>) : typeof (List<>), collectionItemType, hashSetComparer);
      if (collectionObject == null)
        throw new NLogConfigurationException("Cannot create instance of {0} for value {1}", new object[2]
        {
          (object) propertyType.ToString(),
          (object) valueRaw
        });
      collectionAddMethod = collectionObject.GetType().GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);
      if (collectionAddMethod == (MethodInfo) null)
        throw new NLogConfigurationException("Add method on type {0} for value {1} not found", new object[2]
        {
          (object) propertyType.ToString(),
          (object) valueRaw
        });
      return true;
    }
    collectionObject = (object) null;
    collectionAddMethod = (MethodInfo) null;
    collectionItemType = (Type) null;
    return false;
  }

  private static object CreateCollectionObjectInstance(
    Type collectionType,
    Type collectionItemType,
    object hashSetComparer)
  {
    Type type = collectionType.MakeGenericType(collectionItemType);
    if (hashSetComparer != null)
    {
      ConstructorInfo constructor = type.GetConstructor(new Type[1]
      {
        hashSetComparer.GetType()
      });
      if (constructor != (ConstructorInfo) null)
        return constructor.Invoke(new object[1]
        {
          hashSetComparer
        });
    }
    return Activator.CreateInstance(type);
  }

  private static object ExtractHashSetComparer(object obj, PropertyInfo propInfo)
  {
    object propertyValue = propInfo.IsValidPublicProperty() ? propInfo.GetPropertyValue(obj) : (object) null;
    if (propertyValue != null)
    {
      PropertyInfo property = propertyValue.GetType().GetProperty("Comparer", BindingFlags.Instance | BindingFlags.Public);
      if (property.IsValidPublicProperty())
        return property.GetPropertyValue(propertyValue);
    }
    return (object) null;
  }

  private static bool TryTypeConverterConversion(Type type, string value, out object newValue)
  {
    try
    {
      TypeConverter converter = TypeDescriptor.GetConverter(type);
      if (converter.CanConvertFrom(typeof (string)))
      {
        newValue = converter.ConvertFromInvariantString(value);
        return true;
      }
      newValue = (object) null;
      return false;
    }
    catch (MissingMethodException ex)
    {
      object[] objArray = new object[2]
      {
        (object) type,
        (object) value
      };
      InternalLogger.Error((Exception) ex, "Error in lookup of TypeDescriptor for type={0} to convert value '{1}'", objArray);
      newValue = (object) null;
      return false;
    }
  }

  private static bool TryGetPropertyInfo(
    Type targetType,
    string propertyName,
    out PropertyInfo result)
  {
    if (!string.IsNullOrEmpty(propertyName))
    {
      PropertyInfo property = targetType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
      if (property != (PropertyInfo) null)
      {
        result = property;
        return true;
      }
    }
    return PropertyHelper.GetAllConfigItemProperties(targetType).TryGetValue(propertyName, out result);
  }

  private static bool TryCreatePropertyInfoDictionary(
    Type t,
    out Dictionary<string, PropertyInfo> result)
  {
    result = (Dictionary<string, PropertyInfo>) null;
    try
    {
      if (!t.IsDefined(typeof (NLogConfigurationItemAttribute), true))
        return false;
      result = new Dictionary<string, PropertyInfo>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (PropertyInfo property in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        try
        {
          string key = PropertyHelper.LookupPropertySymbolName(property);
          if (!string.IsNullOrEmpty(key))
          {
            result[key] = property;
            if (property.IsDefined(PropertyHelper._defaultParameterAttribute.GetType(), false))
              result[string.Empty] = property;
          }
        }
        catch (Exception ex)
        {
          object[] objArray = new object[2]
          {
            (object) property.Name,
            (object) t
          };
          InternalLogger.Debug(ex, "Type reflection not possible for property {0} on type {1}. Maybe because of .NET Native.", objArray);
        }
      }
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]{ (object) t };
      InternalLogger.Debug(ex, "Type reflection not possible for type {0}. Maybe because of .NET Native.", objArray);
    }
    return result != null;
  }

  private static string LookupPropertySymbolName(PropertyInfo propInfo)
  {
    if (propInfo.PropertyType == (Type) null)
      return (string) null;
    if (PropertyHelper.IsSimplePropertyType(propInfo.PropertyType))
      return propInfo.Name;
    if (typeof (LayoutRenderer).IsAssignableFrom(propInfo.PropertyType))
      return propInfo.Name;
    if (typeof (Layout).IsAssignableFrom(propInfo.PropertyType))
      return propInfo.Name;
    if (propInfo.IsDefined(PropertyHelper._ignorePropertyAttribute.GetType(), false))
      return (string) null;
    ArrayParameterAttribute customAttribute = propInfo.GetCustomAttribute<ArrayParameterAttribute>();
    return customAttribute != null ? customAttribute.ElementName : propInfo.Name;
  }
}
