// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.Zip.RFC2898Key
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Compression.Zip;

internal class RFC2898Key
{
  private int m_block;
  private byte[] m_rfcBuffer;
  private int m_endOffset;
  private int m_iterations;
  private byte[] m_salt;
  private int m_startOffset;
  private byte[] m_password;
  private int m_blockSizeValue = 64 /*0x40*/;
  private byte[] m_inner;
  private byte[] m_outer;
  private bool m_hashing;
  private byte[] m_keyVal;
  private byte[] m_buffer;
  private long m_count;
  private uint[] m_stateSHA1;
  private uint[] m_expandedBuffer;
  private byte[] m_hmacHashVal;
  private byte[] m_sha1HashVal;

  internal byte[] Hash => (byte[]) this.m_hmacHashVal.Clone();

  internal RFC2898Key(string password, byte[] salt, int iterations)
    : this(Encoding.UTF8.GetBytes(password), salt, iterations)
  {
  }

  internal RFC2898Key(byte[] password, byte[] salt, int iterations)
  {
    this.m_password = password;
    this.m_salt = salt;
    this.m_iterations = iterations;
    this.InitializeKey(password);
    this.m_stateSHA1 = new uint[5];
    this.m_buffer = new byte[64 /*0x40*/];
    this.m_expandedBuffer = new uint[80 /*0x50*/];
    this.InitializeState();
    this.InitializeRfc2898();
  }

  private byte[] ByteArray(int input)
  {
    byte[] bytes = BitConverter.GetBytes(input);
    byte[] numArray = new byte[4]
    {
      bytes[3],
      bytes[2],
      bytes[1],
      bytes[0]
    };
    return !BitConverter.IsLittleEndian ? bytes : numArray;
  }

  private byte[] DeriveCryptographicKey()
  {
    byte[] src = this.ByteArray(this.m_block);
    byte[] numArray1 = new byte[this.m_salt.Length + src.Length];
    Buffer.BlockCopy((Array) this.m_salt, 0, (Array) numArray1, 0, this.m_salt.Length);
    Buffer.BlockCopy((Array) src, 0, (Array) numArray1, this.m_salt.Length, src.Length);
    this.ComputeHash(numArray1);
    byte[] hash = this.Hash;
    byte[] numArray2 = hash;
    for (int index1 = 2; index1 <= this.m_iterations; ++index1)
    {
      hash = this.ComputeHash(hash);
      for (int index2 = 0; index2 < 20; ++index2)
        numArray2[index2] = (byte) ((uint) numArray2[index2] ^ (uint) hash[index2]);
    }
    ++this.m_block;
    return numArray2;
  }

  internal byte[] GetBytes(int length)
  {
    byte[] dst = length > 0 ? new byte[length] : throw new ArgumentOutOfRangeException(nameof (length));
    int dstOffset = 0;
    int count = this.m_endOffset - this.m_startOffset;
    if (count > 0)
    {
      if (length < count)
      {
        Buffer.BlockCopy((Array) this.m_rfcBuffer, this.m_startOffset, (Array) dst, 0, length);
        this.m_startOffset += length;
        return dst;
      }
      Buffer.BlockCopy((Array) this.m_rfcBuffer, this.m_startOffset, (Array) dst, 0, count);
      this.m_startOffset = this.m_endOffset = 0;
      dstOffset += count;
    }
    for (; dstOffset < length; dstOffset += 20)
    {
      byte[] src = this.DeriveCryptographicKey();
      int num1 = length - dstOffset;
      if (num1 > 20)
      {
        Buffer.BlockCopy((Array) src, 0, (Array) dst, dstOffset, 20);
      }
      else
      {
        Buffer.BlockCopy((Array) src, 0, (Array) dst, dstOffset, num1);
        int num2 = dstOffset + num1;
        Buffer.BlockCopy((Array) src, num1, (Array) this.m_rfcBuffer, this.m_startOffset, 20 - num1);
        this.m_endOffset += 20 - num1;
        return dst;
      }
    }
    return dst;
  }

  private void InitializeRfc2898()
  {
    if (this.m_rfcBuffer != null)
      Array.Clear((Array) this.m_rfcBuffer, 0, this.m_rfcBuffer.Length);
    this.m_rfcBuffer = new byte[20];
    this.m_block = 1;
    this.m_startOffset = this.m_endOffset = 0;
  }

  private void UpdateInnerAndOuterArrays()
  {
    if (this.m_inner == null)
      this.m_inner = new byte[this.m_blockSizeValue];
    if (this.m_outer == null)
      this.m_outer = new byte[this.m_blockSizeValue];
    for (int index = 0; index < this.m_blockSizeValue; ++index)
    {
      this.m_inner[index] = (byte) 54;
      this.m_outer[index] = (byte) 92;
    }
    for (int index = 0; index < this.m_keyVal.Length; ++index)
    {
      this.m_inner[index] ^= this.m_keyVal[index];
      this.m_outer[index] ^= this.m_keyVal[index];
    }
  }

  private void InitializeKey(byte[] keyVal)
  {
    this.m_inner = (byte[]) null;
    this.m_outer = (byte[]) null;
    this.m_keyVal = keyVal.Length <= this.m_blockSizeValue ? (byte[]) keyVal.Clone() : this.ComputeHash(keyVal);
    this.UpdateInnerAndOuterArrays();
  }

  internal byte[] ComputeHash(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (!this.m_hashing)
    {
      this.UpdateBlock(this.m_inner, 0, this.m_inner.Length, this.m_inner, 0);
      this.m_hashing = true;
    }
    this.UpdateBlock(data, 0, data.Length, data, 0);
    if (!this.m_hashing)
    {
      this.UpdateBlock(this.m_inner, 0, this.m_inner.Length, this.m_inner, 0);
      this.m_hashing = true;
    }
    this.UpdateFinalBlock(new byte[0], 0, 0);
    byte[] sha1HashVal = this.m_sha1HashVal;
    this.m_sha1HashVal = (byte[]) null;
    this.Initialize();
    this.UpdateBlock(this.m_outer, 0, this.m_outer.Length, this.m_outer, 0);
    this.UpdateBlock(sha1HashVal, 0, sha1HashVal.Length, sha1HashVal, 0);
    this.m_hashing = false;
    this.UpdateFinalBlock(new byte[0], 0, 0);
    this.m_hmacHashVal = this.m_sha1HashVal;
    byte[] hash = (byte[]) this.m_hmacHashVal.Clone();
    this.Initialize();
    this.m_hashing = false;
    return hash;
  }

  private void UpdateBlock(byte[] input, int inputOff, int count, byte[] output, int outputOff)
  {
    this.UpdateHashData(input, inputOff, count);
    if (output == null || input == output && inputOff == outputOff)
      return;
    Buffer.BlockCopy((Array) input, inputOff, (Array) output, outputOff, count);
  }

  private void UpdateFinalBlock(byte[] input, int inputOff, int count)
  {
    this.UpdateHashData(input, inputOff, count);
    this.m_sha1HashVal = this.UpdateEndHash();
    byte[] dst = new byte[count];
    if (count == 0)
      return;
    Buffer.BlockCopy((Array) input, inputOff, (Array) dst, 0, count);
  }

  private void Initialize()
  {
    this.InitializeState();
    Array.Clear((Array) this.m_buffer, 0, this.m_buffer.Length);
    Array.Clear((Array) this.m_expandedBuffer, 0, this.m_expandedBuffer.Length);
  }

  private void InitializeState()
  {
    this.m_count = 0L;
    this.m_stateSHA1[0] = 1732584193U;
    this.m_stateSHA1[1] = 4023233417U;
    this.m_stateSHA1[2] = 2562383102U;
    this.m_stateSHA1[3] = 271733878U;
    this.m_stateSHA1[4] = 3285377520U;
  }

  private void UpdateHashData(byte[] inputData, int startOffSet, int size)
  {
    int count = size;
    int srcOffset = startOffSet;
    int dstOffset = (int) (this.m_count & 63L /*0x3F*/);
    this.m_count += (long) count;
    uint[] stateShA1 = this.m_stateSHA1;
    byte[] buffer = this.m_buffer;
    uint[] expandedBuffer = this.m_expandedBuffer;
    if (dstOffset > 0 && dstOffset + count >= 64 /*0x40*/)
    {
      Buffer.BlockCopy((Array) inputData, srcOffset, (Array) this.m_buffer, dstOffset, 64 /*0x40*/ - dstOffset);
      srcOffset += 64 /*0x40*/ - dstOffset;
      count -= 64 /*0x40*/ - dstOffset;
      this.SHAModify(expandedBuffer, stateShA1, buffer);
      dstOffset = 0;
    }
    while (count >= 64 /*0x40*/)
    {
      Buffer.BlockCopy((Array) inputData, srcOffset, (Array) this.m_buffer, 0, 64 /*0x40*/);
      srcOffset += 64 /*0x40*/;
      count -= 64 /*0x40*/;
      this.SHAModify(expandedBuffer, stateShA1, buffer);
    }
    if (count <= 0)
      return;
    Buffer.BlockCopy((Array) inputData, srcOffset, (Array) this.m_buffer, dstOffset, count);
  }

  private byte[] UpdateEndHash()
  {
    byte[] blockData = new byte[20];
    int length = 64 /*0x40*/ - (int) (this.m_count & 63L /*0x3F*/);
    if (length <= 8)
      length += 64 /*0x40*/;
    byte[] inputData = new byte[length];
    inputData[0] = (byte) 128 /*0x80*/;
    long num = this.m_count * 8L;
    inputData[length - 8] = (byte) ((ulong) (num >> 56) & (ulong) byte.MaxValue);
    inputData[length - 7] = (byte) ((ulong) (num >> 48 /*0x30*/) & (ulong) byte.MaxValue);
    inputData[length - 6] = (byte) ((ulong) (num >> 40) & (ulong) byte.MaxValue);
    inputData[length - 5] = (byte) ((ulong) (num >> 32 /*0x20*/) & (ulong) byte.MaxValue);
    inputData[length - 4] = (byte) ((ulong) (num >> 24) & (ulong) byte.MaxValue);
    inputData[length - 3] = (byte) ((ulong) (num >> 16 /*0x10*/) & (ulong) byte.MaxValue);
    inputData[length - 2] = (byte) ((ulong) (num >> 8) & (ulong) byte.MaxValue);
    inputData[length - 1] = (byte) ((ulong) num & (ulong) byte.MaxValue);
    this.UpdateHashData(inputData, 0, inputData.Length);
    this.DWORDToBigEndian(blockData, this.m_stateSHA1, 5);
    this.m_sha1HashVal = blockData;
    return blockData;
  }

  private void SHAModify(uint[] expandedBuffer, uint[] state, byte[] block)
  {
    uint num1 = state[0];
    uint num2 = state[1];
    uint num3 = state[2];
    uint num4 = state[3];
    uint num5 = state[4];
    this.DWORDFromBigEndian(expandedBuffer, 16 /*0x10*/, block);
    this.SHAExpansion(expandedBuffer);
    int index;
    for (index = 0; index < 20; index += 5)
    {
      uint num6 = num5 + (uint) (((int) num1 << 5 | (int) (num1 >> 27)) + ((int) num4 ^ (int) num2 & ((int) num3 ^ (int) num4)) + (int) expandedBuffer[index] + 1518500249);
      uint num7 = num2 << 30 | num2 >> 2;
      uint num8 = num4 + (uint) (((int) num6 << 5 | (int) (num6 >> 27)) + ((int) num3 ^ (int) num1 & ((int) num7 ^ (int) num3)) + (int) expandedBuffer[index + 1] + 1518500249);
      uint num9 = num1 << 30 | num1 >> 2;
      uint num10 = num3 + (uint) (((int) num8 << 5 | (int) (num8 >> 27)) + ((int) num7 ^ (int) num6 & ((int) num9 ^ (int) num7)) + (int) expandedBuffer[index + 2] + 1518500249);
      num5 = num6 << 30 | num6 >> 2;
      num2 = num7 + (uint) (((int) num10 << 5 | (int) (num10 >> 27)) + ((int) num9 ^ (int) num8 & ((int) num5 ^ (int) num9)) + (int) expandedBuffer[index + 3] + 1518500249);
      num4 = num8 << 30 | num8 >> 2;
      num1 = num9 + (uint) (((int) num2 << 5 | (int) (num2 >> 27)) + ((int) num5 ^ (int) num10 & ((int) num4 ^ (int) num5)) + (int) expandedBuffer[index + 4] + 1518500249);
      num3 = num10 << 30 | num10 >> 2;
    }
    for (; index < 40; index += 5)
    {
      uint num11 = num5 + (uint) (((int) num1 << 5 | (int) (num1 >> 27)) + ((int) num2 ^ (int) num3 ^ (int) num4) + (int) expandedBuffer[index] + 1859775393);
      uint num12 = num2 << 30 | num2 >> 2;
      uint num13 = num4 + (uint) (((int) num11 << 5 | (int) (num11 >> 27)) + ((int) num1 ^ (int) num12 ^ (int) num3) + (int) expandedBuffer[index + 1] + 1859775393);
      uint num14 = num1 << 30 | num1 >> 2;
      uint num15 = num3 + (uint) (((int) num13 << 5 | (int) (num13 >> 27)) + ((int) num11 ^ (int) num14 ^ (int) num12) + (int) expandedBuffer[index + 2] + 1859775393);
      num5 = num11 << 30 | num11 >> 2;
      num2 = num12 + (uint) (((int) num15 << 5 | (int) (num15 >> 27)) + ((int) num13 ^ (int) num5 ^ (int) num14) + (int) expandedBuffer[index + 3] + 1859775393);
      num4 = num13 << 30 | num13 >> 2;
      num1 = num14 + (uint) (((int) num2 << 5 | (int) (num2 >> 27)) + ((int) num15 ^ (int) num4 ^ (int) num5) + (int) expandedBuffer[index + 4] + 1859775393);
      num3 = num15 << 30 | num15 >> 2;
    }
    for (; index < 60; index += 5)
    {
      uint num16 = num5 + (uint) (((int) num1 << 5 | (int) (num1 >> 27)) + ((int) num2 & (int) num3 | (int) num4 & ((int) num2 | (int) num3)) + (int) expandedBuffer[index] - 1894007588);
      uint num17 = num2 << 30 | num2 >> 2;
      uint num18 = num4 + (uint) (((int) num16 << 5 | (int) (num16 >> 27)) + ((int) num1 & (int) num17 | (int) num3 & ((int) num1 | (int) num17)) + (int) expandedBuffer[index + 1] - 1894007588);
      uint num19 = num1 << 30 | num1 >> 2;
      uint num20 = num3 + (uint) (((int) num18 << 5 | (int) (num18 >> 27)) + ((int) num16 & (int) num19 | (int) num17 & ((int) num16 | (int) num19)) + (int) expandedBuffer[index + 2] - 1894007588);
      num5 = num16 << 30 | num16 >> 2;
      num2 = num17 + (uint) (((int) num20 << 5 | (int) (num20 >> 27)) + ((int) num18 & (int) num5 | (int) num19 & ((int) num18 | (int) num5)) + (int) expandedBuffer[index + 3] - 1894007588);
      num4 = num18 << 30 | num18 >> 2;
      num1 = num19 + (uint) (((int) num2 << 5 | (int) (num2 >> 27)) + ((int) num20 & (int) num4 | (int) num5 & ((int) num20 | (int) num4)) + (int) expandedBuffer[index + 4] - 1894007588);
      num3 = num20 << 30 | num20 >> 2;
    }
    for (; index < 80 /*0x50*/; index += 5)
    {
      uint num21 = num5 + (uint) (((int) num1 << 5 | (int) (num1 >> 27)) + ((int) num2 ^ (int) num3 ^ (int) num4) + (int) expandedBuffer[index] - 899497514);
      uint num22 = num2 << 30 | num2 >> 2;
      uint num23 = num4 + (uint) (((int) num21 << 5 | (int) (num21 >> 27)) + ((int) num1 ^ (int) num22 ^ (int) num3) + (int) expandedBuffer[index + 1] - 899497514);
      uint num24 = num1 << 30 | num1 >> 2;
      uint num25 = num3 + (uint) (((int) num23 << 5 | (int) (num23 >> 27)) + ((int) num21 ^ (int) num24 ^ (int) num22) + (int) expandedBuffer[index + 2] - 899497514);
      num5 = num21 << 30 | num21 >> 2;
      num2 = num22 + (uint) (((int) num25 << 5 | (int) (num25 >> 27)) + ((int) num23 ^ (int) num5 ^ (int) num24) + (int) expandedBuffer[index + 3] - 899497514);
      num4 = num23 << 30 | num23 >> 2;
      num1 = num24 + (uint) (((int) num2 << 5 | (int) (num2 >> 27)) + ((int) num25 ^ (int) num4 ^ (int) num5) + (int) expandedBuffer[index + 4] - 899497514);
      num3 = num25 << 30 | num25 >> 2;
    }
    state[0] += num1;
    state[1] += num2;
    state[2] += num3;
    state[3] += num4;
    state[4] += num5;
  }

  private void SHAExpansion(uint[] input)
  {
    for (int index = 16 /*0x10*/; index < 80 /*0x50*/; ++index)
    {
      uint num = input[index - 3] ^ input[index - 8] ^ input[index - 14] ^ input[index - 16 /*0x10*/];
      input[index] = num << 1 | num >> 31 /*0x1F*/;
    }
  }

  private void DWORDFromBigEndian(uint[] input, int digits, byte[] block)
  {
    int index1 = 0;
    int index2 = 0;
    while (index1 < digits)
    {
      input[index1] = (uint) ((int) block[index2] << 24 | (int) block[index2 + 1] << 16 /*0x10*/ | (int) block[index2 + 2] << 8) | (uint) block[index2 + 3];
      ++index1;
      index2 += 4;
    }
  }

  private void DWORDToBigEndian(byte[] blockData, uint[] output, int digits)
  {
    int index1 = 0;
    int index2 = 0;
    while (index1 < digits)
    {
      blockData[index2] = (byte) (output[index1] >> 24 & (uint) byte.MaxValue);
      blockData[index2 + 1] = (byte) (output[index1] >> 16 /*0x10*/ & (uint) byte.MaxValue);
      blockData[index2 + 2] = (byte) (output[index1] >> 8 & (uint) byte.MaxValue);
      blockData[index2 + 3] = (byte) (output[index1] & (uint) byte.MaxValue);
      ++index1;
      index2 += 4;
    }
  }
}
