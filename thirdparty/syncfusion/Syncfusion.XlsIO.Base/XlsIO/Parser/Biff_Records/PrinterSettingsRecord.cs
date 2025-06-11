// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PrinterSettingsRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.PrinterSettings)]
[CLSCompliant(false)]
public class PrinterSettingsRecord : BiffRecordWithContinue
{
  protected override bool AddHeaderToProvider => true;

  public override bool NeedDataArray => true;

  public override void ParseStructure()
  {
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.m_iFirstLength = this.m_iLength > 8224 ? 8224 : -1;
  }

  public override object Clone()
  {
    PrinterSettingsRecord printerSettingsRecord = (PrinterSettingsRecord) base.Clone();
    if (this.m_provider != null && !this.m_provider.IsCleared)
    {
      printerSettingsRecord.m_provider.EnsureCapacity(this.m_iLength);
      this.m_provider.CopyTo(0, printerSettingsRecord.m_provider, 0, this.m_iLength);
    }
    return (object) printerSettingsRecord;
  }

  public override int GetStoreSize(ExcelVersion version) => this.m_iLength;
}
