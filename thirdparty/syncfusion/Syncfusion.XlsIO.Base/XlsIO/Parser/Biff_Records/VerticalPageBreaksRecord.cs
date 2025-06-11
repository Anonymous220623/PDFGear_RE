// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.VerticalPageBreaksRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.VerticalPageBreaks)]
[CLSCompliant(false)]
public class VerticalPageBreaksRecord : BiffRecordRaw
{
  internal const int DEF_FIXED_PART_SIZE = 2;
  internal const int DEF_SUBITEM_SIZE = 6;
  [BiffRecordPos(0, 2)]
  private ushort m_usBreaksCount;
  private VerticalPageBreaksRecord.TVPageBreak[] m_arrPageBreaks;

  public VerticalPageBreaksRecord.TVPageBreak[] PageBreaks
  {
    get => this.m_arrPageBreaks;
    set
    {
      this.m_arrPageBreaks = value;
      this.m_usBreaksCount = value != null ? (ushort) value.Length : (ushort) 0;
    }
  }

  public override int MinimumRecordSize => 2;

  public VerticalPageBreaksRecord()
  {
  }

  public VerticalPageBreaksRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public VerticalPageBreaksRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usBreaksCount = provider.ReadUInt16(iOffset);
    this.m_arrPageBreaks = new VerticalPageBreaksRecord.TVPageBreak[(int) this.m_usBreaksCount];
    iOffset += 2;
    int index = 0;
    while (index < (int) this.m_usBreaksCount)
    {
      ushort Col = provider.ReadUInt16(iOffset);
      ushort StartRow = provider.ReadUInt16(iOffset + 2);
      ushort EndRow = provider.ReadUInt16(iOffset + 4);
      this.m_arrPageBreaks[index] = new VerticalPageBreaksRecord.TVPageBreak(Col, StartRow, EndRow);
      ++index;
      iOffset += 6;
    }
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usBreaksCount);
    this.m_iLength = 2;
    int index = 0;
    while (index < (int) this.m_usBreaksCount)
    {
      provider.WriteUInt16(iOffset + this.m_iLength, this.m_arrPageBreaks[index].Column);
      provider.WriteUInt16(iOffset + this.m_iLength + 2, (ushort) this.m_arrPageBreaks[index].StartRow);
      provider.WriteUInt16(iOffset + this.m_iLength + 4, (ushort) this.m_arrPageBreaks[index].EndRow);
      ++index;
      this.m_iLength += 6;
    }
  }

  public override int GetStoreSize(ExcelVersion version) => 2 + 6 * (int) this.m_usBreaksCount;

  public class TVPageBreak : ICloneable
  {
    private ushort m_usCol;
    private uint m_uiStartRow;
    private uint m_uiEndRow;

    public TVPageBreak()
    {
    }

    public TVPageBreak(ushort Col, ushort StartRow, ushort EndRow)
    {
      this.m_usCol = Col;
      this.m_uiStartRow = (uint) StartRow;
      this.m_uiEndRow = (uint) EndRow;
    }

    public ushort Column
    {
      get => this.m_usCol;
      set => this.m_usCol = value;
    }

    public uint StartRow
    {
      get => this.m_uiStartRow;
      set => this.m_uiStartRow = value;
    }

    public uint EndRow
    {
      get => this.m_uiEndRow;
      set => this.m_uiEndRow = value;
    }

    public object Clone() => this.MemberwiseClone();
  }
}
