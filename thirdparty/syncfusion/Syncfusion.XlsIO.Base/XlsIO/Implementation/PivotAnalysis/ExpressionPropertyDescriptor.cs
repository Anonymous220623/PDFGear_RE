// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.ExpressionPropertyDescriptor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class ExpressionPropertyDescriptor : PropertyDescriptor
{
  private string expression;
  private string name;
  private FilterExpression exp;
  private string format;

  public ExpressionPropertyDescriptor(
    string name,
    Attribute[] attributes,
    string expression,
    string format,
    FilterHelper helper)
    : base(name, attributes)
  {
    this.name = name;
    this.expression = expression;
    this.format = format;
    this.exp = new FilterExpression(name, expression);
    if (this.exp.Error != ExpressionError.None)
      throw new ArgumentException($"Invalid expression: {this.exp.ErrorString}");
  }

  public string Expression
  {
    get => this.expression;
    set => this.expression = value;
  }

  public FilterExpression Exp
  {
    get => this.exp;
    set => this.exp = value;
  }

  public string Format
  {
    get => this.format;
    set => this.format = value;
  }

  public override bool IsReadOnly => true;

  public override Type PropertyType => typeof (object);

  public override Type ComponentType => typeof (object);

  public override bool CanResetValue(object component) => true;

  public override object GetValue(object component)
  {
    return this.format == null || this.format.Length <= 0 ? this.exp.ComputedValue(component) : (object) string.Format((IFormatProvider) CultureInfo.CurrentUICulture, this.format, this.exp.ComputedValue(component));
  }

  public override void ResetValue(object component)
  {
    this.exp = new FilterExpression(this.name, this.expression);
    if (this.exp != null && this.exp.Error != ExpressionError.None)
      throw new ArgumentException($"Invalid expression: {this.exp.ErrorString}");
  }

  public override void SetValue(object component, object value)
  {
  }

  public override bool ShouldSerializeValue(object component) => false;
}
