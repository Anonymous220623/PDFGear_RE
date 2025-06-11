// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.ReferencePositionTable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class ReferencePositionTable : BaseWordRecord
{
  private int[] m_arrPositions;
  private ushort[] m_arrNumbers;

  internal ReferencePositionTable()
  {
  }

  internal ReferencePositionTable(byte[] data)
    : base(data)
  {
  }

  internal ReferencePositionTable(byte[] arrData, int iOffset)
    : base(arrData, iOffset)
  {
  }

  internal ReferencePositionTable(byte[] arrData, int iOffset, int iCount)
    : base(arrData, iOffset, iCount)
  {
  }

  internal ReferencePositionTable(Stream stream, int iCount)
    : base(stream, iCount)
  {
  }

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset > arrData.Length - 1)
      throw new ArgumentOutOfRangeException(nameof (iOffset), "Value can not be less than 0 and greater than arrData.Length - 1");
    if (iCount < 0 || iOffset + iCount > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    if (iCount == 0)
    {
      this.m_arrNumbers = (ushort[]) null;
      this.m_arrPositions = (int[]) null;
    }
    else
    {
      int length = (iCount - 4) / 6;
      this.m_arrPositions = new int[length + 1];
      this.m_arrNumbers = new ushort[length];
      int count = (length + 1) * 4;
      Buffer.BlockCopy((Array) arrData, iOffset, (Array) this.m_arrPositions, 0, count);
      iOffset += count;
      Buffer.BlockCopy((Array) arrData, iOffset, (Array) this.m_arrNumbers, 0, length * 2);
    }
  }

  internal int[] Positions => this.m_arrPositions;

  internal ushort[] Numbers => this.m_arrNumbers;

  internal override int Length => this.m_arrNumbers.Length * 2 + this.m_arrPositions.Length * 4;
}
