// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.FilterExpression
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class FilterExpression
{
  private ExpressionError error;
  private string name = "";
  private string expression = "";
  private string dimensionName;
  private string format;
  private string fieldCaption;
  private Delegate evaluator;
  private bool caseSensitive = true;

  public FilterExpression()
  {
  }

  public FilterExpression(string dimensionName)
    : this(dimensionName, dimensionName, (string) null)
  {
  }

  public FilterExpression(string dimensionName, string dimensionHeader, string expression)
  {
    this.DimensionName = dimensionName;
    this.DimensionHeader = dimensionHeader;
    this.expression = expression;
  }

  public FilterExpression(
    string dimensionName,
    string dimensionHeader,
    string expression,
    string format)
  {
    this.DimensionName = dimensionName;
    this.DimensionHeader = dimensionHeader;
    this.expression = expression;
    this.Format = format;
  }

  public FilterExpression(string name, string expression)
  {
    this.expression = expression;
    this.name = this.DimensionName = this.DimensionHeader = name;
  }

  public string DimensionName
  {
    get => this.dimensionName;
    set
    {
      this.dimensionName = value;
      if (this.DimensionHeader != null)
        return;
      this.DimensionHeader = this.DimensionName;
    }
  }

  public string Format
  {
    get => this.format;
    set => this.format = value;
  }

  public string DimensionHeader { get; set; }

  public string FieldCaption
  {
    get => this.fieldCaption;
    set => this.fieldCaption = value;
  }

  [XmlIgnore]
  public Delegate Evaluator
  {
    get => this.evaluator;
    set => this.evaluator = value;
  }

  public ExpressionError Error => this.error;

  public string ErrorString
  {
    get
    {
      return this.error != ExpressionError.ExceptionRaised ? this.error.ToString() : CalculationExtensions.ErrorString;
    }
  }

  public string Name
  {
    get => this.name;
    set => this.name = value;
  }

  public string Expression
  {
    get => this.expression;
    set
    {
      this.evaluator = (Delegate) null;
      this.expression = value;
    }
  }

  [XmlIgnore]
  public object Tag { get; set; }

  public bool CaseSensitive
  {
    get => this.caseSensitive;
    set => this.caseSensitive = value;
  }

  public object ComputedValue(object component, bool loadInBackground, PivotEngine engine)
  {
    if ((object) this.evaluator == null || (object) this.evaluator != null && this.evaluator.Method != (MethodInfo) null && this.evaluator.Method.GetParameters()[1] != null && !this.evaluator.Method.GetParameters()[1].ToString().Contains(component.GetType().Name))
    {
      if (loadInBackground)
      {
        CalculationExtensionsBackground extensionsBackground = new CalculationExtensionsBackground();
        extensionsBackground.Engine = engine;
        this.evaluator = string.IsNullOrEmpty(this.name) ? extensionsBackground.GetCompiledExpression(component, this.CaseSensitive, this.expression, out this.error) : extensionsBackground.GetCompiledExpression(component, this.CaseSensitive, this.expression, out this.error, this.Name);
      }
      else
        this.evaluator = component.GetCompiledExpression(this.CaseSensitive, this.expression, out this.error, this.Name);
    }
    if ((object) this.evaluator != null)
    {
      try
      {
        return this.evaluator.DynamicInvoke(component);
      }
      catch
      {
      }
    }
    return (object) null;
  }

  public object ComputedValue(object component)
  {
    PropertyInfo property = component.GetType().GetProperty(this.name);
    if (property != (PropertyInfo) null)
    {
      if (component is DataRow)
      {
        if (DBNull.Value.Equals((component as DataRow)[this.name]))
          component.GetType().GetProperty(this.name).SetValue(component, (object) " ", (object[]) null);
      }
      else if (property.GetValue(component, (object[]) null) == null)
        component.GetType().GetProperty(this.name).SetValue(component, (object) " ", (object[]) null);
    }
    if ((object) this.evaluator == null || this.evaluator.Method != (MethodInfo) null && this.evaluator.Method.GetParameters()[1] != null && !this.evaluator.Method.GetParameters()[1].ToString().Contains(component.GetType().Name))
      this.evaluator = component.GetCompiledExpression(this.CaseSensitive, this.expression, out this.error, this.Name);
    if ((object) this.evaluator == null)
      return (object) null;
    if (this.name.Contains("."))
    {
      object obj = component.GetType().GetProperty(this.Name.Remove(this.Name.IndexOf("."))).GetValue(component, (object[]) null);
      if (this.DimensionHeader.Contains("."))
        this.DimensionHeader = ((IEnumerable<PropertyInfo>) obj.GetType().GetProperties()).Where<PropertyInfo>((System.Func<PropertyInfo, bool>) (x => this.Name.Contains(x.Name))).FirstOrDefault<PropertyInfo>().Name;
      return (object) this.expression.Contains(obj.GetType().GetProperty(this.DimensionHeader).GetValue(obj, (object[]) null).ToString());
    }
    return this.evaluator.DynamicInvoke(component);
  }
}
