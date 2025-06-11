// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords.ftCblsData
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
internal class ftCblsData : ObjSubRecord
{
  private byte m_btCheckState;
  private bool m_threeD;

  public ExcelCheckState CheckState
  {
    get => (ExcelCheckState) this.m_btCheckState;
    set => this.m_btCheckState = (byte) value;
  }

  public bool Display3DShading
  {
    get => this.m_threeD;
    set => this.m_threeD = value;
  }

  public ftCblsData()
    : base(TObjSubRecordType.ftCblsData)
  {
  }

  public ftCblsData(TObjSubRecordType type, ushort length, byte[] buffer)
    : base(type, length, buffer)
  {
  }

  protected override void Parse(byte[] buffer)
  {
    this.m_btCheckState = buffer != null ? buffer[0] : throw new ArgumentNullException(nameof (buffer));
    this.m_threeD = buffer[6] != (byte) 3;
  }

  public override void FillArray(DataProvider provider, int iOffset)
  {
    provider.WriteInt16(iOffset, (short) this.Type);
    iOffset += 2;
    short num1 = (short) (this.GetStoreSize(OfficeVersion.Excel97to2003) - 4);
    provider.WriteInt16(iOffset, num1);
    iOffset += 2;
    provider.WriteByte(iOffset, this.m_btCheckState);
    ++iOffset;
    provider.WriteInt32(iOffset, 0);
    iOffset += 4;
    provider.WriteByte(iOffset, (byte) 0);
    ++iOffset;
    byte num2 = this.m_threeD ? (byte) 2 : (byte) 3;
    provider.WriteByte(iOffset, num2);
    ++iOffset;
    provider.WriteByte(iOffset, (byte) 0);
    ++iOffset;
  }

  public override int GetStoreSize(OfficeVersion version) => 12;
}
