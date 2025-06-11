// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupTable2
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupTable2 : LookupTable
{
  private IList<LookupTable> m_records = (IList<LookupTable>) new List<LookupTable>();

  internal LookupTable2(OtfTable table, int flag, int[] offsets)
    : base(table, flag, offsets)
  {
    this.ReadSubTables();
  }

  internal override bool ReplaceGlyph(OtfGlyphInfoList glyphList)
  {
    if (glyphList.Index >= glyphList.End)
      return false;
    if (this.OpenTypeFontTable.GDEFTable.IsSkip(glyphList.Glyphs[glyphList.Index].Index, this.Flag))
    {
      ++glyphList.Index;
      return false;
    }
    foreach (LookupTable record in (IEnumerable<LookupTable>) this.m_records)
    {
      if (record.ReplaceGlyph(glyphList))
        return true;
    }
    ++glyphList.Index;
    return false;
  }

  internal override void ReadSubTable(int offset)
  {
    this.OpenTypeFontTable.Reader.Seek((long) offset);
    switch (this.OpenTypeFontTable.Reader.ReadInt16())
    {
      case 1:
        this.m_records.Add((LookupTable) new GPOSTableFormat(this.OpenTypeFontTable, this.Flag, offset, GPOSTableType.Format1));
        break;
      case 2:
        this.m_records.Add((LookupTable) new GPOSTableFormat(this.OpenTypeFontTable, this.Flag, offset, GPOSTableType.Format2));
        break;
    }
  }
}
