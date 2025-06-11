// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.AesEncryptor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class AesEncryptor
{
  private int c_blockSize = 16 /*0x10*/;
  private Aes m_aes;
  private byte[] m_cbcV = new byte[16 /*0x10*/];
  private byte[] m_nextBlockV = new byte[16 /*0x10*/];
  private int m_ivOff;
  private byte[] m_buf = new byte[16 /*0x10*/];
  private bool m_isEncryption;

  internal AesEncryptor(byte[] key, byte[] iv, bool isEncryption)
  {
    this.m_aes = key.Length != this.c_blockSize ? new Aes(Aes.KeySize.Bits256, key) : new Aes(Aes.KeySize.Bits128, key);
    Array.Copy((Array) iv, 0, (Array) this.m_buf, 0, iv.Length);
    Array.Copy((Array) iv, 0, (Array) this.m_cbcV, 0, iv.Length);
    if (isEncryption)
      this.m_ivOff = this.m_buf.Length;
    this.m_isEncryption = isEncryption;
  }

  internal void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
  {
    if (length < 0)
      throw new ArgumentException("input data length cannot be negative");
    int num1 = 0;
    int length1 = this.m_buf.Length - this.m_ivOff;
    if (length > length1)
    {
      Array.Copy((Array) input, inOff, (Array) this.m_buf, this.m_ivOff, length1);
      int num2 = num1 + this.ProcessBlock(this.m_buf, 0, output, outOff);
      this.m_ivOff = 0;
      length -= length1;
      inOff += length1;
      while (length > this.m_buf.Length)
      {
        num2 += this.ProcessBlock(input, inOff, output, outOff + num2);
        length -= this.c_blockSize;
        inOff += this.c_blockSize;
      }
    }
    Array.Copy((Array) input, inOff, (Array) this.m_buf, this.m_ivOff, length);
    this.m_ivOff += length;
  }

  internal int Finalize(byte[] output)
  {
    int num1 = 0;
    int outOff = 0;
    int num2;
    if (this.m_isEncryption)
    {
      if (this.m_ivOff == this.c_blockSize)
      {
        num1 = this.ProcessBlock(this.m_buf, 0, output, outOff);
        this.m_ivOff = 0;
      }
      AesEncryptor.AddPadding(this.m_buf, this.m_ivOff);
      num2 = num1 + this.ProcessBlock(this.m_buf, 0, output, outOff + num1);
    }
    else
    {
      if (this.m_ivOff == this.c_blockSize)
      {
        num1 = this.ProcessBlock(this.m_buf, 0, output, 0);
        this.m_ivOff = 0;
      }
      num2 = num1 - AesEncryptor.CheckPadding(output);
    }
    return num2;
  }

  internal int GetBlockSize(int length)
  {
    int num1 = length + this.m_ivOff;
    int num2 = num1 % this.m_buf.Length;
    return num2 == 0 ? num1 - this.m_buf.Length : num1 - num2;
  }

  internal int CalculateOutputSize()
  {
    int ivOff = this.m_ivOff;
    int num = ivOff % this.m_buf.Length;
    if (num != 0)
      return ivOff - num + this.m_buf.Length;
    return this.m_isEncryption ? ivOff + this.m_buf.Length : ivOff;
  }

  private int ProcessBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
  {
    if (inOff + this.c_blockSize > input.Length)
      throw new ArgumentException("input buffer length is too short");
    int num;
    if (this.m_isEncryption)
    {
      for (int index = 0; index < this.c_blockSize; ++index)
        this.m_cbcV[index] ^= input[inOff + index];
      num = this.m_aes.Cipher(this.m_cbcV, outBytes, outOff);
      Array.Copy((Array) outBytes, outOff, (Array) this.m_cbcV, 0, this.m_cbcV.Length);
    }
    else
    {
      Array.Copy((Array) input, inOff, (Array) this.m_nextBlockV, 0, this.c_blockSize);
      num = this.m_aes.InvCipher(this.m_nextBlockV, outBytes, outOff);
      for (int index = 0; index < this.c_blockSize; ++index)
        outBytes[outOff + index] ^= this.m_cbcV[index];
      byte[] cbcV = this.m_cbcV;
      this.m_cbcV = this.m_nextBlockV;
      this.m_nextBlockV = cbcV;
    }
    return num;
  }

  private static int AddPadding(byte[] input, int inOff)
  {
    byte num = (byte) (input.Length - inOff);
    for (; inOff < input.Length; ++inOff)
      input[inOff] = num;
    return (int) num;
  }

  private static int CheckPadding(byte[] input)
  {
    int num = (int) input[input.Length - 1] & (int) byte.MaxValue;
    for (int index = 1; index <= num; ++index)
    {
      if ((int) input[input.Length - index] != num)
        num = 0;
    }
    return num;
  }
}
