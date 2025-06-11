// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.TableSerializator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Tables;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

internal class TableSerializator
{
  public void Serialize(XmlWriter writer, IListObject table)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (table == null)
      throw new ArgumentNullException(nameof (table));
    writer.WriteStartElement(nameof (table), "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("id", table.Index.ToString());
    writer.WriteAttributeString("name", table.Name);
    writer.WriteAttributeString("displayName", table.DisplayName);
    writer.WriteAttributeString("ref", table.Location.AddressLocal);
    ListObject listObject = table as ListObject;
    if (!listObject.ShowHeaderRow)
      writer.WriteAttributeString("headerRowCount", "0");
    if (table.QueryTable != null)
    {
      writer.WriteAttributeString("tableType", ExcelTableType.queryTable.ToString());
      writer.WriteAttributeString("insertRow", "1");
    }
    if (table.TableType == ExcelTableType.xml)
      writer.WriteAttributeString("tableType", ExcelTableType.xml.ToString());
    Excel2007Serializator.SerializeAttribute(writer, "insertRowShift", listObject.InsertRowShift, 0);
    if (!(table as ListObject).TotalsRowShown)
      Excel2007Serializator.SerializeAttribute(writer, "totalsRowShown", false, true);
    else
      Excel2007Serializator.SerializeAttribute(writer, "totalsRowCount", table.TotalsRowCount, 0);
    (table.Worksheet.Workbook as WorkbookImpl).DataHolder.ParseDxfsCollection();
    FileDataHolder dataHolder = (table.Worksheet.Workbook as WorkbookImpl).DataHolder;
    if (dataHolder != null && dataHolder.ParsedDxfsCount > 0)
    {
      Excel2007Serializator.SerializeAttribute(writer, "dataDxfId", listObject.DataAreaFormatId, int.MinValue);
      Excel2007Serializator.SerializeAttribute(writer, "headerRowBorderDxfId", listObject.HeaderRowBorderFormatId, int.MinValue);
      if (dataHolder.XFIndexes != null && dataHolder.XFIndexes.Count > 0 && dataHolder.XFIndexes.Contains(listObject.HeaderRowFormatId))
        Excel2007Serializator.SerializeAttribute(writer, "headerRowDxfId", listObject.HeaderRowFormatId, int.MinValue);
      Excel2007Serializator.SerializeAttribute(writer, "tableBorderDxfId", listObject.TableBorderFormatId, int.MinValue);
      Excel2007Serializator.SerializeAttribute(writer, "totalsRowBorderDxfId", listObject.TotalsRowBorderFormatId, int.MinValue);
      Excel2007Serializator.SerializeAttribute(writer, "totalsRowDxfId", listObject.TotalsRowFormatId, int.MinValue);
    }
    this.SerializeAutoFilter(writer, table);
    this.SerializeColumns(writer, table.Columns, listObject.TableType);
    this.SerializeStyle(writer, table);
    int tableType = (int) listObject.TableType;
    this.SerializeTableExtensionList(writer, table);
    writer.WriteEndElement();
  }

  private void SerializeTableExtensionList(XmlWriter writer, IListObject table)
  {
    if (string.IsNullOrEmpty(table.AlternativeText) && string.IsNullOrEmpty(table.Summary))
      return;
    writer.WriteStartElement("extLst");
    writer.WriteStartElement("ext");
    writer.WriteAttributeString("uri", "{504A1905-F514-4f6f-8877-14C23A59335A}");
    writer.WriteAttributeString("xmlns", "x14", (string) null, "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
    writer.WriteStartElement("x14", nameof (table), (string) null);
    if (!string.IsNullOrEmpty(table.AlternativeText))
      writer.WriteAttributeString("altText", table.AlternativeText);
    if (!string.IsNullOrEmpty(table.Summary))
      writer.WriteAttributeString("altTextSummary", table.Summary);
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }

  private void SerializeAutoFilter(XmlWriter writer, IListObject table)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (table == null)
      throw new ArgumentNullException(nameof (table));
    if (!table.ShowHeaderRow || !table.ShowAutoFilter)
      return;
    (table.Worksheet.Workbook as WorkbookImpl).DataHolder.Serializator.SerializeAutoFilters(writer, table.AutoFilters);
  }

  private void SerializeStyle(XmlWriter writer, IListObject table)
  {
    ListObject listObject = (ListObject) table;
    TableBuiltInStyles builtInTableStyle = table.BuiltInTableStyle;
    writer.WriteStartElement("tableStyleInfo");
    if (listObject.TableStyleName != null || builtInTableStyle != TableBuiltInStyles.None)
    {
      if (builtInTableStyle != TableBuiltInStyles.None)
        writer.WriteAttributeString("name", builtInTableStyle.ToString());
      else
        writer.WriteAttributeString("name", listObject.TableStyleName);
    }
    writer.WriteAttributeString("showFirstColumn", (listObject.ShowFirstColumn ? 1 : 0).ToString());
    writer.WriteAttributeString("showLastColumn", (listObject.ShowLastColumn ? 1 : 0).ToString());
    writer.WriteAttributeString("showRowStripes", (listObject.ShowTableStyleRowStripes ? 1 : 0).ToString());
    writer.WriteAttributeString("showColumnStripes", (listObject.ShowTableStyleColumnStripes ? 1 : 0).ToString());
    writer.WriteEndElement();
  }

  private void SerializeColumns(
    XmlWriter writer,
    IList<IListObjectColumn> columns,
    ExcelTableType Type)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (columns == null)
      throw new ArgumentNullException(nameof (columns));
    writer.WriteStartElement("tableColumns");
    int index = 0;
    for (int count = columns.Count; index < count; ++index)
    {
      IListObjectColumn column = columns[index];
      this.SerializeColumn(writer, column, Type);
    }
    writer.WriteEndElement();
  }

  private void SerializeColumn(XmlWriter writer, IListObjectColumn column, ExcelTableType type)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (column == null)
      throw new ArgumentNullException(nameof (column));
    writer.WriteStartElement("tableColumn");
    writer.WriteAttributeString("id", column.Id.ToString());
    string str1 = column.Name;
    if (str1.Contains("\n"))
      str1 = str1.Replace("\n", "_x000a_");
    if (str1.Contains("\r"))
      str1 = str1.Replace("\r", "_x000d_");
    if (str1.Contains("_x005f_"))
      str1 = str1.Replace("_x005f", "_x005f_x005f");
    if (str1.Contains("_x005F_"))
      str1 = str1.Replace("_x005F", "_x005f_x005F");
    if (str1.Contains("_x0009_"))
      str1 = str1.Replace("_x0009_", "_x005f_x0009_");
    if (str1.Contains("\t"))
      str1 = str1.Replace("\t", "_x0009_");
    writer.WriteAttributeString("name", str1);
    Excel2007Serializator.SerializeAttribute(writer, "totalsRowLabel", column.TotalsRowLabel, (string) null);
    if (type == ExcelTableType.queryTable)
    {
      writer.WriteAttributeString("queryTableFieldId", column.QueryTableFieldId.ToString());
      writer.WriteAttributeString("uniqueName", column.QueryTableFieldId.ToString());
    }
    if (column.TotalsCalculation != ExcelTotalsCalculation.None)
    {
      string str2 = Excel2007Serializator.LowerFirstLetter(column.TotalsCalculation.ToString());
      writer.WriteAttributeString("totalsRowFunction", str2);
    }
    if (type == ExcelTableType.xml)
    {
      writer.WriteAttributeString("uniqueName", str1);
      writer.WriteStartElement("xmlColumnPr");
      writer.WriteAttributeString("mapId", (column as ListObjectColumn).MapId.ToString());
      writer.WriteAttributeString("xpath", (column as ListObjectColumn).XPath);
      writer.WriteAttributeString("xmlDataType", (column as ListObjectColumn).XmlDataType);
      writer.WriteEndElement();
    }
    string calculatedFormula = column.CalculatedFormula;
    if (calculatedFormula != null)
    {
      if ((column as ListObjectColumn).IsArrayFormula)
      {
        writer.WriteStartElement("calculatedColumnFormula");
        writer.WriteAttributeString("array", "1");
        writer.WriteString(calculatedFormula);
        writer.WriteEndElement();
      }
      else
        writer.WriteElementString("calculatedColumnFormula", calculatedFormula);
    }
    writer.WriteEndElement();
  }

  public void SerializeQueryTable(IListObject Table, XmlWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException("Writer");
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.WriteStartDocument(true);
    writer.WriteStartElement("queryTable", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    writer.WriteAttributeString("name", Table.QueryTable.Name);
    int growShrinkType = (int) Table.QueryTable.GrowShrinkType;
    writer.WriteAttributeString("growShrinkType", Table.QueryTable.GrowShrinkType.ToString());
    if (!Table.QueryTable.AdjustColumnWidth)
      Excel2007Serializator.SerializeBool(writer, "adjustColumnWidth", Table.QueryTable.AdjustColumnWidth);
    Excel2007Serializator.SerializeBool(writer, "refreshOnLoad", Table.QueryTable.RefreshOnFileOpen);
    writer.WriteAttributeString("connectionId", Table.QueryTable.ConncetionId.ToString());
    if (!Table.QueryTable.BackgroundQuery)
      writer.WriteAttributeString("backgroundRefresh", "0");
    writer.WriteStartElement("queryTableRefresh");
    int count = Table.Columns.Count;
    int nextId = Table.QueryTable.QueryTableRefresh.NextId;
    bool sortFilterLayout = Table.QueryTable.QueryTableRefresh.PreserveSortFilterLayout;
    writer.WriteAttributeString("preserveSortFilterLayout", sortFilterLayout ? "1" : "0");
    writer.WriteAttributeString("nextId", (nextId == 0 ? count + 1 : nextId).ToString());
    writer.WriteAttributeString("unboundColumnsLeft", Table.QueryTable.QueryTableRefresh.UnboundColumnsLeft.ToString());
    writer.WriteAttributeString("unboundColumnsRight", Table.QueryTable.QueryTableRefresh.UnboundColumnsRight.ToString());
    writer.WriteStartElement("queryTableFields");
    writer.WriteAttributeString("count", count.ToString());
    IList<IListObjectColumn> columns = Table.Columns;
    for (int index = 0; index < count; ++index)
    {
      writer.WriteStartElement("queryTableField");
      writer.WriteAttributeString("id", columns[index].QueryTableFieldId.ToString());
      if (Table.QueryTable.QueryTableRefresh.QueryFields.Count > index)
      {
        QueryTableField queryField = Table.QueryTable.QueryTableRefresh.QueryFields[index];
        if (!string.IsNullOrEmpty(queryField.Name))
          writer.WriteAttributeString("name", queryField.Name);
        if (!queryField.DataBound)
          writer.WriteAttributeString("dataBound", Convert.ToInt32(queryField.DataBound).ToString());
      }
      writer.WriteAttributeString("tableColumnId", columns[index].Id.ToString());
      writer.WriteEndElement();
    }
    writer.WriteEndElement();
    writer.WriteEndElement();
    writer.WriteEndElement();
  }
}
