// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords.ftCf
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
[method: CLSCompliant(false)]
internal class ftCf(TObjSubRecordType type, ushort length, byte[] buffer) : ObjSubRecord(type, length, buffer)
{
  internal const int DEF_RECORD_SIZE = 6;
  private short m_clipboardFormat;
  private byte[] m_data;

  public short ClipboardFormat => this.m_clipboardFormat;

  protected override void Parse(byte[] buffer)
  {
    if (this.Length == (ushort) 0)
    {
      this.Length = (ushort) 2;
      buffer = new byte[(int) this.Length];
    }
    this.m_data = (byte[]) buffer.Clone();
    this.m_clipboardFormat = BiffRecordRaw.GetInt16(buffer, 0);
  }

  public override void FillArray(DataProvider provider, int iOffset)
  {
    provider.WriteInt16(iOffset, (short) this.Type);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) 2);
    iOffset += 2;
    provider.WriteBytes(iOffset, this.m_data, 0, this.m_data.Length);
  }

  public override object Clone()
  {
    ftCf ftCf = (ftCf) base.Clone();
    ftCf.m_data = CloneUtils.CloneByteArray(this.m_data);
    return (object) ftCf;
  }

  public override int GetStoreSize(OfficeVersion version) => 6;
}
