// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.TypeHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace CommomLib.Commom;

public static class TypeHelper
{
  private static object unusedLocker = (object) "bfcbf101-b91c-4e94-a1f6-b98f3d60a21d";
  private static ConcurrentDictionary<object, object> fieldGetterCache = new ConcurrentDictionary<object, object>();

  private static object GetMemberAccessorLocker(Type targetType, Type fieldType, string fieldName)
  {
    if (targetType == (Type) null || fieldType == (Type) null || string.IsNullOrEmpty(fieldName))
      return TypeHelper.unusedLocker;
    return (object) $"{targetType.FullName}_{fieldType.FullName}_{fieldName}_getter";
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
    if (string.IsNullOrEmpty(fieldName))
      return (Func<TTarget, TMember>) null;
    fieldName = fieldName.Trim();
    Type type = typeof (TTarget);
    Type fieldType = typeof (TMember);
    object memberAccessorLocker = TypeHelper.GetMemberAccessorLocker(type, fieldType, fieldName);
    lock (memberAccessorLocker)
    {
      object obj;
      if (TypeHelper.fieldGetterCache.TryGetValue((object) type, out obj) && obj is Func<TTarget, TMember> orPropertyGetter)
        return orPropertyGetter;
      Func<TTarget, TMember> memberGetterCore = TypeHelper.CreateMemberGetterCore<TTarget, TMember>(fieldName, bindingFlags);
      TypeHelper.fieldGetterCache[memberAccessorLocker] = (object) memberGetterCore;
      return memberGetterCore;
    }
  }

  private static Func<TTarget, TMember> CreateMemberGetterCore<TTarget, TMember>(
    string fieldName,
    BindingFlags bindingFlags)
  {
    Type type1 = typeof (TTarget);
    MemberInfo memberInfo = ((IEnumerable<MemberInfo>) type1.GetMember(fieldName, bindingFlags)).FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (c => c.MemberType == MemberTypes.Field || c.MemberType == MemberTypes.Property));
    Type type2 = typeof (TMember);
    if (!(memberInfo != (MemberInfo) null))
      return (Func<TTarget, TMember>) null;
    ParameterExpression parameterExpression1 = Expression.Parameter(type1, "c");
    MemberExpression memberExpression = (MemberExpression) null;
    ParameterExpression parameterExpression2 = parameterExpression1;
    if ((bindingFlags & BindingFlags.Static) != BindingFlags.Default)
      parameterExpression2 = (ParameterExpression) null;
    if (memberInfo.MemberType == MemberTypes.Field)
      memberExpression = Expression.Field((Expression) parameterExpression2, (FieldInfo) memberInfo);
    else if (memberInfo.MemberType == MemberTypes.Property)
      memberExpression = Expression.Property((Expression) parameterExpression2, (PropertyInfo) memberInfo);
    return Expression.Lambda<Func<TTarget, TMember>>((Expression) Expression.Convert((Expression) memberExpression, type2), parameterExpression1).Compile();
  }
}
