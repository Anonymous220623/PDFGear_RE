// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.PdfParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class PdfParser
{
  private CrossTable m_cTable;
  private PdfReader m_reader;
  private PdfLexer m_lexer;
  private TokenType m_next;
  private Queue<int> m_integerQueue = new Queue<int>();
  private PdfCrossTable m_crossTable;
  private bool m_bEncrypt;
  private bool m_colorSpace;
  private bool m_isPassword;
  private bool m_certString;

  public PdfParser(CrossTable cTable, PdfReader reader, PdfCrossTable crossTable)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cTable == null)
      throw new ArgumentNullException(nameof (cTable));
    if (crossTable == null)
      throw new ArgumentNullException(nameof (crossTable));
    this.m_reader = reader;
    this.m_cTable = cTable;
    this.m_crossTable = crossTable;
    this.m_lexer = new PdfLexer((TextReader) reader);
  }

  internal bool Encrypted
  {
    get => this.m_bEncrypt;
    set => this.m_bEncrypt = value;
  }

  internal PdfLexer Lexer => this.m_lexer;

  internal long Position => this.m_reader.Position;

  public IPdfPrimitive Parse(long offset)
  {
    this.SetOffset(offset);
    this.Advance();
    return this.Parse();
  }

  public IPdfPrimitive Parse()
  {
    this.Match(this.m_next, TokenType.Number);
    this.Simple();
    this.Simple();
    this.Match(this.m_next, TokenType.ObjectStart);
    this.Advance();
    IPdfPrimitive pdfPrimitive = this.Simple();
    if (this.m_next != TokenType.ObjectEnd)
      this.m_next = TokenType.ObjectEnd;
    this.Match(this.m_next, TokenType.ObjectEnd);
    if (!this.m_lexer.Skip)
      this.Advance();
    else
      this.m_lexer.Skip = false;
    return pdfPrimitive;
  }

  public IPdfPrimitive Trailer(long offset)
  {
    this.SetOffset(offset);
    return this.Trailer();
  }

  public IPdfPrimitive Trailer()
  {
    this.Match(this.m_next, TokenType.Trailer);
    this.Advance();
    return this.Dictionary();
  }

  public long StartXRef()
  {
    this.Advance();
    this.Match(this.m_next, TokenType.StartXRef);
    this.Advance();
    return this.Number() is PdfNumber pdfNumber ? pdfNumber.LongValue : 0L;
  }

  public void SetOffset(long offset)
  {
    this.m_reader.Position = offset;
    if (this.m_integerQueue.Count > 0)
      this.m_integerQueue.Clear();
    this.m_lexer.Reset();
  }

  public IPdfPrimitive ParseXRefTable(
    System.Collections.Generic.Dictionary<long, ObjectInformation> objects,
    CrossTable cTable)
  {
    this.Advance();
    IPdfPrimitive stream1;
    if (this.m_next == TokenType.XRef)
    {
      if (!cTable.m_isOpenAndRepair)
      {
        this.ParseOldXRef(cTable, objects);
      }
      else
      {
        do
        {
          this.Advance();
          if (this.m_next == TokenType.Eof && cTable.m_isOpenAndRepair && cTable.Reader.Position == cTable.Reader.Stream.Length)
            cTable.Reader.Position = 0L;
        }
        while (this.m_next != TokenType.Trailer);
      }
      stream1 = this.Trailer();
      PdfDictionary pdfDictionary = stream1 as PdfDictionary;
      if (pdfDictionary.ContainsKey("Size"))
      {
        int intValue = (pdfDictionary["Size"] as PdfNumber).IntValue;
        int num1 = cTable.m_initialSubsectionCount != cTable.m_initialNumberOfSubsection ? (int) cTable.m_initialSubsectionCount : (int) cTable.m_initialNumberOfSubsection;
        int num2 = cTable.m_isOpenAndRepair ? objects.Count - 1 : (int) cTable.m_totalNumberOfSubsection;
        if (intValue < num1 + num2 && num1 > 0 && intValue == num2)
        {
          int num3 = num1 + num2 - intValue;
          System.Collections.Generic.Dictionary<long, ObjectInformation> dictionary = new System.Collections.Generic.Dictionary<long, ObjectInformation>();
          foreach (KeyValuePair<long, ObjectInformation> keyValuePair in objects)
            dictionary.Add(keyValuePair.Key - (long) num3, keyValuePair.Value);
          objects = dictionary;
          cTable.m_objects = dictionary;
        }
        else if (intValue != num2 && !cTable.m_isOpenAndRepair && intValue < num2)
          pdfDictionary["Size"] = (IPdfPrimitive) new PdfNumber(num2);
      }
    }
    else
    {
      stream1 = this.Parse();
      if (cTable.m_isOpenAndRepair && PdfCrossTable.Dereference(stream1) is PdfDictionary pdfDictionary1 && !pdfDictionary1.ContainsKey("Root"))
      {
        for (int index = 0; index < objects.Count; ++index)
        {
          stream1 = this.Parse();
          if (PdfCrossTable.Dereference(stream1) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Root"))
            break;
        }
      }
      cTable.ParseNewTable(stream1 as PdfStream, objects);
    }
    if (stream1 is PdfDictionary pdfDictionary2 && this.m_crossTable != null && !this.m_crossTable.isRepair && pdfDictionary2.ContainsKey("XRefStm"))
    {
      if (objects.Count > 0)
      {
        try
        {
          long offset = 0;
          if (PdfCrossTable.Dereference(pdfDictionary2["XRefStm"]) is PdfNumber pdfNumber)
            offset = pdfNumber.LongValue;
          cTable.Parser.SetOffset(offset);
          IPdfPrimitive stream2 = cTable.Parser.Parse(offset);
          if (stream2 is PdfStream)
            cTable.ParseNewTable(stream2 as PdfStream, objects);
        }
        catch
        {
        }
      }
    }
    return stream1;
  }

  public void RebuildXrefTable(
    System.Collections.Generic.Dictionary<long, ObjectInformation> newObjects,
    CrossTable crosstable)
  {
    PdfReader pdfReader = new PdfReader(this.m_reader.Stream);
    pdfReader.Position = 0L;
    newObjects.Clear();
    byte[] numArray1 = new byte[64 /*0x40*/];
    while (this.m_reader.Position < pdfReader.Stream.Length - 1L)
    {
      long position1 = pdfReader.Position;
      pdfReader.ReadLine(numArray1, true);
      if (numArray1[0] >= (byte) 48 /*0x30*/ && numArray1[0] <= (byte) 57 && PdfString.ByteToString(numArray1).Contains("obj"))
      {
        long[] numArray2 = this.CheckObjectStart(numArray1, this.m_reader.Position);
        if (numArray2 != null)
        {
          long key = numArray2[0];
          if (crosstable.m_isOpenAndRepair)
          {
            long position2 = this.m_reader.Position;
            this.m_reader.Position = position1;
            this.m_reader.SkipWS();
            if (position1 != this.m_reader.Position)
              position1 = this.m_reader.Position;
            this.m_reader.Position = position2;
          }
          ObjectInformation objectInformation = new ObjectInformation(ObjectType.Normal, position1, (ArchiveInformation) null, crosstable);
          if (!newObjects.ContainsKey(key))
            newObjects.Add(key, objectInformation);
          else if (crosstable.m_isOpenAndRepair)
            newObjects[key] = objectInformation;
        }
      }
    }
  }

  private PdfParser(byte[] data)
  {
    this.m_reader = new PdfReader((Stream) new MemoryStream(data));
    this.m_lexer = new PdfLexer((TextReader) this.m_reader);
  }

  private long[] CheckObjectStart(byte[] line, long pos)
  {
    PdfParser pdfParser = new PdfParser(line);
    try
    {
      pdfParser.SetOffset(0L);
      pdfParser.Advance();
      if (pdfParser.GetNext() != TokenType.Number)
        return (long[]) null;
      int intValue1 = pdfParser.ParseInteger().IntValue;
      if (pdfParser.GetNext() != TokenType.Number)
        return (long[]) null;
      int intValue2 = pdfParser.ParseInteger().IntValue;
      if (!pdfParser.Lexer.Text.Equals("obj"))
        return (long[]) null;
      return new long[2]
      {
        (long) intValue1,
        (long) intValue2
      };
    }
    catch
    {
    }
    finally
    {
      pdfParser.m_reader.Stream.Dispose();
    }
    return (long[]) null;
  }

  internal System.Collections.Generic.Dictionary<long, ObjectInformation> FindFirstObject(
    System.Collections.Generic.Dictionary<long, ObjectInformation> newObjects,
    CrossTable crosstable)
  {
    PdfReader pdfReader = new PdfReader(this.m_reader.Stream);
    pdfReader.Position = 0L;
    newObjects.Clear();
    byte[] numArray1 = new byte[64 /*0x40*/];
    bool flag = false;
    while (!flag && this.m_reader.Position < pdfReader.Stream.Length - 1L)
    {
      long position1 = pdfReader.Position;
      pdfReader.ReadLine(numArray1, true);
      if (numArray1[0] >= (byte) 48 /*0x30*/ && numArray1[0] <= (byte) 57 && PdfString.ByteToString(numArray1).Contains("obj"))
      {
        long[] numArray2 = this.CheckObjectStart(numArray1, this.m_reader.Position);
        if (numArray2 != null)
        {
          long key = numArray2[0];
          if (crosstable.m_isOpenAndRepair)
          {
            long position2 = this.m_reader.Position;
            this.m_reader.Position = position1;
            this.m_reader.SkipWS();
            if (position1 != this.m_reader.Position)
              position1 = this.m_reader.Position;
            this.m_reader.Position = position2;
          }
          ObjectInformation objectInformation = new ObjectInformation(ObjectType.Normal, position1, (ArchiveInformation) null, crosstable);
          if (!newObjects.ContainsKey(key))
            newObjects.Add(key, objectInformation);
          else if (crosstable.m_isOpenAndRepair)
            newObjects[key] = objectInformation;
          break;
        }
      }
    }
    return newObjects;
  }

  internal IPdfPrimitive Simple()
  {
    IPdfPrimitive pdfPrimitive;
    if (this.m_integerQueue.Count != 0)
    {
      pdfPrimitive = this.Number();
    }
    else
    {
      switch (this.m_next)
      {
        case TokenType.DictionaryStart:
          pdfPrimitive = this.Dictionary();
          break;
        case TokenType.HexStringStart:
          pdfPrimitive = this.HexString();
          break;
        case TokenType.String:
          pdfPrimitive = this.ReadString();
          break;
        case TokenType.UnicodeString:
          pdfPrimitive = this.ReadUnicodeString();
          break;
        case TokenType.Number:
          pdfPrimitive = this.Number();
          break;
        case TokenType.Real:
          pdfPrimitive = this.Real();
          break;
        case TokenType.Name:
          pdfPrimitive = this.ReadName();
          break;
        case TokenType.ArrayStart:
          pdfPrimitive = this.Array();
          break;
        case TokenType.Boolean:
          pdfPrimitive = this.ReadBoolean();
          break;
        case TokenType.Null:
          pdfPrimitive = (IPdfPrimitive) new PdfNull();
          this.Advance();
          break;
        default:
          pdfPrimitive = (IPdfPrimitive) null;
          break;
      }
    }
    return pdfPrimitive;
  }

  internal char GetObjectFlag()
  {
    this.Match(this.m_next, TokenType.ObjectType);
    char objectFlag = this.m_lexer.Text[0];
    this.Advance();
    return objectFlag;
  }

  internal void StartFrom(long offset)
  {
    this.SetOffset(offset);
    this.Advance();
  }

  internal FdfObject ParseObject()
  {
    PdfNumber objNum = this.Simple() as PdfNumber;
    PdfNumber genNum = this.Simple() as PdfNumber;
    this.Match(this.m_next, TokenType.ObjectStart);
    this.Advance();
    IPdfPrimitive pdfPrimitive = this.Simple();
    if (this.m_next != TokenType.ObjectEnd)
      this.m_next = TokenType.ObjectEnd;
    this.Match(this.m_next, TokenType.ObjectEnd);
    return new FdfObject(objNum, genNum, pdfPrimitive);
  }

  private void ParseOldXRef(CrossTable cTable, System.Collections.Generic.Dictionary<long, ObjectInformation> objects)
  {
    this.Advance();
    while (this.IsSubsection())
      cTable.ParseSubsection(this, objects);
  }

  private bool IsSubsection()
  {
    if (this.m_next == TokenType.Trailer)
      return false;
    if (this.m_next == TokenType.Number)
      return true;
    throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
  }

  private void Error(PdfParser.ErrorType error, string additional)
  {
    string message;
    switch (error)
    {
      case PdfParser.ErrorType.Unexpected:
        message = "Unexpected token ";
        break;
      case PdfParser.ErrorType.BadlyFormedReal:
        message = "Badly formed real number ";
        break;
      case PdfParser.ErrorType.BadlyFormedInteger:
        message = "Badly formed integer number ";
        break;
      case PdfParser.ErrorType.BadlyFormedDictionary:
        message = "Badly formed dictionary ";
        break;
      case PdfParser.ErrorType.UnknownStreamLength:
        message = "Unknown stream length";
        break;
      default:
        message = "Internal error.";
        break;
    }
    if (additional != null)
      message = $"{message}{additional} before {(object) this.m_lexer.Position}";
    throw new PdfException(message);
  }

  private void Match(TokenType token, TokenType match)
  {
    if (token == match)
      return;
    this.Error(PdfParser.ErrorType.Unexpected, token.ToString());
  }

  internal void Advance()
  {
    if (this.m_cTable != null && this.m_cTable.validateSyntax)
      this.m_lexer.objectName = this.m_next.ToString();
    this.m_next = this.m_lexer.GetNextToken();
  }

  internal TokenType GetNext() => this.m_next;

  private IPdfPrimitive ReadName()
  {
    this.Match(this.m_next, TokenType.Name);
    PdfName pdfName = new PdfName(this.m_lexer.Text.Substring(1));
    this.Advance();
    return (IPdfPrimitive) pdfName;
  }

  private IPdfPrimitive ReadBoolean()
  {
    this.Match(this.m_next, TokenType.Boolean);
    PdfBoolean pdfBoolean = new PdfBoolean(this.m_lexer.Text == "true");
    this.Advance();
    return (IPdfPrimitive) pdfBoolean;
  }

  private IPdfPrimitive ReadUnicodeString()
  {
    char[] charArray = this.m_lexer.Text.ToCharArray();
    string text = new string(charArray, 1, charArray.Length - 2);
    string str = PdfString.ByteToString(Encoding.BigEndianUnicode.GetPreamble());
    if (charArray.Length > 1)
    {
      if (text.Substring(0, 2).Equals(str))
      {
        if (this.CheckForExtraSequence(text))
          text = this.ProcessEscapes(text, false);
        this.ProcessUnicodeWithPreamble(ref text);
      }
      else
        text = this.ProcessUnicodeEscapes(text);
    }
    else
      text = this.ProcessUnicodeEscapes(text);
    PdfString pdfString = new PdfString(text);
    if (!this.m_lexer.Skip)
      this.Advance();
    else
      this.m_next = TokenType.DictionaryEnd;
    return (IPdfPrimitive) pdfString;
  }

  private string ProcessUnicodeEscapes(string text)
  {
    StringBuilder stringBuilder = new StringBuilder(text.Length / 2);
    bool flag = true;
    char ch1 = char.MinValue;
    foreach (char ch2 in text)
    {
      if (flag)
      {
        if (ch2 == ' ')
        {
          stringBuilder.Append(ch2);
          flag = !flag;
        }
        else
          ch1 = (char) ((uint) ch2 << 8);
      }
      else if (ch2 != '\\' && ch2 != '\r')
      {
        if ((int) ch1 + (int) ch2 <= 257)
        {
          ch1 += ch2;
          stringBuilder.Append(ch1);
        }
        else if (stringBuilder.Length > 0)
        {
          ch1 = (char) ((uint) char.MinValue + (uint) ch2);
          stringBuilder.Append(ch1);
        }
      }
      else
        flag = !flag;
      flag = !flag;
    }
    return this.ProcessEscapes(stringBuilder.ToString(), false);
  }

  private IPdfPrimitive ReadString()
  {
    this.Match(this.m_next, TokenType.String);
    string text = this.m_lexer.StringText.ToString();
    bool flag1 = false;
    bool flag2 = false;
    if (this.m_isPassword)
      text = this.ProcessEscapes(text, false);
    else if (!this.m_colorSpace)
    {
      if (this.CheckForPreamble(text))
      {
        if (this.CheckEscapeSequence(text.Substring(2)))
        {
          bool flag3 = false;
          while (text.Contains("\\") || flag3 && text.Contains("\0"))
          {
            if (!flag3)
              text = text.Substring(2);
            string str = text;
            text = this.ProcessEscapes(text, true);
            flag3 = true;
            flag2 = true;
            if (str == text || text.EndsWith("\\"))
              break;
          }
        }
        else
        {
          this.ProcessUnicodeWithPreamble(ref text);
          flag1 = true;
        }
      }
      else
      {
        if (!this.CheckUnicodePeramble(text))
          text = this.ProcessEscapes(text, false);
        if (this.CheckForPreamble(text))
        {
          this.ProcessUnicodeWithPreamble(ref text);
          flag1 = true;
        }
        if (this.CheckUnicodePeramble(text))
        {
          text = this.ProcessEscapes(text.Remove(0, 2), true);
          flag1 = true;
        }
      }
    }
    else if (this.m_crossTable != null && this.m_crossTable.Document != null && this.m_crossTable.Document.WasEncrypted)
    {
      text = this.ProcessEscapes(text, false);
    }
    else
    {
      if (this.m_colorSpace)
        text = this.ProcessEscapes(text, false);
      text = "ColorFound" + text;
    }
    PdfString pdfString = new PdfString(text);
    if (this.m_colorSpace)
      pdfString.IsColorSpace = true;
    if (!flag1 && !flag2)
      pdfString.Encode = PdfString.ForceEncoding.ASCII;
    this.Advance();
    return (IPdfPrimitive) pdfString;
  }

  private bool CheckForPreamble(string text)
  {
    string str = PdfString.ByteToString(Encoding.BigEndianUnicode.GetPreamble());
    return text.Length > 1 && text.Substring(0, 2).Equals(str);
  }

  private bool CheckEscapeSequence(string text)
  {
    if (text != null && text.Length > 2)
    {
      char[] charArray = text.ToCharArray();
      if (charArray[0] == '\\' && (charArray[1] == '\\' || charArray[1] == '0') && charArray[2] == '0')
        return true;
    }
    return false;
  }

  private bool CheckForExtraSequence(string text)
  {
    string str = text.Substring(2);
    return str.Length >= 3 && str[2] == '0';
  }

  private bool CheckUnicodePeramble(string text)
  {
    string str = PdfString.ByteToString(Encoding.Unicode.GetPreamble());
    return text.Length > 1 && text.Substring(0, 2).Equals(str);
  }

  private string ProcessEscapes(string text, bool isComplete)
  {
    if (this.m_isPassword)
      return this.ProcessEncryptEscapes(text);
    if (!isComplete)
      text = text.Replace("\r", "");
    if (this.m_isPassword)
      text = text.Replace("\n", "");
    StringBuilder stringBuilder = new StringBuilder(text.Length);
    bool flag = false;
    int i = 0;
    for (int length = text.Length; i < length; ++i)
    {
      char c = text[i];
      if (!flag)
      {
        switch (c)
        {
          case char.MinValue:
            if (!this.Encrypted && !this.m_colorSpace && (c != char.MinValue || !this.m_isPassword))
            {
              if (this.m_certString && c == char.MinValue)
              {
                stringBuilder.Append(c);
                continue;
              }
              continue;
            }
            break;
          case '\\':
            flag = true;
            continue;
        }
        if (isComplete && char.IsControl(c) && i - 1 >= 0)
        {
          byte[] bytes = new byte[2]
          {
            (byte) 32 /*0x20*/,
            (byte) c
          };
          string str = Encoding.BigEndianUnicode.GetString(bytes, 0, bytes.Length);
          if (text[i - 1] == ' ')
          {
            c = str.ToCharArray()[0];
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
          }
          else if (i + 1 <= text.Length - 1 && text[i + 1] == ' ')
          {
            c = str.ToCharArray()[0];
            ++i;
          }
        }
        stringBuilder.Append(c);
      }
      else
      {
        switch (c)
        {
          case '(':
          case ')':
          case '\\':
            stringBuilder.Append(c);
            break;
          case 'b':
            stringBuilder.Append('\b');
            break;
          case 'f':
            stringBuilder.Append('\f');
            break;
          case 'n':
            stringBuilder.Append('\n');
            break;
          case 'r':
            stringBuilder.Append('\r');
            break;
          case 't':
            stringBuilder.Append('\t');
            break;
          default:
            if (c <= '7' && c >= '0')
            {
              c = this.ProcessOctal(text, ref i);
              --i;
            }
            if (c < 'Ā')
            {
              if (isComplete && (c != char.MinValue || this.Encrypted || this.m_colorSpace || c == char.MinValue && this.m_isPassword))
              {
                if (c == '\r' && i + 1 < length && text[i + 1] == '\n')
                {
                  ++i;
                  break;
                }
                stringBuilder.Append(c);
                break;
              }
              if (c != '\n')
              {
                stringBuilder.Append(c);
                break;
              }
              break;
            }
            break;
        }
        flag = false;
      }
    }
    return stringBuilder.ToString();
  }

  private char ProcessOctal(string text, ref int i)
  {
    int length = text.Length;
    int num = 0;
    string empty = string.Empty;
    for (; i < length && num < 3; ++num)
    {
      char ch = text[i];
      switch (ch)
      {
        case '0':
        case '1':
        case '2':
        case '3':
        case '4':
        case '5':
        case '6':
        case '7':
          empty += ch.ToString();
          break;
      }
      ++i;
    }
    return (char) Convert.ToInt32(empty, 8);
  }

  private string ProcessEncryptEscapes(string text)
  {
    StringBuilder stringBuilder = new StringBuilder(text.Length);
    bool flag = false;
    int i = 0;
    for (int length = text.Length; i < length; ++i)
    {
      char ch = text[i];
      if (!flag)
      {
        switch (ch)
        {
          case char.MinValue:
            if (this.Encrypted || this.m_colorSpace || ch == char.MinValue && this.m_isPassword)
              break;
            continue;
          case '\n':
          case '\r':
            goto label_6;
          case '\\':
            flag = true;
            continue;
        }
        stringBuilder.Append(ch);
        continue;
      }
label_6:
      switch (ch)
      {
        case '\n':
          if (text[i - 1] != '\r')
          {
            stringBuilder.Append(ch);
            goto case '\r';
          }
          goto case '\r';
        case '\r':
          flag = false;
          continue;
        case '(':
        case ')':
        case '\\':
          stringBuilder.Append(ch);
          goto case '\r';
        case 'b':
          stringBuilder.Append('\b');
          goto case '\r';
        case 'f':
          stringBuilder.Append('\f');
          goto case '\r';
        case 'n':
          stringBuilder.Append('\n');
          goto case '\r';
        case 'r':
          stringBuilder.Append('\r');
          goto case '\r';
        case 't':
          stringBuilder.Append('\t');
          goto case '\r';
        default:
          if (ch <= '7' && ch >= '0')
          {
            ch = this.ProcessEncryptOctal(text, ref i);
            --i;
          }
          if (ch < 'Ā')
          {
            stringBuilder.Append(ch);
            goto case '\r';
          }
          goto case '\r';
      }
    }
    return stringBuilder.ToString();
  }

  private char ProcessEncryptOctal(string text, ref int i)
  {
    int length = text.Length;
    int num = 0;
    string empty = string.Empty;
    bool flag = false;
    for (; i < length && num < 3; ++num)
    {
      char ch = text[i];
      if (ch == '\\' && this.m_isPassword)
      {
        flag = true;
        break;
      }
      if (ch <= '7' && ch >= '0')
        empty += ch.ToString();
      ++i;
    }
    int int32 = Convert.ToInt32(empty, 8);
    if (flag && empty.Length == 1)
      int32 = (int) empty[0];
    return (char) int32;
  }

  private IPdfPrimitive HexString()
  {
    this.Match(this.m_next, TokenType.HexStringStart);
    this.Advance();
    StringBuilder stringBuilder = new StringBuilder(100);
    bool flag = true;
    while (this.m_next != TokenType.HexStringEnd)
    {
      string str = this.m_lexer.Text;
      if (this.m_next == TokenType.HexStringWeird)
        flag = false;
      else if (this.m_next == TokenType.HexStringWeirdEscape)
      {
        flag = false;
        str = str.Substring(1);
      }
      stringBuilder.Append(str);
      this.Advance();
    }
    this.Match(this.m_next, TokenType.HexStringEnd);
    this.Advance();
    PdfString pdfString = new PdfString(stringBuilder.ToString(), !flag);
    if (this.m_colorSpace)
      pdfString.IsColorSpace = true;
    pdfString.m_isHexString = flag;
    return (IPdfPrimitive) pdfString;
  }

  private IPdfPrimitive Number()
  {
    PdfNumber pdfNumber;
    if (this.m_integerQueue.Count > 0)
    {
      pdfNumber = new PdfNumber(this.m_integerQueue.Dequeue());
    }
    else
    {
      this.Match(this.m_next, TokenType.Number);
      pdfNumber = this.ParseInteger();
    }
    IPdfPrimitive pdfPrimitive = (IPdfPrimitive) pdfNumber;
    if (this.m_next == TokenType.Number)
    {
      PdfNumber integer = this.ParseInteger();
      if (this.m_next == TokenType.Reference)
      {
        pdfPrimitive = (IPdfPrimitive) new PdfReferenceHolder(new PdfReference((long) pdfNumber.IntValue, integer.IntValue), this.m_crossTable);
        this.Advance();
      }
      else
        this.m_integerQueue.Enqueue(integer.IntValue);
    }
    return pdfPrimitive;
  }

  private PdfNumber ParseInteger()
  {
    double result;
    bool flag = double.TryParse(this.m_lexer.Text, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result);
    PdfNumber integer = (PdfNumber) null;
    if (flag)
      integer = new PdfNumber((long) result);
    else
      this.Error(PdfParser.ErrorType.BadlyFormedInteger, this.m_lexer.Text);
    this.Advance();
    return integer;
  }

  private IPdfPrimitive Real()
  {
    this.Match(this.m_next, TokenType.Real);
    double result;
    bool flag = double.TryParse(this.m_lexer.Text, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result);
    PdfNumber pdfNumber = (PdfNumber) null;
    if (flag)
      pdfNumber = new PdfNumber((float) result);
    else
      this.Error(PdfParser.ErrorType.BadlyFormedReal, this.m_lexer.Text);
    this.Advance();
    return (IPdfPrimitive) pdfNumber;
  }

  private IPdfPrimitive Array()
  {
    this.Match(this.m_next, TokenType.ArrayStart);
    this.Advance();
    PdfArray pdfArray = new PdfArray();
    this.m_lexer.isArray = true;
    IPdfPrimitive element;
    while ((element = this.Simple()) != null)
    {
      pdfArray.Add(element);
      this.m_colorSpace = (object) (pdfArray[0] as PdfName) != null && (pdfArray[0] as PdfName).ToString() == "/Indexed";
      if (this.m_next == TokenType.Unknown)
        this.Advance();
    }
    this.Match(this.m_next, TokenType.ArrayEnd);
    this.Advance();
    this.m_lexer.isArray = false;
    pdfArray.FreezeChanges((object) this);
    return (IPdfPrimitive) pdfArray;
  }

  internal IPdfPrimitive Dictionary()
  {
    this.Match(this.m_next, TokenType.DictionaryStart);
    this.Advance();
    PdfDictionary dic = new PdfDictionary();
    Pair pair;
    while ((pair = this.ReadPair()) != (object) Pair.Empty)
    {
      if (pair.Value != null)
        dic[pair.Name] = pair.Value;
    }
    if (this.m_next != TokenType.DictionaryEnd)
      this.m_next = TokenType.DictionaryEnd;
    this.Match(this.m_next, TokenType.DictionaryEnd);
    if (!this.m_lexer.Skip)
    {
      this.Advance();
    }
    else
    {
      this.m_next = TokenType.ObjectEnd;
      this.m_lexer.Skip = false;
    }
    IPdfPrimitive pdfPrimitive = this.m_next != TokenType.StreamStart ? (IPdfPrimitive) dic : this.ReadStream(dic);
    (pdfPrimitive as IPdfChangable).FreezeChanges((object) this);
    return pdfPrimitive;
  }

  private IPdfPrimitive ReadStream(PdfDictionary dic)
  {
    this.Match(this.m_next, TokenType.StreamStart);
    this.m_lexer.SkipToken();
    this.m_lexer.SkipNewLine();
    IPdfPrimitive pdfPrimitive = dic["Length"];
    PdfNumber pdfNumber = pdfPrimitive as PdfNumber;
    PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
    if (pdfNumber == null && pdfReferenceHolder == (PdfReferenceHolder) null)
    {
      long position1 = this.m_lexer.Position;
      long position2 = this.m_reader.Position;
      this.m_reader.Position = position1;
      long num = this.m_reader.SearchForward("endstream");
      long count = num <= position1 ? position1 - num : num - position1;
      this.m_reader.Position = position2;
      byte[] data = this.m_lexer.Read((int) count);
      PdfStream pdfStream = new PdfStream(dic, data);
      this.Advance();
      if (this.m_next != TokenType.StreamEnd)
        this.m_next = TokenType.StreamEnd;
      this.Match(this.m_next, TokenType.StreamEnd);
      this.Advance();
      if (this.m_next != TokenType.ObjectEnd)
        this.m_next = TokenType.ObjectEnd;
      return (IPdfPrimitive) pdfStream;
    }
    if (pdfReferenceHolder != (PdfReferenceHolder) null)
    {
      PdfLexer lexer = this.m_lexer;
      long position = this.m_reader.Position;
      this.m_lexer = new PdfLexer((TextReader) this.m_reader);
      pdfNumber = this.m_cTable.GetObject((IPdfPrimitive) pdfReferenceHolder.Reference) as PdfNumber;
      this.m_reader.Position = position;
      this.m_lexer = lexer;
    }
    int intValue = pdfNumber.IntValue;
    PdfStream pdfStream1;
    if (this.CheckStreamLength(this.m_lexer.Position, intValue))
    {
      byte[] data = this.m_lexer.Read(intValue);
      pdfStream1 = new PdfStream(dic, data);
    }
    else
    {
      long position3 = this.m_lexer.Position;
      long position4 = this.m_reader.Position;
      this.m_reader.Position = position3;
      long num = this.m_reader.SearchForward("endstream");
      long count = num <= position3 ? position3 - num : num - position3;
      this.m_reader.Position = position4;
      byte[] data = this.m_lexer.Read((int) count);
      pdfStream1 = new PdfStream(dic, data);
    }
    this.Advance();
    if (this.m_next != TokenType.StreamEnd)
      this.m_next = TokenType.StreamEnd;
    this.Match(this.m_next, TokenType.StreamEnd);
    this.Advance();
    if (this.m_next != TokenType.ObjectEnd)
      this.m_next = TokenType.ObjectEnd;
    return (IPdfPrimitive) pdfStream1;
  }

  private bool CheckStreamLength(long lexPosition, int value)
  {
    string str = (string) null;
    bool flag = true;
    long position = this.m_reader.Position;
    this.m_reader.Position = lexPosition + (long) value;
    char[] buffer = new char[20];
    this.m_reader.ReadBlock(buffer, 0, 20);
    for (int index = 0; index < buffer.Length; ++index)
      str += buffer[index].ToString();
    if (!str.StartsWith("\nendstream") && !str.StartsWith("\r\nendstream") && !str.StartsWith("\rendstream") && !str.StartsWith("endstream"))
      flag = false;
    this.m_reader.Position = position;
    return flag;
  }

  private Pair ReadPair()
  {
    IPdfPrimitive pdfPrimitive1;
    try
    {
      pdfPrimitive1 = this.Simple();
    }
    catch
    {
      pdfPrimitive1 = (IPdfPrimitive) null;
    }
    if (pdfPrimitive1 == null)
      return Pair.Empty;
    PdfName name = pdfPrimitive1 as PdfName;
    if (name == (PdfName) null)
      this.Error(PdfParser.ErrorType.BadlyFormedDictionary, "next should be a name.");
    if (name == new PdfName("U") || name == new PdfName("O"))
      this.m_isPassword = true;
    if (pdfPrimitive1 != null && name != (PdfName) null && name.Value == "Cert")
      this.m_certString = true;
    IPdfPrimitive pdfPrimitive2 = this.Simple();
    this.m_isPassword = false;
    this.m_certString = false;
    return new Pair(name, pdfPrimitive2);
  }

  private void ProcessUnicodeWithPreamble(ref string text)
  {
    byte[] numArray = PdfString.StringToByte(text.Substring(2));
    int index1 = 0;
    string str = (string) null;
    bool flag = false;
    for (int index2 = 0; index2 < numArray.Length - 1; ++index2)
    {
      if (index2 + 2 <= numArray.Length - 1 && numArray[index2] == (byte) 92 && numArray[index2 + 1] == (byte) 92 && numArray[index2 + 2] == (byte) 114)
      {
        MemoryStream memoryStream = new MemoryStream();
        for (int index3 = 0; index3 <= index2; ++index3)
          memoryStream.WriteByte(numArray[index3]);
        memoryStream.WriteByte((byte) 13);
        for (int index4 = index2 + 3; index4 < numArray.Length; ++index4)
          memoryStream.WriteByte(numArray[index4]);
        numArray = PdfStream.StreamToBytes((Stream) memoryStream);
        memoryStream.Dispose();
        ++index2;
      }
      else if (numArray[index2] == (byte) 92 && (numArray[index2 + 1] == (byte) 40 || numArray[index2 + 1] == (byte) 41 || numArray[index2 + 1] == (byte) 13 || numArray[index2 + 1] == (byte) 62 || numArray[index2 + 1] == (byte) 92) || numArray[index2] == (byte) 13)
      {
        for (int index5 = index2; index5 < numArray.Length - 1; ++index5)
          numArray[index5] = numArray[index5 + 1];
        byte[] dst = new byte[numArray.Length - 1];
        Buffer.BlockCopy((System.Array) numArray, 0, (System.Array) dst, 0, numArray.Length - 1);
        numArray = dst;
        --index2;
      }
      else if (numArray[index2] == (byte) 92 && numArray[index2 + 1] == (byte) 114)
      {
        MemoryStream memoryStream = new MemoryStream();
        for (int index6 = 0; index6 < index2; ++index6)
          memoryStream.WriteByte(numArray[index6]);
        memoryStream.WriteByte((byte) 13);
        for (int index7 = index2 + 2; index7 < numArray.Length; ++index7)
          memoryStream.WriteByte(numArray[index7]);
        numArray = PdfStream.StreamToBytes((Stream) memoryStream);
        memoryStream.Dispose();
      }
      else if (numArray[index2] == (byte) 92 && numArray[index2 + 1] == (byte) 110)
      {
        MemoryStream memoryStream = new MemoryStream();
        for (int index8 = 0; index8 < index2; ++index8)
          memoryStream.WriteByte(numArray[index8]);
        memoryStream.WriteByte((byte) 10);
        for (int index9 = index2 + 2; index9 < numArray.Length; ++index9)
          memoryStream.WriteByte(numArray[index9]);
        numArray = PdfStream.StreamToBytes((Stream) memoryStream);
        memoryStream.Dispose();
      }
    }
    int count = numArray.Length - index1;
    if (flag)
    {
      text = str;
      text += Encoding.BigEndianUnicode.GetString(numArray, index1, count);
    }
    else
      text = Encoding.BigEndianUnicode.GetString(numArray, index1, count);
  }

  private bool CheckForControlSequence(string text)
  {
    if (text.Length > 1)
    {
      string str = PdfString.ByteToString(new byte[2]
      {
        byte.MaxValue,
        (byte) 253
      });
      if (text.Substring(text.Length - 2, 2).Equals(str))
        text = text.Substring(0, text.Length - 2);
    }
    byte[] numArray = PdfString.StringToByte(text);
    Windows1252Encoding windows1252Encoding = new Windows1252Encoding();
    byte[] bytes = new byte[1];
    System.Collections.Generic.Dictionary<byte, string> dictionary = new System.Collections.Generic.Dictionary<byte, string>();
    for (byte key = 127 /*0x7F*/; key < (byte) 164; ++key)
    {
      bytes[0] = key;
      dictionary.Add(key, windows1252Encoding.GetString(bytes, 0, bytes.Length));
    }
    for (int index = 0; index < numArray.Length; ++index)
    {
      int num = (int) numArray[index];
      if (num < 32 /*0x20*/ && num != 9 && num != 10 && num != 11 && num != 12 && num != 13 && num != 25 && num != 28 && num != 29 && num != 19 || num > (int) sbyte.MaxValue && (!dictionary.ContainsKey(numArray[index]) || dictionary[numArray[index]] != ((char) numArray[index]).ToString()))
        return true;
    }
    return false;
  }

  private enum ErrorType
  {
    None,
    Unexpected,
    BadlyFormedReal,
    BadlyFormedInteger,
    BadlyFormedHexString,
    BadlyFormedDictionary,
    UnknownStreamLength,
  }
}
