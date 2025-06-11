// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.DConBinRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.DCONBIN)]
[CLSCompliant(false)]
public class DConBinRecord : BiffRecordRawWithArray
{
  [BiffRecordPos(0, TFieldType.String16Bit)]
  private string m_strName;
  private string m_strWorkbookName;
  private byte[] arrdata;

  public DConBinRecord()
  {
  }

  public DConBinRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public DConBinRecord(int iReserve)
    : base(iReserve)
  {
  }

  public string Name
  {
    get => this.m_strName;
    set => this.m_strName = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  public string WorkbookName
  {
    get => this.m_strWorkbookName;
    set
    {
      this.m_strWorkbookName = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public override void ParseStructure() => this.arrdata = this.m_data;

  public override void InfillInternalData(ExcelVersion version)
  {
    this.AutoGrowData = true;
    this.SetBytes(0, this.arrdata);
  }
}
