﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.DynamicProxy`1
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Dynamic;

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal class DynamicProxy<T>
{
  public virtual IEnumerable<string> GetDynamicMemberNames(T instance)
  {
    return (IEnumerable<string>) CollectionUtils.ArrayEmpty<string>();
  }

  public virtual bool TryBinaryOperation(
    T instance,
    BinaryOperationBinder binder,
    object arg,
    out object? result)
  {
    result = (object) null;
    return false;
  }

  public virtual bool TryConvert(T instance, ConvertBinder binder, out object? result)
  {
    result = (object) null;
    return false;
  }

  public virtual bool TryCreateInstance(
    T instance,
    CreateInstanceBinder binder,
    object[] args,
    out object? result)
  {
    result = (object) null;
    return false;
  }

  public virtual bool TryDeleteIndex(T instance, DeleteIndexBinder binder, object[] indexes)
  {
    return false;
  }

  public virtual bool TryDeleteMember(T instance, DeleteMemberBinder binder) => false;

  public virtual bool TryGetIndex(
    T instance,
    GetIndexBinder binder,
    object[] indexes,
    out object? result)
  {
    result = (object) null;
    return false;
  }

  public virtual bool TryGetMember(T instance, GetMemberBinder binder, out object? result)
  {
    result = (object) null;
    return false;
  }

  public virtual bool TryInvoke(T instance, InvokeBinder binder, object[] args, out object? result)
  {
    result = (object) null;
    return false;
  }

  public virtual bool TryInvokeMember(
    T instance,
    InvokeMemberBinder binder,
    object[] args,
    out object? result)
  {
    result = (object) null;
    return false;
  }

  public virtual bool TrySetIndex(
    T instance,
    SetIndexBinder binder,
    object[] indexes,
    object value)
  {
    return false;
  }

  public virtual bool TrySetMember(T instance, SetMemberBinder binder, object value) => false;

  public virtual bool TryUnaryOperation(T instance, UnaryOperationBinder binder, out object? result)
  {
    result = (object) null;
    return false;
  }
}
