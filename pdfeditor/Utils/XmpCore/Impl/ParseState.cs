// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.ParseState
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace XmpCore.Impl;

internal sealed class ParseState
{
  private readonly string _str;

  public int Pos { get; private set; }

  public ParseState(string str) => this._str = str;

  public bool HasNext => this.Pos < this._str.Length;

  public char Ch(int index) => index >= this._str.Length ? char.MinValue : this._str[index];

  public char Ch() => this.Pos >= this._str.Length ? char.MinValue : this._str[this.Pos];

  public void Skip() => ++this.Pos;

  public int GatherInt(string errorMsg, int maxValue)
  {
    int num = 0;
    bool flag = false;
    for (char ch = this.Ch(this.Pos); '0' <= ch && ch <= '9'; ch = this.Ch(this.Pos))
    {
      num = num * 10 + ((int) ch - 48 /*0x30*/);
      flag = true;
      ++this.Pos;
    }
    if (!flag)
      throw new XmpException(errorMsg, XmpErrorCode.BadValue);
    if (num > maxValue)
      return maxValue;
    return num >= 0 ? num : 0;
  }
}
