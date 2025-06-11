// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontsTags
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal static class SystemFontsTags
{
  internal static readonly uint DEFAULT_TABLE_SCRIPT_TAG = SystemFontsTags.GetTagFromString("DFLT");
  internal static readonly uint CFF_TABLE = SystemFontsTags.GetTagFromString("CFF ");
  internal static readonly uint CMAP_TABLE = SystemFontsTags.GetTagFromString("cmap");
  internal static readonly uint GLYF_TABLE = SystemFontsTags.GetTagFromString("glyf");
  internal static readonly uint MAXP_TABLE = SystemFontsTags.GetTagFromString("maxp");
  internal static readonly uint LOCA_TABLE = SystemFontsTags.GetTagFromString("loca");
  internal static readonly uint HEAD_TABLE = SystemFontsTags.GetTagFromString("head");
  internal static readonly uint HHEA_TABLE = SystemFontsTags.GetTagFromString("hhea");
  internal static readonly uint HMTX_TABLE = SystemFontsTags.GetTagFromString("hmtx");
  internal static readonly uint KERN_TABLE = SystemFontsTags.GetTagFromString("kern");
  internal static readonly uint GSUB_TABLE = SystemFontsTags.GetTagFromString("GSUB");
  internal static readonly uint NAME_TABLE = SystemFontsTags.GetTagFromString("name");
  internal static readonly uint OS_2_TABLE = SystemFontsTags.GetTagFromString("OS/2");
  internal static readonly uint POST_TABLE = SystemFontsTags.GetTagFromString("post");
  internal static readonly uint OTTO_TAG = SystemFontsTags.GetTagFromString("OTTO");
  internal static readonly uint NULL_TAG = SystemFontsTags.GetTagFromString("NULL");
  internal static readonly ushort NULL_TYPE = (ushort) byte.MaxValue;
  internal static readonly uint TRUE_TYPE_COLLECTION = SystemFontsTags.GetTagFromString("ttcf");
  internal static readonly uint FEATURE_ACCESS_ALL_ALTERNATES = SystemFontsTags.GetTagFromString("aalt");
  internal static readonly uint FEATURE_ABOVE_BASE_FORMS = SystemFontsTags.GetTagFromString("abvf");
  internal static readonly uint FEATURE_ABOVE_BASE_MARK_POSITIONING = SystemFontsTags.GetTagFromString("abvm");
  internal static readonly uint FEATURE_ABOVE_BASE_SUBSTITUTIONS = SystemFontsTags.GetTagFromString("abvs");
  internal static readonly uint FEATURE_ALTERNATIVE_FRACTIONS = SystemFontsTags.GetTagFromString("afrc");
  internal static readonly uint FEATURE_AKHANDS = SystemFontsTags.GetTagFromString("akhn");
  internal static readonly uint FEATURE_BELOW_BASE_FORMS = SystemFontsTags.GetTagFromString("blwf");
  internal static readonly uint FEATURE_BELOW_BASE_MARK_POSITIONING = SystemFontsTags.GetTagFromString("blwm");
  internal static readonly uint FEATURE_BELOW_BASE_SUBSTITUTIONS = SystemFontsTags.GetTagFromString("blws");
  internal static readonly uint FEATURE_CONTEXTUAL_ALTERNATES = SystemFontsTags.GetTagFromString("calt");
  internal static readonly uint FEATURE_CASE_SENSITIVE_FORMS = SystemFontsTags.GetTagFromString("case");
  internal static readonly uint FEATURE_GLYPH_COMPOSITION_DECOMPOSITION = SystemFontsTags.GetTagFromString("ccmp");
  internal static readonly uint FEATURE_CONJUNCT_FORM_AFTER_RO = SystemFontsTags.GetTagFromString("cfar");
  internal static readonly uint FEATURE_CONJUNCT_FORMS = SystemFontsTags.GetTagFromString("cjct");
  internal static readonly uint FEATURE_CONTEXTUAL_LIGATURES = SystemFontsTags.GetTagFromString("clig");
  internal static readonly uint FEATURE_CENTERED_CJK_PUNCTUATION = SystemFontsTags.GetTagFromString("cpct");
  internal static readonly uint FEATURE_CAPITAL_SPACING = SystemFontsTags.GetTagFromString("cpsp");
  internal static readonly uint FEATURE_CONTEXTUAL_SWASH = SystemFontsTags.GetTagFromString("cswh");
  internal static readonly uint FEATURE_CURSIVE_POSITIONING = SystemFontsTags.GetTagFromString("curs");
  internal static readonly uint FEATURE_PETITE_CAPITALS_FROM_CAPITALS = SystemFontsTags.GetTagFromString("c2pc");
  internal static readonly uint FEATURE_SMALL_CAPITALS_FROM_CAPITALS = SystemFontsTags.GetTagFromString("c2sc");
  internal static readonly uint FEATURE_DISTANCES = SystemFontsTags.GetTagFromString("dist");
  internal static readonly uint FEATURE_DISCRETIONARY_LIGATURES = SystemFontsTags.GetTagFromString("dlig");
  internal static readonly uint FEATURE_DENOMINATORS = SystemFontsTags.GetTagFromString("dnom");
  internal static readonly uint FEATURE_EXPERT_FORMS = SystemFontsTags.GetTagFromString("expt");
  internal static readonly uint FEATURE_FINAL_GLYPH_ON_LINE_ALTERNATES = SystemFontsTags.GetTagFromString("falt");
  internal static readonly uint FEATURE_TERMINAL_FORMS_2 = SystemFontsTags.GetTagFromString("fin2");
  internal static readonly uint FEATURE_TERMINAL_FORMS_3 = SystemFontsTags.GetTagFromString("fin3");
  internal static readonly uint FEATURE_TERMINAL_FORMS = SystemFontsTags.GetTagFromString("fina");
  internal static readonly uint FEATURE_FRACTIONS = SystemFontsTags.GetTagFromString("frac");
  internal static readonly uint FEATURE_FULL_WIDTHS = SystemFontsTags.GetTagFromString("fwid");
  internal static readonly uint FEATURE_HALF_FORMS = SystemFontsTags.GetTagFromString("half");
  internal static readonly uint FEATURE_HALANT_FORMS = SystemFontsTags.GetTagFromString("haln");
  internal static readonly uint FEATURE_ALTERNATE_HALF_WIDTHS = SystemFontsTags.GetTagFromString("halt");
  internal static readonly uint FEATURE_HISTORICAL_FORMS = SystemFontsTags.GetTagFromString("hist");
  internal static readonly uint FEATURE_HORIZONTAL_KANA_ALTERNATES = SystemFontsTags.GetTagFromString("hkna");
  internal static readonly uint FEATURE_HISTORICAL_LIGATURES = SystemFontsTags.GetTagFromString("hlig");
  internal static readonly uint FEATURE_HANGUL = SystemFontsTags.GetTagFromString("hngl");
  internal static readonly uint FEATURE_HOJO_KANJI_FORMS = SystemFontsTags.GetTagFromString("hojo");
  internal static readonly uint FEATURE_HALF_WIDTHS = SystemFontsTags.GetTagFromString("hwid");
  internal static readonly uint FEATURE_INITIAL_FORMS = SystemFontsTags.GetTagFromString("init");
  internal static readonly uint FEATURE_ISOLATED_FORMS = SystemFontsTags.GetTagFromString("isol");
  internal static readonly uint FEATURE_ITALICS = SystemFontsTags.GetTagFromString("ital");
  internal static readonly uint FEATURE_JUSTIFICATION_ALTERNATES = SystemFontsTags.GetTagFromString("jalt");
  internal static readonly uint FEATURE_JIS78_FORMS = SystemFontsTags.GetTagFromString("jp78");
  internal static readonly uint FEATURE_JIS83_FORMS = SystemFontsTags.GetTagFromString("jp83");
  internal static readonly uint FEATURE_JIS90_FORMS = SystemFontsTags.GetTagFromString("jp90");
  internal static readonly uint FEATURE_JIS2004_FORMS = SystemFontsTags.GetTagFromString("jp04");
  internal static readonly uint FEATURE_KERNING = SystemFontsTags.GetTagFromString("kern");
  internal static readonly uint FEATURE_LEFT_BOUNDS = SystemFontsTags.GetTagFromString("lfbd");
  internal static readonly uint FEATURE_STANDARD_LIGATURES = SystemFontsTags.GetTagFromString("liga");
  internal static readonly uint FEATURE_LEADING_JAMO_FORMS = SystemFontsTags.GetTagFromString("ljmo");
  internal static readonly uint FEATURE_LINING_FIGURES = SystemFontsTags.GetTagFromString("lnum");
  internal static readonly uint FEATURE_LOCALIZED_FORMS = SystemFontsTags.GetTagFromString("locl");
  internal static readonly uint FEATURE_LEFT_TO_RIGHT_ALTERNATES = SystemFontsTags.GetTagFromString("ltra");
  internal static readonly uint FEATURE_LEFT_TO_RIGHT_MIRRORED_FORMS = SystemFontsTags.GetTagFromString("ltrm");
  internal static readonly uint FEATURE_MARK_POSITIONING = SystemFontsTags.GetTagFromString("mark");
  internal static readonly uint FEATURE_MEDIAL_FORMS_2 = SystemFontsTags.GetTagFromString("med2");
  internal static readonly uint FEATURE_MEDIAL_FORMS = SystemFontsTags.GetTagFromString("medi");
  internal static readonly uint FEATURE_MATHEMATICAL_GREEK = SystemFontsTags.GetTagFromString("mgrk");
  internal static readonly uint FEATURE_MARK_TO_MARK_POSITIONING = SystemFontsTags.GetTagFromString("mkmk");
  internal static readonly uint FEATURE_MARK_POSITIONING_VIA_SUBSTITUTION = SystemFontsTags.GetTagFromString("mset");
  internal static readonly uint FEATURE_ALTERNATE_ANNOTATION_FORMS = SystemFontsTags.GetTagFromString("nalt");
  internal static readonly uint FEATURE_NLC_KANJI_FORMS = SystemFontsTags.GetTagFromString("nlck");
  internal static readonly uint FEATURE_NUKTA_FORMS = SystemFontsTags.GetTagFromString("nukt");
  internal static readonly uint FEATURE_NUMERATORS = SystemFontsTags.GetTagFromString("numr");
  internal static readonly uint FEATURE_OLDSTYLE_FIGURES = SystemFontsTags.GetTagFromString("onum");
  internal static readonly uint FEATURE_OPTICAL_BOUNDS = SystemFontsTags.GetTagFromString("opbd");
  internal static readonly uint FEATURE_ORDINALS = SystemFontsTags.GetTagFromString("ordn");
  internal static readonly uint FEATURE_ORNAMENTS = SystemFontsTags.GetTagFromString("ornm");
  internal static readonly uint FEATURE_PROPORTIONAL_ALTERNATE_WIDTHS = SystemFontsTags.GetTagFromString("palt");
  internal static readonly uint FEATURE_PETITE_CAPITALS = SystemFontsTags.GetTagFromString("pcap");
  internal static readonly uint FEATURE_PROPORTIONAL_KANA = SystemFontsTags.GetTagFromString("pkna");
  internal static readonly uint FEATURE_PROPORTIONAL_FIGURES = SystemFontsTags.GetTagFromString("pnum");
  internal static readonly uint FEATURE_PRE_BASE_FORMS = SystemFontsTags.GetTagFromString("pref");
  internal static readonly uint FEATURE_PRE_BASE_SUBSTITUTIONS = SystemFontsTags.GetTagFromString("pres");
  internal static readonly uint FEATURE_POST_BASE_FORMS = SystemFontsTags.GetTagFromString("pstf");
  internal static readonly uint FEATURE_POST_BASE_SUBSTITUTIONS = SystemFontsTags.GetTagFromString("psts");
  internal static readonly uint FEATURE_PROPORTIONAL_WIDTHS = SystemFontsTags.GetTagFromString("pwid");
  internal static readonly uint FEATURE_QUARTER_WIDTHS = SystemFontsTags.GetTagFromString("qwid");
  internal static readonly uint FEATURE_RANDOMIZE = SystemFontsTags.GetTagFromString("rand");
  internal static readonly uint FEATURE_RAKAR_FORMS = SystemFontsTags.GetTagFromString("rkrf");
  internal static readonly uint FEATURE_REQUIRED_LIGATURES = SystemFontsTags.GetTagFromString("rlig");
  internal static readonly uint FEATURE_REPH_FORMS = SystemFontsTags.GetTagFromString("rphf");
  internal static readonly uint FEATURE_RIGHT_BOUNDS = SystemFontsTags.GetTagFromString("rtbd");
  internal static readonly uint FEATURE_RIGHT_TO_LEFT_ALTERNATES = SystemFontsTags.GetTagFromString("rtla");
  internal static readonly uint FEATURE_RIGHT_TO_LEFT_MIRRORED_FORMS = SystemFontsTags.GetTagFromString("rtlm");
  internal static readonly uint FEATURE_RUBY_NOTATION_FORMS = SystemFontsTags.GetTagFromString("ruby");
  internal static readonly uint FEATURE_STYLISTIC_ALTERNATES = SystemFontsTags.GetTagFromString("salt");
  internal static readonly uint FEATURE_SCIENTIFIC_INFERIORS = SystemFontsTags.GetTagFromString("sinf");
  internal static readonly uint FEATURE_OPTICAL_SIZE = SystemFontsTags.GetTagFromString("size");
  internal static readonly uint FEATURE_SMALL_CAPITALS = SystemFontsTags.GetTagFromString("smcp");
  internal static readonly uint FEATURE_SIMPLIFIED_FORMS = SystemFontsTags.GetTagFromString("smpl");
  internal static readonly uint FEATURE_STYLISTIC_SET_1 = SystemFontsTags.GetTagFromString("ss01");
  internal static readonly uint FEATURE_STYLISTIC_SET_2 = SystemFontsTags.GetTagFromString("ss02");
  internal static readonly uint FEATURE_STYLISTIC_SET_3 = SystemFontsTags.GetTagFromString("ss03");
  internal static readonly uint FEATURE_STYLISTIC_SET_4 = SystemFontsTags.GetTagFromString("ss04");
  internal static readonly uint FEATURE_STYLISTIC_SET_5 = SystemFontsTags.GetTagFromString("ss05");
  internal static readonly uint FEATURE_STYLISTIC_SET_6 = SystemFontsTags.GetTagFromString("ss06");
  internal static readonly uint FEATURE_STYLISTIC_SET_7 = SystemFontsTags.GetTagFromString("ss07");
  internal static readonly uint FEATURE_STYLISTIC_SET_8 = SystemFontsTags.GetTagFromString("ss08");
  internal static readonly uint FEATURE_STYLISTIC_SET_9 = SystemFontsTags.GetTagFromString("ss09");
  internal static readonly uint FEATURE_STYLISTIC_SET_10 = SystemFontsTags.GetTagFromString("ss10");
  internal static readonly uint FEATURE_STYLISTIC_SET_11 = SystemFontsTags.GetTagFromString("ss11");
  internal static readonly uint FEATURE_STYLISTIC_SET_12 = SystemFontsTags.GetTagFromString("ss12");
  internal static readonly uint FEATURE_STYLISTIC_SET_13 = SystemFontsTags.GetTagFromString("ss13");
  internal static readonly uint FEATURE_STYLISTIC_SET_14 = SystemFontsTags.GetTagFromString("ss14");
  internal static readonly uint FEATURE_STYLISTIC_SET_15 = SystemFontsTags.GetTagFromString("ss15");
  internal static readonly uint FEATURE_STYLISTIC_SET_16 = SystemFontsTags.GetTagFromString("ss16");
  internal static readonly uint FEATURE_STYLISTIC_SET_17 = SystemFontsTags.GetTagFromString("ss17");
  internal static readonly uint FEATURE_STYLISTIC_SET_18 = SystemFontsTags.GetTagFromString("ss18");
  internal static readonly uint FEATURE_STYLISTIC_SET_19 = SystemFontsTags.GetTagFromString("ss19");
  internal static readonly uint FEATURE_STYLISTIC_SET_20 = SystemFontsTags.GetTagFromString("ss20");
  internal static readonly uint FEATURE_SUBSCRIPT = SystemFontsTags.GetTagFromString("subs");
  internal static readonly uint FEATURE_SUPERSCRIPT = SystemFontsTags.GetTagFromString("sups");
  internal static readonly uint FEATURE_SWASH = SystemFontsTags.GetTagFromString("swsh");
  internal static readonly uint FEATURE_TITLING = SystemFontsTags.GetTagFromString("titl");
  internal static readonly uint FEATURE_TRAILING_JAMO_FORMS = SystemFontsTags.GetTagFromString("tjmo");
  internal static readonly uint FEATURE_TRADITIONAL_NAME_FORMS = SystemFontsTags.GetTagFromString("tnam");
  internal static readonly uint FEATURE_TABULAR_FIGURES = SystemFontsTags.GetTagFromString("tnum");
  internal static readonly uint FEATURE_TRADITIONAL_FORMS = SystemFontsTags.GetTagFromString("trad");
  internal static readonly uint FEATURE_THIRD_WIDTHS = SystemFontsTags.GetTagFromString("twid");
  internal static readonly uint FEATURE_UNICASE = SystemFontsTags.GetTagFromString("unic");
  internal static readonly uint FEATURE_ALTERNATE_VERTICAL_METRICS = SystemFontsTags.GetTagFromString("valt");
  internal static readonly uint FEATURE_VATTU_VARIANTS = SystemFontsTags.GetTagFromString("vatu");
  internal static readonly uint FEATURE_VERTICAL_WRITING = SystemFontsTags.GetTagFromString("vert");
  internal static readonly uint FEATURE_ALTERNATE_VERTICAL_HALF_METRICS = SystemFontsTags.GetTagFromString("vhal");
  internal static readonly uint FEATURE_VOWEL_JAMO_FORMS = SystemFontsTags.GetTagFromString("vjmo");
  internal static readonly uint FEATURE_VERTICAL_KANA_ALTERNATES = SystemFontsTags.GetTagFromString("vkna");
  internal static readonly uint FEATURE_VERTICAL_KERNING = SystemFontsTags.GetTagFromString("vkrn");
  internal static readonly uint FEATURE_PROPORTIONAL_ALTERNATE_VERTICAL_METRICS = SystemFontsTags.GetTagFromString("vpal");
  internal static readonly uint FEATURE_VERTICAL_ALTERNATES_AND_ROTATION = SystemFontsTags.GetTagFromString("vrt2");
  internal static readonly uint FEATURE_SLASHED_ZERO = SystemFontsTags.GetTagFromString("zero");

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
