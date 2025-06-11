// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Drawing.CommonExtensions
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace HandyControl.Expression.Drawing;

internal static class CommonExtensions
{
  public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    if (collection is List<T> objList)
    {
      objList.AddRange(newItems);
    }
    else
    {
      foreach (T newItem in newItems)
        collection.Add(newItem);
    }
  }

  public static bool SetIfDifferent(
    this DependencyObject dependencyObject,
    DependencyProperty dependencyProperty,
    object value)
  {
    if (object.Equals(dependencyObject.GetValue(dependencyProperty), value))
      return false;
    dependencyObject.SetValue(dependencyProperty, value);
    return true;
  }

  public static bool EnsureListCount<T>(this IList<T> list, int count, Func<T> factory = null)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (!list.EnsureListCountAtLeast<T>(count, factory))
    {
      if (list.Count <= count)
        return false;
      if (list is List<T> objList)
      {
        objList.RemoveRange(count, list.Count - count);
      }
      else
      {
        for (int index = list.Count - 1; index >= count; --index)
          list.RemoveAt(index);
      }
    }
    return true;
  }

  public static bool EnsureListCountAtLeast<T>(this IList<T> list, int count, Func<T> factory = null)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count));
    if (list.Count >= count)
      return false;
    if (list is List<T> objList && factory == null)
    {
      objList.AddRange((IEnumerable<T>) new T[count - list.Count]);
    }
    else
    {
      for (int count1 = list.Count; count1 < count; ++count1)
        list.Add(factory == null ? default (T) : factory());
    }
    return true;
  }

  public static bool ClearIfSet(
    this DependencyObject dependencyObject,
    DependencyProperty dependencyProperty)
  {
    if (dependencyObject.ReadLocalValue(dependencyProperty) == DependencyProperty.UnsetValue)
      return false;
    dependencyObject.ClearValue(dependencyProperty);
    return true;
  }

  public static void RemoveLast<T>(this IList<T> list) => list.RemoveAt(list.Count - 1);
}
