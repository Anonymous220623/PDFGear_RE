// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.FixAsciiControlsReader
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System;
using System.IO;

#nullable disable
namespace XmpCore.Impl;

public class FixAsciiControlsReader(StreamReader reader) : PushbackReader(reader, 8)
{
  private const int StateStart = 0;
  private const int StateAmp = 1;
  private const int StateHash = 2;
  private const int StateHex = 3;
  private const int StateDig1 = 4;
  private const int StateError = 5;
  private const int BufferSize = 8;
  private int _state;
  private int _control;
  private int _digits;

  public override int Read(char[] buffer, int off, int len)
  {
    int index = 0;
    int num1 = 0;
    int num2 = off;
    char[] buffer1 = new char[8];
    bool flag = true;
    while (flag && num1 < len)
    {
      flag = base.Read(buffer1, index, 1) == 1;
      if (flag)
      {
        char c = this.ProcessChar(buffer1[index]);
        switch (this._state)
        {
          case 0:
            if (Utils.IsControlChar(c))
              c = ' ';
            buffer[num2++] = c;
            index = 0;
            ++num1;
            continue;
          case 5:
            this.Unread(buffer1, 0, index + 1);
            index = 0;
            continue;
          default:
            ++index;
            continue;
        }
      }
      else if (index > 0)
      {
        this.Unread(buffer1, 0, index);
        this._state = 5;
        index = 0;
        flag = true;
      }
    }
    return !(num1 > 0 | flag) ? -1 : num1;
  }

  private char ProcessChar(char ch)
  {
    switch (this._state)
    {
      case 0:
        if (ch == '&')
          this._state = 1;
        return ch;
      case 1:
        this._state = ch == '#' ? 2 : 5;
        return ch;
      case 2:
        if (ch == 'x')
        {
          this._control = 0;
          this._digits = 0;
          this._state = 3;
        }
        else if ('0' <= ch && ch <= '9')
        {
          this._control = (int) ch - 48 /*0x30*/;
          this._digits = 1;
          this._state = 4;
        }
        else
          this._state = 5;
        return ch;
      case 3:
        if ('0' <= ch && ch <= '9' || 'a' <= ch && ch <= 'f' || 'A' <= ch && ch <= 'F')
        {
          this._control = this._control * 16 /*0x10*/ + Convert.ToInt32(ch.ToString(), 16 /*0x10*/);
          ++this._digits;
          this._state = this._digits <= 4 ? 3 : 5;
        }
        else
        {
          if (ch == ';' && Utils.IsControlChar((char) this._control))
          {
            this._state = 0;
            return (char) this._control;
          }
          this._state = 5;
        }
        return ch;
      case 4:
        if ('0' <= ch && ch <= '9')
        {
          this._control = this._control * 10 + ((int) ch - 48 /*0x30*/);
          ++this._digits;
          this._state = this._digits <= 5 ? 4 : 5;
        }
        else
        {
          if (ch == ';' && Utils.IsControlChar((char) this._control))
          {
            this._state = 0;
            return (char) this._control;
          }
          this._state = 5;
        }
        return ch;
      case 5:
        this._state = 0;
        return ch;
      default:
        return ch;
    }
  }
}
