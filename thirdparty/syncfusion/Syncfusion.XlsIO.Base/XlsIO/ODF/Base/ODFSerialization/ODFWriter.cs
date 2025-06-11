// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFSerialization.ODFWriter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.Compression.Zip;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.ODF.Base.ODFImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFSerialization;

internal class ODFWriter
{
  private ZipArchive m_archieve;
  private XmlWriter m_writer;

  public ODFWriter()
  {
    this.m_archieve = new ZipArchive();
    this.m_archieve.DefaultCompressionLevel = CompressionLevel.Best;
  }

  private XmlWriter CreateWriter(Stream data)
  {
    XmlWriter writer = XmlWriter.Create(data, new XmlWriterSettings()
    {
      Encoding = (Encoding) new UTF8Encoding(false)
    });
    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"");
    return writer;
  }

  internal void SaveDocument(string fileName)
  {
    this.m_archieve.Save(fileName, true);
    this.m_archieve.Dispose();
    this.Dispose();
  }

  internal void SaveDocument(Stream stream)
  {
    this.m_archieve.Save(stream, false);
    this.m_archieve.Dispose();
  }

  internal void SerializeDocumentManifest()
  {
    this.m_archieve.AddItem("META-INF/", (Stream) new MemoryStream(), true, FileAttributes.Archive);
    MemoryStream data = new MemoryStream();
    this.m_writer = this.CreateWriter((Stream) data);
    this.m_writer.WriteStartElement("manifest", "manifest", "urn:oasis:names:tc:opendocument:xmlns:manifest:1.0");
    this.m_writer.WriteStartElement("manifest", "file-entry", (string) null);
    this.m_writer.WriteAttributeString("manifest", "full-path", (string) null, "/");
    this.m_writer.WriteAttributeString("manifest", "media-type", (string) null, "application/vnd.oasis.opendocument.spreadsheet");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("manifest", "file-entry", (string) null);
    this.m_writer.WriteAttributeString("manifest", "full-path", (string) null, "styles.xml");
    this.m_writer.WriteAttributeString("manifest", "media-type", (string) null, "text/xml");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("manifest", "file-entry", (string) null);
    this.m_writer.WriteAttributeString("manifest", "full-path", (string) null, "content.xml");
    this.m_writer.WriteAttributeString("manifest", "media-type", (string) null, "text/xml");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("manifest", "file-entry", (string) null);
    this.m_writer.WriteAttributeString("manifest", "full-path", (string) null, "meta.xml");
    this.m_writer.WriteAttributeString("manifest", "media-type", (string) null, "text/xml");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
    this.m_writer.Flush();
    this.m_archieve.AddItem("META-INF/manifest.xml", (Stream) data, false, FileAttributes.Archive);
  }

  internal void SerializeMimeType()
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) memoryStream, (Encoding) new UTF8Encoding(false));
    xmlTextWriter.WriteString("application/vnd.oasis.opendocument.spreadsheet");
    xmlTextWriter.Flush();
    this.m_archieve.AddItem("mimetype", (Stream) memoryStream, false, FileAttributes.Archive);
  }

  internal void SerializeContent(MemoryStream stream)
  {
    this.m_archieve.AddItem("content.xml", (Stream) stream, false, FileAttributes.Archive);
  }

  internal void SerializeMetaData()
  {
    MemoryStream data = new MemoryStream();
    this.m_writer = this.CreateWriter((Stream) data);
    this.m_writer.WriteStartElement("office", "document-meta", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "office", (string) null, "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "dc", (string) null, "http://purl.org/dc/elements/1.1/");
    this.m_writer.WriteAttributeString("xmlns", "xlink", (string) null, "http://www.w3.org/1999/xlink");
    this.m_writer.WriteEndElement();
    this.m_writer.Flush();
    this.m_archieve.AddItem("meta.xml", (Stream) data, false, FileAttributes.Archive);
  }

  internal void SerializeSettings()
  {
    MemoryStream data = new MemoryStream();
    this.m_writer = this.CreateWriter((Stream) data);
    this.m_writer.WriteStartElement("office", "document-settings", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "anim", (string) null, "urn:oasis:names:tc:opendocument:xmlns:animation:1.0");
    this.m_writer.WriteAttributeString("xmlns", "chart", (string) null, "urn:oasis:names:tc:opendocument:xmlns:chart:1.0");
    this.m_writer.WriteAttributeString("xmlns", "onfig", (string) null, "urn:oasis:names:tc:opendocument:xmlns:config:1.0");
    this.m_writer.WriteAttributeString("xmlns", "db", (string) null, "urn:oasis:names:tc:opendocument:xmlns:database:1.0");
    this.m_writer.WriteAttributeString("xmlns", "dr3d", (string) null, "urn:oasis:names:tc:opendocument:xmlns:dr3d:1.0");
    this.m_writer.WriteAttributeString("xmlns", "draw", (string) null, "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
    this.m_writer.WriteAttributeString("xmlns", "fo", (string) null, "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "form", (string) null, "urn:oasis:names:tc:opendocument:xmlns:form:1.0");
    this.m_writer.WriteAttributeString("xmlns", "meta", (string) null, "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
    this.m_writer.WriteAttributeString("xmlns", "number", (string) null, "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
    this.m_writer.WriteAttributeString("xmlns", "office", (string) null, "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "presentation", (string) null, "urn:oasis:names:tc:opendocument:xmlns:presentation:1.0");
    this.m_writer.WriteAttributeString("xmlns", "script", (string) null, "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
    this.m_writer.WriteAttributeString("xmlns", "smil", (string) null, "urn:oasis:names:tc:opendocument:xmlns:smil-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "style", (string) null, "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
    this.m_writer.WriteAttributeString("xmlns", "svg", (string) null, "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "table", (string) null, "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
    this.m_writer.WriteAttributeString("xmlns", "text", (string) null, "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
    this.m_writer.WriteAttributeString("xmlns", "xlink", (string) null, "http://www.w3.org/1999/xlink");
    this.m_writer.WriteAttributeString("xmlns", "xhtml", (string) null, "http://www.w3.org/1999/xhtml");
    this.m_writer.WriteEndElement();
    this.m_writer.Flush();
    this.m_archieve.AddItem("settings.xml", (Stream) data, false, FileAttributes.Archive);
  }

  internal MemoryStream SerializeContentNameSpace()
  {
    MemoryStream data = new MemoryStream();
    this.m_writer = this.CreateWriter((Stream) data);
    this.m_writer.WriteStartElement("office", "document-content", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "table", (string) null, "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
    this.m_writer.WriteAttributeString("xmlns", "office", (string) null, "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "style", (string) null, "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
    this.m_writer.WriteAttributeString("xmlns", "draw", (string) null, "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
    this.m_writer.WriteAttributeString("xmlns", "fo", (string) null, "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "xlink", (string) null, "http://www.w3.org/1999/xlink");
    this.m_writer.WriteAttributeString("xmlns", "dc", (string) null, "http://purl.org/dc/elements/1.1/");
    this.m_writer.WriteAttributeString("xmlns", "number", (string) null, "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
    this.m_writer.WriteAttributeString("xmlns", "svg", (string) null, "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "text", (string) null, "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
    this.m_writer.WriteAttributeString("xmlns", "of", (string) null, "urn:oasis:names:tc:opendocument:xmlns:of:1.2");
    return data;
  }

  internal void SerializeContentEnd(MemoryStream stream)
  {
    this.m_writer.WriteEndElement();
    this.m_writer.Flush();
    this.m_archieve.AddItem("content.xml", (Stream) stream, false, FileAttributes.Archive);
  }

  internal void SerializeBodyStart()
  {
    this.m_writer.WriteStartElement("office", "body", (string) null);
  }

  internal void SerializeDefaultStyles(DefaultStyleCollection defaultStyle)
  {
    int count = defaultStyle.DefaultStyles.Values.Count;
    DefaultStyle[] array = new DefaultStyle[count];
    defaultStyle.DefaultStyles.Values.CopyTo(array, 0);
    if (this.m_writer == null)
      throw new ArgumentNullException("writer");
    for (int index = 0; index < count; ++index)
    {
      DefaultStyle defaultStyle1 = array[index];
      this.m_writer.WriteStartElement("style", "default-style", (string) null);
      this.m_writer.WriteAttributeString("style", "family", (string) null, "paragraph");
      this.SerializeParagraphProperties(defaultStyle1.ParagraphProperties);
      this.SerializeTextProperties(defaultStyle1.Textproperties);
      this.m_writer.WriteEndElement();
    }
  }

  private void SerializeCalculationSettings()
  {
    this.m_writer.WriteStartElement("table", "calculation-settings", (string) null);
    this.m_writer.WriteAttributeString("table", "use-regular-expressions", (string) null, bool.FalseString.ToLower());
    this.m_writer.WriteEndElement();
  }

  internal void SerializeTables(List<OTable> tables)
  {
    for (int index1 = 0; index1 < tables.Count; ++index1)
    {
      OTable table = tables[index1];
      int level = 0;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      int colIncrement = 0;
      this.m_writer.WriteStartElement("table", "table", (string) null);
      this.m_writer.WriteAttributeString("table", "name", (string) null, table.Name);
      this.m_writer.WriteAttributeString("table", "style-name", (string) null, table.StyleName);
      int count1 = table.Columns.Count;
      for (int index2 = 0; index2 < count1; ++index2)
      {
        OTableColumn column = table.Columns[index2];
        OTableColumn nxtColumn = (OTableColumn) null;
        if (index2 + 1 < count1)
          nxtColumn = table.Columns[index2 + 1];
        if (column.OutlineLevel > 0)
        {
          if (!flag1)
          {
            for (int index3 = 0; index3 < column.OutlineLevel; ++index3)
            {
              this.m_writer.WriteStartElement("table", "table-column-group", (string) null);
              ++level;
            }
            flag1 = true;
            flag2 = true;
          }
          if (colIncrement > 0)
          {
            for (int index4 = 0; index4 < colIncrement; ++index4)
            {
              this.m_writer.WriteStartElement("table", "table-column-group", (string) null);
              ++level;
            }
            colIncrement = 0;
          }
        }
        this.m_writer.WriteStartElement("table", "table-column", (string) null);
        if (count1 == 1)
          this.m_writer.WriteAttributeString("table", "number-columns-repeated", (string) null, 16384 /*0x4000*/.ToString());
        if (!string.IsNullOrEmpty(column.StyleName))
          this.m_writer.WriteAttributeString("table", "style-name", (string) null, column.StyleName);
        if (table.HasDefaultColumnStyle)
          this.m_writer.WriteAttributeString("table", "default-cell-style-name", (string) null, string.IsNullOrEmpty(column.DefaultCellStyleName) ? "CE1" : column.DefaultCellStyleName);
        this.m_writer.WriteEndElement();
        if (column.OutlineLevel > 0)
        {
          this.WriteOutlineEnd(column, nxtColumn, ref level, ref colIncrement);
          if (level == 0)
            flag1 = false;
        }
      }
      if (flag2 || table.HasDefaultColumnStyle)
      {
        this.m_writer.WriteStartElement("table", "table-column", (string) null);
        this.m_writer.WriteAttributeString("table", "number-columns-repeated", (string) null, (16384 /*0x4000*/ - count1).ToString());
        if (!flag2)
          this.m_writer.WriteAttributeString("table", "default-cell-style-name", (string) null, "CE1");
        this.m_writer.WriteEndElement();
      }
      int count2 = table.Rows.Count;
      int outlineLevel = 0;
      int increment = 0;
      bool flag4 = false;
      for (int index5 = 0; index5 < count2; ++index5)
      {
        OTableRow row = table.Rows[index5];
        OTableRow nxtRow = (OTableRow) null;
        if (index5 + 1 < count2)
          nxtRow = table.Rows[index5 + 1];
        if (row.OutlineLevel > 0)
        {
          if (!flag3)
          {
            for (int index6 = 0; index6 < row.OutlineLevel; ++index6)
            {
              this.m_writer.WriteStartElement("table", "table-row-group", (string) null);
              ++outlineLevel;
            }
            if (row.IsCollapsed)
              this.m_writer.WriteAttributeString("table", "display", (string) null, "false");
            flag3 = true;
            flag4 = true;
          }
          if (increment > 0)
          {
            for (int index7 = 0; index7 < increment; ++index7)
            {
              this.m_writer.WriteStartElement("table", "table-row-group", (string) null);
              ++outlineLevel;
            }
            if (row.IsCollapsed)
              this.m_writer.WriteAttributeString("table", "display", (string) null, "false");
            increment = 0;
          }
        }
        this.m_writer.WriteStartElement("table", "table-row", (string) null);
        if (!string.IsNullOrEmpty(row.StyleName))
          this.m_writer.WriteAttributeString("table", "style-name", (string) null, row.StyleName);
        if (!string.IsNullOrEmpty(row.DefaultCellStyleName))
          this.m_writer.WriteAttributeString("table", "default-cell-style-name", (string) null, row.DefaultCellStyleName);
        if (row.IsCollapsed)
          this.m_writer.WriteAttributeString("table", "visibility", (string) null, "collapse");
        int count3 = row.Cells.Count;
        OTableCell otableCell = (OTableCell) null;
        for (int index8 = 0; index8 < count3; ++index8)
        {
          otableCell = row.Cells[index8];
          this.m_writer.WriteStartElement("table", "table-cell", (string) null);
          if (!string.IsNullOrEmpty(otableCell.StyleName))
            this.m_writer.WriteAttributeString("table", "style-name", (string) null, otableCell.StyleName);
          if (otableCell.ColumnsSpanned > 1)
            this.m_writer.WriteAttributeString("table", "number-columns-spanned", (string) null, otableCell.ColumnsSpanned.ToString());
          bool flag5 = otableCell.Comment != null;
          if (!otableCell.IsBlank || flag5)
          {
            this.m_writer.WriteAttributeString("office", "value-type", (string) null, flag5 ? "string" : otableCell.Type.ToString().ToLower());
            if (otableCell.TableFormula != null)
              this.m_writer.WriteAttributeString("table", "formula", (string) null, "of:=" + otableCell.TableFormula.ToString());
            if (!otableCell.IsBlank)
              this.WriteCellType(otableCell);
            this.SerializeComments(otableCell.Comment);
            this.SerializeParagraph(otableCell.Paragraph);
          }
          this.m_writer.WriteEndElement();
        }
        this.WriteRepeatedCells(row, otableCell, 16384 /*0x4000*/ - count3);
        this.m_writer.WriteEndElement();
        if (row.OutlineLevel > 0)
        {
          this.WriteOutlineEndRow(row, nxtRow, ref outlineLevel, ref increment);
          if (outlineLevel == 0)
            flag3 = false;
        }
      }
      this.m_writer.WriteStartElement("table", "table-row", (string) null);
      this.m_writer.WriteAttributeString("table", "number-rows-repeated", (string) null, (1048576 /*0x100000*/ - count2).ToString());
      this.m_writer.WriteAttributeString("table", "style-name", (string) null, "ro1");
      this.m_writer.WriteStartElement("table", "table-cell", (string) null);
      this.m_writer.WriteAttributeString("table", "number-columns-repeated", (string) null, 16384 /*0x4000*/.ToString());
      this.m_writer.WriteEndElement();
      this.m_writer.WriteEndElement();
      this.m_writer.WriteEndElement();
    }
  }

  private void WriteOutlineEnd(
    OTableColumn column,
    OTableColumn nxtColumn,
    ref int level,
    ref int colIncrement)
  {
    if (nxtColumn != null)
    {
      int num = nxtColumn.OutlineLevel - column.OutlineLevel;
      if (num < 0)
      {
        for (int index = 0; index < Math.Abs(num); ++index)
        {
          this.m_writer.WriteEndElement();
          --level;
        }
      }
      else if (num > 0)
        colIncrement = num;
    }
    if (nxtColumn != null && nxtColumn.OutlineLevel != 0)
      return;
    for (int index = 0; index < level; ++index)
      this.m_writer.WriteEndElement();
  }

  private void WriteOutlineEndRow(
    OTableRow curRow,
    OTableRow nxtRow,
    ref int outlineLevel,
    ref int increment)
  {
    if (nxtRow != null)
    {
      int num = nxtRow.OutlineLevel - curRow.OutlineLevel;
      if (num < 0)
      {
        for (int index = 0; index < Math.Abs(num); ++index)
        {
          this.m_writer.WriteEndElement();
          --outlineLevel;
        }
      }
      else if (nxtRow.OutlineLevel > curRow.OutlineLevel)
        increment = nxtRow.OutlineLevel - curRow.OutlineLevel;
    }
    if (nxtRow != null && (nxtRow == null || nxtRow.OutlineLevel != 0))
      return;
    for (int index = 0; index < outlineLevel; ++index)
      this.m_writer.WriteEndElement();
  }

  private void SerilalizeNamedRanges(List<OTable> tables)
  {
    this.m_writer.WriteStartElement("table", "named-expressions", (string) null);
    for (int index1 = 0; index1 < tables.Count; ++index1)
    {
      NamedExpressions expressions = tables[index1].Expressions;
      if (expressions != null)
      {
        for (int index2 = 0; index2 < expressions.NamedRanges.Count; ++index2)
        {
          NamedRange namedRange = expressions.NamedRanges[index2];
          this.m_writer.WriteStartElement("table", "named-range", (string) null);
          this.m_writer.WriteAttributeString("table", "name", (string) null, namedRange.Name);
          this.m_writer.WriteAttributeString("table", "cell-range-address", (string) null, namedRange.CellRangeAddress);
          this.m_writer.WriteAttributeString("table", "base-cell-address", (string) null, namedRange.BaseCellAddress);
          this.m_writer.WriteEndElement();
        }
      }
    }
    this.m_writer.WriteEndElement();
  }

  private void WriteCellType(OTableCell curCell)
  {
    switch (curCell.Type)
    {
      case CellValueType.Float:
      case CellValueType.Percentage:
      case CellValueType.Currency:
        this.m_writer.WriteAttributeString("office", "value", (string) null, curCell.Value != null ? curCell.Value.ToString() : string.Empty);
        break;
      case CellValueType.Date:
        this.m_writer.WriteAttributeString("office", "date-value", (string) null, curCell.DateValue.ToString("yyyy-MM-ddTHH:mm:ss"));
        break;
      case CellValueType.Time:
        this.m_writer.WriteAttributeString("office", "time-value", (string) null, ODFWriter.ToReadableString(curCell.TimeValue));
        break;
      case CellValueType.Boolean:
        this.m_writer.WriteAttributeString("office", "boolean-value", (string) null, curCell.BooleanValue.ToString());
        break;
    }
  }

  private void WriteRepeatedCells(OTableRow row, OTableCell cell, int colsRepeated)
  {
    if (cell == null)
      return;
    this.m_writer.WriteStartElement("table", "table-cell", (string) null);
    if (cell.ColumnsRepeated != 0 && cell.ColumnsRepeated > 1)
      this.m_writer.WriteAttributeString("table", "number-columns-repeated", (string) null, cell.ColumnsRepeated.ToString());
    else
      this.m_writer.WriteAttributeString("table", "number-columns-repeated", (string) null, colsRepeated.ToString());
    if (!string.IsNullOrEmpty(row.DefaultCellStyleName))
      this.m_writer.WriteAttributeString("table", "style-name", (string) null, row.DefaultCellStyleName);
    this.m_writer.WriteEndElement();
  }

  private void SerializeParagraph(OParagraph para)
  {
    if (para == null)
      return;
    this.m_writer.WriteStartElement("text", "p", (string) null);
    if (para.StyleName != null)
      this.m_writer.WriteAttributeString("text", "style-name", (string) null, para.StyleName);
    if (para.OParagraphItemCollection.Count > 0)
    {
      for (int index = 0; index < para.OParagraphItemCollection.Count; ++index)
      {
        OParagraphItem oparagraphItem = para.OParagraphItemCollection[index];
        if (oparagraphItem != null)
        {
          this.m_writer.WriteStartElement("text", "span", (string) null);
          if (oparagraphItem.StyleName != null)
            this.m_writer.WriteAttributeString("text", "style-name", (string) null, oparagraphItem.StyleName);
          this.m_writer.WriteString(oparagraphItem.Text);
          this.m_writer.WriteEndElement();
        }
      }
    }
    else
    {
      if (para.Anchor != null)
      {
        this.m_writer.WriteStartElement("text", "a", (string) null);
        this.m_writer.WriteAttributeString("xlink", "href", (string) null, para.Anchor.href);
      }
      if (para.TextInput != null)
        this.m_writer.WriteString(ODFWriter.ReplaceHexadecimalSymbols(para.TextInput));
      if (para.Anchor != null)
        this.m_writer.WriteEndElement();
    }
    this.m_writer.WriteEndElement();
  }

  private static string ReplaceHexadecimalSymbols(string txt)
  {
    if (!txt.Contains("_") || !txt.Contains("x"))
      return ODFWriter.ReplaceHexSymbols(txt);
    string[] strArray = txt.Split(new char[1]{ '_' }, StringSplitOptions.RemoveEmptyEntries);
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (!ODFWriter.IsHexa(strArray[index]))
        stringBuilder.Append(strArray[index]);
    }
    return stringBuilder.ToString();
  }

  private static string ReplaceHexSymbols(string txt)
  {
    string pattern = "[\0-\b\v\f\u000E-\u001F&]";
    return Regex.Replace(txt, pattern, "", RegexOptions.Compiled);
  }

  private static bool IsHexa(string value)
  {
    if (!value.Contains("x"))
      return false;
    value = value.Replace("x", string.Empty);
    return int.TryParse(value, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out int _);
  }

  internal void SerializeExcelBody(List<OTable> tables)
  {
    this.m_writer.WriteStartElement("office", "spreadsheet", (string) null);
    this.SerializeCalculationSettings();
    this.SerializeTables(tables);
    this.SerilalizeNamedRanges(tables);
    this.m_writer.WriteEndElement();
  }

  internal MemoryStream SerializeStyleStart()
  {
    MemoryStream data = new MemoryStream();
    this.m_writer = this.CreateWriter((Stream) data);
    this.m_writer.WriteStartElement("office", "document-styles", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "table", (string) null, "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
    this.m_writer.WriteAttributeString("xmlns", "office", (string) null, "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
    this.m_writer.WriteAttributeString("xmlns", "style", (string) null, "urn:oasis:names:tc:opendocument:xmlns:style:1.0");
    this.m_writer.WriteAttributeString("xmlns", "draw", (string) null, "urn:oasis:names:tc:opendocument:xmlns:drawing:1.0");
    this.m_writer.WriteAttributeString("xmlns", "fo", (string) null, "urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "xlink", (string) null, "http://www.w3.org/1999/xlink");
    this.m_writer.WriteAttributeString("xmlns", "dc", (string) null, "http://purl.org/dc/elements/1.1/");
    this.m_writer.WriteAttributeString("xmlns", "number", (string) null, "urn:oasis:names:tc:opendocument:xmlns:datastyle:1.0");
    this.m_writer.WriteAttributeString("xmlns", "svg", (string) null, "urn:oasis:names:tc:opendocument:xmlns:svg-compatible:1.0");
    this.m_writer.WriteAttributeString("xmlns", "of", (string) null, "urn:oasis:names:tc:opendocument:xmlns:of:1.2");
    return data;
  }

  internal void SerializeStylesEnd(MemoryStream stream)
  {
    this.m_writer.WriteEndElement();
    this.m_writer.Flush();
    this.m_archieve.AddItem("styles.xml", (Stream) stream, false, FileAttributes.Archive);
  }

  public static string ToReadableString(TimeSpan span)
  {
    return "PT" + $"{(span.TotalHours > 0.0 ? (object) $"{span.TotalHours}{"H"}" : (object) string.Empty)}{(span.TotalMinutes > 0.0 ? (object) $"{span.Minutes}{"M"}" : (object) string.Empty)}{(span.TotalSeconds > 0.0 ? (object) $"{span.Seconds}{"S"}" : (object) string.Empty)}";
  }

  internal void SerializeFontFaceDecls(List<FontFace> fonts)
  {
    if (this.m_writer == null)
      throw new ArgumentNullException("writer");
    if (fonts == null)
      return;
    this.m_writer.WriteStartElement("office", "font-face-decls", (string) null);
    for (int index = 0; index < fonts.Count; ++index)
      this.SerializeFontface(fonts[index]);
    this.m_writer.WriteEndElement();
  }

  internal void SerializeFontface(FontFace font)
  {
    if (this.m_writer == null)
      throw new ArgumentNullException("writer");
    this.m_writer.WriteStartElement("style", "font-face", (string) null);
    this.m_writer.WriteAttributeString("style", "name", (string) null, font.Name);
    this.m_writer.WriteAttributeString("svg", "font-family", (string) null, font.Name.ToString());
    this.m_writer.WriteEndElement();
  }

  internal void SerializeDataStyles(ODFStyleCollection styles)
  {
    this.SerializeCommonStyles(styles);
  }

  internal void SerializeDataStylesStart()
  {
    this.m_writer.WriteStartElement("office", "styles", (string) null);
  }

  internal void SerializeGeneralStyle(NumberStyle style)
  {
    if (style.Number.nFormatFlags == (byte) 0)
      return;
    this.m_writer.WriteStartElement("number", "number", (string) null);
    this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, style.Number.MinIntegerDigits.ToString());
    this.m_writer.WriteEndElement();
  }

  internal void SerializeNumberStyle(DataStyle nFormat)
  {
    switch (nFormat)
    {
      case NumberStyle _:
        NumberStyle numberStyle = nFormat as NumberStyle;
        this.m_writer.WriteStartElement("number", "number-style", (string) null);
        this.m_writer.WriteAttributeString("style", "name", (string) null, numberStyle.Name);
        if (numberStyle.ScientificNumber.nFormatFlags != (byte) 0)
        {
          this.m_writer.WriteStartElement("number", "scientific-number", (string) null);
          this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, numberStyle.ScientificNumber.MinIntegerDigits.ToString());
          this.m_writer.WriteAttributeString("number", "decimal-places", (string) null, numberStyle.ScientificNumber.DecimalPlaces.ToString());
          this.m_writer.WriteAttributeString("number", "min-exponent-digits", (string) null, numberStyle.ScientificNumber.MinExponentDigits.ToString());
          this.m_writer.WriteAttributeString("number", "grouping", (string) null, numberStyle.ScientificNumber.Grouping.ToString().ToLower());
          this.m_writer.WriteEndElement();
        }
        else if (numberStyle.Number.nFormatFlags != (byte) 0)
        {
          this.m_writer.WriteStartElement("number", "number", (string) null);
          this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, numberStyle.Number.MinIntegerDigits.ToString());
          this.m_writer.WriteAttributeString("number", "decimal-places", (string) null, numberStyle.Number.DecimalPlaces.ToString());
          this.m_writer.WriteAttributeString("number", "grouping", (string) null, numberStyle.Number.Grouping.ToString().ToLower());
          this.m_writer.WriteEndElement();
        }
        else if (numberStyle.Fraction.nFormatFlags != (byte) 0)
        {
          this.m_writer.WriteStartElement("number", "fraction", (string) null);
          this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, numberStyle.Fraction.MinIntegerDigits.ToString());
          this.m_writer.WriteAttributeString("number", "min-numerator-digits", (string) null, numberStyle.Fraction.MinNumeratorDigits.ToString());
          if (numberStyle.Fraction.DenominatorValue > 0)
            this.m_writer.WriteAttributeString("number", "denominator-value", (string) null, numberStyle.Fraction.DenominatorValue.ToString().ToLower());
          else
            this.m_writer.WriteAttributeString("number", "min-denominator-digits", (string) null, numberStyle.Fraction.MinDenominatorDigits.ToString().ToLower());
          this.m_writer.WriteEndElement();
        }
        this.m_writer.WriteEndElement();
        break;
      case PercentageStyle _:
        PercentageStyle percentageStyle = nFormat as PercentageStyle;
        this.m_writer.WriteStartElement("number", "percentage-style", (string) null);
        this.m_writer.WriteAttributeString("style", "name", (string) null, percentageStyle.Name);
        this.m_writer.WriteStartElement("number", "number", (string) null);
        this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, percentageStyle.Number.MinIntegerDigits.ToString());
        this.m_writer.WriteAttributeString("number", "decimal-places", (string) null, percentageStyle.Number.DecimalPlaces.ToString());
        this.m_writer.WriteAttributeString("number", "grouping", (string) null, percentageStyle.Number.Grouping.ToString().ToLower());
        this.m_writer.WriteEndElement();
        this.m_writer.WriteEndElement();
        break;
      case CurrencyStyle _:
        CurrencyStyle style = nFormat as CurrencyStyle;
        this.m_writer.WriteStartElement("number", "currency-style", (string) null);
        this.m_writer.WriteAttributeString("style", "name", (string) null, style.Name);
        this.m_writer.WriteStartElement("number", "currency-symbol", (string) null);
        this.m_writer.WriteString(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol);
        this.m_writer.WriteEndElement();
        this.SerializeNumberToken(style);
        if (style.HasSections)
        {
          for (int index = 0; index < style.Map.Count; ++index)
          {
            this.m_writer.WriteStartElement("style", "map", (string) null);
            this.m_writer.WriteAttributeString("style", "condition", (string) null, style.Map[index].Condition);
            this.m_writer.WriteAttributeString("style", "apply-style-name", (string) null, style.Map[index].ApplyStyleName);
            this.m_writer.WriteEndElement();
          }
        }
        this.m_writer.WriteEndElement();
        break;
      case TextStyle _:
        TextStyle textStyle = nFormat as TextStyle;
        this.m_writer.WriteStartElement("number", "text-style", (string) null);
        this.m_writer.WriteAttributeString("style", "name", (string) null, textStyle.Name);
        if (textStyle.TextContent)
          this.m_writer.WriteElementString("number", "text-content", (string) null, string.Empty);
        if (textStyle.HasSections)
        {
          for (int index = 0; index < textStyle.Map.Count; ++index)
          {
            this.m_writer.WriteStartElement("style", "map", (string) null);
            this.m_writer.WriteAttributeString("style", "condition", (string) null, textStyle.Map[index].Condition);
            this.m_writer.WriteAttributeString("style", "apply-style-name", (string) null, textStyle.Map[index].ApplyStyleName);
            this.m_writer.WriteEndElement();
          }
        }
        this.m_writer.WriteEndElement();
        break;
      case DateStyle _:
        DateStyle dateStyle = nFormat as DateStyle;
        this.m_writer.WriteStartElement("number", "date-style", (string) null);
        this.m_writer.WriteAttributeString("style", "name", (string) null, dateStyle.Name);
        this.SerializeDateToken();
        this.m_writer.WriteEndElement();
        break;
    }
  }

  internal void SerializeNumberToken(CurrencyStyle style)
  {
    this.m_writer.WriteStartElement("number", "number", (string) null);
    this.m_writer.WriteAttributeString("number", "min-integer-digits", (string) null, style.Number.MinIntegerDigits.ToString());
    this.m_writer.WriteAttributeString("number", "decimal-places", (string) null, style.Number.DecimalPlaces.ToString());
    this.m_writer.WriteAttributeString("number", "grouping", (string) null, style.Number.Grouping.ToString().ToLower());
    this.m_writer.WriteEndElement();
  }

  internal void SerializeDateToken()
  {
    this.m_writer.WriteElementString("number", "month", (string) null, string.Empty);
    this.m_writer.WriteElementString("number", "text", (string) null, RangeImpl.GetDateSeperator());
    this.m_writer.WriteElementString("number", "day", (string) null, string.Empty);
    this.m_writer.WriteElementString("number", "text", (string) null, RangeImpl.GetDateSeperator());
    this.m_writer.WriteStartElement("number", "year", (string) null);
    this.m_writer.WriteAttributeString("number", "style", (string) null, "long");
    this.m_writer.WriteEndElement();
  }

  internal void SerializeCommonStyles(ODFStyleCollection styles)
  {
    if (this.m_writer == null)
      throw new ArgumentNullException("writer");
    this.SerializeODFStyles(styles);
  }

  internal void SerializeODFStyles(ODFStyleCollection ODFStyles)
  {
    int count = ODFStyles.DictStyles.Values.Count;
    ODFStyle[] array = new ODFStyle[count];
    ODFStyles.DictStyles.Values.CopyTo(array, 0);
    for (int index = 0; index < count; ++index)
    {
      ODFStyle odfStyle = array[index];
      this.m_writer.WriteStartElement("style", "style", (string) null);
      this.m_writer.WriteAttributeString("style", "name", (string) null, odfStyle.Name.Replace('_', '-'));
      this.m_writer.WriteAttributeString("style", "family", (string) null, odfStyle.Family.ToString().ToLower().Replace('_', '-'));
      switch (odfStyle.Family)
      {
        case ODFFontFamily.Paragraph:
          if (!string.IsNullOrEmpty(odfStyle.ParentStyleName))
            this.m_writer.WriteAttributeString("style", "parent-style-name", (string) null, odfStyle.ParentStyleName);
          if (odfStyle.MasterPageName != null)
            this.m_writer.WriteAttributeString("style", "master-page-name", (string) null, odfStyle.MasterPageName);
          this.SerializeParagraphProperties(odfStyle.ParagraphProperties);
          break;
        case ODFFontFamily.Table:
          this.m_writer.WriteAttributeString("style", "master-page-name", (string) null, odfStyle.MasterPageName);
          this.SerializeTableProperties(odfStyle.TableProperties);
          break;
        case ODFFontFamily.Table_Column:
          this.SerializeColumnProprties(odfStyle.TableColumnProperties);
          break;
        case ODFFontFamily.Table_Row:
          this.SerializeRowProprties(odfStyle.TableRowProperties);
          break;
        case ODFFontFamily.Table_Cell:
          if (!string.IsNullOrEmpty(odfStyle.ParentStyleName))
            this.m_writer.WriteAttributeString("style", "parent-style-name", (string) null, odfStyle.ParentStyleName);
          this.m_writer.WriteAttributeString("style", "data-style-name", (string) null, odfStyle.DataStyleName);
          this.SerializeExcelTableCellProperties(odfStyle.TableCellProperties);
          this.SerializeParagraphProperties(odfStyle.ParagraphProperties);
          break;
        case ODFFontFamily.Section:
          this.SerializeSectionProperties(odfStyle.ODFSectionProperties);
          break;
        case ODFFontFamily.Graphic:
          this.SerializeGraphicProperties(odfStyle.GraphicProperties);
          break;
      }
      if (odfStyle.Textproperties != null && odfStyle.Textproperties.CharStyleName != null)
        this.m_writer.WriteAttributeString("style", "parent-style-name", (string) null, odfStyle.Textproperties.CharStyleName);
      this.SerializeTextProperties(odfStyle.Textproperties);
      this.m_writer.WriteEndElement();
    }
  }

  internal void SerializeGraphicProperties(GraphicProperties shapeProp)
  {
    this.m_writer.WriteStartElement("style", "style", (string) null);
    this.m_writer.WriteAttributeString("draw", "stroke", (string) null, shapeProp.Stroke.ToString().ToLower());
    this.m_writer.WriteAttributeString("svg", "stroke-color", (string) null, ODFWriter.HexConverter(shapeProp.StrokeColor));
    if (shapeProp.Fill != FillType.None)
    {
      this.m_writer.WriteAttributeString("draw", "fill", (string) null, shapeProp.Fill.ToString().ToLower());
      this.m_writer.WriteAttributeString("draw", "fill-color", (string) null, ODFWriter.HexConverter(shapeProp.FillColor));
    }
    this.m_writer.WriteEndElement();
  }

  internal void SerializeTableDefaultStyle()
  {
    this.m_writer.WriteStartElement("style", "default-style", (string) null);
    this.m_writer.WriteAttributeString("style", "family", (string) null, "table");
    this.m_writer.WriteStartElement("style", "table-properties", (string) null);
    this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, "0in");
    this.m_writer.WriteAttributeString("table", "border-model", (string) null, "collapsing");
    this.m_writer.WriteAttributeString("style", "writing-mode", (string) null, "lr-tb");
    this.m_writer.WriteAttributeString("table", "align", (string) null, "left");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("style", "default-style", (string) null);
    this.m_writer.WriteAttributeString("style", "family", (string) null, "table-column");
    this.m_writer.WriteStartElement("style", "table-column-properties", (string) null);
    this.m_writer.WriteAttributeString("style", "use-optimal-column-width", (string) null, "true");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("style", "default-style", (string) null);
    this.m_writer.WriteAttributeString("style", "family", (string) null, "table-row");
    this.m_writer.WriteStartElement("style", "table-row-properties", (string) null);
    this.m_writer.WriteAttributeString("style", "min-row-height", (string) null, "0in");
    this.m_writer.WriteAttributeString("style", "use-optimal-column-height", (string) null, "true");
    this.m_writer.WriteAttributeString("fo", "keep-together", (string) null, "auto");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
    this.m_writer.WriteStartElement("style", "default-style", (string) null);
    this.m_writer.WriteAttributeString("style", "family", (string) null, "table-cell");
    this.m_writer.WriteStartElement("style", "table-cell-properties", (string) null);
    this.m_writer.WriteAttributeString("fo", "background-color", (string) null, "transparent");
    this.m_writer.WriteAttributeString("style", "glyph-orientation-vertical", (string) null, "auto");
    this.m_writer.WriteAttributeString("fo", "vertical-align", (string) null, "top");
    this.m_writer.WriteAttributeString("fo", "wrap-option", (string) null, "wrap");
    this.m_writer.WriteEndElement();
    this.m_writer.WriteEndElement();
  }

  internal void SerializeDefaultGraphicStyle()
  {
    this.m_writer.WriteStartElement("style", "default-style", (string) null);
    this.m_writer.WriteAttributeString("style", "family", (string) null, "graphic");
    this.m_writer.WriteAttributeString("draw", "fill", (string) null, "solid");
    this.m_writer.WriteAttributeString("draw", "fill-color", (string) null, "#5b9bd5");
    this.m_writer.WriteAttributeString("draw", "opacity", (string) null, "100%");
    this.m_writer.WriteAttributeString("draw", "stroke", (string) null, "solid");
    this.m_writer.WriteAttributeString("draw", "stroke-width", (string) null, "0.01389in");
    this.m_writer.WriteAttributeString("svg", "stroke-color", (string) null, "#41719c");
    this.m_writer.WriteAttributeString("draw", "stoke-opacity", (string) null, "100%");
    this.m_writer.WriteAttributeString("draw", "stroke-linejoin", (string) null, "miter");
    this.m_writer.WriteAttributeString("draw", "stroke-linecap", (string) null, "butt");
    this.m_writer.WriteEndElement();
  }

  private void SerializeTableProperties(OTableProperties tableProp)
  {
    this.m_writer.WriteStartElement("style", "table-properties", (string) null);
    this.m_writer.WriteAttributeString("table", "display", (string) null, true.ToString().ToLower());
    this.m_writer.WriteAttributeString("style", "writing-mode", (string) null, tableProp.WritingMode.ToString().Replace('_', '-'));
    this.m_writer.WriteEndElement();
  }

  private void SerializeColumnProprties(OTableColumnProperties tableColumnProp)
  {
    this.m_writer.WriteStartElement("style", "table-column-properties", (string) null);
    this.m_writer.WriteAttributeString("style", "column-width", (string) null, tableColumnProp.ColumnWidth.ToString() + "cm");
    this.m_writer.WriteEndElement();
  }

  private void SerializeRowProprties(OTableRowProperties tableRowProp)
  {
    this.m_writer.WriteStartElement("style", "table-row-properties", (string) null);
    this.m_writer.WriteAttributeString("style", "row-height", (string) null, tableRowProp.RowHeight.ToString() + "pt");
    this.m_writer.WriteEndElement();
  }

  private void SerializeSectionProperties(SectionProperties sectionProps)
  {
    if (sectionProps == null)
      return;
    this.m_writer.WriteStartElement("style", "section-properties", (string) null);
    this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, sectionProps.MarginLeft.ToString() + "in");
    this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, sectionProps.MarginRight.ToString() + "in");
    this.m_writer.WriteAttributeString("style", "writing-mode", (string) null, "lr-tb");
    this.m_writer.WriteEndElement();
  }

  private void SerializeParagraphProperties(ODFParagraphProperties paraProp)
  {
    if (paraProp == null)
      return;
    this.m_writer.WriteStartElement("style", "paragraph-properties", (string) null);
    if (paraProp.HasKey(9, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "keep-together", (string) null, paraProp.KeepTogether.ToString());
    if (paraProp.HasKey(3, (int) paraProp.m_CommonstyleFlags))
      this.m_writer.WriteAttributeString("fo", "keep-with-next", (string) null, paraProp.KeepWithNext.ToString());
    if (paraProp.BeforeBreak == BeforeBreak.page)
      this.m_writer.WriteAttributeString("fo", "break-before", (string) null, "page");
    if (paraProp.BeforeBreak == BeforeBreak.column)
      this.m_writer.WriteAttributeString("fo", "break-before", (string) null, "column");
    if (paraProp.HasKey(2, (int) paraProp.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-top", (string) null, paraProp.MarginTop.ToString() + "in");
    if (paraProp.HasKey(3, (int) paraProp.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-bottom", (string) null, paraProp.MarginBottom.ToString() + "in");
    if (paraProp.HasKey(0, (int) paraProp.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, paraProp.MarginBottom.ToString() + "in");
    if (paraProp.HasKey(1, (int) paraProp.m_marginFlag))
      this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, paraProp.MarginRight.ToString() + "in");
    if (paraProp.HasKey(0, (int) paraProp.borderFlags) && paraProp.Border != null)
    {
      this.m_writer.WriteAttributeString("fo", "border", (string) null, $"{paraProp.Border.LineWidth}in {(object) paraProp.Border.LineStyle} {ODFWriter.HexConverter(paraProp.Border.LineColor)}");
      this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, paraProp.PaddingLeft.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, paraProp.PaddingRight.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, paraProp.PaddingTop.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, paraProp.PaddingBottom.ToString() + "in");
    }
    if (paraProp.HasKey(1, (int) paraProp.borderFlags) || paraProp.HasKey(2, (int) paraProp.borderFlags) || paraProp.HasKey(3, (int) paraProp.borderFlags) || paraProp.HasKey(4, (int) paraProp.borderFlags))
    {
      if (paraProp.BorderLeft != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-left", (string) null, $"{paraProp.BorderLeft.LineWidth}in {(object) paraProp.BorderLeft.LineStyle} {ODFWriter.HexConverter(paraProp.BorderLeft.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, paraProp.PaddingLeft.ToString() + "in");
      }
      if (paraProp.BorderRight != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-right", (string) null, $"{paraProp.BorderRight.LineWidth}in {(object) paraProp.BorderRight.LineStyle} {ODFWriter.HexConverter(paraProp.BorderRight.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, paraProp.PaddingRight.ToString() + "in");
      }
      if (paraProp.BorderTop != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-top", (string) null, $"{paraProp.BorderTop.LineWidth}in {(object) paraProp.BorderTop.LineStyle} {ODFWriter.HexConverter(paraProp.BorderTop.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, paraProp.PaddingTop.ToString() + "in");
      }
      if (paraProp.BorderBottom != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-bottom", (string) null, $"{paraProp.BorderBottom.LineWidth}in {(object) paraProp.BorderBottom.LineStyle} {ODFWriter.HexConverter(paraProp.BorderBottom.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, paraProp.PaddingBottom.ToString() + "in");
      }
    }
    if (paraProp.HasKey(21, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "text-align", (string) null, paraProp.TextAlign.ToString().ToLower());
    if (paraProp.HasKey(6, (int) paraProp.m_CommonstyleFlags))
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, paraProp.BackgroundColor);
    if (paraProp.HasKey(19, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "text-indent", (string) null, paraProp.TextIndent.ToString() + "in");
    if (paraProp.HasKey(10, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("style", "line-height-at-least", (string) null, paraProp.LineHeightAtLeast.ToString() + "in");
    if (paraProp.HasKey(28, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "line-height", (string) null, paraProp.LineHeight.ToString() + "in");
    if (paraProp.HasKey(9, paraProp.m_styleFlag1))
      this.m_writer.WriteAttributeString("fo", "line-height", (string) null, paraProp.LineSpacing.ToString() + "%");
    if (paraProp.HasKey(0, (int) paraProp.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-top", (string) null, paraProp.BeforeSpacing.ToString() + "in");
    if (paraProp.HasKey(1, (int) paraProp.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-bottom", (string) null, paraProp.AfterSpacing.ToString() + "in");
    if (paraProp.HasKey(2, (int) paraProp.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, paraProp.LeftIndent.ToString() + "in");
    if (paraProp.HasKey(3, (int) paraProp.m_styleFlag2))
      this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, paraProp.RightIndent.ToString() + "in");
    this.m_writer.WriteEndElement();
  }

  private void SerializeExcelTableCellProperties(OTableCellProperties cellProp)
  {
    if (cellProp == null)
      return;
    this.m_writer.WriteStartElement("style", "table-cell-properties", (string) null);
    VerticalAlign? verticalAlign = cellProp.VerticalAlign;
    if (verticalAlign.HasValue)
      this.m_writer.WriteAttributeString("style", "vertical-align", (string) null, verticalAlign.ToString());
    if (cellProp.HasKey(8, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, cellProp.BackColor == Color.Transparent ? cellProp.BackColor.Name.ToString().ToLower() : ODFWriter.HexConverter(cellProp.BackColor));
    if (cellProp.HasKey(7, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("style", "shrink-to-fit", (string) null, true.ToString().ToLower());
    bool flag = false;
    if (cellProp.BorderLeft != null && cellProp.BorderRight != null && cellProp.BorderBottom != null && cellProp.BorderRight != null)
      flag = cellProp.BorderLeft.Equals((object) cellProp.BorderRight) && cellProp.BorderRight.Equals((object) cellProp.BorderTop) && cellProp.BorderTop.Equals((object) cellProp.BorderBottom);
    if (cellProp.Border != null || flag)
    {
      ODFBorder borderLeft = cellProp.BorderLeft;
      if (borderLeft != null && borderLeft.LineStyle != BorderLineStyle.none)
        this.m_writer.WriteAttributeString("fo", "border", (string) null, $"{borderLeft.LineWidth} {borderLeft.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(borderLeft.LineColor)}");
    }
    else
    {
      if (cellProp.BorderLeft != null)
      {
        ODFBorder borderLeft = cellProp.BorderLeft;
        if (borderLeft != null && borderLeft.LineStyle != BorderLineStyle.none)
          this.m_writer.WriteAttributeString("fo", "border-left", (string) null, $"{borderLeft.LineWidth} {borderLeft.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(borderLeft.LineColor)}");
      }
      if (cellProp.BorderRight != null)
      {
        ODFBorder borderRight = cellProp.BorderRight;
        if (borderRight != null && borderRight.LineStyle != BorderLineStyle.none)
          this.m_writer.WriteAttributeString("fo", "border-right", (string) null, $"{borderRight.LineWidth} {borderRight.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(borderRight.LineColor)}");
      }
      if (cellProp.BorderTop != null)
      {
        ODFBorder borderTop = cellProp.BorderTop;
        if (borderTop != null && borderTop.LineStyle != BorderLineStyle.none)
          this.m_writer.WriteAttributeString("fo", "border-top", (string) null, $"{borderTop.LineWidth} {borderTop.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(borderTop.LineColor)}");
      }
      if (cellProp.BorderBottom != null)
      {
        ODFBorder borderBottom = cellProp.BorderBottom;
        if (borderBottom != null && borderBottom.LineStyle != BorderLineStyle.none)
          this.m_writer.WriteAttributeString("fo", "border-bottom", (string) null, $"{borderBottom.LineWidth} {borderBottom.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(borderBottom.LineColor)}");
      }
    }
    if (cellProp.DiagonalLeft != null)
    {
      ODFBorder diagonalLeft = cellProp.DiagonalLeft;
      if (diagonalLeft != null && diagonalLeft.LineStyle != BorderLineStyle.none)
        this.m_writer.WriteAttributeString("style", "diagonal-tl-br", (string) null, $"{diagonalLeft.LineWidth} {diagonalLeft.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(diagonalLeft.LineColor)}");
    }
    if (cellProp.DiagonalRight != null)
    {
      ODFBorder diagonalRight = cellProp.DiagonalRight;
      if (diagonalRight != null && diagonalRight.LineStyle != BorderLineStyle.none)
        this.m_writer.WriteAttributeString("style", "diagonal-bl-tr", (string) null, $"{diagonalRight.LineWidth} {diagonalRight.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(diagonalRight.LineColor)}");
    }
    if (cellProp.HasKey(1, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("fo", "wrap-option", (string) null, "wrap");
    if (cellProp.HasKey(0, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("style", "rotation-angle", (string) null, cellProp.RotationAngle.ToString());
    if (cellProp.HasKey(15, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("style", "direction", (string) null, cellProp.Direction.ToString());
    if (cellProp.HasKey(14, (int) cellProp.tableCellFlags) && cellProp.RepeatContent)
      this.m_writer.WriteAttributeString("style", "repeat-content", (string) null, "true");
    this.m_writer.WriteEndElement();
  }

  private void SerializeTableCellProperties(OTableCellProperties cellProp)
  {
    if (cellProp == null)
      return;
    this.m_writer.WriteStartElement("style", "table-cell-properties", (string) null);
    VerticalAlign? verticalAlign = cellProp.VerticalAlign;
    if (verticalAlign.HasValue)
      this.m_writer.WriteAttributeString("style", "vertical-align", (string) null, verticalAlign.ToString());
    if (cellProp.HasKey(8, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("fo", "background-color", (string) null, cellProp.BackColor == Color.Transparent ? cellProp.BackColor.Name.ToString().ToLower() : ODFWriter.HexConverter(cellProp.BackColor));
    if (cellProp.HasKey(7, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("style", "shrink-to-fit", (string) null, true.ToString().ToLower());
    if (cellProp.Border != null && cellProp.Border.LineStyle != BorderLineStyle.none)
    {
      if (cellProp.Border != null)
        this.m_writer.WriteAttributeString("fo", "border", (string) null, $"{cellProp.Border.LineWidth}in {(object) cellProp.Border.LineStyle} {ODFWriter.HexConverter(cellProp.Border.LineColor)}");
      this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, cellProp.PaddingTop.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, cellProp.PaddingBottom.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, cellProp.PaddingLeft.ToString() + "in");
      this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, cellProp.PaddingRight.ToString() + "in");
    }
    else
    {
      if (cellProp.BorderLeft != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-left", (string) null, $"{cellProp.BorderLeft.LineWidth}in {cellProp.BorderLeft.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(cellProp.BorderLeft.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-left", (string) null, cellProp.PaddingLeft.ToString() + "in");
      }
      if (cellProp.BorderRight != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-right", (string) null, $"{cellProp.BorderRight.LineWidth}in {cellProp.BorderRight.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(cellProp.BorderRight.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-right", (string) null, cellProp.PaddingRight.ToString() + "in");
      }
      if (cellProp.BorderTop != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-top", (string) null, $"{cellProp.BorderTop.LineWidth}in {cellProp.BorderTop.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(cellProp.BorderTop.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-top", (string) null, cellProp.PaddingTop.ToString() + "in");
      }
      if (cellProp.BorderBottom != null)
      {
        this.m_writer.WriteAttributeString("fo", "border-bottom", (string) null, $"{cellProp.BorderBottom.LineWidth}in {cellProp.BorderBottom.LineStyle.ToString().ToLower()} {ODFWriter.HexConverter(cellProp.BorderBottom.LineColor)}");
        this.m_writer.WriteAttributeString("fo", "padding-bottom", (string) null, cellProp.PaddingBottom.ToString() + "in");
      }
    }
    if (cellProp.HasKey(1, (int) cellProp.tableCellFlags))
      this.m_writer.WriteAttributeString("fo", "wrap-option", (string) null, "wrap");
    this.m_writer.WriteEndElement();
  }

  internal void SerializeTextProperties(TextProperties txtProp)
  {
    if (txtProp == null)
      return;
    this.m_writer.WriteStartElement("style", "text-properties", (string) null);
    if (txtProp.HasKey(16 /*0x10*/, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "font-name", (string) null, txtProp.FontName);
    if (txtProp.HasKey(17, txtProp.m_textFlag1))
    {
      this.m_writer.WriteAttributeString("fo", "font-size", (string) null, txtProp.FontSize.ToString() + "pt");
      this.m_writer.WriteAttributeString("style", "font-size-asian", (string) null, txtProp.FontSize.ToString() + "pt");
      this.m_writer.WriteAttributeString("style", "font-size-complex", (string) null, txtProp.FontSize.ToString() + "pt");
    }
    if (txtProp.HasKey(23, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "color", (string) null, ODFWriter.HexConverter(txtProp.Color));
    if (txtProp.HasKey(22, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("fo", "font-weight", (string) null, txtProp.FontWeight.ToString());
    if (txtProp.FontStyle == ODFFontStyle.italic)
      this.m_writer.WriteAttributeString("fo", "font-style", (string) null, txtProp.FontStyle.ToString());
    if (txtProp.HasKey(5, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "text-underline-type", (string) null, txtProp.TextUnderlineType.ToString().ToLower());
    if (txtProp.HasKey(6, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "text-underline-syle", (string) null, txtProp.TextUnderlineStyle.ToString());
    if (txtProp.HasKey(21, txtProp.m_textFlag1))
      this.m_writer.WriteAttributeString("style", "text-position", (string) null, txtProp.TextPosition);
    if (txtProp.HasKey(9, txtProp.m_textFlag3))
    {
      this.m_writer.WriteAttributeString("style", "text-line-through-type", (string) null, txtProp.LinethroughType.ToString());
      if (txtProp.LinethroughType != LineType.none)
      {
        this.m_writer.WriteAttributeString("style", "text-line-through-style", (string) null, txtProp.LinethroughStyle.ToString());
        this.m_writer.WriteAttributeString("style", "text-line-through-color", (string) null, txtProp.LinethroughColor);
      }
    }
    this.m_writer.WriteEndElement();
  }

  internal void SerializeTextToken(string text)
  {
    this.m_writer.WriteElementString("number", nameof (text), (string) null, text);
  }

  internal void SerializeAutomaticStyles(PageLayoutCollection layouts)
  {
    this.m_writer.WriteStartElement("office", "automatic-styles", (string) null);
    this.SerializePageLayouts(layouts);
    this.m_writer.WriteEndElement();
  }

  internal void SerializeAutoStyleStart()
  {
    this.m_writer.WriteStartElement("office", "automatic-styles", (string) null);
  }

  internal void SerializeContentAutoStyles(ODFStyleCollection styles)
  {
    this.SerializeAutoStyleStart();
    this.SerializeODFStyles(styles);
    this.SerializeEnd();
  }

  internal void SerializeMasterStyles(MasterPageCollection mPages)
  {
    MasterPage[] array = new MasterPage[mPages.DictMasterPages.Values.Count];
    mPages.DictMasterPages.Values.CopyTo(array, 0);
    for (int index = 0; index < array.Length; ++index)
    {
      this.m_writer.WriteStartElement("style", "master-page", (string) null);
      this.m_writer.WriteAttributeString("style", "name", (string) null, array[index].Name);
      this.m_writer.WriteAttributeString("style", "page-layout-name", (string) null, array[index].PageLayoutName);
      this.m_writer.WriteElementString("style", "header", (string) null, "");
      this.m_writer.WriteElementString("style", "header-left", (string) null, "");
      this.m_writer.WriteElementString("style", "footer", (string) null, "");
      this.m_writer.WriteElementString("style", "footer-left", (string) null, "");
      this.m_writer.WriteEndElement();
    }
  }

  internal void SerializeHeaderLeftStart()
  {
    this.m_writer.WriteStartElement("style", "header-left", (string) null);
  }

  internal void SerializeFooterLeftStart()
  {
    this.m_writer.WriteStartElement("style", "footer-left", (string) null);
  }

  internal void SerializeHeaderStart()
  {
    this.m_writer.WriteStartElement("style", "header", (string) null);
  }

  internal void SerializeFooterStart()
  {
    this.m_writer.WriteStartElement("style", "footer", (string) null);
  }

  internal void SerializeMasterStylesStart()
  {
    this.m_writer.WriteStartElement("office", "master-styles", (string) null);
  }

  internal void SerializeEnd() => this.m_writer.WriteEndElement();

  private void SerializePageLayouts(PageLayoutCollection layouts)
  {
    PageLayout[] array = new PageLayout[layouts.DictStyles.Values.Count];
    layouts.DictStyles.Values.CopyTo(array, 0);
    for (int index = 0; index < array.Length; ++index)
    {
      this.m_writer.WriteStartElement("style", "page-layout", (string) null);
      this.m_writer.WriteAttributeString("style", "name", (string) null, array[index].Name);
      this.m_writer.WriteStartElement("style", "page-layout-properties", (string) null);
      PageLayoutProperties layoutProperties = array[index].PageLayoutProperties;
      this.m_writer.WriteAttributeString("fo", "margin-top", (string) null, layoutProperties.MarginTop.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      this.m_writer.WriteAttributeString("fo", "margin-bottom", (string) null, layoutProperties.MarginBottom.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, layoutProperties.MarginLeft.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, layoutProperties.MarginRight.ToString((IFormatProvider) CultureInfo.InvariantCulture) + "in");
      this.m_writer.WriteAttributeString("style", "print-orientation", (string) null, layoutProperties.PageOrientation.ToString());
      this.m_writer.WriteAttributeString("style", "print-page-order", (string) null, layoutProperties.PrintPageOrder.ToString());
      this.m_writer.WriteAttributeString("style", "first-page-number", (string) null, layoutProperties.FirstPageNumber);
      this.m_writer.WriteAttributeString("style", "scale-to", (string) null, layoutProperties.ScaleTo + "%");
      this.m_writer.WriteAttributeString("style", "table-centering", (string) null, layoutProperties.TableCentering.ToString().ToLower());
      this.m_writer.WriteEndElement();
      this.SerializeHeaderFooterStyles(array[index]);
      this.m_writer.WriteEndElement();
    }
  }

  private void SerializeHeaderFooterStyles(PageLayout layout)
  {
    HeaderFooterStyle headerStyle = layout.HeaderStyle;
    this.m_writer.WriteStartElement("style", "header-style", (string) null);
    this.SerializeHeaderFooterProperties(headerStyle);
    this.m_writer.WriteEndElement();
    HeaderFooterStyle footerStyle = layout.FooterStyle;
    this.m_writer.WriteStartElement("style", "footer-style", (string) null);
    this.SerializeHeaderFooterProperties(footerStyle);
    this.m_writer.WriteEndElement();
  }

  private void SerializeHeaderFooterProperties(HeaderFooterStyle HFStyle)
  {
    this.m_writer.WriteStartElement("style", "header-footer-properties", (string) null);
    this.m_writer.WriteAttributeString("fo", "min-height", (string) null, HFStyle.HeaderFooterproperties.MinHeight.ToString() + "in");
    this.m_writer.WriteAttributeString("fo", "margin-left", (string) null, HFStyle.HeaderFooterproperties.MarginLeft.ToString() + "in");
    this.m_writer.WriteAttributeString("fo", "margin-right", (string) null, HFStyle.HeaderFooterproperties.MarginRight.ToString() + "in");
    if (HFStyle.IsHeader)
      this.m_writer.WriteAttributeString("fo", "margin-bottom", (string) null, HFStyle.HeaderFooterproperties.MarginBottom.ToString() + "in");
    else
      this.m_writer.WriteAttributeString("fo", "margin-top", (string) null, HFStyle.HeaderFooterproperties.MarginTop.ToString() + "in");
    this.m_writer.WriteEndElement();
  }

  private static string HexConverter(Color c)
  {
    return $"#{c.R.ToString("X2")}{c.G.ToString("X2")}{c.B.ToString("X2")}";
  }

  internal void Dispose()
  {
    if (this.m_archieve != null)
    {
      this.m_archieve.Dispose();
      this.m_archieve = (ZipArchive) null;
    }
    if (this.m_writer == null)
      return;
    this.m_writer = (XmlWriter) null;
  }

  internal void SerializeComments(Annotation comment)
  {
    if (comment == null)
      return;
    this.m_writer.WriteStartElement("office", "annotation", (string) null);
    if (!string.IsNullOrEmpty(comment.StyleName))
      this.m_writer.WriteAttributeString("draw", "style-name", (string) null, comment.StyleName.ToString());
    if (comment.Display)
      this.m_writer.WriteAttributeString("office", "display", (string) null, "true");
    this.m_writer.WriteAttributeString("svg", "x", (string) null, comment.X.ToString() + "in");
    this.m_writer.WriteAttributeString("svg", "y", (string) null, comment.Y.ToString() + "in");
    this.m_writer.WriteAttributeString("svg", "width", (string) null, comment.Width.ToString() + "in");
    this.m_writer.WriteAttributeString("svg", "height", (string) null, comment.Height.ToString() + "in");
    this.m_writer.WriteStartElement("dc", "creator", (string) null);
    this.m_writer.WriteValue(comment.Creator);
    this.m_writer.WriteEndElement();
    for (int index = 0; index < comment.Paras.Count; ++index)
      this.SerializeParagraph(comment.Paras[index]);
    this.m_writer.WriteEndElement();
  }
}
