// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontType1GlyphData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontType1GlyphData
{
  public SystemFontGlyphOutlinesCollection Oultlines { get; private set; }

  public ushort AdvancedWidth { get; private set; }

  public bool HasWidth { get; private set; }

  public SystemFontType1GlyphData(SystemFontGlyphOutlinesCollection outlines, ushort? width)
  {
    this.Oultlines = outlines;
    if (!width.HasValue)
      return;
    this.AdvancedWidth = width.Value;
    this.HasWidth = true;
  }
}
