// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.TypeInformation
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal class TypeInformation
{
  public Type Type { get; }

  public PrimitiveTypeCode TypeCode { get; }

  public TypeInformation(Type type, PrimitiveTypeCode typeCode)
  {
    this.Type = type;
    this.TypeCode = typeCode;
  }
}
