// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.PivotTables.PivotEngineSerialization
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.PivotAnalysis;
using Syncfusion.XlsIO.Implementation.PivotTables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.PivotTables;

public class PivotEngineSerialization
{
  private ExtendedFormatImpl extendedFormat;
  private PivotTableImpl pivotTableImple;

  public PivotEngine PopulatePivotEngine(IWorksheet sheet, PivotTableImpl pivotTable)
  {
    PivotEngine pivotEngine = new PivotEngine();
    PivotCacheImpl cache = pivotTable.Cache;
    if (cache.SourceRange != null)
    {
      IWorksheet worksheet = cache.SourceRange.Worksheet;
      DataTable dataTable = (worksheet as WorksheetImpl).PEExportDataTable(worksheet[cache.SourceRange.AddressLocal], ExcelExportDataTableOptions.ColumnNames | ExcelExportDataTableOptions.DetectColumnTypes, pivotTable);
      pivotEngine.DataSource = (object) dataTable;
      if (pivotTable.RowFields.Count > 0)
      {
        List<int> intList = new List<int>();
        PivotTableFields fields1 = pivotTable.Fields;
        List<PivotFieldImpl> fields2 = pivotTable.GetFields(PivotAxisTypes.Row);
        int index1 = 0;
        for (int count = fields2.Count; index1 < count; ++index1)
        {
          PivotFieldImpl pivotFieldImpl = fields2[index1];
          intList.Add(fields1.IndexOf(pivotFieldImpl));
        }
        if (pivotTable.RowFieldsOrder.Count > 0)
        {
          intList.Clear();
          for (int index2 = 0; index2 < pivotTable.RowFieldsOrder.Count; ++index2)
            intList.Add(pivotTable.RowFieldsOrder[index2]);
        }
        int index3 = 0;
        for (int count1 = pivotTable.RowFields.Count; index3 < count1; ++index3)
        {
          PivotCacheFieldImpl cacheField = pivotTable.Cache.CacheFields[intList[index3]];
          if ((cacheField.DataType & PivotDataType.String) != (PivotDataType) 0)
            pivotEngine.PivotRows.Add(new PivotItem()
            {
              FieldMappingName = pivotTable.Fields[intList[index3]].Name,
              Comparer = (IComparer) new PivotEngineSerialization.CustomComparer()
            });
          else if ((cacheField.DataType & PivotDataType.Number) != (PivotDataType) 0 && (cacheField.DataType & PivotDataType.Integer) == (PivotDataType) 0)
            pivotEngine.PivotRows.Add(new PivotItem()
            {
              FieldMappingName = pivotTable.Fields[intList[index3]].Name,
              Comparer = (IComparer) new PivotEngineSerialization.CustomComparer()
            });
          else
            pivotEngine.PivotRows.Add(new PivotItem()
            {
              FieldMappingName = pivotTable.Fields[intList[index3]].Name,
              Comparer = (IComparer) new PivotEngineSerialization.CustomComparer()
            });
          PivotFilterCollections pivotFilters = pivotTable.Fields[intList[index3]].PivotFilters as PivotFilterCollections;
          if (pivotFilters.ValueFilter != null)
          {
            IPivotValueLableFilter valueFilter = pivotFilters.ValueFilter;
            if (valueFilter.Type == PivotFilterType.CaptionEqual)
            {
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index3]].Name)}{" = "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else if (valueFilter.Type == PivotFilterType.CaptionGreaterThan)
            {
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index3]].Name)}{" > "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else if (valueFilter.Type == PivotFilterType.CaptionGreaterThanOrEqual)
            {
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index3]].Name)}{" >= "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else if (valueFilter.Type == PivotFilterType.CaptionLessThan)
            {
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index3]].Name)}{" < "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else if (valueFilter.Type == PivotFilterType.CaptionLessThanOrEqual)
            {
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index3]].Name)}{" <= "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else if (valueFilter.Type == PivotFilterType.CaptionBetween)
            {
              string str = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", (object) this.UpdateColumnName(pivotTable.Fields[intList[index3]].Name), (object) " >= ", (object) "'", (object) valueFilter.Value1, (object) "'", (object) "AND ", (object) pivotTable.Fields[intList[index3]].Name, (object) " <= ", (object) "'", (object) valueFilter.Value2, (object) "'");
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else
            {
              if (valueFilter.Type != PivotFilterType.CaptionNotEqual)
                throw new ArgumentException(valueFilter.Type.ToString() + " Filter type is not supported in Layout");
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index3]].Name)}{" <> "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
          }
          int i1 = intList[index3];
          Dictionary<int, Dictionary<string, int>> dictionary1 = new Dictionary<int, Dictionary<string, int>>();
          int num = 0;
          foreach (int i2 in intList)
          {
            List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortFieldValues(fields1[i2].CacheField, (List<string>) null);
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
            for (int index4 = 0; index4 < comparisonPairList.Count; ++index4)
            {
              if (comparisonPairList[index4].Value != null)
                dictionary2.Add(comparisonPairList[index4].Value.ToString(), index4);
            }
            dictionary1.Add(num++, dictionary2);
          }
          Dictionary<string, int> dictionary3 = dictionary1[intList.IndexOf(i1)];
          if (pivotTable.Fields[i1].IsMultiSelected)
          {
            string str1 = (string) null;
            bool flag = true;
            int index5 = 0;
            for (int count2 = pivotTable.Fields[i1].Items.Count; index5 < count2; ++index5)
            {
              if (pivotTable.Fields[i1].Items[index5].Visible)
              {
                PivotFieldItem pivotFieldItem = pivotTable.Fields[i1].Items[index5] as PivotFieldItem;
                if (pivotFieldItem.Text != null && dictionary3.ContainsKey(pivotFieldItem.Text))
                {
                  string str2 = this.UpdateColumnName(pivotTable.Fields[i1].Name);
                  if (!flag)
                    str1 += " OR ";
                  str1 = $"{$"{str1}{str2}="}'{pivotFieldItem.Text}'";
                  flag = false;
                }
              }
            }
            pivotEngine.Filters.Add(new FilterExpression()
            {
              Expression = str1,
              Name = this.UpdateColumnName(pivotTable.Fields[i1].Name)
            });
          }
        }
      }
      if (pivotTable.ColumnFields.Count > 0)
      {
        List<int> intList = new List<int>();
        PivotTableFields fields3 = pivotTable.Fields;
        List<PivotFieldImpl> fields4 = pivotTable.GetFields(PivotAxisTypes.Column);
        int index6 = 0;
        for (int count = fields4.Count; index6 < count; ++index6)
        {
          PivotFieldImpl pivotFieldImpl = fields4[index6];
          intList.Add(fields3.IndexOf(pivotFieldImpl));
        }
        if (pivotTable.ColFieldsOrder.Count > 0)
        {
          intList.Clear();
          for (int index7 = 0; index7 < pivotTable.ColFieldsOrder.Count; ++index7)
          {
            if (pivotTable.ColFieldsOrder[index7] >= 0)
              intList.Add(pivotTable.ColFieldsOrder[index7]);
          }
        }
        int index8 = 0;
        for (int count3 = pivotTable.ColumnFields.Count; index8 < count3; ++index8)
        {
          PivotCacheFieldImpl cacheField = pivotTable.Cache.CacheFields[intList[index8]];
          if ((cacheField.DataType & PivotDataType.String) != (PivotDataType) 0)
            pivotEngine.PivotColumns.Add(new PivotItem()
            {
              FieldMappingName = pivotTable.Fields[intList[index8]].Name
            });
          else if (pivotTable.Options.RowLayout == PivotTableRowLayout.Compact && (cacheField.DataType & PivotDataType.Number) != (PivotDataType) 0 && (cacheField.DataType & PivotDataType.Integer) == (PivotDataType) 0)
            pivotEngine.PivotColumns.Add(new PivotItem()
            {
              FieldMappingName = pivotTable.Fields[intList[index8]].Name,
              Comparer = (IComparer) new PivotEngineSerialization.CustomComparer()
            });
          else
            pivotEngine.PivotColumns.Add(new PivotItem()
            {
              FieldMappingName = pivotTable.Fields[intList[index8]].Name,
              Comparer = (IComparer) new PivotEngineSerialization.CustomComparer()
            });
          PivotFilterCollections pivotFilters = pivotTable.Fields[intList[index8]].PivotFilters as PivotFilterCollections;
          if (pivotFilters.ValueFilter != null)
          {
            IPivotValueLableFilter valueFilter = pivotFilters.ValueFilter;
            if (valueFilter.Type == PivotFilterType.CaptionEqual)
            {
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index8]].Name)}{" = "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else if (valueFilter.Type == PivotFilterType.CaptionGreaterThan)
            {
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index8]].Name)}{" > "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else if (valueFilter.Type == PivotFilterType.CaptionGreaterThanOrEqual)
            {
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index8]].Name)}{" >= "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else if (valueFilter.Type == PivotFilterType.CaptionLessThan)
            {
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index8]].Name)}{" < "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else if (valueFilter.Type == PivotFilterType.CaptionLessThanOrEqual)
            {
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index8]].Name)}{" <= "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else if (valueFilter.Type == PivotFilterType.CaptionBetween)
            {
              string str = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", (object) this.UpdateColumnName(pivotTable.Fields[intList[index8]].Name), (object) " >= ", (object) "'", (object) valueFilter.Value1, (object) "'", (object) "AND ", (object) pivotTable.Fields[intList[index8]].Name, (object) " <= ", (object) "'", (object) valueFilter.Value2, (object) "'");
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
            else
            {
              if (valueFilter.Type != PivotFilterType.CaptionNotEqual)
                throw new ArgumentException(valueFilter.Type.ToString() + " Filter type is not supported in Layout");
              string str = $"{this.UpdateColumnName(pivotTable.Fields[intList[index8]].Name)}{" <> "}{"'"}{valueFilter.Value1}{"'"}";
              pivotEngine.Filters.Add(new FilterExpression()
              {
                Expression = str,
                Name = "A1"
              });
            }
          }
          int i3 = intList[index8];
          Dictionary<int, Dictionary<string, int>> dictionary4 = new Dictionary<int, Dictionary<string, int>>();
          int num = 0;
          foreach (int i4 in intList)
          {
            List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortFieldValues(fields3[i4].CacheField, (List<string>) null);
            Dictionary<string, int> dictionary5 = new Dictionary<string, int>();
            for (int index9 = 0; index9 < comparisonPairList.Count; ++index9)
            {
              if (comparisonPairList[index9].Value != null)
                dictionary5.Add(comparisonPairList[index9].Value.ToString(), index9);
            }
            dictionary4.Add(num++, dictionary5);
          }
          Dictionary<string, int> dictionary6 = dictionary4[intList.IndexOf(i3)];
          if (pivotTable.Fields[i3].IsMultiSelected)
          {
            string str = (string) null;
            bool flag = true;
            int index10 = 0;
            for (int count4 = pivotTable.Fields[i3].Items.Count; index10 < count4; ++index10)
            {
              if (pivotTable.Fields[i3].Items[index10].Visible)
              {
                PivotFieldItem pivotFieldItem = pivotTable.Fields[i3].Items[index10] as PivotFieldItem;
                if (pivotFieldItem.Text != null && dictionary6.ContainsKey(pivotFieldItem.Text))
                {
                  if (!flag)
                    str += " OR ";
                  str = $"{$"{str}{this.UpdateColumnName(pivotTable.Fields[i3].Name)}="}'{pivotFieldItem.Text}'";
                  flag = false;
                }
              }
            }
            pivotEngine.Filters.Add(new FilterExpression()
            {
              Expression = str,
              Name = this.UpdateColumnName(pivotTable.Fields[i3].Name)
            });
          }
        }
      }
      if (pivotTable.DataFields.Count > 0)
      {
        for (int i = 0; i < pivotTable.DataFields.Count; ++i)
        {
          string input = pivotTable.DataFields[i].Field.Name;
          if (input.Contains("_x000a_"))
            input = Regex.Replace(input, "_x000a_", "\n");
          string numberFormat = pivotTable.DataFields[i].NumberFormat;
          if (pivotTable.DataFields[i].Subtotal == PivotSubtotalTypes.Sum || pivotTable.DataFields[i].Subtotal == PivotSubtotalTypes.Default || pivotTable.DataFields[i].Subtotal == PivotSubtotalTypes.None)
            pivotEngine.PivotCalculations.Add(new PivotComputationInfo()
            {
              FieldName = input,
              FieldHeader = pivotTable.DataFields[i].Name,
              SummaryType = SummaryType.DoubleTotalSum,
              Format = numberFormat ?? "#,###0.0"
            });
          else if (pivotTable.DataFields[i].Subtotal == PivotSubtotalTypes.Count)
            pivotEngine.PivotCalculations.Add(new PivotComputationInfo()
            {
              FieldName = pivotTable.DataFields[i].Field.Name,
              FieldHeader = pivotTable.DataFields[i].Name,
              SummaryType = SummaryType.Count,
              Format = numberFormat ?? "#,###0.0"
            });
          else if (pivotTable.DataFields[i].Subtotal == PivotSubtotalTypes.Max)
            pivotEngine.PivotCalculations.Add(new PivotComputationInfo()
            {
              FieldName = pivotTable.DataFields[i].Field.Name,
              FieldHeader = pivotTable.DataFields[i].Name,
              SummaryType = SummaryType.DoubleMaximum,
              Format = numberFormat ?? "#,###0.0"
            });
          else if (pivotTable.DataFields[i].Subtotal == PivotSubtotalTypes.Average)
            pivotEngine.PivotCalculations.Add(new PivotComputationInfo()
            {
              FieldName = pivotTable.DataFields[i].Field.Name,
              FieldHeader = pivotTable.DataFields[i].Name,
              SummaryType = SummaryType.DoubleAverage,
              Format = numberFormat ?? "#,###0.0"
            });
          else if (pivotTable.DataFields[i].Subtotal == PivotSubtotalTypes.Min)
            pivotEngine.PivotCalculations.Add(new PivotComputationInfo()
            {
              FieldName = pivotTable.DataFields[i].Field.Name,
              FieldHeader = pivotTable.DataFields[i].Name,
              SummaryType = SummaryType.DoubleMinimum,
              Format = numberFormat ?? "#,###0.0"
            });
        }
      }
      if (pivotTable.PageFields.Count > 0)
      {
        List<int> intList = new List<int>();
        PivotTableFields fields5 = pivotTable.Fields;
        List<PivotFieldImpl> fields6 = pivotTable.GetFields(PivotAxisTypes.Page);
        int index11 = 0;
        for (int count = fields6.Count; index11 < count; ++index11)
        {
          PivotFieldImpl pivotFieldImpl = fields6[index11];
          intList.Add(fields5.IndexOf(pivotFieldImpl));
        }
        Dictionary<int, Dictionary<string, int>> dictionary7 = new Dictionary<int, Dictionary<string, int>>();
        int num = 0;
        foreach (int i in intList)
        {
          List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortFieldValues(fields5[i].CacheField, (List<string>) null);
          Dictionary<string, int> dictionary8 = new Dictionary<string, int>();
          for (int index12 = 0; index12 < comparisonPairList.Count; ++index12)
          {
            if (comparisonPairList[index12].Value != null)
              dictionary8.Add(comparisonPairList[index12].Value.ToString(), index12);
          }
          dictionary7.Add(num++, dictionary8);
        }
        foreach (int i in intList)
        {
          Dictionary<string, int> dictionary9 = dictionary7[intList.IndexOf(i)];
          if (!pivotTable.Fields[i].IsMultiSelected)
          {
            if (pivotTable.Fields[i].PivotFilters[0] != null)
            {
              string str = pivotTable.Fields[i].PivotFilters[0].Value1;
              if (str != null && dictionary9.ContainsKey(str))
              {
                bool flag1 = int.TryParse(str, out int _);
                bool flag2 = double.TryParse(str, out double _);
                if (!flag1 && !flag2)
                  str = $"'{str}'";
                pivotEngine.Filters.Add(new FilterExpression()
                {
                  Expression = $"{this.UpdateColumnName(pivotTable.Fields[i].Name)}={str}",
                  Name = this.UpdateColumnName(pivotTable.Fields[i].Name)
                });
              }
            }
          }
          else if (pivotTable.Fields[i].IsMultiSelected)
          {
            string str = (string) null;
            bool flag = true;
            int index13 = 0;
            for (int count = pivotTable.Fields[i].Items.Count; index13 < count; ++index13)
            {
              if (pivotTable.Fields[i].Items[index13].Visible)
              {
                PivotFieldItem pivotFieldItem = pivotTable.Fields[i].Items[index13] as PivotFieldItem;
                if (pivotFieldItem.Text != null && dictionary9.ContainsKey(pivotFieldItem.Text))
                {
                  if (!flag)
                    str += " OR ";
                  str = $"{$"{str}{this.UpdateColumnName(pivotTable.Fields[i].Name)}="}'{pivotFieldItem.Text}'";
                  flag = false;
                }
              }
            }
            pivotEngine.Filters.Add(new FilterExpression()
            {
              Expression = str,
              Name = this.UpdateColumnName(pivotTable.Fields[i].Name)
            });
          }
        }
      }
      pivotTable.RowsPerPage = pivotTable.PageFields.Count;
      pivotEngine.Populate();
      if (pivotTable.Options.RowLayout == PivotTableRowLayout.Outline)
        pivotEngine.ReArrangePivotValuesForRows();
      this.RenderPivotTable(pivotEngine, (IWorksheet) pivotTable.Worksheet, pivotTable);
      return pivotEngine;
    }
    if (cache.HasNamedRange)
      throw new ArgumentNullException("PivotTable cannot be created for table with Named Range");
    return (PivotEngine) null;
  }

  internal string UpdateColumnName(string columnName)
  {
    foreach (char ch in new List<char>()
    {
      '~',
      '(',
      ')',
      '#',
      '\\',
      '/',
      '=',
      '>',
      '<',
      '+',
      '-',
      '*',
      '%',
      '&',
      '|',
      '^',
      '\'',
      '"',
      '[',
      ']',
      ' '
    })
    {
      if (columnName.Contains(ch.ToString()))
      {
        if (columnName.Contains("\\"))
          columnName = columnName.Replace("\\", "\\\\");
        if (columnName.Contains("]"))
          columnName = columnName.Replace("]", "\\]");
        columnName = $"[{columnName}]";
        break;
      }
    }
    return columnName;
  }

  private void SetPivotTableOptions(PivotEngine pivotEngine, PivotTableImpl pivotTable)
  {
    bool rowGrand = pivotTable.RowGrand;
    bool columnGrand = pivotTable.ColumnGrand;
    List<int> intList1 = new List<int>();
    List<int> intList2 = new List<int>();
    for (int rowIndex = 0; rowIndex < pivotEngine.RowCount; ++rowIndex)
    {
      for (int columnIndex1 = 0; columnIndex1 < pivotEngine.ColumnCount; ++columnIndex1)
      {
        if (pivotEngine[rowIndex, columnIndex1] != null)
        {
          if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0)
          {
            if (!intList1.Contains(columnIndex1))
              intList1.Add(columnIndex1);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 && !intList2.Contains(rowIndex))
            intList2.Add(rowIndex);
        }
      }
    }
    if (!columnGrand)
    {
      foreach (int num in intList1)
      {
        for (int index = 0; index < pivotEngine.RowCount; ++index)
        {
          if (pivotEngine[index, num] != null)
            pivotEngine.PivotValues[index][num] = (PivotCellInfo) null;
        }
      }
    }
    if (rowGrand)
      return;
    foreach (int num in intList2)
    {
      for (int index = 0; index < pivotEngine.ColumnCount; ++index)
      {
        if (pivotEngine[num, index] != null)
          pivotEngine.PivotValues[num][index] = (PivotCellInfo) null;
      }
    }
  }

  public void RenderPivotTable(
    PivotEngine pivotEngine,
    IWorksheet pivotSheet,
    PivotTableImpl pivotTable)
  {
    switch (pivotTable.Options.RowLayout)
    {
      case PivotTableRowLayout.Compact:
        this.RenderCompactLayout(pivotEngine, pivotSheet, pivotTable);
        break;
      case PivotTableRowLayout.Outline:
        this.RenderOutlineLayout(pivotEngine, pivotSheet, pivotTable);
        break;
      case PivotTableRowLayout.Tabular:
        this.RenderTabularLayout(pivotEngine, pivotSheet, pivotTable);
        break;
    }
  }

  private void RenderOutlineLayout(
    PivotEngine pivotEngine,
    IWorksheet pivotSheet,
    PivotTableImpl pivotTable)
  {
    this.pivotTableImple = pivotTable;
    if (pivotTable.PageFields.Count > 0)
    {
      int num = 2;
      for (int index = pivotTable.PageFields.Count - 1; index >= 0; --index)
      {
        pivotSheet[pivotTable.Location.Row - num, pivotTable.Location.Column].Value2 = (object) pivotTable.PageFields[index].Name;
        pivotSheet[pivotTable.Location.Row - num, pivotTable.Location.Column + 1].Value2 = (pivotTable.PageFields[index] as PivotFieldImpl).IsMultiSelected ? (object) "(Multiple Items)" : (pivotTable.PageFields[index].PivotFilters[0] == null ? (object) "(All)" : (object) pivotTable.PageFields[index].PivotFilters[0].Value1);
        pivotSheet[pivotTable.Location.Row - num, pivotTable.Location.Column + 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
        ++num;
      }
    }
    int num1 = pivotTable.Location.Row;
    int column1 = pivotTable.Location.Column;
    PivotTableLayout pivotTableLayout = new PivotTableLayout();
    PivotValueCollections valueCollections = new PivotValueCollections();
    int row1 = pivotTable.Location.Row;
    int column2 = pivotTable.Location.Column;
    this.extendedFormat = new ExtendedFormatImpl(pivotTable.Workbook.Application, (object) pivotTable.Workbook);
    PivotTableParts pivotTableParts1 = PivotTableParts.None;
    PivotTableParts pivotTableParts2 = PivotTableParts.None;
    int index1 = 0;
    int num2 = 0;
    int num3 = 0;
    bool flag1 = false;
    List<int> intList = new List<int>();
    this.SetOutline(pivotTable, pivotEngine);
    this.SetPivotTableOptions(pivotEngine, pivotTable);
    for (int rowIndex = 0; rowIndex < pivotEngine.RowCount; ++rowIndex)
    {
      bool flag2 = false;
      bool flag3 = false;
      for (int columnIndex1 = 0; columnIndex1 < pivotEngine.ColumnCount; ++columnIndex1)
      {
        if (pivotEngine[rowIndex, columnIndex1] != null)
        {
          if (pivotEngine[rowIndex, columnIndex1].Value != null && pivotEngine[rowIndex, columnIndex1].Value != (object) "x" && pivotEngine[rowIndex, columnIndex1].Value.ToString() == "x")
            pivotEngine[rowIndex, columnIndex1].Value = (object) "";
          if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TopLeftCell) != (PivotCellType) 0)
          {
            int num4 = pivotTable.Location.Row + pivotTable.ColumnFields.Count + (pivotTable.ColumnFields.Count <= 0 || pivotTable.DataFields.Count <= 1 ? 0 : 1);
            if (index1 + 1 <= pivotTable.RowFields.Count)
              pivotSheet[num4 + rowIndex, column1 + columnIndex1].Value2 = (object) pivotTable.Cache.CacheFields[pivotTable.RowFieldsOrder[index1]].Name;
            PivotTableParts partStyle = columnIndex1 != 0 ? PivotTableParts.WholeTable | PivotTableParts.HeaderRow : PivotTableParts.WholeTable | PivotTableParts.FirstColumn | PivotTableParts.HeaderRow | PivotTableParts.FirstHeaderCell;
            this.FillPivotValue(num4 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num4 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
            ++index1;
            num1 = pivotTable.Location.Row + (pivotTable.DataFields.Count > 0 ? 1 : 0);
            if (pivotTable.ColumnFields.Count == 0)
              num1 = pivotTable.Location.Row;
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0)
          {
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = !flag2 ? (columnIndex1 == pivotTable.RowFields.Count - 1 ? PivotTableParts.WholeTable : (columnIndex1 != 0 ? ((columnIndex1 + 1) % 2 != 0 ? PivotTableParts.WholeTable | PivotTableParts.RowSubHeading3 : PivotTableParts.WholeTable | PivotTableParts.RowSubHeading2) : PivotTableParts.WholeTable | PivotTableParts.RowSubHeading1)) : pivotTableParts2;
            flag2 = true;
            pivotTableParts2 = partStyle;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
          {
            flag2 = true;
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = (object) (pivotEngine[rowIndex, columnIndex1].Value.ToString() + " Total");
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.SubtotalRow1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.HeaderCell) != (PivotCellType) 0)
          {
            PivotTableParts partStyle;
            if (flag2 && columnIndex1 + 1 == pivotTable.RowFields.Count)
              partStyle = PivotTableParts.WholeTable | pivotTableParts2;
            else if (flag2 && columnIndex1 + 1 < pivotTable.RowFields.Count)
            {
              partStyle = PivotTableParts.WholeTable | pivotTableParts2;
            }
            else
            {
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
              partStyle = PivotTableParts.WholeTable;
            }
            if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
            {
              if (pivotEngine[rowIndex, columnIndex1].Value != (object) string.Empty)
                pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = (object) (pivotEngine[rowIndex, columnIndex1].Value.ToString() + " Total");
              PivotTableParts pivotTableParts3 = PivotTableParts.WholeTable;
              if (columnIndex1 == 0)
                pivotTableParts3 |= PivotTableParts.FirstColumn;
              partStyle = pivotTableParts3 | PivotTableParts.GrandTotalRow;
              pivotTableParts1 = PivotTableParts.GrandTotalRow;
            }
            else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
              partStyle |= PivotTableParts.SubtotalRow1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
          {
            if (!flag2)
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = (object) (pivotEngine[rowIndex, columnIndex1].Value.ToString() + " Total");
            else if (columnIndex1 + 1 > pivotTable.RowFields.Count)
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.GrandTotalRow;
            flag2 = true;
            pivotTableParts1 = PivotTableParts.GrandTotalRow;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0)
          {
            if (!flag1 && pivotTable.DataFields.Count < 2)
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = (object) (pivotEngine[rowIndex, columnIndex1].Value.ToString() + " Total");
            else if (pivotTable.DataFields.Count >= 2 && pivotEngine[rowIndex, columnIndex1].CellRange != null)
            {
              int num5 = 0;
              for (int left = pivotEngine[rowIndex, columnIndex1].CellRange.Left; left <= pivotEngine[rowIndex, columnIndex1].CellRange.Right; ++left)
              {
                CoveredCellRange cellRange = pivotEngine[rowIndex, columnIndex1].CellRange;
                if (pivotTable.DataFields.Count >= 2 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
                  pivotSheet[num1 + rowIndex, column1 + columnIndex1 + num5].Value2 = (object) "Total";
                if (pivotTable.DataFields.Count > 1)
                  pivotSheet[num1 + rowIndex, column1 + columnIndex1 + num5].Value2 = (object) $"{pivotSheet[num1 + rowIndex, column1 + columnIndex1 + num5].Value2.ToString()} {pivotEngine[pivotEngine[rowIndex, columnIndex1].CellRange.Bottom + 1, left].Value.ToString()}";
                ++num5;
              }
              columnIndex1 += num5 - 1;
            }
            flag1 = true;
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | PivotTableParts.GrandTotalColumn;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0)
          {
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            flag3 = true;
            PivotTableParts pivotTableParts4 = PivotTableParts.WholeTable | PivotTableParts.HeaderRow;
            PivotTableParts partStyle = rowIndex != 0 ? ((rowIndex + 1) % 2 != 0 ? pivotTableParts4 | PivotTableParts.ColumnSubHeading3 : pivotTableParts4 | PivotTableParts.ColumnSubHeading2) : pivotTableParts4 | PivotTableParts.ColumnSubHeading1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
          {
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = (object) (pivotEngine[rowIndex, columnIndex1].Value.ToString() + " Total");
            intList.Add(columnIndex1);
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.HeaderRow;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.HeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0)
          {
            if (!flag3 && !intList.Contains(columnIndex1))
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | PivotTableParts.ColumnSubHeading1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ValueCell) != (PivotCellType) 0)
          {
            if (columnIndex1 != 0 || pivotTable.RowFields.Count <= 1)
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = !flag2 ? PivotTableParts.WholeTable : PivotTableParts.WholeTable | pivotTableParts2;
            if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
            {
              if (pivotTableParts1 != PivotTableParts.None)
                partStyle = pivotTableParts1;
              else
                partStyle |= PivotTableParts.GrandTotalColumn;
            }
            if (!flag2 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
            {
              if (!intList.Contains(columnIndex1))
                partStyle |= PivotTableParts.SubtotalRow1;
              else
                partStyle |= PivotTableParts.SubtotalColumn1;
            }
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.CalculationHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) == (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) == (PivotCellType) 0)
          {
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | PivotTableParts.ColumnSubHeading1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else
          {
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = PivotTableParts.WholeTable;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
        }
        num3 = column1 + columnIndex1;
      }
      num2 = num1 + rowIndex;
    }
    int row2 = pivotTable.Location.Row;
    int column3 = pivotTable.Location.Column;
    for (int index2 = 0; index2 <= pivotTableLayout.maxColumnCount; ++index2)
    {
      PivotTableParts partStyle = index2 != 0 ? PivotTableParts.WholeTable | PivotTableParts.HeaderRow : PivotTableParts.WholeTable | PivotTableParts.FirstColumn | PivotTableParts.HeaderRow;
      this.FillPivotValue(row2 - row1, column3++ - column2, (object) " ", PivotCellType.TopLeftCell, pivotTableLayout, partStyle);
    }
    if (pivotTable.ColumnFields.Count > 0 && pivotTable.RowFields.Count > 0 && pivotTable.DataFields.Count == 1)
      pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column].Value = pivotTable.DataFields[0].Name;
    int count = pivotTable.RowFields.Count;
    for (int index3 = 0; index3 < pivotTable.ColFieldsOrder.Count; ++index3)
    {
      int i = pivotTable.ColFieldsOrder[index3];
      if (i >= 0)
        pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column + count].Value = pivotTable.Cache.CacheFields[i].Name;
      else if (pivotTable.ColFieldsOrder.Count != 1 || pivotTable.ColFieldsOrder[0] >= 0)
        pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column + count].Value = "Values";
      ++count;
    }
    if (num2 > 0 && num3 > 0)
    {
      pivotTable.EndLocation = pivotSheet.Range[num2 - 1, num3 - 1];
      pivotTable.m_location = pivotSheet.Range[pivotTable.Location.Row, pivotTable.Location.Column, num2 - 1, num3 - 1];
    }
    pivotTable.FirstDataCol = pivotTable.RowFields.Count;
    pivotTable.FirstDataRow = pivotTable.ColumnFields.Count + 1 + (pivotTable.ColumnFields.Count <= 0 || pivotTable.DataFields.Count <= 1 ? 0 : 1);
    if (pivotTable.ColFieldsOrder.Count > 0)
      pivotTable.FirstHeaderRow = pivotTable.ColumnFields.Count > 0 ? 1 : 0;
    this.SetSubTotalRow(pivotTableLayout);
    this.SetExtendedFormat(pivotTableLayout);
    new PivotTableStyleRenderer((IWorksheet) this.pivotTableImple.Worksheet).DrawPivotBorder(pivotTableLayout, pivotTable.BuiltInStyle);
    this.SetPivotFormat(pivotTableLayout, pivotTable);
    pivotTable.PivotLayout = pivotTableLayout;
    if (pivotTable.Location.Row > num2 || pivotTable.Location.Column > num3)
      return;
    this.AutoFitPivotTable(pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column, num2 - 1, num3 - 1], (IPivotTable) pivotTable);
  }

  private void SetOutline(PivotTableImpl pivotTable, PivotEngine pivotEngine)
  {
    WorksheetImpl worksheet = pivotTable.Worksheet;
    string str = (string) null;
    for (int columnIndex1 = 0; columnIndex1 < this.pivotTableImple.RowFields.Count; ++columnIndex1)
    {
      int num = 0;
      for (int rowIndex1 = 0; rowIndex1 <= pivotEngine.RowCount - 1; ++rowIndex1)
      {
        if (pivotEngine[rowIndex1, columnIndex1] != null && pivotEngine[rowIndex1, columnIndex1].Value != null && pivotEngine[rowIndex1, columnIndex1].Value.ToString() != string.Empty)
        {
          if (pivotEngine[rowIndex1, columnIndex1].Value != null && pivotEngine[rowIndex1, columnIndex1].Value != (object) "x" && pivotEngine[rowIndex1, columnIndex1].Value.ToString() == "x")
            pivotEngine[rowIndex1, columnIndex1].Value = (object) "";
          if (str == pivotEngine[rowIndex1, columnIndex1].Value.ToString() || str == pivotEngine[rowIndex1, columnIndex1].Value.ToString() + "\u0083")
          {
            bool flag = true;
            if (columnIndex1 != 0)
            {
              for (int rowIndex2 = num + 1; rowIndex2 < rowIndex1; ++rowIndex2)
              {
                if (pivotEngine[rowIndex2, columnIndex1 - 1].CellType != PivotCellType.ExpanderCell)
                  flag = false;
              }
            }
            if (flag)
              pivotEngine[rowIndex1, columnIndex1].Value = (object) null;
            else
              num = rowIndex1;
          }
          else if (pivotEngine[rowIndex1, columnIndex1].Value != (object) "")
          {
            str = pivotEngine[rowIndex1, columnIndex1].Value.ToString();
            num = rowIndex1;
          }
        }
      }
    }
  }

  private void RenderTabularLayout(
    PivotEngine pivotEngine,
    IWorksheet pivotSheet,
    PivotTableImpl pivotTable)
  {
    this.pivotTableImple = pivotTable;
    if (pivotTable.PageFields.Count > 0)
    {
      int num = 2;
      for (int index = pivotTable.PageFields.Count - 1; index >= 0; --index)
      {
        pivotSheet[pivotTable.Location.Row - num, pivotTable.Location.Column].Value2 = (object) pivotTable.PageFields[index].Name;
        pivotSheet[pivotTable.Location.Row - num, pivotTable.Location.Column + 1].Value2 = (pivotTable.PageFields[index] as PivotFieldImpl).IsMultiSelected ? (object) "(Multiple Items)" : (pivotTable.PageFields[index].PivotFilters[0] == null ? (object) "(All)" : (object) pivotTable.PageFields[index].PivotFilters[0].Value1);
        pivotSheet[pivotTable.Location.Row - num, pivotTable.Location.Column + 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
        ++num;
      }
    }
    int num1 = pivotTable.Location.Row;
    int column1 = pivotTable.Location.Column;
    PivotTableLayout pivotTableLayout = new PivotTableLayout();
    PivotValueCollections valueCollections = new PivotValueCollections();
    int row1 = pivotTable.Location.Row;
    int column2 = pivotTable.Location.Column;
    this.extendedFormat = new ExtendedFormatImpl(pivotTable.Workbook.Application, (object) pivotTable.Workbook);
    PivotTableParts pivotTableParts1 = PivotTableParts.None;
    int index1 = 0;
    int num2 = 0;
    int num3 = 0;
    bool flag1 = false;
    List<int> intList = new List<int>();
    this.SetPivotTableOptions(pivotEngine, pivotTable);
    for (int rowIndex = 0; rowIndex < pivotEngine.RowCount; ++rowIndex)
    {
      bool flag2 = false;
      bool flag3 = false;
      for (int columnIndex1 = 0; columnIndex1 < pivotEngine.ColumnCount; ++columnIndex1)
      {
        if (pivotEngine[rowIndex, columnIndex1] != null)
        {
          if (pivotEngine[rowIndex, columnIndex1].Value != null && pivotEngine[rowIndex, columnIndex1].Value != (object) "x" && pivotEngine[rowIndex, columnIndex1].Value.ToString() == "x")
            pivotEngine[rowIndex, columnIndex1].Value = (object) "";
          if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TopLeftCell) != (PivotCellType) 0)
          {
            int num4 = pivotTable.Location.Row + pivotTable.ColumnFields.Count + (pivotTable.ColumnFields.Count <= 0 || pivotTable.DataFields.Count <= 1 ? 0 : 1);
            if (index1 + 1 <= pivotTable.RowFields.Count)
              pivotSheet[num4 + rowIndex, column1 + columnIndex1].Value2 = (object) pivotTable.Cache.CacheFields[pivotTable.RowFieldsOrder[index1]].Name;
            PivotTableParts partStyle = columnIndex1 != 0 ? PivotTableParts.WholeTable | PivotTableParts.HeaderRow : PivotTableParts.WholeTable | PivotTableParts.FirstColumn | PivotTableParts.HeaderRow | PivotTableParts.FirstHeaderCell;
            this.FillPivotValue(num4 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num4 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
            ++index1;
            num1 = pivotTable.Location.Row + (pivotTable.DataFields.Count > 0 ? 1 : 0);
            if (pivotTable.ColumnFields.Count == 0)
              num1 = pivotTable.Location.Row;
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0)
          {
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = !flag2 ? (columnIndex1 == pivotTable.RowFields.Count - 1 ? PivotTableParts.WholeTable : (columnIndex1 != 0 ? ((columnIndex1 + 1) % 2 != 0 ? PivotTableParts.RowSubHeading3 : PivotTableParts.RowSubHeading2) : PivotTableParts.RowSubHeading1)) : PivotTableParts.WholeTable | PivotTableParts.SubtotalRow1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
          {
            flag2 = true;
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = (object) (pivotEngine[rowIndex, columnIndex1].Value.ToString() + " Total");
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.SubtotalRow1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.HeaderCell) != (PivotCellType) 0)
          {
            PivotTableParts partStyle;
            if (flag2 && columnIndex1 + 1 == pivotTable.RowFields.Count)
              partStyle = PivotTableParts.WholeTable | PivotTableParts.SubtotalRow1;
            else if (flag2 && columnIndex1 + 1 < pivotTable.RowFields.Count)
            {
              partStyle = PivotTableParts.WholeTable | PivotTableParts.SubtotalRow1;
            }
            else
            {
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
              partStyle = PivotTableParts.WholeTable;
            }
            if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
            {
              if (pivotEngine[rowIndex, columnIndex1].Value != (object) string.Empty)
                pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = (object) (pivotEngine[rowIndex, columnIndex1].Value.ToString() + " Total");
              PivotTableParts pivotTableParts2 = PivotTableParts.WholeTable;
              if (columnIndex1 == 0)
                pivotTableParts2 |= PivotTableParts.FirstColumn;
              partStyle = pivotTableParts2 | PivotTableParts.GrandTotalRow;
              pivotTableParts1 = PivotTableParts.GrandTotalRow;
            }
            else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
              partStyle |= PivotTableParts.SubtotalRow1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
          {
            if (!flag2)
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = (object) (pivotEngine[rowIndex, columnIndex1].Value.ToString() + " Total");
            else if (columnIndex1 + 1 > pivotTable.RowFields.Count)
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.GrandTotalRow;
            flag2 = true;
            pivotTableParts1 = PivotTableParts.GrandTotalRow;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0)
          {
            if (!flag1 && pivotTable.DataFields.Count < 2)
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = (object) (pivotEngine[rowIndex, columnIndex1].Value.ToString() + " Total");
            else if (pivotTable.DataFields.Count >= 2 && pivotEngine[rowIndex, columnIndex1].CellRange != null)
            {
              int num5 = 0;
              for (int left = pivotEngine[rowIndex, columnIndex1].CellRange.Left; left <= pivotEngine[rowIndex, columnIndex1].CellRange.Right; ++left)
              {
                CoveredCellRange cellRange = pivotEngine[rowIndex, columnIndex1].CellRange;
                if (pivotTable.DataFields.Count >= 2 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
                  pivotSheet[num1 + rowIndex, column1 + columnIndex1 + num5].Value2 = (object) "Total";
                if (pivotTable.DataFields.Count > 1)
                  pivotSheet[num1 + rowIndex, column1 + columnIndex1 + num5].Value2 = (object) $"{pivotSheet[num1 + rowIndex, column1 + columnIndex1 + num5].Value2.ToString()} {pivotEngine[pivotEngine[rowIndex, columnIndex1].CellRange.Bottom + 1, left].Value.ToString()}";
                ++num5;
              }
              columnIndex1 += num5 - 1;
            }
            flag1 = true;
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | PivotTableParts.GrandTotalColumn;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0)
          {
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            flag3 = true;
            PivotTableParts pivotTableParts3 = PivotTableParts.WholeTable | PivotTableParts.HeaderRow;
            PivotTableParts partStyle = rowIndex != 0 ? ((rowIndex + 1) % 2 != 0 ? pivotTableParts3 | PivotTableParts.ColumnSubHeading3 : pivotTableParts3 | PivotTableParts.ColumnSubHeading2) : pivotTableParts3 | PivotTableParts.ColumnSubHeading1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
          {
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = (object) (pivotEngine[rowIndex, columnIndex1].Value.ToString() + " Total");
            intList.Add(columnIndex1);
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.HeaderRow;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.HeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0)
          {
            if (!flag3 && !intList.Contains(columnIndex1))
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | PivotTableParts.ColumnSubHeading1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ValueCell) != (PivotCellType) 0)
          {
            if (columnIndex1 != 0 || pivotTable.RowFields.Count <= 1)
              pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = !flag2 ? (columnIndex1 >= pivotTable.RowFields.Count - 1 ? PivotTableParts.WholeTable : (columnIndex1 != 0 ? ((columnIndex1 + 1) % 2 != 0 ? PivotTableParts.RowSubHeading3 : PivotTableParts.RowSubHeading2) : PivotTableParts.RowSubHeading1)) : PivotTableParts.WholeTable | PivotTableParts.SubtotalRow1;
            if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
            {
              if (pivotTableParts1 != PivotTableParts.None)
                partStyle = pivotTableParts1;
              else
                partStyle |= PivotTableParts.GrandTotalColumn;
            }
            if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
            {
              if (!intList.Contains(columnIndex1) || flag2)
                partStyle |= PivotTableParts.SubtotalRow1;
              else
                partStyle |= PivotTableParts.SubtotalColumn1;
            }
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.CalculationHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) == (PivotCellType) 0 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.GrandTotalCell) == (PivotCellType) 0)
          {
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = !(pivotTable.Options as PivotTableOptions).ShowGridDropZone || pivotTable.DataFields.Count > 1 ? pivotEngine[rowIndex, columnIndex1].Value : (object) "Total";
            PivotTableParts partStyle = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | PivotTableParts.ColumnSubHeading1;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
          else
          {
            pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2 = pivotEngine[rowIndex, columnIndex1].Value;
            PivotTableParts partStyle = PivotTableParts.WholeTable;
            this.FillPivotValue(num1 + rowIndex - row1, column1 + columnIndex1 - column2, pivotSheet[num1 + rowIndex, column1 + columnIndex1].Value2, pivotEngine[rowIndex, columnIndex1].CellType, pivotTableLayout, partStyle);
          }
        }
        num3 = column1 + columnIndex1;
      }
      num2 = num1 + rowIndex;
    }
    int row2 = pivotTable.Location.Row;
    int column3 = pivotTable.Location.Column;
    for (int index2 = 0; index2 <= pivotTableLayout.maxColumnCount; ++index2)
    {
      PivotTableParts partStyle = index2 != 0 ? PivotTableParts.WholeTable | PivotTableParts.HeaderRow : PivotTableParts.WholeTable | PivotTableParts.FirstColumn | PivotTableParts.HeaderRow;
      this.FillPivotValue(row2 - row1, column3++ - column2, (object) " ", PivotCellType.TopLeftCell, pivotTableLayout, partStyle);
    }
    if (pivotTable.ColumnFields.Count > 0 && pivotTable.RowFields.Count > 0 && pivotTable.DataFields.Count == 1)
      pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column].Value = pivotTable.DataFields[0].Name;
    int count = pivotTable.RowFields.Count;
    for (int index3 = 0; index3 < pivotTable.ColFieldsOrder.Count; ++index3)
    {
      int i = pivotTable.ColFieldsOrder[index3];
      if (i >= 0)
        pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column + count].Value = pivotTable.Cache.CacheFields[i].Name;
      else if (pivotTable.ColFieldsOrder.Count != 1 || pivotTable.ColFieldsOrder[0] >= 0)
        pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column + count].Value = "Values";
      ++count;
    }
    if (num2 > 0 && num3 > 0)
    {
      pivotTable.EndLocation = pivotSheet.Range[num2 - 1, num3 - 1];
      pivotTable.m_location = pivotSheet.Range[pivotTable.Location.Row, pivotTable.Location.Column, num2 - 1, num3 - 1];
    }
    pivotTable.FirstDataCol = pivotTable.RowFields.Count;
    pivotTable.FirstDataRow = pivotTable.ColumnFields.Count + 1 + (pivotTable.ColumnFields.Count <= 0 || pivotTable.DataFields.Count <= 1 ? 0 : 1);
    if (pivotTable.ColFieldsOrder.Count > 0)
      pivotTable.FirstHeaderRow = pivotTable.ColumnFields.Count > 0 ? 1 : 0;
    this.SetSubTotalRow(pivotTableLayout);
    this.SetExtendedFormat(pivotTableLayout);
    new PivotTableStyleRenderer((IWorksheet) this.pivotTableImple.Worksheet).DrawPivotBorder(pivotTableLayout, pivotTable.BuiltInStyle);
    this.SetTabularSpecificStyles(pivotTableLayout);
    this.SetPivotFormat(pivotTableLayout, pivotTable);
    pivotTable.PivotLayout = pivotTableLayout;
    if (pivotTable.Location.Row > num2 || pivotTable.Location.Column > num3)
      return;
    this.AutoFitPivotTable(pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column, num2 - 1, num3 - 1], (IPivotTable) pivotTable);
  }

  private void SetTabularSpecificStyles(PivotTableLayout layOut)
  {
    for (int colIndex = 0; colIndex <= layOut.maxColumnCount; ++colIndex)
    {
      for (int rowIndex = 0; rowIndex < layOut.maxRowCount; ++rowIndex)
      {
        if (layOut[rowIndex, colIndex].Value != null)
        {
          if ((layOut[rowIndex + 1, colIndex].PivotTablePartStyle & PivotTableParts.SubtotalRow1) == (PivotTableParts) 0 && (layOut[rowIndex + 1, colIndex].PivotTablePartStyle & PivotTableParts.SubtotalRow2) == (PivotTableParts) 0 && ((layOut[rowIndex, colIndex].PivotTablePartStyle & PivotTableParts.RowSubHeading1) != (PivotTableParts) 0 || (layOut[rowIndex, colIndex].PivotTablePartStyle & PivotTableParts.RowSubHeading2) != (PivotTableParts) 0))
            layOut[rowIndex, colIndex].XF.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = layOut[rowIndex, colIndex].XF.PatternColor;
          if ((layOut[rowIndex, colIndex].PivotTablePartStyle & PivotTableParts.SubtotalRow1) != (PivotTableParts) 0 || (layOut[rowIndex, colIndex].PivotTablePartStyle & PivotTableParts.SubtotalRow2) != (PivotTableParts) 0 || (layOut[rowIndex, colIndex].PivotTablePartStyle & PivotTableParts.SubtotalRow3) != (PivotTableParts) 0)
            layOut[rowIndex, colIndex].XF.Font.Bold = true;
        }
      }
    }
  }

  private void SetSubTotalRow(PivotTableLayout layout)
  {
    int num = 1;
    Dictionary<int, int> dictionary = new Dictionary<int, int>();
    for (int colIndex = 0; colIndex <= layout.maxColumnCount; ++colIndex)
    {
      bool flag = false;
      for (int index = 0; index <= layout.maxRowCount; ++index)
      {
        if (layout[index, colIndex].Value != null && (layout[index, colIndex].PivotTablePartStyle & PivotTableParts.SubtotalRow1) != (PivotTableParts) 0)
        {
          flag = true;
          if (dictionary.ContainsKey(index))
          {
            switch (dictionary[index])
            {
              case 1:
                layout[index, colIndex].PivotTablePartStyle |= PivotTableParts.SubtotalRow1;
                continue;
              case 2:
                layout[index, colIndex].PivotTablePartStyle = (layout[index, colIndex].PivotTablePartStyle | PivotTableParts.SubtotalRow2) & ~PivotTableParts.SubtotalRow1;
                continue;
              case 3:
                layout[index, colIndex].PivotTablePartStyle = (layout[index, colIndex].PivotTablePartStyle | PivotTableParts.SubtotalRow3) & ~PivotTableParts.SubtotalRow1;
                continue;
              default:
                continue;
            }
          }
          else
          {
            switch (num)
            {
              case 1:
                if (!dictionary.ContainsKey(index))
                  dictionary.Add(index, 1);
                layout[index, colIndex].PivotTablePartStyle |= PivotTableParts.SubtotalRow1;
                continue;
              case 2:
                if (!dictionary.ContainsKey(index))
                  dictionary.Add(index, 2);
                layout[index, colIndex].PivotTablePartStyle = (layout[index, colIndex].PivotTablePartStyle | PivotTableParts.SubtotalRow2) & ~PivotTableParts.SubtotalRow1;
                continue;
              default:
                if (!dictionary.ContainsKey(index))
                  dictionary.Add(index, 3);
                layout[index, colIndex].PivotTablePartStyle = (layout[index, colIndex].PivotTablePartStyle | PivotTableParts.SubtotalRow3) & ~PivotTableParts.SubtotalRow1;
                continue;
            }
          }
        }
      }
      if (flag && num != 3)
        ++num;
      else if (num == 3)
        num = 2;
    }
  }

  private void RenderCompactLayout(
    PivotEngine pivotEngine,
    IWorksheet pivotSheet,
    PivotTableImpl pivotTable)
  {
    string delimiter = new string('\u0083', 1);
    this.pivotTableImple = pivotTable;
    if (pivotTable.PageFields.Count > 0)
    {
      int num = 2;
      for (int index = pivotTable.PageFields.Count - 1; index >= 0; --index)
      {
        pivotSheet[pivotTable.Location.Row - num, pivotTable.Location.Column].Value2 = (object) pivotTable.PageFields[index].Name;
        pivotSheet[pivotTable.Location.Row - num, pivotTable.Location.Column + 1].Value2 = (pivotTable.PageFields[index] as PivotFieldImpl).IsMultiSelected ? (object) "(Multiple Items)" : (pivotTable.PageFields[index].PivotFilters[0] == null ? (object) "(All)" : (object) pivotTable.PageFields[index].PivotFilters[0].Value1);
        pivotSheet[pivotTable.Location.Row - num, pivotTable.Location.Column + 1].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
        ++num;
      }
    }
    int num1 = 0;
    int row1 = pivotTable.Location.Row;
    int num2 = row1 + 1;
    if (pivotTable.ColumnFields.Count <= 0)
      num2 = row1;
    int column1 = pivotTable.Location.Column;
    bool flag1 = true;
    if (pivotTable.RowFields.Count <= 0)
      flag1 = false;
    int num3 = 0;
    if (pivotTable.RowFields.Count > 0)
      num3 = -(pivotTable.RowFields.Count - 1);
    if (pivotTable.RowFields.Count == 0 && pivotTable.DataFields.Count > 1)
      --num3;
    int column2 = pivotTable.Location.Column;
    if (pivotTable.ColumnFields.Count > 0 && pivotTable.RowFields.Count >= 1 || pivotTable.DataFields.Count == 1 && pivotTable.ColumnFields.Count > 0)
    {
      pivotSheet[row1, column2 + 1].Value2 = pivotTable.Options.DisplayFieldCaptions ? (string.IsNullOrEmpty(pivotTable.Options.ColumnHeaderCaption) ? (object) "Column Labels" : (object) pivotTable.Options.ColumnHeaderCaption) : (object) string.Empty;
      pivotSheet[row1, column2 + 2].Value2 = (object) " ";
      num1 = column2 + 1;
      ++row1;
    }
    else if (pivotTable.ColumnFields.Count > 0)
    {
      pivotSheet[row1, column2].Value2 = pivotTable.Options.DisplayFieldCaptions ? (string.IsNullOrEmpty(pivotTable.Options.ColumnHeaderCaption) ? (object) "Column Labels" : (object) pivotTable.Options.ColumnHeaderCaption) : (object) string.Empty;
      pivotSheet[row1, column2 + 1].Value2 = (object) " ";
      num1 = column2;
    }
    PivotTableLayout pivotTableLayout = new PivotTableLayout();
    PivotValueCollections valueCollections = new PivotValueCollections();
    int row2 = pivotTable.Location.Row;
    int column3 = pivotTable.Location.Column;
    this.extendedFormat = new ExtendedFormatImpl(pivotTable.Workbook.Application, (object) pivotTable.Workbook);
    int row3 = pivotTable.Location.Row;
    int num4 = 0;
    string str = (string) null;
    int num5 = -1;
    int expanderCount = -1;
    bool flag2 = false;
    PivotTableParts partStyle1 = PivotTableParts.None;
    PivotTableParts pivotTableParts1 = PivotTableParts.None;
    PivotTableParts partStyle2 = PivotTableParts.WholeTable;
    List<int> intList = new List<int>();
    int num6 = pivotTable.RowGrand ? pivotEngine.RowCount : (pivotEngine.RowCount > 2 ? pivotEngine.RowCount - 2 : pivotEngine.RowCount);
    if (pivotEngine.PivotValues != null)
    {
      for (int rowIndex1 = 0; rowIndex1 < num6; ++rowIndex1)
      {
        ++expanderCount;
        for (int columnIndex1_1 = 0; columnIndex1_1 < pivotEngine.ColumnCount; ++columnIndex1_1)
        {
          if (pivotEngine[rowIndex1, columnIndex1_1] != null)
          {
            if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.ColumnHeaderCell) == (PivotCellType) 0 && pivotTable.RowFields.Count > 0)
            {
              if (pivotTable.ColumnFields.Count == 0 && flag1)
              {
                pivotSheet[row1, column2].Value2 = pivotTable.Options.DisplayFieldCaptions ? (string.IsNullOrEmpty(pivotTable.Options.RowHeaderCaption) ? (object) "Row Labels" : (object) pivotTable.Options.RowHeaderCaption) : (object) string.Empty;
                if (pivotEngine[rowIndex1, columnIndex1_1 + 1] != null)
                  ++row1;
                flag1 = false;
                this.FillPivotValue(row1 - row2, column2 - column3, pivotTable.Options.DisplayFieldCaptions ? (string.IsNullOrEmpty(pivotTable.Options.RowHeaderCaption) ? (object) "Row Labels" : (object) pivotTable.Options.RowHeaderCaption) : (object) string.Empty, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, PivotTableParts.WholeTable);
              }
              if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.TopLeftCell) != (PivotCellType) 0)
              {
                pivotSheet[row1, column2].Value2 = (object) (pivotSheet[row1, column2].Value2.ToString() + pivotEngine[rowIndex1, columnIndex1_1].Value);
                partStyle2 = PivotTableParts.WholeTable | PivotTableParts.FirstColumn | PivotTableParts.HeaderRow | PivotTableParts.FirstHeaderCell;
                this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                if (pivotEngine[rowIndex1, columnIndex1_1].Value != null)
                {
                  pivotSheet[row1, column2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                  pivotSheet[row1, column2].CellStyle.IndentLevel = columnIndex1_1;
                }
              }
              else if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0)
              {
                if (flag2)
                {
                  num5 = -1;
                  flag2 = false;
                }
                ++num5;
                ++row1;
                column2 = pivotTable.Location.Column;
                if (pivotEngine[rowIndex1, columnIndex1_1].ParentCell == null)
                {
                  pivotSheet[row1, column2].Value2 = pivotEngine[rowIndex1, columnIndex1_1].Value;
                  if (columnIndex1_1 != pivotTable.RowFields.Count - 1)
                  {
                    PivotTableParts pivotTableParts2 = PivotTableParts.WholeTable | PivotTableParts.FirstColumn;
                    partStyle2 = columnIndex1_1 != 0 ? ((columnIndex1_1 + 1) % 2 != 0 ? pivotTableParts2 | PivotTableParts.RowSubHeading3 : pivotTableParts2 | PivotTableParts.RowSubHeading2) : pivotTableParts2 | PivotTableParts.RowSubHeading1;
                  }
                  this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                  partStyle1 = partStyle2 & ~PivotTableParts.FirstColumn;
                }
                pivotSheet[row1, column2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                pivotSheet[row1, column2].CellStyle.IndentLevel = columnIndex1_1;
                int rowIndex2 = rowIndex1;
                while ((pivotEngine[rowIndex2, columnIndex1_1].CellType & PivotCellType.TotalCell) == (PivotCellType) 0)
                  ++rowIndex2;
                int column4 = column2 + 1;
                for (int columnIndex1_2 = columnIndex1_1; columnIndex1_2 < pivotEngine.ColumnCount; ++columnIndex1_2)
                {
                  if (pivotEngine[rowIndex2, columnIndex1_2] != null && (pivotEngine[rowIndex2, columnIndex1_2].CellType & PivotCellType.ValueCell) != (PivotCellType) 0 && columnIndex1_2 > pivotTable.RowFields.Count - 1)
                  {
                    if (pivotEngine[rowIndex2, columnIndex1_2].ParentCell == null && pivotEngine[rowIndex2, columnIndex1_2].Value != null)
                    {
                      pivotSheet[row1, column4].Value2 = pivotEngine[rowIndex2, columnIndex1_2].Value;
                      if (columnIndex1_2 != 0)
                        partStyle2 &= ~PivotTableParts.FirstColumn;
                      if ((pivotEngine[rowIndex2, columnIndex1_2].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
                        partStyle2 |= PivotTableParts.SubtotalColumn1;
                      if (!intList.Contains(column4))
                        partStyle2 &= ~PivotTableParts.SubtotalColumn1;
                      if ((pivotEngine[rowIndex2, columnIndex1_2].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
                        partStyle2 |= PivotTableParts.GrandTotalColumn;
                      this.FillPivotValue(row1 - row2, column4 - column3, pivotSheet[row1, column4].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                      partStyle1 = partStyle2;
                    }
                    ++column4;
                  }
                }
              }
              else if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.TotalCell) == (PivotCellType) 0)
              {
                ++row1;
                column2 = pivotTable.Location.Column;
                pivotSheet[row1, column2].Value2 = pivotEngine[rowIndex1, columnIndex1_1].Value;
                this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, PivotTableParts.WholeTable | PivotTableParts.FirstColumn);
                partStyle1 = PivotTableParts.WholeTable;
                flag2 = true;
                if (pivotEngine[rowIndex1, columnIndex1_1].Value != null)
                {
                  pivotSheet[row1, column2].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                  if (columnIndex1_1 != 0 && pivotSheet[row1, column2].CellStyle.IndentLevel < columnIndex1_1)
                    pivotSheet[row1, column2].CellStyle.IndentLevel = columnIndex1_1;
                }
                if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
                {
                  pivotSheet[row1, column2].Value2 = (object) (this.ReplaceDelimiter(pivotSheet[row1, column2].Value2, delimiter, " ").ToString() + "Total");
                  PivotTableParts pivotTableParts3 = PivotTableParts.WholeTable;
                  if (columnIndex1_1 == 0)
                    pivotTableParts3 |= PivotTableParts.FirstColumn;
                  partStyle2 = pivotTableParts3 | PivotTableParts.GrandTotalRow;
                  this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                  partStyle1 = PivotTableParts.WholeTable | PivotTableParts.GrandTotalRow;
                  columnIndex1_1 = pivotTable.RowFields.Count - 1;
                  pivotSheet[row1, column2].CellStyle.IndentLevel = 0;
                }
              }
              else if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.ValueCell) != (PivotCellType) 0)
              {
                if (columnIndex1_1 > pivotTable.RowFields.Count - 1)
                {
                  ++column2;
                  if (pivotTable.RowFields.Count == 0)
                  {
                    if (pivotEngine[rowIndex1, columnIndex1_1].Value != null && pivotEngine[rowIndex1, columnIndex1_1].Value.ToString() != "0")
                    {
                      pivotSheet[row1, column2 - 1].Value2 = pivotEngine[rowIndex1, columnIndex1_1].Value;
                      this.FillPivotValue(row1 - row2, column2 - (1 + column3), pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle1);
                    }
                  }
                  else if (pivotEngine[rowIndex1, columnIndex1_1].Value != null)
                  {
                    pivotSheet[row1, column2].Value2 = pivotEngine[rowIndex1, columnIndex1_1].Value;
                    if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
                    {
                      PivotTableParts partStyle3 = rowIndex1 + 2 == pivotEngine.RowCount ? (columnIndex1_1 + 2 == pivotEngine.ColumnCount ? partStyle1 | PivotTableParts.GrandTotalColumn : partStyle1) : partStyle1 | PivotTableParts.GrandTotalColumn;
                      this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle3);
                    }
                    else
                    {
                      if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
                        partStyle1 |= PivotTableParts.SubtotalColumn1;
                      this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle1);
                      partStyle1 &= ~PivotTableParts.SubtotalColumn1;
                    }
                    if (pivotEngine[rowIndex1, columnIndex1_1].Format != "General")
                      pivotSheet[row1, column2].CellStyle.NumberFormat = pivotEngine[rowIndex1, columnIndex1_1].Format;
                  }
                  else
                    this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle1);
                }
              }
              else if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
                columnIndex1_1 = pivotEngine.ColumnCount;
              if (column2 > num1)
              {
                num1 = column2;
                if (pivotTable.RowFields.Count == 1 && pivotTable.DataFields.Count == 0 && pivotTable.ColumnFields.Count == 0)
                  num1 = 1;
              }
              if (row1 > num4)
                num4 = row1;
            }
            else
            {
              row1 = rowIndex1 + num2;
              column2 = columnIndex1_1 + column1 + num3;
              if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.ValueCell) == (PivotCellType) 0 && pivotEngine[rowIndex1, columnIndex1_1 + 1] != null)
                num4 = row1;
              if (column2 > num1)
                num1 = column2;
              if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0 && pivotEngine[rowIndex1, columnIndex1_1 + 1] != null && pivotTable.DataFields.Count == 1 && (pivotEngine[rowIndex1, columnIndex1_1 + 1].CellType & PivotCellType.ValueCell) != (PivotCellType) 0)
              {
                pivotSheet[row1, column2].Value2 = (object) pivotTable.DataFields[0].Name;
                partStyle2 = PivotTableParts.WholeTable | PivotTableParts.FirstColumn | PivotTableParts.FirstHeaderCell;
                this.FillPivotValue(row1 - row2, column2 - column3, (object) pivotTable.DataFields[0].Name, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
              }
              if (pivotEngine[rowIndex1, columnIndex1_1].ParentCell == null && pivotEngine[rowIndex1, columnIndex1_1].Value != null)
              {
                if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.TopLeftCell) != (PivotCellType) 0)
                {
                  pivotSheet[row1, column2].Value2 = (object) (pivotSheet[row1, column2].Value2.ToString() + pivotEngine[rowIndex1, columnIndex1_1].Value);
                  partStyle2 = PivotTableParts.WholeTable | PivotTableParts.FirstColumn | PivotTableParts.FirstHeaderCell;
                  this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                  if (pivotEngine[rowIndex1, columnIndex1_1].Value != null)
                  {
                    str = pivotEngine[rowIndex1, columnIndex1_1].Value.ToString();
                    pivotTable.FirstDataCol = 0;
                  }
                }
                else if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0)
                {
                  pivotSheet[row1, column2].Value2 = pivotEngine[rowIndex1, columnIndex1_1].Value;
                  str = pivotEngine[rowIndex1, columnIndex1_1].Value.ToString();
                  partStyle2 = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | this.GetColumnHeading(expanderCount);
                  this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                  pivotTableParts1 = partStyle2;
                }
                else if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.HeaderCell) != (PivotCellType) 0 && pivotEngine[rowIndex1, columnIndex1_1].Value.ToString() != str && (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) == (PivotCellType) 0)
                {
                  pivotSheet[row1, column2].Value2 = pivotEngine[rowIndex1, columnIndex1_1].Value;
                  partStyle2 = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | this.GetColumnHeading(expanderCount);
                  this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                  pivotTableParts1 = partStyle2;
                }
                else if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.CalculationHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.TotalCell) == (PivotCellType) 0 && (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) == (PivotCellType) 0)
                {
                  pivotSheet[row1, column2].Value2 = pivotEngine[rowIndex1, columnIndex1_1].Value;
                  partStyle2 = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | this.GetColumnHeading(expanderCount);
                  this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                  pivotTableParts1 = partStyle2;
                  ++expanderCount;
                }
                else if (((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0 || (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0) && pivotTable.ColumnGrand)
                {
                  if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.CalculationHeaderCell) == (PivotCellType) 0)
                  {
                    int column5 = column2;
                    int num7 = columnIndex1_1;
                    if (pivotEngine[rowIndex1, columnIndex1_1].CellRange != null)
                    {
                      int num8 = row1;
                      for (int left = pivotEngine[rowIndex1, columnIndex1_1].CellRange.Left; left <= pivotEngine[rowIndex1, columnIndex1_1].CellRange.Right; ++left)
                      {
                        CoveredCellRange cellRange = pivotEngine[rowIndex1, columnIndex1_1].CellRange;
                        pivotSheet[row1, column5].Value2 = pivotTable.DataFields.Count < 2 || (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) == (PivotCellType) 0 ? pivotEngine[rowIndex1, columnIndex1_1].Value : (object) "Total";
                        if (pivotTable.DataFields.Count > 1)
                          pivotSheet[row1, column5].Value2 = (object) $"{pivotSheet[row1, column5].Value2.ToString()} {pivotEngine[pivotEngine[rowIndex1, columnIndex1_1].CellRange.Bottom + 1, num7 + num3].Value.ToString()}";
                        else if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0 || (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
                          pivotSheet[row1, column5].Value2 = (object) (this.ReplaceDelimiter(pivotEngine[rowIndex1, columnIndex1_1].Value, delimiter, " ").ToString() + "Total");
                        for (int top = cellRange.Top; top <= cellRange.Bottom; ++top)
                        {
                          partStyle2 = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | this.GetColumnHeading(expanderCount) | PivotTableParts.SubtotalColumn1;
                          if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
                            partStyle2 &= ~PivotTableParts.SubtotalColumn1;
                          if (top == cellRange.Top)
                            this.FillPivotValue(num8++ - row2, column5 - column3, pivotSheet[row1, column5].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                          else
                            this.FillPivotValue(num8++ - row2, column5 - column3, (object) "", pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                          if (!intList.Contains(column5))
                            intList.Add(column5);
                        }
                        pivotTableParts1 = partStyle2;
                        ++column5;
                        ++num7;
                      }
                    }
                    else
                    {
                      pivotSheet[row1, column2].Value2 = pivotEngine[rowIndex1, columnIndex1_1].Value;
                      if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0 && (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.ValueCell) == (PivotCellType) 0)
                        pivotSheet[row1, column2].Value2 = (object) (this.ReplaceDelimiter(pivotSheet[row1, column2].Value2, delimiter, " ").ToString() + "Total");
                      partStyle2 = PivotTableParts.WholeTable | PivotTableParts.HeaderRow;
                      this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, pivotTableParts1 | PivotTableParts.GrandTotalColumn);
                    }
                  }
                }
                else if ((pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.HeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0 && (pivotEngine[rowIndex1, columnIndex1_1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
                {
                  pivotSheet[row1, column2].Value2 = pivotEngine[rowIndex1, columnIndex1_1].Value;
                  partStyle2 = PivotTableParts.WholeTable | PivotTableParts.HeaderRow | this.GetColumnHeading(expanderCount);
                  this.FillPivotValue(row1 - row2, column2 - column3, pivotSheet[row1, column2].Value2, pivotEngine[rowIndex1, columnIndex1_1].CellType, pivotTableLayout, partStyle2);
                }
              }
            }
          }
        }
      }
    }
    else if (pivotEngine.PivotColumns.Count == 0 && pivotEngine.PivotCalculations != null && pivotEngine.PivotCalculations.Count > 0 && pivotEngine.PivotRows.Count == 1)
    {
      if (num4 == 0)
        num4 = row1;
      if (num1 == 0)
        num1 = column2;
      if (pivotTable.RowFields.Count > 0)
        ++num1;
      foreach (PivotComputationInfo pivotCalculation in pivotEngine.PivotCalculations)
      {
        pivotSheet[num4, num1++].Value2 = (object) pivotCalculation.FieldHeader;
        PivotTableParts partStyle4 = PivotTableParts.WholeTable | PivotTableParts.HeaderRow;
        this.FillPivotValue(num4 - row2, num1 - column3, pivotSheet[num4, num1].Value2, PivotCellType.CalculationHeaderCell, pivotTableLayout, partStyle4);
      }
    }
    if (pivotEngine.RowCount == 1 && pivotEngine.ColumnCount == 1 && pivotTable.ColumnGrand && pivotTable.RowGrand)
    {
      pivotSheet[num4++, num1].Value2 = (object) "Grand Total";
      pivotSheet[num4, num1 - 1].Value2 = (object) "Grand Total";
    }
    int row4 = pivotTable.Location.Row;
    int column6 = pivotTable.Location.Column;
    for (int index = 0; index <= pivotTableLayout.maxColumnCount; ++index)
    {
      PivotTableParts partStyle5 = index != 0 ? PivotTableParts.WholeTable | PivotTableParts.HeaderRow : PivotTableParts.WholeTable | PivotTableParts.FirstColumn | PivotTableParts.HeaderRow;
      this.FillPivotValue(row4 - row2, column6++ - column3, (object) " ", PivotCellType.TopLeftCell, pivotTableLayout, partStyle5);
    }
    if (pivotTable.RowFields.Count > 0)
    {
      int row5 = pivotTable.ColumnFields.Count + row3;
      if (pivotTable.DataFields.Count > 1 && pivotTable.ColumnFields.Count > 0)
        ++row5;
      pivotSheet[row5, pivotTable.Location.Column].Value = pivotTable.Options.DisplayFieldCaptions ? (string.IsNullOrEmpty(pivotTable.Options.RowHeaderCaption) ? "Row Labels" : pivotTable.Options.RowHeaderCaption) : string.Empty;
      this.FillPivotValue(row5 - row2, pivotTable.Location.Column - column3, (object) " ", PivotCellType.TopLeftCell, pivotTableLayout, PivotTableParts.WholeTable | PivotTableParts.FirstColumn | PivotTableParts.HeaderRow);
    }
    if (pivotTable.RowFields.Count == 0 && pivotTable.DataFields.Count > 1)
      pivotTable.FirstDataCol = 0;
    if (pivotTable.ColumnFields.Count > 0 && pivotTable.RowFields.Count > 0 && pivotTable.DataFields.Count == 1)
      pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column].Value = pivotTable.DataFields[0].Name;
    if (num4 > 0 && num1 > 0)
    {
      pivotTable.EndLocation = pivotSheet.Range[num4, num1];
      pivotTable.m_location = pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column, num4, num1];
    }
    if (pivotTable.ColFieldsOrder.Count > 0)
      pivotTable.FirstHeaderRow = pivotTable.ColumnFields.Count > 0 ? 1 : 0;
    this.SetSubtotalColumn(pivotTableLayout);
    this.SetExtendedFormat(pivotTableLayout);
    new PivotTableStyleRenderer((IWorksheet) this.pivotTableImple.Worksheet).DrawPivotBorder(pivotTableLayout, pivotTable.BuiltInStyle);
    this.SetPivotFormat(pivotTableLayout, pivotTable);
    pivotTable.PivotLayout = pivotTableLayout;
    if (pivotTable.Location.Row > num4 || pivotTable.Location.Column > num1)
      return;
    this.AutoFitPivotTable(pivotSheet[pivotTable.Location.Row, pivotTable.Location.Column, num4, num1], (IPivotTable) pivotTable);
  }

  public object ReplaceDelimiter(object replacableValue, string delimiter, string newValue)
  {
    return (object) replacableValue.ToString().Replace(delimiter, newValue);
  }

  public void AutoFitPivotTable(IRange range, IPivotTable pivotTable)
  {
    int num1 = pivotTable.DataFields.Count > 1 ? pivotTable.ColumnFields.Count + 2 : pivotTable.ColumnFields.Count + 1;
    string empty = string.Empty;
    IRange range1 = range.Worksheet[range.Row, range.Column];
    for (int column = range.Column; column <= range.LastColumn; ++column)
    {
      string str1 = string.Empty;
      for (int row = range.Row; row <= range.LastRow; ++row)
      {
        string str2 = range.Worksheet[row, column].Value;
        if (str2.Length > str1.Length)
        {
          range1 = range.Worksheet[row, column];
          str1 = str2;
        }
      }
      range1.AutofitColumns();
      double num2 = range1.ColumnWidth;
      if (range1.IndentLevel > 0)
      {
        if (range.Row + num1 < range1.Row)
          num2 = num2 + (double) range1.IndentLevel + 1.85;
        else
          num2 += 1.85;
      }
      range1.ColumnWidth = num2;
    }
  }

  private void FillPivotValue(
    int rowIndex,
    int columnIndex,
    object value,
    PivotCellType cellType,
    PivotTableLayout layOut,
    PivotTableParts partStyle)
  {
    PivotValueCollections valueCollections = new PivotValueCollections();
    valueCollections.Value = value.ToString();
    PivotTableStyleRenderer tableStyleRenderer = new PivotTableStyleRenderer((IWorksheet) this.pivotTableImple.Worksheet);
    valueCollections.PivotTablePartStyle = partStyle;
    layOut[rowIndex, columnIndex] = valueCollections;
  }

  private void SetSubtotalColumn(PivotTableLayout layout)
  {
    bool flag = true;
    PivotTableParts pivotTableParts = PivotTableParts.SubtotalColumn2;
    int num = 0;
    List<int> intList = new List<int>();
    int rowIndex = 0;
    for (int maxRowCount = layout.maxRowCount; rowIndex <= maxRowCount; ++rowIndex)
    {
      int colIndex = 0;
      for (int index = layout[rowIndex].Count - 1; colIndex <= index; ++colIndex)
      {
        if ((layout[rowIndex, colIndex].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0)
        {
          if (flag)
          {
            num = rowIndex;
            flag = false;
          }
          if (num == rowIndex && layout[rowIndex, colIndex].Value != null)
            intList.Add(colIndex);
          if (num != rowIndex)
          {
            if (!intList.Contains(colIndex))
            {
              layout[rowIndex, colIndex].PivotTablePartStyle &= ~PivotTableParts.SubtotalColumn1;
              layout[rowIndex, colIndex].PivotTablePartStyle |= pivotTableParts;
              pivotTableParts = pivotTableParts != PivotTableParts.SubtotalColumn2 ? PivotTableParts.SubtotalColumn2 : PivotTableParts.SubtotalColumn3;
            }
          }
          else if ((layout[rowIndex, colIndex].PivotTablePartStyle & PivotTableParts.SubtotalColumn1) != (PivotTableParts) 0 && !intList.Contains(colIndex))
            layout[rowIndex, colIndex].PivotTablePartStyle &= ~PivotTableParts.SubtotalColumn1;
        }
      }
    }
  }

  private void SetExtendedFormat(PivotTableLayout layout)
  {
    PivotTableStyleRenderer tableStyleRenderer = new PivotTableStyleRenderer((IWorksheet) this.pivotTableImple.Worksheet);
    Dictionary<int, ExtendedFormatImpl> dictionary = new Dictionary<int, ExtendedFormatImpl>();
    int rowIndex = 0;
    for (int maxRowCount = layout.maxRowCount; rowIndex <= maxRowCount; ++rowIndex)
    {
      int colIndex = 0;
      for (int index = layout[rowIndex].Count - 1; colIndex <= index; ++colIndex)
      {
        if (dictionary.ContainsKey((int) layout[rowIndex, colIndex].PivotTablePartStyle))
        {
          layout[rowIndex, colIndex].XF = (ExtendedFormatImpl) dictionary[(int) layout[rowIndex, colIndex].PivotTablePartStyle].Clone();
        }
        else
        {
          ExtendedFormatImpl extendedFormatImpl = tableStyleRenderer.ApplyStyles(this.pivotTableImple.BuiltInStyle, layout[rowIndex, colIndex].PivotTablePartStyle);
          dictionary.Add((int) layout[rowIndex, colIndex].PivotTablePartStyle, extendedFormatImpl);
          layout[rowIndex, colIndex].XF = extendedFormatImpl;
        }
      }
    }
    dictionary.Clear();
  }

  private void SetPivotFormat(PivotTableLayout layout, PivotTableImpl pivotTable)
  {
    PivotTableStyleRenderer tableStyleRenderer = new PivotTableStyleRenderer((IWorksheet) this.pivotTableImple.Worksheet);
    Dictionary<long, ExtendedFormatImpl> dictionary = pivotTable.ApplyPivotFormats(layout);
    int rowIndex = 0;
    for (int maxRowCount = layout.maxRowCount; rowIndex <= maxRowCount; ++rowIndex)
    {
      int colIndex = 0;
      for (int index = layout[rowIndex].Count - 1; colIndex <= index; ++colIndex)
      {
        long cellIndex = RangeImpl.GetCellIndex(colIndex + 1, rowIndex + 1);
        if (dictionary.ContainsKey(cellIndex))
          layout[rowIndex, colIndex].XF = dictionary[cellIndex];
      }
    }
    dictionary.Clear();
  }

  private PivotTableParts GetRowHeading(int expanderCount)
  {
    if (expanderCount % 3 == 0)
      return PivotTableParts.RowSubHeading1;
    if (expanderCount % 3 == 1)
      return PivotTableParts.RowSubHeading2;
    return expanderCount % 3 == 2 ? PivotTableParts.RowSubHeading3 : PivotTableParts.RowSubHeading1;
  }

  private PivotTableParts GetColumnHeading(int expanderCount)
  {
    if (expanderCount % 3 == 0)
      return PivotTableParts.ColumnSubHeading1;
    if (expanderCount % 3 == 1)
      return PivotTableParts.ColumnSubHeading2;
    return expanderCount % 3 == 2 ? PivotTableParts.ColumnSubHeading3 : PivotTableParts.ColumnSubHeading1;
  }

  public class CustomComparer : IComparer
  {
    public int Compare(object a, object b)
    {
      if (a == b)
        return 0;
      if (a == null)
        return -1;
      if (b == null)
        return 1;
      double result1;
      bool flag1 = double.TryParse(a.ToString(), out result1);
      double result2;
      bool flag2 = double.TryParse(b.ToString(), out result2);
      DateTime result3;
      bool flag3 = DateTime.TryParse(a.ToString(), out result3);
      DateTime result4;
      bool flag4 = DateTime.TryParse(b.ToString(), out result4);
      bool flag5 = int.TryParse(a.ToString(), out int _);
      bool flag6 = int.TryParse(b.ToString(), out int _);
      if (flag1 && flag2)
        return result1.CompareTo(result2);
      if (flag3 && flag4)
      {
        int num = DateTime.Compare(result3, result4);
        if (num == 0)
          return 0;
        return num <= 0 ? -1 : 1;
      }
      if (flag5 && flag6)
      {
        if (int.Parse(b.ToString()) == int.Parse(a.ToString()))
          return 0;
        return int.Parse(b.ToString()) <= int.Parse(a.ToString()) ? -1 : 1;
      }
      return a is IComparable comparable ? comparable.CompareTo(b) : throw new ArgumentException("Argument_ImplementIComparable");
    }
  }
}
