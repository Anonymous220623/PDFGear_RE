// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionMethodExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.Conditions;

internal sealed class ConditionMethodExpression : ConditionExpression
{
  private readonly string _conditionMethodName;
  private readonly bool _acceptsLogEvent;
  private readonly ConditionExpression[] _methodParameters;
  private readonly ReflectionHelpers.LateBoundMethod _lateBoundMethod;
  private readonly object[] _lateBoundMethodDefaultParameters;

  public ConditionMethodExpression(
    string conditionMethodName,
    MethodInfo methodInfo,
    ReflectionHelpers.LateBoundMethod lateBoundMethod,
    IEnumerable<ConditionExpression> methodParameters)
  {
    this.MethodInfo = methodInfo;
    this._lateBoundMethod = lateBoundMethod;
    this._conditionMethodName = conditionMethodName;
    this._methodParameters = new List<ConditionExpression>(methodParameters).ToArray();
    ParameterInfo[] parameters = this.MethodInfo.GetParameters();
    if (parameters.Length != 0 && parameters[0].ParameterType == typeof (LogEventInfo))
      this._acceptsLogEvent = true;
    this._lateBoundMethodDefaultParameters = ConditionMethodExpression.CreateMethodDefaultParameters(parameters, this._methodParameters, this._acceptsLogEvent ? 1 : 0);
    int length = this._methodParameters.Length;
    if (this._acceptsLogEvent)
      ++length;
    int requiredParametersCount;
    int optionalParametersCount;
    ConditionMethodExpression.CountParmameters(parameters, out requiredParametersCount, out optionalParametersCount);
    if (length < requiredParametersCount || length > parameters.Length)
    {
      string message;
      if (optionalParametersCount > 0)
        message = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Condition method '{0}' requires between {1} and {2} parameters, but passed {3}.", (object) conditionMethodName, (object) requiredParametersCount, (object) parameters.Length, (object) length);
      else
        message = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Condition method '{0}' requires {1} parameters, but passed {2}.", new object[3]
        {
          (object) conditionMethodName,
          (object) requiredParametersCount,
          (object) length
        });
      InternalLogger.Error(message);
      throw new ConditionParseException(message);
    }
  }

  public MethodInfo MethodInfo { get; }

  private static object[] CreateMethodDefaultParameters(
    ParameterInfo[] formalParameters,
    ConditionExpression[] methodParameters,
    int parameterOffset)
  {
    int length = formalParameters.Length - methodParameters.Length - parameterOffset;
    if (length <= 0)
      return ArrayHelper.Empty<object>();
    object[] defaultParameters = new object[length];
    for (int index = methodParameters.Length + parameterOffset; index < formalParameters.Length; ++index)
    {
      ParameterInfo formalParameter = formalParameters[index];
      defaultParameters[index - methodParameters.Length + parameterOffset] = formalParameter.DefaultValue;
    }
    return defaultParameters;
  }

  private static void CountParmameters(
    ParameterInfo[] formalParameters,
    out int requiredParametersCount,
    out int optionalParametersCount)
  {
    requiredParametersCount = 0;
    optionalParametersCount = 0;
    foreach (ParameterInfo formalParameter in formalParameters)
    {
      if (formalParameter.IsOptional)
        ++optionalParametersCount;
      else
        ++requiredParametersCount;
    }
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this._conditionMethodName);
    stringBuilder.Append("(");
    string str = string.Empty;
    foreach (ConditionExpression methodParameter in this._methodParameters)
    {
      stringBuilder.Append(str);
      stringBuilder.Append((object) methodParameter);
      str = ", ";
    }
    stringBuilder.Append(")");
    return stringBuilder.ToString();
  }

  protected override object EvaluateNode(LogEventInfo context)
  {
    return this._lateBoundMethod((object) null, this.GenerateCallParameters(context));
  }

  private object[] GenerateCallParameters(LogEventInfo context)
  {
    int num = this._acceptsLogEvent ? 1 : 0;
    int length = this._methodParameters.Length + num + this._lateBoundMethodDefaultParameters.Length;
    if (length == 0)
      return ArrayHelper.Empty<object>();
    object[] callParameters = new object[length];
    if (this._acceptsLogEvent)
      callParameters[0] = (object) context;
    for (int index = 0; index < this._methodParameters.Length; ++index)
    {
      ConditionExpression methodParameter = this._methodParameters[index];
      callParameters[index + num] = methodParameter.Evaluate(context);
    }
    if (this._lateBoundMethodDefaultParameters.Length != 0)
    {
      for (int index = this._lateBoundMethodDefaultParameters.Length - 1; index >= 0; --index)
        callParameters[callParameters.Length - index - 1] = this._lateBoundMethodDefaultParameters[index];
    }
    return callParameters;
  }
}
