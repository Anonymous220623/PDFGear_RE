// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.SheetProtectionRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.SheetProtection)]
internal class SheetProtectionRecord : BiffRecordRaw
{
  public const int ErrorIndicatorType = 3;
  private const int DEF_OPTION_OFFSET = 19;
  private const int DEF_STORE_SIZE = 23;
  private readonly byte[] DEF_EMBEDED_DATA = new byte[8]
  {
    (byte) 0,
    (byte) 2,
    (byte) 0,
    (byte) 1,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue,
    byte.MaxValue
  };
  [BiffRecordPos(19, 2)]
  private ushort m_usOpt = 17408;
  private bool m_bIsContainProtection;
  private short m_sType;

  public int ProtectedOptions
  {
    get => (int) this.m_usOpt;
    set => this.m_usOpt = (ushort) value;
  }

  public bool ContainProtection
  {
    get => this.m_bIsContainProtection;
    set => this.m_bIsContainProtection = value;
  }

  public override int MinimumRecordSize => 19;

  public override int MaximumRecordSize => 23;

  internal short Type
  {
    get => this.m_sType;
    set => this.m_sType = value;
  }

  public SheetProtectionRecord()
  {
  }

  public SheetProtectionRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public SheetProtectionRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_bIsContainProtection = iLength > 19;
    this.m_sType = provider.ReadInt16(iOffset + 12);
    if (!this.m_bIsContainProtection)
      return;
    this.m_usOpt = (ushort) provider.ReadInt32(iOffset + 19);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = this.GetStoreSize(version);
    provider.WriteBytes(iOffset, new byte[this.m_iLength]);
    provider.WriteUInt16(iOffset, (ushort) 2151);
    if (this.m_sType == (short) 3)
    {
      iOffset += 12;
      provider.WriteInt16(iOffset, this.m_sType);
      iOffset += 2;
      provider.WriteByte(iOffset, (byte) 1);
      ++iOffset;
      provider.WriteInt32(iOffset, 0);
    }
    else
    {
      provider.WriteBytes(iOffset + 11, this.DEF_EMBEDED_DATA, 0, 8);
      provider.WriteUInt16(iOffset + 19, this.m_usOpt);
    }
  }

  public override int GetStoreSize(OfficeVersion version) => !this.m_bIsContainProtection ? 19 : 23;
}
