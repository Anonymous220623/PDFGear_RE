// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.FeatureRecordReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class FeatureRecordReader
{
  private IList<FeatureRecord> m_records;

  internal IList<FeatureRecord> Records
  {
    get => this.m_records;
    set => this.m_records = value;
  }

  internal FeatureRecordReader(OtfTable table, int offset)
  {
    this.m_records = (IList<FeatureRecord>) new List<FeatureRecord>();
    table.Reader.Seek((long) offset);
    foreach (FeatureTag featureTag in table.ReadFeatureTag(offset))
    {
      table.Reader.Seek((long) (featureTag.Offset + 2));
      int size = (int) table.Reader.ReadUInt16();
      this.m_records.Add(new FeatureRecord()
      {
        Tag = featureTag.TagName,
        Indexes = table.ReadUInt32(size)
      });
    }
  }
}
