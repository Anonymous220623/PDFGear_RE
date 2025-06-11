// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords.ftUnknown
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
[method: CLSCompliant(false)]
internal class ftUnknown(TObjSubRecordType type, ushort length, byte[] buffer) : ObjSubRecord(type, length, buffer)
{
  private byte[] m_data;

  public byte[] RecordData => this.m_data;

  protected override void Parse(byte[] buffer)
  {
    this.m_data = new byte[(int) this.Length];
    Array.Copy((Array) buffer, 0, (Array) this.m_data, 0, (int) this.Length);
  }

  public override void FillArray(DataProvider provider, int iOffset)
  {
    provider.WriteInt16(iOffset, (short) this.Type);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) this.Length);
    iOffset += 2;
    provider.WriteBytes(iOffset, this.m_data, 0, this.m_data.Length);
  }

  public override object Clone()
  {
    ftUnknown ftUnknown = (ftUnknown) base.Clone();
    ftUnknown.m_data = CloneUtils.CloneByteArray(this.m_data);
    return (object) ftUnknown;
  }

  public override int GetStoreSize(OfficeVersion version) => (int) this.Length + 4;
}
