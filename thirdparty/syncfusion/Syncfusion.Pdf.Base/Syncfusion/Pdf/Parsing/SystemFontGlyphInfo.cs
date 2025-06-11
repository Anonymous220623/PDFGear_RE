// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontGlyphInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontGlyphInfo
{
  public ushort GlyphId { get; private set; }

  public SystemFontGlyphForm Form { get; private set; }

  public SystemFontGlyphInfo(ushort glyphId)
  {
    this.GlyphId = glyphId;
    this.Form = SystemFontGlyphForm.Undefined;
  }

  public SystemFontGlyphInfo(ushort glyphID, SystemFontGlyphForm form)
  {
    this.GlyphId = glyphID;
    this.Form = form;
  }

  public override string ToString() => $"{this.GlyphId}({this.Form});";
}
