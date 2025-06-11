// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.BookmarkDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class BookmarkDescriptor
{
  private int DEF_STRUCT_SIZE = 4;
  private int m_bkmkCount;
  private List<BookmarkFirstStructure> m_bkfArr;
  private List<int> m_bklArr;
  private int m_lastEnd;

  internal int BookmarkCount => this.m_bkfArr.Count;

  private BookmarkFirstStructure[] BkfArray => this.m_bkfArr.ToArray();

  internal BookmarkDescriptor(
    Stream stream,
    int bookmarkCount,
    int bkfPos,
    int bkfLength,
    int bklPos,
    int bklLength)
  {
    this.m_bkmkCount = bookmarkCount;
    this.m_bkfArr = new List<BookmarkFirstStructure>(this.m_bkmkCount);
    this.m_bklArr = new List<int>(this.m_bkmkCount);
    this.ReadBKF(bkfPos, stream, bkfLength);
    this.ReadBKL(bklPos, stream, bklLength);
  }

  internal BookmarkDescriptor()
  {
    this.m_bkmkCount = 0;
    this.m_bkfArr = new List<BookmarkFirstStructure>(this.m_bkmkCount);
    for (int index = 0; index < this.BkfArray.Length; ++index)
      this.m_bkfArr.Add(new BookmarkFirstStructure());
    this.m_bklArr = new List<int>(this.m_bkmkCount);
  }

  internal int GetBeginPos(int i) => this.BkfArray[i].BeginPos;

  internal void SetBeginPos(int i, int position) => this.BkfArray[i].BeginPos = position;

  internal int GetEndPos(int i) => this.m_bklArr[(int) this.BkfArray[i].EndIndex];

  internal void SetEndPos(int i, int position)
  {
    if (this.BkfArray.Length <= i || this.m_bklArr.Count <= this.m_lastEnd)
      return;
    this.BkfArray[i].EndIndex = (short) this.m_lastEnd;
    this.m_bklArr[this.m_lastEnd] = position;
    ++this.m_lastEnd;
  }

  internal void Save(Stream stream, Fib fib, uint endChar)
  {
    if (this.BookmarkCount <= 0)
      return;
    this.WriteBKF(stream, fib, endChar);
    this.WriteBKL(stream, fib, endChar);
  }

  internal void Add(int startPos)
  {
    int count = this.m_bkfArr.Count;
    this.m_bkfArr.Add(new BookmarkFirstStructure());
    this.m_bklArr.Add(startPos);
    this.BkfArray[count].BeginPos = startPos;
  }

  internal bool IsCellGroup(int bookmarkIndex)
  {
    return (this.BkfArray[bookmarkIndex].Props & 32768 /*0x8000*/) >> 15 == 1;
  }

  internal void SetCellGroup(int bookmarkIndex, bool isCellGroup)
  {
    if (this.BkfArray.Length <= bookmarkIndex)
      return;
    this.BkfArray[bookmarkIndex].Props = isCellGroup ? (int) (short) BaseWordRecord.SetBitsByMask(this.BkfArray[bookmarkIndex].Props, 32768 /*0x8000*/, 15, 1) : (int) (short) BaseWordRecord.SetBitsByMask(this.BkfArray[bookmarkIndex].Props, 32768 /*0x8000*/, 15, 0);
  }

  internal short GetStartCellIndex(int bookmarkIndex)
  {
    return (short) (this.BkfArray[bookmarkIndex].Props & (int) sbyte.MaxValue);
  }

  internal void SetStartCellIndex(int bookmarkIndex, int position)
  {
    if (this.BkfArray.Length <= bookmarkIndex)
      return;
    this.BkfArray[bookmarkIndex].Props = (int) (short) BaseWordRecord.SetBitsByMask(this.BkfArray[bookmarkIndex].Props, (int) sbyte.MaxValue, position);
  }

  internal short GetEndCellIndex(int bookmarkIndex)
  {
    return (short) ((this.BkfArray[bookmarkIndex].Props & 32512) >> 8);
  }

  internal void SetEndCellIndex(int bookmarkIndex, int position)
  {
    if (this.BkfArray.Length <= bookmarkIndex)
      return;
    this.BkfArray[bookmarkIndex].Props = (int) (short) BaseWordRecord.SetBitsByMask(this.BkfArray[bookmarkIndex].Props, 32512, 8, position);
  }

  internal void Close()
  {
    if (this.m_bkfArr != null)
    {
      this.m_bkfArr.Clear();
      this.m_bkfArr = (List<BookmarkFirstStructure>) null;
    }
    if (this.m_bklArr == null)
      return;
    this.m_bklArr.Clear();
    this.m_bklArr = (List<int>) null;
  }

  private void ReadBKL(int bklPos, Stream stream, int bklLength)
  {
    stream.Position = (long) bklPos;
    byte[] numArray1 = new byte[this.m_bkmkCount * 4];
    stream.Read(numArray1, 0, numArray1.Length);
    int[] numArray2 = new int[this.m_bkmkCount];
    Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, numArray1.Length);
    this.m_bklArr.AddRange((IEnumerable<int>) numArray2);
    if (stream.Position > (long) (bklPos + bklLength))
      throw new StreamReadException("Too many bytes read for BookmarkLimDescriptor");
  }

  private void ReadBKF(int bkfPos, Stream stream, int bkfLength)
  {
    byte[] numArray1 = new byte[this.m_bkmkCount * this.DEF_STRUCT_SIZE];
    ushort[] numArray2 = new ushort[this.m_bkmkCount * 2];
    int[] dst1 = new int[this.m_bkmkCount];
    stream.Position = (long) bkfPos;
    stream.Read(numArray1, 0, this.m_bkmkCount * this.DEF_STRUCT_SIZE);
    Buffer.BlockCopy((Array) numArray1, 0, (Array) dst1, 0, numArray1.Length);
    for (int index = 0; index < this.m_bkmkCount; ++index)
    {
      this.m_bkfArr.Add(new BookmarkFirstStructure());
      this.BkfArray[index].BeginPos = dst1[index];
    }
    stream.Position += 4L;
    byte[] numArray3 = new byte[this.m_bkmkCount * 4];
    ushort[] dst2 = new ushort[this.m_bkmkCount * 2];
    stream.Read(numArray3, 0, numArray3.Length);
    Buffer.BlockCopy((Array) numArray3, 0, (Array) dst2, 0, numArray3.Length);
    for (int index = 0; index < this.m_bkmkCount; ++index)
    {
      this.BkfArray[index].EndIndex = (short) dst2[2 * index];
      this.BkfArray[index].Props = (int) dst2[2 * index + 1];
    }
    if (stream.Position > (long) (bkfPos + bkfLength))
      throw new StreamReadException("To many bytes read for BookmarkFirstDescriptor");
  }

  private void WriteBKF(Stream stream, Fib fib, uint endChar)
  {
    int bookmarkCount = this.BookmarkCount;
    uint position1 = (uint) stream.Position;
    for (int index = 0; index < this.BkfArray.Length; ++index)
    {
      byte[] buffer = this.BkfArray[index].SavePos();
      stream.Write(buffer, 0, buffer.Length);
    }
    byte[] bytes = BitConverter.GetBytes(endChar);
    stream.Write(bytes, 0, bytes.Length);
    for (int index = 0; index < this.BookmarkCount; ++index)
    {
      byte[] buffer = this.BkfArray[index].SaveProps();
      stream.Write(buffer, 0, buffer.Length);
    }
    uint position2 = (uint) stream.Position;
    if (position2 <= position1)
      return;
    fib.FibRgFcLcb97FcPlcfBkf = position1;
    fib.FibRgFcLcb97LcbPlcfBkf = position2 - position1;
  }

  private void WriteBKL(Stream stream, Fib fib, uint endChar)
  {
    byte[] numArray = new byte[this.BookmarkCount * this.DEF_STRUCT_SIZE];
    fib.FibRgFcLcb97LcbPlcfBkf = (uint) ((ulong) stream.Position - (ulong) fib.FibRgFcLcb97FcPlcfBkf);
    uint position1 = (uint) stream.Position;
    Buffer.BlockCopy((Array) this.m_bklArr.ToArray(), 0, (Array) numArray, 0, numArray.Length);
    stream.Write(numArray, 0, numArray.Length);
    byte[] bytes = BitConverter.GetBytes(endChar);
    stream.Write(bytes, 0, bytes.Length);
    uint position2 = (uint) stream.Position;
    if (position2 <= position1)
      return;
    fib.FibRgFcLcb97FcPlcfBkl = position1;
    fib.FibRgFcLcb97LcbPlcfBkl = position2 - position1;
  }
}
