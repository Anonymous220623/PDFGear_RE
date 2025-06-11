// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.SubsetTableFormat
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class SubsetTableFormat : SubsetTable
{
  private int[] m_glyphs;
  private int[] m_btGlyphs;
  private int[] m_lookupGlyphs;
  private CDEFTable m_cdefTable;
  private LookupSubTableRecord[] m_records;
  private IList<ICollection<int>> m_coverages;
  private IList<ICollection<int>> m_btCoverages;
  private IList<ICollection<int>> m_lookupCoverages;
  private LookupSubTable6Format subTable;
  private bool m_isLookup;
  private bool m_isBackTrack;
  private bool m_isConverage;
  private bool m_cdefMatch;
  private bool m_isFormat2;
  private bool m_isFormat3;

  internal IList<ICollection<int>> Coverages
  {
    get => this.m_coverages;
    set => this.m_coverages = value;
  }

  internal SubsetTableFormat(int[] glyphs, LookupSubTableRecord[] records)
  {
    this.m_glyphs = glyphs;
    this.m_records = records;
  }

  internal SubsetTableFormat(
    int[] btGlyphs,
    int[] glyphs,
    int[] lookupGlyphs,
    LookupSubTableRecord[] records)
  {
    this.m_btGlyphs = btGlyphs;
    this.m_glyphs = glyphs;
    this.m_lookupGlyphs = lookupGlyphs;
    this.m_records = records;
    this.m_isBackTrack = true;
    this.m_isLookup = true;
  }

  internal SubsetTableFormat(
    LookupSubTable5Format subtable5,
    int[] glyphs,
    LookupSubTableRecord[] records)
  {
    this.m_glyphs = glyphs;
    this.m_records = records;
    this.m_cdefTable = subtable5.CDEFTable;
    this.m_cdefMatch = true;
  }

  internal SubsetTableFormat(IList<ICollection<int>> coverages, LookupSubTableRecord[] records)
  {
    this.m_coverages = coverages;
    this.m_records = records;
    this.m_isConverage = true;
  }

  internal SubsetTableFormat(
    LookupSubTable6Format subTable,
    int[] backtrackClassIds,
    int[] inputClassIds,
    int[] lookAheadClassIds,
    LookupSubTableRecord[] substLookupRecords)
  {
    this.subTable = subTable;
    this.m_btGlyphs = backtrackClassIds;
    this.m_glyphs = inputClassIds;
    this.m_lookupGlyphs = lookAheadClassIds;
    this.m_records = substLookupRecords;
    this.m_isFormat2 = true;
  }

  internal SubsetTableFormat(
    IList<ICollection<int>> bkCoverages,
    IList<ICollection<int>> coverages,
    IList<ICollection<int>> lookupCoverages,
    LookupSubTableRecord[] records)
  {
    this.m_btCoverages = bkCoverages;
    this.m_coverages = coverages;
    this.m_lookupCoverages = lookupCoverages;
    this.m_records = records;
    this.m_isFormat3 = true;
  }

  internal override int Length
  {
    get
    {
      return this.m_isConverage || this.m_isFormat3 ? this.m_coverages.Count : this.m_glyphs.Length + 1;
    }
  }

  internal override int LookupLength
  {
    get => this.m_isFormat3 ? this.m_lookupCoverages.Count : this.m_lookupGlyphs.Length;
  }

  internal override int BTCLength
  {
    get => this.m_isFormat3 ? this.m_btCoverages.Count : this.m_btGlyphs.Length;
  }

  internal override LookupSubTableRecord[] LookupRecord => this.m_records;

  internal override bool Match(int id, int index)
  {
    if (this.m_cdefMatch)
      return this.m_cdefTable.GetValue(id) == this.m_glyphs[index - 1];
    if (this.m_isConverage || this.m_isFormat3)
      return this.m_coverages[index].Contains(id);
    return this.m_isFormat2 ? this.subTable.CDEFTable.GetValue(id) == this.m_glyphs[index - 1] : id == this.m_glyphs[index - 1];
  }

  internal override bool IsLookup(int glyphId, int index)
  {
    if (this.m_isLookup)
      return glyphId == this.m_lookupGlyphs[index];
    if (this.m_isFormat2)
      return this.subTable.LookupCDEFTable.GetValue(glyphId) == this.m_lookupGlyphs[index];
    return this.m_isFormat3 && this.m_lookupCoverages[index].Contains(glyphId);
  }

  internal override bool IsBackTrack(int glyphId, int index)
  {
    if (this.m_isBackTrack)
      return glyphId == this.m_btGlyphs[index];
    if (this.m_isFormat2)
      return this.subTable.BtCDEFTable.GetValue(glyphId) == this.m_btGlyphs[index];
    return this.m_isFormat3 && this.m_btCoverages[index].Contains(glyphId);
  }
}
