// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords.ftRbo
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
internal class ftRbo : ObjSubRecord
{
  public ftRbo()
    : base(TObjSubRecordType.ftRbo)
  {
  }

  public ftRbo(ushort length, byte[] buffer)
    : base(TObjSubRecordType.ftRbo, length, buffer)
  {
  }

  protected override void Parse(byte[] buffer)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
  }

  public override void FillArray(DataProvider provider, int iOffset)
  {
    provider.WriteInt16(iOffset, (short) this.Type);
    iOffset += 2;
    short num = (short) (this.GetStoreSize(OfficeVersion.Excel97to2003) - 4);
    provider.WriteInt16(iOffset, num);
    iOffset += 2;
    provider.WriteByte(iOffset, (byte) 0);
    ++iOffset;
    provider.WriteInt32(iOffset, 0);
    iOffset += 4;
    provider.WriteInt32(iOffset, 0);
    iOffset += 4;
    provider.WriteByte(iOffset, (byte) 0);
    ++iOffset;
    provider.WriteByte(iOffset, (byte) 3);
    ++iOffset;
    provider.WriteByte(iOffset, (byte) 0);
    ++iOffset;
  }

  public override int GetStoreSize(OfficeVersion version) => 10;
}
