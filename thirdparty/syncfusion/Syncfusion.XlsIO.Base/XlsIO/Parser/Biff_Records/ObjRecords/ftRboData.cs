// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords.ftRboData
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
public class ftRboData : ObjSubRecord
{
  private byte m_isFirstButton;
  private byte m_nextButton;
  private byte m_ft;
  private byte m_cb;

  public bool IsFirstButton
  {
    get => this.m_isFirstButton == (byte) 1;
    set => this.m_isFirstButton = value ? (byte) 1 : (byte) 0;
  }

  public byte NextButton
  {
    get => this.m_nextButton;
    set => this.m_nextButton = value;
  }

  public ftRboData()
    : base(TObjSubRecordType.ftRboData)
  {
  }

  public ftRboData(ushort length, byte[] buffer)
    : base(TObjSubRecordType.ftRboData, length, buffer)
  {
  }

  protected override void Parse(byte[] buffer)
  {
    this.m_nextButton = buffer != null ? buffer[0] : throw new ArgumentNullException(nameof (buffer));
    this.m_ft = buffer[1];
    this.m_isFirstButton = buffer[2];
    this.m_cb = buffer[3];
  }

  public override void FillArray(DataProvider provider, int iOffset)
  {
    provider.WriteInt16(iOffset, (short) this.Type);
    iOffset += 2;
    short num = (short) (this.GetStoreSize(ExcelVersion.Excel97to2003) - 4);
    provider.WriteInt16(iOffset, num);
    iOffset += 2;
    provider.WriteByte(iOffset, this.m_nextButton);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_ft);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_isFirstButton);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_cb);
  }

  public override int GetStoreSize(ExcelVersion version) => 8;
}
