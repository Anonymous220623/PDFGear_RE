// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.BinaryTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class BinaryTable : BaseWordRecord
{
  private uint[] m_arrFC;
  private BinTableEntry[] m_arrEntry;

  internal BinaryTable()
  {
  }

  internal BinaryTable(byte[] arrData)
    : base(arrData)
  {
  }

  internal BinaryTable(Stream stream, int iCount)
    : base(stream, iCount)
  {
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_arrFC != null)
      this.m_arrFC = (uint[]) null;
    if (this.m_arrEntry == null)
      return;
    this.m_arrEntry = (BinTableEntry[]) null;
  }

  internal override void Parse(byte[] arrData, int iOffset, int iLength)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset != 0)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value cannot be less  and greater ");
    if (iLength != arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iLength), "Value cannot be less  and greater ");
    int length = (iLength - 4) / 8;
    this.m_arrFC = new uint[length + 1];
    this.m_arrEntry = new BinTableEntry[length];
    int count = (length + 1) * 4;
    Buffer.BlockCopy((Array) arrData, 0, (Array) this.m_arrFC, 0, count);
    iOffset = count;
    for (int index = 0; index < length; ++index)
    {
      this.m_arrEntry[index] = new BinTableEntry();
      iOffset = this.m_arrEntry[index].Parse(arrData, iOffset);
    }
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    int count = 4 * this.m_arrFC.Length;
    int num1 = this.m_arrEntry.Length * 4;
    int num2 = count + num1;
    if (iOffset + num2 > arrData.Length)
      throw new ArgumentOutOfRangeException("arrData.Length");
    Buffer.BlockCopy((Array) this.m_arrFC, 0, (Array) arrData, iOffset, count);
    iOffset += count;
    int index = 0;
    for (int length = this.m_arrEntry.Length; index < length; ++index)
    {
      this.m_arrEntry[index].Save(arrData, iOffset);
      iOffset += 4;
    }
    return num2;
  }

  internal uint[] FileCharacterPos => this.m_arrFC;

  internal BinTableEntry[] Entries => this.m_arrEntry;

  internal int EntriesCount
  {
    get => this.m_arrEntry.Length;
    set
    {
      this.m_arrFC = value >= 0 ? new uint[value + 1] : throw new ArgumentOutOfRangeException("Length");
      this.m_arrEntry = new BinTableEntry[value];
    }
  }

  internal override int Length => 4 * this.m_arrFC.Length + this.m_arrEntry.Length * 4;
}
