// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PropertyAccessor
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal class PropertyAccessor : IPropertyAccessor
{
  private Func<object, object> m_getter;
  private MethodInvoker m_setMethodInvoker;

  public Func<object, object> GetMethod => this.m_getter;

  public PropertyInfo PropertyInfo { get; private set; }

  public PropertyAccessor(PropertyInfo propertyInfo)
  {
    this.PropertyInfo = propertyInfo;
    this.InitializeGet(propertyInfo);
    this.InitializeSet(propertyInfo);
  }

  private void InitializeGet(PropertyInfo propertyInfo)
  {
    if (!propertyInfo.CanRead)
      return;
    ParameterExpression parameterExpression;
    this.m_getter = Expression.Lambda<Func<object, object>>((Expression) Expression.Convert((Expression) Expression.Property(propertyInfo.GetGetMethod().IsStatic ? (Expression) null : (Expression) Expression.Convert((Expression) parameterExpression, propertyInfo.DeclaringType), propertyInfo), typeof (object)), parameterExpression).Compile();
  }

  private void InitializeSet(PropertyInfo propertyInfo)
  {
    if (!propertyInfo.CanWrite || propertyInfo.GetSetMethod() == (MethodInfo) null)
      return;
    this.m_setMethodInvoker = new MethodInvoker(propertyInfo.GetSetMethod());
  }

  public object GetValue(object o)
  {
    if (this.m_getter == null)
      throw new NotSupportedException("Get method is not defined for this property.");
    return this.m_getter(o);
  }

  public void SetValue(object o, object value)
  {
    if (this.m_setMethodInvoker == null)
      throw new NotSupportedException("Set method is not defined for this property.");
    this.m_setMethodInvoker.Invoke(o, new object[1]{ value });
  }

  object IPropertyAccessor.GetValue(object instance) => this.GetValue(instance);

  void IPropertyAccessor.SetValue(object instance, object value) => this.SetValue(instance, value);
}
