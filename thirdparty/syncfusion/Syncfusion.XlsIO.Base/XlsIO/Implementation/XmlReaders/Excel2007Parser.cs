// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.Excel2007Parser
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.Compression;
using Syncfusion.Compression.Zip;
using Syncfusion.Drawing;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Implementation.Sorting;
using Syncfusion.XlsIO.Implementation.XmlReaders.Shapes;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Charts;
using Syncfusion.XlsIO.Implementation.XmlSerialization.Constants;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders;

public class Excel2007Parser
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
  private const string DEF_IMAGEPATH = "../media/";
  private const string DoubleEqualent = "==";
  private const string ThreeStars = "3Stars";
  private const string ThreeTriangles = "3Triangles";
  private const string FiveBoxes = "5Boxes";
  private int startRow;
  private int endRow;
  private int previousOutlineLevel;
  private int previousRow;
  private WorkbookImpl m_book;
  private FormulaUtil m_formulaUtil;
  private Dictionary<int, ShapeParser> m_dictShapeParsers = new Dictionary<int, ShapeParser>();
  private List<Color> m_lstThemeColors;
  internal Dictionary<string, Color> m_dicThemeColors;
  private Dictionary<string, FontImpl> m_dicMajorFonts;
  private Dictionary<string, FontImpl> m_dicMinorFonts;
  private Dictionary<int, ShapeLineFormatImpl> m_dicLineStyles;
  private List<string> m_values = new List<string>();
  private List<ZipArchiveItem> m_bgImages;
  private string parentElement = string.Empty;
  private bool m_enableAlternateContent;
  private WorksheetImpl m_workSheet;
  private DrawingParser m_drawingParser;
  private OutlineWrapperUtility m_outlineWrapperUtility;
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
  private bool m_hasDVExtlst;
  private bool m_hasCFExtlst;
  private ExcelCFType m_cfType;
  private Dictionary<string, Color> m_currentChartThemeColors;

  public FormulaUtil FormulaUtil => this.m_formulaUtil;

  internal WorksheetImpl Worksheet => this.m_workSheet;

  internal Dictionary<string, Color> CurrentChartThemeColors
  {
    get => this.m_currentChartThemeColors;
    set => this.m_currentChartThemeColors = value;
  }

  public Excel2007Parser(WorkbookImpl book)
  {
    this.m_book = book != null ? book : throw new ArgumentNullException(nameof (book));
    this.m_formulaUtil = new FormulaUtil(this.m_book.Application, (object) this.m_book, NumberFormatInfo.InvariantInfo, ',', ';');
    this.dpiX = book.AppImplementation.GetdpiX();
    this.dpiY = book.AppImplementation.GetdpiY();
    this.m_dictShapeParsers.Add(202, (ShapeParser) new CommentShapeParser());
    this.m_dictShapeParsers.Add(201, (ShapeParser) new VmlFormControlParser());
    this.m_dictShapeParsers.Add(75, (ShapeParser) new HFImageParser());
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
    bool flag = themeColors != null && themeColors.TryGetValue(colorName, out themeColor);
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
    Stream functionGroups,
    ref List<Dictionary<string, string>> lstCustomBookViews)
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
    if (reader.MoveToAttribute("conformance") && reader.Value == "strict")
      this.m_book.IsStrict = true;
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
          case "customWorkbookViews":
            lstCustomBookViews = this.ParseCustomWorkbookViews(reader);
            continue;
          case "fileSharing":
            this.ParseFileSharing(reader);
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

  internal void ParseMetaProperties(
    XmlReader reader,
    FileDataHolder fileDataHolder,
    Stream stream,
    string itemName)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (fileDataHolder == null)
      throw new ArgumentNullException(nameof (fileDataHolder));
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    switch (reader.LocalName)
    {
      case "contentTypeSchema":
        if (reader.LocalName == "contentTypeSchema" && reader.NamespaceURI == "http://schemas.microsoft.com/office/2006/metadata/contentType")
        {
          while (reader.Read())
          {
            if (reader.LocalName == "element")
              this.parentElement = reader.GetAttribute("name");
            if (reader.LocalName == "complexContent")
              this.ParseChildElements(reader);
            else if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "element")
            {
              string attribute1 = reader.GetAttribute("name");
              switch (attribute1)
              {
                case "documentManagement":
                  Excel2007Parser.ParseDocumentManagmentSchema(reader, ref this.m_values);
                  continue;
                case null:
                  continue;
                default:
                  if (reader.NamespaceURI == "http://www.w3.org/2001/XMLSchema" && this.m_values.IndexOf(attribute1) >= 0)
                  {
                    string attribute2 = reader.GetAttribute("ma:displayName");
                    string attribute3 = reader.GetAttribute("ma:internalName");
                    string attribute4 = reader.GetAttribute("name");
                    if (attribute2 != null && attribute3 != null)
                    {
                      if (!(this.m_book.InnerContentTypeProperties.GetItemByInternalName(attribute3) is MetaPropertyImpl itemByInternalName))
                        itemByInternalName = this.m_book.InnerContentTypeProperties.GetItemByInternalName(attribute4) as MetaPropertyImpl;
                      if (itemByInternalName != null)
                      {
                        itemByInternalName.Name = attribute2;
                        itemByInternalName.ElementName = attribute1;
                        continue;
                      }
                      this.m_book.InnerContentTypeProperties.Add((IMetaProperty) new MetaPropertyImpl()
                      {
                        InternalName = attribute3,
                        Name = attribute2,
                        ElementName = attribute1
                      });
                      continue;
                    }
                    continue;
                  }
                  continue;
              }
            }
          }
        }
        byte[] numArray = new byte[stream.Length];
        stream.Position = 0L;
        stream.Read(numArray, 0, (int) stream.Length);
        this.m_book.InnerContentTypeProperties.SchemaXml = Encoding.UTF8.GetString(numArray);
        break;
      case "properties":
        reader.Read();
        if (reader.NodeType == XmlNodeType.Whitespace)
          reader.Read();
        while (reader.NodeType != XmlNodeType.EndElement)
        {
          if (reader.NodeType == XmlNodeType.Element)
          {
            switch (reader.LocalName)
            {
              case "documentManagement":
                this.m_book.InnerContentTypeProperties.ItemName = itemName;
                this.ParseDocumentManagementPropties(reader);
                continue;
              default:
                reader.Skip();
                continue;
            }
          }
          else
            reader.Read();
        }
        break;
    }
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

  private void ParseDocumentManagementPropties(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    List<Stream> streamList = new List<Stream>();
    int childCount = 0;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Whitespace)
        reader.Read();
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (this.m_values.IndexOf(reader.LocalName) >= 0)
        {
          string localName = reader.LocalName;
          string namespaceUri = reader.NamespaceURI;
          string str = "";
          try
          {
            str = reader.ReadElementContentAsString();
          }
          catch (Exception ex)
          {
            this.ReadManagementProperties(reader, localName, childCount);
          }
          MetaPropertyImpl propertyByName = this.GetPropertyByName(localName);
          if (propertyByName != null)
          {
            propertyByName.Value = str;
            propertyByName.NameSpaceURI = namespaceUri;
          }
        }
        else
        {
          string localName = reader.LocalName;
          string namespaceUri = reader.NamespaceURI;
          string str = "";
          try
          {
            str = reader.ReadElementContentAsString();
          }
          catch (Exception ex)
          {
            this.ReadManagementProperties(reader, localName, childCount);
          }
          MetaPropertyImpl propertyByName = this.GetPropertyByName(localName);
          if (propertyByName != null)
          {
            propertyByName.Value = str;
            propertyByName.NameSpaceURI = namespaceUri;
          }
          else
            this.m_book.InnerContentTypeProperties.Add((IMetaProperty) new MetaPropertyImpl()
            {
              Value = str,
              NameSpaceURI = namespaceUri,
              InternalName = localName
            });
        }
      }
    }
  }

  private MetaPropertyImpl GetPropertyByName(string name)
  {
    return (MetaPropertyImpl) this.m_book.ContentTypeProperties.GetItemByInternalName(name) ?? (MetaPropertyImpl) (this.m_book.ContentTypeProperties as MetaPropertiesImpl).GetItemByDisplayName(name);
  }

  private void ReadManagementProperties(XmlReader reader, string internalName, int childCount)
  {
    List<Stream> streamList = new List<Stream>();
    if (this.m_book.m_childElements.ContainsKey(internalName))
    {
      this.m_book.m_childElements.TryGetValue(internalName, out streamList);
      childCount = streamList.Count;
      streamList.Clear();
      this.m_book.m_childElements.Remove(internalName);
      while (reader.NodeType != XmlNodeType.EndElement)
        streamList.Add(ShapeParser.ReadNodeAsStream(reader));
      reader.Read();
      if (this.m_book.m_childElementValues.ContainsKey(internalName))
        this.m_book.m_childElementValues.Remove(internalName);
      this.m_book.m_childElementValues.Add(internalName, streamList);
    }
    else
    {
      while (reader.NodeType != XmlNodeType.EndElement)
        streamList.Add(ShapeParser.ReadNodeAsStream(reader));
      if (this.m_book.m_childElementValues.ContainsKey(internalName))
        this.m_book.m_childElementValues.Remove(internalName);
      this.m_book.m_childElementValues.Add(internalName, streamList);
      reader.Read();
    }
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
    int index1 = 0;
    for (int count = objects.Count; index1 < count; ++index1)
    {
      if (objects[index1] != null)
      {
        WorksheetBaseImpl worksheetBaseImpl = (WorksheetBaseImpl) objects[index1];
        switch (worksheetBaseImpl)
        {
          case WorksheetImpl _:
            (worksheetBaseImpl as WorksheetImpl).ParseDataOnDemand = parseOnDemand;
            break;
          case ChartImpl _:
            (worksheetBaseImpl as ChartImpl).ParseDataOnDemand = parseOnDemand;
            break;
        }
        worksheetBaseImpl.ParseData(dictUpdatedSSTIndexes);
        worksheetBaseImpl.IsSaved = false;
        appImplementation.RaiseProgressEvent((long) (index1 + 4 + 1), (long) fullSize);
      }
    }
    for (int index2 = 0; index2 < this.m_book.InnerDialogs.Count; ++index2)
      this.m_book.InnerDialogs[index2].PreservedStream = this.m_book.InnerDialogs[index2].DataHolder.ParseDialogMacrosheetData();
    for (int index3 = 0; index3 < this.m_book.InnerMacros.Count; ++index3)
      this.m_book.InnerMacros[index3].PreservedStream = this.m_book.InnerMacros[index3].DataHolder.ParseDialogMacrosheetData();
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
    if (reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
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

  private void ParseFileSharing(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    if (reader.LocalName != "fileSharing")
      throw new XmlException();
    if (reader.MoveToAttribute("algorithmName"))
      this.m_book.AlgorithmName = reader.Value;
    if (reader.MoveToAttribute("hashValue"))
      this.m_book.HashValue = Convert.FromBase64String(reader.Value);
    if (reader.MoveToAttribute("saltValue"))
      this.m_book.SaltValue = Convert.FromBase64String(reader.Value);
    if (reader.MoveToAttribute("spinCount"))
      this.m_book.SpinCount = Convert.ToUInt32(reader.Value);
    if (reader.MoveToAttribute("readOnlyRecommended"))
      this.m_book.ReadOnlyRecommended = XmlConvertExtension.ToBoolean(reader.Value);
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
    if (reader.MoveToAttribute("calcMode"))
    {
      switch (reader.Value)
      {
        case "manual":
          this.m_book.CalculationOptions.CalculationMode = ExcelCalculationMode.Manual;
          break;
        case "autoNoTable":
          this.m_book.CalculationOptions.CalculationMode = ExcelCalculationMode.AutomaticExceptTables;
          break;
      }
    }
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
    if (num == (ushort) 0)
      return;
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
      if (!reader.LocalName.Equals("uid"))
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
    if ((sheet.Application as ApplicationImpl).IsExternBookParsing)
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "sheetData":
              this.ParseSheetData(reader, (IInternalWorksheet) sheet, arrStyles, "c");
              continue;
            case "tableParts":
              this.ParseTableParts(reader, sheet, strParentPath);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          reader.Skip();
      }
      if (dictUpdatedSSTIndexes == null)
        return;
      sheet.UpdateLabelSSTIndexes(dictUpdatedSSTIndexes, new IncreaseIndex(sheet.ParentWorkbook.InnerSST.AddIncrease));
    }
    else
    {
      this.ParseSheetBeforeData(reader, sheet, (Stream) streamStart, arrStyles);
      if (reader.LocalName == "sheetData")
        this.ParseSheetData(reader, (IInternalWorksheet) sheet, arrStyles, "c");
      if (dictUpdatedSSTIndexes != null)
        sheet.UpdateLabelSSTIndexes(dictUpdatedSSTIndexes, new IncreaseIndex(sheet.ParentWorkbook.InnerSST.AddIncrease));
      this.ParseAfterSheetData(reader, sheet, ref streamCF, strParentPath, dictItemsToRemove);
    }
  }

  private void ParseSheetBeforeData(
    XmlReader reader,
    WorksheetImpl sheet,
    Stream streamStart,
    List<int> arrStyles)
  {
    XmlWriter writer = UtilityMethods.CreateWriter(streamStart, Encoding.UTF8);
    writer.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
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
      writer.WriteNode(reader, false);
    }
    writer.WriteEndElement();
    writer.Flush();
    streamStart.Position = 0L;
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
      sheet.GridLineColor = (ExcelKnownColors) XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("view"))
    {
      switch (reader.Value)
      {
        case "pageLayout":
          sheet.View = SheetView.PageLayout;
          break;
        case "pageBreakPreview":
          sheet.View = SheetView.PageBreakPreview;
          sheet.WindowTwo.IsSavedInPageBreakPreview = true;
          break;
        case "normal":
          sheet.View = SheetView.Normal;
          break;
      }
    }
    if (reader.MoveToAttribute("tabSelected") && reader.Value != "0")
    {
      sheetBase.WindowTwo.IsSelected = true;
      if (sheetBase.RealIndex != sheetBase.Workbook.ActiveSheetIndex)
        this.m_book.WorksheetGroup.Add((ITabSheet) sheetBase);
    }
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
    string name = (string) null;
    if (reader.MoveToAttribute("activeCell"))
      name = reader.Value;
    if (reader.MoveToAttribute("sqref") && !reader.Value.Contains(" "))
      name = reader.Value;
    if (name != null)
      sheet.SetActiveCell(sheet.Range[name], false);
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
      sheet.VerticalSplit = (int) XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute("ySplit"))
      sheet.HorizontalSplit = (int) XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute("topLeftCell"))
    {
      string name = reader.Value;
      sheet.PaneFirstVisible = sheet[name];
    }
    if (reader.MoveToAttribute("activePane") && Enum.IsDefined(typeof (Pane.ActivePane), (object) reader.Value))
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
          case "picture":
            string itemName = chart.DataHolder.ArchiveItem.ItemName;
            string strParentPath = itemName.Substring(0, itemName.LastIndexOf('/'));
            this.ParseBackgroundImage(reader, (WorksheetBaseImpl) chart, strParentPath);
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
    if (!reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      throw new XmlException();
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
    bool isChartEx = false;
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
    if (chart != null && chart.Shapes.Count > 0)
      this.CalculateShapesPosition(chart, chart.Width, chart.Height);
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
                if (!reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
                  throw new XmlException();
                string id = reader.Value;
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
    if (!reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      throw new XmlException();
    string id = reader.Value;
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
      foreach (KeyValuePair<string, Relation> keyValuePair in relations1)
      {
        chart.Relations[keyValuePair.Key] = keyValuePair.Value;
        switch (keyValuePair.Value.Type)
        {
          case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/themeOverride":
            relation2 = keyValuePair.Value;
            continue;
          case "http://schemas.microsoft.com/office/2011/relationships/chartColorStyle":
            chart.m_isChartColorStyleSkipped = true;
            continue;
          case "http://schemas.microsoft.com/office/2011/relationships/chartStyle":
            chart.m_isChartStyleSkipped = true;
            continue;
          default:
            continue;
        }
      }
      chart.Relations.ItemPath = relations1.ItemPath;
    }
    if (relation2 != null)
    {
      XmlReader reader2 = dataHolder.CreateReader(relation2, "xl/workbook.xml", out string _);
      while (reader2.NodeType != XmlNodeType.Element)
        reader2.Read();
      if (reader2.LocalName == "themeOverride")
      {
        reader2.Read();
        while (reader2.NodeType != XmlNodeType.EndElement)
        {
          if (reader2.NodeType == XmlNodeType.Element && reader2.LocalName == "clrScheme")
          {
            MemoryStream data = new MemoryStream();
            XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
            writer.WriteNode(reader2, false);
            writer.Flush();
            chart.m_themeOverrideStream = data;
          }
          else
            reader2.Skip();
        }
      }
    }
    if (isChartEx)
      new ChartExParser(this.m_book).ParseChartEx(reader1, chart, relations1);
    else
      new ChartParser(this.m_book).ParseChart(reader1, chart, relations1);
    dataHolder.Archive.RemoveItem(strItemPath);
    dataHolder.DrawingsItemPath.Remove(strItemPath);
    relations.Remove(id);
  }

  private void ExtractDefaultRowHeight(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    bool flag = false;
    if (reader.MoveToAttribute("defaultColWidth"))
    {
      double defaultWidth = XmlConvertExtension.ToDouble(reader.Value);
      sheet.DefaultColumnWidth = defaultWidth;
      int fileWidth = (int) Math.Round(defaultWidth * 256.0);
      double num = (double) sheet.EvaluateRealColumnWidth(fileWidth) / 256.0;
      double width = num >= 0.0 ? num : sheet.StandardWidth;
      sheet.SetStandardWidth(width);
      this.SetDefaultColumnWidth(defaultWidth, false, sheet);
      flag = true;
    }
    if (reader.MoveToAttribute("defaultRowHeight"))
      sheet.StandardHeight = XmlConvertExtension.ToDouble(reader.Value);
    sheet.CustomHeight = false;
    if (reader.MoveToAttribute("customHeight"))
      sheet.CustomHeight = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("outlineLevelCol"))
      sheet.OutlineLevelColumn = XmlConvertExtension.ToByte(reader.Value);
    if (reader.MoveToAttribute("outlineLevelRow"))
      sheet.OutlineLevelRow = XmlConvertExtension.ToByte(reader.Value);
    if (reader.MoveToAttribute("baseColWidth"))
    {
      int result;
      if (!flag && int.TryParse(reader.Value, out result) && result > 0)
      {
        int pixels = sheet.ColumnWidthToPixels((double) result);
        double columnWidth = sheet.PixelsToColumnWidth(pixels + 5);
        sheet.SetStandardWidth(columnWidth);
        sheet.HasBaseColWidth = true;
      }
      sheet.BaseColumnWidth = (int) XmlConvertExtension.ToInt16(reader.Value);
    }
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
          int index;
          string namedRange = this.ParseNamedRange(reader, out index);
          if (index != -1 && index < stringList.Count)
            stringList[index] = namedRange;
          else
            stringList.Add(namedRange);
        }
        reader.Read();
      }
      WorkbookNamesCollection innerNamesColection = this.m_book.InnerNamesColection;
      this.m_book.AppImplementation.IsFormulaParsed = false;
      int index1 = 0;
      for (int count = stringList.Count; index1 < count; ++index1)
      {
        NameImpl nameImpl = (NameImpl) innerNamesColection[index1];
        if (stringList[index1].LastIndexOf('!') == 0)
        {
          nameImpl.IsCommon = true;
          stringList[index1] = !(innerNamesColection[index1].Scope == "Workbook") || this.m_book.ActiveSheet == null ? innerNamesColection[index1].Scope + stringList[index1] : this.m_book.ActiveSheet.Name + stringList[index1];
        }
        nameImpl.SetValue(this.m_formulaUtil.ParseString(stringList[index1]));
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
    List<int> styles1 = (List<int>) null;
    Dictionary<int, int> arrNumberFormatIndexes = (Dictionary<int, int>) null;
    reader.Read();
    if (reader.NodeType == XmlNodeType.None)
    {
      this.m_book.Application.DefaultVersion = this.m_book.Version;
      this.m_book.InsertDefaultFonts();
      this.m_book.InsertDefaultValues();
      List<int> styles2 = new List<int>();
      ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
      for (int index = 0; index < innerExtFormats.Count; ++index)
        styles2.Add(innerExtFormats[index].Index);
      return styles2;
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
            styles1 = this.ParseCellFormats(reader, intList1, arrFills, arrBorders, intList2, arrNumberFormatIndexes);
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
            if (this.m_book.DataHolder != null)
              this.ParseCustomTableStyles(reader);
            if (reader.LocalName != "tableStyles")
            {
              flag = false;
              break;
            }
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
    this.m_book.ArrNewNumberFormatIndexes = arrNumberFormatIndexes;
    if (this.m_book.InnerStyles.Count == 0)
      this.m_book.PrepareStyles(false, new List<StyleRecord>(), (Dictionary<int, int>) null);
    ExtendedFormatWrapper innerStyle = (ExtendedFormatWrapper) this.m_book.InnerStyles["Normal"];
    for (int index = 0; index < styles1.Count; ++index)
    {
      if (this.m_book.InnerExtFormats[styles1[index]].ParentIndex == innerStyle.XFormatIndex)
      {
        this.m_book.DefaultXFIndex = this.m_book.InnerExtFormats[styles1[index]].Index;
        break;
      }
    }
    return styles1;
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
        int num = !reader1.IsEmptyElement ? this.ParseStringItem(reader1) : this.m_book.InnerSST.AddIncrease((object) string.Empty, false);
        if (key != num)
          sst[key] = num;
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
            stringItem = this.ParseRichTextRun(reader, out text);
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
    bool isShapeTypePresent = false;
    while (reader.NodeType != XmlNodeType.EndElement && !reader.EOF)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "shapetype":
            this.ParseShapeType(reader, shapes, dictShapeIdToShape, layoutStream);
            isShapeTypePresent = true;
            continue;
          case "shape":
            if (reader.MoveToAttribute("type"))
            {
              this.ParseShape(reader, dictShapeIdToShape, relations, parentItemPath, shapes, isShapeTypePresent);
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
        reader.Skip();
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
    this.m_outlineWrapperUtility = new OutlineWrapperUtility();
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
      if (this.previousOutlineLevel != 0)
        this.m_outlineWrapperUtility.AddRowLevel(sheet as WorksheetImpl, this.startRow, this.previousRow, this.previousOutlineLevel, true);
      this.startRow = 0;
      this.previousRow = 0;
      this.previousOutlineLevel = 0;
      this.endRow = 0;
    }
    reader.Read();
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
    this.ParseDrawings(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove, false);
  }

  internal void ParseDrawings(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    string drawingsPath,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove,
    bool isChartShape)
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
            this.ParseTwoCellAnchor(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove, isChartShape);
            continue;
          case "AlternateContent":
            this.ParseAlternateContent(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
            continue;
          case "absoluteAnchor":
            double emuValue1 = 0.0;
            double emuValue2 = 0.0;
            double emuValue3 = 0.0;
            double emuValue4 = 0.0;
            MemoryStream data1 = (MemoryStream) null;
            while (reader.LocalName != "graphicData" && reader.LocalName != "pic" && reader.LocalName != "sp" && reader.LocalName != "grpSp")
            {
              if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "absoluteAnchor")
              {
                while (reader.LocalName != "ext")
                {
                  if (reader.LocalName == "pos")
                  {
                    if (reader.MoveToAttribute("x"))
                      emuValue1 = (double) Math.Max(0, reader.ReadContentAsInt());
                    if (reader.MoveToAttribute("y"))
                      emuValue2 = (double) Math.Max(0, reader.ReadContentAsInt());
                    this.m_drawingParser.posX = Helper.ConvertEmuToOffset((int) emuValue1, this.dpiX);
                    this.m_drawingParser.posY = Helper.ConvertEmuToOffset((int) emuValue2, this.dpiX);
                  }
                  reader.Read();
                }
                if (reader.MoveToAttribute("cx"))
                  emuValue3 = (double) int.Parse(reader.Value);
                if (reader.MoveToAttribute("cy"))
                  emuValue4 = (double) int.Parse(reader.Value);
                this.m_drawingParser.extCX = Helper.ConvertEmuToOffset((int) emuValue3, this.dpiX);
                this.m_drawingParser.extCY = Helper.ConvertEmuToOffset((int) emuValue4, this.dpiX);
              }
              else
              {
                if (reader.LocalName == "AlternateContent")
                {
                  ShapeImpl shape = this.CreateShape(reader, sheet, ref data1, drawingsPath, lstRelationIds);
                  shape.XmlDataStream = (Stream) data1;
                  shape.IsEquationShape = true;
                  if (this.m_enableAlternateContent)
                    shape.EnableAlternateContent = this.m_enableAlternateContent;
                  shape.IsAbsoluteAnchor = true;
                  shape.Left = (int) ApplicationImpl.ConvertToPixels(emuValue1, MeasureUnits.EMU);
                  shape.Top = (int) ApplicationImpl.ConvertToPixels(emuValue2, MeasureUnits.EMU);
                  shape.Width = (int) ApplicationImpl.ConvertToPixels(emuValue3, MeasureUnits.EMU);
                  shape.Height = (int) ApplicationImpl.ConvertToPixels(emuValue4, MeasureUnits.EMU);
                  break;
                }
                reader.Read();
              }
            }
            if (reader.LocalName == "pic")
            {
              ShapeImpl picture = this.ParsePicture(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
              picture.IsAbsoluteAnchor = true;
              picture.Left = (int) ApplicationImpl.ConvertToPixels(emuValue1, MeasureUnits.EMU);
              picture.Top = (int) ApplicationImpl.ConvertToPixels(emuValue2, MeasureUnits.EMU);
              picture.Width = (int) ApplicationImpl.ConvertToPixels(emuValue3, MeasureUnits.EMU);
              picture.Height = (int) ApplicationImpl.ConvertToPixels(emuValue4, MeasureUnits.EMU);
            }
            else if (reader.LocalName == "graphicData" || reader.LocalName == "sp" || reader.LocalName == "grpSp")
            {
              MemoryStream data2 = this.ReadSingleNodeIntoStream(reader);
              ShapeImpl chart = this.TryParseChart(data2, sheet, drawingsPath, false);
              if (chart == null)
              {
                ShapeImpl newShape = new ShapeImpl(sheet.Application, (object) sheet.InnerShapes);
                sheet.InnerShapes.AddShape(newShape);
                newShape.IsAbsoluteAnchor = true;
                if (newShape.preservedPictureStreams == null)
                  newShape.preservedPictureStreams = new List<Stream>();
                newShape.preservedPictureStreams.Add((Stream) data2);
                newShape.Left = (int) ApplicationImpl.ConvertToPixels(emuValue1, MeasureUnits.EMU);
                newShape.Top = (int) ApplicationImpl.ConvertToPixels(emuValue2, MeasureUnits.EMU);
                newShape.Width = (int) ApplicationImpl.ConvertToPixels(emuValue3, MeasureUnits.EMU);
                newShape.Height = (int) ApplicationImpl.ConvertToPixels(emuValue4, MeasureUnits.EMU);
              }
              else
              {
                chart.IsAbsoluteAnchor = true;
                (chart as ChartShapeImpl).ChartObject.EMUWidth = emuValue3;
                (chart as ChartShapeImpl).ChartObject.EMUHeight = emuValue4;
                (chart as IChart).XPos = emuValue1;
                (chart as IChart).YPos = emuValue2;
              }
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
          case "Fallback":
            this.ParseFallback(reader, sheet);
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
            this.ParseTwoCellAnchor(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove, false);
            continue;
          case "Fallback":
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
    reader.Read();
  }

  private void ParseChoice(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    OleObjects oleObjects = sheet != null ? (OleObjects) sheet.OleObjects : throw new ArgumentNullException(nameof (sheet));
    if (reader.MoveToAttribute("Requires"))
      oleObjects.Requries = reader.Value;
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
          case "legacyDrawing":
            this.ParseLegacyDrawing(reader, (WorksheetBaseImpl) sheet);
            continue;
          case "oleObject":
            OleObject oleObject = this.ParseOleObject(reader, sheet);
            oleObjects.Add(oleObject);
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

  private void ParseAfterSheetData(
    XmlReader reader,
    WorksheetImpl sheet,
    ref MemoryStream streamCF,
    string strParentPath,
    Dictionary<string, object> dictItemsToRemove)
  {
    streamCF = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) streamCF, Encoding.UTF8);
    writer.WriteStartElement("root", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
    while (!reader.EOF && reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "mergeCells":
            this.ParseMergedCells(reader, sheet);
            continue;
          case "phoneticPr":
            reader.Skip();
            continue;
          case "legacyDrawing":
            this.ParseLegacyDrawing(reader, (WorksheetBaseImpl) sheet);
            continue;
          case "oleObjects":
            this.ParseOleObjects(reader, (WorksheetBaseImpl) sheet);
            continue;
          case "legacyDrawingHF":
            Excel2007Parser.ParseLegacyDrawingHF(reader, (WorksheetBaseImpl) sheet, (RelationCollection) null);
            continue;
          case "drawing":
            this.ParseDrawings(reader, (WorksheetBaseImpl) sheet, dictItemsToRemove);
            continue;
          case "conditionalFormatting":
            writer.WriteNode(reader, false);
            continue;
          case "picture":
            this.ParseBackgroundImage(reader, (WorksheetBaseImpl) sheet, strParentPath);
            continue;
          case "dataValidations":
            this.ParseDataValidations(reader, sheet);
            continue;
          case "autoFilter":
            this.ParseAutoFilters(reader, sheet);
            continue;
          case "hyperlinks":
            this.ParseHyperlinks(reader, sheet);
            continue;
          case "printOptions":
            Excel2007Parser.ParsePrintOptions(reader, (IPageSetupBase) sheet.PageSetup);
            continue;
          case "pageMargins":
            bool notValidSettings = (sheet.PageSetup as PageSetupImpl).IsNotValidSettings;
            Excel2007Parser.ParsePageMargins(reader, (IPageSetupBase) sheet.PageSetup, (IPageSetupConstantsProvider) new WorksheetPageSetupConstants());
            (sheet.PageSetup as PageSetupImpl).IsNotValidSettings = notValidSettings;
            continue;
          case "pageSetup":
            Excel2007Parser.ParsePageSetup(reader, (PageSetupBaseImpl) sheet.PageSetup);
            continue;
          case "headerFooter":
            Excel2007Parser.ParseHeaderFooter(reader, (PageSetupBaseImpl) sheet.PageSetup);
            continue;
          case "rowBreaks":
            this.ParseHorizontalPagebreaks(reader, sheet);
            continue;
          case "colBreaks":
            this.ParseVerticalPagebreaks(reader, sheet);
            continue;
          case "customProperties":
            this.ParseCustomWorksheetProperties(reader, sheet);
            continue;
          case "ignoredErrors":
            this.ParseIgnoreError(reader, sheet);
            continue;
          case "sheetProtection":
            this.ParseSheetProtection(reader, (WorksheetBaseImpl) sheet, nameof (sheet));
            continue;
          case "AlternateContent":
            this.ParseAlternateContent(reader, sheet);
            continue;
          case "controls":
            this.ParseControls(reader, sheet);
            continue;
          case "tableParts":
            this.ParseTableParts(reader, sheet, strParentPath);
            continue;
          case "extLst":
            this.ParseExtensionlist(reader, sheet);
            continue;
          case "sortState":
            this.ParseSorting(reader, sheet);
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
    streamCF.Position = 0L;
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
            case "sparklineGroups":
              this.ParseSparklineGroups(sheet, reader);
              continue;
            case "slicerList":
              Stream stream = ShapeParser.ReadNodeAsStream(reader);
              sheet.WorksheetSlicerStream = stream;
              continue;
            case "conditionalFormattings":
              sheet.DataHolder.m_cfsStream = (Stream) new MemoryStream();
              sheet.DataHolder.m_cfsStream = ShapeParser.ReadNodeAsStream(reader);
              continue;
            case "dataValidations":
              this.m_hasDVExtlst = true;
              this.ParseDataValidations(reader, sheet);
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

  private void ParseSparklineGroups(WorksheetImpl sheet, XmlReader reader)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "sparklineGroups")
      throw new XmlException();
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "sparklineGroup":
            sheet.SparklineGroups.Add((ISparklineGroup) this.ParseSparklineGroup(reader, sheet));
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

  private SparklineGroup ParseSparklineGroup(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "sparklineGroup")
      throw new XmlException();
    SparklineGroup sparklineGroup = new SparklineGroup(sheet.ParentWorkbook);
    if (reader.MoveToAttribute("lineWeight"))
      sparklineGroup.LineWeight = XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute("type"))
    {
      switch (reader.Value)
      {
        case "line":
          sparklineGroup.SparklineType = SparklineType.Line;
          break;
        case "column":
          sparklineGroup.SparklineType = SparklineType.Column;
          break;
        case "stacked":
          sparklineGroup.SparklineType = SparklineType.ColumnStacked100;
          break;
      }
    }
    if (reader.MoveToAttribute("dateAxis"))
      sparklineGroup.HorizontalDateAxis = true;
    if (reader.MoveToAttribute("displayEmptyCellsAs"))
    {
      switch (reader.Value)
      {
        case "span":
          sparklineGroup.DisplayEmptyCellsAs = SparklineEmptyCells.Line;
          break;
        case "gap":
          sparklineGroup.DisplayEmptyCellsAs = SparklineEmptyCells.Gaps;
          break;
        case "zero":
          sparklineGroup.DisplayEmptyCellsAs = SparklineEmptyCells.Zero;
          break;
      }
    }
    if (reader.MoveToAttribute("markers"))
      sparklineGroup.ShowMarkers = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("high"))
      sparklineGroup.ShowHighPoint = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("low"))
      sparklineGroup.ShowLowPoint = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("first"))
      sparklineGroup.ShowFirstPoint = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("last"))
      sparklineGroup.ShowLastPoint = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("negative"))
      sparklineGroup.ShowNegativePoint = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("displayXAxis"))
      sparklineGroup.DisplayAxis = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("displayHidden"))
      sparklineGroup.DisplayHiddenRC = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("maxAxisType"))
    {
      switch (reader.Value)
      {
        case "individual":
          sparklineGroup.VerticalAxisMaximum.VerticalAxisOptions = SparklineVerticalAxisOptions.Automatic;
          break;
        case "group":
          sparklineGroup.VerticalAxisMaximum.VerticalAxisOptions = SparklineVerticalAxisOptions.Same;
          break;
        case "custom":
          sparklineGroup.VerticalAxisMaximum.VerticalAxisOptions = SparklineVerticalAxisOptions.Custom;
          break;
      }
    }
    if (reader.MoveToAttribute("minAxisType"))
    {
      switch (reader.Value)
      {
        case "individual":
          sparklineGroup.VerticalAxisMinimum.VerticalAxisOptions = SparklineVerticalAxisOptions.Automatic;
          break;
        case "group":
          sparklineGroup.VerticalAxisMinimum.VerticalAxisOptions = SparklineVerticalAxisOptions.Same;
          break;
        case "custom":
          sparklineGroup.VerticalAxisMinimum.VerticalAxisOptions = SparklineVerticalAxisOptions.Custom;
          break;
      }
    }
    if (reader.MoveToAttribute("manualMax") && sparklineGroup.VerticalAxisMaximum.VerticalAxisOptions != SparklineVerticalAxisOptions.Automatic)
      sparklineGroup.VerticalAxisMaximum.CustomValue = Convert.ToDouble(reader.Value);
    if (reader.MoveToAttribute("manualMin") && sparklineGroup.VerticalAxisMinimum.VerticalAxisOptions != SparklineVerticalAxisOptions.Automatic)
      sparklineGroup.VerticalAxisMinimum.CustomValue = Convert.ToDouble(reader.Value);
    if (reader.MoveToAttribute("rightToLeft"))
      sparklineGroup.PlotRightToLeft = true;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "colorSeries":
              sparklineGroup.SparklineColor = this.ParseColor(reader).GetRGB((IWorkbook) this.m_book);
              continue;
            case "colorNegative":
              sparklineGroup.NegativePointColor = this.ParseColor(reader).GetRGB((IWorkbook) this.m_book);
              continue;
            case "colorAxis":
              sparklineGroup.AxisColor = this.ParseColor(reader).GetRGB((IWorkbook) this.m_book);
              continue;
            case "colorMarkers":
              sparklineGroup.MarkersColor = this.ParseColor(reader).GetRGB((IWorkbook) this.m_book);
              continue;
            case "colorFirst":
              sparklineGroup.FirstPointColor = this.ParseColor(reader).GetRGB((IWorkbook) this.m_book);
              continue;
            case "colorLast":
              sparklineGroup.LastPointColor = this.ParseColor(reader).GetRGB((IWorkbook) this.m_book);
              continue;
            case "colorHigh":
              sparklineGroup.HighPointColor = this.ParseColor(reader).GetRGB((IWorkbook) this.m_book);
              continue;
            case "colorLow":
              sparklineGroup.LowPointColor = this.ParseColor(reader).GetRGB((IWorkbook) this.m_book);
              continue;
            case "f":
              bool isEmptyElement = reader.IsEmptyElement;
              if (!isEmptyElement)
                reader.Read();
              sparklineGroup.HorizontalDateAxisRange = sheet.Range[reader.Value];
              if (!isEmptyElement)
                reader.Skip();
              reader.Skip();
              continue;
            case "sparklines":
              sparklineGroup.Add((ISparklines) this.ParseSparklines(reader, sheet));
              (sparklineGroup[sparklineGroup.Count - 1] as Sparklines).ParentGroup = sparklineGroup;
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
    reader.Read();
    return sparklineGroup;
  }

  private Sparklines ParseSparklines(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "sparklines")
      throw new XmlException();
    Sparklines sparklines = new Sparklines();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "sparkline":
              sparklines.Add((ISparkline) this.ParseSparkline(reader, sheet));
              (sparklines[sparklines.Count - 1] as Sparkline).Parent = (ISparklines) sparklines;
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
    return sparklines;
  }

  private Sparkline ParseSparkline(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "sparkline")
      throw new XmlException();
    Sparkline sparkline = new Sparkline();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "f":
              bool isEmptyElement1 = reader.IsEmptyElement;
              if (!isEmptyElement1)
                reader.Read();
              sparkline.DataRange = sheet.Range[reader.Value];
              if (!isEmptyElement1)
                reader.Skip();
              reader.Skip();
              continue;
            case "sqref":
              bool isEmptyElement2 = reader.IsEmptyElement;
              if (!isEmptyElement2)
                reader.Read();
              sparkline.ReferenceRange = sheet.Range[reader.Value];
              if (!isEmptyElement2)
                reader.Skip();
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
    return sparkline;
  }

  private void ParseOleObjects(XmlReader reader, WorksheetBaseImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "oleObjects")
      throw new XmlException();
    if (!(sheet is WorksheetImpl sheet1))
      return;
    OleObjects oleObjects = (OleObjects) sheet1.OleObjects;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement && reader.LocalName != "extLst")
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "oleObject":
              OleObject oleObject = this.ParseOleObject(reader, sheet1);
              oleObjects.Add(oleObject);
              continue;
            case "AlternateContent":
              this.ParseAlternateContent(reader, sheet1);
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
    if (reader.NodeType != XmlNodeType.EndElement || !(reader.LocalName == "oleObjects"))
      return;
    reader.Read();
  }

  private void ParseFallback(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "oleObject":
            this.ParseFallbackOleObject(reader, sheet);
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

  private void ParseFallbackOleObject(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    if (!(reader.LocalName == "oleObject") || sheet.OleObjects == null || sheet.OleObjects.Count <= 0)
      return;
    OleObjects oleObjects = (OleObjects) sheet.OleObjects;
    OleObject oleObject = (OleObject) oleObjects[oleObjects.Count - 1];
    if (!reader.MoveToAttribute("shapeId"))
      return;
    oleObject.FallbackShapeId = reader.Value;
  }

  private OleObject ParseOleObject(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "oleObject")
      throw new XmlException();
    OleObject oleObject = new OleObject(sheet);
    if (reader.MoveToAttribute("progId"))
      oleObject.OleObjectType = OleTypeConvertor.ToOleType(reader.Value);
    if (reader.MoveToAttribute("dvAspect") && reader.Value == DVAspect.DVASPECT_ICON.ToString())
      oleObject.DisplayAsIcon = true;
    if (reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
    {
      string relationId = reader.Value;
      sheet.DataHolder.ParseOleData((WorksheetBaseImpl) sheet, relationId, oleObject);
      oleObject.ShapeRId = relationId;
      oleObject.OleType = OleLinkType.Embed;
    }
    else
    {
      if (!reader.MoveToAttribute("link"))
        throw new XmlException();
      oleObject.OleType = OleLinkType.Link;
      string externName = this.ParseExternName(reader.Value, oleObject);
      oleObject.FileName = externName;
    }
    if (reader.MoveToAttribute("shapeId"))
      oleObject.ShapeID = XmlConvertExtension.ToInt32(reader.Value);
    reader.Read();
    if (reader.LocalName != "oleObject")
    {
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "objectPr":
              this.ParseObjectPr(reader, sheet, oleObject);
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
      }
      reader.Read();
    }
    return oleObject;
  }

  private void ParseObjectPr(XmlReader reader, WorksheetImpl sheet, OleObject oleObject)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (oleObject == null)
      throw new ArgumentException(nameof (oleObject));
    if (reader.MoveToAttribute("defaultSize"))
      oleObject.DefaultSizeValue = reader.Value;
    if (reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      oleObject.ObjectPrRelationId = reader.Value;
    reader.Read();
    if (!(reader.LocalName != nameof (oleObject)))
      return;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "anchor":
            this.ParseAnchor(reader, sheet, oleObject);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    reader.Read();
  }

  private void ParseAnchor(XmlReader reader, WorksheetImpl sheet, OleObject oleObject)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (oleObject == null)
      throw new ArgumentException(nameof (oleObject));
    if (reader.MoveToAttribute("moveWithCells"))
      oleObject.MoveWithCellsValue = reader.Value;
    if (reader.MoveToAttribute("sizeWithCells"))
      oleObject.SizeWithCellsValue = reader.Value;
    reader.Read();
    if (!(reader.LocalName != nameof (oleObject)))
      return;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "from":
            reader.Skip();
            continue;
          case "to":
            reader.Skip();
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    reader.Read();
  }

  private string ParseExternName(string link, OleObject oleObject)
  {
    int length = link.IndexOf('!');
    string str1 = length >= 0 ? link.Substring(0, length) : throw new XmlException();
    string str2 = link.Substring(length + 1, link.Length - length - 1);
    string s = str1[0] == '[' && str1[str1.Length - 1] == ']' ? str1.Substring(1, str1.Length - 2) : throw new XmlException();
    if (str2[0] == '\'' && str2[str2.Length - 1] == '\'')
      str2 = str2.Substring(1, str2.Length - 2);
    int num = int.Parse(s);
    str2.Replace("''", "'");
    return this.m_book.ExternWorkbooks[num - 1].URL;
  }

  private void ParseTableParts(XmlReader reader, WorksheetImpl sheet, string sheetPath)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    List<string> stringList = new List<string>();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "tablePart":
              this.ParseTablePart(reader, sheet, sheetPath);
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

  private string ParseTablePart(XmlReader reader, WorksheetImpl sheet, string sheetPath)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    if (sheet == null)
      throw new ArgumentException(nameof (sheet));
    if (reader.LocalName != "tablePart")
      throw new XmlException();
    if (!reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      throw new XmlException();
    string strRelation = reader.Value;
    sheet.DataHolder.ParseTablePart((IWorksheet) sheet, strRelation, sheetPath);
    reader.MoveToElement();
    reader.Skip();
    return strRelation;
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
    data.Position = 0L;
    XmlReader reader1 = UtilityMethods.CreateReader((Stream) data);
    reader1.Read();
    if (!(reader1.LocalName != "control"))
      return;
    while (reader1.NodeType != XmlNodeType.EndElement)
    {
      if (reader1.NodeType == XmlNodeType.Element)
      {
        switch (reader1.LocalName)
        {
          case "AlternateContent":
            this.ParseControlsAlternateContent(reader1, sheet);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      reader1.Read();
    }
  }

  private void ParseControlsAlternateContent(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "Choice":
            this.ParseControlsChoice(reader, sheet);
            break;
          case "Fallback":
            reader.Skip();
            break;
          default:
            reader.Skip();
            break;
        }
      }
      if (reader.LocalName != "AlternateContent")
        reader.Read();
    }
  }

  private void ParseControlsChoice(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "control":
            this.ParseControl(reader, sheet);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      reader.Read();
    }
  }

  private void ParseControl(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    IShape shape = (IShape) null;
    if (reader.MoveToAttribute("shapeId"))
      shape = sheet.InnerShapes.GetShapeById(XmlConvertExtension.ToInt32(reader.Value));
    if (shape != null)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "controlPr":
              this.ParseControlPr(reader, shape as ShapeImpl);
              break;
            default:
              reader.Skip();
              break;
          }
        }
        reader.Read();
      }
    }
    else
      reader.Skip();
  }

  private void ParseControlPr(XmlReader reader, ShapeImpl shape)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    if (shape == null)
      throw new ArgumentException(nameof (shape));
    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "controlPr" && reader.MoveToAttribute("macro"))
    {
      if (string.IsNullOrEmpty(shape.OnAction))
        shape.OnAction = reader.Value;
      reader.MoveToElement();
    }
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "anchor":
            this.ParseAnchor(reader, shape);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      reader.Read();
    }
  }

  private void ParseAnchor(XmlReader reader, ShapeImpl shape)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    if (shape == null)
      throw new ArgumentException(nameof (reader));
    Rectangle fromRect = new Rectangle();
    Rectangle toRect = new Rectangle();
    double posX = 0.0;
    double posY = 0.0;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "from":
            fromRect = this.ParseAnchorValue(reader, false, ref posX, ref posY);
            this.m_drawingParser.leftColumn = fromRect.X;
            this.m_drawingParser.leftColumnOffset = Helper.ConvertEmuToOffset(fromRect.Width, this.dpiX);
            this.m_drawingParser.topRow = fromRect.Y;
            this.m_drawingParser.topRowOffset = Helper.ConvertEmuToOffset(fromRect.Height, this.dpiY);
            break;
          case "to":
            toRect = this.ParseAnchorValue(reader, false, ref posX, ref posY);
            this.m_drawingParser.rightColumn = toRect.X;
            this.m_drawingParser.rightColumnOffset = Helper.ConvertEmuToOffset(toRect.Width, this.dpiX);
            this.m_drawingParser.bottomRow = toRect.Y;
            this.m_drawingParser.bottomRowOffset = Helper.ConvertEmuToOffset(toRect.Height, this.dpiY);
            break;
          default:
            reader.Skip();
            break;
        }
      }
      reader.Read();
    }
    this.SetAnchor(shape, fromRect, toRect);
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
    ExcelSheetProtection excelSheetProtection = ExcelSheetProtection.None;
    ushort result = 0;
    if (reader.MoveToAttribute("password"))
      ushort.TryParse(reader.Value, NumberStyles.AllowHexSpecifier, (IFormatProvider) CultureInfo.InvariantCulture, out result);
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
      if (result == (ushort) 0)
        result = (ushort) 1;
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
      excelSheetProtection = protectionDelegate(reader, protectionAttributes[index], Protection.ProtectionFlags[index], Protection.DefaultValues[index], excelSheetProtection);
    sheet.Protect(result, excelSheetProtection);
    sheet.ProtectContents = flag;
    reader.Read();
  }

  private ExcelSheetProtection CheckChartProtectionAttribute(
    XmlReader reader,
    string attributeName,
    ExcelSheetProtection flag,
    bool defaultValue,
    ExcelSheetProtection protection)
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

  private ExcelSheetProtection CheckProtectionAttribute(
    XmlReader reader,
    string attributeName,
    ExcelSheetProtection flag,
    bool defaultValue,
    ExcelSheetProtection protection)
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
    ExcelIgnoreError options = ExcelIgnoreError.None;
    string strRange = (string) null;
    int i = 0;
    for (int attributeCount = reader.AttributeCount; i < attributeCount; ++i)
    {
      reader.MoveToAttribute(i);
      if (reader.LocalName == "sqref")
        strRange = reader.Value;
      else if (XmlConvertExtension.ToBoolean(reader.Value))
      {
        int index = Array.IndexOf<string>(Excel2007Serializator.ErrorTagsSequence, reader.LocalName);
        if (index >= 0)
          options |= Excel2007Serializator.ErrorsSequence[index];
      }
    }
    if (strRange == null)
      throw new XmlException();
    this.AddErrorIndicator(strRange, options, sheet);
    reader.MoveToElement();
    reader.Read();
  }

  private void AddErrorIndicator(string strRange, ExcelIgnoreError options, WorksheetImpl sheet)
  {
    ErrorIndicatorImpl errorIndicator = new ErrorIndicatorImpl(options);
    if (!string.IsNullOrEmpty(strRange))
    {
      string[] strArray = strRange.Split(' ');
      IWorkbook workbook = sheet.Workbook;
      int index = 0;
      for (int length = strArray.Length; index < length; ++index)
      {
        int iFirstRow;
        int iFirstColumn;
        int iLastRow;
        int iLastColumn;
        RangeImpl.ParseRangeString(strArray[index], workbook, out iFirstRow, out iFirstColumn, out iLastRow, out iLastColumn);
        Rectangle rect = Rectangle.FromLTRB(iFirstColumn - 1, iFirstRow - 1, iLastColumn - 1, iLastRow - 1);
        errorIndicator.AddRange(rect);
      }
    }
    sheet.ErrorIndicators.Add(errorIndicator);
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
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "customPr")
      throw new XmlException();
    string strName = reader.MoveToAttribute("name") ? reader.Value : throw new XmlException();
    if (!reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      throw new XmlException();
    string id = reader.Value;
    sheet.CustomProperties.Add(strName).Value = this.GetPropertyData(id, sheet.DataHolder);
    reader.MoveToElement();
    reader.Skip();
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
    if (!reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      throw new XmlException("Wrong xml format");
    string relationId = reader.Value;
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
    if (!reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      throw new XmlException("Wrong xml format");
    string relationId = reader.Value;
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
    Dictionary<string, object> dictItemsToRemove,
    bool isChartShape)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (lstRelationIds == null)
      throw new ArgumentNullException(nameof (lstRelationIds));
    bool bRelative = reader.LocalName == "relSizeAnchor";
    string editAs;
    if (reader.MoveToAttribute("editAs"))
    {
      editAs = reader.Value;
      this.m_drawingParser.placement = editAs;
    }
    else
      editAs = "twoCell";
    reader.Read();
    Rectangle fromRect = new Rectangle();
    Rectangle toRect = new Rectangle();
    ShapeImpl shapeImpl = (ShapeImpl) null;
    MemoryStream data = (MemoryStream) null;
    Size shapeExtent = new Size(-1, -1);
    double posX1 = 0.0;
    double posY1 = 0.0;
    double posX2 = 0.0;
    double posY2 = 0.0;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "from":
            fromRect = this.ParseAnchorPoint(reader, isChartShape, ref posX1, ref posY1);
            this.m_drawingParser.leftColumn = fromRect.X;
            this.m_drawingParser.leftColumnOffset = Helper.ConvertEmuToOffset(fromRect.Width, this.dpiX);
            this.m_drawingParser.topRow = fromRect.Y;
            this.m_drawingParser.topRowOffset = Helper.ConvertEmuToOffset(fromRect.Height, this.dpiY);
            continue;
          case "to":
            toRect = this.ParseAnchorPoint(reader, isChartShape, ref posX2, ref posY2);
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
            if (this.m_enableAlternateContent)
            {
              shapeImpl.EnableAlternateContent = this.m_enableAlternateContent;
              continue;
            }
            continue;
          case "clientData":
            if (shapeImpl != null && reader.MoveToAttribute("fLocksWithSheet"))
            {
              if (shapeImpl is AutoShapeImpl autoShapeImpl1)
                autoShapeImpl1.ShapeExt.LockWithSheet = XmlConvertExtension.ToBoolean(reader.Value);
              else
                shapeImpl.LockWithSheet = XmlConvertExtension.ToBoolean(reader.Value);
            }
            if (shapeImpl != null && reader.MoveToAttribute("fPrintsWithSheet"))
            {
              if (shapeImpl is AutoShapeImpl autoShapeImpl2)
                autoShapeImpl2.ShapeExt.PrintWithSheet = XmlConvertExtension.ToBoolean(reader.Value);
              else
                shapeImpl.PrintWithSheet = XmlConvertExtension.ToBoolean(reader.Value);
            }
            reader.Skip();
            continue;
          case "sp":
          case "cxnSp":
            this.m_drawingParser.preFix = reader.Prefix;
            this.m_drawingParser.shapeType = reader.LocalName;
            shapeImpl = this.CreateShape(reader, sheet, ref data, drawingsPath, lstRelationIds);
            if (this.m_enableAlternateContent)
            {
              shapeImpl.EnableAlternateContent = this.m_enableAlternateContent;
              continue;
            }
            continue;
          case "AlternateContent":
            shapeImpl = this.CreateShape(reader, sheet, ref data, drawingsPath, lstRelationIds);
            shapeImpl.XmlDataStream = (Stream) data;
            shapeImpl.IsEquationShape = true;
            if (this.m_enableAlternateContent)
            {
              shapeImpl.EnableAlternateContent = this.m_enableAlternateContent;
              continue;
            }
            continue;
          case "grpSp":
            shapeImpl = (ShapeImpl) this.ParseGroupShape(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
            if (shapeImpl != null && shapeImpl is GroupShapeImpl)
            {
              (shapeImpl as GroupShapeImpl).LayoutGroupShape();
              (shapeImpl as GroupShapeImpl).SetUpdatedChildOffset();
              continue;
            }
            continue;
          case "graphicFrame":
            data = this.ReadSingleNodeIntoStream(reader);
            shapeImpl = this.TryParseChart(data, sheet, drawingsPath, false);
            if (shapeImpl == null)
            {
              shapeImpl = new ShapeImpl(sheet.Application, (object) sheet.InnerShapes);
              sheet.InnerShapes.AddShape(shapeImpl);
              continue;
            }
            data = (MemoryStream) null;
            continue;
          default:
            data = this.ReadSingleNodeIntoStream(reader);
            shapeImpl = this.TryParseShape(data, sheet, drawingsPath, lstRelationIds);
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
      this.SetAnchor(shapeImpl, fromRect, toRect, shapeExtent, bRelative);
    if (isChartShape)
    {
      shapeImpl.StartX = posX1;
      shapeImpl.StartY = posY1;
      shapeImpl.ToX = posX2;
      shapeImpl.ToY = posY2;
    }
    if (shapeImpl is ChartShapeImpl chartShapeImpl && chartShapeImpl.ChartObject != null)
      this.CalculateShapesPosition(chartShapeImpl.ChartObject, (double) chartShapeImpl.Width, (double) chartShapeImpl.Height);
    if (shapeImpl.ShapeType != ExcelShapeType.AutoShape)
      shapeImpl.XmlDataStream = (Stream) data;
    this.ParseEditAsValue(shapeImpl, editAs);
    this.m_enableAlternateContent = false;
  }

  internal void CalculateShapesPosition(ChartImpl chartImpl, double width, double height)
  {
    if (chartImpl == null || chartImpl.Shapes.Count <= 0)
      return;
    for (int index = 0; index < chartImpl.Shapes.Count; ++index)
    {
      ShapeImpl shape = chartImpl.Shapes[index] as ShapeImpl;
      shape.ChartShapeX = shape.StartX * width;
      shape.ChartShapeY = shape.StartY * height;
      shape.ChartShapeWidth = shape.ToX * width - shape.StartX * width;
      shape.ChartShapeHeight = shape.ToY * height - shape.StartY * height;
      if (shape is GroupShapeImpl)
      {
        shape.ShapeFrame.OffsetX = (long) ApplicationImpl.ConvertFromPixel(shape.ChartShapeX, MeasureUnits.EMU);
        shape.ShapeFrame.OffsetY = (long) ApplicationImpl.ConvertFromPixel(shape.ChartShapeY, MeasureUnits.EMU);
        shape.ShapeFrame.OffsetCY = (long) ApplicationImpl.ConvertFromPixel(shape.ChartShapeHeight, MeasureUnits.EMU);
        shape.ShapeFrame.OffsetCX = (long) ApplicationImpl.ConvertFromPixel(shape.ChartShapeWidth, MeasureUnits.EMU);
      }
    }
  }

  internal GroupShapeImpl ParseGroupShape(
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
    List<ShapeImpl> shapeImplList = new List<ShapeImpl>();
    MemoryStream data1 = (MemoryStream) null;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    int num = 0;
    bool flag1 = false;
    ShapeImpl newShape = (ShapeImpl) null;
    ShapesCollection innerShapes = sheet.InnerShapes;
    GroupShapeImpl groupShape1 = new GroupShapeImpl(sheet.Application, (object) innerShapes);
    bool flag2 = false;
    MemoryStream data2 = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) data2, Encoding.UTF8);
    writer.WriteStartElement("root");
    reader.Read();
    while (!(reader.LocalName == "grpSp") || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "grpSp":
            ShapeImpl groupShape2 = (ShapeImpl) this.ParseGroupShape(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
            shapeImplList.Add(groupShape2);
            continue;
          case "nvGrpSpPr":
            reader.Read();
            continue;
          case "grpSpPr":
            writer.WriteNode(reader, false);
            writer.Flush();
            flag2 = true;
            continue;
          case "cNvPr":
            if (reader.LocalName == "cNvPr")
            {
              if (reader.MoveToAttribute("name"))
                empty1 = reader.Value;
              int result;
              if (reader.MoveToAttribute("id") && int.TryParse(reader.Value, out result))
                num = result;
              if (reader.MoveToAttribute("hidden"))
                flag1 = XmlConvertExtension.ToBoolean(reader.Value);
              if (reader.MoveToAttribute("descr"))
                empty2 = reader.Value;
            }
            reader.Read();
            continue;
          case "pic":
            newShape = this.ParsePicture(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
            newShape.SetPostion((newShape as BitmapShapeImpl).OffsetX, (newShape as BitmapShapeImpl).OffsetY, (newShape as BitmapShapeImpl).ExtentsX, (newShape as BitmapShapeImpl).ExtentsY);
            newShape.ShapeRotation = (newShape as BitmapShapeImpl).ShapeRotation;
            newShape.ShapeFrame.SetAnchor((newShape as BitmapShapeImpl).ShapeRotation * 60000, (newShape as BitmapShapeImpl).OffsetX, (newShape as BitmapShapeImpl).OffsetY, (newShape as BitmapShapeImpl).ExtentsX, (newShape as BitmapShapeImpl).ExtentsY);
            if (newShape != null)
            {
              shapeImplList.Add(newShape);
              continue;
            }
            continue;
          case "clientData":
            if (newShape != null && reader.MoveToAttribute("fLocksWithSheet"))
            {
              if (newShape is AutoShapeImpl autoShapeImpl1)
                autoShapeImpl1.ShapeExt.LockWithSheet = XmlConvertExtension.ToBoolean(reader.Value);
              else
                newShape.LockWithSheet = XmlConvertExtension.ToBoolean(reader.Value);
            }
            if (newShape != null && reader.MoveToAttribute("fPrintsWithSheet"))
            {
              if (newShape is AutoShapeImpl autoShapeImpl2)
                autoShapeImpl2.ShapeExt.PrintWithSheet = XmlConvertExtension.ToBoolean(reader.Value);
              else
                newShape.PrintWithSheet = XmlConvertExtension.ToBoolean(reader.Value);
            }
            reader.Skip();
            continue;
          case "sp":
          case "cxnSp":
            this.m_drawingParser = new DrawingParser();
            this.m_drawingParser.anchorName = "Group";
            this.m_drawingParser.preFix = reader.Prefix;
            this.m_drawingParser.shapeType = reader.LocalName;
            newShape = this.CreateShape(reader, sheet, ref data1, drawingsPath, lstRelationIds);
            if (newShape is TextBoxShapeImpl)
            {
              newShape.SetPostion((long) (newShape as TextBoxShapeImpl).Coordinates2007.X, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Y, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Width, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Height);
              newShape.ShapeFrame.SetAnchor(newShape.ShapeRotation * 60000, (long) (newShape as TextBoxShapeImpl).Coordinates2007.X, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Y, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Width, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Height);
            }
            else
            {
              newShape.SetPostion((long) this.m_drawingParser.posX, (long) this.m_drawingParser.posY, (long) this.m_drawingParser.extCX, (long) this.m_drawingParser.extCY);
              newShape.ShapeRotation = (int) this.m_drawingParser.shapeRotation;
              newShape.ShapeFrame.SetAnchor((int) this.m_drawingParser.shapeRotation * 60000, (long) this.m_drawingParser.posX, (long) this.m_drawingParser.posY, (long) this.m_drawingParser.extCX, (long) this.m_drawingParser.extCY);
            }
            if (this.m_enableAlternateContent)
              newShape.EnableAlternateContent = this.m_enableAlternateContent;
            if (newShape != null)
              shapeImplList.Add(newShape);
            if (newShape.ShapeType != ExcelShapeType.AutoShape)
            {
              newShape.XmlDataStream = (Stream) data1;
              continue;
            }
            (newShape as AutoShapeImpl).FlipHorizontal = this.m_drawingParser.FlipHorizontal;
            (newShape as AutoShapeImpl).FlipVertical = this.m_drawingParser.FlipVertical;
            continue;
          case "AlternateContent":
            this.m_drawingParser = new DrawingParser();
            this.m_drawingParser.anchorName = "Group";
            this.m_enableAlternateContent = true;
            while (reader.NodeType != XmlNodeType.EndElement)
            {
              if (reader.NodeType == XmlNodeType.Element)
              {
                switch (reader.LocalName)
                {
                  case "sp":
                    newShape = this.CreateShape(reader, sheet, ref data1, drawingsPath, lstRelationIds);
                    newShape.XmlDataStream = (Stream) data1;
                    newShape.IsEquationShape = true;
                    newShape.EnableAlternateContent = true;
                    if (newShape is TextBoxShapeImpl)
                    {
                      newShape.SetPostion((long) (newShape as TextBoxShapeImpl).Coordinates2007.X, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Y, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Width, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Height);
                      newShape.ShapeFrame.SetAnchor(newShape.ShapeRotation * 60000, (long) (newShape as TextBoxShapeImpl).Coordinates2007.X, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Y, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Width, (long) (newShape as TextBoxShapeImpl).Coordinates2007.Height);
                    }
                    else
                    {
                      newShape.SetPostion((long) this.m_drawingParser.posX, (long) this.m_drawingParser.posY, (long) this.m_drawingParser.extCX, (long) this.m_drawingParser.extCY);
                      newShape.ShapeRotation = (int) this.m_drawingParser.shapeRotation;
                      newShape.ShapeFrame.SetAnchor((int) this.m_drawingParser.shapeRotation * 60000, (long) this.m_drawingParser.posX, (long) this.m_drawingParser.posY, (long) this.m_drawingParser.extCX, (long) this.m_drawingParser.extCY);
                    }
                    if (newShape != null)
                    {
                      shapeImplList.Add(newShape);
                      newShape.XmlDataStream = (Stream) data1;
                    }
                    if (newShape.ShapeType == ExcelShapeType.AutoShape)
                    {
                      (newShape as AutoShapeImpl).FlipHorizontal = this.m_drawingParser.FlipHorizontal;
                      (newShape as AutoShapeImpl).FlipVertical = this.m_drawingParser.FlipVertical;
                      continue;
                    }
                    continue;
                  case "grpSp":
                    ShapeImpl groupShape3 = (ShapeImpl) this.ParseGroupShape(reader, sheet, drawingsPath, lstRelationIds, dictItemsToRemove);
                    shapeImplList.Add(groupShape3);
                    continue;
                  default:
                    reader.Read();
                    continue;
                }
              }
              else
                reader.Skip();
            }
            continue;
          case "graphicFrame":
            data1 = this.ReadSingleNodeIntoStream(reader);
            newShape = this.TryParseChart(data1, sheet, drawingsPath, false);
            if (newShape == null)
            {
              newShape = new ShapeImpl(sheet.Application, (object) sheet.InnerShapes);
              sheet.InnerShapes.AddShape(newShape);
            }
            else
              data1 = (MemoryStream) null;
            if (newShape != null)
            {
              shapeImplList.Add(newShape);
              continue;
            }
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Read();
    }
    if (reader.NodeType == XmlNodeType.EndElement)
      reader.Read();
    if (shapeImplList.Count > 0)
      groupShape1 = innerShapes.AddGroupShape(groupShape1, shapeImplList.ToArray());
    groupShape1.Name = empty1;
    groupShape1.ShapeId = num;
    groupShape1.IsShapeVisible = !flag1;
    groupShape1.AlternativeText = empty2;
    writer.WriteEndElement();
    writer.Flush();
    if (flag2)
    {
      data2.Position = 0L;
      XmlReader reader1 = UtilityMethods.CreateReader((Stream) data2);
      reader1.Read();
      if (reader1.NodeType == XmlNodeType.Element && reader1.LocalName == "grpSpPr" && !reader1.IsEmptyElement)
        this.ParseGroupShapeProperties(reader1, groupShape1);
      reader1.Close();
      data2.Dispose();
    }
    return groupShape1;
  }

  private ShapeImpl CreateShape(
    XmlReader reader,
    WorksheetBaseImpl sheet,
    ref MemoryStream data,
    string drawingsPath,
    List<string> lstRelationIds)
  {
    data = this.ReadSingleNodeIntoStream(reader);
    data.Position = 0L;
    reader = UtilityMethods.CreateReader((Stream) data);
    ExcelShapeType excelShapeType = ExcelShapeType.Unknown;
    ShapeImpl newShape = (ShapeImpl) null;
    string str1 = (string) null;
    int? nullable = new int?();
    string str2 = (string) null;
    string str3 = (string) null;
    bool flag1 = true;
    bool flag2 = false;
    string id = (string) null;
    string str4 = (string) null;
    if (reader.MoveToAttribute("macro"))
    {
      str3 = reader.Value;
      reader.MoveToElement();
    }
    if (reader.MoveToAttribute("textlink"))
      str2 = reader.Value;
    if (reader.MoveToAttribute("fLocksText"))
      flag1 = reader.ReadContentAsBoolean();
    while (reader.NodeType != XmlNodeType.None)
    {
      reader.Read();
      if (reader.LocalName == "Choice" && this.IsChartExChoice(reader))
      {
        reader.Read();
        newShape = this.TryParseChart(this.ReadSingleNodeIntoStream(reader), sheet, drawingsPath, true);
        excelShapeType = ExcelShapeType.Chart;
      }
      else if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "cNvSpPr")
        {
          if (reader.MoveToAttribute("txBox") && XmlConvertExtension.ToBoolean(reader.Value))
          {
            excelShapeType = ExcelShapeType.TextBox;
            reader.Read();
            if (reader.LocalName == "spLocks" && reader.MoveToAttribute("noChangeAspect"))
            {
              flag2 = reader.ReadContentAsBoolean();
              break;
            }
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
        if (reader.LocalName == "hlinkClick" && reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
        {
          id = reader.Value;
          if (reader.MoveToAttribute("tooltip"))
            str4 = reader.Value;
        }
        if (reader.LocalName == "xfrm")
        {
          if (reader.MoveToAttribute("rot"))
          {
            string s = reader.Value;
            if (s != null && s.Length > 0)
              this.m_drawingParser.shapeRotation = double.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture) / 60000.0;
          }
          else
            this.m_drawingParser.shapeRotation = 0.0;
          this.m_drawingParser.FlipHorizontal = reader.MoveToAttribute("flipH") && XmlConvertExtension.ToBoolean(reader.Value);
          this.m_drawingParser.FlipVertical = reader.MoveToAttribute("flipV") && XmlConvertExtension.ToBoolean(reader.Value);
          this.ParseForm(reader);
        }
        if ((reader.LocalName == "prstGeom" || reader.LocalName == "custGeom") && !this.m_enableAlternateContent)
        {
          if (reader.LocalName == "prstGeom")
          {
            string shapeString = "";
            if (reader.MoveToAttribute("prst"))
              shapeString = reader.Value;
            AutoShapeType autoShapeType = AutoShapeHelper.GetAutoShapeType(AutoShapeHelper.GetAutoShapeConstant(shapeString));
            if (autoShapeType != AutoShapeType.Unknown)
            {
              excelShapeType = ExcelShapeType.AutoShape;
              this.m_drawingParser.autoShapeType = autoShapeType;
              reader.MoveToElement();
              reader.Read();
            }
            Excel2007Parser.SkipWhiteSpaces(reader);
            if (reader.LocalName == "avLst")
              this.m_drawingParser.CustGeomStream = ShapeParser.ReadNodeAsStream(reader);
          }
          else if (reader.LocalName == "custGeom")
          {
            this.m_drawingParser.CustGeomStream = ShapeParser.ReadNodeAsStream(reader);
            this.m_drawingParser.m_isCustomGeom = true;
          }
        }
        if (reader.LocalName == "solidFill")
        {
          this.m_drawingParser.FillStream = ShapeParser.ReadNodeAsStream(reader);
          break;
        }
        if (reader.LocalName == "Fallback")
          reader.Skip();
      }
    }
    data.Position = 0L;
    reader = UtilityMethods.CreateReader((Stream) data);
    switch (excelShapeType)
    {
      case ExcelShapeType.Unknown:
        newShape = new ShapeImpl(sheet.Application, (object) sheet.InnerShapes);
        newShape.OnAction = str3;
        AutoShapeImpl autoShapeImpl1 = this.CheckShapeIsFreeForm(this.m_drawingParser.CustGeomStream, sheet, reader, lstRelationIds);
        if (autoShapeImpl1 != null)
          newShape = (ShapeImpl) autoShapeImpl1;
        if (nullable.HasValue)
          newShape.ShapeId = nullable.Value;
        if (str1 != null)
          newShape.Name = str1;
        if (this.m_drawingParser.m_isCustomGeom && this.m_drawingParser.CustGeomStream != null && this.m_drawingParser.CustGeomStream.Length > 0L)
        {
          newShape.PreservedElements.Add("custGeom", this.m_drawingParser.CustGeomStream);
          newShape.IsCustomGeometry = true;
        }
        if (this.m_drawingParser.FillStream != null && this.m_drawingParser.FillStream.Length > 0L)
          newShape.PreservedElements.Add("solidFill", this.m_drawingParser.FillStream);
        sheet.InnerShapes.AddShape(newShape);
        break;
      case ExcelShapeType.AutoShape:
        AutoShapeImpl autoShapeImpl2 = new AutoShapeImpl(sheet.Application, (object) sheet.InnerShapes);
        autoShapeImpl2.OnAction = str3;
        if (this.m_workSheet != null)
          this.m_drawingParser.AddShape(autoShapeImpl2, (WorksheetBaseImpl) this.m_workSheet);
        else
          this.m_drawingParser.AddShape(autoShapeImpl2, sheet);
        this.ParseAutoShape(autoShapeImpl2, reader, lstRelationIds);
        sheet.InnerShapes.Add((IShape) autoShapeImpl2);
        if (id != null)
        {
          autoShapeImpl2.ImageRelationId = id;
          IHyperLink hyperLink = (sheet as WorksheetImpl).HyperLinks.Add((IShape) autoShapeImpl2);
          WorksheetDataHolder dataHolder = autoShapeImpl2.Worksheet.DataHolder;
          RelationCollection drawingsRelations = dataHolder.DrawingsRelations;
          FileDataHolder parentHolder = dataHolder.ParentHolder;
          Relation relation = drawingsRelations[id];
          hyperLink.ScreenTip = str4;
          autoShapeImpl2.ImageRelation = relation;
          string strAddress = !relation.Target.StartsWith("mailto") ? Uri.UnescapeDataString(relation.Target) : relation.Target;
          if (strAddress.StartsWith("file:///"))
            strAddress = strAddress.Remove(0, "file:///".Length);
          hyperLink.Type = !relation.IsExternal ? ExcelHyperLinkType.Workbook : (!strAddress.StartsWith("\\\\") ? (strAddress.StartsWith("mailto") || strAddress.IndexOf("://") != -1 || strAddress.StartsWith("javascript:") || strAddress.ToLower().StartsWith("http:") || strAddress.ToLower().StartsWith("https:") ? ExcelHyperLinkType.Url : ExcelHyperLinkType.File) : ExcelHyperLinkType.Unc);
          (hyperLink as HyperLinkImpl).SetAddress(strAddress, false);
        }
        autoShapeImpl2.ShapeExt.Logger.ResetFlag();
        if (autoShapeImpl2.Fill.Visible && (autoShapeImpl2.Fill.FillType == ExcelFillType.Texture || autoShapeImpl2.Fill.FillType == ExcelFillType.Picture))
          autoShapeImpl2.Fill.Visible = true;
        autoShapeImpl2.ShapeExt.Macro = str3;
        autoShapeImpl2.ShapeExt.TextLink = str2;
        autoShapeImpl2.ShapeExt.LocksText = flag1;
        autoShapeImpl2.ShapeExt.NoChangeAspect = flag2;
        autoShapeImpl2.ShapeExt.Coordinates = new Rectangle(this.m_drawingParser.posX, this.m_drawingParser.posY, this.m_drawingParser.extCX, this.m_drawingParser.extCY);
        autoShapeImpl2.ShapeExt.ParentSheet = sheet;
        newShape = (ShapeImpl) autoShapeImpl2;
        break;
      case ExcelShapeType.Chart:
        if (nullable.HasValue)
          newShape.ShapeId = nullable.Value;
        if (str1 != null)
        {
          newShape.Name = str1;
          break;
        }
        break;
      case ExcelShapeType.TextBox:
        ITextBoxShapeEx textBoxShapeEx = sheet.Shapes.AddTextBox();
        (textBoxShapeEx as TextBoxShapeImpl).LocksText = flag1;
        (textBoxShapeEx as TextBoxShapeImpl).NoChangeAspect = flag2;
        if (str2 != null && str2.Length > 0)
          textBoxShapeEx.TextLink = $"={str2}";
        newShape = (ShapeImpl) textBoxShapeEx;
        TextBoxShapeParser.ParseTextBox((ITextBox) textBoxShapeEx, reader, this, lstRelationIds);
        if (nullable.HasValue)
          newShape.ShapeId = nullable.Value;
        if (str3 != null)
        {
          newShape.MacroName = str3;
          newShape.OnAction = str3;
          break;
        }
        break;
    }
    return newShape;
  }

  private AutoShapeImpl CheckShapeIsFreeForm(
    Stream custGeomStream,
    WorksheetBaseImpl sheet,
    XmlReader reader,
    List<string> lstRelationIds)
  {
    if (custGeomStream == null || custGeomStream.Length <= 0L)
      return (AutoShapeImpl) null;
    AutoShapeImpl autoShapeImpl = new AutoShapeImpl(sheet.Application, (object) sheet.InnerShapes);
    this.m_drawingParser.AddShape(autoShapeImpl, sheet);
    autoShapeImpl.ShapeType = ExcelShapeType.AutoShape;
    autoShapeImpl.IsCustomGeometry = true;
    this.ParseAutoShape(autoShapeImpl, reader, lstRelationIds);
    return autoShapeImpl;
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

  public void ParseAutoShape(AutoShapeImpl autoShape, XmlReader reader)
  {
    this.ParseAutoShape(autoShape, reader, (List<string>) null);
  }

  private void ParseAutoShape(
    AutoShapeImpl autoShape,
    XmlReader reader,
    List<string> lstRelationIds)
  {
    if (autoShape == null)
      throw new ArgumentNullException(nameof (autoShape));
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    bool flag = false;
    reader.Read();
    while (reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "txBody":
            Stream data1 = ShapeParser.ReadNodeAsStream(reader);
            data1.Position = 0L;
            autoShape.ShapeExt.PreservedElements.Add("TextBody", data1);
            Excel2007Parser.ParseRichText(UtilityMethods.CreateReader(data1), autoShape, this);
            continue;
          case "spPr":
            this.ParseProperties(reader, autoShape, lstRelationIds);
            continue;
          case "style":
            Stream data2 = ShapeParser.ReadNodeAsStream(reader);
            flag = true;
            autoShape.ShapeExt.PreservedElements.Add("Style", data2);
            XmlReader reader1 = UtilityMethods.CreateReader(data2, "lnRef");
            ShapeLineFormatImpl line = autoShape.Line as ShapeLineFormatImpl;
            int num = -1;
            if (reader1.MoveToAttribute("idx"))
              num = int.Parse(reader1.Value);
            if ((!autoShape.ShapeExt.PreservedElements.ContainsKey("Line") || !line.IsNoFill) && !line.IsSolidFill)
            {
              switch (num)
              {
                case -1:
                  break;
                case 0:
                  line.Visible = false;
                  break;
                default:
                  line.Visible = true;
                  break;
              }
            }
            line.DefaultLineStyleIndex = num;
            XmlReader reader2 = UtilityMethods.CreateReader(data2, "fillRef");
            if (!autoShape.IsNoFill && !autoShape.IsFill && reader2.MoveToAttribute("idx"))
            {
              autoShape.ShapeExt.Fill.Visible = int.Parse(reader2.Value) != 0;
              continue;
            }
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Read();
    }
    if (autoShape == null || flag || autoShape.IsFill || autoShape.IsNoFill)
      return;
    autoShape.ShapeExt.Fill.Visible = false;
  }

  private void ParseProperties(
    XmlReader reader,
    AutoShapeImpl autoShape,
    List<string> lstRelationIds)
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
            autoShape.IsNoFill = true;
            reader.Skip();
            continue;
          case "solidFill":
            Stream data1 = ShapeParser.ReadNodeAsStream(reader);
            data1.Position = 0L;
            autoShape.ShapeExt.PreservedElements.Add("Fill", data1);
            XmlReader reader1 = UtilityMethods.CreateReader(data1);
            IInternalFill fill1 = (IInternalFill) autoShape.ShapeExt.Fill;
            ChartParserCommon.ParseSolidFill(fill1, reader1, this, fill1.ForeColorObject);
            autoShape.IsFill = true;
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
            XmlReader reader2 = UtilityMethods.CreateReader(data3);
            IInternalFill fill2 = (IInternalFill) autoShape.ShapeExt.Fill;
            fill2.FillType = ExcelFillType.Gradient;
            fill2.PreservedGradient = ChartParserCommon.ParseGradientFill(reader2, this, autoShape.ShapeExt.Fill);
            if (fill2.PreservedGradient != null && fill2.PreservedGradient.Count > 2)
              fill2.GradientColorType = ExcelGradientColor.MultiColor;
            autoShape.IsFill = true;
            continue;
          case "blipFill":
            Stream data4 = ShapeParser.ReadNodeAsStream(reader);
            data4.Position = 0L;
            autoShape.ShapeExt.PreservedElements.Add("Fill", data4);
            ChartParserCommon.ParsePictureFill(UtilityMethods.CreateReader(data4), (IFill) (autoShape.Fill as IInternalFill), autoShape.Worksheet.DataHolder.DrawingsRelations, autoShape.ParentWorkbook.DataHolder, autoShape.ParentWorkbook, lstRelationIds);
            autoShape.Fill.Visible = true;
            autoShape.IsFill = true;
            continue;
          case "pattFill":
            Stream data5 = ShapeParser.ReadNodeAsStream(reader);
            data5.Position = 0L;
            autoShape.ShapeExt.PreservedElements.Add("Fill", data5);
            XmlReader reader3 = UtilityMethods.CreateReader(data5);
            IInternalFill fill3 = (IInternalFill) autoShape.ShapeExt.Fill;
            fill3.FillType = ExcelFillType.Pattern;
            ChartParserCommon.ParsePatternFill(reader3, (IFill) fill3, this);
            autoShape.IsFill = true;
            continue;
          case "grpFill":
            autoShape.ShapeExt.PreservedElements.Add("Fill", ShapeParser.ReadNodeAsStream(reader));
            autoShape.IsFill = true;
            autoShape.IsGroupFill = true;
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
          case "xfrm":
            if (reader.MoveToAttribute("rot"))
              autoShape.ShapeRotation = (int) Math.Round((double) long.Parse(reader.Value) / 60000.0);
            reader.Skip();
            continue;
          case "prstGeom":
            Excel2007Parser.ParsePresetGeomentry(UtilityMethods.CreateReader(ShapeParser.ReadNodeAsStream(reader)), autoShape);
            continue;
          case "custGeom":
            autoShape.ShapeExt.IsCustomGeometry = true;
            Excel2007Parser.ParseCustomGeometry(reader, autoShape);
            reader.Skip();
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

  private static void ParseCustomGeometry(XmlReader reader, AutoShapeImpl shape)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "pathLst":
            shape.ShapeExt.Path2DList = new List<Path2D>();
            ShapeParser.ParsePath2D(reader, shape.ShapeExt.Path2DList);
            reader.Skip();
            continue;
          default:
            if (!shape.ShapeExt.PreservedElements.ContainsKey(reader.LocalName))
            {
              shape.ShapeExt.PreservedElements.Add(reader.LocalName, ShapeParser.ReadNodeAsStream(reader));
              continue;
            }
            reader.Skip();
            continue;
        }
      }
      else
        reader.Skip();
    }
    Excel2007Parser.SetReaderPosition(reader);
  }

  internal static void SetReaderPosition(XmlReader reader)
  {
    while (reader.LocalName != "custGeom")
      reader.Read();
  }

  private static void ParsePresetGeomentry(XmlReader reader, AutoShapeImpl shape)
  {
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "avLst")
        {
          if (!reader.IsEmptyElement)
          {
            shape.ShapeExt.ShapeGuide = new Dictionary<string, string>();
            Excel2007Parser.ParseGeomGuideList(reader, shape);
            reader.Skip();
          }
          else
          {
            reader.Read();
            Excel2007Parser.SkipWhitespaces(reader);
            break;
          }
        }
      }
      else
        reader.Skip();
    }
  }

  internal static void SkipWhitespaces(XmlReader reader)
  {
    if (reader.NodeType == XmlNodeType.Element)
      return;
    while (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
  }

  private static void ParseGeomGuideList(XmlReader reader, AutoShapeImpl shape)
  {
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "gd")
        {
          string key = (string) null;
          string str = (string) null;
          if (reader.MoveToAttribute("name"))
            key = reader.Value;
          if (reader.MoveToAttribute("fmla"))
            str = reader.Value;
          if (key != null && str != null)
            shape.ShapeExt.ShapeGuide.Add(key, str);
          reader.Skip();
        }
      }
      else
        reader.Skip();
    }
  }

  private void ParseGroupShapeProperties(XmlReader reader, GroupShapeImpl groupShape)
  {
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "xfrm":
            if (reader.MoveToAttribute("rot"))
            {
              groupShape.ShapeRotation = (int) Math.Round((double) Convert.ToInt64(reader.Value) / 60000.0);
              reader.MoveToElement();
            }
            if (reader.MoveToAttribute("flipH"))
              groupShape.FlipHorizontal = XmlConvertExtension.ToBoolean(reader.Value);
            if (reader.MoveToAttribute("flipV"))
              groupShape.FlipVertical = XmlConvertExtension.ToBoolean(reader.Value);
            this.ParseForm(reader, groupShape);
            continue;
          case "noFill":
            groupShape.Fill.Visible = false;
            (groupShape.Fill as ShapeFillImpl).SetInnerShapesFillVisible();
            reader.Skip();
            continue;
          case "solidFill":
            Stream data1 = ShapeParser.ReadNodeAsStream(reader);
            data1.Position = 0L;
            XmlReader reader1 = UtilityMethods.CreateReader(data1);
            IInternalFill fill1 = groupShape.Fill as IInternalFill;
            ChartParserCommon.ParseSolidFill(reader1, this, fill1.ForeColorObject);
            (groupShape.Fill as ShapeFillImpl).SetInnerShapes((object) fill1.ForeColorObject, "ForeColorObject");
            continue;
          case "ln":
            Stream data2 = ShapeParser.ReadNodeAsStream(reader);
            data2.Position = 0L;
            TextBoxShapeParser.ParseLineProperties(UtilityMethods.CreateReader(data2), groupShape.Line as ShapeLineFormatImpl, false, this);
            continue;
          case "gradFill":
            Stream data3 = ShapeParser.ReadNodeAsStream(reader);
            data3.Position = 0L;
            XmlReader reader2 = UtilityMethods.CreateReader(data3);
            IInternalFill fill2 = groupShape.Fill as IInternalFill;
            fill2.FillType = ExcelFillType.Gradient;
            fill2.PreservedGradient = ChartParserCommon.ParseGradientFill(reader2, this, groupShape.Fill as ShapeFillImpl);
            if (fill2.PreservedGradient != null && fill2.PreservedGradient.Count > 2)
            {
              fill2.GradientColorType = ExcelGradientColor.MultiColor;
              continue;
            }
            continue;
          case "pattFill":
            Stream data4 = ShapeParser.ReadNodeAsStream(reader);
            data4.Position = 0L;
            XmlReader reader3 = UtilityMethods.CreateReader(data4);
            IInternalFill fill3 = groupShape.Fill as IInternalFill;
            fill3.FillType = ExcelFillType.Pattern;
            ChartParserCommon.ParsePatternFill(reader3, (IFill) fill3, this);
            (groupShape.Fill as ShapeFillImpl).SetInnerShapes((object) fill3.Pattern, "Pattern");
            (groupShape.Fill as ShapeFillImpl).SetInnerShapes((object) fill3.ForeColorObject, "ForeColorObject");
            (groupShape.Fill as ShapeFillImpl).SetInnerShapes((object) fill3.BackColorObject, "BackColorObject");
            continue;
          case "scene3d":
            groupShape.PreservedElements.Add("Scene3d", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "sp3d":
            groupShape.PreservedElements.Add("Sp3d", ShapeParser.ReadNodeAsStream(reader));
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
      else
        reader.Read();
    }
    if (reader.NodeType != XmlNodeType.EndElement)
      return;
    reader.Read();
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
    RichTextString richText = autoShape.ShapeExt.TextFrame.TextRange.RichText as RichTextString;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "bodyPr":
            Excel2007Parser.ParseBodyProperties(reader, autoShape.TextFrameInternal.TextBodyProperties);
            autoShape.TextFrameInternal.SetVisible();
            continue;
          case "lstStyle":
            TextBoxShapeParser.ParseListStyles(reader, richText);
            continue;
          case "p":
            Excel2007Parser.ParseParagraphs(reader, autoShape, parser, richText);
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

  internal static void ParseBodyProperties(
    XmlReader reader,
    TextBodyPropertiesHolder TextBodyProperties)
  {
    if (reader.MoveToAttribute("vertOverflow"))
      TextBodyProperties.TextVertOverflowType = Helper.GetVerticalFlowType(reader.Value);
    if (reader.MoveToAttribute("horzOverflow"))
      TextBodyProperties.TextHorzOverflowType = Helper.GetHorizontalFlowType(reader.Value);
    if (reader.MoveToAttribute("vert"))
      TextBodyProperties.TextDirection = Helper.SetTextDirection(reader.Value);
    if (reader.MoveToAttribute("wrap"))
      TextBodyProperties.WrapTextInShape = reader.Value != "none";
    if (reader.MoveToAttribute("lIns"))
    {
      TextBodyProperties.SetLeftMargin(Helper.ParseInt(reader.Value));
      TextBodyProperties.IsAutoMargins = false;
    }
    if (reader.MoveToAttribute("tIns"))
    {
      TextBodyProperties.SetTopMargin(Helper.ParseInt(reader.Value));
      TextBodyProperties.IsAutoMargins = false;
    }
    if (reader.MoveToAttribute("rIns"))
    {
      TextBodyProperties.SetRightMargin(Helper.ParseInt(reader.Value));
      TextBodyProperties.IsAutoMargins = false;
    }
    if (reader.MoveToAttribute("bIns"))
    {
      TextBodyProperties.SetBottomMargin(Helper.ParseInt(reader.Value));
      TextBodyProperties.IsAutoMargins = false;
    }
    if (reader.MoveToAttribute("numCol"))
      TextBodyProperties.Number = Helper.ParseInt(reader.Value);
    if (reader.MoveToAttribute("spcCol"))
      TextBodyProperties.SpacingPt = (int) ((double) Helper.ParseInt(reader.Value) / 12700.0);
    string anchorType = "t";
    bool anchorCtrl = false;
    if (reader.MoveToAttribute("anchor"))
      anchorType = reader.Value;
    if (reader.MoveToAttribute("anchorCtr"))
      anchorCtrl = XmlConvertExtension.ToBoolean(reader.Value);
    Helper.SetAnchorPosition(TextBodyProperties, anchorType, anchorCtrl);
    reader.MoveToElement();
    if (reader.LocalName == "bodyPr" && !reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "spAutoFit":
              TextBodyProperties.IsAutoSize = true;
              reader.Read();
              continue;
            case "prstTxWarp":
              if (reader.MoveToAttribute("prst"))
              {
                if (reader.Value == "textPlain")
                  TextBodyProperties.PresetWrapTextInShape = true;
                reader.Skip();
                continue;
              }
              continue;
            default:
              reader.Skip();
              continue;
          }
        }
        else
          Excel2007Parser.SkipWhiteSpaces(reader);
      }
      reader.Read();
    }
    else
      reader.Skip();
  }

  private static void ParseParagraphs(
    XmlReader reader,
    AutoShapeImpl autoShape,
    Excel2007Parser parser,
    RichTextString textArea)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoShape == null)
      throw new ArgumentNullException("textBox");
    if (reader.LocalName != "p")
      throw new XmlException("Unexpected xml tag.");
    string text = textArea.Text;
    if (text != null && text.Length != 0 && !text.EndsWith("\n"))
      textArea.AddText("\n", textArea.GetFont(text.Length - 1));
    reader.Read();
    BulletImpl bullet = (BulletImpl) null;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "pPr":
            Excel2007Parser.ParseParagraphProperites(reader, autoShape, textArea, out bullet);
            continue;
          case "r":
            TextBoxShapeParser.ParseParagraphRun(reader, textArea, parser, (ITextBox) null, bullet);
            continue;
          case "endParaRPr":
            TextBoxShapeParser.ParseParagraphEnd(reader, textArea, parser);
            continue;
          case "fld":
            TextBoxShapeParser.ParseTextField(reader, (ITextBox) null, textArea, parser);
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

  private static void ParseParagraphProperites(
    XmlReader reader,
    AutoShapeImpl autoShape,
    RichTextString textString,
    out BulletImpl bullet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoShape == null)
      throw new ArgumentNullException("textBox");
    if (reader.MoveToAttribute("algn"))
    {
      string str = reader.Value;
      switch (autoShape.TextFrameInternal.TextBodyProperties.TextDirection)
      {
        case TextDirection.RotateAllText90:
        case TextDirection.StackedRightToLeft:
          switch (str)
          {
            case "ctr":
              autoShape.TextFrame.VerticalAlignment = ExcelVerticalAlignment.Middle;
              break;
            case "r":
              autoShape.TextFrame.VerticalAlignment = ExcelVerticalAlignment.Top;
              break;
            case "l":
              autoShape.TextFrame.VerticalAlignment = ExcelVerticalAlignment.Bottom;
              break;
          }
          break;
        case TextDirection.RotateAllText270:
        case TextDirection.StackedLeftToRight:
          switch (str)
          {
            case "ctr":
              autoShape.TextFrame.VerticalAlignment = ExcelVerticalAlignment.Middle;
              break;
            case "r":
              autoShape.TextFrame.VerticalAlignment = ExcelVerticalAlignment.Bottom;
              break;
            case "l":
              autoShape.TextFrame.VerticalAlignment = ExcelVerticalAlignment.Top;
              break;
          }
          break;
        default:
          switch (str)
          {
            case "ctr":
              autoShape.TextFrame.HorizontalAlignment = ExcelHorizontalAlignment.Center;
              break;
            case "r":
              autoShape.TextFrame.HorizontalAlignment = ExcelHorizontalAlignment.Right;
              break;
            case "l":
              autoShape.TextFrame.HorizontalAlignment = ExcelHorizontalAlignment.Left;
              break;
          }
          break;
      }
    }
    if (reader.MoveToAttribute("lvl"))
    {
      if (textString != null)
        textString.StyleLevel = int.Parse(reader.Value);
    }
    else
      textString.StyleLevel = 0;
    bullet = (BulletImpl) null;
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
            case "buFont":
              bullet = new BulletImpl();
              TextBoxShapeParser.ParseBulletFont(reader, bullet);
              continue;
            case "buChar":
              if (reader.MoveToAttribute("char"))
                bullet.BulletChar = reader.Value;
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
              this.m_drawingParser.posX = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
            if (reader.MoveToAttribute("y"))
            {
              this.m_drawingParser.posY = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
              continue;
            }
            continue;
          case "ext":
            if (reader.MoveToAttribute("cx"))
              this.m_drawingParser.extCX = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
            if (reader.MoveToAttribute("cy"))
            {
              this.m_drawingParser.extCY = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
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

  private void ParseForm(XmlReader reader, BitmapShapeImpl picture)
  {
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "off":
              if (reader.MoveToAttribute("x"))
                picture.OffsetX = XmlConvertExtension.ToInt64(reader.Value);
              if (reader.MoveToAttribute("y"))
              {
                picture.OffsetY = XmlConvertExtension.ToInt64(reader.Value);
                continue;
              }
              continue;
            case "ext":
              if (reader.MoveToAttribute("cx"))
                picture.ExtentsX = XmlConvertExtension.ToInt64(reader.Value);
              if (reader.MoveToAttribute("cy"))
              {
                picture.ExtentsY = XmlConvertExtension.ToInt64(reader.Value);
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

  private void ParseForm(XmlReader reader, GroupShapeImpl groupShape)
  {
    int offsetX = 0;
    int offsetY = 0;
    int offsetCx = 0;
    int offsetCy = 0;
    int childOffsetX = 0;
    int childOffsetY = 0;
    int childOffsetCx = 0;
    int childOffsetCy = 0;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "off":
              if (reader.MoveToAttribute("x"))
                offsetX = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
              if (reader.MoveToAttribute("y"))
              {
                offsetY = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
                continue;
              }
              continue;
            case "ext":
              if (reader.MoveToAttribute("cx"))
                offsetCx = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
              if (reader.MoveToAttribute("cy"))
              {
                offsetCy = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
                continue;
              }
              continue;
            case "chOff":
              if (reader.MoveToAttribute("x"))
                childOffsetX = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
              if (reader.MoveToAttribute("y"))
              {
                childOffsetY = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
                continue;
              }
              continue;
            case "chExt":
              if (reader.MoveToAttribute("cx"))
                childOffsetCx = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
              if (reader.MoveToAttribute("cy"))
              {
                childOffsetCy = (double) int.MaxValue >= XmlConvertExtension.ToDouble(reader.Value) ? XmlConvertExtension.ToInt32(reader.Value) : int.MaxValue;
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
    groupShape.LeftDouble = ApplicationImpl.ConvertToPixels((double) offsetX, MeasureUnits.EMU) >= 0.0 ? ApplicationImpl.ConvertToPixels((double) offsetX, MeasureUnits.EMU) : 0.0;
    groupShape.TopDouble = ApplicationImpl.ConvertToPixels((double) offsetY, MeasureUnits.EMU) >= 0.0 ? ApplicationImpl.ConvertToPixels((double) offsetY, MeasureUnits.EMU) : 0.0;
    groupShape.WidthDouble = ApplicationImpl.ConvertToPixels((double) offsetCx, MeasureUnits.EMU) >= 0.0 ? ApplicationImpl.ConvertToPixels((double) offsetCx, MeasureUnits.EMU) : 0.0;
    groupShape.HeightDouble = ApplicationImpl.ConvertToPixels((double) offsetCy, MeasureUnits.EMU) >= 0.0 ? ApplicationImpl.ConvertToPixels((double) offsetCy, MeasureUnits.EMU) : 0.0;
    groupShape.ShapeFrame.SetAnchor(groupShape.ShapeRotation * 60000, (long) offsetX, (long) offsetY, (long) offsetCx, (long) offsetCy);
    groupShape.ShapeFrame.SetChildAnchor((long) childOffsetX, (long) childOffsetY, (long) childOffsetCx, (long) childOffsetCy);
    reader.Read();
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
    string str1 = "";
    if (reader.MoveToAttribute("macro"))
    {
      str1 = reader.Value;
      reader.MoveToElement();
    }
    reader.Read();
    string str2 = (string) null;
    string s1 = "0";
    string str3 = (string) null;
    bool flag = false;
    while (reader.LocalName != "chart" && reader.NodeType != XmlNodeType.None)
    {
      if (reader.LocalName == "cNvPr" && reader.MoveToAttribute("name"))
      {
        str2 = reader.Value;
        if (reader.MoveToAttribute("id"))
          s1 = reader.Value;
        if (reader.MoveToAttribute("descr"))
          str3 = reader.Value;
        flag = this.ParseBoolean(reader, "hidden", false);
      }
      if (reader.LocalName == "xfrm")
      {
        if (reader.MoveToAttribute("rot"))
        {
          string s2 = reader.Value;
          if (s2 != null && s2.Length > 0)
            this.m_drawingParser.shapeRotation = double.Parse(s2, (IFormatProvider) CultureInfo.InvariantCulture) / 60000.0;
        }
        this.ParseForm(reader);
      }
      reader.Read();
    }
    ChartShapeImpl chart = (ChartShapeImpl) null;
    if (reader.LocalName == "chart")
    {
      chart = (ChartShapeImpl) sheet.Charts.Add();
      chart.SetPostion((long) this.m_drawingParser.posX, (long) this.m_drawingParser.posY, (long) this.m_drawingParser.extCX, (long) this.m_drawingParser.extCY);
      chart.ShapeRotation = (int) this.m_drawingParser.shapeRotation;
      chart.ShapeFrame.SetAnchor((int) this.m_drawingParser.shapeRotation * 60000, (long) this.m_drawingParser.posX, (long) this.m_drawingParser.posY, (long) this.m_drawingParser.extCX, (long) this.m_drawingParser.extCY);
      ChartImpl chartObject = chart.ChartObject;
      WorksheetDataHolder dataHolder = sheet.DataHolder;
      FileDataHolder parentHolder = dataHolder.ParentHolder;
      RelationCollection drawingsRelations = dataHolder.DrawingsRelations;
      chartObject.DataHolder = dataHolder;
      this.ParseChartTag(reader, chartObject, drawingsRelations, parentHolder, drawingPath, isChartEx);
      chartObject.DataHolder = (WorksheetDataHolder) null;
      if (dataHolder.DrawingsRelations.Count == 0 && drawingsRelations.Count != 0)
        dataHolder.AssignDrawingrelation(drawingsRelations);
      if (!string.IsNullOrEmpty(str2))
        chart.Name = str2;
    }
    if (chart != null)
    {
      chart.ShapeId = int.Parse(s1);
      if (!string.IsNullOrEmpty(str3))
        chart.AlternativeText = str3;
      chart.IsShapeVisible = !flag;
      chart.OnAction = str1;
    }
    return (ShapeImpl) chart;
  }

  private ShapeImpl TryParseShape(
    MemoryStream data,
    WorksheetBaseImpl sheet,
    string drawingPath,
    List<string> lstRelationIds)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    data.Position = 0L;
    XmlReader reader = UtilityMethods.CreateReader((Stream) data);
    ShapeImpl parent = sheet.InnerShapes.AddShape(new ShapeImpl(sheet.Application, (object) sheet.InnerShapes));
    if (reader.LocalName == "grpSp")
      parent.preserveStreamOrder = new List<string>();
    reader.Read();
    foreach (KeyValuePair<string, Relation> dicRelation in sheet.DataHolder.DrawingsRelations.DicRelations)
    {
      string strItemPath;
      if (sheet.DataHolder.ParentHolder.GetItem(dicRelation.Value, drawingPath, out strItemPath) != null && !sheet.DataHolder.ParentHolder.DrawingsItemPath.Contains(strItemPath))
      {
        sheet.DataHolder.ParentHolder.DrawingsItemPath.Add(strItemPath);
        string correspondingRelations = FileDataHolder.GetCorrespondingRelations(strItemPath);
        sheet.DataHolder.ParentHolder.DrawingsItemPath.Add(correspondingRelations);
        this.AddRelations(sheet, drawingPath, correspondingRelations);
      }
    }
    string str = (string) null;
    string s = "0";
    ChartShapeImpl chartShapeImpl = (ChartShapeImpl) null;
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
          if (parent.preserveStreamOrder != null)
            parent.preserveStreamOrder.Add("grpSp-" + parent.preservedShapeStreams.Count.ToString());
          flag = true;
          break;
        case "nvGrpSpPr":
          Stream stream2 = (Stream) this.ReadSingleNodeIntoStream(reader);
          if (parent.preservedShapeStreams == null)
            parent.preservedShapeStreams = new List<Stream>();
          parent.preservedShapeStreams.Add(stream2);
          if (parent.preserveStreamOrder != null)
            parent.preserveStreamOrder.Add("nvGrpSpPr-" + parent.preservedShapeStreams.Count.ToString());
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
        case "AlternateContent":
          Stream stream3 = (Stream) this.ReadSingleNodeIntoStream(reader);
          if (parent.preservedShapeStreams == null)
            parent.preservedShapeStreams = new List<Stream>();
          parent.preservedShapeStreams.Add(stream3);
          if (parent.preserveStreamOrder != null)
            parent.preserveStreamOrder.Add("grpSpPr-" + parent.preservedShapeStreams.Count.ToString());
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
          if (parent.preserveStreamOrder != null)
            parent.preserveStreamOrder.Add("pic-" + parent.preservedPictureStreams.Count.ToString());
          this.RemoveRelations("nvPicPr", stream4, lstRelationIds);
          flag = true;
          break;
        case "sp":
          Stream stream5 = (Stream) this.ReadSingleNodeIntoStream(reader);
          if (parent.preservedShapeStreams == null)
            parent.preservedShapeStreams = new List<Stream>();
          parent.preservedShapeStreams.Add(stream5);
          if (parent.preserveStreamOrder != null)
            parent.preserveStreamOrder.Add("sp-" + parent.preservedShapeStreams.Count.ToString());
          this.RemoveRelations("nvSpPr", stream5, lstRelationIds);
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
            chartShapeImpl.ShapeId = int.Parse(s);
          if (parent.preserveStreamOrder != null)
          {
            parent.preserveStreamOrder.Add("chart-" + parent.ChildShapes.Count.ToString());
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
          if (parent.preserveStreamOrder != null)
            parent.preserveStreamOrder.Add("cxnSp-" + parent.preservedInnerCnxnShapeStreams.Count.ToString());
          flag = true;
          break;
      }
      if (!flag)
        reader.Read();
      flag = false;
    }
    return parent;
  }

  private void RemoveRelations(string childName, Stream stream, List<string> lstRelationIds)
  {
    if (stream == null)
      return;
    string str = (string) null;
    stream.Position = 0L;
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.Load(stream);
    foreach (XmlNode childNode in xmlDocument.DocumentElement.ChildNodes)
    {
      if (childNode.LocalName == childName)
      {
        IEnumerator enumerator1 = childNode.ChildNodes.GetEnumerator();
        try
        {
          while (enumerator1.MoveNext())
          {
            XmlNode current1 = (XmlNode) enumerator1.Current;
            if (current1.LocalName == "cNvPr")
            {
              IEnumerator enumerator2 = current1.ChildNodes.GetEnumerator();
              try
              {
                while (enumerator2.MoveNext())
                {
                  XmlNode current2 = (XmlNode) enumerator2.Current;
                  if (current2.LocalName == "hlinkClick")
                  {
                    str = current2.Attributes["r:id"].Value;
                    break;
                  }
                }
                break;
              }
              finally
              {
                if (enumerator2 is IDisposable disposable)
                  disposable.Dispose();
              }
            }
          }
          break;
        }
        finally
        {
          if (enumerator1 is IDisposable disposable)
            disposable.Dispose();
        }
      }
    }
    if (str == null)
      return;
    lstRelationIds.Remove(str);
  }

  private void AddRelations(WorksheetBaseImpl sheet, string drawingPath, string strRelation)
  {
    RelationCollection relations = sheet.DataHolder.ParentHolder.ParseRelations(strRelation);
    if (relations == null)
      return;
    foreach (KeyValuePair<string, Relation> dicRelation in relations.DicRelations)
    {
      string itemName = FileDataHolder.CombinePath(drawingPath, dicRelation.Value.Target);
      sheet.DataHolder.ParentHolder.DrawingsItemPath.Add(itemName);
      strRelation = FileDataHolder.GetCorrespondingRelations(itemName);
      sheet.DataHolder.ParentHolder.DrawingsItemPath.Add(strRelation);
      this.AddRelations(sheet, drawingPath, strRelation);
    }
  }

  private MemoryStream ReadSingleNodeIntoStream(XmlReader reader)
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
    int num1 = -1;
    int num2 = -1;
    if (reader.MoveToAttribute("cx"))
      num1 = int.Parse(reader.Value);
    if (reader.MoveToAttribute("cy"))
      num2 = int.Parse(reader.Value);
    return new Size((int) Math.Round(ApplicationImpl.ConvertToPixels((double) num1, MeasureUnits.EMU)), (int) Math.Round(ApplicationImpl.ConvertToPixels((double) num2, MeasureUnits.EMU)));
  }

  private void ParseEditAsValue(ShapeImpl shape, string editAs)
  {
    if (editAs == null)
      return;
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    switch (editAs)
    {
      case "oneCell":
        shape.IsMoveWithCell = true;
        shape.IsSizeWithCell = false;
        break;
      case "absolute":
        shape.IsMoveWithCell = false;
        shape.IsSizeWithCell = false;
        break;
      default:
        shape.IsMoveWithCell = true;
        shape.IsSizeWithCell = true;
        break;
    }
  }

  private void SetAnchor(
    ShapeImpl shape,
    Rectangle fromRect,
    Rectangle toRect,
    Size shapeExtent,
    bool bRelative)
  {
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    MsofbtClientAnchor msofbtClientAnchor = this.SetAnchor(shape, fromRect, toRect);
    if (shapeExtent.Width < 0)
    {
      shape.UpdateHeight();
      shape.UpdateWidth();
    }
    else
    {
      shape.Width = shapeExtent.Width;
      shape.Height = shapeExtent.Height;
      msofbtClientAnchor.OneCellAnchor = true;
    }
  }

  private MsofbtClientAnchor SetAnchor(ShapeImpl shape, Rectangle fromRect, Rectangle toRect)
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
    return clientAnchor;
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
      if (rowHeightInPixels == 0)
        rowHeightInPixels = (sheet as WorksheetImpl).GetHiddenRowHeightInPixels(iRowIndex);
      num1 = rowHeightInPixels != 0 ? ((double) rowHeightInPixels < pixels ? 256 /*0x0100*/ : (int) Math.Round(pixels * 256.0 / (double) rowHeightInPixels)) : 0;
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
      if (columnWidthInPixels == 0)
        columnWidthInPixels = (sheet as WorksheetImpl).GetHiddenColumnWidthInPixels(iColumnIndex);
      num2 = columnWidthInPixels != 0 ? (int) Math.Round(pixels * 1024.0 / (double) columnWidthInPixels) : 0;
    }
    anchorPoint.Width = num2;
    anchorPoint.Height = num1;
    return anchorPoint;
  }

  private Rectangle ParseAnchorPoint(
    XmlReader reader,
    bool isChartShape,
    ref double posX,
    ref double posY)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    Rectangle anchorValue = this.ParseAnchorValue(reader, isChartShape, ref posX, ref posY);
    reader.Read();
    return anchorValue;
  }

  private Rectangle ParseAnchorValue(
    XmlReader reader,
    bool isChartShape,
    ref double posX,
    ref double posY)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    Rectangle anchorValue = new Rectangle();
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
            anchorValue.X = XmlConvertExtension.ToInt32(s);
            continue;
          case "colOff":
            anchorValue.Width = XmlConvertExtension.ToInt32(s);
            continue;
          case "row":
            anchorValue.Y = XmlConvertExtension.ToInt32(s);
            continue;
          case "rowOff":
            anchorValue.Height = XmlConvertExtension.ToInt32(s);
            continue;
          case "x":
            posX = XmlConvertExtension.ToDouble(s);
            anchorValue.X = (int) (XmlConvertExtension.ToDouble(s) * 1000.0);
            continue;
          case "y":
            posY = XmlConvertExtension.ToDouble(s);
            anchorValue.Y = (int) (XmlConvertExtension.ToDouble(s) * 1000.0);
            continue;
          default:
            throw new XmlException("Unexpected xml tag.");
        }
      }
      else
        reader.Skip();
    }
    return anchorValue;
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
      shape.OnAction = reader.Value;
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
    BitmapShapeImpl picture = shape as BitmapShapeImpl;
    reader.Read();
    if (picture == null)
      return;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "xfrm":
            if (reader.MoveToAttribute("rot"))
            {
              picture.ShapeRotation = (int) Math.Round((double) Convert.ToInt64(reader.Value) / 60000.0);
              reader.MoveToElement();
            }
            if (reader.MoveToAttribute("flipH"))
              picture.FlipHorizontal = XmlConvertExtension.ToBoolean(reader.Value);
            if (reader.MoveToAttribute("flipV"))
              picture.FlipVertical = XmlConvertExtension.ToBoolean(reader.Value);
            this.ParseForm(reader, picture);
            continue;
          case "prstGeom":
            picture.PreservedElements.Add("prstGeom", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "custGeom":
            picture.PreservedElements.Add("custGeom", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "blipFill":
            picture.Fill.Visible = true;
            picture.PreservedElements.Add("blipFill", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "noFill":
            picture.Fill.Visible = false;
            reader.Read();
            continue;
          case "solidFill":
            picture.Fill.FillType = ExcelFillType.SolidColor;
            picture.Fill.Visible = true;
            picture.PreservedElements.Add("solidFill", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "gradFill":
            picture.Fill.FillType = ExcelFillType.Gradient;
            picture.PreservedElements.Add("gradFill", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "pattFill":
            picture.Fill.FillType = ExcelFillType.Pattern;
            picture.PreservedElements.Add("pattFill", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "grpFill":
            picture.Fill.Visible = true;
            picture.PreservedElements.Add("grpFill", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "ln":
            ShapeLineFormatImpl line = (ShapeLineFormatImpl) shape.Line;
            TextBoxShapeParser.ParseLineProperties(reader, line, this);
            continue;
          case "effectLst":
            picture.PreservedElements.Add("effectLst", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "effectTag":
            picture.PreservedElements.Add("effectTag", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "scene3d":
            picture.PreservedElements.Add("scene3d", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "sp3d":
            picture.PreservedElements.Add("sp3d", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "extLst":
            picture.PreservedElements.Add("ShapePropertiesExtensionList", ShapeParser.ReadNodeAsStream(reader));
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
    if (reader.MoveToAttribute("embed", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
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
    if (reader.MoveToAttribute("link", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
    {
      string id = reader.Value;
      Relation relation = relations[id];
      lstRelationIds.Add(id);
      shape.ExternalLink = relation != null ? relation.Target : throw new XmlException("Cannot find required relation");
    }
    reader.MoveToElement();
    this.ParseBlipImage(reader, shape, relations, strParentPath, holder, lstRelationIds);
    reader.Skip();
  }

  private void ParseBlipImage(
    XmlReader reader,
    BitmapShapeImpl picture,
    RelationCollection relations,
    string strParentPath,
    FileDataHolder holder,
    List<string> lstRelationIds)
  {
    if (reader.IsEmptyElement)
      return;
    string localName1 = reader.LocalName;
    string empty = string.Empty;
    reader.Read();
    while (!(reader.LocalName == localName1))
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        bool flag = false;
        switch (reader.LocalName)
        {
          case "alphaBiLevel":
            picture.PreservedElements.Add("alphaBiLevel", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "alphaCeiling":
            picture.PreservedElements.Add("alphaCeiling", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "alphaFloor":
            picture.PreservedElements.Add("alphaFloor", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "grayscl":
            picture.GrayScale = true;
            reader.Skip();
            break;
          case "fillOverlay":
            picture.PreservedElements.Add("fillOverlay", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "alphaMod":
            picture.PreservedElements.Add("alphaMod", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "alphaInv":
            picture.PreservedElements.Add("alphaInv", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "alphaRepl":
            picture.PreservedElements.Add("alphaRepl", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "biLevel":
            if (reader.MoveToAttribute("thresh"))
              picture.Threshold = Convert.ToInt32(reader.Value);
            reader.Skip();
            break;
          case "blur":
            picture.PreservedElements.Add("blur", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "clrChange":
            picture.HasTransparency = true;
            this.ParseColorChangeEffect(reader, picture);
            reader.Skip();
            break;
          case "hsl":
            picture.PreservedElements.Add("hsl", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "lum":
            picture.PreservedElements.Add("lum", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "tint":
            picture.PreservedElements.Add("tint", ShapeParser.ReadNodeAsStream(reader));
            break;
          case "duotone":
            string localName2 = reader.LocalName;
            reader.Read();
            while (!(localName2 == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
            {
              Excel2007Parser.SkipWhitespaces(reader);
              if (reader.NodeType == XmlNodeType.Element)
              {
                ColorObject color = new ColorObject(ColorExtension.Empty);
                int Alpha = 100000;
                ChartParserCommon.ParseColorObject(reader, this, color, out Alpha);
                picture.DuoTone.Add(color);
              }
            }
            reader.Read();
            break;
          case "extLst":
            this.ParseBlipExtensionList(reader, picture, relations, strParentPath, holder, lstRelationIds);
            flag = true;
            break;
          case "alphaModFix":
            if (reader.MoveToAttribute("amt"))
              picture.Amount = Convert.ToInt32(reader.Value);
            reader.Skip();
            break;
        }
        if (flag)
          reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseColorChangeEffect(XmlReader reader, BitmapShapeImpl picture)
  {
    if (reader.MoveToAttribute("useA"))
    {
      picture.IsUseAlpha = XmlConvertExtension.ToBoolean(reader.Value);
      reader.MoveToElement();
    }
    if (reader.IsEmptyElement)
      return;
    string localName = reader.LocalName;
    reader.Read();
    while (!(localName == reader.LocalName) || reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "clrFrom":
          case "clrTo":
            reader.Read();
            Excel2007Parser.SkipWhitespaces(reader);
            ColorObject color = new ColorObject(ColorExtension.Empty);
            int Alpha = 100000;
            ChartParserCommon.ParseColorObject(reader, this, color, out Alpha);
            Color rgb = color.GetRGB(picture.Workbook);
            color.SetRGB(Color.FromArgb((int) (byte) ((double) Alpha / 100000.0 * (double) byte.MaxValue), (int) rgb.R, (int) rgb.G, (int) rgb.B), picture.Workbook);
            picture.ColorChange.Add(color);
            reader.Read();
            Excel2007Parser.SkipWhitespaces(reader);
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
  }

  private void ParseBlipExtensionList(
    XmlReader reader,
    BitmapShapeImpl picture,
    RelationCollection relations,
    string strParentPath,
    FileDataHolder holder,
    List<string> lstRelationIds)
  {
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.LocalName != "extLst")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ext":
            this.ParseImageProperties(reader, picture, relations, strParentPath, holder, lstRelationIds);
            break;
        }
        reader.Read();
      }
      else
        reader.Read();
    }
  }

  private void ParseImageProperties(
    XmlReader reader,
    BitmapShapeImpl picture,
    RelationCollection relations,
    string strParentPath,
    FileDataHolder holder,
    List<string> lstRelationIds)
  {
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.LocalName != "ext")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "imgProps":
            picture.PreservedElements.Add("imgProps", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "useLocalDpi":
            picture.PreservedElements.Add("useLocalDpi", ShapeParser.ReadNodeAsStream(reader));
            continue;
          case "svgBlip":
            this.ReadSvgData(reader, picture, relations, strParentPath, holder, lstRelationIds);
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

  private void ReadSvgData(
    XmlReader reader,
    BitmapShapeImpl picture,
    RelationCollection relations,
    string strParentPath,
    FileDataHolder holder,
    List<string> lstRelationIds)
  {
    bool flag = false;
    string attribute = reader.GetAttribute("embed", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
    if (reader.MoveToAttribute("link", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
    {
      attribute = reader.Value;
      flag = true;
    }
    reader.MoveToElement();
    if (string.IsNullOrEmpty(attribute))
      return;
    Relation relation = relations[attribute];
    lstRelationIds.Add(attribute);
    if (relation == null)
      throw new XmlException("Cannot find required relation");
    ZipArchiveItem zipArchiveItem = holder[relation, strParentPath];
    if (relation.Target != "NULL")
    {
      if (flag)
      {
        picture.ExternalLink = relation.Target;
        picture.IsSvgExternalLink = true;
      }
      else
      {
        picture.SvgPicturePath = zipArchiveItem.ItemName;
        picture.SvgData = zipArchiveItem.DataStream;
      }
      picture.SvgRelId = attribute;
    }
    reader.Read();
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
            Excel2007Parser.ParseNVCanvasProperties(reader, shape, relations, strParentPath, holder, lstRelationIds, dictItemsToRemove);
            continue;
          case "cNvPicPr":
            this.ParseNVPictureCanvas(reader, shape);
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
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "cNvPicPr")
      {
        if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "extLst")
          this.ParseNVPictureCanvasExtension(reader, shape);
        else
          reader.Skip();
      }
      reader.Read();
    }
    else
      reader.Skip();
  }

  private void ParseNVPictureCanvasExtension(XmlReader reader, ShapeImpl shape)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (shape == null)
      throw new ArgumentNullException(nameof (shape));
    if (reader.LocalName != "extLst")
      throw new XmlException("Unexpected xml tag.");
    reader.Read();
    BitmapShapeImpl bitmapShapeImpl = shape as BitmapShapeImpl;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "ext")
      {
        reader.Read();
        if (reader.NodeType != XmlNodeType.EndElement && reader.LocalName == "cameraTool")
        {
          bitmapShapeImpl.Camera = new CameraTool();
          if (reader.MoveToAttribute("cellRange"))
            bitmapShapeImpl.Camera.CellRange = reader.Value;
          if (reader.MoveToAttribute("spid"))
          {
            string shapeId = reader.Value;
            bitmapShapeImpl.Camera.ShapeID = this.GetShapeId(shapeId);
          }
          reader.Skip();
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
  }

  private int GetShapeId(string shapeId)
  {
    int result = shapeId.IndexOf("_s");
    int shapeId1 = -1;
    if (result >= 0 && int.TryParse(shapeId.Substring(result + 2), out result))
      shapeId1 = result;
    return shapeId1;
  }

  public static void ParseNVCanvasProperties(XmlReader reader, IShape shape)
  {
    Excel2007Parser.ParseNVCanvasProperties(reader, shape as ShapeImpl, (RelationCollection) null, (string) null, (FileDataHolder) null, (List<string>) null, (Dictionary<string, object>) null);
  }

  internal static void ParseNVCanvasProperties(
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
    if (reader.LocalName != "cNvPr")
      throw new XmlException("Unexpected xml tag.");
    shape.IsShapeVisible = true;
    if (reader.MoveToAttribute("id"))
      shape.ShapeId = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("name"))
      shape.Name = reader.Value;
    if (reader.MoveToAttribute("descr"))
      shape.AlternativeText = reader.Value;
    if (reader.MoveToAttribute("hidden"))
      shape.IsShapeVisible = !XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement && relations != null && strParentPath != null && holder != null && lstRelationIds != null && dictItemsToRemove != null)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          Excel2007Parser.SkipWhiteSpaces(reader);
          switch (reader.LocalName)
          {
            case "hlinkClick":
              Excel2007Parser.ParseClickHyperlink(reader, shape, relations, strParentPath, holder, lstRelationIds, dictItemsToRemove, (shape.Worksheet as IWorksheet).HyperLinks);
              continue;
            case "extLst":
              if (reader.NodeType == XmlNodeType.Element)
              {
                MemoryStream data = new MemoryStream();
                XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
                writer.WriteStartElement("root");
                while (reader.NodeType != XmlNodeType.EndElement)
                  writer.WriteNode(reader, false);
                writer.WriteEndElement();
                writer.Flush();
                shape.NvPrExtLstStream = (Stream) data;
                continue;
              }
              continue;
            default:
              reader.Read();
              continue;
          }
        }
        else
          reader.Skip();
      }
    }
    else
      reader.Read();
    if (reader.NodeType != XmlNodeType.EndElement)
      return;
    reader.Read();
  }

  private static void ParseClickHyperlink(
    XmlReader reader,
    ShapeImpl shape,
    RelationCollection relations,
    string strParentPath,
    FileDataHolder holder,
    List<string> lstRelationIds,
    Dictionary<string, object> dictItemsToRemove,
    IHyperLinks hyperLinks)
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
    IHyperLink hyperLink = hyperLinks.Add((IShape) shape);
    if (reader.MoveToAttribute("id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
    {
      string id = reader.Value;
      Relation relation = relations[id];
      lstRelationIds.Add(id);
      if (reader.MoveToAttribute("tooltip"))
        hyperLink.ScreenTip = reader.Value;
      if (relation == null)
        return;
      shape.ImageRelation = relation;
      string strAddress = !relation.Target.StartsWith("mailto") ? Uri.UnescapeDataString(relation.Target) : relation.Target;
      if (strAddress.StartsWith("file:///"))
        strAddress = strAddress.Remove(0, "file:///".Length);
      hyperLink.Type = !relation.IsExternal ? ExcelHyperLinkType.Workbook : (!strAddress.StartsWith("\\\\") ? (strAddress.StartsWith("mailto") || strAddress.IndexOf("://") != -1 || strAddress.StartsWith("javascript:") || strAddress.ToLower().StartsWith("http:") || strAddress.ToLower().StartsWith("https:") ? ExcelHyperLinkType.Url : ExcelHyperLinkType.File) : ExcelHyperLinkType.Unc);
      (hyperLink as HyperLinkImpl).SetAddress(strAddress, false);
      reader.Skip();
    }
    else
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
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (authors == null)
      throw new ArgumentNullException(nameof (authors));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "comment")
      throw new XmlException("Unexpected xml tag");
    string name = reader.MoveToAttribute("ref") ? reader.Value : throw new XmlException();
    int index = reader.MoveToAttribute("authorId") ? int.Parse(reader.Value) : throw new XmlException();
    CommentShapeImpl commentShapeImpl = (CommentShapeImpl) sheet[name].AddComment();
    commentShapeImpl.Author = authors[index];
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    if (reader.LocalName != "text")
      throw new XmlException("Unexpected xml tag");
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "text":
            if (!reader.IsEmptyElement)
              reader.Read();
            TextWithFormat textWithFormat = this.ParseTextWithFormat(reader, "text");
            commentShapeImpl.SetText(textWithFormat);
            reader.Read();
            continue;
          case "AlternateContent":
            reader.Skip();
            continue;
          default:
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
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
    string parentItemPath,
    ShapeCollectionBase shapes,
    bool isShapeTypePresent)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dictShapeIdToShape == null)
      throw new ArgumentNullException(nameof (dictShapeIdToShape));
    if (parentItemPath == null || parentItemPath.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (parentItemPath));
    string key = reader.MoveToAttribute("type") ? UtilityMethods.RemoveFirstCharUnsafe(reader.Value) : throw new XmlException();
    ShapeImpl defaultShape;
    if (!dictShapeIdToShape.TryGetValue(key, out defaultShape) && isShapeTypePresent)
    {
      reader.Skip();
    }
    else
    {
      ShapeParser shapeParser1 = (ShapeParser) null;
      int num = -1;
      if (reader.MoveToAttribute("spt"))
      {
        num = int.Parse(reader.Value);
        if (!isShapeTypePresent && this.m_dictShapeParsers.TryGetValue(num, out shapeParser1))
        {
          defaultShape = shapeParser1.CreateShape(shapes);
          defaultShape.SetInstance(num);
        }
      }
      else if (defaultShape != null)
        num = defaultShape.InnerSpRecord.Instance;
      else if (!isShapeTypePresent && reader.MoveToAttribute("type"))
      {
        Regex regex = new Regex("\\d+$");
        if (regex.IsMatch(reader.Value))
        {
          num = int.Parse(regex.Match(reader.Value).Value);
          if (this.m_dictShapeParsers.TryGetValue(num, out shapeParser1))
          {
            defaultShape = shapeParser1.CreateShape(shapes);
            defaultShape.SetInstance(num);
          }
        }
      }
      if (shapeParser1 == null && !this.m_dictShapeParsers.TryGetValue(num, out shapeParser1))
        throw new XmlException();
      reader.MoveToElement();
      MemoryStream data = new MemoryStream();
      XmlWriter writer = UtilityMethods.CreateWriter((Stream) data, Encoding.UTF8);
      writer.WriteNode(reader, false);
      writer.Flush();
      data.Position = 0L;
      reader = UtilityMethods.CreateReader((Stream) data);
      if (shapeParser1.ParseShape(reader, defaultShape, relations, parentItemPath))
        return;
      ShapeParser shapeParser2 = (ShapeParser) new UnknownVmlShapeParser();
      data.Position = 0L;
      reader = UtilityMethods.CreateReader((Stream) data);
      int instance = defaultShape.Instance;
      Stream xmlTypeStream = defaultShape.XmlTypeStream;
      defaultShape = new ShapeImpl(defaultShape.Application, defaultShape.Parent);
      defaultShape.VmlShape = true;
      defaultShape.XmlTypeStream = xmlTypeStream;
      defaultShape.SetInstance(instance);
      shapeParser2.ParseShape(reader, defaultShape, relations, parentItemPath);
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
    int num = key != null && s != null ? Convert.ToInt32(double.Parse(s)) : throw new XmlException();
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
    ShapeParser shapeParser1;
    if (!this.m_dictShapeParsers.TryGetValue(num, out shapeParser1))
    {
      shapeParser1 = (ShapeParser) new UnknownVmlShapeParser();
      this.m_dictShapeParsers[num] = shapeParser1;
    }
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
    if (shapeParser1.ParseShape(reader, defaultShape1, relations, parentItemPath))
      return;
    ShapeParser shapeParser2 = (ShapeParser) new UnknownVmlShapeParser();
    data.Position = 0L;
    reader = UtilityMethods.CreateReader((Stream) data);
    int instance = defaultShape1.Instance;
    Stream xmlTypeStream = defaultShape1.XmlTypeStream;
    ShapeImpl defaultShape2 = new ShapeImpl(defaultShape1.Application, defaultShape1.Parent);
    defaultShape2.VmlShape = true;
    defaultShape2.XmlTypeStream = xmlTypeStream;
    defaultShape2.SetInstance(instance);
    shapeParser2.ParseShape(reader, defaultShape2, relations, parentItemPath);
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
    int num = -1;
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
          num = textWithFormat.Text.Length;
          reader.Read();
          string str = XmlConvert.DecodeName(reader.Value.Replace("\r", string.Empty));
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
    if (iFontIndex < 0)
      return;
    if (num == textWithFormat.Text.Length)
    {
      textWithFormat.FormattingRuns[num] = iFontIndex;
    }
    else
    {
      if (num < 0 || textWithFormat.Text.Length <= 0)
        return;
      textWithFormat.SetTextFontIndex(num, textWithFormat.Text.Length - 1, iFontIndex);
    }
  }

  private int ParseText(XmlReader reader, bool setCount)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "t")
      throw new XmlException(nameof (reader));
    SSTDictionary innerSst = this.m_book.InnerSST;
    bool flag1 = false;
    if (reader.XmlSpace == XmlSpace.Preserve)
      flag1 = true;
    reader.Read();
    if (reader.NodeType == XmlNodeType.EndElement)
    {
      reader.Skip();
      return innerSst.AddIncrease((object) string.Empty, false);
    }
    string name = reader.Value.Replace("\r", string.Empty);
    bool flag2 = name.ToLower() == "_x000d_";
    string key1 = XmlConvert.DecodeName(name);
    if (flag1 || flag2)
    {
      TextWithFormat key2 = new TextWithFormat();
      if (flag2)
      {
        key2.IsEncoded = true;
        key2.Text = name;
      }
      else
      {
        key2.IsPreserved = true;
        key2.Text = key1;
      }
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

  private int ParseRichTextRun(XmlReader reader, out string text)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "r")
      throw new XmlException(nameof (reader));
    SSTDictionary innerSst = this.m_book.InnerSST;
    TextWithFormat textWithFormat = this.ParseTextWithFormat(reader, "si");
    text = textWithFormat.Text;
    return innerSst.AddIncrease((object) textWithFormat, false);
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
        ExtendedFormatImpl extendedFormat = this.ParseExtendedFormat(reader, arrFontIndexes, arrFills, arrBorders, (List<int>) null, new bool?(), arrNumberFormatIndexes, true);
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
        ExtendedFormatImpl extendedFormat = this.ParseExtendedFormat(reader, arrNewFontIndexes, arrFills, arrBorders, namedStyleIndexes, new bool?(false), arrNumberFormatIndexes, false);
        ExtendedFormatImpl extendedFormatImpl = namedStyleIndexes == null ? this.m_book.InnerExtFormats.Add(extendedFormat) : this.m_book.InnerExtFormats.Add(extendedFormat, namedStyleIndexes.Count);
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
      string strStart = "Style";
      string defaultName = reader.Value;
      if (string.IsNullOrEmpty(defaultName))
        defaultName = this.m_book.InnerStyles.GenerateDefaultName(strStart);
      if (defaultName.Length > (int) byte.MaxValue)
      {
        record.IsAsciiConverted = true;
        string asciiString = Excel2007Parser.GetASCIIString(defaultName);
        if (asciiString.Length > (int) byte.MaxValue)
        {
          string str = asciiString.Substring(0, (int) byte.MaxValue);
          record.StyleName = str;
        }
        else
          record.StyleName = asciiString;
        record.StyleNameCache = defaultName;
      }
      else
        record.StyleName = defaultName;
    }
    if (reader.MoveToAttribute("xfId"))
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      record.ExtendedFormatIndex = (ushort) arrNamedStyleIndexes[int32];
      record.DefXFIndex = this.m_book.Version != ExcelVersion.Excel97to2003 ? ushort.MaxValue : (ushort) 0;
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
    Dictionary<int, int> arrNumberFormatIndexes,
    bool isCellStyleXfs)
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
    if (reader.MoveToAttribute("xfId") && !isCellStyleXfs)
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
    else if (!isCellStyleXfs)
    {
      record.ParentIndex = (ushort) 0;
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
    extendedFormatImpl.HorizontalAlignment = ExcelHAlign.HAlignGeneral;
    extendedFormatImpl.VerticalAlignment = ExcelVAlign.VAlignBottom;
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
    if (reader.MoveToAttribute("horizontal") && Enum.IsDefined(typeof (Excel2007HAlign), (object) reader.Value))
    {
      string str = reader.Value;
      record.HAlignmentType = (ExcelHAlign) Enum.Parse(typeof (Excel2007HAlign), str, true);
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
    if (!reader.MoveToAttribute("vertical") || !Enum.IsDefined(typeof (Excel2007VAlign), (object) reader.Value))
      return;
    string str1 = reader.Value;
    record.VAlignmentType = (ExcelVAlign) Enum.Parse(typeof (Excel2007VAlign), str1, true);
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
    if (record.FillIndex > (ushort) 0)
      format.IncludePatterns = true;
    else if (reader.MoveToAttribute("applyFill"))
      format.IncludePatterns = XmlConvertExtension.ToBoolean(reader.Value);
    else if (format.HasParent && (int) this.m_book.InnerExtFormats[(int) record.ParentIndex].Record.FillIndex == (int) record.FillIndex && defaultValue.HasValue && record.FillIndex < (ushort) 1)
      record.IsNotParentPattern = defaultValue.Value;
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
          Excel2007Parser.CopyBorderSettings(arrBorder2, extendedFormat);
          break;
        }
      }
    }
    else
      Excel2007Parser.CopyBorderSettings(arrBorder1, extendedFormat);
  }

  internal static void CopyBorderSettings(BordersCollection borders, ExtendedFormatImpl format)
  {
    if (borders == null)
      throw new ArgumentNullException(nameof (borders));
    ExtendedFormatRecord extendedFormatRecord = format != null ? format.Record : throw new ArgumentNullException("record");
    IBorder border1 = borders[ExcelBordersIndex.EdgeLeft];
    if (border1 != null && (format.IncludeBorder || Excel2007Parser.BordersDifferent(border1, format.LeftBorderColor, format.LeftBorderLineStyle)))
    {
      format.IncludeBorder = true;
      format.LeftBorderColor.CopyFrom(border1.ColorObject, true);
      format.LeftBorderLineStyle = border1.LineStyle;
    }
    else if (border1 != null)
      extendedFormatRecord.BorderLeft = border1.LineStyle;
    IBorder border2 = borders[ExcelBordersIndex.EdgeRight];
    if (border2 != null && (format.IncludeBorder || Excel2007Parser.BordersDifferent(border2, format.RightBorderColor, format.RightBorderLineStyle)))
    {
      format.IncludeBorder = true;
      format.RightBorderColor.CopyFrom(border2.ColorObject, true);
      format.RightBorderLineStyle = border2.LineStyle;
    }
    else if (border2 != null)
      extendedFormatRecord.BorderRight = border2.LineStyle;
    IBorder border3 = borders[ExcelBordersIndex.EdgeTop];
    if (border3 != null && (format.IncludeBorder || Excel2007Parser.BordersDifferent(border3, format.TopBorderColor, format.TopBorderLineStyle)))
    {
      format.IncludeBorder = true;
      format.TopBorderColor.CopyFrom(border3.ColorObject, true);
      format.TopBorderLineStyle = border3.LineStyle;
    }
    else if (border3 != null)
      extendedFormatRecord.BorderTop = border3.LineStyle;
    IBorder border4 = borders[ExcelBordersIndex.EdgeBottom];
    if (border4 != null && (format.IncludeBorder || Excel2007Parser.BordersDifferent(border4, format.BottomBorderColor, format.BottomBorderLineStyle)))
    {
      format.IncludeBorder = true;
      format.BottomBorderColor.CopyFrom(border4.ColorObject, true);
      format.BottomBorderLineStyle = border4.LineStyle;
    }
    else if (border4 != null)
      extendedFormatRecord.BorderBottom = border4.LineStyle;
    IBorder border5 = borders[ExcelBordersIndex.DiagonalDown];
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
    IBorder border6 = borders[ExcelBordersIndex.DiagonalUp];
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

  private static bool BordersDifferent(IBorder border, ColorObject color, ExcelLineStyle lineStyle)
  {
    ColorObject colorObject = border.ColorObject;
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
    string str1 = "styles.xml";
    if (reader.MoveToAttribute("Id"))
      id = reader.Value;
    if (reader.MoveToAttribute("Type"))
    {
      string str2 = reader.Value;
      if (str2.StartsWith("http://purl.oclc.org/ooxml/officeDocument/relationships"))
        str2 = str2.Replace("http://purl.oclc.org/ooxml/officeDocument/relationships", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
      type = str2;
    }
    if (reader.MoveToAttribute("Target"))
    {
      target = reader.Value;
      if (target.ToLower().Contains(str1))
        target = str1;
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
    this.m_book.InnerCharts.Clear();
    this.m_book.InnerDialogs.Clear();
    this.m_book.InnerMacros.Clear();
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
    if (reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
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
        sheet.PageSetupBase.Orientation = ExcelPageOrientation.Landscape;
        break;
      case "http://schemas.openxmlformats.org/officeDocument/2006/relationships/dialogsheet":
        DialogSheet dialogSheet = new DialogSheet(str1)
        {
          DataHolder = new WorksheetDataHolder(holder, relation, bookPath)
        };
        dialogSheet.DataHolder.RelationId = id1;
        dialogSheet.DataHolder.SheetId = str2;
        this.m_book.InnerDialogs.Add(dialogSheet);
        relations.Remove(id1);
        this.m_book.Objects.InnerList.Add((object) dialogSheet);
        return;
      case "http://schemas.microsoft.com/office/2006/relationships/xlMacrosheet":
      case "http://schemas.microsoft.com/office/2006/relationships/xlIntlMacrosheet":
        MacroSheet macroSheet = new MacroSheet(str1, relation.Type == "http://schemas.microsoft.com/office/2006/relationships/xlIntlMacrosheet")
        {
          DataHolder = new WorksheetDataHolder(holder, relation, bookPath)
        };
        macroSheet.DataHolder.RelationId = id1;
        macroSheet.DataHolder.SheetId = str2;
        this.m_book.InnerMacros.Add(macroSheet);
        relations.Remove(id1);
        this.m_book.Objects.InnerList.Add((object) macroSheet);
        return;
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

  internal void SetVisibilityState(WorksheetBaseImpl sheet, string strVisibility)
  {
    if (strVisibility == null)
      return;
    switch (strVisibility)
    {
      case "hidden":
        sheet.Visibility = WorksheetVisibility.Hidden;
        break;
      case "veryHidden":
        sheet.Visibility = WorksheetVisibility.StrongHidden;
        break;
      case "visible":
        sheet.Visibility = WorksheetVisibility.Visible;
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

  private string ParseNamedRange(XmlReader reader, out int index)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    string namedRange = (string) null;
    string str = (string) null;
    bool flag1 = false;
    int index1 = -1;
    bool flag2 = false;
    if (reader.LocalName == "definedName")
    {
      string name1 = reader.MoveToAttribute("name") ? reader.Value : throw new ApplicationException("Cannot find name for named range");
      if (reader.MoveToAttribute("comment"))
        str = reader.Value;
      if (reader.MoveToAttribute("localSheetId"))
      {
        flag1 = true;
        index1 = int.Parse(reader.Value);
      }
      if (reader.MoveToAttribute("hidden"))
        flag2 = XmlConvertExtension.ToBoolean(reader.Value);
      WorksheetImpl worksheetImpl = flag1 ? this.m_book.Objects[index1] as WorksheetImpl : (WorksheetImpl) null;
      bool flag3 = flag1 || worksheetImpl != null;
      IName name2 = !flag3 || worksheetImpl == null ? this.m_book.Names.Add(name1) : worksheetImpl.Names.Add(name1);
      if (flag3 && worksheetImpl == null)
        (name2 as NameImpl).SheetIndex = index1;
      name2.Description = str;
      reader.Read();
      namedRange = reader.Value;
      if (!string.IsNullOrEmpty(namedRange) && !namedRange[0].Equals('\'') && char.IsDigit(namedRange[0]) && namedRange.Contains("!") && !namedRange.Contains("("))
      {
        string oldValue = namedRange.Substring(0, namedRange.IndexOf("!"));
        namedRange = namedRange.Replace(oldValue, $"'{oldValue}'");
      }
      this.m_book.HasApostrophe = namedRange.Contains("'");
      NameImpl nameImpl = (NameImpl) name2;
      nameImpl.Visible = !flag2;
      if (flag1)
        nameImpl.Record.IndexOrGlobal = (ushort) (index1 + 1);
      reader.Skip();
      index = this.m_book.InnerNamesColection.Count - 1;
    }
    else
      index = -1;
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
    FontImpl font = (FontImpl) this.m_book.CreateFont((IFont) null, false);
    if (this.m_book.InnerFonts.Count != 0)
    {
      font.FontName = this.m_book.InnerFonts[0].FontName;
      font.Size = this.m_book.InnerFonts[0].Size;
    }
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType == XmlNodeType.Whitespace)
        reader.Read();
      this.ParseFontSettings(reader, font);
    }
    FontImpl fontImpl = (FontImpl) this.m_book.InnerFonts.Add((IFont) font);
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
          case "scheme":
            font.Scheme = this.ParseValue(reader, "val");
            break;
          case "u":
            string str1 = this.ParseValue(reader, "val");
            font.Underline = str1 == null || !Enum.IsDefined(typeof (ExcelUnderline), (object) Excel2007Serializator.CapitalizeFirstLetter(str1)) ? (font.Underline = ExcelUnderline.Single) : (ExcelUnderline) Enum.Parse(typeof (ExcelUnderline), str1, true);
            break;
          case "vertAlign":
            string str2 = this.ParseValue(reader, "val");
            font.VerticalAlignment = (ExcelFontVertialAlignment) Enum.Parse(typeof (ExcelFontVertialAlignment), str2, true);
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
      family = (byte) int.Parse(reader.Value);
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
    if (!(str != string.Empty))
      return;
    int key = reader.MoveToAttribute("numFmtId") ? Convert.ToInt32(reader.Value) : throw new XmlException("numFmtId wasn't found");
    int formatId = key;
    if (!this.m_book.InnerFormats.ContainsFormat(str))
    {
      str = this.m_book.InnerFormats.GetCustomizedString(str);
      if (Array.IndexOf<int>(this.DEF_NUMBERFORMAT_INDEXES, key) < 0)
        formatId = 163 + this.m_book.InnerFormats.Count - 36 + 1;
    }
    else
      formatId = this.m_book.InnerFormats[str].Index;
    if (!result.ContainsKey(key))
      result.Add(key, formatId);
    this.m_book.InnerFormats.Add(formatId, str);
    this.m_book.InnerFormats[str].isUsed = true;
    this.m_book.InnerFormats.HasNumberFormats = true;
  }

  private ColorObject ParseColor(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    ColorObject color = new ColorObject(ExcelKnownColors.BlackCustom);
    this.ParseColor(reader, color);
    return color;
  }

  private void ParseColor(XmlReader reader, ColorObject color)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.MoveToAttribute("indexed"))
    {
      ExcelKnownColors int32 = (ExcelKnownColors) Convert.ToInt32(reader.Value);
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

  private void ParseFillColor(XmlReader reader, ColorObject color)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.MoveToAttribute("indexed"))
    {
      ExcelKnownColors int32 = (ExcelKnownColors) Convert.ToInt32(reader.Value);
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
    return Excel2007Parser.ConvertColorByTint(color, dTint, false);
  }

  internal static Color ConvertColorByTint(Color color, double dTint, bool updateParsedColor)
  {
    double dHue;
    double dLuminance;
    double dSaturation;
    Excel2007Parser.ConvertRGBtoHLS(color, out dHue, out dLuminance, out dSaturation);
    if (dTint < 0.0)
      dLuminance *= 1.0 + dTint;
    if (dTint > 0.0)
      dLuminance = dLuminance * (1.0 - dTint) + ((double) byte.MaxValue - (double) byte.MaxValue * (1.0 - dTint));
    if (!(dSaturation == 0.0 & updateParsedColor))
      return Excel2007Parser.ConvertHLSToRGB(dHue, dLuminance, dSaturation);
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
      if (dTint == 0.0)
        doubleValue = (double) byte.MaxValue;
      if (dTint < 100.0 && dTint >= 1.0)
        doubleValue = dTint / 10.0;
      numArray[index] = Excel2007Parser.CalcInt(doubleValue);
    }
    return Color.FromArgb((int) color.A, (int) (byte) numArray[0], (int) (byte) numArray[1], (int) (byte) numArray[2]);
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

  internal static void ConvertRGBtoHLSBlip(
    Color color,
    out double dHue,
    out double dLuminance,
    out double dSaturation)
  {
    dHue = 0.0;
    dSaturation = 0.0;
    double val1_1 = (double) color.R / (double) byte.MaxValue;
    double val1_2 = (double) color.G / (double) byte.MaxValue;
    double val2 = (double) color.B / (double) byte.MaxValue;
    double num1 = Math.Max(val1_1, Math.Max(val1_2, val2));
    double num2 = Math.Min(val1_1, Math.Min(val1_2, val2));
    if (num1 == num2)
      dHue = 0.0;
    else if (num1 == val1_1 && val1_2 >= val2)
      dHue = 60.0 * (val1_2 - val2) / (num1 - num2);
    else if (num1 == val1_1 && val1_2 < val2)
      dHue = 60.0 * (val1_2 - val2) / (num1 - num2) + 360.0;
    else if (num1 == val1_2)
      dHue = 60.0 * (val2 - val1_1) / (num1 - num2) + 120.0;
    else if (num1 == val2)
      dHue = 60.0 * (val1_1 - val1_2) / (num1 - num2) + 240.0;
    dLuminance = (num1 + num2) / 2.0;
    if (dLuminance != 0.0 && num1 != num2)
    {
      if (0.0 < dLuminance && dLuminance <= 0.5)
      {
        dSaturation = (num1 - num2) / (num1 + num2);
      }
      else
      {
        if (dLuminance <= 0.5)
          return;
        dSaturation = (num1 - num2) / (2.0 - (num1 + num2));
      }
    }
    else
      dSaturation = 0.0;
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

  internal static Color ConvertHLSToRGBBlip(double dHue, double dLuminance, double dSaturation)
  {
    double num1 = dLuminance < 0.5 ? dLuminance * (1.0 + dSaturation) : dLuminance + dSaturation - dLuminance * dSaturation;
    double num2 = 2.0 * dLuminance - num1;
    double num3 = dHue / 360.0;
    double[] numArray = new double[3]
    {
      num3 + 1.0 / 3.0,
      num3,
      num3 - 1.0 / 3.0
    };
    for (int index = 0; index < 3; ++index)
    {
      if (numArray[index] < 0.0)
        ++numArray[index];
      if (numArray[index] > 1.0)
        --numArray[index];
      numArray[index] = numArray[index] * 6.0 >= 1.0 ? (numArray[index] * 2.0 >= 1.0 ? (numArray[index] * 3.0 >= 2.0 ? num2 : num2 + (num1 - num2) * (2.0 / 3.0 - numArray[index]) * 6.0) : num1) : num2 + (num1 - num2) * 6.0 * numArray[index];
    }
    return Color.FromArgb((int) byte.MaxValue, (int) (byte) Math.Round(numArray[0] * (double) byte.MaxValue, 0), (int) (byte) Math.Round(numArray[1] * (double) byte.MaxValue, 0), (int) (byte) Math.Round(numArray[2] * (double) byte.MaxValue));
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
    if (reader.IsEmptyElement)
      return new FillImpl();
    reader.Read();
    if (reader.LocalName == "fill" && reader.NodeType == XmlNodeType.EndElement)
      return new FillImpl();
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
    pathGradientType.FillType = ExcelFillType.Gradient;
    double attributeValue1 = this.ParseAttributeValue(reader, "top");
    double attributeValue2 = this.ParseAttributeValue(reader, "bottom");
    double attributeValue3 = this.ParseAttributeValue(reader, "left");
    double attributeValue4 = this.ParseAttributeValue(reader, "right");
    if (attributeValue1 == 0.5 && attributeValue2 == 0.5 && attributeValue3 == 0.5 && attributeValue4 == 0.5)
    {
      pathGradientType.GradientStyle = ExcelGradientStyle.From_Center;
      pathGradientType.GradientVariant = ExcelGradientVariants.ShadingVariants_1;
    }
    else if (attributeValue1 == 1.0 && attributeValue2 == 1.0 && attributeValue3 == 1.0 && attributeValue4 == 1.0)
    {
      pathGradientType.GradientStyle = ExcelGradientStyle.From_Corner;
      pathGradientType.GradientVariant = ExcelGradientVariants.ShadingVariants_4;
    }
    else if (attributeValue1 == 1.0 && attributeValue2 == 1.0)
    {
      pathGradientType.GradientStyle = ExcelGradientStyle.From_Corner;
      pathGradientType.GradientVariant = ExcelGradientVariants.ShadingVariants_3;
    }
    else if (attributeValue3 == 1.0 && attributeValue4 == 1.0)
    {
      pathGradientType.GradientStyle = ExcelGradientStyle.From_Corner;
      pathGradientType.GradientVariant = ExcelGradientVariants.ShadingVariants_2;
    }
    else if (double.IsNaN(attributeValue1) && double.IsNaN(attributeValue2) && double.IsNaN(attributeValue3) && double.IsNaN(attributeValue4))
    {
      pathGradientType.GradientStyle = ExcelGradientStyle.From_Corner;
      pathGradientType.GradientVariant = ExcelGradientVariants.ShadingVariants_1;
    }
    reader.Read();
    List<ColorObject> stopColors = this.ParseStopColors(reader);
    pathGradientType.PatternColorObject.CopyFrom(stopColors[0], true);
    pathGradientType.ColorObject.CopyFrom(stopColors[1], true);
    return pathGradientType;
  }

  private List<ColorObject> ParseStopColors(XmlReader reader)
  {
    List<ColorObject> stopColors = new List<ColorObject>();
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
    fill.FillType = ExcelFillType.Gradient;
    double attributeValue = this.ParseAttributeValue(reader, "degree");
    reader.Read();
    List<ColorObject> stopColors = this.ParseStopColors(reader);
    fill.PatternColorObject.CopyFrom(stopColors[0], true);
    fill.ColorObject.CopyFrom(stopColors[1], true);
    if (stopColors.Count == 3)
    {
      fill.GradientVariant = ExcelGradientVariants.ShadingVariants_3;
      switch (double.IsNaN(attributeValue) ? 0 : (int) attributeValue)
      {
        case 0:
          fill.GradientStyle = ExcelGradientStyle.Vertical;
          break;
        case 45:
          fill.GradientStyle = ExcelGradientStyle.Diagonl_Up;
          break;
        case 90:
          fill.GradientStyle = ExcelGradientStyle.Horizontal;
          break;
        case 135:
          fill.GradientStyle = ExcelGradientStyle.Diagonl_Down;
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
        fill.GradientStyle = ExcelGradientStyle.Vertical;
        fill.GradientVariant = ExcelGradientVariants.ShadingVariants_1;
        break;
      case 45:
        fill.GradientStyle = ExcelGradientStyle.Diagonl_Up;
        fill.GradientVariant = ExcelGradientVariants.ShadingVariants_1;
        break;
      case 90:
        fill.GradientStyle = ExcelGradientStyle.Horizontal;
        fill.GradientVariant = ExcelGradientVariants.ShadingVariants_1;
        break;
      case 135:
        fill.GradientStyle = ExcelGradientStyle.Diagonl_Down;
        fill.GradientVariant = ExcelGradientVariants.ShadingVariants_1;
        break;
      case 180:
        fill.GradientStyle = ExcelGradientStyle.Vertical;
        fill.GradientVariant = ExcelGradientVariants.ShadingVariants_2;
        break;
      case 225:
        fill.GradientStyle = ExcelGradientStyle.Diagonl_Up;
        fill.GradientVariant = ExcelGradientVariants.ShadingVariants_2;
        break;
      case 270:
        fill.GradientStyle = ExcelGradientStyle.Horizontal;
        fill.GradientVariant = ExcelGradientVariants.ShadingVariants_2;
        break;
      case 315:
        fill.GradientStyle = ExcelGradientStyle.Diagonl_Down;
        fill.GradientVariant = ExcelGradientVariants.ShadingVariants_2;
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
    if (reader.MoveToAttribute("patternType") && Enum.IsDefined(typeof (Excel2007Pattern), (object) reader.Value))
    {
      patternFill.Pattern = Excel2007Parser.ConvertStringToPattern(reader.Value);
      patternFill.IsDxfPatternNone = patternFill.Pattern == ExcelPattern.None;
    }
    if (reader.NodeType == XmlNodeType.Whitespace)
      reader.Read();
    reader.Read();
    ColorObject colorObject1 = (ColorObject) null;
    ColorObject colorObject2 = (ColorObject) null;
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "fill")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "bgColor":
            colorObject1 = new ColorObject(ExcelKnownColors.BlackCustom);
            this.ParseFillColor(reader, colorObject1);
            break;
          case "fgColor":
            colorObject2 = new ColorObject(ExcelKnownColors.None);
            this.ParseColor(reader, colorObject2);
            break;
        }
      }
      reader.Read();
    }
    if (reader.LocalName == "patternFill")
      reader.Read();
    if (swapColors && patternFill.Pattern != ExcelPattern.Solid)
    {
      ColorObject colorObject3 = colorObject2;
      colorObject2 = colorObject1;
      colorObject1 = colorObject3;
    }
    if (colorObject2 != (ColorObject) null)
      patternFill.ColorObject.CopyFrom(colorObject2, true);
    else if (colorObject1 == (ColorObject) null || patternFill.Pattern != ExcelPattern.Solid)
      patternFill.ColorObject.SetIndexed(ExcelKnownColors.None);
    if (colorObject1 != (ColorObject) null)
      patternFill.PatternColorObject.CopyFrom(colorObject1, true);
    else if (colorObject2 == (ColorObject) null || patternFill.Pattern != ExcelPattern.Solid)
      patternFill.PatternColorObject.SetIndexed(ExcelKnownColors.BlackCustom);
    return patternFill;
  }

  private static ExcelPattern ConvertStringToPattern(string value)
  {
    return (ExcelPattern) Enum.Parse(typeof (Excel2007Pattern), value, true);
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
    DxfImpl dxfImpl = (DxfImpl) null;
    return this.ParseBordersCollection(reader, dxfImpl);
  }

  private BordersCollection ParseBordersCollection(XmlReader reader, DxfImpl dxfImpl)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "border")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    BordersCollection bordersCollection = new BordersCollection(this.m_book.Application, (object) this.m_book, true);
    bool flag1 = false;
    bool flag2 = false;
    if (reader.IsEmptyElement)
    {
      reader.Read();
    }
    else
    {
      if (reader.MoveToAttribute("diagonalUp"))
        flag1 = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("diagonalDown"))
        flag2 = XmlConvertExtension.ToBoolean(reader.Value);
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          string empty = string.Empty;
          string str = reader.LocalName;
          if (str == "start")
            str = str.Replace("start", "left");
          if (str == "end")
            str = str.Replace("end", "right");
          if (Enum.IsDefined(typeof (Excel2007BorderIndex), (object) str))
          {
            Excel2007BorderIndex borderIndex;
            BorderSettingsHolder border = this.ParseBorder(reader, out borderIndex);
            if (borderIndex == Excel2007BorderIndex.diagonal)
            {
              BorderSettingsHolder borderSettingsHolder = (BorderSettingsHolder) border.Clone();
              border.ShowDiagonalLine = flag1;
              borderSettingsHolder.ShowDiagonalLine = flag2;
              bordersCollection.SetBorder(ExcelBordersIndex.DiagonalUp, (IBorder) border);
              bordersCollection.SetBorder(ExcelBordersIndex.DiagonalDown, (IBorder) borderSettingsHolder);
            }
            else
            {
              if (dxfImpl != null)
              {
                if (borderIndex == Excel2007BorderIndex.vertical)
                {
                  dxfImpl.IsVerticalBorderModified = true;
                  if (border.LineStyle != ExcelLineStyle.None)
                  {
                    dxfImpl.VerticalBorderStyle = border.LineStyle;
                    dxfImpl.VerticalColorObject = border.ColorObject;
                  }
                }
                if (borderIndex == Excel2007BorderIndex.horizontal)
                {
                  dxfImpl.IsHorizontalBorderModified = true;
                  if (border.LineStyle != ExcelLineStyle.None)
                  {
                    dxfImpl.HorizontalBorderStyle = border.LineStyle;
                    dxfImpl.HorizontalColorObject = border.ColorObject;
                  }
                }
              }
              ExcelBordersIndex index = (ExcelBordersIndex) borderIndex;
              bordersCollection.SetBorder(index, (IBorder) border);
            }
            if (bordersCollection.IsEmptyBorder && border != null && !border.IsEmptyBorder)
              bordersCollection.IsEmptyBorder = false;
          }
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
    string str = reader.NodeType == XmlNodeType.Element ? reader.LocalName : throw new XmlException("Unexpected node type " + (object) reader.NodeType);
    if (str == "start")
      str = str.Replace("start", "left");
    if (str == "end")
      str = str.Replace("end", "right");
    borderIndex = (Excel2007BorderIndex) Enum.Parse(typeof (Excel2007BorderIndex), str, true);
    BorderSettingsHolder border = new BorderSettingsHolder();
    bool flag = false;
    if (reader.IsEmptyElement)
      flag = true;
    if (reader.MoveToAttribute("style") && Enum.IsDefined(typeof (Excel2007BorderLineStyle), (object) Excel2007Serializator.CapitalizeFirstLetter(reader.Value)))
    {
      Excel2007BorderLineStyle excel2007BorderLineStyle = Excel2007BorderLineStyle.None;
      if (Enum.IsDefined(typeof (Excel2007BorderLineStyle), (object) Excel2007Serializator.CapitalizeFirstLetter(reader.Value)))
        excel2007BorderLineStyle = (Excel2007BorderLineStyle) Enum.Parse(typeof (Excel2007BorderLineStyle), reader.Value, true);
      else if (Enum.IsDefined(typeof (ExcelLineStyle), (object) Excel2007Serializator.CapitalizeFirstLetter(reader.Value)))
        excel2007BorderLineStyle = (Excel2007BorderLineStyle) Enum.Parse(typeof (ExcelLineStyle), reader.Value, true);
      border.LineStyle = (ExcelLineStyle) excel2007BorderLineStyle;
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

  private void ParseAlignment(XmlReader reader, DxfImpl dxfImpl)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dxfImpl == null)
      throw new ArgumentNullException("pivotCellFormat");
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "alignment")
      throw new XmlException();
    dxfImpl.HAlignmentType = ExcelHAlign.HAlignGeneral;
    dxfImpl.VAlignmentType = ExcelVAlign.VAlignBottom;
    if (reader.MoveToAttribute("horizontal") && Enum.IsDefined(typeof (Excel2007HAlign), (object) reader.Value))
    {
      string str = reader.Value;
      dxfImpl.HAlignmentType = (ExcelHAlign) Enum.Parse(typeof (Excel2007HAlign), str, true);
    }
    if (reader.MoveToAttribute("indent"))
      dxfImpl.Indent = XmlConvertExtension.ToByte(reader.Value);
    if (reader.MoveToAttribute("readingOrder"))
      dxfImpl.ReadingOrder = XmlConvertExtension.ToUInt16(reader.Value);
    if (reader.MoveToAttribute("shrinkToFit"))
      dxfImpl.ShrinkToFit = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("textRotation"))
      dxfImpl.Rotation = XmlConvertExtension.ToUInt16(reader.Value);
    if (reader.MoveToAttribute("wrapText"))
      dxfImpl.WrapText = XmlConvertExtension.ToBoolean(reader.Value);
    if (!reader.MoveToAttribute("vertical") || !Enum.IsDefined(typeof (Excel2007VAlign), (object) reader.Value))
      return;
    string str1 = reader.Value;
    dxfImpl.VAlignmentType = (ExcelVAlign) Enum.Parse(typeof (Excel2007VAlign), str1, true);
  }

  private void ParseProtection(XmlReader reader, DxfImpl dxfImpl)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dxfImpl == null)
      throw new ArgumentNullException("pivotCellFormat");
    if (reader.NodeType != XmlNodeType.Element || reader.LocalName != "protection")
      throw new XmlException("Unable to locate necessary xml tag");
    if (reader.MoveToAttribute("hidden"))
      dxfImpl.IsHidden = XmlConvertExtension.ToBoolean(reader.Value);
    if (!reader.MoveToAttribute("locked"))
      return;
    dxfImpl.IsLocked = XmlConvertExtension.ToBoolean(reader.Value);
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
    int result = 0;
    int iStyleIndex = this.m_book.DefaultXFIndex;
    int num1 = sheet.DefaultRowHeight;
    bool flag1 = false;
    if (reader.MoveToAttribute("r"))
    {
      if (int.TryParse(reader.Value, out result))
        generatedRowIndex = result;
      else
        result = generatedRowIndex;
      if (result == 0)
        generatedRowIndex = result = 1;
    }
    else
      result = generatedRowIndex;
    RowStorage row = WorksheetHelper.GetOrCreateRow(sheet, result - 1, true);
    List<int> cellStyleIndex = new List<int>();
    if (reader.MoveToAttribute("collapsed"))
      row.IsCollapsed = XmlConvertExtension.ToBoolean(reader.Value);
    bool flag2 = reader.MoveToAttribute("customFormat") && XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("customHeight"))
      row.IsBadFontHeight = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("outlineLevel"))
    {
      row.OutlineLevel = XmlConvertExtension.ToUInt16(reader.Value);
      if (this.startRow == 0)
      {
        this.startRow = generatedRowIndex;
        this.previousRow = generatedRowIndex - 1;
        this.previousOutlineLevel = (int) row.OutlineLevel;
      }
      if (this.previousOutlineLevel != 0 && (this.previousOutlineLevel != (int) row.OutlineLevel || this.previousRow != generatedRowIndex - 1))
      {
        this.endRow = this.previousRow;
        this.m_outlineWrapperUtility.AddRowLevel(sheet as WorksheetImpl, this.startRow, this.endRow, this.previousOutlineLevel, true);
        this.startRow = generatedRowIndex;
        this.previousOutlineLevel = (int) row.OutlineLevel;
      }
      this.previousRow = generatedRowIndex;
    }
    if (reader.MoveToAttribute("ht"))
    {
      double num2 = XmlConvertExtension.ToDouble(reader.Value);
      if (num2 > 409.5)
        num2 = 409.5;
      num1 = (int) (num2 * 20.0);
      row.HasRowHeight = true;
    }
    if (reader.MoveToAttribute("s"))
    {
      if (arrStyles.Count > XmlConvertExtension.ToInt32(reader.Value))
        iStyleIndex = arrStyles[XmlConvertExtension.ToInt32(reader.Value)];
      this.m_book.AddUsedStyleIndex(iStyleIndex);
    }
    if (reader.MoveToAttribute("x14ac:dyDescent"))
      row.DyDescent = XmlConvertExtension.ToDouble(reader.Value);
    if (reader.MoveToAttribute("hidden"))
      row.IsHidden = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("thickBot"))
      row.IsSpaceBelowRow = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("thickTop"))
      row.IsSpaceAboveRow = XmlConvertExtension.ToBoolean(reader.Value);
    row.ExtendedFormatIndex = (ushort) iStyleIndex;
    row.Height = (ushort) num1;
    if (sheet.FirstRow < 0 || sheet.FirstRow > result)
      sheet.FirstRow = result;
    if (sheet.LastRow < result)
      sheet.LastRow = result;
    reader.MoveToElement();
    int num3 = 1;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == cellTag)
          num3 = this.ParseCell(reader, sheet, arrStyles, result, num3, cellStyleIndex) + 1;
        reader.Skip();
      }
    }
    else if (row.IsHidden)
      sheet.CellRecords.SetBlank(result, num3, this.m_book.DefaultXFIndex);
    foreach (int index in cellStyleIndex)
    {
      if (!flag1)
      {
        IExtendedFormat extFormat = this.m_book.GetExtFormat(index);
        flag1 = extFormat.WrapText || extFormat.HorizontalAlignment == ExcelHAlign.HAlignJustify || extFormat.VerticalAlignment == ExcelVAlign.VAlignJustify;
      }
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
    int num = this.m_book.DefaultXFIndex;
    Excel2007Serializator.CellType cellType = Excel2007Serializator.CellType.n;
    CellRecordCollection cellRecords = sheet.CellRecords;
    if (reader.MoveToAttribute("r"))
      RangeImpl.CellNameToRowColumn(reader.Value, out iRow, out iColumn);
    if (reader.MoveToAttribute("s"))
    {
      num = XmlConvertExtension.ToInt32(reader.Value) <= 0 ? arrStyles[0] : arrStyles[XmlConvertExtension.ToInt32(reader.Value)];
      if (!cellStyleIndex.Contains(num))
        cellStyleIndex.Add(num);
    }
    this.m_book.AddUsedStyleIndex(num);
    if (reader.MoveToAttribute("t") && Enum.IsDefined(typeof (Excel2007Serializator.CellType), (object) reader.Value))
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
          cellRecords.SetSingleStringValue(iRow, iColumn, num, stringItem);
          flag1 = true;
        }
        if (reader.LocalName == "f")
        {
          this.ParseFormula(reader, sheet, iRow, iColumn, num, cellType);
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
              {
                if ((sheet.Application as ApplicationImpl).IsExternBookParsing)
                {
                  if (cellType == Excel2007Serializator.CellType.n)
                    cellRecords.SetNumberValue(iRow, iColumn, XmlConvertExtension.ToDouble(reader.Value), num);
                  else
                    cellRecords.SetNonSSTString(iRow, iColumn, num, reader.Value);
                }
                else
                  this.SetFormulaValue(sheet, cellType, reader.Value, iRow, iColumn);
              }
              else
                this.SetCellRecord(cellType, reader.Value, cellRecords, iRow, iColumn, num);
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
        this.SetCellRecord(Excel2007Serializator.CellType.n, "0", cellRecords, iRow, iColumn, num);
      else if (!(sheet.Application as ApplicationImpl).IsExternBookParsing)
        cellRecords.SetBlank(iRow, iColumn, num);
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
    Excel2007Serializator.FormulaType formulaType = Excel2007Serializator.FormulaType.normal;
    CellRecordCollection cellRecords = sheet.CellRecords;
    bool bCalculateOnOpen = false;
    if (reader.MoveToAttribute("t"))
      formulaType = (Excel2007Serializator.FormulaType) Enum.Parse(typeof (Excel2007Serializator.FormulaType), reader.Value, false);
    if (reader.MoveToAttribute("si"))
      (sheet as WorksheetImpl).m_sharedFormulaGroupIndex = XmlConvertExtension.ToUInt32(reader.Value);
    if (reader.MoveToAttribute("ref"))
      strCellsRange = reader.Value;
    if (reader.MoveToAttribute("ca"))
      bCalculateOnOpen = XmlConvertExtension.ToBoolean(reader.Value);
    if (formulaType == Excel2007Serializator.FormulaType.dataTable)
    {
      CellFormula cellFormula = new CellFormula();
      cellFormula.FormulaType = formulaType;
      cellFormula.Reference = strCellsRange;
      if (reader.MoveToAttribute("dt2D"))
        cellFormula.DataTable2D = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("dtr"))
        cellFormula.DataTableRow = XmlConvertExtension.ToBoolean(reader.Value);
      if (reader.MoveToAttribute("r1"))
        cellFormula.FirstDataTableCell = reader.Value;
      if (reader.MoveToAttribute("r2"))
        cellFormula.SecondDataTableCell = reader.Value;
      if ((sheet as WorksheetImpl).m_cellFormulas == null)
        (sheet as WorksheetImpl).m_cellFormulas = new Dictionary<long, CellFormula>();
      (sheet as WorksheetImpl).m_cellFormulas.Add(RangeImpl.GetCellIndex(iCol, iRow), cellFormula);
    }
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      if (reader.NodeType != XmlNodeType.EndElement)
      {
        strFormulaString += reader.Value.Replace("\r", string.Empty);
        if (strFormulaString.StartsWith("=="))
          strFormulaString = strFormulaString.Substring(1);
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
          if (strFormula.StartsWith("[") && cellType != Excel2007Serializator.CellType.e)
          {
            int startIndex = strFormula.IndexOf('!') + 1;
            string strName = strFormula.Substring(startIndex, strFormula.Length - startIndex);
            int iBookIndex = 0;
            int iNameIndex = 0;
            if (this.m_book.ExternWorkbooks.ContainsExternName(strName, ref iBookIndex, ref iNameIndex))
            {
              ExternWorkbookImpl externWorkbook = this.m_book.ExternWorkbooks[iBookIndex];
              if (externWorkbook.URL.Contains("|"))
                strFormula = externWorkbook.URL + (object) '!' + strName;
            }
            record.ParsedExpression = this.m_formulaUtil.ParseString(strFormula, (IWorksheet) sheet, (Dictionary<string, string>) null);
          }
          else if ((!strFormula.StartsWith("[") ? 1 : (strFormula.StartsWith("[0") ? 1 : 0)) != 0 || cellType != Excel2007Serializator.CellType.e)
            record.ParsedExpression = this.m_formulaUtil.ParseString(strFormula, (IWorksheet) sheet, (Dictionary<string, string>) null);
          else
            (sheet as WorksheetImpl).FormulaValues.Add(RangeImpl.GetCellIndex(iCol, iRow), strFormula);
          record.Row = iRow - 1;
          record.Column = iCol - 1;
          record.ExtendedFormatIndex = (ushort) iXFIndex;
          record.CalculateOnOpen = bCalculateOnOpen;
          cellRecords.SetCellRecord(iRow, iCol, (ICellPositionFormat) record);
          break;
        }
        break;
      case Excel2007Serializator.FormulaType.shared:
        this.SetSharedFormula(sheet as WorksheetImpl, strFormulaString, strCellsRange, (sheet as WorksheetImpl).m_sharedFormulaGroupIndex, iRow, iCol, iXFIndex, bCalculateOnOpen);
        ++(sheet as WorksheetImpl).m_sharedFormulaGroupIndex;
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
    this.m_outlineWrapperUtility = new OutlineWrapperUtility();
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "cols")
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "col")
        this.ParseColumn(reader, sheet, arrStyles);
      reader.Read();
    }
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
      this.m_book.UpdateUsedStyleIndex(arrStyle, this.m_book.MaxRowCount);
      if ((int) record.LastColumn != this.Worksheet.Workbook.MaxColumnCount - 1)
      {
        if (this.Worksheet.FirstColumn == int.MaxValue || this.Worksheet.FirstColumn > (int) record.FirstColumn + 1)
          this.Worksheet.FirstColumn = (int) record.FirstColumn + 1;
        if (this.Worksheet.LastColumn == int.MaxValue || this.Worksheet.LastColumn < (int) record.LastColumn + 1)
          this.Worksheet.LastColumn = (int) record.LastColumn + 1;
      }
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
      this.m_outlineWrapperUtility.AddColumnLevel(sheet, (int) record.OutlineLevel, (int) record.FirstColumn + 1, (int) record.LastColumn + 1, true);
    }
    if (reader.MoveToAttribute("hidden"))
      record.IsHidden = XmlConvertExtension.ToBoolean(reader.Value);
    sheet.ParseColumnInfo(record, false);
    this.ParseColumnInfoRecord(reader, sheet, -1);
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

  public void ParseDataValidations(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "dataValidations")
      throw new XmlException(nameof (reader));
    DValRecord record = (DValRecord) BiffRecordFactory.GetRecord(TBIFFRecord.DVal);
    if (reader.MoveToAttribute("disablePrompts"))
      record.IsPromtBoxVisible = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("xWindow"))
      record.PromtBoxHPos = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("yWindow"))
      record.PromtBoxVPos = XmlConvertExtension.ToInt32(reader.Value);
    DataValidationCollection dvCollection = sheet.DVTable.Add(record);
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "dataValidation")
        this.ParseDataValidation(reader, dvCollection);
      else
        reader.Skip();
    }
    reader.Skip();
    if (!this.m_hasDVExtlst)
      return;
    this.m_hasDVExtlst = false;
  }

  private void ParseDataValidation(XmlReader reader, DataValidationCollection dvCollection)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dvCollection == null)
      throw new ArgumentNullException(nameof (dvCollection));
    if (reader.LocalName != "dataValidation")
      throw new XmlException(nameof (reader));
    DataValidationImpl dataValidationImpl = new DataValidationImpl(dvCollection);
    if (reader.MoveToAttribute("sqref"))
    {
      foreach (TAddr tAddr in this.GetRangesForDataValidation(reader.Value))
        dataValidationImpl.AddRanges(tAddr);
    }
    if (reader.MoveToAttribute("type"))
      dataValidationImpl.AllowType = this.ConvertDataValidationType(reader.Value);
    dataValidationImpl.IsEmptyCellAllowed = reader.MoveToAttribute("allowBlank") && XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("error"))
      dataValidationImpl.ErrorBoxText = this.ConvertToASCII(reader.Value);
    if (reader.MoveToAttribute("errorStyle"))
      dataValidationImpl.ErrorStyle = this.ConvertDataValidationErrorStyle(reader.Value);
    if (reader.MoveToAttribute("errorTitle"))
      dataValidationImpl.ErrorBoxTitle = reader.Value;
    if (reader.MoveToAttribute("operator"))
      dataValidationImpl.CompareOperator = this.ConvertDataValidationOperator(reader.Value);
    if (reader.MoveToAttribute("prompt"))
      dataValidationImpl.PromptBoxText = this.ConvertToASCII(reader.Value);
    if (reader.MoveToAttribute("promptTitle"))
      dataValidationImpl.PromptBoxTitle = reader.Value;
    if (reader.MoveToAttribute("showDropDown"))
      dataValidationImpl.IsSuppressDropDownArrow = XmlConvertExtension.ToBoolean(reader.Value);
    dataValidationImpl.ShowErrorBox = reader.MoveToAttribute("showErrorMessage") && XmlConvertExtension.ToBoolean(reader.Value);
    dataValidationImpl.ShowPromptBox = reader.MoveToAttribute("showInputMessage") && XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "formula1" || reader.LocalName == "formula2")
          this.ParseFormulaOneTwoValues(reader, dataValidationImpl);
        else if (reader.LocalName == "AlternateContent")
          this.ParseAlternateContent(reader, dataValidationImpl);
        if (reader.LocalName != "dataValidation")
          reader.Skip();
      }
    }
    this.DetectIsStringList(dataValidationImpl);
    reader.Read();
    dvCollection.Add(dataValidationImpl);
  }

  private void ParseAlternateContent(XmlReader reader, DataValidationImpl dataValidation)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataValidation == null)
      throw new ArgumentNullException(nameof (dataValidation));
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "Choice":
            reader.Read();
            if (reader.LocalName == "list")
            {
              this.ParseFormulaOneTwoValues(reader, dataValidation);
              reader.Skip();
            }
            reader.Skip();
            continue;
          case "Fallback":
            reader.Read();
            if (reader.LocalName == "formula1" || reader.LocalName == "formula2")
              this.ParseFormulaOneTwoValues(reader, dataValidation);
            reader.Skip();
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

  private void DetectIsStringList(DataValidationImpl dataValidation)
  {
    DVRecord dvRecord = dataValidation.DVRecord;
    Ptg[] firstFormulaTokens = dvRecord.FirstFormulaTokens;
    dvRecord.IsStrListExplicit = dvRecord.DataType == ExcelDataType.User && firstFormulaTokens != null && dvRecord.SecondFormulaTokens == null && firstFormulaTokens.Length == 1 && firstFormulaTokens[0].TokenCode == FormulaToken.tStringConstant;
  }

  private void ParseSorting(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (reader.LocalName != "sortState")
      throw new XmlException(nameof (reader));
    this.ParseSortData(reader, sheet.DataSorter);
  }

  private void ParseSortData(XmlReader reader, IDataSort dataSorter)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataSorter == null)
      throw new ArgumentNullException("sortState");
    if (reader.LocalName != "sortState")
      throw new XmlException(nameof (reader));
    DataSorter sortState = dataSorter as DataSorter;
    string attribute = reader.GetAttribute("ref");
    IRange rangeByString = (sortState.Worksheet as WorksheetImpl).GetRangeByString(attribute, false);
    if (rangeByString.Row > 1)
    {
      IRange range = sortState.Worksheet[rangeByString.Row - 1, rangeByString.Column, rangeByString.LastRow, rangeByString.LastColumn];
      sortState.UpdateRange(range);
    }
    else
    {
      IRange range = rangeByString;
      sortState.UpdateRange(range);
    }
    List<DxfImpl> dxfsCollection = this.m_book.DataHolder.ParseDxfsCollection();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "sortCondition":
            this.ParseSortCondition(reader, sortState, dxfsCollection);
            continue;
          default:
            reader.Read();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Skip();
  }

  private void ParseSortCondition(XmlReader reader, DataSorter sortState, List<DxfImpl> lstDxfs)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sortState == null)
      throw new ArgumentNullException("sheet");
    if (reader.LocalName != "sortCondition")
      throw new XmlException(nameof (reader));
    int bookCfPriorityCount = this.m_book.BookCFPriorityCount;
    bool flag = false;
    string str = "";
    string strRangeValue = "";
    OrderBy orderBy = OrderBy.OnTop;
    SortOn sortBasedOn = SortOn.Values;
    if (reader.MoveToAttribute("descending"))
      flag = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("sortBy"))
      str = reader.Value;
    if (reader.MoveToAttribute("ref"))
      strRangeValue = reader.GetAttribute("ref");
    if (reader.MoveToAttribute("dxfId"))
      bookCfPriorityCount = int.Parse(reader.Value);
    int key = (sortState.Worksheet as WorksheetImpl).GetRangeByString(strRangeValue, false).Column - 1;
    if (str != "")
    {
      switch (str)
      {
        case "cellColor":
          sortBasedOn = SortOn.CellColor;
          break;
        case "fontColor":
          sortBasedOn = SortOn.FontColor;
          break;
      }
    }
    if (flag && sortBasedOn != SortOn.Values)
      orderBy = OrderBy.OnBottom;
    else if (flag)
      orderBy = OrderBy.Descending;
    ISortField sortField = sortState.SortFields.Add(key, sortBasedOn, orderBy);
    if (sortBasedOn == SortOn.Values || lstDxfs == null || lstDxfs.Count <= bookCfPriorityCount)
      return;
    lstDxfs[bookCfPriorityCount].FillSorting(sortField);
  }

  private void ParseFormulaOneTwoValues(XmlReader reader, DataValidationImpl dataValidation)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataValidation == null)
      throw new ArgumentNullException(nameof (dataValidation));
    string localName = reader.LocalName;
    string str1 = (string) null;
    string str2 = (string) null;
    bool flag = false;
    if (this.m_hasDVExtlst && dataValidation.DVRanges.Length == 0)
    {
      while (reader.LocalName != nameof (dataValidation))
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "formula2":
              reader.Read();
              continue;
            case "f":
              reader.Read();
              if (localName == "formula1" && str1 == null)
              {
                str1 = reader.Value;
                flag = true;
                continue;
              }
              str2 = reader.Value;
              continue;
            case "sqref":
              reader.Read();
              foreach (TAddr tAddr in this.GetRangesForDataValidation(reader.Value))
                dataValidation.AddRanges(tAddr);
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
    WorksheetImpl worksheet = dataValidation.Worksheet;
    IRange range = worksheet[dataValidation.DVRanges[0]];
    int row = range.Row - 1;
    int column = range.Column - 1;
    if (str1 == null)
    {
      reader.Read();
      str1 = reader.Value;
    }
    if (localName == "list")
    {
      dataValidation.ChoiceTokens = str1;
      dataValidation.IsFormulaOrChoice = true;
    }
    if (localName == "formula1" || flag)
    {
      Ptg[] formulaPtg = DataValidationImpl.GetFormulaPtg(ref str1, this.m_formulaUtil, worksheet, row, column);
      dataValidation.FirstFormulaTokens = formulaPtg;
      dataValidation.FirstFormula = str1;
      dataValidation.IsFormulaOrChoice = true;
    }
    if (str2 != null || localName == "formula2")
    {
      if (str2 != null)
        str1 = str2;
      Ptg[] formulaPtg = DataValidationImpl.GetFormulaPtg(ref str1, this.m_formulaUtil, worksheet, row, column);
      dataValidation.SecondFormulaTokens = formulaPtg;
      dataValidation.SecondFormula = str1;
      dataValidation.IsFormulaOrChoice = true;
    }
    if (this.m_hasDVExtlst)
      return;
    reader.Skip();
  }

  public void ParseAutoFilters(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    this.ParseAutoFilters(reader, (AutoFiltersCollection) sheet.AutoFilters);
  }

  internal void ParseAutoFilters(XmlReader reader, AutoFiltersCollection filters)
  {
    bool flag = reader != null ? reader.IsEmptyElement : throw new ArgumentNullException(nameof (reader));
    AutoFiltersCollection autoFilters = filters;
    if (reader.MoveToAttribute("ref"))
    {
      TAddr rangeForDvOrAf = this.GetRangeForDVOrAF(reader.Value);
      autoFilters.FilterRange = filters.Worksheet[rangeForDvOrAf.FirstRow + 1, rangeForDvOrAf.FirstCol + 1, rangeForDvOrAf.LastRow + 1, rangeForDvOrAf.LastCol + 1];
    }
    if (!flag)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "filterColumn")
          this.ParseFilterColumn(reader, autoFilters);
        else if (reader.LocalName == "sortState")
          this.ParseSortData(reader, autoFilters.DataSorter);
        else
          reader.Skip();
      }
    }
    reader.Skip();
    if (!(autoFilters.Parent is IListObject) || !(reader.LocalName == "sortState"))
      return;
    this.ParseSortData(reader, autoFilters.DataSorter);
  }

  private void ParseFilterColumn(XmlReader reader, AutoFiltersCollection autoFilters)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoFilters == null)
      throw new ArgumentNullException(nameof (autoFilters));
    bool isEmptyElement = reader.IsEmptyElement;
    int columnIndex = reader.MoveToAttribute("colId") ? XmlConvertExtension.ToInt32(reader.Value) : throw new XmlReadingException("Required attribute wan not present.");
    AutoFilterImpl autoFilter = (AutoFilterImpl) autoFilters[columnIndex];
    autoFilter.Index = columnIndex;
    if (reader.MoveToAttribute("hiddenButton"))
      autoFilter.m_showButton = !Convert.ToBoolean(int.Parse(reader.Value));
    else if (reader.MoveToAttribute("showButton"))
      autoFilter.m_showButton = Convert.ToBoolean(int.Parse(reader.Value));
    if (!isEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "customFilters")
          this.ParseCustomFilters(reader, autoFilter);
        if (reader.LocalName == "top10")
          this.ParseAutoFilterTopTen(reader, autoFilter);
        if (reader.LocalName == "filters")
          this.ParseFilters(reader, autoFilter);
        if (reader.LocalName == "dynamicFilter")
          this.ParseDynamicFilter(reader, autoFilter);
        if (reader.LocalName == "colorFilter")
          this.ParseColorFilter(reader, autoFilter);
        if (reader.LocalName == "iconFilter")
          this.ParseIconFilter(reader, autoFilter);
        if (reader.LocalName == "AlternateContent")
        {
          reader.Read();
          reader.Read();
          this.ParseIconFilter(reader, autoFilter);
          while (reader.LocalName != "AlternateContent")
            reader.Read();
        }
        reader.Skip();
      }
    }
    reader.Skip();
  }

  private void ParseFilters(XmlReader reader, AutoFilterImpl autoFilter)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoFilter == null)
      throw new ArgumentNullException(nameof (autoFilter));
    autoFilter.IsAnd = false;
    autoFilter.FilterType = ExcelFilterType.CombinationFilter;
    if (this.ParseBoolean(reader, "blank", false))
    {
      autoFilter.Record.FirstCondition.DataType = AutoFilterRecord.DOPER.DOPERDataType.MatchBlanks;
      autoFilter.AddTextFilter(string.Empty);
      reader.MoveToElement();
    }
    else
      reader.Read();
    while (reader.LocalName == "filter" || reader.LocalName == "dateGroupItem")
    {
      if (reader.LocalName == "filter")
      {
        if (reader.MoveToAttribute("val"))
        {
          autoFilter.AddTextFilter(reader.Value);
          reader.Skip();
        }
      }
      else
      {
        int month = 1;
        int day = 1;
        int hour = 0;
        int minute = 0;
        int second = 0;
        if (reader.MoveToAttribute("year"))
        {
          int int32 = XmlConvertExtension.ToInt32(reader.Value);
          if (reader.MoveToAttribute("month"))
          {
            month = XmlConvertExtension.ToInt32(reader.Value);
            if (reader.MoveToAttribute("day"))
            {
              day = XmlConvertExtension.ToInt32(reader.Value);
              if (reader.MoveToAttribute("hour"))
              {
                hour = XmlConvertExtension.ToInt32(reader.Value);
                if (reader.MoveToAttribute("minute"))
                {
                  minute = XmlConvertExtension.ToInt32(reader.Value);
                  if (reader.MoveToAttribute("second"))
                    second = XmlConvertExtension.ToInt32(reader.Value);
                }
              }
            }
          }
          if (reader.MoveToAttribute("dateTimeGrouping"))
            autoFilter.AddDateFilter(new DateTime(int32, month, day, hour, minute, second), (DateTimeGroupingType) Enum.Parse(typeof (DateTimeGroupingType), reader.Value, true));
          reader.Skip();
        }
      }
    }
  }

  private void ParseColorFilter(XmlReader reader, AutoFilterImpl autoFilter)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoFilter == null)
      throw new ArgumentNullException(nameof (autoFilter));
    List<DxfImpl> dxfsCollection = this.m_book.DataHolder.ParseDxfsCollection();
    int index = int.Parse(reader.GetAttribute("dxfId"));
    ExcelColorFilterType colorFilterType = reader.GetAttribute("cellColor") == "0" ? ExcelColorFilterType.FontColor : ExcelColorFilterType.CellColor;
    DxfImpl dxfImpl = dxfsCollection[index];
    Color color = dxfImpl.Fill.ColorObject.ColorType == ColorType.Indexed ? Color.FromArgb(0, 0, 0, 0) : dxfImpl.Fill.ColorObject.GetRGB((IWorkbook) this.m_book);
    autoFilter.AddColorFilter(color, colorFilterType);
  }

  private void ParseIconFilter(XmlReader reader, AutoFilterImpl autoFilter)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoFilter == null)
      throw new ArgumentNullException(nameof (autoFilter));
    int iconId = reader.GetAttribute("iconId") == null ? -1 : int.Parse(reader.GetAttribute("iconId"));
    ExcelIconSetType iconSetType = (ExcelIconSetType) Array.IndexOf<string>(CF.IconSetTypeNames, reader.GetAttribute("iconSet"));
    autoFilter.AddIconFilter(iconSetType, iconId);
  }

  private void ParseAutoFilterTopTen(XmlReader reader, AutoFilterImpl autoFilter)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoFilter == null)
      throw new ArgumentNullException(nameof (autoFilter));
    autoFilter.IsTop10 = true;
    autoFilter.IsTop = true;
    if (reader.MoveToAttribute("top"))
      autoFilter.IsTop = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("percent"))
      autoFilter.IsPercent = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("val"))
      autoFilter.Top10Number = XmlConvertExtension.ToInt32(reader.Value);
    if (!reader.MoveToAttribute("filterVal"))
      return;
    AutoFilterConditionImpl firstCondition = (AutoFilterConditionImpl) autoFilter.FirstCondition;
    firstCondition.ConditionOperator = ExcelFilterCondition.GreaterOrEqual;
    firstCondition.DataType = ExcelFilterDataType.FloatingPoint;
    firstCondition.Double = XmlConvertExtension.ToDouble(reader.Value);
  }

  private void ParseCustomFilters(XmlReader reader, AutoFilterImpl autoFilter)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoFilter == null)
      throw new ArgumentNullException(nameof (autoFilter));
    autoFilter.IsAnd = reader.MoveToAttribute("and") && XmlConvertExtension.ToBoolean(reader.Value);
    autoFilter.SelectRangesToFilter();
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "customFilter")
        this.ParseCustomFilter(reader, autoFilter);
      reader.Skip();
    }
  }

  private void ParseCustomFilter(XmlReader reader, AutoFilterImpl autoFilter)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoFilter == null)
      throw new ArgumentNullException(nameof (autoFilter));
    AutoFilterConditionImpl condition = autoFilter.IsFirstCondition ? (AutoFilterConditionImpl) autoFilter.SecondCondition : (AutoFilterConditionImpl) autoFilter.FirstCondition;
    condition.ConditionOperator = ExcelFilterCondition.Equal;
    if (reader.MoveToAttribute("operator"))
      condition.ConditionOperator = this.ConvertAutoFormatFilterCondition(reader.Value);
    if (!reader.MoveToAttribute("val"))
      return;
    this.ChangeConditionOperator(condition, reader.Value);
    condition.String = reader.Value;
    condition.DataType = ExcelFilterDataType.String;
  }

  private void ChangeConditionOperator(AutoFilterConditionImpl condition, string CondtionValue)
  {
    string str = CondtionValue;
    if (string.IsNullOrEmpty(str) || str.Equals("*"))
      return;
    if (str.StartsWith("*") && str.EndsWith("*"))
      condition.ConditionOperator = condition.ConditionOperator == ExcelFilterCondition.Equal ? ExcelFilterCondition.Contains : ExcelFilterCondition.DoesNotContain;
    else if (str.StartsWith("*"))
    {
      condition.ConditionOperator = condition.ConditionOperator == ExcelFilterCondition.Equal ? ExcelFilterCondition.EndsWith : ExcelFilterCondition.DoesNotEndWith;
    }
    else
    {
      if (!str.EndsWith("*"))
        return;
      condition.ConditionOperator = condition.ConditionOperator == ExcelFilterCondition.Equal ? ExcelFilterCondition.BeginsWith : ExcelFilterCondition.DoesNotBeginWith;
    }
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
    this.m_book.LineStyles = this.m_dicLineStyles;
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
            themeElements.Reverse(0, 2);
            themeElements.Reverse(2, 2);
            dicThemeColors.Add("tx1", themeElements[1]);
            dicThemeColors.Add("tx2", themeElements[3]);
            this.m_dicThemeColors = dicThemeColors;
            continue;
          case "fontScheme":
            this.ParseFontScheme(reader);
            continue;
          case "fmtScheme":
            this.ParseFormatScheme(reader);
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

  private void ParseFormatScheme(XmlReader reader)
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
          case "lnStyleLst":
            this.ParseLineStyles(reader, out this.m_dicLineStyles);
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

  private void ParseLineStyles(
    XmlReader reader,
    out Dictionary<int, ShapeLineFormatImpl> m_dicLineStyles)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    m_dicLineStyles = new Dictionary<int, ShapeLineFormatImpl>();
    int key = 1;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "ln":
            ShapeLineFormatImpl border = new ShapeLineFormatImpl(this.m_book.Application, (object) this.m_book);
            TextBoxShapeParser.ParseLineProperties(reader, border, this);
            m_dicLineStyles.Add(key, border);
            ++key;
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
    reader.Read();
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
      font = (FontImpl) this.m_book.CreateFont((IFont) null, false);
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
    DxfImpl dxfImpl = new DxfImpl();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        switch (reader.LocalName)
        {
          case "numFmt":
            dxfImpl.FormatRecord = this.ParseDxfNumberFormat(reader);
            reader.Skip();
            continue;
          case "fill":
            dxfImpl.Fill = this.ParseFill(reader, false);
            reader.Skip();
            continue;
          case "font":
            dxfImpl.Font = this.ParseFont(reader);
            reader.Skip();
            continue;
          case "border":
            dxfImpl.Borders = this.ParseBordersCollection(reader, dxfImpl);
            continue;
          case "alignment":
            this.ParseAlignment(reader, dxfImpl);
            dxfImpl.HasAlignment = true;
            continue;
          case "protection":
            this.ParseProtection(reader, dxfImpl);
            dxfImpl.HasProtection = true;
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    reader.Read();
    return dxfImpl;
  }

  public void ParseSheetConditionalFormatting(
    XmlReader reader,
    WorksheetConditionalFormats sheetConditionalFormats,
    List<DxfImpl> lstDxfs)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType == XmlNodeType.None)
      return;
    if (sheetConditionalFormats == null)
      throw new ArgumentNullException(nameof (sheetConditionalFormats));
    bool throwOnUnknownNames = this.m_book.ThrowOnUnknownNames;
    this.m_book.ThrowOnUnknownNames = false;
    ConditionalFormats conditionalFormats = (ConditionalFormats) null;
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      if (reader.LocalName == "conditionalFormatting")
      {
        conditionalFormats = new ConditionalFormats(this.m_book.Application, (object) sheetConditionalFormats);
        this.ParseConditionalFormatting(reader, conditionalFormats, lstDxfs);
        if (conditionalFormats.Count != 0)
        {
          if (conditionalFormats.CondFMTRecord != null && conditionalFormats.CondFMTRecord.CellList.Count > 0 && conditionalFormats.CondFMTRecord.CFNumber == (ushort) 0)
            conditionalFormats.CondFMTRecord.CFNumber = (ushort) conditionalFormats.InnerList.Count;
          if (conditionalFormats.CondFMT12Record != null && conditionalFormats.CondFMT12Record.CellList.Count > 0 && conditionalFormats.CondFMT12Record.CF12RecordCount == (ushort) 0)
            conditionalFormats.CondFMT12Record.CF12RecordCount = (ushort) conditionalFormats.InnerList.Count;
          sheetConditionalFormats.Add(conditionalFormats);
        }
      }
      else
        reader.Read();
    }
    this.UpdateUsedRange(conditionalFormats);
    reader.Read();
    this.m_book.ThrowOnUnknownNames = throwOnUnknownNames;
  }

  internal void ParseCustomTableStyles(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentException(nameof (reader));
    FileDataHolder dataHolder = this.m_book.DataHolder;
    this.ParseTableStylesFromExcel2007(reader, dataHolder);
  }

  internal void ParseTableStylesFromExcel2007(XmlReader reader, FileDataHolder dataHolder)
  {
    if (dataHolder == null)
      return;
    List<DxfImpl> dxfsCollection = dataHolder.ParseDxfsCollection();
    if (dxfsCollection == null)
      return;
    this.ParseTableStyles(reader, dxfsCollection);
  }

  internal void ParseTableStyles(XmlReader reader, List<DxfImpl> dxfStyles)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType == XmlNodeType.None)
      return;
    if (reader.LocalName != "tableStyles")
      throw new XmlException(nameof (reader));
    if (reader.NodeType == XmlNodeType.EndElement || !(reader.LocalName == "tableStyles"))
      return;
    TableStyles tableStyles = this.m_book.TableStyles as TableStyles;
    if (reader.MoveToAttribute("defaultTableStyle"))
      tableStyles.DefaultTablesStyle = reader.Value;
    if (reader.MoveToAttribute("defaultPivotStyle"))
      tableStyles.DefaultPivotTableStyle = reader.Value;
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement && reader.LocalName == "tableStyle")
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "tableStyle":
            TableStyle tableStyle = new TableStyle(tableStyles);
            this.ParseTableStyle(reader, tableStyle, dxfStyles);
            tableStyles.Add((ITableStyle) tableStyle);
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

  internal void ParseTableStyle(XmlReader reader, TableStyle tableStyle, List<DxfImpl> dxfStyles)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType == XmlNodeType.None)
      return;
    if (reader.LocalName != nameof (tableStyle))
      throw new XmlException(nameof (reader));
    if (tableStyle == null)
      throw new ArgumentNullException(nameof (tableStyle));
    if (reader.NodeType == XmlNodeType.EndElement)
      return;
    if (reader.MoveToAttribute("name"))
      tableStyle.Name = reader.Value;
    TableStyleElements tableStyleElements = tableStyle.TableStyleElements as TableStyleElements;
    if (reader.IsEmptyElement)
      return;
    reader.Read();
    if (!(reader.LocalName != nameof (tableStyle)))
      return;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "tableStyleElement":
            TableStyleElement tableStyleElement = new TableStyleElement(tableStyleElements);
            this.ParseTableStyleElement(reader, tableStyleElement, dxfStyles);
            tableStyleElements.Add((ITableStyleElement) tableStyleElement);
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

  internal void ParseTableStyleElement(
    XmlReader reader,
    TableStyleElement tableStyleElement,
    List<DxfImpl> dxfStyles)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType == XmlNodeType.None)
      return;
    if (reader.LocalName != nameof (tableStyleElement))
      throw new XmlException(nameof (reader));
    if (tableStyleElement == null)
      throw new ArgumentNullException(nameof (tableStyleElement));
    if (reader.NodeType == XmlNodeType.EndElement)
      return;
    if (reader.MoveToAttribute("type"))
    {
      switch (reader.Value)
      {
        case "wholeTable":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.WholeTable;
          break;
        case "headerRow":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.HeaderRow;
          break;
        case "totalRow":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.TotalRow;
          break;
        case "firstColumn":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.FirstColumn;
          break;
        case "lastColumn":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.LastColumn;
          break;
        case "firstRowStripe":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.FirstRowStripe;
          break;
        case "secondRowStripe":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.SecondRowStripe;
          break;
        case "firstColumnStripe":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.FirstColumnStripe;
          break;
        case "secondColumnStripe":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.SecondColumnStripe;
          break;
        case "firstHeaderCell":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.FirstHeaderCell;
          break;
        case "lastHeaderCell":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.LastHeaderCell;
          break;
        case "firstTotalCell":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.FirstTotalCell;
          break;
        case "lastTotalCell":
          tableStyleElement.TableStyleElementType = ExcelTableStyleElementType.LastTotalCell;
          break;
        default:
          tableStyleElement.TableStyleElementName = reader.Value;
          break;
      }
    }
    if (reader.MoveToAttribute("size") && int.Parse(reader.Value) > 1)
      tableStyleElement.StripeSize = int.Parse(reader.Value);
    if (!reader.MoveToAttribute("dxfId"))
      return;
    int int32 = XmlConvertExtension.ToInt32(reader.Value);
    dxfStyles[int32].FillTableStyle(tableStyleElement);
  }

  private void UpdateUsedRange(ConditionalFormats conditionalFormats)
  {
    if (conditionalFormats == null)
      return;
    WorksheetImpl sheet = conditionalFormats.sheet;
    if (this.m_book.Loading || this.m_book.Saving || !sheet.UsedRangeIncludesCF)
      return;
    WorksheetBaseImpl worksheetBaseImpl = (WorksheetBaseImpl) sheet;
    IRange usedRange = sheet.UsedRange;
    if (usedRange.Row > this.minRow)
      worksheetBaseImpl.FirstRow = this.minRow;
    if (usedRange.Column > this.minColumn)
      worksheetBaseImpl.FirstColumn = this.minColumn;
    if (usedRange.LastRow < this.maxRow && this.maxRow < sheet.ParentWorkbook.MaxRowCount)
      worksheetBaseImpl.LastRow = this.maxRow;
    if (usedRange.LastColumn < this.maxColumn)
      worksheetBaseImpl.LastColumn = this.maxColumn;
    if (worksheetBaseImpl.FirstRow < 0 && sheet.LastRow > 0)
      worksheetBaseImpl.FirstRow = this.minRow;
    if (worksheetBaseImpl.FirstColumn != int.MaxValue || sheet.LastColumn <= 0)
      return;
    worksheetBaseImpl.FirstColumn = this.minColumn;
  }

  public bool ParseConditionalFormatting(
    XmlReader reader,
    ConditionalFormats conditionalFormats,
    List<DxfImpl> lstDxfs)
  {
    bool hasExLst = false;
    if (reader == null || reader.LocalName != "conditionalFormatting")
      throw new ArgumentException(nameof (reader));
    if (conditionalFormats == null)
      throw new ArgumentNullException(nameof (conditionalFormats));
    bool conditionalFormatting = true;
    if (reader.MoveToAttribute("sqref"))
    {
      this.UpdateCFRange(reader.Value, conditionalFormats.sheet);
      foreach (TAddr taddr in this.GetRangesForDataValidation(reader.Value))
      {
        Rectangle rectangle = taddr.GetRectangle();
        conditionalFormats.AddRange(rectangle);
        conditionalFormats.EnclosedRange = taddr;
      }
    }
    else
      hasExLst = true;
    if (reader.MoveToAttribute("pivot"))
      conditionalFormats.Pivot = this.ParseBoolean(reader, "pivot", false);
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "cfRule":
            this.ParseCFRuleTag(reader, conditionalFormats, lstDxfs, hasExLst);
            continue;
          default:
            reader.Read();
            continue;
        }
      }
      else
        reader.Skip();
    }
    reader.Read();
    return conditionalFormatting;
  }

  private void UpdateCFRange(string address, WorksheetImpl worksheet)
  {
    if (this.m_book.Loading || this.m_book.Saving || !worksheet.UsedRangeIncludesCF)
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

  private string ParseRangeReference(XmlReader reader, IConditionalFormat format)
  {
    string empty = string.Empty;
    ConditionalFormatImpl conditionalFormatImpl = format as ConditionalFormatImpl;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (reader.LocalName == "sqref")
    {
      reader.Read();
      empty = reader.Value;
      conditionalFormatImpl.RangeRefernce = empty;
      reader.Read();
    }
    return empty;
  }

  public void ParseCFRuleTag(
    XmlReader reader,
    ConditionalFormats conditionalFormats,
    List<DxfImpl> lstDxfs,
    bool hasExLst)
  {
    string refRange = string.Empty;
    this.ParseCFRuleTag(reader, conditionalFormats, lstDxfs, hasExLst, out refRange);
  }

  private void ParseCFRuleTag(
    XmlReader reader,
    ConditionalFormats conditionalFormats,
    List<DxfImpl> lstDxfs,
    bool hasExLst,
    out string refRange)
  {
    refRange = string.Empty;
    bool bIsSupportedType = false;
    bool bIsSupportedOperator = false;
    bool flag = false;
    bool bIsSupportedTimePeriod = false;
    int num1 = 1;
    int num2 = 0;
    string empty = string.Empty;
    string str = string.Empty;
    ExcelCFType excelCfType = ExcelCFType.CellValue;
    ExcelComparisonOperator comparisonOperator = ExcelComparisonOperator.Between;
    CFTimePeriods cfTimePeriods = CFTimePeriods.Yesterday;
    if (reader.MoveToAttribute("type"))
      excelCfType = this.ConvertCFType(reader.Value, out bIsSupportedType);
    if (reader.MoveToAttribute("stopIfTrue"))
      flag = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("priority"))
      num1 = int.Parse(reader.Value);
    if (reader.MoveToAttribute("dxfId"))
      num2 = int.Parse(reader.Value);
    if (reader.MoveToAttribute("id"))
      empty = reader.Value;
    if (reader.MoveToAttribute("timePeriod"))
      cfTimePeriods = this.ConvertCFTimePeriods(reader.Value, out bIsSupportedTimePeriod);
    if (reader.MoveToAttribute("operator"))
    {
      if (excelCfType == ExcelCFType.TimePeriod)
        cfTimePeriods = this.ConvertCFTimePeriods(reader.Value, out bIsSupportedTimePeriod);
      else
        comparisonOperator = this.ConvertCFOperator(reader.Value, out bIsSupportedOperator);
    }
    if (reader.MoveToAttribute("text"))
      str = reader.Value.Replace("\"", "\"\"");
    if (hasExLst && excelCfType == ExcelCFType.DataBar)
    {
      this.ParseExtCFRules(reader, conditionalFormats);
    }
    else
    {
      if (!bIsSupportedType)
        return;
      string conditionalFormatFirstFormula = (string) null;
      string conditionalFormatSecondFormula = (string) null;
      IInternalConditionalFormat conditionalFormat = conditionalFormats.AddCondition() as IInternalConditionalFormat;
      conditionalFormat.FormatType = excelCfType;
      conditionalFormat.StopIfTrue = flag;
      ConditionalFormatImpl conditionalFormatImpl = conditionalFormat as ConditionalFormatImpl;
      conditionalFormatImpl.CFHasExtensionList = hasExLst;
      if (conditionalFormatImpl != null)
      {
        conditionalFormatImpl.Priority = num1;
        conditionalFormatImpl.StartDxf = num2;
        conditionalFormatImpl.CFRuleID = empty;
      }
      if (bIsSupportedOperator)
        conditionalFormat.Operator = comparisonOperator;
      else if (bIsSupportedTimePeriod)
        conditionalFormat.TimePeriodType = cfTimePeriods;
      if (excelCfType == ExcelCFType.SpecificText && str != string.Empty)
        conditionalFormat.Text = str;
      if (excelCfType == ExcelCFType.TopBottom)
      {
        if (reader.MoveToAttribute("bottom"))
          conditionalFormat.TopBottom.Type = XmlConvertExtension.ToBoolean(reader.Value) ? ExcelCFTopBottomType.Bottom : ExcelCFTopBottomType.Top;
        if (reader.MoveToAttribute("percent"))
          conditionalFormat.TopBottom.Percent = XmlConvertExtension.ToBoolean(reader.Value);
        if (reader.MoveToAttribute("rank"))
          conditionalFormat.TopBottom.Rank = int.Parse(reader.Value);
      }
      if (excelCfType == ExcelCFType.AboveBelowAverage)
      {
        int num3 = 0;
        conditionalFormat.AboveBelowAverage.AverageType = ExcelCFAverageType.Above;
        int num4 = 0;
        if (reader.MoveToAttribute("aboveAverage"))
          num3 += XmlConvertExtension.ToBoolean(reader.Value) ? 0 : 1;
        if (reader.MoveToAttribute("equalAverage"))
          num3 += XmlConvertExtension.ToBoolean(reader.Value) ? 2 : 0;
        if (reader.MoveToAttribute("stdDev"))
        {
          num4 = int.Parse(reader.Value);
          num3 += num4 <= 0 || num4 >= 4 ? 0 : 4;
        }
        conditionalFormat.AboveBelowAverage.AverageType = (ExcelCFAverageType) num3;
        if (num4 > 0 && num4 < 4)
          conditionalFormat.AboveBelowAverage.StdDevValue = num4;
      }
      if (conditionalFormatImpl.CFHasExtensionList && conditionalFormatImpl.IconSet != null)
      {
        this.m_cfType = ExcelCFType.IconSet;
        this.m_hasCFExtlst = true;
        this.ParseConditionFormatRule(reader, conditionalFormat, lstDxfs, ref conditionalFormatFirstFormula, ref conditionalFormatSecondFormula);
        this.m_cfType = ExcelCFType.Blank;
        this.m_hasCFExtlst = false;
      }
      else
      {
        this.ParseConditionFormatRule(reader, conditionalFormat, lstDxfs, ref conditionalFormatFirstFormula, ref conditionalFormatSecondFormula);
        if (conditionalFormatImpl.FormatType == ExcelCFType.ColorScale && conditionalFormatImpl.CFHasExtensionList)
          conditionalFormatImpl.CFHasExtensionList = false;
      }
      string refRange1 = string.Empty;
      if (reader.LocalName == "sqref")
        refRange1 = this.ParseRangeReference(reader, (IConditionalFormat) conditionalFormat);
      else if (hasExLst && reader.NodeType != XmlNodeType.EndElement && reader.LocalName == "cfRule")
      {
        this.ParseCFRuleTag(reader, conditionalFormats, lstDxfs, hasExLst, out refRange1);
        (conditionalFormat as ConditionalFormatImpl).RangeRefernce = refRange1;
      }
      if (!string.IsNullOrEmpty(refRange1))
      {
        foreach (TAddr taddr in this.GetRangesForDataValidation(refRange1))
        {
          Rectangle rectangle = taddr.GetRectangle();
          conditionalFormats.AddRange(rectangle);
          conditionalFormats.EnclosedRange = taddr;
        }
        refRange = refRange1;
      }
      if (!string.IsNullOrEmpty(conditionalFormatFirstFormula))
      {
        (conditionalFormat as ConditionalFormatImpl).FirstFormula = conditionalFormatFirstFormula;
        conditionalFormatFirstFormula = (string) null;
      }
      if (string.IsNullOrEmpty(conditionalFormatSecondFormula))
        return;
      (conditionalFormat as ConditionalFormatImpl).SecondFormula = conditionalFormatSecondFormula;
    }
  }

  private void ParseConditionFormatRule(
    XmlReader reader,
    IInternalConditionalFormat conFormat,
    List<DxfImpl> lstDxfs,
    ref string conditionalFormatFirstFormula,
    ref string conditionalFormatSecondFormula)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (conFormat == null)
      throw new ArgumentNullException(nameof (conFormat));
    if (reader.MoveToAttribute("dxfId"))
    {
      int int32 = XmlConvertExtension.ToInt32(reader.Value);
      lstDxfs[int32].FillCondition(conFormat);
    }
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          ConditionalFormatImpl conditionalFormatImpl = (ConditionalFormatImpl) conFormat;
          switch (reader.LocalName)
          {
            case "f":
            case "formula":
              this.ParseCFFormulas(reader, conditionalFormatImpl, ref conditionalFormatFirstFormula, ref conditionalFormatSecondFormula);
              continue;
            case "dxf":
              this.ParseDxfStyle(reader).FillCondition(conFormat);
              continue;
            case "dataBar":
              this.ParseDataBar(reader, conditionalFormatImpl.InnerDataBar, (IWorkbook) conditionalFormatImpl.Workbook);
              continue;
            case "iconSet":
              this.ParseIconSet(reader, conditionalFormatImpl.IconSet, (IWorkbook) conditionalFormatImpl.Workbook);
              continue;
            case "colorScale":
              this.ParseColorScale(reader, conditionalFormatImpl.ColorScale, conditionalFormatImpl.Workbook);
              continue;
            case "extLst":
              this.ParseExtention(reader, conditionalFormatImpl);
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
    if (conFormat.FormatType == ExcelCFType.ColorScale && (conFormat as ConditionalFormatImpl).CFHasExtensionList)
      reader.Read();
    reader.Read();
  }

  private void ParseColorScale(XmlReader reader, IColorScale colorScale, WorkbookImpl book)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (colorScale == null)
      throw new ArgumentNullException(nameof (colorScale));
    if (reader.LocalName != nameof (colorScale))
      throw new XmlException();
    IList<IColorConditionValue> criteria = colorScale.Criteria;
    criteria.Clear();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      int index = 0;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "cfvo":
              ColorConditionValue condition = new ColorConditionValue();
              this.ParseCFValueObject(reader, (IWorkbook) book, (ConditionValue) condition);
              criteria.Add((IColorConditionValue) condition);
              continue;
            case "color":
              criteria[index].FormatColorRGB = this.ParseColor(reader).GetRGB((IWorkbook) book);
              ++index;
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
    reader.Read();
  }

  private void ParseDataBar(XmlReader reader, DataBarImpl dataBar, IWorkbook book)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataBar == (DataBarImpl) null)
      throw new ArgumentNullException(nameof (dataBar));
    if (reader.LocalName != nameof (dataBar))
      throw new XmlException();
    if (reader.MoveToAttribute("minLength"))
      dataBar.PercentMin = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("maxLength"))
      dataBar.PercentMax = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("showValue"))
      dataBar.ShowValue = XmlConvertExtension.ToBoolean(reader.Value);
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      int num = 0;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "cfvo":
              ConditionValue condition = new ConditionValue();
              this.ParseCFValueObject(reader, book, condition);
              if (num == 0)
              {
                dataBar.MinPoint = (IConditionValue) condition;
              }
              else
              {
                if (num != 1)
                  throw new XmlException();
                dataBar.MaxPoint = (IConditionValue) condition;
              }
              ++num;
              continue;
            case "color":
              dataBar.BarColor = this.ParseColor(reader).GetRGB(book);
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
    reader.Read();
  }

  private void ParseIconSet(XmlReader reader, IIconSet iconSet, IWorkbook book)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (iconSet == null)
      throw new ArgumentNullException(nameof (iconSet));
    if (reader.LocalName != nameof (iconSet))
      throw new XmlException();
    bool flag = true;
    if (reader.MoveToAttribute(nameof (iconSet)))
    {
      flag = false;
      iconSet.IconSet = (ExcelIconSetType) Array.IndexOf<string>(CF.IconSetTypeNames, reader.Value);
    }
    if (flag)
      iconSet.IconSet = ExcelIconSetType.ThreeTrafficLights1;
    if (reader.MoveToAttribute("percent"))
      iconSet.PercentileValues = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("reverse"))
      iconSet.ReverseOrder = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("showValue"))
      iconSet.ShowIconOnly = !XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("custom"))
      (iconSet as IconSetImpl).SetCustomIcon(XmlConvertExtension.ToBoolean(reader.Value));
    reader.MoveToElement();
    IList<IConditionValue> iconCriteria = iconSet.IconCriteria;
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      int index1 = 0;
      int index2 = 0;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "cfvo":
              IconConditionValue condition = new IconConditionValue(iconSet.IconSet, index1);
              this.ParseCFValueObject(reader, book, (ConditionValue) condition);
              iconCriteria[index1] = (IConditionValue) condition;
              ++index1;
              continue;
            case "cfIcon":
              this.ParseCustomCFIcons(reader, iconCriteria[index2] as IIconConditionValue);
              ++index2;
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
    reader.Read();
  }

  private void ParseCFValueObject(XmlReader reader, IWorkbook book, ConditionValue condition)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    if (reader.LocalName != "cfvo")
      throw new XmlException();
    string strType = (string) null;
    string strFormula = (string) null;
    bool flag = true;
    if (reader.MoveToAttribute("type"))
      strType = reader.Value;
    if (reader.MoveToAttribute("val"))
      strFormula = reader.Value;
    if (reader.MoveToAttribute("gte"))
      flag = XmlConvertExtension.ToBoolean(reader.Value);
    if (this.m_hasCFExtlst && this.m_cfType == ExcelCFType.IconSet)
    {
      reader.Read();
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "f")
        {
          reader.Read();
          strFormula = reader.Value;
          reader.Skip();
        }
        reader.Skip();
      }
      reader.Skip();
    }
    condition.Operator = flag ? ConditionalFormatOperator.GreaterThanorEqualTo : ConditionalFormatOperator.GreaterThan;
    ConditionValueType valueType = this.GetValueType(strType);
    condition.Type = valueType;
    condition.Value = strFormula;
    if (string.IsNullOrEmpty(strFormula))
      return;
    condition.RefPtg = this.m_book.FormulaUtil.ParseString(strFormula);
  }

  private ConditionValueType GetValueType(string strType)
  {
    int num = Array.IndexOf<string>(CF.ValueTypes, strType);
    return num != 8 ? (ConditionValueType) num : ConditionValueType.Automatic;
  }

  private void ParseCFFormulas(
    XmlReader reader,
    ConditionalFormatImpl cFormat,
    ref string conditionalFormatFirstFormula,
    ref string conditionalFormatSecondFormula)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (cFormat == null)
      throw new ArgumentNullException(nameof (cFormat));
    string strFormula = (string) null;
    if (reader.LocalName == "f")
      cFormat.CFHasExtensionList = true;
    if (reader.IsEmptyElement)
      reader.Skip();
    if (reader.LocalName == "formula" || reader.LocalName == "f")
    {
      reader.Read();
      if (!cFormat.CFHasExtensionList && !cFormat.IsFormula)
      {
        cFormat.SetFirstSecondFormula(this.m_formulaUtil, reader.Value, true);
      }
      else
      {
        conditionalFormatFirstFormula = reader.Value;
        strFormula = reader.Value;
      }
      reader.Skip();
      reader.Skip();
    }
    if (reader.IsEmptyElement)
      reader.Skip();
    if (!(reader.LocalName == "formula") && !(reader.LocalName == "f"))
      return;
    reader.Read();
    if (!cFormat.CFHasExtensionList)
      cFormat.SetFirstSecondFormula(this.m_formulaUtil, reader.Value, false);
    else if (cFormat.FormatType == ExcelCFType.SpecificText)
    {
      cFormat.SetFirstSecondFormula(this.m_formulaUtil, strFormula, true);
      cFormat.SetFirstSecondFormula(this.m_formulaUtil, reader.Value, false);
      if (reader.Value.Contains("$"))
        cFormat.Range = (IRange) ((cFormat.Parent as ConditionalFormats).sheet.Range[reader.Value.Replace("$", string.Empty)] as RangeImpl);
    }
    else if (cFormat.FormatType == ExcelCFType.CellValue)
      conditionalFormatSecondFormula = reader.Value;
    else
      cFormat.AsteriskRange = reader.Value;
    reader.Skip();
    reader.Skip();
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
    pageSetup.PaperSize = !reader.MoveToAttribute("paperSize") ? ExcelPaperSize.PaperLetter : (ExcelPaperSize) XmlConvertExtension.ToInt32(reader.Value);
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
    if (reader.MoveToAttribute("pageOrder") && Enum.IsDefined(typeof (ExcelOrder), (object) Excel2007Serializator.CapitalizeFirstLetter(reader.Value)))
      pageSetup.Order = (ExcelOrder) Enum.Parse(typeof (ExcelOrder), reader.Value, true);
    if (reader.MoveToAttribute("orientation") && (reader.Value.ToUpper() == "LANDSCAPE" || reader.Value.ToUpper() == "PORTRAIT"))
      pageSetup.Orientation = (ExcelPageOrientation) Enum.Parse(typeof (ExcelPageOrientation), reader.Value, true);
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
          Page oddPage = pageSetup.OddPage as Page;
          Page evenPage = pageSetup.EvenPage as Page;
          Page firstPage = pageSetup.FirstPage as Page;
          switch (reader.LocalName)
          {
            case "oddHeader":
              string str1 = reader.ReadElementContentAsString();
              oddPage.FullHeaderString = str1;
              continue;
            case "oddFooter":
              string str2 = reader.ReadElementContentAsString();
              oddPage.FullFooterString = str2;
              continue;
            case "evenHeader":
              string str3 = reader.ReadElementContentAsString();
              evenPage.FullHeaderString = str3;
              continue;
            case "evenFooter":
              string str4 = reader.ReadElementContentAsString();
              evenPage.FullFooterString = str4;
              continue;
            case "firstHeader":
              string str5 = reader.ReadElementContentAsString();
              firstPage.FullHeaderString = str5;
              continue;
            case "firstFooter":
              string str6 = reader.ReadElementContentAsString();
              firstPage.FullFooterString = str6;
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

  private static ExcelPrintLocation StringToPrintComments(string printLocation)
  {
    switch (printLocation)
    {
      case "asDisplayed":
        return ExcelPrintLocation.PrintInPlace;
      case "none":
        return ExcelPrintLocation.PrintNoComments;
      case "atEnd":
        return ExcelPrintLocation.PrintSheetEnd;
      default:
        throw new ArgumentOutOfRangeException(nameof (printLocation));
    }
  }

  private static ExcelPrintErrors StringToPrintErrors(string printErrors)
  {
    switch (printErrors)
    {
      case "blank":
        return ExcelPrintErrors.PrintErrorsBlank;
      case "dash":
        return ExcelPrintErrors.PrintErrorsDash;
      case "displayed":
        return ExcelPrintErrors.PrintErrorsDisplayed;
      case "NA":
        return ExcelPrintErrors.PrintErrorsNA;
      default:
        throw new ArgumentOutOfRangeException("printLocation");
    }
  }

  private void ParseHyperlinks(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    HyperLinksCollection hyperlinks = sheet != null ? sheet.InnerHyperLinks : throw new ArgumentNullException(nameof (sheet));
    RelationCollection relations = sheet.DataHolder.Relations;
    List<string> stringList = new List<string>();
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "hyperlink")
        stringList.Add(this.ParseHyperlink(reader, sheet, hyperlinks, relations));
      reader.Skip();
    }
    foreach (string id in stringList)
      relations.Remove(id);
    reader.Skip();
  }

  private string ParseHyperlink(
    XmlReader reader,
    WorksheetImpl sheet,
    HyperLinksCollection hyperlinks,
    RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (hyperlinks));
    HyperLinkImpl link = new HyperLinkImpl(this.m_book.Application, (object) hyperlinks);
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    if (reader.MoveToAttribute("ref"))
    {
      TAddr rangeForDvOrAf = this.GetRangeForDVOrAF(reader.Value);
      IRange range1 = sheet[rangeForDvOrAf.FirstRow + 1, rangeForDvOrAf.FirstCol + 1, rangeForDvOrAf.LastRow + 1, rangeForDvOrAf.LastCol + 1];
      link.Range = range1;
      IRange range2 = sheet[rangeForDvOrAf.FirstRow + 1, rangeForDvOrAf.FirstCol + 1];
      link.TextToDisplay = range2.HasFormula ? range2.FormulaStringValue : range2.Text;
    }
    if (reader.MoveToAttribute("tooltip"))
      link.ScreenTip = reader.Value;
    if (reader.MoveToAttribute("location"))
      empty2 = reader.Value;
    if (reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
    {
      empty1 = reader.Value;
      Relation relation = relations[empty1];
      string strAddress = !relation.Target.StartsWith("mailto") ? Uri.UnescapeDataString(relation.Target) : relation.Target;
      if (strAddress.StartsWith("file:///"))
        strAddress = strAddress.Remove(0, "file:///".Length);
      link.Type = !strAddress.StartsWith("\\\\") ? (strAddress.StartsWith("mailto") || strAddress.IndexOf("://") != -1 || strAddress.StartsWith("javascript:") || strAddress.ToLower().StartsWith("http:") || strAddress.ToLower().StartsWith("https:") || strAddress.IndexOfAny(Path.GetInvalidFileNameChars()) != -1 && !strAddress.Contains("\\") && !strAddress.Contains("//") ? ExcelHyperLinkType.Url : ExcelHyperLinkType.File) : ExcelHyperLinkType.Unc;
      link.SetAddress(strAddress, false);
      link.SetSubAddress(empty2);
    }
    else
    {
      link.Type = ExcelHyperLinkType.Workbook;
      link.SetAddress(empty2, false);
    }
    if (link.TextToDisplay == string.Empty && reader.MoveToAttribute("display"))
      link.TextToDisplay = reader.Value;
    hyperlinks.Add((IHyperLink) link);
    hyperlinks.AddToHash(link);
    return empty1;
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
    reader.MoveToElement();
    reader.Skip();
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

  private void ParseBackgroundImage(
    XmlReader reader,
    WorksheetBaseImpl worksheetBase,
    string strParentPath)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (worksheetBase == null)
      throw new ArgumentNullException("sheet");
    if (strParentPath == null)
      throw new ArgumentNullException(nameof (strParentPath));
    if (reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
    {
      if (this.m_bgImages == null)
        this.m_bgImages = new List<ZipArchiveItem>();
      string id = reader.Value;
      WorksheetDataHolder dataHolder = worksheetBase.DataHolder;
      RelationCollection relations = dataHolder.Relations;
      Relation relation = relations[id];
      FileDataHolder parentHolder = dataHolder.ParentHolder;
      ZipArchiveItem zipArchiveItem = parentHolder[relation, strParentPath];
      Stream stream = (Stream) new MemoryStream();
      MemoryStream memoryStream = (MemoryStream) null;
      if (zipArchiveItem != null)
      {
        memoryStream = (MemoryStream) zipArchiveItem.DataStream;
        this.m_bgImages.Add(zipArchiveItem);
        parentHolder.Archive.RemoveItem(zipArchiveItem.ItemName);
      }
      else if (this.m_bgImages.Count > 0)
      {
        foreach (ZipArchiveItem bgImage in this.m_bgImages)
        {
          if (bgImage.ItemName.Contains(relation.Target.Replace("../media/", string.Empty)) || bgImage.ItemName.Contains(relation.Target.Replace("xl/media/", string.Empty)))
          {
            memoryStream = (MemoryStream) bgImage.DataStream;
            worksheetBase.sharedBgImageName = bgImage.ItemName;
            break;
          }
        }
      }
      memoryStream.WriteTo(stream);
      if (worksheetBase is WorksheetImpl)
        (worksheetBase as WorksheetImpl).PageSetup.BackgoundImage = (Bitmap) Image.FromStream(stream);
      else
        (worksheetBase as ChartImpl).PageSetup.BackgoundImage = (Bitmap) Image.FromStream(stream);
      relations.Remove(id);
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

  public void ParseDocumentCoreProperties(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "coreProperties")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    if (reader.IsEmptyElement)
      return;
    IBuiltInDocumentProperties documentProperties = this.m_book.BuiltInDocumentProperties;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "category":
            documentProperties.Category = this.GetReaderElementValue(reader);
            continue;
          case "created":
            documentProperties.CreationDate = DateTime.Parse(this.GetReaderElementValue(reader));
            continue;
          case "creator":
            documentProperties.Author = this.GetReaderElementValue(reader);
            continue;
          case "description":
            documentProperties.Comments = this.GetReaderElementValue(reader);
            continue;
          case "keywords":
            documentProperties.Keywords = this.GetReaderElementValue(reader);
            continue;
          case "lastModifiedBy":
            documentProperties.LastAuthor = this.GetReaderElementValue(reader);
            continue;
          case "lastPrinted":
            documentProperties.LastPrinted = DateTime.Parse(this.GetReaderElementValue(reader));
            continue;
          case "modified":
            documentProperties.LastSaveDate = DateTime.Parse(this.GetReaderElementValue(reader));
            continue;
          case "subject":
            documentProperties.Subject = this.GetReaderElementValue(reader);
            continue;
          case "title":
            documentProperties.Title = this.GetReaderElementValue(reader);
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

  public void ParseExtendedProperties(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "Properties")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    if (reader.IsEmptyElement)
      return;
    IBuiltInDocumentProperties documentProperties = this.m_book.BuiltInDocumentProperties;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "Application":
            documentProperties.ApplicationName = this.GetReaderElementValue(reader);
            continue;
          case "Characters":
            documentProperties.CharCount = XmlConvertExtension.ToInt32(this.GetReaderElementValue(reader));
            continue;
          case "Company":
            documentProperties.Company = this.GetReaderElementValue(reader);
            continue;
          case "Lines":
            documentProperties.LineCount = XmlConvertExtension.ToInt32(this.GetReaderElementValue(reader));
            continue;
          case "HeadingPairs":
            ((BuiltInDocumentProperties) documentProperties).HasHeadingPair = true;
            reader.Skip();
            continue;
          case "Manager":
            documentProperties.Manager = this.GetReaderElementValue(reader);
            continue;
          case "MMClips":
            documentProperties.MultimediaClipCount = XmlConvertExtension.ToInt32(this.GetReaderElementValue(reader));
            continue;
          case "Notes":
            documentProperties.SlideCount = XmlConvertExtension.ToInt32(this.GetReaderElementValue(reader));
            continue;
          case "Pages":
            documentProperties.PageCount = XmlConvertExtension.ToInt32(this.GetReaderElementValue(reader));
            continue;
          case "Paragraphs":
            documentProperties.ParagraphCount = XmlConvertExtension.ToInt32(this.GetReaderElementValue(reader));
            continue;
          case "PresentationFormat":
            documentProperties.PresentationTarget = this.GetReaderElementValue(reader);
            continue;
          case "Template":
            documentProperties.Template = this.GetReaderElementValue(reader);
            continue;
          case "TotalTime":
            documentProperties.EditTime = TimeSpan.FromMinutes((double) XmlConvertExtension.ToInt32(this.GetReaderElementValue(reader)));
            continue;
          case "Words":
            documentProperties.WordCount = XmlConvertExtension.ToInt32(this.GetReaderElementValue(reader));
            continue;
          case "HyperlinkBase":
            ((DocumentPropertyImpl) ((CustomDocumentProperties) this.m_book.CustomDocumentProperties).Add("_PID_LINKBASE")).Blob = Encoding.Unicode.GetBytes(this.GetReaderElementValue(reader) + "\0");
            continue;
          case "AppVersion":
            double num = reader.ReadElementContentAsDouble();
            if (num > 16.0)
            {
              this.m_book.Version = ExcelVersion.Xlsx;
              continue;
            }
            if (num > 15.0)
            {
              this.m_book.Version = ExcelVersion.Excel2013;
              continue;
            }
            if (num > 14.0)
            {
              this.m_book.Version = ExcelVersion.Excel2010;
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
    if (((BuiltInDocumentProperties) documentProperties).HasHeadingPair)
      return;
    if (documentProperties.ApplicationName != null && documentProperties.ApplicationName != string.Empty)
      documentProperties.ApplicationName = documentProperties.ApplicationName;
    else
      documentProperties.ApplicationName = "Essential XlsIO";
  }

  public void ParseCustomProperties(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    while (reader.NodeType != XmlNodeType.Element)
      reader.Read();
    if (reader.LocalName != "Properties")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    if (reader.IsEmptyElement)
      return;
    CustomDocumentProperties documentProperties = (CustomDocumentProperties) this.m_book.CustomDocumentProperties;
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        if (reader.LocalName == "property")
          this.ParseCustomProperty(reader, documentProperties);
      }
      else
        reader.Skip();
    }
  }

  public void ParseCustomProperty(XmlReader reader, CustomDocumentProperties customProperties)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (customProperties == null)
      throw new ArgumentNullException(nameof (customProperties));
    if (reader.LocalName != "property")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    DocumentPropertyImpl documentPropertyImpl = (DocumentPropertyImpl) null;
    if (reader.MoveToAttribute("name"))
      documentPropertyImpl = (DocumentPropertyImpl) customProperties.Add(reader.Value);
    if (reader.MoveToAttribute("linkTarget"))
    {
      documentPropertyImpl.LinkToContent = true;
      documentPropertyImpl.LinkSource = reader.Value;
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
            case "lpwstr":
              documentPropertyImpl.PropertyType = PropertyType.String;
              documentPropertyImpl.Text = this.GetReaderElementValue(reader);
              continue;
            case "lpstr":
              documentPropertyImpl.PropertyType = PropertyType.AsciiString;
              documentPropertyImpl.Text = this.GetReaderElementValue(reader);
              continue;
            case "filetime":
              documentPropertyImpl.PropertyType = PropertyType.DateTime;
              documentPropertyImpl.DateTime = DateTime.Parse(this.GetReaderElementValue(reader));
              continue;
            case "r8":
              documentPropertyImpl.PropertyType = PropertyType.Double;
              documentPropertyImpl.Double = XmlConvertExtension.ToDouble(this.GetReaderElementValue(reader));
              continue;
            case "i4":
              documentPropertyImpl.PropertyType = PropertyType.Int32;
              documentPropertyImpl.Int32 = XmlConvertExtension.ToInt32(this.GetReaderElementValue(reader));
              continue;
            case "int":
              documentPropertyImpl.PropertyType = PropertyType.Int;
              documentPropertyImpl.Integer = XmlConvertExtension.ToInt32(this.GetReaderElementValue(reader));
              continue;
            case "bool":
              documentPropertyImpl.PropertyType = PropertyType.Bool;
              documentPropertyImpl.Boolean = XmlConvertExtension.ToBoolean(this.GetReaderElementValue(reader));
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
        this.ParseDDELink(reader, relations);
        reader.Skip();
        externalLink = false;
        break;
      default:
        throw new XmlException("Unsupported xml tag");
    }
    return externalLink;
  }

  private void ParseDDELink(XmlReader reader, RelationCollection relaions)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "ddeLink")
      throw new XmlException("Unexpected xml tag");
    ExternWorkbookImpl book = new ExternWorkbookImpl(this.m_book.ExternWorkbooks.Application, (object) this.m_book.ExternWorkbooks);
    this.m_book.ExternWorkbooks.Add(book);
    string name = (string) null;
    string str1 = (string) null;
    string str2 = (string) null;
    if (reader.MoveToAttribute("ddeService"))
      str1 = reader.Value;
    if (reader.MoveToAttribute("ddeTopic"))
      str2 = reader.Value;
    book.URL = str1 + (object) '|' + str2;
    this.m_book.ExternWorkbooks.m_hashUrlToBook.Add(book.URL, book);
    reader.Read();
    if (reader.LocalName == "ddeItems")
    {
      reader.Read();
      while (reader.LocalName == "ddeItem")
      {
        if (reader.MoveToAttribute("name"))
        {
          name = reader.Value;
          book.ExternNames.Add(name);
        }
        if (reader.MoveToAttribute("advise"))
          book.ExternNames[name].isAdvise = true;
        if (reader.MoveToAttribute("ole"))
          book.ExternNames[name].isOle = true;
        reader.Read();
      }
    }
    book.IsDdeLink = true;
  }

  private void ParseOleObjectLink(XmlReader reader, RelationCollection relations)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "oleLink")
      throw new XmlException("Unexpected xml tag.");
    string str1 = (string) null;
    if (reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
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
    if (reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
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
              externBook = this.UpdateDefaultSheet(externBook, relations, strUrlId);
              this.ParseSheetDataSet(reader, externBook);
              continue;
            case "definedNames":
              externBook = this.UpdateDefaultSheet(externBook, relations, strUrlId);
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

  private ExternWorkbookImpl UpdateDefaultSheet(
    ExternWorkbookImpl externBook,
    RelationCollection relations,
    string strUrlId)
  {
    if (externBook == null)
      externBook = this.CreateExternBook(relations, strUrlId, new List<string>()
      {
        "Sheet1"
      });
    return externBook;
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
    if (reader.MoveToAttribute("sheetId"))
      externBook.ExternNames[index].sheetId = Convert.ToInt32(reader.Value);
    else
      externBook.ExternNames[index].sheetId = -1;
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
    if (externBook.Worksheets.ContainsKey(key))
    {
      ExternWorksheetImpl worksheet = externBook.Worksheets[key];
      reader.MoveToElement();
      worksheet.AdditionalAttributes = this.ParseSheetData(reader, (IInternalWorksheet) worksheet, (List<int>) null, "cell");
    }
    else
      reader.Skip();
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
          if (!sheetNames.Contains(str))
            sheetNames.Add(str);
        }
        reader.Read();
      }
    }
    reader.Read();
    return sheetNames;
  }

  private void ParseHorizontalPagebreaks(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      HPageBreaksCollection hpageBreaks = (HPageBreaksCollection) sheet.HPageBreaks;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.LocalName == "brk")
        {
          int int32_1 = reader.MoveToAttribute("id") ? XmlConvertExtension.ToInt32(reader.Value) : 0;
          int int32_2 = reader.MoveToAttribute("min") ? XmlConvertExtension.ToInt32(reader.Value) : 0;
          int EndCol = reader.MoveToAttribute("max") ? XmlConvertExtension.ToInt32(reader.Value) : 0;
          if (EndCol > sheet.Workbook.MaxColumnCount - 1)
            EndCol = this.m_book.MaxColumnCount - 1;
          HorizontalPageBreaksRecord.THPageBreak pagebreak = new HorizontalPageBreaksRecord.THPageBreak((ushort) int32_1, (ushort) int32_2, (ushort) EndCol);
          HPageBreakImpl pageBreak = new HPageBreakImpl(sheet.Application, (object) sheet, pagebreak);
          if (reader.MoveToAttribute("man"))
            pageBreak.Type = ExcelPageBreak.PageBreakManual;
          hpageBreaks.Add(pageBreak);
        }
        reader.Skip();
      }
      reader.Skip();
    }
    else
      reader.Skip();
  }

  private void ParseVerticalPagebreaks(XmlReader reader, WorksheetImpl sheet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    reader.Read();
    VPageBreaksCollection vpageBreaks = (VPageBreaksCollection) sheet.VPageBreaks;
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "brk")
      {
        VerticalPageBreaksRecord.TVPageBreak pagebreak = new VerticalPageBreaksRecord.TVPageBreak(reader.MoveToAttribute("id") ? (ushort) XmlConvertExtension.ToInt32(reader.Value) : (ushort) 0, reader.MoveToAttribute("min") ? (ushort) XmlConvertExtension.ToInt32(reader.Value) : (ushort) 0, reader.MoveToAttribute("max") ? (ushort) XmlConvertExtension.ToInt32(reader.Value) : (ushort) 0);
        VPageBreakImpl pageBreak = new VPageBreakImpl(sheet.Application, (object) sheet, pagebreak);
        if (reader.MoveToAttribute("man"))
          pageBreak.Type = ExcelPageBreak.PageBreakManual;
        vpageBreaks.Add(pageBreak);
      }
      reader.Skip();
    }
    reader.Skip();
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
    if (!reader.MoveToAttribute("id", this.m_book.IsStrict ? "http://purl.oclc.org/ooxml/officeDocument/relationships" : "http://schemas.openxmlformats.org/officeDocument/2006/relationships"))
      throw new XmlException();
    this.m_book.DataHolder.ParseExternalLink(reader.Value);
  }

  public void ParseConnections(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "connections")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    if (reader.NodeType != XmlNodeType.Element)
      throw new ArgumentException("Element is null");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      Excel2007Parser.SkipWhitespaces(reader);
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "connection":
            this.ParseConnection(reader);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
  }

  public void ParseConnection(XmlReader reader)
  {
    ExternalConnection Connection = (ExternalConnection) null;
    DataBaseProperty DataBase = (DataBaseProperty) null;
    bool flag = false;
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.NodeType == XmlNodeType.Element)
    {
      if (reader.LocalName != "connection")
        throw new ArgumentException("Invalid XML");
      if (reader.LocalName != "connection")
        throw new XmlException("Unexpected xml tag " + reader.LocalName);
      ExcelConnectionsType Type = reader.MoveToAttribute("type") ? (ExcelConnectionsType) Enum.Parse(typeof (ExcelConnectionsType), reader.Value) : throw new ArgumentException("DataBase Type is Missing");
      if (reader.MoveToAttribute("deleted"))
        flag = this.ParseBoolean(reader, "deleted", false);
      if (flag)
      {
        Connection = (this.m_book.DeletedConnections as ExternalConnectionCollection).Add(Type) as ExternalConnection;
        Connection.Deleted = flag;
      }
      else
        Connection = (this.m_book.Connections as ExternalConnectionCollection).Add(Type) as ExternalConnection;
      if (Connection.DataBaseType == ExcelConnectionsType.ConnectionTypeOLEDB)
        DataBase = (DataBaseProperty) Connection.OLEDBConnection;
      else if (Connection.DataBaseType == ExcelConnectionsType.ConnectionTypeODBC)
      {
        DataBase = (DataBaseProperty) Connection.ODBCConnection;
        DataBase.CommandType = ExcelCommandType.Sql;
      }
      if (reader.MoveToAttribute("id"))
        Connection.ConncetionId = (uint) reader.ReadContentAsInt();
      if (reader.MoveToAttribute("sourceFile"))
        Connection.SourceFile = reader.Value;
      if (reader.MoveToAttribute("odcFile"))
        Connection.ConnectionFile = reader.Value;
      if (reader.MoveToAttribute("name"))
        Connection.Name = reader.Value;
      if (reader.MoveToAttribute("description"))
        Connection.Description = reader.Value;
      if (reader.MoveToAttribute("refreshedVersion"))
        Connection.RefershedVersion = (uint) reader.ReadContentAsInt();
      if (DataBase != null)
      {
        if (reader.MoveToAttribute("savePassword"))
          DataBase.SavePassword = reader.ReadContentAsBoolean();
        if (reader.MoveToAttribute("onlyUseConnectionFile"))
          DataBase.AlwaysUseConnectionFile = reader.ReadContentAsBoolean();
        if (reader.MoveToAttribute("interval"))
          DataBase.RefreshPeriod = reader.ReadContentAsInt();
        if (reader.MoveToAttribute("credentials"))
          DataBase.ServerCredentialsMethod = (ExcelCredentialsMethod) Enum.Parse(typeof (ExcelCredentialsMethod), reader.Value);
        if (reader.MoveToAttribute("refreshOnLoad"))
          DataBase.RefreshOnFileOpen = reader.ReadContentAsBoolean();
      }
      reader.Read();
    }
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      switch (reader.LocalName)
      {
        case "dbPr":
          this.ParseDataBaseProperty(reader, DataBase);
          Connection.DBConnectionString = Connection.DataBaseType != ExcelConnectionsType.ConnectionTypeOLEDB ? (string) DataBase.ConnectionString : this.checkconnection((string) DataBase.ConnectionString);
          continue;
        case "webPr":
          this.ParseWebProperties(reader, Connection);
          reader.Skip();
          continue;
        case "olapPr":
          Connection.OlapProperty = ShapeParser.ReadNodeAsStream(reader, false);
          continue;
        case "extLst":
          Connection.ExtLstProperty = ShapeParser.ReadNodeAsStream(reader);
          continue;
        case "textPr":
          Connection.m_textPr = ShapeParser.ReadNodeAsStream(reader);
          continue;
        case "parameters":
          this.ParseParameters(reader, Connection.Parameters);
          continue;
        default:
          reader.Skip();
          continue;
      }
    }
    reader.Read();
  }

  private void ParseParameters(XmlReader reader, IParameters parameters)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != nameof (parameters))
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    if (reader.NodeType != XmlNodeType.Element)
      throw new ArgumentException("Element is null");
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
    {
      if (reader.NodeType == XmlNodeType.Element)
      {
        switch (reader.LocalName)
        {
          case "parameter":
            this.ParseParameter(reader, parameters);
            continue;
          default:
            reader.Skip();
            continue;
        }
      }
    }
    reader.Read();
  }

  private void ParseParameter(XmlReader reader, IParameters parameters)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "parameter")
      throw new XmlException("Unexpected xml tag " + reader.LocalName);
    string empty = string.Empty;
    ExcelParameterDataType dataType = ExcelParameterDataType.ParamTypeUnknown;
    if (reader.NodeType == XmlNodeType.Element)
    {
      if (reader.MoveToAttribute("name"))
        empty = reader.Value;
      if (reader.MoveToAttribute("sqlType"))
        dataType = (ExcelParameterDataType) reader.ReadContentAsInt();
      ParameterImpl parameterImpl = (ParameterImpl) parameters.Add(empty, dataType);
      if (reader.MoveToAttribute("parameterType"))
      {
        switch (reader.Value)
        {
          case "value":
            parameterImpl.Type = ExcelParameterType.Constant;
            break;
          case "cell":
            parameterImpl.Type = ExcelParameterType.Range;
            break;
        }
      }
      if (reader.MoveToAttribute("refreshOnChange"))
        parameterImpl.RefreshOnChange = reader.ReadContentAsBoolean();
      if (reader.MoveToAttribute("prompt"))
      {
        parameterImpl.Type = ExcelParameterType.Prompt;
        parameterImpl.PromptString = reader.Value;
      }
      if (reader.MoveToAttribute("boolean"))
      {
        parameterImpl.Flag = (byte) 1;
        parameterImpl.Value = (object) reader.ReadContentAsBoolean();
      }
      if (reader.MoveToAttribute("cell"))
      {
        parameterImpl.Flag = (byte) 2;
        parameterImpl.CellRange = reader.ReadContentAsString();
      }
      if (reader.MoveToAttribute("double"))
      {
        parameterImpl.Flag = (byte) 3;
        parameterImpl.Value = (object) reader.ReadContentAsDouble();
      }
      if (reader.MoveToAttribute("integer"))
      {
        parameterImpl.Flag = (byte) 4;
        parameterImpl.Value = (object) reader.ReadContentAsInt();
      }
      if (reader.MoveToAttribute("string"))
      {
        parameterImpl.Flag = (byte) 5;
        parameterImpl.Value = (object) reader.ReadContentAsString();
      }
    }
    reader.Read();
  }

  private List<Dictionary<string, string>> ParseCustomWorkbookViews(XmlReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    reader.Read();
    List<Dictionary<string, string>> customWorkbookViews = new List<Dictionary<string, string>>();
    while (reader.NodeType != XmlNodeType.EndElement)
    {
      if (reader.LocalName == "customWorkbookView")
      {
        Dictionary<string, string> workbookView = this.ParseWorkbookView(reader);
        customWorkbookViews.Add(workbookView);
      }
      reader.Skip();
    }
    reader.Skip();
    return customWorkbookViews;
  }

  private void ParseDynamicFilter(XmlReader reader, AutoFilterImpl autoFilter)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (autoFilter == null)
      throw new ArgumentNullException(nameof (autoFilter));
    autoFilter.IsAnd = false;
    autoFilter.FilterType = ExcelFilterType.DynamicFilter;
    if (!reader.MoveToAttribute("type"))
      return;
    autoFilter.AddDynamicFilter(AF.ConvertToDateFilterType(reader.Value));
  }

  public void ParseDataBaseProperty(XmlReader reader, DataBaseProperty DataBase)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "dbPr")
      throw new ArgumentException("Tag is not proper");
    if (reader.NodeType == XmlNodeType.Element)
    {
      if (reader.MoveToAttribute("connection"))
        DataBase.ConnectionString = (object) reader.Value;
      if (reader.MoveToAttribute("command"))
        DataBase.CommandText = (object) reader.Value;
      if (reader.MoveToAttribute("commandType"))
        DataBase.CommandType = (ExcelCommandType) Enum.Parse(typeof (ExcelCommandType), reader.Value);
    }
    reader.Skip();
  }

  public void ParseWebProperties(XmlReader reader, ExternalConnection Connection)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "webPr")
      throw new ArgumentException("Tag is not proper");
    if (reader.MoveToAttribute("xml"))
      Connection.IsXml = reader.ReadContentAsBoolean();
    Connection.ConnectionURL = reader.MoveToAttribute("url") ? reader.Value : throw new ArgumentException("The connection URL is missing");
  }

  private string checkconnection(string connection)
  {
    string lower1 = connection.ToLower();
    if (lower1.Contains("provider"))
    {
      string newValue = "Provider=Microsoft.JET.OLEDB.4.0";
      int startIndex1 = lower1.IndexOf("provider");
      int num = lower1.IndexOf(";", startIndex1);
      num.ToString();
      string oldValue = connection.Substring(startIndex1, num - startIndex1);
      string str = lower1.Substring(startIndex1, num - startIndex1);
      if (oldValue != null && oldValue != "" && str.Contains("ace"))
      {
        connection = connection.Replace(oldValue, newValue);
        string lower2 = connection.ToLower();
        int startIndex2 = lower2.IndexOf(";", lower2.IndexOf("data source"));
        if (startIndex2 > 0)
          connection = connection.Remove(startIndex2);
      }
    }
    return connection;
  }

  public void Dispose()
  {
    this.m_dictShapeParsers.Clear();
    this.m_dictShapeParsers = (Dictionary<int, ShapeParser>) null;
    this.m_values.Clear();
    this.m_values = (List<string>) null;
    this.m_drawingParser = (DrawingParser) null;
    if (this.m_outlineWrapperUtility != null)
      this.m_outlineWrapperUtility = (OutlineWrapperUtility) null;
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
    if (this.m_dicMinorFonts != null)
    {
      this.m_dicMinorFonts.Clear();
      this.m_dicMinorFonts = (Dictionary<string, FontImpl>) null;
    }
    if (this.m_bgImages == null)
      return;
    this.m_bgImages.Clear();
    this.m_bgImages = (List<ZipArchiveItem>) null;
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
            if (!(cells.Application as ApplicationImpl).IsExternBookParsing)
            {
              cells.SetBooleanValue(iRow, iColumn, XmlConvertExtension.ToBoolean(strValue), iXFIndex);
              return;
            }
            cells.SetNonSSTString(iRow, iColumn, iXFIndex, strValue);
            return;
          case Excel2007Serializator.CellType.e:
            if (!(cells.Application as ApplicationImpl).IsExternBookParsing)
            {
              cells.SetErrorValue(iRow, iColumn, strValue, iXFIndex);
              return;
            }
            cells.SetNonSSTString(iRow, iColumn, iXFIndex, strValue);
            return;
          case Excel2007Serializator.CellType.inlineStr:
          case Excel2007Serializator.CellType.s:
            if (!(cells.Application as ApplicationImpl).IsExternBookParsing)
            {
              cells.SetSingleStringValue(iRow, iColumn, iXFIndex, XmlConvertExtension.ToInt32(strValue));
              return;
            }
            cells.SetNonSSTString(iRow, iColumn, iXFIndex, (string) cells.Sheet.ParentWorkbook.InnerSST[XmlConvertExtension.ToInt32(strValue)]);
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
        double result;
        double.TryParse(strValue, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result);
        sheet.SetFormulaNumberValue(iRowIndex, iColumnIndex, result);
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

  internal void SetSharedFormula(
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
        record.Formula = sheet.IsParsing ? this.m_formulaUtil.ParseSharedString(strFormulaString, iRow, iCol, (IWorksheet) sheet) : this.m_book.FormulaUtil.ParseSharedString(strFormulaString, iRow, iCol, (IWorksheet) sheet);
        RecordTable table = cellRecords.Table;
        int count = table.SharedFormulas.Count;
        table.AddSharedFormula(0, (int) sheet.m_sharedFormulaGroupIndex, record);
      }
    }
    FormulaRecord record1 = (FormulaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Formula);
    SharedFormulaRecord sharedFormula = cellRecords.Table.SharedFormulas[(long) (int) sheet.m_sharedFormulaGroupIndex];
    record1.ParsedExpression = FormulaUtil.ConvertSharedFormulaTokens(sharedFormula, (IWorkbook) sheet.ParentWorkbook, iRow - 1, iCol - 1);
    record1.Row = iRow - 1;
    record1.Column = iCol - 1;
    record1.ExtendedFormatIndex = (ushort) iXFIndex;
    record1.CalculateOnOpen = bCalculateOnOpen;
    cellRecords.SetCellRecord(iRow, iCol, (ICellPositionFormat) record1);
    foreach (Ptg ptg in record1.ParsedExpression)
    {
      if (ptg.TokenCode == FormulaToken.tRef2)
      {
        RangeImpl rangeImpl = sheet[ptg.ToString()] as RangeImpl;
        if (sheet.LastColumn < rangeImpl.FirstColumn)
          sheet.LastColumn = rangeImpl.FirstColumn;
        if (sheet.LastRow < rangeImpl.FirstRow)
          sheet.LastRow = rangeImpl.FirstRow;
      }
    }
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
      rangeForDvOrAf = new TAddr(Convert.ToInt32(str.Replace('$'.ToString(), string.Empty)) - 1, RangeImpl.GetColumnIndex(empty.Replace('$'.ToString(), string.Empty)) - 1, Convert.ToInt32(strRow2.Replace('$'.ToString(), string.Empty)) - 1, RangeImpl.GetColumnIndex(strColumn2.Replace('$'.ToString(), string.Empty)) - 1);
    return rangeForDvOrAf;
  }

  private ExcelFilterCondition ConvertAutoFormatFilterCondition(string strCondition)
  {
    switch (strCondition)
    {
      case "equal":
        return ExcelFilterCondition.Equal;
      case "greaterThan":
        return ExcelFilterCondition.Greater;
      case "greaterThanOrEqual":
        return ExcelFilterCondition.GreaterOrEqual;
      case "lessThan":
        return ExcelFilterCondition.Less;
      case "lessThanOrEqual":
        return ExcelFilterCondition.LessOrEqual;
      case "notEqual":
        return ExcelFilterCondition.NotEqual;
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
      case "uniqueValues":
        return ExcelCFType.Unique;
      case "duplicateValues":
        return ExcelCFType.Duplicate;
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
      case "top10":
        return ExcelCFType.TopBottom;
      case "aboveAverage":
        return ExcelCFType.AboveBelowAverage;
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

  private void ParseExtention(XmlReader reader, ConditionalFormatImpl conditionalFormat)
  {
    reader.Read();
    while (reader.NodeType != XmlNodeType.EndElement || reader.LocalName != "extLst")
    {
      if (conditionalFormat.FormatType == ExcelCFType.DataBar && reader.LocalName == "ext" && reader.NodeType != XmlNodeType.EndElement)
      {
        reader.Read();
        DataBarImpl dataBar = conditionalFormat.DataBar as DataBarImpl;
        if (dataBar != (DataBarImpl) null)
        {
          dataBar.HasExtensionList = true;
          if (reader.LocalName == "id")
          {
            reader.Read();
            dataBar.ST_GUID = reader.Value;
          }
        }
      }
      else
        reader.Read();
    }
    reader.Read();
  }

  private void ParseExtCFRules(XmlReader reader, ConditionalFormats conditionalFormats)
  {
    string id = (string) null;
    if (reader.MoveToAttribute("id"))
      id = reader.Value;
    ConditionalFormatImpl conditionalFormatImpl = this.CheckCFId(id, conditionalFormats.sheet, conditionalFormats);
    if (conditionalFormatImpl == null)
      return;
    reader.Read();
    this.ParseExtnDataBar(reader, conditionalFormatImpl.DataBar as DataBarImpl, (IWorkbook) conditionalFormatImpl.Workbook);
  }

  private ConditionalFormatImpl CheckCFId(
    string id,
    WorksheetImpl sheet,
    ConditionalFormats ConditionalFormat)
  {
    for (int i1 = 0; i1 < sheet.ConditionalFormats.Count; ++i1)
    {
      for (int i2 = 0; i2 < sheet.ConditionalFormats[i1].Count; ++i2)
      {
        ConditionalFormatImpl conditionalFormatImpl = sheet.ConditionalFormats[i1][i2] as ConditionalFormatImpl;
        if (conditionalFormatImpl.FormatType == ExcelCFType.DataBar && (conditionalFormatImpl.DataBar as DataBarImpl).ST_GUID == id)
          return conditionalFormatImpl;
      }
    }
    return (ConditionalFormatImpl) null;
  }

  private void ParseExtnDataBar(XmlReader reader, DataBarImpl dataBar, IWorkbook book)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (dataBar == (DataBarImpl) null)
      throw new ArgumentNullException(nameof (dataBar));
    if (reader.LocalName != nameof (dataBar))
      throw new XmlException();
    if (reader.MoveToAttribute("gradient"))
      dataBar.HasGradientFill = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("minLength"))
      dataBar.PercentMin = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("maxLength"))
      dataBar.PercentMax = XmlConvertExtension.ToInt32(reader.Value);
    if (reader.MoveToAttribute("showValue"))
      dataBar.ShowValue = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("axisPosition"))
      dataBar.DataBarAxisPosition = (DataBarAxisPosition) Enum.Parse(typeof (DataBarAxisPosition), reader.Value, false);
    if (reader.MoveToAttribute("border"))
      dataBar.HasBorder = XmlConvertExtension.ToBoolean(reader.Value);
    if (reader.MoveToAttribute("direction"))
      dataBar.DataBarDirection = (DataBarDirection) Enum.Parse(typeof (DataBarDirection), reader.Value, false);
    if (reader.MoveToAttribute("negativeBarColorSameAsPositive"))
    {
      bool flag = XmlConvertExtension.ToBoolean(reader.Value);
      if (flag)
        flag = false;
      dataBar.HasDiffNegativeBarColor = flag;
    }
    if (reader.MoveToAttribute("negativeBarBorderColorSameAsPositive"))
    {
      bool flag = XmlConvertExtension.ToBoolean(reader.Value);
      if (flag)
        flag = false;
      dataBar.HasDiffNegativeBarBorderColor = flag;
    }
    reader.MoveToElement();
    if (!reader.IsEmptyElement)
    {
      reader.Read();
      int num = 0;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          switch (reader.LocalName)
          {
            case "cfvo":
              ConditionValue condition = new ConditionValue();
              this.ParseCFValueObject(reader, book, condition);
              if (num == 0)
              {
                dataBar.MinPoint = (IConditionValue) condition;
              }
              else
              {
                if (num != 1)
                  throw new XmlException();
                dataBar.MaxPoint = (IConditionValue) condition;
              }
              if (condition.Value == null && (condition.Type == ConditionValueType.Formula || condition.Type == ConditionValueType.Percent || condition.Type == ConditionValueType.Number || condition.Type == ConditionValueType.Percentile))
              {
                reader.Read();
                if (reader.LocalName == "f")
                {
                  reader.Read();
                  condition.Value = reader.Value;
                  reader.Skip();
                  reader.Read();
                  reader.Read();
                }
              }
              ++num;
              continue;
            case "borderColor":
              dataBar.BorderColor = this.ParseColor(reader).GetRGB(book);
              if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "borderColor")
              {
                reader.Read();
                continue;
              }
              continue;
            case "negativeFillColor":
              dataBar.NegativeFillColor = this.ParseColor(reader).GetRGB(book);
              if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "negativeFillColor")
              {
                reader.Read();
                continue;
              }
              continue;
            case "negativeBorderColor":
              dataBar.NegativeBorderColor = this.ParseColor(reader).GetRGB(book);
              if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "negativeBorderColor")
              {
                reader.Read();
                continue;
              }
              continue;
            case "axisColor":
              dataBar.BarAxisColor = this.ParseColor(reader).GetRGB(book);
              if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "axisColor")
              {
                reader.Read();
                continue;
              }
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
    reader.Read();
    reader.Read();
    if (!(reader.LocalName == "sqref"))
      return;
    reader.Skip();
  }

  private void ParseCustomCFIcons(XmlReader reader, IIconConditionValue iconSet)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof (reader));
    if (reader.LocalName != "cfIcon")
      throw new XmlException();
    string str = (string) null;
    string s = (string) null;
    if (reader.MoveToAttribute(nameof (iconSet)))
      str = reader.Value;
    if (reader.MoveToAttribute("iconId"))
      s = reader.Value;
    ExcelIconSetType excelIconSetType = (ExcelIconSetType) Array.IndexOf<string>(CF.IconSetTypeNames, str);
    iconSet.IconSet = excelIconSetType;
    iconSet.Index = int.Parse(s);
    reader.Read();
  }

  public static void CopyFillSettings(FillImpl fill, ExtendedFormatImpl extendedFormat)
  {
    if (fill.FillType == ExcelFillType.Gradient)
    {
      extendedFormat.Gradient = (IGradient) new ShapeFillImpl(extendedFormat.Application, (object) extendedFormat, ExcelFillType.Gradient);
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

  internal Dictionary<string, Color> ParseThemeOverideColors(ChartImpl chart)
  {
    XmlReader reader = UtilityMethods.CreateReader((Stream) chart.m_themeOverrideStream);
    Dictionary<string, Color> dicThemeColors = (Dictionary<string, Color>) null;
    this.ParseThemeColors(reader, out dicThemeColors);
    return dicThemeColors;
  }

  internal void ParseXmlMaps(XmlReader reader) => this.m_book.ParseXmlMaps(reader);

  private delegate ExcelSheetProtection ChecProtectionDelegate(
    XmlReader reader,
    string attributeName,
    ExcelSheetProtection flag,
    bool defaultValue,
    ExcelSheetProtection protection);
}
