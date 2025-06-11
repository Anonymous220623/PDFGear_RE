// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.BreakDescriptorTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class BreakDescriptorTable : BaseWordRecord
{
  private uint[] m_arrFC;
  private BreakDescriptorRecord[] m_arrEntry;

  internal BreakDescriptorTable()
  {
  }

  internal BreakDescriptorTable(byte[] data)
    : base(data)
  {
  }

  internal BreakDescriptorTable(byte[] arrData, int iOffset)
    : base(arrData, iOffset)
  {
  }

  internal BreakDescriptorTable(byte[] arrData, int iOffset, int iCount)
    : base(arrData, iOffset, iCount)
  {
  }

  internal BreakDescriptorTable(Stream stream, int iCount)
    : base(stream, iCount)
  {
  }

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length - 1)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value can not be less than 0 and greater than arrData.Length - 1");
    if (iCount < 0 || iCount + iOffset > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    this.m_arrEntry = (BreakDescriptorRecord[]) null;
    this.m_arrFC = (uint[]) null;
    if (iCount == 0)
      return;
    int length = (iCount - 4) / 10;
    this.m_arrFC = new uint[length + 1];
    this.m_arrEntry = new BreakDescriptorRecord[length];
    int count = (length + 1) * 4;
    Buffer.BlockCopy((Array) arrData, 0, (Array) this.m_arrFC, 0, count);
    iOffset += count;
    int index = 0;
    while (index < length)
    {
      this.m_arrEntry[index] = new BreakDescriptorRecord(arrData, iOffset);
      ++index;
      iOffset += 6;
    }
  }

  internal override void Parse(Stream stream, int iCount)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (iCount < 0)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    this.m_arrEntry = (BreakDescriptorRecord[]) null;
    this.m_arrFC = (uint[]) null;
    if (iCount == 0)
      return;
    int length = (iCount - 4) / 10;
    this.m_arrFC = new uint[length + 1];
    this.m_arrEntry = new BreakDescriptorRecord[length];
    int count = (length + 1) * 4;
    byte[] numArray = new byte[count];
    stream.Read(numArray, 0, count);
    Buffer.BlockCopy((Array) numArray, 0, (Array) this.m_arrFC, 0, count);
    if (count < 6)
      numArray = new byte[6];
    for (int index = 0; index < length; ++index)
    {
      stream.Read(numArray, 0, 6);
      this.m_arrEntry[index] = new BreakDescriptorRecord(numArray, 0, 6);
    }
  }

  internal uint[] FileCharacterPos => this.m_arrFC;

  internal BreakDescriptorRecord[] Entries => this.m_arrEntry;

  internal override int Length
  {
    get
    {
      int length = 0;
      if (this.m_arrFC != null)
        length += this.m_arrFC.Length + 4;
      if (this.m_arrEntry != null)
        length += 6 * this.m_arrEntry.Length;
      return length;
    }
  }
}
