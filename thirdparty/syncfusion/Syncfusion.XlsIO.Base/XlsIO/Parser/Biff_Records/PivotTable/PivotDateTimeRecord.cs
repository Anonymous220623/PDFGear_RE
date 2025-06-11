// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotDateTimeRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.PivotDateTime)]
[CLSCompliant(false)]
public class PivotDateTimeRecord : BiffRecordRaw, IValueHolder
{
  private const int DefaultRecordSize = 8;
  [BiffRecordPos(0, 2)]
  private ushort m_usYear;
  [BiffRecordPos(2, 2)]
  private ushort m_usMonth;
  [BiffRecordPos(4, 1)]
  private byte m_btDay;
  [BiffRecordPos(5, 1)]
  private byte m_btHour;
  [BiffRecordPos(6, 1)]
  private byte m_btMinute;
  [BiffRecordPos(7, 1)]
  private byte m_btSecond;

  public PivotDateTimeRecord()
  {
  }

  public PivotDateTimeRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotDateTimeRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Year
  {
    get => this.m_usYear;
    set => this.m_usYear = value;
  }

  public ushort Month
  {
    get => this.m_usMonth;
    set
    {
      this.m_usMonth = value >= (ushort) 1 && value <= (ushort) 12 ? value : throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less 1 and greater than 12");
    }
  }

  public byte Day
  {
    get => this.m_btDay;
    set
    {
      this.m_btDay = value >= (byte) 1 && value <= (byte) 31 /*0x1F*/ ? value : throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less 1 and greater than 31");
    }
  }

  public byte Hour
  {
    get => this.m_btHour;
    set
    {
      this.m_btHour = value >= (byte) 0 && value <= (byte) 23 ? value : throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less than 0 and greater than 23");
    }
  }

  public byte Minute
  {
    get => this.m_btMinute;
    set
    {
      this.m_btMinute = value >= (byte) 0 && value <= (byte) 59 ? value : throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less than 0 and greater than 59");
    }
  }

  public byte Second
  {
    get => this.m_btSecond;
    set
    {
      this.m_btSecond = value >= (byte) 0 && value <= (byte) 59 ? value : throw new ArgumentOutOfRangeException(nameof (value), "Value cannot be less than 0 and greater than 59");
    }
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usYear = provider.ReadUInt16(iOffset);
    this.m_usMonth = provider.ReadUInt16(iOffset + 2);
    this.m_btDay = provider.ReadByte(iOffset + 4);
    this.m_btHour = provider.ReadByte(iOffset + 5);
    this.m_btMinute = provider.ReadByte(iOffset + 6);
    this.m_btSecond = provider.ReadByte(iOffset + 7);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usYear);
    provider.WriteUInt16(iOffset + 2, this.m_usMonth);
    provider.WriteByte(iOffset + 4, this.m_btDay);
    provider.WriteByte(iOffset + 5, this.m_btHour);
    provider.WriteByte(iOffset + 6, this.m_btMinute);
    provider.WriteByte(iOffset + 7, this.m_btSecond);
    this.m_iLength = 8;
  }

  public override int GetStoreSize(ExcelVersion version) => 8;

  object IValueHolder.Value
  {
    get
    {
      return (object) new DateTime((int) this.Year, (int) this.Month, (int) this.Day, (int) this.Hour, (int) this.Minute, (int) this.Second, 0);
    }
    set
    {
      DateTime dateTime = (DateTime) value;
      this.Year = (ushort) dateTime.Year;
      this.Month = (ushort) (byte) dateTime.Month;
      this.Day = (byte) dateTime.Day;
      this.Hour = (byte) dateTime.Hour;
      this.Minute = (byte) dateTime.Minute;
      this.Second = (byte) dateTime.Second;
    }
  }
}
