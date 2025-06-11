// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.DConRefRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.DCONRef)]
[CLSCompliant(false)]
internal class DConRefRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usFirstRow;
  [BiffRecordPos(2, 2)]
  private ushort m_usLastRow;
  [BiffRecordPos(4, 1)]
  private byte m_btFirstColumn;
  [BiffRecordPos(5, 1)]
  private byte m_btLastColumn;
  [BiffRecordPos(6, TFieldType.String16Bit)]
  private string m_strWorkbookName;

  public DConRefRecord()
  {
  }

  public DConRefRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DConRefRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort FirstRow
  {
    get => this.m_usFirstRow;
    set => this.m_usFirstRow = value;
  }

  public ushort LastRow
  {
    get => this.m_usLastRow;
    set => this.m_usLastRow = value;
  }

  public byte FirstColumn
  {
    get => this.m_btFirstColumn;
    set => this.m_btFirstColumn = value;
  }

  public byte LastColumn
  {
    get => this.m_btLastColumn;
    set => this.m_btLastColumn = value;
  }

  public string WorkbookName
  {
    get => this.m_strWorkbookName;
    set => this.m_strWorkbookName = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usFirstRow = provider.ReadUInt16(iOffset);
    this.m_usLastRow = provider.ReadUInt16(iOffset + 2);
    this.m_btFirstColumn = provider.ReadByte(iOffset + 4);
    this.m_btLastColumn = provider.ReadByte(iOffset + 5);
    this.m_strWorkbookName = provider.ReadString16Bit(iOffset + 6, out int _);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usFirstRow);
    provider.WriteUInt16(iOffset + 2, this.m_usLastRow);
    provider.WriteByte(iOffset + 4, this.m_btFirstColumn);
    provider.WriteByte(iOffset + 5, this.m_btLastColumn);
    provider.WriteString16Bit(iOffset + 6, this.m_strWorkbookName);
    this.m_iLength = this.GetStoreSize(version);
  }

  public override int GetStoreSize(OfficeVersion version) => 9 + this.m_strWorkbookName.Length * 2;
}
