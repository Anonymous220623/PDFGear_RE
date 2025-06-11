// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords.ftSbs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
public class ftSbs : ObjSubRecord
{
  private const int DEF_RECORD_SIZE = 24;
  private static readonly byte[] DEF_SAMPLE_RECORD_DATA = new byte[20]
  {
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 1,
    (byte) 0,
    (byte) 8,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 16 /*0x10*/,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  private byte[] m_data;
  private int m_iValue;
  private int m_iMinimum;
  private int m_iMaximum;
  private int m_iIncrement;
  private int m_iPage;
  private int m_iHorizontal;
  private int m_iScrollBarWidth;
  private short m_sOptions;

  public byte[] Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  public int Value
  {
    get => this.m_iValue;
    set => this.m_iValue = value;
  }

  public int Minimum
  {
    get => this.m_iMinimum;
    set => this.m_iMinimum = value;
  }

  public int Maximum
  {
    get => this.m_iMaximum;
    set => this.m_iMaximum = value;
  }

  public int Increment
  {
    get => this.m_iIncrement;
    set => this.m_iIncrement = value;
  }

  public int Page
  {
    get => this.m_iPage;
    set => this.m_iPage = value;
  }

  public int Horizontal
  {
    get => this.m_iHorizontal;
    set => this.m_iHorizontal = value;
  }

  public int ScrollBarWidth
  {
    get => this.m_iScrollBarWidth;
    set => this.m_iScrollBarWidth = value;
  }

  [CLSCompliant(false)]
  public ftSbs()
    : base(TObjSubRecordType.ftSbs, (ushort) 20, ftSbs.DEF_SAMPLE_RECORD_DATA)
  {
  }

  public ftSbs(TObjSubRecordType type, ushort length, byte[] buffer)
    : base(type, length, buffer)
  {
  }

  protected override void Parse(byte[] buffer)
  {
    this.m_data = (byte[]) buffer.Clone();
    this.m_iValue = (int) BitConverter.ToInt16(this.m_data, 4);
    this.m_iMinimum = (int) BitConverter.ToInt16(this.m_data, 6);
    this.m_iMaximum = (int) BitConverter.ToInt16(this.m_data, 8);
    this.m_iIncrement = (int) BitConverter.ToInt16(this.m_data, 10);
    this.m_iPage = (int) BitConverter.ToInt16(this.m_data, 12);
    this.m_iHorizontal = (int) BitConverter.ToInt16(this.m_data, 14);
    this.m_iScrollBarWidth = (int) BitConverter.ToInt16(this.m_data, 16 /*0x10*/);
    this.m_sOptions = BitConverter.ToInt16(this.m_data, 18);
  }

  protected override void Serialize(DataProvider provider, int iOffset)
  {
    provider.WriteInt32(iOffset, 0);
    iOffset += 4;
    provider.WriteInt16(iOffset, (short) this.m_iValue);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) this.m_iMinimum);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) this.m_iMaximum);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) this.m_iIncrement);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) this.m_iPage);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) this.m_iHorizontal);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) this.m_iScrollBarWidth);
    iOffset += 2;
    provider.WriteInt16(iOffset, this.m_sOptions);
  }

  public override object Clone()
  {
    ftSbs ftSbs = (ftSbs) base.Clone();
    ftSbs.m_data = CloneUtils.CloneByteArray(this.m_data);
    return (object) ftSbs;
  }

  public override int GetStoreSize(ExcelVersion version) => 24;
}
