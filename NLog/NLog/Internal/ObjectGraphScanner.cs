// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ObjectGraphScanner
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NLog.Internal;

internal static class ObjectGraphScanner
{
  public static List<T> FindReachableObjects<T>(bool aggressiveSearch, params object[] rootObjects) where T : class
  {
    if (InternalLogger.IsTraceEnabled)
      InternalLogger.Trace<Type>("FindReachableObject<{0}>:", typeof (T));
    List<T> result = new List<T>();
    HashSet<object> visitedObjects = new HashSet<object>((IEqualityComparer<object>) SingleItemOptimizedHashSet<object>.ReferenceEqualityComparer.Default);
    foreach (object rootObject in rootObjects)
    {
      if (ObjectGraphScanner.IncludeConfigurationItem(rootObject))
        ObjectGraphScanner.ScanProperties<T>(aggressiveSearch, result, rootObject, 0, visitedObjects);
    }
    return result;
  }

  private static void ScanProperties<T>(
    bool aggressiveSearch,
    List<T> result,
    object o,
    int level,
    HashSet<object> visitedObjects)
    where T : class
  {
    if (o == null || visitedObjects.Contains(o))
      return;
    Type type = o.GetType();
    if (InternalLogger.IsTraceEnabled)
      InternalLogger.Trace<string, string, object>("{0}Scanning {1} '{2}'", new string(' ', level), type.Name, o);
    if (o is T obj)
    {
      result.Add(obj);
      if (!aggressiveSearch)
        return;
    }
    foreach (KeyValuePair<string, PropertyInfo> configItemProperty in PropertyHelper.GetAllConfigItemProperties(type))
    {
      if (!string.IsNullOrEmpty(configItemProperty.Key))
      {
        PropertyInfo prop = configItemProperty.Value;
        if (!PropertyHelper.IsSimplePropertyType(prop.PropertyType))
        {
          object propValue = prop.GetValue(o, (object[]) null);
          if (propValue != null)
          {
            visitedObjects.Add(o);
            ObjectGraphScanner.ScanPropertyForObject<T>(prop, propValue, aggressiveSearch, result, level, visitedObjects);
          }
        }
      }
    }
  }

  private static void ScanPropertyForObject<T>(
    PropertyInfo prop,
    object propValue,
    bool aggressiveSearch,
    List<T> result,
    int level,
    HashSet<object> visitedObjects)
    where T : class
  {
    if (InternalLogger.IsTraceEnabled)
      InternalLogger.Trace("{0}Scanning Property {1} '{2}' {3}", (object) new string(' ', level + 1), (object) prop.Name, (object) propValue.ToString(), (object) prop.PropertyType.Namespace);
    if (propValue is IEnumerable enumerable)
    {
      IList list = ObjectGraphScanner.ConvertEnumerableToList(enumerable);
      if (list.Count <= 0 || visitedObjects.Contains((object) list))
        return;
      visitedObjects.Add((object) list);
      ObjectGraphScanner.ScanPropertiesList<T>(list, aggressiveSearch, result, level + 1, visitedObjects);
    }
    else
    {
      if (!ObjectGraphScanner.IncludeConfigurationItem(propValue, prop.PropertyType))
        return;
      ObjectGraphScanner.ScanProperties<T>(aggressiveSearch, result, propValue, level + 1, visitedObjects);
    }
  }

  private static void ScanPropertiesList<T>(
    IList list,
    bool aggressiveSearch,
    List<T> result,
    int level,
    HashSet<object> visitedObjects)
    where T : class
  {
    for (int index = 0; index < list.Count; ++index)
    {
      object o = list[index];
      if (ObjectGraphScanner.IncludeConfigurationItem(o))
        ObjectGraphScanner.ScanProperties<T>(aggressiveSearch, result, o, level, visitedObjects);
    }
  }

  private static IList ConvertEnumerableToList(IEnumerable enumerable)
  {
    switch (enumerable)
    {
      case ICollection collection when collection.Count == 0:
        return (IList) ArrayHelper.Empty<object>();
      case IList list2:
        if (list2.IsReadOnly)
          return list2;
        List<object> list1 = new List<object>(list2.Count);
        lock (list2.SyncRoot)
        {
          for (int index = 0; index < list2.Count; ++index)
            list1.Add(list2[index]);
        }
        return (IList) list1;
      default:
        return (IList) enumerable.Cast<object>().ToList<object>();
    }
  }

  private static bool IncludeConfigurationItem(object item, Type propertyType = null)
  {
    Type type1 = propertyType;
    if ((object) type1 == null)
      type1 = item?.GetType();
    propertyType = type1;
    if (propertyType == (Type) null)
      return false;
    if (PropertyHelper.IsConfigurationItemType(propertyType))
      return true;
    Type type2 = item?.GetType();
    return type2 != (Type) null && type2 != propertyType && PropertyHelper.IsConfigurationItemType(type2);
  }
}
