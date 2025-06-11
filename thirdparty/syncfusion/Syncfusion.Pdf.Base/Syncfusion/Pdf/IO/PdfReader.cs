// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.PdfReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class PdfReader : TextReader
{
  private Stream m_stream;
  private string m_delimiters = "()<>[]{}/%";
  private string m_jsonDelimiters = "()<>[]{}/%,\":";
  private int m_peekedByte;
  private bool m_bBytePeeked;

  public long Position
  {
    get => this.m_stream.Position;
    set => this.m_stream.Position = value;
  }

  public Stream Stream => this.m_stream;

  public PdfReader(Stream stream)
  {
    this.m_stream = stream != null ? stream : throw new ArgumentNullException(nameof (stream));
  }

  protected override void Dispose(bool disposing)
  {
    this.m_stream = (Stream) null;
    base.Dispose(disposing);
  }

  public override void Close() => this.Dispose(true);

  public override string ReadLine()
  {
    string str = string.Empty;
    int character;
    for (character = this.m_stream.ReadByte(); character != -1 && !this.IsEol((char) character); character = this.m_stream.ReadByte())
      str = str.Insert(str.Length, ((char) character).ToString());
    if (character == 13 && this.m_stream.ReadByte() != 10)
      --this.m_stream.Position;
    return str;
  }

  internal void ReadLine(byte[] data, bool skipWhiteSpace)
  {
    int num1 = -1;
    bool flag1 = false;
    int num2 = 0;
    int length = data.Length;
    if (num2 < length)
    {
      while (this.IsWhitespaceCharcater(num1 = this.Read(), skipWhiteSpace))
        ;
    }
    while (!flag1 && num2 < length)
    {
      switch (num1)
      {
        case -1:
        case 10:
          flag1 = true;
          break;
        case 13:
          flag1 = true;
          long position = this.m_stream.Position;
          if (this.Read() != 10)
          {
            this.m_stream.Seek(position, SeekOrigin.Begin);
            break;
          }
          break;
        default:
          data[num2++] = (byte) num1;
          break;
      }
      if (!flag1 && length > num2)
        num1 = this.Read();
      else
        break;
    }
    if (num2 >= length)
    {
      bool flag2 = false;
      while (!flag2)
      {
        switch (num1 = this.Read())
        {
          case -1:
          case 10:
            flag2 = true;
            continue;
          case 13:
            flag2 = true;
            long position = this.m_stream.Position;
            if (this.Read() != 10)
            {
              this.m_stream.Seek(position, SeekOrigin.Begin);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    if (num1 == -1 && num2 == 0 || num2 + 2 > length)
      return;
    byte[] numArray = data;
    int index1 = num2;
    int index2 = index1 + 1;
    numArray[index1] = (byte) 32 /*0x20*/;
    data[index2] = (byte) 88;
  }

  private bool IsWhitespaceCharcater(int character, bool isWhitespace)
  {
    return isWhitespace && character == 0 || character == 9 || character == 10 || character == 12 || character == 13 || character == 32 /*0x20*/;
  }

  public override int Read()
  {
    int byteValue;
    if (this.m_bBytePeeked)
      this.GetPeeked(out byteValue);
    else
      byteValue = this.m_stream.ReadByte();
    return byteValue;
  }

  public override int Peek()
  {
    int byteValue;
    if (this.m_bBytePeeked)
    {
      this.GetPeeked(out byteValue);
    }
    else
    {
      this.m_peekedByte = this.Read();
      byteValue = this.m_peekedByte;
    }
    if (this.m_peekedByte != -1)
      this.m_bBytePeeked = true;
    return byteValue;
  }

  public override int Read(char[] buffer, int index, int count)
  {
    if (count < 0)
      throw new ArgumentException("The value can't be less then zero", nameof (count));
    int index1 = index;
    if (this.m_bBytePeeked && count > 0)
    {
      buffer[index1] = (char) this.m_peekedByte;
      this.m_bBytePeeked = false;
      --count;
      ++index1;
    }
    if (count > 0)
    {
      byte[] buffer1 = new byte[count];
      count = this.m_stream.Read(buffer1, 0, count);
      for (int index2 = 0; index2 < count; ++index2)
      {
        char ch = (char) buffer1[index2];
        buffer[index1 + index2] = ch;
      }
      index1 += count;
    }
    return index1 - index;
  }

  public override int ReadBlock(char[] buffer, int index, int count)
  {
    return this.Read(buffer, index, count);
  }

  public override string ReadToEnd()
  {
    string end = string.Empty;
    for (int index = this.Read(); index != -1; index = this.m_stream.ReadByte())
      end = end.Insert(end.Length, ((char) index).ToString());
    return end;
  }

  internal string ReadStream() => new StreamReader(this.m_stream).ReadToEnd();

  public bool IsEol(char character) => character == '\n' || character == '\r';

  public bool IsSeparator(char character)
  {
    return char.IsWhiteSpace(character) || this.IsDelimiter(character);
  }

  internal bool IsJsonSeparator(char character)
  {
    return char.IsWhiteSpace(character) || this.IsJsonDelimiter(character);
  }

  public bool IsDelimiter(char character)
  {
    foreach (int delimiter in this.m_delimiters)
    {
      if (delimiter == (int) character)
        return true;
    }
    return false;
  }

  internal bool IsJsonDelimiter(char character)
  {
    foreach (int jsonDelimiter in this.m_jsonDelimiters)
    {
      if (jsonDelimiter == (int) character)
        return true;
    }
    return false;
  }

  public long SearchBack(string token)
  {
    long position = this.Position;
    this.SkipWSBack();
    if (this.Position < (long) token.Length)
      return -1;
    string str1 = this.ReadBack(token.Length);
    long num1 = this.Position - (long) token.Length;
    while (str1.CompareTo(token) != 0)
    {
      if (num1 < 0L)
        throw new PdfDocumentException($"Invalid/Unknown/Unsupported format\nUnable to find token '{token}'");
      --this.Position;
      if (this.Position < (long) token.Length)
        return -1;
      str1 = this.ReadBack(token.Length);
      num1 = this.Position - (long) token.Length;
    }
    while (token == "xref")
    {
      long num2 = num1;
      if (this.SearchBack("startxref") == num2 - 5L)
      {
        string str2 = "startxref";
        while (str2.CompareTo(token) != 0)
        {
          if (num1 < 0L)
            throw new PdfDocumentException($"Invalid/Unknown/Unsupported format\nUnable to find token '{token}'");
          --this.Position;
          if (this.Position < (long) token.Length)
            return -1;
          str2 = this.ReadBack(token.Length);
          num1 = this.Position - (long) token.Length;
        }
      }
      else
      {
        num1 = num2;
        break;
      }
    }
    this.Position = num1;
    return num1;
  }

  public long SearchForward(string token)
  {
    Encoding utF8 = Encoding.UTF8;
    byte[] numArray1 = new byte[token.Length];
    string str = "startxref";
    bool flag = false;
    int num1;
    do
    {
      long position = this.Position;
      num1 = this.Read();
      numArray1[0] = (byte) num1;
      if ((int) numArray1[0] == (int) token[0])
      {
        if (!flag)
        {
          long num2 = this.Position - 1L;
          int num3 = this.m_stream.Read(numArray1, 1, token.Length - 1);
          this.Position = num2;
          if (num3 < token.Length - 1)
            return -1;
          if (token.CompareTo(utF8.GetString(numArray1, 0, numArray1.Length)) == 0)
            return num2;
          ++this.Position;
        }
      }
      else if ((int) numArray1[0] == (int) str[0])
      {
        flag = false;
        long num4 = this.Position - 1L;
        this.m_stream.Position = num4;
        long num5 = num4;
        byte[] numArray2 = new byte[str.Length];
        this.m_stream.Read(numArray2, 0, str.Length);
        if (str.CompareTo(utF8.GetString(numArray2, 0, numArray2.Length)) == 0)
        {
          flag = true;
          long num6;
          this.Position = num6 = num5 + 1L;
        }
      }
    }
    while (num1 != -1);
    return -1;
  }

  public string ReadBack(int length)
  {
    Encoding utF8 = Encoding.UTF8;
    byte[] numArray = new byte[length];
    if (this.Position < (long) length)
      throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
    this.Position -= (long) length;
    if (this.m_stream.Read(numArray, 0, length) < length)
      throw new PdfDocumentException("Read failure.");
    return utF8.GetString(numArray, 0, numArray.Length);
  }

  public void SkipWSBack()
  {
    if (this.Position == 0L)
      throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
    --this.Position;
    while (char.IsWhiteSpace((char) this.Read()))
      this.Position -= 2L;
  }

  public void SkipWS()
  {
    if (this.Position == this.m_stream.Length)
      return;
    int c;
    do
    {
      c = this.Read();
    }
    while (char.IsWhiteSpace((char) c));
    if (c == -1)
      this.Position = this.m_stream.Length;
    else
      --this.Position;
  }

  public string GetNextToken()
  {
    string empty = string.Empty;
    this.SkipWS();
    int character = this.Peek();
    int num;
    if (this.IsDelimiter((char) character))
    {
      num = this.AppendChar(ref empty);
      return empty;
    }
    for (; character != -1 && !this.IsSeparator((char) character) && empty != "\0"; character = this.Peek())
      num = this.AppendChar(ref empty);
    return empty;
  }

  internal string GetNextJsonToken()
  {
    string empty1 = string.Empty;
    List<byte> byteList = new List<byte>();
    this.SkipWS();
    int character = this.Peek();
    if (this.IsJsonDelimiter((char) character))
    {
      this.AppendChar(ref empty1);
      return empty1;
    }
    for (; character != -1 && !this.IsJsonDelimiter((char) character) && empty1 != "\0"; character = this.Peek())
    {
      byteList.Add((byte) character);
      this.Read();
    }
    string empty2 = string.Empty;
    if (byteList.Count > 0)
      empty2 = Encoding.UTF8.GetString(byteList.ToArray(), 0, byteList.Count);
    byteList.Clear();
    return empty2;
  }

  public long Seek(long offset, SeekOrigin origin) => this.m_stream.Seek(offset, origin);

  private int AppendChar(ref string line)
  {
    int num = this.Read();
    if (num != -1)
      line = line.Insert(line.Length, ((char) num).ToString());
    return num;
  }

  private bool GetPeeked(out int byteValue)
  {
    bool bBytePeeked = this.m_bBytePeeked;
    if (this.m_bBytePeeked)
    {
      this.m_bBytePeeked = false;
      byteValue = this.m_peekedByte;
    }
    else
      byteValue = 0;
    return bBytePeeked;
  }
}
