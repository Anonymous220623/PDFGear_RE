// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotBooleanRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotBoolean)]
public class PivotBooleanRecord : BiffRecordRaw, IValueHolder
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usValue;

  public PivotBooleanRecord()
  {
  }

  public PivotBooleanRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotBooleanRecord(int iReserve)
    : base(iReserve)
  {
  }

  public bool Value
  {
    get => this.m_usValue == (ushort) 1;
    set => this.m_usValue = value ? (ushort) 1 : (ushort) 0;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usValue = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usValue);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;

  object IValueHolder.Value
  {
    get => (object) this.Value;
    set => this.Value = (bool) value;
  }
}
