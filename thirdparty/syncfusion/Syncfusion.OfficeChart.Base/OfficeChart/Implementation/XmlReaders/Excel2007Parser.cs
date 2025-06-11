// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlReaders.Excel2007Parser
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Compression;
using Syncfusion.Compression.Zip;
using Syncfusion.Drawing;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Exceptions;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlReaders.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Charts;
using Syncfusion.OfficeChart.Implementation.XmlSerialization.Constants;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlReaders;

internal class Excel2007Parser
{
  internal const byte HLSMax = 255 /*0xFF*/;
  private const byte RGBMax = 255 /*0xFF*/;
  private const double Undefined = 170.0;
  public const int AdditionalProgressItems = 4;
  private const string CarriageReturn = "_x000d_";
  private const string LineFeed = "_x000a_";
  private const string NullChar = "_x0000_";
  private const string BackSpace = "_x0008_";
  private const string Tab = "_x0009_";
  private const string ContentTypeSchema = "contentTypeSchema";
  private const string ContentTypeNameSpace = "http://schemas.microsoft.com/office/2006/metadata/contentType";
  private const string XmlSchemaNameSpace = "http://www.w3.org/2001/XMLSchema";
  private const string ElementName = "element";
  private const string Name = "name";
  private const string DisplayName = "ma:displayName";
  private const string InternalName = "ma:internalName";
  private const string Reference = "ref";
  private const string ComplexContent = "complexContent";
  private WorkbookImpl m_book;
  private FormulaUtil m_formulaUtil;
  private Dictionary<int, ShapeParser> m_dictShapeParsers = new Dictionary<int, ShapeParser>();
  private List<Color> m_lstThemeColors;
  internal Dictionary<string, Color> m_dicThemeColors;
  private Dictionary<string, FontImpl> m_dicMajorFonts;
  private Dictionary<string, FontImpl> m_dicMinorFonts;
  private List<string> m_values = new List<string>();
  private string parentElement = string.Empty;
  private bool m_enableAlternateContent;
  private WorksheetImpl m_workSheet;
  private DrawingParser m_drawingParser;
  private Dictionary<int, List<Point>> m_outlineLevels = new Dictionary<int, List<Point>>();
  private Dictionary<int, int> m_indexAndLevels = new Dictionary<int, int>();
  private int dpiX;
  private int dpiY;
  private int minRow;
  private int maxRow;
  private int minColumn;
  private int maxColumn;
  private readonly int[] DEF_NUMBERFORMAT_INDEXES = new int[8]
  {
    5,
    6,
    7,
    8,
    41,
    42,
    43,
    44
  };
  internal static bool m_isPresentation;

  public FormulaUtil FormulaUtil => this.m_formulaUtil;

  internal WorksheetImpl Worksheet => this.m_workSheet;

  public Excel2007Parser(WorkbookImpl book)
  {
    this.m_book = book != null ? book : throw new ArgumentNullException(nameof (book));
    this.m_formulaUtil = new FormulaUtil(this.m_book.Application, (object) this.m_book, NumberFormatInfo.InvariantInfo, ',', ';');
    this.dpiX = book.AppImplementation.GetdpiX();
    this.dpiY = book.AppImplementation.GetdpiY();
  }

  public Color GetThemeColor(string colorName)
  {
    return Excel2007Parser.GetThemeColor(colorName, this.m_dicThemeColors);
  }

  public static Color GetThemeColor(string colorName, Dictionary<string, Color> themeColors)
  {
    if (colorName == null || colorName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (colorName));
    Color themeColor = ColorExtension.Empty;
    bool flag = themeColors.TryGetValue(colorName, out themeColor);
    if (!flag && colorName == "bg1")
    {
      colorName = "lt1";
      themeColor = Excel2007Parser.GetThemeColor(colorName, themeColors);
    }
    else if (!flag && colorName == "bg2")
    {
      colorName = "lt2";
      themeColor = Excel2007Parser.GetThemeColor(colorName, themeColors);
    }
    return themeColor;
  }

  public void ParseContentTypes(
    XmlReader reader,
    IDictionary<string, string> contentDefaults,
    IDictionary<string, string> contentOverrides)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (contentDefaults == null)
      throw new ArgumentNullException(nameof (contentDefaults));
    if (contentOverrides == null)
      throw new ArgumentNullException(nameof (contentOverrides));
    while (reader.NodeType != XmlNodeType.Element && !reader.EOF)
      reader.Read();
    if (reader.EOF)
      throw new XmlException("Cannot locate appropriate xml tag");
    if (!(reader.LocalName == "Types") || !(reader.NamespaceURI == "http://schemas.openxmlformats.org/package/2006/content-types"))
      throw new XmlException("Cannot locate appropriate xml tag");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "Default":
            this.ParseDictionaryEntry(reader, contentDefaults, "Extension", "ContentType");
            break;
          case "Override":
            this.ParseDictionaryEntry(reader, contentOverrides, "PartName", "ContentType");
            break;
          default:
            throw new NotImplementedException(reader.LocalName);
        }
        reader.Skip();
      }
      else
        reader.Read();
    }
  }

  public void ParseWorkbook(
    XmlReader reader,
    RelationCollection relations,
    FileDataHolder holder,
    string bookPath,
    Stream streamStart,
    Stream streamEnd,
    ref List<Dictionary<string, string>> lstBookViews,
    Stream functionGroups)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (streamStart == null)
      throw new ArgumentNullException(nameof (streamStart));
    if (streamEnd == null)
      throw new ArgumentNullException(nameof (streamEnd));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (!(reader.LocalName == "workbook"))
      throw new XmlException("Unexpected xml tag: " + reader.LocalName);
    bool bAdd = false;
    StreamWriter textWriter = new StreamWriter(streamStart);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) textWriter);
    writer.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    reader.Read();
    int iActiveSheetIndex = 0;
    int iDisplayedTab = 0;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "definedNames":
            this.m_book.ActiveSheetIndex = iActiveSheetIndex;
            this.ParseNamedRanges(reader);
            this.SwitchStreams(ref bAdd, ref writer, ref textWriter, streamEnd);
            continue;
          case "sheets":
            this.ParseSheetsOptions(reader, relations, holder, bookPath);
            this.SwitchStreams(ref bAdd, ref writer, ref textWriter, streamEnd);
            continue;
          case "bookViews":
            lstBookViews = this.ParseBookViews(reader, out iActiveSheetIndex, out iDisplayedTab);
            this.SwitchStreams(ref bAdd, ref writer, ref textWriter, streamEnd);
            continue;
          case "calcPr":
            this.ParseCalcProperties(reader);
            continue;
          case "externalReferences":
            this.ParseExternalLinksWorkbookPart(reader);
            continue;
          case "workbookProtection":
            this.ParseWorkbookProtection(reader);
            continue;
          case "fileVersion":
            this.ParseFileVersion(reader, this.m_book.DataHolder.FileVersion);
            continue;
          case nameof (functionGroups):
            UtilityMethods.CreateWriter(functionGroups, Encoding.UTF8).WriteNode(reader, false);
            continue;
          case "workbookPr":
            this.ParseWorkbookPr(reader);
            continue;
          case "pivotCaches":
            this.ParsePivotCaches(reader);
            continue;
          default:
            writer.WriteNode(reader, false);
            continue;
        }
      }
      else
        reader.Read();
    }
    this.m_book.ActiveSheetIndex = iActiveSheetIndex;
    this.m_book.DisplayedTab = iDisplayedTab;
    writer.WriteEndElement();
    writer.Flush();
  }

  private static void ParseDocumentManagmentSchema(XmlReader reader, ref List<string> m_values)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "element")
      {
        string attribute = reader.GetAttribute("ref");
        if (attribute != null && attribute.Length > 0)
          m_values.Add(attribute.Split(':')[1]);
      }
      reader.Read();
    }
  }

  private void ParseChildElements(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    List<Stream> streamList = new List<Stream>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Whitespace)
        reader.Read();
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "element" && reader.GetAttribute("name") != null)
        streamList.Add(ShapeParser.ReadNodeAsStream(reader));
      reader.Read();
    }
    if (this.m_book.m_childElements.ContainsKey(this.parentElement) || streamList.Count <= 0)
      return;
    this.m_book.m_childElements.Add(this.parentElement, streamList);
  }

  public void ParsePivotTables()
  {
    foreach (WorksheetImpl worksheet in (CollectionBase<IWorksheet>) (this.m_book.Worksheets as WorksheetsCollection))
      worksheet.DataHolder.ParsePivotTable((IWorksheet) worksheet);
  }

  public void ParseWorksheets(Dictionary<int, int> dictUpdatedSSTIndexes, bool parseOnDemand)
  {
    ITabSheets objects = (ITabSheets) this.m_book.Objects;
    ApplicationImpl appImplementation = this.m_book.AppImplementation;
    int fullSize = objects.Count + 4;
    appImplementation.RaiseProgressEvent(4L, (long) fullSize);
    appImplementation.IsFormulaParsed = false;
    int index = 0;
    for (int count = objects.Count; index < count; ++index)
    {
      WorksheetBaseImpl worksheetBaseImpl = (WorksheetBaseImpl) objects[index];
      switch (worksheetBaseImpl)
      {
        case WorksheetImpl _:
          (worksheetBaseImpl as WorksheetImpl).ParseDataOnDemand = parseOnDemand;
          break;
        case ChartImpl _:
          continue;
      }
      worksheetBaseImpl.ParseData(dictUpdatedSSTIndexes);
      worksheetBaseImpl.IsSaved = false;
      appImplementation.RaiseProgressEvent((long) (index + 4 + 1), (long) fullSize);
    }
    appImplementation.IsFormulaParsed = true;
  }

  private void ParsePivotCaches(XmlReader reader)
  {
    if (reader.LocalName != "pivotCaches")
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
            case "pivotCache":
              this.ParsePivotCache(reader);
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

  private void ParsePivotCache(XmlReader reader)
  {
    if (reader.LocalName != "pivotCache")
      throw new XmlException();
    string cacheId = (string) null;
    string relationId = (string) null;
    if (reader.MoveToAttribute("cacheId"))
      cacheId = reader.Value;
    if (reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      relationId = reader.Value;
    this.m_book.DataHolder.RegisterCache(cacheId, relationId);
    reader.MoveToElement();
    reader.Skip();
  }

  private void ParseFileVersion(XmlReader reader, FileVersion fileVersion)
  {
    if (reader.LocalName != nameof (fileVersion))
      throw new XmlException();
    fileVersion.ApplicationName = reader.MoveToAttribute("appName") ? (fileVersion.ApplicationName = reader.Value) : (string) null;
    fileVersion.BuildVersion = reader.MoveToAttribute("rupBuild") ? reader.Value : (string) null;
    fileVersion.LowestEdited = reader.MoveToAttribute("lowestEdited") ? reader.Value : (string) null;
    fileVersion.LastEdited = reader.MoveToAttribute("lastEdited") ? reader.Value : (string) null;
    fileVersion.CodeName = reader.MoveToAttribute("codeName") ? reader.Value : (string) null;
    reader.MoveToElement();
    reader.Skip();
  }

  private void ParseWorkbookPr(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    if (reader.LocalName != "workbookPr")
      throw new XmlException();
    if (reader.MoveToAttribute("date1904"))
      this.m_book.Date1904 = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("codeName"))
      this.m_book.CodeName = reader.Value;
    if (reader.MoveToAttribute("hidePivotFieldList"))
      this.m_book.HidePivotFieldList = !XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("defaultThemeVersion"))
      this.m_book.DefaultThemeVersion = reader.Value;
    reader.Skip();
  }

  private void ParseCalcProperties(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    if (reader.LocalName != "calcPr")
      throw new XmlException();
    if (reader.MoveToAttribute("fullPrecision"))
      this.m_book.PrecisionAsDisplayed = !XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("calcId"))
      this.m_book.DataHolder.CalculationId = reader.Value;
    reader.Skip();
  }

  private void ParseWorkbookProtection(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "workbookProtection")
      throw new XmlException();
    bool bIsProtectContent = false;
    bool bIsProtectWindow = false;
    ushort num = 0;
    if (reader.MoveToAttribute("workbookPassword"))
      num = ushort.Parse(reader.Value, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.CurrentCulture);
    if (reader.MoveToAttribute("lockStructure"))
      bIsProtectContent = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("lockWindows"))
      bIsProtectWindow = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("workbookAlgorithmName"))
      this.m_book.AlgorithmName = reader.Value;
    if (reader.MoveToAttribute("workbookSaltValue"))
      this.m_book.SaltValue = Convert.FromBase64String(reader.Value);
    if (reader.MoveToAttribute("workbookHashValue"))
      this.m_book.HashValue = Convert.FromBase64String(reader.Value);
    if (reader.MoveToAttribute("workbookSpinCount"))
      this.m_book.SpinCount = Convert.ToUInt32(reader.Value);
    reader.Read();
    if (bIsProtectContent || bIsProtectWindow)
      this.m_book.Protect(bIsProtectWindow, bIsProtectContent);
    this.m_book.Password.IsPassword = num;
  }

  private List<Dictionary<string, string>> ParseBookViews(
    XmlReader reader,
    out int iActiveSheetIndex,
    out int iDisplayedTab)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    iActiveSheetIndex = 0;
    iDisplayedTab = 0;
    List<Dictionary<string, string>> bookViews = new List<Dictionary<string, string>>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "workbookView")
      {
        Dictionary<string, string> workbookView = this.ParseWorkbookView(reader);
        bookViews.Add(workbookView);
      }
      reader.Skip();
    }
    Dictionary<string, string> dictionary = bookViews[0];
    string s;
    if (dictionary.TryGetValue("activeTab", out s))
      iActiveSheetIndex = XmlConvertExtension.ToInt32(s);
    if (dictionary.TryGetValue("firstSheet", out s))
      iDisplayedTab = XmlConvertExtension.ToInt32(s);
    reader.Skip();
    return bookViews;
  }

  private Dictionary<string, string> ParseWorkbookView(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    Dictionary<string, string> workbookView = new Dictionary<string, string>();
    int i = 0;
    for (int attributeCount = reader.AttributeCount; i < attributeCount; ++i)
    {
      reader.MoveToAttribute(i);
      workbookView.Add(reader.Name, reader.Value);
    }
    return workbookView;
  }

  public void ParseSheet(
    XmlReader reader,
    WorksheetImpl sheet,
    string strParentPath,
    ref MemoryStream streamStart,
    ref MemoryStream streamCF,
    List<int> arrStyles,
    Dictionary<string, object> dictItemsToRemove,
    Dictionary<int, int> dictUpdatedSSTIndexes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (streamStart == null)
      throw new ArgumentNullException(nameof (streamStart));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "worksheet")
      throw new XmlException("worksheet tag was not found.");
    reader.Read();
    this.m_workSheet = sheet;
    this.ParseSheetBeforeData(reader, sheet, (Stream) streamStart, arrStyles);
    if (reader.LocalName == "sheetData")
      this.ParseSheetData(reader, (IInternalWorksheet) sheet, arrStyles, "c");
    if (dictUpdatedSSTIndexes == null)
      return;
    sheet.UpdateLabelSSTIndexes(dictUpdatedSSTIndexes, new IncreaseIndex(sheet.ParentWorkbook.InnerSST.AddIncrease));
  }

  private void ParseSheetBeforeData(
    XmlReader reader,
    WorksheetImpl sheet,
    Stream streamStart,
    List<int> arrStyles)
  {
    while (!reader.EOF && reader.NodeType != XmlNodeType.EndElement && reader.LocalName != "sheetData")
    {
      switch (reader.LocalName)
      {
        case "cols":
          this.ParseColumns(reader, sheet, arrStyles);
          reader.Read();
          continue;
        case "sheetPr":
          this.ParseSheetLevelProperties(reader, (WorksheetBaseImpl) sheet);
          continue;
        case "sheetViews":
          this.ParseSheetViews(reader, (WorksheetBaseImpl) sheet);
          continue;
        case "dimension":
          reader.Skip();
          continue;
        case "sheetFormatPr":
          this.ExtractDefaultRowHeight(reader, sheet);
          this.ExtractZeroHeight(reader, sheet);
          reader.MoveToElement();
          break;
      }
      reader.Skip();
    }
  }

  private void ParseSheetViews(XmlReader reader, WorksheetBaseImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "sheetViews")
      throw new XmlException("Wrong xml tag");
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        switch (reader.LocalName)
        {
          case "sheetView":
            this.ParseSheetView(reader, sheet);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    reader.Read();
  }

  private void ParseSheetView(XmlReader reader, WorksheetBaseImpl sheetBase)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheetBase == null)
      throw new ArgumentNullException(nameof (sheetBase));
    if (reader.LocalName != "sheetView")
      throw new XmlException("Wrong xml tag");
    WorksheetImpl sheet = sheetBase as WorksheetImpl;
    if (reader.MoveToAttribute("showGridLines"))
      sheet.IsGridLinesVisible = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("topLeftCell"))
      sheet.TopLeftCell = sheet[reader.Value];
    if (reader.MoveToAttribute("showZeros"))
      sheet.IsDisplayZeros = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showRowColHeaders"))
      sheet.IsRowColumnHeadersVisible = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("rightToLeft"))
      sheet.IsRightToLeft = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("zoomScale"))
      sheetBase.Zoom = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("zoomToFit"))
      (sheetBase as ChartImpl).ZoomToFit = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("defaultGridColor"))
      sheet.WindowTwo.IsDefaultHeader = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("colorId"))
      sheet.GridLineColor = (OfficeKnownColors) XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("view"))
    {
      switch (reader.Value)
      {
        case "pageLayout":
          sheet.View = OfficeSheetView.PageLayout;
          break;
        case "pageBreakPreview":
          sheet.View = OfficeSheetView.PageBreakPreview;
          sheet.WindowTwo.IsSavedInPageBreakPreview = true;
          break;
        case "normal":
          sheet.View = OfficeSheetView.Normal;
          break;
      }
    }
    if (reader.MoveToAttribute("tabSelected") && reader.Value != "0")
      sheetBase.Select();
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        switch (reader.LocalName)
        {
          case "pane":
            if (reader.IsEmptyElement && !reader.HasAttributes)
            {
              reader.Read();
              continue;
            }
            this.ParsePane(reader, sheet);
            continue;
          case "selection":
            this.ParseSelection(reader, sheet);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    reader.Read();
  }

  private void ParseSelection(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "selection")
      throw new XmlException();
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.MoveToAttribute("pane"))
      sheet.Pane.ActivePane = (ushort) this.GetPaneType(reader.Value);
    if (reader.MoveToAttribute("activeCell"))
    {
      string name = reader.Value;
      sheet.SetActiveCell(sheet.Range[name], false);
    }
    reader.MoveToElement();
    reader.Skip();
  }

  private Pane.ActivePane GetPaneType(string value) => Pane.PaneStrings[value];

  private void ParsePane(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "pane")
      throw new XmlException("Wrong xml tag");
    if (reader.MoveToAttribute("xSplit"))
      sheet.VerticalSplit = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("ySplit"))
      sheet.HorizontalSplit = (int) XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute("topLeftCell"))
    {
      string name = reader.Value;
      sheet.PaneFirstVisible = sheet[name];
    }
    if (reader.MoveToAttribute("activePane"))
    {
      Pane.ActivePane activePane = (Pane.ActivePane) Enum.Parse(typeof (Pane.ActivePane), reader.Value, false);
      sheet.ActivePane = (int) activePane;
    }
    if (!reader.MoveToAttribute("state"))
      return;
    this.ParsePaneState(sheet.WindowTwo, reader.Value);
  }

  private void ParsePaneState(WindowTwoRecord windowTwo, string state)
  {
    if (windowTwo == null)
      throw new ArgumentNullException(nameof (windowTwo));
    if (state == null)
      throw new ArgumentNullException(nameof (state));
    switch (state)
    {
      case "frozen":
        windowTwo.IsFreezePanes = true;
        windowTwo.IsFreezePanesNoSplit = true;
        break;
      case "frozenSplit":
        windowTwo.IsFreezePanes = true;
        windowTwo.IsFreezePanesNoSplit = false;
        break;
      case "split":
        windowTwo.IsFreezePanes = false;
        windowTwo.IsFreezePanesNoSplit = false;
        break;
      default:
        throw new XmlException();
    }
  }

  public void ParseChartsheet(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "chartsheet")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    PageSetupBaseImpl pageSetupBase = chart.PageSetupBase;
    pageSetupBase.IsNotValidSettings = true;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "sheetPr":
            this.ParseSheetLevelProperties(reader, (WorksheetBaseImpl) chart);
            continue;
          case "sheetViews":
            this.ParseSheetViews(reader, (WorksheetBaseImpl) chart);
            continue;
          case "pageMargins":
            bool notValidSettings1 = pageSetupBase.IsNotValidSettings;
            Excel2007Parser.ParsePageMargins(reader, (IPageSetupBase) chart.PageSetup, (IPageSetupConstantsProvider) new WorksheetPageSetupConstants());
            pageSetupBase.IsNotValidSettings = notValidSettings1;
            continue;
          case "pageSetup":
            Excel2007Parser.ParsePageSetup(reader, chart.PageSetupBase);
            continue;
          case "headerFooter":
            bool notValidSettings2 = pageSetupBase.IsNotValidSettings;
            Excel2007Parser.ParseHeaderFooter(reader, chart.PageSetupBase);
            pageSetupBase.IsNotValidSettings = notValidSettings2;
            continue;
          case "drawing":
            this.ParseChartDrawing(reader, chart);
            continue;
          case "legacyDrawing":
            this.ParseLegacyDrawing(reader, (WorksheetBaseImpl) chart);
            continue;
          case "legacyDrawingHF":
            Excel2007Parser.ParseLegacyDrawingHF(reader, (WorksheetBaseImpl) chart, (RelationCollection) null);
            continue;
          case "sheetProtection":
            this.ParseSheetProtection(reader, (WorksheetBaseImpl) chart, "content");
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

  private void ParseChartDrawing(XmlReader reader, ChartImpl chart)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    if (reader.LocalName != "drawing")
      throw new XmlException("Unexpected xml tag.");
    if (!reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      throw new XmlException();
    bool isChartEx = false;
    string id = reader.Value;
    Relation relation = chart.m_dataHolder.Relations[id];
    chart.m_dataHolder.Relations.Remove(id);
    string str = relation != null ? relation.Target : throw new XmlException();
    FileDataHolder parentHolder = chart.m_dataHolder.ParentHolder;
    string strItemPath = chart.m_dataHolder.ArchiveItem.ItemName;
    string path;
    FileDataHolder.SeparateItemName(strItemPath, out path);
    XmlReader reader1 = parentHolder.CreateReader(relation, path, out strItemPath);
    string correspondingRelations = FileDataHolder.GetCorrespondingRelations(strItemPath);
    RelationCollection relations = parentHolder.ParseRelations(correspondingRelations);
    while (reader1.LocalName != nameof (chart))
    {
      if (reader1.NodeType == XmlNodeType.Element && reader1.LocalName == "absoluteAnchor")
      {
        Size absoluteAnchorExtent = this.ParseAbsoluteAnchorExtent(reader1);
        chart.Width = (double) absoluteAnchorExtent.Width;
        chart.Height = (double) absoluteAnchorExtent.Height;
      }
      else if (reader1.NodeType == XmlNodeType.Element && reader1.LocalName == "AlternateContent")
      {
        reader1.Read();
        Excel2007Parser.SkipWhiteSpaces(reader1);
        if (reader1.LocalName == "Choice" && this.IsChartExChoice(reader1))
          isChartEx = true;
      }
      else
        reader1.Read();
    }
    this.ParseChartTag(reader1, chart, relations, parentHolder, strItemPath, isChartEx);
    if (!isChartEx)
      return;
    this.TryRemoveChartSheetFallBackRelations(reader1, chart, strItemPath, parentHolder, relations);
  }

  private void TryRemoveChartSheetFallBackRelations(
    XmlReader reader,
    ChartImpl chart,
    string drawingItemName,
    FileDataHolder holder,
    RelationCollection relations)
  {
    bool flag1 = false;
    bool flag2 = false;
    while (reader.NodeType != XmlNodeType.None && !flag2)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "Fallback":
            flag1 = true;
            reader.Read();
            continue;
          case "graphicData":
            if (flag1)
            {
              reader.Read();
              Excel2007Parser.SkipWhiteSpaces(reader);
              if (reader.LocalName == nameof (chart))
              {
                string id = reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships") ? reader.Value : throw new XmlException();
                Relation relation = relations[id];
                string path;
                FileDataHolder.SeparateItemName(drawingItemName, out path);
                string strItemPath1;
                holder.GetItem(relation, path, out strItemPath1);
                string correspondingRelations1 = FileDataHolder.GetCorrespondingRelations(strItemPath1);
                RelationCollection relations1 = holder.ParseRelations(correspondingRelations1);
                ZipArchive archive = holder.Archive;
                foreach (KeyValuePair<string, Relation> keyValuePair in relations1)
                {
                  string strItemPath2;
                  holder.GetItem(keyValuePair.Value, path, out strItemPath2);
                  string correspondingRelations2 = FileDataHolder.GetCorrespondingRelations(strItemPath2);
                  archive.RemoveItem(strItemPath2);
                  archive.RemoveItem(correspondingRelations2);
                }
                relations1.Clear();
                archive.RemoveItem(strItemPath1);
                archive.RemoveItem(correspondingRelations1);
                relations.Remove(id);
              }
              flag2 = true;
              continue;
            }
            reader.Read();
            continue;
          default:
            reader.Read();
            continue;
        }
      }
      else
        reader.Read();
    }
  }

  private bool IsChartExChoice(XmlReader reader)
  {
    bool flag = false;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "Choice")
      throw new XmlException("Unexpected xml tag");
    if (reader.MoveToAttribute("Requires"))
    {
      string str = reader.Value;
      if (reader.MoveToAttribute("xmlns:" + str) && reader.Value.ToLower().Contains("chartex"))
        flag = true;
    }
    return flag;
  }

  private Size ParseAbsoluteAnchorExtent(XmlReader reader)
  {
    while (reader.LocalName != "ext")
      reader.Read();
    return this.ParseExtent(reader);
  }

  private void ParseChartTag(
    XmlReader reader,
    ChartImpl chart,
    RelationCollection relations,
    FileDataHolder dataHolder,
    string itemName,
    bool isChartEx)
  {
    string id = reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships") ? reader.Value : throw new XmlException();
    Relation relation1 = relations[id];
    string path;
    FileDataHolder.SeparateItemName(itemName, out path);
    Relation relation2 = (Relation) null;
    if (relation1 == null)
      return;
    string strItemPath;
    XmlReader reader1 = dataHolder.CreateReader(relation1, path, out strItemPath);
    string correspondingRelations = FileDataHolder.GetCorrespondingRelations(strItemPath);
    RelationCollection relations1 = dataHolder.ParseRelations(correspondingRelations);
    if (relations1 != null)
    {
      relations1.RemoveByContentType("http://schemas.microsoft.com/office/2011/relationships/chartColorStyle");
      relations1.RemoveByContentType("http://schemas.microsoft.com/office/2011/relationships/chartStyle");
      foreach (KeyValuePair<string, Relation> keyValuePair in relations1)
      {
        chart.Relations[keyValuePair.Key] = keyValuePair.Value;
        if (keyValuePair.Value.Type == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/themeOverride")
          relation2 = keyValuePair.Value;
      }
      chart.Relations.ItemPath = relations1.ItemPath;
    }
    if (relation2 != null)
    {
      reader1 = dataHolder.CreateReader(relation2, "xl/workbook.xml", out string _);
      while (reader1.NodeType != XmlNodeType.Element)
        reader1.Read();
      if (reader1.LocalName == "themeOverride")
      {
        reader1.Read();
        while (reader1.NodeType != XmlNodeType.EndElement)
        {
          if (reader1.NodeType == XmlNodeType.Element && reader1.LocalName == "clrScheme")
          {
            MemoryStream data = new MemoryStream();
            XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
            writer.WriteNode(reader1, false);
            writer.Flush();
            chart.m_themeOverrideStream = data;
          }
          else
            reader1.Skip();
        }
      }
    }
    if (isChartEx)
      new ChartExParser(this.m_book).ParseChartEx(reader1, chart, relations1);
    else
      new ChartParser(this.m_book).ParseChart(reader1, chart, relations1);
    dataHolder.Archive.RemoveItem(itemName);
    relations.Remove(id);
  }

  private void ExtractDefaultRowHeight(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.MoveToAttribute("defaultColWidth"))
    {
      double num1 = XmlConvertExtension.ToDouble(reader.Value);
      sheet.DefaultColumnWidth = num1;
      int fileWidth = (int) Math.Round(num1 * 256.0);
      double num2 = (double) sheet.EvaluateRealColumnWidth(fileWidth) / 256.0;
      sheet.StandardWidth = num2 > 0.0 ? num2 : sheet.StandardWidth;
      this.SetDefaultColumnWidth(Helper.ParseDouble(reader.Value), false, sheet);
    }
    if (reader.MoveToAttribute("defaultRowHeight"))
      sheet.StandardHeight = XmlConvertExtension.ToDouble(reader.Value);
    sheet.CustomHeight = false;
    if (reader.MoveToAttribute("customHeight"))
      sheet.CustomHeight = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("outlineLevelCol"))
      sheet.OutlineLevelColumn = XmlConvert.ToByte(reader.Value);
    if (reader.MoveToAttribute("outlineLevelRow"))
      sheet.OutlineLevelRow = XmlConvertExtension.ToByte(reader.Value);
    if (reader.MoveToAttribute("baseColWidth"))
      sheet.BaseColumnWidth = (int) XmlConvertExtension.ToInt16(reader.Value);
    if (reader.MoveToAttribute("thickBottom"))
      sheet.IsThickBottom = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("thickTop"))
      sheet.IsThickTop = XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToElement();
  }

  private void SetDefaultColumnWidth(double defaultWidth, bool bool_2, WorksheetImpl worksheet)
  {
    if (Math.Abs(defaultWidth - 0.0) < 0.0001)
    {
      worksheet.Columnss.Width = defaultWidth;
    }
    else
    {
      int fontCalc2 = worksheet.GetAppImpl().GetFontCalc2();
      double num = defaultWidth * (double) fontCalc2;
      if (bool_2)
        num += 10.0;
      if (num > 5.0)
      {
        worksheet.Columnss.Width = (num - 5.0) / (double) fontCalc2;
      }
      else
      {
        worksheet.Columnss.Width = 0.0;
        ColumnCollection columnss = worksheet.Columnss;
        if (columnss.column != null)
          return;
        columnss.GetOrCreateColumn().SetWidth((int) (num + 0.5));
      }
    }
  }

  private void ExtractZeroHeight(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.MoveToAttribute("zeroHeight"))
    {
      sheet.IsZeroHeight = XmlConvertExtension.ToBoolean(reader.Value);
      (sheet.PageSetup as PageSetupImpl).DefaultRowHeightFlag = false;
    }
    reader.MoveToElement();
  }

  public void ParseMergedCells(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.NodeType != XmlNodeType.Element || !(reader.LocalName == "mergeCells"))
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
        this.ParseMergeRegion(reader, sheet);
      else
        reader.Read();
    }
    reader.Read();
  }

  public void ParseNamedRanges(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType != XmlNodeType.Element || !(reader.LocalName == "definedNames"))
      return;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      List<string> stringList = new List<string>();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          string namedRange = this.ParseNamedRange(reader);
          stringList.Add(namedRange);
        }
        reader.Read();
      }
      INames names = this.m_book.Names;
      this.m_book.AppImplementation.IsFormulaParsed = false;
      int index = 0;
      for (int count = stringList.Count; index < count; ++index)
      {
        NameImpl nameImpl = (NameImpl) names[index];
        if (stringList[index].LastIndexOf('!') == 0)
        {
          nameImpl.IsCommon = true;
          stringList[index] = !(names[index].Scope == "Workbook") || this.m_book.ActiveSheet == null ? names[index].Scope + stringList[index] : this.m_book.ActiveSheet.Name + stringList[index];
        }
        nameImpl.SetValue(this.m_formulaUtil.ParseString(stringList[index]));
      }
    }
    this.m_book.AppImplementation.IsFormulaParsed = true;
    reader.Read();
  }

  public List<int> ParseStyles(XmlReader reader, ref Stream streamDxfs)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "styleSheet")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    List<int> intList1 = (List<int>) null;
    List<BordersCollection> arrBorders = (List<BordersCollection>) null;
    List<FillImpl> arrFills = (List<FillImpl>) null;
    List<int> intList2 = (List<int>) null;
    List<int> styles = (List<int>) null;
    Dictionary<int, int> arrNumberFormatIndexes = (Dictionary<int, int>) null;
    reader.Read();
    if (reader.NodeType == XmlNodeType.None)
    {
      this.m_book.InsertDefaultFonts();
      this.m_book.InsertDefaultValues();
      return (List<int>) null;
    }
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      bool flag = true;
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "numFmts":
            arrNumberFormatIndexes = this.ParseNumberFormats(reader);
            break;
          case "fonts":
            intList1 = this.ParseFonts(reader);
            break;
          case "fills":
            arrFills = this.ParseFills(reader);
            break;
          case "borders":
            arrBorders = this.ParseBorders(reader);
            break;
          case "cellStyleXfs":
            intList2 = this.ParseNamedStyles(reader, intList1, arrFills, arrBorders, arrNumberFormatIndexes);
            break;
          case "cellXfs":
            styles = this.ParseCellFormats(reader, intList1, arrFills, arrBorders, intList2, arrNumberFormatIndexes);
            break;
          case "cellStyles":
            this.ParseStyles(reader, intList2);
            break;
          case "dxfs":
            streamDxfs = (Stream) new MemoryStream();
            XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter(streamDxfs));
            writer.WriteNode(reader, false);
            writer.Flush();
            flag = false;
            break;
          case "tableStyles":
            this.m_book.CustomTableStylesStream = ShapeParser.ReadNodeAsStream(reader);
            flag = false;
            break;
          case "colors":
            this.ParseColors(reader);
            break;
          case "extLst":
            this.ParseBookExtensions(reader);
            flag = false;
            break;
          default:
            throw new NotImplementedException(reader.LocalName);
        }
      }
      if (flag)
        reader.Read();
    }
    if (this.m_book.InnerStyles.Count == 0)
      this.m_book.PrepareStyles(false, new List<StyleRecord>(), (Dictionary<int, int>) null);
    this.m_book.DefaultXFIndex = this.m_book.InnerExtFormats.Add(((ExtendedFormatWrapper) this.m_book.InnerStyles["Normal"]).Wrapped.CreateChildFormat()).Index;
    return styles;
  }

  private void ParseBookExtensions(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    this.m_book.DataHolder.ExtensionStream = !(reader.LocalName != "extLst") ? ShapeParser.ReadNodeAsStream(reader, true) : throw new XmlException();
  }

  public Dictionary<int, int> ParseSST(XmlReader reader, bool parseOnDemand)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "sst")
      throw new XmlException(nameof (reader));
    if (reader.IsEmptyElement)
      return (Dictionary<int, int>) null;
    this.m_book.SSTStream = ShapeParser.ReadNodeAsStream(reader);
    if (parseOnDemand)
      this.m_book.ParseOnDemand = parseOnDemand;
    this.m_book.SSTStream.Position = 0L;
    XmlReader reader1 = UtilityMethods.CreateReader(this.m_book.SSTStream);
    reader1.Read();
    int key = 0;
    Dictionary<int, int> sst = new Dictionary<int, int>();
    SSTDictionary innerSst = this.m_book.InnerSST;
    while (reader1.NodeType != XmlNodeType.EndElement && reader1.NodeType != XmlNodeType.None)
    {
      if (reader1.LocalName == "si")
      {
        int stringItem = this.ParseStringItem(reader1);
        if (key != stringItem)
          sst[key] = stringItem;
        ++key;
      }
      reader1.Skip();
    }
    innerSst.UpdateRefCounts();
    return sst;
  }

  public int ParseStringItem(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "si" && reader.LocalName != "is")
      throw new XmlException(nameof (reader));
    bool setCount = reader.LocalName == "is";
    int stringItem = -1;
    reader.Read();
    if (reader.IsEmptyElement)
    {
      stringItem = this.m_book.InnerSST.AddIncrease((object) string.Empty, false);
      reader.Skip();
    }
    else
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        switch (reader.LocalName)
        {
          case "t":
            stringItem = this.ParseText(reader, setCount);
            continue;
          case "r":
            stringItem = this.ParseRichTextRun(reader);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    return stringItem;
  }

  public int ParseStringItem(XmlReader reader, out string text)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "si" && reader.LocalName != "is")
      throw new XmlException(nameof (reader));
    bool setCount = reader.LocalName == "is";
    int stringItem = -1;
    reader.Read();
    text = string.Empty;
    if (reader.IsEmptyElement)
    {
      stringItem = this.m_book.InnerSST.AddIncrease((object) string.Empty, false);
      reader.Skip();
    }
    else
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        switch (reader.LocalName)
        {
          case "t":
            stringItem = this.ParseText(reader, setCount, out text);
            continue;
          case "r":
            stringItem = this.ParseRichTextRun(reader);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    return stringItem;
  }

  public void ParseVmlShapes(
    XmlReader reader,
    ShapeCollectionBase shapes,
    RelationCollection relations,
    string parentItemPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    while (reader.NodeType != XmlNodeType.Element)
    {
      reader.Read();
      if (reader.NodeType == XmlNodeType.None)
        break;
    }
    if (reader.LocalName != "xml")
      throw new XmlException("Unexpected tag");
    reader.Read();
    Dictionary<string, ShapeImpl> dictShapeIdToShape = new Dictionary<string, ShapeImpl>();
    Stream layoutStream = (Stream) null;
    while (reader.NodeType != XmlNodeType.EndElement && !reader.EOF)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "shapetype":
            this.ParseShapeType(reader, shapes, dictShapeIdToShape, layoutStream);
            continue;
          case "shape":
            if (reader.MoveToAttribute("type"))
            {
              this.ParseShape(reader, dictShapeIdToShape, relations, parentItemPath);
              continue;
            }
            this.ParseShapeWithoutType(reader, shapes, relations, parentItemPath);
            continue;
          case "shapelayout":
            layoutStream = ShapeParser.ReadNodeAsStream(reader);
            layoutStream.Position = 0L;
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Read();
    }
  }

  public RelationCollection ParseRelations(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    RelationCollection relations = new RelationCollection();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "Relationships")
      throw new XmlException("Unexpected tag " + reader.LocalName);
    reader.Read();
    if (reader.NodeType != XmlNodeType.None)
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          if (!(reader.LocalName == "Relationship"))
            throw new XmlException("Unexpected tag " + reader.Value);
          Excel2007Parser.ParseRelation(reader, relations);
        }
        reader.Read();
      }
    }
    return relations;
  }

  public Dictionary<string, string> ParseSheetData(
    XmlReader reader,
    IInternalWorksheet sheet,
    List<int> arrStyles,
    string cellTag)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "sheetData")
      throw new XmlException(nameof (reader));
    reader.MoveToElement();
    Dictionary<string, string> sheetData = (Dictionary<string, string>) null;
    if (this.m_outlineLevels == null)
      this.m_outlineLevels = new Dictionary<int, List<Point>>();
    if (sheet is WorksheetImpl && (sheet as WorksheetImpl).RowOutlineLevels == null)
      (sheet as WorksheetImpl).RowOutlineLevels = new Dictionary<int, List<Point>>();
    if (reader.MoveToFirstAttribute())
    {
      sheetData = new Dictionary<string, string>();
      sheetData.Add(reader.LocalName, reader.Value);
      while (reader.MoveToNextAttribute())
        sheetData.Add(reader.LocalName, reader.Value);
      reader.MoveToElement();
    }
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      int generatedRowIndex = 1;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "row")
          generatedRowIndex = this.ParseRow(reader, sheet, arrStyles, cellTag, generatedRowIndex) + 1;
        reader.Skip();
      }
    }
    reader.Read();
    if (sheet is WorksheetImpl && this.m_indexAndLevels != null && this.m_indexAndLevels.Count > 0)
    {
      this.m_indexAndLevels = (Dictionary<int, int>) null;
      this.m_outlineLevels = (Dictionary<int, List<Point>>) null;
    }
    return sheetData;
  }

  public void ParseComments(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    do
    {
      reader.Read();
    }
    while (reader.NodeType != XmlNodeType.Element);
    if (reader.LocalName == "comments")
      reader.Read();
    List<string> arrAuthors = (List<string>) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "authors":
            arrAuthors = this.ParseAuthors(reader);
            continue;
          case "commentList":
            this.ParseCommentList(reader, arrAuthors, sheet);
            continue;
          default:
            throw new XmlException("Unexpected xml tag.");
        }
      }
      else
        reader.Skip();
    }
  }

  public void ParseDrawings(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    string drawingsPath,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (lstRelationIds == null)
      throw new ArgumentNullException("lstRelationdIds");
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "wsDr" && reader.LocalName != "userShapes")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    reader.Read();
    if (reader.NodeType == XmlNodeType.None)
      return;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        string localName = reader.LocalName;
        this.m_drawingParser = new DrawingParser();
        this.m_drawingParser.anchorName = localName;
        switch (localName)
        {
          case "twoCellAnchor":
          case "oneCellAnchor":
          case "relSizeAnchor":
            this.ParseTwoCellAnchor(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
            continue;
          case "AlternateContent":
            this.ParseAlternateContent(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
            continue;
          case "absoluteAnchor":
            double emuValue1 = 0.0;
            double emuValue2 = 0.0;
            double emuValue3 = 0.0;
            double emuValue4 = 0.0;
            while (reader.LocalName != "graphicData")
            {
              if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "absoluteAnchor")
              {
                while (reader.LocalName != "ext")
                {
                  if (reader.LocalName == "pos")
                  {
                    if (reader.MoveToAttribute("x"))
                      emuValue1 = (double) reader.ReadContentAsInt();
                    if (reader.MoveToAttribute("y"))
                      emuValue2 = (double) reader.ReadContentAsInt();
                    this.m_drawingParser.posX = Helper.ConvertEmuToOffset((int) emuValue1, this.dpiX);
                    this.m_drawingParser.posY = Helper.ConvertEmuToOffset((int) emuValue2, this.dpiX);
                  }
                  reader.Read();
                }
                if (reader.MoveToAttribute("cx"))
                  emuValue3 = (double) int.Parse(reader.Value);
                if (reader.MoveToAttribute("cy"))
                  emuValue4 = (double) int.Parse(reader.Value);
                this.m_drawingParser.cx = Helper.ConvertEmuToOffset((int) emuValue3, this.dpiX);
                this.m_drawingParser.cy = Helper.ConvertEmuToOffset((int) emuValue4, this.dpiX);
              }
              else
                reader.Read();
            }
            ShapeImpl chart = this.TryParseChart(this.ReadSingleNodeIntoStream(reader), sheet, drawingsPath, false);
            chart.IsAbsoluteAnchor = true;
            if (chart == null)
            {
              ShapeImpl newShape = new ShapeImpl(sheet.Application, (object) sheet.InnerShapes);
              sheet.InnerShapes.AddShape(newShape);
            }
            else
            {
              (chart as ChartShapeImpl).ChartObject.EMUWidth = emuValue3;
              (chart as ChartShapeImpl).ChartObject.EMUHeight = emuValue4;
              (chart as IOfficeChart).XPos = emuValue1;
              (chart as IOfficeChart).YPos = emuValue2;
            }
            while (reader.LocalName != "absoluteAnchor")
              reader.Read();
            reader.Read();
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

  private void ParseAlternateContent(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    string drawingsPath,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (lstRelationIds == null)
      throw new ArgumentNullException(nameof (lstRelationIds));
    reader.Read();
    this.m_enableAlternateContent = true;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "Choice":
            this.ParseChoice(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseAlternateContent(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    sheet.HasAlternateContent = true;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "Choice":
            this.ParseChoice(reader, sheet);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseChoice(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    string drawingsPath,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (lstRelationIds == null)
      throw new ArgumentNullException(nameof (lstRelationIds));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "twoCellAnchor":
          case "oneCellAnchor":
          case "relSizeAnchor":
            this.ParseTwoCellAnchor(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseChoice(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "controls":
            this.ParseControls(reader, sheet);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  internal static Stream ParseMiscSheetElements(XmlReader reader)
  {
    Stream data = (Stream) new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter(data, Encoding.UTF8);
    writer.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    while (!reader.EOF)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "mergeCells":
          case "legacyDrawing":
          case "legacyDrawingHF":
          case "drawing":
          case "conditionalFormatting":
          case "picture":
          case "dataValidations":
          case "autoFilter":
          case "hyperlinks":
          case "printOptions":
          case "pageMargins":
          case "pageSetup":
          case "headerFooter":
          case "customProperties":
          case "ignoredErrors":
          case "sheetProtection":
          case "AlternateContent":
          case "controls":
          case "extLst":
          case "phoneticPr":
          case "rowBreaks":
          case "colBreaks":
          case "oleObjects":
          case "sortState":
            writer.WriteNode(reader, false);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    writer.WriteEndElement();
    writer.Flush();
    data.Position = 0L;
    return data;
  }

  private void ParseExtensionlist(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
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
              this.ParseExt(sheet, reader);
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

  private void ParseExt(WorksheetImpl sheet, XmlReader reader)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
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
            case "slicerList":
              Stream stream = ShapeParser.ReadNodeAsStream(reader);
              sheet.WorksheetSlicerStream = stream;
              continue;
            case "conditionalFormattings":
              sheet.DataHolder.m_cfsStream = (Stream) new MemoryStream();
              sheet.DataHolder.m_cfsStream = ShapeParser.ReadNodeAsStream(reader);
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

  private void ParseControls(XmlReader reader, WorksheetImpl sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "controls")
      throw new XmlException();
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    MemoryStream data = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
    writer.WriteNode(reader, false);
    writer.Flush();
    dataHolder.ControlsStream = (Stream) data;
  }

  private void ParseSheetProtection(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    string protectContentTag)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "sheetProtection")
      throw new XmlException();
    OfficeSheetProtection officeSheetProtection = OfficeSheetProtection.None;
    ushort password = 0;
    if (reader.MoveToAttribute("password"))
      password = ushort.Parse(reader.Value, NumberStyles.AllowHexSpecifier);
    if (reader.MoveToAttribute("algorithmName"))
      sheet.AlgorithmName = reader.Value;
    if (reader.MoveToAttribute("saltValue"))
      sheet.SaltValue = Convert.FromBase64String(reader.Value);
    if (reader.MoveToAttribute("hashValue"))
      sheet.HashValue = Convert.FromBase64String(reader.Value);
    if (reader.MoveToAttribute("spinCount"))
      sheet.SpinCount = Convert.ToUInt32(reader.Value);
    bool flag = false;
    if (reader.MoveToAttribute(protectContentTag))
    {
      flag = XmlConvertExtension.ToBoolean(reader.Value);
      if (password == (ushort) 0)
        password = (ushort) 1;
    }
    string[] protectionAttributes = Protection.ProtectionAttributes;
    Excel2007Parser.ChecProtectionDelegate protectionDelegate = new Excel2007Parser.ChecProtectionDelegate(this.CheckProtectionAttribute);
    if (sheet is ChartImpl)
    {
      protectionAttributes = Protection.ChartProtectionAttributes;
      protectionDelegate = new Excel2007Parser.ChecProtectionDelegate(this.CheckChartProtectionAttribute);
    }
    int index = 0;
    for (int length = protectionAttributes.Length; index < length; ++index)
      officeSheetProtection = protectionDelegate(reader, protectionAttributes[index], Protection.ProtectionFlags[index], Protection.DefaultValues[index], officeSheetProtection);
    sheet.Protect(password, officeSheetProtection);
    sheet.ProtectContents = flag;
    reader.Read();
  }

  private OfficeSheetProtection CheckChartProtectionAttribute(
    XmlReader reader,
    string attributeName,
    OfficeSheetProtection flag,
    bool defaultValue,
    OfficeSheetProtection protection)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (attributeName == null || attributeName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (attributeName));
    bool flag1 = defaultValue;
    if (reader.MoveToAttribute(attributeName))
      flag1 = XmlConvertExtension.ToBoolean(reader.Value);
    if (flag1)
      protection |= flag;
    else
      protection &= ~flag;
    return protection;
  }

  private OfficeSheetProtection CheckProtectionAttribute(
    XmlReader reader,
    string attributeName,
    OfficeSheetProtection flag,
    bool defaultValue,
    OfficeSheetProtection protection)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (attributeName == null || attributeName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (attributeName));
    bool flag1 = defaultValue;
    if (reader.MoveToAttribute(attributeName))
      flag1 = XmlConvertExtension.ToBoolean(reader.Value);
    if (!flag1)
      protection |= flag;
    else
      protection &= ~flag;
    return protection;
  }

  private void ParseIgnoreError(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "ignoredErrors")
      throw new XmlException();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "ignoredError")
          this.ExtractIgnoredError(reader, sheet);
        else
          reader.Read();
      }
    }
    reader.Read();
  }

  private void ExtractIgnoredError(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "ignoredError")
      throw new XmlException();
    ExcelIgnoreError excelIgnoreError = ExcelIgnoreError.None;
    string str = (string) null;
    int i = 0;
    for (int attributeCount = reader.AttributeCount; i < attributeCount; ++i)
    {
      reader.MoveToAttribute(i);
      if (reader.LocalName == "sqref")
        str = reader.Value;
      else if (XmlConvertExtension.ToBoolean(reader.Value))
      {
        int index = Array.IndexOf<string>(Excel2007Serializator.ErrorTagsSequence, reader.LocalName);
        if (index >= 0)
          excelIgnoreError |= Excel2007Serializator.ErrorsSequence[index];
      }
    }
    if (str == null)
      throw new XmlException();
    reader.MoveToElement();
    reader.Read();
  }

  private void ParseCustomWorksheetProperties(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "customProperties")
      throw new XmlException();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "customPr")
          this.ParseCustomProperty(reader, sheet);
        else
          reader.Skip();
      }
    }
    reader.Read();
  }

  private void ParseCustomProperty(XmlReader reader, WorksheetImpl sheet)
  {
  }

  private static int GetParsedXmlValue(string xmlValue)
  {
    return !xmlValue.StartsWith("-") ? (int) XmlConvertExtension.ToUInt32(xmlValue) : XmlConvertExtension.ToInt32(xmlValue);
  }

  private string GetPropertyData(string id, WorksheetDataHolder dataHolder)
  {
    RelationCollection relations = dataHolder.Relations;
    Relation relation = relations[id];
    relations.Remove(id);
    string parentItemPath = Path.GetDirectoryName(dataHolder.ArchiveItem.ItemName).Replace('\\', '/');
    byte[] data = dataHolder.ParentHolder.GetData(relation, parentItemPath, true);
    return Encoding.Unicode.GetString(data, 0, data.Length);
  }

  public static void ParseLegacyDrawingHF(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "legacyDrawingHF")
      throw new XmlException("Unexpected xml tag.");
    string relationId = reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships") ? reader.Value : throw new XmlException("Wrong xml format");
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    dataHolder.VmlHFDrawingsId = relationId;
    dataHolder.ParseVmlShapes((ShapeCollectionBase) sheet.HeaderFooterShapes, relationId, relations);
    reader.MoveToElement();
    reader.Skip();
  }

  private void ParseDrawings(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "drawing")
      throw new XmlException("Unexpected xml tag.");
    string relationId = reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships") ? reader.Value : throw new XmlException("Wrong xml format");
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    dataHolder.ParseDrawings(sheet, relationId, dictItemsToRemove);
    dataHolder.DrawingsId = relationId;
    reader.MoveToElement();
    reader.Skip();
  }

  private void ParseLegacyDrawing(XmlReader reader, WorksheetBaseImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "legacyDrawing")
      throw new XmlException("Unexpected xml tag.");
    string relationId = reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships") ? reader.Value : throw new XmlException("Wrong xml format");
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    dataHolder.ParseVmlShapes((ShapeCollectionBase) sheet.InnerShapes, relationId, (RelationCollection) null);
    dataHolder.VmlDrawingsId = relationId;
    reader.MoveToElement();
    reader.Skip();
  }

  private void ParseTwoCellAnchor(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    string drawingsPath,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (lstRelationIds == null)
      throw new ArgumentNullException(nameof (lstRelationIds));
    bool bRelative = reader.LocalName == "relSizeAnchor";
    string editAs = (string) null;
    if (reader.MoveToAttribute("editAs"))
    {
      editAs = reader.Value;
      this.m_drawingParser.placement = editAs;
    }
    reader.Read();
    Rectangle fromRect = new Rectangle();
    Rectangle toRect = new Rectangle();
    ShapeImpl shapeImpl = (ShapeImpl) null;
    MemoryStream data = (MemoryStream) null;
    Size shapeExtent = new Size(-1, -1);
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "from":
            fromRect = this.ParseAnchorPoint(reader);
            this.m_drawingParser.leftColumn = fromRect.X;
            this.m_drawingParser.leftColumnOffset = Helper.ConvertEmuToOffset(fromRect.Width, this.dpiX);
            this.m_drawingParser.topRow = fromRect.Y;
            this.m_drawingParser.topRowOffset = Helper.ConvertEmuToOffset(fromRect.Height, this.dpiY);
            continue;
          case "to":
            toRect = this.ParseAnchorPoint(reader);
            this.m_drawingParser.rightColumn = toRect.X;
            this.m_drawingParser.rightColumnOffset = Helper.ConvertEmuToOffset(toRect.Width, this.dpiX);
            this.m_drawingParser.bottomRow = toRect.Y;
            this.m_drawingParser.bottomRowOffset = Helper.ConvertEmuToOffset(toRect.Height, this.dpiY);
            continue;
          case "ext":
            shapeExtent = this.ParseExtent(reader);
            continue;
          case "pic":
            shapeImpl = this.ParsePicture(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
            continue;
          case "clientData":
            reader.Skip();
            continue;
          case "sp":
          case "cxnSp":
            this.m_drawingParser.preFix = reader.Prefix;
            this.m_drawingParser.shapeType = reader.LocalName;
            shapeImpl = this.CreateShape(reader, sheet, ref data, drawingsPath);
            if (this.m_enableAlternateContent)
            {
              shapeImpl.EnableAlternateContent = this.m_enableAlternateContent;
              continue;
            }
            continue;
          case "AlternateContent":
            shapeImpl = this.CreateShape(reader, sheet, ref data, drawingsPath);
            shapeImpl.XmlDataStream = (Stream) data;
            shapeImpl.IsEquationShape = true;
            continue;
          case "graphicFrame":
            reader.Skip();
            continue;
          default:
            data = this.ReadSingleNodeIntoStream(reader);
            shapeImpl = this.TryParseShape(data, sheet, drawingsPath);
            if (shapeImpl == null)
            {
              shapeImpl = new ShapeImpl(sheet.Application, (object) sheet.InnerShapes);
              sheet.InnerShapes.AddShape(shapeImpl);
              continue;
            }
            data = (MemoryStream) null;
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
    if (shapeImpl != null)
    {
      this.SetAnchor(shapeImpl, fromRect, toRect, shapeExtent, bRelative);
      shapeImpl.XmlDataStream = (Stream) data;
      this.ParseEditAsValue(shapeImpl, editAs);
    }
    this.m_enableAlternateContent = false;
  }

  private ShapeImpl CreateShape(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    ref MemoryStream data,
    string drawingsPath)
  {
    data = this.ReadSingleNodeIntoStream(reader);
    data.Position = 0L;
    reader = UtilityMethods.CreateReader((Stream) data);
    OfficeShapeType officeShapeType = OfficeShapeType.Unknown;
    ShapeImpl newShape = (ShapeImpl) null;
    string str1 = (string) null;
    int? nullable = new int?();
    string str2 = (string) null;
    string str3 = (string) null;
    if (reader.MoveToAttribute("macro"))
    {
      str3 = reader.Value;
      reader.MoveToElement();
    }
    if (reader.MoveToAttribute("textlink"))
      str2 = reader.Value;
    while (reader.NodeType != XmlNodeType.None)
    {
      reader.Read();
      if (reader.LocalName == "Choice" && this.IsChartExChoice(reader))
      {
        reader.Read();
        newShape = this.TryParseChart(this.ReadSingleNodeIntoStream(reader), sheet, drawingsPath, true);
        officeShapeType = OfficeShapeType.Chart;
      }
      else if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "cNvSpPr")
        {
          if (reader.MoveToAttribute("txBox") && XmlConvertExtension.ToBoolean(reader.Value))
          {
            officeShapeType = OfficeShapeType.TextBox;
            break;
          }
        }
        else if (reader.LocalName == "cNvPr")
        {
          int result;
          if (reader.MoveToAttribute("id") && int.TryParse(reader.Value, out result))
          {
            nullable = new int?(result);
            this.m_drawingParser.id = result;
          }
          if (reader.MoveToAttribute("name"))
          {
            this.m_drawingParser.name = reader.Value;
            str1 = reader.Value;
          }
          if (reader.MoveToAttribute("descr"))
            this.m_drawingParser.descr = reader.Value;
          if (reader.MoveToAttribute("hidden"))
            this.m_drawingParser.IsHidden = XmlConvertExtension.ToBoolean(reader.Value);
          if (reader.MoveToAttribute("title"))
            this.m_drawingParser.tittle = reader.Value;
        }
        if (reader.LocalName == "xfrm")
        {
          if (reader.MoveToAttribute("rot"))
          {
            string s = reader.Value;
            if (s != null && s.Length > 0)
              this.m_drawingParser.shapeRotation = double.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture) / 60000.0;
          }
          if (reader.MoveToAttribute("flipH"))
            this.m_drawingParser.FlipHorizontal = XmlConvertExtension.ToBoolean(reader.Value);
          if (reader.MoveToAttribute("flipV"))
            this.m_drawingParser.FlipVertical = XmlConvertExtension.ToBoolean(reader.Value);
          this.ParseForm(reader);
        }
        if (reader.LocalName == "prstGeom" || reader.LocalName == "custGeom")
        {
          if (!this.m_enableAlternateContent && this.m_drawingParser.preFix != "cdr")
          {
            if (reader.LocalName == "prstGeom")
            {
              string shapeString = "";
              if (reader.MoveToAttribute("prst"))
                shapeString = reader.Value;
              AutoShapeType autoShapeType = AutoShapeHelper.GetAutoShapeType(AutoShapeHelper.GetAutoShapeConstant(shapeString));
              if (autoShapeType != AutoShapeType.Unknown)
              {
                officeShapeType = OfficeShapeType.AutoShape;
                this.m_drawingParser.autoShapeType = autoShapeType;
                reader.MoveToElement();
                reader.Read();
              }
            }
            if (reader.LocalName == "avLst")
            {
              this.m_drawingParser.CustGeomStream = ShapeParser.ReadNodeAsStream(reader);
              break;
            }
            break;
          }
          break;
        }
        if (reader.LocalName == "Fallback")
          reader.Skip();
      }
    }
    data.Position = 0L;
    reader = UtilityMethods.CreateReader((Stream) data);
    switch (officeShapeType)
    {
      case OfficeShapeType.Unknown:
        newShape = new ShapeImpl(sheet.Application, (object) sheet.InnerShapes);
        if (nullable.HasValue)
          newShape.ShapeId = nullable.Value;
        if (str1 != null)
          newShape.Name = str1;
        sheet.InnerShapes.AddShape(newShape);
        break;
      case OfficeShapeType.AutoShape:
        AutoShapeImpl autoShapeImpl = new AutoShapeImpl(sheet.Application, (object) sheet.InnerShapes);
        this.m_drawingParser.AddShape(autoShapeImpl, this.m_workSheet);
        this.ParseAutoShape(autoShapeImpl, reader);
        sheet.InnerShapes.Add((IShape) autoShapeImpl);
        autoShapeImpl.ShapeExt.Logger.ResetFlag();
        autoShapeImpl.ShapeExt.Macro = str3;
        autoShapeImpl.ShapeExt.TextLink = str2;
        break;
      case OfficeShapeType.TextBox:
        ITextBoxShapeEx textBoxShapeEx = sheet.Shapes.AddTextBox();
        if (str2 != null && str2.Length > 0)
          textBoxShapeEx.TextLink = $"={str2}";
        newShape = (ShapeImpl) textBoxShapeEx;
        TextBoxShapeParser.ParseTextBox((ITextBox) textBoxShapeEx, reader, this);
        if (nullable.HasValue)
        {
          newShape.ShapeId = nullable.Value;
          break;
        }
        break;
    }
    return newShape;
  }

  public void ParseAutoShape(AutoShapeImpl autoShape, XmlReader reader)
  {
    if (autoShape == null)
      throw new ArgumentNullException(nameof (autoShape));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    while (reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "txBody":
            Stream data = ShapeParser.ReadNodeAsStream(reader);
            data.Position = 0L;
            autoShape.ShapeExt.PreservedElements.Add("TextBody", data);
            Excel2007Parser.ParseRichText(UtilityMethods.CreateReader(data), autoShape, this);
            continue;
          case "spPr":
            this.ParseProperties(reader, autoShape);
            continue;
          case "style":
            autoShape.ShapeExt.PreservedElements.Add("Style", ShapeParser.ReadNodeAsStream(reader));
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Read();
    }
  }

  private void ParseProperties(XmlReader reader, AutoShapeImpl autoShape)
  {
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "noFill":
            autoShape.ShapeExt.Fill.Visible = false;
            reader.Skip();
            continue;
          case "solidFill":
            Stream data1 = ShapeParser.ReadNodeAsStream(reader);
            data1.Position = 0L;
            autoShape.ShapeExt.PreservedElements.Add("Fill", data1);
            ChartParserCommon.ParseSolidFill(UtilityMethods.CreateReader(data1), this, autoShape.ShapeExt.Fill.ForeColorObject);
            continue;
          case "ln":
            Stream data2 = ShapeParser.ReadNodeAsStream(reader);
            data2.Position = 0L;
            autoShape.ShapeExt.PreservedElements.Add("Line", data2);
            TextBoxShapeParser.ParseLineProperties(UtilityMethods.CreateReader(data2), autoShape.ShapeExt.Line, false, this);
            continue;
          case "gradFill":
            Stream data3 = ShapeParser.ReadNodeAsStream(reader);
            data3.Position = 0L;
            autoShape.ShapeExt.PreservedElements.Add("Fill", data3);
            XmlReader reader1 = UtilityMethods.CreateReader(data3);
            IInternalFill fill1 = (IInternalFill) autoShape.ShapeExt.Fill;
            fill1.FillType = OfficeFillType.Gradient;
            fill1.PreservedGradient = ChartParserCommon.ParseGradientFill(reader1, this);
            continue;
          case "blipFill":
            autoShape.ShapeExt.PreservedElements.Add("Fill", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "pattFill":
            Stream data4 = ShapeParser.ReadNodeAsStream(reader);
            data4.Position = 0L;
            autoShape.ShapeExt.PreservedElements.Add("Fill", data4);
            XmlReader reader2 = UtilityMethods.CreateReader(data4);
            IInternalFill fill2 = (IInternalFill) autoShape.ShapeExt.Fill;
            fill2.FillType = OfficeFillType.Pattern;
            ChartParserCommon.ParsePatternFill(reader2, (IOfficeFill) fill2, this);
            continue;
          case "grpFill":
            autoShape.ShapeExt.PreservedElements.Add("Fill", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "effectLst":
            autoShape.ShapeExt.PreservedElements.Add("Effect", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "effectDag":
            autoShape.ShapeExt.PreservedElements.Add("Effect", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "scene3d":
            autoShape.ShapeExt.PreservedElements.Add("Scene3d", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "sp3d":
            autoShape.ShapeExt.PreservedElements.Add("Sp3d", ShapeParser.ReadNodeAsStream(reader));
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Read();
    }
  }

  private static void ParseRichText(
    XmlReader reader,
    AutoShapeImpl autoShape,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoShape == null)
      throw new ArgumentNullException("textBox");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "bodyPr":
            Excel2007Parser.ParseBodyProperties(reader, autoShape.TextFrameInternal);
            continue;
          case "p":
            Excel2007Parser.ParseParagraphs(reader, autoShape, parser);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private static void ParseBodyProperties(XmlReader reader, TextFrame textFrame)
  {
    if (reader.MoveToAttribute("vertOverflow"))
      textFrame.TextVertOverflowType = Helper.GetVerticalFlowType(reader.Value);
    if (reader.MoveToAttribute("horzOverflow"))
      textFrame.TextHorzOverflowType = Helper.GetHorizontalFlowType(reader.Value);
    if (reader.MoveToAttribute("vert"))
      textFrame.TextDirection = Helper.SetTextDirection(reader.Value);
    if (reader.MoveToAttribute("wrap"))
      textFrame.WrapTextInShape = reader.Value != "none";
    if (reader.MoveToAttribute("lIns"))
    {
      textFrame.SetLeftMargin(Helper.ParseInt(reader.Value));
      textFrame.IsAutoMargins = false;
    }
    if (reader.MoveToAttribute("tIns"))
    {
      textFrame.SetTopMargin(Helper.ParseInt(reader.Value));
      textFrame.IsAutoMargins = false;
    }
    if (reader.MoveToAttribute("rIns"))
    {
      textFrame.SetRightMargin(Helper.ParseInt(reader.Value));
      textFrame.IsAutoMargins = false;
    }
    if (reader.MoveToAttribute("bIns"))
    {
      textFrame.SetBottomMargin(Helper.ParseInt(reader.Value));
      textFrame.IsAutoMargins = false;
    }
    if (reader.MoveToAttribute("numCol"))
      textFrame.Columns.Number = Helper.ParseInt(reader.Value);
    if (reader.MoveToAttribute("spcCol"))
      textFrame.Columns.SpacingPt = (int) ((double) Helper.ParseInt(reader.Value) / 12700.0);
    string anchorType = "t";
    bool anchorCtrl = false;
    if (reader.MoveToAttribute("anchor"))
      anchorType = reader.Value;
    if (reader.MoveToAttribute("anchorCtr"))
      anchorCtrl = XmlConvertExtension.ToBoolean(reader.Value);
    Helper.SetAnchorPosition(textFrame, anchorType, anchorCtrl);
    reader.Read();
  }

  private static void ParseParagraphs(
    XmlReader reader,
    AutoShapeImpl autoShape,
    Excel2007Parser parser)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoShape == null)
      throw new ArgumentNullException("textBox");
    if (reader.LocalName != "p")
      throw new XmlException("Unexpected xml tag.");
    RichTextString richText = autoShape.ShapeExt.TextFrame.TextRange.RichText as RichTextString;
    string text = richText.Text;
    if (text != null && text.Length != 0 && !text.EndsWith("\n"))
      richText.AddText("\n", richText.GetFont(text.Length - 1));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "r":
            TextBoxShapeParser.ParseParagraphRun(reader, richText, parser, (ITextBox) null);
            continue;
          case "endParaRPr":
            TextBoxShapeParser.ParseParagraphEnd(reader, richText, parser);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private static void ParseParagraphRun(XmlReader reader, AutoShapeImpl autoShape)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoShape == null)
      throw new ArgumentNullException("textArea");
    if (reader.LocalName != "r")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    string str = (string) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "t":
            str = reader.ReadElementContentAsString();
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    if (str == null || str.Length == 0)
      str = "\n";
    string text = autoShape.ShapeExt.TextFrame.TextRange.Text;
    autoShape.ShapeExt.TextFrame.TextRange.Text = text + str;
    reader.Read();
  }

  private void ParseForm(XmlReader reader)
  {
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "off":
            if (reader.MoveToAttribute("x"))
              this.m_drawingParser.posX = XmlConvertExtension.ToInt32(reader.Value);
            if (reader.MoveToAttribute("y"))
            {
              this.m_drawingParser.posY = XmlConvertExtension.ToInt32(reader.Value);
              continue;
            }
            continue;
          case "ext":
            if (reader.MoveToAttribute("cx"))
              this.m_drawingParser.extCX = XmlConvertExtension.ToInt32(reader.Value);
            if (reader.MoveToAttribute("cy"))
            {
              this.m_drawingParser.extCY = XmlConvertExtension.ToInt32(reader.Value);
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

  private ShapeImpl TryParseChart(
    MemoryStream data,
    WorksheetBaseImpl sheet,
    string drawingPath,
    bool isChartEx)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    data.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader((Stream) data);
    reader.Read();
    string str = (string) null;
    string s = "0";
    while (reader.LocalName != "chart" && reader.NodeType != XmlNodeType.None)
    {
      if (reader.LocalName == "cNvPr" && reader.MoveToAttribute("name"))
      {
        str = reader.Value;
        if (reader.MoveToAttribute("id"))
          s = reader.Value;
      }
      reader.Read();
    }
    ChartShapeImpl chart = (ChartShapeImpl) null;
    if (reader.LocalName == "chart")
    {
      chart = (ChartShapeImpl) sheet.Charts.Add();
      ChartImpl chartObject = chart.ChartObject;
      WorksheetDataHolder dataHolder = sheet.DataHolder;
      FileDataHolder parentHolder = dataHolder.ParentHolder;
      RelationCollection drawingsRelations = dataHolder.DrawingsRelations;
      chartObject.DataHolder = dataHolder;
      this.ParseChartTag(reader, chartObject, drawingsRelations, parentHolder, drawingPath, isChartEx);
      chartObject.DataHolder = (WorksheetDataHolder) null;
      if (dataHolder.DrawingsRelations.Count == 0 && drawingsRelations.Count != 0)
        dataHolder.AssignDrawingrelation(drawingsRelations);
      if (str != null)
        chart.Name = str;
    }
    if (chart != null)
      chart.ShapeId = int.Parse(s);
    return (ShapeImpl) chart;
  }

  private ShapeImpl TryParseShape(MemoryStream data, WorksheetBaseImpl sheet, string drawingPath)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    data.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader((Stream) data);
    reader.Read();
    string str = (string) null;
    string s = "0";
    ChartShapeImpl chartShapeImpl = (ChartShapeImpl) null;
    ShapeImpl parent = sheet.InnerShapes.AddShape(new ShapeImpl(sheet.Application, (object) sheet.InnerShapes));
    bool flag = false;
    while (!(reader.LocalName == "grpSp") || reader.NodeType != XmlNodeType.EndElement)
    {
      switch (reader.LocalName)
      {
        case "grpSp":
          Stream stream1 = (Stream) this.ReadSingleNodeIntoStream(reader);
          if (parent.preservedShapeStreams == null)
            parent.preservedShapeStreams = new List<Stream>();
          parent.preservedShapeStreams.Add(stream1);
          flag = true;
          break;
        case "nvGrpSpPr":
          Stream stream2 = (Stream) this.ReadSingleNodeIntoStream(reader);
          if (parent.preservedShapeStreams == null)
            parent.preservedShapeStreams = new List<Stream>();
          parent.preservedShapeStreams.Add(stream2);
          flag = true;
          break;
        case "slicer":
          if (chartShapeImpl == null)
            chartShapeImpl = new ChartShapeImpl(sheet.Application, (object) parent);
          if (str != null)
            chartShapeImpl.Name = str;
          if (chartShapeImpl != null)
            chartShapeImpl.ShapeId = int.Parse(s);
          parent.GraphicFrameStream = ShapeParser.ReadNodeAsStream(reader);
          chartShapeImpl.GraphicFrameStream = parent.GraphicFrameStream;
          parent.ChildShapes.Add((ShapeImpl) chartShapeImpl);
          break;
        case "grpSpPr":
          Stream stream3 = (Stream) this.ReadSingleNodeIntoStream(reader);
          if (parent.preservedShapeStreams == null)
            parent.preservedShapeStreams = new List<Stream>();
          parent.preservedShapeStreams.Add(stream3);
          flag = true;
          break;
        case "cNvPr":
          if (reader.LocalName == "cNvPr" && reader.MoveToAttribute("name"))
          {
            str = reader.Value;
            if (reader.MoveToAttribute("id"))
            {
              s = reader.Value;
              break;
            }
            break;
          }
          break;
        case "pic":
          Stream stream4 = (Stream) this.ReadSingleNodeIntoStream(reader);
          if (parent.preservedPictureStreams == null)
            parent.preservedPictureStreams = new List<Stream>();
          parent.preservedPictureStreams.Add(stream4);
          flag = true;
          break;
        case "sp":
          Stream stream5 = (Stream) this.ReadSingleNodeIntoStream(reader);
          if (parent.preservedShapeStreams == null)
            parent.preservedShapeStreams = new List<Stream>();
          parent.preservedShapeStreams.Add(stream5);
          flag = true;
          break;
        case "chart":
          if (reader.LocalName == "chart")
          {
            if (chartShapeImpl == null)
              chartShapeImpl = new ChartShapeImpl(sheet.Application, (object) parent);
            parent.ChildShapes.Add((ShapeImpl) chartShapeImpl);
            ChartImpl chartObject = chartShapeImpl.ChartObject;
            WorksheetDataHolder dataHolder = sheet.DataHolder;
            FileDataHolder parentHolder = dataHolder.ParentHolder;
            RelationCollection drawingsRelations = dataHolder.DrawingsRelations;
            chartObject.DataHolder = dataHolder;
            this.ParseChartTag(reader, chartObject, drawingsRelations, parentHolder, drawingPath, false);
            chartObject.DataHolder = (WorksheetDataHolder) null;
            if (str != null)
              chartShapeImpl.Name = str;
          }
          if (chartShapeImpl != null)
          {
            chartShapeImpl.ShapeId = int.Parse(s);
            break;
          }
          break;
        case "off":
          chartShapeImpl = new ChartShapeImpl(sheet.Application, (object) parent);
          if (reader.LocalName == "off")
          {
            if (reader.MoveToAttribute("x"))
              chartShapeImpl.OffsetX = XmlConvertExtension.ToInt32(reader.Value);
            if (reader.MoveToAttribute("y"))
            {
              chartShapeImpl.OffsetY = XmlConvertExtension.ToInt32(reader.Value);
              break;
            }
            break;
          }
          break;
        case "ext":
          if (chartShapeImpl == null)
            chartShapeImpl = new ChartShapeImpl(sheet.Application, (object) parent);
          if (reader.LocalName == "ext")
          {
            if (reader.MoveToAttribute("cx"))
              chartShapeImpl.ExtentsX = XmlConvertExtension.ToInt32(reader.Value);
            if (reader.MoveToAttribute("cy"))
            {
              chartShapeImpl.ExtentsY = XmlConvertExtension.ToInt32(reader.Value);
              break;
            }
            break;
          }
          break;
        case "cxnSp":
          Stream stream6 = (Stream) this.ReadSingleNodeIntoStream(reader);
          if (parent.preservedInnerCnxnShapeStreams == null)
            parent.preservedInnerCnxnShapeStreams = new List<Stream>();
          parent.preservedInnerCnxnShapeStreams.Add(stream6);
          flag = true;
          break;
      }
      if (!flag)
        reader.Read();
      flag = false;
    }
    return parent;
  }

  internal MemoryStream ReadSingleNodeIntoStream(XmlReader reader)
  {
    MemoryStream data = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
    writer.WriteNode(reader, false);
    writer.Flush();
    return data;
  }

  private Size ParseExtent(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    int emuValue1 = -1;
    int emuValue2 = -1;
    if (reader.MoveToAttribute("cx"))
      emuValue1 = int.Parse(reader.Value);
    if (reader.MoveToAttribute("cy"))
      emuValue2 = int.Parse(reader.Value);
    if (this.m_drawingParser != null)
    {
      this.m_drawingParser.cx = Helper.ConvertEmuToOffset(emuValue1, this.dpiX);
      this.m_drawingParser.cy = Helper.ConvertEmuToOffset(emuValue2, this.dpiX);
    }
    return new Size((int) Math.Round(ApplicationImpl.ConvertToPixels((double) emuValue1, MeasureUnits.EMU)), (int) Math.Round(ApplicationImpl.ConvertToPixels((double) emuValue2, MeasureUnits.EMU)));
  }

  private void ParseEditAsValue(ShapeImpl shape, string editAs)
  {
    if (editAs == null)
      return;
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    switch (editAs)
    {
      case "twoCell":
        shape.IsMoveWithCell = true;
        shape.IsSizeWithCell = true;
        break;
      case "oneCell":
        shape.IsMoveWithCell = true;
        shape.IsSizeWithCell = false;
        break;
      case "absolute":
        shape.IsMoveWithCell = false;
        shape.IsSizeWithCell = false;
        break;
      default:
        throw new XmlException();
    }
  }

  private void SetAnchor(
    ShapeImpl shape,
    Rectangle fromRect,
    Rectangle toRect,
    Size shapeExtent,
    bool bRelative)
  {
    IWorksheet sheet = shape != null ? (IWorksheet) (shape.Worksheet as WorksheetImpl) : throw new ArgumentNullException(nameof (shape));
    fromRect = this.NormalizeAnchor(fromRect, sheet);
    toRect = this.NormalizeAnchor(toRect, sheet);
    MsofbtClientAnchor clientAnchor = shape.ClientAnchor;
    clientAnchor.LeftColumn = fromRect.Left;
    clientAnchor.LeftOffset = fromRect.Width;
    clientAnchor.TopRow = fromRect.Top;
    clientAnchor.TopOffset = fromRect.Height;
    clientAnchor.RightColumn = toRect.Left;
    clientAnchor.RightOffset = toRect.Width;
    clientAnchor.BottomRow = toRect.Top;
    clientAnchor.BottomOffset = toRect.Height;
    shape.EvaluateTopLeftPosition();
    if (shapeExtent.Width < 0)
    {
      shape.UpdateHeight();
      shape.UpdateWidth();
    }
    else
    {
      shape.Width = shapeExtent.Width;
      shape.Height = shapeExtent.Height;
      clientAnchor.OneCellAnchor = true;
    }
  }

  private Rectangle NormalizeAnchor(Rectangle anchorPoint, IWorksheet sheet)
  {
    if (sheet == null)
      return anchorPoint;
    int iColumnIndex = anchorPoint.Left + 1;
    int iRowIndex = anchorPoint.Top + 1;
    int num1;
    if (iRowIndex > sheet.Workbook.MaxRowCount)
    {
      anchorPoint.Y = sheet.Workbook.MaxRowCount - 1;
      num1 = 256 /*0x0100*/;
    }
    else
    {
      double pixels = ApplicationImpl.ConvertToPixels((double) anchorPoint.Height, MeasureUnits.EMU);
      int rowHeightInPixels = sheet.GetRowHeightInPixels(iRowIndex);
      num1 = rowHeightInPixels != 0 ? (int) Math.Round(pixels * 256.0 / (double) rowHeightInPixels) : 0;
    }
    int num2;
    if (iColumnIndex > sheet.Workbook.MaxColumnCount)
    {
      anchorPoint.X = sheet.Workbook.MaxColumnCount - 1;
      num2 = 1024 /*0x0400*/;
    }
    else
    {
      double pixels = ApplicationImpl.ConvertToPixels((double) anchorPoint.Width, MeasureUnits.EMU);
      int columnWidthInPixels = sheet.GetColumnWidthInPixels(iColumnIndex);
      num2 = columnWidthInPixels != 0 ? (int) Math.Round(pixels * 1024.0 / (double) columnWidthInPixels) : 0;
    }
    anchorPoint.Width = num2;
    anchorPoint.Height = num1;
    return anchorPoint;
  }

  private Rectangle ParseAnchorPoint(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    Rectangle anchorPoint = new Rectangle();
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        string localName = reader.LocalName;
        string s = reader.ReadElementContentAsString();
        switch (localName)
        {
          case "col":
            anchorPoint.X = XmlConvertExtension.ToInt32(s);
            continue;
          case "colOff":
            anchorPoint.Width = XmlConvertExtension.ToInt32(s);
            continue;
          case "row":
            anchorPoint.Y = XmlConvertExtension.ToInt32(s);
            continue;
          case "rowOff":
            anchorPoint.Height = XmlConvertExtension.ToInt32(s);
            continue;
          case "x":
            anchorPoint.X = (int) (XmlConvertExtension.ToDouble(s) * 1000.0);
            continue;
          case "y":
            anchorPoint.Y = (int) (XmlConvertExtension.ToDouble(s) * 1000.0);
            continue;
          default:
            throw new XmlException("Unexpected xml tag.");
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
    return anchorPoint;
  }

  private ShapeImpl ParsePicture(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    string drawingsPath,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (lstRelationIds == null)
      throw new ArgumentNullException(nameof (lstRelationIds));
    if (reader.LocalName != "pic")
      throw new XmlException("Unexpected xml tag.");
    BitmapShapeImpl shape = new BitmapShapeImpl(sheet.Application, (object) sheet.InnerShapes);
    WorksheetDataHolder dataHolder = sheet.DataHolder;
    RelationCollection drawingsRelations = dataHolder.DrawingsRelations;
    FileDataHolder parentHolder = dataHolder.ParentHolder;
    if (reader.MoveToAttribute("macro"))
    {
      shape.Macro = reader.Value;
      reader.MoveToElement();
    }
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "nvPicPr":
            this.ParsePictureProperties(reader, (ShapeImpl) shape, drawingsRelations, drawingsPath, parentHolder, lstRelationIds, dictItemsToRemove);
            continue;
          case "blipFill":
            this.ParseBlipFill(reader, shape, drawingsRelations, drawingsPath, parentHolder, lstRelationIds, dictItemsToRemove);
            continue;
          case "spPr":
            this.ParseShapeProperties(reader, (ShapeImpl) shape);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
    sheet.InnerShapes.AddPicture(shape);
    return (ShapeImpl) shape;
  }

  private void ParseShapeProperties(XmlReader reader, ShapeImpl shape)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (reader.LocalName != "spPr")
      throw new XmlException("Unexpected xml tag.");
    if (shape is BitmapShapeImpl bitmapShapeImpl)
    {
      Stream data = (Stream) new MemoryStream();
      XmlWriter writer = UtilityMethods.CreateWriter(data, Encoding.UTF8);
      writer.WriteNode(reader, false);
      writer.Flush();
      bitmapShapeImpl.ShapePropertiesStream = data;
    }
    else
      reader.Skip();
  }

  private void ParseBlipFill(
    XmlReader reader,
    BitmapShapeImpl shape,
    RelationCollection relations,
    string parentPath,
    FileDataHolder holder,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (lstRelationIds == null)
      throw new ArgumentNullException(nameof (lstRelationIds));
    if (reader.LocalName != "blipFill")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "blip":
            this.ParseBlipTag(reader, shape, relations, parentPath, holder, lstRelationIds, dictItemsToRemove);
            continue;
          case "srcRect":
            MemoryStream data = new MemoryStream();
            XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
            writer.WriteNode(reader, false);
            writer.Flush();
            shape.SourceRectStream = (Stream) data;
            data.Position = 0L;
            XmlReader reader1 = UtilityMethods.CreateReader((Stream) data);
            if (reader1.MoveToAttribute("l"))
              shape.CropLeftOffset = Convert.ToInt32(reader1.Value);
            if (reader1.MoveToAttribute("t"))
              shape.CropTopOffset = Convert.ToInt32(reader1.Value);
            if (reader1.MoveToAttribute("r"))
              shape.CropRightOffset = Convert.ToInt32(reader1.Value);
            if (reader1.MoveToAttribute("b"))
            {
              shape.CropBottomOffset = Convert.ToInt32(reader1.Value);
              continue;
            }
            continue;
          case "stretch":
          case "tile":
            reader.Skip();
            continue;
          default:
            throw new XmlException("Unexpected xml tag.");
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseBlipTag(
    XmlReader reader,
    BitmapShapeImpl shape,
    RelationCollection relations,
    string strParentPath,
    FileDataHolder holder,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    if (strParentPath == null)
      throw new ArgumentNullException(nameof (strParentPath));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (lstRelationIds == null)
      throw new ArgumentNullException(nameof (lstRelationIds));
    if (reader.MoveToAttribute("embed", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
    {
      string id = reader.Value;
      Relation relation = relations[id];
      lstRelationIds.Add(id);
      if (relation == null)
        throw new XmlException("Cannot find required relation");
      ZipArchiveItem zipArchiveItem = holder[relation, strParentPath];
      if (relation.Target != "NULL")
      {
        Image image = holder.GetImage(zipArchiveItem.ItemName);
        shape.Picture = image;
        ((WorkbookImpl) shape.Workbook).ShapesData.Pictures[(int) shape.BlipId - 1].PicturePath = zipArchiveItem.ItemName;
        dictItemsToRemove[zipArchiveItem.ItemName] = (object) null;
      }
    }
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      MemoryStream data = new MemoryStream();
      XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
      writer.WriteStartElement("root");
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "clrChange")
          shape.HasTransparency = true;
        writer.WriteNode(reader, false);
      }
      writer.WriteEndElement();
      writer.Flush();
      shape.BlipSubNodesStream = (Stream) data;
    }
    reader.Skip();
  }

  private void ParsePictureProperties(
    XmlReader reader,
    ShapeImpl shape,
    RelationCollection relations,
    string strParentPath,
    FileDataHolder holder,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    if (strParentPath == null)
      throw new ArgumentNullException(nameof (strParentPath));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (lstRelationIds == null)
      throw new ArgumentNullException(nameof (lstRelationIds));
    if (reader.LocalName != "nvPicPr")
      throw new XmlException("Unexcpected xml tag");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cNvPr":
            Excel2007Parser.ParseNVCanvasProperties(reader, (IShape) shape);
            continue;
          case "cNvPicPr":
            this.ParseNVPictureCanvas(reader, shape);
            continue;
          case "hlinkClick":
            this.ParseClickHyperlink(reader, shape, relations, strParentPath, holder, lstRelationIds, dictItemsToRemove);
            continue;
          default:
            throw new XmlException("Unexpected xml tag.");
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseNVPictureCanvas(XmlReader reader, ShapeImpl shape)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (reader.LocalName != "cNvPicPr")
      throw new XmlException("Unexpected xml tag.");
    reader.Skip();
  }

  public static void ParseNVCanvasProperties(XmlReader reader, IShape shape)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (reader.LocalName != "cNvPr")
      throw new XmlException("Unexpected xml tag.");
    shape.IsShapeVisible = true;
    if (reader.MoveToAttribute("id"))
      ((ShapeImpl) shape).ShapeId = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("name"))
      shape.Name = reader.Value;
    if (reader.MoveToAttribute("descr"))
      shape.AlternativeText = reader.Value;
    if (reader.MoveToAttribute("hidden"))
      shape.IsShapeVisible = !XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToElement();
    reader.Read();
    if (reader.NodeType != XmlNodeType.EndElement)
      return;
    reader.Read();
  }

  private void ParseClickHyperlink(
    XmlReader reader,
    ShapeImpl shape,
    RelationCollection relations,
    string strParentPath,
    FileDataHolder holder,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    if (strParentPath == null)
      throw new ArgumentNullException(nameof (strParentPath));
    if (holder == null)
      throw new ArgumentNullException(nameof (holder));
    if (lstRelationIds == null)
      throw new ArgumentNullException(nameof (lstRelationIds));
    if (reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
    {
      string id = reader.Value;
      Relation relation = relations[id];
      lstRelationIds.Add(id);
      shape.ImageRelation = relation;
      shape.IsHyperlink = true;
      reader.Skip();
    }
    reader.Skip();
  }

  private void ParseCommentList(XmlReader reader, List<string> arrAuthors, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (arrAuthors == null)
      throw new ArgumentNullException(nameof (arrAuthors));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "commentList")
      throw new XmlException("Unexpected tag");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "comment")
        this.ParseComment(reader, (IList<string>) arrAuthors, sheet);
      else
        reader.Skip();
    }
  }

  private void ParseComment(XmlReader reader, IList<string> authors, WorksheetImpl sheet)
  {
    reader.Skip();
  }

  private List<string> ParseAuthors(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "authors")
      throw new XmlException();
    List<string> authors = new List<string>();
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "author")
        authors.Add(reader.ReadElementContentAsString());
      else
        reader.Read();
    }
    reader.Read();
    return authors;
  }

  private void ParseShape(
    XmlReader reader,
    Dictionary<string, ShapeImpl> dictShapeIdToShape,
    RelationCollection relations,
    string parentItemPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dictShapeIdToShape == null)
      throw new ArgumentNullException(nameof (dictShapeIdToShape));
    if (parentItemPath == null || parentItemPath.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (parentItemPath));
    string key = reader.MoveToAttribute("type") ? UtilityMethods.RemoveFirstCharUnsafe(reader.Value) : throw new XmlException();
    ShapeImpl defaultShape;
    if (!dictShapeIdToShape.TryGetValue(key, out defaultShape))
    {
      reader.Skip();
    }
    else
    {
      ShapeParser shapeParser;
      if (!this.m_dictShapeParsers.TryGetValue(!reader.MoveToAttribute("spt") ? defaultShape.InnerSpRecord.Instance : int.Parse(reader.Value), out shapeParser))
        throw new XmlException();
      reader.MoveToElement();
      MemoryStream data = new MemoryStream();
      XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
      writer.WriteNode(reader, false);
      writer.Flush();
      data.Position = 0L;
      reader = UtilityMethods.CreateReader((Stream) data);
      if (shapeParser.ParseShape(reader, defaultShape, relations, parentItemPath))
        return;
      data.Position = 0L;
      reader = UtilityMethods.CreateReader((Stream) data);
      int instance = defaultShape.Instance;
      Stream xmlTypeStream = defaultShape.XmlTypeStream;
      defaultShape = new ShapeImpl(defaultShape.Application, defaultShape.Parent);
      defaultShape.VmlShape = true;
      defaultShape.XmlTypeStream = xmlTypeStream;
      defaultShape.SetInstance(instance);
      shapeParser.ParseShape(reader, defaultShape, relations, parentItemPath);
    }
  }

  private void ParseShapeType(
    XmlReader reader,
    ShapeCollectionBase shapes,
    Dictionary<string, ShapeImpl> dictShapeIdToShape,
    Stream layoutStream)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shapes == null)
      throw new ArgumentNullException(nameof (shapes));
    if (dictShapeIdToShape == null)
      throw new ArgumentNullException(nameof (dictShapeIdToShape));
    string key = (string) null;
    if (reader.MoveToAttribute("id"))
      key = reader.Value;
    if (!reader.MoveToAttribute("spt", "urn:schemas-microsoft-com:office:office"))
      return;
    string s = reader.Value;
    int num = key != null && s != null ? int.Parse(s) : throw new XmlException();
    reader.MoveToElement();
    ShapeParser shapeParser;
    if (!this.m_dictShapeParsers.TryGetValue(num, out shapeParser))
    {
      shapeParser = (ShapeParser) new UnknownVmlShapeParser();
      this.m_dictShapeParsers[num] = shapeParser;
    }
    if (layoutStream != null)
      shapes.ShapeLayoutStream = layoutStream;
    ShapeImpl shapeType = shapeParser.ParseShapeType(reader, shapes);
    shapeType.SetInstance(num);
    shapeType.VmlShape = true;
    if (dictShapeIdToShape.ContainsKey(key))
      return;
    dictShapeIdToShape.Add(key, shapeType);
  }

  private void ParseShapeWithoutType(
    XmlReader reader,
    ShapeCollectionBase shapes,
    RelationCollection relations,
    string parentItemPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (parentItemPath == null || parentItemPath.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (parentItemPath));
    if (reader.MoveToAttribute("type"))
      throw new XmlException("shape type exists");
    int num = -1;
    if (reader.MoveToAttribute("spt"))
      num = int.Parse(reader.Value);
    ShapeParser shapeParser;
    if (!this.m_dictShapeParsers.TryGetValue(num, out shapeParser))
      this.m_dictShapeParsers[num] = shapeParser;
    ShapeImpl defaultShape1 = new ShapeImpl(shapes.Application, (object) shapes);
    defaultShape1.SetInstance(num);
    defaultShape1.VmlShape = true;
    reader.MoveToElement();
    MemoryStream data = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
    writer.WriteNode(reader, false);
    writer.Flush();
    data.Position = 0L;
    reader = UtilityMethods.CreateReader((Stream) data);
    if (shapeParser.ParseShape(reader, defaultShape1, relations, parentItemPath))
      return;
    data.Position = 0L;
    reader = UtilityMethods.CreateReader((Stream) data);
    int instance = defaultShape1.Instance;
    Stream xmlTypeStream = defaultShape1.XmlTypeStream;
    ShapeImpl defaultShape2 = new ShapeImpl(defaultShape1.Application, defaultShape1.Parent);
    defaultShape2.VmlShape = true;
    defaultShape2.XmlTypeStream = xmlTypeStream;
    defaultShape2.SetInstance(instance);
    shapeParser.ParseShape(reader, defaultShape2, relations, parentItemPath);
  }

  private int ParseRichTextRun(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "r")
      throw new XmlException(nameof (reader));
    return this.m_book.InnerSST.AddIncrease((object) this.ParseTextWithFormat(reader, "si"), false);
  }

  private TextWithFormat ParseTextWithFormat(XmlReader reader, string closingTagName)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (closingTagName == null || closingTagName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (closingTagName));
    TextWithFormat textWithFormat = new TextWithFormat();
    while (reader.LocalName != closingTagName && reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "r")
        this.ParseFormattingRun(reader, textWithFormat);
      else
        reader.Skip();
    }
    return textWithFormat;
  }

  private void ParseFormattingRun(XmlReader reader, TextWithFormat textWithFormat)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (textWithFormat == null)
      throw new ArgumentNullException(nameof (textWithFormat));
    int iFontIndex = -1;
    int iStartPos = -1;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "r")
    {
      if (reader.LocalName == "rPr")
      {
        iFontIndex = this.ParseFont(reader, (List<int>) null);
        reader.Skip();
      }
      else if (reader.LocalName == "t")
      {
        if (!reader.IsEmptyElement)
        {
          iStartPos = textWithFormat.Text.Length;
          reader.Read();
          string str = reader.Value.Replace("\r", string.Empty);
          textWithFormat.Text += str;
          reader.Skip();
          if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName == "t")
            reader.Skip();
        }
        else
          reader.Skip();
      }
      else
        reader.Skip();
    }
    reader.Skip();
    if (iFontIndex < 0 || iStartPos < 0)
      return;
    textWithFormat.SetTextFontIndex(iStartPos, textWithFormat.Text.Length - 1, iFontIndex);
  }

  private int ParseText(XmlReader reader, bool setCount)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "t")
      throw new XmlException(nameof (reader));
    SSTDictionary innerSst = this.m_book.InnerSST;
    bool flag = false;
    if (reader.XmlSpace == XmlSpace.Preserve)
      flag = true;
    reader.Read();
    string key1 = XmlConvert.DecodeName(reader.Value).Replace("\r", string.Empty);
    if (flag)
    {
      TextWithFormat key2 = new TextWithFormat();
      key2.IsPreserved = true;
      key2.Text = key1;
      reader.Skip();
      reader.Skip();
      return innerSst.AddIncrease((object) key2, false);
    }
    int text = innerSst.AddIncrease((object) key1, setCount);
    reader.Skip();
    if (!string.IsNullOrEmpty(key1))
      reader.Skip();
    return text;
  }

  private int ParseText(XmlReader reader, bool setCount, out string text)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "t")
      throw new XmlException(nameof (reader));
    SSTDictionary innerSst = this.m_book.InnerSST;
    reader.Read();
    string key = (text = XmlConvert.DecodeName(reader.Value)).Replace("\r", string.Empty);
    int text1 = innerSst.AddIncrease((object) key, setCount);
    reader.Skip();
    reader.Skip();
    return text1;
  }

  private List<int> ParseNamedStyles(
    XmlReader reader,
    List<int> arrFontIndexes,
    List<FillImpl> arrFills,
    List<BordersCollection> arrBorders,
    Dictionary<int, int> arrNumberFormatIndexes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (arrFontIndexes == null)
      throw new ArgumentNullException(nameof (arrFontIndexes));
    if (arrFills == null)
      throw new ArgumentNullException(nameof (arrFills));
    if (arrBorders == null)
      throw new ArgumentNullException(nameof (arrBorders));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "cellStyleXfs")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    if (reader.IsEmptyElement)
      return (List<int>) null;
    reader.Read();
    List<int> namedStyles = new List<int>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        ExtendedFormatImpl extendedFormat = this.ParseExtendedFormat(reader, arrFontIndexes, arrFills, arrBorders, (List<int>) null, new bool?(), arrNumberFormatIndexes);
        extendedFormat.Record.ParentIndex = (ushort) this.m_book.MaxXFCount;
        ExtendedFormatImpl extendedFormatImpl = this.m_book.InnerExtFormats.ForceAdd(extendedFormat);
        namedStyles.Add(extendedFormatImpl.Index);
      }
      reader.Read();
    }
    return namedStyles;
  }

  private List<int> ParseCellFormats(
    XmlReader reader,
    List<int> arrNewFontIndexes,
    List<FillImpl> arrFills,
    List<BordersCollection> arrBorders,
    List<int> namedStyleIndexes,
    Dictionary<int, int> arrNumberFormatIndexes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (namedStyleIndexes == null)
    {
      this.m_book.InsertDefaultFonts();
      this.m_book.InsertDefaultValues();
    }
    if (arrNewFontIndexes == null)
      throw new ArgumentNullException(nameof (arrNewFontIndexes));
    if (arrFills == null)
      throw new ArgumentNullException(nameof (arrFills));
    if (arrBorders == null)
      throw new ArgumentNullException(nameof (arrBorders));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "cellXfs")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    if (reader.IsEmptyElement)
      return (List<int>) null;
    reader.Read();
    List<int> cellFormats = new List<int>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        ExtendedFormatImpl extendedFormatImpl = this.m_book.InnerExtFormats.Add(this.ParseExtendedFormat(reader, arrNewFontIndexes, arrFills, arrBorders, namedStyleIndexes, new bool?(false), arrNumberFormatIndexes));
        cellFormats.Add(extendedFormatImpl.Index);
      }
      reader.Read();
    }
    if (!arrBorders[0].IsEmptyBorder)
      arrBorders.RemoveAt(0);
    return cellFormats;
  }

  private void ParseStyles(XmlReader reader, List<int> arrNamedStyleIndexes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (arrNamedStyleIndexes == null)
    {
      this.m_book.InsertDefaultFonts();
      this.m_book.InsertDefaultValues();
    }
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "cellStyles")
      throw new XmlException("Unexpected xml element " + reader.LocalName);
    reader.Read();
    this.m_book.InnerStyles.Clear();
    List<int> validate = new List<int>();
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "cellStyles")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (!(reader.LocalName == "cellStyle"))
          throw new XmlException("Unexpected xml tag " + reader.LocalName);
        this.ParseStyle(reader, arrNamedStyleIndexes, ref validate);
      }
      reader.Read();
    }
  }

  private static string GetASCIIString(string plainText)
  {
    string[] strArray = plainText.Split(new char[1]{ '_' }, StringSplitOptions.RemoveEmptyEntries);
    List<string> stringList = new List<string>();
    foreach (string HexString in strArray)
    {
      if (HexString.StartsWith("x"))
        stringList.Add(Excel2007Parser.ConvertHextoAscii(HexString));
      else
        stringList.Add(HexString);
    }
    return string.Join("", stringList.ToArray()).Replace("\0", string.Empty);
  }

  private static string ConvertHextoAscii(string HexString)
  {
    HexString = HexString.TrimStart('x');
    string str = "";
    for (int startIndex = 0; startIndex < HexString.Length; startIndex += 2)
    {
      if (HexString.Length >= startIndex + 2)
      {
        HexString.Substring(startIndex, 2);
        str += Convert.ToChar(Convert.ToUInt32(HexString.Substring(startIndex, 2), 16 /*0x10*/)).ToString();
      }
    }
    return str;
  }

  private void ParseStyle(XmlReader reader, List<int> arrNamedStyleIndexes, ref List<int> validate)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (arrNamedStyleIndexes == null)
      throw new ArgumentNullException(nameof (arrNamedStyleIndexes));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "cellStyle")
      throw new XmlException("Unexpected xml item " + reader.LocalName);
    StyleRecord record = (StyleRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Style);
    if (reader.MoveToAttribute("name"))
    {
      string plainText = reader.Value;
      if (plainText.Length > (int) byte.MaxValue)
      {
        record.IsAsciiConverted = true;
        record.StyleName = Excel2007Parser.GetASCIIString(plainText);
        record.StyleNameCache = plainText;
      }
      else
        record.StyleName = plainText;
    }
    if (reader.MoveToAttribute("xfId"))
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      record.ExtendedFormatIndex = (ushort) arrNamedStyleIndexes[int32];
      record.DefXFIndex = this.m_book.Version != OfficeVersion.Excel97to2003 ? ushort.MaxValue : (ushort) 0;
    }
    if (reader.MoveToAttribute("builtinId"))
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      record.BuildInOrNameLen = (byte) int32;
      record.IsBuildInStyle = true;
    }
    if (reader.MoveToAttribute("customBuiltin"))
      record.IsBuiltIncustomized = this.ParseBoolean(reader, "customBuiltin", false);
    if (reader.MoveToAttribute("iLevel"))
      record.OutlineStyleLevel = XmlConvertExtension.ToByte(reader.Value);
    if (validate.Contains((int) record.ExtendedFormatIndex) && !record.IsBuildInStyle)
      return;
    validate.Add((int) record.ExtendedFormatIndex);
    this.m_book.InnerStyles.Add(record);
  }

  private ExtendedFormatImpl ParseExtendedFormat(
    XmlReader reader,
    List<int> arrFontIndexes,
    List<FillImpl> arrFills,
    List<BordersCollection> arrBorders,
    List<int> namedStyleIndexes,
    bool? includeDefault,
    Dictionary<int, int> arrNumberFormatIndexes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (arrFontIndexes == null)
      throw new ArgumentNullException(nameof (arrFontIndexes));
    if (arrFills == null)
      throw new ArgumentNullException(nameof (arrFills));
    if (arrBorders == null)
      throw new ArgumentNullException(nameof (arrBorders));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "xf")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    ExtendedFormatImpl extendedFormatImpl = new ExtendedFormatImpl(this.m_book.Application, (object) this.m_book);
    ExtendedFormatRecord record = extendedFormatImpl.Record;
    ExtendedXFRecord xfRecord = extendedFormatImpl.XFRecord;
    if (reader.MoveToAttribute("xfId"))
    {
      int uint16 = (int) XmlConvertExtension.ToUInt16(reader.Value);
      if (namedStyleIndexes != null)
        record.ParentIndex = (ushort) namedStyleIndexes[uint16];
      record.XFType = ExtendedFormatRecord.TXFType.XF_STYLE;
    }
    else if (namedStyleIndexes != null)
    {
      record.ParentIndex = (ushort) namedStyleIndexes[0];
      record.XFType = ExtendedFormatRecord.TXFType.XF_STYLE;
    }
    else
    {
      record.ParentIndex = (ushort) this.m_book.MaxXFCount;
      record.XFType = ExtendedFormatRecord.TXFType.XF_CELL;
    }
    this.ParseFontFillBorder(reader, extendedFormatImpl, arrFontIndexes, arrFills, arrBorders);
    if (reader.MoveToAttribute("numFmtId"))
    {
      int uint16 = (int) XmlConvertExtension.ToUInt16(reader.Value);
      record.FormatIndex = arrNumberFormatIndexes == null || !arrNumberFormatIndexes.ContainsKey(uint16) ? (ushort) uint16 : (ushort) arrNumberFormatIndexes[uint16];
    }
    if (reader.MoveToAttribute("pivotButton"))
      extendedFormatImpl.PivotButton = XmlConvertExtension.ToBoolean(reader.Value);
    bool hasAlignment;
    this.ParseIncludeAttributes(reader, extendedFormatImpl, includeDefault, out hasAlignment, arrNumberFormatIndexes);
    extendedFormatImpl.HorizontalAlignment = OfficeHAlign.HAlignGeneral;
    extendedFormatImpl.VerticalAlignment = OfficeVAlign.VAlignBottom;
    this.ParseAlignmentAndProtection(reader, extendedFormatImpl, hasAlignment);
    return this.m_book.AddExtendedProperties(extendedFormatImpl);
  }

  private void ParseAlignmentAndProtection(
    XmlReader reader,
    ExtendedFormatImpl format,
    bool hasAlignment)
  {
    ExtendedFormatRecord record = format.Record;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (record == null)
      throw new ArgumentNullException("record");
    reader.MoveToElement();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "xf")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "alignment":
            this.ParseAlignment(reader, record);
            if (!hasAlignment)
            {
              format.IncludeAlignment = true;
              break;
            }
            break;
          case "protection":
            this.ParseProtection(reader, record);
            break;
          default:
            throw new NotImplementedException(reader.LocalName);
        }
      }
      reader.Read();
    }
  }

  private void ParseAlignment(XmlReader reader, ExtendedFormatRecord record)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "alignment")
      throw new XmlException();
    if (reader.MoveToAttribute("horizontal"))
    {
      string str = reader.Value;
      record.HAlignmentType = (OfficeHAlign) Enum.Parse(typeof (Excel2007HAlign), str, true);
    }
    if (reader.MoveToAttribute("indent"))
      record.Indent = XmlConvertExtension.ToByte(reader.Value);
    if (reader.MoveToAttribute("justifyLastLine"))
      record.JustifyLast = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("readingOrder"))
      record.ReadingOrder = XmlConvertExtension.ToUInt16(reader.Value);
    if (reader.MoveToAttribute("shrinkToFit"))
      record.ShrinkToFit = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("textRotation"))
      record.Rotation = XmlConvertExtension.ToUInt16(reader.Value);
    if (reader.MoveToAttribute("wrapText"))
      record.WrapText = XmlConvertExtension.ToBoolean(reader.Value);
    if (!reader.MoveToAttribute("vertical"))
      return;
    string str1 = reader.Value;
    record.VAlignmentType = (OfficeVAlign) Enum.Parse(typeof (Excel2007VAlign), str1, true);
  }

  private void ParseProtection(XmlReader reader, ExtendedFormatRecord record)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "protection")
      throw new XmlException("Unable to locate necessary xml tag");
    if (reader.MoveToAttribute("hidden"))
      record.IsHidden = XmlConvertExtension.ToBoolean(reader.Value);
    if (!reader.MoveToAttribute("locked"))
      return;
    record.IsLocked = XmlConvertExtension.ToBoolean(reader.Value);
  }

  private void ParseIncludeAttributes(
    XmlReader reader,
    ExtendedFormatImpl format,
    bool? defaultValue,
    out bool hasAlignment,
    Dictionary<int, int> arrNumberFormatIndexes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    hasAlignment = false;
    ExtendedFormatRecord record = format.Record;
    if (reader.MoveToAttribute("applyAlignment"))
    {
      format.IncludeAlignment = XmlConvertExtension.ToBoolean(reader.Value);
      if (format.IncludeAlignment)
        hasAlignment = true;
    }
    else if (defaultValue.HasValue)
      record.IsNotParentAlignment = defaultValue.Value;
    if (reader.MoveToAttribute("applyBorder"))
      format.IncludeBorder = XmlConvertExtension.ToBoolean(reader.Value);
    else if (defaultValue.HasValue && record.BorderIndex < (ushort) 1)
      record.IsNotParentBorder = defaultValue.Value;
    else if (record.BorderIndex > (ushort) 0)
      format.IncludeBorder = true;
    if (reader.MoveToAttribute("applyFont"))
      format.IncludeFont = XmlConvertExtension.ToBoolean(reader.Value);
    else if (defaultValue.HasValue && record.FontIndex < (ushort) 1)
      record.IsNotParentFont = defaultValue.Value;
    else if (record.FontIndex > (ushort) 0)
      format.IncludeFont = true;
    if (reader.MoveToAttribute("applyNumberFormat"))
      format.IncludeNumberFormat = arrNumberFormatIndexes != null && arrNumberFormatIndexes.ContainsValue((int) record.FormatIndex) || XmlConvertExtension.ToBoolean(reader.Value);
    else if (defaultValue.HasValue)
    {
      record.IsNotParentFormat = defaultValue.Value;
      if (record.FormatIndex > (ushort) 0)
        format.IncludeNumberFormat = true;
    }
    if (reader.MoveToAttribute("applyFill"))
      format.IncludePatterns = XmlConvertExtension.ToBoolean(reader.Value);
    else if (defaultValue.HasValue && record.FillIndex < (ushort) 1)
      record.IsNotParentPattern = defaultValue.Value;
    else if (record.FillIndex > (ushort) 0)
      format.IncludePatterns = true;
    if (reader.MoveToAttribute("applyProtection"))
      format.IncludeProtection = XmlConvertExtension.ToBoolean(reader.Value);
    else if (defaultValue.HasValue)
      record.IsNotParentCellOptions = defaultValue.Value;
    defaultValue = new bool?(false);
    if (reader.MoveToAttribute("quotePrefix"))
    {
      format.IsFirstSymbolApostrophe = XmlConvertExtension.ToBoolean(reader.Value);
    }
    else
    {
      if (!defaultValue.HasValue)
        return;
      record._123Prefix = defaultValue.Value;
    }
  }

  private void ParseFontFillBorder(
    XmlReader reader,
    ExtendedFormatImpl extendedFormat,
    List<int> arrFontIndexes,
    List<FillImpl> arrFills,
    List<BordersCollection> arrBorders)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (extendedFormat == null)
      throw new ArgumentNullException(nameof (extendedFormat));
    if (arrFontIndexes == null)
      throw new ArgumentNullException(nameof (arrFontIndexes));
    if (arrFills == null)
      throw new ArgumentNullException(nameof (arrFills));
    if (arrBorders == null)
      throw new ArgumentNullException(nameof (arrBorders));
    ExtendedFormatRecord record = extendedFormat.Record;
    if (reader.MoveToAttribute("fontId"))
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      record.FontIndex = (ushort) arrFontIndexes[int32];
    }
    if (reader.MoveToAttribute("fillId"))
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      record.FillIndex = (ushort) int32;
      Excel2007Parser.CopyFillSettings(arrFills[int32], extendedFormat);
    }
    if (!reader.MoveToAttribute("borderId"))
      return;
    int index1 = XmlConvertExtension.ToInt32(reader.Value);
    record.BorderIndex = (ushort) index1;
    if (index1 > 0)
      extendedFormat.HasBorder = true;
    if (index1 == arrBorders.Count)
      index1 = arrBorders.Count - 1;
    if (index1 == -1)
      return;
    BordersCollection arrBorder1 = arrBorders[index1];
    if (index1 == 0 && !arrBorder1.IsEmptyBorder)
    {
      for (int index2 = 1; index2 < arrBorders.Count; ++index2)
      {
        BordersCollection arrBorder2 = arrBorders[index2];
        if (arrBorder2.IsEmptyBorder)
        {
          this.CopyBorderSettings(arrBorder2, extendedFormat);
          break;
        }
      }
    }
    else
      this.CopyBorderSettings(arrBorder1, extendedFormat);
  }

  private void CopyBorderSettings(BordersCollection borders, ExtendedFormatImpl format)
  {
    if (borders == null)
      throw new ArgumentNullException(nameof (borders));
    ExtendedFormatRecord extendedFormatRecord = format != null ? format.Record : throw new ArgumentNullException("record");
    IBorder border1 = borders[OfficeBordersIndex.EdgeLeft];
    if (border1 != null && (format.IncludeBorder || Excel2007Parser.BordersDifferent(border1, format.LeftBorderColor, format.LeftBorderLineStyle)))
    {
      format.IncludeBorder = true;
      format.LeftBorderColor.CopyFrom(border1.ColorObject, true);
      format.LeftBorderLineStyle = border1.LineStyle;
    }
    else if (border1 != null)
      extendedFormatRecord.BorderLeft = border1.LineStyle;
    IBorder border2 = borders[OfficeBordersIndex.EdgeRight];
    if (border2 != null && (format.IncludeBorder || Excel2007Parser.BordersDifferent(border2, format.RightBorderColor, format.RightBorderLineStyle)))
    {
      format.IncludeBorder = true;
      format.RightBorderColor.CopyFrom(border2.ColorObject, true);
      format.RightBorderLineStyle = border2.LineStyle;
    }
    else if (border2 != null)
      extendedFormatRecord.BorderRight = border2.LineStyle;
    IBorder border3 = borders[OfficeBordersIndex.EdgeTop];
    if (border3 != null && (format.IncludeBorder || Excel2007Parser.BordersDifferent(border3, format.TopBorderColor, format.TopBorderLineStyle)))
    {
      format.IncludeBorder = true;
      format.TopBorderColor.CopyFrom(border3.ColorObject, true);
      format.TopBorderLineStyle = border3.LineStyle;
    }
    else if (border3 != null)
      extendedFormatRecord.BorderTop = border3.LineStyle;
    IBorder border4 = borders[OfficeBordersIndex.EdgeBottom];
    if (border4 != null && (format.IncludeBorder || Excel2007Parser.BordersDifferent(border4, format.BottomBorderColor, format.BottomBorderLineStyle)))
    {
      format.IncludeBorder = true;
      format.BottomBorderColor.CopyFrom(border4.ColorObject, true);
      format.BottomBorderLineStyle = border4.LineStyle;
    }
    else if (border4 != null)
      extendedFormatRecord.BorderBottom = border4.LineStyle;
    IBorder border5 = borders[OfficeBordersIndex.DiagonalDown];
    if (border5 != null)
    {
      if (format.IncludeBorder || Excel2007Parser.BordersDifferent(border5, format.DiagonalBorderColor, format.DiagonalDownBorderLineStyle))
      {
        format.IncludeBorder = true;
        format.DiagonalBorderColor.CopyFrom(border5.ColorObject, true);
        format.DiagonalDownBorderLineStyle = border5.LineStyle;
      }
      else
      {
        extendedFormatRecord.DiagonalLineStyle = (ushort) border5.LineStyle;
        extendedFormatRecord.DiagonalFromTopLeft = border5.ShowDiagonalLine;
      }
      format.DiagonalDownVisible = border5.ShowDiagonalLine;
    }
    IBorder border6 = borders[OfficeBordersIndex.DiagonalUp];
    if (border6 == null)
      return;
    if (format.IncludeBorder || Excel2007Parser.BordersDifferent(border6, format.DiagonalBorderColor, format.DiagonalUpBorderLineStyle))
    {
      format.IncludeBorder = true;
      format.DiagonalBorderColor.CopyFrom(border6.ColorObject, true);
      format.DiagonalUpBorderLineStyle = border6.LineStyle;
    }
    else
    {
      extendedFormatRecord.DiagonalLineStyle = (ushort) border6.LineStyle;
      extendedFormatRecord.DiagonalFromBottomLeft = border6.ShowDiagonalLine;
    }
    format.DiagonalUpVisible = border6.ShowDiagonalLine;
  }

  private static bool BordersDifferent(IBorder border, ChartColor color, OfficeLineStyle lineStyle)
  {
    ChartColor colorObject = border.ColorObject;
    return colorObject.ColorType == color.ColorType || colorObject.Value != color.Value || border.LineStyle != lineStyle;
  }

  private static void ParseRelation(XmlReader reader, RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    string id = (string) null;
    string type = (string) null;
    string target = (string) null;
    bool isExternal = false;
    string str = "styles.xml";
    if (reader.MoveToAttribute("Id"))
      id = reader.Value;
    if (reader.MoveToAttribute("Type"))
      type = reader.Value;
    if (reader.MoveToAttribute("Target"))
    {
      target = reader.Value;
      if (target.ToLower().Contains(str))
        target = str;
    }
    if (reader.MoveToAttribute("TargetMode"))
      isExternal = reader.Value == "External";
    Relation relation = new Relation(target, type, isExternal);
    relations[id] = relation;
  }

  private void ParseSheetsOptions(
    XmlReader reader,
    RelationCollection relations,
    FileDataHolder holder,
    string bookPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    if (reader.LocalName != "sheets")
      throw new XmlException("Unexpected tag name " + reader.LocalName);
    this.m_book.Objects.Clear();
    this.m_book.InnerWorksheets.Clear();
    reader.Read();
    int num = 0;
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "sheets")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "sheet")
          this.ParseWorkbookSheetEntry(reader, relations, holder, bookPath, ++num);
        else
          reader.Skip();
      }
      else
        reader.Read();
    }
    reader.Read();
  }

  private void ParseWorkbookSheetEntry(
    XmlReader reader,
    RelationCollection relations,
    FileDataHolder holder,
    string bookPath,
    int sheetRelationIdCount)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    string str1 = (string) null;
    string strVisibility = (string) null;
    string id1 = (string) null;
    string str2 = (string) null;
    if (reader.MoveToAttribute("name"))
      str1 = reader.Value;
    if (reader.MoveToAttribute("state"))
      strVisibility = reader.Value;
    if (reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      id1 = reader.Value;
    if (reader.MoveToAttribute("sheetId"))
      str2 = reader.Value;
    if (id1 == "" && strVisibility == "veryHidden")
      return;
    Relation relation = relations[id1];
    if (relation == null)
    {
      relation = new Relation($"worksheets/sheet{sheetRelationIdCount}.xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet");
      string id2 = $"rId{relations.Count}";
      relations[id2] = relation;
      id1 = id2;
    }
    WorksheetBaseImpl sheet;
    switch (relation.Type)
    {
      case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet":
        sheet = (WorksheetBaseImpl) this.m_book.InnerWorksheets.Add(str1);
        break;
      case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartsheet":
        sheet = (WorksheetBaseImpl) this.m_book.InnerCharts.Add(str1);
        sheet.PageSetupBase.Orientation = OfficePageOrientation.Landscape;
        break;
      default:
        throw new XmlException("Unknown part type: " + relation.Type);
    }
    sheet.DataHolder = new WorksheetDataHolder(holder, relation, bookPath);
    sheet.m_dataHolder.RelationId = id1;
    sheet.m_dataHolder.SheetId = str2;
    sheet.IsSaved = true;
    this.SetVisibilityState(sheet, strVisibility);
    relations.Remove(id1);
  }

  private void SetVisibilityState(WorksheetBaseImpl sheet, string strVisibility)
  {
    if (strVisibility == null)
      return;
    switch (strVisibility)
    {
      case "hidden":
        sheet.Visibility = OfficeWorksheetVisibility.Hidden;
        break;
      case "veryHidden":
        sheet.Visibility = OfficeWorksheetVisibility.StrongHidden;
        break;
      case "visible":
        sheet.Visibility = OfficeWorksheetVisibility.Visible;
        break;
      default:
        throw new ArgumentException("Unknown visibility state type");
    }
  }

  private void ParseDictionaryEntry(
    XmlReader reader,
    IDictionary<string, string> dictionary,
    string keyAttribute,
    string valueAttribute)
  {
    string key = (string) null;
    string str = (string) null;
    if (reader.MoveToAttribute(keyAttribute))
      key = reader.Value;
    if (reader.MoveToAttribute(valueAttribute))
      str = reader.Value;
    if (key == null || str == null)
      throw new XmlReadingException("Unable to parse dictionary entry item from Content type");
    dictionary.Add(key, str);
  }

  private void ParseMergeRegion(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (!(reader.LocalName == "mergeCell"))
      return;
    string name = reader.MoveToAttribute("ref") ? reader.Value : throw new InvalidDataException();
    (sheet.Range[name] as RangeImpl).MergeWithoutCheck();
    reader.Skip();
  }

  private string ParseNamedRange(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    string namedRange = (string) null;
    bool flag1 = false;
    int index = -1;
    bool flag2 = false;
    if (reader.LocalName == "definedName")
    {
      string name1 = reader.MoveToAttribute("name") ? reader.Value : throw new ApplicationException("Cannot find name for named range");
      if (reader.MoveToAttribute("localSheetId"))
      {
        flag1 = true;
        index = int.Parse(reader.Value);
      }
      if (reader.MoveToAttribute("hidden"))
        flag2 = XmlConvertExtension.ToBoolean(reader.Value);
      WorksheetImpl worksheetImpl = flag1 ? this.m_book.Objects[index] as WorksheetImpl : (WorksheetImpl) null;
      IName name2 = !flag1 && worksheetImpl == null ? this.m_book.Names.Add(name1) : worksheetImpl.Names.Add(name1);
      reader.Read();
      namedRange = reader.Value;
      this.m_book.HasApostrophe = namedRange.Contains("'");
      NameImpl nameImpl = (NameImpl) name2;
      nameImpl.Visible = !flag2;
      if (flag1)
        nameImpl.Record.IndexOrGlobal = (ushort) (index + 1);
      reader.Skip();
    }
    return namedRange;
  }

  private FormatImpl ParseDxfNumberFormat(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "numFmt")
      throw new XmlException("Unexpected tag " + reader.LocalName);
    string str = reader.MoveToAttribute("formatCode") ? reader.Value : throw new XmlException("formatCode wasn't found");
    int num = reader.MoveToAttribute("numFmtId") ? Convert.ToInt32(reader.Value) : throw new XmlException("numFmtId wasn't found");
    if (str != string.Empty)
    {
      if (!this.m_book.InnerFormats.ContainsFormat(str))
      {
        if (Array.IndexOf<int>(this.DEF_NUMBERFORMAT_INDEXES, num) < 0)
          num = 163 + this.m_book.InnerFormats.Count - 36 + 1;
      }
      else
        num = this.m_book.InnerFormats[str].Index;
    }
    this.m_book.InnerFormats.Add(num, str);
    return this.m_book.InnerFormats[num];
  }

  private List<int> ParseFonts(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    List<int> fontIndexes = new List<int>();
    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "fonts")
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
          this.ParseFont(reader, fontIndexes);
        reader.Read();
      }
    }
    return fontIndexes;
  }

  private int ParseFont(XmlReader reader, List<int> fontIndexes)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    FontImpl font = (FontImpl) this.m_book.CreateFont((IOfficeFont) null, false);
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType == XmlNodeType.Whitespace)
        reader.Read();
      this.ParseFontSettings(reader, font);
    }
    FontImpl fontImpl = (FontImpl) this.m_book.InnerFonts.Add((IOfficeFont) font);
    fontIndexes?.Add(fontImpl.Index);
    return fontImpl.Index;
  }

  private FontImpl ParseFont(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    FontImpl font = new FontImpl(this.m_book.Application, (object) this.m_book);
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      this.ParseFontSettings(reader, font);
    }
    return font;
  }

  private void ParseFontSettings(XmlReader reader, FontImpl font)
  {
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != nameof (font) && reader.LocalName != "rPr")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "b":
            font.Bold = this.ParseBoolean(reader, "val", true);
            break;
          case "i":
            font.Italic = this.ParseBoolean(reader, "val", true);
            break;
          case "name":
          case "rFont":
            font.FontName = this.ParseValue(reader, "val");
            break;
          case "sz":
            string s = this.ParseValue(reader, "val");
            font.Size = double.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture);
            break;
          case "strike":
            font.Strikethrough = this.ParseBoolean(reader, "val", true);
            break;
          case "u":
            string str1 = this.ParseValue(reader, "val");
            font.Underline = str1 != null ? (OfficeUnderline) Enum.Parse(typeof (OfficeUnderline), str1, true) : (font.Underline = OfficeUnderline.Single);
            break;
          case "vertAlign":
            string str2 = this.ParseValue(reader, "val");
            font.VerticalAlignment = (OfficeFontVerticalAlignment) Enum.Parse(typeof (OfficeFontVerticalAlignment), str2, true);
            break;
          case "shadow":
            font.MacOSShadow = this.ParseBoolean(reader, "val", true);
            break;
          case "color":
            font.ColorObject.CopyFrom(this.ParseColor(reader), true);
            break;
          case "charset":
            font.CharSet = this.ParseCharSet(reader);
            break;
          case "family":
            font.Family = this.ParseFamily(reader);
            break;
        }
        reader.Read();
      }
      else
        reader.Skip();
    }
  }

  private byte ParseFamily(XmlReader reader)
  {
    byte family = 0;
    if (reader.MoveToAttribute("val"))
      family = byte.Parse(reader.Value);
    return family;
  }

  private byte ParseCharSet(XmlReader reader)
  {
    byte charSet = 1;
    if (reader.MoveToAttribute("val"))
      charSet = byte.Parse(reader.Value);
    return charSet;
  }

  private Dictionary<int, int> ParseNumberFormats(XmlReader reader)
  {
    Dictionary<int, int> result = new Dictionary<int, int>();
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "numFmts")
      throw new XmlException("Unexpected tag " + reader.LocalName);
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "numFmts")
      {
        if (reader.NodeType == XmlNodeType.Element)
          this.ParseNumberFormat(reader, result);
        reader.Read();
      }
    }
    return result;
  }

  private void ParseNumberFormat(XmlReader reader, Dictionary<int, int> result)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    string str = reader.MoveToAttribute("formatCode") ? reader.Value : throw new XmlException("formatCode wasn't found");
    int num1 = reader.MoveToAttribute("numFmtId") ? Convert.ToInt32(reader.Value) : throw new XmlException("numFmtId wasn't found");
    int num2 = num1;
    if (str != string.Empty)
    {
      if (!this.m_book.InnerFormats.ContainsFormat(str))
      {
        if (Array.IndexOf<int>(this.DEF_NUMBERFORMAT_INDEXES, num1) < 0)
          num2 = 163 + this.m_book.InnerFormats.Count - 36 + 1;
      }
      else
        num2 = this.m_book.InnerFormats[str].Index;
      result.Add(num1, num2);
      num1 = num2;
    }
    this.m_book.InnerFormats.Add(num1, str);
    this.m_book.InnerFormats.HasNumberFormats = true;
  }

  private ChartColor ParseColor(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    ChartColor color = new ChartColor(OfficeKnownColors.BlackCustom);
    this.ParseColor(reader, color);
    return color;
  }

  private void ParseColor(XmlReader reader, ChartColor color)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.MoveToAttribute("indexed"))
    {
      OfficeKnownColors int32 = (OfficeKnownColors) Convert.ToInt32(reader.Value);
      color.SetIndexed(int32, true, this.m_book);
    }
    else
    {
      Color empty = ColorExtension.Empty;
      double dTintValue = 0.0;
      if (reader.MoveToAttribute("tint"))
        dTintValue = XmlConvertExtension.ToDouble(reader.Value);
      if (reader.MoveToAttribute("rgb"))
      {
        Color rgb = ColorExtension.FromArgb(int.Parse(reader.Value, NumberStyles.HexNumber));
        color.SetRGB(rgb, (IWorkbook) this.m_book, dTintValue);
        color.ColorType = ColorType.RGB;
      }
      else
      {
        if (!reader.MoveToAttribute("theme"))
          return;
        int int32 = Convert.ToInt32(reader.Value);
        color.SetTheme(int32, (IWorkbook) this.m_book, dTintValue);
      }
    }
  }

  private void ParseFillColor(XmlReader reader, ChartColor color)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.MoveToAttribute("indexed"))
    {
      OfficeKnownColors int32 = (OfficeKnownColors) Convert.ToInt32(reader.Value);
      color.SetIndexed(int32, true, this.m_book);
    }
    else
    {
      Color empty = ColorExtension.Empty;
      double dTintValue = 0.0;
      if (reader.MoveToAttribute("auto"))
      {
        int themeIndex = 0;
        color.SetTheme(themeIndex, (IWorkbook) this.m_book, dTintValue);
      }
      if (reader.MoveToAttribute("tint"))
        dTintValue = XmlConvertExtension.ToDouble(reader.Value);
      if (reader.MoveToAttribute("rgb"))
      {
        Color rgb = ColorExtension.FromArgb(int.Parse(reader.Value, NumberStyles.HexNumber));
        color.SetRGB(rgb, (IWorkbook) this.m_book, dTintValue);
        color.ColorType = ColorType.RGB;
      }
      else
      {
        if (!reader.MoveToAttribute("theme"))
          return;
        int int32 = Convert.ToInt32(reader.Value);
        color.SetTheme(int32, (IWorkbook) this.m_book, dTintValue);
      }
    }
  }

  public static Color ConvertColorByTint(Color color, double dTint)
  {
    double dHue;
    double dLuminance;
    double dSaturation;
    Excel2007Parser.ConvertRGBtoHLS(color, out dHue, out dLuminance, out dSaturation);
    if (dTint < 0.0)
      dLuminance *= 1.0 + dTint;
    if (dTint > 0.0)
      dLuminance = dLuminance * (1.0 - dTint) + ((double) byte.MaxValue - (double) byte.MaxValue * (1.0 - dTint));
    Excel2007Parser.ConvertHLSToRGB(dHue, dLuminance, dSaturation);
    return Excel2007Parser.ConvertHLSToRGB(dHue, dLuminance, dSaturation);
  }

  internal static Color ConvertColorByTintBlip(Color color, double dTint)
  {
    int[] numArray = new int[3]
    {
      (int) color.R,
      (int) color.G,
      (int) color.B
    };
    for (int index = 0; index < 3; ++index)
    {
      double num = Excel2007Parser.CalcDouble(numArray[index]);
      double doubleValue = num * (1.0 + (1.0 - dTint));
      if (dTint > 0.0)
        doubleValue = num * (1.0 - (1.0 - dTint)) + (1.0 - dTint);
      numArray[index] = Excel2007Parser.CalcInt(doubleValue);
    }
    return Color.FromArgb((int) color.A, (int) (byte) numArray[0], (int) (byte) numArray[1], (int) (byte) numArray[2]);
  }

  internal static double CalcDouble(int integer)
  {
    double num = (double) integer / (double) byte.MaxValue;
    if (num < 0.0)
      return 0.0;
    if (num <= 0.04045)
      return num / 12.92;
    return num > 1.0 ? 1.0 : Math.Pow((num + 0.055) / 1.055, 2.4);
  }

  internal static int CalcInt(double doubleValue)
  {
    return (int) Math.Round((doubleValue >= 0.0 ? (doubleValue > 0.0031308 ? (doubleValue >= 1.0 ? 1.0 : 1.055 * Math.Pow(doubleValue, 5.0 / 12.0) - 0.055) : doubleValue * 12.92) : 0.0) * (double) byte.MaxValue, 0);
  }

  public static void ConvertRGBtoHLS(
    Color color,
    out double dHue,
    out double dLuminance,
    out double dSaturation)
  {
    dHue = 0.0;
    dLuminance = 0.0;
    dSaturation = 0.0;
    byte r = color.R;
    byte g = color.G;
    byte b = color.B;
    byte num1 = Math.Min(r, Math.Min(g, b));
    byte num2 = Math.Max(r, Math.Max(g, b));
    double num3 = (double) ((int) num2 - (int) num1);
    double num4 = (double) ((int) num2 + (int) num1);
    dLuminance = (num4 * (double) byte.MaxValue + (double) byte.MaxValue) / 510.0;
    if ((int) num2 == (int) num1)
    {
      dSaturation = 0.0;
      dHue = 170.0;
    }
    else
    {
      dSaturation = dLuminance > (double) sbyte.MaxValue ? (num3 * (double) byte.MaxValue + (510.0 - num4) / 2.0) / (510.0 - num4) : (num3 * (double) byte.MaxValue + num4 / 2.0) / num4;
      double num5 = ((double) (((int) num2 - (int) r) * 42) + num3 / 2.0) / num3;
      double num6 = ((double) (((int) num2 - (int) g) * 42) + num3 / 2.0) / num3;
      double num7 = ((double) (((int) num2 - (int) b) * 42) + num3 / 2.0) / num3;
      dHue = (int) r != (int) num2 ? ((int) g != (int) num2 ? 170.0 + num6 - num5 : 85.0 + num5 - num7) : num7 - num6;
      if (dHue < 0.0)
        dHue += (double) byte.MaxValue;
      if (dHue > (double) byte.MaxValue)
        dHue -= (double) byte.MaxValue;
    }
    if (dSaturation < 0.0)
      dSaturation = 0.0;
    if (dSaturation > (double) byte.MaxValue)
      dSaturation = (double) byte.MaxValue;
    if (dLuminance < 0.0)
      dLuminance = 0.0;
    if (dLuminance <= (double) byte.MaxValue)
      return;
    dLuminance = (double) byte.MaxValue;
  }

  public static Color ConvertHLSToRGB(double dHue, double dLuminance, double dSaturation)
  {
    dHue /= (double) byte.MaxValue;
    dLuminance /= (double) byte.MaxValue;
    dSaturation /= (double) byte.MaxValue;
    int red;
    int green;
    int blue;
    if (dSaturation == 0.0)
    {
      red = (int) (byte) (dLuminance * (double) byte.MaxValue);
      green = red;
      blue = red;
    }
    else
    {
      double dN2 = dLuminance >= 0.5 ? dLuminance + dSaturation - dLuminance * dSaturation : dLuminance * (1.0 + dSaturation);
      double dN1 = 2.0 * dLuminance - dN2;
      double rgb1 = Excel2007Parser.HueToRGB(dN1, dN2, dHue + 0.33);
      double rgb2 = Excel2007Parser.HueToRGB(dN1, dN2, dHue);
      double rgb3 = Excel2007Parser.HueToRGB(dN1, dN2, dHue - 0.33);
      red = (int) Math.Round((double) byte.MaxValue * rgb1);
      green = (int) Math.Round((double) byte.MaxValue * rgb2);
      blue = (int) Math.Round((double) byte.MaxValue * rgb3);
    }
    if (red < 0)
      red = 0;
    if (green < 0)
      green = 0;
    if (blue < 0)
      blue = 0;
    if (green > (int) byte.MaxValue)
      red = (int) byte.MaxValue;
    if (green > (int) byte.MaxValue)
      green = (int) byte.MaxValue;
    if (blue > (int) byte.MaxValue)
      blue = (int) byte.MaxValue;
    return Color.FromArgb(0, (int) (byte) red, (int) (byte) green, (int) (byte) blue);
  }

  public static double HueToRGB(double dN1, double dN2, double dHue)
  {
    if (dHue < 0.0)
      ++dHue;
    if (dHue > 1.0)
      --dHue;
    return dHue >= 1.0 / 6.0 ? (dHue >= 0.5 ? (dHue >= 2.0 / 3.0 ? dN1 : dN1 + (dN2 - dN1) * (2.0 / 3.0 - dHue) * 6.0) : dN2) : dN1 + (dN2 - dN1) * 6.0 * dHue;
  }

  private bool ParseBoolean(XmlReader reader, string valueAttribute, bool defaultValue)
  {
    bool boolean = defaultValue;
    if (reader.MoveToAttribute(valueAttribute))
      boolean = XmlConvertExtension.ToBoolean(reader.Value);
    return boolean;
  }

  private string ParseValue(XmlReader reader, string valueAttribute)
  {
    string str = (string) null;
    if (reader.MoveToAttribute(valueAttribute))
      str = reader.Value;
    return str;
  }

  private List<FillImpl> ParseFills(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    List<FillImpl> fills = new List<FillImpl>();
    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "fills")
    {
      reader.Read();
      if (reader.NodeType == XmlNodeType.Whitespace)
        reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          FillImpl fill = this.ParseFill(reader, true);
          fills.Add(fill);
        }
        reader.Read();
      }
    }
    return fills;
  }

  private FillImpl ParseFill(XmlReader reader, bool swapColors)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "fill")
      throw new XmlException("Wrong tag name " + reader.LocalName);
    reader.Read();
    if (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
    FillImpl fill;
    switch (reader.LocalName)
    {
      case "patternFill":
        fill = this.ParsePatternFill(reader, swapColors);
        break;
      case "gradientFill":
        fill = this.ParseGradientFill(reader);
        break;
      default:
        throw new ArgumentException("Unexpected tag  " + reader.LocalName);
    }
    if (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
    return fill;
  }

  private FillImpl ParseGradientFill(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "gradientFill")
      throw new XmlException("Unexpected tag " + reader.LocalName);
    FillImpl gradientFill = !reader.MoveToAttribute("type") || !(reader.Value == "path") ? this.ParseLinearGradientType(reader) : this.ParsePathGradientType(reader);
    reader.Skip();
    return gradientFill;
  }

  private FillImpl ParsePathGradientType(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    FillImpl pathGradientType = new FillImpl();
    pathGradientType.FillType = OfficeFillType.Gradient;
    double attributeValue1 = this.ParseAttributeValue(reader, "top");
    double attributeValue2 = this.ParseAttributeValue(reader, "bottom");
    double attributeValue3 = this.ParseAttributeValue(reader, "left");
    double attributeValue4 = this.ParseAttributeValue(reader, "right");
    if (attributeValue1 == 0.5 && attributeValue2 == 0.5 && attributeValue3 == 0.5 && attributeValue4 == 0.5)
    {
      pathGradientType.GradientStyle = OfficeGradientStyle.FromCenter;
      pathGradientType.GradientVariant = OfficeGradientVariants.ShadingVariants_1;
    }
    else if (attributeValue1 == 1.0 && attributeValue2 == 1.0 && attributeValue3 == 1.0 && attributeValue4 == 1.0)
    {
      pathGradientType.GradientStyle = OfficeGradientStyle.FromCorner;
      pathGradientType.GradientVariant = OfficeGradientVariants.ShadingVariants_4;
    }
    else if (attributeValue1 == 1.0 && attributeValue2 == 1.0)
    {
      pathGradientType.GradientStyle = OfficeGradientStyle.FromCorner;
      pathGradientType.GradientVariant = OfficeGradientVariants.ShadingVariants_3;
    }
    else if (attributeValue3 == 1.0 && attributeValue4 == 1.0)
    {
      pathGradientType.GradientStyle = OfficeGradientStyle.FromCorner;
      pathGradientType.GradientVariant = OfficeGradientVariants.ShadingVariants_2;
    }
    else if (double.IsNaN(attributeValue1) && double.IsNaN(attributeValue2) && double.IsNaN(attributeValue3) && double.IsNaN(attributeValue4))
    {
      pathGradientType.GradientStyle = OfficeGradientStyle.FromCorner;
      pathGradientType.GradientVariant = OfficeGradientVariants.ShadingVariants_1;
    }
    reader.Read();
    List<ChartColor> stopColors = this.ParseStopColors(reader);
    pathGradientType.PatternColorObject.CopyFrom(stopColors[0], true);
    pathGradientType.ColorObject.CopyFrom(stopColors[1], true);
    return pathGradientType;
  }

  private List<ChartColor> ParseStopColors(XmlReader reader)
  {
    List<ChartColor> stopColors = new List<ChartColor>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "stop")
      {
        reader.Read();
        stopColors.Add(this.ParseColor(reader));
        reader.Skip();
      }
      reader.Skip();
    }
    return stopColors;
  }

  private FillImpl ParseLinearGradientType(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    FillImpl fill = new FillImpl();
    fill.FillType = OfficeFillType.Gradient;
    double attributeValue = this.ParseAttributeValue(reader, "degree");
    reader.Read();
    List<ChartColor> stopColors = this.ParseStopColors(reader);
    fill.PatternColorObject.CopyFrom(stopColors[0], true);
    fill.ColorObject.CopyFrom(stopColors[1], true);
    if (stopColors.Count == 3)
    {
      fill.GradientVariant = OfficeGradientVariants.ShadingVariants_3;
      switch (double.IsNaN(attributeValue) ? 0 : (int) attributeValue)
      {
        case 0:
          fill.GradientStyle = OfficeGradientStyle.Vertical;
          break;
        case 45:
          fill.GradientStyle = OfficeGradientStyle.DiagonalUp;
          break;
        case 90:
          fill.GradientStyle = OfficeGradientStyle.Horizontal;
          break;
        case 135:
          fill.GradientStyle = OfficeGradientStyle.DiagonalDown;
          break;
        default:
          throw new ArgumentException("Unsupported degree value");
      }
    }
    else
      this.SetGradientStyleVariant(fill, attributeValue);
    return fill;
  }

  private void SetGradientStyleVariant(FillImpl fill, double dDegree)
  {
    switch (double.IsNaN(dDegree) ? 0 : (int) dDegree)
    {
      case 0:
        fill.GradientStyle = OfficeGradientStyle.Vertical;
        fill.GradientVariant = OfficeGradientVariants.ShadingVariants_1;
        break;
      case 45:
        fill.GradientStyle = OfficeGradientStyle.DiagonalUp;
        fill.GradientVariant = OfficeGradientVariants.ShadingVariants_1;
        break;
      case 90:
        fill.GradientStyle = OfficeGradientStyle.Horizontal;
        fill.GradientVariant = OfficeGradientVariants.ShadingVariants_1;
        break;
      case 135:
        fill.GradientStyle = OfficeGradientStyle.DiagonalDown;
        fill.GradientVariant = OfficeGradientVariants.ShadingVariants_1;
        break;
      case 180:
        fill.GradientStyle = OfficeGradientStyle.Vertical;
        fill.GradientVariant = OfficeGradientVariants.ShadingVariants_2;
        break;
      case 225:
        fill.GradientStyle = OfficeGradientStyle.DiagonalUp;
        fill.GradientVariant = OfficeGradientVariants.ShadingVariants_2;
        break;
      case 270:
        fill.GradientStyle = OfficeGradientStyle.Horizontal;
        fill.GradientVariant = OfficeGradientVariants.ShadingVariants_2;
        break;
      case 315:
        fill.GradientStyle = OfficeGradientStyle.DiagonalDown;
        fill.GradientVariant = OfficeGradientVariants.ShadingVariants_2;
        break;
      default:
        throw new ArgumentException("Unsupported degree value");
    }
  }

  private double ParseAttributeValue(XmlReader reader, string strAttributeName)
  {
    return !reader.MoveToAttribute(strAttributeName) ? double.NaN : XmlConvertExtension.ToDouble(reader.Value);
  }

  private FillImpl ParsePatternFill(XmlReader reader, bool swapColors)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "patternFill")
      throw new XmlException("Unexpected tag " + reader.LocalName);
    FillImpl patternFill = new FillImpl();
    if (reader.MoveToAttribute("patternType"))
      patternFill.Pattern = Excel2007Parser.ConvertStringToPattern(reader.Value);
    if (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
    reader.Read();
    ChartColor chartColor1 = (ChartColor) null;
    ChartColor chartColor2 = (ChartColor) null;
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "fill")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "bgColor":
            chartColor1 = new ChartColor(OfficeKnownColors.BlackCustom);
            this.ParseFillColor(reader, chartColor1);
            break;
          case "fgColor":
            chartColor2 = new ChartColor(OfficeKnownColors.White | OfficeKnownColors.BlackCustom);
            this.ParseColor(reader, chartColor2);
            break;
        }
      }
      reader.Read();
    }
    if (reader.LocalName == "patternFill")
      reader.Read();
    if (swapColors && patternFill.Pattern != OfficePattern.Solid)
    {
      ChartColor chartColor3 = chartColor2;
      chartColor2 = chartColor1;
      chartColor1 = chartColor3;
    }
    if (chartColor2 != (ChartColor) null)
      patternFill.ColorObject.CopyFrom(chartColor2, true);
    else if (chartColor1 == (ChartColor) null || patternFill.Pattern != OfficePattern.Solid)
      patternFill.ColorObject.SetIndexed(OfficeKnownColors.White | OfficeKnownColors.BlackCustom);
    if (chartColor1 != (ChartColor) null)
      patternFill.PatternColorObject.CopyFrom(chartColor1, true);
    else if (chartColor2 == (ChartColor) null || patternFill.Pattern != OfficePattern.Solid)
      patternFill.PatternColorObject.SetIndexed(OfficeKnownColors.BlackCustom);
    return patternFill;
  }

  private static OfficePattern ConvertStringToPattern(string value)
  {
    return (OfficePattern) Enum.Parse(typeof (Excel2007Pattern), value, true);
  }

  private List<BordersCollection> ParseBorders(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    List<BordersCollection> borders = new List<BordersCollection>();
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "borders")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        BordersCollection bordersCollection = reader.LocalName == "border" ? this.ParseBordersCollection(reader) : throw new XmlException("Unexpected xml tag " + reader.LocalName);
        borders.Add(bordersCollection);
      }
      else
        reader.Read();
    }
    return borders;
  }

  private BordersCollection ParseBordersCollection(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "border")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    BordersCollection bordersCollection = new BordersCollection(this.m_book.Application, (object) this.m_book, true);
    bool flag1 = false;
    bool flag2 = false;
    if (reader.MoveToAttribute("diagonalUp"))
      flag1 = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("diagonalDown"))
      flag2 = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.IsEmptyElement)
    {
      reader.Read();
    }
    else
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          Excel2007BorderIndex borderIndex;
          BorderSettingsHolder border = this.ParseBorder(reader, out borderIndex);
          if (borderIndex == Excel2007BorderIndex.diagonal)
          {
            BorderSettingsHolder borderSettingsHolder = (BorderSettingsHolder) border.Clone();
            border.ShowDiagonalLine = flag1;
            borderSettingsHolder.ShowDiagonalLine = flag2;
            bordersCollection.SetBorder(OfficeBordersIndex.DiagonalUp, (IBorder) border);
            bordersCollection.SetBorder(OfficeBordersIndex.DiagonalDown, (IBorder) borderSettingsHolder);
          }
          else
          {
            OfficeBordersIndex index = (OfficeBordersIndex) borderIndex;
            bordersCollection.SetBorder(index, (IBorder) border);
          }
          if (bordersCollection.IsEmptyBorder && border != null && !border.IsEmptyBorder)
            bordersCollection.IsEmptyBorder = false;
        }
        reader.Read();
      }
      reader.Read();
    }
    return bordersCollection;
  }

  private BorderSettingsHolder ParseBorder(XmlReader reader, out Excel2007BorderIndex borderIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    borderIndex = reader.NodeType == XmlNodeType.Element ? (Excel2007BorderIndex) Enum.Parse(typeof (Excel2007BorderIndex), reader.LocalName, true) : throw new XmlException("Unexpected node type " + (object) reader.NodeType);
    BorderSettingsHolder border = new BorderSettingsHolder();
    bool flag = false;
    if (reader.IsEmptyElement)
      flag = true;
    if (reader.MoveToAttribute("style"))
    {
      Excel2007BorderLineStyle excel2007BorderLineStyle = (Excel2007BorderLineStyle) Enum.Parse(typeof (Excel2007BorderLineStyle), reader.Value, true);
      border.LineStyle = (OfficeLineStyle) excel2007BorderLineStyle;
      border.IsEmptyBorder = false;
    }
    if (!flag && !reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName == "color")
      {
        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "color")
          this.ParseColor(reader, border.ColorObject);
        reader.Read();
      }
    }
    return border;
  }

  private int ParseRow(
    XmlReader reader,
    IInternalWorksheet sheet,
    List<int> arrStyles,
    string cellTag,
    int generatedRowIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "row")
      throw new XmlException(nameof (reader));
    int num1 = this.m_book.DefaultXFIndex;
    int num2 = sheet.DefaultRowHeight;
    bool flag1 = false;
    int num3;
    if (reader.MoveToAttribute("r"))
    {
      generatedRowIndex = num3 = XmlConvertExtension.ToInt32(reader.Value);
      if (num3 == 0)
        generatedRowIndex = num3 = 1;
    }
    else
      num3 = generatedRowIndex;
    RowStorage row = WorksheetHelper.GetOrCreateRow(sheet, num3 - 1, true);
    List<int> cellStyleIndex = new List<int>();
    if (reader.MoveToAttribute("collapsed"))
      row.IsCollapsed = XmlConvertExtension.ToBoolean(reader.Value);
    bool flag2 = reader.MoveToAttribute("customFormat") && XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("customHeight"))
      row.IsBadFontHeight = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("outlineLevel"))
    {
      row.OutlineLevel = XmlConvertExtension.ToUInt16(reader.Value);
      this.m_indexAndLevels.Add(generatedRowIndex, (int) row.OutlineLevel);
    }
    if (reader.MoveToAttribute("ht"))
    {
      double num4 = XmlConvertExtension.ToDouble(reader.Value);
      if (num4 > 409.5)
        num4 = 409.5;
      num2 = (int) (num4 * 20.0);
      row.HasRowHeight = true;
    }
    if (reader.MoveToAttribute("s") && arrStyles.Count > XmlConvertExtension.ToInt32(reader.Value))
      num1 = arrStyles[XmlConvertExtension.ToInt32(reader.Value)];
    if (reader.MoveToAttribute("hidden"))
    {
      row.IsHidden = XmlConvertExtension.ToBoolean(reader.Value);
      row.IsCollapsed = XmlConvertExtension.ToBoolean(reader.Value);
    }
    else
      row.IsCollapsed = false;
    if (reader.MoveToAttribute("thickBot"))
      row.IsSpaceBelowRow = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("thickTop"))
      row.IsSpaceAboveRow = XmlConvertExtension.ToBoolean(reader.Value);
    row.ExtendedFormatIndex = (ushort) num1;
    row.Height = (ushort) num2;
    if (sheet.FirstRow < 0 || sheet.FirstRow > num3)
      sheet.FirstRow = num3;
    if (sheet.LastRow < num3)
      sheet.LastRow = num3;
    reader.MoveToElement();
    int num5 = 1;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == cellTag)
          num5 = this.ParseCell(reader, sheet, arrStyles, num3, num5, cellStyleIndex) + 1;
        reader.Skip();
      }
    }
    else if (row.IsHidden)
      sheet.CellRecords.SetBlank(num3, num5, this.m_book.DefaultXFIndex);
    foreach (int index in cellStyleIndex)
    {
      if (!flag1)
        flag1 = this.m_book.GetExtFormat(index).WrapText;
      else
        break;
    }
    row.IsFormatted = flag2;
    row.IsWrapText = flag1;
    return generatedRowIndex;
  }

  private int ParseCell(
    XmlReader reader,
    IInternalWorksheet sheet,
    List<int> arrStyles,
    int rowIndex,
    int columnIndex,
    List<int> cellStyleIndex)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    bool flag1 = false;
    int iRow = rowIndex;
    int iColumn = columnIndex;
    int iXFIndex = this.m_book.DefaultXFIndex;
    Excel2007Serializator.CellType cellType = Excel2007Serializator.CellType.n;
    CellRecordCollection cellRecords = sheet.CellRecords;
    if (reader.MoveToAttribute("r"))
      RangeImpl.CellNameToRowColumn(reader.Value, out iRow, out iColumn);
    if (reader.MoveToAttribute("s"))
    {
      iXFIndex = arrStyles[XmlConvertExtension.ToInt32(reader.Value)];
      if (!cellStyleIndex.Contains(iXFIndex))
        cellStyleIndex.Add(iXFIndex);
    }
    if (reader.MoveToAttribute("t"))
      cellType = Excel2007Parser.GetCellType(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "is")
        {
          string text = string.Empty;
          int stringItem = this.ParseStringItem(reader, out text);
          SSTDictionary innerSst = this.m_book.InnerSST;
          cellRecords.SetSingleStringValue(iRow, iColumn, iXFIndex, stringItem);
          if (text != null)
          {
            this.Worksheet.InlineStrings.Add(RangeImpl.GetCellName(iColumn, iRow), text);
            if (!this.m_book.HasInlineStrings)
              this.m_book.HasInlineStrings = true;
          }
          if (stringItem == 1 || stringItem == 2)
            innerSst.RemoveDecrease(stringItem);
          flag1 = true;
        }
        if (reader.LocalName == "f")
        {
          this.ParseFormula(reader, sheet, iRow, iColumn, iXFIndex, cellType);
          flag1 = true;
        }
        if (reader.LocalName == "is")
          reader.Skip();
        if (reader.NodeType != XmlNodeType.EndElement)
        {
          if (reader.LocalName == "v")
          {
            bool flag2 = reader.IsEmptyElement;
            if (!flag2)
              reader.Read();
            if (reader.NodeType == XmlNodeType.EndElement)
            {
              flag2 = true;
            }
            else
            {
              if (WorksheetHelper.HasFormulaRecord(sheet, iRow, iColumn))
                this.SetFormulaValue(sheet, cellType, reader.Value, iRow, iColumn);
              else
                this.SetCellRecord(cellType, reader.Value, cellRecords, iRow, iColumn, iXFIndex);
              flag1 = true;
            }
            if (!flag2)
              reader.Skip();
          }
          reader.Skip();
        }
      }
    }
    if (!flag1)
    {
      if (cellRecords.Sheet is ExternWorksheetImpl)
        this.SetCellRecord(Excel2007Serializator.CellType.n, "0", cellRecords, iRow, iColumn, iXFIndex);
      else
        cellRecords.SetBlank(iRow, iColumn, iXFIndex);
    }
    return iColumn;
  }

  private static Excel2007Serializator.CellType GetCellType(string cellType)
  {
    switch (cellType)
    {
      case "b":
        return Excel2007Serializator.CellType.b;
      case "e":
        return Excel2007Serializator.CellType.e;
      case "inlineStr":
        return Excel2007Serializator.CellType.inlineStr;
      case "n":
        return Excel2007Serializator.CellType.n;
      case "s":
        return Excel2007Serializator.CellType.s;
      case "str":
        return Excel2007Serializator.CellType.str;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  private void ParseFormula(
    XmlReader reader,
    IInternalWorksheet sheet,
    int iRow,
    int iCol,
    int iXFIndex,
    Excel2007Serializator.CellType cellType)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "f")
      throw new XmlException(nameof (reader));
    string strCellsRange = (string) null;
    string strFormulaString = "=";
    uint uiSharedGroupIndex = 0;
    Excel2007Serializator.FormulaType formulaType = Excel2007Serializator.FormulaType.normal;
    CellRecordCollection cellRecords = sheet.CellRecords;
    bool bCalculateOnOpen = false;
    if (reader.MoveToAttribute("t"))
      formulaType = (Excel2007Serializator.FormulaType) Enum.Parse(typeof (Excel2007Serializator.FormulaType), reader.Value, false);
    if (reader.MoveToAttribute("si"))
      uiSharedGroupIndex = XmlConvertExtension.ToUInt32(reader.Value);
    if (reader.MoveToAttribute("ref"))
      strCellsRange = reader.Value;
    if (reader.MoveToAttribute("ca"))
      bCalculateOnOpen = XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      if (reader.NodeType != XmlNodeType.EndElement)
      {
        strFormulaString += reader.Value;
        reader.Skip();
      }
    }
    switch (formulaType)
    {
      case Excel2007Serializator.FormulaType.array:
        this.SetArrayFormula(sheet as WorksheetImpl, strFormulaString, strCellsRange, iXFIndex);
        break;
      case Excel2007Serializator.FormulaType.normal:
        string strFormula = UtilityMethods.RemoveFirstCharUnsafe(strFormulaString);
        if (strFormula.Length > 0)
        {
          FormulaRecord record = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
          if (!strFormula.StartsWith("[") || cellType != Excel2007Serializator.CellType.e)
            record.ParsedExpression = this.m_formulaUtil.ParseString(strFormula, (IWorksheet) sheet, (Dictionary<string, string>) null);
          else
            (sheet as WorksheetImpl).m_formulaString = strFormula;
          record.Row = iRow - 1;
          record.Column = iCol - 1;
          record.ExtendedFormatIndex = (ushort) iXFIndex;
          record.CalculateOnOpen = bCalculateOnOpen;
          cellRecords.SetCellRecord(iRow, iCol, (ICellPositionFormat) record);
          break;
        }
        break;
      case Excel2007Serializator.FormulaType.shared:
        this.SetSharedFormula(sheet as WorksheetImpl, strFormulaString, strCellsRange, uiSharedGroupIndex, iRow, iCol, iXFIndex, bCalculateOnOpen);
        break;
    }
    reader.Skip();
  }

  private void ParsePalette(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "indexedColors")
      throw new XmlException("Cannot locate tag indexedColors");
    reader.Read();
    int index = 0;
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "indexedColors")
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "rgbColor")
      {
        reader.MoveToAttribute("rgb");
        Color color = ColorExtension.FromArgb(int.Parse(reader.Value, NumberStyles.HexNumber));
        this.m_book.SetPaletteColor(index, color);
        ++index;
      }
      reader.Read();
    }
  }

  private void ParseColors(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "colors")
      throw new XmlException("Cannot locate tag colors");
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "indexedColors")
        this.ParsePalette(reader);
      reader.Read();
    }
  }

  private void ParseColumns(XmlReader reader, WorksheetImpl sheet, List<int> arrStyles)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (arrStyles == null)
      throw new ArgumentNullException(nameof (arrStyles));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "cols")
      throw new XmlException("Unable to locate xml tag cols");
    if (this.m_outlineLevels == null)
      this.m_outlineLevels = new Dictionary<int, List<Point>>();
    if (this.m_indexAndLevels == null)
      this.m_indexAndLevels = new Dictionary<int, int>();
    if (sheet != null && sheet.ColumnOutlineLevels == null)
      sheet.ColumnOutlineLevels = new Dictionary<int, List<Point>>();
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "cols")
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "col")
        this.ParseColumn(reader, sheet, arrStyles);
      reader.Read();
    }
    if (sheet == null || this.m_indexAndLevels == null || this.m_indexAndLevels.Count <= 0)
      return;
    this.m_indexAndLevels.Clear();
    this.m_outlineLevels = (Dictionary<int, List<Point>>) null;
  }

  private void ParseColumn(XmlReader reader, WorksheetImpl sheet, List<int> arrStyles)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (arrStyles == null)
      throw new ArgumentNullException(nameof (arrStyles));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "col")
      throw new XmlException("Unable to locate xml tag col");
    ColumnInfoRecord record = (ColumnInfoRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ColumnInfo);
    if (reader.MoveToAttribute("min"))
      record.FirstColumn = (ushort) ((uint) XmlConvertExtension.ToUInt16(reader.Value) - 1U);
    if (reader.MoveToAttribute("max"))
      record.LastColumn = (ushort) ((uint) XmlConvertExtension.ToUInt16(reader.Value) - 1U);
    if (reader.MoveToAttribute("width"))
    {
      int num = (int) Math.Round(XmlConvertExtension.ToDouble(reader.Value) * 256.0);
      record.ColumnWidth = (ushort) num;
    }
    if (reader.MoveToAttribute("style"))
    {
      int index = int.Parse(reader.Value);
      int arrStyle = arrStyles[index];
      record.ExtendedFormatIndex = (ushort) arrStyle;
    }
    else
      record.ExtendedFormatIndex = (ushort) this.m_book.DefaultXFIndex;
    if (reader.MoveToAttribute("bestFit"))
      record.IsBestFit = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("phonetic"))
      record.IsPhenotic = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("customWidth"))
      record.IsUserSet = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("collapsed"))
      record.IsCollapsed = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("outlineLevel"))
    {
      record.OutlineLevel = XmlConvertExtension.ToUInt16(reader.Value);
      for (int firstColumn = (int) record.FirstColumn; firstColumn <= (int) record.LastColumn; ++firstColumn)
        this.m_indexAndLevels.Add(firstColumn + 1, (int) record.OutlineLevel);
    }
    if (reader.MoveToAttribute("hidden"))
    {
      record.IsHidden = XmlConvertExtension.ToBoolean(reader.Value);
      record.IsCollapsed = XmlConvertExtension.ToBoolean(reader.Value);
    }
    else
      record.IsCollapsed = false;
    sheet.ParseColumnInfo(record, false);
  }

  private int ParseColumnInfoRecord(XmlReader xmlTextReader, WorksheetImpl worksheet, int index)
  {
    if (!xmlTextReader.HasAttributes)
    {
      xmlTextReader.Skip();
      return index;
    }
    int minCol = -1;
    int num1 = -1;
    int num2 = -1;
    short outLineLevel = -1;
    double width1 = -1.0;
    bool flag1 = false;
    bool flag2 = false;
    bool isCollapsed = false;
    bool isBestFit = false;
    xmlTextReader.MoveToElement();
    while (xmlTextReader.MoveToNextAttribute())
    {
      string localName = xmlTextReader.LocalName;
      if (localName != null)
      {
        if (Helper.columnAttributes == null)
          Helper.columnAttributes = new Dictionary<string, int>(9)
          {
            {
              "min",
              0
            },
            {
              "max",
              1
            },
            {
              "width",
              2
            },
            {
              "style",
              3
            },
            {
              "hidden",
              4
            },
            {
              "customWidth",
              5
            },
            {
              "outlineLevel",
              6
            },
            {
              "collapsed",
              7
            },
            {
              "bestFit",
              8
            }
          };
        int num3;
        if (Helper.columnAttributes.TryGetValue(localName, out num3))
        {
          switch (num3)
          {
            case 0:
              minCol = Helper.ParseInt(xmlTextReader.Value) - 1;
              num1 = minCol;
              continue;
            case 1:
              num1 = Helper.ParseInt(xmlTextReader.Value) - 1;
              continue;
            case 2:
              width1 = Helper.ParseDouble(xmlTextReader.Value);
              flag1 = true;
              continue;
            case 3:
              num2 = Helper.ParseInt(xmlTextReader.Value);
              continue;
            case 4:
              flag2 = Helper.ParseBoolen(xmlTextReader.Value);
              continue;
            case 6:
              outLineLevel = Helper.ParseShort(xmlTextReader.Value);
              continue;
            case 7:
              isCollapsed = Helper.ParseBoolen(xmlTextReader.Value);
              continue;
            case 8:
              isBestFit = Helper.ParseBoolen(xmlTextReader.Value);
              continue;
            default:
              continue;
          }
        }
      }
    }
    xmlTextReader.MoveToElement();
    ColumnCollection columnss = worksheet.Columnss;
    double width2 = columnss.Width;
    double num4 = flag1 ? worksheet.CharacterWidth(width1) : width2;
    int styleIndex = 15;
    if (num2 != -1)
    {
      object obj = (object) null;
      if (obj != null)
        styleIndex = (int) obj;
    }
    for (int index1 = minCol; index1 <= num1; ++index1)
    {
      Column column = (Column) null;
      if (num1 >= 16383 /*0x3FFF*/)
      {
        column = columnss.GetOrCreateColumn();
        column.SetMinColumnIndex(minCol);
      }
      else if (minCol >= index)
        column = columnss.AddColumn(index1);
      if (flag1)
        column.Width = num4;
      column.SetStyleIndex(styleIndex);
      if (outLineLevel != (short) -1)
        column.SetOutLineLevel((byte) outLineLevel);
      column.IsHidden = flag2;
      column.SetCollapsedInfo(isCollapsed);
      column.SetBestFitInfo(isBestFit);
      if (num1 >= 16383 /*0x3FFF*/)
        break;
    }
    return index <= num1 ? num1 : index;
  }

  private void SwitchStreams(
    ref bool bAdd,
    ref XmlWriter writer,
    ref StreamWriter textWriter,
    Stream streamEnd)
  {
    if (bAdd)
      return;
    bAdd = true;
    writer.WriteEndElement();
    writer.Flush();
    textWriter = new StreamWriter(streamEnd);
    writer = UtilityMethods.CreateWriter((TextWriter) textWriter);
    writer.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
  }

  public List<Color> ParseThemes(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "theme")
      throw new XmlException(nameof (reader));
    if (reader.IsEmptyElement)
      return (List<Color>) null;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "themeElements")
        this.m_lstThemeColors = this.ParseThemeElements(reader);
      reader.Skip();
    }
    this.m_book.MajorFonts = this.m_dicMajorFonts;
    this.m_book.MinorFonts = this.m_dicMinorFonts;
    return this.m_lstThemeColors;
  }

  public List<Color> ParseThemeElements(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.IsEmptyElement)
      return (List<Color>) null;
    List<Color> themeElements = new List<Color>();
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "clrScheme":
            Dictionary<string, Color> dicThemeColors = (Dictionary<string, Color>) null;
            themeElements = this.ParseThemeColors(reader, out dicThemeColors);
            this.m_dicThemeColors = dicThemeColors;
            continue;
          case "fontScheme":
            this.ParseFontScheme(reader);
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
    return themeElements;
  }

  private void ParseFontScheme(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "majorFont":
            this.ParseMajorFont(reader, out this.m_dicMajorFonts);
            continue;
          case "minorFont":
            this.ParseMinorFont(reader, out this.m_dicMinorFonts);
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

  private void ParseMinorFont(XmlReader reader, out Dictionary<string, FontImpl> dicMinorFonts)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    dicMinorFonts = new Dictionary<string, FontImpl>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "latin":
            FontImpl font1 = this.GetFont(reader);
            dicMinorFonts.Add("latin", font1);
            continue;
          case "ea":
            FontImpl font2 = this.GetFont(reader);
            dicMinorFonts.Add("ea", font2);
            continue;
          case "cs":
            FontImpl font3 = this.GetFont(reader);
            dicMinorFonts.Add("cs", font3);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private void ParseMajorFont(XmlReader reader, out Dictionary<string, FontImpl> dicMajorFonts)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    dicMajorFonts = new Dictionary<string, FontImpl>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "latin":
            FontImpl font1 = this.GetFont(reader);
            dicMajorFonts.Add("lt", font1);
            continue;
          case "ea":
            FontImpl font2 = this.GetFont(reader);
            dicMajorFonts.Add("ea", font2);
            continue;
          case "cs":
            FontImpl font3 = this.GetFont(reader);
            dicMajorFonts.Add("cs", font3);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  public FontImpl GetFont(XmlReader reader)
  {
    FontImpl font = (FontImpl) null;
    if (reader.MoveToAttribute("typeface"))
    {
      font = (FontImpl) this.m_book.CreateFont((IOfficeFont) null, false);
      font.FontName = reader.Value;
    }
    return font;
  }

  public static void SkipWhiteSpaces(XmlReader reader)
  {
    while (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
  }

  public List<Color> ParseThemeColors(
    XmlReader reader,
    out Dictionary<string, Color> dicThemeColors)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    dicThemeColors = (Dictionary<string, Color>) null;
    if (reader.IsEmptyElement)
      return (List<Color>) null;
    reader.Read();
    List<Color> themeColors = new List<Color>();
    dicThemeColors = new Dictionary<string, Color>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        string localName = reader.LocalName;
        reader.Read();
        Excel2007Parser.SkipWhiteSpaces(reader);
        Color color = ColorExtension.Black;
        if (reader.LocalName == "srgbClr")
        {
          if (reader.MoveToAttribute("val"))
            color = ColorExtension.FromArgb(int.Parse(reader.Value, NumberStyles.HexNumber));
          reader.Skip();
          Excel2007Parser.SkipWhiteSpaces(reader);
        }
        else if (reader.LocalName == "sysClr")
        {
          if (reader.MoveToAttribute("val"))
            color = ColorExtension.FromName(reader.Value);
          reader.Skip();
          Excel2007Parser.SkipWhiteSpaces(reader);
        }
        themeColors.Add(color);
        dicThemeColors.Add(localName, color);
        reader.Skip();
      }
      else
        reader.Skip();
    }
    themeColors.Reverse(0, 2);
    themeColors.Reverse(2, 2);
    dicThemeColors.Add("tx1", themeColors[1]);
    dicThemeColors.Add("tx2", themeColors[3]);
    reader.Read();
    return themeColors;
  }

  public List<DxfImpl> ParseDxfCollection(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "dxfs")
      throw new XmlException(nameof (reader));
    List<DxfImpl> dxfCollection = new List<DxfImpl>();
    reader.Read();
    if (reader.NodeType != XmlNodeType.None)
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "dxf")
          dxfCollection.Add(this.ParseDxfStyle(reader));
        else
          reader.Skip();
      }
    }
    return dxfCollection;
  }

  private DxfImpl ParseDxfStyle(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    DxfImpl dxfStyle = new DxfImpl();
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      switch (reader.LocalName)
      {
        case "numFmt":
          dxfStyle.FormatRecord = this.ParseDxfNumberFormat(reader);
          reader.Skip();
          continue;
        case "fill":
          dxfStyle.Fill = this.ParseFill(reader, false);
          reader.Skip();
          continue;
        case "font":
          dxfStyle.Font = this.ParseFont(reader);
          reader.Skip();
          continue;
        case "border":
          dxfStyle.Borders = this.ParseBordersCollection(reader);
          continue;
        default:
          reader.Skip();
          continue;
      }
    }
    reader.Read();
    return dxfStyle;
  }

  private void UpdateCFRange(string address, WorksheetImpl worksheet)
  {
    if (this.m_book.IsWorkbookOpening || this.m_book.Saving || !worksheet.UsedRangeIncludesCF)
      return;
    string str = address;
    char[] chArray = new char[1]{ ' ' };
    foreach (string name in str.Split(chArray))
    {
      IRange range = worksheet.Range[name];
      if (this.minRow > range.Row || this.minRow == 0)
        this.minRow = range.Row;
      if (this.minColumn > range.Column || this.minColumn == 0)
        this.minColumn = range.Column;
      if (this.maxColumn < range.LastColumn || this.maxColumn == 0)
        this.maxColumn = range.LastColumn;
      if (this.maxRow < range.LastRow || this.maxRow == 0)
        this.maxRow = range.LastRow;
    }
  }

  public static void ParsePrintOptions(XmlReader reader, IPageSetupBase pageSetup)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    if (reader.LocalName != "printOptions")
      throw new XmlException("Unexpected xml tag.");
    if (pageSetup is PageSetupImpl pageSetupImpl1)
    {
      pageSetupImpl1.PrintGridlines = reader.MoveToAttribute("gridLines") && XmlConvertExtension.ToBoolean(reader.Value);
      PageSetupImpl pageSetupImpl = pageSetupImpl1;
      pageSetupImpl.PrintGridlines = ((pageSetupImpl.PrintGridlines ? 1 : 0) | (reader.MoveToAttribute("gridLinesSet") ? (XmlConvertExtension.ToBoolean(reader.Value) ? 1 : 0) : 0)) != 0;
      pageSetupImpl1.PrintHeadings = reader.MoveToAttribute("headings") && XmlConvertExtension.ToBoolean(reader.Value);
    }
    pageSetup.CenterHorizontally = reader.MoveToAttribute("horizontalCentered") && XmlConvertExtension.ToBoolean(reader.Value);
    pageSetup.CenterVertically = reader.MoveToAttribute("verticalCentered") && XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
  }

  public static void ParsePageMargins(
    XmlReader reader,
    IPageSetupBase pageSetup,
    IPageSetupConstantsProvider constants)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    if (reader.LocalName != constants.PageMarginsTag)
      throw new XmlException("Unexpected xml tag.");
    if (reader.MoveToAttribute(constants.LeftMargin))
      pageSetup.LeftMargin = XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute(constants.RightMargin))
      pageSetup.RightMargin = XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute(constants.TopMargin))
      pageSetup.TopMargin = XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute(constants.BottomMargin))
      pageSetup.BottomMargin = XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute(constants.HeaderMargin))
      pageSetup.HeaderMargin = XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute(constants.FooterMargin))
      pageSetup.FooterMargin = XmlConvertExtension.ToDouble(reader.Value);
    reader.MoveToElement();
    reader.Skip();
  }

  public static void ParsePageSetup(XmlReader reader, PageSetupBaseImpl pageSetup)
  {
    if (reader == null)
      throw new ArgumentNullException("writer");
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    if (reader.LocalName != nameof (pageSetup))
      throw new XmlException("Unexpected xml tag.");
    pageSetup.PaperSize = !reader.MoveToAttribute("paperSize") ? OfficePaperSize.PaperLetter : (OfficePaperSize) XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("scale"))
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      int num1 = int32 > 400 ? 400 : int32;
      int num2 = num1 < 10 ? 10 : num1;
      pageSetup.Zoom = num2;
    }
    else
      pageSetup.Zoom = 100;
    if (reader.MoveToAttribute("firstPageNumber"))
    {
      uint uint32 = XmlConvertExtension.ToUInt32(reader.Value);
      pageSetup.FirstPageNumber = (short) uint32;
    }
    if (reader.MoveToAttribute("fitToWidth"))
      pageSetup.FitToPagesWide = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("fitToHeight"))
      pageSetup.FitToPagesTall = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("pageOrder"))
      pageSetup.Order = (OfficeOrder) Enum.Parse(typeof (OfficeOrder), reader.Value, true);
    if (reader.MoveToAttribute("orientation") && (reader.Value.ToUpper() == "LANDSCAPE" || reader.Value.ToUpper() == "PORTRAIT"))
      pageSetup.Orientation = (OfficePageOrientation) Enum.Parse(typeof (OfficePageOrientation), reader.Value, true);
    if (reader.MoveToAttribute("blackAndWhite"))
      pageSetup.BlackAndWhite = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("draft"))
      pageSetup.Draft = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("cellComments"))
      pageSetup.PrintComments = Excel2007Parser.StringToPrintComments(reader.Value);
    if (reader.MoveToAttribute("useFirstPageNumber"))
      pageSetup.AutoFirstPageNumber = !XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("errors"))
      pageSetup.PrintErrors = Excel2007Parser.StringToPrintErrors(reader.Value);
    if (reader.MoveToAttribute("horizontalDpi"))
      pageSetup.HResolution = Excel2007Parser.GetParsedXmlValue(reader.Value);
    if (reader.MoveToAttribute("verticalDpi"))
      pageSetup.VResolution = Excel2007Parser.GetParsedXmlValue(reader.Value);
    if (reader.MoveToAttribute("copies"))
      pageSetup.Copies = XmlConvertExtension.ToInt32(reader.Value);
    if (pageSetup is PageSetupImpl pageSetupImpl && reader.MoveToAttribute("id"))
      pageSetupImpl.RelationId = reader.Value;
    reader.MoveToElement();
    reader.Skip();
  }

  public static void ParseHeaderFooter(XmlReader reader, PageSetupBaseImpl pageSetup)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    if (reader.LocalName != "headerFooter")
      throw new XmlException("Unexpected xml tag.");
    pageSetup.AlignHFWithPageMargins = true;
    if (reader.MoveToAttribute("scaleWithDoc"))
      pageSetup.HFScaleWithDoc = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("alignWithMargins"))
      pageSetup.AlignHFWithPageMargins = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("differentFirst"))
      pageSetup.DifferentFirstPageHF = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("differentOddEven"))
      pageSetup.DifferentOddAndEvenPagesHF = XmlConvertExtension.ToBoolean(reader.Value);
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
            case "oddHeader":
              string str1 = reader.ReadElementContentAsString();
              pageSetup.FullHeaderString = str1;
              continue;
            case "oddFooter":
              string str2 = reader.ReadElementContentAsString();
              pageSetup.FullFooterString = str2;
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

  private static OfficePrintLocation StringToPrintComments(string printLocation)
  {
    switch (printLocation)
    {
      case "asDisplayed":
        return OfficePrintLocation.PrintInPlace;
      case "none":
        return OfficePrintLocation.PrintNoComments;
      case "atEnd":
        return OfficePrintLocation.PrintSheetEnd;
      default:
        throw new ArgumentOutOfRangeException(nameof (printLocation));
    }
  }

  private static OfficePrintErrors StringToPrintErrors(string printErrors)
  {
    switch (printErrors)
    {
      case "blank":
        return OfficePrintErrors.PrintErrorsBlank;
      case "dash":
        return OfficePrintErrors.PrintErrorsDash;
      case "displayed":
        return OfficePrintErrors.PrintErrorsDisplayed;
      case "NA":
        return OfficePrintErrors.PrintErrorsNA;
      default:
        throw new ArgumentOutOfRangeException("printLocation");
    }
  }

  private void ParseHyperlinks(XmlReader reader, WorksheetImpl sheet)
  {
  }

  private void ParseSheetLevelProperties(XmlReader reader, WorksheetBaseImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.MoveToAttribute("codeName"))
      sheet.CodeName = reader.Value;
    if (reader.MoveToAttribute("transitionEvaluation"))
      sheet.IsTransitionEvaluation = XmlConvertExtension.ToBoolean(reader.Value);
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
            case "tabColor":
              this.ParseColor(reader, sheet.TabColorObject);
              continue;
            case "outlinePr":
              this.ParseOutlineProperites(reader, sheet.PageSetupBase as IPageSetup);
              continue;
            case "pageSetUpPr":
              this.ParsePageSetupProperties(reader, sheet.PageSetupBase as IPageSetup);
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

  private void ParsePageSetupProperties(XmlReader reader, IPageSetup pageSetup)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    if (reader.LocalName != "pageSetUpPr")
      throw new XmlException();
    if (reader.MoveToAttribute("fitToPage"))
      pageSetup.IsFitToPage = XmlConvertExtension.ToBoolean(reader.Value);
    reader.Read();
  }

  private void ParseOutlineProperites(XmlReader reader, IPageSetup pageSetup)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (pageSetup == null)
      throw new ArgumentNullException(nameof (pageSetup));
    if (reader.MoveToAttribute("summaryBelow"))
      pageSetup.IsSummaryRowBelow = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("summaryRight"))
      pageSetup.IsSummaryColumnRight = XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToElement();
    reader.Skip();
  }

  private void ParseBackgroundImage(XmlReader reader, WorksheetImpl sheet, string strParentPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (strParentPath == null)
      throw new ArgumentNullException(nameof (strParentPath));
    if (reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
    {
      string id = reader.Value;
      WorksheetDataHolder dataHolder = sheet.DataHolder;
      RelationCollection relations = dataHolder.Relations;
      Relation relation = relations[id];
      FileDataHolder parentHolder = dataHolder.ParentHolder;
      ZipArchiveItem zipArchiveItem = parentHolder[relation, strParentPath];
      Stream stream = (Stream) new MemoryStream();
      ((MemoryStream) zipArchiveItem.DataStream).WriteTo(stream);
      sheet.PageSetup.BackgoundImage = (Bitmap) Image.FromStream(stream);
      relations.Remove(id);
      parentHolder.Archive.RemoveItem(zipArchiveItem.ItemName);
    }
    reader.Skip();
  }

  public string ParseItemProperties(XmlReader reader, ref List<string> schemas)
  {
    string itemProperties = (string) null;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    List<string> stringList = new List<string>();
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "datastoreItem":
            if (reader.MoveToAttribute("ds:itemID"))
              itemProperties = reader.Value;
            reader.Read();
            continue;
          case "schemaRefs":
            this.ParseschemaReference(reader, ref schemas);
            reader.Read();
            continue;
          default:
            continue;
        }
      }
    }
    return itemProperties;
  }

  private void ParseschemaReference(XmlReader reader, ref List<string> schemas)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "schemaRefs")
      throw new XmlException("Wrong xml tag");
    schemas = new List<string>();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      switch (reader.LocalName)
      {
        case "schemaRef":
          this.ParseSchemaRef(reader, ref schemas);
          continue;
        default:
          reader.Skip();
          continue;
      }
    }
  }

  private void ParseSchemaRef(XmlReader reader, ref List<string> schemas)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "schemaRef")
      throw new XmlException("Wrong xml tag");
    if (reader.MoveToAttribute("ds:uri"))
    {
      string str = reader.Value;
      schemas.Add(str);
    }
    reader.Read();
  }

  internal bool ParseExternalLink(XmlReader reader, RelationCollection relations)
  {
    bool externalLink = true;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "externalLink")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    switch (reader.LocalName)
    {
      case "externalBook":
        this.ParseExternalWorkbook(reader, relations);
        break;
      case "oleLink":
        this.ParseOleObjectLink(reader, relations);
        break;
      case "ddeLink":
        reader.Skip();
        externalLink = false;
        break;
      default:
        throw new XmlException("Unsupported xml tag");
    }
    return externalLink;
  }

  private void ParseOleObjectLink(XmlReader reader, RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "oleLink")
      throw new XmlException("Unexpected xml tag.");
    string str1 = (string) null;
    if (reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      str1 = reader.Value;
    ExternWorkbookImpl externBook = this.CreateExternBook(relations, str1, (List<string>) null);
    int index = externBook.ExternNames.Add("'");
    if (reader.MoveToAttribute("progId"))
      externBook.ProgramId = reader.Value;
    ExternNameRecord record = externBook.ExternNames[index].Record;
    record.OleLink = true;
    record.Ole = false;
    record.WantPicture = true;
    record.WantAdvise = true;
    record.BuiltIn = false;
    string str2 = Uri.UnescapeDataString(relations[str1].Target);
    if (str2.StartsWith("file:///"))
      str2 = str2.Substring("file:///".Length);
    externBook.URL = str2;
    reader.Skip();
  }

  private void ParseExternalWorkbook(XmlReader reader, RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "externalBook")
      throw new XmlException("Unexpected xml tag.");
    string strUrlId = (string) null;
    if (reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      strUrlId = reader.Value;
    ExternWorkbookImpl externBook = (ExternWorkbookImpl) null;
    if (!reader.IsEmptyElement)
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "sheetNames":
              List<string> sheetNames = this.ParseSheetNames(reader);
              externBook = this.CreateExternBook(relations, strUrlId, sheetNames);
              continue;
            case "sheetDataSet":
              this.ParseSheetDataSet(reader, externBook);
              continue;
            case "definedNames":
              this.ParseExternalDefinedNames(reader, externBook);
              continue;
            default:
              throw new XmlException("Unexpected xml tag.");
          }
        }
        else
          reader.Read();
      }
    }
    reader.Read();
  }

  private void ParseExternalDefinedNames(XmlReader reader, ExternWorkbookImpl externBook)
  {
    if (reader.LocalName != "definedNames")
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
            case "definedName":
              this.ParseExternalName(reader, externBook);
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

  private void ParseExternalName(XmlReader reader, ExternWorkbookImpl externBook)
  {
    if (reader.LocalName != "definedName")
      throw new XmlException();
    string name = (string) null;
    string str = (string) null;
    if (reader.MoveToAttribute("name"))
      name = reader.Value;
    if (reader.MoveToAttribute("refersTo"))
      str = reader.Value;
    int index = externBook.ExternNames.Add(name);
    externBook.ExternNames[index].RefersTo = str;
    if (!reader.MoveToAttribute("sheetId"))
      return;
    externBook.ExternNames[index].sheetId = Convert.ToInt32(reader.Value);
  }

  private void ParseSheetDataSet(XmlReader reader, ExternWorkbookImpl externBook)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (externBook == null)
      throw new ArgumentNullException(nameof (externBook));
    if (reader.LocalName != "sheetDataSet")
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
            case "sheetData":
              this.ParseExternalSheetData(reader, externBook);
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
    reader.Skip();
  }

  private void ParseExternalSheetData(XmlReader reader, ExternWorkbookImpl externBook)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    if (externBook == null)
      throw new ArgumentNullException(nameof (externBook));
    if (reader.LocalName != "sheetData")
      throw new XmlException();
    int key = reader.MoveToAttribute("sheetId") ? XmlConvertExtension.ToInt32(reader.Value) : throw new XmlException();
    ExternWorksheetImpl worksheet = externBook.Worksheets[key];
    reader.MoveToElement();
    worksheet.AdditionalAttributes = this.ParseSheetData(reader, (IInternalWorksheet) worksheet, (List<int>) null, "cell");
  }

  private ExternWorkbookImpl CreateExternBook(
    RelationCollection relations,
    string strUrlId,
    List<string> arrSheetNames)
  {
    string stringToUnescape = relations[strUrlId].Target;
    if (stringToUnescape.StartsWith("file:///"))
      stringToUnescape = stringToUnescape.Substring("file:///".Length);
    string path = Uri.UnescapeDataString(stringToUnescape);
    string fileName = Path.GetFileName(path);
    return this.m_book.ExternWorkbooks[this.m_book.ExternWorkbooks.Add(path.Substring(0, path.Length - fileName.Length), fileName, arrSheetNames, (string[]) null)];
  }

  private List<string> ParseSheetNames(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "sheetNames")
      throw new XmlException("Unexpected xml tag");
    List<string> sheetNames = new List<string>();
    if (!reader.IsEmptyElement)
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "sheetName")
        {
          string str = reader.MoveToAttribute("val") ? reader.Value : throw new XmlException();
          sheetNames.Add(str);
        }
        reader.Read();
      }
    }
    reader.Read();
    return sheetNames;
  }

  private void ParseExternalLinksWorkbookPart(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    bool flag = !(reader.LocalName != "externalReferences") ? reader.IsEmptyElement : throw new XmlException("Unexpected xml tag.");
    reader.Read();
    if (flag)
      return;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "externalReference")
        this.ParseExternalLinkWorkbookPart(reader);
      else
        reader.Read();
    }
    reader.Read();
  }

  private void ParseExternalLinkWorkbookPart(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "externalReference")
      throw new XmlException("Unexpected xml tag.");
    if (!reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      throw new XmlException();
    this.m_book.DataHolder.ParseExternalLink(reader.Value);
  }

  public void Dispose()
  {
    this.m_dictShapeParsers.Clear();
    this.m_dictShapeParsers = (Dictionary<int, ShapeParser>) null;
    this.m_values.Clear();
    this.m_values = (List<string>) null;
    this.m_drawingParser = (DrawingParser) null;
    this.m_formulaUtil.Dispose();
    this.m_formulaUtil = (FormulaUtil) null;
    if (this.m_lstThemeColors != null)
    {
      this.m_lstThemeColors.Clear();
      this.m_lstThemeColors = (List<Color>) null;
    }
    if (this.m_dicThemeColors != null)
    {
      this.m_dicThemeColors.Clear();
      this.m_dicThemeColors = (Dictionary<string, Color>) null;
    }
    if (this.m_dicMajorFonts != null)
    {
      this.m_dicMajorFonts.Clear();
      this.m_dicMajorFonts = (Dictionary<string, FontImpl>) null;
    }
    if (this.m_dicMinorFonts == null)
      return;
    this.m_dicMinorFonts.Clear();
    this.m_dicMinorFonts = (Dictionary<string, FontImpl>) null;
  }

  private void SetCellRecord(
    Excel2007Serializator.CellType type,
    string strValue,
    CellRecordCollection cells,
    int iRow,
    int iColumn,
    int iXFIndex)
  {
    switch (strValue)
    {
      case null:
        break;
      case "":
        break;
      default:
        if (cells == null)
          throw new ArgumentNullException(nameof (cells));
        switch (type)
        {
          case Excel2007Serializator.CellType.b:
            cells.SetBooleanValue(iRow, iColumn, XmlConvertExtension.ToBoolean(strValue), iXFIndex);
            return;
          case Excel2007Serializator.CellType.e:
            cells.SetErrorValue(iRow, iColumn, strValue, iXFIndex);
            return;
          case Excel2007Serializator.CellType.inlineStr:
          case Excel2007Serializator.CellType.s:
            cells.SetSingleStringValue(iRow, iColumn, iXFIndex, XmlConvertExtension.ToInt32(strValue));
            return;
          case Excel2007Serializator.CellType.n:
            cells.SetNumberValue(iRow, iColumn, XmlConvertExtension.ToDouble(strValue), iXFIndex);
            return;
          case Excel2007Serializator.CellType.str:
            cells.SetNonSSTString(iRow, iColumn, iXFIndex, strValue);
            return;
          default:
            return;
        }
    }
  }

  private void SetFormulaValue(
    IInternalWorksheet sheet,
    Excel2007Serializator.CellType cellType,
    string strValue,
    int iRowIndex,
    int iColumnIndex)
  {
    if (strValue == null)
      throw new NullReferenceException(nameof (strValue));
    switch (cellType)
    {
      case Excel2007Serializator.CellType.b:
        sheet.SetFormulaBoolValue(iRowIndex, iColumnIndex, XmlConvertExtension.ToBoolean(strValue));
        break;
      case Excel2007Serializator.CellType.e:
        if (!(strValue != string.Empty))
          break;
        sheet.SetFormulaErrorValue(iRowIndex, iColumnIndex, strValue);
        break;
      case Excel2007Serializator.CellType.n:
        sheet.SetFormulaNumberValue(iRowIndex, iColumnIndex, XmlConvertExtension.ToDouble(strValue));
        break;
      case Excel2007Serializator.CellType.str:
        sheet.SetFormulaStringValue(iRowIndex, iColumnIndex, strValue);
        break;
    }
  }

  private void SetArrayFormula(
    WorksheetImpl sheet,
    string strFormulaString,
    string strCellsRange,
    int iXFIndex)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (strFormulaString == null)
      throw new ArgumentNullException(nameof (strFormulaString));
    if (strCellsRange == null)
      throw new ArgumentNullException("strCellRange");
    ArrayRecord record = (ArrayRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Array);
    strFormulaString = UtilityMethods.RemoveFirstCharUnsafe(strFormulaString);
    record.Formula = this.m_formulaUtil.ParseString(strFormulaString, (IWorksheet) sheet, (Dictionary<string, string>) null);
    string strRow1 = (string) null;
    string strColumn1 = (string) null;
    string strRow2 = (string) null;
    string strColumn2 = (string) null;
    if (this.m_formulaUtil.IsCellRange(strCellsRange, false, out strRow1, out strColumn1, out strRow2, out strColumn2))
    {
      record.FirstRow = Convert.ToInt32(strRow1) - 1;
      record.FirstColumn = RangeImpl.GetColumnIndex(strColumn1) - 1;
      record.LastRow = Convert.ToInt32(strRow2) - 1;
      record.LastColumn = RangeImpl.GetColumnIndex(strColumn2) - 1;
    }
    else
    {
      int iRow = 0;
      int iColumn = 0;
      RangeImpl.CellNameToRowColumn(strCellsRange, out iRow, out iColumn);
      record.FirstRow = iRow - 1;
      record.FirstColumn = iColumn - 1;
      record.LastRow = iRow - 1;
      record.LastColumn = iColumn - 1;
    }
    ((RangeImpl) sheet.Range[strCellsRange]).SetFormulaArrayRecord(record, iXFIndex);
  }

  private void SetSharedFormula(
    WorksheetImpl sheet,
    string strFormulaString,
    string strCellsRange,
    uint uiSharedGroupIndex,
    int iRow,
    int iCol,
    int iXFIndex,
    bool bCalculateOnOpen)
  {
    CellRecordCollection cellRecords = sheet.CellRecords;
    if (strCellsRange != null && strFormulaString != null)
    {
      string str = (string) null;
      string columnName = (string) null;
      string strRow2 = (string) null;
      string strColumn2 = (string) null;
      SharedFormulaRecord record = (SharedFormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.SharedFormula2);
      bool flag;
      if (!(flag = this.m_formulaUtil.IsCellRange(strCellsRange, false, out str, out columnName, out strRow2, out strColumn2)) && (flag = FormulaUtil.IsCell(strCellsRange, false, out str, out columnName)))
      {
        strRow2 = str;
        strColumn2 = columnName;
      }
      if (flag)
      {
        int int32 = Convert.ToInt32(str);
        int columnIndex = RangeImpl.GetColumnIndex(columnName);
        record.FirstRow = int32;
        record.FirstColumn = columnIndex;
        record.LastRow = Convert.ToInt32(strRow2);
        record.LastColumn = RangeImpl.GetColumnIndex(strColumn2);
        strFormulaString = UtilityMethods.RemoveFirstCharUnsafe(strFormulaString);
        record.Formula = this.m_formulaUtil.ParseSharedString(strFormulaString, iRow, iCol, (IWorksheet) sheet);
        RecordTable table = cellRecords.Table;
        int count = table.SharedFormulas.Count;
        table.AddSharedFormula(0, (int) uiSharedGroupIndex, record);
      }
    }
    FormulaRecord record1 = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
    SharedFormulaRecord sharedFormula = cellRecords.Table.SharedFormulas[(long) (int) uiSharedGroupIndex];
    record1.ParsedExpression = FormulaUtil.ConvertSharedFormulaTokens(sharedFormula, (IWorkbook) sheet.ParentWorkbook, iRow - 1, iCol - 1);
    record1.Row = iRow - 1;
    record1.Column = iCol - 1;
    record1.ExtendedFormatIndex = (ushort) iXFIndex;
    record1.CalculateOnOpen = bCalculateOnOpen;
    cellRecords.SetCellRecord(iRow, iCol, (ICellPositionFormat) record1);
  }

  private ExcelDataType ConvertDataValidationType(string dataValidationType)
  {
    if (dataValidationType == null || dataValidationType == string.Empty)
      throw new ArgumentNullException("strErrorStyle");
    switch (dataValidationType)
    {
      case "custom":
        return ExcelDataType.Formula;
      case "date":
        return ExcelDataType.Date;
      case "decimal":
        return ExcelDataType.Decimal;
      case "list":
        return ExcelDataType.User;
      case "none":
        return ExcelDataType.Any;
      case "textLength":
        return ExcelDataType.TextLength;
      case "time":
        return ExcelDataType.Time;
      case "whole":
        return ExcelDataType.Integer;
      default:
        throw new ArgumentOutOfRangeException(nameof (dataValidationType));
    }
  }

  private ExcelErrorStyle ConvertDataValidationErrorStyle(string strErrorStyle)
  {
    if (strErrorStyle == null || strErrorStyle == string.Empty)
      throw new ArgumentNullException(nameof (strErrorStyle));
    switch (strErrorStyle)
    {
      case "information":
        return ExcelErrorStyle.Info;
      case "stop":
        return ExcelErrorStyle.Stop;
      case "warning":
        return ExcelErrorStyle.Warning;
      default:
        throw new ArgumentOutOfRangeException(nameof (strErrorStyle));
    }
  }

  private ExcelDataValidationComparisonOperator ConvertDataValidationOperator(string strOperator)
  {
    if (strOperator == null || strOperator == string.Empty)
      throw new ArgumentNullException(nameof (strOperator));
    switch (strOperator)
    {
      case "between":
        return ExcelDataValidationComparisonOperator.Between;
      case "equal":
        return ExcelDataValidationComparisonOperator.Equal;
      case "greaterThan":
        return ExcelDataValidationComparisonOperator.Greater;
      case "greaterThanOrEqual":
        return ExcelDataValidationComparisonOperator.GreaterOrEqual;
      case "lessThan":
        return ExcelDataValidationComparisonOperator.Less;
      case "lessThanOrEqual":
        return ExcelDataValidationComparisonOperator.LessOrEqual;
      case "notBetween":
        return ExcelDataValidationComparisonOperator.NotBetween;
      case "notEqual":
        return ExcelDataValidationComparisonOperator.NotEqual;
      default:
        throw new ArgumentOutOfRangeException(nameof (strOperator));
    }
  }

  private TAddr[] GetRangesForDataValidation(string strRange)
  {
    string[] strArray = strRange != null && !(strRange == string.Empty) ? strRange.Split(' ') : throw new ArgumentNullException(nameof (strRange));
    List<TAddr> taddrList = new List<TAddr>();
    foreach (string strRange1 in strArray)
      taddrList.Add(this.GetRangeForDVOrAF(strRange1));
    return taddrList.ToArray();
  }

  private TAddr GetRangeForDVOrAF(string strRange)
  {
    string str = strRange != null && !(strRange == string.Empty) ? string.Empty : throw new ArgumentNullException(nameof (strRange));
    string empty = string.Empty;
    string strRow2 = string.Empty;
    string strColumn2 = string.Empty;
    TAddr rangeForDvOrAf = new TAddr();
    if (FormulaUtil.IsCell(strRange, false, out str, out empty))
    {
      int num1 = Convert.ToInt32(str) - 1;
      int num2 = RangeImpl.GetColumnIndex(empty) - 1;
      rangeForDvOrAf = new TAddr(num1, num2, num1, num2);
    }
    else if (this.m_formulaUtil.IsCellRange(strRange, false, out str, out empty, out strRow2, out strColumn2))
      rangeForDvOrAf = new TAddr(Convert.ToInt32(str) - 1, RangeImpl.GetColumnIndex(empty) - 1, Convert.ToInt32(strRow2) - 1, RangeImpl.GetColumnIndex(strColumn2) - 1);
    return rangeForDvOrAf;
  }

  private OfficeFilterCondition ConvertAutoFormatFilterCondition(string strCondition)
  {
    switch (strCondition)
    {
      case "equal":
        return OfficeFilterCondition.Equal;
      case "greaterThan":
        return OfficeFilterCondition.Greater;
      case "greaterThanOrEqual":
        return OfficeFilterCondition.GreaterOrEqual;
      case "lessThan":
        return OfficeFilterCondition.Less;
      case "lessThanOrEqual":
        return OfficeFilterCondition.LessOrEqual;
      case "notEqual":
        return OfficeFilterCondition.NotEqual;
      default:
        throw new ArgumentOutOfRangeException(nameof (strCondition));
    }
  }

  private ExcelCFType ConvertCFType(string strType, out bool bIsSupportedType)
  {
    bIsSupportedType = true;
    switch (strType)
    {
      case "cellIs":
        return ExcelCFType.CellValue;
      case "endsWith":
      case "beginsWith":
      case "containsText":
      case "notContainsText":
        return ExcelCFType.SpecificText;
      case "expression":
        return ExcelCFType.Formula;
      case "dataBar":
        return ExcelCFType.DataBar;
      case "iconSet":
        return ExcelCFType.IconSet;
      case "colorScale":
        return ExcelCFType.ColorScale;
      case "containsBlanks":
        return ExcelCFType.Blank;
      case "notContainsBlanks":
        return ExcelCFType.NoBlank;
      case "containsErrors":
        return ExcelCFType.ContainsErrors;
      case "notContainsErrors":
        return ExcelCFType.NotContainsErrors;
      case "timePeriod":
        return ExcelCFType.TimePeriod;
      default:
        bIsSupportedType = false;
        return ExcelCFType.CellValue;
    }
  }

  private ExcelComparisonOperator ConvertCFOperator(
    string strOperator,
    out bool bIsSupportedOperator)
  {
    bIsSupportedOperator = true;
    switch (strOperator)
    {
      case "between":
        return ExcelComparisonOperator.Between;
      case "equal":
        return ExcelComparisonOperator.Equal;
      case "greaterThan":
        return ExcelComparisonOperator.Greater;
      case "greaterThanOrEqual":
        return ExcelComparisonOperator.GreaterOrEqual;
      case "lessThan":
        return ExcelComparisonOperator.Less;
      case "lessThanOrEqual":
        return ExcelComparisonOperator.LessOrEqual;
      case "notContains":
        return ExcelComparisonOperator.NotContainsText;
      case "notBetween":
        return ExcelComparisonOperator.NotBetween;
      case "notEqual":
        return ExcelComparisonOperator.NotEqual;
      case "beginsWith":
        return ExcelComparisonOperator.BeginsWith;
      case "containsText":
        return ExcelComparisonOperator.ContainsText;
      case "endsWith":
        return ExcelComparisonOperator.EndsWith;
      default:
        throw new ArgumentOutOfRangeException(nameof (strOperator));
    }
  }

  private CFTimePeriods ConvertCFTimePeriods(string timePeriod, out bool bIsSupportedTimePeriod)
  {
    bIsSupportedTimePeriod = true;
    switch (timePeriod)
    {
      case "yesterday":
        return CFTimePeriods.Yesterday;
      case "today":
        return CFTimePeriods.Today;
      case "tomorrow":
        return CFTimePeriods.Tomorrow;
      case "last7Days":
        return CFTimePeriods.Last7Days;
      case "lastWeek":
        return CFTimePeriods.LastWeek;
      case "thisWeek":
        return CFTimePeriods.ThisWeek;
      case "nextWeek":
        return CFTimePeriods.NextWeek;
      case "lastMonth":
        return CFTimePeriods.LastMonth;
      case "thisMonth":
        return CFTimePeriods.ThisMonth;
      case "nextMonth":
        return CFTimePeriods.NextMonth;
      default:
        throw new ArgumentOutOfRangeException(nameof (timePeriod));
    }
  }

  private string GetReaderElementValue(XmlReader reader)
  {
    if (reader.IsEmptyElement)
    {
      reader.Read();
      return string.Empty;
    }
    reader.Read();
    string empty;
    if (reader.NodeType != XmlNodeType.EndElement)
    {
      empty = reader.Value;
      reader.Skip();
    }
    else
      empty = string.Empty;
    reader.Skip();
    return empty;
  }

  internal Color ConvertColorByShade(Color result, double shade)
  {
    return Color.FromArgb((int) (byte) ((double) result.A * shade), (int) (byte) ((double) result.R * shade), (int) (byte) ((double) result.G * shade), (int) (byte) ((double) result.B * shade));
  }

  internal Color ConvertColorByShadeBlip(Color result, double shade)
  {
    int[] numArray = new int[3]
    {
      (int) result.R,
      (int) result.G,
      (int) result.B
    };
    for (int index = 0; index < 3; ++index)
    {
      double doubleValue = Excel2007Parser.CalcDouble(numArray[index]) * shade;
      if (doubleValue < 0.0)
        doubleValue = 0.0;
      else if (doubleValue > 1.0)
        doubleValue = 1.0;
      numArray[index] = Excel2007Parser.CalcInt(doubleValue);
    }
    return Color.FromArgb((int) result.A, (int) (byte) numArray[0], (int) (byte) numArray[1], (int) (byte) numArray[2]);
  }

  private string ConvertToASCII(string value)
  {
    value = value.Replace("_x000a_", "\n");
    value = value.Replace("_x000d_", "\r");
    value = value.Replace("_x0009_", "\t");
    value = value.Replace("_x0008_", "\b");
    value = value.Replace("_x0000_", "\0");
    return value;
  }

  public static void CopyFillSettings(FillImpl fill, ExtendedFormatImpl extendedFormat)
  {
    if (fill.FillType == OfficeFillType.Gradient)
    {
      extendedFormat.Gradient = (IGradient) new ShapeFillImpl(extendedFormat.Application, (object) extendedFormat, OfficeFillType.Gradient);
      IGradient gradient = extendedFormat.Gradient;
      gradient.GradientStyle = fill.GradientStyle;
      gradient.GradientVariant = fill.GradientVariant;
      gradient.BackColorObject.CopyFrom(fill.PatternColorObject, true);
      gradient.ForeColorObject.CopyFrom(fill.ColorObject, true);
      extendedFormat.Record.AdtlFillPattern = (ushort) 4000;
    }
    else
    {
      extendedFormat.IncludePatterns = true;
      extendedFormat.ColorObject.CopyFrom(fill.ColorObject, true);
      extendedFormat.PatternColorObject.CopyFrom(fill.PatternColorObject, true);
      extendedFormat.FillPattern = fill.Pattern;
    }
  }

  internal List<Color> ParseThemeOverideColors(ChartImpl chart)
  {
    XmlReader reader = UtilityMethods.CreateReader((Stream) chart.m_themeOverrideStream);
    Dictionary<string, Color> dicThemeColors = (Dictionary<string, Color>) null;
    return this.ParseThemeColors(reader, out dicThemeColors);
  }

  private delegate OfficeSheetProtection ChecProtectionDelegate(
    XmlReader reader,
    string attributeName,
    OfficeSheetProtection flag,
    bool defaultValue,
    OfficeSheetProtection protection);
}
