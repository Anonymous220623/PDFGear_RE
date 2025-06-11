// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.CustomPropertyRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.CustomProperty)]
internal class CustomPropertyRecord : BiffRecordRaw
{
  private const int DEF_FIXED_SIZE = 7;
  private const int DEF_MAX_NAME_LENGTH = 255 /*0xFF*/;
  private static readonly byte[] DEF_HEADER = new byte[2]
  {
    (byte) 0,
    (byte) 16 /*0x10*/
  };
  private string m_strName;
  private string m_strValue;

  public CustomPropertyRecord()
  {
  }

  public CustomPropertyRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public CustomPropertyRecord(int iReserve)
    : base(iReserve)
  {
  }

  public string Name
  {
    get => this.m_strName;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value - string cannot be empty.");
        default:
          this.m_strName = value.Length <= (int) byte.MaxValue ? value : throw new ArgumentException("value - string is too long.");
          break;
      }
    }
  }

  public string Value
  {
    get => this.m_strValue;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          throw new ArgumentException("value - string cannot be empty.");
        default:
          this.m_strValue = value;
          break;
      }
    }
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    iOffset += 2;
    int stringLength1 = provider.ReadInt32(iOffset);
    iOffset += 4;
    int stringLength2 = (int) provider.ReadByte(iOffset);
    ++iOffset;
    this.m_strName = provider.ReadString(iOffset, stringLength2, Encoding.UTF8, true);
    iOffset += stringLength2;
    this.m_strValue = provider.ReadString(iOffset, stringLength1, Encoding.Unicode, true);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteBytes(iOffset, CustomPropertyRecord.DEF_HEADER, 0, CustomPropertyRecord.DEF_HEADER.Length);
    iOffset += CustomPropertyRecord.DEF_HEADER.Length;
    int iOffset1 = iOffset;
    iOffset += 4;
    ++iOffset;
    byte[] bytes1 = Encoding.UTF8.GetBytes(this.m_strName);
    int length1 = bytes1.Length;
    provider.WriteBytes(iOffset, bytes1, 0, length1);
    provider.WriteByte(iOffset1 + 4, (byte) length1);
    iOffset += length1;
    byte[] bytes2 = Encoding.Unicode.GetBytes(this.m_strValue);
    int length2 = bytes2.Length;
    provider.WriteBytes(iOffset, bytes2, 0, length2);
    provider.WriteInt32(iOffset1, length2);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return 7 + Encoding.UTF8.GetByteCount(this.m_strName) + Encoding.Unicode.GetByteCount(this.m_strValue);
  }
}
