// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ContentParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

internal class ContentParser
{
  private ContentLexer m_lexer;
  private StringBuilder m_operatorParams;
  private PdfRecordCollection m_recordCollection;
  private List<string> m_operands = new List<string>();
  private List<byte> m_inlineImageBytes = new List<byte>();
  private bool m_isByteOperands;
  internal bool IsTextExtractionProcess;
  internal bool ConformanceEnabled;
  private static string[] operators = new string[79]
  {
    "b",
    "B",
    "bx",
    "Bx",
    "BDC",
    "BI",
    "BMC",
    "BT",
    "BX",
    "c",
    "cm",
    "CS",
    "cs",
    "d",
    "d0",
    "d1",
    "Do",
    "DP",
    "EI",
    "EMC",
    "ET",
    "EX",
    "f",
    "F",
    "fx",
    "G",
    "g",
    "gs",
    "h",
    "i",
    "ID",
    "j",
    "J",
    "K",
    "k",
    "l",
    "m",
    "M",
    "MP",
    "n",
    "q",
    "Q",
    "re",
    "RG",
    "rg",
    "ri",
    "s",
    "S",
    "SC",
    "sc",
    "SCN",
    "scn",
    "sh",
    "f*",
    "Tx",
    "Tc",
    "Td",
    "TD",
    "Tf",
    "Tj",
    "TJ",
    "TL",
    "Tm",
    "Tr",
    "Ts",
    "Tw",
    "Tz",
    "v",
    "w",
    "W",
    "W*",
    "Wx",
    "y",
    "T*",
    "b*",
    "B*",
    "'",
    "\"",
    "true"
  };

  public ContentParser(byte[] contentStream)
  {
    this.m_lexer = new ContentLexer(contentStream);
    this.m_operatorParams = this.m_lexer.OperatorParams;
    this.m_recordCollection = new PdfRecordCollection();
  }

  internal void Dispose()
  {
    this.m_operatorParams.Clear();
    this.m_lexer.Dispose();
    this.m_recordCollection.RecordCollection.Clear();
  }

  public PdfRecordCollection ReadContent()
  {
    this.ParseObject(TokenType.Eof);
    if (this.IsTextExtractionProcess)
      this.m_lexer.Dispose();
    return this.m_recordCollection;
  }

  private void ParseObject(TokenType stop)
  {
    TokenType nextToken;
    while ((nextToken = this.GetNextToken()) != TokenType.Eof && nextToken != stop)
    {
      switch (nextToken)
      {
        case TokenType.None:
          return;
        case TokenType.Integer:
          if (this.m_operatorParams.ToString() == "-")
          {
            this.m_operands.Add("0");
            continue;
          }
          this.m_operands.Add(this.m_operatorParams.ToString());
          continue;
        case TokenType.Real:
          this.m_operands.Add(this.m_operatorParams.ToString());
          continue;
        case TokenType.String:
        case TokenType.HexString:
        case TokenType.UnicodeString:
        case TokenType.UnicodeHexString:
          this.m_operands.Add(this.m_operatorParams.ToString());
          continue;
        case TokenType.Name:
          if (this.m_operatorParams.ToString() == "/Artifact")
            this.m_lexer.IsContainsArtifacts = true;
          this.m_operands.Add(this.m_operatorParams.ToString());
          continue;
        case TokenType.Operator:
          if (this.m_operatorParams.ToString() == "true")
          {
            this.m_operands.Add(this.m_operatorParams.ToString());
            continue;
          }
          if (this.m_operatorParams.ToString() == "ID")
          {
            this.CreateRecord();
            this.m_operands.Clear();
            this.ConsumeValue();
            continue;
          }
          this.CreateRecord();
          this.m_operands.Clear();
          continue;
        case TokenType.EndArray:
          throw new InvalidOperationException("Error while parsing content");
        default:
          continue;
      }
    }
  }

  private TokenType GetNextToken() => this.m_lexer.GetNextToken();

  private void ConsumeValue()
  {
    char nextInlineChar1;
    char charforInlineStream1;
    if (this.ConformanceEnabled)
    {
      char charforInlineStream2;
      char nextInlineChar2;
      while (true)
      {
        List<char> charList = new List<char>();
        int count = 0;
        charforInlineStream2 = this.m_lexer.GetNextCharforInlineStream();
        if (charforInlineStream2 == 'E')
        {
          nextInlineChar2 = this.m_lexer.GetNextInlineChar();
          if (nextInlineChar2 == 'I')
          {
            char nextInlineChar3 = this.m_lexer.GetNextInlineChar();
            charList.Add(this.m_lexer.GetNextChar(true));
            if ((nextInlineChar3 == ' ' || nextInlineChar3 == '\n' || nextInlineChar3 == char.MaxValue || nextInlineChar3 == '\r') && charList.Count > 0)
            {
              while (charList[charList.Count - 1] == ' ' || charList[charList.Count - 1] == '\r' || charList[charList.Count - 1] == '\n')
              {
                charList.Add(this.m_lexer.GetNextChar());
                ++count;
              }
            }
            if (!this.IsTextExtractionProcess)
              this.m_lexer.ResetContentPointer(count);
            if ((nextInlineChar3 == ' ' || nextInlineChar3 == '\n' || nextInlineChar3 == char.MaxValue || nextInlineChar3 == '\r') && charList.Count > 0)
            {
              if (charList[charList.Count - 1] != 'Q' && charList[charList.Count - 1] != char.MaxValue && charList[charList.Count - 1] != 'S')
              {
                this.m_inlineImageBytes.Add((byte) charforInlineStream2);
                this.m_inlineImageBytes.Add((byte) nextInlineChar2);
                this.m_inlineImageBytes.Add((byte) nextInlineChar3);
                if (charList.Count > 1)
                {
                  charList.RemoveAt(0);
                  charList.RemoveAt(charList.Count - 1);
                }
                foreach (byte num in charList)
                  this.m_inlineImageBytes.Add(num);
                charforInlineStream1 = this.m_lexer.GetNextCharforInlineStream();
              }
              else
                break;
            }
            else
            {
              this.m_inlineImageBytes.Add((byte) charforInlineStream2);
              this.m_inlineImageBytes.Add((byte) nextInlineChar2);
              this.m_inlineImageBytes.Add((byte) nextInlineChar3);
              if (charList.Count > 0)
                this.m_inlineImageBytes.Add((byte) charList[0]);
              charforInlineStream1 = this.m_lexer.GetNextCharforInlineStream();
            }
          }
          else
          {
            this.m_inlineImageBytes.Add((byte) charforInlineStream2);
            this.m_inlineImageBytes.Add((byte) nextInlineChar2);
          }
        }
        else
          this.m_inlineImageBytes.Add((byte) charforInlineStream2);
        charList.Clear();
      }
      this.m_operatorParams.Length = 0;
      this.m_operatorParams.Append(charforInlineStream2);
      this.m_operatorParams.Append(nextInlineChar2);
      this.m_isByteOperands = true;
      this.CreateRecord();
      this.m_isByteOperands = false;
      this.m_inlineImageBytes.Clear();
      nextInlineChar1 = this.m_lexer.GetNextInlineChar();
    }
    else
    {
      char charforInlineStream3;
      char nextInlineChar4;
      while (true)
      {
        char nextInlineChar5;
        char nextChar;
        do
        {
          int count = 0;
          charforInlineStream3 = this.m_lexer.GetNextCharforInlineStream();
          if (charforInlineStream3 == 'E')
          {
            nextInlineChar4 = this.m_lexer.GetNextInlineChar();
            if (nextInlineChar4 == 'I')
            {
              nextInlineChar5 = this.m_lexer.GetNextInlineChar();
              nextChar = this.m_lexer.GetNextChar(true);
              while (nextChar == ' ' || nextChar == '\r' || nextChar == '\n')
              {
                nextChar = this.m_lexer.GetNextChar();
                ++count;
              }
              if (!this.IsTextExtractionProcess)
                this.m_lexer.ResetContentPointer(count);
              if (nextInlineChar5 != ' ' && nextInlineChar5 != '\n' && nextInlineChar5 != char.MaxValue && nextInlineChar5 != '\r')
                goto label_35;
            }
            else
              goto label_36;
          }
          else
            goto label_37;
        }
        while (nextChar != 'Q' && nextChar != char.MaxValue && nextChar != 'S');
        break;
label_35:
        this.m_inlineImageBytes.Add((byte) charforInlineStream3);
        this.m_inlineImageBytes.Add((byte) nextInlineChar4);
        this.m_inlineImageBytes.Add((byte) nextInlineChar5);
        this.m_inlineImageBytes.Add((byte) nextChar);
        charforInlineStream1 = this.m_lexer.GetNextCharforInlineStream();
        continue;
label_36:
        this.m_inlineImageBytes.Add((byte) charforInlineStream3);
        this.m_inlineImageBytes.Add((byte) nextInlineChar4);
        continue;
label_37:
        this.m_inlineImageBytes.Add((byte) charforInlineStream3);
      }
      this.m_operatorParams.Length = 0;
      this.m_operatorParams.Append(charforInlineStream3);
      this.m_operatorParams.Append(nextInlineChar4);
      this.m_isByteOperands = true;
      this.CreateRecord();
      this.m_isByteOperands = false;
      this.m_inlineImageBytes.Clear();
      nextInlineChar1 = this.m_lexer.GetNextInlineChar();
    }
  }

  private void CreateRecord()
  {
    string name = this.m_operatorParams.ToString();
    Array.IndexOf<string>(ContentParser.operators, name);
    this.m_recordCollection.Add(this.m_isByteOperands ? new PdfRecord(name, this.m_inlineImageBytes.ToArray()) : new PdfRecord(name, this.m_operands.ToArray()));
  }
}
