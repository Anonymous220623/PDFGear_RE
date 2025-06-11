// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionAndExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.Conditions;

internal sealed class ConditionAndExpression : ConditionExpression
{
  public ConditionAndExpression(ConditionExpression left, ConditionExpression right)
  {
    this.Left = left;
    this.Right = right;
  }

  public ConditionExpression Left { get; private set; }

  public ConditionExpression Right { get; private set; }

  public override string ToString() => $"({this.Left} and {this.Right})";

  protected override object EvaluateNode(LogEventInfo context)
  {
    return !(bool) this.Left.Evaluate(context) || !(bool) this.Right.Evaluate(context) ? ConditionExpression.BoxedFalse : ConditionExpression.BoxedTrue;
  }
}
