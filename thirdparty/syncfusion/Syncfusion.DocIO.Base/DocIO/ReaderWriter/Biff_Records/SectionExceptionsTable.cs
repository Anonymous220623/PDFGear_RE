// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.SectionExceptionsTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class SectionExceptionsTable : BaseWordRecord
{
  private int[] m_arrPositions;
  private SectionDescriptor[] m_arrDescriptors;

  internal SectionExceptionsTable()
  {
  }

  internal SectionExceptionsTable(byte[] arrData)
    : base(arrData)
  {
  }

  internal SectionExceptionsTable(Stream stream, int iCount)
    : base(stream, iCount)
  {
  }

  internal override void Close()
  {
    base.Close();
    this.m_arrPositions = (int[]) null;
    this.m_arrDescriptors = (SectionDescriptor[]) null;
  }

  internal override void Parse(byte[] data, int iOffset, int iCount)
  {
    if (data == null)
      throw new ArgumentNullException("arrData");
    if (iOffset != 0)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    if (iCount < 0)
      throw new ArgumentOutOfRangeException(nameof (iCount));
    if (iOffset + iCount > data.Length)
      throw new ArgumentOutOfRangeException("iOffset + iCount");
    int length = data.Length / 16 /*0x10*/;
    this.m_arrPositions = new int[length + 1];
    this.m_arrDescriptors = new SectionDescriptor[length];
    iOffset = (length + 1) * 4;
    Buffer.BlockCopy((Array) data, 0, (Array) this.m_arrPositions, 0, iOffset);
    int index = 0;
    while (index < length)
    {
      this.m_arrDescriptors[index] = new SectionDescriptor(data, iOffset);
      ++index;
      iOffset += 12;
    }
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    int length1 = this.Length;
    if (iOffset < 0 || iOffset + length1 > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    int num = iOffset;
    int count = this.m_arrPositions.Length * 4;
    Buffer.BlockCopy((Array) this.m_arrPositions, 0, (Array) arrData, iOffset, count);
    iOffset += count;
    int index = 0;
    for (int length2 = this.m_arrDescriptors.Length; index < length2; ++index)
    {
      this.m_arrDescriptors[index].Save(arrData, iOffset);
      iOffset += this.m_arrDescriptors[index].Length;
    }
    return iOffset - num;
  }

  internal int[] Positions => this.m_arrPositions;

  internal SectionDescriptor[] Descriptors => this.m_arrDescriptors;

  internal override int Length
  {
    get
    {
      int length1 = this.m_arrPositions.Length * 4;
      int index = 0;
      for (int length2 = this.m_arrDescriptors.Length; index < length2; ++index)
        length1 += this.m_arrDescriptors[index].Length;
      return length1;
    }
  }

  internal int EntriesCount
  {
    get => this.m_arrDescriptors != null ? this.m_arrDescriptors.Length : 0;
    set
    {
      if (value < 0)
        throw new ArgumentOutOfRangeException(nameof (EntriesCount));
      if (value == this.EntriesCount)
        return;
      this.m_arrDescriptors = new SectionDescriptor[value];
      this.m_arrPositions = new int[value + 1];
      for (int index = 0; index < value; ++index)
        this.m_arrDescriptors[index] = new SectionDescriptor();
    }
  }
}
