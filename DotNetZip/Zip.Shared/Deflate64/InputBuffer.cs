// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.Deflate64.InputBuffer
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;

#nullable disable
namespace Ionic.Zip.Deflate64;

internal sealed class InputBuffer
{
  private byte[] _buffer;
  private int _start;
  private int _end;
  private uint _bitBuffer;
  private int _bitsInBuffer;

  public int AvailableBits => this._bitsInBuffer;

  public int AvailableBytes => this._end - this._start + this._bitsInBuffer / 8;

  public bool EnsureBitsAvailable(int count)
  {
    if (this._bitsInBuffer < count)
    {
      if (this.NeedsInput())
        return false;
      this._bitBuffer |= (uint) this._buffer[this._start++] << this._bitsInBuffer;
      this._bitsInBuffer += 8;
      if (this._bitsInBuffer < count)
      {
        if (this.NeedsInput())
          return false;
        this._bitBuffer |= (uint) this._buffer[this._start++] << this._bitsInBuffer;
        this._bitsInBuffer += 8;
      }
    }
    return true;
  }

  public uint TryLoad16Bits()
  {
    if (this._bitsInBuffer < 8)
    {
      if (this._start < this._end)
      {
        this._bitBuffer |= (uint) this._buffer[this._start++] << this._bitsInBuffer;
        this._bitsInBuffer += 8;
      }
      if (this._start < this._end)
      {
        this._bitBuffer |= (uint) this._buffer[this._start++] << this._bitsInBuffer;
        this._bitsInBuffer += 8;
      }
    }
    else if (this._bitsInBuffer < 16 /*0x10*/ && this._start < this._end)
    {
      this._bitBuffer |= (uint) this._buffer[this._start++] << this._bitsInBuffer;
      this._bitsInBuffer += 8;
    }
    return this._bitBuffer;
  }

  private uint GetBitMask(int count) => (uint) ((1 << count) - 1);

  public int GetBits(int count)
  {
    if (!this.EnsureBitsAvailable(count))
      return -1;
    int bits = (int) this._bitBuffer & (int) this.GetBitMask(count);
    this._bitBuffer >>= count;
    this._bitsInBuffer -= count;
    return bits;
  }

  public int CopyTo(byte[] output, int offset, int length)
  {
    int num1 = 0;
    while (this._bitsInBuffer > 0 && length > 0)
    {
      output[offset++] = (byte) this._bitBuffer;
      this._bitBuffer >>= 8;
      this._bitsInBuffer -= 8;
      --length;
      ++num1;
    }
    if (length == 0)
      return num1;
    int num2 = this._end - this._start;
    if (length > num2)
      length = num2;
    Array.Copy((Array) this._buffer, this._start, (Array) output, offset, length);
    this._start += length;
    return num1 + length;
  }

  public bool NeedsInput() => this._start == this._end;

  public void SetInput(byte[] buffer, int offset, int length)
  {
    if (this._start != this._end)
      return;
    this._buffer = buffer;
    this._start = offset;
    this._end = offset + length;
  }

  public void SkipBits(int n)
  {
    this._bitBuffer >>= n;
    this._bitsInBuffer -= n;
  }

  public void SkipToByteBoundary()
  {
    this._bitBuffer >>= this._bitsInBuffer % 8;
    this._bitsInBuffer -= this._bitsInBuffer % 8;
  }
}
