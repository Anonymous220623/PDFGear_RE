// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.DimensionsRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Dimensions)]
public class DimensionsRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 14;
  [BiffRecordPos(0, 4, true)]
  private int m_iFirstRow;
  [BiffRecordPos(4, 4, true)]
  private int m_iLastRow;
  [BiffRecordPos(8, 2)]
  private ushort m_usFirstColumn;
  [BiffRecordPos(10, 2)]
  private ushort m_usLastColumn;
  [BiffRecordPos(12, 2)]
  private ushort m_usReserved;

  public ushort Reserved => this.m_usReserved;

  public int FirstRow
  {
    get => this.m_iFirstRow;
    set => this.m_iFirstRow = value;
  }

  public int LastRow
  {
    get => this.m_iLastRow;
    set => this.m_iLastRow = value;
  }

  public ushort FirstColumn
  {
    get => this.m_usFirstColumn;
    set => this.m_usFirstColumn = value;
  }

  public ushort LastColumn
  {
    get => this.m_usLastColumn;
    set => this.m_usLastColumn = value;
  }

  public override int MinimumRecordSize => 14;

  public override int MaximumRecordSize => 14;

  public DimensionsRecord()
  {
  }

  public DimensionsRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DimensionsRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_iFirstRow = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_iLastRow = provider.ReadInt32(iOffset);
    iOffset += 4;
    this.m_usFirstColumn = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usLastColumn = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usReserved = provider.ReadUInt16(iOffset);
    if ((int) this.m_usLastColumn > (int) this.m_usFirstColumn)
      return;
    this.m_usLastColumn = (ushort) ((uint) this.m_usFirstColumn + 1U);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = 14;
    provider.WriteInt32(iOffset, this.m_iFirstRow);
    iOffset += 4;
    provider.WriteInt32(iOffset, this.m_iLastRow);
    iOffset += 4;
    provider.WriteUInt16(iOffset, this.m_usFirstColumn);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usLastColumn);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usReserved);
  }
}
