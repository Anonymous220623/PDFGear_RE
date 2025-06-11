// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords.ftNts
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
[method: CLSCompliant(false)]
public class ftNts(TObjSubRecordType type, ushort length, byte[] buffer) : ObjSubRecord(type, length, buffer)
{
  private const int DEF_RECORD_SIZE = 26;
  private byte[] m_data;

  public byte[] Data => this.m_data;

  protected override void Parse(byte[] buffer) => this.m_data = (byte[]) buffer.Clone();

  public override void FillArray(DataProvider provider, int iOffset)
  {
    provider.WriteInt16(iOffset, (short) this.Type);
    iOffset += 2;
    provider.WriteInt16(iOffset, (short) 22);
    iOffset += 2;
    provider.WriteBytes(iOffset, this.m_data, 0, this.m_data.Length);
  }

  public override object Clone()
  {
    ftNts ftNts = (ftNts) base.Clone();
    ftNts.m_data = CloneUtils.CloneByteArray(this.m_data);
    return (object) ftNts;
  }

  public override int GetStoreSize(ExcelVersion version) => 26;
}
