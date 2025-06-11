// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PostScriptParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class PostScriptParser : FontFileParser, IPostScriptParser
{
  private readonly EncryptionCollection encryption;
  private readonly Queue<byte> decryptedBuffer;

  public string Result { get; set; }

  public override long Position => base.Position - (long) this.decryptedBuffer.Count;

  public PostScriptParser(byte[] data)
    : base(data)
  {
    this.encryption = new EncryptionCollection();
    this.decryptedBuffer = new Queue<byte>();
  }

  public Token ReadToken()
  {
    this.SkipUnusedCharacters();
    if (this.EndOfFile)
      return Token.Unknown;
    char ch = (char) this.Peek(0);
    if (ch <= '/')
    {
      if (ch == '(')
        return this.ReadLiteralString();
      if (ch == '/')
        return this.ReadName();
      int num = (int) this.Read();
      return Token.Unknown;
    }
    if (ch == '>')
      return this.ReadHexadecimalString();
    switch ((int) ch - 91)
    {
      case 0:
label_27:
        int num1 = (int) this.Read();
        return Token.ArrayStart;
      case 1:
        if (Chars.IsValidNumberChar((IPostScriptParser) this))
          return this.ReadNumber();
        if (Chars.IsLetter((int) this.Peek(0)) || this.Peek(0) == (byte) 45 || this.Peek(0) == (byte) 124)
          return this.ReadOperatorOrKeyword();
        int num2 = (int) this.Read();
        return Token.Unknown;
      case 2:
        int num3 = (int) this.Read();
        return Token.ArrayEnd;
      default:
        switch ((int) ch - 123)
        {
          case 0:
            goto label_27;
          case 1:
            if (Chars.IsValidNumberChar((IPostScriptParser) this))
              return this.ReadNumber();
            if (Chars.IsLetter((int) this.Peek(0)) || this.Peek(0) == (byte) 45 || this.Peek(0) == (byte) 124)
              return this.ReadOperatorOrKeyword();
            int num4 = (int) this.Read();
            return Token.Unknown;
          case 2:
            int num5 = (int) this.Read();
            return Token.ArrayEnd;
          default:
            if (Chars.IsValidNumberChar((IPostScriptParser) this))
              return this.ReadNumber();
            if (Chars.IsLetter((int) this.Peek(0)) || this.Peek(0) == (byte) 45 || this.Peek(0) == (byte) 124)
              return this.ReadOperatorOrKeyword();
            int num6 = (int) this.Read();
            return Token.Unknown;
        }
    }
  }

  internal void SkipUnusedCharacters()
  {
    PostScriptParserHelper.SkipUnusedCharacters((IPostScriptParser) this);
  }

  internal void GoToNextLine() => PostScriptParserHelper.GoToNextLine((IPostScriptParser) this);

  internal void SkipWhiteSpaces()
  {
    PostScriptParserHelper.SkipWhiteSpaces((IPostScriptParser) this);
  }

  private Token ReadOperatorOrKeyword()
  {
    this.Result = PostScriptParserHelper.ReadKeyword((IPostScriptParser) this);
    switch (this.Result)
    {
      case "true":
      case "false":
        return Token.Boolean;
      default:
        if (PdfKeywords.IsKeyword(this.Result))
          return Token.Keyword;
        return PostScriptOperators.IsOperator(this.Result) ? Token.Operator : Token.Unknown;
    }
  }

  private Token ReadName()
  {
    this.Result = PostScriptParserHelper.ReadName((IPostScriptParser) this);
    return Token.Name;
  }

  private Token ReadNumber()
  {
    this.Result = PostScriptParserHelper.ReadNumber((IPostScriptParser) this);
    return !Countable.Contains<char>((IEnumerable<char>) this.Result, '.') ? Token.Integer : Token.Real;
  }

  private Token ReadHexadecimalString()
  {
    this.Result = PostScriptParserHelper.GetString(PostScriptParserHelper.ReadHexadecimalString((IPostScriptParser) this));
    return Token.String;
  }

  private Token ReadLiteralString()
  {
    this.Result = PostScriptParserHelper.GetString(PostScriptParserHelper.ReadLiteralString((IPostScriptParser) this));
    return Token.String;
  }

  public override void BeginReadingBlock() => base.BeginReadingBlock();

  public override void EndReadingBlock() => base.EndReadingBlock();

  public override void Seek(long offset, SeekOrigin origin) => base.Seek(offset, origin);

  public override byte Read()
  {
    if (!this.encryption.HasEncryption)
    {
      byte num1 = base.Read();
      if (num1 == (byte) 13)
      {
        if (this.Peek(0) == (byte) 10)
        {
          int num2 = (int) this.Read();
        }
        num1 = (byte) 10;
      }
      return num1;
    }
    return this.decryptedBuffer.Count > 0 ? this.decryptedBuffer.Dequeue() : this.encryption.Decrypt(base.Read());
  }

  public override byte Peek(int skip)
  {
    byte num1 = 0;
    if (!this.encryption.HasEncryption)
    {
      byte num2 = base.Peek(skip);
      if (num2 == (byte) 13)
        num2 = (byte) 10;
      return num2;
    }
    if (skip < this.decryptedBuffer.Count)
      return Countable.ElementAt<byte>((IEnumerable<byte>) this.decryptedBuffer, skip);
    skip -= this.decryptedBuffer.Count;
    for (int index = 0; index <= skip; ++index)
    {
      num1 = this.encryption.Decrypt(base.Read());
      this.decryptedBuffer.Enqueue(num1);
    }
    return num1;
  }

  public void PushEncryption(EncryptionStdHelper encrypt)
  {
    this.encryption.PushEncryption(encrypt);
  }

  public void PopEncryption()
  {
    this.encryption.PopEncryption();
    if (this.encryption.HasEncryption)
      return;
    this.decryptedBuffer.Clear();
  }
}
