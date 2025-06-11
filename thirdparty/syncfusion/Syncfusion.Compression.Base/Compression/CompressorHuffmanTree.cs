// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.CompressorHuffmanTree
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;

#nullable disable
namespace Syncfusion.Compression;

public class CompressorHuffmanTree
{
  private short[] m_CodeFrequences;
  private short[] m_Codes;
  private byte[] m_CodeLengths;
  private int[] m_LengthCounts;
  private int m_CodeMinimumCount;
  private int m_CodeCount;
  private int m_MaximumLength;
  private CompressedStreamWriter m_Writer;

  public CompressorHuffmanTree(
    CompressedStreamWriter writer,
    int iElementsCount,
    int iMinimumCodes,
    int iMaximumLength)
  {
    this.m_Writer = writer;
    this.m_CodeMinimumCount = iMinimumCodes;
    this.m_MaximumLength = iMaximumLength;
    this.m_CodeFrequences = new short[iElementsCount];
    this.m_LengthCounts = new int[iMaximumLength];
  }

  public void Reset()
  {
    for (int index = 0; index < this.m_CodeFrequences.Length; ++index)
      this.m_CodeFrequences[index] = (short) 0;
    this.m_Codes = (short[]) null;
    this.m_CodeLengths = (byte[]) null;
  }

  public void WriteCodeToStream(int code)
  {
    this.m_Writer.PendingBufferWriteBits((int) this.m_Codes[code] & (int) ushort.MaxValue, (int) this.m_CodeLengths[code]);
  }

  public void CheckEmpty()
  {
    int num = 0;
    while (num < this.m_CodeFrequences.Length)
      ++num;
  }

  public void SetStaticCodes(short[] codes, byte[] lengths)
  {
    this.m_Codes = (short[]) codes.Clone();
    this.m_CodeLengths = (byte[]) lengths.Clone();
  }

  public void BuildCodes()
  {
    int[] numArray = new int[this.m_MaximumLength];
    this.m_Codes = new short[this.m_CodeCount];
    int num = 0;
    for (int index = 0; index < this.m_MaximumLength; ++index)
    {
      numArray[index] = num;
      num += this.m_LengthCounts[index] << 15 - index;
    }
    for (int index = 0; index < this.m_CodeCount; ++index)
    {
      int codeLength = (int) this.m_CodeLengths[index];
      if (codeLength > 0)
      {
        this.m_Codes[index] = Utils.BitReverse(numArray[codeLength - 1]);
        numArray[codeLength - 1] += 1 << 16 /*0x10*/ - codeLength;
      }
    }
  }

  private void BuildLength(int[] childs)
  {
    this.m_CodeLengths = new byte[this.m_CodeFrequences.Length];
    int length = childs.Length / 2;
    int num1 = (length + 1) / 2;
    int num2 = 0;
    for (int index = 0; index < this.m_MaximumLength; ++index)
      this.m_LengthCounts[index] = 0;
    int[] numArray = new int[length];
    numArray[length - 1] = 0;
    for (int index1 = length - 1; index1 >= 0; --index1)
    {
      int index2 = 2 * index1 + 1;
      if (childs[index2] != -1)
      {
        int num3 = numArray[index1] + 1;
        if (num3 > this.m_MaximumLength)
        {
          num3 = this.m_MaximumLength;
          ++num2;
        }
        numArray[childs[index2 - 1]] = numArray[childs[index2]] = num3;
      }
      else
      {
        ++this.m_LengthCounts[numArray[index1] - 1];
        this.m_CodeLengths[childs[index2 - 1]] = (byte) numArray[index1];
      }
    }
    if (num2 == 0)
      return;
    int index3 = this.m_MaximumLength - 1;
    do
    {
      do
        ;
      while (this.m_LengthCounts[--index3] == 0);
      do
      {
        --this.m_LengthCounts[index3];
        ++this.m_LengthCounts[++index3];
        num2 -= 1 << this.m_MaximumLength - 1 - index3;
      }
      while (num2 > 0 && index3 < this.m_MaximumLength - 1);
    }
    while (num2 > 0);
    this.m_LengthCounts[this.m_MaximumLength - 1] += num2;
    this.m_LengthCounts[this.m_MaximumLength - 2] -= num2;
    int num4 = 2 * num1;
    for (int maximumLength = this.m_MaximumLength; maximumLength != 0; --maximumLength)
    {
      int lengthCount = this.m_LengthCounts[maximumLength - 1];
      while (lengthCount > 0)
      {
        int index4 = 2 * childs[num4++];
        if (childs[index4 + 1] == -1)
        {
          this.m_CodeLengths[childs[index4]] = (byte) maximumLength;
          --lengthCount;
        }
      }
    }
  }

  public void BuildTree()
  {
    int length = this.m_CodeFrequences.Length;
    int[] numArray1 = new int[length];
    int num1 = 0;
    int num2 = 0;
    for (int index1 = 0; index1 < length; ++index1)
    {
      int codeFrequence = (int) this.m_CodeFrequences[index1];
      if (codeFrequence != 0)
      {
        int index2;
        int index3;
        for (index2 = num1++; index2 > 0 && (int) this.m_CodeFrequences[numArray1[index3 = (index2 - 1) / 2]] > codeFrequence; index2 = index3)
          numArray1[index2] = numArray1[index3];
        numArray1[index2] = index1;
        num2 = index1;
      }
    }
    while (num1 < 2)
    {
      int[] numArray2 = numArray1;
      int index = num1++;
      int num3;
      if (num2 >= 2)
        num3 = 0;
      else
        num2 = num3 = num2 + 1;
      numArray2[index] = num3;
    }
    this.m_CodeCount = Math.Max(num2 + 1, this.m_CodeMinimumCount);
    int num4 = num1;
    int[] childs = new int[4 * num1 - 2];
    int[] numArray3 = new int[2 * num1 - 1];
    for (int index4 = 0; index4 < num1; ++index4)
    {
      int index5 = numArray1[index4];
      int index6 = 2 * index4;
      childs[index6] = index5;
      childs[index6 + 1] = -1;
      numArray3[index4] = (int) this.m_CodeFrequences[index5] << 8;
      numArray1[index4] = index4;
    }
    do
    {
      int index7 = numArray1[0];
      int index8 = numArray1[--num1];
      int num5 = numArray3[index8];
      int index9 = 0;
      for (int index10 = 1; index10 < num1; index10 = index9 * 2 + 1)
      {
        if (index10 + 1 < num1 && numArray3[numArray1[index10]] > numArray3[numArray1[index10 + 1]])
          ++index10;
        numArray1[index9] = numArray1[index10];
        index9 = index10;
      }
      int index11;
      while ((index11 = index9) > 0 && numArray3[numArray1[index9 = (index11 - 1) / 2]] > num5)
        numArray1[index11] = numArray1[index9];
      numArray1[index11] = index8;
      int index12 = numArray1[0];
      int index13 = num4++;
      childs[2 * index13] = index7;
      childs[2 * index13 + 1] = index12;
      int num6 = Math.Min(numArray3[index7] & (int) byte.MaxValue, numArray3[index12] & (int) byte.MaxValue);
      int num7;
      numArray3[index13] = num7 = numArray3[index7] + numArray3[index12] - num6 + 1;
      int index14 = 0;
      for (int index15 = 1; index15 < num1; index15 = index14 * 2 + 1)
      {
        if (index15 + 1 < num1 && numArray3[numArray1[index15]] > numArray3[numArray1[index15 + 1]])
          ++index15;
        numArray1[index14] = numArray1[index15];
        index14 = index15;
      }
      int index16;
      while ((index16 = index14) > 0 && numArray3[numArray1[index14 = (index16 - 1) / 2]] > num7)
        numArray1[index16] = numArray1[index14];
      numArray1[index16] = index13;
    }
    while (num1 > 1);
    if (numArray1[0] != childs.Length / 2 - 1)
      throw new ApplicationException("Heap invariant violated");
    this.BuildLength(childs);
  }

  public int GetEncodedLength()
  {
    int encodedLength = 0;
    for (int index = 0; index < this.m_CodeFrequences.Length; ++index)
      encodedLength += (int) this.m_CodeFrequences[index] * (int) this.m_CodeLengths[index];
    return encodedLength;
  }

  public void CalcBLFreq(CompressorHuffmanTree blTree)
  {
    int index1 = -1;
    int index2 = 0;
    while (index2 < this.m_CodeCount)
    {
      int num1 = 1;
      int codeLength = (int) this.m_CodeLengths[index2];
      int num2;
      int num3;
      if (codeLength == 0)
      {
        num2 = 138;
        num3 = 3;
      }
      else
      {
        num2 = 6;
        num3 = 3;
        if (index1 != codeLength)
        {
          ++blTree.m_CodeFrequences[codeLength];
          num1 = 0;
        }
      }
      index1 = codeLength;
      ++index2;
      while (index2 < this.m_CodeCount && index1 == (int) this.m_CodeLengths[index2])
      {
        ++index2;
        if (++num1 >= num2)
          break;
      }
      if (num1 < num3)
        blTree.m_CodeFrequences[index1] += (short) num1;
      else if (index1 != 0)
        ++blTree.m_CodeFrequences[16 /*0x10*/];
      else if (num1 <= 10)
        ++blTree.m_CodeFrequences[17];
      else
        ++blTree.m_CodeFrequences[18];
    }
  }

  public void WriteTree(CompressorHuffmanTree blTree)
  {
    int code = -1;
    int index = 0;
    while (index < this.m_CodeCount)
    {
      int num1 = 1;
      int codeLength = (int) this.m_CodeLengths[index];
      int num2;
      int num3;
      if (codeLength == 0)
      {
        num2 = 138;
        num3 = 3;
      }
      else
      {
        num2 = 6;
        num3 = 3;
        if (code != codeLength)
        {
          blTree.WriteCodeToStream(codeLength);
          num1 = 0;
        }
      }
      code = codeLength;
      ++index;
      while (index < this.m_CodeCount && code == (int) this.m_CodeLengths[index])
      {
        ++index;
        if (++num1 >= num2)
          break;
      }
      if (num1 < num3)
      {
        while (num1-- > 0)
          blTree.WriteCodeToStream(code);
      }
      else if (code != 0)
      {
        blTree.WriteCodeToStream(16 /*0x10*/);
        this.m_Writer.PendingBufferWriteBits(num1 - 3, 2);
      }
      else if (num1 <= 10)
      {
        blTree.WriteCodeToStream(17);
        this.m_Writer.PendingBufferWriteBits(num1 - 3, 3);
      }
      else
      {
        blTree.WriteCodeToStream(18);
        this.m_Writer.PendingBufferWriteBits(num1 - 11, 7);
      }
    }
  }

  public int TreeLength => this.m_CodeCount;

  public byte[] CodeLengths => this.m_CodeLengths;

  public short[] CodeFrequences => this.m_CodeFrequences;
}
