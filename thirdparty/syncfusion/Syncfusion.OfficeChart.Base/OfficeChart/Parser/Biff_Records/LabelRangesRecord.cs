// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.LabelRangesRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.LabelRanges)]
internal class LabelRangesRecord : BiffRecordRawWithArray
{
  [BiffRecordPos(0, 2)]
  private ushort m_usRowRangesCount;
  private TAddr[] m_arrRowRanges;
  private ushort m_usColRangesCount;
  private TAddr[] m_arrColRanges;

  public ushort RowRangesCount => this.m_usRowRangesCount;

  public TAddr[] RowRanges
  {
    get => this.m_arrRowRanges;
    set
    {
      this.m_arrRowRanges = value;
      this.m_usRowRangesCount = value != null ? (ushort) value.Length : (ushort) 0;
    }
  }

  public ushort ColRangesCount => this.m_usColRangesCount;

  public TAddr[] ColRanges
  {
    get => this.m_arrColRanges;
    set
    {
      this.m_arrColRanges = value;
      this.m_usColRangesCount = value != null ? (ushort) value.Length : (ushort) 0;
    }
  }

  public override int MinimumRecordSize => 4;

  public LabelRangesRecord()
  {
  }

  public LabelRangesRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public LabelRangesRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
    this.AutoExtractFields();
    this.m_arrRowRanges = new TAddr[(int) this.m_usRowRangesCount];
    int offset1 = 2;
    int index1 = 0;
    while (index1 < (int) this.m_usRowRangesCount)
    {
      this.m_arrRowRanges[index1] = this.GetAddr(offset1);
      ++index1;
      offset1 += 8;
    }
    this.m_usColRangesCount = this.GetUInt16(offset1);
    this.m_arrColRanges = new TAddr[(int) this.m_usColRangesCount];
    int offset2 = offset1 + 2;
    int index2 = 0;
    while (index2 < (int) this.m_usColRangesCount)
    {
      this.m_arrColRanges[index2] = this.GetAddr(offset2);
      ++index2;
      offset2 += 8;
    }
    if (offset2 != this.m_iLength)
      throw new WrongBiffRecordDataException();
  }

  public override void InfillInternalData(OfficeVersion version)
  {
    this.AutoExtractFields();
    int offset1 = 2;
    int index1 = 0;
    while (index1 < (int) this.m_usRowRangesCount)
    {
      this.SetAddr(offset1, this.m_arrRowRanges[index1]);
      ++index1;
      offset1 += 8;
    }
    this.SetUInt16(offset1, this.m_usColRangesCount);
    int offset2 = offset1 + 2;
    int index2 = 0;
    while (index2 < (int) this.m_usColRangesCount)
    {
      this.SetAddr(offset2, this.m_arrColRanges[index2]);
      ++index2;
      offset2 += 8;
    }
  }
}
