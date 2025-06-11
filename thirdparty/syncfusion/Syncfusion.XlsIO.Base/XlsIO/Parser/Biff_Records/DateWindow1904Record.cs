// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.DateWindow1904Record
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.DateWindow1904)]
[CLSCompliant(false)]
public class DateWindow1904Record : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usWindow;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bIs1904Windowing;

  public ushort Windowing
  {
    get => this.m_usWindow;
    set => this.m_usWindow = value;
  }

  public bool Is1904Windowing
  {
    get => this.m_bIs1904Windowing;
    set => this.m_bIs1904Windowing = value;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public DateWindow1904Record()
  {
  }

  public DateWindow1904Record(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DateWindow1904Record(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usWindow = provider.ReadUInt16(iOffset);
    this.m_bIs1904Windowing = provider.ReadBit(iOffset, 0);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usWindow);
    provider.WriteBit(iOffset, this.m_bIs1904Windowing, 0);
    this.m_iLength = 2;
  }
}
