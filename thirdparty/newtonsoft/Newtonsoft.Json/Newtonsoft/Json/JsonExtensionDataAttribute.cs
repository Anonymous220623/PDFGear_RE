// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonExtensionDataAttribute
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class JsonExtensionDataAttribute : Attribute
{
  public bool WriteData { get; set; }

  public bool ReadData { get; set; }

  public JsonExtensionDataAttribute()
  {
    this.WriteData = true;
    this.ReadData = true;
  }
}
