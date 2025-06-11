// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.Deflate64.HuffmanTree
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System.IO;

#nullable disable
namespace Ionic.Zip.Deflate64;

internal sealed class HuffmanTree
{
  internal const int MaxLiteralTreeElements = 288;
  internal const int MaxDistTreeElements = 32 /*0x20*/;
  internal const int EndOfBlockCode = 256 /*0x0100*/;
  internal const int NumberOfCodeLengthTreeElements = 19;
  private readonly int _tableBits;
  private readonly short[] _table;
  private readonly short[] _left;
  private readonly short[] _right;
  private readonly byte[] _codeLengthArray;
  private readonly int _tableMask;

  public static HuffmanTree StaticLiteralLengthTree { get; } = new HuffmanTree(HuffmanTree.GetStaticLiteralTreeLength());

  public static HuffmanTree StaticDistanceTree { get; } = new HuffmanTree(HuffmanTree.GetStaticDistanceTreeLength());

  public HuffmanTree(byte[] codeLengths)
  {
    this._codeLengthArray = codeLengths;
    this._tableBits = this._codeLengthArray.Length != 288 ? 7 : 9;
    this._tableMask = (1 << this._tableBits) - 1;
    this._table = new short[1 << this._tableBits];
    this._left = new short[2 * this._codeLengthArray.Length];
    this._right = new short[2 * this._codeLengthArray.Length];
    this.CreateTable();
  }

  private static byte[] GetStaticLiteralTreeLength()
  {
    byte[] literalTreeLength = new byte[288];
    for (int index = 0; index <= 143; ++index)
      literalTreeLength[index] = (byte) 8;
    for (int index = 144 /*0x90*/; index <= (int) byte.MaxValue; ++index)
      literalTreeLength[index] = (byte) 9;
    for (int index = 256 /*0x0100*/; index <= 279; ++index)
      literalTreeLength[index] = (byte) 7;
    for (int index = 280; index <= 287; ++index)
      literalTreeLength[index] = (byte) 8;
    return literalTreeLength;
  }

  private static byte[] GetStaticDistanceTreeLength()
  {
    byte[] distanceTreeLength = new byte[32 /*0x20*/];
    for (int index = 0; index < 32 /*0x20*/; ++index)
      distanceTreeLength[index] = (byte) 5;
    return distanceTreeLength;
  }

  private static uint BitReverse(uint code, int length)
  {
    uint num = 0;
    do
    {
      num = (num | code & 1U) << 1;
      code >>= 1;
    }
    while (--length > 0);
    return num >> 1;
  }

  private uint[] CalculateHuffmanCode()
  {
    uint[] numArray1 = new uint[17];
    foreach (int codeLength in this._codeLengthArray)
      ++numArray1[codeLength];
    numArray1[0] = 0U;
    uint[] numArray2 = new uint[17];
    uint num = 0;
    for (int index = 1; index <= 16 /*0x10*/; ++index)
    {
      num = (uint) ((int) num + (int) numArray1[index - 1] << 1);
      numArray2[index] = num;
    }
    uint[] huffmanCode = new uint[288];
    for (int index = 0; index < this._codeLengthArray.Length; ++index)
    {
      int codeLength = (int) this._codeLengthArray[index];
      if (codeLength > 0)
      {
        huffmanCode[index] = HuffmanTree.BitReverse(numArray2[codeLength], codeLength);
        ++numArray2[codeLength];
      }
    }
    return huffmanCode;
  }

  private void CreateTable()
  {
    uint[] huffmanCode = this.CalculateHuffmanCode();
    short length = (short) this._codeLengthArray.Length;
    for (int index1 = 0; index1 < this._codeLengthArray.Length; ++index1)
    {
      int codeLength = (int) this._codeLengthArray[index1];
      if (codeLength > 0)
      {
        int index2 = (int) huffmanCode[index1];
        if (codeLength <= this._tableBits)
        {
          int num1 = 1 << codeLength;
          if (index2 >= num1)
            throw new InvalidDataException("InvalidHuffmanData");
          int num2 = 1 << this._tableBits - codeLength;
          for (int index3 = 0; index3 < num2; ++index3)
          {
            this._table[index2] = (short) index1;
            index2 += num1;
          }
        }
        else
        {
          int num3 = codeLength - this._tableBits;
          int num4 = 1 << this._tableBits;
          int index4 = index2 & (1 << this._tableBits) - 1;
          short[] numArray = this._table;
          do
          {
            short num5 = numArray[index4];
            if (num5 == (short) 0)
            {
              numArray[index4] = -length;
              num5 = -length;
              ++length;
            }
            if (num5 > (short) 0)
              throw new InvalidDataException("InvalidHuffmanData");
            numArray = (index2 & num4) != 0 ? this._right : this._left;
            index4 = (int) -num5;
            num4 <<= 1;
            --num3;
          }
          while (num3 != 0);
          numArray[index4] = (short) index1;
        }
      }
    }
  }

  public int GetNextSymbol(InputBuffer input)
  {
    uint num1 = input.TryLoad16Bits();
    if (input.AvailableBits == 0)
      return -1;
    int nextSymbol = (int) this._table[(long) num1 & (long) this._tableMask];
    if (nextSymbol < 0)
    {
      uint num2 = (uint) (1 << this._tableBits);
      do
      {
        int index = -nextSymbol;
        nextSymbol = ((int) num1 & (int) num2) != 0 ? (int) this._right[index] : (int) this._left[index];
        num2 <<= 1;
      }
      while (nextSymbol < 0);
    }
    int codeLength = (int) this._codeLengthArray[nextSymbol];
    if (codeLength <= 0)
      throw new InvalidDataException("InvalidHuffmanData");
    if (codeLength > input.AvailableBits)
      return -1;
    input.SkipBits(codeLength);
    return nextSymbol;
  }
}
