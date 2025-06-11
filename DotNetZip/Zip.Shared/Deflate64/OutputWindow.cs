// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.Deflate64.OutputWindow
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;

#nullable disable
namespace Ionic.Zip.Deflate64;

internal sealed class OutputWindow
{
  private const int WindowSize = 262144 /*0x040000*/;
  private const int WindowMask = 262143 /*0x03FFFF*/;
  private readonly byte[] _window = new byte[262144 /*0x040000*/];
  private int _end;
  private int _bytesUsed;

  internal void ClearBytesUsed() => this._bytesUsed = 0;

  public void Write(byte b)
  {
    this._window[this._end++] = b;
    this._end &= 262143 /*0x03FFFF*/;
    ++this._bytesUsed;
  }

  public void WriteLengthDistance(int length, int distance)
  {
    this._bytesUsed += length;
    int sourceIndex = this._end - distance & 262143 /*0x03FFFF*/;
    int num1 = 262144 /*0x040000*/ - length;
    if (sourceIndex <= num1 && this._end < num1)
    {
      if (length <= distance)
      {
        Array.Copy((Array) this._window, sourceIndex, (Array) this._window, this._end, length);
        this._end += length;
      }
      else
      {
        while (length-- > 0)
          this._window[this._end++] = this._window[sourceIndex++];
      }
    }
    else
    {
      while (length-- > 0)
      {
        byte[] window1 = this._window;
        int index1 = this._end++;
        byte[] window2 = this._window;
        int index2 = sourceIndex;
        int num2 = index2 + 1;
        int num3 = (int) window2[index2];
        window1[index1] = (byte) num3;
        this._end &= 262143 /*0x03FFFF*/;
        sourceIndex = num2 & 262143 /*0x03FFFF*/;
      }
    }
  }

  public int CopyFrom(InputBuffer input, int length)
  {
    length = Math.Min(Math.Min(length, 262144 /*0x040000*/ - this._bytesUsed), input.AvailableBytes);
    int length1 = 262144 /*0x040000*/ - this._end;
    int num;
    if (length > length1)
    {
      num = input.CopyTo(this._window, this._end, length1);
      if (num == length1)
        num += input.CopyTo(this._window, 0, length - length1);
    }
    else
      num = input.CopyTo(this._window, this._end, length);
    this._end = this._end + num & 262143 /*0x03FFFF*/;
    this._bytesUsed += num;
    return num;
  }

  public int FreeBytes => 262144 /*0x040000*/ - this._bytesUsed;

  public int AvailableBytes => this._bytesUsed;

  public int CopyTo(byte[] output, int offset, int length)
  {
    int num1;
    if (length > this._bytesUsed)
    {
      num1 = this._end;
      length = this._bytesUsed;
    }
    else
      num1 = this._end - this._bytesUsed + length & 262143 /*0x03FFFF*/;
    int num2 = length;
    int length1 = length - num1;
    if (length1 > 0)
    {
      Array.Copy((Array) this._window, 262144 /*0x040000*/ - length1, (Array) output, offset, length1);
      offset += length1;
      length = num1;
    }
    Array.Copy((Array) this._window, num1 - length, (Array) output, offset, length);
    this._bytesUsed -= num2;
    return num2;
  }
}
