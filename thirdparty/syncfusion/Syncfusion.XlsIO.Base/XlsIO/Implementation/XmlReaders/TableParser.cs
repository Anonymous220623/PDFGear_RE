// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.TableParser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Tables;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders;

internal class TableParser
{
  public IListObject Parse(XmlReader reader, IWorksheet sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "table")
      throw new XmlException();
    string name = (string) null;
    IRange range = (IRange) null;
    string str = (string) null;
    if (reader.MoveToAttribute("name"))
      name = reader.Value;
    if (reader.MoveToAttribute("ref"))
      range = sheet[reader.Value];
    if (reader.MoveToAttribute("displayName"))
      str = reader.Value;
    if (name == null && str != null)
      name = str;
    IListObject table = sheet.ListObjects.Create(name, range);
    if (reader.MoveToAttribute("id"))
    {
      int val2 = (table as ListObject).Index = XmlConvertExtension.ToInt32(reader.Value);
      WorkbookImpl workbook = sheet.Workbook as WorkbookImpl;
      int maxTableIndex = workbook.MaxTableIndex;
      workbook.MaxTableIndex = Math.Max(maxTableIndex, val2);
    }
    table.DisplayName = str;
    if (reader.MoveToAttribute("tableType"))
      (table as ListObject).TableType = (ExcelTableType) Enum.Parse(typeof (ExcelTableType), reader.Value);
    ListObject listObject = table as ListObject;
    if (reader.MoveToAttribute("headerRowCount"))
      listObject.ShowHeaderRow = XmlConvertExtension.ToBoolean(reader.Value);
    listObject.TotalsRowCount = reader.MoveToAttribute("totalsRowCount") ? XmlConvertExtension.ToInt32(reader.Value) : 0;
    if (reader.MoveToAttribute("dataDxfId"))
      listObject.DataAreaFormatId = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("headerRowBorderDxfId"))
      listObject.HeaderRowBorderFormatId = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("headerRowDxfId"))
      listObject.HeaderRowFormatId = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("tableBorderDxfId"))
      listObject.TableBorderFormatId = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("totalsRowBorderDxfId"))
      listObject.TotalsRowBorderFormatId = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("totalsRowDxfId"))
      listObject.TotalsRowFormatId = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("totalsRowShown"))
    {
      listObject.TotalsRowShown = XmlConvertExtension.ToInt32(reader.Value) != 0;
      listObject.TotalsRowCount = 0;
    }
    else if (listObject.TotalsRowCount != 0)
      listObject.TotalsRowShown = true;
    if (reader.MoveToAttribute("insertRowShift"))
    {
      string s = reader.Value;
      listObject.InsertRowShift = XmlConvertExtension.ToInt32(s);
    }
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "autoFilter":
              this.ParseAutoFilter(reader, table);
              continue;
            case "tableColumns":
              this.ParseColumns(reader, table.Columns);
              continue;
            case "tableStyleInfo":
              this.ParseStyle(reader, table);
              continue;
            case "extLst":
              this.ParseExtensionList(reader, table);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    (table as ListObject).checkAndCloneNameRange(table.Name + "[All]", table.Worksheet.Workbook as WorkbookImpl);
    reader.Read();
    return table;
  }

  private void ParseExtensionList(XmlReader reader, IListObject table)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (table == null)
      throw new ArgumentNullException(nameof (table));
    if (reader.LocalName != "extLst")
      throw new XmlException();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "ext":
              this.ParseExtension(reader, table);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private void ParseExtension(XmlReader reader, IListObject table)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (table == null)
      throw new ArgumentNullException(nameof (table));
    if (reader.LocalName != "ext")
      throw new XmlException();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case nameof (table):
              if (reader.MoveToAttribute("altText"))
                table.AlternativeText = reader.Value;
              if (reader.MoveToAttribute("altTextSummary"))
              {
                table.Summary = reader.Value;
                continue;
              }
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private void ParseAutoFilter(XmlReader reader, IListObject table)
  {
    (table.Worksheet.Workbook as WorkbookImpl).DataHolder.Parser.ParseAutoFilters(reader, (AutoFiltersCollection) table.AutoFilters);
  }

  private void ParseStyle(XmlReader reader, IListObject table)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (table == null)
      throw new ArgumentNullException(nameof (table));
    if (reader.LocalName != "tableStyleInfo")
      throw new XmlException();
    ListObject listObject = (ListObject) table;
    if (reader.MoveToAttribute("name"))
      listObject.TableStyleName = reader.Value;
    if (reader.MoveToAttribute("showFirstColumn"))
      listObject.ShowFirstColumn = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showLastColumn"))
      listObject.ShowLastColumn = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showRowStripes"))
      listObject.ShowTableStyleRowStripes = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showColumnStripes"))
      listObject.ShowTableStyleColumnStripes = XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToElement();
    reader.Skip();
  }

  private void ParseColumns(XmlReader reader, IList<IListObjectColumn> columns)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (columns == null)
      throw new ArgumentNullException(nameof (columns));
    if (reader.LocalName != "tableColumns")
      throw new XmlException();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      int columnIndex = 0;
      for (int count = columns.Count; columnIndex < count; ++columnIndex)
        this.ParseColumn(reader, columns, columnIndex);
    }
    if (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
    reader.Read();
  }

  private void ParseColumn(XmlReader reader, IList<IListObjectColumn> columns, int columnIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
    if (reader.LocalName != "tableColumn")
      throw new XmlException();
    IListObjectColumn column = (IListObjectColumn) null;
    if (reader.MoveToAttribute("id"))
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      column = columns[columnIndex];
      (column as ListObjectColumn).Id = int32;
    }
    if (reader.MoveToAttribute("name"))
    {
      string str = reader.Value;
      if (str.Contains("_x000d_"))
        str = str.Replace("_x000d_", "\r");
      if (str.Contains("_x000a_"))
        str = str.Replace("_x000a_", "\n");
      if (str.Contains("_x0009_"))
        str = str.Replace("_x0009_", "\t");
      if (str.Contains("_x005f\t") || str.Contains("_x005f_x005f_") || str.Contains("_x005f_x005F_"))
        str = str.Replace("_x005f\t", "_x0009_").Replace("_x005f_x005f", "_x005f").Replace("_x005f_x005F", "_x005F");
      (column as ListObjectColumn).SetName(str);
    }
    if (reader.MoveToAttribute("totalsRowLabel"))
      column.TotalsRowLabel = reader.Value;
    if (reader.MoveToAttribute("totalsRowFunction"))
      column.TotalsCalculation = (ExcelTotalsCalculation) Enum.Parse(typeof (ExcelTotalsCalculation), reader.Value, true);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "calculatedColumnFormula":
              if (reader.MoveToAttribute("array"))
              {
                bool boolean = XmlConvertExtension.ToBoolean(reader.Value);
                reader.Read();
                column.CalculatedFormula = reader.Value.Replace("\r", string.Empty);
                (column as ListObjectColumn).IsArrayFormula = boolean;
                reader.Read();
                reader.Skip();
                continue;
              }
              column.CalculatedFormula = reader.ReadElementContentAsString().Replace("\r", string.Empty);
              continue;
            case "xmlColumnPr":
              this.ParseXmlColumnProperties(reader, column as ListObjectColumn);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  internal void ParseXmlColumnProperties(XmlReader reader, ListObjectColumn column)
  {
    if (column == null)
      return;
    if (reader.MoveToAttribute("mapId"))
      column.MapId = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("xpath"))
      column.XPath = reader.Value;
    if (!reader.MoveToAttribute("xmlDataType"))
      return;
    column.XmlDataType = reader.Value;
  }

  public void ParseQueryTable(XmlReader reader, IListObject Table)
  {
    ListObject listObject = Table as ListObject;
    int num = 0;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (Table == null)
      throw new ArgumentNullException("sheet");
    if (reader.LocalName != "queryTable")
      throw new XmlException();
    WorkbookImpl workbook = Table.Location.Worksheet.Workbook as WorkbookImpl;
    int QuertTableId = 0;
    if (reader.MoveToAttribute("connectionId"))
      QuertTableId = reader.ReadContentAsInt();
    ExternalConnection ExternalConnection = this.FindConnection(workbook.Connections as ExternalConnectionCollection, QuertTableId) ?? this.FindConnection(workbook.DeletedConnections as ExternalConnectionCollection, QuertTableId);
    ExternalConnection.Range = Table.Location;
    ExternalConnection.IsExist = true;
    QueryTableImpl queryTable = new QueryTableImpl(Table.Location.Application, Table.Location.Parent, ExternalConnection);
    if (reader.MoveToAttribute("name"))
      queryTable.Name = reader.Value;
    if (reader.MoveToAttribute("growShrinkType"))
      queryTable.GrowShrinkType = (GrowShrinkType) Enum.Parse(typeof (GrowShrinkType), reader.Value);
    if (reader.MoveToAttribute("adjustColumnWidth"))
      queryTable.AdjustColumnWidth = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("backgroundRefresh"))
      queryTable.BackgroundQuery = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
    if (reader.LocalName == "queryTableRefresh")
    {
      queryTable.QueryTableRefresh.PreserveSortFilterLayout = !reader.MoveToAttribute("preserveSortFilterLayout") || XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("nextId"))
        queryTable.QueryTableRefresh.NextId = Convert.ToInt32(reader.Value);
      if (reader.MoveToAttribute("unboundColumnsLeft"))
        queryTable.QueryTableRefresh.UnboundColumnsLeft = Convert.ToInt32(reader.Value);
      if (reader.MoveToAttribute("unboundColumnsRight"))
        queryTable.QueryTableRefresh.UnboundColumnsRight = Convert.ToInt32(reader.Value);
      reader.Read();
    }
    if (reader.LocalName == "queryTableFields")
    {
      if (reader.MoveToAttribute("count"))
      {
        num = reader.ReadContentAsInt();
        reader.Read();
      }
      for (int index = 0; index < num; ++index)
      {
        QueryTableField queryTableField = this.ParseQueryTableField(reader, Table.Columns[index], queryTable);
        queryTable.QueryTableRefresh.QueryFields.Add(queryTableField);
      }
    }
    if (queryTable.Parameters.Count > 0)
    {
      for (int index = 0; index < queryTable.Parameters.Count; ++index)
      {
        ParameterImpl parameter = queryTable.Parameters[index] as ParameterImpl;
        if (queryTable.Parameters[index].Type == ExcelParameterType.Range)
          parameter.SourceRange = ChartParser.GetRange(workbook, parameter.CellRange);
      }
    }
    listObject.QueryTable = queryTable;
  }

  public QueryTableField ParseQueryTableField(
    XmlReader reader,
    IListObjectColumn TableColumn,
    QueryTableImpl queryTable)
  {
    ListObjectColumn listObjectColumn = TableColumn as ListObjectColumn;
    if (reader.MoveToAttribute("id"))
      listObjectColumn.QueryTableFieldId = reader.ReadContentAsInt();
    if (reader.MoveToAttribute("tableColumnId"))
      listObjectColumn.Id = reader.ReadContentAsInt();
    QueryTableField queryTableField = new QueryTableField(listObjectColumn.QueryTableFieldId, listObjectColumn.Id, queryTable.QueryTableRefresh);
    if (reader.MoveToAttribute("name"))
      queryTableField.Name = reader.Value;
    if (reader.MoveToAttribute("dataBound"))
      queryTableField.DataBound = Convert.ToBoolean(Convert.ToInt32(reader.Value));
    reader.Read();
    return queryTableField;
  }

  private ExternalConnection FindConnection(
    ExternalConnectionCollection Connections,
    int QuertTableId)
  {
    for (int i = 0; i < Connections.Count; ++i)
    {
      ExternalConnection connection = Connections[i] as ExternalConnection;
      if ((long) connection.ConncetionId == (long) QuertTableId)
        return connection;
    }
    return (ExternalConnection) null;
  }
}
