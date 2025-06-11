// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ArrayMultipleIndexFilter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Collections.Generic;

#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath;

internal class ArrayMultipleIndexFilter : PathFilter
{
  internal List<int> Indexes;

  public ArrayMultipleIndexFilter(List<int> indexes) => this.Indexes = indexes;

  public override IEnumerable<JToken> ExecuteFilter(
    JToken root,
    IEnumerable<JToken> current,
    JsonSelectSettings? settings)
  {
    foreach (JToken t in current)
    {
      foreach (int index in this.Indexes)
      {
        JToken tokenIndex = PathFilter.GetTokenIndex(t, settings, index);
        if (tokenIndex != null)
          yield return tokenIndex;
      }
    }
  }
}
