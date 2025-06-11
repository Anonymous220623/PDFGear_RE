// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BufferedCipher
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class BufferedCipher : BufferedBlockPaddingBase
{
  internal byte[] m_bytes;
  internal int m_offset;
  internal bool m_isEncryption;
  internal ICipher m_cipher;

  protected BufferedCipher()
  {
  }

  internal BufferedCipher(ICipher cipher)
  {
    this.m_cipher = cipher != null ? cipher : throw new ArgumentNullException(nameof (cipher));
    this.m_bytes = new byte[cipher.BlockSize];
    this.m_offset = 0;
  }

  public override string AlgorithmName => this.m_cipher.AlgorithmName;

  public override void Initialize(bool isEncryption, ICipherParam parameters)
  {
    this.m_isEncryption = isEncryption;
    this.Reset();
    this.m_cipher.Initialize(isEncryption, parameters);
  }

  public override int BlockSize => this.m_cipher.BlockSize;

  public override int GetUpdateOutputSize(int length)
  {
    int num1 = length + this.m_offset;
    int num2 = num1 % this.m_bytes.Length;
    return num1 - num2;
  }

  public override int GetOutputSize(int length) => length + this.m_offset;

  public override int ProcessByte(byte input, byte[] bytes, int offset)
  {
    this.m_bytes[this.m_offset++] = input;
    if (this.m_offset != this.m_bytes.Length)
      return 0;
    if (offset + this.m_bytes.Length > bytes.Length)
      throw new Exception("output buffer too short");
    this.m_offset = 0;
    return this.m_cipher.ProcessBlock(this.m_bytes, 0, bytes, offset);
  }

  public override byte[] ProcessByte(byte input)
  {
    int updateOutputSize = this.GetUpdateOutputSize(1);
    byte[] numArray = updateOutputSize > 0 ? new byte[updateOutputSize] : (byte[]) null;
    int length = this.ProcessByte(input, numArray, 0);
    if (updateOutputSize > 0 && length < updateOutputSize)
    {
      byte[] destinationArray = new byte[length];
      Array.Copy((Array) numArray, 0, (Array) destinationArray, 0, length);
      numArray = destinationArray;
    }
    return numArray;
  }

  public override byte[] ProcessBytes(byte[] input, int offset, int length)
  {
    if (input == null)
      throw new ArgumentNullException(nameof (input));
    if (length < 1)
      return (byte[]) null;
    int updateOutputSize = this.GetUpdateOutputSize(length);
    byte[] numArray = updateOutputSize > 0 ? new byte[updateOutputSize] : (byte[]) null;
    int length1 = this.ProcessBytes(input, offset, length, numArray, 0);
    if (updateOutputSize > 0 && length1 < updateOutputSize)
    {
      byte[] destinationArray = new byte[length1];
      Array.Copy((Array) numArray, 0, (Array) destinationArray, 0, length1);
      numArray = destinationArray;
    }
    return numArray;
  }

  public override int ProcessBytes(
    byte[] input,
    int inOffset,
    int length,
    byte[] output,
    int outOffset)
  {
    if (length < 1)
      return 0;
    int blockSize = this.BlockSize;
    this.GetUpdateOutputSize(length);
    int num = 0;
    int length1 = this.m_bytes.Length - this.m_offset;
    if (length > length1)
    {
      Array.Copy((Array) input, inOffset, (Array) this.m_bytes, this.m_offset, length1);
      num += this.m_cipher.ProcessBlock(this.m_bytes, 0, output, outOffset);
      this.m_offset = 0;
      length -= length1;
      inOffset += length1;
      while (length > this.m_bytes.Length)
      {
        num += this.m_cipher.ProcessBlock(input, inOffset, output, outOffset + num);
        length -= blockSize;
        inOffset += blockSize;
      }
    }
    Array.Copy((Array) input, inOffset, (Array) this.m_bytes, this.m_offset, length);
    this.m_offset += length;
    if (this.m_offset == this.m_bytes.Length)
    {
      num += this.m_cipher.ProcessBlock(this.m_bytes, 0, output, outOffset + num);
      this.m_offset = 0;
    }
    return num;
  }

  public override byte[] DoFinal()
  {
    byte[] numArray = BufferedBlockPaddingBase.EmptyBuffer;
    int outputSize = this.GetOutputSize(0);
    if (outputSize > 0)
    {
      numArray = new byte[outputSize];
      int length = this.DoFinal(numArray, 0);
      if (length < numArray.Length)
      {
        byte[] destinationArray = new byte[length];
        Array.Copy((Array) numArray, 0, (Array) destinationArray, 0, length);
        numArray = destinationArray;
      }
    }
    else
      this.Reset();
    return numArray;
  }

  public override byte[] DoFinal(byte[] input, int inOffset, int inLength)
  {
    if (input == null)
      throw new ArgumentNullException(nameof (input));
    int outputSize = this.GetOutputSize(inLength);
    byte[] numArray = BufferedBlockPaddingBase.EmptyBuffer;
    if (outputSize > 0)
    {
      numArray = new byte[outputSize];
      int outOff = inLength > 0 ? this.ProcessBytes(input, inOffset, inLength, numArray, 0) : 0;
      int length = outOff + this.DoFinal(numArray, outOff);
      if (length < numArray.Length)
      {
        byte[] destinationArray = new byte[length];
        Array.Copy((Array) numArray, 0, (Array) destinationArray, 0, length);
        numArray = destinationArray;
      }
    }
    else
      this.Reset();
    return numArray;
  }

  public override int DoFinal(byte[] bytes, int offset)
  {
    try
    {
      if (this.m_offset != 0)
      {
        this.m_cipher.ProcessBlock(this.m_bytes, 0, this.m_bytes, 0);
        Array.Copy((Array) this.m_bytes, 0, (Array) bytes, offset, this.m_offset);
      }
      return this.m_offset;
    }
    finally
    {
      this.Reset();
    }
  }

  public override void Reset()
  {
    Array.Clear((Array) this.m_bytes, 0, this.m_bytes.Length);
    this.m_offset = 0;
    this.m_cipher.Reset();
  }
}
