// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.QuickTipRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.QuickTip)]
[CLSCompliant(false)]
internal class QuickTipRecord : BiffRecordRawWithArray
{
  private const int DEF_FIXED_PART_SIZE = 10;
  [BiffRecordPos(0, 2)]
  private ushort m_usRecordId = 2048 /*0x0800*/;
  private TAddr m_addrCellRange;
  private string m_strToolTip = string.Empty;

  public TAddr CellRange
  {
    get => this.m_addrCellRange;
    set => this.m_addrCellRange = value;
  }

  public string ToolTip
  {
    get => this.m_strToolTip;
    set => this.m_strToolTip = value[value.Length - 1] != char.MinValue ? value + "\0" : value;
  }

  public override int MinimumRecordSize => 10;

  public QuickTipRecord()
  {
  }

  public QuickTipRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public QuickTipRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure()
  {
    this.m_usRecordId = this.GetUInt16(0);
    if (this.m_usRecordId != (ushort) 2048 /*0x0800*/)
      throw new WrongBiffRecordDataException("QuickTip first word must be 0x0800.");
    if (this.m_iLength % 2 != 0)
      throw new WrongBiffRecordDataException();
    this.m_addrCellRange = this.GetAddr(2);
    this.m_strToolTip = Encoding.Unicode.GetString(this.m_data, 10, this.m_iLength - 10);
    int startIndex = this.m_strToolTip.IndexOf(char.MinValue);
    if (startIndex != this.m_strToolTip.Length - 1)
      throw new WrongBiffRecordDataException("Zero-terminated string does not fit data array.");
    this.m_strToolTip = this.m_strToolTip.Remove(startIndex, 1);
  }

  public override void InfillInternalData(OfficeVersion version)
  {
    this.m_data = new byte[this.GetStoreSize(OfficeVersion.Excel97to2003)];
    this.SetUInt16(0, this.m_usRecordId);
    this.m_iLength = 2;
    this.SetAddr(this.m_iLength, this.m_addrCellRange);
    this.m_iLength += 8;
    byte[] bytes = Encoding.Unicode.GetBytes(this.m_strToolTip);
    int length = bytes.Length;
    this.SetBytes(this.m_iLength, bytes);
    this.m_iLength += length;
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return 10 + Encoding.Unicode.GetByteCount(this.m_strToolTip);
  }
}
