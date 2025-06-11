// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.ReflectionHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace PDFKit.Utils;

internal static class ReflectionHelper
{
  private static Dictionary<ReflectionHelper.FunctionCacheKey, Delegate> funcs = new Dictionary<ReflectionHelper.FunctionCacheKey, Delegate>();

  public static Func<T> BuildPropertyGetter<T>(PropertyInfo propertyInfo)
  {
    if ((object) propertyInfo == null)
      throw new ArgumentNullException(nameof (propertyInfo));
    ParameterExpression parameterExpression = typeof (T).IsAssignableFrom(propertyInfo.PropertyType) ? Expression.Parameter(propertyInfo.DeclaringType, "c") : throw new ArgumentException(nameof (propertyInfo));
    return Expression.Lambda<Func<T>>((Expression) Expression.Convert((Expression) Expression.MakeMemberAccess((Expression) parameterExpression, (MemberInfo) propertyInfo), typeof (T)), parameterExpression).Compile();
  }

  public static Type[] CreateParameterTypes(params Type[] types)
  {
    if (types == null || types.Length == 0)
      return (Type[]) null;
    return !((IEnumerable<Type>) types).Any<Type>((Func<Type, bool>) (c => c == (Type) null)) ? types : throw new ArgumentException(nameof (types));
  }

  public static Action<TInstance> BuildMethodAction<TInstance>(MethodInfo methodInfo)
  {
    return (Action<TInstance>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, (Type) null).Compile();
  }

  public static Action<TInstance, T> BuildMethodAction<TInstance, T>(
    MethodInfo methodInfo,
    Type parameterType = null)
  {
    Type[] parameterTypes = (Type[]) null;
    if (parameterType != (Type) null)
      parameterTypes = new Type[1]{ parameterType };
    return (Action<TInstance, T>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2> BuildMethodAction<TInstance, T1, T2>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3> BuildMethodAction<TInstance, T1, T2, T3>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3, T4> BuildMethodAction<TInstance, T1, T2, T3, T4>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3, T4>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3, T4, T5> BuildMethodAction<TInstance, T1, T2, T3, T4, T5>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3, T4, T5>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3, T4, T5, T6> BuildMethodAction<TInstance, T1, T2, T3, T4, T5, T6>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3, T4, T5, T6>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3, T4, T5, T6, T7> BuildMethodAction<TInstance, T1, T2, T3, T4, T5, T6, T7>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3, T4, T5, T6, T7>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3, T4, T5, T6, T7, T8> BuildMethodAction<TInstance, T1, T2, T3, T4, T5, T6, T7, T8>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3, T4, T5, T6, T7, T8>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance> BuildMethodAction<TInstance>(
    Type instanceType,
    MethodInfo methodInfo)
  {
    return (Action<TInstance>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, (Type) null).Compile();
  }

  public static Action<TInstance, T> BuildMethodAction<TInstance, T>(
    Type instanceType,
    MethodInfo methodInfo,
    Type parameterType = null)
  {
    Type[] parameterTypes = (Type[]) null;
    if (parameterType != (Type) null)
      parameterTypes = new Type[1]{ parameterType };
    return (Action<TInstance, T>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2> BuildMethodAction<TInstance, T1, T2>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3> BuildMethodAction<TInstance, T1, T2, T3>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3, T4> BuildMethodAction<TInstance, T1, T2, T3, T4>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3, T4>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3, T4, T5> BuildMethodAction<TInstance, T1, T2, T3, T4, T5>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3, T4, T5>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3, T4, T5, T6> BuildMethodAction<TInstance, T1, T2, T3, T4, T5, T6>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3, T4, T5, T6>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3, T4, T5, T6, T7> BuildMethodAction<TInstance, T1, T2, T3, T4, T5, T6, T7>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3, T4, T5, T6, T7>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Action<TInstance, T1, T2, T3, T4, T5, T6, T7, T8> BuildMethodAction<TInstance, T1, T2, T3, T4, T5, T6, T7, T8>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Action<TInstance, T1, T2, T3, T4, T5, T6, T7, T8>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, (Type) null, parameterTypes).Compile();
  }

  public static Func<TInstance, TReturn> BuildMethodFunction<TInstance, TReturn>(
    MethodInfo methodInfo)
  {
    return (Func<TInstance, TReturn>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, typeof (TReturn)).Compile();
  }

  public static Func<TInstance, T1, TReturn> BuildMethodFunction<TInstance, T1, TReturn>(
    MethodInfo methodInfo,
    Type parameterType = null)
  {
    Type[] parameterTypes = (Type[]) null;
    if (parameterType != (Type) null)
      parameterTypes = new Type[1]{ parameterType };
    return (Func<TInstance, T1, TReturn>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, TReturn> BuildMethodFunction<TInstance, T1, T2, TReturn>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, TReturn>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, TReturn>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, TReturn>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, T4, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, T4, TReturn>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, T4, TReturn>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, T4, T5, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, T4, T5, TReturn>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, T4, T5, TReturn>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, T4, T5, T6, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, T4, T5, T6, TReturn>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, T4, T5, T6, TReturn>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, T4, T5, T6, T7, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, T4, T5, T6, T7, TReturn>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, T4, T5, T6, T7, TReturn>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, TReturn>(
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, TReturn>) ReflectionHelper.BuildMethodFunctionCore(typeof (TInstance), methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, TReturn> BuildMethodFunction<TInstance, TReturn>(
    Type instanceType,
    MethodInfo methodInfo)
  {
    return (Func<TInstance, TReturn>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, typeof (TReturn)).Compile();
  }

  public static Func<TInstance, T1, TReturn> BuildMethodFunction<TInstance, T1, TReturn>(
    Type instanceType,
    MethodInfo methodInfo,
    Type parameterType = null)
  {
    Type[] parameterTypes = (Type[]) null;
    if (parameterType != (Type) null)
      parameterTypes = new Type[1]{ parameterType };
    return (Func<TInstance, T1, TReturn>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, TReturn> BuildMethodFunction<TInstance, T1, T2, TReturn>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, TReturn>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, TReturn>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, TReturn>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, T4, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, T4, TReturn>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, T4, TReturn>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, T4, T5, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, T4, T5, TReturn>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, T4, T5, TReturn>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, T4, T5, T6, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, T4, T5, T6, TReturn>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, T4, T5, T6, TReturn>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, T4, T5, T6, T7, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, T4, T5, T6, T7, TReturn>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, T4, T5, T6, T7, TReturn>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  public static Func<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, TReturn> BuildMethodFunction<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, TReturn>(
    Type instanceType,
    MethodInfo methodInfo,
    Type[] parameterTypes = null)
  {
    return (Func<TInstance, T1, T2, T3, T4, T5, T6, T7, T8, TReturn>) ReflectionHelper.BuildMethodFunctionCore(instanceType, methodInfo, typeof (TReturn), parameterTypes).Compile();
  }

  private static LambdaExpression BuildMethodFunctionCore(
    Type instanceType,
    MethodInfo methodInfo,
    Type actualReturnType,
    Type[] parameterTypes = null)
  {
    if ((object) methodInfo == null)
      throw new ArgumentNullException(nameof (methodInfo));
    if (!methodInfo.IsStatic)
    {
      if ((object) instanceType == null)
        throw new ArgumentNullException(nameof (instanceType));
      if (!instanceType.IsAssignableFrom(methodInfo.DeclaringType))
        throw new ArgumentException(nameof (methodInfo));
    }
    if (methodInfo.ReturnType != typeof (void) && actualReturnType != (Type) null && !actualReturnType.IsAssignableFrom(methodInfo.ReturnType))
      throw new ArgumentException(nameof (methodInfo));
    ParameterInfo[] parameterInfoArray = methodInfo.GetParameters() ?? Array.Empty<ParameterInfo>();
    if (parameterTypes == null)
    {
      parameterTypes = ((IEnumerable<ParameterInfo>) methodInfo.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (x => x.ParameterType)).ToArray<Type>();
    }
    else
    {
      if (parameterTypes.Length != parameterInfoArray.Length)
        throw new ArgumentException(nameof (parameterTypes));
      for (int index = 0; index < parameterTypes.Length; ++index)
      {
        if (!parameterInfoArray[index].ParameterType.IsAssignableFrom(parameterTypes[index]))
          throw new ArgumentException(nameof (parameterTypes));
      }
    }
    ParameterExpression parameterExpression = Expression.Parameter(instanceType, "c");
    ParameterExpression[] source = new ParameterExpression[parameterInfoArray.Length + 1];
    source[0] = parameterExpression;
    for (int index = 0; index < parameterTypes.Length; ++index)
      source[index + 1] = Expression.Parameter(parameterTypes[index], parameterInfoArray[index].Name);
    Expression expression = !methodInfo.IsStatic ? (Expression) Expression.Call((Expression) Expression.Convert((Expression) parameterExpression, methodInfo.DeclaringType), methodInfo, (Expression[]) ((IEnumerable<ParameterExpression>) source).Skip<ParameterExpression>(1).ToArray<ParameterExpression>()) : (Expression) Expression.Call(methodInfo, (Expression[]) ((IEnumerable<ParameterExpression>) source).Skip<ParameterExpression>(1).ToArray<ParameterExpression>());
    if (methodInfo.ReturnType != typeof (void))
    {
      if (actualReturnType == (Type) null)
        actualReturnType = methodInfo.ReturnType;
      expression = (Expression) Expression.Convert(expression, actualReturnType);
    }
    return Expression.Lambda(expression, source);
  }

  private class FunctionCacheKey : IEquatable<ReflectionHelper.FunctionCacheKey>
  {
    private int _hashCode;

    public FunctionCacheKey(
      ReflectionHelper.FunctionType type,
      Type instanceType,
      ParameterInfo[] parameters,
      Type[] genericParameters)
    {
      if (type != ReflectionHelper.FunctionType.Action && type != ReflectionHelper.FunctionType.Func)
        throw new ArgumentException(nameof (type));
      if ((object) instanceType == null)
        throw new ArgumentNullException(nameof (instanceType));
      this.Type = type;
      this.InstanceType = instanceType;
      this.Parameters = (IReadOnlyList<ParameterInfo>) (parameters ?? Array.Empty<ParameterInfo>());
      this.GenericParameters = (IReadOnlyList<Type>) (genericParameters ?? Array.Empty<Type>());
      this._hashCode = HashCode.Combine<ReflectionHelper.FunctionType, Type>(type, instanceType);
      if (parameters != null)
      {
        foreach (ParameterInfo parameter in parameters)
          this._hashCode = HashCode.Combine<int, ParameterInfo>(this._hashCode, parameter);
      }
      if (genericParameters == null)
        return;
      foreach (Type genericParameter in genericParameters)
        this._hashCode = HashCode.Combine<int, Type>(this._hashCode, genericParameter);
    }

    public ReflectionHelper.FunctionType Type { get; }

    public Type InstanceType { get; }

    public IReadOnlyList<ParameterInfo> Parameters { get; }

    public IReadOnlyList<Type> GenericParameters { get; }

    public override int GetHashCode() => this._hashCode;

    public override bool Equals(object obj)
    {
      return obj is ReflectionHelper.FunctionCacheKey functionCacheKey && functionCacheKey.Equals(this);
    }

    public bool Equals(ReflectionHelper.FunctionCacheKey other)
    {
      return this == other || other != null && this._hashCode == other._hashCode && this.Type == other.Type && this.GenericParameters.SequenceEqual<Type>((IEnumerable<Type>) other.GenericParameters) && this.Parameters.Select<ParameterInfo, (string, Type, bool)>((Func<ParameterInfo, (string, Type, bool)>) (c => (c.Name, c.ParameterType, c.IsOut))).SequenceEqual<(string, Type, bool)>(other.Parameters.Select<ParameterInfo, (string, Type, bool)>((Func<ParameterInfo, (string, Type, bool)>) (c => (c.Name, c.ParameterType, c.IsOut))));
    }
  }

  private enum FunctionType
  {
    Action,
    Func,
  }
}
