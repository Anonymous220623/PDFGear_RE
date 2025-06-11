// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.QueryFilter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Collections.Generic;

#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath;

internal class QueryFilter : PathFilter
{
  internal QueryExpression Expression;

  public QueryFilter(QueryExpression expression) => this.Expression = expression;

  public override IEnumerable<JToken> ExecuteFilter(
    JToken root,
    IEnumerable<JToken> current,
    JsonSelectSettings? settings)
  {
    foreach (IEnumerable<JToken> jtokens in current)
    {
      foreach (JToken t in jtokens)
      {
        if (this.Expression.IsMatch(root, t, settings))
          yield return t;
      }
    }
  }
}
