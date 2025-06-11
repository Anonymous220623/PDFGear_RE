// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfColor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public struct PdfColor
{
  private const float MaxColourChannelValue = 255f;
  private static Dictionary<int, object> s_rgbStrings = new Dictionary<int, object>();
  private static Dictionary<float, object> s_grayStringsSroke = new Dictionary<float, object>();
  private static Dictionary<float, object> s_grayStringsFill = new Dictionary<float, object>();
  private static PdfColor s_emptyColor = new PdfColor();
  private byte m_red;
  private float m_cyan;
  private byte m_green;
  private float m_magenta;
  private byte m_blue;
  private float m_yellow;
  private float m_black;
  private float m_gray;
  private byte m_alpha;
  private bool m_isFilled;

  public static PdfColor Empty => PdfColor.s_emptyColor;

  public bool IsEmpty => !this.m_isFilled;

  public byte B
  {
    get => this.m_blue;
    set
    {
      this.m_blue = value;
      this.AssignCMYK(this.m_red, this.m_green, this.m_blue);
      this.m_isFilled = true;
    }
  }

  public float Blue => (float) this.B / (float) byte.MaxValue;

  public float C
  {
    get => this.m_cyan;
    set
    {
      this.m_cyan = (double) value >= 0.0 ? ((double) value <= 1.0 ? value : 1f) : 0.0f;
      this.AssignRGB(this.m_cyan, this.m_magenta, this.m_yellow, this.m_black);
      this.m_isFilled = true;
    }
  }

  public byte G
  {
    get => this.m_green;
    set
    {
      this.m_green = value;
      this.AssignCMYK(this.m_red, this.m_green, this.m_blue);
      this.m_isFilled = true;
    }
  }

  public float Green => (float) this.G / (float) byte.MaxValue;

  public float Gray
  {
    get => (float) ((int) this.m_red + (int) this.m_green + (int) this.m_blue) / 765f;
    set
    {
      this.m_gray = (double) value >= 0.0 ? ((double) value <= 1.0 ? value : 1f) : 0.0f;
      this.R = (byte) ((double) this.m_gray * (double) byte.MaxValue);
      this.G = (byte) ((double) this.m_gray * (double) byte.MaxValue);
      this.B = (byte) ((double) this.m_gray * (double) byte.MaxValue);
      this.AssignCMYK(this.m_red, this.m_green, this.m_blue);
      this.m_isFilled = true;
    }
  }

  public float K
  {
    get => this.m_black;
    set
    {
      this.m_black = (double) value >= 0.0 ? ((double) value <= 1.0 ? value : 1f) : 0.0f;
      this.AssignRGB(this.m_cyan, this.m_magenta, this.m_yellow, this.m_black);
      this.m_isFilled = true;
    }
  }

  public float M
  {
    get => this.m_magenta;
    set
    {
      this.m_magenta = (double) value >= 0.0 ? ((double) value <= 1.0 ? value : 1f) : 0.0f;
      this.AssignRGB(this.m_cyan, this.m_magenta, this.m_yellow, this.m_black);
      this.m_isFilled = true;
    }
  }

  public byte R
  {
    get => this.m_red;
    set
    {
      this.m_red = value;
      this.AssignCMYK(this.m_red, this.m_green, this.m_blue);
      this.m_isFilled = true;
    }
  }

  public float Red => (float) this.R / (float) byte.MaxValue;

  public float Y
  {
    get => this.m_yellow;
    set
    {
      if ((double) value < 0.0)
      {
        this.m_yellow = 0.0f;
      }
      else
      {
        this.m_yellow = (double) value <= 1.0 ? value : 1f;
        this.AssignRGB(this.m_cyan, this.m_magenta, this.m_yellow, this.m_black);
        this.m_isFilled = true;
      }
    }
  }

  internal byte A
  {
    get => this.m_alpha;
    set
    {
      if (value < (byte) 0)
        this.m_alpha = (byte) 0;
      else if ((int) this.m_alpha != (int) value)
        this.m_alpha = value;
      this.m_isFilled = true;
    }
  }

  public PdfColor(PdfColor color)
  {
    this.m_red = color.R;
    this.m_cyan = color.C;
    this.m_green = color.G;
    this.m_magenta = color.M;
    this.m_blue = color.B;
    this.m_yellow = color.Y;
    this.m_black = color.K;
    this.m_gray = color.Gray;
    this.m_alpha = color.m_alpha;
    this.m_isFilled = this.m_alpha != (byte) 0;
  }

  public PdfColor(Color color)
    : this(color.A, color.R, color.G, color.B)
  {
    if (color.Equals((object) Color.Empty))
    {
      this.m_isFilled = false;
    }
    else
    {
      if (!PdfColor.CompareColours(color, Color.Empty))
        return;
      this.m_isFilled = false;
    }
  }

  public PdfColor(float gray)
  {
    if ((double) gray < 0.0)
      gray = 0.0f;
    if ((double) gray > 1.0)
      gray = 1f;
    this.m_red = (byte) ((double) gray * (double) byte.MaxValue);
    this.m_green = (byte) ((double) gray * (double) byte.MaxValue);
    this.m_blue = (byte) ((double) gray * (double) byte.MaxValue);
    this.m_cyan = gray;
    this.m_magenta = gray;
    this.m_yellow = gray;
    this.m_black = gray;
    this.m_gray = gray;
    this.m_alpha = byte.MaxValue;
    this.m_isFilled = true;
  }

  public PdfColor(byte red, byte green, byte blue)
    : this(byte.MaxValue, red, green, blue)
  {
  }

  internal PdfColor(float red, float green, float blue)
    : this((byte) ((double) red * (double) byte.MaxValue), (byte) ((double) green * (double) byte.MaxValue), (byte) ((double) blue * (double) byte.MaxValue))
  {
  }

  internal PdfColor(byte a, byte red, byte green, byte blue)
  {
    this.m_black = 0.0f;
    this.m_cyan = 0.0f;
    this.m_magenta = 0.0f;
    this.m_yellow = 0.0f;
    this.m_gray = 0.0f;
    this.m_red = red;
    this.m_green = green;
    this.m_blue = blue;
    this.m_alpha = a;
    this.m_isFilled = this.m_alpha != (byte) 0;
    this.AssignCMYK(red, green, blue);
  }

  public PdfColor(float cyan, float magenta, float yellow, float black)
  {
    this.m_red = (byte) 0;
    this.m_cyan = cyan;
    this.m_green = (byte) 0;
    this.m_magenta = magenta;
    this.m_blue = (byte) 0;
    this.m_yellow = yellow;
    this.m_black = black;
    this.m_gray = 0.0f;
    this.m_alpha = byte.MaxValue;
    this.m_isFilled = true;
    this.AssignRGB(cyan, magenta, yellow, black);
  }

  public int ToArgb() => PdfColor.FromRGBColor((int) this.R, (int) this.G, (int) this.B).ToArgb();

  private static Color FromRGBColor(int r, int g, int b) => Color.FromArgb(r, g, b);

  public static implicit operator PdfColor(Color color) => new PdfColor(color);

  public static implicit operator Color(PdfColor color)
  {
    return Color.FromArgb((int) color.A, (int) color.R, (int) color.G, (int) color.B);
  }

  public static bool operator ==(PdfColor colour1, PdfColor colour2) => colour1.Equals(colour2);

  public static bool operator !=(PdfColor colour1, PdfColor colour2) => !(colour1 == colour2);

  public override bool Equals(object obj)
  {
    return obj is PdfColor colour ? this.Equals(colour) : base.Equals(obj);
  }

  public bool Equals(PdfColor colour)
  {
    bool flag = false;
    if (this.IsEmpty && colour.IsEmpty)
      flag = true;
    if (!this.IsEmpty || !colour.IsEmpty)
      flag = flag | (double) this.m_black != (double) colour.m_black | (double) this.m_cyan != (double) colour.m_cyan | (double) this.m_magenta != (double) colour.m_magenta | (double) this.m_yellow != (double) colour.m_yellow | (double) this.m_gray != (double) colour.m_gray | (int) this.m_red != (int) colour.m_red | (int) this.m_green != (int) colour.m_green | (int) this.m_blue != (int) colour.m_blue | (int) this.m_alpha != (int) colour.m_alpha;
    return !flag;
  }

  public override int GetHashCode() => base.GetHashCode();

  private string RGBToString(bool ifStroking)
  {
    byte r = this.R;
    byte g = this.G;
    byte b = this.B;
    int key = ((int) r << 16 /*0x10*/) + ((int) g << 8) + (int) b;
    if (ifStroking)
      key += 16777216 /*0x01000000*/;
    string str;
    lock (PdfColor.s_rgbStrings)
    {
      object obj = (object) null;
      if (PdfColor.s_rgbStrings.Count > 0 && PdfColor.s_rgbStrings.ContainsKey(key))
        obj = PdfColor.s_rgbStrings[key];
      if (obj == null)
      {
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:#0.######} {1:#0.######} {2:#0.######} {3}{4}", (object) ((float) r / (float) byte.MaxValue), (object) ((float) g / (float) byte.MaxValue), (object) ((float) b / (float) byte.MaxValue), ifStroking ? (object) "RG" : (object) "rg", (object) "\r\n");
        PdfColor.s_rgbStrings[key] = (object) str;
      }
      else
        str = obj.ToString();
    }
    return str;
  }

  private string CalRGBToString(bool ifStroking)
  {
    if (this.R > (byte) 1 || this.R < (byte) 0 || this.G > (byte) 1 || this.G < (byte) 0 || this.G > (byte) 1 || this.G < (byte) 0)
      return this.CalLabToString(ifStroking);
    byte num1 = Convert.ToByte((int) this.R * (int) byte.MaxValue);
    byte num2 = Convert.ToByte((int) this.G * (int) byte.MaxValue);
    byte num3 = Convert.ToByte((int) this.B * (int) byte.MaxValue);
    int key = ((int) num1 << 16 /*0x10*/) + ((int) num2 << 8) + (int) num3;
    if (ifStroking)
      key += 16777216 /*0x01000000*/;
    string str = string.Empty;
    lock (PdfColor.s_rgbStrings)
    {
      if (PdfColor.s_rgbStrings.Count > 0 && !PdfColor.s_rgbStrings.ContainsKey(key))
      {
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:#0.######} {1:#0.######} {2:#0.######} {3}{4}", (object) ((float) num1 / (float) byte.MaxValue), (object) ((float) num2 / (float) byte.MaxValue), (object) ((float) num3 / (float) byte.MaxValue), ifStroking ? (object) "SC" : (object) "sc", (object) "\r\n");
        PdfColor.s_rgbStrings[key] = (object) str;
      }
      else if (PdfColor.s_rgbStrings.Count > 0)
      {
        if (PdfColor.s_rgbStrings.ContainsKey(key))
          str = PdfColor.s_rgbStrings[key].ToString();
      }
    }
    return str;
  }

  private string CalLabToString(bool ifStroking)
  {
    byte r = this.R;
    byte g = this.G;
    byte b = this.B;
    int key = ((int) r << 16 /*0x10*/) + ((int) g << 8) + (int) b;
    if (ifStroking)
      key += 16777216 /*0x01000000*/;
    string str;
    lock (PdfColor.s_rgbStrings)
    {
      object rgbString = PdfColor.s_rgbStrings.ContainsKey(key) ? PdfColor.s_rgbStrings[key] : (object) null;
      if (rgbString == null)
      {
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:#0.######} {1:#0.######} {2:#0.######} {3}{4}", (object) (float) r, (object) (float) g, (object) (float) b, ifStroking ? (object) "SC" : (object) "sc", (object) "\r\n");
        PdfColor.s_rgbStrings[key] = (object) str;
      }
      else
        str = rgbString.ToString();
    }
    return str;
  }

  private string CalGrayscaleToString(bool ifStroking)
  {
    float gray = this.Gray;
    string str;
    lock (PdfColor.s_grayStringsSroke)
    {
      object obj = ifStroking ? (PdfColor.s_grayStringsSroke.ContainsKey(gray) ? PdfColor.s_grayStringsSroke[gray] : (object) null) : (PdfColor.s_grayStringsFill.ContainsKey(gray) ? PdfColor.s_grayStringsFill[gray] : (object) null);
      if (obj == null)
      {
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}{2}", (object) gray, ifStroking ? (object) "SC" : (object) "sc", (object) "\r\n");
        if (ifStroking)
          PdfColor.s_grayStringsSroke[gray] = (object) str;
        else
          PdfColor.s_grayStringsFill[gray] = (object) str;
      }
      else
        str = obj.ToString();
    }
    return str;
  }

  private string IccRGBToString(bool ifStroking)
  {
    if (this.R > (byte) 1 || this.R < (byte) 0 || this.G > (byte) 1 || this.G < (byte) 0 || this.G > (byte) 1 || this.G < (byte) 0)
      return this.CalLabToString(ifStroking);
    byte num1 = Convert.ToByte((int) this.R * (int) byte.MaxValue);
    byte num2 = Convert.ToByte((int) this.G * (int) byte.MaxValue);
    byte num3 = Convert.ToByte((int) this.B * (int) byte.MaxValue);
    int key = ((int) num1 << 16 /*0x10*/) + ((int) num2 << 8) + (int) num3;
    if (ifStroking)
      key += 16777216 /*0x01000000*/;
    string str;
    lock (PdfColor.s_rgbStrings)
    {
      object obj = (object) null;
      if (PdfColor.s_rgbStrings.Count > 0 && PdfColor.s_rgbStrings.ContainsKey(key))
        obj = PdfColor.s_rgbStrings[key];
      if (obj == null)
      {
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:#0.######} {1:#0.######} {2:#0.######} {3}{4}", (object) ((float) num1 / (float) byte.MaxValue), (object) ((float) num2 / (float) byte.MaxValue), (object) ((float) num3 / (float) byte.MaxValue), ifStroking ? (object) "SCN" : (object) "scn", (object) "\r\n");
        PdfColor.s_rgbStrings[key] = (object) str;
      }
      else
        str = obj.ToString();
    }
    return str;
  }

  private string CalCMYKToString(bool ifStroking)
  {
    return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:#0.######} {1:#0.######} {2:#0.######} {3:#0.######} {4}{5}", (object) this.m_cyan, (object) this.m_magenta, (object) this.m_yellow, (object) this.m_black, ifStroking ? (object) "SCN" : (object) "scn", (object) "\r\n");
  }

  private string IccLabToString(bool ifStroking)
  {
    byte r = this.R;
    byte g = this.G;
    byte b = this.B;
    int key = ((int) r << 16 /*0x10*/) + ((int) g << 8) + (int) b;
    if (ifStroking)
      key += 16777216 /*0x01000000*/;
    string str;
    lock (PdfColor.s_rgbStrings)
    {
      object obj = (object) null;
      if (PdfColor.s_rgbStrings.Count > 0 && PdfColor.s_rgbStrings.ContainsKey(key))
        obj = PdfColor.s_rgbStrings[key];
      if (obj == null)
      {
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:#0.######} {1:#0.######} {2:#0.######} {3}{4}", (object) (float) r, (object) (float) g, (object) (float) b, ifStroking ? (object) "SC" : (object) "sc", (object) "\r\n");
        PdfColor.s_rgbStrings[key] = (object) str;
      }
      else
        str = obj.ToString();
    }
    return str;
  }

  private string IccGrayscaleToString(bool ifStroking)
  {
    float gray = this.Gray;
    string str;
    lock (PdfColor.s_grayStringsSroke)
    {
      object obj = ifStroking ? (PdfColor.s_grayStringsSroke.ContainsKey(gray) ? PdfColor.s_grayStringsSroke[gray] : (object) null) : (PdfColor.s_grayStringsSroke.ContainsKey(gray) ? PdfColor.s_grayStringsFill[gray] : (object) null);
      if (obj == null)
      {
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}{2}", (object) gray, ifStroking ? (object) "SCN" : (object) "scn", (object) "\r\n");
        if (ifStroking)
          PdfColor.s_grayStringsSroke[gray] = (object) str;
        else
          PdfColor.s_grayStringsFill[gray] = (object) str;
      }
      else
        str = obj.ToString();
    }
    return str;
  }

  internal string IndexedToString(bool ifStroking)
  {
    float g = (float) this.G;
    string str;
    lock (PdfColor.s_grayStringsSroke)
    {
      object obj = ifStroking ? (PdfColor.s_grayStringsSroke.ContainsKey(g) ? PdfColor.s_grayStringsSroke[g] : (object) null) : (PdfColor.s_grayStringsFill.ContainsKey(g) ? PdfColor.s_grayStringsFill[g] : (object) null);
      if (obj == null)
      {
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}{2}", (object) g, ifStroking ? (object) "SC" : (object) "sc", (object) "\r\n");
        if (ifStroking)
          PdfColor.s_grayStringsSroke[g] = (object) str;
        else
          PdfColor.s_grayStringsFill[g] = (object) str;
      }
      else
        str = obj.ToString();
    }
    return str;
  }

  private string GrayscaleToString(bool ifStroking)
  {
    float gray = this.Gray;
    string str;
    lock (PdfColor.s_grayStringsSroke)
    {
      object obj = ifStroking ? (PdfColor.s_grayStringsSroke.ContainsKey(gray) ? PdfColor.s_grayStringsSroke[gray] : (object) null) : (PdfColor.s_grayStringsFill.ContainsKey(gray) ? PdfColor.s_grayStringsFill[gray] : (object) null);
      if (obj == null)
      {
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}{2}", (object) gray, ifStroking ? (object) "G" : (object) "g", (object) "\r\n");
        if (ifStroking)
          PdfColor.s_grayStringsSroke[gray] = (object) str;
        else
          PdfColor.s_grayStringsFill[gray] = (object) str;
      }
      else
        str = obj.ToString();
    }
    return str;
  }

  private string CMYKToString(bool ifStroking)
  {
    return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0:#0.######} {1:#0.######} {2:#0.######} {3:#0.######} {4}{5}", (object) this.m_cyan, (object) this.m_magenta, (object) this.m_yellow, (object) this.m_black, ifStroking ? (object) "K" : (object) "k", (object) "\r\n");
  }

  private void RGBToStringBuilder(StringBuilder sb, bool stroke)
  {
    float number1 = (float) this.R / (float) byte.MaxValue;
    float number2 = (float) this.G / (float) byte.MaxValue;
    float number3 = (float) this.B / (float) byte.MaxValue;
    sb.Append(PdfNumber.FloatToString(number1));
    sb.Append(" ");
    sb.Append(PdfNumber.FloatToString(number2));
    sb.Append(" ");
    sb.Append(PdfNumber.FloatToString(number3));
    sb.Append(" ");
    if (stroke)
      sb.Append("RG");
    else
      sb.Append("rg");
  }

  private void CMYKToStringBuilder(StringBuilder sb, bool stroke)
  {
    sb.Append(PdfNumber.FloatToString(this.m_cyan));
    sb.Append(" ");
    sb.Append(PdfNumber.FloatToString(this.m_magenta));
    sb.Append(" ");
    sb.Append(PdfNumber.FloatToString(this.m_yellow));
    sb.Append(" ");
    sb.Append(PdfNumber.FloatToString(this.m_black));
    sb.Append(" ");
    if (stroke)
      sb.Append("K");
    else
      sb.Append("k");
  }

  private void GrayscaleToStringBuilder(StringBuilder sb, bool stroke)
  {
    sb.Append(PdfNumber.FloatToString(this.Gray));
    sb.Append(" ");
    if (stroke)
      sb.Append("G");
    else
      sb.Append("g");
  }

  internal string ToString(PdfColorSpace colorSpace, bool stroke)
  {
    if (this.IsEmpty)
      return string.Empty;
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_X1A2001)
      colorSpace = PdfColorSpace.CMYK;
    switch (colorSpace)
    {
      case PdfColorSpace.RGB:
        return this.RGBToString(stroke);
      case PdfColorSpace.CMYK:
        return this.CMYKToString(stroke);
      case PdfColorSpace.GrayScale:
        return this.GrayscaleToString(stroke);
      default:
        throw new ArgumentException("Unsupported colour space: " + (object) colorSpace);
    }
  }

  internal string CalToString(PdfColorSpace colorSpace, bool stroke)
  {
    if (this.IsEmpty)
      return string.Empty;
    switch (colorSpace)
    {
      case PdfColorSpace.RGB:
        return this.CalRGBToString(stroke);
      case PdfColorSpace.CMYK:
        return this.CMYKToString(stroke);
      case PdfColorSpace.GrayScale:
        return this.CalGrayscaleToString(stroke);
      default:
        throw new ArgumentException("Unsupported colour space: " + (object) colorSpace);
    }
  }

  internal string IccColorToString(PdfColorSpace colorSpace, bool stroke)
  {
    if (this.IsEmpty)
      return string.Empty;
    switch (colorSpace)
    {
      case PdfColorSpace.RGB:
        return this.IccRGBToString(stroke);
      case PdfColorSpace.CMYK:
        return this.CalCMYKToString(stroke);
      case PdfColorSpace.GrayScale:
        return this.IccGrayscaleToString(stroke);
      default:
        throw new ArgumentException("Unsupported colour space: " + (object) colorSpace);
    }
  }

  internal void WriteToStringBuilder(StringBuilder sb, PdfColorSpace colorSpace, bool stroke)
  {
    if (sb == null)
      throw new ArgumentNullException(nameof (sb));
    if (this.IsEmpty)
    {
      sb.Append(string.Empty);
    }
    else
    {
      switch (colorSpace)
      {
        case PdfColorSpace.RGB:
          this.RGBToStringBuilder(sb, stroke);
          break;
        case PdfColorSpace.CMYK:
          this.CMYKToStringBuilder(sb, stroke);
          break;
        case PdfColorSpace.GrayScale:
          this.GrayscaleToStringBuilder(sb, stroke);
          break;
      }
    }
  }

  private void AssignCMYK(byte r, byte g, byte b)
  {
    float num1 = (float) r / (float) byte.MaxValue;
    float num2 = (float) g / (float) byte.MaxValue;
    float num3 = (float) b / (float) byte.MaxValue;
    float num4 = PdfNumber.Min(1f - num1, 1f - num2, 1f - num3);
    float num5 = (double) num4 == 1.0 ? 0.0f : (float) ((1.0 - (double) num1 - (double) num4) / (1.0 - (double) num4));
    float num6 = (double) num4 == 1.0 ? 0.0f : (float) ((1.0 - (double) num2 - (double) num4) / (1.0 - (double) num4));
    float num7 = (double) num4 == 1.0 ? 0.0f : (float) ((1.0 - (double) num3 - (double) num4) / (1.0 - (double) num4));
    this.m_black = num4;
    this.m_cyan = num5;
    this.m_magenta = num6;
    this.m_yellow = num7;
  }

  private void AssignRGB(float cyan, float magenta, float yellow, float black)
  {
    float num = black * (float) byte.MaxValue;
    float val2_1 = cyan * ((float) byte.MaxValue - num) + num;
    float val2_2 = magenta * ((float) byte.MaxValue - num) + num;
    float val2_3 = yellow * ((float) byte.MaxValue - num) + num;
    this.m_red = (byte) ((double) byte.MaxValue - (double) Math.Min((float) byte.MaxValue, val2_1));
    this.m_green = (byte) ((double) byte.MaxValue - (double) Math.Min((float) byte.MaxValue, val2_2));
    this.m_blue = (byte) ((double) byte.MaxValue - (double) Math.Min((float) byte.MaxValue, val2_3));
  }

  private static bool CompareColours(Color color1, Color color2)
  {
    return true & (int) color1.A == (int) color2.A & (int) color1.R == (int) color2.R & (int) color1.G == (int) color2.G & (int) color1.B == (int) color2.B;
  }

  internal PdfArray ToArray() => this.ToArray(PdfColorSpace.RGB);

  internal PdfArray ToArray(PdfColorSpace colorSpace)
  {
    PdfArray array = new PdfArray();
    switch (colorSpace)
    {
      case PdfColorSpace.RGB:
        array.Add((IPdfPrimitive) new PdfNumber(this.Red));
        array.Add((IPdfPrimitive) new PdfNumber(this.Green));
        array.Add((IPdfPrimitive) new PdfNumber(this.Blue));
        break;
      case PdfColorSpace.CMYK:
        array.Add((IPdfPrimitive) new PdfNumber(this.C));
        array.Add((IPdfPrimitive) new PdfNumber(this.M));
        array.Add((IPdfPrimitive) new PdfNumber(this.Y));
        array.Add((IPdfPrimitive) new PdfNumber(this.K));
        break;
      case PdfColorSpace.GrayScale:
        array.Add((IPdfPrimitive) new PdfNumber(this.Gray));
        break;
      default:
        throw new NotSupportedException("Unsupported colour space.");
    }
    return array;
  }

  internal static void Clear()
  {
    lock (PdfColor.s_rgbStrings)
      PdfColor.s_rgbStrings.Clear();
    lock (PdfColor.s_grayStringsSroke)
    {
      PdfColor.s_grayStringsSroke.Clear();
      PdfColor.s_grayStringsFill.Clear();
    }
  }
}
