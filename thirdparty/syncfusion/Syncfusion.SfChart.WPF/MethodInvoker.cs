// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.MethodInvoker
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class MethodInvoker : IMethodInvoker
{
  private Func<object, object[], object> m_invoker;

  public MethodInfo MethodInfo { get; private set; }

  public MethodInvoker(MethodInfo methodInfo)
  {
    this.MethodInfo = methodInfo;
    this.m_invoker = MethodInvoker.CreateInvokeDelegate(methodInfo);
  }

  public object Invoke(object instance, params object[] parameters)
  {
    return this.m_invoker(instance, parameters);
  }

  private static Func<object, object[], object> CreateInvokeDelegate(MethodInfo methodInfo)
  {
    ParameterExpression parameterExpression = Expression.Parameter(typeof (object), "instance");
    ParameterExpression array = Expression.Parameter(typeof (object[]), "parameters");
    List<Expression> arguments = new List<Expression>();
    ParameterInfo[] parameters1 = methodInfo.GetParameters();
    for (int index = 0; index < parameters1.Length; ++index)
    {
      UnaryExpression unaryExpression = Expression.Convert((Expression) Expression.ArrayIndex((Expression) array, (Expression) Expression.Constant((object) index)), parameters1[index].ParameterType);
      arguments.Add((Expression) unaryExpression);
    }
    MethodCallExpression body = Expression.Call(methodInfo.IsStatic ? (Expression) null : (Expression) Expression.Convert((Expression) parameterExpression, methodInfo.DeclaringType), methodInfo, (IEnumerable<Expression>) arguments);
    if (body.Type == typeof (void))
    {
      Action<object, object[]> execute = Expression.Lambda<Action<object, object[]>>((Expression) body, parameterExpression, array).Compile();
      return (Func<object, object[], object>) ((instance, parameters) =>
      {
        execute(instance, parameters);
        return (object) null;
      });
    }
    return Expression.Lambda<Func<object, object[], object>>((Expression) Expression.Convert((Expression) body, typeof (object)), parameterExpression, array).Compile();
  }

  object IMethodInvoker.Invoke(object instance, params object[] parameters)
  {
    return this.Invoke(instance, parameters);
  }
}
