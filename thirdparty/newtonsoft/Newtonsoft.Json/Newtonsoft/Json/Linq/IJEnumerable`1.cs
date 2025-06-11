// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.IJEnumerable`1
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace Newtonsoft.Json.Linq;

public interface IJEnumerable<out T> : IEnumerable<T>, IEnumerable where T : JToken
{
  IJEnumerable<JToken> this[object key] { get; }
}
