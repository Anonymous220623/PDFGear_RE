// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionTokenizer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.Conditions;

internal sealed class ConditionTokenizer
{
  private static readonly ConditionTokenType[] CharIndexToTokenType = ConditionTokenizer.BuildCharIndexToTokenType();
  private readonly SimpleStringReader _stringReader;

  public ConditionTokenizer(SimpleStringReader stringReader)
  {
    this._stringReader = stringReader;
    this.TokenType = ConditionTokenType.BeginningOfInput;
    this.GetNextToken();
  }

  public ConditionTokenType TokenType { get; private set; }

  public string TokenValue { get; private set; }

  public string StringTokenValue
  {
    get
    {
      string tokenValue = this.TokenValue;
      return tokenValue.Substring(1, tokenValue.Length - 2).Replace("''", "'");
    }
  }

  public void Expect(ConditionTokenType tokenType)
  {
    if (this.TokenType != tokenType)
      throw new ConditionParseException($"Expected token of type: {tokenType}, got {this.TokenType} ({this.TokenValue}).");
    this.GetNextToken();
  }

  public string EatKeyword()
  {
    if (this.TokenType != ConditionTokenType.Keyword)
      throw new ConditionParseException("Identifier expected");
    string tokenValue = this.TokenValue;
    this.GetNextToken();
    return tokenValue;
  }

  public bool IsKeyword(string keyword)
  {
    return this.TokenType == ConditionTokenType.Keyword && this.TokenValue.Equals(keyword, StringComparison.OrdinalIgnoreCase);
  }

  public bool IsEOF() => this.TokenType == ConditionTokenType.EndOfInput;

  public bool IsNumber() => this.TokenType == ConditionTokenType.Number;

  public bool IsToken(ConditionTokenType tokenType) => this.TokenType == tokenType;

  public void GetNextToken()
  {
    if (this.TokenType == ConditionTokenType.EndOfInput)
      throw new ConditionParseException("Cannot read past end of stream.");
    this.SkipWhitespace();
    int num = this.PeekChar();
    if (num == -1)
    {
      this.TokenType = ConditionTokenType.EndOfInput;
    }
    else
    {
      char index = (char) num;
      if (char.IsDigit(index))
        this.ParseNumber(index);
      else if (index == '\'')
        this.ParseSingleQuotedString(index);
      else if (index == '_' || char.IsLetter(index))
        this.ParseKeyword(index);
      else if (index == '}' || index == ':')
      {
        this.TokenType = ConditionTokenType.EndOfInput;
      }
      else
      {
        this.TokenValue = index.ToString();
        if (this.TryGetComparisonToken(index) || this.TryGetLogicalToken(index))
          return;
        ConditionTokenType conditionTokenType = index >= ' ' && index < '\u0080' ? ConditionTokenizer.CharIndexToTokenType[(int) index] : throw new ConditionParseException($"Invalid token: {index}");
        this.TokenType = conditionTokenType != ConditionTokenType.Invalid ? conditionTokenType : throw new ConditionParseException($"Invalid punctuation: {index}");
        this.TokenValue = new string(index, 1);
        this.ReadChar();
      }
    }
  }

  private bool TryGetComparisonToken(char ch)
  {
    switch (ch)
    {
      case '<':
        this.ReadChar();
        switch (this.PeekChar())
        {
          case 61:
            this.TokenType = ConditionTokenType.LessThanOrEqualTo;
            this.TokenValue = "<=";
            this.ReadChar();
            return true;
          case 62:
            this.TokenType = ConditionTokenType.NotEqual;
            this.TokenValue = "<>";
            this.ReadChar();
            return true;
          default:
            this.TokenType = ConditionTokenType.LessThan;
            this.TokenValue = "<";
            return true;
        }
      case '>':
        this.ReadChar();
        if (this.PeekChar() == 61)
        {
          this.TokenType = ConditionTokenType.GreaterThanOrEqualTo;
          this.TokenValue = ">=";
          this.ReadChar();
          return true;
        }
        this.TokenType = ConditionTokenType.GreaterThan;
        this.TokenValue = ">";
        return true;
      default:
        return false;
    }
  }

  private bool TryGetLogicalToken(char ch)
  {
    switch (ch)
    {
      case '!':
        this.ReadChar();
        if (this.PeekChar() == 61)
        {
          this.TokenType = ConditionTokenType.NotEqual;
          this.TokenValue = "!=";
          this.ReadChar();
          return true;
        }
        this.TokenType = ConditionTokenType.Not;
        this.TokenValue = "!";
        return true;
      case '&':
        this.ReadChar();
        if (this.PeekChar() != 38)
          throw new ConditionParseException("Expected '&&' but got '&'");
        this.TokenType = ConditionTokenType.And;
        this.TokenValue = "&&";
        this.ReadChar();
        return true;
      case '=':
        this.ReadChar();
        if (this.PeekChar() == 61)
        {
          this.TokenType = ConditionTokenType.EqualTo;
          this.TokenValue = "==";
          this.ReadChar();
          return true;
        }
        this.TokenType = ConditionTokenType.EqualTo;
        this.TokenValue = "=";
        return true;
      case '|':
        this.ReadChar();
        if (this.PeekChar() != 124)
          throw new ConditionParseException("Expected '||' but got '|'");
        this.TokenType = ConditionTokenType.Or;
        this.TokenValue = "||";
        this.ReadChar();
        return true;
      default:
        return false;
    }
  }

  private static ConditionTokenType[] BuildCharIndexToTokenType()
  {
    ConditionTokenizer.CharToTokenType[] charToTokenTypeArray = new ConditionTokenizer.CharToTokenType[6]
    {
      new ConditionTokenizer.CharToTokenType('(', ConditionTokenType.LeftParen),
      new ConditionTokenizer.CharToTokenType(')', ConditionTokenType.RightParen),
      new ConditionTokenizer.CharToTokenType('.', ConditionTokenType.Dot),
      new ConditionTokenizer.CharToTokenType(',', ConditionTokenType.Comma),
      new ConditionTokenizer.CharToTokenType('!', ConditionTokenType.Not),
      new ConditionTokenizer.CharToTokenType('-', ConditionTokenType.Minus)
    };
    ConditionTokenType[] tokenType = new ConditionTokenType[128 /*0x80*/];
    for (int index = 0; index < 128 /*0x80*/; ++index)
      tokenType[index] = ConditionTokenType.Invalid;
    foreach (ConditionTokenizer.CharToTokenType charToTokenType in charToTokenTypeArray)
      tokenType[(int) charToTokenType.Character] = charToTokenType.TokenType;
    return tokenType;
  }

  private void ParseSingleQuotedString(char ch)
  {
    this.TokenType = ConditionTokenType.String;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(ch);
    this.ReadChar();
    int num;
    while ((num = this.PeekChar()) != -1)
    {
      ch = (char) num;
      stringBuilder.Append((char) this.ReadChar());
      if (ch == '\'')
      {
        if (this.PeekChar() == 39)
        {
          stringBuilder.Append('\'');
          this.ReadChar();
        }
        else
          break;
      }
    }
    if (num == -1)
      throw new ConditionParseException("String literal is missing a closing quote character.");
    this.TokenValue = stringBuilder.ToString();
  }

  private void ParseKeyword(char ch)
  {
    this.TokenType = ConditionTokenType.Keyword;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(ch);
    this.ReadChar();
    int c;
    while ((c = this.PeekChar()) != -1 && ((ushort) c == (ushort) 95 || (ushort) c == (ushort) 45 || char.IsLetterOrDigit((char) c)))
      stringBuilder.Append((char) this.ReadChar());
    this.TokenValue = stringBuilder.ToString();
  }

  private void ParseNumber(char ch)
  {
    this.TokenType = ConditionTokenType.Number;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(ch);
    this.ReadChar();
    int num;
    while ((num = this.PeekChar()) != -1)
    {
      ch = (char) num;
      if (char.IsDigit(ch) || ch == '.')
        stringBuilder.Append((char) this.ReadChar());
      else
        break;
    }
    this.TokenValue = stringBuilder.ToString();
  }

  private void SkipWhitespace()
  {
    int c;
    while ((c = this.PeekChar()) != -1 && char.IsWhiteSpace((char) c))
      this.ReadChar();
  }

  private int PeekChar() => this._stringReader.Peek();

  private int ReadChar() => this._stringReader.Read();

  private struct CharToTokenType(char character, ConditionTokenType tokenType)
  {
    public readonly char Character = character;
    public readonly ConditionTokenType TokenType = tokenType;
  }
}
