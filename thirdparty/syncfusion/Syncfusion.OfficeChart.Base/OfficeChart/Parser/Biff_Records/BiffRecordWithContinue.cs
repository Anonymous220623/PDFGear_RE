// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.BiffRecordWithContinue
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
internal abstract class BiffRecordWithContinue : BiffRecordRawWithDataProvider
{
  private int DEF_WORD_MASK = (int) ushort.MaxValue;
  internal List<int> m_arrContinuePos = new List<int>();
  protected int m_iFirstLength = -1;

  public virtual TBIFFRecord FirstContinueType => TBIFFRecord.Continue;

  protected virtual bool AddHeaderToProvider => false;

  public override object Clone()
  {
    BiffRecordWithContinue recordWithContinue = (BiffRecordWithContinue) base.Clone();
    recordWithContinue.m_arrContinuePos = CloneUtils.CloneCloneable<int>(this.m_arrContinuePos);
    if (this.m_provider != null)
    {
      recordWithContinue.m_provider = ApplicationImpl.CreateDataProvider();
      recordWithContinue.m_provider.EnsureCapacity(this.m_provider.Capacity);
      this.m_provider.CopyTo(0, recordWithContinue.m_provider, 0, this.m_provider.Capacity);
    }
    return (object) recordWithContinue;
  }
}
