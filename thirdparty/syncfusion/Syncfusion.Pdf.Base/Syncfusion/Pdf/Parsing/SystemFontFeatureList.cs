// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFeatureList
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFeatureList(SystemFontOpenTypeFontSourceBase fontFile) : SystemFontTableBase(fontFile)
{
  private SystemFontFeatureRecord[] featureRecords;
  private SystemFontFeature[] features;

  private SystemFontFeature ReadFeature(
    SystemFontOpenTypeFontReader reader,
    SystemFontFeatureRecord record)
  {
    SystemFontFeatureInfo featureInfo = SystemFontFeatureInfo.CreateFeatureInfo(record.FeatureTag);
    if (featureInfo == null)
      return (SystemFontFeature) null;
    reader.BeginReadingBlock();
    reader.Seek(this.Offset + (long) record.FeatureOffset, SeekOrigin.Begin);
    SystemFontFeature systemFontFeature = new SystemFontFeature(this.FontSource, featureInfo);
    systemFontFeature.Read(reader);
    reader.EndReadingBlock();
    return systemFontFeature;
  }

  public SystemFontFeature GetFeature(int index)
  {
    if (this.features[index] == null)
      this.features[index] = this.ReadFeature(this.Reader, this.featureRecords[index]);
    return this.features[index];
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.featureRecords = new SystemFontFeatureRecord[(int) length];
    this.features = new SystemFontFeature[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      this.featureRecords[index] = new SystemFontFeatureRecord();
      this.featureRecords[index].Read(reader);
    }
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort((ushort) this.featureRecords.Length);
    for (int index = 0; index < this.featureRecords.Length; ++index)
    {
      SystemFontFeature feature = this.GetFeature(index);
      if (feature == null)
        writer.WriteULong(SystemFontTags.NULL_TAG);
      else
        feature.Write(writer);
    }
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.features = new SystemFontFeature[(int) length];
    for (int index = 0; index < (int) length; ++index)
    {
      uint featureTag = reader.ReadULong();
      if ((int) featureTag != (int) SystemFontTags.NULL_TAG)
      {
        SystemFontFeature systemFontFeature = new SystemFontFeature(this.FontSource, SystemFontFeatureInfo.CreateFeatureInfo(featureTag));
        systemFontFeature.Import(reader);
        this.features[index] = systemFontFeature;
      }
    }
  }
}
