// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.DecompressorHuffmanTree
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;

#nullable disable
namespace Syncfusion.Compression;

public class DecompressorHuffmanTree
{
  private static int MAX_BITLEN = 15;
  private short[] m_Tree;
  private static DecompressorHuffmanTree m_LengthTree;
  private static DecompressorHuffmanTree m_DistanceTree;

  static DecompressorHuffmanTree()
  {
    try
    {
      byte[] codeLengths1 = new byte[288];
      int num1 = 0;
      while (num1 < 144 /*0x90*/)
        codeLengths1[num1++] = (byte) 8;
      while (num1 < 256 /*0x0100*/)
        codeLengths1[num1++] = (byte) 9;
      while (num1 < 280)
        codeLengths1[num1++] = (byte) 7;
      while (num1 < 288)
        codeLengths1[num1++] = (byte) 8;
      DecompressorHuffmanTree.m_LengthTree = new DecompressorHuffmanTree(codeLengths1);
      byte[] codeLengths2 = new byte[32 /*0x20*/];
      int num2 = 0;
      while (num2 < 32 /*0x20*/)
        codeLengths2[num2++] = (byte) 5;
      DecompressorHuffmanTree.m_DistanceTree = new DecompressorHuffmanTree(codeLengths2);
    }
    catch (Exception ex)
    {
      throw new Exception("DecompressorHuffmanTree: fixed trees generation failed", ex);
    }
  }

  public DecompressorHuffmanTree(byte[] codeLengths) => this.BuildTree(codeLengths);

  private int PrepareData(int[] blCount, int[] nextCode, byte[] lengths, out int treeSize)
  {
    int num1 = 0;
    treeSize = 512 /*0x0200*/;
    for (int index = 0; index < lengths.Length; ++index)
    {
      int length = (int) lengths[index];
      if (length > 0)
        ++blCount[length];
    }
    for (int index = 1; index <= DecompressorHuffmanTree.MAX_BITLEN; ++index)
    {
      nextCode[index] = num1;
      num1 += blCount[index] << 16 /*0x10*/ - index;
      if (index >= 10)
      {
        int num2 = nextCode[index] & 130944;
        int num3 = num1 & 130944;
        treeSize += num3 - num2 >> 16 /*0x10*/ - index;
      }
    }
    return num1;
  }

  private short[] TreeFromData(
    int[] blCount,
    int[] nextCode,
    byte[] lengths,
    int code,
    int treeSize)
  {
    short[] numArray = new short[treeSize];
    int num1 = 512 /*0x0200*/;
    int num2 = 128 /*0x80*/;
    for (int maxBitlen = DecompressorHuffmanTree.MAX_BITLEN; maxBitlen >= 10; --maxBitlen)
    {
      int num3 = code & 130944;
      code -= blCount[maxBitlen] << 16 /*0x10*/ - maxBitlen;
      for (int index = code & 130944; index < num3; index += num2)
      {
        numArray[(int) Utils.BitReverse(index)] = (short) (-num1 << 4 | maxBitlen);
        num1 += 1 << maxBitlen - 9;
      }
    }
    for (int index1 = 0; index1 < lengths.Length; ++index1)
    {
      int length = (int) lengths[index1];
      if (length != 0)
      {
        code = nextCode[length];
        int index2 = (int) Utils.BitReverse(code);
        if (length <= 9)
        {
          do
          {
            numArray[index2] = (short) (index1 << 4 | length);
            index2 += 1 << length;
          }
          while (index2 < 512 /*0x0200*/);
        }
        else
        {
          int num4 = (int) numArray[index2 & 511 /*0x01FF*/];
          int num5 = 1 << (num4 & 15);
          int num6 = -(num4 >> 4);
          do
          {
            numArray[num6 | index2 >> 9] = (short) (index1 << 4 | length);
            index2 += 1 << length;
          }
          while (index2 < num5);
        }
        nextCode[length] = code + (1 << 16 /*0x10*/ - length);
      }
    }
    return numArray;
  }

  private void BuildTree(byte[] lengths)
  {
    int[] blCount = new int[DecompressorHuffmanTree.MAX_BITLEN + 1];
    int[] nextCode = new int[DecompressorHuffmanTree.MAX_BITLEN + 1];
    int treeSize;
    int code = this.PrepareData(blCount, nextCode, lengths, out treeSize);
    this.m_Tree = this.TreeFromData(blCount, nextCode, lengths, code, treeSize);
  }

  public int UnpackSymbol(CompressedStreamReader input)
  {
    int index;
    if ((index = input.PeekBits(9)) >= 0)
    {
      int num1;
      if ((num1 = (int) this.m_Tree[index]) >= 0)
      {
        input.SkipBits(num1 & 15);
        return num1 >> 4;
      }
      int num2 = -(num1 >> 4);
      int count = num1 & 15;
      int num3;
      if ((num3 = input.PeekBits(count)) >= 0)
      {
        int num4 = (int) this.m_Tree[num2 | num3 >> 9];
        input.SkipBits(num4 & 15);
        return num4 >> 4;
      }
      int availableBits = input.AvailableBits;
      int num5 = input.PeekBits(availableBits);
      int num6 = (int) this.m_Tree[num2 | num5 >> 9];
      if ((num6 & 15) > availableBits)
        return -1;
      input.SkipBits(num6 & 15);
      return num6 >> 4;
    }
    int availableBits1 = input.AvailableBits;
    int num = (int) this.m_Tree[input.PeekBits(availableBits1)];
    if (num < 0 || (num & 15) > availableBits1)
      return -1;
    input.SkipBits(num & 15);
    return num >> 4;
  }

  public static DecompressorHuffmanTree LengthTree => DecompressorHuffmanTree.m_LengthTree;

  public static DecompressorHuffmanTree DistanceTree => DecompressorHuffmanTree.m_DistanceTree;
}
