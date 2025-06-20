﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonContainerAttribute
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;

#nullable enable
namespace Newtonsoft.Json;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
public abstract class JsonContainerAttribute : Attribute
{
  internal bool? _isReference;
  internal bool? _itemIsReference;
  internal ReferenceLoopHandling? _itemReferenceLoopHandling;
  internal TypeNameHandling? _itemTypeNameHandling;
  private Type? _namingStrategyType;
  private object[]? _namingStrategyParameters;

  public string? Id { get; set; }

  public string? Title { get; set; }

  public string? Description { get; set; }

  public Type? ItemConverterType { get; set; }

  public object[]? ItemConverterParameters { get; set; }

  public Type? NamingStrategyType
  {
    get => this._namingStrategyType;
    set
    {
      this._namingStrategyType = value;
      this.NamingStrategyInstance = (NamingStrategy) null;
    }
  }

  public object[]? NamingStrategyParameters
  {
    get => this._namingStrategyParameters;
    set
    {
      this._namingStrategyParameters = value;
      this.NamingStrategyInstance = (NamingStrategy) null;
    }
  }

  internal NamingStrategy? NamingStrategyInstance { get; set; }

  public bool IsReference
  {
    get => this._isReference.GetValueOrDefault();
    set => this._isReference = new bool?(value);
  }

  public bool ItemIsReference
  {
    get => this._itemIsReference.GetValueOrDefault();
    set => this._itemIsReference = new bool?(value);
  }

  public ReferenceLoopHandling ItemReferenceLoopHandling
  {
    get => this._itemReferenceLoopHandling.GetValueOrDefault();
    set => this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
  }

  public TypeNameHandling ItemTypeNameHandling
  {
    get => this._itemTypeNameHandling.GetValueOrDefault();
    set => this._itemTypeNameHandling = new TypeNameHandling?(value);
  }

  protected JsonContainerAttribute()
  {
  }

  protected JsonContainerAttribute(string id) => this.Id = id;
}
