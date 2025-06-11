// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.FontTableHeader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal static class FontTableHeader
{
  internal static readonly uint DEFAULT_TABLE_SCRIPT_TAG = FontTableHeader.GetTagFromString("DFLT");
  internal static readonly uint CFF_TABLE = FontTableHeader.GetTagFromString("CFF ");
  internal static readonly uint CMAP_TABLE = FontTableHeader.GetTagFromString("cmap");
  internal static readonly uint GLYF_TABLE = FontTableHeader.GetTagFromString("glyf");
  internal static readonly uint MAXP_TABLE = FontTableHeader.GetTagFromString("maxp");
  internal static readonly uint LOCA_TABLE = FontTableHeader.GetTagFromString("loca");
  internal static readonly uint HEAD_TABLE = FontTableHeader.GetTagFromString("head");
  internal static readonly uint HHEA_TABLE = FontTableHeader.GetTagFromString("hhea");
  internal static readonly uint HMTX_TABLE = FontTableHeader.GetTagFromString("hmtx");
  internal static readonly uint KERN_TABLE = FontTableHeader.GetTagFromString("kern");
  internal static readonly uint GSUB_TABLE = FontTableHeader.GetTagFromString("GSUB");
  internal static readonly uint NAME_TABLE = FontTableHeader.GetTagFromString("name");
  internal static readonly uint OS_2_TABLE = FontTableHeader.GetTagFromString("OS/2");
  internal static readonly uint POST_TABLE = FontTableHeader.GetTagFromString("post");
  internal static readonly uint OTTO_TAG = FontTableHeader.GetTagFromString("OTTO");
  internal static readonly uint NULL_TAG = FontTableHeader.GetTagFromString("NULL");
  internal static readonly ushort NULL_TYPE = (ushort) byte.MaxValue;
  internal static readonly uint TRUE_TYPE_COLLECTION = FontTableHeader.GetTagFromString("ttcf");
  internal static readonly uint FEATURE_ACCESS_ALL_ALTERNATES = FontTableHeader.GetTagFromString("aalt");
  internal static readonly uint FEATURE_ABOVE_BASE_FORMS = FontTableHeader.GetTagFromString("abvf");
  internal static readonly uint FEATURE_ABOVE_BASE_MARK_POSITIONING = FontTableHeader.GetTagFromString("abvm");
  internal static readonly uint FEATURE_ABOVE_BASE_SUBSTITUTIONS = FontTableHeader.GetTagFromString("abvs");
  internal static readonly uint FEATURE_ALTERNATIVE_FRACTIONS = FontTableHeader.GetTagFromString("afrc");
  internal static readonly uint FEATURE_AKHANDS = FontTableHeader.GetTagFromString("akhn");
  internal static readonly uint FEATURE_BELOW_BASE_FORMS = FontTableHeader.GetTagFromString("blwf");
  internal static readonly uint FEATURE_BELOW_BASE_MARK_POSITIONING = FontTableHeader.GetTagFromString("blwm");
  internal static readonly uint FEATURE_BELOW_BASE_SUBSTITUTIONS = FontTableHeader.GetTagFromString("blws");
  internal static readonly uint FEATURE_CONTEXTUAL_ALTERNATES = FontTableHeader.GetTagFromString("calt");
  internal static readonly uint FEATURE_CASE_SENSITIVE_FORMS = FontTableHeader.GetTagFromString("case");
  internal static readonly uint FEATURE_GLYPH_COMPOSITION_DECOMPOSITION = FontTableHeader.GetTagFromString("ccmp");
  internal static readonly uint FEATURE_CONJUNCT_FORM_AFTER_RO = FontTableHeader.GetTagFromString("cfar");
  internal static readonly uint FEATURE_CONJUNCT_FORMS = FontTableHeader.GetTagFromString("cjct");
  internal static readonly uint FEATURE_CONTEXTUAL_LIGATURES = FontTableHeader.GetTagFromString("clig");
  internal static readonly uint FEATURE_CENTERED_CJK_PUNCTUATION = FontTableHeader.GetTagFromString("cpct");
  internal static readonly uint FEATURE_CAPITAL_SPACING = FontTableHeader.GetTagFromString("cpsp");
  internal static readonly uint FEATURE_CONTEXTUAL_SWASH = FontTableHeader.GetTagFromString("cswh");
  internal static readonly uint FEATURE_CURSIVE_POSITIONING = FontTableHeader.GetTagFromString("curs");
  internal static readonly uint FEATURE_PETITE_CAPITALS_FROM_CAPITALS = FontTableHeader.GetTagFromString("c2pc");
  internal static readonly uint FEATURE_SMALL_CAPITALS_FROM_CAPITALS = FontTableHeader.GetTagFromString("c2sc");
  internal static readonly uint FEATURE_DISTANCES = FontTableHeader.GetTagFromString("dist");
  internal static readonly uint FEATURE_DISCRETIONARY_LIGATURES = FontTableHeader.GetTagFromString("dlig");
  internal static readonly uint FEATURE_DENOMINATORS = FontTableHeader.GetTagFromString("dnom");
  internal static readonly uint FEATURE_EXPERT_FORMS = FontTableHeader.GetTagFromString("expt");
  internal static readonly uint FEATURE_FINAL_GLYPH_ON_LINE_ALTERNATES = FontTableHeader.GetTagFromString("falt");
  internal static readonly uint FEATURE_TERMINAL_FORMS_2 = FontTableHeader.GetTagFromString("fin2");
  internal static readonly uint FEATURE_TERMINAL_FORMS_3 = FontTableHeader.GetTagFromString("fin3");
  internal static readonly uint FEATURE_TERMINAL_FORMS = FontTableHeader.GetTagFromString("fina");
  internal static readonly uint FEATURE_FRACTIONS = FontTableHeader.GetTagFromString("frac");
  internal static readonly uint FEATURE_FULL_WIDTHS = FontTableHeader.GetTagFromString("fwid");
  internal static readonly uint FEATURE_HALF_FORMS = FontTableHeader.GetTagFromString("half");
  internal static readonly uint FEATURE_HALANT_FORMS = FontTableHeader.GetTagFromString("haln");
  internal static readonly uint FEATURE_ALTERNATE_HALF_WIDTHS = FontTableHeader.GetTagFromString("halt");
  internal static readonly uint FEATURE_HISTORICAL_FORMS = FontTableHeader.GetTagFromString("hist");
  internal static readonly uint FEATURE_HORIZONTAL_KANA_ALTERNATES = FontTableHeader.GetTagFromString("hkna");
  internal static readonly uint FEATURE_HISTORICAL_LIGATURES = FontTableHeader.GetTagFromString("hlig");
  internal static readonly uint FEATURE_HANGUL = FontTableHeader.GetTagFromString("hngl");
  internal static readonly uint FEATURE_HOJO_KANJI_FORMS = FontTableHeader.GetTagFromString("hojo");
  internal static readonly uint FEATURE_HALF_WIDTHS = FontTableHeader.GetTagFromString("hwid");
  internal static readonly uint FEATURE_INITIAL_FORMS = FontTableHeader.GetTagFromString("init");
  internal static readonly uint FEATURE_ISOLATED_FORMS = FontTableHeader.GetTagFromString("isol");
  internal static readonly uint FEATURE_ITALICS = FontTableHeader.GetTagFromString("ital");
  internal static readonly uint FEATURE_JUSTIFICATION_ALTERNATES = FontTableHeader.GetTagFromString("jalt");
  internal static readonly uint FEATURE_JIS78_FORMS = FontTableHeader.GetTagFromString("jp78");
  internal static readonly uint FEATURE_JIS83_FORMS = FontTableHeader.GetTagFromString("jp83");
  internal static readonly uint FEATURE_JIS90_FORMS = FontTableHeader.GetTagFromString("jp90");
  internal static readonly uint FEATURE_JIS2004_FORMS = FontTableHeader.GetTagFromString("jp04");
  internal static readonly uint FEATURE_KERNING = FontTableHeader.GetTagFromString("kern");
  internal static readonly uint FEATURE_LEFT_BOUNDS = FontTableHeader.GetTagFromString("lfbd");
  internal static readonly uint FEATURE_STANDARD_LIGATURES = FontTableHeader.GetTagFromString("liga");
  internal static readonly uint FEATURE_LEADING_JAMO_FORMS = FontTableHeader.GetTagFromString("ljmo");
  internal static readonly uint FEATURE_LINING_FIGURES = FontTableHeader.GetTagFromString("lnum");
  internal static readonly uint FEATURE_LOCALIZED_FORMS = FontTableHeader.GetTagFromString("locl");
  internal static readonly uint FEATURE_LEFT_TO_RIGHT_ALTERNATES = FontTableHeader.GetTagFromString("ltra");
  internal static readonly uint FEATURE_LEFT_TO_RIGHT_MIRRORED_FORMS = FontTableHeader.GetTagFromString("ltrm");
  internal static readonly uint FEATURE_MARK_POSITIONING = FontTableHeader.GetTagFromString("mark");
  internal static readonly uint FEATURE_MEDIAL_FORMS_2 = FontTableHeader.GetTagFromString("med2");
  internal static readonly uint FEATURE_MEDIAL_FORMS = FontTableHeader.GetTagFromString("medi");
  internal static readonly uint FEATURE_MATHEMATICAL_GREEK = FontTableHeader.GetTagFromString("mgrk");
  internal static readonly uint FEATURE_MARK_TO_MARK_POSITIONING = FontTableHeader.GetTagFromString("mkmk");
  internal static readonly uint FEATURE_MARK_POSITIONING_VIA_SUBSTITUTION = FontTableHeader.GetTagFromString("mset");
  internal static readonly uint FEATURE_ALTERNATE_ANNOTATION_FORMS = FontTableHeader.GetTagFromString("nalt");
  internal static readonly uint FEATURE_NLC_KANJI_FORMS = FontTableHeader.GetTagFromString("nlck");
  internal static readonly uint FEATURE_NUKTA_FORMS = FontTableHeader.GetTagFromString("nukt");
  internal static readonly uint FEATURE_NUMERATORS = FontTableHeader.GetTagFromString("numr");
  internal static readonly uint FEATURE_OLDSTYLE_FIGURES = FontTableHeader.GetTagFromString("onum");
  internal static readonly uint FEATURE_OPTICAL_BOUNDS = FontTableHeader.GetTagFromString("opbd");
  internal static readonly uint FEATURE_ORDINALS = FontTableHeader.GetTagFromString("ordn");
  internal static readonly uint FEATURE_ORNAMENTS = FontTableHeader.GetTagFromString("ornm");
  internal static readonly uint FEATURE_PROPORTIONAL_ALTERNATE_WIDTHS = FontTableHeader.GetTagFromString("palt");
  internal static readonly uint FEATURE_PETITE_CAPITALS = FontTableHeader.GetTagFromString("pcap");
  internal static readonly uint FEATURE_PROPORTIONAL_KANA = FontTableHeader.GetTagFromString("pkna");
  internal static readonly uint FEATURE_PROPORTIONAL_FIGURES = FontTableHeader.GetTagFromString("pnum");
  internal static readonly uint FEATURE_PRE_BASE_FORMS = FontTableHeader.GetTagFromString("pref");
  internal static readonly uint FEATURE_PRE_BASE_SUBSTITUTIONS = FontTableHeader.GetTagFromString("pres");
  internal static readonly uint FEATURE_POST_BASE_FORMS = FontTableHeader.GetTagFromString("pstf");
  internal static readonly uint FEATURE_POST_BASE_SUBSTITUTIONS = FontTableHeader.GetTagFromString("psts");
  internal static readonly uint FEATURE_PROPORTIONAL_WIDTHS = FontTableHeader.GetTagFromString("pwid");
  internal static readonly uint FEATURE_QUARTER_WIDTHS = FontTableHeader.GetTagFromString("qwid");
  internal static readonly uint FEATURE_RANDOMIZE = FontTableHeader.GetTagFromString("rand");
  internal static readonly uint FEATURE_RAKAR_FORMS = FontTableHeader.GetTagFromString("rkrf");
  internal static readonly uint FEATURE_REQUIRED_LIGATURES = FontTableHeader.GetTagFromString("rlig");
  internal static readonly uint FEATURE_REPH_FORMS = FontTableHeader.GetTagFromString("rphf");
  internal static readonly uint FEATURE_RIGHT_BOUNDS = FontTableHeader.GetTagFromString("rtbd");
  internal static readonly uint FEATURE_RIGHT_TO_LEFT_ALTERNATES = FontTableHeader.GetTagFromString("rtla");
  internal static readonly uint FEATURE_RIGHT_TO_LEFT_MIRRORED_FORMS = FontTableHeader.GetTagFromString("rtlm");
  internal static readonly uint FEATURE_RUBY_NOTATION_FORMS = FontTableHeader.GetTagFromString("ruby");
  internal static readonly uint FEATURE_STYLISTIC_ALTERNATES = FontTableHeader.GetTagFromString("salt");
  internal static readonly uint FEATURE_SCIENTIFIC_INFERIORS = FontTableHeader.GetTagFromString("sinf");
  internal static readonly uint FEATURE_OPTICAL_SIZE = FontTableHeader.GetTagFromString("size");
  internal static readonly uint FEATURE_SMALL_CAPITALS = FontTableHeader.GetTagFromString("smcp");
  internal static readonly uint FEATURE_SIMPLIFIED_FORMS = FontTableHeader.GetTagFromString("smpl");
  internal static readonly uint FEATURE_STYLISTIC_SET_1 = FontTableHeader.GetTagFromString("ss01");
  internal static readonly uint FEATURE_STYLISTIC_SET_2 = FontTableHeader.GetTagFromString("ss02");
  internal static readonly uint FEATURE_STYLISTIC_SET_3 = FontTableHeader.GetTagFromString("ss03");
  internal static readonly uint FEATURE_STYLISTIC_SET_4 = FontTableHeader.GetTagFromString("ss04");
  internal static readonly uint FEATURE_STYLISTIC_SET_5 = FontTableHeader.GetTagFromString("ss05");
  internal static readonly uint FEATURE_STYLISTIC_SET_6 = FontTableHeader.GetTagFromString("ss06");
  internal static readonly uint FEATURE_STYLISTIC_SET_7 = FontTableHeader.GetTagFromString("ss07");
  internal static readonly uint FEATURE_STYLISTIC_SET_8 = FontTableHeader.GetTagFromString("ss08");
  internal static readonly uint FEATURE_STYLISTIC_SET_9 = FontTableHeader.GetTagFromString("ss09");
  internal static readonly uint FEATURE_STYLISTIC_SET_10 = FontTableHeader.GetTagFromString("ss10");
  internal static readonly uint FEATURE_STYLISTIC_SET_11 = FontTableHeader.GetTagFromString("ss11");
  internal static readonly uint FEATURE_STYLISTIC_SET_12 = FontTableHeader.GetTagFromString("ss12");
  internal static readonly uint FEATURE_STYLISTIC_SET_13 = FontTableHeader.GetTagFromString("ss13");
  internal static readonly uint FEATURE_STYLISTIC_SET_14 = FontTableHeader.GetTagFromString("ss14");
  internal static readonly uint FEATURE_STYLISTIC_SET_15 = FontTableHeader.GetTagFromString("ss15");
  internal static readonly uint FEATURE_STYLISTIC_SET_16 = FontTableHeader.GetTagFromString("ss16");
  internal static readonly uint FEATURE_STYLISTIC_SET_17 = FontTableHeader.GetTagFromString("ss17");
  internal static readonly uint FEATURE_STYLISTIC_SET_18 = FontTableHeader.GetTagFromString("ss18");
  internal static readonly uint FEATURE_STYLISTIC_SET_19 = FontTableHeader.GetTagFromString("ss19");
  internal static readonly uint FEATURE_STYLISTIC_SET_20 = FontTableHeader.GetTagFromString("ss20");
  internal static readonly uint FEATURE_SUBSCRIPT = FontTableHeader.GetTagFromString("subs");
  internal static readonly uint FEATURE_SUPERSCRIPT = FontTableHeader.GetTagFromString("sups");
  internal static readonly uint FEATURE_SWASH = FontTableHeader.GetTagFromString("swsh");
  internal static readonly uint FEATURE_TITLING = FontTableHeader.GetTagFromString("titl");
  internal static readonly uint FEATURE_TRAILING_JAMO_FORMS = FontTableHeader.GetTagFromString("tjmo");
  internal static readonly uint FEATURE_TRADITIONAL_NAME_FORMS = FontTableHeader.GetTagFromString("tnam");
  internal static readonly uint FEATURE_TABULAR_FIGURES = FontTableHeader.GetTagFromString("tnum");
  internal static readonly uint FEATURE_TRADITIONAL_FORMS = FontTableHeader.GetTagFromString("trad");
  internal static readonly uint FEATURE_THIRD_WIDTHS = FontTableHeader.GetTagFromString("twid");
  internal static readonly uint FEATURE_UNICASE = FontTableHeader.GetTagFromString("unic");
  internal static readonly uint FEATURE_ALTERNATE_VERTICAL_METRICS = FontTableHeader.GetTagFromString("valt");
  internal static readonly uint FEATURE_VATTU_VARIANTS = FontTableHeader.GetTagFromString("vatu");
  internal static readonly uint FEATURE_VERTICAL_WRITING = FontTableHeader.GetTagFromString("vert");
  internal static readonly uint FEATURE_ALTERNATE_VERTICAL_HALF_METRICS = FontTableHeader.GetTagFromString("vhal");
  internal static readonly uint FEATURE_VOWEL_JAMO_FORMS = FontTableHeader.GetTagFromString("vjmo");
  internal static readonly uint FEATURE_VERTICAL_KANA_ALTERNATES = FontTableHeader.GetTagFromString("vkna");
  internal static readonly uint FEATURE_VERTICAL_KERNING = FontTableHeader.GetTagFromString("vkrn");
  internal static readonly uint FEATURE_PROPORTIONAL_ALTERNATE_VERTICAL_METRICS = FontTableHeader.GetTagFromString("vpal");
  internal static readonly uint FEATURE_VERTICAL_ALTERNATES_AND_ROTATION = FontTableHeader.GetTagFromString("vrt2");
  internal static readonly uint FEATURE_SLASHED_ZERO = FontTableHeader.GetTagFromString("zero");

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
