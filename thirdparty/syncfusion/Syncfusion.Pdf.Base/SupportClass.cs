// Decompiled with JetBrains decompiler
// Type: SupportClass
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.IO;
using System.Text;

#nullable disable
internal class SupportClass
{
  public static byte[] ToByteArray(sbyte[] sbyteArray)
  {
    byte[] byteArray = (byte[]) null;
    if (sbyteArray != null)
    {
      byteArray = new byte[sbyteArray.Length];
      for (int index = 0; index < sbyteArray.Length; ++index)
        byteArray[index] = (byte) sbyteArray[index];
    }
    return byteArray;
  }

  public static byte[] ToByteArray(string sourceString) => Encoding.UTF8.GetBytes(sourceString);

  public static byte[] ToByteArray(object[] tempObjectArray)
  {
    byte[] byteArray = (byte[]) null;
    if (tempObjectArray != null)
    {
      byteArray = new byte[tempObjectArray.Length];
      for (int index = 0; index < tempObjectArray.Length; ++index)
        byteArray[index] = (byte) tempObjectArray[index];
    }
    return byteArray;
  }

  public static void WriteStackTrace(Exception throwable, TextWriter stream)
  {
    stream.Write(throwable.StackTrace);
    stream.Flush();
  }

  public static sbyte[] ToSByteArray(byte[] byteArray)
  {
    sbyte[] sbyteArray = (sbyte[]) null;
    if (byteArray != null)
    {
      sbyteArray = new sbyte[byteArray.Length];
      for (int index = 0; index < byteArray.Length; ++index)
        sbyteArray[index] = (sbyte) byteArray[index];
    }
    return sbyteArray;
  }

  public static char[] ToCharArray(sbyte[] sByteArray)
  {
    return Encoding.UTF8.GetChars(SupportClass.ToByteArray(sByteArray));
  }

  public static char[] ToCharArray(byte[] byteArray) => Encoding.UTF8.GetChars(byteArray);

  public static long Identity(long literal) => literal;

  public static ulong Identity(ulong literal) => literal;

  public static float Identity(float literal) => literal;

  public static double Identity(double literal) => literal;

  public static int URShift(int number, int bits)
  {
    return number >= 0 ? number >> bits : (number >> bits) + (2 << ~bits);
  }

  public static int URShift(int number, long bits) => SupportClass.URShift(number, (int) bits);

  public static long URShift(long number, int bits)
  {
    return number >= 0L ? number >> bits : (number >> bits) + (2L << ~bits);
  }

  public static long URShift(long number, long bits) => SupportClass.URShift(number, (int) bits);

  public static int ReadInput(Stream sourceStream, sbyte[] target, int start, int count)
  {
    if (target.Length == 0)
      return 0;
    byte[] buffer = new byte[target.Length];
    int num = sourceStream.Read(buffer, start, count);
    if (num == 0)
      return -1;
    for (int index = start; index < start + num; ++index)
      target[index] = (sbyte) buffer[index];
    return num;
  }

  public static int ReadInput(TextReader sourceTextReader, sbyte[] target, int start, int count)
  {
    if (target.Length == 0)
      return 0;
    char[] buffer = new char[target.Length];
    int num = sourceTextReader.Read(buffer, start, count);
    if (num == 0)
      return -1;
    for (int index = start; index < start + num; ++index)
      target[index] = (sbyte) buffer[index];
    return num;
  }

  internal class Tokenizer : IEnumerator
  {
    private long currentPos;
    private bool includeDelims;
    private char[] chars;
    private string delimiters = " \t\n\r\f";

    public Tokenizer(string source) => this.chars = source.ToCharArray();

    public string NextToken() => this.NextToken(this.delimiters);

    public string NextToken(string delimiters)
    {
      this.delimiters = delimiters;
      if (this.currentPos == (long) this.chars.Length)
        throw new ArgumentOutOfRangeException();
      return Array.IndexOf<char>(delimiters.ToCharArray(), this.chars[this.currentPos]) != -1 && this.includeDelims ? string.Concat((object) this.chars[(IntPtr) this.currentPos++]) : this.nextToken(delimiters.ToCharArray());
    }

    private string nextToken(char[] delimiters)
    {
      string str = "";
      long currentPos = this.currentPos;
      while (Array.IndexOf<char>(delimiters, this.chars[this.currentPos]) != -1)
      {
        if (++this.currentPos == (long) this.chars.Length)
        {
          this.currentPos = currentPos;
          throw new ArgumentOutOfRangeException();
        }
      }
      while (Array.IndexOf<char>(delimiters, this.chars[this.currentPos]) == -1)
      {
        str += (string) (object) this.chars[this.currentPos];
        if (++this.currentPos == (long) this.chars.Length)
          break;
      }
      return str;
    }

    public bool HasMoreTokens()
    {
      long currentPos = this.currentPos;
      try
      {
        this.NextToken();
      }
      catch (ArgumentOutOfRangeException ex)
      {
        return false;
      }
      finally
      {
        this.currentPos = currentPos;
      }
      return true;
    }

    public int Count
    {
      get
      {
        long currentPos = this.currentPos;
        int count = 0;
        try
        {
          while (true)
          {
            this.NextToken();
            ++count;
          }
        }
        catch (ArgumentOutOfRangeException ex)
        {
          this.currentPos = currentPos;
          return count;
        }
      }
    }

    public object Current => (object) this.NextToken();

    public bool MoveNext() => this.HasMoreTokens();

    public void Reset()
    {
    }
  }

  private class BackStringReader : StringReader
  {
    private char[] buffer;
    private int position = 1;

    public BackStringReader(string s)
      : base(s)
    {
      this.buffer = new char[this.position];
    }

    public override int Read()
    {
      return this.position >= 0 && this.position < this.buffer.Length ? (int) this.buffer[this.position++] : base.Read();
    }

    public override int Read(char[] array, int index, int count)
    {
      int length = this.buffer.Length - this.position;
      if (count <= 0)
        return 0;
      if (length > 0)
      {
        if (count < length)
          length = count;
        Array.Copy((Array) this.buffer, this.position, (Array) array, index, length);
        count -= length;
        index += length;
        this.position += length;
      }
      if (count <= 0)
        return length;
      count = base.Read(array, index, count);
      if (count != -1)
        return length + count;
      return length == 0 ? -1 : length;
    }

    public void UnRead(int unReadChar)
    {
      --this.position;
      this.buffer[this.position] = (char) unReadChar;
    }

    public void UnRead(char[] array, int index, int count) => this.Move(array, index, count);

    public void UnRead(char[] array) => this.Move(array, 0, array.Length - 1);

    private void Move(char[] array, int index, int count)
    {
      for (int index1 = index + count; index1 >= index; --index1)
        this.UnRead((int) array[index1]);
    }
  }

  internal class StreamTokenizerSupport
  {
    private const string TOKEN = "Token[";
    private const string NOTHING = "NOTHING";
    private const string NUMBER = "number=";
    private const string EOF = "EOF";
    private const string EOL = "EOL";
    private const string QUOTED = "quoted string=";
    private const string LINE = "], Line ";
    private const string DASH = "-.";
    private const string DOT = ".";
    private const int TT_NOTHING = -4;
    private const sbyte ORDINARYCHAR = 0;
    private const sbyte WORDCHAR = 1;
    private const sbyte WHITESPACECHAR = 2;
    private const sbyte COMMENTCHAR = 4;
    private const sbyte QUOTECHAR = 8;
    private const sbyte NUMBERCHAR = 16 /*0x10*/;
    private const int STATE_NEUTRAL = 0;
    private const int STATE_WORD = 1;
    private const int STATE_NUMBER1 = 2;
    private const int STATE_NUMBER2 = 3;
    private const int STATE_NUMBER3 = 4;
    private const int STATE_NUMBER4 = 5;
    private const int STATE_STRING = 6;
    private const int STATE_LINECOMMENT = 7;
    private const int STATE_DONE_ON_EOL = 8;
    private const int STATE_PROCEED_ON_EOL = 9;
    private const int STATE_POSSIBLEC_COMMENT = 10;
    private const int STATE_POSSIBLEC_COMMENT_END = 11;
    private const int STATE_C_COMMENT = 12;
    private const int STATE_STRING_ESCAPE_SEQ = 13;
    private const int STATE_STRING_ESCAPE_SEQ_OCTAL = 14;
    private const int STATE_DONE = 100;
    public const int TT_EOF = -1;
    public const int TT_EOL = 10;
    public const int TT_NUMBER = -2;
    public const int TT_WORD = -3;
    private sbyte[] attribute = new sbyte[256 /*0x0100*/];
    private bool eolIsSignificant;
    private bool slashStarComments;
    private bool slashSlashComments;
    private bool lowerCaseMode;
    private bool pushedback;
    private int lineno = 1;
    private SupportClass.BackReader inReader;
    private SupportClass.BackStringReader inStringReader;
    private SupportClass.BackInputStream inStream;
    private StringBuilder buf;
    public double nval;
    public string sval;
    public int ttype;

    private int read()
    {
      if (this.inReader != null)
        return this.inReader.Read();
      return this.inStream != null ? this.inStream.Read() : this.inStringReader.Read();
    }

    private void unread(int ch)
    {
      if (this.inReader != null)
        this.inReader.UnRead(ch);
      else if (this.inStream != null)
        this.inStream.UnRead(ch);
      else
        this.inStringReader.UnRead(ch);
    }

    private void init()
    {
      this.buf = new StringBuilder();
      this.ttype = -4;
      this.WordChars(65, 90);
      this.WordChars(97, 122);
      this.WordChars(160 /*0xA0*/, (int) byte.MaxValue);
      this.WhitespaceChars(0, 32 /*0x20*/);
      this.CommentChar(47);
      this.QuoteChar(39);
      this.QuoteChar(34);
      this.ParseNumbers();
    }

    private void setAttributes(int low, int hi, sbyte attrib)
    {
      int num1 = Math.Max(0, low);
      int num2 = Math.Min((int) byte.MaxValue, hi);
      for (int index = num1; index <= num2; ++index)
        this.attribute[index] = attrib;
    }

    private bool isWordChar(int data)
    {
      char index = (char) data;
      if (data == -1)
        return false;
      return index > 'ÿ' || this.attribute[(int) index] == (sbyte) 1 || this.attribute[(int) index] == (sbyte) 16 /*0x10*/;
    }

    public StreamTokenizerSupport(StringReader reader)
    {
      string s = "";
      for (int index = reader.Read(); index != -1; index = reader.Read())
        s += (string) (object) (char) index;
      this.inStringReader = new SupportClass.BackStringReader(s);
      this.init();
    }

    public StreamTokenizerSupport(StreamReader reader)
    {
      this.inReader = new SupportClass.BackReader(new StreamReader(reader.BaseStream, reader.CurrentEncoding).BaseStream, 2, reader.CurrentEncoding);
      this.init();
    }

    public StreamTokenizerSupport(Stream stream)
    {
      this.inStream = new SupportClass.BackInputStream(stream, 2);
      this.init();
    }

    public virtual void CommentChar(int ch)
    {
      if (ch < 0 || ch > (int) byte.MaxValue)
        return;
      this.attribute[ch] = (sbyte) 4;
    }

    public virtual void EOLIsSignificant(bool flag) => this.eolIsSignificant = flag;

    public virtual int Lineno() => this.lineno;

    public virtual void LowerCaseMode(bool flag) => this.lowerCaseMode = flag;

    public virtual int NextToken()
    {
      char index1 = char.MinValue;
      char ch = char.MinValue;
      int num1 = 0;
      if (this.pushedback)
      {
        this.pushedback = false;
        return this.ttype;
      }
      this.ttype = -4;
      int num2 = 0;
      this.nval = 0.0;
      this.sval = (string) null;
      this.buf.Length = 0;
      do
      {
        int data = this.read();
        char index2 = index1;
        index1 = (char) data;
        switch (num2)
        {
          case 0:
            if (data == -1)
            {
              this.ttype = -1;
              num2 = 100;
              break;
            }
            if (index1 > 'ÿ')
            {
              this.buf.Append(index1);
              this.ttype = -3;
              num2 = 1;
              break;
            }
            if (this.attribute[(int) index1] == (sbyte) 4)
            {
              num2 = 7;
              break;
            }
            if (this.attribute[(int) index1] == (sbyte) 1)
            {
              this.buf.Append(index1);
              this.ttype = -3;
              num2 = 1;
              break;
            }
            if (this.attribute[(int) index1] == (sbyte) 16 /*0x10*/)
            {
              this.ttype = -2;
              this.buf.Append(index1);
              switch (index1)
              {
                case '-':
                  num2 = 2;
                  break;
                case '.':
                  num2 = 4;
                  break;
                default:
                  num2 = 3;
                  break;
              }
            }
            else
            {
              if (this.attribute[(int) index1] == (sbyte) 8)
              {
                ch = index1;
                this.ttype = (int) index1;
                num2 = 6;
                break;
              }
              if ((this.slashSlashComments || this.slashStarComments) && index1 == '/')
              {
                num2 = 10;
                break;
              }
              if (this.attribute[(int) index1] == (sbyte) 0)
              {
                this.ttype = (int) index1;
                num2 = 100;
                break;
              }
              if (index1 == '\n' || index1 == '\r')
              {
                ++this.lineno;
                if (this.eolIsSignificant)
                {
                  this.ttype = 10;
                  switch (index1)
                  {
                    case '\n':
                      num2 = 100;
                      break;
                    case '\r':
                      num2 = 8;
                      break;
                  }
                }
                else
                {
                  if (index1 == '\r')
                  {
                    num2 = 9;
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
            break;
          case 1:
            if (this.isWordChar(data))
            {
              this.buf.Append(index1);
              break;
            }
            if (data != -1)
              this.unread((int) index1);
            this.sval = this.buf.ToString();
            num2 = 100;
            break;
          case 2:
            if (data == -1 || this.attribute[(int) index1] != (sbyte) 16 /*0x10*/ || index1 == '-')
            {
              if (this.attribute[(int) index1] == (sbyte) 4 && char.IsNumber(index1))
              {
                this.buf.Append(index1);
                num2 = 3;
                break;
              }
              if (data != -1)
                this.unread((int) index1);
              this.ttype = 45;
              num2 = 100;
              break;
            }
            this.buf.Append(index1);
            num2 = index1 != '.' ? 3 : 4;
            break;
          case 3:
            if (data == -1 || this.attribute[(int) index1] != (sbyte) 16 /*0x10*/ || index1 == '-')
            {
              if (char.IsNumber(index1) && this.attribute[(int) index1] == (sbyte) 1)
              {
                this.buf.Append(index1);
                break;
              }
              if (index1 == '.' && this.attribute[(int) index1] == (sbyte) 2)
              {
                this.buf.Append(index1);
                break;
              }
              if (data != -1 && this.attribute[(int) index1] == (sbyte) 4 && char.IsNumber(index1))
              {
                this.buf.Append(index1);
                break;
              }
              if (data != -1)
                this.unread((int) index1);
              try
              {
                this.nval = double.Parse(this.buf.ToString());
              }
              catch (FormatException ex)
              {
              }
              num2 = 100;
              break;
            }
            this.buf.Append(index1);
            if (index1 == '.')
            {
              num2 = 4;
              break;
            }
            break;
          case 4:
            if (data == -1 || this.attribute[(int) index1] != (sbyte) 16 /*0x10*/ || index1 == '-' || index1 == '.')
            {
              if (this.attribute[(int) index1] == (sbyte) 4 && char.IsNumber(index1))
              {
                this.buf.Append(index1);
                break;
              }
              if (data != -1)
                this.unread((int) index1);
              string s = this.buf.ToString();
              switch (s)
              {
                case "-.":
                  this.unread(46);
                  this.ttype = 45;
                  break;
                case ".":
                  if ((sbyte) 1 == this.attribute[(int) index2])
                  {
                    this.ttype = 46;
                    break;
                  }
                  goto default;
                default:
                  try
                  {
                    this.nval = double.Parse(s);
                    break;
                  }
                  catch (FormatException ex)
                  {
                    break;
                  }
              }
              num2 = 100;
              break;
            }
            this.buf.Append(index1);
            num2 = 5;
            break;
          case 5:
            if (data == -1 || this.attribute[(int) index1] != (sbyte) 16 /*0x10*/ || index1 == '-' || index1 == '.')
            {
              if (data != -1)
                this.unread((int) index1);
              try
              {
                this.nval = double.Parse(this.buf.ToString());
              }
              catch (FormatException ex)
              {
              }
              num2 = 100;
              break;
            }
            this.buf.Append(index1);
            break;
          case 6:
            if (data != -1 && (int) index1 != (int) ch)
            {
              switch (index1)
              {
                case '\n':
                case '\r':
                  break;
                case '\\':
                  num2 = 13;
                  goto label_129;
                default:
                  this.buf.Append(index1);
                  goto label_129;
              }
            }
            this.sval = this.buf.ToString();
            if (index1 == '\r' || index1 == '\n')
              this.unread((int) index1);
            num2 = 100;
            break;
          case 7:
            if (data == -1)
            {
              this.ttype = -1;
              num2 = 100;
              break;
            }
            if (index1 == '\n' || index1 == '\r')
            {
              this.unread((int) index1);
              num2 = 0;
              break;
            }
            break;
          case 8:
            if (index1 != '\n' && data != -1)
              this.unread((int) index1);
            num2 = 100;
            break;
          case 9:
            if (index1 != '\n' && data != -1)
              this.unread((int) index1);
            num2 = 0;
            break;
          case 10:
            switch (index1)
            {
              case '*':
                num2 = 12;
                break;
              case '/':
                num2 = 7;
                break;
              default:
                if (data != -1)
                  this.unread((int) index1);
                this.ttype = 47;
                num2 = 100;
                break;
            }
            break;
          case 11:
            if (data == -1)
            {
              this.ttype = -1;
              num2 = 100;
              break;
            }
            switch (index1)
            {
              case '*':
                break;
              case '/':
                num2 = 0;
                break;
              default:
                num2 = 12;
                break;
            }
            break;
          case 12:
            if (index1 == '*')
              num2 = 11;
            if (index1 == '\n')
            {
              ++this.lineno;
              break;
            }
            if (data == -1)
            {
              this.ttype = -1;
              num2 = 100;
              break;
            }
            break;
          case 13:
            if (data == -1)
            {
              this.sval = this.buf.ToString();
              num2 = 100;
              break;
            }
            num2 = 6;
            switch (index1)
            {
              case '0':
              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
                num1 = (int) index1 - 48 /*0x30*/;
                num2 = 14;
                break;
              case 'a':
                this.buf.Append(7);
                break;
              case 'b':
                this.buf.Append('\b');
                break;
              case 'f':
                this.buf.Append(12);
                break;
              case 'n':
                this.buf.Append('\n');
                break;
              case 'r':
                this.buf.Append('\r');
                break;
              case 't':
                this.buf.Append('\t');
                break;
              case 'v':
                this.buf.Append(11);
                break;
              default:
                this.buf.Append(index1);
                break;
            }
            break;
          case 14:
            if (data == -1 || index1 < '0' || index1 > '7')
            {
              this.buf.Append((char) num1);
              if (data == -1)
              {
                this.sval = this.buf.ToString();
                num2 = 100;
                break;
              }
              this.unread((int) index1);
              num2 = 6;
              break;
            }
            int num3 = num1 * 8 + ((int) index1 - 48 /*0x30*/);
            if (num3 < 256 /*0x0100*/)
            {
              num1 = num3;
              break;
            }
            this.buf.Append((char) num1);
            this.buf.Append(index1);
            num2 = 6;
            break;
        }
label_129:;
      }
      while (num2 != 100);
      if (this.ttype == -3 && this.lowerCaseMode)
        this.sval = this.sval.ToLower();
      return this.ttype;
    }

    public virtual void OrdinaryChar(int ch)
    {
      if (ch < 0 || ch > (int) byte.MaxValue)
        return;
      this.attribute[ch] = (sbyte) 0;
    }

    public virtual void OrdinaryChars(int low, int hi) => this.setAttributes(low, hi, (sbyte) 0);

    public virtual void ParseNumbers()
    {
      for (int index = 48 /*0x30*/; index <= 57; ++index)
        this.attribute[index] = (sbyte) 16 /*0x10*/;
      this.attribute[46] = (sbyte) 16 /*0x10*/;
      this.attribute[45] = (sbyte) 16 /*0x10*/;
    }

    public virtual void PushBack()
    {
      if (this.ttype == -4)
        return;
      this.pushedback = true;
    }

    public virtual void QuoteChar(int ch)
    {
      if (ch < 0 || ch > (int) byte.MaxValue)
        return;
      this.attribute[ch] = (sbyte) 8;
    }

    public virtual void ResetSyntax() => this.OrdinaryChars(0, (int) byte.MaxValue);

    public virtual void SlashSlashComments(bool flag) => this.slashSlashComments = flag;

    public virtual void SlashStarComments(bool flag) => this.slashStarComments = flag;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder("Token[");
      switch (this.ttype)
      {
        case -4:
          stringBuilder.Append("NOTHING");
          break;
        case -3:
          stringBuilder.Append(this.sval);
          break;
        case -2:
          stringBuilder.Append("number=");
          stringBuilder.Append(this.nval);
          break;
        case -1:
          stringBuilder.Append("EOF");
          break;
        case 10:
          stringBuilder.Append("EOL");
          break;
      }
      if (this.ttype > 0)
      {
        if (this.attribute[this.ttype] == (sbyte) 8)
        {
          stringBuilder.Append("quoted string=");
          stringBuilder.Append(this.sval);
        }
        else
        {
          stringBuilder.Append('\'');
          stringBuilder.Append((char) this.ttype);
          stringBuilder.Append('\'');
        }
      }
      stringBuilder.Append("], Line ");
      stringBuilder.Append(this.lineno);
      return stringBuilder.ToString();
    }

    public virtual void WhitespaceChars(int low, int hi) => this.setAttributes(low, hi, (sbyte) 2);

    public virtual void WordChars(int low, int hi) => this.setAttributes(low, hi, (sbyte) 1);
  }

  internal class BackReader : StreamReader
  {
    private char[] buffer;
    private int position = 1;

    public BackReader(Stream streamReader, int size, Encoding encoding)
      : base(streamReader, encoding)
    {
      this.buffer = new char[size];
      this.position = size;
    }

    public BackReader(Stream streamReader, Encoding encoding)
      : base(streamReader, encoding)
    {
      this.buffer = new char[this.position];
    }

    public bool MarkSupported() => false;

    public void Mark(int position) => throw new IOException("Mark operations are not allowed");

    public void Reset() => throw new IOException("Mark operations are not allowed");

    public override int Read()
    {
      return this.position >= 0 && this.position < this.buffer.Length ? (int) this.buffer[this.position++] : base.Read();
    }

    public override int Read(char[] array, int index, int count)
    {
      int length = this.buffer.Length - this.position;
      if (count <= 0)
        return 0;
      if (length > 0)
      {
        if (count < length)
          length = count;
        Array.Copy((Array) this.buffer, this.position, (Array) array, index, length);
        count -= length;
        index += length;
        this.position += length;
      }
      if (count <= 0)
        return length;
      count = base.Read(array, index, count);
      if (count != -1)
        return length + count;
      return length == 0 ? -1 : length;
    }

    public bool IsReady()
    {
      return this.position >= this.buffer.Length || this.BaseStream.Position >= this.BaseStream.Length;
    }

    public void UnRead(int unReadChar)
    {
      --this.position;
      this.buffer[this.position] = (char) unReadChar;
    }

    public void UnRead(char[] array, int index, int count) => this.Move(array, index, count);

    public void UnRead(char[] array) => this.Move(array, 0, array.Length - 1);

    private void Move(char[] array, int index, int count)
    {
      for (int index1 = index + count; index1 >= index; --index1)
        this.UnRead((int) array[index1]);
    }
  }

  internal class BackInputStream : BinaryReader
  {
    private byte[] buffer;
    private int position = 1;

    public BackInputStream(Stream streamReader, int size)
      : base(streamReader)
    {
      this.buffer = new byte[size];
      this.position = size;
    }

    public BackInputStream(Stream streamReader)
      : base(streamReader)
    {
      this.buffer = new byte[this.position];
    }

    public bool MarkSupported() => false;

    public override int Read()
    {
      return this.position >= 0 && this.position < this.buffer.Length ? (int) this.buffer[this.position++] : base.Read();
    }

    public virtual int Read(sbyte[] array, int index, int count)
    {
      int num1 = count + index;
      byte[] byteArray = SupportClass.ToByteArray(array);
      int num2 = 0;
      while (this.position < this.buffer.Length && index < num1)
      {
        byteArray[index++] = this.buffer[this.position++];
        ++num2;
      }
      if (index < num1)
        num2 += this.Read(byteArray, index, num1 - index);
      for (int index1 = 0; index1 < byteArray.Length; ++index1)
        array[index1] = (sbyte) byteArray[index1];
      return num2;
    }

    public void UnRead(int element)
    {
      --this.position;
      if (this.position < 0)
        return;
      this.buffer[this.position] = (byte) element;
    }

    public void UnRead(byte[] array, int index, int count) => this.Move(array, index, count);

    public void UnRead(byte[] array) => this.Move(array, 0, array.Length - 1);

    public long Skip(long numberOfBytes)
    {
      return this.BaseStream.Seek(numberOfBytes, SeekOrigin.Current) - this.BaseStream.Position;
    }

    private void Move(byte[] array, int index, int count)
    {
      for (int index1 = index + count; index1 >= index; --index1)
        this.UnRead((int) array[index1]);
    }
  }
}
