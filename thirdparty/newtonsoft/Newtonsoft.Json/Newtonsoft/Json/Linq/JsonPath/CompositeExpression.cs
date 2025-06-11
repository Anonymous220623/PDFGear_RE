// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.CompositeExpression
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath;

internal class CompositeExpression : QueryExpression
{
  public List<QueryExpression> Expressions { get; set; }

  public CompositeExpression(QueryOperator @operator)
    : base(@operator)
  {
    this.Expressions = new List<QueryExpression>();
  }

  public override bool IsMatch(JToken root, JToken t, JsonSelectSettings? settings)
  {
    switch (this.Operator)
    {
      case QueryOperator.And:
        foreach (QueryExpression expression in this.Expressions)
        {
          if (!expression.IsMatch(root, t, settings))
            return false;
        }
        return true;
      case QueryOperator.Or:
        foreach (QueryExpression expression in this.Expressions)
        {
          if (expression.IsMatch(root, t, settings))
            return true;
        }
        return false;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}
