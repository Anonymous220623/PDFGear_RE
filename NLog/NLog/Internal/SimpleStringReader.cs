// Decompiled with JetBrains decompiler
// Type: NLog.Internal.SimpleStringReader
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Internal;

internal class SimpleStringReader
{
  private readonly string _text;

  public SimpleStringReader(string text)
  {
    this._text = text;
    this.Position = 0;
  }

  internal int Position { get; set; }

  internal string Text => this._text;

  internal int Peek() => this.Position < this._text.Length ? (int) this._text[this.Position] : -1;

  internal int Read() => this.Position < this._text.Length ? (int) this._text[this.Position++] : -1;

  internal string Substring(int startIndex, int endIndex)
  {
    return this._text.Substring(startIndex, endIndex - startIndex);
  }

  internal string ReadUntilMatch(Func<int, bool> charFinder)
  {
    int position = this.Position;
    int num;
    while ((num = this.Peek()) != -1 && !charFinder(num))
      this.Read();
    return this.Substring(position, this.Position);
  }
}
