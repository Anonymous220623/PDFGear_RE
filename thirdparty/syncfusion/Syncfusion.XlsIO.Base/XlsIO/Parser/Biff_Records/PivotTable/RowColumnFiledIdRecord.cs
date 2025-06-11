// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.RowColumnFiledIdRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.RowColumnFieldId)]
public class RowColumnFiledIdRecord : BiffRecordRaw
{
  private ushort[] m_arrFieldId;

  public RowColumnFiledIdRecord()
  {
  }

  public RowColumnFiledIdRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public RowColumnFiledIdRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort[] FieldIds
  {
    get => this.m_arrFieldId;
    set
    {
      this.m_arrFieldId = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    int length = this.m_iLength / 2;
    this.m_arrFieldId = new ushort[length];
    int index = 0;
    while (index < length)
    {
      this.m_arrFieldId[index] = provider.ReadUInt16(iOffset);
      ++index;
      iOffset += 2;
    }
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = this.m_arrFieldId.Length * 2;
    provider.WriteByte(iOffset + this.m_iLength - 1, (byte) 0);
    int index = 0;
    int length = this.m_arrFieldId.Length;
    while (index < length)
    {
      provider.WriteUInt16(iOffset, this.m_arrFieldId[index]);
      ++index;
      iOffset += 2;
    }
  }

  public override int GetStoreSize(ExcelVersion version) => this.m_arrFieldId.Length * 2;
}
