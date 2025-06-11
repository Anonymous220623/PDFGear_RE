// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PresettedEncodings
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal static class PresettedEncodings
{
  public static string[] StandardEncoding { get; private set; }

  private static void InitializeStandardEncoding()
  {
    PresettedEncodings.StandardEncoding = new string[256 /*0x0100*/];
    PresettedEncodings.StandardEncoding[179] = "adblgrave";
    PresettedEncodings.StandardEncoding[100] = "d";
    PresettedEncodings.StandardEncoding[101] = "e";
    PresettedEncodings.StandardEncoding[102] = "f";
    PresettedEncodings.StandardEncoding[103] = "g";
    PresettedEncodings.StandardEncoding[104] = "h";
    PresettedEncodings.StandardEncoding[105] = "i";
    PresettedEncodings.StandardEncoding[106] = "j";
    PresettedEncodings.StandardEncoding[107] = "k";
    PresettedEncodings.StandardEncoding[108] = "l";
    PresettedEncodings.StandardEncoding[109] = "m";
    PresettedEncodings.StandardEncoding[110] = "n";
    PresettedEncodings.StandardEncoding[111] = "o";
    PresettedEncodings.StandardEncoding[112 /*0x70*/] = "p";
    PresettedEncodings.StandardEncoding[113] = "q";
    PresettedEncodings.StandardEncoding[114] = "r";
    PresettedEncodings.StandardEncoding[115] = "s";
    PresettedEncodings.StandardEncoding[116] = "t";
    PresettedEncodings.StandardEncoding[117] = "u";
    PresettedEncodings.StandardEncoding[118] = "v";
    PresettedEncodings.StandardEncoding[119] = "w";
    PresettedEncodings.StandardEncoding[120] = "x";
    PresettedEncodings.StandardEncoding[121] = "y";
    PresettedEncodings.StandardEncoding[122] = "z";
    PresettedEncodings.StandardEncoding[123] = "braceleft";
    PresettedEncodings.StandardEncoding[124] = "bar";
    PresettedEncodings.StandardEncoding[125] = "braceright";
    PresettedEncodings.StandardEncoding[126] = "asciitilde";
    PresettedEncodings.StandardEncoding[161] = "exclamdown";
    PresettedEncodings.StandardEncoding[162] = "cent";
    PresettedEncodings.StandardEncoding[163] = "sterling";
    PresettedEncodings.StandardEncoding[164] = "fraction";
    PresettedEncodings.StandardEncoding[165] = "yen";
    PresettedEncodings.StandardEncoding[166] = "florin";
    PresettedEncodings.StandardEncoding[167] = "section";
    PresettedEncodings.StandardEncoding[168] = "currency";
    PresettedEncodings.StandardEncoding[169] = "quotesingle";
    PresettedEncodings.StandardEncoding[170] = "quotedblleft";
    PresettedEncodings.StandardEncoding[171] = "guillemotleft";
    PresettedEncodings.StandardEncoding[172] = "guilsinglleft";
    PresettedEncodings.StandardEncoding[173] = "guilsinglright";
    PresettedEncodings.StandardEncoding[174] = "fi";
    PresettedEncodings.StandardEncoding[175] = "fl";
    PresettedEncodings.StandardEncoding[177] = "endash";
    PresettedEncodings.StandardEncoding[178] = "dagger";
    PresettedEncodings.StandardEncoding[180] = "middot";
    PresettedEncodings.StandardEncoding[182] = "paragraph";
    PresettedEncodings.StandardEncoding[183] = "bullet";
    PresettedEncodings.StandardEncoding[184] = "quotesinglbase";
    PresettedEncodings.StandardEncoding[185] = "quotedblbase";
    PresettedEncodings.StandardEncoding[186] = "quotedblright";
    PresettedEncodings.StandardEncoding[187] = "guillemotright";
    PresettedEncodings.StandardEncoding[188] = "ellipsis";
    PresettedEncodings.StandardEncoding[189] = "perthousand";
    PresettedEncodings.StandardEncoding[191] = "questiondown";
    PresettedEncodings.StandardEncoding[193] = "grave";
    PresettedEncodings.StandardEncoding[194] = "acute";
    PresettedEncodings.StandardEncoding[195] = "circumflex";
    PresettedEncodings.StandardEncoding[196] = "ilde";
    PresettedEncodings.StandardEncoding[197] = "macron";
    PresettedEncodings.StandardEncoding[198] = "breve";
    PresettedEncodings.StandardEncoding[199] = "dotaccent";
    PresettedEncodings.StandardEncoding[200] = "dieresis";
    PresettedEncodings.StandardEncoding[202] = "ring";
    PresettedEncodings.StandardEncoding[203] = "cedilla";
    PresettedEncodings.StandardEncoding[205] = "hungarumlaut";
    PresettedEncodings.StandardEncoding[206] = "ogonek";
    PresettedEncodings.StandardEncoding[207] = "caron";
    PresettedEncodings.StandardEncoding[208 /*0xD0*/] = "emdash";
    PresettedEncodings.StandardEncoding[225] = "AE";
    PresettedEncodings.StandardEncoding[227] = "ordfeminine";
    PresettedEncodings.StandardEncoding[232] = "Lslash";
    PresettedEncodings.StandardEncoding[233] = "Oslash";
    PresettedEncodings.StandardEncoding[234] = "OE";
    PresettedEncodings.StandardEncoding[235] = "ordmasculine";
    PresettedEncodings.StandardEncoding[241] = "ae";
    PresettedEncodings.StandardEncoding[245] = "dotlessi";
    PresettedEncodings.StandardEncoding[248] = "lslash";
    PresettedEncodings.StandardEncoding[249] = "oslash";
    PresettedEncodings.StandardEncoding[250] = "oe";
    PresettedEncodings.StandardEncoding[251] = "germandbls";
    PresettedEncodings.StandardEncoding[32 /*0x20*/] = "space";
    PresettedEncodings.StandardEncoding[33] = "exclam";
    PresettedEncodings.StandardEncoding[34] = "quotedbl";
    PresettedEncodings.StandardEncoding[35] = "numbersign";
    PresettedEncodings.StandardEncoding[36] = "dollar";
    PresettedEncodings.StandardEncoding[37] = "percent";
    PresettedEncodings.StandardEncoding[38] = "ampersand";
    PresettedEncodings.StandardEncoding[39] = "quoteright";
    PresettedEncodings.StandardEncoding[40] = "parenleft";
    PresettedEncodings.StandardEncoding[41] = "parenright";
    PresettedEncodings.StandardEncoding[42] = "asterisk";
    PresettedEncodings.StandardEncoding[43] = "plus";
    PresettedEncodings.StandardEncoding[44] = "comma";
    PresettedEncodings.StandardEncoding[45] = "hyphen";
    PresettedEncodings.StandardEncoding[46] = "period";
    PresettedEncodings.StandardEncoding[47] = "slash";
    PresettedEncodings.StandardEncoding[48 /*0x30*/] = "zero";
    PresettedEncodings.StandardEncoding[49] = "one";
    PresettedEncodings.StandardEncoding[50] = "two";
    PresettedEncodings.StandardEncoding[51] = "three";
    PresettedEncodings.StandardEncoding[52] = "four";
    PresettedEncodings.StandardEncoding[53] = "five";
    PresettedEncodings.StandardEncoding[54] = "six";
    PresettedEncodings.StandardEncoding[55] = "seven";
    PresettedEncodings.StandardEncoding[56] = "eight";
    PresettedEncodings.StandardEncoding[57] = "nine";
    PresettedEncodings.StandardEncoding[58] = "colon";
    PresettedEncodings.StandardEncoding[59] = "semicolon";
    PresettedEncodings.StandardEncoding[60] = "less";
    PresettedEncodings.StandardEncoding[61] = "equal";
    PresettedEncodings.StandardEncoding[62] = "greater";
    PresettedEncodings.StandardEncoding[63 /*0x3F*/] = "question";
    PresettedEncodings.StandardEncoding[64 /*0x40*/] = "at";
    PresettedEncodings.StandardEncoding[65] = "A";
    PresettedEncodings.StandardEncoding[66] = "B";
    PresettedEncodings.StandardEncoding[67] = "C";
    PresettedEncodings.StandardEncoding[68] = "D";
    PresettedEncodings.StandardEncoding[69] = "E";
    PresettedEncodings.StandardEncoding[70] = "F";
    PresettedEncodings.StandardEncoding[71] = "G";
    PresettedEncodings.StandardEncoding[72] = "H";
    PresettedEncodings.StandardEncoding[73] = "I";
    PresettedEncodings.StandardEncoding[74] = "J";
    PresettedEncodings.StandardEncoding[75] = "K";
    PresettedEncodings.StandardEncoding[76] = "L";
    PresettedEncodings.StandardEncoding[77] = "M";
    PresettedEncodings.StandardEncoding[78] = "N";
    PresettedEncodings.StandardEncoding[79] = "O";
    PresettedEncodings.StandardEncoding[80 /*0x50*/] = "P";
    PresettedEncodings.StandardEncoding[81] = "Q";
    PresettedEncodings.StandardEncoding[82] = "R";
    PresettedEncodings.StandardEncoding[83] = "S";
    PresettedEncodings.StandardEncoding[84] = "T";
    PresettedEncodings.StandardEncoding[85] = "U";
    PresettedEncodings.StandardEncoding[86] = "V";
    PresettedEncodings.StandardEncoding[87] = "W";
    PresettedEncodings.StandardEncoding[88] = "X";
    PresettedEncodings.StandardEncoding[89] = "Y";
    PresettedEncodings.StandardEncoding[90] = "Z";
    PresettedEncodings.StandardEncoding[91] = "bracketleft";
    PresettedEncodings.StandardEncoding[92] = "backslash";
    PresettedEncodings.StandardEncoding[93] = "bracketright";
    PresettedEncodings.StandardEncoding[94] = "asciicircum";
    PresettedEncodings.StandardEncoding[95] = "underscore";
    PresettedEncodings.StandardEncoding[96 /*0x60*/] = "quoteleft";
    PresettedEncodings.StandardEncoding[97] = "a";
    PresettedEncodings.StandardEncoding[98] = "b";
    PresettedEncodings.StandardEncoding[99] = "c";
  }

  static PresettedEncodings() => PresettedEncodings.InitializeStandardEncoding();

  public static PostScriptArray CreateEncoding(string predefinedEncoding)
  {
    string[] strArray = (string[]) null;
    switch (predefinedEncoding)
    {
      case "StandardEncoding":
        strArray = PresettedEncodings.StandardEncoding;
        break;
    }
    if (strArray == null)
      return (PostScriptArray) null;
    PostScriptArray encoding = new PostScriptArray(strArray.Length);
    foreach (string str in strArray)
      encoding.Add((object) str);
    return encoding;
  }
}
