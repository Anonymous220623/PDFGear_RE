// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartDataRange
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartDataRange : IOfficeDataRange
{
  private ChartImpl m_chart;
  private IRange m_dataRange;
  private WorksheetImpl m_sheet;
  private bool IsSerieEmpty;

  internal ChartDataRange(IOfficeChart chart)
  {
    this.m_chart = chart as ChartImpl;
    this.m_sheet = this.m_chart.Workbook.Worksheets[this.m_chart.ActiveSheetIndex] as WorksheetImpl;
  }

  public int FirstRow => this.m_dataRange.Row;

  public int LastRow => this.m_dataRange.LastRow;

  public int FirstColumn => this.m_dataRange.Column;

  public int LastColumn => this.m_dataRange.LastColumn;

  internal int Count => this.m_dataRange.Count;

  public void SetValue(int rowIndex, int columnIndex, int value)
  {
    if (this.m_sheet[rowIndex, columnIndex].HasFormulaNumberValue)
      this.m_sheet[rowIndex, columnIndex].FormulaNumberValue = (double) value;
    else
      this.m_sheet[rowIndex, columnIndex].Number = (double) value;
    if (this.IsSerieEmpty)
      return;
    this.UpdateChartCache();
  }

  private void UpdateChartCache()
  {
    this.m_chart.CategoryLabelValues = (object[]) null;
    foreach (ChartSerieImpl chartSerieImpl in (IEnumerable<IOfficeChartSerie>) this.m_chart.Series)
    {
      chartSerieImpl.EnteredDirectlyValues = (object[]) null;
      chartSerieImpl.EnteredDirectlyCategoryLabels = (object[]) null;
    }
    this.m_chart.DetectChartType();
    foreach (ChartSerieImpl chartSerieImpl in (IEnumerable<IOfficeChartSerie>) this.m_chart.Series)
    {
      string addressGlobal1 = ((ChartDataRange) chartSerieImpl.NameRange).Range.AddressGlobal;
      if (addressGlobal1 != null)
      {
        if (!(chartSerieImpl.NameRange as ChartDataRange).Range.HasFormula)
          chartSerieImpl.NameRangeIRange.Value2 = this.m_chart.Workbook.ActiveSheet.Range[addressGlobal1].Value2;
        if ((chartSerieImpl.NameRange as ChartDataRange).Range.HasFormulaNumberValue)
          chartSerieImpl.NameRangeIRange.Value2 = (object) this.m_chart.Workbook.ActiveSheet.Range[addressGlobal1].FormulaNumberValue;
        else if ((chartSerieImpl.NameRange as ChartDataRange).Range.HasFormulaStringValue)
          chartSerieImpl.NameRangeIRange.Value2 = (object) this.m_chart.Workbook.ActiveSheet.Range[addressGlobal1].FormulaStringValue;
        else if ((chartSerieImpl.NameRange as ChartDataRange).Range.HasFormulaDateTime)
          chartSerieImpl.NameRangeIRange.Value2 = (object) this.m_chart.Workbook.ActiveSheet.Range[addressGlobal1].FormulaDateTime;
      }
      foreach (RangeImpl cell in chartSerieImpl.ValuesIRange.Cells)
      {
        string addressGlobal2 = cell.AddressGlobal;
        if (addressGlobal2 != null)
        {
          if (!this.m_chart.Workbook.ActiveSheet.Range[addressGlobal2].HasFormula)
            cell.Value2 = this.m_chart.Workbook.ActiveSheet.Range[addressGlobal2].Value2;
          else if (this.m_chart.Workbook.ActiveSheet.Range[addressGlobal2].HasFormulaNumberValue)
            cell.Value2 = (object) this.m_chart.Workbook.ActiveSheet.Range[addressGlobal2].FormulaNumberValue;
          else if (this.m_chart.Workbook.ActiveSheet.Range[addressGlobal2].HasFormulaStringValue)
            cell.Value2 = (object) this.m_chart.Workbook.ActiveSheet.Range[addressGlobal2].FormulaStringValue;
          else if (this.m_chart.Workbook.ActiveSheet.Range[addressGlobal2].HasFormulaDateTime)
            cell.Value2 = (object) this.m_chart.Workbook.ActiveSheet.Range[addressGlobal2].FormulaDateTime;
        }
      }
    }
    foreach (ChartCategory category in (IEnumerable<IOfficeChartCategory>) this.m_chart.Categories)
    {
      foreach (RangeImpl cell in category.CategoryLabelIRange.Cells)
      {
        string addressGlobal = cell.AddressGlobal;
        if (addressGlobal != null)
          cell.Value2 = this.m_chart.Workbook.ActiveSheet.Range[addressGlobal].Value2;
      }
    }
    this.IsSerieEmpty = true;
  }

  public void SetValue(int rowIndex, int columnIndex, double value)
  {
    if (this.m_sheet[rowIndex, columnIndex].HasFormulaNumberValue)
      this.m_sheet[rowIndex, columnIndex].FormulaNumberValue = value;
    else
      this.m_sheet[rowIndex, columnIndex].Number = value;
    if (this.IsSerieEmpty)
      return;
    this.UpdateChartCache();
  }

  public void SetValue(int rowIndex, int columnIndex, string value)
  {
    DateTime result;
    if (this.m_sheet[rowIndex, columnIndex].HasFormulaStringValue)
    {
      if (DateTime.TryParse(value, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
        this.m_sheet[rowIndex, columnIndex].FormulaDateTime = Convert.ToDateTime(value);
      else
        this.m_sheet[rowIndex, columnIndex].FormulaStringValue = value;
    }
    else if (DateTime.TryParse(value, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
      this.m_sheet[rowIndex, columnIndex].DateTime = Convert.ToDateTime(value);
    else
      this.m_sheet[rowIndex, columnIndex].Text = value;
    if (this.IsSerieEmpty)
      return;
    this.UpdateChartCache();
  }

  public void SetValue(int rowIndex, int columnIndex, object value)
  {
    this.m_sheet[rowIndex, columnIndex].Value2 = value;
    if (this.IsSerieEmpty)
      return;
    this.UpdateChartCache();
  }

  public void SetValue(int rowIndex, int columnIndex, DateTime value)
  {
    if (this.m_sheet[rowIndex, columnIndex].HasFormulaDateTime)
      this.m_sheet[rowIndex, columnIndex].FormulaDateTime = value;
    else
      this.m_sheet[rowIndex, columnIndex].DateTime = value;
    if (this.IsSerieEmpty)
      return;
    this.UpdateChartCache();
  }

  public object GetValue(int rowIndex, int columnIndex)
  {
    return this.m_sheet[rowIndex, columnIndex].Value2;
  }

  internal IRange Range
  {
    get => this.m_dataRange;
    set
    {
      this.m_dataRange = value;
      if (this.m_sheet == null || value == null || this.m_sheet == value.Worksheet)
        return;
      this.m_sheet = value.Worksheet as WorksheetImpl;
    }
  }

  internal WorksheetImpl SheetImpl => this.m_sheet;
}
