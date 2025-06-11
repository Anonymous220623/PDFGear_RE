// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.StringTokenizer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class StringTokenizer
{
  public const char WhiteSpace = ' ';
  public const char Tab = '\t';
  private const RegexOptions c_regexOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled;
  private const string c_whiteSpacePatterm = "^[ \\t]+$";
  public static readonly char[] Spaces = new char[2]
  {
    ' ',
    '\t'
  };
  private static Regex s_whiteSpaceRegex = new Regex("^[ \\t]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
  private string m_text;
  private int m_position;

  public StringTokenizer(string text)
  {
    this.m_text = text != null ? text : throw new ArgumentNullException(nameof (text));
  }

  public bool EOF => this.m_position == this.m_text.Length;

  public int Length => this.m_text.Length;

  public int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  public static int GetCharsCount(string text, char symbol)
  {
    if (text == null)
      throw new ArgumentNullException("wholeText");
    int charsCount = 0;
    int startIndex = 0;
    do
    {
      int num = text.IndexOf(symbol, startIndex);
      if (num != -1)
      {
        ++charsCount;
        startIndex = num + 1;
      }
      else
        break;
    }
    while (startIndex != text.Length);
    return charsCount;
  }

  public static int GetCharsCount(string text, char[] symbols)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (symbols == null)
      throw new ArgumentNullException(nameof (symbols));
    int charsCount = 0;
    int index = 0;
    for (int length = text.Length; index < length; ++index)
    {
      char symbol = text[index];
      if (symbol == ' ' && char.IsWhiteSpace(text, index) && index != 0 && index != length - 1 && !char.IsWhiteSpace(text, index - 1) && !char.IsWhiteSpace(text, index + 1))
        ++charsCount;
      if (StringTokenizer.Contains(symbols, symbol))
        ++charsCount;
    }
    return charsCount;
  }

  public string ReadLine()
  {
    int position;
    for (position = this.m_position; position < this.Length; ++position)
    {
      char ch = this.m_text[position];
      switch (ch)
      {
        case '\n':
        case '\r':
          string str = this.m_text.Substring(this.m_position, position - this.m_position);
          this.m_position = position + 1;
          if (ch == '\r' && this.m_position < this.Length && this.m_text[this.m_position] == '\n')
            ++this.m_position;
          return str;
        default:
          continue;
      }
    }
    if (position <= this.m_position)
      return (string) null;
    string str1 = this.m_text.Substring(this.m_position, position - this.m_position);
    this.m_position = position;
    return str1;
  }

  public string PeekLine()
  {
    int position = this.m_position;
    string str = this.ReadLine();
    this.m_position = position;
    return str;
  }

  public string ReadWord()
  {
    int position;
    for (position = this.m_position; position < this.Length; ++position)
    {
      char ch = this.m_text[position];
      switch (ch)
      {
        case '\t':
        case ' ':
          if (position == this.m_position)
            ++position;
          string str1 = this.m_text.Substring(this.m_position, position - this.m_position);
          this.m_position = position;
          return str1;
        case '\n':
        case '\r':
          string str2 = this.m_text.Substring(this.m_position, position - this.m_position);
          this.m_position = position + 1;
          if (ch == '\r' && this.m_position < this.Length && this.m_text[this.m_position] == '\n')
            ++this.m_position;
          return str2;
        default:
          continue;
      }
    }
    if (position <= this.m_position)
      return (string) null;
    string str = this.m_text.Substring(this.m_position, position - this.m_position);
    this.m_position = position;
    return str;
  }

  public string PeekWord()
  {
    int position = this.m_position;
    string str = this.ReadWord();
    this.m_position = position;
    return str;
  }

  public char Read()
  {
    char minValue = char.MinValue;
    if (!this.EOF)
    {
      minValue = this.m_text[this.m_position];
      ++this.m_position;
    }
    return minValue;
  }

  public string Read(int count)
  {
    int num = 0;
    StringBuilder stringBuilder = new StringBuilder();
    for (; !this.EOF && num < count; ++num)
    {
      char ch = this.Read();
      stringBuilder.Append(ch);
    }
    return stringBuilder.ToString();
  }

  public string ReadToSymbol(char symbol, bool readSymbol)
  {
    StringBuilder stringBuilder = new StringBuilder();
    while (!this.EOF)
    {
      char ch = this.Peek();
      if ((int) ch == (int) symbol)
      {
        if (readSymbol)
        {
          int num = (int) this.Read();
          stringBuilder.Append(ch);
          break;
        }
        break;
      }
      stringBuilder.Append(ch);
      int num1 = (int) this.Read();
    }
    return stringBuilder.ToString();
  }

  public char Peek()
  {
    char minValue = char.MinValue;
    if (!this.EOF)
      minValue = this.m_text[this.m_position];
    return minValue;
  }

  public void Close() => this.m_text = (string) null;

  public string ReadToEnd()
  {
    string end = this.m_position != 0 ? this.m_text.Substring(this.m_position, this.Length - this.m_position) : this.m_text;
    this.m_position = this.Length;
    return end;
  }

  internal static bool IsWhitespace(string token)
  {
    if (token == null)
      return false;
    try
    {
      return StringTokenizer.s_whiteSpaceRegex.Match(token).Success;
    }
    catch
    {
      return false;
    }
  }

  internal static bool IsSpace(char token) => token == ' ';

  internal static bool IsTab(char token) => token == '\t';

  internal static int GetWhitespaceCount(string line, bool start)
  {
    if (line == null)
      throw new ArgumentNullException(nameof (line));
    int whitespaceCount = 0;
    if (line.Length > 0)
    {
      for (int index = start ? 0 : line.Length - 1; index >= 0 && index < line.Length; index = start ? index + 1 : index - 1)
      {
        char token = line[index];
        if (StringTokenizer.IsSpace(token) || StringTokenizer.IsTab(token))
          ++whitespaceCount;
        else
          break;
      }
    }
    return whitespaceCount;
  }

  private static bool Contains(char[] array, char symbol)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    bool flag = false;
    for (int index = 0; index < array.Length; ++index)
    {
      if ((int) array[index] == (int) symbol)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }
}
