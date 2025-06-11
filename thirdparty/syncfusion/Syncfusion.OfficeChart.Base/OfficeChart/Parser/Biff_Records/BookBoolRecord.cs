// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.BookBoolRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.BookBool)]
internal class BookBoolRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usSaveLinkValue;

  public ushort SaveLinkValue
  {
    get => this.m_usSaveLinkValue;
    set => this.m_usSaveLinkValue = value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public BookBoolRecord()
  {
  }

  public BookBoolRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public BookBoolRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usSaveLinkValue = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usSaveLinkValue);
    this.m_iLength = 2;
  }
}
