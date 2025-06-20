﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ReflectionValueProvider
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;

#nullable enable
namespace Newtonsoft.Json.Serialization;

public class ReflectionValueProvider : IValueProvider
{
  private readonly MemberInfo _memberInfo;

  public ReflectionValueProvider(MemberInfo memberInfo)
  {
    ValidationUtils.ArgumentNotNull((object) memberInfo, nameof (memberInfo));
    this._memberInfo = memberInfo;
  }

  public void SetValue(object target, object? value)
  {
    try
    {
      ReflectionUtils.SetMemberValue(this._memberInfo, target, value);
    }
    catch (Exception ex)
    {
      throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
    }
  }

  public object? GetValue(object target)
  {
    try
    {
      PropertyInfo memberInfo = this._memberInfo as PropertyInfo;
      if ((object) memberInfo != null && memberInfo.PropertyType.IsByRef)
        throw new InvalidOperationException("Could not create getter for {0}. ByRef return values are not supported.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) memberInfo));
      return ReflectionUtils.GetMemberValue(this._memberInfo, target);
    }
    catch (Exception ex)
    {
      throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
    }
  }
}
