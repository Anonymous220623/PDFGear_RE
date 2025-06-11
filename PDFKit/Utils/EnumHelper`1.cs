// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.EnumHelper`1
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace PDFKit.Utils;

public static class EnumHelper<T> where T : struct, Enum
{
  private static object locker = new object();
  private static IReadOnlyList<T> allValues;
  private static object valuesHash;
  private static Dictionary<T, string> valueNameDict;
  private static Dictionary<string, T> nameValueDict;
  private static Type hashSetType;
  private static Type type;
  private static Type underlyingType;
  private static bool inited;
  private static Func<object, T, bool> containsFunc;

  public static bool ValidValue(T value)
  {
    EnumHelper<T>.InitValues();
    return EnumHelper<T>.containsFunc(EnumHelper<T>.valuesHash, value);
  }

  public static IReadOnlyList<T> AllValues
  {
    get
    {
      EnumHelper<T>.InitValues();
      return EnumHelper<T>.allValues;
    }
  }

  public static Dictionary<T, string> ValueNames
  {
    get
    {
      if (EnumHelper<T>.valueNameDict == null)
      {
        lock (EnumHelper<T>.locker)
        {
          if (EnumHelper<T>.valueNameDict == null)
          {
            EnumHelper<T>.InitValues();
            EnumHelper<T>.valueNameDict = EnumHelper<T>.allValues.Select<T, (T, string)>((Func<T, (T, string)>) (c => (c, c.ToString()))).GroupBy<(T, string), T>((Func<(T, string), T>) (c => c.c)).ToDictionary<IGrouping<T, (T, string)>, T, string>((Func<IGrouping<T, (T, string)>, T>) (c => c.Key), (Func<IGrouping<T, (T, string)>, string>) (c => c.First<(T, string)>().Item2));
          }
        }
      }
      return EnumHelper<T>.valueNameDict;
    }
  }

  public static Dictionary<string, T> NameValueDict
  {
    get
    {
      if (EnumHelper<T>.nameValueDict == null)
      {
        lock (EnumHelper<T>.locker)
        {
          if (EnumHelper<T>.nameValueDict == null)
          {
            EnumHelper<T>.InitValues();
            EnumHelper<T>.nameValueDict = EnumHelper<T>.allValues.Select<T, (string, T)>((Func<T, (string, T)>) (c => (c.ToString(), c))).GroupBy<(string, T), string>((Func<(string, T), string>) (c => c.Item1)).ToDictionary<IGrouping<string, (string, T)>, string, T>((Func<IGrouping<string, (string, T)>, string>) (c => c.Key), (Func<IGrouping<string, (string, T)>, T>) (c => c.First<(string, T)>().Item2));
          }
        }
      }
      return EnumHelper<T>.nameValueDict;
    }
  }

  private static Func<object, T, bool> CreateContainsFunc()
  {
    ParameterExpression parameterExpression1 = Expression.Parameter(typeof (object), "p1");
    ParameterExpression parameterExpression2 = Expression.Parameter(EnumHelper<T>.type, "p2");
    UnaryExpression instance = Expression.Convert((Expression) parameterExpression1, EnumHelper<T>.hashSetType);
    UnaryExpression unaryExpression = Expression.Convert((Expression) parameterExpression2, EnumHelper<T>.underlyingType);
    return Expression.Lambda<Func<object, T, bool>>((Expression) Expression.Call((Expression) instance, EnumHelper<T>.hashSetType.GetMethod("Contains"), (Expression) unaryExpression), parameterExpression1, parameterExpression2).Compile();
  }

  private static void InitValues()
  {
    if (EnumHelper<T>.inited)
      return;
    lock (EnumHelper<T>.locker)
    {
      if (!EnumHelper<T>.inited)
      {
        EnumHelper<T>.type = typeof (T);
        EnumHelper<T>.underlyingType = EnumHelper<T>.type.GetEnumUnderlyingType();
        EnumHelper<T>.allValues = (IReadOnlyList<T>) Enum.GetValues(EnumHelper<T>.type).OfType<T>().ToArray<T>();
        EnumHelper<T>.hashSetType = typeof (HashSet<>).MakeGenericType(EnumHelper<T>.underlyingType);
        Array instance = Array.CreateInstance(EnumHelper<T>.underlyingType, EnumHelper<T>.allValues.Count);
        for (int index = 0; index < EnumHelper<T>.allValues.Count; ++index)
          instance.SetValue((object) EnumHelper<T>.allValues[index], index);
        EnumHelper<T>.valuesHash = Activator.CreateInstance(EnumHelper<T>.hashSetType, (object) instance);
        EnumHelper<T>.containsFunc = EnumHelper<T>.CreateContainsFunc();
        EnumHelper<T>.inited = true;
      }
    }
  }
}
