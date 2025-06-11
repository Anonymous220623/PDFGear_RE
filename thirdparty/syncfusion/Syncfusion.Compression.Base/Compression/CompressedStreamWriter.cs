// Decompiled with JetBrains decompiler
// Type: Syncfusion.Compression.CompressedStreamWriter
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Compression;

public class CompressedStreamWriter
{
  private const int DEF_ZLIB_HEADER_TEMPLATE = 30720;
  private const int DEFAULT_MEM_LEVEL = 8;
  private const int DEF_PENDING_BUFFER_SIZE = 65536 /*0x010000*/;
  private const int DEF_HUFFMAN_BUFFER_SIZE = 16384 /*0x4000*/;
  private const int DEF_HUFFMAN_LITERAL_ALPHABET_LENGTH = 286;
  private const int DEF_HUFFMAN_DISTANCES_ALPHABET_LENGTH = 30;
  private const int DEF_HUFFMAN_BITLEN_TREE_LENGTH = 19;
  private const int DEF_HUFFMAN_ENDBLOCK_SYMBOL = 256 /*0x0100*/;
  private const int TOO_FAR = 4096 /*0x1000*/;
  private const int WSIZE = 32768 /*0x8000*/;
  public const int WMASK = 32767 /*0x7FFF*/;
  public const int HASH_BITS = 15;
  public const int HASH_SIZE = 32768 /*0x8000*/;
  public const int HASH_MASK = 32767 /*0x7FFF*/;
  public const int MAX_MATCH = 258;
  public const int MIN_MATCH = 3;
  public const int HASH_SHIFT = 5;
  public const int MIN_LOOKAHEAD = 262;
  public const int MAX_DIST = 32506;
  public static int[] GOOD_LENGTH = new int[10]
  {
    0,
    4,
    4,
    4,
    4,
    8,
    8,
    8,
    32 /*0x20*/,
    32 /*0x20*/
  };
  public static int[] MAX_LAZY = new int[10]
  {
    0,
    4,
    5,
    6,
    4,
    16 /*0x10*/,
    16 /*0x10*/,
    32 /*0x20*/,
    128 /*0x80*/,
    258
  };
  public static int[] NICE_LENGTH = new int[10]
  {
    0,
    8,
    16 /*0x10*/,
    32 /*0x20*/,
    16 /*0x10*/,
    32 /*0x20*/,
    128 /*0x80*/,
    128 /*0x80*/,
    258,
    258
  };
  public static int[] MAX_CHAIN = new int[10]
  {
    0,
    4,
    8,
    32 /*0x20*/,
    16 /*0x10*/,
    32 /*0x20*/,
    128 /*0x80*/,
    256 /*0x0100*/,
    1024 /*0x0400*/,
    4096 /*0x1000*/
  };
  public static int[] COMPR_FUNC = new int[10]
  {
    0,
    1,
    1,
    1,
    1,
    2,
    2,
    2,
    2,
    2
  };
  public static int MAX_BLOCK_SIZE = Math.Min((int) ushort.MaxValue, 65531);
  private Stream m_stream;
  private byte[] m_PendingBuffer = new byte[65536 /*0x010000*/];
  private int m_PendingBufferLength;
  private uint m_PendingBufferBitsCache;
  private int m_PendingBufferBitsInCache;
  private bool m_bNoWrap;
  private long m_CheckSum = 1;
  private CompressionLevel m_Level;
  private CompressorHuffmanTree m_treeLiteral;
  private CompressorHuffmanTree m_treeDistances;
  private CompressorHuffmanTree m_treeCodeLengths;
  private int m_iBufferPosition;
  private byte[] m_arrLiteralsBuffer;
  private short[] m_arrDistancesBuffer;
  private int m_iExtraBits;
  private static short[] m_arrLiteralCodes = new short[286];
  private static byte[] m_arrLiteralLengths = new byte[286];
  private static short[] m_arrDistanceCodes;
  private static byte[] m_arrDistanceLengths;
  private bool m_bStreamClosed;
  private int m_CurrentHash;
  private short[] m_HashHead;
  private short[] m_HashPrevious;
  private int m_MatchStart;
  private int m_MatchLength;
  private bool m_MatchPreviousAvailable;
  private int m_BlockStart;
  private int m_StringStart;
  private int m_LookAhead;
  private byte[] m_DataWindow;
  private int m_MaximumChainLength;
  private int m_MaximumLazySearch;
  private int m_NiceLength;
  private int m_GoodLength;
  private int m_CompressionFunction;
  private byte[] m_InputBuffer;
  private int m_TotalBytesIn;
  private int m_InputOffset;
  private int m_InputEnd;
  private bool m_bCloseStream;

  static CompressedStreamWriter()
  {
    int index1;
    for (index1 = 0; index1 < 144 /*0x90*/; CompressedStreamWriter.m_arrLiteralLengths[index1++] = (byte) 8)
      CompressedStreamWriter.m_arrLiteralCodes[index1] = Utils.BitReverse(48 /*0x30*/ + index1 << 8);
    for (; index1 < 256 /*0x0100*/; CompressedStreamWriter.m_arrLiteralLengths[index1++] = (byte) 9)
      CompressedStreamWriter.m_arrLiteralCodes[index1] = Utils.BitReverse(256 /*0x0100*/ + index1 << 7);
    for (; index1 < 280; CompressedStreamWriter.m_arrLiteralLengths[index1++] = (byte) 7)
      CompressedStreamWriter.m_arrLiteralCodes[index1] = Utils.BitReverse(index1 - 256 /*0x0100*/ << 9);
    for (; index1 < 286; CompressedStreamWriter.m_arrLiteralLengths[index1++] = (byte) 8)
      CompressedStreamWriter.m_arrLiteralCodes[index1] = Utils.BitReverse(index1 - 88 << 8);
    CompressedStreamWriter.m_arrDistanceCodes = new short[30];
    CompressedStreamWriter.m_arrDistanceLengths = new byte[30];
    for (int index2 = 0; index2 < 30; ++index2)
    {
      CompressedStreamWriter.m_arrDistanceCodes[index2] = Utils.BitReverse(index2 << 11);
      CompressedStreamWriter.m_arrDistanceLengths[index2] = (byte) 5;
    }
  }

  public CompressedStreamWriter(
    Stream outputStream,
    bool bNoWrap,
    CompressionLevel level,
    bool bCloseStream)
  {
    if (outputStream == null)
      throw new ArgumentNullException(nameof (outputStream));
    if (!outputStream.CanWrite)
      throw new ArgumentException("Output stream does not support writing.", nameof (outputStream));
    this.m_treeLiteral = new CompressorHuffmanTree(this, 286, 257, 15);
    this.m_treeDistances = new CompressorHuffmanTree(this, 30, 1, 15);
    this.m_treeCodeLengths = new CompressorHuffmanTree(this, 19, 4, 7);
    this.m_arrDistancesBuffer = new short[16384 /*0x4000*/];
    this.m_arrLiteralsBuffer = new byte[16384 /*0x4000*/];
    this.m_stream = outputStream;
    this.m_Level = level;
    this.m_bNoWrap = bNoWrap;
    this.m_bCloseStream = bCloseStream;
    this.m_DataWindow = new byte[65536 /*0x010000*/];
    this.m_HashHead = new short[32768 /*0x8000*/];
    this.m_HashPrevious = new short[32768 /*0x8000*/];
    this.m_BlockStart = this.m_StringStart = 1;
    this.m_GoodLength = CompressedStreamWriter.GOOD_LENGTH[(int) level];
    this.m_MaximumLazySearch = CompressedStreamWriter.MAX_LAZY[(int) level];
    this.m_NiceLength = CompressedStreamWriter.NICE_LENGTH[(int) level];
    this.m_MaximumChainLength = CompressedStreamWriter.MAX_CHAIN[(int) level];
    this.m_CompressionFunction = CompressedStreamWriter.COMPR_FUNC[(int) level];
    if (bNoWrap)
      return;
    this.WriteZLIBHeader();
  }

  public CompressedStreamWriter(Stream outputStream, bool bNoWrap, bool bCloseStream)
    : this(outputStream, bNoWrap, CompressionLevel.Normal, bCloseStream)
  {
  }

  public CompressedStreamWriter(Stream outputStream, CompressionLevel level, bool bCloseStream)
    : this(outputStream, false, level, bCloseStream)
  {
  }

  public CompressedStreamWriter(Stream outputStream, bool bCloseStream)
    : this(outputStream, false, CompressionLevel.Normal, bCloseStream)
  {
  }

  public void Write(byte[] data, int offset, int length, bool bCloseAfterWrite)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    int num = offset + length;
    if (0 > offset || offset > num || num > data.Length)
      throw new ArgumentOutOfRangeException("Offset or length is incorrect.");
    this.m_InputBuffer = data;
    this.m_InputOffset = offset;
    this.m_InputEnd = num;
    if (length == 0)
      return;
    if (this.m_bStreamClosed)
      throw new IOException("Stream was closed.");
    ChecksumCalculator.ChecksumUpdate(ref this.m_CheckSum, this.m_InputBuffer, this.m_InputOffset, length);
    while (!this.NeedsInput || !this.PendingBufferIsFlushed)
    {
      this.PendingBufferFlush();
      if (!this.CompressData(bCloseAfterWrite) && bCloseAfterWrite)
      {
        this.PendingBufferFlush();
        this.PendingBufferAlignToByte();
        if (!this.m_bNoWrap)
        {
          this.PendingBufferWriteShortMSB((int) (this.m_CheckSum >> 16 /*0x10*/));
          this.PendingBufferWriteShortMSB((int) (this.m_CheckSum & (long) ushort.MaxValue));
        }
        this.PendingBufferFlush();
        this.m_bStreamClosed = true;
        if (this.m_bCloseStream)
          this.m_stream.Close();
      }
    }
  }

  public void Close()
  {
    if (this.m_bStreamClosed)
      return;
    do
    {
      this.PendingBufferFlush();
      if (!this.CompressData(true))
      {
        this.PendingBufferFlush();
        this.PendingBufferAlignToByte();
        if (!this.m_bNoWrap)
        {
          this.PendingBufferWriteShortMSB((int) (this.m_CheckSum >> 16 /*0x10*/));
          this.PendingBufferWriteShortMSB((int) (this.m_CheckSum & (long) ushort.MaxValue));
        }
        this.PendingBufferFlush();
      }
    }
    while (!this.NeedsInput || !this.PendingBufferIsFlushed);
    this.m_bStreamClosed = true;
    if (!this.m_bCloseStream)
      return;
    this.m_stream.Close();
  }

  public int TotalIn => this.m_TotalBytesIn;

  private bool NeedsInput => this.m_InputEnd == this.m_InputOffset;

  private bool HuffmanIsFull => this.m_iBufferPosition >= 16384 /*0x4000*/;

  private void WriteZLIBHeader()
  {
    int num = 30720 | ((int) this.m_Level >> 2 & 3) << 6;
    this.PendingBufferWriteShortMSB(num + (31 /*0x1F*/ - num % 31 /*0x1F*/));
  }

  private void FillWindow()
  {
    if (this.m_StringStart >= 65274)
      this.SlideWindow();
    int length;
    for (; this.m_LookAhead < 262 && this.m_InputOffset < this.m_InputEnd; this.m_LookAhead += length)
    {
      length = 65536 /*0x010000*/ - this.m_LookAhead - this.m_StringStart;
      if (length > this.m_InputEnd - this.m_InputOffset)
        length = this.m_InputEnd - this.m_InputOffset;
      Array.Copy((Array) this.m_InputBuffer, this.m_InputOffset, (Array) this.m_DataWindow, this.m_StringStart + this.m_LookAhead, length);
      this.m_InputOffset += length;
      this.m_TotalBytesIn += length;
    }
    if (this.m_LookAhead < 3)
      return;
    this.UpdateHash();
  }

  private void SlideWindow()
  {
    Array.Copy((Array) this.m_DataWindow, 32768 /*0x8000*/, (Array) this.m_DataWindow, 0, 32768 /*0x8000*/);
    this.m_MatchStart -= 32768 /*0x8000*/;
    this.m_StringStart -= 32768 /*0x8000*/;
    this.m_BlockStart -= 32768 /*0x8000*/;
    for (int index = 0; index < 32768 /*0x8000*/; ++index)
    {
      int num = (int) this.m_HashHead[index] & (int) ushort.MaxValue;
      this.m_HashHead[index] = num >= 32768 /*0x8000*/ ? (short) (num - 32768 /*0x8000*/) : (short) 0;
    }
    for (int index = 0; index < 32768 /*0x8000*/; ++index)
    {
      int num = (int) this.m_HashPrevious[index] & (int) ushort.MaxValue;
      this.m_HashPrevious[index] = num >= 32768 /*0x8000*/ ? (short) (num - 32768 /*0x8000*/) : (short) 0;
    }
  }

  private void UpdateHash()
  {
    this.m_CurrentHash = (int) this.m_DataWindow[this.m_StringStart] << 5 ^ (int) this.m_DataWindow[this.m_StringStart + 1];
  }

  private int InsertString()
  {
    int index = (this.m_CurrentHash << 5 ^ (int) this.m_DataWindow[this.m_StringStart + 2]) & (int) short.MaxValue;
    short num;
    this.m_HashPrevious[this.m_StringStart & (int) short.MaxValue] = num = this.m_HashHead[index];
    this.m_HashHead[index] = (short) this.m_StringStart;
    this.m_CurrentHash = index;
    return (int) num & (int) ushort.MaxValue;
  }

  private bool FindLongestMatch(int curMatch)
  {
    int maximumChainLength = this.m_MaximumChainLength;
    int num1 = this.m_NiceLength;
    short[] hashPrevious = this.m_HashPrevious;
    int stringStart = this.m_StringStart;
    int index = this.m_StringStart + this.m_MatchLength;
    int val1 = Math.Max(this.m_MatchLength, 2);
    int num2 = Math.Max(this.m_StringStart - 32506, 0);
    int num3 = this.m_StringStart + 258 - 1;
    byte num4 = this.m_DataWindow[index - 1];
    byte num5 = this.m_DataWindow[index];
    if (val1 >= this.m_GoodLength)
      maximumChainLength >>= 2;
    if (num1 > this.m_LookAhead)
      num1 = this.m_LookAhead;
    do
    {
      if ((int) this.m_DataWindow[curMatch + val1] == (int) num5 && (int) this.m_DataWindow[curMatch + val1 - 1] == (int) num4 && (int) this.m_DataWindow[curMatch] == (int) this.m_DataWindow[stringStart] && (int) this.m_DataWindow[curMatch + 1] == (int) this.m_DataWindow[stringStart + 1])
      {
        int num6 = curMatch + 2;
        int num7 = stringStart + 2;
        int num8;
        int num9;
        int num10;
        int num11;
        int num12;
        int num13;
        int num14;
        do
          ;
        while ((int) this.m_DataWindow[++num7] == (int) this.m_DataWindow[num8 = num6 + 1] && (int) this.m_DataWindow[++num7] == (int) this.m_DataWindow[num9 = num8 + 1] && (int) this.m_DataWindow[++num7] == (int) this.m_DataWindow[num10 = num9 + 1] && (int) this.m_DataWindow[++num7] == (int) this.m_DataWindow[num11 = num10 + 1] && (int) this.m_DataWindow[++num7] == (int) this.m_DataWindow[num12 = num11 + 1] && (int) this.m_DataWindow[++num7] == (int) this.m_DataWindow[num13 = num12 + 1] && (int) this.m_DataWindow[++num7] == (int) this.m_DataWindow[num14 = num13 + 1] && (int) this.m_DataWindow[++num7] == (int) this.m_DataWindow[num6 = num14 + 1] && num7 < num3);
        if (num7 > index)
        {
          this.m_MatchStart = curMatch;
          index = num7;
          val1 = num7 - this.m_StringStart;
          if (val1 < num1)
          {
            num4 = this.m_DataWindow[index - 1];
            num5 = this.m_DataWindow[index];
          }
          else
            break;
        }
        stringStart = this.m_StringStart;
      }
    }
    while ((curMatch = (int) hashPrevious[curMatch & (int) short.MaxValue] & (int) ushort.MaxValue) > num2 && --maximumChainLength != 0);
    this.m_MatchLength = Math.Min(val1, this.m_LookAhead);
    return this.m_MatchLength >= 3;
  }

  private bool SaveStored(bool flush, bool finish)
  {
    if (!flush && this.m_LookAhead == 0)
      return false;
    this.m_StringStart += this.m_LookAhead;
    this.m_LookAhead = 0;
    int storedLength = this.m_StringStart - this.m_BlockStart;
    if (storedLength < CompressedStreamWriter.MAX_BLOCK_SIZE && (this.m_BlockStart >= 32768 /*0x8000*/ || storedLength < 32506) && !flush)
      return true;
    bool lastBlock = finish;
    if (storedLength > CompressedStreamWriter.MAX_BLOCK_SIZE)
    {
      storedLength = CompressedStreamWriter.MAX_BLOCK_SIZE;
      lastBlock = false;
    }
    this.HuffmanFlushStoredBlock(this.m_DataWindow, this.m_BlockStart, storedLength, lastBlock);
    this.m_BlockStart += storedLength;
    return !lastBlock;
  }

  private bool CompressFast(bool flush, bool finish)
  {
    if (this.m_LookAhead < 262 && !flush)
      return false;
    while (this.m_LookAhead >= 262 || flush)
    {
      if (this.m_LookAhead == 0)
      {
        this.HuffmanFlushBlock(this.m_DataWindow, this.m_BlockStart, this.m_StringStart - this.m_BlockStart, finish);
        this.m_BlockStart = this.m_StringStart;
        return false;
      }
      if (this.m_StringStart > 65274)
        this.SlideWindow();
      int curMatch;
      if (this.m_LookAhead >= 3 && (curMatch = this.InsertString()) != 0 && this.m_StringStart - curMatch <= 32506 && this.FindLongestMatch(curMatch))
      {
        if (this.HuffmanTallyDist(this.m_StringStart - this.m_MatchStart, this.m_MatchLength))
        {
          this.HuffmanFlushBlock(this.m_DataWindow, this.m_BlockStart, this.m_StringStart - this.m_BlockStart, finish && this.m_LookAhead == 0);
          this.m_BlockStart = this.m_StringStart;
        }
        this.m_LookAhead -= this.m_MatchLength;
        if (this.m_MatchLength <= this.m_MaximumLazySearch && this.m_LookAhead >= 3)
        {
          while (--this.m_MatchLength > 0)
          {
            ++this.m_StringStart;
            this.InsertString();
          }
          ++this.m_StringStart;
        }
        else
        {
          this.m_StringStart += this.m_MatchLength;
          if (this.m_LookAhead >= 2)
            this.UpdateHash();
        }
        this.m_MatchLength = 2;
      }
      else
      {
        this.HuffmanTallyLit((int) this.m_DataWindow[this.m_StringStart] & (int) byte.MaxValue);
        ++this.m_StringStart;
        --this.m_LookAhead;
        if (this.HuffmanIsFull)
        {
          bool lastBlock = finish && this.m_LookAhead == 0;
          this.HuffmanFlushBlock(this.m_DataWindow, this.m_BlockStart, this.m_StringStart - this.m_BlockStart, lastBlock);
          this.m_BlockStart = this.m_StringStart;
          return !lastBlock;
        }
      }
    }
    return true;
  }

  private bool CompressSlow(bool flush, bool finish)
  {
    if (this.m_LookAhead < 262 && !flush)
      return false;
    while (this.m_LookAhead >= 262 || flush)
    {
      if (this.m_LookAhead == 0)
      {
        if (this.m_MatchPreviousAvailable)
          this.HuffmanTallyLit((int) this.m_DataWindow[this.m_StringStart - 1] & (int) byte.MaxValue);
        this.m_MatchPreviousAvailable = false;
        this.HuffmanFlushBlock(this.m_DataWindow, this.m_BlockStart, this.m_StringStart - this.m_BlockStart, finish);
        this.m_BlockStart = this.m_StringStart;
        return false;
      }
      if (this.m_StringStart >= 65274)
        this.SlideWindow();
      int matchStart = this.m_MatchStart;
      int matchLength = this.m_MatchLength;
      if (this.m_LookAhead >= 3)
      {
        int curMatch = this.InsertString();
        if (curMatch != 0 && this.m_StringStart - curMatch <= 32506 && this.FindLongestMatch(curMatch) && this.m_MatchLength <= 5 && this.m_MatchLength == 3 && this.m_StringStart - this.m_MatchStart > 4096 /*0x1000*/)
          this.m_MatchLength = 2;
      }
      if (matchLength >= 3 && this.m_MatchLength <= matchLength)
      {
        this.HuffmanTallyDist(this.m_StringStart - 1 - matchStart, matchLength);
        int num = matchLength - 2;
        do
        {
          ++this.m_StringStart;
          --this.m_LookAhead;
          if (this.m_LookAhead >= 3)
            this.InsertString();
        }
        while (--num > 0);
        ++this.m_StringStart;
        --this.m_LookAhead;
        this.m_MatchPreviousAvailable = false;
        this.m_MatchLength = 2;
      }
      else
      {
        if (this.m_MatchPreviousAvailable)
          this.HuffmanTallyLit((int) this.m_DataWindow[this.m_StringStart - 1] & (int) byte.MaxValue);
        this.m_MatchPreviousAvailable = true;
        ++this.m_StringStart;
        --this.m_LookAhead;
      }
      if (this.HuffmanIsFull)
      {
        int storedLength = this.m_StringStart - this.m_BlockStart;
        if (this.m_MatchPreviousAvailable)
          --storedLength;
        bool lastBlock = finish && this.m_LookAhead == 0 && !this.m_MatchPreviousAvailable;
        this.HuffmanFlushBlock(this.m_DataWindow, this.m_BlockStart, storedLength, lastBlock);
        this.m_BlockStart += storedLength;
        return !lastBlock;
      }
    }
    return true;
  }

  private bool CompressData(bool finish)
  {
    bool flag;
    do
    {
      this.FillWindow();
      bool flush = finish && this.NeedsInput;
      switch (this.m_CompressionFunction)
      {
        case 0:
          flag = this.SaveStored(flush, finish);
          break;
        case 1:
          flag = this.CompressFast(flush, finish);
          break;
        case 2:
          flag = this.CompressSlow(flush, finish);
          break;
        default:
          throw new InvalidOperationException("unknown m_CompressionFunction");
      }
    }
    while (this.PendingBufferIsFlushed && flag);
    return flag;
  }

  private void HuffmanReset()
  {
    this.m_iBufferPosition = 0;
    this.m_iExtraBits = 0;
    this.m_treeLiteral.Reset();
    this.m_treeDistances.Reset();
    this.m_treeCodeLengths.Reset();
  }

  private int HuffmanLengthCode(int len)
  {
    if (len == (int) byte.MaxValue)
      return 285;
    int num = 257;
    for (; len >= 8; len >>= 1)
      num += 4;
    return num + len;
  }

  private int HuffmanDistanceCode(int distance)
  {
    int num = 0;
    for (; distance >= 4; distance >>= 1)
      num += 2;
    return num + distance;
  }

  private void HuffmanSendAllTrees(int blTreeCodes)
  {
    this.m_treeCodeLengths.BuildCodes();
    this.m_treeLiteral.BuildCodes();
    this.m_treeDistances.BuildCodes();
    this.PendingBufferWriteBits(this.m_treeLiteral.TreeLength - 257, 5);
    this.PendingBufferWriteBits(this.m_treeDistances.TreeLength - 1, 5);
    this.PendingBufferWriteBits(blTreeCodes - 4, 4);
    for (int index = 0; index < blTreeCodes; ++index)
      this.PendingBufferWriteBits((int) this.m_treeCodeLengths.CodeLengths[Utils.DEF_HUFFMAN_DYNTREE_CODELENGTHS_ORDER[index]], 3);
    this.m_treeLiteral.WriteTree(this.m_treeCodeLengths);
    this.m_treeDistances.WriteTree(this.m_treeCodeLengths);
  }

  private void HuffmanCompressBlock()
  {
    for (int index = 0; index < this.m_iBufferPosition; ++index)
    {
      int num1 = (int) this.m_arrLiteralsBuffer[index] & (int) byte.MaxValue;
      int num2 = (int) this.m_arrDistancesBuffer[index];
      int distance = num2 - 1;
      if (num2 != 0)
      {
        int code1 = this.HuffmanLengthCode(num1);
        this.m_treeLiteral.WriteCodeToStream(code1);
        int count1 = (code1 - 261) / 4;
        if (count1 > 0 && count1 <= 5)
          this.PendingBufferWriteBits(num1 & (1 << count1) - 1, count1);
        int code2 = this.HuffmanDistanceCode(distance);
        this.m_treeDistances.WriteCodeToStream(code2);
        int count2 = code2 / 2 - 1;
        if (count2 > 0)
          this.PendingBufferWriteBits(distance & (1 << count2) - 1, count2);
      }
      else
        this.m_treeLiteral.WriteCodeToStream(num1);
    }
    this.m_treeLiteral.WriteCodeToStream(256 /*0x0100*/);
  }

  private void HuffmanFlushStoredBlock(
    byte[] stored,
    int storedOffset,
    int storedLength,
    bool lastBlock)
  {
    this.PendingBufferWriteBits(lastBlock ? 1 : 0, 3);
    this.PendingBufferAlignToByte();
    this.PendingBufferWriteShort(storedLength);
    this.PendingBufferWriteShort(~storedLength);
    this.PendingBufferWriteByteBlock(stored, storedOffset, storedLength);
    this.HuffmanReset();
  }

  private void HuffmanFlushBlock(
    byte[] stored,
    int storedOffset,
    int storedLength,
    bool lastBlock)
  {
    ++this.m_treeLiteral.CodeFrequences[256 /*0x0100*/];
    this.m_treeLiteral.BuildTree();
    this.m_treeDistances.BuildTree();
    this.m_treeLiteral.CalcBLFreq(this.m_treeCodeLengths);
    this.m_treeDistances.CalcBLFreq(this.m_treeCodeLengths);
    this.m_treeCodeLengths.BuildTree();
    int blTreeCodes = 4;
    for (int index = 18; index > blTreeCodes; --index)
    {
      if (this.m_treeCodeLengths.CodeLengths[Utils.DEF_HUFFMAN_DYNTREE_CODELENGTHS_ORDER[index]] > (byte) 0)
        blTreeCodes = index + 1;
    }
    int num = 14 + blTreeCodes * 3 + this.m_treeCodeLengths.GetEncodedLength() + this.m_treeLiteral.GetEncodedLength() + this.m_treeDistances.GetEncodedLength() + this.m_iExtraBits;
    int iExtraBits = this.m_iExtraBits;
    for (int index = 0; index < 286; ++index)
      iExtraBits += (int) this.m_treeLiteral.CodeFrequences[index] * (int) CompressedStreamWriter.m_arrLiteralLengths[index];
    for (int index = 0; index < 30; ++index)
      iExtraBits += (int) this.m_treeDistances.CodeFrequences[index] * (int) CompressedStreamWriter.m_arrDistanceLengths[index];
    if (num >= iExtraBits)
      num = iExtraBits;
    if (storedOffset >= 0 && storedLength + 4 < num >> 3)
      this.HuffmanFlushStoredBlock(stored, storedOffset, storedLength, lastBlock);
    else if (num == iExtraBits)
    {
      this.PendingBufferWriteBits(2 + (lastBlock ? 1 : 0), 3);
      this.m_treeLiteral.SetStaticCodes(CompressedStreamWriter.m_arrLiteralCodes, CompressedStreamWriter.m_arrLiteralLengths);
      this.m_treeDistances.SetStaticCodes(CompressedStreamWriter.m_arrDistanceCodes, CompressedStreamWriter.m_arrDistanceLengths);
      this.HuffmanCompressBlock();
      this.HuffmanReset();
    }
    else
    {
      this.PendingBufferWriteBits(4 + (lastBlock ? 1 : 0), 3);
      this.HuffmanSendAllTrees(blTreeCodes);
      this.HuffmanCompressBlock();
      this.HuffmanReset();
    }
  }

  private bool HuffmanTallyLit(int literal)
  {
    this.m_arrDistancesBuffer[this.m_iBufferPosition] = (short) 0;
    this.m_arrLiteralsBuffer[this.m_iBufferPosition++] = (byte) literal;
    ++this.m_treeLiteral.CodeFrequences[literal];
    return this.HuffmanIsFull;
  }

  private bool HuffmanTallyDist(int dist, int len)
  {
    this.m_arrDistancesBuffer[this.m_iBufferPosition] = (short) dist;
    this.m_arrLiteralsBuffer[this.m_iBufferPosition++] = (byte) (len - 3);
    int index1 = this.HuffmanLengthCode(len - 3);
    ++this.m_treeLiteral.CodeFrequences[index1];
    if (index1 >= 265 && index1 < 285)
      this.m_iExtraBits += (index1 - 261) / 4;
    int index2 = this.HuffmanDistanceCode(dist - 1);
    ++this.m_treeDistances.CodeFrequences[index2];
    if (index2 >= 4)
      this.m_iExtraBits += index2 / 2 - 1;
    return this.HuffmanIsFull;
  }

  internal void PendingBufferWriteByte(int b)
  {
    this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) b;
  }

  internal void PendingBufferWriteShort(int s)
  {
    this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) s;
    this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) (s >> 8);
  }

  internal void PendingBufferWriteInt(int s)
  {
    this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) s;
    this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) (s >> 8);
    this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) (s >> 16 /*0x10*/);
    this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) (s >> 24);
  }

  internal void PendingBufferWriteByteBlock(byte[] data, int offset, int length)
  {
    Array.Copy((Array) data, offset, (Array) this.m_PendingBuffer, this.m_PendingBufferLength, length);
    this.m_PendingBufferLength += length;
  }

  internal int PendingBufferBitCount => this.m_PendingBufferBitsInCache;

  internal void PendingBufferAlignToByte()
  {
    if (this.m_PendingBufferBitsInCache > 0)
    {
      this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) this.m_PendingBufferBitsCache;
      if (this.m_PendingBufferBitsInCache > 8)
        this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) (this.m_PendingBufferBitsCache >> 8);
    }
    this.m_PendingBufferBitsCache = 0U;
    this.m_PendingBufferBitsInCache = 0;
  }

  internal void PendingBufferWriteBits(int b, int count)
  {
    this.m_PendingBufferBitsCache |= (uint) (b << this.m_PendingBufferBitsInCache);
    this.m_PendingBufferBitsInCache += count;
    this.PendingBufferFlushBits();
  }

  internal void PendingBufferWriteShortMSB(int s)
  {
    this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) (s >> 8);
    this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) s;
  }

  internal bool PendingBufferIsFlushed => this.m_PendingBufferLength == 0;

  internal void PendingBufferFlush()
  {
    this.PendingBufferFlushBits();
    this.m_stream.Write(this.m_PendingBuffer, 0, this.m_PendingBufferLength);
    this.m_PendingBufferLength = 0;
    this.m_stream.Flush();
  }

  internal int PendingBufferFlushBits()
  {
    int num = 0;
    while (this.m_PendingBufferBitsInCache >= 8 && this.m_PendingBufferLength < 65536 /*0x010000*/)
    {
      this.m_PendingBuffer[this.m_PendingBufferLength++] = (byte) this.m_PendingBufferBitsCache;
      this.m_PendingBufferBitsCache >>= 8;
      this.m_PendingBufferBitsInCache -= 8;
      ++num;
    }
    return num;
  }

  internal byte[] PendingBufferToByteArray()
  {
    byte[] destinationArray = new byte[this.m_PendingBufferLength];
    Array.Copy((Array) this.m_PendingBuffer, 0, (Array) destinationArray, 0, destinationArray.Length);
    this.m_PendingBufferLength = 0;
    return destinationArray;
  }

  private enum BlockType
  {
    Stored,
    FixedHuffmanCodes,
    DynamicHuffmanCodes,
  }
}
