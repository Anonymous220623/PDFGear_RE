// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.PrinterSettingsRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.PrinterSettings)]
[CLSCompliant(false)]
internal class PrinterSettingsRecord : BiffRecordWithContinue
{
  protected override bool AddHeaderToProvider => true;

  public override bool NeedDataArray => true;

  public override void ParseStructure()
  {
  }

  public override void InfillInternalData(OfficeVersion version)
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

  public override int GetStoreSize(OfficeVersion version) => this.m_iLength;
}
