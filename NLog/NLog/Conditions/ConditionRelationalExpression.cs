// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionRelationalExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace NLog.Conditions;

internal sealed class ConditionRelationalExpression : ConditionExpression
{
  private static Dictionary<Type, int> TypePromoteOrder = ConditionRelationalExpression.BuildTypeOrderDictionary();

  public ConditionRelationalExpression(
    ConditionExpression leftExpression,
    ConditionExpression rightExpression,
    ConditionRelationalOperator relationalOperator)
  {
    this.LeftExpression = leftExpression;
    this.RightExpression = rightExpression;
    this.RelationalOperator = relationalOperator;
  }

  public ConditionExpression LeftExpression { get; private set; }

  public ConditionExpression RightExpression { get; private set; }

  public ConditionRelationalOperator RelationalOperator { get; private set; }

  public override string ToString()
  {
    return $"({this.LeftExpression} {this.GetOperatorString()} {this.RightExpression})";
  }

  protected override object EvaluateNode(LogEventInfo context)
  {
    return !ConditionRelationalExpression.Compare(this.LeftExpression.Evaluate(context), this.RightExpression.Evaluate(context), this.RelationalOperator) ? ConditionExpression.BoxedFalse : ConditionExpression.BoxedTrue;
  }

  private static bool Compare(
    object leftValue,
    object rightValue,
    ConditionRelationalOperator relationalOperator)
  {
    IComparer invariantCulture = (IComparer) StringComparer.InvariantCulture;
    ConditionRelationalExpression.PromoteTypes(ref leftValue, ref rightValue);
    switch (relationalOperator)
    {
      case ConditionRelationalOperator.Equal:
        return invariantCulture.Compare(leftValue, rightValue) == 0;
      case ConditionRelationalOperator.NotEqual:
        return invariantCulture.Compare(leftValue, rightValue) != 0;
      case ConditionRelationalOperator.Less:
        return invariantCulture.Compare(leftValue, rightValue) < 0;
      case ConditionRelationalOperator.Greater:
        return invariantCulture.Compare(leftValue, rightValue) > 0;
      case ConditionRelationalOperator.LessOrEqual:
        return invariantCulture.Compare(leftValue, rightValue) <= 0;
      case ConditionRelationalOperator.GreaterOrEqual:
        return invariantCulture.Compare(leftValue, rightValue) >= 0;
      default:
        throw new NotSupportedException($"Relational operator {relationalOperator} is not supported.");
    }
  }

  private static void PromoteTypes(ref object leftValue, ref object rightValue)
  {
    if (leftValue == rightValue || leftValue == null || rightValue == null)
      return;
    Type type1 = leftValue.GetType();
    Type type2 = rightValue.GetType();
    if (!(type1 == type2))
    {
      if (ConditionRelationalExpression.GetOrder(type1) < ConditionRelationalExpression.GetOrder(type2))
      {
        if (ConditionRelationalExpression.TryPromoteTypes(ref rightValue, type1, ref leftValue, type2))
          return;
      }
      else if (ConditionRelationalExpression.TryPromoteTypes(ref leftValue, type2, ref rightValue, type1))
        return;
      throw new ConditionEvaluationException($"Cannot find common type for '{type1.Name}' and '{type2.Name}'.");
    }
  }

  private static bool TryPromoteType(ref object val, Type type1)
  {
    try
    {
      if (type1 == typeof (DateTime))
      {
        val = (object) Convert.ToDateTime(val, (IFormatProvider) CultureInfo.InvariantCulture);
        return true;
      }
      if (type1 == typeof (double))
      {
        val = (object) Convert.ToDouble(val, (IFormatProvider) CultureInfo.InvariantCulture);
        return true;
      }
      if (type1 == typeof (float))
      {
        val = (object) Convert.ToSingle(val, (IFormatProvider) CultureInfo.InvariantCulture);
        return true;
      }
      if (type1 == typeof (Decimal))
      {
        val = (object) Convert.ToDecimal(val, (IFormatProvider) CultureInfo.InvariantCulture);
        return true;
      }
      if (type1 == typeof (long))
      {
        val = (object) Convert.ToInt64(val, (IFormatProvider) CultureInfo.InvariantCulture);
        return true;
      }
      if (type1 == typeof (int))
      {
        val = (object) Convert.ToInt32(val, (IFormatProvider) CultureInfo.InvariantCulture);
        return true;
      }
      if (type1 == typeof (bool))
      {
        val = (object) Convert.ToBoolean(val, (IFormatProvider) CultureInfo.InvariantCulture);
        return true;
      }
      if (type1 == typeof (string))
      {
        val = (object) Convert.ToString(val, (IFormatProvider) CultureInfo.InvariantCulture);
        InternalLogger.Debug("Using string comparision");
        return true;
      }
    }
    catch (Exception ex)
    {
      InternalLogger.Debug<object, string>("conversion of {0} to {1} failed", val, type1.Name);
    }
    return false;
  }

  private static bool TryPromoteTypes(ref object val1, Type type1, ref object val2, Type type2)
  {
    return ConditionRelationalExpression.TryPromoteType(ref val1, type1) || ConditionRelationalExpression.TryPromoteType(ref val2, type2);
  }

  private static int GetOrder(Type type1)
  {
    int num;
    return ConditionRelationalExpression.TypePromoteOrder.TryGetValue(type1, out num) ? num : int.MaxValue;
  }

  private static Dictionary<Type, int> BuildTypeOrderDictionary()
  {
    List<Type> typeList = new List<Type>()
    {
      typeof (DateTime),
      typeof (double),
      typeof (float),
      typeof (Decimal),
      typeof (long),
      typeof (int),
      typeof (bool),
      typeof (string)
    };
    Dictionary<Type, int> dictionary = new Dictionary<Type, int>(typeList.Count);
    for (int index = 0; index < typeList.Count; ++index)
      dictionary.Add(typeList[index], index);
    return dictionary;
  }

  private string GetOperatorString()
  {
    switch (this.RelationalOperator)
    {
      case ConditionRelationalOperator.Equal:
        return "==";
      case ConditionRelationalOperator.NotEqual:
        return "!=";
      case ConditionRelationalOperator.Less:
        return "<";
      case ConditionRelationalOperator.Greater:
        return ">";
      case ConditionRelationalOperator.LessOrEqual:
        return "<=";
      case ConditionRelationalOperator.GreaterOrEqual:
        return ">=";
      default:
        throw new NotSupportedException($"Relational operator {this.RelationalOperator} is not supported.");
    }
  }
}
