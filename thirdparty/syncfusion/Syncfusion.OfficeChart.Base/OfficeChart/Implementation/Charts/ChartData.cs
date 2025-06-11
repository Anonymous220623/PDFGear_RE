// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartData
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartData : IOfficeChartData
{
  private ChartImpl m_chart;
  private WorksheetImpl m_sheet;
  private bool IsSerieEmpty;

  internal ChartData(IOfficeChart chart)
  {
    this.m_chart = chart as ChartImpl;
    this.m_sheet = this.m_chart.Workbook.Worksheets[0] as WorksheetImpl;
  }

  public IOfficeDataRange this[int firstRow, int firstCol, int lastRow, int lastCol]
  {
    get
    {
      return (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.m_chart)
      {
        Range = this.m_sheet[firstRow, firstCol, lastRow, lastCol]
      };
    }
  }

  internal ChartData Clone(IOfficeChart chart)
  {
    ChartData chartData = (ChartData) this.MemberwiseClone();
    chartData.m_chart = chart as ChartImpl;
    chartData.m_sheet = chartData.m_chart.Workbook.Worksheets[0] as WorksheetImpl;
    return chartData;
  }

  public void SetValue(int rowIndex, int columnIndex, int value)
  {
    if (this.m_sheet[rowIndex, columnIndex].HasFormulaNumberValue)
      this.m_sheet[rowIndex, columnIndex].FormulaNumberValue = (double) value;
    else
      this.m_sheet[rowIndex, columnIndex].Number = (double) value;
    if (this.IsSerieEmpty)
      return;
    this.ClearCacheValues();
  }

  public void SetValue(int rowIndex, int columnIndex, double value)
  {
    if (this.m_sheet[rowIndex, columnIndex].HasFormulaNumberValue)
      this.m_sheet[rowIndex, columnIndex].FormulaNumberValue = value;
    else
      this.m_sheet[rowIndex, columnIndex].Number = value;
    if (this.IsSerieEmpty)
      return;
    this.ClearCacheValues();
  }

  public void SetValue(int rowIndex, int columnIndex, string value)
  {
    DateTime result;
    if (this.m_sheet[rowIndex, columnIndex].HasFormulaStringValue)
    {
      if (DateTime.TryParse(value, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
        this.m_sheet[rowIndex, columnIndex].FormulaDateTime = Convert.ToDateTime(value);
      else
        this.m_sheet[rowIndex, columnIndex].FormulaStringValue = !string.IsNullOrEmpty(value) ? value : string.Empty;
    }
    else if (DateTime.TryParse(value, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
      this.m_sheet[rowIndex, columnIndex].DateTime = Convert.ToDateTime(value);
    else
      this.m_sheet[rowIndex, columnIndex].Text = !string.IsNullOrEmpty(value) ? value : string.Empty;
    if (this.IsSerieEmpty)
      return;
    this.ClearCacheValues();
  }

  public void SetValue(int rowIndex, int columnIndex, object value)
  {
    this.m_sheet[rowIndex, columnIndex].Value2 = value;
    if (this.IsSerieEmpty)
      return;
    this.ClearCacheValues();
  }

  public object GetValue(int rowIndex, int columnIndex)
  {
    return this.m_chart.Categories != null && this.m_chart.Categories.Count == 0 && this.m_chart.Series != null && this.m_chart.Series.Count == 0 ? (object) null : this.m_sheet[rowIndex, columnIndex].Value2;
  }

  public void SetValue(int rowIndex, int columnIndex, DateTime value)
  {
    if (this.m_sheet[rowIndex, columnIndex].HasFormulaDateTime)
      this.m_sheet[rowIndex, columnIndex].FormulaDateTime = value;
    else
      this.m_sheet[rowIndex, columnIndex].DateTime = value;
    if (this.IsSerieEmpty)
      return;
    this.ClearCacheValues();
  }

  public void SetChartData(object[][] data)
  {
    this.SetDataRange(data, 1, 1);
    if (this.IsSerieEmpty)
      return;
    this.ClearCacheValues();
  }

  public void SetDataRange(object[][] data, int rowIndex, int columnIndex)
  {
    for (int index1 = 0; index1 < data.Length; ++index1)
    {
      for (int index2 = 0; index2 < data[index1].Length; ++index2)
        this.m_sheet[index1 + rowIndex, index2 + columnIndex].Value2 = data[index1][index2];
    }
    this.m_chart.DataRange = this[rowIndex, columnIndex, data.Length, data[0].Length];
  }

  public void SetDataRange(IEnumerable enumerable, int rowIndex, int columnIndex)
  {
    int iRow = rowIndex;
    int num1 = columnIndex;
    IEnumerator enumerator = enumerable.GetEnumerator();
    if (enumerator == null)
      return;
    bool flag = false;
    List<PropertyInfo> propertyInfo = (List<PropertyInfo>) null;
    int num2 = 0;
    enumerator.MoveNext();
    object current1 = enumerator.Current;
    if (current1 == null)
      return;
    Type type = current1.GetType();
    if (type.Namespace == null || type.Namespace != null && !type.Namespace.Contains("System"))
      flag = true;
    if (!flag)
      return;
    IMigrantRange migrantRange = this.m_sheet.MigrantRange;
    List<TypeCode> objectMembersInfo = this.GetObjectMembersInfo(current1, out propertyInfo);
    int index1 = 0;
    int iColumn1 = num1;
    while (index1 < propertyInfo.Count)
    {
      migrantRange.ResetRowColumn(iRow, iColumn1);
      migrantRange.SetValue(propertyInfo[index1].Name);
      ++index1;
      ++iColumn1;
    }
    int num3 = iRow + 1;
    do
    {
      object current2 = enumerator.Current;
      if (current2 != null)
      {
        int index2 = 0;
        int iColumn2 = num1;
        while (index2 < propertyInfo.Count)
        {
          PropertyInfo strProperty = propertyInfo[index2];
          migrantRange.ResetRowColumn(num3 + num2, iColumn2);
          object valueFromProperty1 = this.GetValueFromProperty(current2, strProperty);
          if (valueFromProperty1 != null)
          {
            switch (objectMembersInfo[index2])
            {
              case TypeCode.Boolean:
                migrantRange.SetValue((bool) valueFromProperty1);
                break;
              case TypeCode.Int16:
                migrantRange.SetValue((int) Convert.ToInt16(valueFromProperty1));
                break;
              case TypeCode.Int32:
                migrantRange.SetValue((int) valueFromProperty1);
                break;
              case TypeCode.Int64:
              case TypeCode.Decimal:
                migrantRange.SetValue(Convert.ToDouble(valueFromProperty1));
                break;
              case TypeCode.Double:
                migrantRange.SetValue((double) valueFromProperty1);
                break;
              case TypeCode.DateTime:
                migrantRange.SetValue((DateTime) valueFromProperty1);
                break;
              case TypeCode.String:
                string valueFromProperty2 = (string) this.GetValueFromProperty(current2, strProperty);
                if (valueFromProperty2 != null && valueFromProperty2.Length != 0)
                {
                  migrantRange.SetValue(valueFromProperty2);
                  break;
                }
                break;
            }
          }
          ++index2;
          ++iColumn2;
        }
        ++num2;
      }
    }
    while (enumerator.MoveNext());
    this.m_chart.DataRange = this[rowIndex, columnIndex, num2 + rowIndex, propertyInfo.Count + columnIndex - 1];
  }

  public void Clear()
  {
    IWorkbook workbook = this.m_chart.Workbook;
    int activeSheetIndex = workbook.ActiveSheetIndex;
    if ((workbook as WorkbookImpl).Objects[activeSheetIndex] is WorksheetImpl worksheetImpl)
    {
      string[] strArray = worksheetImpl.UsedRange.AddressR1C1Local.Split('R', 'C');
      if (strArray[1] + strArray[2] != "00")
        worksheetImpl.UsedRange.Clear();
    }
    if (this.m_chart.Categories != null)
      this.m_chart.Categories.Clear();
    if (this.m_chart.Series != null)
      this.m_chart.Series.Clear();
    this.m_chart.IsChartCleared = true;
  }

  private string[] GetHeaderTypes(object[][] data)
  {
    string[] headerTypes = new string[data[0].Length];
    for (int index = 0; index < data[0].Length; ++index)
      headerTypes[index] = data[0][index].GetType().Name;
    return headerTypes;
  }

  private List<TypeCode> GetObjectMembersInfo(object obj, out List<PropertyInfo> propertyInfo)
  {
    Type type = obj.GetType();
    List<TypeCode> objectMembersInfo = new List<TypeCode>();
    propertyInfo = new List<PropertyInfo>();
    foreach (PropertyInfo property in type.GetProperties())
    {
      propertyInfo.Add(property);
      objectMembersInfo.Add(Type.GetTypeCode(property.PropertyType));
    }
    return objectMembersInfo;
  }

  private object GetValueFromProperty(object value, PropertyInfo strProperty)
  {
    value = !(strProperty == (PropertyInfo) null) ? strProperty.GetValue(value, (object[]) null) : throw new ArgumentOutOfRangeException("Can't find property");
    return value;
  }

  private void ClearCacheValues()
  {
    this.m_chart.CategoryLabelValues = (object[]) null;
    foreach (ChartSerieImpl chartSerieImpl in (IEnumerable<IOfficeChartSerie>) this.m_chart.Series)
    {
      chartSerieImpl.EnteredDirectlyCategoryLabels = (object[]) null;
      chartSerieImpl.EnteredDirectlyValues = (object[]) null;
      if (this.m_chart.ChartType == OfficeChartType.Bubble || this.m_chart.ChartType == OfficeChartType.Bubble_3D)
        chartSerieImpl.EnteredDirectlyBubbles = (object[]) null;
    }
    this.IsSerieEmpty = true;
  }

  internal IOfficeDataRange this[IRange range]
  {
    get
    {
      return (IOfficeDataRange) new ChartDataRange((IOfficeChart) this.m_chart)
      {
        Range = this.m_sheet[range.Row, range.Column, range.LastRow, range.LastColumn]
      };
    }
  }

  internal WorksheetImpl SheetImpl
  {
    get => this.m_sheet;
    set => this.m_sheet = value;
  }
}
