// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverterAttribute
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable enable
namespace Newtonsoft.Json;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class JsonConverterAttribute : Attribute
{
  private readonly Type _converterType;

  public Type ConverterType => this._converterType;

  public object[]? ConverterParameters { get; }

  public JsonConverterAttribute(Type converterType)
  {
    this._converterType = !(converterType == (Type) null) ? converterType : throw new ArgumentNullException(nameof (converterType));
  }

  public JsonConverterAttribute(Type converterType, params object[] converterParameters)
    : this(converterType)
  {
    this.ConverterParameters = converterParameters;
  }
}
