// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.QueryScanFilter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Collections.Generic;

#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath;

internal class QueryScanFilter : PathFilter
{
  internal QueryExpression Expression;

  public QueryScanFilter(QueryExpression expression) => this.Expression = expression;

  public override IEnumerable<JToken> ExecuteFilter(
    JToken root,
    IEnumerable<JToken> current,
    JsonSelectSettings? settings)
  {
    foreach (JToken t1 in current)
    {
      if (t1 is JContainer jcontainer)
      {
        foreach (JToken t2 in jcontainer.DescendantsAndSelf())
        {
          if (this.Expression.IsMatch(root, t2, settings))
            yield return t2;
        }
      }
      else if (this.Expression.IsMatch(root, t1, settings))
        yield return t1;
    }
  }
}
