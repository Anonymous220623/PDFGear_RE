// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.StyleRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Interfaces;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Style)]
public class StyleRecord : BiffRecordRaw, INamedObject
{
  private const ushort DEF_XF_INDEX_BIT_MASK = 4095 /*0x0FFF*/;
  [BiffRecordPos(0, 2)]
  private ushort m_usExtFormatIndex;
  [BiffRecordPos(1, 7, TFieldType.Bit)]
  private bool m_bIsBuildIn = true;
  [BiffRecordPos(2, 1)]
  private byte m_BuildInOrNameLen;
  private byte m_OutlineStyleLevel = byte.MaxValue;
  private string m_strName;
  private string m_strNameCache;
  private ushort m_DefXFIndex;
  private bool m_isBuiltInCustomized;
  private bool m_isAsciiConverted;

  public bool IsBuildInStyle
  {
    get => this.m_bIsBuildIn;
    set => this.m_bIsBuildIn = value;
  }

  internal bool IsAsciiConverted
  {
    get => this.m_isAsciiConverted;
    set => this.m_isAsciiConverted = value;
  }

  public ushort ExtendedFormatIndex
  {
    get
    {
      return this.DefXFIndex > (ushort) 0 ? (ushort) ((uint) this.m_usExtFormatIndex & (uint) this.DefXFIndex) : (ushort) ((uint) this.m_usExtFormatIndex & 4095U /*0x0FFF*/);
    }
    set => this.m_usExtFormatIndex = value;
  }

  public byte BuildInOrNameLen
  {
    get => this.m_BuildInOrNameLen;
    set => this.m_BuildInOrNameLen = value;
  }

  public byte OutlineStyleLevel
  {
    get => this.m_OutlineStyleLevel;
    set => this.m_OutlineStyleLevel = value;
  }

  public string StyleName
  {
    get => this.m_strName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (StyleName));
        case "":
          throw new ArgumentException("StyleName - string cannot be empty.");
        default:
          this.m_strName = value.Length <= (int) byte.MaxValue ? value : throw new ArgumentOutOfRangeException(nameof (StyleName), "Style name cannot be larger 255 symbols.");
          this.m_bIsBuildIn = false;
          this.m_BuildInOrNameLen = checked ((byte) this.m_strName.Length);
          break;
      }
    }
  }

  internal string StyleNameCache
  {
    get => this.m_strNameCache;
    set => this.m_strNameCache = value;
  }

  public bool IsBuiltIncustomized
  {
    get => this.m_isBuiltInCustomized;
    set => this.m_isBuiltInCustomized = value;
  }

  public override int MinimumRecordSize => 4;

  public string Name => this.m_strName;

  internal ushort DefXFIndex
  {
    get => this.m_DefXFIndex;
    set => this.m_DefXFIndex = value;
  }

  public StyleRecord()
  {
  }

  public StyleRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public StyleRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_iLength = iLength;
    this.m_usExtFormatIndex = provider.ReadUInt16(iOffset);
    this.m_bIsBuildIn = provider.ReadBit(iOffset + 1, 7);
    this.m_BuildInOrNameLen = provider.ReadByte(iOffset + 2);
    this.m_usExtFormatIndex &= (ushort) 4095 /*0x0FFF*/;
    if (this.m_bIsBuildIn)
    {
      if (iLength > 4)
        throw new LargeBiffRecordDataException();
      this.m_OutlineStyleLevel = provider.ReadByte(iOffset + 3);
    }
    else
    {
      int num = (int) provider.ReadByte(iOffset + 4);
      int buildInOrNameLen = (int) this.m_BuildInOrNameLen;
      this.m_strName = provider.ReadString(iOffset + 4, buildInOrNameLen, out int _, false);
    }
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    if (this.m_usExtFormatIndex > (ushort) 4095 /*0x0FFF*/)
      throw new ArgumentOutOfRangeException();
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteUInt16(iOffset, this.m_usExtFormatIndex);
    provider.WriteBit(iOffset + 1, this.m_bIsBuildIn, 7);
    provider.WriteByte(iOffset + 2, this.m_BuildInOrNameLen);
    if (this.m_bIsBuildIn)
    {
      provider.WriteByte(iOffset + 3, this.m_OutlineStyleLevel);
    }
    else
    {
      provider.WriteByte(iOffset + 2, (byte) this.m_strName.Length);
      provider.WriteByte(iOffset + 3, (byte) 0);
      byte[] bytes = Encoding.Unicode.GetBytes(this.m_strName);
      provider.WriteByte(iOffset + 4, (byte) 1);
      provider.WriteBytes(iOffset + 5, bytes, 0, bytes.Length);
    }
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return this.m_bIsBuildIn ? 4 : 5 + Encoding.Unicode.GetByteCount(this.m_strName);
  }

  public override void CopyTo(BiffRecordRaw raw)
  {
    if (!(raw is StyleRecord styleRecord))
      throw new ArgumentNullException("Wrong BiffRecord type");
    styleRecord.m_usExtFormatIndex = this.m_usExtFormatIndex;
    styleRecord.m_bIsBuildIn = this.m_bIsBuildIn;
    styleRecord.m_BuildInOrNameLen = this.m_BuildInOrNameLen;
    styleRecord.m_OutlineStyleLevel = this.m_OutlineStyleLevel;
    styleRecord.m_strName = this.m_strName;
  }
}
