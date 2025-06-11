// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CachedAttributeGetter`1
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;

#nullable enable
namespace Newtonsoft.Json.Serialization;

internal static class CachedAttributeGetter<T> where T : Attribute
{
  private static readonly ThreadSafeStore<object, T?> TypeAttributeCache = new ThreadSafeStore<object, T>(new Func<object, T>(JsonTypeReflector.GetAttribute<T>));

  public static T? GetAttribute(object type)
  {
    return CachedAttributeGetter<T>.TypeAttributeCache.Get(type);
  }
}
