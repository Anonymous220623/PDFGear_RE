// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExcelToHtmlConverter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ExcelToHtmlConverter : IDisposable
{
  private const string Pt = "pt";
  private const string Px = "px";
  private const string Em = "em";
  private const string TabPart = "tabpart";
  private const string HyperlinkStyleName = "Hyperlink";
  private const string ConditionFormatsClass = "C";
  private const string Sheet = "Sheet";
  private const string ExtendedFormatClass = "X";
  private const string MergedRangeClass = "M";
  private const string BlackColorHexName = "ff000000";
  private XmlWriter m_writer;
  private Dictionary<string, string> m_cssStyles;
  private Dictionary<string, string> m_commonStyles;
  private StringBuilder builder = new StringBuilder();
  private List<MergeCellsRecord.MergedRegion> lstRegions;
  private string m_keyNoPosition;
  private ExcelToHtmlConverter.ConversionMode m_conversionMode;
  private Dictionary<string, Dictionary<string, LinkedList<string>>> m_worksheetStyleCollections;
  private int increment;
  private ItemSizeHelper m_columnWidthGetter;
  private IWorksheet workSheet;
  private Dictionary<long, List<ExcelToHtmlConverter.ShapeInfo>> shapeCollection;
  private CFApplier conditionalFormatApplier;
  private bool m_isLastCellNotEmpty;
  private SortedList<long, ExtendedFormatImpl> m_sortedList;

  private XmlWriterSettings XmlSettings
  {
    get
    {
      return new XmlWriterSettings()
      {
        Encoding = Encoding.UTF8,
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n",
        NewLineHandling = NewLineHandling.Replace,
        OmitXmlDeclaration = true
      };
    }
  }

  private XmlWriter Writer => this.m_writer;

  internal ItemSizeHelper ColumnWidthGetter
  {
    get
    {
      if (this.m_columnWidthGetter == null)
        this.m_columnWidthGetter = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(this.workSheet.GetColumnWidthInPixels));
      return this.m_columnWidthGetter;
    }
  }

  public ExcelToHtmlConverter() => this.m_cssStyles = new Dictionary<string, string>(10);

  public void ConvertToHtml(
    Stream stream,
    WorkbookImpl book,
    string outputDirectoryPath,
    HtmlSaveOptions saveOption)
  {
    if (book.AppImplementation.EvalExpired)
    {
      book.AddWatermark((IWorkbook) book);
      book.Worksheets["Evaluation Warning"]["A10:O10"].Merge();
    }
    this.m_conversionMode = ExcelToHtmlConverter.ConversionMode.Workbook;
    this.m_writer = XmlWriter.Create(stream, this.XmlSettings);
    this.conditionalFormatApplier = new CFApplier();
    this.lstRegions = new List<MergeCellsRecord.MergedRegion>();
    this.WriteDocumentStart();
    this.BuildMainPage(book, outputDirectoryPath, saveOption);
    this.BuildTabPage(book, outputDirectoryPath);
    this.BuildHtmlFiles(book, outputDirectoryPath, saveOption);
  }

  internal void ConvertToHtml(Stream stream, WorkbookImpl book, HtmlSaveOptions saveOption)
  {
    if (book.AppImplementation.EvalExpired)
    {
      book.AddWatermark((IWorkbook) book);
      book.Worksheets["Evaluation Warning"]["A10:O10"].Merge();
    }
    this.m_conversionMode = ExcelToHtmlConverter.ConversionMode.Workbook;
    this.m_writer = XmlWriter.Create(stream, this.XmlSettings);
    this.conditionalFormatApplier = new CFApplier();
    this.lstRegions = new List<MergeCellsRecord.MergedRegion>();
    this.WriteDocumentStart();
    string styles1 = (string) null;
    this.BuildStyles(book, saveOption, this.m_writer, out styles1);
    this.m_commonStyles = new Dictionary<string, string>((IDictionary<string, string>) this.m_cssStyles);
    this.Writer.WriteStartElement("body");
    this.Writer.WriteAttributeString("id", "bg_img");
    this.Writer.WriteAttributeString("style", "overflow: hidden");
    this.Writer.WriteStartElement("div");
    this.Writer.WriteStartElement("div");
    this.Writer.WriteAttributeString("style", "overflow:auto;position:absolute;width:100%;left:2;top:2;bottom:35px;");
    int num = 0;
    foreach (WorksheetImpl worksheet in (IEnumerable<IWorksheet>) book.Worksheets)
    {
      if (worksheet.Visibility == WorksheetVisibility.Visible)
      {
        ++num;
        this.workSheet = (IWorksheet) worksheet;
        if (worksheet.HasPictures || worksheet.HasCharts)
          this.InitiateShapeCollection();
        this.Writer.WriteStartElement("div");
        if (worksheet.IsRightToLeft)
          this.Writer.WriteAttributeString("dir", "RTL");
        this.Writer.WriteAttributeString("id", $"link{num}_content");
        if (worksheet == book.ActiveSheet)
          this.Writer.WriteAttributeString("style", "display:inline-block;overflow:hidden;");
        else
          this.Writer.WriteAttributeString("style", "display:none;overflow:hidden;");
        this.m_cssStyles.Clear();
        string styles2 = this.GetStyles(worksheet, saveOption);
        if (styles2 != string.Empty)
        {
          this.Writer.WriteStartElement("style");
          this.Writer.WriteAttributeString("id", "tabpart");
          this.Writer.WriteAttributeString("type", "text/css");
          this.Writer.WriteString(styles2);
          this.Writer.WriteEndElement();
        }
        string style = styles2 + styles1;
        this.WriteSheetContent(worksheet, saveOption.ImagePath, saveOption, this.m_writer, style);
        this.Writer.WriteEndElement();
      }
    }
    this.m_commonStyles.Clear();
    this.Writer.WriteEndElement();
    this.BuildTabPage(book);
    this.BuildScripts(book);
    this.Writer.WriteEndElement();
    this.Writer.WriteEndElement();
    this.WriteDocumentEnd();
  }

  public void ConvertToHtml(
    Stream stream,
    WorksheetImpl sheet,
    string outputDirectoryPath,
    HtmlSaveOptions saveOption)
  {
    this.workSheet = (IWorksheet) sheet;
    this.m_conversionMode = ExcelToHtmlConverter.ConversionMode.Worksheet;
    this.m_writer = XmlWriter.Create(stream, this.XmlSettings);
    this.conditionalFormatApplier = new CFApplier();
    this.m_cssStyles.Clear();
    this.lstRegions = new List<MergeCellsRecord.MergedRegion>();
    this.WriteDocumentStart();
    string style;
    this.BuildStyles(sheet, saveOption, this.m_writer, out style);
    if (sheet.HasPictures || sheet.HasCharts)
      this.InitiateShapeCollection();
    this.Writer.WriteStartElement("body");
    if (sheet.AppImplementation.EvalExpired)
    {
      this.Writer.WriteStartElement("div");
      this.Writer.WriteAttributeString("height", "31");
      this.Writer.WriteAttributeString("style", "color:black;font-size:14.0pt;font-weight:700;font-family:Arial;text-align:center;");
      this.Writer.WriteString("Created with a trial version of Syncfusion Essential XlsIO");
      this.Writer.WriteEndElement();
    }
    if (sheet.IsRightToLeft)
    {
      this.Writer.WriteStartElement("div");
      this.Writer.WriteAttributeString("dir", "RTL");
    }
    if (sheet.PageSetup.BackgoundImage != null)
      this.WriteBackGroundImage((WorksheetBaseImpl) sheet, this.Writer, outputDirectoryPath, saveOption, (Image) sheet.PageSetup.BackgoundImage);
    this.WriteSheetContent(sheet, outputDirectoryPath, saveOption, this.m_writer, style);
    if (sheet.IsRightToLeft)
      this.Writer.WriteEndElement();
    this.Writer.WriteEndElement();
    this.WriteDocumentEnd();
  }

  private void BuildMainPage(
    WorkbookImpl book,
    string outputDirectoryPath,
    HtmlSaveOptions saveOption)
  {
    string name = new DirectoryInfo(outputDirectoryPath).Name;
    this.Writer.WriteStartElement("frameset");
    this.Writer.WriteAttributeString("rows", "94%,6%");
    this.Writer.WriteAttributeString("frameborder", "0");
    this.Writer.WriteStartElement("frame");
    string path2 = "";
    foreach (WorksheetImpl worksheet in (IEnumerable<IWorksheet>) book.Worksheets)
    {
      if (worksheet == book.ActiveSheet)
      {
        path2 = $"{worksheet.Name}.html";
        break;
      }
    }
    this.Writer.WriteAttributeString("src", Path.Combine(name, path2));
    this.Writer.WriteAttributeString("name", "top");
    this.Writer.WriteAttributeString("scrolling", "auto");
    this.Writer.WriteEndElement();
    this.Writer.WriteStartElement("frame");
    this.Writer.WriteAttributeString("src", Path.Combine(name, "tabs.html"));
    this.Writer.WriteAttributeString("name", "bottom");
    this.Writer.WriteAttributeString("scrolling", "auto");
    this.Writer.WriteEndElement();
    this.Writer.WriteStartElement("noframes");
    this.Writer.WriteStartElement("body");
    this.Writer.WriteString("This text will appear only if the browser does not support frames.");
    this.WriteDocumentEnd();
    this.WriteDocumentEnd();
    this.WriteDocumentEnd();
    this.WriteDocumentEnd();
  }

  private void BuildScripts(WorkbookImpl book)
  {
    this.Writer.WriteStartElement("script");
    this.Writer.WriteAttributeString("type", "text/javascript");
    this.Writer.WriteRaw(this.GetScript(book));
    this.Writer.WriteEndElement();
  }

  private void BuildTabPage(WorkbookImpl book)
  {
    this.Writer.WriteStartElement("div");
    this.Writer.WriteAttributeString("id", "footer");
    this.Writer.WriteAttributeString("style", "position: absolute; overflow:auto; bottom: 0px;left:0; width:100%; background-color:#808080; background-repeat:repeat-x ; height:35px ");
    this.Writer.WriteStartElement("table");
    this.Writer.WriteAttributeString("border", "0");
    this.Writer.WriteAttributeString("bgColor", "#808080");
    this.Writer.WriteStartElement("tr");
    int num = 0;
    foreach (WorksheetImpl worksheet in (IEnumerable<IWorksheet>) book.Worksheets)
    {
      if (worksheet.Visibility == WorksheetVisibility.Visible)
      {
        ++num;
        this.Writer.WriteStartElement("td");
        this.Writer.WriteAttributeString("id", $"td{num}");
        this.Writer.WriteAttributeString("nowrap", string.Empty);
        if (worksheet == book.ActiveSheet)
          this.Writer.WriteAttributeString("style", "background-color:#FFFFFF");
        this.Writer.WriteStartElement("b");
        this.Writer.WriteStartElement("small");
        this.Writer.WriteStartElement("small");
        this.Writer.WriteRaw("&nbsp;");
        this.Writer.WriteStartElement("a");
        this.Writer.WriteAttributeString("id", $"link{num}");
        this.Writer.WriteAttributeString("onclick", "hyperclick(this)");
        this.Writer.WriteAttributeString("href", "#");
        this.Writer.WriteAttributeString("style", "text-decoration:none");
        this.Writer.WriteStartElement("font");
        this.Writer.WriteAttributeString("face", "Verdana");
        this.Writer.WriteAttributeString("color", "#000000");
        this.Writer.WriteAttributeString("font-size", "13Px");
        this.Writer.WriteString(worksheet.Name);
        this.Writer.WriteEndElement();
        this.Writer.WriteEndElement();
        this.Writer.WriteRaw("&nbsp;");
        this.Writer.WriteEndElement();
        this.Writer.WriteEndElement();
        this.Writer.WriteEndElement();
        this.Writer.WriteEndElement();
      }
    }
    this.Writer.WriteEndElement();
    this.Writer.WriteEndElement();
    this.Writer.WriteEndElement();
  }

  private void BuildTabPage(WorkbookImpl book, string outputDirectoryPath)
  {
    string name = new DirectoryInfo(outputDirectoryPath).Name;
    int num = 0;
    this.m_writer = XmlWriter.Create((Stream) new FileStream(Path.Combine(outputDirectoryPath, "tabs.html"), FileMode.OpenOrCreate), this.XmlSettings);
    this.Writer.WriteStartElement("html");
    this.Writer.WriteStartElement("style");
    this.Writer.WriteAttributeString("type", "text/css");
    this.Writer.WriteString("table");
    this.Writer.WriteString("{");
    this.Writer.WriteString("border-collapse");
    this.Writer.WriteString(":");
    this.Writer.WriteString("collapse");
    this.Writer.WriteString(";");
    this.Writer.WriteString("border-spacing");
    this.Writer.WriteString(":");
    this.Writer.WriteString("0");
    this.Writer.WriteString(";");
    this.Writer.WriteString("empty-cells");
    this.Writer.WriteString(":");
    this.Writer.WriteString("show");
    this.Writer.WriteString(";");
    this.Writer.WriteString("}");
    this.Writer.WriteString("a");
    this.Writer.WriteString("{");
    this.Writer.WriteString("text-decoration:none;");
    this.Writer.WriteString("font-family:Verdana;");
    this.Writer.WriteString("font-size:13px;");
    this.Writer.WriteString("font-weight:bold;");
    this.Writer.WriteString("color:#000000;");
    this.Writer.WriteString("}");
    this.Writer.WriteString("a:hover");
    this.Writer.WriteString("{");
    this.Writer.WriteString("text-decoration:none;");
    this.Writer.WriteString("}");
    this.Writer.WriteString("td");
    this.Writer.WriteString("{");
    this.Writer.WriteString("white-space:nowrap;");
    this.Writer.WriteString("}");
    this.Writer.WriteString(".X1");
    this.Writer.WriteString("{");
    this.Writer.WriteString("position");
    this.Writer.WriteString(":");
    this.Writer.WriteString("absolute");
    this.Writer.WriteString(";");
    this.Writer.WriteString("left");
    this.Writer.WriteString(":");
    this.Writer.WriteString("0em");
    this.Writer.WriteString(";");
    this.Writer.WriteString("top");
    this.Writer.WriteString(":");
    this.Writer.WriteString("0px");
    this.Writer.WriteString(";");
    this.Writer.WriteString("}");
    this.WriteDocumentEnd();
    this.Writer.WriteStartElement("script");
    this.Writer.WriteAttributeString("type", "text/javascript");
    this.Writer.WriteRaw(this.GetTabScript(book));
    this.Writer.WriteEndElement();
    this.Writer.WriteStartElement("body");
    this.Writer.WriteAttributeString("alink", "rgb(0,0,255)");
    this.Writer.WriteAttributeString("vlink", "rgb(0,0,0)");
    this.Writer.WriteAttributeString("link", "rgb(0,0,0)");
    this.Writer.WriteAttributeString("bgColor", "#808080");
    this.Writer.WriteStartElement("div");
    this.Writer.WriteAttributeString("style", "position: absolute; overflow:auto; bottom: 0px;left:0; width:100%; background-color:#808080; background-repeat:repeat-x ; height:35px ");
    this.Writer.WriteStartElement("table");
    this.Writer.WriteAttributeString("border", "0");
    this.Writer.WriteStartElement("tr");
    this.Writer.WriteAttributeString("id", "tr_book");
    this.Writer.WriteAttributeString("class", "X1");
    foreach (WorksheetImpl worksheet in (IEnumerable<IWorksheet>) book.Worksheets)
    {
      if (worksheet.Visibility == WorksheetVisibility.Visible)
      {
        ++num;
        string str = $"{worksheet.Name}.html";
        this.Writer.WriteStartElement("td");
        this.Writer.WriteAttributeString("id", $"td{num}");
        if (worksheet == book.ActiveSheet)
          this.Writer.WriteAttributeString("bgColor", "#FFFFFF");
        this.Writer.WriteAttributeString("nowrap", string.Empty);
        this.Writer.WriteStartElement("a");
        this.Writer.WriteAttributeString("id", $"link{num}");
        this.Writer.WriteAttributeString("onclick", "hyperclick(this)");
        this.Writer.WriteAttributeString("href", str);
        this.Writer.WriteAttributeString("target", "top");
        this.Writer.WriteString(worksheet.Name);
        this.Writer.WriteEndElement();
        this.Writer.WriteEndElement();
      }
    }
    this.WriteDocumentEnd();
    this.WriteDocumentEnd();
    this.WriteDocumentEnd();
    this.WriteDocumentEnd();
    this.WriteDocumentEnd();
    this.Writer.Close();
  }

  private void BuildStyles(
    WorksheetImpl sheet,
    HtmlSaveOptions saveOption,
    XmlWriter writer,
    out string style)
  {
    if (this.m_conversionMode == ExcelToHtmlConverter.ConversionMode.Workbook)
    {
      writer.WriteStartElement("link");
      writer.WriteAttributeString("rel", "Stylesheet");
      writer.WriteAttributeString("href", "stylesheet.css");
      writer.WriteEndElement();
    }
    style = this.GetStyles(sheet, saveOption);
    if (!(style != string.Empty))
      return;
    this.Writer.WriteStartElement(nameof (style));
    this.Writer.WriteAttributeString("type", "text/css");
    this.Writer.WriteString(style);
    this.WriteDocumentEnd();
  }

  private void BuildCommonStyles(string styleSheet, WorkbookImpl book, out string styles)
  {
    this.m_cssStyles.Clear();
    styles = this.GetCommonStyles(book);
    File.WriteAllText(styleSheet, styles);
  }

  private string GetCommonStyles(WorkbookImpl book)
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (ExtendedFormatImpl innerExtFormat in (CollectionBase<ExtendedFormatImpl>) book.InnerExtFormats)
    {
      if (!innerExtFormat.HasParent)
        ++this.increment;
      else
        this.GetStyle(innerExtFormat, (IRange) null, false);
    }
    foreach (string str in this.m_cssStyles.Values)
    {
      stringBuilder.AppendLine();
      stringBuilder.Append(str);
    }
    return stringBuilder.ToString();
  }

  private void BuildStyles(
    WorkbookImpl book,
    HtmlSaveOptions saveOption,
    XmlWriter m_writer,
    out string styles)
  {
    string commonStyles = this.GetCommonStyles(book);
    styles = commonStyles;
    this.Writer.WriteStartElement("style");
    this.Writer.WriteAttributeString("id", "tabpart");
    this.Writer.WriteAttributeString("type", "text/css");
    this.Writer.WriteString(commonStyles);
    this.Writer.WriteEndElement();
  }

  private void WriteDocumentStart()
  {
    this.Writer.WriteStartElement("html");
    this.Writer.WriteStartElement("head");
    this.WriteDocumentEnd();
  }

  private void WriteDocumentEnd()
  {
    this.Writer.WriteEndElement();
    this.Writer.Flush();
  }

  private string GetStreamStyles(WorksheetImpl sheet, HtmlSaveOptions saveOption)
  {
    Dictionary<string, LinkedList<string>> dictionary1 = new Dictionary<string, LinkedList<string>>();
    ItemSizeHelper itemSizeHelper1 = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(sheet.GetRowHeightInPixels));
    ItemSizeHelper itemSizeHelper2 = new ItemSizeHelper(new ItemSizeHelper.SizeGetter(sheet.GetColumnWidthInPixels));
    StringBuilder builder = new StringBuilder();
    IRange usedRange = sheet.UsedRange;
    int row = usedRange.Row;
    int column = usedRange.Column;
    int lastRow = usedRange.LastRow;
    int lastColumn = usedRange.LastColumn;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(sheet.Application, (IWorksheet) sheet);
    int num1 = 0;
    int num2 = 0;
    string str1 = (string) null;
    int index1 = 0;
    if (sheet.HasMergedCells)
      sheet.MergeCells.CacheMerges(new Rectangle(column, row, lastColumn - column, lastRow - row), this.lstRegions);
    int num3;
    for (int index2 = 1; index2 <= lastRow; ++index2)
    {
      for (int index3 = 1; index3 <= lastColumn; ++index3)
      {
        ++num1;
        sheet.GetColumnWidthInPixels(index3);
        string str2 = index2.ToString() + this.GetColumnName(index3);
        migrantRangeImpl.ResetRowColumn(index2, index3);
        if (saveOption.TextMode == HtmlSaveOptions.GetText.DisplayText)
        {
          string displayText = migrantRangeImpl.DisplayText;
        }
        else
        {
          string str3 = migrantRangeImpl.Value;
        }
        ExtendedFormatImpl wrapped = (migrantRangeImpl.CellStyle as CellStyle).Wrapped;
        ExtendedFormatImpl extendedFormatImpl = this.conditionalFormatApplier.ApplyCF((IRange) migrantRangeImpl, wrapped);
        sheet.GetColumnWidthInPixels(index3);
        int rowHeightInPixels = sheet.GetRowHeightInPixels(index2);
        int rotation = migrantRangeImpl.CellStyle.Rotation;
        string str4 = migrantRangeImpl.CellStyle.Font.FontName.ToString();
        Color color = this.NormalizeColor(migrantRangeImpl.CellStyle.Font.RGBColor);
        string str5 = $"rgb({(object) (int) color.R},{(object) (int) color.G},{(object) (int) color.B})";
        double size = migrantRangeImpl.CellStyle.Font.Size;
        string name1 = extendedFormatImpl.Color.Name;
        extendedFormatImpl.ColorIndex.ToString();
        string str6 = $"rgb({(object) (int) extendedFormatImpl.Color.R},{(object) (int) extendedFormatImpl.Color.G},{(object) (int) extendedFormatImpl.Color.B})";
        string str7 = "normal";
        if (!migrantRangeImpl.WrapText)
          str7 = "nowrap";
        string str8 = migrantRangeImpl.CellStyle.Font.Underline.ToString((IFormatProvider) CultureInfo.InstalledUICulture.NumberFormat);
        this.m_keyNoPosition = $"{{color:{str5};font-family:{str4};font-size:{(object) size}pt;height:{(object) rowHeightInPixels};white-space:{str7};";
        if (extendedFormatImpl.FillPattern != ExcelPattern.None)
        {
          ExcelToHtmlConverter excelToHtmlConverter = this;
          excelToHtmlConverter.m_keyNoPosition = $"{excelToHtmlConverter.m_keyNoPosition}background-color:{str6};";
        }
        Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
        Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
        if (index1 < this.lstRegions.Count)
        {
          int rowFrom = this.lstRegions[index1].RowFrom;
          int columnFrom = this.lstRegions[index1].ColumnFrom;
          int rowTo = this.lstRegions[index1].RowTo;
          int columnTo = this.lstRegions[index1].ColumnTo;
          num3 = rowTo + 1;
          string name2 = num3.ToString() + this.GetColumnName(columnTo + 1);
          IRange result = sheet.Range[name2];
          if (index2 == rowFrom + 1 && index3 == columnFrom + 1)
          {
            Dictionary<string, string> dictionary4 = this.BuildBorders((IRange) migrantRangeImpl);
            Dictionary<string, string> dictionary5 = this.BuildBorders(result);
            foreach (string key in dictionary5.Keys)
            {
              if (!dictionary4.ContainsKey(key))
              {
                ExcelToHtmlConverter excelToHtmlConverter = this;
                excelToHtmlConverter.m_keyNoPosition = $"{excelToHtmlConverter.m_keyNoPosition}{key}:{dictionary5[key]};";
              }
            }
            ++index1;
          }
        }
        this.m_keyNoPosition = this.GetBorderStyles((IRange) migrantRangeImpl, this.m_keyNoPosition);
        string str9 = $".{str2}{this.m_keyNoPosition}";
        if (migrantRangeImpl.CellStyle.Font.Bold)
        {
          str9 += "font-weight:bold;";
          this.m_keyNoPosition += "font-weight:bold;";
        }
        if (migrantRangeImpl.CellStyle.Font.Italic)
        {
          str9 += "font-style:italic;";
          this.m_keyNoPosition += "font-style:italic;";
        }
        if (migrantRangeImpl.CellStyle.Font.Strikethrough)
        {
          str9 += "font-style:strike-through;";
          this.m_keyNoPosition += "font-style:strike-through;";
        }
        if (!str8.Equals("None"))
        {
          str9 += "text-decoration:underline;";
          this.m_keyNoPosition += "text-decoration:underline;";
        }
        if (migrantRangeImpl.VerticalAlignment == ExcelVAlign.VAlignBottom)
        {
          str9 += "vertical-align:bottom;";
          this.m_keyNoPosition += "vertical-align:bottom;";
        }
        if (migrantRangeImpl.VerticalAlignment == ExcelVAlign.VAlignTop)
        {
          str9 += "vertical-align:top;";
          this.m_keyNoPosition += "vertical-align:top;";
        }
        if (migrantRangeImpl.VerticalAlignment == ExcelVAlign.VAlignCenter)
        {
          str9 += "vertical-align:center;";
          this.m_keyNoPosition += "vertical-align:center;";
        }
        if (migrantRangeImpl.VerticalAlignment == ExcelVAlign.VAlignDistributed)
        {
          str9 += "vertical-align:distributed;";
          this.m_keyNoPosition += "vertical-align:distributed;";
        }
        if (migrantRangeImpl.VerticalAlignment == ExcelVAlign.VAlignJustify)
        {
          str9 += "vertical-align:justify;";
          this.m_keyNoPosition += "vertical-align:justify;";
        }
        if (migrantRangeImpl.HorizontalAlignment == ExcelHAlign.HAlignCenter)
        {
          str9 += "text-align:center";
          this.m_keyNoPosition += "text-align:center";
        }
        if (migrantRangeImpl.HorizontalAlignment == ExcelHAlign.HAlignLeft)
        {
          str9 += "text-align:left";
          this.m_keyNoPosition += "text-align:left";
        }
        int extendedFormatIndex = (int) migrantRangeImpl.ExtendedFormatIndex;
        ExtendedFormatImpl innerExtFormat = migrantRangeImpl.Workbook.InnerExtFormats[extendedFormatIndex];
        if (migrantRangeImpl.HasNumber || migrantRangeImpl.HasDateTime || migrantRangeImpl.HasFormula && migrantRangeImpl.FormulaStringValue == null && !migrantRangeImpl.HasFormulaErrorValue && !migrantRangeImpl.HasFormulaBoolValue)
        {
          ExcelFormatType formatType = (migrantRangeImpl.Worksheet.Workbook as WorkbookImpl).InnerFormats[innerExtFormat.NumberFormatIndex].GetFormatType(migrantRangeImpl.Number);
          migrantRangeImpl.HorizontalAlignment = formatType == ExcelFormatType.Text ? ExcelHAlign.HAlignLeft : ExcelHAlign.HAlignRight;
        }
        if (migrantRangeImpl.HorizontalAlignment == ExcelHAlign.HAlignRight)
        {
          str9 += "text-align:right";
          this.m_keyNoPosition += "text-align:right";
        }
        if (migrantRangeImpl.HorizontalAlignment == ExcelHAlign.HAlignJustify)
        {
          str9 += "text-align:justify";
          this.m_keyNoPosition += "text-align:justify";
        }
        if (migrantRangeImpl.HorizontalAlignment == ExcelHAlign.HAlignDistributed)
        {
          str9 += "text-align:distributed";
          this.m_keyNoPosition += "text-align:distributed";
        }
        if (migrantRangeImpl.HorizontalAlignment == ExcelHAlign.HAlignGeneral)
        {
          string str10 = "general";
          if (migrantRangeImpl.HasNumber)
            str10 = "right";
          str9 = $"{str9}text-align:{str10}";
          ExcelToHtmlConverter excelToHtmlConverter = this;
          excelToHtmlConverter.m_keyNoPosition = $"{excelToHtmlConverter.m_keyNoPosition}text-align:{str10}";
        }
        str1 = str9 + "}";
        this.m_keyNoPosition += "}";
        string str11 = ".X" + (object) this.increment;
        if (!dictionary1.ContainsKey(this.m_keyNoPosition))
        {
          LinkedList<string> linkedList = new LinkedList<string>();
          linkedList.AddFirst(str11);
          ++this.increment;
          linkedList.AddLast(str2);
          dictionary1.Add(this.m_keyNoPosition, linkedList);
          ++num2;
        }
        else
          dictionary1[this.m_keyNoPosition].AddLast(str2);
      }
    }
    num3 = sheet.Index;
    string str12 = $".table{num3.ToString()}{{border-collapse:collapse;border-spacing:0;empty-cells:show}}";
    builder.Append(str12);
    foreach (string key in dictionary1.Keys)
    {
      LinkedList<string> linkedList = dictionary1[key];
      builder.AppendLine();
      builder.Append(linkedList.First.Value);
      builder.Append(key);
    }
    if (sheet.HasPictures && saveOption.ImagePath != null)
      this.GetImageStyles(sheet, builder);
    this.m_worksheetStyleCollections.Add(sheet.Name, dictionary1);
    return builder.ToString();
  }

  private string GetScript(WorkbookImpl book)
  {
    int num = 0;
    string empty = string.Empty;
    string str1 = "[";
    foreach (WorksheetImpl worksheet in (IEnumerable<IWorksheet>) book.Worksheets)
    {
      if (worksheet.Visibility == WorksheetVisibility.Visible)
      {
        ++num;
        Image backgoundImage = (Image) worksheet.PageSetup.BackgoundImage;
        string str2 = string.Empty;
        string str3 = string.Empty;
        if (backgoundImage != null)
        {
          ImageFormat rawFormat = backgoundImage.RawFormat;
          str2 = this.GetExtension(rawFormat);
          str3 = this.ImageToBase64(backgoundImage, rawFormat);
        }
        str1 = str1 + (object) '"' + $"data:image/{str2};base64,{str3}" + (object) '"';
        str1 = worksheet.Equals((object) book.Worksheets[book.Worksheets.Count - 1]) ? str1 : str1 + ",";
      }
    }
    string str4 = str1 + "]";
    return $" function hyperclick(test) {{var content = test.id; content = content + \"_content\";var count ={(object) num};var images ={str4};for (var i = 1;i <= count; i++) {{var formatName = \"link\" + i + \"_content\";if (content == formatName) {{document.getElementById(formatName).style.display = \"block\";document.getElementById(\"td\" + i).style.backgroundColor = \"FFFFFF\";var divChildrenCount = document.getElementById(formatName).childNodes.length;for (var j = 0; j <  divChildrenCount; j++){{if(document.getElementById(formatName).childNodes[j].nodeName.toLocaleUpperCase() == \"TABLE\"){{document.getElementById(formatName).style.height = document.getElementById(formatName).childNodes[j].scrollHeight+\"px\";document.getElementById(formatName).style.width = document.getElementById(formatName).childNodes[j].scrollWidth+\"px\";break;}}}} var img_Id = document.getElementById('bg_img');var k=i-1;img_Id.style.backgroundImage=\"url('\"+images[k]+\"')\";}}else {{document.getElementById(formatName).style.display = \"none\";document.getElementById(\"td\" + i).style.backgroundColor = \"gray\";}}}}}}window.onload = function(){{var count ={(object) num};var images ={str4};for (var i = 1; i <= count; i++){{var formatName = \"link\" + i + \"_content\";if(\"block\" == document.getElementById(formatName).style.display ){{var divChildrenCount = document.getElementById(formatName).childNodes.length;for (var j = 0; j <  divChildrenCount; j++){{if(document.getElementById(formatName).childNodes[j].nodeName.toLocaleUpperCase() == \"TABLE\"){{document.getElementById(formatName).style.height = document.getElementById(formatName).childNodes[j].scrollHeight+\"px\";document.getElementById(formatName).style.width = document.getElementById(formatName).childNodes[j].scrollWidth+\"px\";}}}}var img_Id = document.getElementById('bg_img');var k=i-1;img_Id.style.backgroundImage =\"url('\"+images[k]+\"')\";}}}}}}";
  }

  private string GetTabScript(WorkbookImpl book)
  {
    return " function hyperclick(element) {var anchorId = element.id;var clicking_td_tag = document.getElementById(anchorId).parentNode.id;var count = document.getElementById('tr_book').cells.length;for (var i = 1; i <= count; i++) {var td_Id= \"td\"+i;if(clicking_td_tag == td_Id) {document.getElementById(td_Id).setAttribute(\"bgcolor\", \"#FFFFFF\"); }else {     document.getElementById(td_Id).setAttribute(\"bgcolor\", \"#808080\"); }}}";
  }

  private string GetStyles(WorksheetImpl sheet, HtmlSaveOptions saveOption)
  {
    this.builder = new StringBuilder();
    this.m_sortedList = sheet.ApplyCF();
    if (this.m_conversionMode == ExcelToHtmlConverter.ConversionMode.Worksheet)
    {
      foreach (ExtendedFormatImpl innerExtFormat in (CollectionBase<ExtendedFormatImpl>) (sheet.Workbook as WorkbookImpl).InnerExtFormats)
      {
        if (!innerExtFormat.HasParent)
          ++this.increment;
        else
          this.GetStyle(innerExtFormat, (IRange) null, false);
      }
    }
    if (sheet.ConditionalFormats.Count > 0)
    {
      for (int index = 0; index < this.m_sortedList.Count; ++index)
      {
        long key = this.m_sortedList.Keys[index];
        int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(key);
        int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(key);
        IRange cell = sheet[rowFromCellIndex, columnFromCellIndex];
        this.GetStyle(this.m_sortedList.Values[index], cell, true);
      }
    }
    if (sheet.HasMergedCells)
    {
      foreach (IRange mergedCell in sheet.MergedCells)
      {
        if (mergedCell.LastColumn < sheet.Workbook.MaxColumnCount && mergedCell.LastRow < sheet.Workbook.MaxRowCount)
        {
          int row1 = mergedCell.Row;
          int column = mergedCell.Column;
          int lastRow = mergedCell.LastRow;
          int lastColumn = mergedCell.LastColumn;
          IRange range1 = sheet[row1, lastColumn];
          IRange range2 = sheet[lastRow, lastColumn];
          ExcelLineStyle excelLineStyle1 = range1.Borders[ExcelBordersIndex.EdgeRight].LineStyle;
          ExcelLineStyle excelLineStyle2 = range2.Borders[ExcelBordersIndex.EdgeBottom].LineStyle;
          RangeImpl rangeImpl1 = lastColumn + 1 <= sheet.Workbook.MaxColumnCount ? sheet[row1, lastColumn + 1] as RangeImpl : (RangeImpl) null;
          RangeImpl rangeImpl2 = lastRow + 1 <= sheet.Workbook.MaxRowCount ? sheet[lastRow + 1, lastColumn] as RangeImpl : (RangeImpl) null;
          RangeImpl rangeImpl3 = rangeImpl1 == null || rangeImpl1.IsMerged ? sheet[rangeImpl1.MergeArea.Row, rangeImpl1.MergeArea.Column] as RangeImpl : rangeImpl1;
          RangeImpl rangeImpl4 = rangeImpl2 == null || rangeImpl2.IsMerged ? sheet[rangeImpl2.MergeArea.Row, rangeImpl2.MergeArea.Column] as RangeImpl : rangeImpl2;
          bool flag = false;
          do
          {
            RowStorage row2 = WorksheetHelper.GetOrCreateRow(rangeImpl4.Worksheet as IInternalWorksheet, rangeImpl4.Row - 1, false);
            flag = row2 != null ? row2.IsHidden : flag;
            if (flag)
              rangeImpl4 = this.workSheet[rangeImpl4.Row + 1, rangeImpl4.Column] as RangeImpl;
          }
          while (flag);
          long cellIndex1 = rangeImpl3 != null ? RangeImpl.GetCellIndex(rangeImpl3.Column, rangeImpl3.Row) : 0L;
          long cellIndex2 = rangeImpl4 != null ? RangeImpl.GetCellIndex(rangeImpl4.Column, rangeImpl4.Row) : 0L;
          string cssStyle1;
          if (this.m_sortedList.ContainsKey(cellIndex1))
            cssStyle1 = this.m_cssStyles[$".CSheet{(object) rangeImpl3.Worksheet.Index}{rangeImpl3.AddressLocal}"];
          else if (this.m_commonStyles == null || !this.m_commonStyles.TryGetValue(".X" + (object) rangeImpl3.ExtendedFormatIndex, out cssStyle1))
            cssStyle1 = this.m_cssStyles[".X" + (object) rangeImpl3.ExtendedFormatIndex];
          if (cssStyle1.Contains("border-left:"))
            excelLineStyle1 = ExcelLineStyle.None;
          string cssStyle2;
          if (this.m_sortedList.ContainsKey(cellIndex2))
            cssStyle2 = this.m_cssStyles[$".CSheet{(object) rangeImpl4.Worksheet.Index}{rangeImpl4.AddressLocal}"];
          else if (this.m_commonStyles == null || !this.m_commonStyles.TryGetValue(".X" + (object) rangeImpl4.ExtendedFormatIndex, out cssStyle2))
            cssStyle2 = this.m_cssStyles[".X" + (object) rangeImpl4.ExtendedFormatIndex];
          if (cssStyle2.Contains("border-top:"))
            excelLineStyle2 = ExcelLineStyle.None;
          this.m_keyNoPosition = "{";
          long cellIndex3 = RangeImpl.GetCellIndex(column, row1);
          RangeImpl rangeImpl5 = mergedCell.Worksheet[row1, column] as RangeImpl;
          string str1 = (string) null;
          if (this.m_sortedList.ContainsKey(cellIndex3))
            str1 = this.m_cssStyles[$".CSheet{(object) rangeImpl5.Worksheet.Index}{rangeImpl5.AddressLocal}"];
          else if (this.m_commonStyles == null || !this.m_commonStyles.TryGetValue(".X" + (object) rangeImpl5.ExtendedFormatIndex, out str1))
            str1 = this.m_cssStyles[".X" + (object) rangeImpl5.ExtendedFormatIndex];
          if (excelLineStyle1 != ExcelLineStyle.None)
          {
            Color colorRgb = range1.Borders[ExcelBordersIndex.EdgeRight].ColorRGB;
            string str2 = $"rgb({colorRgb.R.ToString()},{colorRgb.G.ToString()},{colorRgb.B.ToString()})";
            string str3 = $"border-right:{this.GetDashStyle(excelLineStyle1.ToString())};";
            this.m_keyNoPosition += str1.Contains(str3) ? (string) null : str3;
            string str4 = $"border-right-width:{(object) this.GetBorderWidth(excelLineStyle1.ToString())};";
            this.m_keyNoPosition += str1.Contains(str4) ? (string) null : str4;
            string str5 = $"border-right-color:{str2};";
            this.m_keyNoPosition += str1.Contains(str5) ? (string) null : str5;
          }
          if (excelLineStyle2 != ExcelLineStyle.None)
          {
            Color colorRgb = range2.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB;
            string str6 = $"rgb({colorRgb.R.ToString()},{colorRgb.G.ToString()},{colorRgb.B.ToString()})";
            string str7 = $"border-bottom:{this.GetDashStyle(excelLineStyle2.ToString())};";
            this.m_keyNoPosition += str1.Contains(str7) ? (string) null : str7;
            string str8 = $"border-bottom-width:{(object) this.GetBorderWidth(excelLineStyle2.ToString())};";
            this.m_keyNoPosition += str1.Contains(str8) ? (string) null : str8;
            string str9 = $"border-bottom-color:{str6};";
            this.m_keyNoPosition += str1.Contains(str9) ? (string) null : str9;
          }
          if (this.m_keyNoPosition.Length > 1)
          {
            string key = $".MSheet{(object) sheet.Index}{sheet[row1, column].AddressLocal}";
            this.m_keyNoPosition = $"{key}{this.m_keyNoPosition}}}";
            this.m_cssStyles.Add(key, this.m_keyNoPosition);
          }
        }
      }
    }
    foreach (string str in this.m_cssStyles.Values)
    {
      this.builder.AppendLine();
      this.builder.Append(str);
    }
    return this.builder.ToString();
  }

  private void GetStyle(ExtendedFormatImpl format, IRange cell, bool conditionalFormats)
  {
    string str1 = "None";
    IFont font = format.Font;
    Color black = Color.Black;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    Color color1 = format.Color;
    format.ColorIndex.ToString();
    string name = color1.Name;
    int r = (int) color1.R;
    int g = (int) color1.G;
    int b = (int) color1.B;
    int rotation = format.Rotation;
    string str2 = this.SwitchFont(font.FontName);
    Color color2 = this.NormalizeColor(font.RGBColor);
    string str3 = $"rgb({(object) (int) color2.R},{(object) (int) color2.G},{(object) (int) color2.B})";
    double size = font.Size;
    string empty3 = string.Empty;
    string str4;
    if (format is ExtendedFormatStandAlone && ((ExtendedFormatStandAlone) format).HasDataBar)
      str4 = $"rgb({(object) (int) byte.MaxValue},{(object) (int) byte.MaxValue},{(object) (int) byte.MaxValue})";
    else
      str4 = $"rgb({(object) r},{(object) g},{(object) b})";
    string str5 = "normal";
    if (!format.WrapText)
      str5 = "nowrap";
    string str6 = font.Underline.ToString((IFormatProvider) CultureInfo.InstalledUICulture.NumberFormat);
    this.m_keyNoPosition = $"{{color:{str3};font-family:{str2};font-size:{(object) size}pt;white-space:{str5};";
    if (format.FillPattern != ExcelPattern.None)
    {
      if (RangeImpl.GetHatchStyle(format.FillPattern) != ~HatchStyle.Horizontal)
      {
        string str7 = string.Empty;
        if (format.PatternColor.Name != "ff000000")
          str7 = $"rgb({(object) format.PatternColor.R},{(object) format.PatternColor.G},{(object) format.PatternColor.B})";
        else if (format.Color.Name != "ff000000")
          str7 = $"rgb({(object) format.Color.R},{(object) format.Color.G},{(object) format.Color.B})";
        ExcelToHtmlConverter excelToHtmlConverter = this;
        excelToHtmlConverter.m_keyNoPosition = $"{excelToHtmlConverter.m_keyNoPosition}background-color:{str7};";
      }
      else
      {
        ExcelToHtmlConverter excelToHtmlConverter = this;
        excelToHtmlConverter.m_keyNoPosition = $"{excelToHtmlConverter.m_keyNoPosition}background-color:{str4};";
      }
    }
    string border1 = format.Borders[ExcelBordersIndex.EdgeLeft].LineStyle.ToString();
    string border2 = format.Borders[ExcelBordersIndex.EdgeRight].LineStyle.ToString();
    string border3 = format.Borders[ExcelBordersIndex.EdgeTop].LineStyle.ToString();
    string border4 = format.Borders[ExcelBordersIndex.EdgeBottom].LineStyle.ToString();
    string str8 = $"rgb({format.Borders[ExcelBordersIndex.EdgeTop].ColorRGB.R.ToString()},{format.Borders[ExcelBordersIndex.EdgeTop].ColorRGB.G.ToString()},{format.Borders[ExcelBordersIndex.EdgeTop].ColorRGB.B.ToString()})";
    string str9 = format.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB.R.ToString();
    byte num = format.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB.G;
    string str10 = num.ToString();
    Color colorRgb = format.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB;
    num = colorRgb.B;
    string str11 = num.ToString();
    string str12 = $"rgb({str9},{str10},{str11})";
    colorRgb = format.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB;
    num = colorRgb.R;
    string str13 = num.ToString();
    colorRgb = format.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB;
    num = colorRgb.G;
    string str14 = num.ToString();
    colorRgb = format.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB;
    num = colorRgb.B;
    string str15 = num.ToString();
    string str16 = $"rgb({str13},{str14},{str15})";
    colorRgb = format.Borders[ExcelBordersIndex.EdgeRight].ColorRGB;
    num = colorRgb.R;
    string str17 = num.ToString();
    colorRgb = format.Borders[ExcelBordersIndex.EdgeRight].ColorRGB;
    num = colorRgb.G;
    string str18 = num.ToString();
    colorRgb = format.Borders[ExcelBordersIndex.EdgeRight].ColorRGB;
    num = colorRgb.B;
    string str19 = num.ToString();
    string str20 = $"rgb({str17},{str18},{str19})";
    if (border3 != str1)
    {
      ExcelToHtmlConverter excelToHtmlConverter = this;
      excelToHtmlConverter.m_keyNoPosition = $"{excelToHtmlConverter.m_keyNoPosition}border-top:{this.GetDashStyle(border3)};border-top-width:{(object) this.GetBorderWidth(border3)};";
    }
    if (border4 != str1)
    {
      ExcelToHtmlConverter excelToHtmlConverter = this;
      excelToHtmlConverter.m_keyNoPosition = $"{excelToHtmlConverter.m_keyNoPosition}border-bottom:{this.GetDashStyle(border4)};border-bottom-width:{(object) this.GetBorderWidth(border4)};";
    }
    if (border1 != str1)
    {
      ExcelToHtmlConverter excelToHtmlConverter = this;
      excelToHtmlConverter.m_keyNoPosition = $"{excelToHtmlConverter.m_keyNoPosition}border-left:{this.GetDashStyle(border1)};border-left-width:{(object) this.GetBorderWidth(border1)};";
    }
    if (border2 != str1)
    {
      ExcelToHtmlConverter excelToHtmlConverter = this;
      excelToHtmlConverter.m_keyNoPosition = $"{excelToHtmlConverter.m_keyNoPosition}border-right:{this.GetDashStyle(border2)};border-right-width:{(object) this.GetBorderWidth(border2)};";
    }
    ExcelToHtmlConverter excelToHtmlConverter1 = this;
    excelToHtmlConverter1.m_keyNoPosition = $"{excelToHtmlConverter1.m_keyNoPosition}border-top-color:{str8};border-bottom-color:{str12};border-left-color:{str16};border-right-color:{str20};";
    if (font.Bold)
      this.m_keyNoPosition += "font-weight:bold;";
    if (font.Italic)
      this.m_keyNoPosition += "font-style:italic;";
    if (font.Strikethrough)
      this.m_keyNoPosition += "font-style:strike-through;";
    if (!str6.Equals("None"))
      this.m_keyNoPosition += "text-decoration:underline;";
    switch (format.VerticalAlignment)
    {
      case ExcelVAlign.VAlignTop:
        this.m_keyNoPosition += "vertical-align:top;";
        break;
      case ExcelVAlign.VAlignCenter:
        this.m_keyNoPosition += "vertical-align:center;";
        break;
      case ExcelVAlign.VAlignBottom:
        this.m_keyNoPosition += "vertical-align:bottom;";
        break;
      case ExcelVAlign.VAlignJustify:
        this.m_keyNoPosition += "vertical-align:justify;";
        break;
      case ExcelVAlign.VAlignDistributed:
        this.m_keyNoPosition += "vertical-align:distributed;";
        break;
    }
    switch (format.HorizontalAlignment)
    {
      case ExcelHAlign.HAlignLeft:
        this.m_keyNoPosition += "text-align:left;";
        break;
      case ExcelHAlign.HAlignCenter:
        this.m_keyNoPosition += "text-align:center;";
        break;
      case ExcelHAlign.HAlignRight:
        this.m_keyNoPosition += "text-align:right;";
        break;
      case ExcelHAlign.HAlignJustify:
        this.m_keyNoPosition += "text-align:justify;";
        break;
      case ExcelHAlign.HAlignDistributed:
        this.m_keyNoPosition += "text-align:distributed;";
        break;
    }
    if (format.IndentLevel > 0)
    {
      string str21 = (9 * format.IndentLevel).ToString() + "px";
      switch (format.HorizontalAlignment)
      {
        case ExcelHAlign.HAlignLeft:
          ExcelToHtmlConverter excelToHtmlConverter2 = this;
          excelToHtmlConverter2.m_keyNoPosition = $"{excelToHtmlConverter2.m_keyNoPosition}padding-left:{str21};";
          break;
        case ExcelHAlign.HAlignRight:
          ExcelToHtmlConverter excelToHtmlConverter3 = this;
          excelToHtmlConverter3.m_keyNoPosition = $"{excelToHtmlConverter3.m_keyNoPosition}padding-right:{str21};";
          break;
        case ExcelHAlign.HAlignDistributed:
          ExcelToHtmlConverter excelToHtmlConverter4 = this;
          excelToHtmlConverter4.m_keyNoPosition = $"{excelToHtmlConverter4.m_keyNoPosition}padding-left:{str21};";
          ExcelToHtmlConverter excelToHtmlConverter5 = this;
          excelToHtmlConverter5.m_keyNoPosition = $"{excelToHtmlConverter5.m_keyNoPosition}padding-right:{str21};";
          break;
      }
      ExcelToHtmlConverter excelToHtmlConverter6 = this;
      excelToHtmlConverter6.m_keyNoPosition = $"{excelToHtmlConverter6.m_keyNoPosition}mso-char-indent-count:{format.IndentLevel.ToString()};";
    }
    this.m_keyNoPosition += "}";
    string empty4 = string.Empty;
    string key;
    if (conditionalFormats)
    {
      key = $".CSheet{(object) cell.Worksheet.Index}{cell.AddressLocal}";
    }
    else
    {
      key = ".X" + (object) this.increment;
      ++this.increment;
    }
    this.m_keyNoPosition = key + this.m_keyNoPosition;
    this.m_cssStyles.Add(key, this.m_keyNoPosition);
  }

  private string SwitchFont(string fontFamily)
  {
    switch (fontFamily)
    {
      case "Georgia":
      case "Palatino Linotype":
      case "Book Antiqua":
      case "Times New Roman":
      case "Times New Roman CE":
        fontFamily += ", serif";
        break;
      case "Helvetica":
      case "Arial Black":
      case "Gadget":
      case "Comic Sans MS":
      case "cursive":
      case "Impact":
      case "Charcoal":
      case "Lucida Sans Unicode":
      case "Lucida Grande":
      case "Geneva":
      case "Trebuchet MS":
      case "Tahoma":
      case "Univers":
      case "Calibri":
      case "Verdana":
      case "Arial":
        fontFamily += ", sans-serif";
        break;
      case "Courier New":
      case "Lucida Console":
        fontFamily += ", monospace";
        break;
    }
    return fontFamily;
  }

  private Color NormalizeColor(Color color)
  {
    return Color.FromArgb((int) byte.MaxValue, (int) color.R, (int) color.G, (int) color.B);
  }

  private string GetBorderStyles(IRange result, string keyNoPosition)
  {
    IWorksheet worksheet = result.Worksheet;
    string str1 = "None";
    string border1 = this.CanDrawBorder(result.Borders[ExcelBordersIndex.EdgeTop], ExcelBordersIndex.EdgeTop, result);
    IRange range = worksheet.Range[result.Row + 1, result.Column];
    string border2 = this.CanDrawBorder(result.Borders[ExcelBordersIndex.EdgeBottom], ExcelBordersIndex.EdgeBottom, result);
    if (!range.IsMerged || result.IsMerged)
      border2 = result.Borders[ExcelBordersIndex.EdgeBottom].LineStyle.ToString();
    string border3 = this.CanDrawBorder(result.Borders[ExcelBordersIndex.EdgeLeft], ExcelBordersIndex.EdgeLeft, result);
    string border4 = result.Borders[ExcelBordersIndex.EdgeRight].LineStyle.ToString();
    string str2 = $"rgb({result.CellStyle.Borders[ExcelBordersIndex.EdgeTop].ColorRGB.R.ToString()},{result.CellStyle.Borders[ExcelBordersIndex.EdgeTop].ColorRGB.G.ToString()},{result.CellStyle.Borders[ExcelBordersIndex.EdgeTop].ColorRGB.B.ToString()})";
    string str3 = $"rgb({result.CellStyle.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB.R.ToString()},{result.CellStyle.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB.G.ToString()},{result.CellStyle.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB.B.ToString()})";
    string str4 = $"rgb({result.CellStyle.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB.R.ToString()},{result.CellStyle.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB.G.ToString()},{result.CellStyle.Borders[ExcelBordersIndex.EdgeLeft].ColorRGB.B.ToString()})";
    string str5 = $"rgb({result.CellStyle.Borders[ExcelBordersIndex.EdgeRight].ColorRGB.R.ToString()},{result.CellStyle.Borders[ExcelBordersIndex.EdgeRight].ColorRGB.G.ToString()},{result.CellStyle.Borders[ExcelBordersIndex.EdgeRight].ColorRGB.B.ToString()})";
    if (border1 != str1)
      keyNoPosition = $"{keyNoPosition}border-top:{this.GetDashStyle(border1)};border-top-width:{(object) this.GetBorderWidth(border1)};";
    if (border2 != str1)
      keyNoPosition = $"{keyNoPosition}border-bottom:{this.GetDashStyle(border2)};border-bottom-width:{(object) this.GetBorderWidth(border2)};";
    if (border3 != str1)
      keyNoPosition = $"{keyNoPosition}border-left:{this.GetDashStyle(border3)};border-left-width:{(object) this.GetBorderWidth(border3)};";
    if (border4 != str1)
      keyNoPosition = $"{keyNoPosition}border-right:{this.GetDashStyle(border4)};border-right-width:{(object) this.GetBorderWidth(border4)};";
    keyNoPosition = $"{keyNoPosition}border-top-color:{str2};border-bottom-color:{str3};border-left-color:{str4};border-right-color:{str5};";
    return keyNoPosition;
  }

  private string CanDrawBorder(IBorder border, ExcelBordersIndex index, IRange result)
  {
    int extendedFormatIndex = (int) (result as RangeImpl).ExtendedFormatIndex;
    return this.CheckCellBorderStyle((result.Worksheet.Workbook as WorkbookImpl).InnerExtFormats[extendedFormatIndex].Borders, index);
  }

  private string CheckCellBorderStyle(IBorders borders, ExcelBordersIndex index)
  {
    switch (index)
    {
      case ExcelBordersIndex.DiagonalDown:
        return borders[ExcelBordersIndex.DiagonalDown].LineStyle.ToString();
      case ExcelBordersIndex.DiagonalUp:
        return borders[ExcelBordersIndex.DiagonalUp].LineStyle.ToString();
      case ExcelBordersIndex.EdgeLeft:
        return borders[ExcelBordersIndex.EdgeLeft].LineStyle.ToString();
      case ExcelBordersIndex.EdgeTop:
        return borders[ExcelBordersIndex.EdgeTop].LineStyle.ToString();
      case ExcelBordersIndex.EdgeBottom:
        return borders[ExcelBordersIndex.EdgeBottom].LineStyle.ToString();
      case ExcelBordersIndex.EdgeRight:
        return borders[ExcelBordersIndex.EdgeRight].LineStyle.ToString();
      default:
        return ExcelLineStyle.None.ToString();
    }
  }

  private float GetBorderWidth(string border)
  {
    float borderWidth = 0.0f;
    switch (border)
    {
      case "Hair":
      case "Thin":
      case "Dashed":
      case "Dotted":
      case "Dash_dot":
      case "Slanted_dash_dot":
      case "Dash_dot_dot":
        borderWidth = 0.5f;
        break;
      case "Medium":
      case "Medium_dashed":
      case "Medium_dash_dot":
      case "Medium_dash_dot_dot":
        borderWidth = 1.5f;
        break;
      case "Thick":
        borderWidth = 2f;
        break;
      case "Double":
        borderWidth = 3f;
        break;
    }
    return borderWidth;
  }

  private string GetDashStyle(string border)
  {
    string dashStyle = "solid";
    switch (border)
    {
      case "Thin":
      case "Medium":
      case "Thick":
      case "Hair":
        dashStyle = "solid";
        break;
      case "Double":
        dashStyle = "double";
        break;
      case "Dashed":
      case "Medium_dashed":
        dashStyle = "dashed";
        break;
      case "Dotted":
      case "Dash_dot":
      case "Medium_dash_dot":
      case "Slanted_dash_dot":
      case "Dash_dot_dot":
      case "Medium_dash_dot_dot":
        dashStyle = "dotted";
        break;
    }
    return dashStyle;
  }

  private Dictionary<string, string> BuildBorders(IRange result)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    string str = "None";
    string border1 = result.Borders[ExcelBordersIndex.EdgeTop].LineStyle.ToString();
    string border2 = result.Borders[ExcelBordersIndex.EdgeBottom].LineStyle.ToString();
    string border3 = result.Borders[ExcelBordersIndex.EdgeLeft].LineStyle.ToString();
    string border4 = result.Borders[ExcelBordersIndex.EdgeRight].LineStyle.ToString();
    if (border1 != str)
    {
      dictionary.Add("border-top", this.GetDashStyle(border1));
      dictionary.Add("border-top-width", this.GetBorderWidth(border1).ToString());
    }
    if (border2 != str)
    {
      dictionary.Add("border-bottom", this.GetDashStyle(border2));
      dictionary.Add("border-bottom-width", this.GetBorderWidth(border2).ToString());
    }
    if (border3 != str)
    {
      dictionary.Add("border-left", this.GetDashStyle(border3));
      dictionary.Add("border-left-width", this.GetBorderWidth(border3).ToString());
    }
    if (border4 != str)
    {
      dictionary.Add("border-right", this.GetDashStyle(border4));
      dictionary.Add("border-right-width", this.GetBorderWidth(border4).ToString());
    }
    return dictionary;
  }

  private void GetImageStyles(WorksheetImpl sheet, StringBuilder builder)
  {
    IPictures pictures = sheet.Pictures;
    for (int Index = 0; Index < pictures.Count; ++Index)
    {
      string uniqueId = ".ix" + (object) Index;
      string shapeStyle = this.GetShapeStyle((IShape) pictures[Index], uniqueId);
      builder.Append(shapeStyle);
      builder.AppendLine();
    }
  }

  private string GetShapeStyle(IShape shape, string uniqueId)
  {
    return $"{$"{$"{$"{$"{uniqueId + "{"}margin-left:{(object) shape.Left}px;"}margin-top:{(object) shape.Top}px;"}height:{(object) shape.Height}px;"}width:{(object) shape.Width}px;"}position:absolute}}";
  }

  private float GetColumnWidth(WorksheetImpl sheetImpl, int columnIndex)
  {
    List<float> maxList = new List<float>();
    MigrantRangeImpl cell = new MigrantRangeImpl(sheetImpl.Application, (IWorksheet) sheetImpl);
    int row = sheetImpl.UsedRange.Row;
    int lastRow = sheetImpl.UsedRange.LastRow;
    int maxColumnCount = sheetImpl.ParentWorkbook.MaxColumnCount;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(sheetImpl.Application, (IWorksheet) sheetImpl);
    for (; row < lastRow; ++row)
    {
      cell.ResetRowColumn(row, columnIndex);
      int column = cell.Column;
      if (!cell.IsBlank && !cell.WrapText && (cell.HasString || cell.FormulaStringValue != null))
      {
        int width = (int) sheetImpl.MeasureCell((IRange) cell, false, false).Width;
        int columnWidthInPixels = sheetImpl.GetColumnWidthInPixels(columnIndex);
        if (width > columnWidthInPixels)
        {
          int num = 1;
          migrantRangeImpl.ResetRowColumn(row, column + num);
          while (migrantRangeImpl.IsBlank && width > columnWidthInPixels && column < maxColumnCount)
          {
            columnWidthInPixels += sheetImpl.GetColumnWidthInPixels(column + num);
            column += num;
            migrantRangeImpl.ResetRowColumn(row, column + num);
          }
          float total = this.ColumnWidthGetter.GetTotal(Math.Min(column, cell.Column), Math.Max(column, cell.Column));
          maxList.Add(total);
        }
        else
          maxList.Add((float) columnWidthInPixels);
      }
    }
    return this.GetMaxWidth(maxList);
  }

  private float GetMaxWidth(List<float> maxList)
  {
    float maxWidth = 0.0f;
    for (int index = 0; index < maxList.Count; ++index)
    {
      float max = maxList[index];
      if ((double) max > (double) maxWidth)
        maxWidth = max;
    }
    return maxWidth;
  }

  private void WriteSheetContent(
    WorksheetImpl sheet,
    string outputDirectoryPath,
    HtmlSaveOptions saveOption,
    XmlWriter Writer,
    string style)
  {
    StringBuilder stringBuilder = new StringBuilder();
    IRange usedRange = sheet.UsedRange;
    int row1 = usedRange.Row;
    int column = usedRange.Column;
    int lastRow = usedRange.LastRow;
    int num1 = usedRange.LastColumn;
    Dictionary<long, MergedCellInfo> dictionary1 = (Dictionary<long, MergedCellInfo>) null;
    Dictionary<long, long> dictionary2 = new Dictionary<long, long>();
    List<int> columnsWidth = new List<int>();
    RecordExtractor recordExtractor = new RecordExtractor();
    IPictures pictures = sheet.Pictures;
    int num2 = 0;
    int num3 = 0;
    MigrantRangeImpl result = new MigrantRangeImpl(sheet.Application, (IWorksheet) sheet);
    int num4 = 0;
    int num5 = 0;
    int pixels = sheet.ColumnWidthToPixels(sheet.StandardWidth);
    if (sheet.HasMergedCells)
      dictionary1 = sheet.BuildMergedRegions();
    if (this.shapeCollection != null && this.shapeCollection.Count > 0)
      this.GetUnUsedRowColumnOnShapes(sheet, out lastRow, out num1);
    if (usedRange.Row == usedRange.LastRow && usedRange.Column == usedRange.LastColumn && lastRow > 0 && num1 > 0 && (sheet.Shapes.Count != 0 || sheet.Pictures.Count != 0))
    {
      usedRange = sheet.Range[1, 1, lastRow, num1];
      num1 = usedRange.LastColumn;
      lastRow = usedRange.LastRow;
    }
    for (int iColumnIndex = 1; iColumnIndex <= num1; ++iColumnIndex)
    {
      int num6 = sheet.GetColumnWidthInPixels(iColumnIndex);
      if (num6 == 0)
        num6 = 64 /*0x40*/;
      columnsWidth.Add(num6);
      num5 += num6;
    }
    double num7 = this.workSheet.Workbook.Application.ConvertUnits((double) num5, MeasureUnits.Pixel, MeasureUnits.Point);
    Writer.WriteStartElement("table");
    Writer.WriteAttributeString("cellspacing", "0");
    Writer.WriteAttributeString("width", num5.ToString());
    Writer.WriteStartAttribute(nameof (style));
    Writer.WriteString("table-layout");
    Writer.WriteString(":");
    Writer.WriteString("fixed");
    Writer.WriteString(";");
    Writer.WriteString("border-collapse");
    Writer.WriteString(":");
    Writer.WriteString("collapse");
    Writer.WriteString(";");
    Writer.WriteString("width");
    Writer.WriteString(":");
    Writer.WriteString(num7.ToString() + "pt");
    Writer.WriteEndAttribute();
    foreach (int num8 in columnsWidth)
    {
      Writer.WriteStartElement("Col");
      Writer.WriteAttributeString("width", num8.ToString());
      Writer.WriteEndElement();
    }
    for (int returnValuerow = 1; returnValuerow <= lastRow; ++returnValuerow)
    {
      result.ResetRowColumn(returnValuerow, 1);
      int rowHeightInPixels = sheet.GetRowHeightInPixels(returnValuerow);
      int columnNo = 0;
      Writer.WriteStartElement("tr");
      Writer.WriteAttributeString("height", rowHeightInPixels.ToString());
      if (rowHeightInPixels == 0)
        Writer.WriteAttributeString(nameof (style), "display:none");
      num4 += rowHeightInPixels;
      int num9 = 0;
      int num10 = num1;
      RowStorage row2 = WorksheetHelper.GetOrCreateRow((IInternalWorksheet) sheet, returnValuerow - 1, false);
      num1 = row2 == null || row2.IsFormatted || sheet.Shapes.Count != 0 && sheet.Pictures.Count != 0 ? usedRange.LastColumn : row2.LastColumn + 1;
      for (int index1 = 1; index1 <= num1; ++index1)
      {
        result.ResetRowColumn(returnValuerow, index1);
        int num11 = index1 < columnsWidth.Count ? columnsWidth[index1 - 1] : pixels;
        num9 += num11;
        string row3 = returnValuerow.ToString();
        string str1 = row3 + this.GetColumnName(index1);
        bool isMerged = result.IsMerged;
        ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
        string imageValue = (string) null;
        this.m_sortedList.TryGetValue(RangeImpl.GetCellIndex(index1, returnValuerow), out extendedFormatImpl);
        if (extendedFormatImpl != null && extendedFormatImpl is ExtendedFormatStandAlone && (extendedFormatImpl as ExtendedFormatStandAlone).AdvancedCFIcon != null)
          imageValue = this.ImageToBase64((extendedFormatImpl as ExtendedFormatStandAlone).AdvancedCFIcon, ImageFormat.Png);
        ++columnNo;
        string str2 = result.ColumnWidth.ToString() + ",";
        ExtendedFormatImpl wrapped = (result.CellStyle as CellStyle).Wrapped;
        string cellText = saveOption.TextMode != HtmlSaveOptions.GetText.DisplayText ? result.Value : result.DisplayText;
        ExcelHAlign hAlign = extendedFormatImpl == null ? wrapped.HorizontalAlignment : extendedFormatImpl.HorizontalAlignment;
        if (!sheet.HasMergedCells)
        {
          long key = ((long) returnValuerow << 32 /*0x20*/) + (long) index1;
          if (dictionary2.ContainsKey(key))
          {
            dictionary2.Remove(key);
            continue;
          }
          Writer.WriteStartElement("td");
        }
        if (sheet.HasMergedCells)
        {
          long key1 = ((long) returnValuerow << 32 /*0x20*/) + (long) index1;
          bool flag1 = false;
          string str3 = string.Empty;
          if (dictionary1.ContainsKey(key1))
          {
            MergedCellInfo mergedCellInfo = dictionary1[key1];
            if (this.m_isLastCellNotEmpty && index1 == num1)
            {
              Writer.WriteStartElement("td");
              if (!result.IsGroupedByColumn && !result.IsGroupedByRow)
                Writer.WriteAttributeString("class", this.GetCssClassName(result, wrapped, isMerged));
              this.WriteCellStyle(result);
              Writer.WriteEndElement();
              this.m_isLastCellNotEmpty = false;
            }
            if (mergedCellInfo.IsFirst)
            {
              Writer.WriteStartElement("td");
              num2 = mergedCellInfo.ColSpan;
              num3 = mergedCellInfo.RowSpan;
              if (num3 > 1)
              {
                if (!sheet.IsRowVisible(returnValuerow))
                {
                  for (int firstRow = returnValuerow + 1; firstRow < returnValuerow + num3; ++firstRow)
                  {
                    for (int firstColumn = index1; firstColumn < index1 + num2; ++firstColumn)
                      dictionary1.Remove(RangeImpl.GetCellIndex(firstColumn, firstRow));
                  }
                  num3 = 1;
                }
                else
                {
                  int num12 = returnValuerow + num3;
                  for (int rowIndex = returnValuerow + 1; rowIndex < num12; ++rowIndex)
                  {
                    if (!sheet.IsRowVisible(rowIndex))
                      --num3;
                  }
                }
              }
              if ((mergedCellInfo.TableSpan & TableSpan.Column) != TableSpan.Default)
                Writer.WriteAttributeString("COLSPAN", num2.ToString());
              if ((mergedCellInfo.TableSpan & TableSpan.Row) != TableSpan.Default)
                Writer.WriteAttributeString("ROWSPAN", num3.ToString());
              if (this.shapeCollection != null && this.shapeCollection.ContainsKey(key1))
                str3 = "text-align:initial;vertical-align:initial;";
              flag1 = true;
            }
            else
              continue;
          }
          else
            Writer.WriteStartElement("td");
          string cssClassName = this.GetCssClassName(result, wrapped, isMerged);
          string[] strArray = cssClassName.Split(' ');
          bool flag2 = false;
          foreach (string str4 in strArray)
          {
            if (style.Contains(str4) && (str4.StartsWith("X") || str4.StartsWith("C") || str4.StartsWith("M")))
            {
              string str5 = style.Substring(style.IndexOf(str4));
              string str6 = str5.Substring(str5.IndexOf("{"), str5.IndexOf("}") - str5.IndexOf("{") + 1);
              if (str6.Contains("border-bottom:") || str6.Contains("border-left:") || str6.Contains("border-top:") || str6.Contains("border-right:") || str6.Contains("background-color:"))
              {
                flag2 = true;
                break;
              }
            }
          }
          if (!result.IsGroupedByColumn && !result.IsGroupedByRow)
            Writer.WriteAttributeString("class", cssClassName);
          if (result.HorizontalAlignment == ExcelHAlign.HAlignGeneral && this.GetHorizontalAlignmentGeneral(result) != null)
            str3 = $"{str3}text-align:{this.GetHorizontalAlignmentGeneral(result)};";
          if (result.IsGroupedByColumn || result.IsGroupedByRow)
            str3 += "display:none;";
          bool flag3 = false;
          if (this.shapeCollection != null)
          {
            long cellIndex = RangeImpl.GetCellIndex(index1, returnValuerow);
            bool flag4;
            if (this.shapeCollection.ContainsKey(cellIndex))
            {
              string str7 = str3 + "vertical-align:top;text-align:left;";
              if (str7 != string.Empty)
                Writer.WriteAttributeString(nameof (style), str7);
              this.WriteShapesOnWorksheet(cellIndex, (WorksheetBaseImpl) sheet, outputDirectoryPath, saveOption, -1L);
              this.shapeCollection.Remove(cellIndex);
              flag3 = true;
              if (flag1)
              {
                long key2 = ((long) returnValuerow << 32 /*0x20*/) + (long) index1;
                long locationOnMerged = this.CheckAndGetLocationOnMerged(sheet, dictionary1[key2], returnValuerow, index1, outputDirectoryPath, saveOption);
                if (locationOnMerged != -1L)
                {
                  this.WriteShapesOnWorksheet(locationOnMerged, (WorksheetBaseImpl) sheet, outputDirectoryPath, saveOption, cellIndex);
                  this.shapeCollection.Remove(locationOnMerged);
                }
                flag4 = false;
                flag3 = true;
              }
            }
            else if (flag1)
            {
              long key3 = ((long) returnValuerow << 32 /*0x20*/) + (long) index1;
              long locationOnMerged = this.CheckAndGetLocationOnMerged(sheet, dictionary1[key3], returnValuerow, index1, outputDirectoryPath, saveOption);
              if (locationOnMerged != -1L)
              {
                string str8 = str3 + "vertical-align:top;text-align:left;";
                if (str8 != string.Empty)
                  Writer.WriteAttributeString(nameof (style), str8);
                this.WriteShapesOnWorksheet(locationOnMerged, (WorksheetBaseImpl) sheet, outputDirectoryPath, saveOption, cellIndex);
                this.shapeCollection.Remove(locationOnMerged);
              }
              else if (str3 != string.Empty)
                Writer.WriteAttributeString(nameof (style), str3);
              flag4 = false;
              flag3 = true;
            }
            else if (str3 != string.Empty)
              Writer.WriteAttributeString(nameof (style), str3);
          }
          else if (str3 != string.Empty)
            Writer.WriteAttributeString(nameof (style), str3);
          if (index1 - 1 < columnsWidth.Count)
            this.DrawIconSet(imageValue, columnsWidth[index1 - 1], rowHeightInPixels, hAlign, ref cssClassName);
          else
            this.DrawIconSet(imageValue, pixels, rowHeightInPixels, hAlign, ref cssClassName);
          if (flag3)
            Writer.WriteStartElement("span");
          if (cellText.Equals("") && flag2)
            Writer.WriteRaw("&nbsp;");
          else if (!cellText.Equals(""))
          {
            int num13 = 0;
            int count = 0;
            if (num3 <= 1 && !result.WrapText)
              count = this.WriteCellWidth((IRange) result, Writer, columnsWidth, columnNo, row3, sheet, saveOption, cellText);
            int num14 = index1;
            if (num2 <= 1 && num13 > 1)
              num2 = num13;
            for (int index2 = 0; index2 < num2; ++index2)
            {
              long key4 = ((long) returnValuerow << 32 /*0x20*/) + (long) num14;
              if (!dictionary1.ContainsKey(key4))
                dictionary1.Add(key4, new MergedCellInfo());
              ++num14;
            }
            this.NormalizeString(cellText, (IRange) result, wrapped, count, columnsWidth, columnNo, sheet, saveOption, isMerged);
          }
          num2 = 0;
          num3 = 0;
          if (flag3)
            Writer.WriteEndElement();
          if (imageValue != null)
          {
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteEndElement();
          }
          Writer.WriteEndElement();
        }
        if (!sheet.HasMergedCells)
        {
          int num15 = 0;
          int count = 0;
          if (!result.WrapText)
            count = this.WriteCellWidth((IRange) result, Writer, columnsWidth, columnNo, row3, sheet, saveOption, cellText);
          int num16 = index1;
          if (num15 > 1)
          {
            for (int index3 = 0; index3 < num15; ++index3)
            {
              long key = ((long) returnValuerow << 32 /*0x20*/) + (long) num16;
              if (!dictionary2.ContainsKey(key))
                dictionary2.Add(key, key);
              ++num16;
            }
          }
          string cssClassName = this.GetCssClassName(result, wrapped, isMerged);
          if (!result.IsGroupedByColumn && !result.IsGroupedByColumn)
            Writer.WriteAttributeString("class", cssClassName);
          string[] strArray = cssClassName.Split(' ');
          bool flag5 = false;
          foreach (string str9 in strArray)
          {
            if (style.Contains(str9) && (str9.StartsWith("X") || str9.StartsWith("C") || str9.StartsWith("M")))
            {
              string str10 = style.Substring(style.IndexOf(str9));
              string str11 = str10.Substring(str10.IndexOf("{"), str10.IndexOf("}") - str10.IndexOf("{") + 1);
              if (str11.Contains("border-bottom:") || str11.Contains("border-left:") || str11.Contains("border-top:") || str11.Contains("border-right:") || str11.Contains("background-color:"))
              {
                flag5 = true;
                break;
              }
            }
          }
          bool flag6 = false;
          string str12 = string.Empty;
          if (result.IsGroupedByColumn || result.IsGroupedByRow)
            str12 += "display:none;";
          if (result.HorizontalAlignment == ExcelHAlign.HAlignGeneral && this.GetHorizontalAlignmentGeneral(result) != null)
            str12 = $"{str12}text-align:{this.GetHorizontalAlignmentGeneral(result)};";
          if (this.shapeCollection != null)
          {
            long cellIndex = RangeImpl.GetCellIndex(index1, returnValuerow);
            if (this.shapeCollection.ContainsKey(cellIndex))
            {
              string str13 = str12 + "vertical-align:top;text-align:left;";
              if (str13 != string.Empty)
                Writer.WriteAttributeString(nameof (style), str13);
              this.WriteShapesOnWorksheet(cellIndex, (WorksheetBaseImpl) sheet, outputDirectoryPath, saveOption, -1L);
              this.shapeCollection.Remove(cellIndex);
              flag6 = true;
            }
            else if (str12 != string.Empty)
              Writer.WriteAttributeString(nameof (style), str12);
          }
          else if (str12 != string.Empty)
            Writer.WriteAttributeString(nameof (style), str12);
          if (index1 - 1 < columnsWidth.Count)
            this.DrawIconSet(imageValue, columnsWidth[index1 - 1], rowHeightInPixels, hAlign, ref cssClassName);
          else
            this.DrawIconSet(imageValue, pixels, rowHeightInPixels, hAlign, ref cssClassName);
          if (flag6)
            Writer.WriteStartElement("span");
          if (cellText.Equals("") && flag5)
          {
            if (result.CellStyle.Font.Underline != ExcelUnderline.None)
            {
              Writer.WriteStartElement("u");
              Writer.WriteAttributeString(nameof (style), "visibility:hidden;mso-ignore:visibility");
              Writer.WriteRaw("&nbsp;");
              Writer.WriteEndElement();
            }
            else
              Writer.WriteRaw("&nbsp;");
          }
          else if (!cellText.Equals(""))
            this.NormalizeString(cellText, (IRange) result, wrapped, count, columnsWidth, columnNo, sheet, saveOption, isMerged);
          if (flag6)
            Writer.WriteEndElement();
          if (imageValue != null)
          {
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteEndElement();
            Writer.WriteEndElement();
          }
          Writer.WriteEndElement();
        }
        if (this.shapeCollection != null && this.shapeCollection.Count > 0 && index1 == num1)
          this.CheckandUpdateUnUsedRangeImages(sheet, false, returnValuerow, num1, out returnValuerow, out num1);
      }
      num1 = num10;
      Writer.WriteEndElement();
      if (this.shapeCollection != null && this.shapeCollection.Count > 0 && returnValuerow == lastRow)
        this.CheckandUpdateUnUsedRangeImages(sheet, true, lastRow, sheet.UsedRange.LastColumn, out lastRow, out num1);
    }
    if (sheet.IsEmpty)
      Writer.WriteString("");
    Writer.WriteEndElement();
  }

  private void DrawIconSet(
    string imageValue,
    int width,
    int height,
    ExcelHAlign hAlign,
    ref string className)
  {
    if (imageValue == null)
      return;
    string str;
    switch (hAlign)
    {
      case ExcelHAlign.HAlignLeft:
        str = "left";
        break;
      case ExcelHAlign.HAlignCenter:
        str = "center";
        break;
      default:
        str = "right";
        break;
    }
    this.Writer.WriteStartElement("table");
    this.Writer.WriteAttributeString("cellSpacing", "0");
    this.Writer.WriteAttributeString("Height", height.ToString());
    this.Writer.WriteAttributeString("Width", width.ToString());
    this.Writer.WriteStartElement("body");
    this.Writer.WriteStartElement("tr");
    this.Writer.WriteAttributeString("class", className);
    this.Writer.WriteStartElement("td");
    this.Writer.WriteAttributeString(nameof (width), "16");
    this.Writer.WriteStartElement("div");
    this.Writer.WriteAttributeString("style", "margin-left:1px;margin-top:1px;width:16px;height:16px");
    this.Writer.WriteStartElement("img");
    this.Writer.WriteAttributeString("src", "data:image/png;base64," + imageValue);
    this.Writer.WriteEndElement();
    this.Writer.WriteEndElement();
    this.Writer.WriteStartElement("td");
    this.Writer.WriteStartElement("div");
    this.Writer.WriteAttributeString("style", $"text-align:{str};");
    className = string.Empty;
  }

  private string GetCssClassName(MigrantRangeImpl result, ExtendedFormatImpl xf, bool isCellMerged)
  {
    string empty = string.Empty;
    string cssClassName = !result.HasConditionFormats ? "X" + result.ExtendedFormatIndex.ToString() : $"CSheet{(object) result.Worksheet.Index}{result.AddressLocal}";
    if (isCellMerged && this.m_cssStyles.ContainsKey($".MSheet{(object) result.Worksheet.Index}{result.AddressLocal}"))
      cssClassName = $"{cssClassName} MSheet{(object) result.Worksheet.Index}{result.AddressLocal}";
    return cssClassName;
  }

  private string GetHorizontalAlignmentGeneral(MigrantRangeImpl result)
  {
    switch (result.CellType)
    {
      case RangeImpl.TCellType.Formula:
        switch ((result.Worksheet as WorksheetImpl).GetCellType(result.Row, result.Column, true))
        {
          case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
          case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
            return "center";
          case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
            return !(result.NumberFormat == "@") ? "right" : "left";
          case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
            return "left";
        }
        break;
      case RangeImpl.TCellType.RString:
      case RangeImpl.TCellType.LabelSST:
      case RangeImpl.TCellType.Label:
        return "left";
      case RangeImpl.TCellType.Number:
      case RangeImpl.TCellType.RK:
        return !(result.NumberFormat == "@") ? "right" : "left";
      case RangeImpl.TCellType.BoolErr:
        return "center";
    }
    return (string) null;
  }

  private void WriteCellStyle(MigrantRangeImpl result)
  {
    if (result.HorizontalAlignment != ExcelHAlign.HAlignGeneral)
      return;
    string alignmentGeneral = this.GetHorizontalAlignmentGeneral(result);
    if (alignmentGeneral == null)
      return;
    this.Writer.WriteAttributeString("style", "text-align:" + alignmentGeneral);
  }

  private void SerializeHyperLink(IRange cell)
  {
    IHyperLinks hyperLinks = this.workSheet.HyperLinks;
    if (cell.HasFormula && cell.Formula.StartsWith("=HYPERLINK") && cell.DisplayText.Length > 0)
    {
      this.Writer.WriteStartElement("a");
      string formula = cell.Formula;
      this.Writer.WriteAttributeString("href", this.GetAddress(formula.Substring(formula.IndexOf('(') + 1)));
      this.Writer.WriteAttributeString("target", "_parent");
      this.Writer.WriteEndElement();
    }
    else if (hyperLinks.Count == 0)
      return;
    CollectionBase<HyperLinkImpl> hyperlinks = (CollectionBase<HyperLinkImpl>) (cell.Hyperlinks as HyperLinksCollection);
    if (hyperlinks.InnerList.Count <= 0)
      return;
    HyperLinkImpl inner = hyperlinks.InnerList[0];
    switch (inner.Type)
    {
      case ExcelHyperLinkType.Url:
        this.Writer.WriteStartElement("a");
        this.Writer.WriteAttributeString("href", inner.Address);
        this.Writer.WriteAttributeString("target", "_parent");
        this.Writer.WriteEndElement();
        break;
      case ExcelHyperLinkType.File:
      case ExcelHyperLinkType.Unc:
        this.Writer.WriteStartElement("a");
        Path.GetDirectoryName(inner.Address);
        this.Writer.WriteAttributeString("href", "../" + inner.Address);
        this.Writer.WriteAttributeString("target", "_parent");
        this.Writer.WriteEndElement();
        break;
      case ExcelHyperLinkType.Workbook:
        string[] strArray = inner.Address.Split('!');
        this.Writer.WriteStartElement("a");
        strArray[0] = strArray[0].Replace("'", string.Empty);
        this.Writer.WriteAttributeString("href", $"{strArray[0]}.html#RANGE!{strArray[1]}");
        this.Writer.WriteAttributeString("target", "_parent");
        this.Writer.WriteEndElement();
        break;
    }
  }

  private string GetAddress(string address)
  {
    string empty = string.Empty;
    string displayText;
    if (address.StartsWith("\""))
    {
      displayText = address.Split(new string[1]{ "\"" }, StringSplitOptions.None)[1];
    }
    else
    {
      address = !address.Contains(",") ? address.Remove(address.IndexOf(")")) : address.Substring(0, address.IndexOf(","));
      displayText = this.workSheet[address].DisplayText;
    }
    return displayText;
  }

  private void NormalizeString(
    string value,
    IRange result,
    ExtendedFormatImpl xf,
    int count,
    List<int> columnsWidth,
    int columnNo,
    WorksheetImpl sheet,
    HtmlSaveOptions saveOption,
    bool isCellMerged)
  {
    int num1 = 0;
    if (count == columnsWidth.Count)
      this.m_isLastCellNotEmpty = true;
    if (!result.IsMerged && count == 1)
      count = 0;
    for (int column = result.Column; column <= result.Column + count; ++column)
    {
      int columnWidthInPixels = sheet.GetColumnWidthInPixels(column);
      num1 += int.Parse(columnWidthInPixels.ToString());
    }
    IFont font = result.CellStyle.Font;
    Font nativeFont1 = font.GenerateNativeFont();
    ExcelToHtmlConverter.StringMeasurer stringMeasurer = new ExcelToHtmlConverter.StringMeasurer();
    if (Convert.ToInt32(double.Parse(stringMeasurer.Measure(value, nativeFont1).Width.ToString())) <= num1 || isCellMerged || result.WrapText)
    {
      if (result.CellStyleName == "Hyperlink" || result.HasFormula && result.Formula.StartsWith("=HYPERLINK") && value.Length > 0)
        this.SerializeHyperLink(result);
      Color numberFormatColor = ((RangeImpl) result).GetNumberFormatColor(xf);
      bool flag1 = false;
      if (numberFormatColor != Color.Empty)
      {
        flag1 = true;
        this.Writer.WriteStartElement("font");
        this.Writer.WriteStartAttribute("color");
        this.Writer.WriteString($"#{numberFormatColor.ToArgb() & 16777215 /*0xFFFFFF*/:X6}");
        this.Writer.WriteStartAttribute("style");
        this.Writer.WriteString("mso-ignore:color");
        this.Writer.WriteEndAttribute();
      }
      this.Writer.WriteStartElement("span");
      if (value.StartsWith(" "))
      {
        this.Writer.WriteStartAttribute("style");
        this.Writer.WriteString("white-space");
        this.Writer.WriteString(":");
        this.Writer.WriteString("pre");
        this.Writer.WriteEndAttribute();
      }
      ExcelFormatType formatType = (result.Worksheet.Workbook as WorkbookImpl).InnerFormats[xf.NumberFormatIndex].GetFormatType(result.Number);
      if (result.HasRichText)
      {
        char[] charArray = result.HtmlString.ToCharArray();
        bool flag2 = false;
        foreach (char ch in charArray)
        {
          if (ch > 'ÿ')
            flag2 = true;
        }
        if (flag2 && !result.HtmlString.Contains("<Font"))
          this.Writer.WriteRaw(ExcelToHtmlConverter.GetUnicode(result.HtmlString));
        else
          this.Writer.WriteRaw(result.HtmlString.Replace("\u0004", ExcelToHtmlConverter.GetUnicode("\u0004")).Replace("\u0005", ExcelToHtmlConverter.GetUnicode("\u0005")));
      }
      else if (result.WrapText)
      {
        value = value.Replace("&", "&amp;");
        value = value.Replace("<", "&lt;");
        value = value.Replace(">", "&gt;");
        value = value.Replace("\n", "<br>");
        this.Writer.WriteRaw(value);
      }
      else
      {
        if (formatType == ExcelFormatType.Text && result.NumberFormat.StartsWith("@"))
        {
          if (result.NumberFormat.TrimStart('@') != string.Empty)
          {
            this.Writer.WriteStartAttribute("style");
            this.Writer.WriteString("text-align");
            this.Writer.WriteString(":");
            string text = (string) null;
            switch (result.HorizontalAlignment)
            {
              case ExcelHAlign.HAlignGeneral:
              case ExcelHAlign.HAlignLeft:
              case ExcelHAlign.HAlignFill:
              case ExcelHAlign.HAlignDistributed:
                text = "left;";
                break;
              case ExcelHAlign.HAlignCenter:
              case ExcelHAlign.HAlignCenterAcrossSelection:
                text = "center;";
                break;
              case ExcelHAlign.HAlignRight:
                text = "right;";
                break;
              case ExcelHAlign.HAlignJustify:
                text = "justify;";
                break;
            }
            this.Writer.WriteString(text);
            this.Writer.WriteString("display:block");
            this.Writer.WriteEndAttribute();
            this.Writer.WriteString(value + result.NumberFormat.TrimStart('@').Trim('"'));
            goto label_37;
          }
        }
        if (value.Contains("<") || value.Contains(">"))
        {
          this.Writer.WriteString(value);
        }
        else
        {
          value = ExcelToHtmlConverter.GetUnicode(value);
          this.Writer.WriteRaw(value);
        }
      }
label_37:
      this.Writer.WriteEndElement();
      if (!flag1)
        return;
      this.Writer.WriteEndElement();
    }
    else
    {
      value.ToCharArray();
      double num2 = double.Parse(num1.ToString());
      string text1 = "";
      string text2 = "0";
      double num3 = double.Parse(stringMeasurer.Measure(text2, nativeFont1).Width.ToString());
      double num4 = 0.0;
      StringBuilder stringBuilder = new StringBuilder();
      if (result.HasRichText)
      {
        char[] charArray = result.HtmlString.ToCharArray();
        bool flag = false;
        foreach (char ch in charArray)
        {
          if (ch > 'ÿ')
            flag = true;
        }
        string[] strArray1 = Regex.Split(!flag || result.HtmlString.Contains("<Font") ? result.HtmlString.Replace("\u0004", ExcelToHtmlConverter.GetUnicode("\u0004")).Replace("\u0005", ExcelToHtmlConverter.GetUnicode("\u0005")) : ExcelToHtmlConverter.GetUnicode(result.HtmlString), "</Font>");
        RangeRichTextString richText = result.RichText as RangeRichTextString;
        List<IFont> richTextFonts = new List<IFont>();
        (sheet.Workbook as WorkbookImpl).GetDrawString(result.Text, (RichTextString) richText, out richTextFonts, font);
        for (int index = 0; index < strArray1.Length - 1; ++index)
        {
          string empty1 = string.Empty;
          string[] strArray2 = strArray1[index].Split('>');
          int length = 0;
          string empty2 = string.Empty;
          string empty3 = string.Empty;
          if (strArray2.Length == 2)
          {
            string str1 = strArray2[0];
            string str2 = strArray2[1];
            Font nativeFont2 = richTextFonts[index].GenerateNativeFont();
            for (int startIndex = 0; startIndex < str2.Length; ++startIndex)
            {
              string text3 = str2.Substring(startIndex, 1);
              double num5 = double.Parse((string.IsNullOrEmpty(text3) || text3.Trim().Length == 0 ? stringMeasurer.Measure(text3 + text2, nativeFont2) : stringMeasurer.Measure(text3, nativeFont2)).Width.ToString());
              if (string.IsNullOrEmpty(text3) || text3.Trim().Length == 0)
                num5 -= num3;
              num4 += num5;
              if (num4 <= num2)
                empty1 += text3;
              else
                ++length;
            }
            if (length < 1)
            {
              stringBuilder.Append($"{str1}>{empty1}</Font>");
            }
            else
            {
              int startIndex = str2.Length - length;
              string str3 = length != str2.Length ? str2.Substring(startIndex, length) : str2.Substring(startIndex, str2.Length);
              stringBuilder.Append($"{str1}>{empty1}<span>{str3}</span></Font>");
            }
          }
          else
            stringBuilder.Append(strArray1[index]);
        }
        this.Writer.WriteRaw(stringBuilder.ToString());
      }
      else
      {
        int length;
        for (length = 0; length < value.Length; ++length)
        {
          string text4 = value.Substring(0, value.Length - length);
          double num6 = double.Parse((string.IsNullOrEmpty(text4) || text4.Trim().Length == 0 ? stringMeasurer.Measure(text4 + text2, nativeFont1) : stringMeasurer.Measure(text4, nativeFont1)).Width.ToString());
          if (string.IsNullOrEmpty(text4) || text4.Trim().Length == 0)
            num6 -= num3;
          if (num6 <= num2)
          {
            text1 = text4;
            break;
          }
        }
        int startIndex = value.Length - length;
        string text5 = value.Substring(startIndex, length);
        if (result.CellStyleName == "Hyperlink" || result.HasFormula && result.Formula.StartsWith("=HYPERLINK") && value.Length > 0)
          this.SerializeHyperLink(result);
        this.Writer.WriteString(text1);
        this.Writer.WriteStartElement("span");
        this.Writer.WriteAttributeString("style", "display:none");
        this.Writer.WriteString(text5);
        this.Writer.WriteEndElement();
      }
    }
  }

  internal static string GetUnicode(string unicodeText)
  {
    string unicode = "";
    foreach (char ch in unicodeText)
    {
      int int32 = Convert.ToInt32(ch);
      unicode = int32 < 49 || int32 > 122 ? unicode + $"&#{int32.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo)};" : unicode + (object) ch;
    }
    return unicode;
  }

  private int WriteCellWidth(
    IRange result,
    XmlWriter Writer,
    List<int> columnsWidth,
    int columnNo,
    string row,
    WorksheetImpl sheet,
    HtmlSaveOptions saveOption,
    string cellText)
  {
    Font nativeFont = result.CellStyle.Font.GenerateNativeFont();
    int num1 = 0;
    int num2 = 0;
    if (cellText != "" || result.HasStyle)
    {
      int int32 = Convert.ToInt32(double.Parse(new ExcelToHtmlConverter.StringMeasurer().Measure(cellText, nativeFont).Width.ToString()));
      int index = columnNo - 1;
      int num3 = result.Column + 1;
      int num4 = index < columnsWidth.Count ? int.Parse(columnsWidth[index].ToString()) : sheet.ColumnWidthToPixels(sheet.StandardWidth);
      num1 = 1;
      num2 = 1;
      for (int column = result.Column; column < columnsWidth.Count && int32 > num4; ++column)
      {
        string columnName = this.GetColumnName(column + 1);
        ++num3;
        string name = row + columnName;
        result = sheet.Range[name];
        string str = saveOption.TextMode != HtmlSaveOptions.GetText.DisplayText ? result.Value : result.DisplayText;
        if (str == " " || str == "  ")
          str = "";
        if (!(str != ""))
        {
          if (str == "")
          {
            num4 += int.Parse(columnsWidth[column].ToString());
            ++num2;
          }
        }
        else
          break;
      }
    }
    if (num1 > 1)
      Writer.WriteAttributeString("COLSPAN", num1.ToString());
    return num2;
  }

  private void WriteImage(
    WorksheetBaseImpl sheet,
    XmlWriter Writer,
    string outputDirectoryPath,
    HtmlSaveOptions saveOption,
    IPictureShape pictureShape,
    int index,
    long startCellIndex)
  {
    Image picture = pictureShape.Picture;
    if (picture == null)
      return;
    ImageFormat rawFormat = picture.RawFormat;
    string extension = this.GetExtension(rawFormat);
    string name = sheet.Name;
    string.Format("{0}." + extension, (object) (name + (object) index));
    string str1 = $"{this.GetShapeStyleInline((IShape) pictureShape, startCellIndex)};z-index:{(object) index}";
    Writer.WriteStartElement("span");
    if (pictureShape.Hyperlink != null)
    {
      switch (pictureShape.Hyperlink.Type)
      {
        case ExcelHyperLinkType.Url:
          Writer.WriteStartElement("a");
          Writer.WriteAttributeString("href", pictureShape.Hyperlink.Address);
          Writer.WriteAttributeString("target", "_parent");
          break;
        case ExcelHyperLinkType.File:
        case ExcelHyperLinkType.Unc:
          Writer.WriteStartElement("a");
          Writer.WriteAttributeString("href", "../" + pictureShape.Hyperlink.Address);
          Writer.WriteAttributeString("target", "_parent");
          break;
        case ExcelHyperLinkType.Workbook:
          string[] strArray = pictureShape.Hyperlink.Address.Split('!');
          Writer.WriteStartElement("a");
          strArray[0] = strArray[0].Replace("'", string.Empty);
          Writer.WriteAttributeString("href", $"{strArray[0]}.html#RANGE!{strArray[1]}");
          Writer.WriteAttributeString("target", "_parent");
          break;
      }
    }
    Writer.WriteStartElement("img");
    Writer.WriteAttributeString("style", str1);
    if (outputDirectoryPath != null)
    {
      string fullPath1 = Path.GetFullPath(Path.Combine(saveOption.ImagePath, string.Format("{0}." + extension, (object) (name + (object) index))));
      Writer.WriteAttributeString("src", "file:///" + fullPath1);
      string fullPath2 = Path.GetFullPath(saveOption.ImagePath);
      if (!Directory.Exists(fullPath2))
        Directory.CreateDirectory(fullPath2);
      picture.Save(fullPath1);
    }
    else
    {
      string base64 = this.ImageToBase64(picture, rawFormat);
      Writer.WriteAttributeString("src", $"data:image/{extension};base64,{base64}");
    }
    string str2 = string.IsNullOrEmpty(pictureShape.AlternativeText) ? "Image" : pictureShape.AlternativeText;
    Writer.WriteAttributeString("alt", str2);
    Writer.WriteEndElement();
    Writer.WriteEndElement();
    if (pictureShape.Hyperlink != null)
      Writer.WriteEndElement();
    if (!(pictureShape is BitmapShapeImpl))
      return;
    BitmapShapeImpl bitmapShapeImpl = pictureShape as BitmapShapeImpl;
    double leftOffset = (double) bitmapShapeImpl.CropLeftOffset / 1000.0;
    double topOffset = (double) (bitmapShapeImpl.CropTopOffset / 1000);
    double rightOffset = (double) (bitmapShapeImpl.CropRightOffset / 1000);
    double bottomOffset = (double) (bitmapShapeImpl.CropBottomOffset / 1000);
    if (pictureShape.Height >= picture.Size.Height || pictureShape.Width >= picture.Width || bitmapShapeImpl.CropLeftOffset <= 0 || bitmapShapeImpl.CropTopOffset <= 0 || bitmapShapeImpl.CropRightOffset <= 0 || bitmapShapeImpl.CropLeftOffset <= 0)
      return;
    RangeImpl.CropImage(picture, leftOffset, topOffset, rightOffset, bottomOffset, bitmapShapeImpl.HasTransparency);
  }

  public Image ConvertChartToImage(IChart chart)
  {
    MemoryStream imageAsStream = new MemoryStream();
    chart.SaveAsImage((Stream) imageAsStream);
    if (imageAsStream.Length <= 0L)
      return (Image) null;
    Image image = Image.FromStream((Stream) imageAsStream);
    imageAsStream.Dispose();
    return image;
  }

  private Stream ConvertChartToImageStream(IChart chart)
  {
    MemoryStream imageAsStream = new MemoryStream();
    chart.SaveAsImage((Stream) imageAsStream);
    return (Stream) imageAsStream;
  }

  private void WriteChart(
    WorksheetBaseImpl sheet,
    XmlWriter Writer,
    string outputDirectoryPath,
    HtmlSaveOptions saveOption,
    Stream chart,
    string imageName,
    int index,
    long startCellIndex)
  {
    if (chart.Length <= 0L)
      return;
    Image img = Image.FromStream(chart);
    ImageFormat rawFormat = img.RawFormat;
    string extension = this.GetExtension(rawFormat);
    string name = sheet.Name;
    string.Format("{0}." + extension, (object) imageName);
    string shapeStyleInline = this.GetShapeStyleInline((IShape) sheet.Charts[index], startCellIndex);
    Writer.WriteStartElement("span");
    Writer.WriteStartElement("img");
    if (outputDirectoryPath != null)
    {
      Writer.WriteAttributeString("style", shapeStyleInline);
      string filename = Path.GetFullPath(Path.Combine(saveOption.ImagePath, string.Format("{0}." + extension, (object) imageName)));
      string fullPath = Path.GetFullPath(saveOption.ImagePath);
      if (!Directory.Exists(fullPath))
        Directory.CreateDirectory(fullPath);
      string[] files = Directory.GetFiles(fullPath);
      if (Array.IndexOf<string>(files, filename) > -1)
      {
        while (Array.IndexOf<string>(files, filename.Replace(imageName, imageName + (object) index)) > -1)
          ++index;
        filename = filename.Replace(imageName, imageName + (object) index);
      }
      Writer.WriteAttributeString("src", "file:///" + filename);
      img.Save(filename);
      img.Dispose();
      chart.Dispose();
    }
    else
    {
      string empty = string.Empty;
      string base64 = this.ImageToBase64(img, rawFormat);
      Writer.WriteAttributeString("style", shapeStyleInline);
      Writer.WriteAttributeString("src", $"data:image/{extension};base64,{base64}");
    }
    Writer.WriteAttributeString("alt", imageName);
    Writer.WriteEndElement();
    Writer.WriteEndElement();
  }

  private string ImageToBase64(Image img, ImageFormat imageFormat)
  {
    string empty = string.Empty;
    Image image = (Image) new Bitmap(img.Width, img.Height);
    using (Graphics graphics = Graphics.FromImage(image))
      graphics.DrawImage(img, 0, 0, image.Width, image.Height);
    using (MemoryStream memoryStream = new MemoryStream())
    {
      image.Save((Stream) memoryStream, imageFormat);
      return Convert.ToBase64String(memoryStream.ToArray());
    }
  }

  private string GetExtension(ImageFormat format)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    return !format.Equals((object) ImageFormat.Bmp) ? (!format.Equals((object) ImageFormat.Jpeg) ? (!format.Equals((object) ImageFormat.Png) ? (!format.Equals((object) ImageFormat.Emf) ? (!format.Equals((object) ImageFormat.Gif) ? "png" : "gif") : "emf") : "png") : "jpeg") : "bmp";
  }

  private void WriteBackGroundImage(
    WorksheetBaseImpl sheet,
    XmlWriter Writer,
    string outputDirectoryPath,
    HtmlSaveOptions saveOption,
    Image pictureShape)
  {
    Image img = pictureShape;
    if (img == null)
      return;
    ImageFormat rawFormat = img.RawFormat;
    string extension = this.GetExtension(rawFormat);
    string name = sheet.Name;
    string.Format("{0}." + extension, (object) name);
    if (outputDirectoryPath != null)
    {
      if (saveOption.ImagePath == null)
        saveOption.ImagePath = outputDirectoryPath;
      string fullPath1 = Path.GetFullPath(Path.Combine(saveOption.ImagePath, string.Format("{0}." + extension, (object) name)));
      Writer.WriteAttributeString("background", "file:///" + fullPath1);
      string fullPath2 = Path.GetFullPath(saveOption.ImagePath);
      if (!Directory.Exists(fullPath2))
        Directory.CreateDirectory(fullPath2);
      img.Save(fullPath1);
    }
    else
    {
      string base64 = this.ImageToBase64(img, rawFormat);
      Writer.WriteAttributeString("background", $"data:image/{extension};base64,{base64}");
    }
  }

  private string GetColumnName(int iColumn)
  {
    --iColumn;
    string columnName = string.Empty;
    do
    {
      int num = iColumn % 26;
      iColumn = iColumn / 26 - 1;
      columnName = ((char) (65 + num)).ToString() + columnName;
    }
    while (iColumn >= 0);
    return columnName;
  }

  private void BuildHtmlFiles(
    WorkbookImpl book,
    string outputDirectoryPath,
    HtmlSaveOptions saveOption)
  {
    string name = new DirectoryInfo(outputDirectoryPath).Name;
    int count = book.Worksheets.Count;
    string styleSheet = Path.Combine(outputDirectoryPath, "stylesheet.css");
    string styles = (string) null;
    this.BuildCommonStyles(styleSheet, book, out styles);
    this.m_commonStyles = new Dictionary<string, string>((IDictionary<string, string>) this.m_cssStyles);
    for (int Index = 0; Index < count; ++Index)
    {
      if (book.Worksheets[Index].Visibility == WorksheetVisibility.Visible)
      {
        string path2 = $"{book.Worksheets[Index].Name}.html";
        string path = Path.Combine(outputDirectoryPath, path2);
        if (File.Exists(path))
          File.Delete(path);
        using (FileStream output = new FileStream(path, FileMode.CreateNew))
        {
          WorksheetImpl worksheet = (WorksheetImpl) book.Worksheets[Index];
          this.workSheet = (IWorksheet) worksheet;
          this.m_writer = XmlWriter.Create((Stream) output, this.XmlSettings);
          this.m_cssStyles.Clear();
          this.lstRegions = new List<MergeCellsRecord.MergedRegion>();
          this.WriteDocumentStart();
          string style;
          this.BuildStyles(worksheet, saveOption, this.m_writer, out style);
          if (worksheet.HasPictures || worksheet.HasCharts)
            this.InitiateShapeCollection();
          this.Writer.WriteStartElement("body");
          if (worksheet.PageSetup.BackgoundImage != null)
            this.WriteBackGroundImage((WorksheetBaseImpl) worksheet, this.Writer, outputDirectoryPath, saveOption, (Image) worksheet.PageSetup.BackgoundImage);
          style += styles;
          if (worksheet.IsRightToLeft)
          {
            this.Writer.WriteStartElement("div");
            this.Writer.WriteAttributeString("dir", "RTL");
          }
          this.WriteSheetContent(worksheet, outputDirectoryPath, saveOption, this.m_writer, style);
          if (worksheet.IsRightToLeft)
            this.Writer.WriteEndElement();
          this.Writer.WriteEndElement();
          this.WriteDocumentEnd();
          output.Close();
        }
      }
    }
    this.m_commonStyles.Clear();
  }

  private void InitiateShapeCollection()
  {
    this.shapeCollection = new Dictionary<long, List<ExcelToHtmlConverter.ShapeInfo>>();
    if (this.workSheet.Charts.Count > 0 && this.workSheet.Application.ChartToImageConverter != null)
    {
      for (int index = 0; index < this.workSheet.Charts.Count; ++index)
      {
        ShapeImpl chart = this.workSheet.Charts[index] as ShapeImpl;
        int topRow = chart.TopRow;
        long cellIndex = RangeImpl.GetCellIndex(chart.LeftColumn, topRow);
        if (!this.shapeCollection.ContainsKey(cellIndex))
          this.shapeCollection.Add(cellIndex, new List<ExcelToHtmlConverter.ShapeInfo>()
          {
            new ExcelToHtmlConverter.ShapeInfo()
            {
              Index = index,
              Shape = (IShape) chart
            }
          });
        else
          this.shapeCollection[cellIndex].Add(new ExcelToHtmlConverter.ShapeInfo()
          {
            Index = index,
            Shape = (IShape) chart
          });
      }
    }
    if (this.workSheet.Pictures.Count <= 0)
      return;
    for (int Index = 0; Index < this.workSheet.Pictures.Count; ++Index)
    {
      ShapeImpl picture = this.workSheet.Pictures[Index] as ShapeImpl;
      int topRow = picture.TopRow;
      long cellIndex = RangeImpl.GetCellIndex(picture.LeftColumn, topRow);
      if (!this.shapeCollection.ContainsKey(cellIndex))
        this.shapeCollection.Add(cellIndex, new List<ExcelToHtmlConverter.ShapeInfo>()
        {
          new ExcelToHtmlConverter.ShapeInfo()
          {
            Index = Index,
            Shape = (IShape) picture
          }
        });
      else
        this.shapeCollection[cellIndex].Add(new ExcelToHtmlConverter.ShapeInfo()
        {
          Index = Index,
          Shape = (IShape) picture
        });
    }
  }

  private string GetShapeStyleInline(IShape shape, long startCellIndex)
  {
    string str = "";
    ShapeImpl shapeImpl = shape as ShapeImpl;
    int shapeOffsetInPixels1 = this.GetShapeOffsetInPixels(shapeImpl, shapeImpl.TopRow, shapeImpl.TopRowOffset, 256 /*0x0100*/);
    int shapeOffsetInPixels2 = this.GetShapeOffsetInPixels(shapeImpl, shapeImpl.LeftColumn, shapeImpl.LeftColumnOffset, 1024 /*0x0400*/);
    if (startCellIndex != -1L)
    {
      IWorksheet worksheet = shapeImpl.Worksheet as IWorksheet;
      shapeOffsetInPixels1 += this.GetMergedRegionOffset(worksheet, startCellIndex, shapeImpl.TopRow, shapeImpl.LeftColumn, true);
      shapeOffsetInPixels2 += this.GetMergedRegionOffset(worksheet, startCellIndex, shapeImpl.TopRow, shapeImpl.LeftColumn, false);
    }
    return $"{$"{$"{$"{$"{str}margin-left:{(object) shapeOffsetInPixels2}px;"}margin-top:{(object) shapeOffsetInPixels1}px;"}height:{(object) shape.Height}px;"}width:{(object) shape.Width}px;"}position:absolute";
  }

  private int GetShapeOffsetInPixels(
    ShapeImpl shapeImpl,
    int position,
    int offSetValue,
    int fullConstValue)
  {
    int shapeOffsetInPixels = 0;
    if (offSetValue > 0)
      shapeOffsetInPixels = (fullConstValue != 1024 /*0x0400*/ ? (shapeImpl.Worksheet as WorksheetImpl).GetRowHeightInPixels(position) : (shapeImpl.Worksheet as WorksheetImpl).GetColumnWidthInPixels(position)) * offSetValue / fullConstValue;
    return shapeOffsetInPixels;
  }

  private void CheckandUpdateUnUsedRangeImages(
    WorksheetImpl sheet,
    bool isRow,
    int rowPosition,
    int colPosition,
    out int returnValuerow,
    out int returnValuecol)
  {
    returnValuecol = colPosition;
    returnValuerow = rowPosition;
    int lastRow = sheet.UsedRange.LastRow;
    List<long> longList = new List<long>((IEnumerable<long>) this.shapeCollection.Keys);
    for (int index = 0; index < longList.Count; ++index)
    {
      int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(longList[index]);
      if (isRow && rowFromCellIndex > lastRow)
      {
        returnValuerow = rowFromCellIndex;
        rowPosition = rowFromCellIndex;
      }
      else if (rowPosition == rowFromCellIndex)
      {
        int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(longList[index]);
        if (columnFromCellIndex > colPosition)
        {
          returnValuecol = columnFromCellIndex;
          colPosition = columnFromCellIndex;
        }
      }
    }
  }

  private void WriteShapesOnWorksheet(
    long cellIndex,
    WorksheetBaseImpl sheet,
    string outputDirectoryPath,
    HtmlSaveOptions saveOption,
    long startCellIndex)
  {
    List<ExcelToHtmlConverter.ShapeInfo> shape1 = this.shapeCollection[cellIndex];
    for (int index = 0; index < shape1.Count; ++index)
    {
      if (shape1[index].Shape is IPictureShape)
      {
        IPictureShape shape2 = shape1[index].Shape as IPictureShape;
        this.WriteImage(sheet, this.Writer, outputDirectoryPath, saveOption, shape2, shape1[index].Index, startCellIndex);
      }
      else if (shape1[index].Shape is IChart)
      {
        IChart shape3 = shape1[index].Shape as IChart;
        string name = shape3.Name;
        Stream imageStream = this.ConvertChartToImageStream(shape3);
        this.WriteChart(sheet, this.Writer, outputDirectoryPath, saveOption, imageStream, name, shape1[index].Index, startCellIndex);
      }
    }
  }

  private void GetUnUsedRowColumnOnShapes(
    WorksheetImpl sheet,
    out int moreNoOfRows,
    out int moreNoOfColumns)
  {
    int num1 = sheet.UsedRange.LastRow;
    int num2 = sheet.UsedRange.LastColumn;
    moreNoOfRows = num1;
    moreNoOfColumns = num2;
    List<long> longList = new List<long>((IEnumerable<long>) this.shapeCollection.Keys);
    for (int index = 0; index < longList.Count; ++index)
    {
      int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(longList[index]);
      int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(longList[index]);
      if (columnFromCellIndex > num2)
      {
        moreNoOfColumns = columnFromCellIndex;
        num2 = columnFromCellIndex;
      }
      if (rowFromCellIndex > num1)
      {
        moreNoOfRows = rowFromCellIndex;
        num1 = rowFromCellIndex;
      }
    }
  }

  private long CheckAndGetLocationOnMerged(
    WorksheetImpl sheet,
    MergedCellInfo mergedCellInfo,
    int row,
    int column,
    string outputDirectoryPath,
    HtmlSaveOptions saveOption)
  {
    int colSpan = mergedCellInfo.ColSpan;
    int rowSpan = mergedCellInfo.RowSpan;
    RangeImpl.GetCellIndex(column, row);
    long locationOnMerged = -1;
    if ((mergedCellInfo.TableSpan & TableSpan.Column) != TableSpan.Default && (mergedCellInfo.TableSpan & TableSpan.Row) != TableSpan.Default)
    {
      for (int index1 = 0; index1 < rowSpan; ++index1)
      {
        int firstRow = row + index1;
        for (int index2 = 0; index2 < colSpan; ++index2)
        {
          long cellIndex = RangeImpl.GetCellIndex(column + index2, firstRow);
          if (this.shapeCollection.ContainsKey(cellIndex))
            locationOnMerged = cellIndex;
        }
      }
    }
    else if ((mergedCellInfo.TableSpan & TableSpan.Column) != TableSpan.Default)
    {
      int num = column;
      for (int index = 0; index < colSpan; ++index)
      {
        long cellIndex = RangeImpl.GetCellIndex(num + index, row);
        if (this.shapeCollection.ContainsKey(cellIndex))
          locationOnMerged = cellIndex;
      }
    }
    else if ((mergedCellInfo.TableSpan & TableSpan.Row) != TableSpan.Default)
    {
      for (int index = 0; index < rowSpan; ++index)
      {
        long cellIndex = RangeImpl.GetCellIndex(column, row + index);
        if (this.shapeCollection.ContainsKey(cellIndex))
          locationOnMerged = cellIndex;
      }
    }
    return locationOnMerged;
  }

  private int GetMergedRegionOffset(
    IWorksheet sheet,
    long startCellIndex,
    int row,
    int column,
    bool isTopOffset)
  {
    int mergedRegionOffset = 0;
    int rowFromCellIndex = RangeImpl.GetRowFromCellIndex(startCellIndex);
    int columnFromCellIndex = RangeImpl.GetColumnFromCellIndex(startCellIndex);
    if (isTopOffset)
    {
      for (int iRowIndex = rowFromCellIndex; iRowIndex < row; ++iRowIndex)
        mergedRegionOffset += sheet.GetRowHeightInPixels(iRowIndex);
    }
    else
    {
      for (int iColumnIndex = columnFromCellIndex; iColumnIndex < column; ++iColumnIndex)
        mergedRegionOffset += sheet.GetColumnWidthInPixels(iColumnIndex);
    }
    return mergedRegionOffset;
  }

  public void Dispose()
  {
    this.m_cssStyles.Clear();
    this.m_writer.Close();
  }

  private enum ConversionMode
  {
    Workbook,
    Worksheet,
  }

  private struct ShapeInfo
  {
    private int index;
    private IShape shape;

    public int Index
    {
      get => this.index;
      set => this.index = value;
    }

    public IShape Shape
    {
      get => this.shape;
      set => this.shape = value;
    }
  }

  private class StringMeasurer
  {
    private Graphics m_graphics;

    public StringMeasurer() => this.InitializeGraphics();

    public void InitializeGraphics()
    {
      this.m_graphics = Graphics.FromImage((Image) new Bitmap(1, 1));
    }

    public SizeF Measure(string text, Font font)
    {
      return this.m_graphics.MeasureString(text, font, new SizeF(float.MaxValue, float.MaxValue), StringFormat.GenericTypographic);
    }
  }

  internal class HtmlTags
  {
    public const string Html = "html";
    public const string Head = "head";
    public const string Title = "title";
    public const string Style = "style";
    public const string Body = "body";
    public const string Div = "div";
    public const string A = "a";
    public const string P = "p";
    public const string Font = "font";
    public const string Table = "table";
    public const string Td = "td";
    public const string Tr = "tr";
    public const string Th = "th";
    public const string Img = "img";
    public const string Col = "Col";
    public const string span = "span";
    public const string Script = "script";
    public const string Iframe = "iframe";
    public const string Input = "input";
    public const string FrameSet = "frameset";
    public const string Frame = "frame";
    public const string NoFrames = "noframes";
    public const string Bold = "b";
    public const string Small = "small";
    public const string Header1 = "h1";
    public const string Header2 = "h2";
    public const string Header3 = "h3";
    public const string Header4 = "h4";
    public const string Header5 = "h5";
    public const string Header6 = "h6";
    public const string SuperScript = "sup";
    public const string SubScript = "sub";
    public const string Break = "br";
  }

  internal class HtmlAttributes
  {
    public const string Align = "align";
    public const string Italic = "italic";
    public const string Bold = "bold";
    public const string Underline = "underline";
    public const string BackgroundColor = "background-color";
    public const string BgColor = "bgColor";
    public const string BorderColor = "border-color";
    public const string Class = "class";
    public const string Border = "border";
    public const string FontColor = "color";
    public const string FontName = "font-name";
    public const string FontStyle = "font-style";
    public const string FontSize = "font-size";
    public const string StrikeThrough = "strike-through";
    public const string TextAlignment = "text-align";
    public const string Left = "left";
    public const string Right = "right";
    public const string Center = "center";
    public const string MarginLeft = "margin-left";
    public const string MarginTop = "margin-top";
    public const string Position = "position";
    public const string Absolute = "absolute";
    public const string Source = "src";
    public const string Alt = "alt";
    public const string TopBorder = "border-top";
    public const string BottomBorder = "border-bottom";
    public const string LeftBorder = "border-left";
    public const string RightBorder = "border-right";
    public const string TopBorderColor = "border-top-color";
    public const string BottomBorderColor = "border-bottom-color";
    public const string LeftBorderColor = "border-left-color";
    public const string RightBorderColor = "border-right-color";
    public const string Rgb = "rgb";
    public const string Solid = "solid";
    public const string Double = "double";
    public const string RowSpan = "ROWSPAN";
    public const string ColumnSpan = "COLSPAN";
    public const string FontWeight = "font-weight";
    public const string TopBorderWidth = "border-top-width";
    public const string BottomBorderWidth = "border-bottom-width";
    public const string LeftBorderWidth = "border-left-width";
    public const string RightBorderWidth = "border-right-width";
    public const string Dashed = "dashed";
    public const string Dotted = "dotted";
    public const string FontFamily = "font-family";
    public const string UnderLine = "underline";
    public const string TextDecoration = "text-decoration";
    public const string VerticalAlign = "vertical-align";
    public const string Top = "top";
    public const string Bottom = "bottom";
    public const string Distriburted = "distributed";
    internal const string PaddingLeft = "padding-left";
    internal const string PaddingRight = "padding-right";
    public const string Justify = "justify";
    public const string General = "general";
    internal const string IndentLevel = "mso-char-indent-count";
    public const string Id = "id";
    public const string Type = "type";
    public const string Value = "value";
    public const string Height = "height";
    public const string Width = "width";
    public const string Scrollbar = "scrollbar";
    public const string Rows = "rows";
    public const string FrameBorder = "frameborder";
    public const string Name = "name";
    public const string Scrolling = "scrolling";
    public const string None = "none";
    public const string Alink = "alink";
    public const string Vlink = "vlink";
    public const string Link = "link";
    public const string Target = "target";
    public const string Href = "href";
    public const string Ahover = "a:hover";
    public const string BorderCollapse = "border-collapse";
    public const string BorderSpacing = "border-spacing";
    public const string EmptyCells = "empty-cells";
    public const string CellSpacing = "cellspacing";
    public const string Style = "style";
    public const string OnClick = "onclick";
    public const string Face = "face";
    public const string Color = "color";
    public const string NoWarp = "nowrap";
    public const string Parent = "_parent";
    public const string TableLayout = "table-layout";
    public const string WhiteSpace = "white-space";
    public const string Rel = "rel";
    public const string Collapse = "collapse";
    public const string Strong = "strong";
    public const string Oblique = "oblique";
    public const string Strike = "strike";
    public const string Normal = "normal";
    internal const string Smallar = "smallar";
    internal const string Larger = "larger";
    internal const string Medium = "medium";
    internal const string XXSmall = "xx-small";
    internal const string XSmall = "x-small";
    internal const string Large = "large";
    internal const string XXlarge = "xx-large";
    internal const string Xlarge = "x-large";
    internal const string Small = "small";
    public const string I = "i";
    public const string B = "b";
    public const string U = "u";
    public const string LineThrough = "line-through";
    internal const string Background = "background";
    internal const string Direction = "dir";
    internal const string Display = "display";
  }

  private class Operators
  {
    public const string OpenCurlyBrace = "{";
    public const string CloseCurlyBrace = "}";
    public const string Comma = ",";
    public const string Dot = ".";
    public const string Colon = ":";
    public const string SemiColon = ";";
    public const string OpenBrace = "(";
    public const string CloseBrace = ")";
  }
}
