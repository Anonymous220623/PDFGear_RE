// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ReflectionHelpers
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NLog.Internal;

internal static class ReflectionHelpers
{
  public static Type[] SafeGetTypes(this Assembly assembly)
  {
    try
    {
      return assembly.GetTypes();
    }
    catch (ReflectionTypeLoadException ex)
    {
      foreach (Exception loaderException in ex.LoaderExceptions)
        InternalLogger.Warn(loaderException, "Type load exception.");
      List<Type> typeList = new List<Type>();
      foreach (Type type in ex.Types)
      {
        if (type != (Type) null)
          typeList.Add(type);
      }
      return typeList.ToArray();
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "Type load exception.");
      return ArrayHelper.Empty<Type>();
    }
  }

  public static bool IsStaticClass(this Type type)
  {
    return type.IsClass() && type.IsAbstract() && type.IsSealed();
  }

  public static ReflectionHelpers.LateBoundMethod CreateLateBoundMethod(MethodInfo methodInfo)
  {
    ParameterExpression instanceParameter = Expression.Parameter(typeof (object), "instance");
    ParameterExpression parametersParameter = Expression.Parameter(typeof (object[]), "parameters");
    MethodCallExpression body = ReflectionHelpers.BuildParameterList(methodInfo, instanceParameter, parametersParameter);
    if (body.Type == typeof (void))
    {
      Action<object, object[]> execute = Expression.Lambda<Action<object, object[]>>((Expression) body, instanceParameter, parametersParameter).Compile();
      return (ReflectionHelpers.LateBoundMethod) ((instance, parameters) =>
      {
        execute(instance, parameters);
        return (object) null;
      });
    }
    return Expression.Lambda<ReflectionHelpers.LateBoundMethod>((Expression) Expression.Convert((Expression) body, typeof (object)), instanceParameter, parametersParameter).Compile();
  }

  public static ReflectionHelpers.LateBoundMethodSingle CreateLateBoundMethodSingle(
    MethodInfo methodInfo)
  {
    ParameterExpression instanceParameter = Expression.Parameter(typeof (object), "instance");
    ParameterExpression parameterParameter = Expression.Parameter(typeof (object), "parameters");
    MethodCallExpression body = ReflectionHelpers.BuildParameterListSingle(methodInfo, instanceParameter, parameterParameter);
    if (body.Type == typeof (void))
    {
      Action<object, object> execute = Expression.Lambda<Action<object, object>>((Expression) body, instanceParameter, parameterParameter).Compile();
      return (ReflectionHelpers.LateBoundMethodSingle) ((instance, parameters) =>
      {
        execute(instance, parameters);
        return (object) null;
      });
    }
    return Expression.Lambda<ReflectionHelpers.LateBoundMethodSingle>((Expression) Expression.Convert((Expression) body, typeof (object)), instanceParameter, parameterParameter).Compile();
  }

  private static MethodCallExpression BuildParameterList(
    MethodInfo methodInfo,
    ParameterExpression instanceParameter,
    ParameterExpression parametersParameter)
  {
    List<Expression> parameterExpressions = new List<Expression>();
    ParameterInfo[] parameters = methodInfo.GetParameters();
    for (int index = 0; index < parameters.Length; ++index)
    {
      BinaryExpression binaryExpression = Expression.ArrayIndex((Expression) parametersParameter, (Expression) Expression.Constant((object) index));
      UnaryExpression parameterExpression = ReflectionHelpers.CreateParameterExpression(parameters[index], (Expression) binaryExpression);
      parameterExpressions.Add((Expression) parameterExpression);
    }
    return ReflectionHelpers.CreateMethodCallExpression(methodInfo, instanceParameter, (IEnumerable<Expression>) parameterExpressions);
  }

  private static MethodCallExpression BuildParameterListSingle(
    MethodInfo methodInfo,
    ParameterExpression instanceParameter,
    ParameterExpression parameterParameter)
  {
    return ReflectionHelpers.CreateMethodCallExpression(methodInfo, instanceParameter, (IEnumerable<Expression>) new List<Expression>()
    {
      (Expression) ReflectionHelpers.CreateParameterExpression(((IEnumerable<ParameterInfo>) methodInfo.GetParameters()).Single<ParameterInfo>(), (Expression) parameterParameter)
    });
  }

  private static MethodCallExpression CreateMethodCallExpression(
    MethodInfo methodInfo,
    ParameterExpression instanceParameter,
    IEnumerable<Expression> parameterExpressions)
  {
    return Expression.Call(methodInfo.IsStatic ? (Expression) null : (Expression) Expression.Convert((Expression) instanceParameter, methodInfo.DeclaringType), methodInfo, parameterExpressions);
  }

  private static UnaryExpression CreateParameterExpression(
    ParameterInfo parameterInfo,
    Expression expression)
  {
    Type type = parameterInfo.ParameterType;
    if (type.IsByRef)
      type = type.GetElementType();
    return Expression.Convert(expression, type);
  }

  public static bool IsEnum(this Type type) => type.IsEnum;

  public static bool IsPrimitive(this Type type) => type.IsPrimitive;

  public static bool IsValueType(this Type type) => type.IsValueType;

  public static bool IsSealed(this Type type) => type.IsSealed;

  public static bool IsAbstract(this Type type) => type.IsAbstract;

  public static bool IsClass(this Type type) => type.IsClass;

  public static bool IsGenericType(this Type type) => type.IsGenericType;

  public static TAttr GetCustomAttribute<TAttr>(this Type type) where TAttr : Attribute
  {
    return (TAttr) Attribute.GetCustomAttribute((MemberInfo) type, typeof (TAttr));
  }

  public static TAttr GetCustomAttribute<TAttr>(this PropertyInfo info) where TAttr : Attribute
  {
    return (TAttr) Attribute.GetCustomAttribute((MemberInfo) info, typeof (TAttr));
  }

  public static TAttr GetCustomAttribute<TAttr>(this Assembly assembly) where TAttr : Attribute
  {
    return (TAttr) Attribute.GetCustomAttribute(assembly, typeof (TAttr));
  }

  public static IEnumerable<TAttr> GetCustomAttributes<TAttr>(this Type type, bool inherit) where TAttr : Attribute
  {
    return (IEnumerable<TAttr>) type.GetCustomAttributes(typeof (TAttr), inherit);
  }

  public static Assembly GetAssembly(this Type type) => type.Assembly;

  public static bool IsValidPublicProperty(this PropertyInfo p)
  {
    return p.CanRead && p.GetIndexParameters().Length == 0 && p.GetGetMethod() != (MethodInfo) null;
  }

  public static object GetPropertyValue(this PropertyInfo p, object instance)
  {
    return p.GetValue(instance);
  }

  public delegate object LateBoundMethod(object target, object[] arguments);

  public delegate object LateBoundMethodSingle(object target, object argument);
}
