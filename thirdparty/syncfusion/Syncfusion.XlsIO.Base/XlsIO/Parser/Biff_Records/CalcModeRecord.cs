// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.CalcModeRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.CalcMode)]
public class CalcModeRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usCalcMode = 1;

  public ExcelCalculationMode CalculationMode
  {
    get => (ExcelCalculationMode) this.m_usCalcMode;
    set => this.m_usCalcMode = (ushort) value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public CalcModeRecord()
  {
  }

  public CalcModeRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CalcModeRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usCalcMode = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usCalcMode);
    this.m_iLength = 2;
  }
}
