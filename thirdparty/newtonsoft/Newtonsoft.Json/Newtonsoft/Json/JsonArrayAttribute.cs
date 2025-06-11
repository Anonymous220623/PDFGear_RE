// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonArrayAttribute
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable enable
namespace Newtonsoft.Json;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
public sealed class JsonArrayAttribute : JsonContainerAttribute
{
  private bool _allowNullItems;

  public bool AllowNullItems
  {
    get => this._allowNullItems;
    set => this._allowNullItems = value;
  }

  public JsonArrayAttribute()
  {
  }

  public JsonArrayAttribute(bool allowNullItems) => this._allowNullItems = allowNullItems;

  public JsonArrayAttribute(string id)
    : base(id)
  {
  }
}
