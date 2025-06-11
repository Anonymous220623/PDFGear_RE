// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionOrExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.Conditions;

internal sealed class ConditionOrExpression : ConditionExpression
{
  public ConditionOrExpression(ConditionExpression left, ConditionExpression right)
  {
    this.LeftExpression = left;
    this.RightExpression = right;
  }

  public ConditionExpression LeftExpression { get; private set; }

  public ConditionExpression RightExpression { get; private set; }

  public override string ToString() => $"({this.LeftExpression} or {this.RightExpression})";

  protected override object EvaluateNode(LogEventInfo context)
  {
    return (bool) this.LeftExpression.Evaluate(context) || (bool) this.RightExpression.Evaluate(context) ? ConditionExpression.BoxedTrue : ConditionExpression.BoxedFalse;
  }
}
