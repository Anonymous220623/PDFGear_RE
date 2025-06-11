// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.QueryExpression
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath;

internal abstract class QueryExpression
{
  internal QueryOperator Operator;

  public QueryExpression(QueryOperator @operator) => this.Operator = @operator;

  public bool IsMatch(JToken root, JToken t) => this.IsMatch(root, t, (JsonSelectSettings) null);

  public abstract bool IsMatch(JToken root, JToken t, JsonSelectSettings? settings);
}
