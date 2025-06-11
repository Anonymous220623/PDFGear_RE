// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.NoThrowExpressionVisitor
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Linq.Expressions;

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal class NoThrowExpressionVisitor : ExpressionVisitor
{
  internal static readonly object ErrorResult = new object();

  protected override Expression VisitConditional(ConditionalExpression node)
  {
    return node.IfFalse.NodeType == ExpressionType.Throw ? (Expression) Expression.Condition(node.Test, node.IfTrue, (Expression) Expression.Constant(NoThrowExpressionVisitor.ErrorResult)) : base.VisitConditional(node);
  }
}
