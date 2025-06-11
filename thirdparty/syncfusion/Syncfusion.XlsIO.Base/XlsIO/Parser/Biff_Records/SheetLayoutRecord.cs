// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.SheetLayoutRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.SheetLayout)]
public class SheetLayoutRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 20;
  [BiffRecordPos(0, 2, true)]
  private short m_id;
  [BiffRecordPos(2, 4, true)]
  private int m_iReserved1;
  [BiffRecordPos(6, 4, true)]
  private int m_iReserved2;
  [BiffRecordPos(10, 2, true)]
  private short m_sReserved3;
  [BiffRecordPos(12, 4, true)]
  private int m_uSize = 20;
  [BiffRecordPos(16 /*0x10*/, 4, true)]
  private int m_iColorIndex;
  private short m_usOptions;
  private int m_colorType;
  private int m_colorValue;
  private double m_tintShade;

  public SheetLayoutRecord() => this.m_id = (short) this.TypeCode;

  public SheetLayoutRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public SheetLayoutRecord(int iReserve)
    : base(iReserve)
  {
  }

  public short Id
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  public int Reserved1
  {
    get => this.m_iReserved1;
    set => this.m_iReserved1 = value;
  }

  public int Reserved2
  {
    get => this.m_iReserved2;
    set => this.m_iReserved2 = value;
  }

  public short Reserved3
  {
    get => this.m_sReserved3;
    set => this.m_sReserved3 = value;
  }

  public int Unknown
  {
    get => this.m_uSize;
    set => this.m_uSize = value;
  }

  public int ColorIndex
  {
    get => this.m_iColorIndex;
    set => this.m_iColorIndex = value;
  }

  public override int MinimumRecordSize => 20;

  public override int MaximumRecordSize => 20;

  public override int MaximumMemorySize => 20;

  internal bool IsCondFmtCalc => this.IsBitEnabled((long) this.m_usOptions, 128 /*0x80*/);

  internal bool IsNotPublished => this.IsBitEnabled((long) this.m_usOptions, 256 /*0x0100*/);

  internal ColorType ColorType
  {
    get => (ColorType) this.m_colorType;
    set => this.m_colorType = (int) (byte) value;
  }

  internal int ColorValue
  {
    get => this.m_colorValue;
    set => this.m_colorValue = value;
  }

  internal double TintShade
  {
    get => this.m_tintShade;
    set => this.m_tintShade = value;
  }

  internal short Options
  {
    get => this.m_usOptions;
    set => this.m_usOptions = value;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_id = provider.ReadInt16(iOffset);
    this.m_iReserved1 = provider.ReadInt32(iOffset + 2);
    this.m_iReserved2 = provider.ReadInt32(iOffset + 6);
    this.m_sReserved3 = provider.ReadInt16(iOffset + 10);
    this.m_uSize = provider.ReadInt32(iOffset + 12);
    this.m_iColorIndex = provider.ReadInt32(iOffset + 16 /*0x10*/);
    if (this.m_uSize != 40)
      return;
    this.m_usOptions = provider.ReadInt16(iOffset + 20);
    this.m_colorType = provider.ReadInt32(iOffset + 24);
    this.m_colorValue = provider.ReadInt32(iOffset + 28);
    this.m_tintShade = (double) provider.ReadInt64(iOffset + 32 /*0x20*/);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteInt16(iOffset, this.m_id);
    provider.WriteInt32(iOffset + 2, this.m_iReserved1);
    provider.WriteInt32(iOffset + 6, this.m_iReserved2);
    provider.WriteInt16(iOffset + 10, this.m_sReserved3);
    provider.WriteInt32(iOffset + 12, this.m_uSize);
    provider.WriteInt32(iOffset + 16 /*0x10*/, this.m_iColorIndex);
    if (this.m_uSize == 40 && this.m_colorType != 0)
    {
      byte iColorIndex = (byte) this.m_iColorIndex;
      if (this.IsCondFmtCalc)
        iColorIndex |= (byte) 128 /*0x80*/;
      provider.WriteByte(iOffset + 20, iColorIndex);
      if (this.IsNotPublished)
        provider.WriteByte(iOffset + 21, (byte) 1);
      else
        provider.WriteByte(iOffset + 21, (byte) 0);
      provider.WriteInt16(iOffset + 22, (short) 0);
      provider.WriteInt32(iOffset + 24, this.m_colorType);
      provider.WriteInt32(iOffset + 28, this.m_colorValue);
      provider.WriteInt64(iOffset + 32 /*0x20*/, (long) this.m_tintShade);
      this.m_iLength = this.m_uSize;
    }
    else
      this.m_iLength = 20;
  }

  public override int GetStoreSize(ExcelVersion version) => this.m_uSize;

  internal bool IsBitEnabled(long inputValue, int compareValue)
  {
    return (inputValue & (long) compareValue) == (long) compareValue;
  }
}
