// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartExDataCache
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartExDataCache
{
  private string m_categoryFormula;
  private string m_seriesFormula;
  private object[] m_seriesValues;
  private object[] m_categoryValues;
  private bool m_isRowWiseCategory;
  private bool m_isRowWiseSeries;
  private string m_seriesFormatCode;
  private string m_categoryFormatCode;

  internal string CategoryFormula
  {
    get => this.m_categoryFormula;
    set => this.m_categoryFormula = value;
  }

  internal string SeriesFormula
  {
    get => this.m_seriesFormula;
    set => this.m_seriesFormula = value;
  }

  internal object[] SeriesValues
  {
    get => this.m_seriesValues;
    set => this.m_seriesValues = value;
  }

  internal object[] CategoryValues
  {
    get => this.m_categoryValues;
    set => this.m_categoryValues = value;
  }

  internal bool IsRowWiseCategory
  {
    get => this.m_isRowWiseCategory;
    set => this.m_isRowWiseCategory = value;
  }

  internal bool IsRowWiseSeries
  {
    get => this.m_isRowWiseSeries;
    set => this.m_isRowWiseSeries = value;
  }

  internal string SeriesFormatCode
  {
    get => this.m_seriesFormatCode;
    set => this.m_seriesFormatCode = value;
  }

  internal string CategoriesFormatCode
  {
    get => this.m_categoryFormatCode;
    set => this.m_categoryFormatCode = value;
  }

  internal void CopyProperties(ChartSerieImpl serie, WorkbookImpl workbook)
  {
    if (this.m_categoryFormatCode != null)
      serie.CategoriesFormatCode = this.m_categoryFormatCode;
    if (this.m_seriesFormatCode != null)
      serie.FormatCode = this.m_seriesFormatCode;
    if (this.m_categoryValues != null)
      serie.EnteredDirectlyCategoryLabels = this.m_categoryValues;
    if (this.m_seriesValues != null)
      serie.EnteredDirectlyValues = this.m_seriesValues;
    if (this.m_categoryFormula != null && this.m_categoryFormula != "")
    {
      IRange range = ChartParser.GetRange(workbook, this.m_categoryFormula);
      if (range is IName name && name.RefersToRange != null)
        serie.CategoryLabels = (IOfficeDataRange) name.RefersToRange;
      else
        serie.CategoryLabelsIRange = range;
    }
    if (this.m_seriesFormula != null && this.m_seriesFormula != null)
    {
      IRange range = ChartParser.GetRange(workbook, this.m_seriesFormula);
      serie.ValuesIRange = !(range is IName name) || name.RefersToRange == null ? range : name.RefersToRange;
    }
    serie.IsRowWiseCategory = this.m_isRowWiseCategory;
    serie.IsRowWiseSeries = this.m_isRowWiseSeries;
  }
}
