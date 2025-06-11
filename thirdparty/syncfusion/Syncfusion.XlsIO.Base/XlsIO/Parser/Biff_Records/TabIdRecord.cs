// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.TabIdRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.TabId)]
public class TabIdRecord : BiffRecordRaw
{
  private ushort[] m_arrTabIds = new ushort[1]{ (ushort) 1 };

  public ushort[] TabIds
  {
    get => this.m_arrTabIds;
    set => this.m_arrTabIds = value;
  }

  public TabIdRecord()
  {
  }

  public TabIdRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public TabIdRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.InternalDataIntegrityCheck();
    this.m_arrTabIds = new ushort[this.Length / 2];
    int index1 = 0;
    for (int index2 = this.m_iLength + iOffset; iOffset < index2; iOffset += 2)
    {
      this.m_arrTabIds[index1] = provider.ReadUInt16(iOffset);
      ++index1;
    }
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.GetStoreSize(ExcelVersion.Excel97to2003);
    int index = 0;
    for (int length = this.m_arrTabIds.Length; index < length; ++index)
    {
      provider.WriteUInt16(iOffset, this.m_arrTabIds[index]);
      iOffset += 2;
    }
  }

  private void InternalDataIntegrityCheck()
  {
    if (this.m_iLength % 2 != 0)
      throw new WrongBiffRecordDataException("MergeCellsRecord");
  }

  public override int GetStoreSize(ExcelVersion version) => this.m_arrTabIds.Length * 2;
}
