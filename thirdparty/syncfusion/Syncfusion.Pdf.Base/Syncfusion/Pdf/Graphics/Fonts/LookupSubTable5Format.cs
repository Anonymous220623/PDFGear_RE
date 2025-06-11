// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.LookupSubTable5Format
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class LookupSubTable5Format : BaseTable
{
  private IDictionary<int, IList<SubsetTable>> m_records;
  private ICollection<int> m_glyphIds;
  private IList<IList<SubsetTable>> m_subsetTables;
  private SubsetTable m_subSetTable;
  private CDEFTable m_cdefTable;
  private LookupSubTableFormat m_format;

  internal CDEFTable CDEFTable
  {
    get => this.m_cdefTable;
    set => this.m_cdefTable = value;
  }

  internal IList<IList<SubsetTable>> SubsetTables
  {
    get => this.m_subsetTables;
    set => this.m_subsetTables = value;
  }

  internal LookupSubTable5Format(
    OtfTable table,
    int flag,
    IDictionary<int, IList<SubsetTable>> records,
    LookupSubTableFormat format)
    : base(table, flag)
  {
    this.m_records = records;
    this.m_format = format;
  }

  internal LookupSubTable5Format(
    OtfTable table,
    int flag,
    ICollection<int> glyphIds,
    CDEFTable ctable,
    LookupSubTableFormat format)
    : base(table, flag)
  {
    this.m_glyphIds = glyphIds;
    this.m_cdefTable = ctable;
    this.m_format = format;
  }

  internal LookupSubTable5Format(
    OtfTable table,
    int flag,
    SubsetTableFormat subsetTable,
    LookupSubTableFormat format)
    : base(table, flag)
  {
    this.m_subSetTable = (SubsetTable) subsetTable;
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
        return this.m_glyphIds.Contains(index) && !this.OTFontTable.GDEFTable.IsSkip(index, this.LookupID) ? this.m_subsetTables[this.m_cdefTable.GetValue(index)] : (IList<SubsetTable>) new List<SubsetTable>();
      case LookupSubTableFormat.Format3:
        if (!((SubsetTableFormat) this.m_subSetTable).Coverages[0].Contains(index) || this.OTFontTable.GDEFTable.IsSkip(index, this.LookupID))
          return (IList<SubsetTable>) new List<SubsetTable>();
        return (IList<SubsetTable>) new List<SubsetTable>()
        {
          this.m_subSetTable
        };
      default:
        return (IList<SubsetTable>) new List<SubsetTable>();
    }
  }
}
