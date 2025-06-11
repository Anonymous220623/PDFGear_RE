// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.SheetCenterRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.HCenter)]
[Biff(TBIFFRecord.VCenter)]
public class SheetCenterRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usCenter;

  public ushort IsCenter
  {
    get => this.m_usCenter;
    set => this.m_usCenter = value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public SheetCenterRecord()
  {
  }

  public SheetCenterRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public SheetCenterRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usCenter = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usCenter);
  }
}
