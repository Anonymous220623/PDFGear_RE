// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlMap
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Tables;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class XmlMap
{
  internal const string SchemaElement = "Schema";
  internal const string IDAttribute = "ID";
  internal const string MapElement = "Map";
  internal const string NameAttribute = "Name";
  internal const string RootElementAttribute = "RootElement";
  internal const string SchemaIDAttribute = "SchemaID";
  internal const string ShowImportExportValidationErrorsAttribute = "ShowImportExportValidationErrors";
  internal const string AutoFitAttribute = "AutoFit";
  internal const string AppendAttribute = "Append";
  internal const string PreserveSortAFLayoutAttribute = "PreserveSortAFLayout";
  internal const string PreserveFormatAttribute = "PreserveFormat";
  private IWorksheet m_sheet;
  private string m_name;
  private string m_schemaId;
  private string m_rootElement;
  private Stream m_xmlSchema;
  private int m_mapId;

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal IWorksheet Worksheet
  {
    get => this.m_sheet;
    set => this.m_sheet = value;
  }

  internal string SchemaId
  {
    get => this.m_schemaId;
    set => this.m_schemaId = value;
  }

  internal string RootElement
  {
    get => this.m_rootElement;
    set => this.m_rootElement = value;
  }

  internal Stream XmlSchemaData
  {
    get => this.m_xmlSchema;
    set => this.m_xmlSchema = value;
  }

  internal int MapId
  {
    get => this.m_mapId;
    set => this.m_mapId = value;
  }

  internal bool BindXml(
    Stream stream,
    WorkbookImpl workbook,
    IWorksheet sheet,
    int row,
    int column,
    int count)
  {
    DataSet dataSet = new DataSet();
    int num = (int) dataSet.ReadXml(stream);
    if (dataSet.Tables.Count > 0)
    {
      DataTable table = dataSet.Tables[dataSet.Tables.Count - 1];
      for (int index = dataSet.Tables.Count - 2; index >= 0; --index)
        table.Merge(dataSet.Tables[index]);
      stream.Position = 0L;
      XmlSchema xmlSchema = this.ReadSchema(stream);
      if (xmlSchema != null && table != null && !workbook.XmlMaps.Contains(dataSet.DataSetName))
      {
        this.RootElement = this.m_name = dataSet.DataSetName;
        this.Worksheet = sheet;
        this.MapId = count + 1;
        this.RootElement = this.m_name = dataSet.DataSetName;
        this.SchemaId = "Schema" + (object) (workbook.XmlMaps.Count + 1);
        this.Worksheet.ImportDataTable(table, true, row, column);
        int count1 = workbook.InnerNamesColection.Count;
        ListObject listObject = (ListObject) this.m_sheet.ListObjects.Create("Table" + (object) (count1 == 0 ? 1 : count1), this.m_sheet[1, 1, table.Rows.Count + 1, table.Columns.Count]);
        listObject.BuiltInTableStyle = TableBuiltInStyles.TableStyleMedium2;
        listObject.TableType = ExcelTableType.xml;
        this.XmlSchemaData = (Stream) new MemoryStream();
        XmlWriter writer = UtilityMethods.CreateWriter(this.XmlSchemaData, Encoding.UTF8);
        writer.WriteStartElement("root");
        xmlSchema.Write(this.XmlSchemaData);
        writer.WriteEndElement();
        this.XmlSchemaData.Flush();
        for (int index = 0; index < listObject.Columns.Count; ++index)
        {
          if (!this.GetColumnInformation((ListObjectColumn) listObject.Columns[index], xmlSchema))
          {
            this.m_sheet.DeleteColumn(index + column);
            --index;
          }
        }
        return true;
      }
    }
    return false;
  }

  internal bool GetColumnInformation(ListObjectColumn tableColumn, XmlSchema xmlSchema)
  {
    if (xmlSchema.Elements.Values.Count > 0)
    {
      this.FindElement(string.Empty, tableColumn, xmlSchema.Elements.Values);
      if (tableColumn.XPath != null)
        return true;
    }
    return false;
  }

  internal void FindElement(string xpath, ListObjectColumn tableColumn, ICollection collection)
  {
    foreach (XmlSchemaElement xmlSchemaElement in (IEnumerable) collection)
    {
      if (xmlSchemaElement.Name == tableColumn.Name)
      {
        tableColumn.XmlDataType = xmlSchemaElement.SchemaTypeName.Name;
        tableColumn.XPath = $"{xpath}/{tableColumn.Name}";
        tableColumn.MapId = this.m_mapId;
        break;
      }
      if (xmlSchemaElement != null && xmlSchemaElement.ElementSchemaType is XmlSchemaComplexType elementSchemaType)
      {
        XmlSchemaSequence contentTypeParticle = elementSchemaType.ContentTypeParticle as XmlSchemaSequence;
        if (contentTypeParticle.Items.Count > 0)
          this.FindElement($"{xpath}/{xmlSchemaElement.Name}", tableColumn, (ICollection) contentTypeParticle.Items);
      }
    }
  }

  internal XmlSchema ReadSchema(Stream stream)
  {
    XmlReader instanceDocument = XmlReader.Create(stream);
    XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
    ICollection collection = new XmlSchemaInference().InferSchema(instanceDocument).Schemas();
    if (collection.Count > 0)
    {
      IEnumerator enumerator = collection.GetEnumerator();
      try
      {
        if (enumerator.MoveNext())
          return (XmlSchema) enumerator.Current;
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }
    return (XmlSchema) null;
  }

  internal void ParseSchema(XmlReader reader)
  {
    this.XmlSchemaData = (Stream) new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter(this.XmlSchemaData, Encoding.UTF8);
    writer.WriteStartElement("root");
    writer.WriteNode(reader, false);
    writer.WriteEndElement();
    writer.Flush();
  }

  internal void SerializeSchema(XmlWriter writer)
  {
    writer.WriteStartElement("Schema");
    writer.WriteAttributeString("ID", this.SchemaId);
    if (this.XmlSchemaData != null)
      Excel2007Serializator.SerializeStream(writer, this.XmlSchemaData, "root");
    writer.WriteEndElement();
  }

  internal void SerializeMapInformation(XmlWriter writer)
  {
    writer.WriteStartElement("Map");
    writer.WriteAttributeString("ID", this.MapId.ToString());
    writer.WriteAttributeString("Name", this.Name);
    writer.WriteAttributeString("RootElement", this.RootElement);
    writer.WriteAttributeString("SchemaID", this.SchemaId);
    writer.WriteAttributeString("ShowImportExportValidationErrors", "false");
    writer.WriteAttributeString("AutoFit", "true");
    writer.WriteAttributeString("Append", "false");
    writer.WriteAttributeString("PreserveSortAFLayout", "true");
    writer.WriteAttributeString("PreserveFormat", "true");
    writer.WriteEndElement();
  }

  internal void Dispose()
  {
    if (this.m_sheet != null)
      this.m_sheet = (IWorksheet) null;
    if (this.m_name != null)
      this.m_name = (string) null;
    if (this.m_rootElement != null)
      this.m_rootElement = (string) null;
    if (this.m_schemaId != null)
      this.m_schemaId = (string) null;
    if (this.m_xmlSchema == null)
      return;
    this.m_xmlSchema = (Stream) null;
  }
}
