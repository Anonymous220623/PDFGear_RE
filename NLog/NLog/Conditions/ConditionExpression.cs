// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;

#nullable disable
namespace NLog.Conditions;

[NLogConfigurationItem]
public abstract class ConditionExpression
{
  internal static readonly object BoxedTrue = (object) true;
  internal static readonly object BoxedFalse = (object) false;

  public static implicit operator ConditionExpression(string conditionExpressionText)
  {
    return ConditionParser.ParseExpression(conditionExpressionText);
  }

  public object Evaluate(LogEventInfo context)
  {
    try
    {
      return this.EvaluateNode(context);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "Exception occurred when evaluating condition");
      if (!ex.MustBeRethrownImmediately())
        throw new ConditionEvaluationException("Exception occurred when evaluating condition", ex);
      throw;
    }
  }

  public abstract override string ToString();

  protected abstract object EvaluateNode(LogEventInfo context);
}
