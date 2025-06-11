// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionLiteralExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Globalization;

#nullable disable
namespace NLog.Conditions;

internal sealed class ConditionLiteralExpression : ConditionExpression
{
  private readonly string _toStringValue;

  public ConditionLiteralExpression(object literalValue, string toStringValue = null)
  {
    this.LiteralValue = literalValue;
    this._toStringValue = toStringValue;
  }

  public object LiteralValue { get; private set; }

  public override string ToString()
  {
    if (this._toStringValue != null)
      return this._toStringValue;
    return this.LiteralValue == null ? "null" : Convert.ToString(this.LiteralValue, (IFormatProvider) CultureInfo.InvariantCulture);
  }

  protected override object EvaluateNode(LogEventInfo context) => this.LiteralValue;
}
