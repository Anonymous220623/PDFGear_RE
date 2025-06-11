// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JTokenEqualityComparer
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Collections.Generic;

#nullable enable
namespace Newtonsoft.Json.Linq;

public class JTokenEqualityComparer : IEqualityComparer<JToken>
{
  public bool Equals(JToken x, JToken y) => JToken.DeepEquals(x, y);

  public int GetHashCode(JToken obj) => obj == null ? 0 : obj.GetDeepHashCode();
}
