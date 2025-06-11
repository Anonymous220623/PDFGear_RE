// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionParser
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace NLog.Conditions;

public class ConditionParser
{
  private readonly ConditionTokenizer _tokenizer;
  private readonly ConfigurationItemFactory _configurationItemFactory;

  private ConditionParser(
    SimpleStringReader stringReader,
    ConfigurationItemFactory configurationItemFactory)
  {
    this._configurationItemFactory = configurationItemFactory;
    this._tokenizer = new ConditionTokenizer(stringReader);
  }

  public static ConditionExpression ParseExpression(string expressionText)
  {
    return ConditionParser.ParseExpression(expressionText, ConfigurationItemFactory.Default);
  }

  public static ConditionExpression ParseExpression(
    string expressionText,
    ConfigurationItemFactory configurationItemFactories)
  {
    if (expressionText == null)
      return (ConditionExpression) null;
    ConditionParser conditionParser = new ConditionParser(new SimpleStringReader(expressionText), configurationItemFactories);
    ConditionExpression expression = conditionParser.ParseExpression();
    if (conditionParser._tokenizer.IsEOF())
      return expression;
    throw new ConditionParseException("Unexpected token: " + conditionParser._tokenizer.TokenValue);
  }

  internal static ConditionExpression ParseExpression(
    SimpleStringReader stringReader,
    ConfigurationItemFactory configurationItemFactories)
  {
    return new ConditionParser(stringReader, configurationItemFactories).ParseExpression();
  }

  private ConditionMethodExpression ParsePredicate(string functionName)
  {
    List<ConditionExpression> methodParameters = new List<ConditionExpression>();
    while (!this._tokenizer.IsEOF() && this._tokenizer.TokenType != ConditionTokenType.RightParen)
    {
      methodParameters.Add(this.ParseExpression());
      if (this._tokenizer.TokenType == ConditionTokenType.Comma)
        this._tokenizer.GetNextToken();
      else
        break;
    }
    this._tokenizer.Expect(ConditionTokenType.RightParen);
    try
    {
      MethodInfo instance1 = this._configurationItemFactory.ConditionMethods.CreateInstance(functionName);
      ReflectionHelpers.LateBoundMethod instance2 = this._configurationItemFactory.ConditionMethodDelegates.CreateInstance(functionName);
      return new ConditionMethodExpression(functionName, instance1, instance2, (IEnumerable<ConditionExpression>) methodParameters);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "Cannot resolve function '{0}'", (object) functionName);
      if (!ex.MustBeRethrownImmediately())
        throw new ConditionParseException($"Cannot resolve function '{functionName}'", ex);
      throw;
    }
  }

  private ConditionExpression ParseLiteralExpression()
  {
    if (this._tokenizer.IsToken(ConditionTokenType.LeftParen))
    {
      this._tokenizer.GetNextToken();
      ConditionExpression expression = this.ParseExpression();
      this._tokenizer.Expect(ConditionTokenType.RightParen);
      return expression;
    }
    if (this._tokenizer.IsToken(ConditionTokenType.Minus))
    {
      this._tokenizer.GetNextToken();
      if (!this._tokenizer.IsNumber())
        throw new ConditionParseException($"Number expected, got {this._tokenizer.TokenType}");
      return this.ParseNumber(true);
    }
    if (this._tokenizer.IsNumber())
      return this.ParseNumber(false);
    if (this._tokenizer.TokenType == ConditionTokenType.String)
    {
      SimpleLayout layout = new SimpleLayout(this._tokenizer.StringTokenValue, this._configurationItemFactory);
      this._tokenizer.GetNextToken();
      return layout.IsFixedText ? (ConditionExpression) new ConditionLiteralExpression((object) layout.FixedText, layout.ToString()) : (ConditionExpression) new ConditionLayoutExpression(layout);
    }
    if (this._tokenizer.TokenType == ConditionTokenType.Keyword)
    {
      string str = this._tokenizer.EatKeyword();
      ConditionExpression expression;
      if (this.TryPlainKeywordToExpression(str, out expression))
        return expression;
      if (this._tokenizer.TokenType == ConditionTokenType.LeftParen)
      {
        this._tokenizer.GetNextToken();
        return (ConditionExpression) this.ParsePredicate(str);
      }
    }
    throw new ConditionParseException("Unexpected token: " + this._tokenizer.TokenValue);
  }

  private bool TryPlainKeywordToExpression(string keyword, out ConditionExpression expression)
  {
    if (string.Compare(keyword, "level", StringComparison.OrdinalIgnoreCase) == 0)
    {
      expression = (ConditionExpression) new ConditionLevelExpression();
      return true;
    }
    if (string.Compare(keyword, "logger", StringComparison.OrdinalIgnoreCase) == 0)
    {
      expression = (ConditionExpression) new ConditionLoggerNameExpression();
      return true;
    }
    if (string.Compare(keyword, "message", StringComparison.OrdinalIgnoreCase) == 0)
    {
      expression = (ConditionExpression) new ConditionMessageExpression();
      return true;
    }
    if (string.Compare(keyword, "loglevel", StringComparison.OrdinalIgnoreCase) == 0)
    {
      this._tokenizer.Expect(ConditionTokenType.Dot);
      expression = (ConditionExpression) new ConditionLiteralExpression((object) NLog.LogLevel.FromString(this._tokenizer.EatKeyword()));
      return true;
    }
    if (string.Compare(keyword, "true", StringComparison.OrdinalIgnoreCase) == 0)
    {
      expression = (ConditionExpression) new ConditionLiteralExpression(ConditionExpression.BoxedTrue);
      return true;
    }
    if (string.Compare(keyword, "false", StringComparison.OrdinalIgnoreCase) == 0)
    {
      expression = (ConditionExpression) new ConditionLiteralExpression(ConditionExpression.BoxedFalse);
      return true;
    }
    if (string.Compare(keyword, "null", StringComparison.OrdinalIgnoreCase) == 0)
    {
      expression = (ConditionExpression) new ConditionLiteralExpression((object) null);
      return true;
    }
    expression = (ConditionExpression) null;
    return false;
  }

  private ConditionExpression ParseNumber(bool negative)
  {
    string tokenValue = this._tokenizer.TokenValue;
    this._tokenizer.GetNextToken();
    if (tokenValue.IndexOf('.') >= 0)
    {
      double num = double.Parse(tokenValue, (IFormatProvider) CultureInfo.InvariantCulture);
      return (ConditionExpression) new ConditionLiteralExpression((object) (negative ? -num : num));
    }
    int num1 = int.Parse(tokenValue, (IFormatProvider) CultureInfo.InvariantCulture);
    return (ConditionExpression) new ConditionLiteralExpression((object) (negative ? -num1 : num1));
  }

  private ConditionExpression ParseBooleanRelation()
  {
    ConditionExpression literalExpression = this.ParseLiteralExpression();
    if (this._tokenizer.IsToken(ConditionTokenType.EqualTo))
    {
      this._tokenizer.GetNextToken();
      return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.Equal);
    }
    if (this._tokenizer.IsToken(ConditionTokenType.NotEqual))
    {
      this._tokenizer.GetNextToken();
      return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.NotEqual);
    }
    if (this._tokenizer.IsToken(ConditionTokenType.LessThan))
    {
      this._tokenizer.GetNextToken();
      return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.Less);
    }
    if (this._tokenizer.IsToken(ConditionTokenType.GreaterThan))
    {
      this._tokenizer.GetNextToken();
      return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.Greater);
    }
    if (this._tokenizer.IsToken(ConditionTokenType.LessThanOrEqualTo))
    {
      this._tokenizer.GetNextToken();
      return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.LessOrEqual);
    }
    if (!this._tokenizer.IsToken(ConditionTokenType.GreaterThanOrEqualTo))
      return literalExpression;
    this._tokenizer.GetNextToken();
    return (ConditionExpression) new ConditionRelationalExpression(literalExpression, this.ParseLiteralExpression(), ConditionRelationalOperator.GreaterOrEqual);
  }

  private ConditionExpression ParseBooleanPredicate()
  {
    if (!this._tokenizer.IsKeyword("not") && !this._tokenizer.IsToken(ConditionTokenType.Not))
      return this.ParseBooleanRelation();
    this._tokenizer.GetNextToken();
    return (ConditionExpression) new ConditionNotExpression(this.ParseBooleanPredicate());
  }

  private ConditionExpression ParseBooleanAnd()
  {
    ConditionExpression left = this.ParseBooleanPredicate();
    while (this._tokenizer.IsKeyword("and") || this._tokenizer.IsToken(ConditionTokenType.And))
    {
      this._tokenizer.GetNextToken();
      left = (ConditionExpression) new ConditionAndExpression(left, this.ParseBooleanPredicate());
    }
    return left;
  }

  private ConditionExpression ParseBooleanOr()
  {
    ConditionExpression left = this.ParseBooleanAnd();
    while (this._tokenizer.IsKeyword("or") || this._tokenizer.IsToken(ConditionTokenType.Or))
    {
      this._tokenizer.GetNextToken();
      left = (ConditionExpression) new ConditionOrExpression(left, this.ParseBooleanAnd());
    }
    return left;
  }

  private ConditionExpression ParseBooleanExpression() => this.ParseBooleanOr();

  private ConditionExpression ParseExpression() => this.ParseBooleanExpression();
}
