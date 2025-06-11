// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.OpenTypeFontSourceFromMetaFile
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class OpenTypeFontSourceFromMetaFile : SystemFontOpenTypeFontSourceBase
{
  private SystemFontCMap cmap;
  private SystemFontHead head;
  private SystemFontHorizontalHeader hhea;
  private SystemFontHorizontalMetrics hmtx;
  private SystemFontKerning kern;
  private SystemFontGlyphSubstitution gsub;
  private ushort glyphsCount;
  private string fontFamily;
  private bool isBold;
  private bool isItalic;

  internal override SystemFontOutlines Outlines => SystemFontOutlines.TrueType;

  internal override SystemFontCFFFontSource CFF => (SystemFontCFFFontSource) null;

  internal override SystemFontCMap CMap => this.cmap;

  internal override SystemFontHead Head => this.head;

  internal override SystemFontHorizontalHeader HHea => this.hhea;

  internal override SystemFontHorizontalMetrics HMtx => this.hmtx;

  internal override SystemFontKerning Kern => this.kern;

  internal override SystemFontGlyphSubstitution GSub => this.gsub;

  internal override ushort GlyphCount => this.glyphsCount;

  public override string FontFamily => this.fontFamily;

  public override bool IsBold => this.isBold;

  public override bool IsItalic => this.isItalic;

  public OpenTypeFontSourceFromMetaFile(SystemFontOpenTypeFontReader reader)
    : base(reader)
  {
    this.Initialize();
  }

  private void Initialize()
  {
    this.fontFamily = this.Reader.ReadString();
    ushort n = this.Reader.ReadUShort();
    this.isBold = SystemFontBitsHelper.GetBit((int) n, (byte) 0);
    this.isItalic = SystemFontBitsHelper.GetBit((int) n, (byte) 1);
    this.glyphsCount = this.Reader.ReadUShort();
    this.head = new SystemFontHead((SystemFontOpenTypeFontSourceBase) this);
    this.head.Import(this.Reader);
    this.cmap = new SystemFontCMap((SystemFontOpenTypeFontSourceBase) this);
    this.cmap.Import(this.Reader);
    this.hhea = new SystemFontHorizontalHeader((SystemFontOpenTypeFontSourceBase) this);
    this.hhea.Import(this.Reader);
    this.hmtx = new SystemFontHorizontalMetrics((SystemFontOpenTypeFontSourceBase) this);
    this.hmtx.Import(this.Reader);
    this.kern = new SystemFontKerning((SystemFontOpenTypeFontSourceBase) this);
    this.kern.Import(this.Reader);
    this.gsub = new SystemFontGlyphSubstitution((SystemFontOpenTypeFontSourceBase) this);
    this.gsub.Import(this.Reader);
  }

  internal override SystemFontGlyphData GetGlyphData(ushort glyphID)
  {
    throw new NotImplementedException();
  }
}
