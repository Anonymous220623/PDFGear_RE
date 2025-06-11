// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.SerializationBinderAdapter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Runtime.Serialization;

#nullable enable
namespace Newtonsoft.Json.Serialization;

internal class SerializationBinderAdapter : ISerializationBinder
{
  public readonly SerializationBinder SerializationBinder;

  public SerializationBinderAdapter(SerializationBinder serializationBinder)
  {
    this.SerializationBinder = serializationBinder;
  }

  public Type BindToType(string? assemblyName, string typeName)
  {
    return this.SerializationBinder.BindToType(assemblyName, typeName);
  }

  public void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
  {
    this.SerializationBinder.BindToName(serializedType, out assemblyName, out typeName);
  }
}
