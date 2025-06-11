// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.PdfDefaultAppearance
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

public struct PdfDefaultAppearance
{
  private static readonly PdfDefaultAppearance _default = new PdfDefaultAppearance("Arial", 9f, new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, 0, 0), new FS_COLOR((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue));

  public PdfDefaultAppearance(
    string fontFamily,
    float fontSize,
    FS_COLOR strokeColor,
    FS_COLOR fillColor)
  {
    this.FontFamily = fontFamily;
    this.FontSize = fontSize;
    this.StrokeColor = strokeColor;
    this.FillColor = fillColor;
  }

  public PdfDefaultAppearance(string defaultAppearance)
    : this(defaultAppearance, 1f)
  {
  }

  public PdfDefaultAppearance(string defaultAppearance, float opacity)
  {
    if (!string.IsNullOrEmpty(defaultAppearance))
    {
      string fontName;
      float fontSize;
      FS_COLOR strokeColor;
      FS_COLOR fillColor;
      PdfDefaultAppearance.ParseDefaultAppearance(defaultAppearance, opacity, out fontName, out fontSize, out strokeColor, out fillColor, out FS_MATRIX _);
      this.FontFamily = fontName;
      this.FontSize = fontSize;
      this.StrokeColor = strokeColor;
      this.FillColor = fillColor;
    }
    else
    {
      this.FontFamily = PdfDefaultAppearance.Default.FontFamily;
      this.FontSize = PdfDefaultAppearance.Default.FontSize;
      this.StrokeColor = PdfDefaultAppearance.Default.StrokeColor;
      this.FillColor = PdfDefaultAppearance.Default.FillColor;
    }
  }

  public static PdfDefaultAppearance Default => PdfDefaultAppearance._default;

  public string FontFamily { get; set; }

  public float FontSize { get; set; }

  public FS_COLOR StrokeColor { get; set; }

  public FS_COLOR FillColor { get; set; }

  public override string ToString()
  {
    return FormattableStringFactory.Create("/{0} {1} Tf {2} {3} {4} RG {5} {6} {7} rg", string.IsNullOrEmpty(this.FontFamily) ? (object) "Arial" : (object) this.FontFamily, (object) this.FontSize, (object) (float) ((double) this.FillColor.R / (double) byte.MaxValue), (object) (float) ((double) this.FillColor.G / (double) byte.MaxValue), (object) (float) ((double) this.FillColor.B / (double) byte.MaxValue), (object) (float) ((double) this.StrokeColor.R / (double) byte.MaxValue), (object) (float) ((double) this.StrokeColor.G / (double) byte.MaxValue), (object) (float) ((double) this.StrokeColor.B / (double) byte.MaxValue)).ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  public PdfRichTextStyle ToRichTextStyle()
  {
    return PdfRichTextStyle.Default with
    {
      FontFamily = this.FontFamily,
      FontSize = new float?(this.FontSize)
    };
  }

  public static bool TryParse(string defaultAppearance, out PdfDefaultAppearance pdfFontStyle)
  {
    return PdfDefaultAppearance.TryParse(defaultAppearance, 1f, out pdfFontStyle);
  }

  public static bool TryParse(
    string defaultAppearance,
    float opacity,
    out PdfDefaultAppearance pdfFontStyle)
  {
    pdfFontStyle = PdfDefaultAppearance.Default;
    string fontName;
    float fontSize;
    FS_COLOR strokeColor;
    FS_COLOR fillColor;
    bool defaultAppearance1 = PdfDefaultAppearance.ParseDefaultAppearance(defaultAppearance, opacity, out fontName, out fontSize, out strokeColor, out fillColor, out FS_MATRIX _);
    pdfFontStyle.FontFamily = fontName;
    pdfFontStyle.FontSize = fontSize;
    pdfFontStyle.StrokeColor = strokeColor;
    pdfFontStyle.FillColor = fillColor;
    return defaultAppearance1;
  }

  public static PdfDefaultAppearance Parse(string defaultAppearance)
  {
    return PdfDefaultAppearance.Parse(defaultAppearance);
  }

  public static PdfDefaultAppearance Parse(string defaultAppearance, float opacity)
  {
    PdfDefaultAppearance pdfFontStyle;
    if (PdfDefaultAppearance.TryParse(defaultAppearance, opacity, out pdfFontStyle))
      return pdfFontStyle;
    throw new ArgumentException(nameof (defaultAppearance));
  }

  private static bool ParseDefaultAppearance(
    string da,
    float opacity,
    out string fontName,
    out float fontSize,
    out FS_COLOR strokeColor,
    out FS_COLOR fillColor,
    out FS_MATRIX matrix)
  {
    float[] strokeColor1;
    float[] fillColor1;
    bool defaultAppearance = Pdfium.FPDFTOOLS_ParseDefaultAppearance(da, out strokeColor1, out fillColor1, out fontName, out fontSize, out matrix);
    if (fillColor1 == null)
    {
      strokeColor = new FS_COLOR(0);
    }
    else
    {
      FS_COLOR fsColor = new FS_COLOR(fillColor1);
      strokeColor = new FS_COLOR((int) ((double) opacity * (double) byte.MaxValue), fsColor.R, fsColor.G, fsColor.B);
    }
    if (fillColor1 == null && strokeColor1 == null)
      fillColor = new FS_COLOR((int) byte.MaxValue, 0, 0, 0);
    else if (strokeColor1 == null)
    {
      fillColor = new FS_COLOR(0);
    }
    else
    {
      FS_COLOR fsColor = new FS_COLOR(strokeColor1);
      fillColor = new FS_COLOR((int) ((double) opacity * (double) byte.MaxValue), fsColor.R, fsColor.G, fsColor.B);
    }
    if ((fontName ?? "") == "")
    {
      fontName = "Arial";
      fontSize = 12f;
    }
    return defaultAppearance;
  }
}
