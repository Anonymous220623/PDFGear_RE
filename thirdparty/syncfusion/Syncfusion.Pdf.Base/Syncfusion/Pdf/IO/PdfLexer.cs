// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.PdfLexer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class PdfLexer
{
  private const int YY_BUFFER_SIZE = 8192 /*0x2000*/;
  private const int YY_F = -1;
  private const int YY_NO_STATE = -1;
  private const int YY_NOT_ACCEPT = 0;
  private const int YY_START = 1;
  private const int YY_END = 2;
  private const int YY_NO_ANCHOR = 4;
  private const int YY_BOL = 256 /*0x0100*/;
  private const int YY_EOF = 257;
  private const string Prefix = "<<";
  private StringBuilder m_string = new StringBuilder();
  private int m_paren;
  private bool m_bSkip;
  internal bool isArray;
  internal string objectName;
  private TextReader m_yyReader;
  private int m_yyBufferIndex;
  private int m_yyBufferRead;
  private int m_yyBufferStart;
  private int m_yyBufferEnd;
  private char[] m_yyBuffer;
  private int m_yyLine;
  private bool TType = true;
  private bool m_yyAtBol;
  private PdfLexer.State m_yyLexicalState;
  private static readonly int[] m_yyStateDtrans = new int[3]
  {
    0,
    81,
    83
  };
  private bool m_yyLastWasCr;
  private string[] m_yyErrorString = new string[2]
  {
    "Error: Internal error.\n",
    "Error: Unmatched input.\n"
  };
  private int[] m_yyAccept = new int[88]
  {
    0,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    4,
    0,
    4,
    4,
    4,
    4,
    0,
    4,
    0,
    4,
    0,
    4,
    0,
    4,
    0,
    4,
    0,
    4,
    0,
    4,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0
  };
  private int[] m_yyCmap = PdfLexer.UnpackFromString(1, 258, "3,17:8,3,11,17,3,4,17:18,3,17:4,1,17:2,7,2,17,26,17,26,28,16,27:10,17:2,5,17,6,17:2,13:6,17:11,35,17:8,14,12,15,17:3,23,30,13,33,21,22,17:2,36,31,17,24,34,32,29,17:2,19,25,18,20,17:2,37,17:2,10,17,10,17:128,8,9,0:2")[0];
  private int[] m_yyRmap = PdfLexer.UnpackFromString(1, 88, "0,1,2,1:2,3,4,1:2,5,6,7,1:3,8,1:18,9,1,10,11,12,13,14,15,16,17,18,19,20,21,7,8:2,22,23,24,25,13,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57")[0];
  private int[][] m_yyNext = PdfLexer.UnpackFromString(58, 38, "1,2,3,4:2,5,37,6,3:3,4,3:2,7,8,9,3,42,3:2,44,10,3:2,46,48,11,50,52,3:2,38,3:2,12,3,54,-1:39,2:3,-1,2:6,-1,2:26,-1:5,13,-1:40,36,-1:37,9:2,-1:2,9:2,-1:3,9:21,-1:23,45,-1:41,11,49,-1:36,15,-1:11,35:3,84,35:33,-1:9,55,-1:34,14,-1:51,85,-1:18,63,17,63:8,64,63:26,-1,30:3,82,30:33,-1:20,56,-1:2,57,-1:33,41,-1:51,58,-1:36,43,-1:29,59,-1:31,47,-1:38,86,-1:3,60,-1:45,16,-1:36,51,-1:28,62,-1:35,53,-1:39,18,-1:52,65,-1:26,66,-1:3,67,-1:33,56,-1:31,87,-1:42,19,-1:35,20,-1:16,55:3,-1,55:6,-1,-1:26,-1,64,39,64,63,64:33,-1:24,69,-1:31,70,-1:49,71,-1:30,72,-1:35,74,-1:35,75,-1:49,21,-1:40,22,-1:40,76,-1:19,23,-1:39,77,-1:35,78,-1:41,79,-1:35,80,-1:50,24,-1:25,25,-1:15,1,26:2,27:2,26,28,26:4,27,40,29,26:7,29:3,26:3,29,26:2,29,26:2,29,26:4,-1:11,30,-1:26,1,31,32,31:4,33,31:4,34,31:25,-1:11,35,-1:50,61,-1:34,68,-1:34,73,-1:19");

  internal string Text => this.YyText();

  internal int Line => this.m_yyLine;

  internal StringBuilder StringText => this.m_string;

  internal long Position
  {
    get
    {
      return (this.m_yyReader as PdfReader).Position - (long) this.m_yyBufferRead + (long) this.m_yyBufferIndex;
    }
  }

  internal bool Skip
  {
    get => this.m_bSkip;
    set => this.m_bSkip = value;
  }

  internal void Reset()
  {
    this.m_yyBuffer = new char[8192 /*0x2000*/];
    this.m_yyBufferRead = 0;
    this.m_yyBufferIndex = 0;
    this.m_yyBufferStart = 0;
    this.m_yyBufferEnd = 0;
    this.m_yyLine = 0;
    this.m_yyAtBol = true;
    this.m_yyLexicalState = PdfLexer.State.YYINITIAL;
  }

  internal byte[] Read(int count)
  {
    List<byte> byteList = new List<byte>(count);
    this.YyMarkStart();
    if (this.m_yyBufferRead - this.m_yyBufferStart < count)
    {
      while (this.m_yyBuffer.Length - this.m_yyBufferStart < count)
        this.m_yyBuffer = this.YyDouble(this.m_yyBuffer);
    }
    int num = this.YyRead();
    if (this.m_yyBufferRead - this.m_yyBufferStart < num && num > count)
      count = num;
    int yyBufferStart = this.m_yyBufferStart;
    while (yyBufferStart < this.m_yyBufferStart + count)
    {
      byteList.Add((byte) this.m_yyBuffer[yyBufferStart]);
      ++yyBufferStart;
      this.m_yyBufferIndex = yyBufferStart;
    }
    this.YyMarkStart();
    this.YyMarkEnd();
    return byteList.ToArray();
  }

  internal void SkipNewLine()
  {
    this.m_yyBufferIndex = this.m_yyBufferStart + 1;
    if (this.m_yyBuffer[this.m_yyBufferIndex] == '\r')
    {
      if (this.m_yyBuffer[this.m_yyBufferIndex + 1] == '\n')
        this.m_yyBufferIndex += 2;
    }
    else if (this.m_yyBuffer[this.m_yyBufferIndex] == '\n' && this.m_yyBuffer[this.m_yyBufferIndex - 1] != '\n')
      ++this.m_yyBufferIndex;
    this.YyMarkStart();
  }

  internal void SkipToken() => this.m_yyBufferStart = this.m_yyBufferEnd;

  internal PdfLexer(TextReader reader)
    : this()
  {
    this.m_yyReader = reader != null ? reader : throw new PdfException("Error: Bad input stream initializer.");
  }

  internal PdfLexer(Stream inStream)
    : this()
  {
    this.m_yyReader = inStream != null ? (TextReader) new StreamReader(inStream) : throw new PdfException("Error: Bad input stream initializer.");
  }

  private PdfLexer()
  {
    this.m_yyBuffer = new char[8192 /*0x2000*/];
    this.m_yyBufferRead = 0;
    this.m_yyBufferIndex = 0;
    this.m_yyBufferStart = 0;
    this.m_yyBufferEnd = 0;
    this.m_yyLine = 0;
    this.m_yyAtBol = true;
    this.m_yyLexicalState = PdfLexer.State.YYINITIAL;
  }

  public TokenType GetNextToken()
  {
    int index1 = PdfLexer.m_yyStateDtrans[(int) this.m_yyLexicalState];
    int num1 = -1;
    int index2 = -1;
    bool flag = true;
    this.YyMarkStart();
    if (this.m_yyAccept[index1] != 0)
    {
      index2 = index1;
      this.YyMarkEnd();
    }
    while (true)
    {
      do
      {
        int index3 = !flag || !this.m_yyAtBol ? this.YyAdvance() : 256 /*0x0100*/;
        num1 = -1;
        int num2 = this.m_yyNext[this.m_yyRmap[index1]][this.m_yyCmap[index3]];
        if (257 == index3 && flag)
          return TokenType.Eof;
        if (-1 != num2)
        {
          index1 = num2;
          flag = false;
          if (this.m_yyAccept[index1] != 0)
          {
            index2 = index1;
            this.YyMarkEnd();
          }
        }
        else
        {
          if (-1 == index2)
            throw new PdfException("Lexical Error: Unmatched Input.");
          if ((2 & this.m_yyAccept[index2]) != 0)
            this.YyMoveEnd();
          this.YyToMark();
          switch (index2)
          {
            case -47:
            case -46:
            case -45:
            case -44:
            case -43:
            case -42:
            case -41:
            case -40:
            case -39:
            case -38:
            case -37:
            case -36:
            case -35:
            case -34:
            case -33:
            case -32:
            case -31:
            case -30:
            case -29:
            case -28:
            case -27:
            case -26:
            case -25:
            case -24:
            case -23:
            case -22:
            case -21:
            case -20:
            case -19:
            case -18:
            case -17:
            case -16:
            case -15:
            case -14:
            case -13:
            case -12:
            case -11:
            case -10:
            case -9:
            case -8:
            case -7:
            case -6:
            case -5:
            case -4:
            case -3:
            case -2:
            case 1:
            case 2:
            case 3:
            case 4:
            case 34:
            case 52:
              flag = true;
              index1 = PdfLexer.m_yyStateDtrans[(int) this.m_yyLexicalState];
              num1 = -1;
              index2 = -1;
              this.YyMarkStart();
              continue;
            case 5:
              this.YyBegin(PdfLexer.State.HexString);
              return TokenType.HexStringStart;
            case 6:
              this.YyBegin(PdfLexer.State.String);
              this.StringText.Length = 0;
              goto case -47;
            case 7:
              return TokenType.ArrayStart;
            case 8:
              return TokenType.ArrayEnd;
            case 9:
              return TokenType.Name;
            case 10:
              return TokenType.ObjectType;
            case 11:
              return TokenType.Number;
            case 12:
              return TokenType.Reference;
            case 13:
              return TokenType.DictionaryStart;
            case 14:
              return TokenType.DictionaryEnd;
            case 15:
              return TokenType.Real;
            case 16 /*0x10*/:
              return TokenType.ObjectStart;
            case 17:
              return TokenType.UnicodeString;
            case 18:
              return TokenType.Boolean;
            case 19:
              return TokenType.Null;
            case 20:
              return TokenType.XRef;
            case 21:
              return TokenType.ObjectEnd;
            case 22:
              return TokenType.StreamStart;
            case 23:
              return TokenType.Trailer;
            case 24:
              return TokenType.StreamEnd;
            case 25:
              return TokenType.StartXRef;
            case 26:
              return TokenType.HexStringWeird;
            case 27:
              return TokenType.WhiteSpace;
            case 28:
              this.YyBegin(PdfLexer.State.YYINITIAL);
              return TokenType.HexStringEnd;
            case 29:
              return TokenType.HexDigit;
            case 30:
              return TokenType.HexStringWeirdEscape;
            case 31 /*0x1F*/:
              this.StringText.Append(this.YyText());
              goto case -47;
            case 32 /*0x20*/:
              if (this.m_paren > 0)
              {
                this.StringText.Append(this.YyText());
                --this.m_paren;
                goto case -47;
              }
              this.YyBegin(PdfLexer.State.YYINITIAL);
              return TokenType.String;
            case 33:
              this.StringText.Append(this.YyText());
              ++this.m_paren;
              goto case -47;
            case 35:
              this.StringText.Append(this.YyText());
              goto case -47;
            case 37:
              this.YyError(PdfLexer.YYError.Match, true);
              goto case -47;
            case 38:
              return TokenType.ObjectType;
            case 39:
              return TokenType.UnicodeString;
            case 40:
              return TokenType.HexStringWeird;
            case 42:
              return TokenType.Unknown;
            case 44:
              return TokenType.Unknown;
            case 46:
              if (this.m_yyBuffer[this.m_yyBufferIndex - 1] != 's' || this.m_yyBuffer[this.m_yyBufferIndex] != 't' && this.m_yyBuffer[this.m_yyBufferIndex] != '%')
              {
                this.YyError(PdfLexer.YYError.Match, true);
                goto case -47;
              }
              goto case -47;
            case 48 /*0x30*/:
              this.YyError(PdfLexer.YYError.Match, true);
              goto case -47;
            case 50:
              if (this.isArray)
              {
                if (double.TryParse(new string(this.m_yyBuffer, this.m_yyBufferIndex - 2, 2), NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out double _) && this.m_yyBuffer[this.m_yyBufferIndex - 1] == '.' && (this.m_yyBuffer[this.m_yyBufferIndex] == ' ' || this.m_yyBuffer[this.m_yyBufferIndex] == ']'))
                  goto case -47;
              }
              else if (this.m_yyBuffer[this.m_yyBufferIndex - 1] == '.' && this.m_yyBuffer[this.m_yyBufferIndex] == '-')
                return TokenType.Unknown;
              this.YyError(PdfLexer.YYError.Match, true);
              goto case -47;
            case 54:
              this.YyError(PdfLexer.YYError.Match, true);
              goto case -47;
            default:
              this.YyError(PdfLexer.YYError.Internal, false);
              goto case -47;
          }
        }
      }
      while (this.m_yyAccept[index1] == 0);
      index2 = index1;
      this.YyMarkEnd();
    }
  }

  private void YyBegin(PdfLexer.State state) => this.m_yyLexicalState = state;

  private int YyAdvance()
  {
    if (this.m_yyBufferIndex < this.m_yyBufferRead)
      return (int) this.m_yyBuffer[this.m_yyBufferIndex++];
    if (this.m_yyBufferStart != 0)
    {
      int yyBufferStart = this.m_yyBufferStart;
      int index = 0;
      while (yyBufferStart < this.m_yyBufferRead)
      {
        this.m_yyBuffer[index] = this.m_yyBuffer[yyBufferStart];
        ++yyBufferStart;
        ++index;
      }
      this.m_yyBufferEnd -= this.m_yyBufferStart;
      this.m_yyBufferStart = 0;
      this.m_yyBufferRead = index;
      this.m_yyBufferIndex = index;
      if (this.YyRead() <= 0)
        return 257;
    }
    while (this.m_yyBufferIndex >= this.m_yyBufferRead)
    {
      if (this.m_yyBufferIndex >= this.m_yyBuffer.Length)
        this.m_yyBuffer = this.YyDouble(this.m_yyBuffer);
      if (this.YyRead() <= 0)
        return 257;
    }
    return (int) this.m_yyBuffer[this.m_yyBufferIndex++];
  }

  private int YyRead()
  {
    int num = this.m_yyReader.Read(this.m_yyBuffer, this.m_yyBufferRead, this.m_yyBuffer.Length - this.m_yyBufferRead);
    if (num > 0)
      this.m_yyBufferRead += num;
    return num;
  }

  private void YyMoveEnd()
  {
    if (this.m_yyBufferEnd > this.m_yyBufferStart && '\n' == this.m_yyBuffer[this.m_yyBufferEnd - 1])
      --this.m_yyBufferEnd;
    if (this.m_yyBufferEnd <= this.m_yyBufferStart || '\r' != this.m_yyBuffer[this.m_yyBufferEnd - 1])
      return;
    --this.m_yyBufferEnd;
  }

  private void YyMarkStart()
  {
    for (int yyBufferStart = this.m_yyBufferStart; yyBufferStart < this.m_yyBufferIndex; ++yyBufferStart)
    {
      if ('\n' == this.m_yyBuffer[yyBufferStart] && !this.m_yyLastWasCr)
        ++this.m_yyLine;
      if ('\r' == this.m_yyBuffer[yyBufferStart])
      {
        ++this.m_yyLine;
        this.m_yyLastWasCr = true;
      }
      else
        this.m_yyLastWasCr = false;
    }
    this.m_yyBufferStart = this.m_yyBufferIndex;
  }

  private void YyMarkEnd() => this.m_yyBufferEnd = this.m_yyBufferIndex;

  private void YyToMark()
  {
    this.m_yyBufferIndex = this.m_yyBufferEnd;
    this.m_yyAtBol = this.m_yyBufferEnd > this.m_yyBufferStart && ('\r' == this.m_yyBuffer[this.m_yyBufferEnd - 1] || '\n' == this.m_yyBuffer[this.m_yyBufferEnd - 1] || '߬' == this.m_yyBuffer[this.m_yyBufferEnd - 1] || '߭' == this.m_yyBuffer[this.m_yyBufferEnd - 1]);
  }

  private string YyText()
  {
    if (this.m_yyBuffer.Length > 2 && this.m_yyBufferEnd > 2)
    {
      char ch1 = this.m_yyBuffer[this.m_yyBufferEnd - 1];
      char ch2 = this.m_yyBuffer[this.m_yyBufferEnd - 2];
      int num1 = this.m_yyBufferEnd - this.m_yyBufferStart;
      if (ch1 == ')' && (ch2 == '\\' || ch2 == char.MinValue) && num1 > 3)
      {
        int yyBufferEnd = this.m_yyBufferEnd;
        string str = new string(this.m_yyBuffer);
        int num2 = str.IndexOf(ch1, this.m_yyBufferStart) + 1;
        int num3 = 0;
        while (str[num2 - 2] == '\\')
        {
          num2 = str.IndexOf(ch1, num2) + 1;
          if (num2 > 0)
          {
            num3 = num2;
          }
          else
          {
            num2 = num3;
            break;
          }
        }
        if (str[num2] == '>' && str[num2 + 1] == '>')
        {
          this.m_yyBufferIndex = num2;
          this.Skip = false;
        }
        else if (str.Length > num2 + 2)
        {
          if (str[num2 + 2] == '/')
          {
            this.m_yyBufferIndex = num2;
            this.Skip = false;
          }
          else if (str[num2 + 1] == '/')
          {
            this.m_yyBufferIndex = num2;
            this.Skip = false;
          }
          else if (str[num2] == '/')
          {
            this.m_yyBufferIndex = num2;
            this.Skip = false;
          }
          else if (str[num2 - 1] == ')')
          {
            this.m_yyBufferIndex = num2;
            this.Skip = false;
          }
          else
            this.Skip = true;
        }
        else
          this.Skip = true;
        this.m_yyBufferEnd = str.IndexOf(')', this.m_yyBufferEnd + 1) < 0 || str[num2 - 1] != ')' || this.m_yyBufferEnd >= num2 + 1 ? num2 : this.m_yyBufferIndex;
      }
      else if (ch1 == ')' && num1 > 3)
      {
        int yyBufferEnd = this.m_yyBufferEnd;
        string str = new string(this.m_yyBuffer);
        int startIndex = str.IndexOf(ch1, this.m_yyBufferStart) + 1;
        while (str[startIndex - 2] == '\\')
          startIndex = str.IndexOf(ch1, startIndex) + 1;
        if (this.m_yyBufferEnd > startIndex + 1)
          this.m_yyBufferEnd = startIndex;
        if (str[startIndex - 1] == ')')
        {
          this.m_yyBufferIndex = startIndex - 1;
          this.Skip = false;
        }
        else
          this.Skip = true;
      }
    }
    return new string(this.m_yyBuffer, this.m_yyBufferStart, this.m_yyBufferEnd - this.m_yyBufferStart);
  }

  private int YyLength() => this.m_yyBufferEnd - this.m_yyBufferStart;

  private char[] YyDouble(char[] buffer)
  {
    int length = buffer.Length;
    char[] dst = new char[2 * length];
    int num = 2;
    Buffer.BlockCopy((Array) buffer, 0, (Array) dst, 0, length * num);
    return dst;
  }

  private void YyError(PdfLexer.YYError code, bool fatal)
  {
    if (!fatal)
      return;
    long position = this.Position;
    if (this.objectName != null)
      throw new PdfException($"Fatal Error occurred at {$"{(object) position} when reading object type of {this.objectName}"}.\n");
    throw new PdfException($"Fatal Error at {position}.\n");
  }

  private static int[][] UnpackFromString(int size1, int size2, string st)
  {
    int num1 = 0;
    int num2 = 0;
    int[][] numArray = new int[size1][];
    for (int index = 0; index < size1; ++index)
      numArray[index] = new int[size2];
    for (int index1 = 0; index1 < size1; ++index1)
    {
      for (int index2 = 0; index2 < size2; ++index2)
      {
        if (num1 != 0)
        {
          numArray[index1][index2] = num2;
          --num1;
        }
        else
        {
          int length1 = st.IndexOf(',');
          string s = length1 == -1 ? st : st.Substring(0, length1);
          st = st.Substring(length1 + 1);
          int length2 = s.IndexOf(':');
          if (length2 == -1)
          {
            numArray[index1][index2] = int.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture);
          }
          else
          {
            int num3 = int.Parse(s.Substring(length2 + 1), (IFormatProvider) CultureInfo.InvariantCulture);
            num2 = int.Parse(s.Substring(0, length2), (IFormatProvider) CultureInfo.InvariantCulture);
            numArray[index1][index2] = num2;
            num1 = num3 - 1;
          }
        }
      }
    }
    return numArray;
  }

  private enum YYError
  {
    Internal,
    Match,
  }

  private enum State
  {
    YYINITIAL,
    HexString,
    String,
  }
}
