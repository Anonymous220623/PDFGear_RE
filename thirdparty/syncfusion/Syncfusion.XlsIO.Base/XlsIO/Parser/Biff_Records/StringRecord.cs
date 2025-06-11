// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.StringRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.String)]
[CLSCompliant(false)]
public class StringRecord : BiffRecordRaw
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
    ExcelVersion version)
  {
    this.m_usStringLength = provider.ReadUInt16(iOffset);
    int iBytesInString;
    this.m_strValue = provider.ReadString(iOffset + 2, (int) this.m_usStringLength, out iBytesInString, false);
    this.m_bIsUnicode = this.m_strValue.Length * 2 > iBytesInString;
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usStringLength);
    int offset = iOffset + 2;
    provider.WriteStringNoLenUpdateOffset(ref offset, this.m_strValue, this.m_bIsUnicode);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return 3 + (this.m_bIsUnicode ? Encoding.Unicode.GetByteCount(this.m_strValue) : this.m_strValue.Length);
  }
}
