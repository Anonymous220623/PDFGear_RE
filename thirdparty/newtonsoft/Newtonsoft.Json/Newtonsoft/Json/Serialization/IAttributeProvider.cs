// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.IAttributeProvider
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace Newtonsoft.Json.Serialization;

public interface IAttributeProvider
{
  IList<Attribute> GetAttributes(bool inherit);

  IList<Attribute> GetAttributes(Type attributeType, bool inherit);
}
