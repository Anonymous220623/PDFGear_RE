// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.FontSource
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal abstract class FontSource
{
  public abstract string FontFamily { get; }

  public abstract bool IsBold { get; }

  public abstract bool IsItalic { get; }

  public abstract short Ascender { get; }

  public abstract short Descender { get; }

  public virtual void GetAdvancedWidth(Glyph glyph)
  {
  }

  public virtual void GetGlyphOutlines(Glyph glyph, double fontSize)
  {
  }

  public virtual void GetGlyphName(Glyph glyph)
  {
  }
}
