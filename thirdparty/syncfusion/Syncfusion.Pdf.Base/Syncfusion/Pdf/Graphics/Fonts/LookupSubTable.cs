// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupSubTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal abstract class LookupSubTable : BaseTable
{
  internal LookupSubTable(OtfTable table, int flag)
    : base(table, flag)
  {
  }

  internal override SubsetTable GetTable(OtfGlyphInfoList glyphList)
  {
    if (glyphList.Index >= glyphList.End)
      return (SubsetTable) null;
    foreach (SubsetTable subsetTable in (IEnumerable<SubsetTable>) this.GetSubsetTables(glyphList.Glyphs[glyphList.Index].Index))
    {
      int index = this.Match(glyphList, subsetTable);
      if (index != -1 && this.Lookup(glyphList, subsetTable, index) && this.BackTrack(glyphList, subsetTable))
      {
        glyphList.Start = glyphList.Index;
        glyphList.End = index + 1;
        return subsetTable;
      }
    }
    return (SubsetTable) null;
  }

  internal bool Lookup(OtfGlyphInfoList glyphList, SubsetTable subsetTable, int index)
  {
    GlyphInfoIndex glyphInfoIndex = new GlyphInfoIndex();
    glyphInfoIndex.GlyphInfoList = glyphList;
    glyphInfoIndex.Index = index;
    int index1;
    for (index1 = 0; index1 < subsetTable.LookupLength; ++index1)
    {
      glyphInfoIndex.MoveNext(this.OTFontTable, this.LookupID);
      if (glyphInfoIndex.GlyphInfo == null || !subsetTable.IsLookup(glyphInfoIndex.GlyphInfo.Index, index1))
        break;
    }
    return index1 == subsetTable.LookupLength;
  }

  internal bool BackTrack(OtfGlyphInfoList glyphList, SubsetTable subsetTable)
  {
    GlyphInfoIndex glyphInfoIndex = new GlyphInfoIndex();
    glyphInfoIndex.GlyphInfoList = glyphList;
    glyphInfoIndex.Index = glyphList.Index;
    int index;
    for (index = 0; index < subsetTable.BTCLength; ++index)
    {
      glyphInfoIndex.MovePrevious(this.OTFontTable, this.LookupID);
      if (glyphInfoIndex.GlyphInfo == null || !subsetTable.IsBackTrack(glyphInfoIndex.GlyphInfo.Index, index))
        break;
    }
    return index == subsetTable.BTCLength;
  }
}
