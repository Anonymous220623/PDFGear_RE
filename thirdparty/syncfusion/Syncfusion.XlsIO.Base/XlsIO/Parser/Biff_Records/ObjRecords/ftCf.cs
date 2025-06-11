// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords.ftCf
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;

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

  public override int GetStoreSize(ExcelVersion version) => 6;
}
