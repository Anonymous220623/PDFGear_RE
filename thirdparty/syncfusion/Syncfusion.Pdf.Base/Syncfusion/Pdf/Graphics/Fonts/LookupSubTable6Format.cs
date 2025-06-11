// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupSubTable6Format
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupSubTable6Format : LookupSubTable
{
  private IDictionary<int, IList<SubsetTable>> m_records;
  private LookupSubTableFormat m_format;
  private ICollection<int> m_glyphs;
  private IList<IList<SubsetTable>> m_subSetTables;
  private CDEFTable m_btCdefTable;
  private CDEFTable m_cdefTable;
  private CDEFTable m_lookupCdefTable;
  private SubsetTable m_subsetTable;

  internal IList<IList<SubsetTable>> SubSetTables
  {
    get => this.m_subSetTables;
    set => this.m_subSetTables = value;
  }

  internal CDEFTable CDEFTable
  {
    get => this.m_cdefTable;
    set => this.m_cdefTable = value;
  }

  internal CDEFTable BtCDEFTable
  {
    get => this.m_btCdefTable;
    set => this.m_btCdefTable = value;
  }

  internal CDEFTable LookupCDEFTable
  {
    get => this.m_lookupCdefTable;
    set => this.m_lookupCdefTable = value;
  }

  internal LookupSubTable6Format(
    OtfTable table,
    int flag,
    IDictionary<int, IList<SubsetTable>> substMap,
    LookupSubTableFormat format)
    : base(table, flag)
  {
    this.m_records = substMap;
    this.m_format = format;
  }

  internal LookupSubTable6Format(
    OtfTable table,
    int flag,
    ICollection<int> glyphs,
    CDEFTable btcdef,
    CDEFTable cdef,
    CDEFTable lookupcdef,
    LookupSubTableFormat format)
    : base(table, flag)
  {
    this.m_glyphs = glyphs;
    this.m_btCdefTable = btcdef;
    this.m_cdefTable = cdef;
    this.m_lookupCdefTable = lookupcdef;
    this.m_format = format;
  }

  internal LookupSubTable6Format(
    OtfTable table,
    int flag,
    SubsetTableFormat subsetFormat,
    LookupSubTableFormat format)
    : base(table, flag)
  {
    this.m_subsetTable = (SubsetTable) subsetFormat;
    this.m_format = format;
  }

  internal override IList<SubsetTable> GetSubsetTables(int index)
  {
    switch (this.m_format)
    {
      case LookupSubTableFormat.Format1:
        if (!this.m_records.ContainsKey(index) || this.OTFontTable.GDEFTable.IsSkip(index, this.LookupID))
          return (IList<SubsetTable>) new List<SubsetTable>();
        IList<SubsetTable> subsetTables;
        this.m_records.TryGetValue(index, out subsetTables);
        return subsetTables;
      case LookupSubTableFormat.Format2:
        return this.m_glyphs.Contains(index) && !this.OTFontTable.GDEFTable.IsSkip(index, this.LookupID) ? this.m_subSetTables[this.m_cdefTable.GetValue(index)] : (IList<SubsetTable>) new List<SubsetTable>();
      case LookupSubTableFormat.Format3:
        if (!((SubsetTableFormat) this.m_subsetTable).Coverages[0].Contains(index) || this.OTFontTable.GDEFTable.IsSkip(index, this.LookupID))
          return (IList<SubsetTable>) new List<SubsetTable>();
        return (IList<SubsetTable>) new List<SubsetTable>()
        {
          this.m_subsetTable
        };
      default:
        return (IList<SubsetTable>) new List<SubsetTable>();
    }
  }
}
