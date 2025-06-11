// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.HtmlToXaml.HtmlLexicalAnalyzer
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings.HtmlToXaml;

internal class HtmlLexicalAnalyzer
{
  private readonly StringReader _inputStringReader;
  private int _nextCharacterCode;
  private int _lookAheadCharacterCode;
  private char _lookAheadCharacter;
  private char _previousCharacter;
  private bool _ignoreNextWhitespace;
  private readonly StringBuilder _nextToken;

  internal HtmlLexicalAnalyzer(string inputTextString)
  {
    this._inputStringReader = new StringReader(inputTextString);
    this._nextCharacterCode = 0;
    this.NextCharacter = ' ';
    this._lookAheadCharacterCode = this._inputStringReader.Read();
    this._lookAheadCharacter = (char) this._lookAheadCharacterCode;
    this._previousCharacter = ' ';
    this._ignoreNextWhitespace = true;
    this._nextToken = new StringBuilder(100);
    this.NextTokenType = HtmlTokenType.Text;
    this.GetNextCharacter();
  }

  internal void GetNextContentToken()
  {
    this._nextToken.Length = 0;
    if (this.IsAtEndOfStream)
      this.NextTokenType = HtmlTokenType.Eof;
    else if (this.IsAtTagStart)
    {
      this.GetNextCharacter();
      if (this.NextCharacter == '/')
      {
        this._nextToken.Append("</");
        this.NextTokenType = HtmlTokenType.ClosingTagStart;
        this.GetNextCharacter();
        this._ignoreNextWhitespace = false;
      }
      else
      {
        this.NextTokenType = HtmlTokenType.OpeningTagStart;
        this._nextToken.Append("<");
        this._ignoreNextWhitespace = true;
      }
    }
    else if (this.IsAtDirectiveStart)
    {
      this.GetNextCharacter();
      if (this._lookAheadCharacter == '[')
        this.ReadDynamicContent();
      else if (this._lookAheadCharacter == '-')
        this.ReadComment();
      else
        this.ReadUnknownDirective();
    }
    else
    {
      this.NextTokenType = HtmlTokenType.Text;
      while (!this.IsAtTagStart && !this.IsAtEndOfStream && !this.IsAtDirectiveStart)
      {
        if (this.NextCharacter == '<' && !this.IsNextCharacterEntity && this._lookAheadCharacter == '?')
        {
          this.SkipProcessingDirective();
        }
        else
        {
          if (this.NextCharacter <= ' ')
          {
            if (!this._ignoreNextWhitespace)
              this._nextToken.Append(' ');
            this._ignoreNextWhitespace = true;
          }
          else
          {
            this._nextToken.Append(this.NextCharacter);
            this._ignoreNextWhitespace = false;
          }
          this.GetNextCharacter();
        }
      }
    }
  }

  internal void GetNextTagToken()
  {
    this._nextToken.Length = 0;
    if (this.IsAtEndOfStream)
    {
      this.NextTokenType = HtmlTokenType.Eof;
    }
    else
    {
      this.SkipWhiteSpace();
      if (this.NextCharacter == '>' && !this.IsNextCharacterEntity)
      {
        this.NextTokenType = HtmlTokenType.TagEnd;
        this._nextToken.Append('>');
        this.GetNextCharacter();
      }
      else if (this.NextCharacter == '/' && this._lookAheadCharacter == '>')
      {
        this.NextTokenType = HtmlTokenType.EmptyTagEnd;
        this._nextToken.Append("/>");
        this.GetNextCharacter();
        this.GetNextCharacter();
        this._ignoreNextWhitespace = false;
      }
      else if (this.IsGoodForNameStart(this.NextCharacter))
      {
        this.NextTokenType = HtmlTokenType.Name;
        while (this.IsGoodForName(this.NextCharacter) && !this.IsAtEndOfStream)
        {
          this._nextToken.Append(this.NextCharacter);
          this.GetNextCharacter();
        }
      }
      else
      {
        this.NextTokenType = HtmlTokenType.Atom;
        this._nextToken.Append(this.NextCharacter);
        this.GetNextCharacter();
      }
    }
  }

  internal void GetNextEqualSignToken()
  {
    this._nextToken.Length = 0;
    this._nextToken.Append('=');
    this.NextTokenType = HtmlTokenType.EqualSign;
    this.SkipWhiteSpace();
    if (this.NextCharacter != '=')
      return;
    this.GetNextCharacter();
  }

  internal void GetNextAtomToken()
  {
    this._nextToken.Length = 0;
    this.SkipWhiteSpace();
    this.NextTokenType = HtmlTokenType.Atom;
    if ((this.NextCharacter == '\'' || this.NextCharacter == '"') && !this.IsNextCharacterEntity)
    {
      char nextCharacter = this.NextCharacter;
      this.GetNextCharacter();
      while (((int) this.NextCharacter != (int) nextCharacter || this.IsNextCharacterEntity) && !this.IsAtEndOfStream)
      {
        this._nextToken.Append(this.NextCharacter);
        this.GetNextCharacter();
      }
      if ((int) this.NextCharacter != (int) nextCharacter)
        return;
      this.GetNextCharacter();
    }
    else
    {
      while (!this.IsAtEndOfStream && !char.IsWhiteSpace(this.NextCharacter) && this.NextCharacter != '>')
      {
        this._nextToken.Append(this.NextCharacter);
        this.GetNextCharacter();
      }
    }
  }

  internal HtmlTokenType NextTokenType { get; private set; }

  internal string NextToken => this._nextToken.ToString();

  private void GetNextCharacter()
  {
    if (this._nextCharacterCode == -1)
      throw new InvalidOperationException("GetNextCharacter method called at the end of a stream");
    this._previousCharacter = this.NextCharacter;
    this.NextCharacter = this._lookAheadCharacter;
    this._nextCharacterCode = this._lookAheadCharacterCode;
    this.IsNextCharacterEntity = false;
    this.ReadLookAheadCharacter();
    if (this.NextCharacter != '&')
      return;
    if (this._lookAheadCharacter == '#')
    {
      int num = 0;
      this.ReadLookAheadCharacter();
      for (int index = 0; index < 7 && char.IsDigit(this._lookAheadCharacter); ++index)
      {
        num = 10 * num + (this._lookAheadCharacterCode - 48 /*0x30*/);
        this.ReadLookAheadCharacter();
      }
      if (this._lookAheadCharacter == ';')
      {
        this.ReadLookAheadCharacter();
        this._nextCharacterCode = num;
        this.NextCharacter = (char) this._nextCharacterCode;
        this.IsNextCharacterEntity = true;
      }
      else
      {
        this.NextCharacter = this._lookAheadCharacter;
        this._nextCharacterCode = this._lookAheadCharacterCode;
        this.ReadLookAheadCharacter();
        this.IsNextCharacterEntity = false;
      }
    }
    else if (char.IsLetter(this._lookAheadCharacter))
    {
      string entityName = "";
      for (int index = 0; index < 10 && (char.IsLetter(this._lookAheadCharacter) || char.IsDigit(this._lookAheadCharacter)); ++index)
      {
        entityName += this._lookAheadCharacter.ToString();
        this.ReadLookAheadCharacter();
      }
      if (this._lookAheadCharacter == ';')
      {
        this.ReadLookAheadCharacter();
        if (HtmlSchema.IsEntity(entityName))
        {
          this.NextCharacter = HtmlSchema.EntityCharacterValue(entityName);
          this._nextCharacterCode = (int) this.NextCharacter;
          this.IsNextCharacterEntity = true;
        }
        else
        {
          this.NextCharacter = this._lookAheadCharacter;
          this._nextCharacterCode = this._lookAheadCharacterCode;
          this.ReadLookAheadCharacter();
          this.IsNextCharacterEntity = false;
        }
      }
      else
      {
        this.NextCharacter = this._lookAheadCharacter;
        this.ReadLookAheadCharacter();
        this.IsNextCharacterEntity = false;
      }
    }
  }

  private void ReadLookAheadCharacter()
  {
    if (this._lookAheadCharacterCode == -1)
      return;
    this._lookAheadCharacterCode = this._inputStringReader.Read();
    this._lookAheadCharacter = (char) this._lookAheadCharacterCode;
  }

  private void SkipWhiteSpace()
  {
    while (true)
    {
      if (this.NextCharacter == '<' && (this._lookAheadCharacter == '?' || this._lookAheadCharacter == '!'))
      {
        this.GetNextCharacter();
        if (this._lookAheadCharacter == '[')
        {
          while (!this.IsAtEndOfStream && (this._previousCharacter != ']' || this.NextCharacter != ']' || this._lookAheadCharacter != '>'))
            this.GetNextCharacter();
          if (this.NextCharacter == '>')
            this.GetNextCharacter();
        }
        else
        {
          while (!this.IsAtEndOfStream && this.NextCharacter != '>')
            this.GetNextCharacter();
          if (this.NextCharacter == '>')
            this.GetNextCharacter();
        }
      }
      if (char.IsWhiteSpace(this.NextCharacter))
        this.GetNextCharacter();
      else
        break;
    }
  }

  private bool IsGoodForNameStart(char character) => character == '_' || char.IsLetter(character);

  private bool IsGoodForName(char character)
  {
    return this.IsGoodForNameStart(character) || character == '.' || character == '-' || character == ':' || char.IsDigit(character) || this.IsCombiningCharacter(character) || this.IsExtender(character);
  }

  private bool IsCombiningCharacter(char character) => false;

  private bool IsExtender(char character) => false;

  private void ReadDynamicContent()
  {
    this.NextTokenType = HtmlTokenType.Text;
    this._nextToken.Length = 0;
    this.GetNextCharacter();
    this.GetNextCharacter();
    while ((this.NextCharacter != ']' || this._lookAheadCharacter != '>') && !this.IsAtEndOfStream)
      this.GetNextCharacter();
    if (this.IsAtEndOfStream)
      return;
    this.GetNextCharacter();
    this.GetNextCharacter();
  }

  private void ReadComment()
  {
    this.NextTokenType = HtmlTokenType.Comment;
    this._nextToken.Length = 0;
    this.GetNextCharacter();
    this.GetNextCharacter();
    this.GetNextCharacter();
    while (true)
    {
      while (!this.IsAtEndOfStream && (this.NextCharacter != '-' || this._lookAheadCharacter != '-') && (this.NextCharacter != '!' || this._lookAheadCharacter != '>'))
      {
        this._nextToken.Append(this.NextCharacter);
        this.GetNextCharacter();
      }
      this.GetNextCharacter();
      if (this._previousCharacter != '-' || this.NextCharacter != '-' || this._lookAheadCharacter != '>')
      {
        if (this._previousCharacter != '!' || this.NextCharacter != '>')
          this._nextToken.Append(this._previousCharacter);
        else
          goto label_9;
      }
      else
        break;
    }
    this.GetNextCharacter();
label_9:
    if (this.NextCharacter != '>')
      return;
    this.GetNextCharacter();
  }

  private void ReadUnknownDirective()
  {
    this.NextTokenType = HtmlTokenType.Text;
    this._nextToken.Length = 0;
    this.GetNextCharacter();
    while ((this.NextCharacter != '>' || this.IsNextCharacterEntity) && !this.IsAtEndOfStream)
      this.GetNextCharacter();
    if (this.IsAtEndOfStream)
      return;
    this.GetNextCharacter();
  }

  private void SkipProcessingDirective()
  {
    this.GetNextCharacter();
    this.GetNextCharacter();
    while ((this.NextCharacter != '?' && this.NextCharacter != '/' || this._lookAheadCharacter != '>') && !this.IsAtEndOfStream)
      this.GetNextCharacter();
    if (this.IsAtEndOfStream)
      return;
    this.GetNextCharacter();
    this.GetNextCharacter();
  }

  private char NextCharacter { get; set; }

  private bool IsAtEndOfStream => this._nextCharacterCode == -1;

  private bool IsAtTagStart
  {
    get
    {
      return this.NextCharacter == '<' && (this._lookAheadCharacter == '/' || this.IsGoodForNameStart(this._lookAheadCharacter)) && !this.IsNextCharacterEntity;
    }
  }

  private bool IsAtTagEnd
  {
    get
    {
      return (this.NextCharacter == '>' || this.NextCharacter == '/' && this._lookAheadCharacter == '>') && !this.IsNextCharacterEntity;
    }
  }

  private bool IsAtDirectiveStart
  {
    get
    {
      return this.NextCharacter == '<' && this._lookAheadCharacter == '!' && !this.IsNextCharacterEntity;
    }
  }

  private bool IsNextCharacterEntity { get; set; }
}
