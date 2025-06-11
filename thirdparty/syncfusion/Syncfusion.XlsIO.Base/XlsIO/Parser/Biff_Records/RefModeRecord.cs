// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.RefModeRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.RefMode)]
[CLSCompliant(false)]
public class RefModeRecord : BiffRecordRaw
{
  private const int DEF_RECORD_SIZE = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usRefMode = 1;

  public ushort IsA1ReferenceMode
  {
    get => this.m_usRefMode;
    set => this.m_usRefMode = value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public RefModeRecord()
  {
  }

  public RefModeRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public RefModeRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usRefMode = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = 2;
    provider.WriteUInt16(iOffset, this.m_usRefMode);
  }
}
