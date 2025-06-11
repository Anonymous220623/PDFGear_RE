// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFeature
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFeature : SystemFontTableBase
{
  private ushort[] lookupListIndices;

  public SystemFontFeatureInfo FeatureInfo { get; private set; }

  public ushort[] LookupsListIndices => this.lookupListIndices;

  public SystemFontFeature(
    SystemFontOpenTypeFontSourceBase fontFile,
    SystemFontFeatureInfo featureInfo)
    : base(fontFile)
  {
    this.FeatureInfo = featureInfo;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    int num = (int) reader.ReadUShort();
    ushort length = reader.ReadUShort();
    this.lookupListIndices = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.lookupListIndices[index] = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteULong(this.FeatureInfo.Tag);
    writer.WriteUShort((ushort) this.lookupListIndices.Length);
    for (int index = 0; index < this.lookupListIndices.Length; ++index)
      writer.WriteUShort(this.lookupListIndices[index]);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.lookupListIndices = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.lookupListIndices[index] = reader.ReadUShort();
  }

  public override string ToString()
  {
    return this.FeatureInfo != null ? SystemFontTags.GetStringFromTag(this.FeatureInfo.Tag) : "Not supported";
  }
}
