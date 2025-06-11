// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSubstLookupRecord
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSubstLookupRecord(SystemFontOpenTypeFontSourceBase fontFile) : 
  SystemFontTableBase(fontFile)
{
  private ushort lookupIndex;
  private SystemFontLookup lookup;

  public ushort SequenceIndex { get; private set; }

  public SystemFontLookup Lookup
  {
    get
    {
      if (this.lookup == null)
        this.lookup = this.FontSource.GetLookup(this.lookupIndex);
      return this.lookup;
    }
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    this.SequenceIndex = reader.ReadUShort();
    this.lookupIndex = reader.ReadUShort();
  }
}
