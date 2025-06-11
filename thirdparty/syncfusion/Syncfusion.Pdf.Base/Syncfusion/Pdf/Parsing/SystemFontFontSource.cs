// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontFontSource
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontFontSource
{
  public abstract string FontFamily { get; }

  public abstract bool IsBold { get; }

  public abstract bool IsItalic { get; }

  public abstract short Ascender { get; }

  public abstract short Descender { get; }

  internal static SystemFontFontType GetFontType(SystemFontOpenTypeFontReader reader)
  {
    SystemFontFontType fontType = SystemFontFontType.Unknown;
    reader.BeginReadingBlock();
    uint num1 = reader.ReadULong();
    reader.EndReadingBlock();
    reader.BeginReadingBlock();
    double num2 = (double) reader.ReadFixed();
    reader.EndReadingBlock();
    if ((int) num1 == (int) SystemFontTags.TRUE_TYPE_COLLECTION)
      fontType = SystemFontFontType.TrueTypeCollection;
    else if (num2 == 1.0)
      fontType = SystemFontFontType.TrueType;
    return fontType;
  }

  public virtual void GetAdvancedWidth(SystemFontGlyph glyph)
  {
  }

  public virtual void GetGlyphOutlines(SystemFontGlyph glyph, double fontSize)
  {
  }
}
