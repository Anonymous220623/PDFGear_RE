// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontMaxProfile
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontMaxProfile(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontTrueTypeTableBase(fontFile)
{
  internal override uint Tag => SystemFontTags.MAXP_TABLE;

  public float Version { get; private set; }

  public ushort NumGlyphs { get; private set; }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    this.Version = reader.ReadFixed();
    this.NumGlyphs = reader.ReadUShort();
  }
}
