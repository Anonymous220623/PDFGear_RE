// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontTags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal static class SystemFontTags
{
  internal static readonly uint DEFAULT_TABLE_SCRIPT_TAG = SystemFontTags.GetTagFromString("DFLT");
  internal static readonly uint CFF_TABLE = SystemFontTags.GetTagFromString("CFF ");
  internal static readonly uint CMAP_TABLE = SystemFontTags.GetTagFromString("cmap");
  internal static readonly uint GLYF_TABLE = SystemFontTags.GetTagFromString("glyf");
  internal static readonly uint MAXP_TABLE = SystemFontTags.GetTagFromString("maxp");
  internal static readonly uint LOCA_TABLE = SystemFontTags.GetTagFromString("loca");
  internal static readonly uint HEAD_TABLE = SystemFontTags.GetTagFromString("head");
  internal static readonly uint HHEA_TABLE = SystemFontTags.GetTagFromString("hhea");
  internal static readonly uint HMTX_TABLE = SystemFontTags.GetTagFromString("hmtx");
  internal static readonly uint KERN_TABLE = SystemFontTags.GetTagFromString("kern");
  internal static readonly uint GSUB_TABLE = SystemFontTags.GetTagFromString("GSUB");
  internal static readonly uint NAME_TABLE = SystemFontTags.GetTagFromString("name");
  internal static readonly uint OS_2_TABLE = SystemFontTags.GetTagFromString("OS/2");
  internal static readonly uint POST_TABLE = SystemFontTags.GetTagFromString("post");
  internal static readonly uint OTTO_TAG = SystemFontTags.GetTagFromString("OTTO");
  internal static readonly uint NULL_TAG = SystemFontTags.GetTagFromString("NULL");
  internal static readonly ushort NULL_TYPE = (ushort) byte.MaxValue;
  internal static readonly uint TRUE_TYPE_COLLECTION = SystemFontTags.GetTagFromString("ttcf");
  internal static readonly uint FEATURE_ACCESS_ALL_ALTERNATES = SystemFontTags.GetTagFromString("aalt");
  internal static readonly uint FEATURE_ABOVE_BASE_FORMS = SystemFontTags.GetTagFromString("abvf");
  internal static readonly uint FEATURE_ABOVE_BASE_MARK_POSITIONING = SystemFontTags.GetTagFromString("abvm");
  internal static readonly uint FEATURE_ABOVE_BASE_SUBSTITUTIONS = SystemFontTags.GetTagFromString("abvs");
  internal static readonly uint FEATURE_ALTERNATIVE_FRACTIONS = SystemFontTags.GetTagFromString("afrc");
  internal static readonly uint FEATURE_AKHANDS = SystemFontTags.GetTagFromString("akhn");
  internal static readonly uint FEATURE_BELOW_BASE_FORMS = SystemFontTags.GetTagFromString("blwf");
  internal static readonly uint FEATURE_BELOW_BASE_MARK_POSITIONING = SystemFontTags.GetTagFromString("blwm");
  internal static readonly uint FEATURE_BELOW_BASE_SUBSTITUTIONS = SystemFontTags.GetTagFromString("blws");
  internal static readonly uint FEATURE_CONTEXTUAL_ALTERNATES = SystemFontTags.GetTagFromString("calt");
  internal static readonly uint FEATURE_CASE_SENSITIVE_FORMS = SystemFontTags.GetTagFromString("case");
  internal static readonly uint FEATURE_GLYPH_COMPOSITION_DECOMPOSITION = SystemFontTags.GetTagFromString("ccmp");
  internal static readonly uint FEATURE_CONJUNCT_FORM_AFTER_RO = SystemFontTags.GetTagFromString("cfar");
  internal static readonly uint FEATURE_CONJUNCT_FORMS = SystemFontTags.GetTagFromString("cjct");
  internal static readonly uint FEATURE_CONTEXTUAL_LIGATURES = SystemFontTags.GetTagFromString("clig");
  internal static readonly uint FEATURE_CENTERED_CJK_PUNCTUATION = SystemFontTags.GetTagFromString("cpct");
  internal static readonly uint FEATURE_CAPITAL_SPACING = SystemFontTags.GetTagFromString("cpsp");
  internal static readonly uint FEATURE_CONTEXTUAL_SWASH = SystemFontTags.GetTagFromString("cswh");
  internal static readonly uint FEATURE_CURSIVE_POSITIONING = SystemFontTags.GetTagFromString("curs");
  internal static readonly uint FEATURE_PETITE_CAPITALS_FROM_CAPITALS = SystemFontTags.GetTagFromString("c2pc");
  internal static readonly uint FEATURE_SMALL_CAPITALS_FROM_CAPITALS = SystemFontTags.GetTagFromString("c2sc");
  internal static readonly uint FEATURE_DISTANCES = SystemFontTags.GetTagFromString("dist");
  internal static readonly uint FEATURE_DISCRETIONARY_LIGATURES = SystemFontTags.GetTagFromString("dlig");
  internal static readonly uint FEATURE_DENOMINATORS = SystemFontTags.GetTagFromString("dnom");
  internal static readonly uint FEATURE_EXPERT_FORMS = SystemFontTags.GetTagFromString("expt");
  internal static readonly uint FEATURE_FINAL_GLYPH_ON_LINE_ALTERNATES = SystemFontTags.GetTagFromString("falt");
  internal static readonly uint FEATURE_TERMINAL_FORMS_2 = SystemFontTags.GetTagFromString("fin2");
  internal static readonly uint FEATURE_TERMINAL_FORMS_3 = SystemFontTags.GetTagFromString("fin3");
  internal static readonly uint FEATURE_TERMINAL_FORMS = SystemFontTags.GetTagFromString("fina");
  internal static readonly uint FEATURE_FRACTIONS = SystemFontTags.GetTagFromString("frac");
  internal static readonly uint FEATURE_FULL_WIDTHS = SystemFontTags.GetTagFromString("fwid");
  internal static readonly uint FEATURE_HALF_FORMS = SystemFontTags.GetTagFromString("half");
  internal static readonly uint FEATURE_HALANT_FORMS = SystemFontTags.GetTagFromString("haln");
  internal static readonly uint FEATURE_ALTERNATE_HALF_WIDTHS = SystemFontTags.GetTagFromString("halt");
  internal static readonly uint FEATURE_HISTORICAL_FORMS = SystemFontTags.GetTagFromString("hist");
  internal static readonly uint FEATURE_HORIZONTAL_KANA_ALTERNATES = SystemFontTags.GetTagFromString("hkna");
  internal static readonly uint FEATURE_HISTORICAL_LIGATURES = SystemFontTags.GetTagFromString("hlig");
  internal static readonly uint FEATURE_HANGUL = SystemFontTags.GetTagFromString("hngl");
  internal static readonly uint FEATURE_HOJO_KANJI_FORMS = SystemFontTags.GetTagFromString("hojo");
  internal static readonly uint FEATURE_HALF_WIDTHS = SystemFontTags.GetTagFromString("hwid");
  internal static readonly uint FEATURE_INITIAL_FORMS = SystemFontTags.GetTagFromString("init");
  internal static readonly uint FEATURE_ISOLATED_FORMS = SystemFontTags.GetTagFromString("isol");
  internal static readonly uint FEATURE_ITALICS = SystemFontTags.GetTagFromString("ital");
  internal static readonly uint FEATURE_JUSTIFICATION_ALTERNATES = SystemFontTags.GetTagFromString("jalt");
  internal static readonly uint FEATURE_JIS78_FORMS = SystemFontTags.GetTagFromString("jp78");
  internal static readonly uint FEATURE_JIS83_FORMS = SystemFontTags.GetTagFromString("jp83");
  internal static readonly uint FEATURE_JIS90_FORMS = SystemFontTags.GetTagFromString("jp90");
  internal static readonly uint FEATURE_JIS2004_FORMS = SystemFontTags.GetTagFromString("jp04");
  internal static readonly uint FEATURE_KERNING = SystemFontTags.GetTagFromString("kern");
  internal static readonly uint FEATURE_LEFT_BOUNDS = SystemFontTags.GetTagFromString("lfbd");
  internal static readonly uint FEATURE_STANDARD_LIGATURES = SystemFontTags.GetTagFromString("liga");
  internal static readonly uint FEATURE_LEADING_JAMO_FORMS = SystemFontTags.GetTagFromString("ljmo");
  internal static readonly uint FEATURE_LINING_FIGURES = SystemFontTags.GetTagFromString("lnum");
  internal static readonly uint FEATURE_LOCALIZED_FORMS = SystemFontTags.GetTagFromString("locl");
  internal static readonly uint FEATURE_LEFT_TO_RIGHT_ALTERNATES = SystemFontTags.GetTagFromString("ltra");
  internal static readonly uint FEATURE_LEFT_TO_RIGHT_MIRRORED_FORMS = SystemFontTags.GetTagFromString("ltrm");
  internal static readonly uint FEATURE_MARK_POSITIONING = SystemFontTags.GetTagFromString("mark");
  internal static readonly uint FEATURE_MEDIAL_FORMS_2 = SystemFontTags.GetTagFromString("med2");
  internal static readonly uint FEATURE_MEDIAL_FORMS = SystemFontTags.GetTagFromString("medi");
  internal static readonly uint FEATURE_MATHEMATICAL_GREEK = SystemFontTags.GetTagFromString("mgrk");
  internal static readonly uint FEATURE_MARK_TO_MARK_POSITIONING = SystemFontTags.GetTagFromString("mkmk");
  internal static readonly uint FEATURE_MARK_POSITIONING_VIA_SUBSTITUTION = SystemFontTags.GetTagFromString("mset");
  internal static readonly uint FEATURE_ALTERNATE_ANNOTATION_FORMS = SystemFontTags.GetTagFromString("nalt");
  internal static readonly uint FEATURE_NLC_KANJI_FORMS = SystemFontTags.GetTagFromString("nlck");
  internal static readonly uint FEATURE_NUKTA_FORMS = SystemFontTags.GetTagFromString("nukt");
  internal static readonly uint FEATURE_NUMERATORS = SystemFontTags.GetTagFromString("numr");
  internal static readonly uint FEATURE_OLDSTYLE_FIGURES = SystemFontTags.GetTagFromString("onum");
  internal static readonly uint FEATURE_OPTICAL_BOUNDS = SystemFontTags.GetTagFromString("opbd");
  internal static readonly uint FEATURE_ORDINALS = SystemFontTags.GetTagFromString("ordn");
  internal static readonly uint FEATURE_ORNAMENTS = SystemFontTags.GetTagFromString("ornm");
  internal static readonly uint FEATURE_PROPORTIONAL_ALTERNATE_WIDTHS = SystemFontTags.GetTagFromString("palt");
  internal static readonly uint FEATURE_PETITE_CAPITALS = SystemFontTags.GetTagFromString("pcap");
  internal static readonly uint FEATURE_PROPORTIONAL_KANA = SystemFontTags.GetTagFromString("pkna");
  internal static readonly uint FEATURE_PROPORTIONAL_FIGURES = SystemFontTags.GetTagFromString("pnum");
  internal static readonly uint FEATURE_PRE_BASE_FORMS = SystemFontTags.GetTagFromString("pref");
  internal static readonly uint FEATURE_PRE_BASE_SUBSTITUTIONS = SystemFontTags.GetTagFromString("pres");
  internal static readonly uint FEATURE_POST_BASE_FORMS = SystemFontTags.GetTagFromString("pstf");
  internal static readonly uint FEATURE_POST_BASE_SUBSTITUTIONS = SystemFontTags.GetTagFromString("psts");
  internal static readonly uint FEATURE_PROPORTIONAL_WIDTHS = SystemFontTags.GetTagFromString("pwid");
  internal static readonly uint FEATURE_QUARTER_WIDTHS = SystemFontTags.GetTagFromString("qwid");
  internal static readonly uint FEATURE_RANDOMIZE = SystemFontTags.GetTagFromString("rand");
  internal static readonly uint FEATURE_RAKAR_FORMS = SystemFontTags.GetTagFromString("rkrf");
  internal static readonly uint FEATURE_REQUIRED_LIGATURES = SystemFontTags.GetTagFromString("rlig");
  internal static readonly uint FEATURE_REPH_FORMS = SystemFontTags.GetTagFromString("rphf");
  internal static readonly uint FEATURE_RIGHT_BOUNDS = SystemFontTags.GetTagFromString("rtbd");
  internal static readonly uint FEATURE_RIGHT_TO_LEFT_ALTERNATES = SystemFontTags.GetTagFromString("rtla");
  internal static readonly uint FEATURE_RIGHT_TO_LEFT_MIRRORED_FORMS = SystemFontTags.GetTagFromString("rtlm");
  internal static readonly uint FEATURE_RUBY_NOTATION_FORMS = SystemFontTags.GetTagFromString("ruby");
  internal static readonly uint FEATURE_STYLISTIC_ALTERNATES = SystemFontTags.GetTagFromString("salt");
  internal static readonly uint FEATURE_SCIENTIFIC_INFERIORS = SystemFontTags.GetTagFromString("sinf");
  internal static readonly uint FEATURE_OPTICAL_SIZE = SystemFontTags.GetTagFromString("size");
  internal static readonly uint FEATURE_SMALL_CAPITALS = SystemFontTags.GetTagFromString("smcp");
  internal static readonly uint FEATURE_SIMPLIFIED_FORMS = SystemFontTags.GetTagFromString("smpl");
  internal static readonly uint FEATURE_STYLISTIC_SET_1 = SystemFontTags.GetTagFromString("ss01");
  internal static readonly uint FEATURE_STYLISTIC_SET_2 = SystemFontTags.GetTagFromString("ss02");
  internal static readonly uint FEATURE_STYLISTIC_SET_3 = SystemFontTags.GetTagFromString("ss03");
  internal static readonly uint FEATURE_STYLISTIC_SET_4 = SystemFontTags.GetTagFromString("ss04");
  internal static readonly uint FEATURE_STYLISTIC_SET_5 = SystemFontTags.GetTagFromString("ss05");
  internal static readonly uint FEATURE_STYLISTIC_SET_6 = SystemFontTags.GetTagFromString("ss06");
  internal static readonly uint FEATURE_STYLISTIC_SET_7 = SystemFontTags.GetTagFromString("ss07");
  internal static readonly uint FEATURE_STYLISTIC_SET_8 = SystemFontTags.GetTagFromString("ss08");
  internal static readonly uint FEATURE_STYLISTIC_SET_9 = SystemFontTags.GetTagFromString("ss09");
  internal static readonly uint FEATURE_STYLISTIC_SET_10 = SystemFontTags.GetTagFromString("ss10");
  internal static readonly uint FEATURE_STYLISTIC_SET_11 = SystemFontTags.GetTagFromString("ss11");
  internal static readonly uint FEATURE_STYLISTIC_SET_12 = SystemFontTags.GetTagFromString("ss12");
  internal static readonly uint FEATURE_STYLISTIC_SET_13 = SystemFontTags.GetTagFromString("ss13");
  internal static readonly uint FEATURE_STYLISTIC_SET_14 = SystemFontTags.GetTagFromString("ss14");
  internal static readonly uint FEATURE_STYLISTIC_SET_15 = SystemFontTags.GetTagFromString("ss15");
  internal static readonly uint FEATURE_STYLISTIC_SET_16 = SystemFontTags.GetTagFromString("ss16");
  internal static readonly uint FEATURE_STYLISTIC_SET_17 = SystemFontTags.GetTagFromString("ss17");
  internal static readonly uint FEATURE_STYLISTIC_SET_18 = SystemFontTags.GetTagFromString("ss18");
  internal static readonly uint FEATURE_STYLISTIC_SET_19 = SystemFontTags.GetTagFromString("ss19");
  internal static readonly uint FEATURE_STYLISTIC_SET_20 = SystemFontTags.GetTagFromString("ss20");
  internal static readonly uint FEATURE_SUBSCRIPT = SystemFontTags.GetTagFromString("subs");
  internal static readonly uint FEATURE_SUPERSCRIPT = SystemFontTags.GetTagFromString("sups");
  internal static readonly uint FEATURE_SWASH = SystemFontTags.GetTagFromString("swsh");
  internal static readonly uint FEATURE_TITLING = SystemFontTags.GetTagFromString("titl");
  internal static readonly uint FEATURE_TRAILING_JAMO_FORMS = SystemFontTags.GetTagFromString("tjmo");
  internal static readonly uint FEATURE_TRADITIONAL_NAME_FORMS = SystemFontTags.GetTagFromString("tnam");
  internal static readonly uint FEATURE_TABULAR_FIGURES = SystemFontTags.GetTagFromString("tnum");
  internal static readonly uint FEATURE_TRADITIONAL_FORMS = SystemFontTags.GetTagFromString("trad");
  internal static readonly uint FEATURE_THIRD_WIDTHS = SystemFontTags.GetTagFromString("twid");
  internal static readonly uint FEATURE_UNICASE = SystemFontTags.GetTagFromString("unic");
  internal static readonly uint FEATURE_ALTERNATE_VERTICAL_METRICS = SystemFontTags.GetTagFromString("valt");
  internal static readonly uint FEATURE_VATTU_VARIANTS = SystemFontTags.GetTagFromString("vatu");
  internal static readonly uint FEATURE_VERTICAL_WRITING = SystemFontTags.GetTagFromString("vert");
  internal static readonly uint FEATURE_ALTERNATE_VERTICAL_HALF_METRICS = SystemFontTags.GetTagFromString("vhal");
  internal static readonly uint FEATURE_VOWEL_JAMO_FORMS = SystemFontTags.GetTagFromString("vjmo");
  internal static readonly uint FEATURE_VERTICAL_KANA_ALTERNATES = SystemFontTags.GetTagFromString("vkna");
  internal static readonly uint FEATURE_VERTICAL_KERNING = SystemFontTags.GetTagFromString("vkrn");
  internal static readonly uint FEATURE_PROPORTIONAL_ALTERNATE_VERTICAL_METRICS = SystemFontTags.GetTagFromString("vpal");
  internal static readonly uint FEATURE_VERTICAL_ALTERNATES_AND_ROTATION = SystemFontTags.GetTagFromString("vrt2");
  internal static readonly uint FEATURE_SLASHED_ZERO = SystemFontTags.GetTagFromString("zero");

  internal static string GetStringFromTag(uint tag)
  {
    return new string(new char[4]
    {
      (char) ((uint) byte.MaxValue & tag >> 24),
      (char) ((uint) byte.MaxValue & tag >> 16 /*0x10*/),
      (char) ((uint) byte.MaxValue & tag >> 8),
      (char) ((uint) byte.MaxValue & tag)
    });
  }

  internal static uint GetTagFromString(string str)
  {
    return (uint) ((int) str[0] << 24 | (int) str[1] << 16 /*0x10*/ | (int) str[2] << 8) | (uint) str[3];
  }
}
