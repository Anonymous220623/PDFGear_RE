// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.BiffOffsetsRecordsAttribute
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal sealed class BiffOffsetsRecordsAttribute : Attribute
{
  private TBIFFRecord m_type;

  public TBIFFRecord OffsetsRecordsType => this.m_type;

  private BiffOffsetsRecordsAttribute()
  {
  }

  public BiffOffsetsRecordsAttribute(TBIFFRecord type) => this.m_type = type;
}
