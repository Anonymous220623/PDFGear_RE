// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BufferedBlockPadding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class BufferedBlockPadding : BufferedCipher
{
  private IPadding m_padding;

  internal BufferedBlockPadding(ICipher cipher, IPadding padding)
  {
    this.m_cipher = cipher;
    this.m_padding = padding;
    this.m_bytes = new byte[cipher.BlockSize];
    this.m_offset = 0;
  }

  internal BufferedBlockPadding(ICipher cipher)
    : this(cipher, (IPadding) new Pkcs7Padding())
  {
  }

  public override void Initialize(bool isEncryption, ICipherParam parameters)
  {
    this.m_isEncryption = isEncryption;
    SecureRandomAlgorithm random = (SecureRandomAlgorithm) null;
    this.Reset();
    this.m_padding.Initialize(random);
    this.m_cipher.Initialize(isEncryption, parameters);
  }

  public override int GetOutputSize(int length)
  {
    int num1 = length + this.m_offset;
    int num2 = num1 % this.m_bytes.Length;
    if (num2 != 0)
      return num1 - num2 + this.m_bytes.Length;
    return this.m_isEncryption ? num1 + this.m_bytes.Length : num1;
  }

  public override int GetUpdateOutputSize(int length)
  {
    int num1 = length + this.m_offset;
    int num2 = num1 % this.m_bytes.Length;
    return num2 == 0 ? num1 - this.m_bytes.Length : num1 - num2;
  }

  public override int ProcessByte(byte input, byte[] output, int outOff)
  {
    int num = 0;
    if (this.m_offset == this.m_bytes.Length)
    {
      num = this.m_cipher.ProcessBlock(this.m_bytes, 0, output, outOff);
      this.m_offset = 0;
    }
    this.m_bytes[this.m_offset++] = (byte) (int) input;
    return num;
  }

  public override int ProcessBytes(
    byte[] input,
    int inOffset,
    int length,
    byte[] output,
    int outOffset)
  {
    if (length < 0)
      throw new ArgumentException("Invalid length");
    int blockSize = this.BlockSize;
    int updateOutputSize = this.GetUpdateOutputSize(length);
    if (updateOutputSize > 0 && outOffset + updateOutputSize > output.Length)
      throw new Exception("Invalid buffer length");
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
    return num;
  }

  public override int DoFinal(byte[] output, int outOff)
  {
    int blockSize = this.m_cipher.BlockSize;
    int num = 0;
    int length;
    if (this.m_isEncryption)
    {
      if (this.m_offset == blockSize)
      {
        if (outOff + 2 * blockSize > output.Length)
        {
          this.Reset();
          throw new Exception("output buffer too short");
        }
        num = this.m_cipher.ProcessBlock(this.m_bytes, 0, output, outOff);
        this.m_offset = 0;
      }
      this.m_padding.AddPadding(this.m_bytes, this.m_offset);
      length = num + this.m_cipher.ProcessBlock(this.m_bytes, 0, output, outOff + num);
      this.Reset();
    }
    else if (this.m_offset == blockSize)
    {
      length = this.m_cipher.ProcessBlock(this.m_bytes, 0, this.m_bytes, 0);
      this.m_offset = 0;
      try
      {
        length -= this.m_padding.Count(this.m_bytes);
        Array.Copy((Array) this.m_bytes, 0, (Array) output, outOff, length);
      }
      finally
      {
        this.Reset();
      }
    }
    else
    {
      this.Reset();
      throw new Exception("incomplete in decryption");
    }
    return length;
  }
}
