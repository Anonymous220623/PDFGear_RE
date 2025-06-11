// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.PdfRichTextStyle
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

public struct PdfRichTextStyle : IEquatable<PdfRichTextStyle>
{
  private static readonly PdfRichTextStyle _default = new PdfRichTextStyle()
  {
    TextAlignment = new PdfRichTextStyleTextAlignment?(PdfRichTextStyleTextAlignment.Left),
    VerticalAlignment = new float?(),
    FontSize = new float?(12f),
    FontStyle = new PdfRichTextStyleFontStyle?(),
    FontWeight = new PdfRichTextStyleFontWeight?(PdfRichTextStyleFontWeight.Normal),
    FontFamily = "Arial",
    Color = new System.Windows.Media.Color?(System.Windows.Media.Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0)),
    TextDecoration = new PdfRichTextStyleTextDecoration?(PdfRichTextStyleTextDecoration.None),
    FontStretch = new PdfRichTextStyleFontStretch?(PdfRichTextStyleFontStretch.Normal)
  };

  public static PdfRichTextStyle Default => PdfRichTextStyle._default;

  public PdfRichTextStyleTextAlignment? TextAlignment { get; set; }

  public float? VerticalAlignment { get; set; }

  public float? FontSize { get; set; }

  public PdfRichTextStyleFontStyle? FontStyle { get; set; }

  public PdfRichTextStyleFontWeight? FontWeight { get; set; }

  public string FontFamily { get; set; }

  public System.Windows.Media.Color? Color { get; set; }

  public PdfRichTextStyleTextDecoration? TextDecoration { get; set; }

  public PdfRichTextStyleFontStretch? FontStretch { get; set; }

  public string ToString(PdfRichTextStyle? defaultStyle) => this.Merge(defaultStyle).ToString();

  public override string ToString()
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    if (this.TextAlignment.HasValue)
      stringBuilder1.Append("text-align:").Append(PdfRichTextStyle.ToStyleString(this.TextAlignment.Value)).Append(';');
    float num;
    if (this.VerticalAlignment.HasValue)
    {
      StringBuilder stringBuilder2 = stringBuilder1.Append("vertical-align:");
      num = this.VerticalAlignment.Value;
      string str = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      stringBuilder2.Append(str).Append("pt;");
    }
    if (this.FontStyle.HasValue && this.FontWeight.HasValue && this.FontSize.HasValue && !string.IsNullOrEmpty(this.FontFamily))
    {
      stringBuilder1.Append("font:").Append(PdfRichTextStyle.ToStyleString(this.FontStyle.Value, this.FontWeight.Value, this.FontSize.Value, this.FontFamily)).Append(';');
    }
    else
    {
      if (this.FontSize.HasValue)
      {
        StringBuilder stringBuilder3 = stringBuilder1.Append("font-size:");
        num = this.FontSize.Value;
        string str = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        stringBuilder3.Append(str).Append("pt;");
      }
      if (this.FontStyle.HasValue)
        stringBuilder1.Append("font-style:").Append(PdfRichTextStyle.ToStyleString(this.FontStyle.Value)).Append(';');
      if (this.FontWeight.HasValue)
        stringBuilder1.Append("font-weight:").Append(PdfRichTextStyle.ToStyleString(this.FontWeight.Value)).Append(';');
      if (!string.IsNullOrEmpty(this.FontFamily))
        stringBuilder1.Append("font-family:").Append(this.FontFamily).Append(';');
    }
    if (this.Color.HasValue)
      stringBuilder1.Append("color:").Append(PdfRichTextStyle.ToStyleString(this.Color.Value)).Append(';');
    if (this.TextDecoration.HasValue)
      stringBuilder1.Append("text-decoration:").Append(PdfRichTextStyle.ToStyleString(this.TextDecoration.Value)).Append(';');
    if (this.FontStretch.HasValue)
      stringBuilder1.Append("font-stretch:").Append(PdfRichTextStyle.ToStyleString(this.FontStretch.Value)).Append(';');
    return stringBuilder1.ToString();
  }

  public override bool Equals(object obj) => obj is PdfRichTextStyle other && this.Equals(other);

  public bool Equals(PdfRichTextStyle other)
  {
    PdfRichTextStyleTextAlignment? textAlignment1 = this.TextAlignment;
    PdfRichTextStyleTextAlignment? textAlignment2 = other.TextAlignment;
    int num;
    if (textAlignment1.GetValueOrDefault() == textAlignment2.GetValueOrDefault() & textAlignment1.HasValue == textAlignment2.HasValue)
    {
      float? nullable1 = this.VerticalAlignment;
      float? nullable2 = other.VerticalAlignment;
      if ((double) nullable1.GetValueOrDefault() == (double) nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue)
      {
        nullable2 = this.FontSize;
        nullable1 = other.FontSize;
        if ((double) nullable2.GetValueOrDefault() == (double) nullable1.GetValueOrDefault() & nullable2.HasValue == nullable1.HasValue)
        {
          PdfRichTextStyleFontStyle? fontStyle1 = this.FontStyle;
          PdfRichTextStyleFontStyle? fontStyle2 = other.FontStyle;
          if (fontStyle1.GetValueOrDefault() == fontStyle2.GetValueOrDefault() & fontStyle1.HasValue == fontStyle2.HasValue)
          {
            PdfRichTextStyleFontWeight? fontWeight1 = this.FontWeight;
            PdfRichTextStyleFontWeight? fontWeight2 = other.FontWeight;
            if (fontWeight1.GetValueOrDefault() == fontWeight2.GetValueOrDefault() & fontWeight1.HasValue == fontWeight2.HasValue && this.FontFamily == other.FontFamily)
            {
              System.Windows.Media.Color? color1 = this.Color;
              System.Windows.Media.Color? color2 = other.Color;
              if ((color1.HasValue == color2.HasValue ? (color1.HasValue ? (color1.GetValueOrDefault() == color2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
              {
                PdfRichTextStyleTextDecoration? textDecoration1 = this.TextDecoration;
                PdfRichTextStyleTextDecoration? textDecoration2 = other.TextDecoration;
                if (textDecoration1.GetValueOrDefault() == textDecoration2.GetValueOrDefault() & textDecoration1.HasValue == textDecoration2.HasValue)
                {
                  PdfRichTextStyleFontStretch? fontStretch1 = this.FontStretch;
                  PdfRichTextStyleFontStretch? fontStretch2 = other.FontStretch;
                  num = fontStretch1.GetValueOrDefault() == fontStretch2.GetValueOrDefault() & fontStretch1.HasValue == fontStretch2.HasValue ? 1 : 0;
                  goto label_9;
                }
              }
            }
          }
        }
      }
    }
    num = 0;
label_9:
    return num != 0;
  }

  public PdfRichTextStyle Merge(PdfRichTextStyle style2)
  {
    PdfRichTextStyle pdfRichTextStyle = new PdfRichTextStyle();
    pdfRichTextStyle.TextAlignment = style2.TextAlignment ?? this.TextAlignment;
    ref PdfRichTextStyle local1 = ref pdfRichTextStyle;
    float? nullable1 = style2.VerticalAlignment;
    float? nullable2 = nullable1 ?? this.VerticalAlignment;
    local1.VerticalAlignment = nullable2;
    ref PdfRichTextStyle local2 = ref pdfRichTextStyle;
    nullable1 = style2.FontSize;
    float? nullable3 = nullable1 ?? this.FontSize;
    local2.FontSize = nullable3;
    pdfRichTextStyle.FontStyle = style2.FontStyle ?? this.FontStyle;
    pdfRichTextStyle.FontWeight = style2.FontWeight ?? this.FontWeight;
    pdfRichTextStyle.FontFamily = string.IsNullOrEmpty(style2.FontFamily) ? this.FontFamily : style2.FontFamily;
    pdfRichTextStyle.Color = style2.Color ?? this.Color;
    pdfRichTextStyle.TextDecoration = style2.TextDecoration ?? this.TextDecoration;
    pdfRichTextStyle.FontStretch = style2.FontStretch ?? this.FontStretch;
    return pdfRichTextStyle;
  }

  public PdfRichTextStyle Merge(PdfRichTextStyle? style2)
  {
    return style2.HasValue ? this.Merge(style2.Value) : this;
  }

  public static bool TryParse(string str, out PdfRichTextStyle value)
  {
    return PdfRichTextStyle.TryParse(str, new PdfRichTextStyle(), out value);
  }

  public static bool TryParse(
    string str,
    PdfRichTextStyle defaultStyle,
    out PdfRichTextStyle value)
  {
    value = PdfRichTextStyle.Default;
    if (string.IsNullOrEmpty(str) || str.Length < 2)
      return false;
    ReadOnlySpan<char> str1 = str.AsSpan(0, str.Length);
    if (str1[0] == '"')
    {
      if (str1[str1.Length - 1] != '"')
        return false;
      str1 = str.AsSpan(1, str.Length - 2);
    }
    Dictionary<string, string> core = PdfRichTextStyle.ParseCore(str1);
    if (core == null)
      return false;
    value = PdfRichTextStyle.ApplyStyleCore(core, defaultStyle);
    return true;
  }

  private static Dictionary<string, string> ParseCore(ReadOnlySpan<char> str)
  {
    string str1 = string.Empty;
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    int num = 0;
    int start = -1;
    int index = 0;
    bool flag;
    ReadOnlySpan<char> readOnlySpan;
    for (flag = false; !flag && index < str.Length; ++index)
    {
      char ch = str[index];
      switch (ch)
      {
        case ' ':
          continue;
        case ':':
          if (num == 1)
          {
            if (start != -1)
            {
              readOnlySpan = str.Slice(start, index - start);
              str1 = readOnlySpan.ToString().Trim();
              if (string.IsNullOrEmpty(str1))
              {
                flag = true;
                continue;
              }
              start = -1;
              num = 2;
              continue;
            }
            flag = true;
            continue;
          }
          flag = true;
          continue;
        case ';':
          switch (num)
          {
            case 1:
              flag = true;
              continue;
            case 2:
              if (start != -1)
              {
                readOnlySpan = str.Slice(start, index - start);
                string str2 = readOnlySpan.ToString();
                dictionary[str1.Trim()] = str2.Trim();
                start = -1;
                num = 0;
              }
              else
                flag = true;
              continue;
            default:
              continue;
          }
        default:
          if (ch >= '0' && ch <= '9' || ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'X' || ch == '-' || ch == '#' || ch == '(' || ch == ')')
          {
            if (num == 0)
            {
              if (ch >= '0' && ch <= '9' || ch == '-')
                flag = true;
              num = 1;
            }
            if (start == -1)
            {
              start = index;
              continue;
            }
            continue;
          }
          continue;
      }
    }
    if (start != -1)
    {
      if (num == 2)
      {
        readOnlySpan = str.Slice(start, str.Length - start);
        string str3 = readOnlySpan.ToString();
        dictionary[str1.Trim()] = str3.Trim();
      }
      else
        flag = true;
    }
    return flag ? (Dictionary<string, string>) null : dictionary;
  }

  private static PdfRichTextStyle ApplyStyleCore(
    Dictionary<string, string> dict,
    PdfRichTextStyle defaultStyle)
  {
    PdfRichTextStyle pdfRichTextStyle = defaultStyle;
    string font;
    string fontFamily;
    float? fontSize;
    PdfRichTextStyleFontWeight? fontWeight;
    PdfRichTextStyleFontStyle? fontStyle;
    if (dict.TryGetValue("font", out font) && !string.IsNullOrEmpty(font) && PdfRichTextStyle.TryParseFont(font, out fontFamily, out fontSize, out fontWeight, out fontStyle))
    {
      pdfRichTextStyle.FontSize = fontSize;
      pdfRichTextStyle.FontFamily = fontFamily;
      pdfRichTextStyle.FontWeight = fontWeight ?? pdfRichTextStyle.FontWeight;
      pdfRichTextStyle.FontStyle = fontStyle ?? pdfRichTextStyle.FontStyle;
    }
    foreach (KeyValuePair<string, string> keyValuePair in dict)
    {
      string key = keyValuePair.Key;
      string v = keyValuePair.Value;
      if (key == "text-align")
        pdfRichTextStyle.TextAlignment = PdfRichTextStyle.ParseTextAlignment(v, pdfRichTextStyle.TextAlignment);
      if (key == "vertical-align")
        pdfRichTextStyle.VerticalAlignment = PdfRichTextStyle.ParseVerticalAlignment(v, pdfRichTextStyle.VerticalAlignment);
      else if (key == "font-size")
        pdfRichTextStyle.FontSize = PdfRichTextStyle.ParseFontSize(v, pdfRichTextStyle.FontSize);
      else if (key == "font-style")
        pdfRichTextStyle.FontStyle = PdfRichTextStyle.ParseFontStyle(v);
      else if (key == "font-weight")
        pdfRichTextStyle.FontWeight = PdfRichTextStyle.ParseFontWeight(v, pdfRichTextStyle.FontWeight);
      else if (key == "font-family")
        pdfRichTextStyle.FontFamily = v;
      else if (key == "color")
        pdfRichTextStyle.Color = PdfRichTextStyle.ParseColor(v, pdfRichTextStyle.Color);
      else if (key == "text-decoration")
        pdfRichTextStyle.TextDecoration = new PdfRichTextStyleTextDecoration?(PdfRichTextStyle.ParseTextDecoration(v));
      else if (key == "font-stretch")
        pdfRichTextStyle.FontStretch = PdfRichTextStyle.ParseFontStretch(v, pdfRichTextStyle.FontStretch);
    }
    return pdfRichTextStyle;
  }

  private static PdfRichTextStyleTextAlignment? ParseTextAlignment(
    string v,
    PdfRichTextStyleTextAlignment? defaultValue)
  {
    switch (v)
    {
      case "left":
        return new PdfRichTextStyleTextAlignment?(PdfRichTextStyleTextAlignment.Left);
      case "center":
        return new PdfRichTextStyleTextAlignment?(PdfRichTextStyleTextAlignment.Center);
      case "right":
        return new PdfRichTextStyleTextAlignment?(PdfRichTextStyleTextAlignment.Right);
      default:
        return defaultValue;
    }
  }

  private static float? ParseVerticalAlignment(string v, float? defaultValue)
  {
    v = v.Trim();
    float result;
    return v.Length > 2 && v[v.Length - 2] == 'p' && v[v.Length - 1] == 't' && float.TryParse(v.Substring(0, v.Length - 2), NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? new float?(result) : defaultValue;
  }

  private static float? ParseFontSize(string v, float? defaultValue)
  {
    v = v.Trim();
    bool flag = false;
    if (v.Length > 2 && v[v.Length - 2] == 'p' && v[v.Length - 1] == 't')
    {
      v = v.Substring(0, v.Length - 2);
      flag = true;
    }
    Decimal result;
    if (!Decimal.TryParse(v, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result))
      return defaultValue;
    return flag ? new float?((float) result) : new float?((float) (result * 72M / 96M));
  }

  private static PdfRichTextStyleFontStyle? ParseFontStyle(string v)
  {
    switch (v)
    {
      case "italic":
        return new PdfRichTextStyleFontStyle?(PdfRichTextStyleFontStyle.Italic);
      case "normal":
        return new PdfRichTextStyleFontStyle?(PdfRichTextStyleFontStyle.Normal);
      default:
        return new PdfRichTextStyleFontStyle?();
    }
  }

  private static PdfRichTextStyleFontWeight? ParseFontWeight(
    string v,
    PdfRichTextStyleFontWeight? defaultValue)
  {
    switch (v)
    {
      case "normal":
        return new PdfRichTextStyleFontWeight?(PdfRichTextStyleFontWeight.Normal);
      case "bold":
        return new PdfRichTextStyleFontWeight?(PdfRichTextStyleFontWeight.Bold);
      default:
        int result;
        return int.TryParse(v, out result) && result % 100 == 0 && result <= 900 && result >= 400 ? new PdfRichTextStyleFontWeight?((PdfRichTextStyleFontWeight) result) : defaultValue;
    }
  }

  private static System.Windows.Media.Color? ParseColor(string v, System.Windows.Media.Color? defaultValue)
  {
    if (v.Length > 0)
    {
      if (v[0] == '#')
      {
        try
        {
          if (v.Length == 2)
            v = $"#{v[1]}{v[1]}{v[1]}";
          return new System.Windows.Media.Color?((System.Windows.Media.Color) ColorConverter.ConvertFromString(v));
        }
        catch
        {
        }
      }
      else
      {
        Match match = Regex.Match(v, "rgb[\\s]*\\((.+?)\\)");
        if (match.Success && match.Groups.Count > 1)
        {
          string[] strArray = match.Groups[1].Value.Split(',');
          double result1;
          double result2;
          double result3;
          if (strArray.Length == 3 && double.TryParse(strArray[0].Trim(), NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result1) && double.TryParse(strArray[1].Trim(), NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result2) && double.TryParse(strArray[2].Trim(), NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result3))
            return new System.Windows.Media.Color?(System.Windows.Media.Color.FromRgb((byte) Math.Max(0.0, Math.Min((double) byte.MaxValue, result1)), (byte) Math.Max(0.0, Math.Min((double) byte.MaxValue, result2)), (byte) Math.Max(0.0, Math.Min((double) byte.MaxValue, result3))));
        }
      }
    }
    return defaultValue;
  }

  private static PdfRichTextStyleTextDecoration ParseTextDecoration(string v)
  {
    switch (v)
    {
      case "underline":
        return PdfRichTextStyleTextDecoration.Underline;
      case "line-through":
        return PdfRichTextStyleTextDecoration.LineThrough;
      default:
        return PdfRichTextStyleTextDecoration.None;
    }
  }

  private static PdfRichTextStyleFontStretch? ParseFontStretch(
    string v,
    PdfRichTextStyleFontStretch? defaultValue)
  {
    switch (v)
    {
      case "ultra-condensed":
        return new PdfRichTextStyleFontStretch?(PdfRichTextStyleFontStretch.UltraCondensed);
      case "extra-condensed":
        return new PdfRichTextStyleFontStretch?(PdfRichTextStyleFontStretch.ExtraCondensed);
      case "condensed":
        return new PdfRichTextStyleFontStretch?(PdfRichTextStyleFontStretch.Condensed);
      case "semi-condensed":
        return new PdfRichTextStyleFontStretch?(PdfRichTextStyleFontStretch.SemiCondensed);
      case "normal":
        return new PdfRichTextStyleFontStretch?(PdfRichTextStyleFontStretch.Normal);
      case "semi-expanded":
        return new PdfRichTextStyleFontStretch?(PdfRichTextStyleFontStretch.SemiExpanded);
      case "expanded":
        return new PdfRichTextStyleFontStretch?(PdfRichTextStyleFontStretch.Expanded);
      case "extra-expanded":
        return new PdfRichTextStyleFontStretch?(PdfRichTextStyleFontStretch.ExtraExpanded);
      case "andultra-expanded":
        return new PdfRichTextStyleFontStretch?(PdfRichTextStyleFontStretch.AndultraExpanded);
      default:
        return defaultValue;
    }
  }

  private static bool TryParseFont(
    string font,
    out string fontFamily,
    out float? fontSize,
    out PdfRichTextStyleFontWeight? fontWeight,
    out PdfRichTextStyleFontStyle? fontStyle)
  {
    fontFamily = (string) null;
    fontSize = new float?(12f);
    fontWeight = new PdfRichTextStyleFontWeight?();
    fontStyle = new PdfRichTextStyleFontStyle?();
    if (string.IsNullOrEmpty(font))
      return false;
    List<string> list = ((IEnumerable<string>) font.Split(' ')).Where<string>((Func<string, bool>) (c => c != "")).ToList<string>();
    if (list.Count == 1)
      return false;
    for (int index = list.Count - 1; index >= 1; --index)
    {
      List<string> source = new List<string>();
      source.Add(list[index]);
      for (; index >= 1; --index)
      {
        string str = list[index - 1];
        if (str[str.Length - 1] == ',')
        {
          source.Add(str);
          list.RemoveAt(index);
        }
        else
          break;
      }
      if (source.Count > 1)
      {
        StringBuilder stringBuilder = source.Reverse<string>().Aggregate<string, StringBuilder>(new StringBuilder(), (Func<StringBuilder, string, StringBuilder>) ((s, c) => s.Append(c).Append(' ')));
        --stringBuilder.Length;
        list[index] = stringBuilder.ToString();
      }
    }
    if (list.Count > 4)
      return false;
    string str1 = (string) null;
    float? nullable1 = new float?(12f);
    PdfRichTextStyleFontStyle? nullable2 = new PdfRichTextStyleFontStyle?();
    PdfRichTextStyleFontWeight? nullable3 = new PdfRichTextStyleFontWeight?();
    bool flag = false;
    for (int index = list.Count - 1; index >= 0 && !flag; --index)
    {
      int num1 = list.Count - index - 1;
      if (num1 == 0)
        str1 = list[index];
      if (num1 == 1)
      {
        float? fontSize1 = PdfRichTextStyle.ParseFontSize(list[index], new float?(-1f));
        float? nullable4 = fontSize1;
        float num2 = -1f;
        if ((double) nullable4.GetValueOrDefault() == (double) num2 & nullable4.HasValue)
        {
          fontSize1 = PdfRichTextStyle.ParseFontSize(list[index + 1], new float?(-1f));
          nullable4 = fontSize1;
          float num3 = -1f;
          if (!((double) nullable4.GetValueOrDefault() == (double) num3 & nullable4.HasValue))
            str1 = list[index];
        }
        nullable4 = fontSize1;
        float num4 = -1f;
        if ((double) nullable4.GetValueOrDefault() == (double) num4 & nullable4.HasValue)
        {
          flag = true;
          break;
        }
        nullable1 = fontSize1;
      }
      if (num1 == 2)
      {
        nullable3 = PdfRichTextStyle.ParseFontWeight(list[index], new PdfRichTextStyleFontWeight?());
        if (!nullable3.HasValue)
        {
          nullable2 = PdfRichTextStyle.ParseFontStyle(list[index]);
          if (!nullable2.HasValue)
          {
            flag = true;
            break;
          }
        }
      }
      if (num1 == 3)
      {
        if (nullable2.HasValue)
        {
          flag = true;
          break;
        }
        nullable2 = PdfRichTextStyle.ParseFontStyle(list[index]);
        if (!nullable2.HasValue)
        {
          flag = true;
          break;
        }
      }
    }
    if (flag)
      return false;
    fontFamily = str1;
    fontSize = nullable1;
    fontWeight = nullable3;
    fontStyle = nullable2;
    return true;
  }

  private static string ToStyleString(System.Windows.Media.Color color)
  {
    return $"#{color.R:x2}{color.G:x2}{color.B:x2}";
  }

  private static string ToStyleString(PdfRichTextStyleTextAlignment value)
  {
    switch (value)
    {
      case PdfRichTextStyleTextAlignment.Right:
        return "right";
      case PdfRichTextStyleTextAlignment.Center:
        return "center";
      default:
        return "left";
    }
  }

  private static string ToStyleString(PdfRichTextStyleFontStyle value)
  {
    switch (value)
    {
      case PdfRichTextStyleFontStyle.Italic:
        return "italic";
      default:
        return "normal";
    }
  }

  private static string ToStyleString(PdfRichTextStyleFontWeight value)
  {
    switch (value)
    {
      case PdfRichTextStyleFontWeight.Bold:
        return "bold";
      case PdfRichTextStyleFontWeight.W100:
      case PdfRichTextStyleFontWeight.W200:
      case PdfRichTextStyleFontWeight.W300:
      case PdfRichTextStyleFontWeight.W400:
      case PdfRichTextStyleFontWeight.W500:
      case PdfRichTextStyleFontWeight.W600:
      case PdfRichTextStyleFontWeight.W700:
      case PdfRichTextStyleFontWeight.W800:
      case PdfRichTextStyleFontWeight.W900:
        return value.ToString();
      default:
        return "normal";
    }
  }

  private static string ToStyleString(PdfRichTextStyleTextDecoration value)
  {
    switch (value)
    {
      case PdfRichTextStyleTextDecoration.Underline:
        return "underline";
      case PdfRichTextStyleTextDecoration.LineThrough:
        return "line-through";
      default:
        return "none";
    }
  }

  private static string ToStyleString(PdfRichTextStyleFontStretch value)
  {
    switch (value)
    {
      case PdfRichTextStyleFontStretch.UltraCondensed:
        return "ultra-condensed";
      case PdfRichTextStyleFontStretch.ExtraCondensed:
        return "extra-condensed";
      case PdfRichTextStyleFontStretch.Condensed:
        return "condensed";
      case PdfRichTextStyleFontStretch.SemiCondensed:
        return "semi-condensed";
      case PdfRichTextStyleFontStretch.SemiExpanded:
        return "semi-expanded";
      case PdfRichTextStyleFontStretch.Expanded:
        return "expanded";
      case PdfRichTextStyleFontStretch.ExtraExpanded:
        return "extra-expanded";
      case PdfRichTextStyleFontStretch.AndultraExpanded:
        return "andultra-expanded";
      default:
        return "normal";
    }
  }

  private static string ToStyleString(
    PdfRichTextStyleFontStyle fontStyle,
    PdfRichTextStyleFontWeight fontWeight,
    float fontSize,
    string fontFamily)
  {
    return FormattableStringFactory.Create("{0} {1} {2}pt {3}", (object) PdfRichTextStyle.ToStyleString(fontStyle), (object) PdfRichTextStyle.ToStyleString(fontWeight), (object) fontSize, (object) fontFamily).ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }
}
