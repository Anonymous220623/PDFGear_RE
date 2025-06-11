// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFeatureInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontFeatureInfo
{
  public abstract uint Tag { get; }

  public abstract SystemFontFeatureInfo.FeatureType Type { get; }

  internal static SystemFontFeatureInfo CreateFeatureInfo(uint featureTag)
  {
    if ((int) featureTag == (int) SystemFontTags.FEATURE_INITIAL_FORMS)
      return (SystemFontFeatureInfo) new SystemFontInitFeatureInfo();
    if ((int) featureTag == (int) SystemFontTags.FEATURE_TERMINAL_FORMS)
      return (SystemFontFeatureInfo) new SystemFontFinalFeatureInfo();
    if ((int) featureTag == (int) SystemFontTags.FEATURE_ISOLATED_FORMS)
      return (SystemFontFeatureInfo) new SystemFontIsolatedFeatureInfo();
    if ((int) featureTag == (int) SystemFontTags.FEATURE_MEDIAL_FORMS)
      return (SystemFontFeatureInfo) new SystemFontMedialFeatureInfo();
    if ((int) featureTag == (int) SystemFontTags.FEATURE_REQUIRED_LIGATURES)
      return (SystemFontFeatureInfo) new SystemFontRLigFeatureInfo();
    return (int) featureTag == (int) SystemFontTags.FEATURE_STANDARD_LIGATURES ? (SystemFontFeatureInfo) new SystemFontLigaFeatureInfo() : (SystemFontFeatureInfo) null;
  }

  private static SystemFontGlyphsSequence ApplyMultipleSubstitutionLookup(
    SystemFontLookup lookup,
    SystemFontGlyphsSequence glyphIDs)
  {
    return lookup.Apply(glyphIDs);
  }

  public abstract bool ShouldApply(SystemFontGlyphInfo glyphIndex);

  public SystemFontGlyphsSequence ApplyLookup(
    SystemFontLookup lookup,
    SystemFontGlyphsSequence glyphIDs)
  {
    if (lookup == null)
      return glyphIDs;
    switch (this.Type)
    {
      case SystemFontFeatureInfo.FeatureType.SingleSubstitution:
        return this.ApplySingleGlyphSubstitutionLookup(lookup, glyphIDs);
      case SystemFontFeatureInfo.FeatureType.MultipleSubstitution:
        return SystemFontFeatureInfo.ApplyMultipleSubstitutionLookup(lookup, glyphIDs);
      default:
        return glyphIDs;
    }
  }

  private SystemFontGlyphsSequence ApplySingleGlyphSubstitutionLookup(
    SystemFontLookup lookup,
    SystemFontGlyphsSequence glyphIDs)
  {
    SystemFontGlyphsSequence fontGlyphsSequence = new SystemFontGlyphsSequence();
    foreach (SystemFontGlyphInfo glyphId in glyphIDs)
    {
      if (this.ShouldApply(glyphId))
        fontGlyphsSequence.AddRange((IEnumerable<SystemFontGlyphInfo>) lookup.Apply(new SystemFontGlyphsSequence(1)
        {
          glyphId
        }));
      else
        fontGlyphsSequence.Add(glyphId);
    }
    return fontGlyphsSequence;
  }

  internal enum FeatureType
  {
    SingleSubstitution,
    MultipleSubstitution,
    Unsupported,
  }
}
