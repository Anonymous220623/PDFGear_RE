// Decompiled with JetBrains decompiler
// Type: RtfLexer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;

#nullable disable
internal class RtfLexer
{
  private const char GroupStart = '{';
  private const char GroupEnd = '}';
  private const char ControlStart = '\\';
  private const char Space = ' ';
  private const char WhiteSpace = '\r';
  private const char NewLine = '\n';
  private const char SemiColon = ';';
  private const char DoubleQuotes = '"';
  private const char BackQuote = '`';
  private const char OpenParenthesis = '(';
  private const char CloseParenthesis = ')';
  private const char Ambersion = '&';
  private const char Percentage = '%';
  private const char DollarSign = '$';
  private const char Hash = '#';
  private const char AtSymbol = '@';
  private const char Exclamation = '!';
  private const char Plus = '+';
  private const char Caret = '^';
  private const char OpenBracket = '[';
  private const char CloseBracket = ']';
  private const char ForwardSlash = '/';
  private const char QuestionMark = '?';
  private const char GreaterThan = '>';
  private const char LesserThan = '<';
  private const char Comma = ',';
  private const char VerticalBar = '|';
  private const char Colon = ':';
  private RtfTableType m_currRtfTableType;
  private RtfReader m_rtfReader;
  private string m_token;
  internal char m_prevChar;
  private bool m_bIsImageBytes;
  private bool m_bIsReadNewChar = true;
  private RtfTokenType m_rtfTokenType;
  private char m_newChar;
  private List<string> m_commentStartRange;
  private int m_commentCount = 2;
  private string m_prevToken = string.Empty;
  private string m_commStartRangeTokenKey = string.Empty;
  private bool m_hasCommentTagmark;
  internal char[] m_delimeters = new char[28]
  {
    '{',
    '}',
    '\\',
    ' ',
    '\r',
    '\n',
    ';',
    '"',
    '`',
    '(',
    ')',
    '[',
    ']',
    '&',
    '%',
    '$',
    '#',
    '@',
    '!',
    '+',
    '^',
    '/',
    '?',
    '>',
    '<',
    ',',
    '|',
    ':'
  };

  internal bool IsImageBytes
  {
    get => this.m_bIsImageBytes;
    set => this.m_bIsImageBytes = value;
  }

  internal List<string> CommentRangeStartId
  {
    get
    {
      if (this.m_commentStartRange == null)
        this.m_commentStartRange = new List<string>();
      return this.m_commentStartRange;
    }
    set => this.m_commentStartRange = value;
  }

  internal RtfTableType CurrRtfTableType
  {
    get => this.m_currRtfTableType;
    set => this.m_currRtfTableType = value;
  }

  internal RtfTokenType CurrRtfTokenType
  {
    get => this.m_rtfTokenType;
    set => this.m_rtfTokenType = value;
  }

  internal RtfLexer(RtfReader rtfReader) => this.m_rtfReader = rtfReader;

  internal string ReadNextToken(string prevTokenKey)
  {
    this.m_token = (string) null;
    this.m_newChar = !this.m_bIsReadNewChar ? this.m_prevChar : this.m_rtfReader.ReadChar();
    switch (this.m_newChar)
    {
      case '\n':
      case '\r':
        this.m_bIsReadNewChar = true;
        this.m_token = this.m_newChar.ToString();
        return this.m_token;
      case ' ':
        this.m_token = ' '.ToString();
        this.m_token = this.ReadDocumentElement(this.m_token, prevTokenKey);
        if (!this.m_prevToken.Equals(string.Empty) && this.m_token != this.m_prevToken && this.m_token != "-")
        {
          this.m_token = this.GenerateCommentInfo(this.m_prevToken, this.m_token);
          this.m_prevToken = string.Empty;
        }
        return this.m_token;
      case '\\':
        this.m_token = '\\'.ToString();
        this.m_token = this.ReadControlWord(this.m_token, prevTokenKey);
        return this.m_token;
      case '{':
        this.m_bIsReadNewChar = true;
        return this.m_newChar.ToString();
      case '}':
        this.m_bIsReadNewChar = true;
        return this.m_newChar.ToString();
      default:
        if (this.IsImageBytes)
        {
          this.m_token = this.m_newChar.ToString() + this.m_rtfReader.ReadImageBytes();
          this.m_bIsReadNewChar = true;
          return this.m_token;
        }
        this.m_bIsReadNewChar = true;
        if (!(this.m_commStartRangeTokenKey != string.Empty))
          return this.m_newChar.ToString();
        this.m_token += (string) (object) this.m_newChar;
        this.m_token = this.ReadDocumentElement(this.m_token, prevTokenKey);
        this.m_commStartRangeTokenKey = string.Empty;
        return this.m_token;
    }
  }

  private string ReadControlWord(string token, string prevTokenKey)
  {
    this.m_newChar = this.m_rtfReader.ReadChar();
    if (this.m_newChar == '\\' || this.m_newChar == '{' || this.m_newChar == '}')
    {
      this.m_bIsReadNewChar = true;
      return token + (object) this.m_newChar;
    }
    this.m_bIsReadNewChar = false;
    while (Array.IndexOf<char>(this.m_delimeters, this.m_newChar) == -1 && (!token.StartsWith("\\u") || token.Length <= 2 || !char.IsNumber(token[2]) || char.IsNumber(this.m_newChar)))
    {
      token += (string) (object) this.m_newChar;
      if ((long) this.m_rtfReader.Position < this.m_rtfReader.Length)
      {
        this.m_newChar = this.m_rtfReader.ReadChar();
        if (token.Equals("\\atrfstart") || token.Equals("\\atrfend") || token.Equals("\\atnref") || token.Equals("\\atnid") || token.StartsWith("\\atndate") || token.Equals("\\atnauthor") || token.Equals("\\atnparent"))
        {
          if (token.Equals("\\atrfstart") || token.Equals("\\atrfend") || token.Equals("\\atnref"))
            this.m_hasCommentTagmark = true;
          this.m_prevToken = token;
        }
        if (token.Equals("\\annotation"))
        {
          ++this.m_commentCount;
          string str = this.m_commentCount.ToString();
          token += str;
        }
        if ((token.StartsWith("\\bin") || prevTokenKey == "bin") && this.m_newChar == '\u0001')
        {
          this.m_bIsReadNewChar = true;
          break;
        }
      }
      else
        break;
    }
    this.m_prevChar = this.m_newChar;
    return token == null ? this.m_newChar.ToString() : token;
  }

  private string GenerateCommentInfo(string prevToken, string token)
  {
    if (prevToken.Equals("\\atrfstart"))
    {
      this.m_commStartRangeTokenKey = "atrfstart";
      ++this.m_commentCount;
      this.CommentRangeStartId.Add(token);
      prevToken += token.TrimStart();
    }
    if (prevToken.Equals("\\atrfend"))
    {
      foreach (string str in this.CommentRangeStartId)
      {
        if (str.Equals(token))
          prevToken += token.TrimStart();
      }
    }
    if (prevToken.Equals("\\atnref"))
    {
      foreach (string str in this.CommentRangeStartId)
      {
        if (str.Equals(token))
          prevToken += token.TrimStart();
      }
      this.m_hasCommentTagmark = false;
    }
    if (prevToken.Equals("\\atndate"))
      prevToken += token.TrimStart();
    if (prevToken.Equals("\\atnid"))
      prevToken += token.TrimStart();
    if (prevToken.Equals("\\atnauthor"))
      prevToken += token.TrimStart();
    if (prevToken.Equals("\\atnparent"))
      prevToken += token;
    return prevToken;
  }

  private string ReadDocumentElement(string token, string prevTokenKey)
  {
    if (this.IsImageBytes)
    {
      this.m_newChar = this.m_rtfReader.ReadChar();
      if (this.m_newChar != '\\' && this.m_newChar != ';' && this.m_newChar != '{' && this.m_newChar != '}' && this.m_newChar != '\r' && this.m_newChar != '\n')
      {
        token = this.m_newChar.ToString();
        if (!(prevTokenKey == "bin") || this.m_newChar != '\u0001')
          token += this.m_rtfReader.ReadImageBytes();
        this.m_bIsReadNewChar = true;
        return token;
      }
      this.m_bIsReadNewChar = false;
      this.m_prevChar = this.m_newChar;
      return token;
    }
    for (this.m_newChar = this.m_rtfReader.ReadChar(); this.m_newChar != '\\' && this.m_newChar != ';' && this.m_newChar != '{' && this.m_newChar != '}' && this.m_newChar != '\r' && this.m_newChar != '\n'; this.m_newChar = this.m_rtfReader.ReadChar())
    {
      token += (string) (object) this.m_newChar;
      if ((long) this.m_rtfReader.Position >= this.m_rtfReader.Length)
        break;
    }
    this.m_prevChar = this.m_newChar;
    this.m_bIsReadNewChar = false;
    if (token != null)
      return token;
    if (this.m_prevChar != '\\')
      this.m_bIsReadNewChar = true;
    return this.m_newChar.ToString();
  }

  internal void Close() => this.m_rtfReader.Close();
}
