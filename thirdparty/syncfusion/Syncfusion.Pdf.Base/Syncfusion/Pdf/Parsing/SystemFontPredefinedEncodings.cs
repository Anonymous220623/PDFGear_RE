// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontPredefinedEncodings
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal static class SystemFontPredefinedEncodings
{
  public static string[] StandardEncoding { get; private set; }

  private static void InitializeStandardEncoding()
  {
    SystemFontPredefinedEncodings.StandardEncoding = new string[256 /*0x0100*/];
    SystemFontPredefinedEncodings.StandardEncoding[179] = "adblgrave";
    SystemFontPredefinedEncodings.StandardEncoding[100] = "d";
    SystemFontPredefinedEncodings.StandardEncoding[101] = "e";
    SystemFontPredefinedEncodings.StandardEncoding[102] = "f";
    SystemFontPredefinedEncodings.StandardEncoding[103] = "g";
    SystemFontPredefinedEncodings.StandardEncoding[104] = "h";
    SystemFontPredefinedEncodings.StandardEncoding[105] = "i";
    SystemFontPredefinedEncodings.StandardEncoding[106] = "j";
    SystemFontPredefinedEncodings.StandardEncoding[107] = "k";
    SystemFontPredefinedEncodings.StandardEncoding[108] = "l";
    SystemFontPredefinedEncodings.StandardEncoding[109] = "m";
    SystemFontPredefinedEncodings.StandardEncoding[110] = "n";
    SystemFontPredefinedEncodings.StandardEncoding[111] = "o";
    SystemFontPredefinedEncodings.StandardEncoding[112 /*0x70*/] = "p";
    SystemFontPredefinedEncodings.StandardEncoding[113] = "q";
    SystemFontPredefinedEncodings.StandardEncoding[114] = "r";
    SystemFontPredefinedEncodings.StandardEncoding[115] = "s";
    SystemFontPredefinedEncodings.StandardEncoding[116] = "t";
    SystemFontPredefinedEncodings.StandardEncoding[117] = "u";
    SystemFontPredefinedEncodings.StandardEncoding[118] = "v";
    SystemFontPredefinedEncodings.StandardEncoding[119] = "w";
    SystemFontPredefinedEncodings.StandardEncoding[120] = "x";
    SystemFontPredefinedEncodings.StandardEncoding[121] = "y";
    SystemFontPredefinedEncodings.StandardEncoding[122] = "z";
    SystemFontPredefinedEncodings.StandardEncoding[123] = "braceleft";
    SystemFontPredefinedEncodings.StandardEncoding[124] = "bar";
    SystemFontPredefinedEncodings.StandardEncoding[125] = "braceright";
    SystemFontPredefinedEncodings.StandardEncoding[126] = "asciitilde";
    SystemFontPredefinedEncodings.StandardEncoding[161] = "exclamdown";
    SystemFontPredefinedEncodings.StandardEncoding[162] = "cent";
    SystemFontPredefinedEncodings.StandardEncoding[163] = "sterling";
    SystemFontPredefinedEncodings.StandardEncoding[164] = "fraction";
    SystemFontPredefinedEncodings.StandardEncoding[165] = "yen";
    SystemFontPredefinedEncodings.StandardEncoding[166] = "florin";
    SystemFontPredefinedEncodings.StandardEncoding[167] = "section";
    SystemFontPredefinedEncodings.StandardEncoding[168] = "currency";
    SystemFontPredefinedEncodings.StandardEncoding[169] = "quotesingle";
    SystemFontPredefinedEncodings.StandardEncoding[170] = "quotedblleft";
    SystemFontPredefinedEncodings.StandardEncoding[171] = "guillemotleft";
    SystemFontPredefinedEncodings.StandardEncoding[172] = "guilsinglleft";
    SystemFontPredefinedEncodings.StandardEncoding[173] = "guilsinglright";
    SystemFontPredefinedEncodings.StandardEncoding[174] = "fi";
    SystemFontPredefinedEncodings.StandardEncoding[175] = "fl";
    SystemFontPredefinedEncodings.StandardEncoding[177] = "endash";
    SystemFontPredefinedEncodings.StandardEncoding[178] = "dagger";
    SystemFontPredefinedEncodings.StandardEncoding[180] = "middot";
    SystemFontPredefinedEncodings.StandardEncoding[182] = "paragraph";
    SystemFontPredefinedEncodings.StandardEncoding[183] = "bullet";
    SystemFontPredefinedEncodings.StandardEncoding[184] = "quotesinglbase";
    SystemFontPredefinedEncodings.StandardEncoding[185] = "quotedblbase";
    SystemFontPredefinedEncodings.StandardEncoding[186] = "quotedblright";
    SystemFontPredefinedEncodings.StandardEncoding[187] = "guillemotright";
    SystemFontPredefinedEncodings.StandardEncoding[188] = "ellipsis";
    SystemFontPredefinedEncodings.StandardEncoding[189] = "perthousand";
    SystemFontPredefinedEncodings.StandardEncoding[191] = "questiondown";
    SystemFontPredefinedEncodings.StandardEncoding[193] = "grave";
    SystemFontPredefinedEncodings.StandardEncoding[194] = "acute";
    SystemFontPredefinedEncodings.StandardEncoding[195] = "circumflex";
    SystemFontPredefinedEncodings.StandardEncoding[196] = "ilde";
    SystemFontPredefinedEncodings.StandardEncoding[197] = "macron";
    SystemFontPredefinedEncodings.StandardEncoding[198] = "breve";
    SystemFontPredefinedEncodings.StandardEncoding[199] = "dotaccent";
    SystemFontPredefinedEncodings.StandardEncoding[200] = "dieresis";
    SystemFontPredefinedEncodings.StandardEncoding[202] = "ring";
    SystemFontPredefinedEncodings.StandardEncoding[203] = "cedilla";
    SystemFontPredefinedEncodings.StandardEncoding[205] = "hungarumlaut";
    SystemFontPredefinedEncodings.StandardEncoding[206] = "ogonek";
    SystemFontPredefinedEncodings.StandardEncoding[207] = "caron";
    SystemFontPredefinedEncodings.StandardEncoding[208 /*0xD0*/] = "emdash";
    SystemFontPredefinedEncodings.StandardEncoding[225] = "AE";
    SystemFontPredefinedEncodings.StandardEncoding[227] = "ordfeminine";
    SystemFontPredefinedEncodings.StandardEncoding[232] = "Lslash";
    SystemFontPredefinedEncodings.StandardEncoding[233] = "Oslash";
    SystemFontPredefinedEncodings.StandardEncoding[234] = "OE";
    SystemFontPredefinedEncodings.StandardEncoding[235] = "ordmasculine";
    SystemFontPredefinedEncodings.StandardEncoding[241] = "ae";
    SystemFontPredefinedEncodings.StandardEncoding[245] = "dotlessi";
    SystemFontPredefinedEncodings.StandardEncoding[248] = "lslash";
    SystemFontPredefinedEncodings.StandardEncoding[249] = "oslash";
    SystemFontPredefinedEncodings.StandardEncoding[250] = "oe";
    SystemFontPredefinedEncodings.StandardEncoding[251] = "germandbls";
    SystemFontPredefinedEncodings.StandardEncoding[32 /*0x20*/] = "space";
    SystemFontPredefinedEncodings.StandardEncoding[33] = "exclam";
    SystemFontPredefinedEncodings.StandardEncoding[34] = "quotedbl";
    SystemFontPredefinedEncodings.StandardEncoding[35] = "numbersign";
    SystemFontPredefinedEncodings.StandardEncoding[36] = "dollar";
    SystemFontPredefinedEncodings.StandardEncoding[37] = "percent";
    SystemFontPredefinedEncodings.StandardEncoding[38] = "ampersand";
    SystemFontPredefinedEncodings.StandardEncoding[39] = "quoteright";
    SystemFontPredefinedEncodings.StandardEncoding[40] = "parenleft";
    SystemFontPredefinedEncodings.StandardEncoding[41] = "parenright";
    SystemFontPredefinedEncodings.StandardEncoding[42] = "asterisk";
    SystemFontPredefinedEncodings.StandardEncoding[43] = "plus";
    SystemFontPredefinedEncodings.StandardEncoding[44] = "comma";
    SystemFontPredefinedEncodings.StandardEncoding[45] = "hyphen";
    SystemFontPredefinedEncodings.StandardEncoding[46] = "period";
    SystemFontPredefinedEncodings.StandardEncoding[47] = "slash";
    SystemFontPredefinedEncodings.StandardEncoding[48 /*0x30*/] = "zero";
    SystemFontPredefinedEncodings.StandardEncoding[49] = "one";
    SystemFontPredefinedEncodings.StandardEncoding[50] = "two";
    SystemFontPredefinedEncodings.StandardEncoding[51] = "three";
    SystemFontPredefinedEncodings.StandardEncoding[52] = "four";
    SystemFontPredefinedEncodings.StandardEncoding[53] = "five";
    SystemFontPredefinedEncodings.StandardEncoding[54] = "six";
    SystemFontPredefinedEncodings.StandardEncoding[55] = "seven";
    SystemFontPredefinedEncodings.StandardEncoding[56] = "eight";
    SystemFontPredefinedEncodings.StandardEncoding[57] = "nine";
    SystemFontPredefinedEncodings.StandardEncoding[58] = "colon";
    SystemFontPredefinedEncodings.StandardEncoding[59] = "semicolon";
    SystemFontPredefinedEncodings.StandardEncoding[60] = "less";
    SystemFontPredefinedEncodings.StandardEncoding[61] = "equal";
    SystemFontPredefinedEncodings.StandardEncoding[62] = "greater";
    SystemFontPredefinedEncodings.StandardEncoding[63 /*0x3F*/] = "question";
    SystemFontPredefinedEncodings.StandardEncoding[64 /*0x40*/] = "at";
    SystemFontPredefinedEncodings.StandardEncoding[65] = "A";
    SystemFontPredefinedEncodings.StandardEncoding[66] = "B";
    SystemFontPredefinedEncodings.StandardEncoding[67] = "C";
    SystemFontPredefinedEncodings.StandardEncoding[68] = "D";
    SystemFontPredefinedEncodings.StandardEncoding[69] = "E";
    SystemFontPredefinedEncodings.StandardEncoding[70] = "F";
    SystemFontPredefinedEncodings.StandardEncoding[71] = "G";
    SystemFontPredefinedEncodings.StandardEncoding[72] = "H";
    SystemFontPredefinedEncodings.StandardEncoding[73] = "I";
    SystemFontPredefinedEncodings.StandardEncoding[74] = "J";
    SystemFontPredefinedEncodings.StandardEncoding[75] = "K";
    SystemFontPredefinedEncodings.StandardEncoding[76] = "L";
    SystemFontPredefinedEncodings.StandardEncoding[77] = "M";
    SystemFontPredefinedEncodings.StandardEncoding[78] = "N";
    SystemFontPredefinedEncodings.StandardEncoding[79] = "O";
    SystemFontPredefinedEncodings.StandardEncoding[80 /*0x50*/] = "P";
    SystemFontPredefinedEncodings.StandardEncoding[81] = "Q";
    SystemFontPredefinedEncodings.StandardEncoding[82] = "R";
    SystemFontPredefinedEncodings.StandardEncoding[83] = "S";
    SystemFontPredefinedEncodings.StandardEncoding[84] = "T";
    SystemFontPredefinedEncodings.StandardEncoding[85] = "U";
    SystemFontPredefinedEncodings.StandardEncoding[86] = "V";
    SystemFontPredefinedEncodings.StandardEncoding[87] = "W";
    SystemFontPredefinedEncodings.StandardEncoding[88] = "X";
    SystemFontPredefinedEncodings.StandardEncoding[89] = "Y";
    SystemFontPredefinedEncodings.StandardEncoding[90] = "Z";
    SystemFontPredefinedEncodings.StandardEncoding[91] = "bracketleft";
    SystemFontPredefinedEncodings.StandardEncoding[92] = "backslash";
    SystemFontPredefinedEncodings.StandardEncoding[93] = "bracketright";
    SystemFontPredefinedEncodings.StandardEncoding[94] = "asciicircum";
    SystemFontPredefinedEncodings.StandardEncoding[95] = "underscore";
    SystemFontPredefinedEncodings.StandardEncoding[96 /*0x60*/] = "quoteleft";
    SystemFontPredefinedEncodings.StandardEncoding[97] = "a";
    SystemFontPredefinedEncodings.StandardEncoding[98] = "b";
    SystemFontPredefinedEncodings.StandardEncoding[99] = "c";
  }

  static SystemFontPredefinedEncodings()
  {
    SystemFontPredefinedEncodings.InitializeStandardEncoding();
  }

  public static SystemFontPostScriptArray CreateEncoding(string predefinedEncoding)
  {
    string[] strArray = (string[]) null;
    switch (predefinedEncoding)
    {
      case "StandardEncoding":
        strArray = SystemFontPredefinedEncodings.StandardEncoding;
        break;
    }
    if (strArray == null)
      return (SystemFontPostScriptArray) null;
    SystemFontPostScriptArray encoding = new SystemFontPostScriptArray(strArray.Length);
    foreach (string str in strArray)
      encoding.Add((object) str);
    return encoding;
  }
}
