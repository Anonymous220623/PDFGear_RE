// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.Deflate64.InflaterManaged
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System;
using System.IO;

#nullable disable
namespace Ionic.Zip.Deflate64;

internal sealed class InflaterManaged
{
  private static readonly byte[] s_extraLengthBits = new byte[29]
  {
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 1,
    (byte) 1,
    (byte) 1,
    (byte) 1,
    (byte) 2,
    (byte) 2,
    (byte) 2,
    (byte) 2,
    (byte) 3,
    (byte) 3,
    (byte) 3,
    (byte) 3,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 4,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 5,
    (byte) 16 /*0x10*/
  };
  private static readonly int[] s_lengthBase = new int[29]
  {
    3,
    4,
    5,
    6,
    7,
    8,
    9,
    10,
    11,
    13,
    15,
    17,
    19,
    23,
    27,
    31 /*0x1F*/,
    35,
    43,
    51,
    59,
    67,
    83,
    99,
    115,
    131,
    163,
    195,
    227,
    3
  };
  private static readonly int[] s_distanceBasePosition = new int[32 /*0x20*/]
  {
    1,
    2,
    3,
    4,
    5,
    7,
    9,
    13,
    17,
    25,
    33,
    49,
    65,
    97,
    129,
    193,
    257,
    385,
    513,
    769,
    1025,
    1537,
    2049,
    3073,
    4097,
    6145,
    8193,
    12289,
    16385,
    24577,
    32769,
    49153
  };
  private static readonly byte[] s_codeOrder = new byte[19]
  {
    (byte) 16 /*0x10*/,
    (byte) 17,
    (byte) 18,
    (byte) 0,
    (byte) 8,
    (byte) 7,
    (byte) 9,
    (byte) 6,
    (byte) 10,
    (byte) 5,
    (byte) 11,
    (byte) 4,
    (byte) 12,
    (byte) 3,
    (byte) 13,
    (byte) 2,
    (byte) 14,
    (byte) 1,
    (byte) 15
  };
  private static readonly byte[] s_staticDistanceTreeTable = new byte[32 /*0x20*/]
  {
    (byte) 0,
    (byte) 16 /*0x10*/,
    (byte) 8,
    (byte) 24,
    (byte) 4,
    (byte) 20,
    (byte) 12,
    (byte) 28,
    (byte) 2,
    (byte) 18,
    (byte) 10,
    (byte) 26,
    (byte) 6,
    (byte) 22,
    (byte) 14,
    (byte) 30,
    (byte) 1,
    (byte) 17,
    (byte) 9,
    (byte) 25,
    (byte) 5,
    (byte) 21,
    (byte) 13,
    (byte) 29,
    (byte) 3,
    (byte) 19,
    (byte) 11,
    (byte) 27,
    (byte) 7,
    (byte) 23,
    (byte) 15,
    (byte) 31 /*0x1F*/
  };
  private readonly OutputWindow _output;
  private readonly InputBuffer _input;
  private HuffmanTree _literalLengthTree;
  private HuffmanTree _distanceTree;
  private InflaterState _state;
  private readonly bool _hasFormatReader;
  private int _bfinal;
  private BlockType _blockType;
  private readonly byte[] _blockLengthBuffer = new byte[4];
  private int _blockLength;
  private int _length;
  private int _distanceCode;
  private int _extraBits;
  private int _loopCounter;
  private int _literalLengthCodeCount;
  private int _distanceCodeCount;
  private int _codeLengthCodeCount;
  private int _codeArraySize;
  private int _lengthCode;
  private readonly byte[] _codeList;
  private readonly byte[] _codeLengthTreeCodeLength;
  private readonly bool _deflate64;
  private HuffmanTree _codeLengthTree;
  private readonly long _uncompressedSize;
  private long _currentInflatedCount;
  private readonly IFileFormatReader _formatReader;

  internal InflaterManaged(IFileFormatReader reader, bool deflate64, long uncompressedSize)
  {
    this._output = new OutputWindow();
    this._input = new InputBuffer();
    this._codeList = new byte[320];
    this._codeLengthTreeCodeLength = new byte[19];
    this._deflate64 = deflate64;
    this._uncompressedSize = uncompressedSize;
    if (reader != null)
    {
      this._formatReader = reader;
      this._hasFormatReader = true;
    }
    this.Reset();
  }

  private void Reset()
  {
    this._state = this._hasFormatReader ? InflaterState.ReadingHeader : InflaterState.ReadingBFinal;
  }

  public void SetInput(byte[] inputBytes, int offset, int length)
  {
    this._input.SetInput(inputBytes, offset, length);
  }

  public bool Finished()
  {
    return this._state == InflaterState.Done || this._state == InflaterState.VerifyingFooter;
  }

  public int AvailableOutput => this._output.AvailableBytes;

  public int Inflate(byte[] bytes, int offset, int length)
  {
    int num = 0;
    do
    {
      int bytesToCopy = 0;
      if (this._uncompressedSize == -1L)
        bytesToCopy = this._output.CopyTo(bytes, offset, length);
      else if (this._uncompressedSize > this._currentInflatedCount)
      {
        length = (int) Math.Min((long) length, this._uncompressedSize - this._currentInflatedCount);
        bytesToCopy = this._output.CopyTo(bytes, offset, length);
        this._currentInflatedCount += (long) bytesToCopy;
      }
      else
      {
        this._state = InflaterState.Done;
        this._output.ClearBytesUsed();
      }
      if (bytesToCopy > 0)
      {
        if (this._hasFormatReader)
          this._formatReader.UpdateWithBytesRead(bytes, offset, bytesToCopy);
        offset += bytesToCopy;
        num += bytesToCopy;
        length -= bytesToCopy;
      }
    }
    while (length != 0 && !this.Finished() && this.Decode());
    if (this._state == InflaterState.VerifyingFooter && this._output.AvailableBytes == 0)
      this._formatReader.Validate();
    return num;
  }

  private bool Decode()
  {
    bool flag1 = false;
    if (this.Finished())
      return true;
    if (this._hasFormatReader)
    {
      if (this._state == InflaterState.ReadingHeader)
      {
        if (!this._formatReader.ReadHeader(this._input))
          return false;
        this._state = InflaterState.ReadingBFinal;
      }
      else if (this._state == InflaterState.StartReadingFooter || this._state == InflaterState.ReadingFooter)
      {
        if (!this._formatReader.ReadFooter(this._input))
          return false;
        this._state = InflaterState.VerifyingFooter;
        return true;
      }
    }
    if (this._state == InflaterState.ReadingBFinal)
    {
      if (!this._input.EnsureBitsAvailable(1))
        return false;
      this._bfinal = this._input.GetBits(1);
      this._state = InflaterState.ReadingBType;
    }
    if (this._state == InflaterState.ReadingBType)
    {
      if (!this._input.EnsureBitsAvailable(2))
      {
        this._state = InflaterState.ReadingBType;
        return false;
      }
      this._blockType = (BlockType) this._input.GetBits(2);
      if (this._blockType == BlockType.Dynamic)
        this._state = InflaterState.ReadingNumLitCodes;
      else if (this._blockType == BlockType.Static)
      {
        this._literalLengthTree = HuffmanTree.StaticLiteralLengthTree;
        this._distanceTree = HuffmanTree.StaticDistanceTree;
        this._state = InflaterState.DecodeTop;
      }
      else
      {
        if (this._blockType != BlockType.Uncompressed)
          throw new InvalidDataException("UnknownBlockType");
        this._state = InflaterState.UncompressedAligning;
      }
    }
    bool flag2;
    if (this._blockType == BlockType.Dynamic)
      flag2 = this._state >= InflaterState.DecodeTop ? this.DecodeBlock(out flag1) : this.DecodeDynamicBlockHeader();
    else if (this._blockType == BlockType.Static)
    {
      flag2 = this.DecodeBlock(out flag1);
    }
    else
    {
      if (this._blockType != BlockType.Uncompressed)
        throw new InvalidDataException("UnknownBlockType");
      flag2 = this.DecodeUncompressedBlock(out flag1);
    }
    if (flag1 && this._bfinal != 0)
      this._state = !this._hasFormatReader ? InflaterState.Done : InflaterState.StartReadingFooter;
    return flag2;
  }

  private bool DecodeUncompressedBlock(out bool end_of_block)
  {
    end_of_block = false;
    while (true)
    {
      switch (this._state)
      {
        case InflaterState.UncompressedAligning:
          this._input.SkipToByteBoundary();
          this._state = InflaterState.UncompressedByte1;
          goto case InflaterState.UncompressedByte1;
        case InflaterState.UncompressedByte1:
        case InflaterState.UncompressedByte2:
        case InflaterState.UncompressedByte3:
        case InflaterState.UncompressedByte4:
          int bits = this._input.GetBits(8);
          if (bits >= 0)
          {
            this._blockLengthBuffer[(int) (this._state - 16 /*0x10*/)] = (byte) bits;
            if (this._state == InflaterState.UncompressedByte4)
            {
              this._blockLength = (int) this._blockLengthBuffer[0] + (int) this._blockLengthBuffer[1] * 256 /*0x0100*/;
              if ((int) (ushort) this._blockLength != (int) (ushort) ~((int) this._blockLengthBuffer[2] + (int) this._blockLengthBuffer[3] * 256 /*0x0100*/))
                goto label_7;
            }
            ++this._state;
            continue;
          }
          goto label_4;
        case InflaterState.DecodingUncompressed:
          goto label_9;
        default:
          goto label_14;
      }
    }
label_4:
    return false;
label_7:
    throw new InvalidDataException("InvalidBlockLength");
label_9:
    this._blockLength -= this._output.CopyFrom(this._input, this._blockLength);
    if (this._blockLength == 0)
    {
      this._state = InflaterState.ReadingBFinal;
      end_of_block = true;
      return true;
    }
    return this._output.FreeBytes == 0;
label_14:
    throw new InvalidDataException("UnknownState");
  }

  private bool DecodeBlock(out bool end_of_block_code_seen)
  {
    end_of_block_code_seen = false;
    int freeBytes = this._output.FreeBytes;
    while (freeBytes > 65536 /*0x010000*/)
    {
      switch (this._state)
      {
        case InflaterState.DecodeTop:
          int nextSymbol = this._literalLengthTree.GetNextSymbol(this._input);
          if (nextSymbol < 0)
            return false;
          if (nextSymbol < 256 /*0x0100*/)
          {
            this._output.Write((byte) nextSymbol);
            --freeBytes;
            continue;
          }
          if (nextSymbol == 256 /*0x0100*/)
          {
            end_of_block_code_seen = true;
            this._state = InflaterState.ReadingBFinal;
            return true;
          }
          int index = nextSymbol - 257;
          if (index < 8)
          {
            index += 3;
            this._extraBits = 0;
          }
          else if (!this._deflate64 && index == 28)
          {
            index = 258;
            this._extraBits = 0;
          }
          else
          {
            if (index < 0 || index >= InflaterManaged.s_extraLengthBits.Length)
              throw new InvalidDataException("GenericInvalidData");
            this._extraBits = (int) InflaterManaged.s_extraLengthBits[index];
          }
          this._length = index;
          goto case InflaterState.HaveInitialLength;
        case InflaterState.HaveInitialLength:
          if (this._extraBits > 0)
          {
            this._state = InflaterState.HaveInitialLength;
            int bits = this._input.GetBits(this._extraBits);
            if (bits < 0)
              return false;
            if (this._length < 0 || this._length >= InflaterManaged.s_lengthBase.Length)
              throw new InvalidDataException("GenericInvalidData");
            this._length = InflaterManaged.s_lengthBase[this._length] + bits;
          }
          this._state = InflaterState.HaveFullLength;
          goto case InflaterState.HaveFullLength;
        case InflaterState.HaveFullLength:
          if (this._blockType == BlockType.Dynamic)
          {
            this._distanceCode = this._distanceTree.GetNextSymbol(this._input);
          }
          else
          {
            this._distanceCode = this._input.GetBits(5);
            if (this._distanceCode >= 0)
              this._distanceCode = (int) InflaterManaged.s_staticDistanceTreeTable[this._distanceCode];
          }
          if (this._distanceCode < 0)
            return false;
          this._state = InflaterState.HaveDistCode;
          goto case InflaterState.HaveDistCode;
        case InflaterState.HaveDistCode:
          int distance;
          if (this._distanceCode > 3)
          {
            this._extraBits = this._distanceCode - 2 >> 1;
            int bits = this._input.GetBits(this._extraBits);
            if (bits < 0)
              return false;
            distance = InflaterManaged.s_distanceBasePosition[this._distanceCode] + bits;
          }
          else
            distance = this._distanceCode + 1;
          this._output.WriteLengthDistance(this._length, distance);
          freeBytes -= this._length;
          this._state = InflaterState.DecodeTop;
          continue;
        default:
          throw new InvalidDataException("UnknownState");
      }
    }
    return true;
  }

  private bool DecodeDynamicBlockHeader()
  {
    switch (this._state)
    {
      case InflaterState.ReadingNumLitCodes:
        this._literalLengthCodeCount = this._input.GetBits(5);
        if (this._literalLengthCodeCount < 0)
          return false;
        this._literalLengthCodeCount += 257;
        this._state = InflaterState.ReadingNumDistCodes;
        goto case InflaterState.ReadingNumDistCodes;
      case InflaterState.ReadingNumDistCodes:
        this._distanceCodeCount = this._input.GetBits(5);
        if (this._distanceCodeCount < 0)
          return false;
        ++this._distanceCodeCount;
        this._state = InflaterState.ReadingNumCodeLengthCodes;
        goto case InflaterState.ReadingNumCodeLengthCodes;
      case InflaterState.ReadingNumCodeLengthCodes:
        this._codeLengthCodeCount = this._input.GetBits(4);
        if (this._codeLengthCodeCount < 0)
          return false;
        this._codeLengthCodeCount += 4;
        this._loopCounter = 0;
        this._state = InflaterState.ReadingCodeLengthCodes;
        goto case InflaterState.ReadingCodeLengthCodes;
      case InflaterState.ReadingCodeLengthCodes:
        for (; this._loopCounter < this._codeLengthCodeCount; ++this._loopCounter)
        {
          int bits = this._input.GetBits(3);
          if (bits < 0)
            return false;
          this._codeLengthTreeCodeLength[(int) InflaterManaged.s_codeOrder[this._loopCounter]] = (byte) bits;
        }
        for (int codeLengthCodeCount = this._codeLengthCodeCount; codeLengthCodeCount < InflaterManaged.s_codeOrder.Length; ++codeLengthCodeCount)
          this._codeLengthTreeCodeLength[(int) InflaterManaged.s_codeOrder[codeLengthCodeCount]] = (byte) 0;
        this._codeLengthTree = new HuffmanTree(this._codeLengthTreeCodeLength);
        this._codeArraySize = this._literalLengthCodeCount + this._distanceCodeCount;
        this._loopCounter = 0;
        this._state = InflaterState.ReadingTreeCodesBefore;
        goto case InflaterState.ReadingTreeCodesBefore;
      case InflaterState.ReadingTreeCodesBefore:
      case InflaterState.ReadingTreeCodesAfter:
        while (this._loopCounter < this._codeArraySize)
        {
          if (this._state == InflaterState.ReadingTreeCodesBefore && (this._lengthCode = this._codeLengthTree.GetNextSymbol(this._input)) < 0)
            return false;
          if (this._lengthCode <= 15)
            this._codeList[this._loopCounter++] = (byte) this._lengthCode;
          else if (this._lengthCode == 16 /*0x10*/)
          {
            if (!this._input.EnsureBitsAvailable(2))
            {
              this._state = InflaterState.ReadingTreeCodesAfter;
              return false;
            }
            byte num1 = this._loopCounter != 0 ? this._codeList[this._loopCounter - 1] : throw new InvalidDataException();
            int num2 = this._input.GetBits(2) + 3;
            if (this._loopCounter + num2 > this._codeArraySize)
              throw new InvalidDataException();
            for (int index = 0; index < num2; ++index)
              this._codeList[this._loopCounter++] = num1;
          }
          else if (this._lengthCode == 17)
          {
            if (!this._input.EnsureBitsAvailable(3))
            {
              this._state = InflaterState.ReadingTreeCodesAfter;
              return false;
            }
            int num = this._input.GetBits(3) + 3;
            if (this._loopCounter + num > this._codeArraySize)
              throw new InvalidDataException();
            for (int index = 0; index < num; ++index)
              this._codeList[this._loopCounter++] = (byte) 0;
          }
          else
          {
            if (!this._input.EnsureBitsAvailable(7))
            {
              this._state = InflaterState.ReadingTreeCodesAfter;
              return false;
            }
            int num = this._input.GetBits(7) + 11;
            if (this._loopCounter + num > this._codeArraySize)
              throw new InvalidDataException();
            for (int index = 0; index < num; ++index)
              this._codeList[this._loopCounter++] = (byte) 0;
          }
          this._state = InflaterState.ReadingTreeCodesBefore;
        }
        byte[] numArray1 = new byte[288];
        byte[] numArray2 = new byte[32 /*0x20*/];
        Array.Copy((Array) this._codeList, 0, (Array) numArray1, 0, this._literalLengthCodeCount);
        Array.Copy((Array) this._codeList, this._literalLengthCodeCount, (Array) numArray2, 0, this._distanceCodeCount);
        this._literalLengthTree = numArray1[256 /*0x0100*/] != (byte) 0 ? new HuffmanTree(numArray1) : throw new InvalidDataException();
        this._distanceTree = new HuffmanTree(numArray2);
        this._state = InflaterState.DecodeTop;
        return true;
      default:
        throw new InvalidDataException("UnknownState");
    }
  }

  public void Dispose()
  {
  }
}
