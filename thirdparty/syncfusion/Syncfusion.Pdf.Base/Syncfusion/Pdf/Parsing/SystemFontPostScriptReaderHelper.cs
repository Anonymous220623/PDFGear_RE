// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPostScriptReaderHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal static class SystemFontPostScriptReaderHelper
{
  private static int[] charValues = new int[128 /*0x80*/];

  static SystemFontPostScriptReaderHelper()
  {
    for (int index = 0; index < 10; ++index)
      SystemFontPostScriptReaderHelper.charValues[48 /*0x30*/ + index] = index;
    for (int index = 0; index < 6; ++index)
    {
      SystemFontPostScriptReaderHelper.charValues[97 + index] = 10 + index;
      SystemFontPostScriptReaderHelper.charValues[65 + index] = 10 + index;
    }
  }

  public static void GoToNextLine(SystemFontIPostScriptReader reader)
  {
    do
      ;
    while (!reader.EndOfFile && reader.Read() != (byte) 10);
  }

  public static void SkipUnusedCharacters(SystemFontIPostScriptReader reader)
  {
    SystemFontPostScriptReaderHelper.SkipWhiteSpaces(reader);
    while (!reader.EndOfFile && reader.Peek(0) == (byte) 37)
    {
      SystemFontPostScriptReaderHelper.GoToNextLine(reader);
      SystemFontPostScriptReaderHelper.SkipWhiteSpaces(reader);
    }
  }

  public static void SkipWhiteSpaces(SystemFontIPostScriptReader reader)
  {
    while (!reader.EndOfFile && SystemFontCharacters.IsWhiteSpace((int) reader.Peek(0)))
    {
      int num = (int) reader.Read();
    }
  }

  public static string GetString(byte[] bytes) => Encoding.UTF8.GetString(bytes, 0, bytes.Length);

  public static string ReadNumber(SystemFontIPostScriptReader reader)
  {
    StringBuilder stringBuilder = new StringBuilder();
    while (!reader.EndOfFile && SystemFontCharacters.IsValidNumberChar(reader))
      stringBuilder.Append((char) reader.Read());
    return stringBuilder.ToString();
  }

  public static string ReadName(SystemFontIPostScriptReader reader)
  {
    StringBuilder stringBuilder = new StringBuilder();
    int num = (int) reader.Read();
    while (!reader.EndOfFile && !SystemFontCharacters.IsDelimiter((int) reader.Peek(0)))
    {
      char ch = (char) reader.Read();
      if (ch == '#')
      {
        string result;
        if (SystemFontPostScriptReaderHelper.TryReadHexChar(reader, out result))
        {
          stringBuilder.Append(result);
        }
        else
        {
          stringBuilder.Append(ch);
          stringBuilder.Append(result);
        }
      }
      else
        stringBuilder.Append(ch);
    }
    return stringBuilder.ToString();
  }

  public static string ReadKeyword(SystemFontIPostScriptReader reader)
  {
    StringBuilder stringBuilder = new StringBuilder();
    while (!reader.EndOfFile && !SystemFontCharacters.IsDelimiter((int) reader.Peek(0)) && !char.IsNumber((char) reader.Peek(0)))
      stringBuilder.Append((char) reader.Read());
    return stringBuilder.ToString();
  }

  public static byte[] ReadHexadecimalString(SystemFontIPostScriptReader reader)
  {
    StringBuilder stringBuilder = new StringBuilder();
    int num1 = (int) reader.Read();
    while (!reader.EndOfFile && reader.Peek(0) != (byte) 62)
    {
      if (SystemFontCharacters.IsHexChar((int) reader.Peek(0)))
      {
        stringBuilder.Append((char) reader.Read());
      }
      else
      {
        int num2 = (int) reader.Read();
      }
    }
    if (!reader.EndOfFile)
    {
      int num3 = (int) reader.Read();
    }
    if (stringBuilder.Length % 2 == 1)
      stringBuilder.Append("0");
    return SystemFontPostScriptReaderHelper.GetBytesFromHexString(stringBuilder.ToString());
  }

  public static byte[] ReadLiteralString(SystemFontIPostScriptReader reader)
  {
    List<byte> byteList1 = new List<byte>();
    int num1 = (int) reader.Read();
    int num2 = 1;
    while (num2 > 0)
    {
      if (reader.Peek(0) == (byte) 92)
      {
        if (SystemFontPostScriptReaderHelper.IsValidEscape((int) reader.Peek(1)))
        {
          int num3 = (int) reader.Read();
          if (reader.Peek(0) != (byte) 13 && reader.Peek(0) != (byte) 10)
            byteList1.Add((byte) SystemFontPostScriptReaderHelper.GetSymbolFromEscapeSymbol((int) reader.Read()));
          else
            SystemFontPostScriptReaderHelper.SkipWhiteSpaces(reader);
        }
        else if (SystemFontCharacters.IsOctalChar((int) reader.Peek(1)))
        {
          int num4 = 3;
          int num5 = (int) reader.Read();
          List<byte> byteList2 = new List<byte>();
          for (; SystemFontCharacters.IsOctalChar((int) reader.Peek(0)) && num4 > 0; --num4)
            byteList2.Add((byte) ((uint) reader.Read() - 48U /*0x30*/));
          while (byteList2.Count < 3)
            byteList2.Insert(0, (byte) 0);
          int num6 = 0;
          int num7 = 1;
          for (int index = 2; index >= 0; --index)
          {
            num6 += (int) byteList2[index] * num7;
            num7 *= 8;
          }
          byteList1.Add((byte) num6);
        }
        else
        {
          int num8 = (int) reader.Read();
        }
      }
      else
      {
        if (reader.Peek(0) == (byte) 40)
          ++num2;
        else if (reader.Peek(0) == (byte) 41)
        {
          --num2;
          if (num2 == 0)
            break;
        }
        byteList1.Add(reader.Read());
      }
    }
    int num9 = (int) reader.Read();
    return byteList1.ToArray();
  }

  public static byte[] GetBytesFromHexString(string hexString)
  {
    if (string.IsNullOrEmpty(hexString))
      return new byte[0];
    if (hexString.Length % 2 != 0)
      hexString = "0" + hexString;
    byte[] bytesFromHexString = new byte[hexString.Length >> 1];
    int index = 0;
    for (int length = hexString.Length; index < length; index += 2)
      bytesFromHexString[index >> 1] = (byte) (SystemFontPostScriptReaderHelper.charValues[(int) hexString[index]] << 4 | SystemFontPostScriptReaderHelper.charValues[(int) hexString[index + 1]]);
    return bytesFromHexString;
  }

  public static bool IsValidEscape(int b)
  {
    switch (b)
    {
      case 10:
      case 13:
      case 40:
      case 41:
      case 92:
      case 98:
      case 102:
      case 110:
      case 114:
      case 116:
        return true;
      case 93:
      case 94:
      case 95:
      case 96 /*0x60*/:
      case 97:
      case 99:
      case 100:
      case 101:
        return false;
      case 115:
        return false;
      default:
        return false;
    }
  }

  public static char GetSymbolFromEscapeSymbol(int symbol)
  {
    switch (symbol)
    {
      case 40:
        return '(';
      case 41:
        return ')';
      case 92:
        return '\\';
      case 98:
        return '\b';
      case 102:
        return '\f';
      case 110:
        return '\n';
      case 114:
        return '\r';
      case 116:
        return '\t';
      default:
        return char.MinValue;
    }
  }

  private static bool TryReadHexChar(SystemFontIPostScriptReader reader, out string result)
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < 2; ++index)
    {
      char b = (char) reader.Read();
      if (reader.EndOfFile || SystemFontCharacters.IsDelimiter((int) b) || !SystemFontCharacters.IsHexChar((int) b))
      {
        result = stringBuilder.ToString();
        return false;
      }
      stringBuilder.Append(b);
    }
    char ch = (char) SystemFontPostScriptReaderHelper.GetBytesFromHexString(stringBuilder.ToString())[0];
    result = ch.ToString();
    return true;
  }
}
