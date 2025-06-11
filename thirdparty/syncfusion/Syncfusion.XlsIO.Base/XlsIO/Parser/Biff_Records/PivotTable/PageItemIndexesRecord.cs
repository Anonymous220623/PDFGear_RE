// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PageItemIndexesRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[Biff(TBIFFRecord.PageItemIndexes)]
[CLSCompliant(false)]
public class PageItemIndexesRecord : BiffRecordRawWithArray
{
  private ushort[] m_arrIndexes;

  public PageItemIndexesRecord()
  {
  }

  public PageItemIndexesRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PageItemIndexesRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort[] Indexes
  {
    get => this.m_arrIndexes;
    set
    {
      this.m_arrIndexes = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public override void ParseStructure()
  {
    this.m_arrIndexes = new ushort[this.m_iLength / 2];
    Buffer.BlockCopy((Array) this.m_data, 0, (Array) this.m_arrIndexes, 0, this.m_iLength);
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    int count = this.m_arrIndexes.Length * 2;
    this.m_data = new byte[count];
    Buffer.BlockCopy((Array) this.m_arrIndexes, 0, (Array) this.m_data, 0, count);
  }
}
