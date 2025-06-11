// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.TypeHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace PDFKit.Utils;

internal static class TypeHelper
{
  private static object unusedLocker = (object) "bfcbf101-b91c-4e94-a1f6-b98f3d60a21d";
  private static ConcurrentDictionary<object, object> fieldGetterCache = new ConcurrentDictionary<object, object>();

  private static object GetMemberAccessorLocker(Type targetType, string fieldName)
  {
    return targetType == (Type) null || string.IsNullOrEmpty(fieldName) ? TypeHelper.unusedLocker : (object) $"{targetType.FullName}_{fieldName}_getter";
  }

  public static Func<TTarget, object> CreateFieldOrPropertyGetter<TTarget>(
    string fieldName,
    BindingFlags bindingFlags)
  {
    return TypeHelper.CreateFieldOrPropertyGetter<TTarget, object>(fieldName, bindingFlags);
  }

  public static Func<TTarget, TMember> CreateFieldOrPropertyGetter<TTarget, TMember>(
    string fieldName,
    BindingFlags bindingFlags)
  {
    Func<object, object> func = TypeHelper.CreateFieldOrPropertyGetter(typeof (TTarget), typeof (TMember), fieldName, bindingFlags);
    return func != null ? (Func<TTarget, TMember>) (c => (TMember) func((object) c)) : (Func<TTarget, TMember>) null;
  }

  public static Func<object, object> CreateFieldOrPropertyGetter(
    Type targetType,
    Type resultType,
    string fieldName,
    BindingFlags bindingFlags)
  {
    if (string.IsNullOrEmpty(fieldName))
      return (Func<object, object>) null;
    fieldName = fieldName.Trim();
    object memberAccessorLocker = TypeHelper.GetMemberAccessorLocker(targetType, fieldName);
    lock (memberAccessorLocker)
    {
      object obj;
      Func<object, object> orPropertyGetter;
      int num;
      if (TypeHelper.fieldGetterCache.TryGetValue(memberAccessorLocker, out obj))
      {
        orPropertyGetter = obj as Func<object, object>;
        num = orPropertyGetter != null ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
        return orPropertyGetter;
      Func<object, object> memberGetterCore = TypeHelper.CreateMemberGetterCore(targetType, resultType, fieldName, bindingFlags);
      TypeHelper.fieldGetterCache[memberAccessorLocker] = (object) memberGetterCore;
      return memberGetterCore;
    }
  }

  private static Func<object, object> CreateMemberGetterCore(
    Type targetType,
    Type resultType,
    string fieldName,
    BindingFlags bindingFlags)
  {
    try
    {
      MemberInfo memberInfo = ((IEnumerable<MemberInfo>) targetType.GetMember(fieldName, bindingFlags)).FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (c => c.MemberType == MemberTypes.Field || c.MemberType == MemberTypes.Property));
      if (memberInfo != (MemberInfo) null)
      {
        bool flag = false;
        if (memberInfo.MemberType.HasFlag((Enum) MemberTypes.Property))
          flag = resultType.IsAssignableFrom(((PropertyInfo) memberInfo).PropertyType);
        else if (memberInfo.MemberType.HasFlag((Enum) MemberTypes.Field))
          flag = resultType.IsAssignableFrom(((FieldInfo) memberInfo).FieldType);
        if (!flag)
          return (Func<object, object>) null;
        Type type = typeof (object);
        ParameterExpression parameterExpression = Expression.Parameter(type, "c");
        UnaryExpression unaryExpression = Expression.Convert((Expression) parameterExpression, targetType);
        MemberExpression memberExpression = (MemberExpression) null;
        if (memberInfo.MemberType.HasFlag((Enum) MemberTypes.Field))
          memberExpression = Expression.Field((Expression) unaryExpression, (FieldInfo) memberInfo);
        else if (memberInfo.MemberType.HasFlag((Enum) MemberTypes.Property))
          memberExpression = Expression.Property((Expression) unaryExpression, (PropertyInfo) memberInfo);
        return Expression.Lambda<Func<object, object>>((Expression) Expression.Convert((Expression) memberExpression, type), parameterExpression).Compile();
      }
    }
    catch
    {
    }
    return (Func<object, object>) null;
  }
}
