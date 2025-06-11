// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.Bidi
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class Bidi
{
  private int[] m_indexes;
  private byte[] m_indexLevels;
  private Dictionary<int, int> m_mirroringShapeCharacters = new Dictionary<int, int>();

  internal Bidi() => this.Update();

  private string DoMirrorShaping(string text)
  {
    char[] chArray = new char[text.Length];
    for (int index = 0; index < text.Length; ++index)
      chArray[index] = ((int) this.m_indexLevels[index] & 1) != 1 || !this.m_mirroringShapeCharacters.ContainsKey((int) text[index]) ? text[index] : (char) this.m_mirroringShapeCharacters[(int) text[index]];
    return new string(chArray);
  }

  private OtfGlyphInfo[] DoMirrorShaping(OtfGlyphInfo[] text, TtfReader ttfReader)
  {
    OtfGlyphInfo[] otfGlyphInfoArray = new OtfGlyphInfo[text.Length];
    for (int index = 0; index < text.Length; ++index)
    {
      char key = text[index].CharCode > 0 ? (char) text[index].CharCode : (text[index].Characters != null ? text[index].Characters[0] : char.MinValue);
      if (((int) this.m_indexLevels[index] & 1) == 1 && this.m_mirroringShapeCharacters.ContainsKey((int) key))
      {
        TtfGlyphInfo glyph = ttfReader.GetGlyph((char) this.m_mirroringShapeCharacters[(int) key]);
        OtfGlyphInfo otfGlyphInfo = new OtfGlyphInfo(glyph.CharCode, glyph.Index, (float) glyph.Width);
        otfGlyphInfoArray[index] = otfGlyphInfo;
      }
      else
        otfGlyphInfoArray[index] = text[index];
    }
    return otfGlyphInfoArray;
  }

  internal string GetLogicalToVisualString(string inputText, bool isRTL)
  {
    this.m_indexLevels = new byte[inputText.Length];
    this.m_indexes = new int[inputText.Length];
    this.m_indexLevels = new RTLCharacters().GetVisualOrder(inputText, isRTL);
    this.SetDefaultIndexLevel();
    this.DoOrder(0, this.m_indexLevels.Length - 1);
    string str = this.DoMirrorShaping(inputText);
    StringBuilder stringBuilder = new StringBuilder();
    for (int index1 = 0; index1 < this.m_indexes.Length; ++index1)
    {
      int index2 = this.m_indexes[index1];
      stringBuilder.Append(str[index2]);
    }
    return stringBuilder.ToString();
  }

  internal OtfGlyphInfo[] GetLogicalToVisualGlyphs(
    List<OtfGlyphInfo> inputText,
    bool isRTL,
    TtfReader ttfReader)
  {
    this.m_indexLevels = new byte[inputText.Count];
    this.m_indexes = new int[inputText.Count];
    this.m_indexLevels = new RTLCharacters().GetVisualOrder(inputText.ToArray(), isRTL);
    this.SetDefaultIndexLevel();
    this.DoOrder(0, this.m_indexLevels.Length - 1);
    OtfGlyphInfo[] otfGlyphInfoArray = this.DoMirrorShaping(inputText.ToArray(), ttfReader);
    List<OtfGlyphInfo> otfGlyphInfoList = new List<OtfGlyphInfo>();
    for (int index1 = 0; index1 < this.m_indexes.Length; ++index1)
    {
      int index2 = this.m_indexes[index1];
      otfGlyphInfoList.Add(otfGlyphInfoArray[index2]);
    }
    return otfGlyphInfoList.ToArray();
  }

  private void SetDefaultIndexLevel()
  {
    for (int index = 0; index < this.m_indexLevels.Length; ++index)
      this.m_indexes[index] = index;
  }

  private void DoOrder(int sIndex, int eIndex)
  {
    byte num1 = this.m_indexLevels[sIndex];
    byte num2 = num1;
    byte num3 = num1;
    byte num4 = num1;
    for (int index = sIndex + 1; index <= eIndex; ++index)
    {
      byte indexLevel = this.m_indexLevels[index];
      if ((int) indexLevel > (int) num1)
        num1 = indexLevel;
      else if ((int) indexLevel < (int) num2)
        num2 = indexLevel;
      num3 &= indexLevel;
      num4 |= indexLevel;
    }
    if (((int) num4 & 1) == 0)
      return;
    if (((int) num3 & 1) == 1)
    {
      this.ReArrange(sIndex, eIndex + 1);
    }
    else
    {
label_21:
      for (byte index = (byte) ((uint) num2 | 1U); (int) num1 >= (int) index; --num1)
      {
        int i = sIndex;
        while (true)
        {
          int j;
          for (; i > eIndex || (int) this.m_indexLevels[i] >= (int) num1; i = j + 1)
          {
            if (i <= eIndex)
            {
              j = i + 1;
              while (j <= eIndex && (int) this.m_indexLevels[j] >= (int) num1)
                ++j;
              this.ReArrange(i, j);
            }
            else
              goto label_21;
          }
          ++i;
        }
      }
    }
  }

  private void ReArrange(int i, int j)
  {
    int num = (i + j) / 2;
    --j;
    while (i < num)
    {
      int index = this.m_indexes[i];
      this.m_indexes[i] = this.m_indexes[j];
      this.m_indexes[j] = index;
      ++i;
      --j;
    }
  }

  private void Update()
  {
    this.m_mirroringShapeCharacters[40] = 41;
    this.m_mirroringShapeCharacters[41] = 40;
    this.m_mirroringShapeCharacters[60] = 62;
    this.m_mirroringShapeCharacters[62] = 60;
    this.m_mirroringShapeCharacters[91] = 93;
    this.m_mirroringShapeCharacters[93] = 91;
    this.m_mirroringShapeCharacters[123] = 125;
    this.m_mirroringShapeCharacters[125] = 123;
    this.m_mirroringShapeCharacters[171] = 187;
    this.m_mirroringShapeCharacters[187] = 171;
    this.m_mirroringShapeCharacters[8249] = 8250;
    this.m_mirroringShapeCharacters[8250] = 8249;
    this.m_mirroringShapeCharacters[8261] = 8262;
    this.m_mirroringShapeCharacters[8262] = 8261;
    this.m_mirroringShapeCharacters[8317] = 8318;
    this.m_mirroringShapeCharacters[8318] = 8317;
    this.m_mirroringShapeCharacters[8333] = 8334;
    this.m_mirroringShapeCharacters[8334] = 8333;
    this.m_mirroringShapeCharacters[8712] = 8715;
    this.m_mirroringShapeCharacters[8713] = 8716;
    this.m_mirroringShapeCharacters[8714] = 8717;
    this.m_mirroringShapeCharacters[8715] = 8712;
    this.m_mirroringShapeCharacters[8716] = 8713;
    this.m_mirroringShapeCharacters[8717] = 8714;
    this.m_mirroringShapeCharacters[8725] = 10741;
    this.m_mirroringShapeCharacters[8764] = 8765;
    this.m_mirroringShapeCharacters[8765] = 8764;
    this.m_mirroringShapeCharacters[8771] = 8909;
    this.m_mirroringShapeCharacters[8786] = 8787;
    this.m_mirroringShapeCharacters[8787] = 8786;
    this.m_mirroringShapeCharacters[8788] = 8789;
    this.m_mirroringShapeCharacters[8789] = 8788;
    this.m_mirroringShapeCharacters[8804] = 8805;
    this.m_mirroringShapeCharacters[8805] = 8804;
    this.m_mirroringShapeCharacters[8806] = 8807;
    this.m_mirroringShapeCharacters[8807] = 8806;
    this.m_mirroringShapeCharacters[8808] = 8809;
    this.m_mirroringShapeCharacters[8809] = 8808;
    this.m_mirroringShapeCharacters[8810] = 8811;
    this.m_mirroringShapeCharacters[8811] = 8810;
    this.m_mirroringShapeCharacters[8814] = 8815;
    this.m_mirroringShapeCharacters[8815] = 8814;
    this.m_mirroringShapeCharacters[8816] = 8817;
    this.m_mirroringShapeCharacters[8817] = 8816;
    this.m_mirroringShapeCharacters[8818] = 8819;
    this.m_mirroringShapeCharacters[8819] = 8818;
    this.m_mirroringShapeCharacters[8820] = 8821;
    this.m_mirroringShapeCharacters[8821] = 8820;
    this.m_mirroringShapeCharacters[8822] = 8823;
    this.m_mirroringShapeCharacters[8823] = 8822;
    this.m_mirroringShapeCharacters[8824] = 8825;
    this.m_mirroringShapeCharacters[8825] = 8824;
    this.m_mirroringShapeCharacters[8826] = 8827;
    this.m_mirroringShapeCharacters[8827] = 8826;
    this.m_mirroringShapeCharacters[8828] = 8829;
    this.m_mirroringShapeCharacters[8829] = 8828;
    this.m_mirroringShapeCharacters[8830] = 8831;
    this.m_mirroringShapeCharacters[8831] = 8830;
    this.m_mirroringShapeCharacters[8832] = 8833;
    this.m_mirroringShapeCharacters[8833] = 8832;
    this.m_mirroringShapeCharacters[8834] = 8835;
    this.m_mirroringShapeCharacters[8835] = 8834;
    this.m_mirroringShapeCharacters[8836] = 8837;
    this.m_mirroringShapeCharacters[8837] = 8836;
    this.m_mirroringShapeCharacters[8838] = 8839;
    this.m_mirroringShapeCharacters[8839] = 8838;
    this.m_mirroringShapeCharacters[8840] = 8841;
    this.m_mirroringShapeCharacters[8841] = 8840;
    this.m_mirroringShapeCharacters[8842] = 8843;
    this.m_mirroringShapeCharacters[8843] = 8842;
    this.m_mirroringShapeCharacters[8847] = 8848;
    this.m_mirroringShapeCharacters[8848] = 8847;
    this.m_mirroringShapeCharacters[8849] = 8850;
    this.m_mirroringShapeCharacters[8850] = 8849;
    this.m_mirroringShapeCharacters[8856] = 10680;
    this.m_mirroringShapeCharacters[8866] = 8867;
    this.m_mirroringShapeCharacters[8867] = 8866;
    this.m_mirroringShapeCharacters[8870] = 10974;
    this.m_mirroringShapeCharacters[8872] = 10980;
    this.m_mirroringShapeCharacters[8873] = 10979;
    this.m_mirroringShapeCharacters[8875] = 10981;
    this.m_mirroringShapeCharacters[8880] = 8881;
    this.m_mirroringShapeCharacters[8881] = 8880;
    this.m_mirroringShapeCharacters[8882] = 8883;
    this.m_mirroringShapeCharacters[8883] = 8882;
    this.m_mirroringShapeCharacters[8884] = 8885;
    this.m_mirroringShapeCharacters[8885] = 8884;
    this.m_mirroringShapeCharacters[8886] = 8887;
    this.m_mirroringShapeCharacters[8887] = 8886;
    this.m_mirroringShapeCharacters[8905] = 8906;
    this.m_mirroringShapeCharacters[8906] = 8905;
    this.m_mirroringShapeCharacters[8907] = 8908;
    this.m_mirroringShapeCharacters[8908] = 8907;
    this.m_mirroringShapeCharacters[8909] = 8771;
    this.m_mirroringShapeCharacters[8912] = 8913;
    this.m_mirroringShapeCharacters[8913] = 8912;
    this.m_mirroringShapeCharacters[8918] = 8919;
    this.m_mirroringShapeCharacters[8919] = 8918;
    this.m_mirroringShapeCharacters[8920] = 8921;
    this.m_mirroringShapeCharacters[8921] = 8920;
    this.m_mirroringShapeCharacters[8922] = 8923;
    this.m_mirroringShapeCharacters[8923] = 8922;
    this.m_mirroringShapeCharacters[8924] = 8925;
    this.m_mirroringShapeCharacters[8925] = 8924;
    this.m_mirroringShapeCharacters[8926] = 8927;
    this.m_mirroringShapeCharacters[8927] = 8926;
    this.m_mirroringShapeCharacters[8928] = 8929;
    this.m_mirroringShapeCharacters[8929] = 8928;
    this.m_mirroringShapeCharacters[8930] = 8931;
    this.m_mirroringShapeCharacters[8931] = 8930;
    this.m_mirroringShapeCharacters[8932] = 8933;
    this.m_mirroringShapeCharacters[8933] = 8932;
    this.m_mirroringShapeCharacters[8934] = 8935;
    this.m_mirroringShapeCharacters[8935] = 8934;
    this.m_mirroringShapeCharacters[8936] = 8937;
    this.m_mirroringShapeCharacters[8937] = 8936;
    this.m_mirroringShapeCharacters[8938] = 8939;
    this.m_mirroringShapeCharacters[8939] = 8938;
    this.m_mirroringShapeCharacters[8940] = 8941;
    this.m_mirroringShapeCharacters[8941] = 8940;
    this.m_mirroringShapeCharacters[8944] = 8945;
    this.m_mirroringShapeCharacters[8945] = 8944;
    this.m_mirroringShapeCharacters[8946] = 8954;
    this.m_mirroringShapeCharacters[8947] = 8955;
    this.m_mirroringShapeCharacters[8948] = 8956;
    this.m_mirroringShapeCharacters[8950] = 8957;
    this.m_mirroringShapeCharacters[8951] = 8958;
    this.m_mirroringShapeCharacters[8954] = 8946;
    this.m_mirroringShapeCharacters[8955] = 8947;
    this.m_mirroringShapeCharacters[8956] = 8948;
    this.m_mirroringShapeCharacters[8957] = 8950;
    this.m_mirroringShapeCharacters[8958] = 8951;
    this.m_mirroringShapeCharacters[8968] = 8969;
    this.m_mirroringShapeCharacters[8969] = 8968;
    this.m_mirroringShapeCharacters[8970] = 8971;
    this.m_mirroringShapeCharacters[8971] = 8970;
    this.m_mirroringShapeCharacters[9001] = 9002;
    this.m_mirroringShapeCharacters[9002] = 9001;
    this.m_mirroringShapeCharacters[10088] = 10089;
    this.m_mirroringShapeCharacters[10089] = 10088;
    this.m_mirroringShapeCharacters[10090] = 10091;
    this.m_mirroringShapeCharacters[10091] = 10090;
    this.m_mirroringShapeCharacters[10092] = 10093;
    this.m_mirroringShapeCharacters[10093] = 10092;
    this.m_mirroringShapeCharacters[10094] = 10095;
    this.m_mirroringShapeCharacters[10095] = 10094;
    this.m_mirroringShapeCharacters[10096] = 10097;
    this.m_mirroringShapeCharacters[10097] = 10096;
    this.m_mirroringShapeCharacters[10098] = 10099;
    this.m_mirroringShapeCharacters[10099] = 10098;
    this.m_mirroringShapeCharacters[10100] = 10101;
    this.m_mirroringShapeCharacters[10101] = 10100;
    this.m_mirroringShapeCharacters[10197] = 10198;
    this.m_mirroringShapeCharacters[10198] = 10197;
    this.m_mirroringShapeCharacters[10205] = 10206;
    this.m_mirroringShapeCharacters[10206] = 10205;
    this.m_mirroringShapeCharacters[10210] = 10211;
    this.m_mirroringShapeCharacters[10211] = 10210;
    this.m_mirroringShapeCharacters[10212] = 10213;
    this.m_mirroringShapeCharacters[10213] = 10212;
    this.m_mirroringShapeCharacters[10214] = 10215;
    this.m_mirroringShapeCharacters[10215] = 10214;
    this.m_mirroringShapeCharacters[10216] = 10217;
    this.m_mirroringShapeCharacters[10217] = 10216;
    this.m_mirroringShapeCharacters[10218] = 10219;
    this.m_mirroringShapeCharacters[10219] = 10218;
    this.m_mirroringShapeCharacters[10627] = 10628;
    this.m_mirroringShapeCharacters[10628] = 10627;
    this.m_mirroringShapeCharacters[10629] = 10630;
    this.m_mirroringShapeCharacters[10630] = 10629;
    this.m_mirroringShapeCharacters[10631] = 10632;
    this.m_mirroringShapeCharacters[10632] = 10631;
    this.m_mirroringShapeCharacters[10633] = 10634;
    this.m_mirroringShapeCharacters[10634] = 10633;
    this.m_mirroringShapeCharacters[10635] = 10636;
    this.m_mirroringShapeCharacters[10636] = 10635;
    this.m_mirroringShapeCharacters[10637] = 10640;
    this.m_mirroringShapeCharacters[10638] = 10639;
    this.m_mirroringShapeCharacters[10639] = 10638;
    this.m_mirroringShapeCharacters[10640] = 10637;
    this.m_mirroringShapeCharacters[10641] = 10642;
    this.m_mirroringShapeCharacters[10642] = 10641;
    this.m_mirroringShapeCharacters[10643] = 10644;
    this.m_mirroringShapeCharacters[10644] = 10643;
    this.m_mirroringShapeCharacters[10645] = 10646;
    this.m_mirroringShapeCharacters[10646] = 10645;
    this.m_mirroringShapeCharacters[10647] = 10648;
    this.m_mirroringShapeCharacters[10648] = 10647;
    this.m_mirroringShapeCharacters[10680] = 8856;
    this.m_mirroringShapeCharacters[10688] = 10689;
    this.m_mirroringShapeCharacters[10689] = 10688;
    this.m_mirroringShapeCharacters[10692] = 10693;
    this.m_mirroringShapeCharacters[10693] = 10692;
    this.m_mirroringShapeCharacters[10703] = 10704;
    this.m_mirroringShapeCharacters[10704] = 10703;
    this.m_mirroringShapeCharacters[10705] = 10706;
    this.m_mirroringShapeCharacters[10706] = 10705;
    this.m_mirroringShapeCharacters[10708] = 10709;
    this.m_mirroringShapeCharacters[10709] = 10708;
    this.m_mirroringShapeCharacters[10712] = 10713;
    this.m_mirroringShapeCharacters[10713] = 10712;
    this.m_mirroringShapeCharacters[10714] = 10715;
    this.m_mirroringShapeCharacters[10715] = 10714;
    this.m_mirroringShapeCharacters[10741] = 8725;
    this.m_mirroringShapeCharacters[10744] = 10745;
    this.m_mirroringShapeCharacters[10745] = 10744;
    this.m_mirroringShapeCharacters[10748] = 10749;
    this.m_mirroringShapeCharacters[10749] = 10748;
    this.m_mirroringShapeCharacters[10795] = 10796;
    this.m_mirroringShapeCharacters[10796] = 10795;
    this.m_mirroringShapeCharacters[10797] = 10796;
    this.m_mirroringShapeCharacters[10798] = 10797;
    this.m_mirroringShapeCharacters[10804] = 10805;
    this.m_mirroringShapeCharacters[10805] = 10804;
    this.m_mirroringShapeCharacters[10812] = 10813;
    this.m_mirroringShapeCharacters[10813] = 10812;
    this.m_mirroringShapeCharacters[10852] = 10853;
    this.m_mirroringShapeCharacters[10853] = 10852;
    this.m_mirroringShapeCharacters[10873] = 10874;
    this.m_mirroringShapeCharacters[10874] = 10873;
    this.m_mirroringShapeCharacters[10877] = 10878;
    this.m_mirroringShapeCharacters[10878] = 10877;
    this.m_mirroringShapeCharacters[10879] = 10880;
    this.m_mirroringShapeCharacters[10880] = 10879;
    this.m_mirroringShapeCharacters[10881] = 10882;
    this.m_mirroringShapeCharacters[10882] = 10881;
    this.m_mirroringShapeCharacters[10883] = 10884;
    this.m_mirroringShapeCharacters[10884] = 10883;
    this.m_mirroringShapeCharacters[10891] = 10892;
    this.m_mirroringShapeCharacters[10892] = 10891;
    this.m_mirroringShapeCharacters[10897] = 10898;
    this.m_mirroringShapeCharacters[10898] = 10897;
    this.m_mirroringShapeCharacters[10899] = 10900;
    this.m_mirroringShapeCharacters[10900] = 10899;
    this.m_mirroringShapeCharacters[10901] = 10902;
    this.m_mirroringShapeCharacters[10902] = 10901;
    this.m_mirroringShapeCharacters[10903] = 10904;
    this.m_mirroringShapeCharacters[10904] = 10903;
    this.m_mirroringShapeCharacters[10905] = 10906;
    this.m_mirroringShapeCharacters[10906] = 10905;
    this.m_mirroringShapeCharacters[10907] = 10908;
    this.m_mirroringShapeCharacters[10908] = 10907;
    this.m_mirroringShapeCharacters[10913] = 10914;
    this.m_mirroringShapeCharacters[10914] = 10913;
    this.m_mirroringShapeCharacters[10918] = 10919;
    this.m_mirroringShapeCharacters[10919] = 10918;
    this.m_mirroringShapeCharacters[10920] = 10921;
    this.m_mirroringShapeCharacters[10921] = 10920;
    this.m_mirroringShapeCharacters[10922] = 10923;
    this.m_mirroringShapeCharacters[10923] = 10922;
    this.m_mirroringShapeCharacters[10924] = 10925;
    this.m_mirroringShapeCharacters[10925] = 10924;
    this.m_mirroringShapeCharacters[10927] = 10928;
    this.m_mirroringShapeCharacters[10928] = 10927;
    this.m_mirroringShapeCharacters[10931] = 10932;
    this.m_mirroringShapeCharacters[10932] = 10931;
    this.m_mirroringShapeCharacters[10939] = 10940;
    this.m_mirroringShapeCharacters[10940] = 10939;
    this.m_mirroringShapeCharacters[10941] = 10942;
    this.m_mirroringShapeCharacters[10942] = 10941;
    this.m_mirroringShapeCharacters[10943] = 10944;
    this.m_mirroringShapeCharacters[10944] = 10943;
    this.m_mirroringShapeCharacters[10945] = 10946;
    this.m_mirroringShapeCharacters[10946] = 10945;
    this.m_mirroringShapeCharacters[10947] = 10948;
    this.m_mirroringShapeCharacters[10948] = 10947;
    this.m_mirroringShapeCharacters[10949] = 10950;
    this.m_mirroringShapeCharacters[10950] = 10949;
    this.m_mirroringShapeCharacters[10957] = 10958;
    this.m_mirroringShapeCharacters[10958] = 10957;
    this.m_mirroringShapeCharacters[10959] = 10960;
    this.m_mirroringShapeCharacters[10960] = 10959;
    this.m_mirroringShapeCharacters[10961] = 10962;
    this.m_mirroringShapeCharacters[10962] = 10961;
    this.m_mirroringShapeCharacters[10963] = 10964;
    this.m_mirroringShapeCharacters[10964] = 10963;
    this.m_mirroringShapeCharacters[10965] = 10966;
    this.m_mirroringShapeCharacters[10966] = 10965;
    this.m_mirroringShapeCharacters[10974] = 8870;
    this.m_mirroringShapeCharacters[10979] = 8873;
    this.m_mirroringShapeCharacters[10980] = 8872;
    this.m_mirroringShapeCharacters[10981] = 8875;
    this.m_mirroringShapeCharacters[10988] = 10989;
    this.m_mirroringShapeCharacters[10989] = 10988;
    this.m_mirroringShapeCharacters[10999] = 11000;
    this.m_mirroringShapeCharacters[11000] = 10999;
    this.m_mirroringShapeCharacters[11001] = 11002;
    this.m_mirroringShapeCharacters[11002] = 11001;
    this.m_mirroringShapeCharacters[12296] = 12297;
    this.m_mirroringShapeCharacters[12297] = 12296;
    this.m_mirroringShapeCharacters[12298] = 12299;
    this.m_mirroringShapeCharacters[12299] = 12298;
    this.m_mirroringShapeCharacters[12300] = 12301;
    this.m_mirroringShapeCharacters[12301] = 12300;
    this.m_mirroringShapeCharacters[12302] = 12303;
    this.m_mirroringShapeCharacters[12303] = 12302;
    this.m_mirroringShapeCharacters[12304] = 12305;
    this.m_mirroringShapeCharacters[12305] = 12304;
    this.m_mirroringShapeCharacters[12308] = 12309;
    this.m_mirroringShapeCharacters[12309] = 12308;
    this.m_mirroringShapeCharacters[12310] = 12311;
    this.m_mirroringShapeCharacters[12311] = 12310;
    this.m_mirroringShapeCharacters[12312] = 12313;
    this.m_mirroringShapeCharacters[12313] = 12312;
    this.m_mirroringShapeCharacters[12314] = 12315;
    this.m_mirroringShapeCharacters[12315] = 12314;
    this.m_mirroringShapeCharacters[65288] = 65289;
    this.m_mirroringShapeCharacters[65289] = 65288;
    this.m_mirroringShapeCharacters[65308] = 65310;
    this.m_mirroringShapeCharacters[65310] = 65308;
    this.m_mirroringShapeCharacters[65339] = 65341;
    this.m_mirroringShapeCharacters[65341] = 65339;
    this.m_mirroringShapeCharacters[65371] = 65373;
    this.m_mirroringShapeCharacters[65373] = 65371;
    this.m_mirroringShapeCharacters[65375] = 65376;
    this.m_mirroringShapeCharacters[65376] = 65375;
    this.m_mirroringShapeCharacters[65378] = 65379;
    this.m_mirroringShapeCharacters[65379] = 65378;
  }
}
