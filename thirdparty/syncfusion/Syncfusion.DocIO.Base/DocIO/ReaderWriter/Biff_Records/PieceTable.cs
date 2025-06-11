// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.PieceTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class PieceTable : BaseWordRecord
{
  private uint[] m_arrFC;
  private PieceDescriptorRecord[] m_arrEntry;

  internal PieceTable()
  {
  }

  internal PieceTable(byte[] arrData)
    : base(arrData)
  {
  }

  internal override void Close()
  {
    base.Close();
    this.m_arrFC = (uint[]) null;
    this.m_arrEntry = (PieceDescriptorRecord[]) null;
  }

  internal override void Parse(byte[] arrData)
  {
    int length = (arrData.Length - 4) / 12;
    this.m_arrFC = new uint[length + 1];
    this.m_arrEntry = new PieceDescriptorRecord[length];
    int count = (length + 1) * 4;
    Buffer.BlockCopy((Array) arrData, 0, (Array) this.m_arrFC, 0, count);
    int iOffset = count;
    int index = 0;
    while (index < length)
    {
      this.m_arrEntry[index] = new PieceDescriptorRecord();
      this.m_arrEntry[index].ParseBytes(arrData, iOffset);
      ++index;
      iOffset += 8;
    }
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    int length = this.Length;
    if (iOffset < 0 || iOffset + length > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    int num = iOffset;
    arrData[iOffset++] = (byte) 2;
    BitConverter.GetBytes(length - 1 - 4).CopyTo((Array) arrData, iOffset);
    iOffset += 4;
    if (this.EntriesCount > 0)
    {
      int count = this.m_arrFC.Length * 4;
      Buffer.BlockCopy((Array) this.m_arrFC, 0, (Array) arrData, iOffset, count);
      iOffset += count;
      int index = 0;
      for (int entriesCount = this.EntriesCount; index < entriesCount; ++index)
        iOffset += this.m_arrEntry[index].Save(arrData, iOffset);
    }
    return iOffset - num;
  }

  internal uint[] FileCharacterPos => this.m_arrFC;

  internal PieceDescriptorRecord[] Entries => this.m_arrEntry;

  internal override int Length
  {
    get
    {
      int length1 = this.m_arrFC.Length * 4 + 1 + 4;
      int index = 0;
      for (int length2 = this.m_arrEntry.Length; index < length2; ++index)
        length1 += this.m_arrEntry[index].Length;
      return length1;
    }
  }

  internal int EntriesCount
  {
    get => this.m_arrEntry == null ? 0 : this.m_arrEntry.Length;
    set
    {
      if (value < 0)
        throw new ArgumentOutOfRangeException(nameof (EntriesCount));
      if (value == this.EntriesCount)
        return;
      this.m_arrEntry = new PieceDescriptorRecord[value];
      this.m_arrFC = new uint[value + 1];
      for (int index = 0; index < value; ++index)
        this.m_arrEntry[index] = new PieceDescriptorRecord();
    }
  }
}
