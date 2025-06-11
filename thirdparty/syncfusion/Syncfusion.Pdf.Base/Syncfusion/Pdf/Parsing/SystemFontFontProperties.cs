// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFontProperties
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFontProperties
{
  private readonly FontFamily fontFamily;
  private readonly FontStyle fontStyle;
  private readonly bool isMonoSpaced;

  public string FontFamilyName => this.FontFamily.Name;

  public FontStyle FontStyle => this.fontStyle;

  public FontFamily FontFamily => this.fontFamily;

  public bool IsMonoSpaced => this.isMonoSpaced;

  public SystemFontFontProperties(FontFamily fontFamily, FontStyle fontStyle)
  {
    this.fontFamily = fontFamily;
    this.fontStyle = fontStyle;
    this.isMonoSpaced = SystemFontsManager.IsMonospaced(fontFamily.Name);
  }

  public SystemFontFontProperties(FontFamily fontFamily)
    : this(fontFamily, FontStyle.Regular)
  {
  }

  public override bool Equals(object obj)
  {
    return obj is SystemFontFontProperties fontFontProperties && this.FontFamilyName == fontFontProperties.FontFamilyName && this.FontStyle == fontFontProperties.FontStyle;
  }

  public override int GetHashCode()
  {
    return (17 * 23 + this.fontFamily.GetHashCode()) * 23 + this.fontStyle.GetHashCode();
  }
}
