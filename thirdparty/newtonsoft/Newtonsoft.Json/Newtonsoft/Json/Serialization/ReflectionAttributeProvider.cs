// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ReflectionAttributeProvider
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;

#nullable enable
namespace Newtonsoft.Json.Serialization;

public class ReflectionAttributeProvider : IAttributeProvider
{
  private readonly object _attributeProvider;

  public ReflectionAttributeProvider(object attributeProvider)
  {
    ValidationUtils.ArgumentNotNull(attributeProvider, nameof (attributeProvider));
    this._attributeProvider = attributeProvider;
  }

  public IList<Attribute> GetAttributes(bool inherit)
  {
    return (IList<Attribute>) ReflectionUtils.GetAttributes(this._attributeProvider, (Type) null, inherit);
  }

  public IList<Attribute> GetAttributes(Type attributeType, bool inherit)
  {
    return (IList<Attribute>) ReflectionUtils.GetAttributes(this._attributeProvider, attributeType, inherit);
  }
}
