// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.PivotTables.PivotTableSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.PivotAnalysis;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization.PivotTables;

internal class PivotTableSerializator
{
  private const int DataFieldsIndex = -2;
  private const string PercentageIndexFormat = "10";

  public static void SerializePivotTable(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    WorksheetImpl worksheetImpl = pivotTable != null ? pivotTable.Worksheet : throw new ArgumentNullException(nameof (pivotTable));
    if (worksheetImpl.PreservePivotTables.Count > 0)
    {
      Stream preservePivotTable = worksheetImpl.PreservePivotTables[0];
      preservePivotTable.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, preservePivotTable);
      worksheetImpl.PreservePivotTables.RemoveAt(0);
    }
    else
    {
      PivotTableOptions options = pivotTable.Options as PivotTableOptions;
      writer.WriteStartElement("pivotTableDefinition", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
      writer.WriteAttributeString("name", pivotTable.Name);
      writer.WriteAttributeString("cacheId", pivotTable.CacheIndex.ToString());
      PivotTableSerializator.SerializeAttributeString(writer, "applyNumberFormats", options.IsNumberAutoFormat, true);
      PivotTableSerializator.SerializeAttributeString(writer, "applyBorderFormats", options.IsBorderAutoFormat, true);
      PivotTableSerializator.SerializeAttributeString(writer, "applyFontFormats", options.IsFontAutoFormat, true);
      PivotTableSerializator.SerializeAttributeString(writer, "applyPatternFormats", options.IsPatternAutoFormat, true);
      PivotTableSerializator.SerializeAttributeString(writer, "applyAlignmentFormats", options.IsAlignAutoFormat, true);
      PivotTableSerializator.SerializeAttributeString(writer, "applyWidthHeightFormats", options.IsWHAutoFormat, false);
      Excel2007Serializator.SerializeAttribute(writer, "colGrandTotals", pivotTable.ColumnGrand, true);
      Excel2007Serializator.SerializeAttribute(writer, "rowGrandTotals", pivotTable.RowGrand, true);
      Excel2007Serializator.SerializeAttribute(writer, "fieldPrintTitles", options.PrintTitles, false);
      string columnHeaderCaption = options.ColumnHeaderCaption;
      if (columnHeaderCaption != null && columnHeaderCaption.Length > 0)
        writer.WriteAttributeString("colHeaderCaption", options.ColumnHeaderCaption);
      string rowHeaderCaption = options.RowHeaderCaption;
      if (rowHeaderCaption != null && rowHeaderCaption.Length > 0)
        writer.WriteAttributeString("rowHeaderCaption", options.RowHeaderCaption);
      if (options.RowLayout == PivotTableRowLayout.Compact)
      {
        writer.WriteAttributeString("outline", "1");
        writer.WriteAttributeString("outlineData", "1");
      }
      else if (options.RowLayout == PivotTableRowLayout.Outline)
      {
        writer.WriteAttributeString("compact", "0");
        writer.WriteAttributeString("compactData", "0");
        writer.WriteAttributeString("outline", "1");
        writer.WriteAttributeString("outlineData", "1");
      }
      else
      {
        writer.WriteAttributeString("compact", "0");
        writer.WriteAttributeString("compactData", "0");
      }
      PivotTableSerializator.SerializeAttributeString(writer, "createdVersion", options.CreatedVersion);
      PivotTableSerializator.SerializeAttributeString(writer, "updatedVersion", options.UpdatedVersion);
      options.MiniRefreshVersion = (byte) 3;
      PivotTableSerializator.SerializeAttributeString(writer, "minRefreshableVersion", options.MiniRefreshVersion);
      PivotTableSerializator.SerializeAttributeString(writer, "customListSort", options.ShowCustomSortList, true);
      writer.WriteAttributeString("dataCaption", options.DataCaption);
      if (options.GrandTotalCaption != null)
        writer.WriteAttributeString("grandTotalCaption", options.GrandTotalCaption);
      PivotTableSerializator.SerializeAttributeString(writer, "dataOnRows", pivotTable.ShowDataFieldInRow, false);
      if (options.DataPosition > (ushort) 0)
        PivotTableSerializator.SerializeAttributeString(writer, "dataPosition", options.DataPosition);
      PivotTableSerializator.SerializeAttributeString(writer, "editData", options.IsDataEditable, false);
      PivotTableSerializator.SerializeAttributeString(writer, "enableDrill", pivotTable.EnableDrilldown, false);
      PivotTableSerializator.SerializeAttributeString(writer, "enableFieldProperties", options.EnableFieldProperties, false);
      PivotTableSerializator.SerializeAttributeString(writer, "enableWizard", pivotTable.EnableWizard, false);
      Excel2007Serializator.SerializeAttribute(writer, "asteriskTotals", options.ShowAsteriskTotals, false);
      Excel2007Serializator.SerializeAttribute(writer, "mergeItem", options.MergeLabels, false);
      PivotTableSerializator.SerializeAttributeString(writer, "showCalcMbrs", options.ShowCalcMembers, false);
      Excel2007Serializator.SerializeAttribute(writer, "showDrill", pivotTable.ShowDrillIndicators, true);
      PivotTableSerializator.SerializeAttributeString(writer, "useAutoFormatting", options.IsAutoFormat, false);
      PivotTableSerializator.SerializeAttributeString(writer, "showDataTips", options.ShowTooltips, true);
      Excel2007Serializator.SerializeAttribute(writer, "itemPrintTitles", pivotTable.RepeatItemsOnEachPrintedPage, false);
      if (options.Indent <= options.MaxIndent)
        PivotTableSerializator.SerializeAttributeString(writer, "indent", options.Indent);
      else
        PivotTableSerializator.SerializeAttributeString(writer, "indent", options.MaxIndent);
      Excel2007Serializator.SerializeAttribute(writer, "showHeaders", pivotTable.DisplayFieldCaptions, true);
      Excel2007Serializator.SerializeAttribute(writer, "pageWrap", options.PageFieldWrapCount, 0);
      PivotTableSerializator.SerializeAttributeString(writer, "multipleFieldFilters", options.IsMultiFieldFilter, true);
      PivotTableSerializator.SerializeAttributeString(writer, "gridDropZones", options.ShowGridDropZone, false);
      if (options.PageFieldsOrder == PivotPageAreaFieldsOrder.OverThenDown)
        Excel2007Serializator.SerializeAttribute(writer, "pageOverThenDown", true, false);
      PivotTableSerializator.SerializeAttributeString(writer, "showMissing ", options.DisplayNullString, true);
      if (options.NullString.Length > 0)
        writer.WriteAttributeString("missingCaption", options.NullString);
      PivotTableSerializator.SerializeAttributeString(writer, "showError", options.DisplayErrorString, false);
      if (options.ErrorString.Length > 0)
        writer.WriteAttributeString("errorCaption", options.ErrorString);
      Excel2007Serializator.SerializeAttribute(writer, "preserveFormatting", options.PreserveFormatting, true);
      if (options.IsDefaultAutoSort)
        writer.WriteAttributeString("fieldListSortAscending", "1");
      PivotEngine pivotEngine = new PivotEngine();
      PivotEngine pivotEngineValues = pivotTable.PivotEngineValues;
      PivotTableSerializator.SerializeLocation(writer, pivotTable);
      PivotTableSerializator.SerializePivotFields(writer, pivotTable);
      PivotTableSerializator.SerializeRowFields(writer, pivotTable);
      if (pivotTable.PivotEngineValues != null)
        PivotTableSerializator.SerializeRowItems(writer, pivotTable, pivotEngineValues);
      PivotTableSerializator.SerializeColumnFields(writer, pivotTable);
      if (pivotTable.PivotEngineValues != null)
        PivotTableSerializator.SerializeColumnItems(writer, pivotTable, pivotEngineValues);
      PivotTableSerializator.SerializePageFields(writer, pivotTable);
      PivotTableSerializator.SerializeDataFields(writer, pivotTable);
      PivotTableSerializator.SerializeCustomFormats(writer, pivotTable);
      PivotTableSerializator.SerializeConditionalFormats(writer, pivotTable);
      PivotTableSerializator.SerializeChartFormats(writer, pivotTable);
      PivotTableSerializator.SerializePivotHierarchies(writer, pivotTable);
      PivotTableSerializator.SerializeStyle(writer, pivotTable);
      PivotTableSerializator.SerializeFilters(writer, pivotTable);
      PivotTableSerializator.SerializeRowHierarchies(writer, pivotTable);
      PivotTableSerializator.SerializeColumnHierarchies(writer, pivotTable);
      if (worksheetImpl.Version >= ExcelVersion.Excel2010)
        PivotTableSerializator.SerializeTableDefinitionExtensionList(writer, pivotTable);
      writer.WriteEndElement();
    }
  }

  private static void SerializeRowItems(
    XmlWriter writer,
    PivotTableImpl pivotTable,
    PivotEngine pivotEngine)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (pivotTable.RowFields.Count <= 0)
      return;
    List<int> intList = new List<int>();
    PivotTableFields fields = pivotTable.Fields;
    foreach (PivotFieldImpl field in pivotTable.GetFields(PivotAxisTypes.Row))
      intList.Add(fields.IndexOf(field));
    Dictionary<int, Dictionary<string, int>> dictionary1 = new Dictionary<int, Dictionary<string, int>>();
    int num = 0;
    foreach (int i in intList)
    {
      PivotFieldImpl pivotFieldImpl = fields[i];
      List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortFieldValues(pivotFieldImpl.CacheField, (List<string>) null);
      Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
      for (int index = 0; index < comparisonPairList.Count; ++index)
      {
        if (comparisonPairList[index].Value != null)
          dictionary2.Add(comparisonPairList[index].Value.ToString(), index);
      }
      if (pivotFieldImpl.Axis == PivotAxisTypes.Row)
        dictionary1.Add(num++, dictionary2);
    }
    if (pivotTable.Options.RowLayout == PivotTableRowLayout.Compact || pivotTable.Options.RowLayout == PivotTableRowLayout.Outline)
    {
      if (pivotEngine.ColumnCount == 1 && pivotEngine.RowCount == 1)
        return;
      if (pivotEngine.RowCount > 0)
        writer.WriteStartElement("rowItems");
      int count = pivotTable.RowFields.Count;
      for (int rowIndex = 0; rowIndex < pivotEngine.RowCount; ++rowIndex)
      {
        for (int index = 0; index < count; ++index)
        {
          if (pivotEngine[rowIndex, index] != null && ((pivotEngine[rowIndex, index].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 || (pivotEngine[rowIndex, index].CellType & PivotCellType.TopLeftCell) != (PivotCellType) 0))
          {
            if ((pivotEngine[rowIndex, index].CellType & PivotCellType.GrandTotalCell) == (PivotCellType) 0 && (pivotEngine[rowIndex, index].CellType & PivotCellType.TotalCell) == (PivotCellType) 0)
            {
              if (pivotEngine[rowIndex, index].Value != null && pivotEngine[rowIndex, index].Value != (object) "Row Labels")
              {
                Dictionary<string, int> dictionary3 = dictionary1[index];
                string key = pivotEngine[rowIndex, index].Value.ToString().Replace("\u0083", "");
                if (dictionary3.ContainsKey(key))
                {
                  writer.WriteStartElement("i");
                  if (index != 0)
                    writer.WriteAttributeString("r", index.ToString());
                  writer.WriteStartElement("x");
                  if (dictionary3.ContainsKey(key) && Convert.ToInt32(dictionary3[key]) != 0)
                    writer.WriteAttributeString("v", dictionary3[key].ToString());
                  writer.WriteEndElement();
                  writer.WriteEndElement();
                }
              }
            }
            else if ((pivotEngine[rowIndex, index].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
            {
              writer.WriteStartElement("i");
              writer.WriteAttributeString("t", "grand");
              writer.WriteStartElement("x");
              writer.WriteEndElement();
              writer.WriteEndElement();
            }
          }
        }
      }
      if (pivotEngine.RowCount <= 0)
        return;
      writer.WriteEndElement();
    }
    else
    {
      if (pivotTable.Options.RowLayout != PivotTableRowLayout.Tabular)
        return;
      bool flag = false;
      if (pivotEngine.ColumnCount == 1 && pivotEngine.RowCount == 1)
        return;
      if (pivotEngine.RowCount > 0)
        writer.WriteStartElement("rowItems");
      int count = pivotTable.RowFields.Count;
      for (int rowIndex = 0; rowIndex < pivotEngine.RowCount; ++rowIndex)
      {
        for (int index = 0; index < count; ++index)
        {
          if (pivotEngine[rowIndex, index] != null && pivotEngine[rowIndex, index].Value != null)
          {
            Dictionary<string, int> dictionary4 = dictionary1[index];
            if ((pivotEngine[rowIndex, index].CellType & PivotCellType.RowHeaderCell) != (PivotCellType) 0 || (pivotEngine[rowIndex, index].CellType & PivotCellType.TopLeftCell) != (PivotCellType) 0)
            {
              if ((pivotEngine[rowIndex, index].CellType & PivotCellType.GrandTotalCell) == (PivotCellType) 0 && (pivotEngine[rowIndex, index].CellType & PivotCellType.TotalCell) == (PivotCellType) 0)
              {
                string key1 = pivotEngine[rowIndex, index].Value.ToString().Replace("\u0083", "");
                if (dictionary4.ContainsKey(key1))
                {
                  writer.WriteStartElement("i");
                  if (index != 0)
                    writer.WriteAttributeString("r", index.ToString());
                  for (; count > 0 && index < dictionary1.Count; --count)
                  {
                    string key2 = pivotEngine[rowIndex, index].Value.ToString().Replace("\u0083", "");
                    Dictionary<string, int> dictionary5 = dictionary1[index];
                    writer.WriteStartElement("x");
                    if (dictionary5.ContainsKey(key2) && Convert.ToInt32(dictionary5[key2]) != 0)
                      writer.WriteAttributeString("v", dictionary5[key2].ToString());
                    writer.WriteEndElement();
                    ++index;
                  }
                  count = pivotTable.RowFields.Count;
                  writer.WriteEndElement();
                }
              }
              else if ((pivotEngine[rowIndex, index].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
              {
                writer.WriteStartElement("i");
                writer.WriteAttributeString("t", "default");
                Dictionary<string, int> dictionary6 = dictionary1[index];
                if (index != 0)
                  writer.WriteAttributeString("r", index.ToString());
                writer.WriteStartElement("x");
                string key = pivotEngine[rowIndex, index].Value.ToString().Replace("\u0083", "");
                if (dictionary6.ContainsKey(key) && Convert.ToInt32(dictionary6[key]) != 0)
                  writer.WriteAttributeString("v", dictionary6[key].ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
              }
              else if ((pivotEngine[rowIndex, index].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0 && !flag)
              {
                flag = true;
                writer.WriteStartElement("i");
                writer.WriteAttributeString("t", "grand");
                writer.WriteStartElement("x");
                writer.WriteEndElement();
                writer.WriteEndElement();
              }
            }
          }
        }
      }
      if (pivotEngine.RowCount <= 0)
        return;
      writer.WriteEndElement();
    }
  }

  private static void SerializeColumnItems(
    XmlWriter writer,
    PivotTableImpl pivotTable,
    PivotEngine pivotEngine)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (pivotTable.ColumnFields.Count <= 0)
      return;
    List<int> intList = new List<int>();
    PivotTableFields fields1 = pivotTable.Fields;
    List<PivotFieldImpl> fields2 = pivotTable.GetFields(PivotAxisTypes.Column);
    int index1 = 0;
    for (int count = fields2.Count; index1 < count; ++index1)
    {
      PivotFieldImpl pivotFieldImpl = fields2[index1];
      intList.Add(fields1.IndexOf(pivotFieldImpl));
    }
    if (!pivotTable.ShowDataFieldInRow && pivotTable.DataFields.Count > 1)
      intList.Add(-2);
    Dictionary<int, Dictionary<string, int>> dictionary1 = new Dictionary<int, Dictionary<string, int>>();
    int num1 = 0;
    foreach (int i1 in intList)
    {
      if (i1 >= 0)
      {
        PivotFieldImpl pivotFieldImpl = fields1[i1];
        List<PivotTableSerializator.ComparisonPair> comparisonPairList = PivotTableSerializator.SortFieldValues(pivotFieldImpl.CacheField, (List<string>) null);
        Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
        for (int index2 = 0; index2 < comparisonPairList.Count; ++index2)
        {
          if (comparisonPairList[index2].Value != null)
            dictionary2.Add(comparisonPairList[index2].Value.ToString(), index2);
        }
        if (pivotFieldImpl.Axis == PivotAxisTypes.Column)
          dictionary1.Add(num1++, dictionary2);
      }
      else
      {
        Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
        for (int i2 = 0; i2 < pivotTable.DataFields.Count; ++i2)
          dictionary3.Add(pivotTable.DataFields[i2].Name, i2);
        dictionary1.Add(num1++, dictionary3);
      }
    }
    if (pivotEngine.ColumnCount == 1 && pivotEngine.RowCount == 1)
      return;
    if (pivotEngine.ColumnCount > 0)
      writer.WriteStartElement("colItems");
    int count1 = pivotTable.RowFields.Count;
    int count2 = pivotTable.ColumnFields.Count;
    if (pivotTable.DataFields.Count > 1)
      ++count2;
    int num2 = 0;
    bool flag1 = false;
    for (int columnIndex1 = 0; columnIndex1 < pivotEngine.ColumnCount; ++columnIndex1)
    {
      bool flag2 = true;
      for (int index3 = 0; index3 < count2; ++index3)
      {
        if (pivotEngine[index3, columnIndex1] != null && ((pivotEngine[index3, columnIndex1].CellType & PivotCellType.ColumnHeaderCell) != (PivotCellType) 0 || (pivotEngine[index3, columnIndex1].CellType & PivotCellType.TopLeftCell) != (PivotCellType) 0))
        {
          if ((pivotEngine[index3, columnIndex1].CellType & PivotCellType.TotalCell) == (PivotCellType) 0)
          {
            if (pivotEngine[index3, columnIndex1].Value != null)
            {
              if (index3 == 0)
              {
                for (int rowIndex = 0; rowIndex < count2 && (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TotalCell) == (PivotCellType) 0; ++rowIndex)
                {
                  if ((pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0 || (pivotEngine[rowIndex, columnIndex1].CellType & PivotCellType.TopLeftCell) != (PivotCellType) 0)
                  {
                    index3 = rowIndex;
                    break;
                  }
                }
              }
              if ((pivotEngine[index3, columnIndex1].CellType & PivotCellType.ExpanderCell) != (PivotCellType) 0 && index3 > num2)
                num2 = index3;
              if ((pivotEngine[index3, columnIndex1].CellType & PivotCellType.GrandTotalCell) != (PivotCellType) 0)
              {
                writer.WriteStartElement("i");
                writer.WriteAttributeString("t", "grand");
                writer.WriteStartElement("x");
                writer.WriteEndElement();
                writer.WriteEndElement();
                break;
              }
              if ((pivotEngine[index3, columnIndex1].CellType & PivotCellType.CalculationHeaderCell) == (PivotCellType) 0)
              {
                bool flag3 = true;
                if (pivotTable.ColumnFields.Count > 1 && index3 <= num2 && (pivotEngine[index3, columnIndex1].CellType & PivotCellType.ExpanderCell) == (PivotCellType) 0)
                  flag3 = false;
                if (flag3)
                {
                  Dictionary<string, int> dictionary4 = dictionary1[index3];
                  if (dictionary4.ContainsKey(pivotEngine[index3, columnIndex1].Value.ToString()))
                  {
                    if (flag2)
                    {
                      writer.WriteStartElement("i");
                      if (index3 != 0)
                        writer.WriteAttributeString("r", index3.ToString());
                      flag2 = false;
                      flag1 = true;
                    }
                    writer.WriteStartElement("x");
                    if (Convert.ToInt32(dictionary4[pivotEngine[index3, columnIndex1].Value.ToString()]) != 0)
                      writer.WriteAttributeString("v", dictionary4[pivotEngine[index3, columnIndex1].Value.ToString()].ToString());
                    writer.WriteEndElement();
                  }
                }
                else
                  index3 = index3;
              }
              else if ((pivotEngine[index3, columnIndex1].CellType & PivotCellType.CalculationHeaderCell) != (PivotCellType) 0)
              {
                Dictionary<string, int> dictionary5 = new Dictionary<string, int>();
                for (int i = 0; i < pivotTable.DataFields.Count; ++i)
                  dictionary5.Add(pivotTable.DataFields[i].Name, i);
                if (flag2)
                {
                  writer.WriteStartElement("i");
                  if (index3 != 0)
                    writer.WriteAttributeString("r", index3.ToString());
                  if (Convert.ToInt32(dictionary5[pivotEngine[index3, columnIndex1].Value.ToString()]) != 0)
                    writer.WriteAttributeString("i", dictionary5[pivotEngine[index3, columnIndex1].Value.ToString()].ToString());
                  flag2 = false;
                  flag1 = true;
                }
                writer.WriteStartElement("x");
                string key;
                if (dictionary5.ContainsKey(key = pivotEngine[index3, columnIndex1].Value.ToString()) && Convert.ToInt32(dictionary5[key]) != 0)
                  writer.WriteAttributeString("v", dictionary5[pivotEngine[index3, columnIndex1].Value.ToString()].ToString());
                writer.WriteEndElement();
              }
              if (index3 == count2 - 1 && flag1)
              {
                writer.WriteEndElement();
                flag1 = false;
                flag2 = true;
              }
            }
          }
          else if ((pivotEngine[index3, columnIndex1].CellType & PivotCellType.TotalCell) != (PivotCellType) 0)
          {
            if ((pivotEngine[index3, columnIndex1].CellType & PivotCellType.CalculationHeaderCell) == (PivotCellType) 0)
            {
              Dictionary<string, int> dictionary6 = dictionary1[index3];
              writer.WriteStartElement("i");
              writer.WriteAttributeString("t", "default");
              if (index3 != 0)
                writer.WriteAttributeString("r", index3.ToString());
              writer.WriteStartElement("x");
              string str = pivotEngine[index3, columnIndex1].Value.ToString();
              string key = str;
              if (pivotTable.Options.RowLayout == PivotTableRowLayout.Compact)
                key = str.Remove(str.Length - 1);
              if (dictionary6.ContainsKey(key) && Convert.ToInt32(dictionary6[key]) != 0)
                writer.WriteAttributeString("v", dictionary6[key].ToString());
              writer.WriteEndElement();
              writer.WriteEndElement();
              index3 = count2;
            }
            else if ((pivotEngine[index3, columnIndex1].CellType & PivotCellType.CalculationHeaderCell) != (PivotCellType) 0)
            {
              Dictionary<string, int> dictionary7 = new Dictionary<string, int>();
              for (int i = 0; i < pivotTable.DataFields.Count; ++i)
                dictionary7.Add(pivotTable.DataFields[i].Name, i);
              writer.WriteStartElement("i");
              writer.WriteAttributeString("t", "default");
              if (Convert.ToInt32(dictionary7[pivotEngine[index3, columnIndex1].Value.ToString()]) != 0)
                writer.WriteAttributeString("i", dictionary7[pivotEngine[index3, columnIndex1].Value.ToString()].ToString());
              writer.WriteStartElement("x");
              if (dictionary7.ContainsKey(pivotEngine[index3, columnIndex1].Value.ToString()) && Convert.ToInt32(dictionary7[pivotEngine[index3, columnIndex1].Value.ToString()]) != 0)
                writer.WriteAttributeString("v", dictionary7[pivotEngine[index3, columnIndex1].Value.ToString()].ToString());
              writer.WriteEndElement();
              writer.WriteEndElement();
              index3 = count2;
            }
          }
        }
      }
    }
    if (pivotEngine.ColumnCount <= 0)
      return;
    writer.WriteEndElement();
  }

  private static void SerializeTableDefinitionExtensionList(
    XmlWriter writer,
    PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (pivotTable.PreservedElements.ContainsKey("extLst"))
    {
      Stream preservedElement = pivotTable.PreservedElements["extLst"];
      preservedElement.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, preservedElement);
    }
    else
    {
      writer.WriteStartElement("extLst");
      writer.WriteStartElement("ext");
      writer.WriteAttributeString("uri", "{962EF5D1-5CA2-4c93-8EF4-DBF5C05439D2}");
      writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
      writer.WriteStartElement("x14", "pivotTableDefinition", (string) null);
      writer.WriteAttributeString("hideValuesRow", "1");
      writer.WriteAttributeString("xmlns", "xm", (string) null, "http://schemas.microsoft.com/office/excel/2006/main");
      writer.WriteEndElement();
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }

  private static void SerializeChartFormats(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (!pivotTable.PreservedElements.ContainsKey("chartFormats"))
      return;
    Stream preservedElement = pivotTable.PreservedElements["chartFormats"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  private static void SerializeCustomFormats(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (pivotTable.PivotFormatsStream == null)
      return;
    Excel2007Serializator.SerializeStream(writer, pivotTable.PivotFormatsStream, "root");
  }

  private static void SerializeFilters(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (pivotTable.Filters.Count <= 0)
      return;
    writer.WriteStartElement("filters");
    PivotTableFilters filters = pivotTable.Filters;
    if (filters.Count > 0)
    {
      for (int index = 0; index < filters.Count; ++index)
        PivotTableSerializator.SerializePivotFilter(writer, pivotTable.Filters[index]);
    }
    writer.WriteEndElement();
  }

  private static void SerializePivotFilter(XmlWriter writer, PivotTableFilter filter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (filter == null)
      throw new ArgumentNullException(nameof (filter));
    writer.WriteStartElement(nameof (filter));
    if (filter.DescriptionAttribute != null)
      writer.WriteAttributeString("description", filter.DescriptionAttribute);
    if (filter.EvalOrder != 0)
      Excel2007Serializator.SerializeAttribute(writer, "evalOrder", filter.EvalOrder, 0);
    writer.WriteAttributeString("fld", filter.Field.ToString());
    if (filter.Type == PivotFilterType.NexWeek)
      writer.WriteAttributeString("type", "nextWeek");
    else
      writer.WriteAttributeString("type", ((PivotFilterType2007) filter.Type).ToString());
    writer.WriteAttributeString("iMeasureFld", filter.MeasureFld.ToString());
    if (filter.MeasureHier > 0)
      writer.WriteAttributeString("iMeasureHier", filter.MeasureHier.ToString());
    writer.WriteAttributeString("id", filter.FilterId.ToString());
    if (filter.Value1 != null)
      writer.WriteAttributeString("stringValue1", filter.Value1);
    if (filter.Value2 != null)
      writer.WriteAttributeString("stringValue2", filter.Value2);
    if (filter.Count > 0)
    {
      for (int index = 0; index < filter.Count; ++index)
        PivotTableSerializator.SerializeAutoFilter(writer, filter[index]);
    }
    writer.WriteEndElement();
  }

  private static void SerializeAutoFilter(XmlWriter writer, PivotAutoFilter autoFilters)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (autoFilters == null)
      throw new ArgumentNullException(nameof (autoFilters));
    writer.WriteStartElement("autoFilter");
    if (autoFilters.FilterRange != null)
      writer.WriteAttributeString("ref", autoFilters.FilterRange);
    if (autoFilters.Count > 0)
    {
      for (int index = 0; index < autoFilters.Count; ++index)
        PivotTableSerializator.SerializeFilterColumn(writer, autoFilters[index]);
    }
    writer.WriteEndElement();
  }

  private static void SerializeFilterColumn(XmlWriter writer, PivotFilterColumn filterColumn)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (filterColumn == null)
      throw new ArgumentNullException(nameof (filterColumn));
    writer.WriteStartElement(nameof (filterColumn));
    writer.WriteAttributeString("colId", filterColumn.ColumnId.ToString());
    PivotTableSerializator.SerializeAttributeString(writer, "hiddenButton", filterColumn.HiddenButton, false);
    PivotTableSerializator.SerializeAttributeString(writer, "showButton", filterColumn.ShowButton, true);
    if (filterColumn.CustomFilters != null)
      PivotTableSerializator.SerializeCustomFilters(writer, filterColumn.CustomFilters);
    if (filterColumn.FilterColumnFilter != null)
      PivotTableSerializator.SerializeFilterColumnFilters(writer, filterColumn.FilterColumnFilter);
    if (filterColumn.Top10Filters != null)
      PivotTableSerializator.SerializeTop10Filter(writer, filterColumn.Top10Filters);
    if (filterColumn.DynamicFilter != null)
      PivotTableSerializator.SerializeDynamicFilter(writer, filterColumn.DynamicFilter);
    writer.WriteEndElement();
  }

  private static void SerializeCustomFilters(XmlWriter writer, PivotCustomFilters customFilters)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (customFilters == null)
      throw new ArgumentNullException("colorFilter");
    writer.WriteStartElement(nameof (customFilters));
    PivotTableSerializator.SerializeAttributeString(writer, "and", customFilters.HasAnd, false);
    for (int index = 0; index < customFilters.Count; ++index)
      PivotTableSerializator.SerializeCustomFilter(writer, customFilters[index]);
    writer.WriteEndElement();
  }

  private static void SerializeCustomFilter(XmlWriter writer, PivotCustomFilter customFilter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (customFilter == null)
      throw new ArgumentNullException("colorFilter");
    writer.WriteStartElement(nameof (customFilter));
    if (customFilter.FilterOperator != FilterOperator2007.Equal)
      writer.WriteAttributeString("operator", ((FilterOperator) customFilter.FilterOperator).ToString());
    if (customFilter.Value != null)
      writer.WriteAttributeString("val", customFilter.Value);
    writer.WriteEndElement();
  }

  private static void SerializeFilterColumnFilters(XmlWriter writer, FilterColumnFilters filters)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (filters == null)
      throw new ArgumentNullException(nameof (filters));
    writer.WriteStartElement(nameof (filters));
    if (filters.Count() > 0)
    {
      writer.WriteStartElement("filter");
      for (int index = 0; index < filters.Count(); ++index)
        writer.WriteAttributeString("val", filters[index]);
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  private static void SerializeTop10Filter(XmlWriter writer, PivotTop10Filter top10Filter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (top10Filter == null)
      throw new ArgumentNullException(nameof (top10Filter));
    writer.WriteStartElement("top10");
    PivotTableSerializator.SerializeAttributeString(writer, "percent", top10Filter.IsPercent, false);
    PivotTableSerializator.SerializeAttributeString(writer, "top", top10Filter.IsTop, true);
    writer.WriteAttributeString("filterVal", top10Filter.FilterValue.ToString());
    writer.WriteAttributeString("val", top10Filter.Value.ToString());
    writer.WriteEndElement();
  }

  private static void SerializeDynamicFilter(XmlWriter writer, PivotDynamicFilter dynamicFilter)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (dynamicFilter == null)
      throw new ArgumentNullException(nameof (dynamicFilter));
    writer.WriteStartElement(nameof (dynamicFilter));
    writer.WriteAttributeString("type", AF.ConvertDateFilterTypeToString(dynamicFilter.DateFilterType));
    writer.WriteEndElement();
  }

  private static void SerializeConditionalFormats(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (!pivotTable.PreservedElements.ContainsKey("conditionalFormats"))
      return;
    Stream preservedElement = pivotTable.PreservedElements["conditionalFormats"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  private static void SerializePageFields(XmlWriter writer, PivotTableImpl pivotTable)
  {
    bool flag1 = true;
    bool flag2 = false;
    List<PivotFieldImpl> fields1 = pivotTable.GetFields(PivotAxisTypes.Page);
    int count1 = fields1.Count;
    if (pivotTable.PivotPageFields.Count != 0)
    {
      flag2 = true;
      count1 = pivotTable.PivotPageFields.Count;
    }
    Dictionary<int, Dictionary<string, int>> dictionary1 = new Dictionary<int, Dictionary<string, int>>();
    if (count1 > 0)
    {
      List<int> intList = new List<int>();
      PivotTableFields fields2 = pivotTable.Fields;
      int index1 = 0;
      for (int count2 = fields1.Count; index1 < count2; ++index1)
      {
        PivotFieldImpl pivotFieldImpl = fields1[index1];
        intList.Add(fields2.IndexOf(pivotFieldImpl));
      }
      int num = 0;
      foreach (int i in intList)
      {
        PivotFieldImpl pivotFieldImpl = fields2[i];
        Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
        for (int index2 = 0; index2 < pivotFieldImpl.CacheField.ItemCount; ++index2)
        {
          if (pivotFieldImpl.CacheField.Items[index2] != null)
            dictionary2.Add(pivotFieldImpl.CacheField.Items[index2].ToString(), index2);
        }
        dictionary1.Add(num++, dictionary2);
      }
    }
    int num1 = 0;
    for (int index = count1; num1 < index; ++num1)
    {
      PivotFieldImpl pivotFieldImpl = flag2 ? pivotTable.PivotPageFields[num1] as PivotFieldImpl : pivotTable.PageFields[num1] as PivotFieldImpl;
      if (pivotFieldImpl.Axis == PivotAxisTypes.Page)
      {
        if (flag1)
        {
          writer.WriteStartElement("pageFields");
          flag1 = false;
        }
        if (pivotFieldImpl.PivotFilters[0] != null)
        {
          Dictionary<string, int> dictionary3 = dictionary1[num1];
          if (dictionary3.ContainsKey(pivotFieldImpl.PivotFilters[0].Value1))
            pivotFieldImpl.ItemIndex = dictionary3[pivotFieldImpl.PivotFilters[0].Value1.ToString()];
        }
        writer.WriteStartElement("pageField");
        writer.WriteAttributeString("fld", pivotTable.Fields.IndexOf(pivotFieldImpl).ToString());
        if (pivotFieldImpl.ItemIndex > -1 && !pivotFieldImpl.IsMultiSelected)
          writer.WriteAttributeString("item", pivotFieldImpl.ItemIndex.ToString());
        if (pivotFieldImpl.PageFieldHierarchyIndex >= 0)
          writer.WriteAttributeString("hier", pivotFieldImpl.PageFieldHierarchyIndex.ToString());
        else
          writer.WriteAttributeString("hier", "-1");
        if (pivotFieldImpl.PageFieldName != null)
          writer.WriteAttributeString("name", pivotFieldImpl.PageFieldName);
        if (pivotFieldImpl.PageFieldCaption != null)
          writer.WriteAttributeString("cap", pivotFieldImpl.PageFieldCaption);
        writer.WriteEndElement();
      }
    }
    if (flag1)
      return;
    writer.WriteEndElement();
  }

  private static void SerializeStyle(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    writer.WriteStartElement("pivotTableStyleInfo");
    PivotBuiltInStyles? builtInStyle = pivotTable.BuiltInStyle;
    if (builtInStyle.HasValue)
      writer.WriteAttributeString("name", builtInStyle.ToString());
    else if (pivotTable.CustomStyleName != null)
      writer.WriteAttributeString("name", pivotTable.CustomStyleName);
    Excel2007Serializator.SerializeAttribute(writer, "showRowHeaders", pivotTable.ShowRowHeaderStyle, false);
    Excel2007Serializator.SerializeAttribute(writer, "showColHeaders", pivotTable.ShowColHeaderStyle, false);
    Excel2007Serializator.SerializeAttribute(writer, "showRowStripes", pivotTable.ShowRowStripes, false);
    Excel2007Serializator.SerializeAttribute(writer, "showColStripes", pivotTable.ShowColStripes, false);
    Excel2007Serializator.SerializeAttribute(writer, "showLastColumn", pivotTable.ShowLastCol, false);
    writer.WriteEndElement();
  }

  private static void SerializeRowFields(XmlWriter writer, PivotTableImpl pivotTable)
  {
    PivotTableSerializator.SerializeFields(writer, pivotTable, PivotAxisTypes.Row, "rowFields", "rowItems", pivotTable.ShowDataFieldInRow);
  }

  private static void SerializeColumnFields(XmlWriter writer, PivotTableImpl pivotTable)
  {
    PivotTableSerializator.SerializeFields(writer, pivotTable, PivotAxisTypes.Column, "colFields", "colItems", !pivotTable.ShowDataFieldInRow);
  }

  private static void SerializeFields(
    XmlWriter writer,
    PivotTableImpl pivotTable,
    PivotAxisTypes axis,
    string tagName,
    string itemsTagName,
    bool bAddDataFields)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    List<int> lstFields = new List<int>();
    PivotTableFields fields1 = pivotTable.Fields;
    List<PivotFieldImpl> fields2 = pivotTable.GetFields(axis);
    int index1 = 0;
    for (int count = fields2.Count; index1 < count; ++index1)
    {
      PivotFieldImpl pivotFieldImpl = fields2[index1];
      lstFields.Add(fields1.IndexOf(pivotFieldImpl));
    }
    if (lstFields.Count > 0)
    {
      lstFields.Clear();
      switch (axis)
      {
        case PivotAxisTypes.Row:
          lstFields = pivotTable.RowFieldsOrder;
          break;
        case PivotAxisTypes.Column:
          if (pivotTable.ColFieldsOrder.Contains(-2) && !bAddDataFields)
            pivotTable.ColFieldsOrder.Remove(-2);
          lstFields = pivotTable.ColFieldsOrder;
          break;
      }
    }
    if (bAddDataFields && pivotTable.DataFields.Count > 1 && !lstFields.Contains(-2))
      lstFields.Add(-2);
    int count1 = lstFields.Count;
    if (count1 > 0)
    {
      writer.WriteStartElement(tagName);
      for (int index2 = 0; index2 < count1; ++index2)
      {
        int num = lstFields[index2];
        writer.WriteStartElement("field");
        writer.WriteAttributeString("x", num.ToString());
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    if (pivotTable.PivotEngineValues != null)
      return;
    PivotTableSerializator.SerializeColumnItems(writer, lstFields, itemsTagName, pivotTable);
  }

  private static void CheckIsEqual(List<int> lstFields, int[] copyIndexs)
  {
    if (lstFields.Count == copyIndexs.Length)
      return;
    if (lstFields.Count == 0 && copyIndexs.Length != 0)
      lstFields.AddRange((IEnumerable<int>) copyIndexs);
    for (int index = 0; index < copyIndexs.Length; ++index)
    {
      if (copyIndexs[index] == -2)
        lstFields.Add(-2);
    }
  }

  private static void SerializeColumnItems(
    XmlWriter writer,
    List<int> lstFields,
    string itemsTagName,
    PivotTableImpl table)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (lstFields == null)
      throw new ArgumentNullException(nameof (lstFields));
    if (itemsTagName == "colItems" && table.ColumnItemsStream != null)
    {
      table.ColumnItemsStream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, table.ColumnItemsStream);
    }
    else if (itemsTagName == "rowItems" && table.RowItemsStream != null)
    {
      table.RowItemsStream.Position = 0L;
      ShapeParser.WriteNodeFromStream(writer, table.RowItemsStream);
    }
    else
    {
      if (lstFields.Count <= 0)
        return;
      writer.WriteStartElement(itemsTagName);
      int num = 0;
      for (int count = lstFields.Count; num < count; ++num)
      {
        writer.WriteStartElement("i");
        writer.WriteStartElement("x");
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
  }

  private static void SerializeDataFields(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (pivotTable.Fields == null)
      return;
    List<int> intList = new List<int>();
    List<PivotFieldImpl> pivotFieldImplList = new List<PivotFieldImpl>();
    for (int i1 = 0; i1 < pivotTable.Fields.Count; ++i1)
    {
      int num = 0;
      if (pivotTable.Fields[i1].IsDataField)
      {
        pivotFieldImplList.Add(pivotTable.Fields[i1]);
        for (int i2 = 0; i2 < pivotTable.DataFields.Count; ++i2)
        {
          if (pivotTable.DataFields[i2].Field.CacheField.Index == i1)
            ++num;
        }
        if (num > 1)
        {
          for (int index = 1; index < num; ++index)
            pivotFieldImplList.Add(pivotTable.Fields[i1]);
        }
      }
    }
    int count = pivotFieldImplList.Count;
    if (count <= 0)
      return;
    writer.WriteStartElement("dataFields");
    writer.WriteAttributeString("count", count.ToString());
    for (int index1 = 0; index1 < count; ++index1)
    {
      writer.WriteStartElement("dataField");
      PivotFieldImpl pivotFieldImpl = pivotFieldImplList[index1];
      string name = pivotFieldImpl.Name;
      int index2 = pivotFieldImpl.CacheField.Index;
      if (pivotFieldImpl.Name != pivotTable.DataFields[index1].Name)
      {
        name = pivotTable.DataFields[index1].Name;
        index2 = pivotTable.DataFields[index1].Field.CacheField.Index;
      }
      PivotFieldImpl field = pivotTable.DataFields[index1].Field;
      writer.WriteAttributeString("name", name);
      writer.WriteAttributeString("fld", index2.ToString());
      PivotTableSerializator.SerializeSubtotal(writer, pivotTable.DataFields[index1].Subtotal);
      if (pivotTable.DataFields[index1].ShowDataAs != PivotFieldDataFormat.Normal && !pivotTable.DataFields[index1].IsExcel2010Data())
        writer.WriteAttributeString("showDataAs", pivotTable.DataFields[index1].GetShowData(pivotTable.DataFields[index1].ShowDataAs));
      writer.WriteAttributeString("baseField", pivotTable.DataFields[index1].BaseField.ToString());
      writer.WriteAttributeString("baseItem", pivotTable.DataFields[index1].BaseItem.ToString());
      if (pivotTable.DataFields[index1].NumberFormatIndex == 0 && pivotTable.DataFields[index1].Field.NumberFormatIndex == 0)
      {
        string numberFormatIndex = PivotTableSerializator.GetFieldNumberFormatIndex(pivotTable.DataFields[index1].ShowDataAs, pivotTable.DataFields[index1].NumberFormatIndex.ToString());
        if (numberFormatIndex != null)
          writer.WriteAttributeString("numFmtId", numberFormatIndex);
      }
      else if (pivotTable.DataFields[index1].NumberFormatIndex != 0)
        writer.WriteAttributeString("numFmtId", pivotTable.DataFields[index1].NumberFormatIndex.ToString());
      else
        writer.WriteAttributeString("numFmtId", pivotTable.DataFields[index1].Field.NumberFormatIndex.ToString());
      if (pivotTable.DataFields[index1].IsExcel2010Data())
      {
        writer.WriteStartElement("extLst");
        writer.WriteStartElement("ext");
        writer.WriteAttributeString("uri", "{E15A36E0-9728-4e99-A89B-3F7291B0FE68}");
        writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
        writer.WriteStartElement("x14", "dataField", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
        writer.WriteAttributeString("pivotShowAs", pivotTable.DataFields[index1].GetShowData(pivotTable.DataFields[index1].ShowDataAs));
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndElement();
      }
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
  }

  internal static string GetFieldNumberFormatIndex(
    PivotFieldDataFormat dataFormat,
    string FieldIndex)
  {
    switch (dataFormat)
    {
      case PivotFieldDataFormat.Difference:
        FieldIndex = (string) null;
        break;
      case PivotFieldDataFormat.Percent:
      case PivotFieldDataFormat.PercentageOfDifference:
      case PivotFieldDataFormat.PercentageOfColumn:
      case PivotFieldDataFormat.PercentageOfRow:
      case PivotFieldDataFormat.PercentageOfTotal:
      case PivotFieldDataFormat.PercentageOfParent:
      case PivotFieldDataFormat.PercentageOfParentColumn:
      case PivotFieldDataFormat.PercentageOfParentRow:
      case PivotFieldDataFormat.PercentageOfRunningTotal:
        FieldIndex = "10";
        break;
    }
    return FieldIndex;
  }

  internal static void SerializeSubtotal(XmlWriter writer, PivotSubtotalTypes pivotSubtotalTypes)
  {
    if (pivotSubtotalTypes == PivotSubtotalTypes.Default || pivotSubtotalTypes == PivotSubtotalTypes.None)
      return;
    PivotSubtotalTypes2007 subtotalTypes2007 = (PivotSubtotalTypes2007) pivotSubtotalTypes;
    writer.WriteAttributeString("subtotal", subtotalTypes2007.ToString());
  }

  private static List<PivotFieldImpl> SerializePivotFields(
    XmlWriter writer,
    PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    PivotTableFields pivotTableFields = pivotTable != null ? pivotTable.Fields : throw new ArgumentNullException(nameof (pivotTable));
    List<PivotFieldImpl> pivotFieldImplList = new List<PivotFieldImpl>();
    List<string> fieldName = new List<string>();
    if (pivotTableFields == null)
      return pivotFieldImplList;
    writer.WriteStartElement("pivotFields");
    int i = 0;
    for (int count = pivotTableFields.Count; i < count; ++i)
    {
      PivotFieldImpl pivotField = pivotTableFields[i];
      if (pivotField.Axis == PivotAxisTypes.Page)
        pivotFieldImplList.Add(pivotField);
      PivotTableSerializator.SerializePivotField(writer, pivotField, pivotTable, fieldName);
    }
    fieldName.Clear();
    writer.WriteEndElement();
    return pivotFieldImplList;
  }

  private static void SerializePivotField(
    XmlWriter writer,
    PivotFieldImpl pivotField,
    PivotTableImpl table,
    List<string> fieldName)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotField == null)
      throw new ArgumentNullException("field");
    writer.WriteStartElement(nameof (pivotField));
    string name = pivotField.Name;
    if (name != pivotField.CacheField.Name)
    {
      if (fieldName.Contains(pivotField.Name))
        name = pivotField.CacheField.Name;
      writer.WriteAttributeString("name", name);
    }
    fieldName.Add(name);
    if (pivotField.SubTotalName != null)
      writer.WriteAttributeString("subtotalCaption", pivotField.SubTotalName);
    PivotAxisTypes axis = pivotField.Axis;
    switch (axis)
    {
      case PivotAxisTypes.None:
      case PivotAxisTypes.Data:
        PivotTableSerializator.SerializeAttributeString(writer, "autoShow", pivotField.IsAutoShow, false);
        int rowLayout = (int) table.Options.RowLayout;
        if (!pivotField.Compact)
          writer.WriteAttributeString("compact", "0");
        if (!pivotField.ShowOutline)
          writer.WriteAttributeString("outline", "0");
        PivotTableSerializator.SerializeAttributeString(writer, "dragOff", pivotField.CanDragOff, true);
        PivotTableSerializator.SerializeAttributeString(writer, "dragToCol", pivotField.CanDragToColumn, true);
        PivotTableSerializator.SerializeAttributeString(writer, "dragToData", pivotField.CanDragToData, true);
        PivotTableSerializator.SerializeAttributeString(writer, "dragToPage", pivotField.CanDragToPage, true);
        PivotTableSerializator.SerializeAttributeString(writer, "dragToRow", pivotField.CanDragToRow, true);
        PivotTableSerializator.SerializeAttributeString(writer, "hideNewItems", !pivotField.ShowNewItemsOnRefresh, true);
        PivotTableSerializator.SerializeAttributeString(writer, "includeNewItemsInFilter", pivotField.ShowNewItemsInFilter, false);
        PivotTableSerializator.SerializeAttributeString(writer, "insertBlankRow", pivotField.ShowBlankRow, false);
        PivotTableSerializator.SerializeAttributeString(writer, "insertPageBreak", pivotField.ShowPageBreak, false);
        Excel2007Serializator.SerializeAttribute(writer, "itemPageCount", pivotField.ItemsPerPage, 10);
        PivotTableSerializator.SerializeAttributeString(writer, "measureFilter", pivotField.IsMeasureField, false);
        PivotTableSerializator.SerializeAttributeString(writer, "multipleItemSelectionAllowed", pivotField.IsMultiSelected, false);
        PivotTableSerializator.SerializeAttributeString(writer, "showAll", pivotField.IsShowAllItems, true);
        PivotTableSerializator.SerializeAttributeString(writer, "showDropDowns", pivotField.ShowDropDown, false);
        PivotTableSerializator.SerializeAttributeString(writer, "showPropAsCaption", pivotField.ShowPropAsCaption, false);
        PivotTableSerializator.SerializeAttributeString(writer, "showPropTip", pivotField.ShowToolTip, false);
        PivotTableSerializator.SerializeAttributeString(writer, "defaultAttributeDrillState", pivotField.IsDefaultDrill, false);
        PivotTableSerializator.SerializeAttributeString(writer, "dataSourceSort", pivotField.IsDataSourceSorted, false);
        PivotTableSerializator.SerializeAttributeString(writer, "allDrilled", pivotField.IsAllDrilled, false);
        if (pivotField.SortType.HasValue)
          writer.WriteAttributeString("sortType", pivotField.SortType.ToString().ToLower());
        string caption = pivotField.Caption;
        if (caption != null && caption.Length > 0)
          writer.WriteAttributeString("uniqueMemberProperty", caption);
        Excel2007Serializator.SerializeAttribute(writer, "numFmtId", pivotField.NumberFormatIndex, 0);
        if (pivotField.IsDataField)
          PivotTableSerializator.SerializeDataField(writer, pivotField);
        PivotTableSerializator.SerializeSubtotalFlags(writer, pivotField.Subtotals);
        if (axis != PivotAxisTypes.Data && axis != PivotAxisTypes.None)
          PivotTableSerializator.SerializeOrdinaryField(writer, pivotField);
        PivotTableSerializator.SerializePivotFieldExtensionList(writer, pivotField);
        writer.WriteEndElement();
        break;
      default:
        PivotAxisTypes2007 pivotAxisTypes2007 = (PivotAxisTypes2007) axis;
        writer.WriteAttributeString("axis", pivotAxisTypes2007.ToString());
        goto case PivotAxisTypes.None;
    }
  }

  private static void SerializePivotFieldExtensionList(XmlWriter writer, PivotFieldImpl pivotField)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotField == null)
      throw new ArgumentNullException(nameof (pivotField));
    if (!pivotField.RepeatLabels && !pivotField.Ignore)
      return;
    writer.WriteStartElement("extLst");
    writer.WriteStartElement("ext");
    writer.WriteAttributeString("uri", "{2946ED86-A175-432a-8AC1-64E0C546D7DE}");
    writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    writer.WriteStartElement("x14", nameof (pivotField), (string) null);
    PivotTableSerializator.SerializeAttributeString(writer, "fillDownLabels", pivotField.RepeatLabels, false);
    PivotTableSerializator.SerializeAttributeString(writer, "ignore", pivotField.Ignore, false);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private static void SerializeSubtotalFlags(XmlWriter writer, PivotSubtotalTypes subtotal)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (subtotal == PivotSubtotalTypes.Default)
      return;
    if (subtotal == PivotSubtotalTypes.None)
    {
      Excel2007Serializator.SerializeAttribute(writer, "defaultSubtotal", false, true);
    }
    else
    {
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Sum, false, "sumSubtotal");
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Counta, false, "countASubtotal");
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Average, false, "avgSubtotal");
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Max, false, "maxSubtotal");
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Min, false, "minSubtotal");
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Product, false, "productSubtotal");
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Count, false, "countSubtotal");
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Stdev, false, "stdDevSubtotal");
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Stdevp, false, "stdDevPSubtotal");
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Var, false, "varSubtotal");
      PivotTableSerializator.SerializeSubtotalFlags(writer, subtotal, PivotSubtotalTypes.Varp, false, "varPSubtotal");
    }
  }

  private static void SerializeSubtotalFlags(
    XmlWriter writer,
    PivotSubtotalTypes subtotal,
    PivotSubtotalTypes subtotalItem,
    bool defaultValue,
    string attributeName)
  {
    bool flag = (subtotal & subtotalItem) != PivotSubtotalTypes.None;
    Excel2007Serializator.SerializeAttribute(writer, attributeName, flag, defaultValue);
  }

  private static void SerializeDataField(XmlWriter writer, PivotFieldImpl field)
  {
    Excel2007Serializator.SerializeAttribute(writer, "dataField", field.IsDataField, false);
  }

  private static void SerializeOrdinaryField(XmlWriter writer, PivotFieldImpl field)
  {
    bool flag1 = false;
    Dictionary<int, PivotItemOptions> itemOptions = field.ItemOptions;
    IList<object> objectList = (IList<object>) new List<object>();
    if ((field.CacheField.DataType == PivotDataType.Date || field.CacheField.DataType == (PivotDataType.Blank | PivotDataType.Date)) && field.CacheField.FieldGroup != null && field.CacheField.FieldGroup.PivotRangeGroupNames != null && field.CacheField.FieldGroup.PivotRangeGroupNames.Count > 0)
    {
      foreach (object pivotRangeGroupName in field.CacheField.FieldGroup.PivotRangeGroupNames)
        objectList.Add(pivotRangeGroupName);
    }
    else
      objectList = field.CacheField.Items;
    if (itemOptions.Count > 0 && !field.ItemOptionSorted)
    {
      int index = 0;
      bool flag2 = true;
      writer.WriteStartElement("items");
      flag1 = true;
      foreach (KeyValuePair<int, PivotItemOptions> keyValuePair in itemOptions)
      {
        if (field.Subtotals != PivotSubtotalTypes.None || keyValuePair.Key != -1)
        {
          writer.WriteStartElement("item");
          if (keyValuePair.Key != -1)
            writer.WriteAttributeString("x", keyValuePair.Key.ToString());
          if (field.Items.Count > 0 && !field.Items[index].Visible && (field.Axis == PivotAxisTypes.Page || field.Axis == PivotAxisTypes.Row || field.Axis == PivotAxisTypes.Column) && flag2 && field.IsMultiSelected)
          {
            writer.WriteAttributeString("h", "1");
            if (keyValuePair.Value != null)
              keyValuePair.Value.IsHidden = false;
          }
          if (keyValuePair.Value != null)
            PivotTableSerializator.SerializeFieldItem(writer, keyValuePair.Key, keyValuePair.Value);
          writer.WriteEndElement();
          if (field.Items.Count > index + 1)
            ++index;
          else
            flag2 = false;
        }
      }
    }
    else if (field.Items.Count > 0 && field.Items.Count <= 20000)
    {
      writer.WriteStartElement("items");
      flag1 = true;
      int index1 = 0;
      for (int count = field.Items.Count; index1 < count; ++index1)
      {
        PivotFieldItem pivotFieldItem = field.Items[index1] as PivotFieldItem;
        writer.WriteStartElement("item");
        object obj1 = PivotTableSerializator.GetValue(field.CacheField.DataType, pivotFieldItem.Name);
        int index2 = objectList.IndexOf(obj1);
        int result1;
        if (index2 == -1 && (field.CacheField.DataType == PivotDataType.Date || field.CacheField.DataType == (PivotDataType.String | PivotDataType.Date) || field.CacheField.DataType == (PivotDataType.Date | PivotDataType.Boolean)) && !int.TryParse(pivotFieldItem.Name, out result1))
          index2 = field.CacheField.DataType != (PivotDataType.Date | PivotDataType.Boolean) ? objectList.IndexOf((object) pivotFieldItem.Name) : objectList.IndexOf((object) bool.Parse(pivotFieldItem.Name));
        result1 = 0;
        if (index2 == -1)
        {
          foreach (object obj2 in (IEnumerable<object>) objectList)
          {
            double result2 = 0.0;
            double result3 = 0.0;
            if (obj2 != null && double.TryParse(obj2.ToString(), out result2) && double.TryParse(obj1.ToString(), out result3))
            {
              if (Math.Round(result2, 2) == Math.Round(result3, 2))
              {
                index2 = result1;
                break;
              }
              ++result1;
            }
          }
        }
        writer.WriteAttributeString("x", index2.ToString());
        if (!pivotFieldItem.Visible && field.IsMultiSelected)
          writer.WriteAttributeString("h", "1");
        if (pivotFieldItem.ItemOptions != null && (pivotFieldItem.ItemOptions.HasItemProperties || index2 == -1))
          PivotTableSerializator.SerializeFieldItem(writer, index2, pivotFieldItem.ItemOptions);
        writer.WriteEndElement();
      }
    }
    else if (field.Items.Count > 20000)
    {
      SortedList<PivotTableSerializator.ComparisonPair, object> sortedList = PivotTableSerializator.SortFieldValues(field.CacheField);
      writer.WriteStartElement("items");
      flag1 = true;
      int index = 0;
      for (int count = sortedList.Count; index < count; ++index)
      {
        IPivotFieldItem pivotFieldItem = field.Items[index];
        writer.WriteStartElement("item");
        writer.WriteAttributeString("x", sortedList.Keys[index].Index.ToString());
        if (!field.Items[index].Visible && field.IsMultiSelected)
          writer.WriteAttributeString("h", "1");
        writer.WriteEndElement();
      }
    }
    PivotTableSerializator.SerializeSubtotalItems(writer, field.Subtotals);
    if (flag1)
      writer.WriteEndElement();
    PivotTableSerializator.SerializeAutoSortScope(writer, field);
  }

  private static object GetValue(PivotDataType type, string value)
  {
    if (value == null || value == string.Empty)
      return (object) value;
    if ((type & PivotDataType.Date) != (PivotDataType) 0)
    {
      DateTime result;
      if (DateTime.TryParse(value, out result))
        return (object) result;
    }
    else if ((type & PivotDataType.Boolean) != (PivotDataType) 0)
    {
      bool result;
      if (bool.TryParse(value, out result))
        return (object) result;
    }
    else
    {
      double result;
      if (((type & PivotDataType.Number) != (PivotDataType) 0 || (type & PivotDataType.Integer) != (PivotDataType) 0 || (type & PivotDataType.Float) != (PivotDataType) 0) && double.TryParse(value, out result))
        return (object) result;
    }
    return (object) value;
  }

  private static void SerializeAutoSortScope(XmlWriter writer, PivotFieldImpl field)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    if (field.PivotArea == null || field.PivotArea.References.Count <= 0)
      return;
    writer.WriteStartElement("autoSortScope");
    PivotCacheSerializator.SerializePivotArea(writer, field.PivotArea, field.PivotArea.IsAutoSort);
    writer.WriteEndElement();
  }

  private static void SerializeFieldItem(XmlWriter writer, int index, PivotItemOptions item)
  {
    if (index == -1)
    {
      PivotTableSerializator.SerializeSubtotalItems(writer, item.ItemType);
    }
    else
    {
      if (item.HasChildItems)
        writer.WriteAttributeString("c", "1");
      if (item.IsExpaned)
        writer.WriteAttributeString("d", "1");
      if (item.DrillAcross)
        writer.WriteAttributeString("e", "1");
      if (item.IsCalculatedItem)
        writer.WriteAttributeString("f", "1");
      if (item.IsHidden)
        writer.WriteAttributeString("h", "1");
      if (item.IsMissing)
        writer.WriteAttributeString("m", "1");
      if (item.UserCaption != null)
        writer.WriteAttributeString("n", item.UserCaption);
      if (item.IsChar)
        writer.WriteAttributeString("s", "1");
      if (item.IsHiddenDetails)
        return;
      writer.WriteAttributeString("sd", "0");
    }
  }

  private static void SerializeSubtotalItems(XmlWriter writer, PivotItemType subtotal)
  {
    PivotSubtotalItems2007[] values = (PivotSubtotalItems2007[]) Enum.GetValues(typeof (PivotItemType2007));
    PivotItemType2007 pivotItemType2007 = (PivotItemType2007) subtotal;
    if (pivotItemType2007 == PivotItemType2007.defaults)
      writer.WriteAttributeString("t", "default");
    else
      writer.WriteAttributeString("t", pivotItemType2007.ToString());
  }

  private static void SerializeSubtotalItems(XmlWriter writer, PivotSubtotalTypes subtotal)
  {
    if (subtotal == PivotSubtotalTypes.None)
      return;
    if (subtotal == PivotSubtotalTypes.Default)
    {
      writer.WriteStartElement("item");
      writer.WriteAttributeString("t", "default");
      writer.WriteEndElement();
    }
    else
    {
      PivotSubtotalItems2007[] values = (PivotSubtotalItems2007[]) Enum.GetValues(typeof (PivotSubtotalItems2007));
      PivotSubtotalItems2007 subtotalItems2007_1 = (PivotSubtotalItems2007) subtotal;
      int index = 0;
      for (int length = values.Length; index < length; ++index)
      {
        PivotSubtotalItems2007 subtotalItems2007_2 = values[index];
        if (subtotalItems2007_2 != (PivotSubtotalItems2007) 0 && (subtotalItems2007_1 & subtotalItems2007_2) != (PivotSubtotalItems2007) 0)
        {
          writer.WriteStartElement("item");
          writer.WriteAttributeString("t", subtotalItems2007_2.ToString());
          writer.WriteEndElement();
        }
      }
    }
  }

  internal static SortedList<PivotTableSerializator.ComparisonPair, object> SortFieldValues(
    PivotCacheFieldImpl field)
  {
    SortedList<PivotTableSerializator.ComparisonPair, object> sortedList = new SortedList<PivotTableSerializator.ComparisonPair, object>();
    int index = 0;
    for (int itemCount = field.ItemCount; index < itemCount; ++index)
    {
      object obj = field.GetValue(index);
      if (Convert.ToString(obj).Length >= 0 && !field.IsMixedType)
        sortedList[new PivotTableSerializator.ComparisonPair()
        {
          Value = obj,
          Index = index.ToString()
        }] = (object) null;
    }
    return sortedList;
  }

  public static List<PivotTableSerializator.ComparisonPair> SortFieldValues(
    PivotCacheFieldImpl field,
    List<string> orderBy)
  {
    List<PivotTableSerializator.ComparisonPair> comparisonPairList = new List<PivotTableSerializator.ComparisonPair>();
    int index = 0;
    for (int itemCount = field.ItemCount; index < itemCount; ++index)
    {
      object obj = field.GetValue(index);
      if (Convert.ToString(obj).Length >= 0 && !field.IsMixedType)
        comparisonPairList.Add(new PivotTableSerializator.ComparisonPair()
        {
          Value = obj,
          Index = field.Name
        });
    }
    if (orderBy != null)
      comparisonPairList.Sort((IComparer<PivotTableSerializator.ComparisonPair>) new PivotTableSerializator.CustomComparer(orderBy));
    else
      comparisonPairList.Sort();
    return comparisonPairList;
  }

  internal static List<PivotTableSerializator.ComparisonPair> SortFieldValues(
    PivotFieldImpl field,
    List<string> orderBy,
    bool bBuiltInSort)
  {
    List<PivotTableSerializator.ComparisonPair> comparisonPairList = new List<PivotTableSerializator.ComparisonPair>();
    int index = 0;
    for (int count = field.Items.Count; index < count; ++index)
    {
      object text = (object) field.Items[index].Text;
      if (Convert.ToString(text).Length >= 0 && !field.CacheField.IsMixedType)
      {
        comparisonPairList.Add(new PivotTableSerializator.ComparisonPair()
        {
          Value = text,
          Index = (field.Items[index] as PivotFieldItem).Name
        });
        if (orderBy != null && text != null && !orderBy.Contains(text.ToString()))
          orderBy.Add(text.ToString());
      }
    }
    List<string> orderBy1 = new List<string>();
    if (bBuiltInSort)
    {
      List<List<string>> customLists = (field.m_table.Application as ApplicationImpl).CustomLists;
      foreach (PivotTableSerializator.ComparisonPair comparisonPair in comparisonPairList)
      {
        foreach (List<string> collection in customLists)
        {
          if (comparisonPair.Value != null && collection.Contains(comparisonPair.Value.ToString()) && !orderBy1.Contains(comparisonPair.Value.ToString()))
          {
            orderBy1.AddRange((IEnumerable<string>) collection);
            break;
          }
        }
      }
      if (orderBy != null)
      {
        customLists.Add(orderBy);
        foreach (string str in orderBy)
        {
          if (!orderBy1.Contains(str.ToString()))
            orderBy1.Add(str.ToString());
        }
      }
      else
      {
        foreach (PivotTableSerializator.ComparisonPair comparisonPair in comparisonPairList)
        {
          if (comparisonPair.Value != null && !orderBy1.Contains(comparisonPair.Value.ToString()))
            orderBy1.Add(comparisonPair.Value.ToString());
        }
      }
      comparisonPairList.Sort((IComparer<PivotTableSerializator.ComparisonPair>) new PivotTableSerializator.CustomComparer(orderBy1));
    }
    else if (orderBy != null)
      comparisonPairList.Sort((IComparer<PivotTableSerializator.ComparisonPair>) new PivotTableSerializator.CustomComparer(orderBy));
    else
      comparisonPairList.Sort();
    return comparisonPairList;
  }

  internal static List<PivotTableSerializator.ComparisonPair> SortFieldValues(PivotFieldImpl field)
  {
    List<PivotTableSerializator.ComparisonPair> comparisonPairList = new List<PivotTableSerializator.ComparisonPair>();
    int index = 0;
    for (int count = field.Items.Count; index < count; ++index)
    {
      object text = (object) field.Items[index].Text;
      if (Convert.ToString(text).Length >= 0)
        comparisonPairList.Add(new PivotTableSerializator.ComparisonPair()
        {
          Value = text,
          Index = (field.Items[index] as PivotFieldItem).Name
        });
    }
    List<string> orderBy = new List<string>();
    List<List<string>> customLists = (field.m_table.Application as ApplicationImpl).CustomLists;
    foreach (PivotTableSerializator.ComparisonPair comparisonPair in comparisonPairList)
    {
      foreach (List<string> collection in customLists)
      {
        if (comparisonPair.Value != null && collection.Contains(comparisonPair.Value.ToString()) && !orderBy.Contains(comparisonPair.Value.ToString()))
        {
          orderBy.AddRange((IEnumerable<string>) collection);
          break;
        }
      }
    }
    if (orderBy.Count != 0)
    {
      foreach (PivotTableSerializator.ComparisonPair comparisonPair in comparisonPairList)
      {
        if (comparisonPair.Value != null && !orderBy.Contains(comparisonPair.Value.ToString()))
          orderBy.Add(comparisonPair.Value.ToString());
      }
      comparisonPairList.Sort((IComparer<PivotTableSerializator.ComparisonPair>) new PivotTableSerializator.CustomComparer(orderBy));
    }
    else if (field.CacheField != null)
      comparisonPairList.Sort();
    return comparisonPairList;
  }

  internal static List<PivotTableSerializator.ComparisonPair> SortPivotInnerItems(
    List<PivotInnerItem> innerItems,
    PivotTableImpl pivotTable)
  {
    List<PivotTableSerializator.ComparisonPair> comparisonPairList = new List<PivotTableSerializator.ComparisonPair>();
    int index1 = 0;
    for (int count = innerItems.Count; index1 < count; ++index1)
    {
      object name = (object) innerItems[index1].Name;
      if (Convert.ToString(name).Length >= 0)
        comparisonPairList.Add(new PivotTableSerializator.ComparisonPair()
        {
          Value = name,
          Index = innerItems[index1].Name
        });
    }
    List<string> orderBy = new List<string>();
    List<List<string>> customLists = (pivotTable.Application as ApplicationImpl).CustomLists;
    foreach (PivotTableSerializator.ComparisonPair comparisonPair in comparisonPairList)
    {
      foreach (List<string> collection in customLists)
      {
        if (comparisonPair.Value != null && collection.Contains(comparisonPair.Value.ToString()) && !orderBy.Contains(comparisonPair.Value.ToString()))
        {
          orderBy.AddRange((IEnumerable<string>) collection);
          break;
        }
      }
    }
    if (orderBy.Count > 0)
    {
      List<string> collection = new List<string>();
      int index2 = 0;
      for (int count = innerItems.Count; index2 < count; ++index2)
      {
        string name = innerItems[index2].Name;
        if (!orderBy.Contains(name) && !collection.Contains(name))
          collection.Add(name);
      }
      if (collection.Count > 0)
      {
        collection.Sort();
        orderBy.AddRange((IEnumerable<string>) collection);
      }
    }
    if (orderBy.Count > 0)
      comparisonPairList.Sort((IComparer<PivotTableSerializator.ComparisonPair>) new PivotTableSerializator.CustomComparer(orderBy));
    else
      comparisonPairList.Sort();
    return comparisonPairList;
  }

  private static void SerializeLocation(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    writer.WriteStartElement("location");
    string[] strArray = pivotTable.Location.AddressLocal.Split(':');
    if (pivotTable.PivotEngineValues == null || strArray.Length == 2)
      writer.WriteAttributeString("ref", pivotTable.Location.AddressLocal);
    else if (pivotTable.EndLocation != null)
      writer.WriteAttributeString("ref", $"{pivotTable.Location.AddressLocal}:{pivotTable.EndLocation.AddressLocal}");
    else
      writer.WriteAttributeString("ref", pivotTable.Location.AddressLocal);
    Excel2007Serializator.SerializeAttribute(writer, "colPageCount", pivotTable.ColumnsPerPage, 0);
    Excel2007Serializator.SerializeAttribute(writer, "rowPageCount", pivotTable.RowsPerPage, 0);
    Excel2007Serializator.SerializeAttribute(writer, "firstHeaderRow", pivotTable.FirstHeaderRow, -1);
    Excel2007Serializator.SerializeAttribute(writer, "firstDataRow", pivotTable.FirstDataRow, -1);
    Excel2007Serializator.SerializeAttribute(writer, "firstDataCol", pivotTable.FirstDataCol, -1);
    writer.WriteEndElement();
  }

  private static void SerializePivotHierarchies(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (!pivotTable.PreservedElements.ContainsKey("pivotHierarchies"))
      return;
    Stream preservedElement = pivotTable.PreservedElements["pivotHierarchies"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  private static void SerializeRowHierarchies(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (!pivotTable.PreservedElements.ContainsKey("rowHierarchiesUsage"))
      return;
    Stream preservedElement = pivotTable.PreservedElements["rowHierarchiesUsage"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  private static void SerializeColumnHierarchies(XmlWriter writer, PivotTableImpl pivotTable)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (pivotTable == null)
      throw new ArgumentNullException(nameof (pivotTable));
    if (!pivotTable.PreservedElements.ContainsKey("colHierarchiesUsage"))
      return;
    Stream preservedElement = pivotTable.PreservedElements["colHierarchiesUsage"];
    preservedElement.Position = 0L;
    ShapeParser.WriteNodeFromStream(writer, preservedElement);
  }

  internal static void SerializeAttributeString(
    XmlWriter writer,
    string attributeName,
    bool value,
    bool defaultValue)
  {
    Excel2007Serializator.SerializeAttribute(writer, attributeName, value, defaultValue);
  }

  internal static void SerializeAttributeString(XmlWriter writer, string attributeName, byte value)
  {
    writer.WriteAttributeString(attributeName, XmlConvert.ToString(value));
  }

  internal static void SerializeAttributeString(
    XmlWriter writer,
    string attributeName,
    ushort value)
  {
    writer.WriteAttributeString(attributeName, XmlConvert.ToString(value));
  }

  internal static void SerializeAttributeString(XmlWriter writer, string attributeName, uint value)
  {
    writer.WriteAttributeString(attributeName, XmlConvert.ToString(value));
  }

  internal static void SerializeAttributeString(XmlWriter writer, string attributeName, int value)
  {
    writer.WriteAttributeString(attributeName, XmlConvert.ToString(value));
  }

  internal static bool IsDataFieldsInRow(PivotTableImpl table)
  {
    int index = 0;
    for (int count = table.ColumnFields.Count; index < count; ++index)
    {
      if ((table.ColumnFields[index] as PivotFieldImpl).IsDataField)
        return true;
    }
    return false;
  }

  public class ComparisonPair : IComparable
  {
    public object Value;
    public string Index;
    public IComparer Comparer = (IComparer) new PivotTableSerializator.ComparisonPair.GeneralComparer();

    public int CompareTo(object obj)
    {
      PivotTableSerializator.ComparisonPair comparisonPair = obj as PivotTableSerializator.ComparisonPair;
      int num = 1;
      if (comparisonPair != null)
      {
        num = this.Comparer.Compare(this.Value, comparisonPair.Value);
        if (num == 0)
          num = this.Comparer.Compare((object) this.Index, (object) comparisonPair.Index);
      }
      return num;
    }

    private class GeneralComparer : IComparer
    {
      private IComparer m_comparer = (IComparer) Comparer<object>.Default;

      public int Compare(object x, object y)
      {
        try
        {
          double result1;
          double result2;
          if (x == null || !double.TryParse(x.ToString(), out result1) || y == null || !double.TryParse(y.ToString(), out result2))
            return this.m_comparer.Compare(x, y);
          if (result1 > result2)
            return 1;
          return result1 < result2 ? -1 : 0;
        }
        catch
        {
          return x.GetHashCode() - y.GetHashCode();
        }
      }
    }
  }

  public class CustomComparer : IComparer<PivotTableSerializator.ComparisonPair>
  {
    private List<string> orderBy;

    public CustomComparer(List<string> orderBy) => this.orderBy = orderBy;

    public int Compare(
      PivotTableSerializator.ComparisonPair x,
      PivotTableSerializator.ComparisonPair y)
    {
      try
      {
        return this.orderBy.IndexOf(x.Value.ToString()).CompareTo(this.orderBy.IndexOf(y.Value.ToString()));
      }
      catch
      {
        return x.GetHashCode() - y.GetHashCode();
      }
    }
  }
}
