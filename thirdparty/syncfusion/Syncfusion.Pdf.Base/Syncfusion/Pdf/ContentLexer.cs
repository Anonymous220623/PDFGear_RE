// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ContentLexer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

internal class ContentLexer
{
  private StringBuilder m_operatorParams = new StringBuilder();
  private TokenType m_tType;
  private char m_currentChar;
  private bool m_isArtifactContentEnds;
  internal bool IsContainsArtifacts;
  private char m_nextChar;
  private byte[] m_contentStream;
  private int m_charPointer;
  private static string[] m_textShowers = new string[4]
  {
    "Tj",
    "'",
    "TJ",
    "\""
  };
  private bool m_isContentEnded;

  internal TokenType Token => this.m_tType;

  internal StringBuilder OperatorParams => this.m_operatorParams;

  public ContentLexer(byte[] contentStream) => this.m_contentStream = contentStream;

  public TokenType GetNextToken()
  {
    this.ResetToken();
    char nextChar = this.MoveToNextChar();
    switch (nextChar)
    {
      case '"':
      case '\'':
        return this.m_tType = this.GetOperator();
      case '%':
        return this.m_tType = this.GetComment();
      case '(':
      case '[':
        return this.m_tType = this.GetLiteralString();
      case '+':
      case '-':
        return this.m_tType = this.GetNumber();
      case '.':
        return this.m_tType = this.GetNumber();
      case '/':
        return this.m_tType = this.GetName();
      case '<':
        return this.m_tType = this.GetHexadecimalString();
      default:
        if (char.IsDigit(nextChar))
          return this.m_tType = this.GetNumber();
        if (char.IsLetter(nextChar))
          return this.m_tType = this.GetOperator();
        return nextChar == char.MaxValue ? (this.m_tType = TokenType.Eof) : TokenType.None;
    }
  }

  private void ResetToken() => this.m_operatorParams.Length = 0;

  private char MoveToNextChar()
  {
    while (this.m_currentChar != char.MaxValue)
    {
      switch (this.m_currentChar)
      {
        case char.MinValue:
        case '\b':
        case '\t':
        case '\n':
        case '\f':
        case '\r':
        case ' ':
          int nextChar = (int) this.GetNextChar();
          continue;
        default:
          return this.m_currentChar;
      }
    }
    return this.m_currentChar;
  }

  internal void ResetContentPointer(int count) => this.m_charPointer -= count;

  internal char GetNextChar()
  {
    if (this.m_contentStream.Length <= this.m_charPointer)
    {
      if (this.m_nextChar == 'Q' || this.m_currentChar == 'D' && this.m_nextChar == 'o')
      {
        this.m_currentChar = this.m_nextChar;
        this.m_nextChar = char.MaxValue;
        return this.m_currentChar;
      }
      this.m_currentChar = char.MaxValue;
      this.m_nextChar = char.MaxValue;
    }
    else
    {
      this.m_currentChar = this.m_nextChar;
      this.m_nextChar = (char) this.m_contentStream[this.m_charPointer++];
      if (this.m_currentChar == '\r')
      {
        if (this.m_nextChar == '\n')
        {
          this.m_currentChar = this.m_nextChar;
          this.m_nextChar = this.m_contentStream.Length > this.m_charPointer ? (char) this.m_contentStream[this.m_charPointer++] : char.MaxValue;
        }
        else
          this.m_currentChar = '\n';
      }
    }
    return this.m_currentChar;
  }

  internal char GetNextInlineChar()
  {
    if (this.m_contentStream.Length <= this.m_charPointer)
    {
      this.m_currentChar = char.MaxValue;
      this.m_nextChar = char.MaxValue;
    }
    else
    {
      this.m_currentChar = this.m_nextChar;
      this.m_nextChar = (char) this.m_contentStream[this.m_charPointer++];
      if (this.m_currentChar == '\r')
        this.m_currentChar = this.m_nextChar != '\n' ? '\n' : '\r';
    }
    return this.m_currentChar;
  }

  internal char GetNextCharforInlineStream()
  {
    if (this.m_contentStream.Length <= this.m_charPointer)
    {
      this.m_currentChar = char.MaxValue;
      this.m_nextChar = char.MaxValue;
    }
    else
    {
      this.m_currentChar = this.m_nextChar;
      this.m_nextChar = (char) this.m_contentStream[this.m_charPointer++];
      if (this.m_currentChar == '\r' && this.m_nextChar == '\n' && this.m_contentStream.Length <= this.m_charPointer)
      {
        this.m_currentChar = this.m_nextChar;
        this.m_nextChar = char.MaxValue;
      }
    }
    return this.m_currentChar;
  }

  internal char GetNextChar(bool value) => this.m_nextChar;

  private TokenType GetComment()
  {
    this.ResetToken();
    char ch;
    do
      ;
    while ((ch = this.ConsumeValue()) != '\n' && ch != char.MaxValue);
    return TokenType.Comment;
  }

  private TokenType GetName()
  {
    this.ResetToken();
    char ch;
    do
    {
      ch = this.ConsumeValue();
    }
    while (!this.IsWhiteSpace(ch) && !this.IsDelimiter(ch));
    return TokenType.Name;
  }

  private TokenType GetNumber()
  {
    char c = this.m_currentChar;
    switch (c)
    {
      case '+':
      case '-':
        this.m_operatorParams.Append(this.m_currentChar);
        c = this.GetNextChar();
        break;
    }
    while (true)
    {
      if (char.IsDigit(c))
        this.m_operatorParams.Append(this.m_currentChar);
      else if (c == '.')
        this.m_operatorParams.Append(this.m_currentChar);
      else
        break;
      c = this.GetNextChar();
    }
    return TokenType.Integer;
  }

  private TokenType GetLiteralString()
  {
    this.ResetToken();
    char ch1 = this.m_currentChar != '(' ? this.m_currentChar : this.m_currentChar;
    char ch2 = this.ConsumeValue();
    char ch3;
    while (ch1 != '(')
    {
      switch (ch2)
      {
        case '(':
          this.m_operatorParams.Append(this.GetLiterals(this.ConsumeValue()));
          ch2 = this.GetNextChar();
          continue;
        case ']':
          ch3 = this.ConsumeValue();
          goto label_7;
        default:
          ch2 = this.ConsumeValue();
          continue;
      }
    }
    this.m_operatorParams.Append(this.GetLiterals(ch2));
    ch3 = this.GetNextChar();
label_7:
    return TokenType.String;
  }

  private string GetLiterals(char ch)
  {
    int num = 0;
    string str1 = "";
    while (true)
    {
      switch (ch)
      {
        case '(':
          ++num;
          str1 += ch.ToString();
          ch = this.GetNextChar();
          continue;
        case '\\':
          string str2 = str1 + ch.ToString();
          ch = this.GetNextChar();
          str1 = str2 + ch.ToString();
          ch = this.GetNextChar();
          continue;
        default:
          if (ch == ')' && num != 0)
          {
            str1 += ch.ToString();
            ch = this.GetNextChar();
            --num;
            continue;
          }
          if (ch != ')' || num != 0)
          {
            str1 += ch.ToString();
            ch = this.GetNextChar();
            continue;
          }
          goto label_7;
      }
    }
label_7:
    return str1 + ch.ToString();
  }

  private TokenType GetHexadecimalString()
  {
    char ch1 = '<';
    char ch2 = '>';
    char ch3 = ' ';
    int num1 = 0;
    char ch4 = this.ConsumeValue();
    while (true)
    {
      while ((int) ch4 != (int) ch1)
      {
        if ((int) ch4 == (int) ch2 && !this.m_isArtifactContentEnds)
        {
          switch (num1)
          {
            case 0:
              int num2 = (int) this.ConsumeValue();
              break;
            case 1:
              ch4 = this.ConsumeValue();
              if (ch4.Equals('>'))
                --num1;
              if (num1 != 1 || (int) ch4 != (int) ch3 && (!this.IsContainsArtifacts || ch4 != 'B'))
                continue;
              break;
            default:
              if (ch4.Equals('>'))
                --num1;
              ch4 = this.ConsumeValue();
              continue;
          }
        }
        else
        {
          ch4 = this.ConsumeValue();
          if (ch4 != char.MaxValue)
            continue;
        }
        this.m_isContentEnded = false;
        this.IsContainsArtifacts = false;
        return TokenType.HexString;
      }
      ++num1;
      ch4 = this.ConsumeValue();
    }
  }

  private TokenType GetOperator()
  {
    this.ResetToken();
    char ch = this.m_currentChar;
    while (this.IsOperator(ch))
      ch = this.ConsumeValue();
    return TokenType.Operator;
  }

  private bool IsOperator(char ch)
  {
    if (char.IsLetter(ch))
      return true;
    switch (ch)
    {
      case '"':
      case '\'':
      case '*':
      case '0':
      case '1':
        return true;
      default:
        return false;
    }
  }

  private char ConsumeValue()
  {
    this.m_operatorParams.Append(this.m_currentChar);
    if (this.IsContainsArtifacts && this.m_operatorParams.ToString().Contains("/Contents") && !this.m_isContentEnded)
    {
      this.m_isArtifactContentEnds = true;
      if (this.m_nextChar == ')' && this.m_currentChar != '\\')
      {
        this.m_isArtifactContentEnds = false;
        this.m_isContentEnded = true;
      }
    }
    return this.GetNextChar();
  }

  private bool IsWhiteSpace(char ch)
  {
    switch (ch)
    {
      case char.MinValue:
      case '\t':
      case '\n':
      case '\f':
      case '\r':
      case ' ':
        return true;
      default:
        return false;
    }
  }

  private bool IsDelimiter(char ch)
  {
    switch (ch)
    {
      case '%':
      case '(':
      case ')':
      case '/':
      case '<':
      case '>':
      case '[':
      case ']':
        return true;
      default:
        return false;
    }
  }

  private bool CheckForTextOperator()
  {
    char nextChar = this.m_nextChar;
    int num = 0;
    if (Array.IndexOf<string>(ContentLexer.m_textShowers, nextChar.ToString()) >= 0)
      return true;
    if (this.IsWhiteSpace(nextChar))
    {
      nextChar = (char) this.m_contentStream[this.m_charPointer];
      ++num;
    }
    string str = nextChar.ToString() + ((char) this.m_contentStream[this.m_charPointer + num]).ToString();
    return Array.IndexOf<string>(ContentLexer.m_textShowers, str) >= 0;
  }

  internal void Dispose()
  {
    if (this.m_contentStream == null)
      return;
    this.m_contentStream = (byte[]) null;
  }
}
