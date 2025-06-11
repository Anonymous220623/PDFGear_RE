// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.HorizontalPageBreaksRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.HorizontalPageBreaks)]
[CLSCompliant(false)]
internal class HorizontalPageBreaksRecord : BiffRecordRawWithArray
{
  private const int DEF_FIXED_PART_SIZE = 2;
  internal const int DEF_SUBITEM_SIZE = 6;
  internal const int FixedSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usBreaksCount;
  private HorizontalPageBreaksRecord.THPageBreak[] m_arrPageBreaks;

  public HorizontalPageBreaksRecord.THPageBreak[] PageBreaks
  {
    get => this.m_arrPageBreaks;
    set
    {
      this.m_arrPageBreaks = value;
      this.m_usBreaksCount = value != null ? (ushort) value.Length : (ushort) 0;
    }
  }

  public override int MinimumRecordSize => 2;

  public HorizontalPageBreaksRecord()
  {
  }

  public HorizontalPageBreaksRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public HorizontalPageBreaksRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
    this.m_usBreaksCount = this.GetUInt16(0);
    this.m_arrPageBreaks = new HorizontalPageBreaksRecord.THPageBreak[(int) this.m_usBreaksCount];
    int offset = 2;
    int index = 0;
    while (index < (int) this.m_usBreaksCount)
    {
      ushort uint16_1 = this.GetUInt16(offset);
      ushort uint16_2 = this.GetUInt16(offset + 2);
      ushort uint16_3 = this.GetUInt16(offset + 4);
      this.m_arrPageBreaks[index] = new HorizontalPageBreaksRecord.THPageBreak(uint16_1, uint16_2, uint16_3);
      ++index;
      offset += 6;
    }
    if (offset != this.m_iLength)
      throw new WrongBiffRecordDataException();
  }

  public override void InfillInternalData(OfficeVersion version)
  {
    this.m_data = new byte[this.GetStoreSize(OfficeVersion.Excel97to2003)];
    this.SetUInt16(0, this.m_usBreaksCount);
    this.m_iLength = 2;
    int index = 0;
    while (index < (int) this.m_usBreaksCount)
    {
      this.SetUInt16(this.m_iLength, this.m_arrPageBreaks[index].Row);
      this.SetUInt16(this.m_iLength + 2, this.m_arrPageBreaks[index].StartColumn);
      this.SetUInt16(this.m_iLength + 4, this.m_arrPageBreaks[index].EndColumn);
      ++index;
      this.m_iLength += 6;
    }
  }

  public override int GetStoreSize(OfficeVersion version) => 2 + 6 * (int) this.m_usBreaksCount;

  public class THPageBreak : ICloneable
  {
    private ushort m_usRow;
    private ushort m_usStartCol;
    private ushort m_usEndCol;

    public THPageBreak()
    {
    }

    public THPageBreak(ushort Row, ushort StartCol, ushort EndCol)
    {
      this.m_usRow = Row;
      this.m_usStartCol = StartCol;
      this.m_usEndCol = EndCol;
    }

    public ushort Row
    {
      get => this.m_usRow;
      set => this.m_usRow = value;
    }

    public ushort StartColumn
    {
      get => this.m_usStartCol;
      set => this.m_usStartCol = value;
    }

    public ushort EndColumn
    {
      get => this.m_usEndCol;
      set => this.m_usEndCol = value;
    }

    public object Clone() => this.MemberwiseClone();
  }
}
