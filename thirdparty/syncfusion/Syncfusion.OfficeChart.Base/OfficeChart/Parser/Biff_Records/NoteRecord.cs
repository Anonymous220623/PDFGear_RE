// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.NoteRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Note)]
[CLSCompliant(false)]
internal class NoteRecord : BiffRecordRaw
{
  private const int DEF_FIXED_PART_SIZE = 10;
  [BiffRecordPos(0, 2)]
  private ushort m_usRow;
  [BiffRecordPos(2, 2)]
  private ushort m_usColumn;
  [BiffRecordPos(4, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(4, 1, TFieldType.Bit)]
  private bool m_bShow;
  [BiffRecordPos(6, 2)]
  private ushort m_usObjId;
  [BiffRecordPos(8, 2)]
  private ushort m_usAuthorNameLen;
  private string m_strAuthorName = string.Empty;

  public ushort Row
  {
    get => this.m_usRow;
    set => this.m_usRow = value;
  }

  public ushort Column
  {
    get => this.m_usColumn;
    set => this.m_usColumn = value;
  }

  public string AuthorName
  {
    get => this.m_strAuthorName;
    set
    {
      this.m_strAuthorName = value;
      this.m_usAuthorNameLen = value != null ? (ushort) this.m_strAuthorName.Length : (ushort) 0;
    }
  }

  public ushort ObjId
  {
    get => this.m_usObjId;
    set => this.m_usObjId = value;
  }

  public bool IsVisible
  {
    get => this.m_bShow;
    set => this.m_bShow = value;
  }

  public ushort Reserved => this.m_usOptions;

  public override int MinimumRecordSize => 8;

  public NoteRecord() => this.m_bShow = false;

  public NoteRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public NoteRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usRow = provider.ReadUInt16(iOffset);
    this.m_usColumn = provider.ReadUInt16(iOffset + 2);
    this.m_usOptions = provider.ReadUInt16(iOffset + 4);
    this.m_bShow = provider.ReadBit(iOffset + 4, 1);
    this.m_usObjId = provider.ReadUInt16(iOffset + 6);
    this.m_usAuthorNameLen = provider.ReadUInt16(iOffset + 8);
    if (this.m_usAuthorNameLen <= (ushort) 0)
      return;
    this.m_strAuthorName = provider.ReadString(10, (int) this.m_usAuthorNameLen, out int _, false);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.GetStoreSize(OfficeVersion.Excel97to2003);
    this.m_iLength = 0;
    int num = iOffset;
    provider.WriteUInt16(iOffset, this.m_usRow);
    provider.WriteUInt16(iOffset + 2, this.m_usColumn);
    provider.WriteUInt16(iOffset + 4, this.m_usOptions);
    provider.WriteBit(iOffset + 4, this.m_bShow, 1);
    provider.WriteUInt16(iOffset + 6, this.m_usObjId);
    provider.WriteUInt16(iOffset + 8, this.m_usAuthorNameLen);
    iOffset += 10;
    if (this.m_usAuthorNameLen > (ushort) 0)
    {
      provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strAuthorName);
      this.m_iLength = iOffset - num;
      if (this.m_iLength % 2 == 0)
        return;
      provider.WriteByte(iOffset, (byte) 0);
      ++this.m_iLength;
    }
    else
    {
      provider.WriteByte(iOffset++, (byte) 0);
      provider.WriteByte(iOffset++, (byte) 0);
      this.m_iLength = iOffset - num;
    }
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int num;
    if (this.m_usAuthorNameLen > (ushort) 0)
    {
      num = Encoding.Unicode.GetByteCount(this.m_strAuthorName) + 1;
      if (num % 2 != 0)
        ++num;
    }
    else
      num = 2;
    return 10 + num;
  }
}
