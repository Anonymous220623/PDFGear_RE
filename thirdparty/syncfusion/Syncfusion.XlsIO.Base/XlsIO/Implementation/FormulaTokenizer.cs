// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.FormulaTokenizer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class FormulaTokenizer
{
  private const char FormulaEnd = '\u0001';
  private const char Whitespace = ' ';
  private const char Colon = ':';
  private char m_chCurrent;
  private int m_iFormulaLength;
  private int m_iPos;
  private string m_strFormula;
  private int m_iStartPos;
  public FormulaToken TokenType;
  public FormulaToken PreviousTokenType;
  private int m_iPrevPos;
  private StringBuilder m_value = new StringBuilder();
  private WorkbookImpl m_book;
  private char m_chArgumentSeparator = ',';
  private NumberFormatInfo m_numberFormat = NumberFormatInfo.CurrentInfo;
  private int m_lastIndexQuote = -1;
  private bool m_hasSubtract;

  public FormulaTokenizer(WorkbookImpl book)
  {
    this.m_book = book != null ? book : throw new ArgumentNullException(nameof (book));
  }

  public void Prepare(string formula)
  {
    this.m_strFormula = formula;
    this.m_iFormulaLength = formula.Length;
    this.m_iPos = 0;
    this.NextChar();
  }

  private void NextChar()
  {
    if (this.m_iPos < this.m_iFormulaLength)
    {
      this.m_chCurrent = this.m_strFormula[this.m_iPos];
      ++this.m_iPos;
    }
    else
      this.m_chCurrent = '\u0001';
  }

  private void MoveBack(char charToMoveTo)
  {
    int startIndex = Math.Min(this.m_strFormula.Length - 1, this.m_iPos);
    int num1 = this.m_strFormula.LastIndexOf(charToMoveTo, startIndex);
    if (num1 < 0)
      return;
    int num2 = this.m_iPos - num1;
    this.m_iPos = num1 + 1;
    this.m_value.Remove(this.m_value.Length - num2 + 1, num2 - 1);
    this.m_chCurrent = charToMoveTo;
  }

  internal bool HasSubtract
  {
    get => this.m_hasSubtract;
    set => this.m_hasSubtract = value;
  }

  public void NextToken()
  {
    this.PreviousTokenType = this.TokenType;
    this.m_iPrevPos = this.m_iStartPos;
    this.m_value.Length = 0;
    if (this.TokenType != FormulaToken.DDELink)
      this.TokenType = FormulaToken.None;
    char ch1 = this.m_numberFormat.NumberDecimalSeparator[0];
    char ch2 = ' ';
    char c = ' ';
    if (this.m_strFormula.IndexOf(this.m_chCurrent) > 0)
      ch2 = this.m_strFormula[this.m_strFormula.IndexOf(this.m_chCurrent) - 1];
    if (this.m_strFormula.IndexOf(this.m_chCurrent) < this.m_strFormula.Length - 1)
      c = this.m_strFormula[this.m_strFormula.IndexOf(this.m_chCurrent) + 1];
    while (true)
    {
      this.m_iStartPos = this.m_iPos;
      if (char.IsDigit(this.m_chCurrent))
        this.ParseNumber();
      else if ((this.m_chCurrent == '.' || (int) this.m_chCurrent == (int) ch1) && char.IsDigit(c) && this.PreviousTokenType != FormulaToken.CloseParenthesis)
      {
        this.ParseNumber();
      }
      else
      {
        switch (this.m_chCurrent)
        {
          case '\u0001':
            this.TokenType = FormulaToken.EndOfFormula;
            break;
          case '"':
            this.m_lastIndexQuote = -1;
            this.ParseString(true);
            this.TokenType = FormulaToken.tStringConstant;
            break;
          case '#':
            this.ParseError();
            break;
          case '%':
            this.NextChar();
            this.TokenType = FormulaToken.tPercent;
            break;
          case '&':
            this.NextChar();
            this.TokenType = FormulaToken.tConcat;
            break;
          case '\'':
            this.m_lastIndexQuote = -1;
            if (this.m_strFormula.Contains("'"))
            {
              int num1 = this.CharOccurs(this.m_strFormula, '\'') - this.GetCountInsideQuoted(this.m_strFormula, '\'');
              int num2 = this.CharOccurs(this.m_strFormula, '!', '\'');
              if (num1 % 2 != 0 || num2 != 0 && num1 / num2 <= 2 && (this.CharOccurs(this.m_strFormula, '(') - this.GetCountInsideQuoted(this.m_strFormula, '(') + (this.CharOccurs(this.m_strFormula, ')') - this.GetCountInsideQuoted(this.m_strFormula, ')'))) % 2 != 0)
                this.m_lastIndexQuote = this.m_strFormula.IndexOf('!') - 1;
            }
            this.ParseString(true);
            if (this.PreviousTokenType == FormulaToken.DDELink)
            {
              this.NextChar();
              break;
            }
            this.TokenType = FormulaToken.Identifier;
            this.ParseIdentifier();
            break;
          case '(':
            this.NextChar();
            this.TokenType = FormulaToken.tParentheses;
            break;
          case ')':
            this.NextChar();
            this.TokenType = FormulaToken.CloseParenthesis;
            break;
          case '*':
            this.NextChar();
            this.TokenType = FormulaToken.tMul;
            break;
          case '+':
            this.NextChar();
            this.TokenType = FormulaToken.tAdd;
            if (char.IsDigit(this.m_chCurrent) && ch2 == ' ' && this.PreviousTokenType == FormulaToken.Space && this.HasSubtract)
            {
              this.ParseNumber();
              StringBuilder stringBuilder = new StringBuilder("+");
              stringBuilder.Append((object) this.m_value);
              this.m_value = stringBuilder;
              this.TokenType = FormulaToken.tNumber;
              break;
            }
            break;
          case '-':
            this.NextChar();
            this.TokenType = FormulaToken.tSub;
            if (char.IsDigit(this.m_chCurrent) && ch2 == ' ' && this.PreviousTokenType == FormulaToken.Space && this.HasSubtract)
            {
              this.ParseNumber();
              StringBuilder stringBuilder = new StringBuilder("-");
              stringBuilder.Append((object) this.m_value);
              this.m_value = stringBuilder;
              this.TokenType = FormulaToken.tNumber;
              break;
            }
            break;
          case '/':
            this.NextChar();
            this.TokenType = FormulaToken.tDiv;
            break;
          case ':':
            this.NextChar();
            this.TokenType = FormulaToken.tCellRange;
            break;
          case '<':
            this.ProcessLess();
            break;
          case '=':
            this.NextChar();
            this.TokenType = FormulaToken.tEqual;
            break;
          case '>':
            this.ProcessGreater();
            break;
          case '^':
            this.TokenType = FormulaToken.tPower;
            this.NextChar();
            break;
          case '{':
            this.ParseArray();
            this.TokenType = FormulaToken.tArray1;
            break;
          default:
            if ((int) this.m_chCurrent == (int) this.m_chArgumentSeparator || (int) this.m_chCurrent == (int) Convert.ToChar(","))
            {
              this.NextChar();
              this.TokenType = FormulaToken.Comma;
              break;
            }
            if (this.m_chCurrent > ' ')
            {
              this.ParseIdentifier();
              break;
            }
            if (this.m_chCurrent == ' ')
            {
              this.ParseSpace();
              break;
            }
            break;
        }
      }
      if (this.TokenType == FormulaToken.None)
        this.NextChar();
      else
        break;
    }
  }

  private int GetCountInsideQuoted(string formula, char symbol)
  {
    int countInsideQuoted = 0;
    foreach (Match match in new Regex("\".*?\"").Matches(formula))
    {
      string stringToSearch = match.ToString();
      if (stringToSearch.Contains(symbol.ToString()))
        countInsideQuoted += this.CharOccurs(stringToSearch, symbol);
    }
    return countInsideQuoted;
  }

  private int CharOccurs(string stringToSearch, char charToFind)
  {
    int num = 0;
    int startIndex = 0;
    while (stringToSearch.IndexOf(charToFind, startIndex) > -1)
    {
      startIndex = stringToSearch.IndexOf(charToFind, startIndex) + 1;
      ++num;
    }
    return num;
  }

  private int CharOccurs(string stringToSearch, char charToFind, char previousChar)
  {
    int num1 = 0;
    int num2 = 0;
    char[] charArray = stringToSearch.ToCharArray();
    foreach (int num3 in charArray)
    {
      if (num3 == (int) charToFind && (int) charArray[num2 - 1] == (int) previousChar)
        ++num1;
      ++num2;
    }
    return num1;
  }

  public void SaveState()
  {
    this.m_iPrevPos = this.m_iStartPos;
    this.PreviousTokenType = this.TokenType;
  }

  public void RestoreState()
  {
    this.m_iStartPos = this.m_iPrevPos;
    this.TokenType = this.PreviousTokenType;
    this.m_chCurrent = this.m_strFormula[this.m_iStartPos];
    this.m_value.Length = 0;
  }

  private void ParseNumber()
  {
    this.TokenType = FormulaToken.tInteger;
    this.AppendNumbers();
    if ((int) this.m_chCurrent == (int) this.m_numberFormat.NumberDecimalSeparator[0] && char.IsDigit(this.m_strFormula[this.m_iPos]))
    {
      this.TokenType = FormulaToken.tNumber;
      this.m_value.Append(this.m_chCurrent);
      this.NextChar();
      this.AppendNumbers();
    }
    if (char.ToUpper(this.m_chCurrent) == 'E')
    {
      this.TokenType = FormulaToken.tNumber;
      this.m_value.Append(this.m_chCurrent);
      this.NextChar();
      if (this.m_chCurrent == '-' || this.m_chCurrent == '+')
      {
        this.m_value.Append(this.m_chCurrent);
        this.NextChar();
      }
      this.AppendNumbers();
    }
    else
    {
      if (this.m_chCurrent != ':')
        return;
      this.TokenType = FormulaToken.Identifier;
      this.m_value.Append(this.m_chCurrent);
      this.NextChar();
      this.AppendNumbers();
    }
  }

  private void ProcessGreater()
  {
    this.NextChar();
    if (this.m_chCurrent == '=')
    {
      this.NextChar();
      this.TokenType = FormulaToken.tGreaterEqual;
    }
    else
      this.TokenType = FormulaToken.tGreater;
  }

  private void ProcessLess()
  {
    this.NextChar();
    if (this.m_chCurrent == '=')
    {
      this.NextChar();
      this.TokenType = FormulaToken.tLessEqual;
    }
    else if (this.m_chCurrent == '>')
    {
      this.NextChar();
      this.TokenType = FormulaToken.tNotEqual;
    }
    else
      this.TokenType = FormulaToken.tLessThan;
  }

  private void AppendNumbers()
  {
    while (char.IsDigit(this.m_chCurrent))
    {
      this.m_value.Append(this.m_chCurrent);
      this.NextChar();
    }
  }

  private void ParseIdentifier()
  {
    bool flag1 = false;
    bool flag2 = false;
    int num1 = 0;
    bool flag3 = false;
    int num2 = 0;
    while (char.IsLetterOrDigit(this.m_chCurrent) || this.m_chCurrent >= '\u0080' || this.m_chCurrent == '_' || this.m_chCurrent == '!' || this.m_chCurrent == ':' || this.m_chCurrent == '.' || this.m_chCurrent == '$' || this.m_chCurrent == '[' || this.m_chCurrent == '\'' || this.m_chCurrent == '#' || this.m_chCurrent == ']' || this.m_chCurrent == '?' || this.m_chCurrent == ' ' && flag3 || this.m_chCurrent == '\\')
    {
      if (this.m_chCurrent == '!')
      {
        flag1 = true;
        ++num1;
        if (this.TokenType == FormulaToken.DDELink)
        {
          this.NextChar();
          break;
        }
        if (num1 > 1 && flag2)
        {
          this.MoveBack(':');
          break;
        }
      }
      else if (this.m_chCurrent == ':')
      {
        flag2 = true;
        num2 = this.m_iPos;
        flag3 = true;
      }
      if (this.m_chCurrent == ' ' && flag3)
      {
        this.NextChar();
      }
      else
      {
        this.m_value.Append(this.m_chCurrent);
        if (this.m_chCurrent != ':')
          flag3 = false;
        char ch = '\u0001';
        if (this.m_chCurrent == '[')
          ch = ']';
        else if (this.m_chCurrent == '\'')
          ch = this.m_chCurrent;
        if (ch != '\u0001')
        {
          int num3 = 1;
          while (this.m_chCurrent != '\u0001' && num3 > 0)
          {
            this.NextChar();
            if (this.m_chCurrent != '\u0001')
              this.m_value.Append(this.m_chCurrent);
            if ((int) this.m_chCurrent == (int) ch)
              --num3;
            if (this.m_chCurrent == '[')
              ++num3;
            if (this.m_chCurrent == '\'' && num3 == 1 && this.m_iPos < this.m_strFormula.Length && this.m_strFormula[this.m_iPos] == '!')
              break;
          }
        }
        this.NextChar();
      }
    }
    string str1 = this.m_value.ToString();
    string[] strArray1 = str1.Split(':');
    bool flag4 = false;
    if (str1 != null && (str1.StartsWith("http") || str1.StartsWith("ftp") || str1.StartsWith("file")))
    {
      Match match = new Regex("(?<Protocol>http:|https:|file:|ftp:)(?<DirectoryName>[\\S ]*\\/|[\\S ]*\\\\)*(?<BookName>\\[[\\S^ ']+\\])?(?<SheetName>[\\S ]+)!(?<Range>[\\S]+)", RegexOptions.IgnoreCase).Match(str1);
      if (match.Success)
      {
        strArray1 = new string[2]
        {
          match.Groups["Protocol"].Value + match.Groups["DirectoryName"].Value,
          $"{match.Groups["BookName"].Value}!{match.Groups["Range"].Value}"
        };
        flag4 = true;
      }
    }
    else if (str1.Contains(":") && !str1.EndsWith(":") && !str1.StartsWith(":"))
    {
      string text = strArray1[0];
      string[] strArray2 = strArray1[1].Split(new char[1]
      {
        '!'
      }, StringSplitOptions.RemoveEmptyEntries);
      string str2 = this.ProcessPtg(text);
      for (int index = 0; index < strArray2.Length; ++index)
        strArray2[index] = this.ProcessPtg(strArray2[index]);
      char c1 = str2[str2.Length - 1];
      string str3 = strArray2[strArray2.Length - 1];
      char c2 = str3[str3.Length - 1];
      if (!char.IsNumber(c2) && char.IsLetter(c2) && (char.IsNumber(c1) || str2.EndsWith("#REF!")))
      {
        this.m_iPos = num2;
        this.m_chCurrent = ':';
        this.m_value.Remove(0, this.m_value.Length);
        this.m_value.Append(strArray1[0]);
      }
    }
    if (this.TokenType == FormulaToken.DDELink)
      return;
    if (this.m_chCurrent == '(')
      this.TokenType = FormulaToken.tFunction1;
    else if (string.Compare(str1, "true", StringComparison.CurrentCultureIgnoreCase) == 0)
      this.TokenType = FormulaToken.ValueTrue;
    else if (string.Compare(str1, "false", StringComparison.CurrentCultureIgnoreCase) == 0)
      this.TokenType = FormulaToken.ValueFalse;
    else if (this.m_chCurrent == '|')
    {
      this.NextChar();
      this.TokenType = FormulaToken.DDELink;
    }
    else if (this.m_value.ToString() == "Overview!#REF!")
    {
      if (strArray1.Length <= 2)
        this.TokenType = FormulaToken.Identifier3D;
      else
        this.TokenType = FormulaToken.Identifier;
    }
    else if (this.m_value.ToString().EndsWith("!#REF!"))
    {
      if (strArray1.Length <= 2 || flag4)
        this.TokenType = FormulaToken.tError;
      else
        this.TokenType = FormulaToken.Identifier;
    }
    else
      this.TokenType = flag1 ? FormulaToken.Identifier3D : FormulaToken.Identifier;
  }

  private string ProcessPtg(string text)
  {
    if (FormulaUtil.IsR1C1(text) && !char.IsNumber(text[text.Length - 1]))
      text = text.Replace(text, "R0C0");
    return text;
  }

  private void ParseSpace()
  {
    while (this.m_chCurrent == ' ')
    {
      this.m_value.Append(this.m_chCurrent);
      this.NextChar();
    }
    this.TokenType = FormulaToken.Space;
  }

  private void ParseString(bool InQuote)
  {
    char ch = char.MinValue;
    if (InQuote)
    {
      ch = this.m_chCurrent;
      this.NextChar();
    }
    while (this.m_chCurrent != '\u0001')
    {
      if (InQuote && (int) this.m_chCurrent == (int) ch && this.CheckQuote())
      {
        this.NextChar();
        if ((int) this.m_chCurrent != (int) ch)
          return;
        this.m_value.Append(this.m_chCurrent);
        this.NextChar();
      }
      else
      {
        this.m_value.Append(this.m_chCurrent);
        this.NextChar();
      }
    }
    if (!InQuote)
      return;
    this.RaiseException($"Incomplete string, missing {(object) ch}; String started", (Exception) null);
  }

  private bool CheckQuote()
  {
    return this.m_lastIndexQuote == -1 || this.m_iPos == this.m_lastIndexQuote + 1;
  }

  private void ParseError()
  {
    ICollection<string> keys = (ICollection<string>) FormulaUtil.ErrorNameToCode.Keys;
    string str = (string) null;
    foreach (string strA in (IEnumerable<string>) keys)
    {
      int length = strA.Length;
      if (string.Compare(strA, 0, this.m_strFormula, this.m_iPos - 1, length, StringComparison.CurrentCultureIgnoreCase) == 0)
      {
        str = strA;
        break;
      }
    }
    this.m_value.Length = 0;
    this.m_value.Append(str);
    this.m_iPos += str.Length - 1;
    this.TokenType = FormulaToken.tError;
    this.NextChar();
  }

  private void ParseArray()
  {
    this.m_value.Length = 0;
    while (this.m_chCurrent != '}' && this.m_chCurrent != '\u0001')
    {
      this.m_value.Append(this.m_chCurrent);
      if (this.m_chCurrent == '"')
        this.SkipString();
      else
        this.NextChar();
    }
    this.m_value.Append(this.m_chCurrent);
    if (this.m_chCurrent == '\u0001')
      this.RaiseException("Couldn't find end of array", (Exception) null);
    this.NextChar();
  }

  private void SkipString()
  {
    char chCurrent = this.m_chCurrent;
    this.NextChar();
    while (this.m_chCurrent != '\u0001')
    {
      this.m_value.Append(this.m_chCurrent);
      if ((int) this.m_chCurrent == (int) chCurrent)
      {
        this.NextChar();
        if ((int) this.m_chCurrent == (int) chCurrent)
          this.m_value.Append(this.m_chCurrent);
        else
          break;
      }
      this.NextChar();
    }
    if (this.m_chCurrent != '\u0001')
      return;
    this.RaiseException("Can't find end of the string", (Exception) null);
  }

  public void RaiseException(string msg, Exception ex)
  {
    if (ex is ParseException)
    {
      msg = $"{msg}. {ex.Message}";
    }
    else
    {
      msg = $"{msg}  at position {(object) this.m_iStartPos}";
      if (ex != null)
        msg = $"{msg}. {ex.Message}";
    }
    throw new ParseException(msg, this.m_strFormula, this.m_iPos, ex);
  }

  public void RaiseUnexpectedToken(string msg)
  {
    if (msg == null || msg.Length == 0)
      msg = string.Empty;
    this.RaiseException($"{msg}Unexpected token type: {this.TokenType}, string value: {this.m_value}", (Exception) null);
  }

  private int CharOccurs(string stringToSearch, char charToFind, char previousChar, char nextChar)
  {
    int num1 = 0;
    int num2 = 0;
    char[] charArray = stringToSearch.ToCharArray();
    foreach (int num3 in charArray)
    {
      if (num3 == (int) charToFind && num2 != 0 && (int) charArray[num2 - 1] == (int) previousChar && num2 != charArray.Length - 1 && (int) charArray[num2 + 1] == (int) nextChar)
        ++num1;
      ++num2;
    }
    return num1;
  }

  public string TokenString => this.m_value.ToString();

  public char ArgumentSeparator
  {
    get => this.m_chArgumentSeparator;
    set => this.m_chArgumentSeparator = value;
  }

  public NumberFormatInfo NumberFormat
  {
    get => this.m_numberFormat;
    set => this.m_numberFormat = value;
  }
}
