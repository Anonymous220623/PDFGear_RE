// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ContinueRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[Biff(TBIFFRecord.Continue)]
[CLSCompliant(false)]
internal class ContinueRecord : BiffRecordRawWithArray, ILengthSetter
{
  public override bool NeedDataArray => true;

  public void SetLength(int len) => this.m_iLength = len;

  public void SetData(byte[] arrData) => this.m_data = arrData;

  public override void ParseStructure()
  {
  }

  public override void InfillInternalData(OfficeVersion version)
  {
  }

  public override int GetStoreSize(OfficeVersion version) => this.m_iLength;
}
