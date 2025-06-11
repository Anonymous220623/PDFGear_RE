// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ObjectProtectRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.ObjectProtect)]
[CLSCompliant(false)]
public class ObjectProtectRecord : BiffRecordRaw
{
  private const int DefaultRecordSize = 2;
  [BiffRecordPos(0, 2)]
  private ushort m_usProtect;

  public bool IsProtected
  {
    get => this.m_usProtect != (ushort) 0;
    set => this.m_usProtect = value ? (ushort) 1 : (ushort) 0;
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 2;

  public ObjectProtectRecord()
  {
  }

  public ObjectProtectRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ObjectProtectRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usProtect = provider.ReadUInt16(iOffset);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usProtect);
    this.m_iLength = 2;
  }

  public override int GetStoreSize(ExcelVersion version) => 2;
}
