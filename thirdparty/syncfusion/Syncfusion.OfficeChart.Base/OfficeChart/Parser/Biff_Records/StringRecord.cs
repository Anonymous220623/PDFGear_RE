// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.StringRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.String)]
internal class StringRecord : BiffRecordRaw
{
  private const int DEF_FIXED_SIZE = 3;
  [BiffRecordPos(0, 2)]
  private ushort m_usStringLength;
  private string m_strValue;
  private bool m_bIsUnicode;

  public string Value
  {
    get => this.m_strValue;
    set
    {
      this.m_strValue = value;
      this.m_usStringLength = value != null ? (ushort) value.Length : (ushort) 0;
      this.m_bIsUnicode = !BiffRecordRawWithArray.IsAsciiString(value);
    }
  }

  public override int MinimumRecordSize => 4;

  public StringRecord()
  {
  }

  public StringRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public StringRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usStringLength = provider.ReadUInt16(iOffset);
    int iBytesInString;
    this.m_strValue = provider.ReadString(iOffset + 2, (int) this.m_usStringLength, out iBytesInString, false);
    this.m_bIsUnicode = this.m_strValue.Length * 2 > iBytesInString;
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usStringLength);
    int offset = iOffset + 2;
    provider.WriteStringNoLenUpdateOffset(ref offset, this.m_strValue, this.m_bIsUnicode);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return 3 + (this.m_bIsUnicode ? Encoding.Unicode.GetByteCount(this.m_strValue) : this.m_strValue.Length);
  }
}
