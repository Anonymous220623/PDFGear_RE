// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ColorObject
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.Themes;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation;

public class ColorObject : IColor
{
  private const int ArgbValueAlphaShift = 24;
  private const int ArgbValueBlueShift = 0;
  private const int ArgbValueGreenShift = 8;
  private const int ArgbValueRedShift = 16 /*0x10*/;
  private const short ArgbValueState = 2;
  private const short KnownColorState = 1;
  private const short MaskValueState = 2;
  private const long NotDefined = 0;
  private const short ValidNameState = 8;
  internal const byte MaxRgb = 255 /*0xFF*/;
  private ColorTransFormCollection _colorTransFormCollection;
  private byte _colorType;
  private byte _isUpdatedColor;
  private uint _colorValue;
  internal string ColorName;
  private static int[] _knownColorValue;
  private uint _newColorValue;
  internal bool IsGradient;
  internal string ReplaceColor;
  private Syncfusion.Presentation.RichText.Font _baseFont;
  public static IColor Empty = (IColor) new ColorObject();
  private int _knownColor;
  private short _state;
  private string _themeColorValue;

  internal ColorObject(bool isShapeColor)
  {
    this.IsShapeColor = isShapeColor;
    this._state = (short) 2;
  }

  internal ColorObject(bool isShapeColor, Syncfusion.Presentation.RichText.Font font)
  {
    this.IsShapeColor = isShapeColor;
    this._state = (short) 2;
    this._baseFont = font;
  }

  internal ColorObject(long value, short state, KnownColor knownColor)
  {
    this._newColorValue = (uint) value;
    this._colorValue = (uint) value;
    this._state = state;
    this._knownColor = (int) (short) knownColor;
  }

  static ColorObject()
  {
    if (ColorObject._knownColorValue != null)
      return;
    ColorObject._knownColorValue = new int[174];
    lock (ColorObject._knownColorValue)
    {
      ColorObject._knownColorValue[27] = 16777215 /*0xFFFFFF*/;
      ColorObject._knownColorValue[28] = -984833;
      ColorObject._knownColorValue[29] = -332841;
      ColorObject._knownColorValue[30] = -16711681;
      ColorObject._knownColorValue[31 /*0x1F*/] = -8388652;
      ColorObject._knownColorValue[32 /*0x20*/] = -983041;
      ColorObject._knownColorValue[33] = -657956;
      ColorObject._knownColorValue[34] = -6972;
      ColorObject._knownColorValue[35] = -16777216 /*0xFF000000*/;
      ColorObject._knownColorValue[36] = -5171;
      ColorObject._knownColorValue[37] = -16776961;
      ColorObject._knownColorValue[38] = -7722014;
      ColorObject._knownColorValue[39] = -5952982;
      ColorObject._knownColorValue[40] = -2180985;
      ColorObject._knownColorValue[41] = -10510688;
      ColorObject._knownColorValue[42] = -8388864;
      ColorObject._knownColorValue[43] = -2987746;
      ColorObject._knownColorValue[44] = -32944;
      ColorObject._knownColorValue[45] = -10185235;
      ColorObject._knownColorValue[46] = -1828;
      ColorObject._knownColorValue[47] = -2354116;
      ColorObject._knownColorValue[48 /*0x30*/] = -16711681;
      ColorObject._knownColorValue[49] = -16777077;
      ColorObject._knownColorValue[50] = -16741493;
      ColorObject._knownColorValue[51] = -4684277;
      ColorObject._knownColorValue[52] = -5658199;
      ColorObject._knownColorValue[53] = -16751616;
      ColorObject._knownColorValue[54] = -4343957;
      ColorObject._knownColorValue[55] = -7667573;
      ColorObject._knownColorValue[56] = -11179217;
      ColorObject._knownColorValue[57] = -29696;
      ColorObject._knownColorValue[58] = -6737204;
      ColorObject._knownColorValue[59] = -7667712;
      ColorObject._knownColorValue[60] = -1468806;
      ColorObject._knownColorValue[61] = -7357301;
      ColorObject._knownColorValue[62] = -12042869;
      ColorObject._knownColorValue[63 /*0x3F*/] = -13676721;
      ColorObject._knownColorValue[64 /*0x40*/] = -16724271;
      ColorObject._knownColorValue[65] = -7077677;
      ColorObject._knownColorValue[66] = -60269;
      ColorObject._knownColorValue[67] = -16728065;
      ColorObject._knownColorValue[68] = -9868951;
      ColorObject._knownColorValue[69] = -14774017;
      ColorObject._knownColorValue[70] = -5103070;
      ColorObject._knownColorValue[71] = -1296;
      ColorObject._knownColorValue[72] = -14513374;
      ColorObject._knownColorValue[73] = -65281;
      ColorObject._knownColorValue[74] = -2302756;
      ColorObject._knownColorValue[75] = -460545;
      ColorObject._knownColorValue[76] = -10496;
      ColorObject._knownColorValue[77] = -2448096;
      ColorObject._knownColorValue[78] = -8355712;
      ColorObject._knownColorValue[79] = -16744448 /*0xFF008000*/;
      ColorObject._knownColorValue[80 /*0x50*/] = -5374161;
      ColorObject._knownColorValue[81] = -983056 /*0xFFF0FFF0*/;
      ColorObject._knownColorValue[82] = -38476;
      ColorObject._knownColorValue[83] = -3318692;
      ColorObject._knownColorValue[84] = -11861886;
      ColorObject._knownColorValue[85] = -16;
      ColorObject._knownColorValue[86] = -989556;
      ColorObject._knownColorValue[87] = -1644806;
      ColorObject._knownColorValue[88] = -3851;
      ColorObject._knownColorValue[89] = -8586240;
      ColorObject._knownColorValue[90] = -1331;
      ColorObject._knownColorValue[91] = -5383962;
      ColorObject._knownColorValue[92] = -1015680;
      ColorObject._knownColorValue[93] = -2031617;
      ColorObject._knownColorValue[94] = -329006;
      ColorObject._knownColorValue[95] = -7278960;
      ColorObject._knownColorValue[96 /*0x60*/] = -2894893;
      ColorObject._knownColorValue[97] = -18751;
      ColorObject._knownColorValue[98] = -24454;
      ColorObject._knownColorValue[99] = -14634326;
      ColorObject._knownColorValue[100] = -7876870;
      ColorObject._knownColorValue[101] = -8943463;
      ColorObject._knownColorValue[102] = -5192482;
      ColorObject._knownColorValue[103] = -32;
      ColorObject._knownColorValue[104] = -16711936 /*0xFF00FF00*/;
      ColorObject._knownColorValue[105] = -13447886;
      ColorObject._knownColorValue[106] = -331546;
      ColorObject._knownColorValue[107] = -65281;
      ColorObject._knownColorValue[108] = -8388608 /*0xFF800000*/;
      ColorObject._knownColorValue[109] = -10039894;
      ColorObject._knownColorValue[110] = -16777011;
      ColorObject._knownColorValue[111] = -4565549;
      ColorObject._knownColorValue[112 /*0x70*/] = -7114533;
      ColorObject._knownColorValue[113] = -12799119;
      ColorObject._knownColorValue[114] = -8689426;
      ColorObject._knownColorValue[115] = -16713062;
      ColorObject._knownColorValue[116] = -12004916;
      ColorObject._knownColorValue[117] = -3730043;
      ColorObject._knownColorValue[118] = -15132304;
      ColorObject._knownColorValue[119] = -655366;
      ColorObject._knownColorValue[120] = -6943;
      ColorObject._knownColorValue[121] = -6987;
      ColorObject._knownColorValue[122] = -8531;
      ColorObject._knownColorValue[123] = -16777088 /*0xFF000080*/;
      ColorObject._knownColorValue[124] = -133658;
      ColorObject._knownColorValue[125] = -8355840;
      ColorObject._knownColorValue[126] = -9728477;
      ColorObject._knownColorValue[(int) sbyte.MaxValue] = -23296;
      ColorObject._knownColorValue[128 /*0x80*/] = -47872;
      ColorObject._knownColorValue[129] = -2461482;
      ColorObject._knownColorValue[130] = -1120086;
      ColorObject._knownColorValue[131] = -6751336;
      ColorObject._knownColorValue[132] = -5247250;
      ColorObject._knownColorValue[133] = -2396013;
      ColorObject._knownColorValue[134] = -4139;
      ColorObject._knownColorValue[135] = -9543;
      ColorObject._knownColorValue[136] = -3308225;
      ColorObject._knownColorValue[137] = -16181;
      ColorObject._knownColorValue[138] = -2252579;
      ColorObject._knownColorValue[139] = -5185306;
      ColorObject._knownColorValue[140] = -8388480;
      ColorObject._knownColorValue[141] = -65536;
      ColorObject._knownColorValue[142] = -4419697;
      ColorObject._knownColorValue[143] = -12490271;
      ColorObject._knownColorValue[144 /*0x90*/] = -7650029;
      ColorObject._knownColorValue[145] = -360334;
      ColorObject._knownColorValue[146] = -744352;
      ColorObject._knownColorValue[147] = -13726889;
      ColorObject._knownColorValue[148] = -2578;
      ColorObject._knownColorValue[149] = -6270419;
      ColorObject._knownColorValue[150] = -4144960;
      ColorObject._knownColorValue[151] = -7876885;
      ColorObject._knownColorValue[152] = -9807155;
      ColorObject._knownColorValue[153] = -9404272;
      ColorObject._knownColorValue[154] = -1286;
      ColorObject._knownColorValue[155] = -16711809;
      ColorObject._knownColorValue[156] = -12156236;
      ColorObject._knownColorValue[157] = -2968436;
      ColorObject._knownColorValue[158] = -16744320;
      ColorObject._knownColorValue[159] = -2572328;
      ColorObject._knownColorValue[160 /*0xA0*/] = -40121;
      ColorObject._knownColorValue[161] = -12525360;
      ColorObject._knownColorValue[162] = -1146130;
      ColorObject._knownColorValue[163] = -663885;
      ColorObject._knownColorValue[164] = -1;
      ColorObject._knownColorValue[165] = -657931;
      ColorObject._knownColorValue[166] = -256;
      ColorObject._knownColorValue[167] = -6632142;
    }
  }

  internal bool IsUpdatedColor
  {
    get => this._isUpdatedColor == (byte) 1;
    set
    {
      if (value)
        this._isUpdatedColor = (byte) 1;
      else
        this._isUpdatedColor = (byte) 0;
    }
  }

  internal ColorTransFormCollection ColorTransFormCollection
  {
    get
    {
      return this._colorTransFormCollection ?? (this._colorTransFormCollection = new ColorTransFormCollection());
    }
  }

  internal ColorType ColorType
  {
    get
    {
      switch ((int) this._colorType & 15)
      {
        case 0:
          return ColorType.Automatic;
        case 1:
          return ColorType.RGB;
        case 2:
          return ColorType.Theme;
        case 3:
          return ColorType.IndexedColor;
        case 4:
          return ColorType.AutomaticIndex;
        default:
          return ColorType.Automatic;
      }
    }
  }

  internal uint GetColorValue => this._colorValue;

  internal bool IsShapeColor
  {
    get => ((int) this._colorType & 16 /*0x10*/) != 0;
    set
    {
      this._colorType &= (byte) 239;
      if (!value)
        return;
      this._colorType |= (byte) 16 /*0x10*/;
    }
  }

  internal int ApplyTransformation(int baseColor)
  {
    if (this._colorTransFormCollection == null || this._colorTransFormCollection.Count == 0)
      return baseColor;
    this._state = (short) 2;
    return this._colorTransFormCollection.ApplyTransformation(baseColor, this.IsShapeColor);
  }

  internal void UpdateColorObject(object value)
  {
    if ((int) this._newColorValue == (int) this._colorValue && this._themeColorValue == null)
      return;
    this.IsUpdatedColor = true;
    switch (this.ColorType)
    {
      case ColorType.Automatic:
      case ColorType.AutomaticIndex:
        this._newColorValue = this._colorValue;
        break;
      default:
        ColorExtension.Empty.ToArgb();
        int baseColor = (int) this._colorValue & 16777215 /*0xFFFFFF*/;
        switch ((int) this._colorType & 15)
        {
          case 0:
          case 4:
            this._newColorValue = (uint) ColorExtension.Empty.ToArgb();
            return;
          case 1:
            this._newColorValue = (uint) this.ApplyTransformation(baseColor);
            return;
          case 2:
            Theme theme = (Theme) null;
            switch (value)
            {
              case Syncfusion.Presentation.Presentation _:
                theme = ((Syncfusion.Presentation.Presentation) value).Theme;
                break;
              case MasterSlide _:
                theme = ((MasterSlide) value).Theme;
                break;
            }
            this._newColorValue = (uint) this.ApplyTransformation(theme.GetThemeColorValue(this._themeColorValue));
            return;
          case 3:
            return;
          default:
            return;
        }
    }
  }

  internal string GetColorString() => this._themeColorValue;

  internal static double HueToRgb(double n1, double n2, double hue)
  {
    if (hue < 0.0)
      ++hue;
    if (hue > 1.0)
      --hue;
    return 6.0 * hue >= 1.0 ? (2.0 * hue >= 1.0 ? (3.0 * hue >= 2.0 ? n1 : n1 + (n2 - n1) * (2.0 / 3.0 - hue) * 6.0) : n2) : n1 + (n2 - n1) * 6.0 * hue;
  }

  internal bool ContainsTransformation(ColorObject colorObject)
  {
    return this._colorTransFormCollection != null && this._colorTransFormCollection.Count != 0 ? colorObject._colorTransFormCollection != null && colorObject._colorTransFormCollection.Count != 0 : colorObject._colorTransFormCollection == null || colorObject._colorTransFormCollection.Count == 0;
  }

  internal void Copy(ColorObject colorObject)
  {
    this._colorValue = colorObject._colorValue;
    this._colorType = colorObject._colorType;
    if (colorObject._colorTransFormCollection != null)
    {
      this._colorTransFormCollection = new ColorTransFormCollection();
      this._colorTransFormCollection.Copy(colorObject.ColorTransFormCollection);
    }
    else
      this._colorTransFormCollection = (ColorTransFormCollection) null;
  }

  internal bool Equals(ColorObject colorObject)
  {
    if (this.IsAutomaticIndex())
      return colorObject.IsAutomaticIndex();
    return !colorObject.IsAutomaticIndex() && this.ColorType == colorObject.ColorType && ((int) this._colorValue & 16777215 /*0xFFFFFF*/) == ((int) colorObject._colorValue & 16777215 /*0xFFFFFF*/) && this.IsTransformationEquals(colorObject);
  }

  internal int GetColorInt() => (int) this._colorValue & 16777215 /*0xFFFFFF*/;

  internal bool IsAutomaticIndex()
  {
    return this.ColorType == ColorType.Automatic || this.ColorType == ColorType.AutomaticIndex;
  }

  internal bool IsTransformationEquals(ColorObject colorObject)
  {
    return this._colorTransFormCollection != null && this._colorTransFormCollection.Count != 0 ? colorObject._colorTransFormCollection != null && colorObject._colorTransFormCollection.Count != 0 && this._colorTransFormCollection.Equals(colorObject._colorTransFormCollection) : colorObject._colorTransFormCollection == null || colorObject._colorTransFormCollection.Count == 0;
  }

  internal void ResetColorTransForm()
  {
    this._colorTransFormCollection = (ColorTransFormCollection) null;
  }

  internal void SetAutoMatic(bool isAutoMatic)
  {
    this._colorType &= (byte) 240 /*0xF0*/;
    if (isAutoMatic)
      return;
    this._colorType |= (byte) 1;
  }

  internal void SetColor(ColorType colorType, int value)
  {
    this._colorType &= (byte) 240 /*0xF0*/;
    this._colorValue &= 4278190080U /*0xFF000000*/;
    switch (colorType)
    {
      case ColorType.Automatic:
        this._colorValue |= (uint) value;
        break;
      case ColorType.AutomaticIndex:
        this._colorValue |= (uint) value;
        this._colorType |= (byte) 4;
        break;
      case ColorType.RGB:
        this._colorValue |= (uint) value;
        this._colorType |= (byte) 1;
        break;
      case ColorType.IndexedColor:
        this._colorValue |= (uint) value;
        this._colorType |= (byte) 3;
        break;
      case ColorType.Theme:
        this._colorValue |= (uint) value;
        this._colorType |= (byte) 2;
        break;
    }
  }

  internal void SetColor(ColorType colorType, string value)
  {
    this._colorType &= (byte) 240 /*0xF0*/;
    this._themeColorValue = value;
    if (colorType != ColorType.Theme)
      return;
    this._colorType |= (byte) 2;
  }

  public static IColor AliceBlue => (IColor) new ColorObject(KnownColor.AliceBlue);

  public static IColor AntiqueWhite => (IColor) new ColorObject(KnownColor.AntiqueWhite);

  public static IColor Aqua => (IColor) new ColorObject(KnownColor.Aqua);

  public static IColor Aquamarine => (IColor) new ColorObject(KnownColor.Aquamarine);

  public static IColor Azure => (IColor) new ColorObject(KnownColor.Azure);

  public static IColor Beige => (IColor) new ColorObject(KnownColor.Beige);

  public static IColor Bisque => (IColor) new ColorObject(KnownColor.Bisque);

  public static IColor Black => (IColor) new ColorObject(KnownColor.Black);

  public static IColor BlanchedAlmond => (IColor) new ColorObject(KnownColor.BlanchedAlmond);

  public static IColor Blue => (IColor) new ColorObject(KnownColor.Blue);

  public static IColor BlueViolet => (IColor) new ColorObject(KnownColor.BlueViolet);

  public static IColor Brown => (IColor) new ColorObject(KnownColor.Brown);

  public static IColor BurlyWood => (IColor) new ColorObject(KnownColor.BurlyWood);

  public static IColor CadetBlue => (IColor) new ColorObject(KnownColor.CadetBlue);

  public static IColor Chartreuse => (IColor) new ColorObject(KnownColor.Chartreuse);

  public static IColor Chocolate => (IColor) new ColorObject(KnownColor.Chocolate);

  public static IColor Coral => (IColor) new ColorObject(KnownColor.Coral);

  public static IColor CornflowerBlue => (IColor) new ColorObject(KnownColor.CornflowerBlue);

  public static IColor Cornsilk => (IColor) new ColorObject(KnownColor.Cornsilk);

  public static IColor Crimson => (IColor) new ColorObject(KnownColor.Crimson);

  public static IColor Cyan => (IColor) new ColorObject(KnownColor.Cyan);

  public static IColor DarkBlue => (IColor) new ColorObject(KnownColor.DarkBlue);

  public static IColor DarkCyan => (IColor) new ColorObject(KnownColor.DarkCyan);

  public static IColor DarkGoldenrod => (IColor) new ColorObject(KnownColor.DarkGoldenrod);

  public static IColor DarkGray => (IColor) new ColorObject(KnownColor.DarkGray);

  public static IColor DarkGreen => (IColor) new ColorObject(KnownColor.DarkGreen);

  public static IColor DarkKhaki => (IColor) new ColorObject(KnownColor.DarkKhaki);

  public static IColor DarkMagenta => (IColor) new ColorObject(KnownColor.DarkMagenta);

  public static IColor DarkOliveGreen => (IColor) new ColorObject(KnownColor.DarkOliveGreen);

  public static IColor DarkOrange => (IColor) new ColorObject(KnownColor.DarkOrange);

  public static IColor DarkOrchid => (IColor) new ColorObject(KnownColor.DarkOrchid);

  public static IColor DarkRed => (IColor) new ColorObject(KnownColor.DarkRed);

  public static IColor DarkSalmon => (IColor) new ColorObject(KnownColor.DarkSalmon);

  public static IColor DarkSeaGreen => (IColor) new ColorObject(KnownColor.DarkSeaGreen);

  public static IColor DarkSlateBlue => (IColor) new ColorObject(KnownColor.DarkSlateBlue);

  public static IColor DarkSlateGray => (IColor) new ColorObject(KnownColor.DarkSlateGray);

  public static IColor DarkTurquoise => (IColor) new ColorObject(KnownColor.DarkTurquoise);

  public static IColor DarkViolet => (IColor) new ColorObject(KnownColor.DarkViolet);

  public static IColor DeepPink => (IColor) new ColorObject(KnownColor.DeepPink);

  public static IColor DeepSkyBlue => (IColor) new ColorObject(KnownColor.DeepSkyBlue);

  public static IColor DimGray => (IColor) new ColorObject(KnownColor.DimGray);

  public static IColor DodgerBlue => (IColor) new ColorObject(KnownColor.DodgerBlue);

  public static IColor Firebrick => (IColor) new ColorObject(KnownColor.Firebrick);

  public static IColor FloralWhite => (IColor) new ColorObject(KnownColor.FloralWhite);

  public static IColor ForestGreen => (IColor) new ColorObject(KnownColor.ForestGreen);

  public static IColor Fuchsia => (IColor) new ColorObject(KnownColor.Fuchsia);

  public static IColor Gainsboro => (IColor) new ColorObject(KnownColor.Gainsboro);

  public static IColor GhostWhite => (IColor) new ColorObject(KnownColor.GhostWhite);

  public static IColor Gold => (IColor) new ColorObject(KnownColor.Gold);

  public static IColor Goldenrod => (IColor) new ColorObject(KnownColor.Goldenrod);

  public static IColor Gray => (IColor) new ColorObject(KnownColor.Gray);

  public static IColor Green => (IColor) new ColorObject(KnownColor.Green);

  public static IColor GreenYellow => (IColor) new ColorObject(KnownColor.GreenYellow);

  public static IColor Honeydew => (IColor) new ColorObject(KnownColor.Honeydew);

  public static IColor HotPink => (IColor) new ColorObject(KnownColor.HotPink);

  public static IColor IndianRed => (IColor) new ColorObject(KnownColor.IndianRed);

  public static IColor Indigo => (IColor) new ColorObject(KnownColor.Indigo);

  public static IColor Ivory => (IColor) new ColorObject(KnownColor.Ivory);

  public static IColor Khaki => (IColor) new ColorObject(KnownColor.Khaki);

  public static IColor Lavender => (IColor) new ColorObject(KnownColor.Lavender);

  public static IColor LavenderBlush => (IColor) new ColorObject(KnownColor.LavenderBlush);

  public static IColor LawnGreen => (IColor) new ColorObject(KnownColor.LawnGreen);

  public static IColor LemonChiffon => (IColor) new ColorObject(KnownColor.LemonChiffon);

  public static IColor LightBlue => (IColor) new ColorObject(KnownColor.LightBlue);

  public static IColor LightCoral => (IColor) new ColorObject(KnownColor.LightCoral);

  public static IColor LightCyan => (IColor) new ColorObject(KnownColor.LightCyan);

  public static IColor LightGoldenrodYellow
  {
    get => (IColor) new ColorObject(KnownColor.LightGoldenrodYellow);
  }

  public static IColor LightGray => (IColor) new ColorObject(KnownColor.LightGray);

  public static IColor LightGreen => (IColor) new ColorObject(KnownColor.LightGreen);

  public static IColor LightPink => (IColor) new ColorObject(KnownColor.LightPink);

  public static IColor LightSalmon => (IColor) new ColorObject(KnownColor.LightSalmon);

  public static IColor LightSeaGreen => (IColor) new ColorObject(KnownColor.LightSeaGreen);

  public static IColor LightSkyBlue => (IColor) new ColorObject(KnownColor.LightSkyBlue);

  public static IColor LightSlateGray => (IColor) new ColorObject(KnownColor.LightSlateGray);

  public static IColor LightSteelBlue => (IColor) new ColorObject(KnownColor.LightSteelBlue);

  public static IColor LightYellow => (IColor) new ColorObject(KnownColor.LightYellow);

  public static IColor Lime => (IColor) new ColorObject(KnownColor.Lime);

  public static IColor LimeGreen => (IColor) new ColorObject(KnownColor.LimeGreen);

  public static IColor Linen => (IColor) new ColorObject(KnownColor.Linen);

  public static IColor Magenta => (IColor) new ColorObject(KnownColor.Magenta);

  public static IColor Maroon => (IColor) new ColorObject(KnownColor.Maroon);

  public static IColor MediumAquamarine => (IColor) new ColorObject(KnownColor.MediumAquamarine);

  public static IColor MediumBlue => (IColor) new ColorObject(KnownColor.MediumBlue);

  public static IColor MediumOrchid => (IColor) new ColorObject(KnownColor.MediumOrchid);

  public static IColor MediumPurple => (IColor) new ColorObject(KnownColor.MediumPurple);

  public static IColor MediumSeaGreen => (IColor) new ColorObject(KnownColor.MediumSeaGreen);

  public static IColor MediumSlateBlue => (IColor) new ColorObject(KnownColor.MediumSlateBlue);

  public static IColor MediumSpringGreen => (IColor) new ColorObject(KnownColor.MediumSpringGreen);

  public static IColor MediumTurquoise => (IColor) new ColorObject(KnownColor.MediumTurquoise);

  public static IColor MediumVioletRed => (IColor) new ColorObject(KnownColor.MediumVioletRed);

  public static IColor MidnightBlue => (IColor) new ColorObject(KnownColor.MidnightBlue);

  public static IColor MintCream => (IColor) new ColorObject(KnownColor.MintCream);

  public static IColor MistyRose => (IColor) new ColorObject(KnownColor.MistyRose);

  public static IColor Moccasin => (IColor) new ColorObject(KnownColor.Moccasin);

  public static IColor NavajoWhite => (IColor) new ColorObject(KnownColor.NavajoWhite);

  public static IColor Navy => (IColor) new ColorObject(KnownColor.Navy);

  public static IColor OldLace => (IColor) new ColorObject(KnownColor.OldLace);

  public static IColor Olive => (IColor) new ColorObject(KnownColor.Olive);

  public static IColor OliveDrab => (IColor) new ColorObject(KnownColor.OliveDrab);

  public static IColor Orange => (IColor) new ColorObject(KnownColor.Orange);

  public static IColor OrangeRed => (IColor) new ColorObject(KnownColor.OrangeRed);

  public static IColor Orchid => (IColor) new ColorObject(KnownColor.Orchid);

  public static IColor PaleGoldenrod => (IColor) new ColorObject(KnownColor.PaleGoldenrod);

  public static IColor PaleGreen => (IColor) new ColorObject(KnownColor.PaleGreen);

  public static IColor PaleTurquoise => (IColor) new ColorObject(KnownColor.PaleTurquoise);

  public static IColor PaleVioletRed => (IColor) new ColorObject(KnownColor.PaleVioletRed);

  public static IColor PapayaWhip => (IColor) new ColorObject(KnownColor.PapayaWhip);

  public static IColor PeachPuff => (IColor) new ColorObject(KnownColor.PeachPuff);

  public static IColor Peru => (IColor) new ColorObject(KnownColor.Peru);

  public static IColor Pink => (IColor) new ColorObject(KnownColor.Pink);

  public static IColor Plum => (IColor) new ColorObject(KnownColor.Plum);

  public static IColor PowderBlue => (IColor) new ColorObject(KnownColor.PowderBlue);

  public static IColor Purple => (IColor) new ColorObject(KnownColor.Purple);

  public static IColor Red => (IColor) new ColorObject(KnownColor.Red);

  public static IColor RosyBrown => (IColor) new ColorObject(KnownColor.RosyBrown);

  public static IColor RoyalBlue => (IColor) new ColorObject(KnownColor.RoyalBlue);

  public static IColor SaddleBrown => (IColor) new ColorObject(KnownColor.SaddleBrown);

  public static IColor Salmon => (IColor) new ColorObject(KnownColor.Salmon);

  public static IColor SandyBrown => (IColor) new ColorObject(KnownColor.SandyBrown);

  public static IColor SeaGreen => (IColor) new ColorObject(KnownColor.SeaGreen);

  public static IColor SeaShell => (IColor) new ColorObject(KnownColor.SeaShell);

  public static IColor Sienna => (IColor) new ColorObject(KnownColor.Sienna);

  public static IColor Silver => (IColor) new ColorObject(KnownColor.Silver);

  public static IColor SkyBlue => (IColor) new ColorObject(KnownColor.SkyBlue);

  public static IColor SlateBlue => (IColor) new ColorObject(KnownColor.SlateBlue);

  public static IColor SlateGray => (IColor) new ColorObject(KnownColor.SlateGray);

  public static IColor Snow => (IColor) new ColorObject(KnownColor.Snow);

  public static IColor SpringGreen => (IColor) new ColorObject(KnownColor.SpringGreen);

  public static IColor SteelBlue => (IColor) new ColorObject(KnownColor.SteelBlue);

  public static IColor Tan => (IColor) new ColorObject(KnownColor.Tan);

  public static IColor Teal => (IColor) new ColorObject(KnownColor.Teal);

  public static IColor Thistle => (IColor) new ColorObject(KnownColor.Thistle);

  public static IColor Tomato => (IColor) new ColorObject(KnownColor.Tomato);

  public static IColor Transparent => (IColor) new ColorObject(KnownColor.Transparent);

  public static IColor Turquoise => (IColor) new ColorObject(KnownColor.Turquoise);

  public static IColor Violet => (IColor) new ColorObject(KnownColor.Violet);

  public static IColor Wheat => (IColor) new ColorObject(KnownColor.Wheat);

  public static IColor White => (IColor) new ColorObject(KnownColor.White);

  public static IColor WhiteSmoke => (IColor) new ColorObject(KnownColor.WhiteSmoke);

  public static IColor Yellow => (IColor) new ColorObject(KnownColor.Yellow);

  public static IColor YellowGreen => (IColor) new ColorObject(KnownColor.YellowGreen);

  internal ColorObject(KnownColor knownColor)
  {
    this._colorValue = 0U;
    this._state = (short) 1;
    this._knownColor = (int) knownColor;
  }

  internal ColorObject()
  {
  }

  internal ColorObject(bool isShapeColor, short state)
  {
    this.IsShapeColor = isShapeColor;
    this._state = state;
  }

  internal string ThemeColorValue
  {
    get => this._themeColorValue;
    set => this._themeColorValue = value;
  }

  public byte A => (byte) ((ulong) (this.Value >> 24) & (ulong) byte.MaxValue);

  public byte B => (byte) ((ulong) this.Value & (ulong) byte.MaxValue);

  public byte G => (byte) ((ulong) (this.Value >> 8) & (ulong) byte.MaxValue);

  public bool IsEmpty => this._state == (short) 0;

  public bool IsKnownColor => ((int) this._state & 1) != 0;

  public bool IsNamedColor => ((int) this._state & 8) != 0 || this.IsKnownColor;

  public bool IsSystemColor
  {
    get
    {
      if (!this.IsKnownColor)
        return false;
      return this._knownColor <= 26 || this._knownColor > 167;
    }
  }

  public byte R => (byte) ((ulong) (this.Value >> 16 /*0x10*/) & (ulong) byte.MaxValue);

  public Color SystemColor
  {
    get
    {
      Syncfusion.Presentation.Presentation presentation = (Syncfusion.Presentation.Presentation) null;
      this.ColorTransFormCollection.GetColorModeValue(ColorMode.Alpha);
      if (this._newColorValue == 0U && this._themeColorValue == null)
        this.UpdateColorObject((object) presentation);
      return this.A < byte.MaxValue && this.A > (byte) 0 ? Color.FromArgb((int) this._newColorValue) : Color.FromArgb((int) this._newColorValue | -16777216 /*0xFF000000*/);
    }
    set
    {
      if (value.A < byte.MaxValue)
        this.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, (int) ((100.0 - (double) ((int) value.A * 100) / (double) byte.MaxValue) * 1000.0));
      this.SetColor(ColorType.RGB, value.ToArgb());
      this._newColorValue = 0U;
      if (this._baseFont == null)
        return;
      this._baseFont.IsColorChanged = true;
    }
  }

  private long Value
  {
    get
    {
      if (((int) this._state & 2) != 0)
        return (long) this._newColorValue;
      return this.IsKnownColor ? (long) ColorObject._knownColorValue[this._knownColor] : 0L;
    }
  }

  public static IColor FromArgb(int argb)
  {
    return (IColor) new ColorObject((long) argb & (long) uint.MaxValue, (short) 2, (KnownColor) 0);
  }

  public static IColor FromArgb(int alpha, int red, int green, int blue)
  {
    return (IColor) new ColorObject(ColorObject.UpdateArgb((byte) alpha, (byte) red, (byte) green, (byte) blue), (short) 2, (KnownColor) 0);
  }

  public static IColor FromArgb(int alpha, IColor baseColor)
  {
    return (IColor) new ColorObject(ColorObject.UpdateArgb((byte) alpha, baseColor.R, baseColor.G, baseColor.B), (short) 2, (KnownColor) 0);
  }

  public static IColor FromArgb(int red, int green, int blue)
  {
    return ColorObject.FromArgb(0, red, green, blue);
  }

  public static IColor FromKnownColor(KnownColor color) => (IColor) new ColorObject(color);

  public float GetBrightness()
  {
    float num1 = (float) this.R / (float) byte.MaxValue;
    float num2 = (float) this.G / (float) byte.MaxValue;
    float num3 = (float) this.B / (float) byte.MaxValue;
    float num4 = num1;
    if ((double) num2 > (double) num4)
      num4 = num2;
    if ((double) num3 > (double) num4)
      num4 = num3;
    float num5 = num1;
    if ((double) num2 < (double) num5)
      num5 = num2;
    if ((double) num3 < (double) num5)
      num5 = num3;
    return (float) (((double) num4 + (double) num5) / 2.0);
  }

  public float GetHue()
  {
    if (ColorObject.Red == ColorObject.Green && ColorObject.Green == ColorObject.Blue)
      return 0.0f;
    float num1 = (float) this.R / (float) byte.MaxValue;
    float num2 = (float) this.G / (float) byte.MaxValue;
    float num3 = (float) this.B / (float) byte.MaxValue;
    float num4 = 0.0f;
    float num5 = num1;
    if ((double) num2 > (double) num5)
      num5 = num2;
    if ((double) num3 > (double) num5)
      num5 = num3;
    float num6 = num1;
    if ((double) num2 < (double) num6)
      num6 = num2;
    if ((double) num3 < (double) num6)
      num6 = num3;
    float num7 = num5 - num6;
    if ((double) num1 == (double) num5)
      num4 = (num2 - num3) / num7;
    else if ((double) num2 == (double) num5)
      num4 = (float) (2.0 + ((double) num3 - (double) num1) / (double) num7);
    else if ((double) num3 == (double) num5)
      num4 = (float) (4.0 + ((double) num1 - (double) num2) / (double) num7);
    float hue = num4 * 60f;
    if ((double) hue < 0.0)
      hue += 360f;
    return hue;
  }

  public float GetSaturation()
  {
    float num1 = (float) this.R / (float) byte.MaxValue;
    float num2 = (float) this.G / (float) byte.MaxValue;
    float num3 = (float) this.B / (float) byte.MaxValue;
    float num4 = 0.0f;
    float num5 = num1;
    if ((double) num2 > (double) num5)
      num5 = num2;
    if ((double) num3 > (double) num5)
      num5 = num3;
    float num6 = num1;
    if ((double) num2 < (double) num6)
      num6 = num2;
    if ((double) num3 < (double) num6)
      num6 = num3;
    return (double) num5 == (double) num6 ? num4 : (((double) num5 + (double) num6) / 2.0 > 0.5 ? (float) (((double) num5 - (double) num6) / (2.0 - (double) num5 - (double) num6)) : (float) (((double) num5 - (double) num6) / ((double) num5 + (double) num6)));
  }

  public int ToArgb() => (int) this.Value;

  public KnownColor ToKnownColor() => (KnownColor) this._knownColor;

  private static long UpdateArgb(byte alpha, byte red, byte green, byte blue)
  {
    return (long) (uint) ((int) red << 16 /*0x10*/ | (int) green << 8 | (int) blue | (int) alpha << 24) & (long) uint.MaxValue;
  }

  internal void Close() => this.CloseAll();

  private void CloseAll()
  {
    if (this._colorTransFormCollection == null)
      return;
    this._colorTransFormCollection.Close();
    this._colorTransFormCollection = (ColorTransFormCollection) null;
  }

  internal ColorObject CloneColorObject()
  {
    ColorObject colorObject = (ColorObject) this.MemberwiseClone();
    if (this._colorTransFormCollection != null)
      colorObject._colorTransFormCollection = this._colorTransFormCollection.Clone();
    return colorObject;
  }
}
