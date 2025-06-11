// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.CompressedStreamReader
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Compression;

public class CompressedStreamReader
{
  private const int DEF_HEADER_METHOD_MASK = 3840 /*0x0F00*/;
  private const int DEF_HEADER_INFO_MASK = 61440 /*0xF000*/;
  private const int DEF_HEADER_FLAGS_FCHECK = 31 /*0x1F*/;
  private const int DEF_HEADER_FLAGS_FDICT = 32 /*0x20*/;
  private const int DEF_HEADER_FLAGS_FLEVEL = 192 /*0xC0*/;
  private const int DEF_MAX_WINDOW_SIZE = 65535 /*0xFFFF*/;
  private const int DEF_HUFFMAN_REPEATE_MAX = 258;
  private const int DEF_HUFFMAN_END_BLOCK = 256 /*0x0100*/;
  private const int DEF_HUFFMAN_LENGTH_MINIMUM_CODE = 257;
  private const int DEF_HUFFMAN_LENGTH_MAXIMUM_CODE = 285;
  private const int DEF_HUFFMAN_DISTANCE_MAXIMUM_CODE = 29;
  private static readonly int[] DEF_HUFFMAN_DYNTREE_REPEAT_MINIMUMS = new int[3]
  {
    3,
    3,
    11
  };
  private static readonly int[] DEF_HUFFMAN_DYNTREE_REPEAT_BITS = new int[3]
  {
    2,
    3,
    7
  };
  private static readonly int[] DEF_HUFFMAN_REPEAT_LENGTH_BASE = new int[29]
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
    258
  };
  private static readonly int[] DEF_HUFFMAN_REPEAT_LENGTH_EXTENSION = new int[29]
  {
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    0,
    1,
    1,
    1,
    1,
    2,
    2,
    2,
    2,
    3,
    3,
    3,
    3,
    4,
    4,
    4,
    4,
    5,
    5,
    5,
    5,
    0
  };
  private static readonly int[] DEF_HUFFMAN_REPEAT_DISTANCE_BASE = new int[30]
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
    24577
  };
  private static readonly int[] DEF_HUFFMAN_REPEAT_DISTANCE_EXTENSION = new int[30]
  {
    0,
    0,
    0,
    0,
    1,
    1,
    2,
    2,
    3,
    3,
    4,
    4,
    5,
    5,
    6,
    6,
    7,
    7,
    8,
    8,
    9,
    9,
    10,
    10,
    11,
    11,
    12,
    12,
    13,
    13
  };
  private Stream m_InputStream;
  private long m_CheckSum = 1;
  private uint m_Buffer;
  private int m_BufferedBits;
  private byte[] m_temp_buffer = new byte[4];
  private byte[] m_Block_Buffer = new byte[(int) ushort.MaxValue];
  private bool m_bNoWrap;
  private int m_WindowSize;
  private long m_CurrentPosition;
  private long m_DataLength;
  private bool m_bReadingUncompressed;
  private int m_UncompressedDataLength;
  private bool m_bCanReadNextBlock = true;
  private bool m_bCanReadMoreData = true;
  private DecompressorHuffmanTree m_CurrentLengthTree;
  private DecompressorHuffmanTree m_CurrentDistanceTree;
  private bool m_bCheckSumRead;

  public CompressedStreamReader(Stream stream)
    : this(stream, false)
  {
  }

  public CompressedStreamReader(Stream stream, bool bNoWrap)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_InputStream = stream.Length != 0L ? stream : throw new ArgumentException("stream - string can not be empty");
    this.m_bNoWrap = bNoWrap;
    if (!this.m_bNoWrap)
      this.ReadZLibHeader();
    this.DecodeBlockHeader();
  }

  protected internal int AvailableBits => this.m_BufferedBits;

  protected internal long AvailableBytes
  {
    get
    {
      return this.m_InputStream.Length - this.m_InputStream.Position + (long) this.m_BufferedBits >> 3;
    }
  }

  protected void ChecksumReset() => this.m_CheckSum = 1L;

  protected void ChecksumUpdate(byte[] buffer, int offset, int length)
  {
    ChecksumCalculator.ChecksumUpdate(ref this.m_CheckSum, buffer, offset, length);
  }

  protected internal void SkipToBoundary()
  {
    this.m_Buffer >>= this.m_BufferedBits & 7;
    this.m_BufferedBits &= -8;
  }

  protected internal int ReadPackedBytes(byte[] buffer, int offset, int length)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    if (offset < 0 || offset > buffer.Length - 1)
      throw new ArgumentOutOfRangeException(nameof (offset), "Offset can not be less than zero or greater than buffer length - 1.");
    if (length < 0)
      throw new ArgumentOutOfRangeException(nameof (length), "Length can not be less than zero.");
    if (length > buffer.Length - offset)
      throw new ArgumentOutOfRangeException(nameof (length), "Length is too large.");
    if ((this.m_BufferedBits & 7) != 0)
      throw new NotSupportedException("Reading of unalligned data is not supported.");
    if (length == 0)
      return 0;
    int num = 0;
    while (this.m_BufferedBits > 0 && length > 0)
    {
      buffer[offset++] = (byte) this.m_Buffer;
      this.m_BufferedBits -= 8;
      this.m_Buffer >>= 8;
      --length;
      ++num;
    }
    if (length > 0)
      num += this.m_InputStream.Read(buffer, offset, length);
    return num;
  }

  protected void FillBuffer()
  {
    int count = 4 - (this.m_BufferedBits >> 3) - ((this.m_BufferedBits & 7) != 0 ? 1 : 0);
    if (count == 0)
      return;
    int num = this.m_InputStream.Read(this.m_temp_buffer, 0, count);
    for (int index = 0; index < num; ++index)
    {
      this.m_Buffer |= (uint) this.m_temp_buffer[index] << this.m_BufferedBits;
      this.m_BufferedBits += 8;
    }
  }

  protected internal int PeekBits(int count)
  {
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count), "Bits count can not be less than zero.");
    if (count > 32 /*0x20*/)
      throw new ArgumentOutOfRangeException(nameof (count), "Count of bits is too large.");
    if (this.m_BufferedBits < count)
      this.FillBuffer();
    return this.m_BufferedBits < count ? -1 : (int) this.m_Buffer & ~(-1 << count);
  }

  protected internal void SkipBits(int count)
  {
    if (count < 0)
      throw new ArgumentOutOfRangeException(nameof (count), "Bits count can not be less than zero.");
    if (count == 0)
      return;
    if (count >= this.m_BufferedBits)
    {
      count -= this.m_BufferedBits;
      this.m_BufferedBits = 0;
      this.m_Buffer = 0U;
      if (count <= 0)
        return;
      this.m_InputStream.Position += (long) (count >> 3);
      count &= 7;
      if (count <= 0)
        return;
      this.FillBuffer();
      this.m_BufferedBits -= count;
      this.m_Buffer >>= count;
    }
    else
    {
      this.m_BufferedBits -= count;
      this.m_Buffer >>= count;
    }
  }

  protected internal int ReadBits(int count)
  {
    int num = this.PeekBits(count);
    if (num == -1)
      return -1;
    this.m_BufferedBits -= count;
    this.m_Buffer >>= count;
    return num;
  }

  protected internal int ReadInt16() => this.ReadBits(8) << 8 | this.ReadBits(8);

  protected internal int ReadInt16Inverted() => this.ReadBits(8) | this.ReadBits(8) << 8;

  protected internal long ReadInt32()
  {
    return (long) (uint) (this.ReadBits(8) << 24) | (long) (uint) (this.ReadBits(8) << 16 /*0x10*/) | (long) (uint) (this.ReadBits(8) << 8) | (long) (uint) this.ReadBits(8);
  }

  protected void ReadZLibHeader()
  {
    int num = this.ReadInt16();
    if (num == -1)
      throw new Exception("Header of the stream can not be read.");
    if (num % 31 /*0x1F*/ != 0)
      throw new FormatException("Header checksum illegal");
    this.m_WindowSize = (num & 3840 /*0x0F00*/) == 2048 /*0x0800*/ ? (int) Math.Pow(2.0, (double) (((num & 61440 /*0xF000*/) >> 12) + 8)) : throw new FormatException("Unsupported compression method.");
    if (this.m_WindowSize > (int) ushort.MaxValue)
      throw new FormatException("Unsupported window size for deflate compression method.");
    if ((num & 32 /*0x20*/) >> 5 == 1)
      throw new NotImplementedException("Custom dictionary is not supported at the moment.");
  }

  protected string BitsToString(int bits, int count)
  {
    string str = "";
    for (int index = 0; index < count; ++index)
    {
      if ((index & 7) == 0)
        str = " " + str;
      str = (bits & 1).ToString() + str;
      bits >>= 1;
    }
    return str;
  }

  protected void DecodeDynHeader(
    out DecompressorHuffmanTree lengthTree,
    out DecompressorHuffmanTree distanceTree)
  {
    byte num1 = 0;
    int num2 = this.ReadBits(5);
    int num3 = this.ReadBits(5);
    int num4 = this.ReadBits(4);
    if (num2 < 0 || num3 < 0 || num4 < 0)
      throw new FormatException("Wrong dynamic huffman codes.");
    int length1 = num2 + 257;
    int length2 = num3 + 1;
    int length3 = length1 + length2;
    byte[] sourceArray = new byte[length3];
    byte[] codeLengths = new byte[19];
    int num5 = num4 + 4;
    int num6;
    for (int index = 0; index < num5; codeLengths[Utils.DEF_HUFFMAN_DYNTREE_CODELENGTHS_ORDER[index++]] = (byte) num6)
    {
      num6 = this.ReadBits(3);
      if (num6 < 0)
        throw new FormatException("Wrong dynamic huffman codes.");
    }
    DecompressorHuffmanTree decompressorHuffmanTree = new DecompressorHuffmanTree(codeLengths);
    int num7 = 0;
    do
    {
      bool flag = false;
      int num8;
      while (((num8 = decompressorHuffmanTree.UnpackSymbol(this)) & -16) == 0)
      {
        sourceArray[num7++] = num1 = (byte) num8;
        if (num7 == length3)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        if (num8 < 0)
          throw new FormatException("Wrong dynamic huffman codes.");
        if (num8 >= 17)
          num1 = (byte) 0;
        else if (num7 == 0)
          throw new FormatException("Wrong dynamic huffman codes.");
        int index = num8 - 16 /*0x10*/;
        int num9 = this.ReadBits(CompressedStreamReader.DEF_HUFFMAN_DYNTREE_REPEAT_BITS[index]);
        if (num9 < 0)
          throw new FormatException("Wrong dynamic huffman codes.");
        int num10 = num9 + CompressedStreamReader.DEF_HUFFMAN_DYNTREE_REPEAT_MINIMUMS[index];
        if (num7 + num10 > length3)
          throw new FormatException("Wrong dynamic huffman codes.");
        while (num10-- > 0)
          sourceArray[num7++] = num1;
      }
      else
        break;
    }
    while (num7 != length3);
    byte[] numArray1 = new byte[length1];
    Array.Copy((Array) sourceArray, 0, (Array) numArray1, 0, length1);
    lengthTree = new DecompressorHuffmanTree(numArray1);
    byte[] numArray2 = new byte[length2];
    Array.Copy((Array) sourceArray, length1, (Array) numArray2, 0, length2);
    distanceTree = new DecompressorHuffmanTree(numArray2);
  }

  protected bool DecodeBlockHeader()
  {
    if (!this.m_bCanReadNextBlock)
      return false;
    int num1 = this.ReadBits(1);
    if (num1 == -1)
      return false;
    int num2 = this.ReadBits(2);
    if (num2 == -1)
      return false;
    this.m_bCanReadNextBlock = num1 == 0;
    switch (num2)
    {
      case 0:
        this.m_bReadingUncompressed = true;
        this.SkipToBoundary();
        int num3 = this.ReadInt16Inverted();
        int num4 = this.ReadInt16Inverted();
        if (num3 != (num4 ^ (int) ushort.MaxValue))
          throw new FormatException("Wrong block length.");
        this.m_UncompressedDataLength = num3 <= (int) ushort.MaxValue ? num3 : throw new FormatException("Uncompressed block length can not be more than 65535.");
        this.m_CurrentLengthTree = (DecompressorHuffmanTree) null;
        this.m_CurrentDistanceTree = (DecompressorHuffmanTree) null;
        break;
      case 1:
        this.m_bReadingUncompressed = false;
        this.m_UncompressedDataLength = -1;
        this.m_CurrentLengthTree = DecompressorHuffmanTree.LengthTree;
        this.m_CurrentDistanceTree = DecompressorHuffmanTree.DistanceTree;
        break;
      case 2:
        this.m_bReadingUncompressed = false;
        this.m_UncompressedDataLength = -1;
        this.DecodeDynHeader(out this.m_CurrentLengthTree, out this.m_CurrentDistanceTree);
        break;
      default:
        throw new FormatException("Wrong block type.");
    }
    return true;
  }

  private bool ReadHuffman()
  {
    int num1 = (int) ushort.MaxValue - (int) (this.m_DataLength - this.m_CurrentPosition);
    bool flag = false;
    while (num1 >= 258)
    {
      int num2;
      while (((num2 = this.m_CurrentLengthTree.UnpackSymbol(this)) & -256) == 0)
      {
        this.m_Block_Buffer[(IntPtr) (this.m_DataLength++ % (long) ushort.MaxValue)] = (byte) num2;
        flag = true;
        if (--num1 < 258)
          return true;
      }
      if (num2 < 257)
      {
        if (num2 < 256 /*0x0100*/)
          throw new FormatException("Illegal code.");
        return flag | (this.m_bCanReadMoreData = this.DecodeBlockHeader());
      }
      int num3 = num2 <= 285 ? CompressedStreamReader.DEF_HUFFMAN_REPEAT_LENGTH_BASE[num2 - 257] : throw new FormatException("Illegal repeat code length.");
      int count1 = CompressedStreamReader.DEF_HUFFMAN_REPEAT_LENGTH_EXTENSION[num2 - 257];
      if (count1 > 0)
      {
        int num4 = this.ReadBits(count1);
        if (num4 < 0)
          throw new FormatException("Wrong data.");
        num3 += num4;
      }
      int index1 = this.m_CurrentDistanceTree.UnpackSymbol(this);
      if (index1 < 0 || index1 > CompressedStreamReader.DEF_HUFFMAN_REPEAT_DISTANCE_BASE.Length)
        throw new FormatException("Wrong distance code.");
      int num5 = CompressedStreamReader.DEF_HUFFMAN_REPEAT_DISTANCE_BASE[index1];
      int count2 = CompressedStreamReader.DEF_HUFFMAN_REPEAT_DISTANCE_EXTENSION[index1];
      if (count2 > 0)
      {
        int num6 = this.ReadBits(count2);
        if (num6 < 0)
          throw new FormatException("Wrong data.");
        num5 += num6;
      }
      for (int index2 = 0; index2 < num3; ++index2)
      {
        this.m_Block_Buffer[this.m_DataLength % (long) ushort.MaxValue] = this.m_Block_Buffer[(this.m_DataLength - (long) num5) % (long) ushort.MaxValue];
        ++this.m_DataLength;
        --num1;
      }
      flag = true;
    }
    return flag;
  }

  public int Read(byte[] buffer, int offset, int length)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    if (offset < 0 || offset > buffer.Length - 1)
      throw new ArgumentOutOfRangeException(nameof (offset), "Offset does not belong to specified buffer.");
    if (length < 0 || length > buffer.Length - offset)
      throw new ArgumentOutOfRangeException(nameof (length), "Length is illegal.");
    int num1 = length;
    while (length > 0)
    {
      if (this.m_CurrentPosition < this.m_DataLength)
      {
        int sourceIndex = (int) (this.m_CurrentPosition % (long) ushort.MaxValue);
        int length1 = Math.Min(Math.Min((int) ushort.MaxValue - sourceIndex, (int) (this.m_DataLength - this.m_CurrentPosition)), length);
        Array.Copy((Array) this.m_Block_Buffer, sourceIndex, (Array) buffer, offset, length1);
        this.m_CurrentPosition += (long) length1;
        offset += length1;
        length -= length1;
      }
      else if (this.m_bCanReadMoreData)
      {
        long dataLength = this.m_DataLength;
        if (!this.m_bReadingUncompressed)
        {
          if (!this.ReadHuffman())
            break;
        }
        else if (this.m_UncompressedDataLength == 0)
        {
          if (!(this.m_bCanReadMoreData = this.DecodeBlockHeader()))
            break;
        }
        else
        {
          int offset1 = (int) (this.m_DataLength % (long) ushort.MaxValue);
          int length2 = Math.Min(this.m_UncompressedDataLength, (int) ushort.MaxValue - offset1);
          int num2 = this.ReadPackedBytes(this.m_Block_Buffer, offset1, length2);
          if (length2 != num2)
            throw new FormatException("Not enough data in stream.");
          this.m_UncompressedDataLength -= num2;
          this.m_DataLength += (long) num2;
        }
        if (dataLength < this.m_DataLength)
        {
          int offset2 = (int) (dataLength % (long) ushort.MaxValue);
          int length3 = (int) (this.m_DataLength % (long) ushort.MaxValue);
          if (offset2 < length3)
          {
            this.ChecksumUpdate(this.m_Block_Buffer, offset2, length3 - offset2);
          }
          else
          {
            this.ChecksumUpdate(this.m_Block_Buffer, offset2, (int) ushort.MaxValue - offset2);
            if (length3 > 0)
              this.ChecksumUpdate(this.m_Block_Buffer, 0, length3);
          }
        }
      }
      else
        break;
    }
    if (!this.m_bCanReadMoreData && !this.m_bCheckSumRead && !this.m_bNoWrap)
    {
      this.SkipToBoundary();
      if (this.ReadInt32() != this.m_CheckSum)
        throw new Exception("Checksum check failed.");
      this.m_bCheckSumRead = true;
    }
    return num1 - length;
  }
}
