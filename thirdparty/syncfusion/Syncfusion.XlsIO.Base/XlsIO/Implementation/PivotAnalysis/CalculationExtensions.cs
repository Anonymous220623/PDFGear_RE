// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.CalculationExtensions
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
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

internal static class CalculationExtensions
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
  private static Dictionary<string, string> strings = new Dictionary<string, string>();
  private static Dictionary<string, Expression> expressions = new Dictionary<string, Expression>();
  private static char[] allOperations = new char[27]
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
  private static char[] unaryOperations = new char[5]
  {
    '\u008D',
    '\u008E',
    '\u008F',
    '\u0090',
    '\u0091'
  };
  private static char[] mathOperations = new char[2]
  {
    '\u009E',
    '\u009F'
  };
  private static char[] conditionalOperations = new char[7]
  {
    '\u0084',
    '\u0085',
    '\u0086',
    '\u0087',
    '\u0088',
    '\u0099',
    '\u009A'
  };
  private static char listSeparator = ',';

  public static Dictionary<string, Type> DynamicPropertyTypeTable { get; set; }

  public static char ListSeparator
  {
    get => CalculationExtensions.listSeparator;
    set => CalculationExtensions.listSeparator = value;
  }

  internal static PivotEngine Engine { get; set; }

  internal static string FieldName { get; set; }

  public static Delegate GetCompiledExpression(
    this object source,
    bool caseSensitive,
    string formula,
    out ExpressionError error)
  {
    return source.GetCompiledExpression(caseSensitive, formula, out error, string.Empty);
  }

  public static object GetDynamicValue(object o, string property)
  {
    return o is IDictionary<string, object> dictionary ? dictionary[property] : (object) null;
  }

  public static double GetDictionaryDoubleValue(IDictionary o, string property)
  {
    return o != null && o[(object) property] != null ? (double) o[(object) property] : double.NaN;
  }

  public static bool CheckInList(object s, BinaryList list)
  {
    return list.BinarySearch((IComparable) s.ToString()) > -1;
  }

  internal static void Dispose()
  {
    CalculationExtensions.Engine = (PivotEngine) null;
    CalculationExtensions.FieldName = (string) null;
    if (CalculationExtensions.expressions != null)
    {
      CalculationExtensions.expressions.Clear();
      CalculationExtensions.expressions = (Dictionary<string, Expression>) null;
    }
    if (CalculationExtensions.strings == null)
      return;
    CalculationExtensions.strings.Clear();
    CalculationExtensions.strings = (Dictionary<string, string>) null;
  }

  internal static Delegate GetCompiledExpression(
    this object source,
    bool caseSensitive,
    string formula,
    out ExpressionError error,
    string fieldName)
  {
    CalculationExtensions.FieldName = fieldName;
    error = ExpressionError.None;
    CalculationExtensions.ErrorString = "";
    Type type = source.GetType();
    ParameterExpression paramExp = Expression.Parameter(type, type.Name);
    formula = CalculationExtensions.TokenizeStrings(formula, ref error);
    char[] delimiterChars = new char[7]
    {
      ' ',
      ',',
      '.',
      ':',
      ')',
      '(',
      '\t'
    };
    StringBuilder stringBuilder1 = new StringBuilder(formula).Replace(" OR ", '\u0088'.ToString()).Replace(" AND ", '\u0087'.ToString()).Replace(" >= ", '\u0084'.ToString()).Replace(" <= ", '\u0085'.ToString()).Replace(" <> ", '\u0086'.ToString()).Replace(" || ", '\u0088'.ToString()).Replace(">=", '\u0084'.ToString()).Replace("<=", '\u0085'.ToString()).Replace("<>", '\u0086'.ToString()).Replace("||", '\u0088'.ToString()).Replace("&&", '\u0087'.ToString());
    bool flag = CalculationExtensions.Engine.AllowedFields.Any<FieldInfo>((Func<FieldInfo, bool>) (i => i.Expression == formula));
    if (flag && (formula.ToUpper().Contains("SUM") || formula.ToUpper().Contains("PRODUCT(") || formula.ToUpper().Contains("PRODUCT (") || formula.ToUpper().Contains("POWER") || formula.ToUpper().Contains("AVERAGE") || formula.ToUpper().Contains("MAX") || formula.ToUpper().Contains("MIN")))
    {
      if (formula.ToList<char>().FindAll((Predicate<char>) (x => x.ToString() == "(")).Count > 1)
      {
        string str1 = "";
        string[] strArray = Regex.Split(formula, "\\W+");
        string str2 = stringBuilder1.Remove(0, strArray[0].Length + 1).Remove(stringBuilder1.Length - 1, 1).ToString();
        char[] chArray = new char[1]{ ')' };
        foreach (string str3 in str2.Split(chArray))
        {
          if (!string.IsNullOrEmpty(str3))
          {
            string str4 = str3[str3.Length - 1].ToString() != ")" ? str3.Insert(str3.Length, ")") : str3;
            string oldValue = str4[0].ToString() == "," ? str4.Remove(0, str4[1].ToString() == " " ? 2 : 1) : str4;
            StringBuilder functionalExpression = CalculationExtensions.GetMultiFunctionalExpression(new StringBuilder(oldValue), formula, delimiterChars);
            str1 = string.IsNullOrEmpty(str1) ? formula.Replace(oldValue, functionalExpression.ToString()) : str1.Replace(oldValue, functionalExpression.ToString());
          }
          else
            break;
        }
        stringBuilder1 = CalculationExtensions.GetMultiFunctionalExpression(new StringBuilder(str1), formula, delimiterChars);
      }
      else
        stringBuilder1 = CalculationExtensions.GetMultiFunctionalExpression(stringBuilder1, formula, delimiterChars);
    }
    if (flag && formula.ToUpper().Contains("ABS"))
      stringBuilder1 = stringBuilder1.Replace(formula.Split(delimiterChars)[0], "").Replace('('.ToString(), "").Replace(')'.ToString(), "");
    StringBuilder stringBuilder2 = (formula.Contains(fieldName) ? stringBuilder1.Replace(" = ", '\u009B'.ToString()) : stringBuilder1.Replace(" = ", '\u009B'.ToString()).Replace('['.ToString(), "").Replace(']'.ToString(), "")).Replace(" STARTSWITH ", '\u008A'.ToString()).Replace(" StartsWith ", '\u008A'.ToString()).Replace(" startswith ", '\u008A'.ToString()).Replace(" TOSTRING ", '\u009C'.ToString()).Replace(" ToString ", '\u009C'.ToString()).Replace(" tostring ", '\u009C'.ToString()).Replace(" ENDSWITH ", '\u008B'.ToString()).Replace(" EndsWith ", '\u008B'.ToString()).Replace(" endswith ", '\u008B'.ToString()).Replace(" CONTAINS ", '\u008C'.ToString()).Replace(" Contains ", '\u008C'.ToString()).Replace(" contains ", '\u008C'.ToString()).Replace("DAY(", '\u008D'.ToString()).Replace("Day(", '\u008D'.ToString()).Replace("day(", '\u008D'.ToString()).Replace("WEEK(", '\u008E'.ToString()).Replace("Week(", '\u008E'.ToString()).Replace("week(", '\u008E'.ToString());
    char ch = '\u008F';
    string newValue1 = ch.ToString();
    StringBuilder stringBuilder3 = stringBuilder2.Replace("MONTH(", newValue1);
    ch = '\u008F';
    string newValue2 = ch.ToString();
    StringBuilder stringBuilder4 = stringBuilder3.Replace("Month(", newValue2);
    ch = '\u008F';
    string newValue3 = ch.ToString();
    StringBuilder stringBuilder5 = stringBuilder4.Replace("month(", newValue3);
    ch = '\u0090';
    string newValue4 = ch.ToString();
    StringBuilder stringBuilder6 = stringBuilder5.Replace("QUARTER(", newValue4);
    ch = '\u0090';
    string newValue5 = ch.ToString();
    StringBuilder stringBuilder7 = stringBuilder6.Replace("Quarter(", newValue5);
    ch = '\u0090';
    string newValue6 = ch.ToString();
    StringBuilder stringBuilder8 = stringBuilder7.Replace("quarter(", newValue6);
    ch = '\u0091';
    string newValue7 = ch.ToString();
    StringBuilder stringBuilder9 = stringBuilder8.Replace("YEAR(", newValue7);
    ch = '\u0091';
    string newValue8 = ch.ToString();
    StringBuilder stringBuilder10 = stringBuilder9.Replace("Year(", newValue8);
    ch = '\u0091';
    string newValue9 = ch.ToString();
    StringBuilder stringBuilder11 = stringBuilder10.Replace("year(", newValue9);
    ch = '\u0092';
    string newValue10 = ch.ToString();
    StringBuilder stringBuilder12 = stringBuilder11.Replace(" IN ", newValue10);
    ch = '\u0092';
    string newValue11 = ch.ToString();
    StringBuilder stringBuilder13 = stringBuilder12.Replace(" In ", newValue11);
    ch = '\u0092';
    string newValue12 = ch.ToString();
    StringBuilder stringBuilder14 = stringBuilder13.Replace(" in ", newValue12);
    ch = '\u009D';
    string newValue13 = ch.ToString();
    StringBuilder stringBuilder15 = stringBuilder14.Replace(" ? ", newValue13);
    ch = '\u0099';
    string newValue14 = ch.ToString();
    StringBuilder stringBuilder16 = stringBuilder15.Replace(" > ", newValue14);
    ch = '\u009A';
    string newValue15 = ch.ToString();
    StringBuilder stringBuilder17 = stringBuilder16.Replace(" < ", newValue15);
    if (!formula.Contains(fieldName))
    {
      StringBuilder stringBuilder18 = stringBuilder17;
      ch = '\u0093';
      string newValue16 = ch.ToString();
      stringBuilder18.Replace(" + ", newValue16);
      StringBuilder stringBuilder19 = stringBuilder17;
      ch = '\u0094';
      string newValue17 = ch.ToString();
      stringBuilder19.Replace(" - ", newValue17);
      StringBuilder stringBuilder20 = stringBuilder17;
      ch = '\u0095';
      string newValue18 = ch.ToString();
      stringBuilder20.Replace(" * ", newValue18);
      StringBuilder stringBuilder21 = stringBuilder17;
      ch = '\u0096';
      string newValue19 = ch.ToString();
      stringBuilder21.Replace(" / ", newValue19);
      StringBuilder stringBuilder22 = stringBuilder17;
      ch = '\u0097';
      string newValue20 = ch.ToString();
      stringBuilder22.Replace(" ^ ", newValue20);
      StringBuilder stringBuilder23 = stringBuilder17;
      ch = '\u0098';
      string newValue21 = ch.ToString();
      stringBuilder23.Replace(" % ", newValue21);
    }
    string str5 = stringBuilder17.ToString();
    ch = '\u0081';
    string str6 = ch.ToString();
    if (str5.Contains(str6))
    {
      StringBuilder stringBuilder24 = stringBuilder17;
      ch = '\u0081';
      string oldValue = ch.ToString();
      stringBuilder24.Replace(oldValue, " ");
    }
    formula = stringBuilder17.ToString();
    int num = formula.IndexOfAny(CalculationExtensions.unaryOperations);
    while (num > -1 && num < formula.Length && error == ExpressionError.None)
    {
      int length = formula.IndexOf(')', num + 1);
      if (length == -1)
      {
        error = ExpressionError.MismatchedParentheses;
      }
      else
      {
        string str7 = formula.Substring(0, length);
        if (length < formula.Length - 1)
          str7 += formula.Substring(length + 1);
        formula = str7;
      }
      if (length + 1 < formula.Length)
      {
        num = formula.Substring(length + 1).IndexOfAny(CalculationExtensions.unaryOperations);
        if (num > -1)
          num += length + 1;
      }
      else
        num = -1;
    }
    int length1;
    for (length1 = formula.IndexOfAny(CalculationExtensions.mathOperations); length1 > -1 && length1 < formula.Length && error == ExpressionError.None; --length1)
    {
      if (formula.IndexOf('(', length1 + 1) == -1)
      {
        error = ExpressionError.MismatchedParentheses;
      }
      else
      {
        string str8 = "";
        if (formula[0] == '\u009E')
          str8 = "Math.Max" + formula;
        if (formula[0] == '\u009F')
          str8 = "Math.Min" + formula;
        formula = str8;
      }
    }
    if (formula.IndexOfAny(CalculationExtensions.mathOperations) == -1 && !formula.Contains(fieldName))
      length1 = formula.IndexOf(')');
    for (; length1 > -1 && length1 < formula.Length && error == ExpressionError.None; length1 = formula.IndexOf(')'))
    {
      int length2 = formula.Substring(0, length1).LastIndexOf('(');
      if (length2 == -1)
      {
        error = ExpressionError.MismatchedParentheses;
      }
      else
      {
        string formula1 = formula.Substring(length2 + 1, length1 - length2 - 1);
        string simpleExpression = source.GetSimpleExpression(caseSensitive, formula1, paramExp, ref error);
        string str9 = "";
        if (length2 > 0)
          str9 = formula.Substring(0, length2);
        string str10 = str9 + simpleExpression;
        if (length1 < formula.Length - 1)
          str10 += formula.Substring(length1 + 1);
        formula = str10;
      }
    }
    if (error == ExpressionError.None)
    {
      formula.IndexOfAny(CalculationExtensions.conditionalOperations);
      string simpleExpression = source.GetSimpleExpression(caseSensitive, formula, paramExp, ref error);
      if (simpleExpression == null && error == ExpressionError.None)
      {
        Expression expressionPiece = source.GetExpressionPiece(caseSensitive, paramExp, formula, ref error, (object) null);
        if (expressionPiece != null)
        {
          LambdaExpression lambdaExpression = Expression.Lambda(expressionPiece, paramExp);
          CalculationExtensions.strings.Clear();
          return lambdaExpression.Compile();
        }
      }
      if (error == ExpressionError.None)
      {
        LambdaExpression lambdaExpression = Expression.Lambda(CalculationExtensions.expressions[simpleExpression], paramExp);
        CalculationExtensions.strings.Clear();
        return lambdaExpression.Compile();
      }
    }
    if (error == ExpressionError.None)
      error = ExpressionError.NotAValidFormula;
    CalculationExtensions.strings.Clear();
    return (Delegate) null;
  }

  private static string GetSimpleExpression(
    this object source,
    bool caseSensitive,
    string formula,
    ParameterExpression paramExp,
    ref ExpressionError error)
  {
    bool expression = source.CompileToExpression(paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[1]
    {
      '\u0092'
    }, CalculationExtensions.allOperations, out error);
    if (!expression && error == ExpressionError.None)
    {
      expression = source.CompileToExpression(paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[10]
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
      }, CalculationExtensions.allOperations, out error);
      if (!expression && error == ExpressionError.None)
      {
        expression = source.CompileToExpression(paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[2]
        {
          '\u0095',
          '\u0096'
        }, CalculationExtensions.allOperations, out error);
        if (!expression && error == ExpressionError.None && !expression && error == ExpressionError.None)
        {
          expression = source.CompileToExpression(paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[2]
          {
            '\u0093',
            '\u0094'
          }, CalculationExtensions.allOperations, out error);
          if (!expression && error == ExpressionError.None)
          {
            expression = source.CompileToExpression(paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[2]
            {
              '\u0097',
              '\u0098'
            }, CalculationExtensions.allOperations, out error);
            if (!expression && error == ExpressionError.None)
            {
              expression = source.CompileToExpression(paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[6]
              {
                '\u0084',
                '\u0085',
                '\u0086',
                '\u009A',
                '\u0099',
                '\u009B'
              }, CalculationExtensions.allOperations, out error);
              if (!expression && error == ExpressionError.None)
              {
                expression = source.CompileToExpression(paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[2]
                {
                  '\u009E',
                  '\u009F'
                }, CalculationExtensions.allOperations, out error);
                if (!expression && error == ExpressionError.None)
                  expression = source.CompileToExpression(paramExp, (caseSensitive ? 1 : 0) != 0, ref formula, new char[2]
                  {
                    '\u0087',
                    '\u0088'
                  }, CalculationExtensions.allOperations, out error);
              }
            }
          }
        }
      }
    }
    return expression && error == ExpressionError.None ? formula : (string) null;
  }

  private static bool CompileToExpression(
    this object source,
    ParameterExpression paramExp,
    bool caseSensitive,
    ref string formula,
    char[] operations,
    char[] allOperations,
    out ExpressionError error)
  {
    error = ExpressionError.None;
    int num1 = 0;
    string str1 = "";
    string str2 = "";
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
        if (((IEnumerable<char>) CalculationExtensions.unaryOperations).Contains<char>(formula[num1]))
        {
          string left = "";
          string str3 = formula.Substring(num1 + 1, startIndex - num1 - 1).Trim();
          if (paramExp.Type.Name == "ExpandoObject" && CalculationExtensions.DynamicPropertyTypeTable != null && CalculationExtensions.DynamicPropertyTypeTable.ContainsKey(str3))
          {
            string funcName = "GetDynamicValue";
            Expression expression = (Expression) Expression.Call((Expression) null, ((IEnumerable<MethodInfo>) typeof (CalculationExtensions).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName)).FirstOrDefault<MethodInfo>(), new Expression[2]
            {
              (Expression) paramExp,
              (Expression) Expression.Constant((object) str3)
            });
            str3 = '\u0083'.ToString() + CalculationExtensions.expressions.Count.ToString() + (object) '\u0083';
            CalculationExtensions.expressions.Add(str3, expression);
          }
          Expression expression1 = source.GetExpression(caseSensitive, paramExp, left, str3, formula[num1], ref error);
          key = '\u0083'.ToString() + CalculationExtensions.expressions.Count.ToString() + (object) '\u0083';
          CalculationExtensions.expressions.Add(key, expression1);
        }
        else
        {
          string str4 = "";
          string str5 = "";
          if (((IEnumerable<char>) CalculationExtensions.mathOperations).Contains<char>(formula[num1]))
          {
            char[] chArray = new char[6]
            {
              ' ',
              ',',
              ':',
              ')',
              '(',
              '\t'
            };
            str4 = formula.Split(chArray)[1];
            str5 = formula.Split(chArray)[2];
            str1 = string.IsNullOrEmpty(str1) || str1 != str4 ? str4 : str1;
            str2 = string.IsNullOrEmpty(str1) || str2 != str5 ? str5 : str2;
          }
          if (string.IsNullOrEmpty(str4) && string.IsNullOrEmpty(str5))
          {
            str4 = formula.Substring(num2 + 1, num1 - num2 - 1).Trim();
            str5 = formula.Substring(num1 + 1, startIndex - num1 - 1);
          }
          IDictionary dictionary = source as IDictionary;
          if (paramExp.Type.Name == "ExpandoObject" && CalculationExtensions.DynamicPropertyTypeTable != null && CalculationExtensions.DynamicPropertyTypeTable.ContainsKey(str4))
          {
            string funcName = "GetDynamicValue";
            Expression expression = (Expression) Expression.Call((Expression) null, ((IEnumerable<MethodInfo>) typeof (CalculationExtensions).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName)).FirstOrDefault<MethodInfo>(), new Expression[2]
            {
              (Expression) paramExp,
              (Expression) Expression.Constant((object) str4)
            });
            str4 = '\u0083'.ToString() + CalculationExtensions.expressions.Count.ToString() + (object) '\u0083';
            CalculationExtensions.expressions.Add(str4, expression);
          }
          else if (dictionary != null)
          {
            if (dictionary.Contains((object) str5))
            {
              Expression lookUpExpression = CalculationExtensions.GetDictionaryLookUpExpression(paramExp, str5);
              str5 = '\u0083'.ToString() + CalculationExtensions.expressions.Count.ToString() + (object) '\u0083';
              CalculationExtensions.expressions.Add(str5, lookUpExpression);
            }
            if (dictionary.Contains((object) str4))
            {
              Expression lookUpExpression = CalculationExtensions.GetDictionaryLookUpExpression(paramExp, str4);
              str4 = '\u0083'.ToString() + CalculationExtensions.expressions.Count.ToString() + (object) '\u0083';
              CalculationExtensions.expressions.Add(str4, lookUpExpression);
            }
          }
          Expression expression2 = source.GetExpression(caseSensitive, paramExp, str4, str5, formula[num1], ref error);
          key = '\u0083'.ToString() + CalculationExtensions.expressions.Count.ToString() + (object) '\u0083';
          CalculationExtensions.expressions.Add(key, expression2);
        }
        string str6 = "";
        if (num2 > 0)
          str6 = formula.Substring(0, num2 + 1);
        string str7 = str6 + key;
        if (startIndex < formula.Length - 1)
          str7 += formula.Substring(startIndex);
        formula = str7;
        num1 = 0;
      }
    }
    return formula.StartsWith('\u0083'.ToString()) && formula.EndsWith('\u0083'.ToString()) && formula.IndexOf('\u0083', 1, formula.Length - 2) == -1;
  }

  private static Expression GetExpression(
    this object source,
    bool caseSensitive,
    ParameterExpression paramExp,
    string left,
    string right,
    char operand,
    ref ExpressionError error)
  {
    if (operand == '\u009D')
      operand = '\u009C';
    Expression expressionPiece = source.GetExpressionPiece(caseSensitive, paramExp, left, ref error, (object) null);
    Expression rightExp = source.GetExpressionPiece(caseSensitive, paramExp, right, ref error, expressionPiece == null ? (object) (Type) null : (object) expressionPiece.Type);
    if (!((IEnumerable<char>) CalculationExtensions.unaryOperations).Contains<char>(operand) && operand != '\u009C')
      CalculationExtensions.CoerceType(ref expressionPiece, ref rightExp, ref error);
    Expression left1 = (Expression) null;
    if (error == ExpressionError.None)
    {
      try
      {
        switch (operand)
        {
          case '\u0084':
          case '\u009E':
            left1 = (Expression) Expression.GreaterThanOrEqual(expressionPiece, rightExp);
            break;
          case '\u0085':
          case '\u009F':
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
            string funcName1 = CalculationExtensions.GetFunctionName(operand);
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
            string funcName2 = CalculationExtensions.GetFunctionName(operand);
            if (funcName2.Length > 0)
            {
              PropertyInfo property = ((IEnumerable<PropertyInfo>) typeof (DateTime).GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (m => m.Name == funcName2)).FirstOrDefault<PropertyInfo>();
              Type type = rightExp.Type;
              if (CalculationExtensions.DynamicPropertyTypeTable != null)
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
            if (CalculationExtensions.DynamicPropertyTypeTable != null)
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
            string str = rightExp.ToString();
            BinaryList lookUpList = CalculationExtensions.GetLookUpList(str.Substring(1, str.Length - 2));
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
            string funcName4 = CalculationExtensions.GetFunctionName(operand);
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
        CalculationExtensions.ErrorString = ex.Message;
      }
    }
    return left1;
  }

  private static StringBuilder GetMultiFunctionalExpression(
    StringBuilder stringBuilder,
    string formula,
    char[] delimiterChars)
  {
    if (!stringBuilder.ToString().ToUpper().Contains("MAX") && !stringBuilder.ToString().ToUpper().Contains("MIN") && !stringBuilder.ToString().ToUpper().Contains("MEADIAN"))
      stringBuilder = stringBuilder.Replace(stringBuilder.ToString().Split(delimiterChars)[0], "").Replace('('.ToString(), "").Replace(')'.ToString(), "").Replace(" ", "");
    if (formula.ToString().ToUpper().Contains("SUM") || formula.ToString().ToUpper().Contains("AVERAGE"))
      stringBuilder.Replace(",", '\u0093'.ToString());
    else if (formula.ToString().ToUpper().Contains("PRODUCT"))
      stringBuilder.Replace(",", '\u0095'.ToString());
    else if (formula.ToString().ToUpper().Contains("POWER"))
      stringBuilder.Replace(",", '\u0097'.ToString());
    else if (formula.ToString().ToUpper().Contains("MAX"))
      stringBuilder.Replace(stringBuilder.ToString().Split(delimiterChars)[0], '\u009E'.ToString());
    else if (formula.ToString().ToUpper().Contains("MIN"))
      stringBuilder.Replace(stringBuilder.ToString().Split(delimiterChars)[0], '\u009F'.ToString());
    return stringBuilder;
  }

  private static BinaryList GetLookUpList(string item)
  {
    BinaryList lookUpList = new BinaryList();
    string str = item;
    char[] chArray = new char[1]
    {
      CalculationExtensions.ListSeparator
    };
    foreach (string o in str.Split(chArray))
      lookUpList.AddIfUnique((IComparable) o);
    return lookUpList;
  }

  private static string GetFunctionName(char c)
  {
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

  private static void CoerceType(
    ref Expression leftExp,
    ref Expression rightExp,
    ref ExpressionError error)
  {
    if (!(leftExp.Type != rightExp.Type))
      return;
    if (leftExp.Type == typeof (double) && (rightExp.Type == typeof (int) || rightExp.Type == typeof (float)))
      rightExp = (Expression) Expression.Convert(rightExp, typeof (double));
    else if (rightExp.Type == typeof (double) && (leftExp.Type == typeof (int) || leftExp.Type == typeof (float) || leftExp.Type == typeof (int?) || leftExp.Type == typeof (float?) || leftExp.Type == typeof (double?) || leftExp.Type == typeof (short?) || leftExp.Type == typeof (long?) || leftExp.Type == typeof (Decimal?)))
      leftExp = (Expression) Expression.Convert(leftExp, typeof (double));
    else if (leftExp.Type != typeof (DateTime))
    {
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
      catch
      {
        try
        {
          if (!(rightExp.Type == typeof (string)))
            return;
          string funcName = "ToString";
          MethodInfo method = ((IEnumerable<MethodInfo>) leftExp.Type.GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName && ((IEnumerable<ParameterInfo>) m.GetParameters()).Count<ParameterInfo>() == 0)).FirstOrDefault<MethodInfo>();
          leftExp = (Expression) Expression.Call(leftExp, method);
        }
        catch
        {
        }
      }
    }
    else if (leftExp.NodeType != ExpressionType.Constant && rightExp.NodeType == ExpressionType.Constant)
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
      error = ExpressionError.ExceptionRaised;
  }

  private static Expression GetExpressionPiece(
    this object source,
    bool caseSensitive,
    ParameterExpression paramExp,
    string piece,
    ref ExpressionError error,
    object leftExpType)
  {
    if (piece.StartsWith('\u0083'.ToString()) && CalculationExtensions.expressions.ContainsKey(piece))
      return CalculationExtensions.expressions[piece];
    Type type1 = source.GetType();
    Type type2 = (Type) leftExpType;
    if (source is IDictionary dictionary)
    {
      if (leftExpType == null)
        leftExpType = (object) (type2 = typeof (double));
      if (dictionary.Contains((object) piece))
        return CalculationExtensions.GetDictionaryLookUpExpression(paramExp, piece);
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
    if (properties[piece] != null && (CalculationExtensions.Engine.AllowedFields == null || !CalculationExtensions.Engine.AllowedFields.Any<FieldInfo>((Func<FieldInfo, bool>) (i => i.Name == CalculationExtensions.FieldName && i.Expression != null)) ? (leftExpType == null ? 1 : 0) : 1) != 0)
      return (Expression) Expression.PropertyOrField((Expression) paramExp, piece);
    if (type2 == typeof (float) || type2 == typeof (Decimal) || type2 == typeof (double) || type2 == typeof (int) || type2 == typeof (int?) || type2 == typeof (float?) || type2 == typeof (double?) || type2 == typeof (short?) || type2 == typeof (long?) || type2 == typeof (Decimal?))
    {
      double result = 0.0;
      if (paramExp.Name != "ExpandoObject" && double.TryParse(piece, out result))
        return (Expression) Expression.Constant((object) result);
    }
    if (type2 == typeof (bool))
      return (Expression) Expression.Constant((object) (bool) (!(piece == "True") ? (false ? 1 : 0) : (true ? 1 : 0)));
    if (piece.IndexOf('\u009D') > -1)
      piece = piece.Replace('\u009D'.ToString(), "");
    for (int startIndex = piece.IndexOf('\u0082'); startIndex > -1; startIndex = piece.IndexOf('\u0082'))
    {
      int num = piece.IndexOf('\u0082', startIndex + 1);
      string str = piece.Substring(startIndex, num - startIndex + 1);
      piece = piece.Replace(str, CalculationExtensions.strings[str]);
    }
    return (Expression) Expression.Constant((object) piece);
  }

  private static Expression GetDictionaryLookUpExpression(
    ParameterExpression paramExp,
    string piece)
  {
    string funcName = "GetDictionaryDoubleValue";
    return (Expression) Expression.Call((Expression) null, ((IEnumerable<MethodInfo>) typeof (CalculationExtensions).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == funcName)).FirstOrDefault<MethodInfo>(), new Expression[2]
    {
      (Expression) paramExp,
      (Expression) Expression.Constant((object) piece)
    });
  }

  private static string TokenizeStrings(string formula, ref ExpressionError error)
  {
    error = ExpressionError.None;
    int startIndex1 = 0;
    int num1 = 0;
    CalculationExtensions.strings.Clear();
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
        CalculationExtensions.strings.Add(key, formula.Substring(startIndex1, num2 - startIndex1 + 1));
        stringBuilder.Append(key);
        startIndex1 = num2 + 1;
      }
      else
        stringBuilder.Append(formula.Substring(startIndex2));
    }
    return stringBuilder.ToString();
  }
}
