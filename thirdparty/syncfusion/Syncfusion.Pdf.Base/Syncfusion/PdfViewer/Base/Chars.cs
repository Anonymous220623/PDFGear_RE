// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Chars
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal static class Chars
{
  public const char NotDef = '\0';
  public const char Null = '\0';
  public const byte SingleByteSpace = 32 /*0x20*/;
  public const char Ap = '\'';
  public const char Qu = '"';
  public const char Space = ' ';
  public const char Tab = '\t';
  public const char LF = '\n';
  public const char FF = '\f';
  public const char CR = '\r';
  public const char DecimalPoint = '.';
  public const char LiteralStringStart = '(';
  public const char LiteralStringEnd = ')';
  public const char HexadecimalStringStart = '<';
  public const char HexadecimalStringEnd = '>';
  public const char Comment = '%';
  public const char Name = '/';
  public const char ArrayStart = '[';
  public const char ArrayEnd = ']';
  public const char ExecuteableArrayStart = '{';
  public const char ExecuteableArrayEnd = '}';
  public const char StringEscapeCharacter = '\\';
  public const char Minus = '-';
  public const char Plus = '+';
  public const char VerticalBar = '|';
  public static readonly char[] FontFamilyDelimiters = new char[2]
  {
    ',',
    '-'
  };

  public static bool IsWhiteSpace(int b) => char.IsWhiteSpace((char) b);

  public static bool IsOctalChar(int b)
  {
    char ch = (char) b;
    return '0' <= ch && ch <= '7';
  }

  public static bool IsHexChar(int b)
  {
    char ch = (char) b;
    if ('0' <= ch && ch <= '9' || 'A' <= ch && ch <= 'F')
      return true;
    return 'a' <= ch && ch <= 'f';
  }

  public static bool IsDelimiter(int b)
  {
    char c = (char) b;
    if (char.IsWhiteSpace(c))
      return true;
    switch (c)
    {
      case char.MinValue:
      case '%':
      case '(':
      case ')':
      case '/':
      case '<':
      case '>':
      case '[':
      case ']':
      case '{':
      case '}':
        return true;
      case '&':
      case '\'':
        return false;
      case '=':
        return false;
      case '\\':
        return false;
      case '|':
        return false;
      default:
        return false;
    }
  }

  public static bool IsLetter(int b) => char.IsLetter((char) b);

  public static bool IsValidNumberChar(IPostScriptParser reader)
  {
    char ch = (char) reader.Peek(0);
    switch (ch)
    {
      case '+':
      case '-':
        return Chars.IsDigitOrDecimalPoint((char) reader.Peek(1));
      default:
        return Chars.IsDigitOrDecimalPoint(ch);
    }
  }

  public static bool IsValidHexCharacter(int ch) => 80 /*0x50*/ <= ch && ch <= 128 /*0x80*/;

  private static bool IsDigitOrDecimalPoint(char ch) => char.IsDigit(ch) || ch == '.';
}
