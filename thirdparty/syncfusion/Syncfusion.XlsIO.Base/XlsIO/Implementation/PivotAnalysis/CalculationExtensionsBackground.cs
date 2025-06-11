// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.CalculationExtensionsBackground
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

internal class CalculationExtensionsBackground
{
  private const char stringMarker = '\u0082';
  private const char compiledExpressionMarker = '\u0083';
  private const char geMarker = '\u0084';
  private const char leMarker = '\u0085';
  private const char neMarker = '\u0086';
  private const char andMarker = '\u0087';
  private const char orMarker = '\u0088';
  private const char startsWithMarker = '\u008A';
  private const char endsWithMarker = '\u008B';
  private const char containsMarker = '\u008C';
  private const char dayMarker = '\u008D';
  private const char weekMarker = '\u008E';
  private const char monthMarker = '\u008F';
  private const char quarterMarker = '\u0090';
  private const char yearMarker = '\u0091';
  private const char inMarker = '\u0092';
  private const char plusMarker = '\u0093';
  private const char minusMarker = '\u0094';
  private const char multMarker = '\u0095';
  private const char divideMarker = '\u0096';
  private const char powerMarker = '\u0097';
  private const char modMarker = '\u0098';
  private const char greaterMarker = '\u0099';
  private const char lesserMarker = '\u009A';
  private const char equalMarker = '\u009B';
  private const char toStringMarker = '\u009C';
  private const char formatMarker = '\u009D';
  private const char maxMarker = '\u009E';
  private const char minMarker = '\u009F';
  private const char quoteMarker = '"';
  private const char leftBracket = '[';
  private const char rightBracket = ']';
  private const char leftParen = '(';
  private const char rightParen = ')';
  internal static string ErrorString = "";
  private Dictionary<string, string> strings = new Dictionary<string, string>();
  private Dictionary<string, Expression> expressions = new Dictionary<string, Expression>();
  private char[] allOperations = new char[27]
  {
    '\u0084',
    '\u0085',
    '\u0086',
    '\u0087',
    '\u0088',
    '\u0099',
    '\u009A',
    '\u008A',
    '\u008B',
    '\u008C',
    '\u008D',
    '\u008E',
    '\u008F',
    '\u0090',
    '\u0091',
    '\u0092',
    '\u0093',
    '\u0094',
    '\u0095',
    '\u0096',
    '\u0097',
    '\u0098',
    '\u009B',
    '\u009C',
    '\u009E',
    '\u009F',
    '\u009D'
  };
  private char[] unaryOperations = new char[5]
  {
    '\u008D',
    '\u008E',
    '\u008F',
    '\u0090',
    '\u0091'
  };
  private char listSeparator = ',';

  public Dictionary<string, Type> DynamicPropertyTypeTable { get; set; }

  public char ListSeparator
  {
    get => this.listSeparator;
    set => this.listSeparator = value;
  }

  internal PivotEngine Engine { get; set; }

  internal string FieldName { get; set; }

  public Delegate GetCompiledExpression(
    object source,
    bool caseSensitive,
    string formula,
    out ExpressionError error)
  {
    return this.GetCompiledExpression(source, caseSensitive, formula, out error, string.Empty);
  }

  public object GetDynamicValue(object o, string property)
  {
    if (this.Engine == null)
      return (object) null;
    return o is IDictionary<string, object> dictionary ? dictionary[property] : (object) null;
  }

  public double GetDictionaryDoubleValue(IDictionary o, string property)
  {
    return this.Engine == null || o == null || o[(object) property] == null ? double.NaN : (double) o[(object) property];
  }

  public bool CheckInList(object s, BinaryList list)
  {
    return this.Engine != null && list.BinarySearch((IComparable) s.ToString()) > -1;
  }

  internal Delegate GetCompiledExpression(
    object source,
    bool caseSensitive,
    string formula,
    out ExpressionError error,
    string fieldName)
  {
    this.FieldName = fieldName;
    error = ExpressionError.None;
    Type type = source.GetType();
    ParameterExpression paramExp = Expression.Parameter(type, type.Name);
    formula = this.TokenizeStrings(formula, ref error);
    StringBuilder stringBuilder1 = new StringBuilder(formula).Replace(" OR ", '\u0088'.ToString()).Replace(" AND ", '\u0087'.ToString()).Replace(" >= ", '\u0084'.ToString()).Replace(" <= ", '\u0085'.ToString()).Replace(" <> ", '\u0086'.ToString()).Replace(" || ", '\u0088'.ToString());
    StringBuilder stringBuilder2 = (formula.Contains(fieldName) ? stringBuilder1.Replace(" = ", '\u009B'.ToString()) : stringBuilder1.Replace(" = ", '\u009B'.ToString()).Replace('['.ToString(), "").Replace(']'.ToString(), "")).Replace(" STARTSWITH ", '\u008A'.ToString()).Replace(" StartsWith ", '\u008A'.ToString()).Replace(" startswith ", '\u008A'.ToString()).Replace(" TOSTRING ", '\u009C'.ToString()).Replace(" ToString ", '\u009C'.ToString()).Replace(" tostring ", '\u009C'.ToString()).Replace(" ENDSWITH ", '\u008B'.ToString()).Replace(" EndsWith ", '\u008B'.ToString()).Replace(" endswith ", '\u008B'.ToString()).Replace(" CONTAINS ", '\u008C'.ToString()).Replace(" Contains ", '\u008C'.ToString()).Replace(" contains ", '\u008C'.ToString()).Replace("DAY(", '\u008D'.ToString()).Replace("Day(", '\u008D'.ToString()).Replace("day(", '\u008D'.ToString()).Replace("WEEK(", '\u008E'.ToString()).Replace("Week(", '\u008E'.ToString()).Replace("week(", '\u008E'.ToString()).Replace("MONTH(", '\u008F'.ToString()).Replace("Month(", '\u008F'.ToString()).Replace("month(", '\u008F'.ToString()).Replace("QUARTER(", '\u0090'.ToString()).Replace("Quarter(", '\u0090'.ToString()).Replace("quarter(", '\u0090'.ToString()).Replace("YEAR(", '\u0091'.ToString()).Replace("Year(", '\u0091'.ToString()).Replace("year(", '\u0091'.ToString()).Replace(" IN ", '\u0092'.ToString()).Replace(" In ", '\u0092'.ToString()).Replace(" in ", '\u0092'.ToString()).Replace(" ? ", '\u009D'.ToString()).Replace(" > ", '\u0099'.ToString()).Replace(" < ", '\u009A'.ToString());
    if (!formula.Contains(fieldName))
    {
      stringBuilder2.Replace(" + ", '\u0093'.ToString());
      stringBuilder2.Replace(" - ", '\u0094'.ToString());
      stringBuilder2.Replace(" * ", '\u0095'.ToString());
      stringBuilder2.Replace(" / ", '\u0096'.ToString());
      stringBuilder2.Replace(" ^ ", '\u0097'.ToString());
      stringBuilder2.Replace(" % ", '\u0098'.ToString());
    }
    if (stringBuilder2.ToString().Contains('\u0081'.ToString()))
      stringBuilder2.Replace('\u0081'.ToString(), " ");
    formula = stringBuilder2.ToString();
    int length1 = formula.IndexOfAny(this.unaryOperations);
    while (length1 > -1 && length1 < formula.Length && error == ExpressionError.None)
    {
      int length2 = formula.IndexOf(')', length1 + 1);
      if (length2 == -1)
      {
        error = ExpressionError.MismatchedParentheses;
      }
      else
      {
        string str = formula.Substring(0, length2);
        if (length2 < formula.Length - 1)
          str += formula.Substring(length2 + 1);
        formula = str;
      }
      if (length2 + 1 < formula.Length)
      {
        length1 = formula.Substring(length2 + 1).IndexOfAny(this.unaryOperations);
        if (length1 > -1)
          length1 += length2 + 1;
      }
      else
        length1 = -1;
    }
    if (!formula.Contains(fieldName))
      length1 = formula.IndexOf(')');
    for (; length1 > -1 && length1 < formula.Length && error == ExpressionError.None; length1 = formula.IndexOf(')'))
    {
      int length3 = formula.Substring(0, length1).LastIndexOf('(');
      if (length3 == -1)
      {
        error = ExpressionError.MismatchedParentheses;
      }
      else
      {
        string formula1 = formula.Substring(length3 + 1, length1 - length3 - 1);
        string simpleExpression = this.GetSimpleExpression(source, caseSensitive, formula1, paramExp, ref error);
        string str1 = "";
        if (length3 > 0)
          str1 = formula.Substring(0, length3);
        string str2 = str1 + simpleExpression;
        if (length1 < formula.Length - 1)
          str2 += formula.Substring(length1 + 1);
        formula = str2;
      }
    }
    if (error == ExpressionError.None)
    {
      string simpleExpression = this.GetSimpleExpression(source, caseSensitive, formula, paramExp, ref error);
      if (simpleExpression == null && error == ExpressionError.None)
      {
        Expression expressionPiece = this.GetExpressionPiece(source, caseSensitive, paramExp, formula, ref error, (object) null);
        if (expressionPiece != null)
        {
          LambdaExpression lambdaExpression = Expression.Lambda(expressionPiece, paramExp);
          this.strings.Clear();
          return lambdaExpression.Compile();
        }
      }
      if (error == ExpressionError.None)
      {
        LambdaExpression lambdaExpression = Expression.Lambda(this.expressions[simpleExpression], paramExp);
        this.strings.Clear();
        return lambdaExpression.Compile();
      }
    }
    if (error == ExpressionError.None)
      error = ExpressionError.NotAValidFormula;
    this.strings.Clear();
    return (Delegate) null;
  }

  private string GetSimpleExpression(
    object source,
    bool caseSensitive,
    string formula,
    ParameterExpression paramExp,
    ref ExpressionError error)
  {
    bool expression = this.CompileToExpression(source, paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[1]
    {
      '\u0092'
    }, this.allOperations, out error);
    if (!expression && error == ExpressionError.None)
    {
      expression = this.CompileToExpression(source, paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[10]
      {
        '\u009D',
        '\u009C',
        '\u008A',
        '\u008B',
        '\u008C',
        '\u008D',
        '\u008E',
        '\u008F',
        '\u0090',
        '\u0091'
      }, this.allOperations, out error);
      if (!expression && error == ExpressionError.None)
      {
        expression = this.CompileToExpression(source, paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[2]
        {
          '\u0095',
          '\u0096'
        }, this.allOperations, out error);
        if (!expression && error == ExpressionError.None && !expression && error == ExpressionError.None)
        {
          expression = this.CompileToExpression(source, paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[2]
          {
            '\u0093',
            '\u0094'
          }, this.allOperations, out error);
          if (!expression && error == ExpressionError.None)
          {
            expression = this.CompileToExpression(source, paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[2]
            {
              '\u0097',
              '\u0098'
            }, this.allOperations, out error);
            if (!expression && error == ExpressionError.None)
            {
              expression = this.CompileToExpression(source, paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[6]
              {
                '\u0084',
                '\u0085',
                '\u0086',
                '\u009A',
                '\u0099',
                '\u009B'
              }, this.allOperations, out error);
              if (!expression && error == ExpressionError.None)
                expression = this.CompileToExpression(source, paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[2]
                {
                  '\u0087',
                  '\u0088'
                }, this.allOperations, out error);
            }
          }
        }
      }
    }
    return expression && error == ExpressionError.None ? formula : (string) null;
  }

  private bool CompileToExpression(
    object source,
    ParameterExpression paramExp,
    bool caseSensitive,
    ref string formula,
    char[] operations,
    char[] allOperations,
    out ExpressionError error)
  {
    error = ExpressionError.None;
    int num1 = 0;
    while (num1 > -1 && num1 < formula.Length)
    {
      num1 = formula.IndexOfAny(operations);
      if (num1 > -1)
      {
        int num2 = formula.Substring(0, num1).LastIndexOfAny(allOperations);
        int startIndex = formula.IndexOfAny(allOperations, num1 + 1);
        if (startIndex == -1)
          startIndex = formula.Length;
        string key;
        if (((IEnumerable<char>) this.unaryOperations).Contains<char>(formula[num1]))
        {
          string left = "";
          string str = formula.Substring(num1 + 1, startIndex - num1 - 1).Trim();
          if (paramExp.Type.Name == "ExpandoObject" && this.DynamicPropertyTypeTable != null && this.DynamicPropertyTypeTable.ContainsKey(str))
          {
            string funcName = "GetDynamicValue";
            Expression expression = (Expression) Expression.Call((Expression) null, ((IEnumerable<MethodInfo>) typeof (CalculationExtensions).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName)).FirstOrDefault<MethodInfo>(), new Expression[2]
            {
              (Expression) paramExp,
              (Expression) Expression.Constant((object) str)
            });
            str = '\u0083'.ToString() + this.expressions.Count.ToString() + (object) '\u0083';
            this.expressions.Add(str, expression);
          }
          Expression expression1 = this.GetExpression(source, caseSensitive, paramExp, left, str, formula[num1], ref error);
          key = '\u0083'.ToString() + this.expressions.Count.ToString() + (object) '\u0083';
          this.expressions.Add(key, expression1);
        }
        else
        {
          string str1 = formula.Substring(num2 + 1, num1 - num2 - 1).Trim();
          string str2 = formula.Substring(num1 + 1, startIndex - num1 - 1);
          IDictionary o = source as IDictionary;
          if (paramExp.Type.Name == "ExpandoObject" && this.DynamicPropertyTypeTable != null && this.DynamicPropertyTypeTable.ContainsKey(str1))
          {
            string funcName = "GetDynamicValue";
            Expression expression = (Expression) Expression.Call((Expression) null, ((IEnumerable<MethodInfo>) typeof (CalculationExtensions).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName)).FirstOrDefault<MethodInfo>(), new Expression[2]
            {
              (Expression) paramExp,
              (Expression) Expression.Constant((object) str1)
            });
            str1 = '\u0083'.ToString() + this.expressions.Count.ToString() + (object) '\u0083';
            this.expressions.Add(str1, expression);
          }
          else if (o != null)
          {
            if (o.Contains((object) str2))
            {
              Expression lookUpExpression = this.GetDictionaryLookUpExpression(paramExp, str2);
              str2 = '\u0083'.ToString() + this.expressions.Count.ToString() + (object) '\u0083';
              this.expressions.Add(str2, lookUpExpression);
            }
            if (o.Contains((object) str1))
            {
              Expression lookUpExpression = this.GetDictionaryLookUpExpression(paramExp, str1);
              str1 = '\u0083'.ToString() + this.expressions.Count.ToString() + (object) '\u0083';
              this.expressions.Add(str1, lookUpExpression);
            }
          }
          Expression expression2 = this.GetExpression(source, caseSensitive, paramExp, str1, str2, formula[num1], ref error);
          key = '\u0083'.ToString() + this.expressions.Count.ToString() + (object) '\u0083';
          this.expressions.Add(key, expression2);
          this.GetDictionaryDoubleValue(o, str1);
        }
        string str3 = "";
        if (num2 > 0)
          str3 = formula.Substring(0, num2 + 1);
        string str4 = str3 + key;
        if (startIndex < formula.Length - 1)
          str4 += formula.Substring(startIndex);
        formula = str4;
        num1 = 0;
      }
    }
    return formula.StartsWith('\u0083'.ToString()) && formula.EndsWith('\u0083'.ToString()) && formula.IndexOf('\u0083', 1, formula.Length - 2) == -1;
  }

  private Expression GetExpression(
    object source,
    bool caseSensitive,
    ParameterExpression paramExp,
    string left,
    string right,
    char operand,
    ref ExpressionError error)
  {
    if (operand == '\u009D')
      operand = '\u009C';
    Expression expressionPiece = this.GetExpressionPiece(source, caseSensitive, paramExp, left, ref error, (object) null);
    Expression rightExp = this.GetExpressionPiece(source, caseSensitive, paramExp, right, ref error, expressionPiece == null ? (object) (Type) null : (object) expressionPiece.Type);
    if (!((IEnumerable<char>) this.unaryOperations).Contains<char>(operand) && operand != '\u009C')
      this.CoerceType(ref expressionPiece, ref rightExp, ref error);
    Expression left1 = (Expression) null;
    if (error == ExpressionError.None)
    {
      try
      {
        switch (operand)
        {
          case '\u0084':
            left1 = (Expression) Expression.GreaterThanOrEqual(expressionPiece, rightExp);
            break;
          case '\u0085':
            left1 = (Expression) Expression.LessThanOrEqual(expressionPiece, rightExp);
            break;
          case '\u0086':
            left1 = (Expression) Expression.NotEqual(expressionPiece, rightExp);
            break;
          case '\u0087':
            left1 = (Expression) Expression.And(expressionPiece, rightExp);
            break;
          case '\u0088':
            left1 = (Expression) Expression.Or(expressionPiece, rightExp);
            break;
          case '\u008A':
          case '\u008B':
          case '\u008C':
            string funcName1 = this.GetFunctionName(operand);
            if (funcName1.Length > 0)
            {
              MethodInfo method = ((IEnumerable<MethodInfo>) typeof (string).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName1)).FirstOrDefault<MethodInfo>();
              if (!(expressionPiece.Type == typeof (string)))
                throw new InvalidOperationException("Underlying type is not a string");
              left1 = (Expression) Expression.Call(expressionPiece, method, rightExp);
              break;
            }
            break;
          case '\u008D':
          case '\u008E':
          case '\u008F':
          case '\u0091':
            string funcName2 = this.GetFunctionName(operand);
            if (funcName2.Length > 0)
            {
              PropertyInfo property = ((IEnumerable<PropertyInfo>) typeof (DateTime).GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (m => m.Name == funcName2)).FirstOrDefault<PropertyInfo>();
              Type type = rightExp.Type;
              if (this.DynamicPropertyTypeTable != null)
              {
                rightExp = (Expression) Expression.Convert(rightExp, typeof (DateTime));
                type = rightExp.Type;
              }
              if (!(type == typeof (DateTime)))
                throw new InvalidOperationException("Underlying type is not a DateTime");
              left1 = (Expression) Expression.Property(rightExp, property);
              break;
            }
            break;
          case '\u0090':
            PropertyInfo property1 = ((IEnumerable<PropertyInfo>) typeof (DateTime).GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (m => m.Name == "Month")).FirstOrDefault<PropertyInfo>();
            Type type1 = rightExp.Type;
            if (this.DynamicPropertyTypeTable != null)
            {
              rightExp = (Expression) Expression.Convert(rightExp, typeof (DateTime));
              type1 = rightExp.Type;
            }
            if (!(type1 == typeof (DateTime)))
              throw new InvalidOperationException("Underlying type is not a DateTime");
            left1 = (Expression) Expression.Property(rightExp, property1);
            left1 = (Expression) Expression.Subtract(left1, (Expression) Expression.Constant((object) 1));
            left1 = (Expression) Expression.Divide(left1, (Expression) Expression.Constant((object) 3));
            left1 = (Expression) Expression.Add(left1, (Expression) Expression.Constant((object) 1));
            break;
          case '\u0092':
            string s = rightExp.ToString();
            BinaryList lookUpList = this.GetLookUpList(s.Substring(1, s.Length - 2));
            this.CheckInList((object) s, lookUpList);
            string funcName3 = "CheckInList";
            MethodInfo method1 = ((IEnumerable<MethodInfo>) typeof (CalculationExtensions).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName3)).FirstOrDefault<MethodInfo>();
            if (!(expressionPiece.Type == typeof (string)))
              throw new InvalidOperationException("Underlying type is not a string");
            left1 = (Expression) Expression.Call((Expression) null, method1, new Expression[2]
            {
              expressionPiece,
              (Expression) Expression.Constant((object) lookUpList)
            });
            break;
          case '\u0093':
            left1 = (Expression) Expression.Add(expressionPiece, rightExp);
            break;
          case '\u0094':
            left1 = (Expression) Expression.Subtract(expressionPiece, rightExp);
            break;
          case '\u0095':
            left1 = (Expression) Expression.Multiply(expressionPiece, rightExp);
            break;
          case '\u0096':
            left1 = (Expression) Expression.Divide(expressionPiece, rightExp);
            break;
          case '\u0097':
            left1 = (Expression) Expression.Power(expressionPiece, rightExp);
            break;
          case '\u0098':
            left1 = (Expression) Expression.Modulo(expressionPiece, rightExp);
            break;
          case '\u0099':
            left1 = (Expression) Expression.GreaterThan(expressionPiece, rightExp);
            break;
          case '\u009A':
            left1 = (Expression) Expression.LessThan(expressionPiece, rightExp);
            break;
          case '\u009B':
            left1 = (Expression) Expression.Equal(expressionPiece, rightExp);
            break;
          case '\u009C':
            string funcName4 = this.GetFunctionName(operand);
            if (funcName4.Length > 0)
            {
              MethodInfo method2 = ((IEnumerable<MethodInfo>) expressionPiece.Type.GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName4 && ((IEnumerable<ParameterInfo>) m.GetParameters()).Count<ParameterInfo>() == 1)).FirstOrDefault<MethodInfo>();
              left1 = (Expression) Expression.Call(expressionPiece, method2, (Expression) Expression.Constant((object) right, typeof (string)));
              break;
            }
            break;
          default:
            error = ExpressionError.UnknownOperator;
            break;
        }
      }
      catch (Exception ex)
      {
        error = ExpressionError.ExceptionRaised;
        CalculationExtensionsBackground.ErrorString = string.IsNullOrEmpty(CalculationExtensionsBackground.ErrorString) ? ex.Message : CalculationExtensionsBackground.ErrorString;
      }
    }
    return left1;
  }

  private BinaryList GetLookUpList(string item)
  {
    BinaryList lookUpList = new BinaryList();
    this.ListSeparator = ',';
    string str = item;
    char[] chArray = new char[1]{ this.ListSeparator };
    foreach (string o in str.Split(chArray))
      lookUpList.AddIfUnique((IComparable) o);
    return lookUpList;
  }

  private string GetFunctionName(char c)
  {
    if (this.Engine == null)
      return (string) null;
    string functionName = "";
    switch (c)
    {
      case '\u008A':
        functionName = "StartsWith";
        break;
      case '\u008B':
        functionName = "EndsWith";
        break;
      case '\u008C':
        functionName = "Contains";
        break;
      case '\u008D':
        functionName = "Day";
        break;
      case '\u008E':
        functionName = "Week";
        break;
      case '\u008F':
        functionName = "Month";
        break;
      case '\u0090':
        functionName = "Quarter";
        break;
      case '\u0091':
        functionName = "Year";
        break;
      case '\u009C':
        functionName = "ToString";
        break;
    }
    return functionName;
  }

  private void CoerceType(
    ref Expression leftExp,
    ref Expression rightExp,
    ref ExpressionError error)
  {
    if (this.Engine == null || !(leftExp.Type != rightExp.Type))
      return;
    if (leftExp.Type == typeof (double) && (rightExp.Type == typeof (int) || rightExp.Type == typeof (float)))
      rightExp = (Expression) Expression.Convert(rightExp, typeof (double));
    else if (rightExp.Type == typeof (double) && (leftExp.Type == typeof (int) || leftExp.Type == typeof (float)) || rightExp.Type == typeof (double) && rightExp.ToString() == "0" && (leftExp.Type == typeof (long?) || leftExp.Type == typeof (int?) || leftExp.Type == typeof (float?) || leftExp.Type == typeof (double?) || leftExp.Type == typeof (short?) || leftExp.Type == typeof (Decimal?)))
    {
      leftExp = (Expression) Expression.Convert(leftExp, typeof (double));
    }
    else
    {
      if (rightExp.Type == typeof (double))
      {
        if (!(leftExp.Type == typeof (long?)) && !(leftExp.Type == typeof (int?)) && !(leftExp.Type == typeof (float?)) && !(leftExp.Type == typeof (double?)) && !(leftExp.Type == typeof (short?)))
        {
          if (!(leftExp.Type == typeof (Decimal?)))
            goto label_9;
        }
        rightExp = (Expression) Expression.Convert(rightExp, leftExp.Type);
        return;
      }
label_9:
      try
      {
        bool flag = true;
        TypeConverter converter1 = TypeDescriptor.GetConverter(leftExp.Type);
        if (converter1 != null && converter1.CanConvertTo(rightExp.Type))
        {
          leftExp = (Expression) Expression.Convert(leftExp, rightExp.Type);
          flag = false;
        }
        else
        {
          TypeConverter converter2 = TypeDescriptor.GetConverter(rightExp.Type);
          if (converter2 != null && converter2.CanConvertTo(leftExp.Type))
          {
            if (rightExp.Type.IsValueType && leftExp.NodeType == ExpressionType.Call && ((MethodCallExpression) leftExp).Method.Name == "ToString")
            {
              string funcName = "ToString";
              Type type = rightExp.Type;
              if (((MethodCallExpression) leftExp).Object.Type == type || ((MethodCallExpression) leftExp).Object.Type != typeof (DateTime))
              {
                MethodInfo method = ((IEnumerable<MethodInfo>) type.GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName && ((IEnumerable<ParameterInfo>) m.GetParameters()).Count<ParameterInfo>() == 1)).FirstOrDefault<MethodInfo>();
                rightExp = (Expression) Expression.Call(rightExp, method, (IEnumerable<Expression>) ((MethodCallExpression) leftExp).Arguments);
              }
              else
              {
                MethodInfo method = ((IEnumerable<MethodInfo>) type.GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName && ((IEnumerable<ParameterInfo>) m.GetParameters()).Count<ParameterInfo>() == 0)).FirstOrDefault<MethodInfo>();
                rightExp = (Expression) Expression.Call(rightExp, method);
              }
            }
            else if (leftExp.Type == typeof (string))
            {
              string funcName = "ToString";
              MethodInfo method = ((IEnumerable<MethodInfo>) rightExp.Type.GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName && ((IEnumerable<ParameterInfo>) m.GetParameters()).Count<ParameterInfo>() == 0)).FirstOrDefault<MethodInfo>();
              rightExp = (Expression) Expression.Call(rightExp, method);
            }
            else
              rightExp = (Expression) Expression.Convert(rightExp, leftExp.Type);
            flag = false;
          }
        }
        if (!flag)
          return;
        error = ExpressionError.CannotCompareDifferentTypes;
      }
      catch (Exception ex)
      {
        if (leftExp.NodeType != ExpressionType.Constant && rightExp.NodeType == ExpressionType.Constant)
        {
          if (leftExp.Type == typeof (DateTime))
          {
            DateTime result;
            if (DateTime.TryParseExact(rightExp is ConstantExpression ? (rightExp as ConstantExpression).Value.ToString() : rightExp.ToString(), new string[49]
            {
              "yyyyy",
              "yyyy",
              "yyy",
              "yy",
              "y",
              "MMMM",
              "MMM",
              "MM",
              "M",
              "ddd",
              "dd",
              "d",
              "G",
              "gg",
              "g",
              "HH",
              "hh",
              "mm",
              "m",
              "ss",
              "s",
              "tt",
              "t",
              "T",
              "u",
              "U",
              "zzz",
              "zz",
              "z",
              "FFFFFFF",
              "FFFFFF",
              "FFFFF",
              "FFFF",
              "FFF",
              "FF",
              "F",
              "fffffff",
              "ffffff",
              "fffff",
              "ffff",
              "fff",
              "ff",
              "f",
              "o",
              "O",
              "r",
              "R",
              "/",
              ":"
            }, (IFormatProvider) null, DateTimeStyles.None, out result))
            {
              rightExp = (Expression) Expression.Constant((object) result, leftExp.Type);
              error = ExpressionError.None;
              return;
            }
          }
          rightExp = (Expression) Expression.Constant(Convert.ChangeType((object) rightExp.ToString(), leftExp.Type, (IFormatProvider) null));
          error = ExpressionError.None;
        }
        else if (leftExp.NodeType == ExpressionType.Constant && rightExp.NodeType != ExpressionType.Constant)
        {
          leftExp = (Expression) Expression.Constant(Convert.ChangeType((object) leftExp.ToString(), rightExp.Type));
          error = ExpressionError.None;
        }
        else
        {
          error = ExpressionError.ExceptionRaised;
          CalculationExtensionsBackground.ErrorString = string.IsNullOrEmpty(CalculationExtensionsBackground.ErrorString) ? ex.Message : CalculationExtensionsBackground.ErrorString;
        }
      }
    }
  }

  private Expression GetExpressionPiece(
    object source,
    bool caseSensitive,
    ParameterExpression paramExp,
    string piece,
    ref ExpressionError error,
    object leftExpType)
  {
    if (piece.StartsWith('\u0083'.ToString()) && this.expressions.ContainsKey(piece))
      return this.expressions[piece];
    Type type1 = source.GetType();
    Type type2 = (Type) leftExpType;
    if (source is IDictionary dictionary)
    {
      if (leftExpType == null)
        leftExpType = (object) (type2 = typeof (double));
      if (dictionary.Contains((object) piece))
        return this.GetDictionaryLookUpExpression(paramExp, piece);
    }
    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type1);
    if (!caseSensitive && properties[piece] == null)
    {
      string lower = piece.ToLower();
      foreach (PropertyDescriptor propertyDescriptor in properties)
      {
        if (lower == propertyDescriptor.Name.ToLower())
        {
          piece = propertyDescriptor.Name;
          break;
        }
      }
    }
    if (properties[piece] != null && (this.Engine.PivotCalculations.Any<PivotComputationInfo>((Func<PivotComputationInfo, bool>) (i => i.FieldName == this.FieldName)) ? 1 : (leftExpType == null ? 1 : 0)) != 0)
      return (Expression) Expression.PropertyOrField((Expression) paramExp, piece);
    if (type2 == typeof (float) || type2 == typeof (long) || type2 == typeof (short) || type2 == typeof (Decimal) || type2 == typeof (double) || type2 == typeof (int) || type2 == typeof (int?) || type2 == typeof (float?) || type2 == typeof (double?) || type2 == typeof (short?) || type2 == typeof (long?) || type2 == typeof (Decimal?))
    {
      double result = 0.0;
      if (piece == "null" && (type2 == typeof (int?) || type2 == typeof (float?) || type2 == typeof (double?) || type2 == typeof (short?) || type2 == typeof (long?) || type2 == typeof (Decimal?)))
        piece = "0";
      if (paramExp.Name != "ExpandoObject" && double.TryParse(piece, out result))
        return (Expression) Expression.Constant((object) result);
    }
    if (type2 == typeof (bool) || type2 == typeof (bool?))
      return (Expression) Expression.Constant((object) (bool) (piece == "True" || piece == "null" ? (true ? 1 : 0) : (false ? 1 : 0)));
    if (piece.IndexOf('\u009D') > -1)
      piece = piece.Replace('\u009D'.ToString(), "");
    for (int startIndex = piece.IndexOf('\u0082'); startIndex > -1; startIndex = piece.IndexOf('\u0082'))
    {
      int num = piece.IndexOf('\u0082', startIndex + 1);
      string str = piece.Substring(startIndex, num - startIndex + 1);
      piece = piece.Replace(str, this.strings[str]);
    }
    if (string.IsNullOrEmpty(piece))
      source = this.GetDynamicValue(source, type1.Name);
    return (Expression) Expression.Constant((object) piece);
  }

  private Expression GetDictionaryLookUpExpression(ParameterExpression paramExp, string piece)
  {
    if (this.Engine == null)
      return (Expression) null;
    string funcName = "GetDictionaryDoubleValue";
    return (Expression) Expression.Call((Expression) null, ((IEnumerable<MethodInfo>) typeof (CalculationExtensions).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName)).FirstOrDefault<MethodInfo>(), new Expression[2]
    {
      (Expression) paramExp,
      (Expression) Expression.Constant((object) piece)
    });
  }

  private string TokenizeStrings(string formula, ref ExpressionError error)
  {
    error = ExpressionError.None;
    int startIndex1 = 0;
    int num1 = 0;
    this.strings.Clear();
    StringBuilder stringBuilder = new StringBuilder();
    while (startIndex1 < formula.Length && startIndex1 > -1)
    {
      int startIndex2 = startIndex1;
      startIndex1 = formula.IndexOf('"', startIndex1);
      if (startIndex1 > -1)
      {
        stringBuilder.Append(formula.Substring(startIndex2, startIndex1 - startIndex2));
        int num2 = formula.IndexOf('"', startIndex1 + 1);
        if (num2 == -1)
        {
          error = ExpressionError.MissingRightQuote;
          break;
        }
        string key = '\u0082'.ToString() + num1.ToString() + (object) '\u0082';
        ++num1;
        this.strings.Add(key, formula.Substring(startIndex1, num2 - startIndex1 + 1));
        stringBuilder.Append(key);
        startIndex1 = num2 + 1;
      }
      else
        stringBuilder.Append(formula.Substring(startIndex2));
    }
    return stringBuilder.ToString();
  }
}
