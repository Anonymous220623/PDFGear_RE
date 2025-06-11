// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.WriteAccessRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.WriteAccess)]
internal class WriteAccessRecord : BiffRecordRaw
{
  private const string DEF_USER_NAME = "User";
  private const int DEF_MIN_SIZE = 112 /*0x70*/;
  private const int DEF_MAX_SIZE = 112 /*0x70*/;
  private const byte DEF_SPACE = 32 /*0x20*/;
  private string m_strUserName = "User";

  public string UserName
  {
    get => this.m_strUserName;
    set => this.m_strUserName = value;
  }

  public override int MinimumRecordSize => 112 /*0x70*/;

  public override int MaximumRecordSize => 112 /*0x70*/;

  public override bool IsAllowShortData => true;

  public WriteAccessRecord()
  {
  }

  public WriteAccessRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public WriteAccessRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    if ((long) provider.ReadUInt16(iOffset) >= (long) iLength)
      return;
    this.m_strUserName = provider.ReadString16Bit(iOffset, out int _);
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    this.m_iLength = iOffset;
    if (this.m_strUserName == null)
      this.m_strUserName = "User";
    provider.WriteUInt16(iOffset, (ushort) this.m_strUserName.Length);
    iOffset += 2;
    provider.WriteStringNoLenUpdateOffset(ref iOffset, this.m_strUserName, false);
    if (iOffset - this.m_iLength < 112 /*0x70*/)
    {
      int num1 = 0;
      int num2 = this.m_iLength - iOffset;
      while (num1 < num2)
      {
        provider.WriteByte(iOffset, (byte) 32 /*0x20*/);
        ++num1;
        ++iOffset;
      }
    }
    this.m_iLength = 112 /*0x70*/;
  }
}
