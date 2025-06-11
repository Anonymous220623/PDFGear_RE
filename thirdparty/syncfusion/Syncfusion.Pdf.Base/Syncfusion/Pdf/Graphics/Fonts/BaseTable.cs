// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.BaseTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal abstract class BaseTable
{
  private OtfTable m_otfontTable;
  private int m_lookupID;

  internal OtfTable OTFontTable => this.m_otfontTable;

  internal int LookupID => this.m_lookupID;

  internal BaseTable(OtfTable openReader, int lookupFlag)
  {
    this.m_otfontTable = openReader;
    this.m_lookupID = lookupFlag;
  }

  internal virtual SubsetTable GetTable(OtfGlyphInfoList glyphList)
  {
    if (glyphList.Index >= glyphList.End)
      return (SubsetTable) null;
    foreach (SubsetTable subsetTable in (IEnumerable<SubsetTable>) this.GetSubsetTables(glyphList.Glyphs[glyphList.Index].Index))
    {
      int num = this.Match(glyphList, subsetTable);
      if (num != -1)
      {
        glyphList.Start = glyphList.Index;
        glyphList.End = num + 1;
        return subsetTable;
      }
    }
    return (SubsetTable) null;
  }

  internal virtual int Match(OtfGlyphInfoList glyphList, SubsetTable subsetTable)
  {
    GlyphInfoIndex glyphInfoIndex = new GlyphInfoIndex();
    glyphInfoIndex.GlyphInfoList = glyphList;
    glyphInfoIndex.Index = glyphList.Index;
    int index;
    for (index = 1; index < subsetTable.Length; ++index)
    {
      glyphInfoIndex.MoveNext(this.OTFontTable, this.LookupID);
      if (glyphInfoIndex.GlyphInfo == null || !subsetTable.Match(glyphInfoIndex.GlyphInfo.Index, index))
        break;
    }
    return index == subsetTable.Length ? glyphInfoIndex.Index : -1;
  }

  internal abstract IList<SubsetTable> GetSubsetTables(int index);
}
