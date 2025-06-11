// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.ScriptRecordReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class ScriptRecordReader
{
  private OtfTable m_table;
  private IList<ScriptRecord> m_records;

  internal IList<ScriptRecord> Records
  {
    get => this.m_records;
    set => this.m_records = value;
  }

  internal ScriptRecordReader(OtfTable table, int offset)
  {
    this.m_table = table;
    this.m_records = (IList<ScriptRecord>) new List<ScriptRecord>();
    table.Reader.Seek((long) offset);
    foreach (FeatureTag featureTag in table.ReadFeatureTag(offset))
      this.ReadScriptRecord(featureTag);
  }

  private void ReadScriptRecord(FeatureTag featureTag)
  {
    this.m_table.Reader.Seek((long) featureTag.Offset);
    int num = (int) this.m_table.Reader.ReadUInt16();
    if (num > 0)
      num += featureTag.Offset;
    FeatureTag[] featureTagArray = this.m_table.ReadFeatureTag(featureTag.Offset);
    ScriptRecord scriptRecord = new ScriptRecord();
    scriptRecord.ScriptTag = featureTag.TagName;
    scriptRecord.LanguageRecord = new LanguageRecord[featureTagArray.Length];
    for (int index = 0; index < featureTagArray.Length; ++index)
      scriptRecord.LanguageRecord[index] = this.ReadLanguageRecord(featureTagArray[index]);
    if (num > 0)
      scriptRecord.Language = this.ReadLanguageRecord(new FeatureTag()
      {
        TagName = "",
        Offset = num
      });
    this.m_records.Add(scriptRecord);
  }

  private LanguageRecord ReadLanguageRecord(FeatureTag featureTag)
  {
    LanguageRecord languageRecord = new LanguageRecord();
    this.m_table.Reader.Seek((long) (featureTag.Offset + 2));
    int num = (int) this.m_table.Reader.ReadUInt16();
    int size = (int) this.m_table.Reader.ReadUInt16();
    languageRecord.Records = this.m_table.ReadUInt32(size);
    languageRecord.LanguageTag = featureTag.TagName;
    return languageRecord;
  }
}
