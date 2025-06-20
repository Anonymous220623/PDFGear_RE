﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ReflectionDelegateFactory
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;
using System.Reflection;

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal abstract class ReflectionDelegateFactory
{
  public Func<T, object?> CreateGet<T>(MemberInfo memberInfo)
  {
    PropertyInfo propertyInfo = memberInfo as PropertyInfo;
    if ((object) propertyInfo != null)
    {
      if (propertyInfo.PropertyType.IsByRef)
        throw new InvalidOperationException("Could not create getter for {0}. ByRef return values are not supported.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) propertyInfo));
      return this.CreateGet<T>(propertyInfo);
    }
    return this.CreateGet<T>(memberInfo as FieldInfo ?? throw new Exception("Could not create getter for {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) memberInfo)));
  }

  public Action<T, object?> CreateSet<T>(MemberInfo memberInfo)
  {
    PropertyInfo propertyInfo = memberInfo as PropertyInfo;
    if ((object) propertyInfo != null)
      return this.CreateSet<T>(propertyInfo);
    return this.CreateSet<T>(memberInfo as FieldInfo ?? throw new Exception("Could not create setter for {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) memberInfo)));
  }

  public abstract MethodCall<T, object?> CreateMethodCall<T>(MethodBase method);

  public abstract ObjectConstructor<object> CreateParameterizedConstructor(MethodBase method);

  public abstract Func<T> CreateDefaultConstructor<T>(Type type);

  public abstract Func<T, object?> CreateGet<T>(PropertyInfo propertyInfo);

  public abstract Func<T, object?> CreateGet<T>(FieldInfo fieldInfo);

  public abstract Action<T, object?> CreateSet<T>(FieldInfo fieldInfo);

  public abstract Action<T, object?> CreateSet<T>(PropertyInfo propertyInfo);
}
