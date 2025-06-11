// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPostScriptReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontPostScriptReader : SystemFontReaderBase, SystemFontIPostScriptReader
{
  private readonly SystemFontEncryptionCollection encryption;
  private readonly Queue<byte> decryptedBuffer;

  public string Result { get; set; }

  public override long Position => base.Position - (long) this.decryptedBuffer.Count;

  public SystemFontPostScriptReader(byte[] data)
    : base(data)
  {
    this.encryption = new SystemFontEncryptionCollection();
    this.decryptedBuffer = new Queue<byte>();
  }

  public SystemFontToken ReadToken()
  {
    this.SkipUnusedCharacters();
    if (this.EndOfFile)
      return SystemFontToken.Unknown;
    char ch = (char) this.Peek(0);
    if (ch <= '/')
    {
      if (ch == '(')
        return this.ReadLiteralString();
      if (ch == '/')
        return this.ReadName();
      int num = (int) this.Read();
      return SystemFontToken.Unknown;
    }
    if (ch == '>')
      return this.ReadHexadecimalString();
    switch ((int) ch - 91)
    {
      case 0:
label_27:
        int num1 = (int) this.Read();
        return SystemFontToken.ArrayStart;
      case 1:
        if (SystemFontCharacters.IsValidNumberChar((SystemFontIPostScriptReader) this))
          return this.ReadNumber();
        if (SystemFontCharacters.IsLetter((int) this.Peek(0)) || this.Peek(0) == (byte) 45 || this.Peek(0) == (byte) 124)
          return this.ReadOperatorOrKeyword();
        int num2 = (int) this.Read();
        return SystemFontToken.Unknown;
      case 2:
        int num3 = (int) this.Read();
        return SystemFontToken.ArrayEnd;
      default:
        switch ((int) ch - 123)
        {
          case 0:
            goto label_27;
          case 1:
            if (SystemFontCharacters.IsValidNumberChar((SystemFontIPostScriptReader) this))
              return this.ReadNumber();
            if (SystemFontCharacters.IsLetter((int) this.Peek(0)) || this.Peek(0) == (byte) 45 || this.Peek(0) == (byte) 124)
              return this.ReadOperatorOrKeyword();
            int num4 = (int) this.Read();
            return SystemFontToken.Unknown;
          case 2:
            int num5 = (int) this.Read();
            return SystemFontToken.ArrayEnd;
          default:
            if (SystemFontCharacters.IsValidNumberChar((SystemFontIPostScriptReader) this))
              return this.ReadNumber();
            if (SystemFontCharacters.IsLetter((int) this.Peek(0)) || this.Peek(0) == (byte) 45 || this.Peek(0) == (byte) 124)
              return this.ReadOperatorOrKeyword();
            int num6 = (int) this.Read();
            return SystemFontToken.Unknown;
        }
    }
  }

  internal void SkipUnusedCharacters()
  {
    SystemFontPostScriptReaderHelper.SkipUnusedCharacters((SystemFontIPostScriptReader) this);
  }

  internal void GoToNextLine()
  {
    SystemFontPostScriptReaderHelper.GoToNextLine((SystemFontIPostScriptReader) this);
  }

  internal void SkipWhiteSpaces()
  {
    SystemFontPostScriptReaderHelper.SkipWhiteSpaces((SystemFontIPostScriptReader) this);
  }

  private SystemFontToken ReadOperatorOrKeyword()
  {
    this.Result = SystemFontPostScriptReaderHelper.ReadKeyword((SystemFontIPostScriptReader) this);
    switch (this.Result)
    {
      case "true":
      case "false":
        return SystemFontToken.Boolean;
      default:
        if (SystemFontKeywords.IsKeyword(this.Result))
          return SystemFontToken.Keyword;
        return SystemFontPostScriptOperator.IsOperator(this.Result) ? SystemFontToken.Operator : SystemFontToken.Unknown;
    }
  }

  private SystemFontToken ReadName()
  {
    this.Result = SystemFontPostScriptReaderHelper.ReadName((SystemFontIPostScriptReader) this);
    return SystemFontToken.Name;
  }

  private SystemFontToken ReadNumber()
  {
    this.Result = SystemFontPostScriptReaderHelper.ReadNumber((SystemFontIPostScriptReader) this);
    return !SystemFontEnumerable.Contains<char>((IEnumerable<char>) this.Result, '.') ? SystemFontToken.Integer : SystemFontToken.Real;
  }

  private SystemFontToken ReadHexadecimalString()
  {
    this.Result = SystemFontPostScriptReaderHelper.GetString(SystemFontPostScriptReaderHelper.ReadHexadecimalString((SystemFontIPostScriptReader) this));
    return SystemFontToken.String;
  }

  private SystemFontToken ReadLiteralString()
  {
    this.Result = SystemFontPostScriptReaderHelper.GetString(SystemFontPostScriptReaderHelper.ReadLiteralString((SystemFontIPostScriptReader) this));
    return SystemFontToken.String;
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
      return SystemFontEnumerable.ElementAt<byte>((IEnumerable<byte>) this.decryptedBuffer, skip);
    skip -= this.decryptedBuffer.Count;
    for (int index = 0; index <= skip; ++index)
    {
      num1 = this.encryption.Decrypt(base.Read());
      this.decryptedBuffer.Enqueue(num1);
    }
    return num1;
  }

  public void PushEncryption(SystemFontEncryptionBase encrypt)
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
