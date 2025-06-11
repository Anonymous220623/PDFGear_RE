// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.RtfLexer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

public class RtfLexer
{
  private const char c_groupStart = '{';
  private const char c_groupEnd = '}';
  private const char c_controlStart = '\\';
  private const char c_space = ' ';
  private const char c_whiteSpace = '\r';
  private const char c_newLine = '\n';
  private const char c_semiColon = ';';
  private const char c_doubleQuotes = '"';
  private const char c_backQuote = '`';
  private const char c_openParenthesis = '(';
  private const char c_closeParenthesis = ')';
  private const char c_ambersion = '&';
  private const char c_percentage = '%';
  private const char c_dollarSign = '$';
  private const char c_hash = '#';
  private const char c_atsymbol = '@';
  private const char c_exclamation = '!';
  private const char c_plus = '+';
  private const char c_caret = '^';
  private const char c_openBracket = '[';
  private const char c_closeBracket = ']';
  private const char c_forwardSlash = '/';
  private const char c_questionmark = '?';
  private const char c_greaterthan = '>';
  private const char c_lesserthan = '<';
  private const char c_comma = ',';
  private const char c_verticalBar = '|';
  private const char c_colon = ':';
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

  public bool IsImageBytes
  {
    get => this.m_bIsImageBytes;
    set => this.m_bIsImageBytes = value;
  }

  public List<string> CommentRangeStartId
  {
    get
    {
      if (this.m_commentStartRange == null)
        this.m_commentStartRange = new List<string>();
      return this.m_commentStartRange;
    }
    set => this.m_commentStartRange = value;
  }

  public RtfTableType CurrRtfTableType
  {
    get => this.m_currRtfTableType;
    set => this.m_currRtfTableType = value;
  }

  public RtfTokenType CurrRtfTokenType
  {
    get => this.m_rtfTokenType;
    set => this.m_rtfTokenType = value;
  }

  public RtfLexer(RtfReader rtfReader) => this.m_rtfReader = rtfReader;

  public string ReadNextToken(string prevTokenKey) => this.ReadNextToken(prevTokenKey, false);

  internal string ReadNextToken(string prevTokenKey, bool isLevelText)
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
        this.m_token = this.ReadControlWord(this.m_token, prevTokenKey, isLevelText);
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

  private string ReadControlWord(string token, string prevTokenKey, bool isLevelText)
  {
    this.m_newChar = this.m_rtfReader.ReadChar();
    if (this.m_newChar == '\\' || this.m_newChar == '{' || this.m_newChar == '}')
    {
      this.m_bIsReadNewChar = true;
      return token + (object) this.m_newChar;
    }
    this.m_bIsReadNewChar = false;
    while ((Array.IndexOf<char>(this.m_delimeters, this.m_newChar) == -1 || isLevelText && this.m_newChar == ')') && (!this.StartsWithExt(token, "\\u") || token.Length <= 2 || !char.IsNumber(token[2]) || char.IsNumber(this.m_newChar)))
    {
      token += (string) (object) this.m_newChar;
      if ((long) this.m_rtfReader.Position < this.m_rtfReader.Length)
      {
        this.m_newChar = this.m_rtfReader.ReadChar();
        if (token.Equals("\\atrfstart") || token.Equals("\\atrfend") || token.Equals("\\atnref") || token.Equals("\\atnid") || this.StartsWithExt(token, "\\atndate") || token.Equals("\\atnauthor") || token.Equals("\\atnparent"))
          this.m_prevToken = token;
        if (token.Equals("\\annotation"))
        {
          ++this.m_commentCount;
          string str = this.m_commentCount.ToString();
          token += str;
        }
        if ((this.StartsWithExt(token, "\\bin") || prevTokenKey == "bin") && this.m_newChar == '\u0001')
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
    this.m_newChar = this.m_rtfReader.ReadChar();
    while (this.m_newChar != '\\' && this.m_newChar != ';' && this.m_newChar != '{' && this.m_newChar != '}' && this.m_newChar != '\r' && this.m_newChar != '\n')
    {
      token += (string) (object) this.m_newChar;
      if ((long) this.m_rtfReader.Position < this.m_rtfReader.Length)
      {
        this.m_newChar = this.m_rtfReader.ReadChar();
        if ((this.m_newChar == '{' || this.m_newChar == '}') && this.m_prevChar == '\u0084')
        {
          token += (string) (object) this.m_newChar;
          if ((long) this.m_rtfReader.Position < this.m_rtfReader.Length)
            this.m_newChar = this.m_rtfReader.ReadChar();
          else
            break;
        }
        this.m_prevChar = this.m_newChar;
      }
      else
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

  public void Close() => this.m_rtfReader.Close();

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);
}
