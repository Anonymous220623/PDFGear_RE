﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.NoThrowGetBinderMember
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Dynamic;

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal class NoThrowGetBinderMember : GetMemberBinder
{
  private readonly GetMemberBinder _innerBinder;

  public NoThrowGetBinderMember(GetMemberBinder innerBinder)
    : base(innerBinder.Name, innerBinder.IgnoreCase)
  {
    this._innerBinder = innerBinder;
  }

  public override DynamicMetaObject FallbackGetMember(
    DynamicMetaObject target,
    DynamicMetaObject errorSuggestion)
  {
    DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, CollectionUtils.ArrayEmpty<DynamicMetaObject>());
    return new DynamicMetaObject(new NoThrowExpressionVisitor().Visit(dynamicMetaObject.Expression), dynamicMetaObject.Restrictions);
  }
}
