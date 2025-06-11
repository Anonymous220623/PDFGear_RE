// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotIndexListRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotIndexList)]
public class PivotIndexListRecord : BiffRecordRawWithArray
{
  private byte[] m_arrIndexes;

  public PivotIndexListRecord()
  {
  }

  public PivotIndexListRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotIndexListRecord(int iReserve)
    : base(iReserve)
  {
  }

  public byte[] Indexes
  {
    get => this.m_arrIndexes;
    set
    {
      this.m_arrIndexes = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public override void ParseStructure()
  {
    this.m_arrIndexes = new byte[this.m_iLength];
    Array.Copy((Array) this.m_data, 0, (Array) this.m_arrIndexes, 0, this.m_iLength);
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    bool autoGrowData = this.AutoGrowData;
    this.AutoGrowData = true;
    this.m_iLength = this.m_arrIndexes.Length;
    this.SetBytes(0, this.m_arrIndexes, 0, this.m_iLength);
    this.AutoGrowData = autoGrowData;
  }
}
