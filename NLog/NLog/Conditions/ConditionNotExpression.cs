// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionNotExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.Conditions;

internal sealed class ConditionNotExpression : ConditionExpression
{
  public ConditionNotExpression(ConditionExpression expression) => this.Expression = expression;

  public ConditionExpression Expression { get; private set; }

  public override string ToString() => $"(not {this.Expression})";

  protected override object EvaluateNode(LogEventInfo context)
  {
    return !(bool) this.Expression.Evaluate(context) ? ConditionExpression.BoxedTrue : ConditionExpression.BoxedFalse;
  }
}
