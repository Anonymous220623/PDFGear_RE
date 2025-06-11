// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFontDescriptor
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontFontDescriptor
{
  private readonly string fontFamily;
  private readonly FontStyle fontStyle;

  public string FontFamily => this.fontFamily;

  public FontStyle FontStyle => this.fontStyle;

  public SystemFontFontDescriptor(string fontFamily)
    : this(fontFamily, FontStyle.Regular)
  {
  }

  public SystemFontFontDescriptor(string fontFamily, FontStyle fontStyle)
  {
    this.fontFamily = fontFamily;
    this.fontStyle = fontStyle;
  }

  public static bool operator ==(SystemFontFontDescriptor left, SystemFontFontDescriptor right)
  {
    return object.ReferenceEquals((object) left, (object) null) ? object.ReferenceEquals((object) right, (object) null) : left.Equals((object) right);
  }

  public static bool operator !=(SystemFontFontDescriptor left, SystemFontFontDescriptor right)
  {
    return !(left == right);
  }

  private static FontStyle GetFontStyle(string styles, FontStyle baseStyle)
  {
    styles = styles.ToLower();
    return styles.Contains("italic") ? FontStyle.Italic : baseStyle;
  }

  public override int GetHashCode()
  {
    return (17 * 23 + (this.FontFamily != null ? this.FontFamily.GetHashCode() : 0)) * 23 + this.FontStyle.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    SystemFontFontDescriptor fontFontDescriptor = obj as SystemFontFontDescriptor;
    return !(fontFontDescriptor == (SystemFontFontDescriptor) null) && this.FontFamily == fontFontDescriptor.FontFamily && this.FontStyle == fontFontDescriptor.FontStyle;
  }

  public override string ToString() => this.FontFamily;
}
