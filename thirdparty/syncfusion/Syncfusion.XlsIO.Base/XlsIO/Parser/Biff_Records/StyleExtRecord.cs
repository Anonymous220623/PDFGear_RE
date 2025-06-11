// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.StyleExtRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

internal class StyleExtRecord : BiffRecordRaw
{
  private const int StartLength = 22;
  private FutureHeader m_header;
  [BiffRecordPos(12, 0, TFieldType.Bit)]
  private bool m_bIsBuildIn = true;
  [BiffRecordPos(12, 1, TFieldType.Bit)]
  private bool m_bIsHidden;
  [BiffRecordPos(12, 2, TFieldType.Bit)]
  private bool m_bIsCustom;
  [BiffRecordPos(12, 8)]
  private long m_reserved;
  private ushort m_category;
  private ushort m_builtInData;
  private string m_strName;
  private byte m_OutlineStyleLevel = byte.MaxValue;
  [BiffRecordPos(2, 1)]
  private ushort m_BuildInOrNameLen;
  private ushort m_xfPropCount;
  private byte[] m_preservedProperties;

  public bool IsBuildInStyle
  {
    get => this.m_bIsBuildIn;
    set => this.m_bIsBuildIn = value;
  }

  public bool IsHidden
  {
    get => this.m_bIsHidden;
    set => this.m_bIsHidden = value;
  }

  public bool IsCustom
  {
    get => this.m_bIsCustom;
    set => this.m_bIsCustom = value;
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
          break;
      }
    }
  }

  public ushort Category
  {
    get => this.m_category;
    set => this.m_category = value;
  }

  public StyleExtRecord() => this.InitializeObjects();

  private void InitializeObjects()
  {
    this.m_header = new FutureHeader();
    this.m_header.Type = (ushort) 2194;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_header.Type = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_header.Attributes = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_reserved = provider.ReadInt64(iOffset);
    iOffset += 8;
    this.m_bIsBuildIn = provider.ReadBit(iOffset, 0);
    this.m_bIsHidden = provider.ReadBit(iOffset, 1);
    this.m_bIsCustom = provider.ReadBit(iOffset, 2);
    this.m_category = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_builtInData = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_BuildInOrNameLen = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_strName = provider.ReadString(iOffset, (int) this.m_BuildInOrNameLen * 2, Encoding.Unicode, true);
    iOffset += (int) this.m_BuildInOrNameLen * 2;
    int num = (int) provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_xfPropCount = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_preservedProperties = new byte[iLength - iOffset];
    provider.ReadArray(iOffset, this.m_preservedProperties, this.m_preservedProperties.Length);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_header.Type);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_header.Attributes);
    iOffset += 2;
    provider.WriteInt64(iOffset, 0L);
    iOffset += 8;
    provider.WriteUInt16(iOffset, this.m_category);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_builtInData);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_BuildInOrNameLen);
    iOffset += 2;
    byte[] bytes = Encoding.Unicode.GetBytes(this.m_strName);
    provider.WriteBytes(iOffset, bytes, 0, bytes.Length);
    iOffset += (int) this.m_BuildInOrNameLen * 2;
    provider.WriteUInt32(iOffset, 0U);
    iOffset += 2;
    provider.WriteUInt32(iOffset, (uint) this.m_xfPropCount);
    iOffset += 2;
    provider.WriteBytes(iOffset, this.m_preservedProperties);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return 22 + Encoding.Unicode.GetByteCount(this.m_strName) + this.m_preservedProperties.Length;
  }
}
