// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotFormulaRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.PivotFormula)]
[CLSCompliant(false)]
public class PivotFormulaRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 4;
  [BiffRecordPos(0, 2)]
  private ushort m_usReserved;
  [BiffRecordPos(2, 2, true)]
  private short m_usAppliedField;

  public PivotFormulaRecord()
  {
  }

  public PivotFormulaRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotFormulaRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Reserved => this.m_usReserved;

  public short AppliedField
  {
    get => this.m_usAppliedField;
    set => this.m_usAppliedField = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usReserved = provider.ReadUInt16(iOffset);
    this.m_usAppliedField = provider.ReadInt16(iOffset + 2);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usReserved);
    provider.WriteInt16(iOffset + 2, this.m_usAppliedField);
    this.m_iLength = 4;
  }

  public override int GetStoreSize(ExcelVersion version) => 4;
}
