// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.WinZipAesCipherStream
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

#nullable disable
namespace Ionic.Zip;

internal class WinZipAesCipherStream : Stream
{
  private WinZipAesCrypto _params;
  private Stream _s;
  private CryptoMode _mode;
  private int _nonce;
  private bool _finalBlock;
  internal HMACSHA1 _mac;
  internal Aes _aesCipher;
  internal ICryptoTransform _xform;
  private const int BLOCK_SIZE_IN_BYTES = 16 /*0x10*/;
  private byte[] counter = new byte[16 /*0x10*/];
  private byte[] counterOut = new byte[16 /*0x10*/];
  private long _length;
  private long _totalBytesXferred;
  private byte[] _PendingWriteBlock;
  private int _pendingCount;
  private byte[] _iobuf;
  private object _outputLock = new object();

  internal WinZipAesCipherStream(
    Stream s,
    WinZipAesCrypto cryptoParams,
    long length,
    CryptoMode mode)
    : this(s, cryptoParams, mode)
  {
    if (s == null)
      throw new ArgumentNullException(nameof (s));
    this._length = length;
  }

  internal WinZipAesCipherStream(Stream s, WinZipAesCrypto cryptoParams, CryptoMode mode)
  {
    this._params = cryptoParams;
    this._s = s;
    this._mode = mode;
    this._nonce = 1;
    if (this._params == null)
      throw new BadPasswordException("Supply a password to use AES encryption.");
    int num = this._params.KeyBytes.Length * 8;
    switch (num)
    {
      case 128 /*0x80*/:
      case 192 /*0xC0*/:
      case 256 /*0x0100*/:
        this._mac = new HMACSHA1(this._params.MacIv);
        this._aesCipher = Aes.Create();
        this._aesCipher.BlockSize = 128 /*0x80*/;
        this._aesCipher.KeySize = num;
        this._aesCipher.Mode = CipherMode.ECB;
        this._aesCipher.Padding = PaddingMode.None;
        this._xform = this._aesCipher.CreateEncryptor(this._params.KeyBytes, new byte[16 /*0x10*/]);
        if (this._mode != CryptoMode.Encrypt)
          break;
        this._iobuf = new byte[2048 /*0x0800*/];
        this._PendingWriteBlock = new byte[16 /*0x10*/];
        break;
      default:
        throw new ArgumentOutOfRangeException("keysize", "size of key must be 128, 192, or 256");
    }
  }

  private void XorInPlace(byte[] buffer, int offset, int count)
  {
    for (int index = 0; index < count; ++index)
      buffer[offset + index] = (byte) ((uint) this.counterOut[index] ^ (uint) buffer[offset + index]);
  }

  private void WriteTransformOneBlock(byte[] buffer, int offset)
  {
    Array.Copy((Array) BitConverter.GetBytes(this._nonce++), 0, (Array) this.counter, 0, 4);
    this._xform.TransformBlock(this.counter, 0, 16 /*0x10*/, this.counterOut, 0);
    this.XorInPlace(buffer, offset, 16 /*0x10*/);
    this._mac.TransformBlock(buffer, offset, 16 /*0x10*/, (byte[]) null, 0);
  }

  private void WriteTransformBlocks(byte[] buffer, int offset, int count)
  {
    int offset1 = offset;
    for (int index = count + offset; offset1 < buffer.Length && offset1 < index; offset1 += 16 /*0x10*/)
      this.WriteTransformOneBlock(buffer, offset1);
  }

  private void WriteTransformFinalBlock()
  {
    if (this._pendingCount == 0)
      throw new InvalidOperationException("No bytes available.");
    if (this._finalBlock)
      throw new InvalidOperationException("The final block has already been transformed.");
    Array.Copy((Array) BitConverter.GetBytes(this._nonce++), 0, (Array) this.counter, 0, 4);
    this.counterOut = this._xform.TransformFinalBlock(this.counter, 0, 16 /*0x10*/);
    this.XorInPlace(this._PendingWriteBlock, 0, this._pendingCount);
    this._mac.TransformFinalBlock(this._PendingWriteBlock, 0, this._pendingCount);
    this._finalBlock = true;
  }

  private int ReadTransformOneBlock(byte[] buffer, int offset, int last)
  {
    if (this._finalBlock)
      throw new NotSupportedException();
    int num1 = last - offset;
    int num2 = num1 > 16 /*0x10*/ ? 16 /*0x10*/ : num1;
    Array.Copy((Array) BitConverter.GetBytes(this._nonce++), 0, (Array) this.counter, 0, 4);
    if (num2 == num1 && this._length > 0L && this._totalBytesXferred + (long) last == this._length)
    {
      this._mac.TransformFinalBlock(buffer, offset, num2);
      this.counterOut = this._xform.TransformFinalBlock(this.counter, 0, 16 /*0x10*/);
      this._finalBlock = true;
    }
    else
    {
      this._mac.TransformBlock(buffer, offset, num2, (byte[]) null, 0);
      this._xform.TransformBlock(this.counter, 0, 16 /*0x10*/, this.counterOut, 0);
    }
    this.XorInPlace(buffer, offset, num2);
    return num2;
  }

  private void ReadTransformBlocks(byte[] buffer, int offset, int count)
  {
    int offset1 = offset;
    int num;
    for (int last = count + offset; offset1 < buffer.Length && offset1 < last; offset1 += num)
      num = this.ReadTransformOneBlock(buffer, offset1, last);
  }

  public override int Read(byte[] buffer, int offset, int count)
  {
    if (this._mode == CryptoMode.Encrypt)
      throw new NotSupportedException();
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset), "Must not be less than zero.");
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count), "Must not be less than zero.");
    if (buffer.Length < offset + count)
      throw new ArgumentException("The buffer is too small");
    int count1 = count;
    if (this._totalBytesXferred >= this._length)
      return 0;
    long num1 = this._length - this._totalBytesXferred;
    if (num1 < (long) count)
      count1 = (int) num1;
    int num2 = this._s.Read(buffer, offset, count1);
    this.ReadTransformBlocks(buffer, offset, count1);
    this._totalBytesXferred += (long) num2;
    return num2;
  }

  public byte[] FinalAuthentication
  {
    get
    {
      if (!this._finalBlock)
      {
        if (this._totalBytesXferred != 0L)
          throw new BadStateException("The final hash has not been computed.");
        this._mac.ComputeHash(new byte[0]);
      }
      byte[] destinationArray = new byte[10];
      Array.Copy((Array) this._mac.Hash, 0, (Array) destinationArray, 0, 10);
      return destinationArray;
    }
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    if (this._finalBlock)
      throw new InvalidOperationException("The final block has already been transformed.");
    if (this._mode == CryptoMode.Decrypt)
      throw new NotSupportedException();
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    if (offset < 0)
      throw new ArgumentOutOfRangeException(nameof (offset), "Must not be less than zero.");
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count), "Must not be less than zero.");
    if (buffer.Length < offset + count)
      throw new ArgumentException("The offset and count are too large");
    if (count == 0)
      return;
    if (count + this._pendingCount <= 16 /*0x10*/)
    {
      Buffer.BlockCopy((Array) buffer, offset, (Array) this._PendingWriteBlock, this._pendingCount, count);
      this._pendingCount += count;
    }
    else
    {
      int num1 = count;
      int srcOffset = offset;
      if (this._pendingCount != 0)
      {
        int count1 = 16 /*0x10*/ - this._pendingCount;
        if (count1 > 0)
        {
          Buffer.BlockCopy((Array) buffer, offset, (Array) this._PendingWriteBlock, this._pendingCount, count1);
          num1 -= count1;
          srcOffset += count1;
        }
        this.WriteTransformOneBlock(this._PendingWriteBlock, 0);
        this._s.Write(this._PendingWriteBlock, 0, 16 /*0x10*/);
        this._totalBytesXferred += 16L /*0x10*/;
        this._pendingCount = 0;
      }
      int num2 = (num1 - 1) / 16 /*0x10*/;
      this._pendingCount = num1 - num2 * 16 /*0x10*/;
      Buffer.BlockCopy((Array) buffer, srcOffset + num1 - this._pendingCount, (Array) this._PendingWriteBlock, 0, this._pendingCount);
      int num3 = num1 - this._pendingCount;
      this._totalBytesXferred += (long) num3;
      if (num2 <= 0)
        return;
      do
      {
        int count2 = this._iobuf.Length;
        if (count2 > num3)
          count2 = num3;
        Buffer.BlockCopy((Array) buffer, srcOffset, (Array) this._iobuf, 0, count2);
        this.WriteTransformBlocks(this._iobuf, 0, count2);
        this._s.Write(this._iobuf, 0, count2);
        num3 -= count2;
        srcOffset += count2;
      }
      while (num3 > 0);
    }
  }

  public override void Close()
  {
    if (this._pendingCount > 0)
    {
      this.WriteTransformFinalBlock();
      this._s.Write(this._PendingWriteBlock, 0, this._pendingCount);
      this._totalBytesXferred += (long) this._pendingCount;
      this._pendingCount = 0;
    }
    this._s.Close();
    this._xform.Dispose();
    this._aesCipher.Dispose();
  }

  public override bool CanRead => this._mode == CryptoMode.Decrypt;

  public override bool CanSeek => false;

  public override bool CanWrite => this._mode == CryptoMode.Encrypt;

  public override void Flush() => this._s.Flush();

  public override long Length => throw new NotImplementedException();

  public override long Position
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

  public override void SetLength(long value) => throw new NotImplementedException();

  [Conditional("Trace")]
  private void TraceOutput(string format, params object[] varParams)
  {
    lock (this._outputLock)
    {
      int hashCode = Thread.CurrentThread.GetHashCode();
      Console.ForegroundColor = (ConsoleColor) (hashCode % 8 + 8);
      Console.Write("{0:000} WZACS ", (object) hashCode);
      Console.WriteLine(format, varParams);
      Console.ResetColor();
    }
  }
}
