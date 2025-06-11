// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontLangSys
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontLangSys(SystemFontOpenTypeFontSourceBase fontFile) : SystemFontTableBase(fontFile)
{
  private ushort reqFeatureIndex;
  private ushort[] featureIndices;
  private List<Tuple<SystemFontFeatureInfo, SystemFontLookup>> lookups;

  private IEnumerable<Tuple<SystemFontFeatureInfo, SystemFontLookup>> Lookups
  {
    get
    {
      if (this.lookups == null)
      {
        IEnumerable<Tuple<SystemFontFeatureInfo, ushort>> lookupIndices = this.GetLookupIndices();
        this.lookups = new List<Tuple<SystemFontFeatureInfo, SystemFontLookup>>(SystemFontEnumerable.Count<Tuple<SystemFontFeatureInfo, ushort>>(lookupIndices));
        foreach (Tuple<SystemFontFeatureInfo, ushort> tuple in lookupIndices)
          this.lookups.Add(new Tuple<SystemFontFeatureInfo, SystemFontLookup>(tuple.Item1, this.FontSource.GetLookup(tuple.Item2)));
      }
      return (IEnumerable<Tuple<SystemFontFeatureInfo, SystemFontLookup>>) this.lookups;
    }
  }

  private int Compare(
    Tuple<SystemFontFeatureInfo, ushort> left,
    Tuple<SystemFontFeatureInfo, ushort> right)
  {
    return left.Item2.CompareTo(right.Item2);
  }

  private IEnumerable<SystemFontFeature> GetFeatures()
  {
    List<SystemFontFeature> features = new List<SystemFontFeature>(this.featureIndices.Length);
    for (int index = 0; index < this.featureIndices.Length; ++index)
    {
      SystemFontFeature feature = this.FontSource.GetFeature(this.featureIndices[index]);
      if (feature != null)
        features.Add(feature);
    }
    if (this.reqFeatureIndex != ushort.MaxValue)
      features.Add(this.FontSource.GetFeature(this.reqFeatureIndex));
    return (IEnumerable<SystemFontFeature>) features;
  }

  private IEnumerable<Tuple<SystemFontFeatureInfo, ushort>> GetLookupIndices()
  {
    List<Tuple<SystemFontFeatureInfo, ushort>> lookupIndices = new List<Tuple<SystemFontFeatureInfo, ushort>>();
    foreach (SystemFontFeature feature in this.GetFeatures())
    {
      foreach (ushort lookupsListIndex in feature.LookupsListIndices)
        lookupIndices.Add(new Tuple<SystemFontFeatureInfo, ushort>(feature.FeatureInfo, lookupsListIndex));
    }
    lookupIndices.Sort(new Comparison<Tuple<SystemFontFeatureInfo, ushort>>(this.Compare));
    return (IEnumerable<Tuple<SystemFontFeatureInfo, ushort>>) lookupIndices;
  }

  public SystemFontGlyphsSequence Apply(SystemFontGlyphsSequence glyphIDs)
  {
    SystemFontGlyphsSequence glyphIDs1 = glyphIDs;
    foreach (Tuple<SystemFontFeatureInfo, SystemFontLookup> lookup in this.Lookups)
      glyphIDs1 = lookup.Item1.ApplyLookup(lookup.Item2, glyphIDs1);
    return glyphIDs1;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    int num = (int) reader.ReadUShort();
    this.reqFeatureIndex = reader.ReadUShort();
    ushort length = reader.ReadUShort();
    this.featureIndices = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.featureIndices[index] = reader.ReadUShort();
  }

  internal override void Write(SystemFontFontWriter writer)
  {
    writer.WriteUShort(this.reqFeatureIndex);
    writer.WriteUShort((ushort) this.featureIndices.Length);
    for (int index = 0; index < this.featureIndices.Length; ++index)
      writer.WriteUShort(this.featureIndices[index]);
  }

  internal override void Import(SystemFontOpenTypeFontReader reader)
  {
    this.reqFeatureIndex = reader.ReadUShort();
    ushort length = reader.ReadUShort();
    this.featureIndices = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.featureIndices[index] = reader.ReadUShort();
  }
}
