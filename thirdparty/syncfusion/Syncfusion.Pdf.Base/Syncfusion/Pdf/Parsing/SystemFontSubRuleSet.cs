// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontSubRuleSet
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontSubRuleSet(SystemFontOpenTypeFontSourceBase fontFile) : SystemFontTableBase(fontFile)
{
  private ushort[] subRuleOffsets;
  private SystemFontSubRule[] subRules;

  public SystemFontSubRule[] SubRules
  {
    get
    {
      if (this.subRules == null)
      {
        this.subRules = new SystemFontSubRule[this.subRuleOffsets.Length];
        for (int index = 0; index < this.subRules.Length; ++index)
          this.subRules[index] = this.ReadSubRule(this.Reader, this.subRuleOffsets[index]);
      }
      return this.subRules;
    }
  }

  private SystemFontSubRule ReadSubRule(SystemFontOpenTypeFontReader reader, ushort offset)
  {
    reader.BeginReadingBlock();
    long offset1 = this.Offset + (long) offset;
    reader.Seek(offset1, SeekOrigin.Begin);
    SystemFontSubRule systemFontSubRule = new SystemFontSubRule(this.FontSource);
    systemFontSubRule.Read(reader);
    systemFontSubRule.Offset = offset1;
    reader.EndReadingBlock();
    return systemFontSubRule;
  }

  public override void Read(SystemFontOpenTypeFontReader reader)
  {
    ushort length = reader.ReadUShort();
    this.subRuleOffsets = new ushort[(int) length];
    for (int index = 0; index < (int) length; ++index)
      this.subRuleOffsets[index] = reader.ReadUShort();
  }
}
