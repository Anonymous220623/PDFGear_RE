// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlSerialization.FileDataHolder
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.XmlReaders;
using Syncfusion.OfficeChart.Interfaces;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlSerialization;

internal class FileDataHolder : IWorkbookSerializator, IDisposable
{
  private const string ContentTypesItemName = "[Content_Types].xml";
  internal const string RelationsDirectory = "_rels";
  internal const string RelationExtension = ".rels";
  private const string TopRelationsPath = "_rels/.rels";
  private const string XmlExtension = "xml";
  private const string RelsExtension = "rels";
  public const string BinaryExtension = "bin";
  private const string WorkbookPartName = "xl/workbook.xml";
  private const string CustomXmlPartName = "customXml/item{0}.xml";
  private const string SSTPartName = "/xl/sharedStrings.xml";
  private const string StylesPartName = "xl/styles.xml";
  private const string ThemesPartName = "xl/theme/theme1.xml";
  private const string DefaultWorksheetPathFormat = "xl/worksheets/sheet{0}.xml";
  private const string DefaultChartsheetPathFormat = "xl/chartsheets/sheet{0}.xml";
  public const string DefaultPicturePathFormat = "xl/media/image{0}.";
  public const string ExtendedPropertiesPartName = "docProps/app.xml";
  public const string CorePropertiesPartName = "docProps/core.xml";
  public const string CustomPropertiesPartName = "docProps/custom.xml";
  private const string RelationIdFormat = "rId{0}";
  public const string ExternLinksPathFormat = "xl/externalLinks/externalLink{0}.xml";
  private const string ExtenalLinksPathStart = "xl/externalLinks/externalLink";
  public const string CustomPropertyPathStart = "xl/customProperty";
  public const string PivotCacheDefinitionPathFormat = "xl/pivotCache/pivotCacheDefinition{0}.xml";
  public const string PivotCacheRecordsPathFormat = "xl/pivotCache/pivotCacheRecords{0}.xml";
  public const string PivotTablePathFormat = "xl/pivotTables/pivotTable{0}.xml";
  private const string TablePathFormat = "xl/tables/table{0}.xml";
  private const string ConnectionPathFormat = "xl/connections.xml";
  private const string QueryTablePathFormat = "/xl/queryTables/queryTable{0}.xml";
  private Dictionary<string, MemoryStream> m_metafileStream = new Dictionary<string, MemoryStream>();
  private ZipArchive m_archive = new ZipArchive();
  private WorkbookImpl m_book;
  private Excel2007Parser m_parser;
  private IDictionary<string, string> m_dicDefaultTypes = (IDictionary<string, string>) new Dictionary<string, string>((IEqualityComparer<string>) System.StringComparer.OrdinalIgnoreCase);
  private IDictionary<string, string> m_dicOverriddenTypes = (IDictionary<string, string>) new Dictionary<string, string>((IEqualityComparer<string>) System.StringComparer.InvariantCultureIgnoreCase);
  private RelationCollection m_topRelations;
  private string m_strWorkbookPartName = "xl/workbook.xml";
  private string m_strSSTPartName = "/xl/sharedStrings.xml";
  private string m_strStylesPartName = "xl/styles.xml";
  private string m_connectionPartName = "xl/connections.xml";
  private string m_queryTablePartName = "/xl/queryTables/queryTable{0}.xml";
  private string m_strThemesPartName = "xl/theme/theme1.xml";
  private Excel2007Serializator m_serializator;
  private List<int> m_arrCellFormats;
  private RelationCollection m_workbookRelations;
  private string m_strStylesRelationId;
  private string m_strSSTRelationId;
  private string m_strThemeRelationId;
  private string m_strWorkbookContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml";
  private Stream m_streamEnd = (Stream) new MemoryStream();
  private Stream m_streamStart = (Stream) new MemoryStream();
  private Stream m_streamDxfs;
  private int m_iCommentIndex;
  private int m_iVmlIndex;
  private int m_iDrawingIndex;
  private int m_iImageIndex;
  private int m_iImageId;
  private int m_iLastChartIndex;
  private int m_iLastPivotCacheIndex;
  private int m_iLastPivotCacheRecordsIndex;
  private int m_iExternLinkIndex;
  private string[] m_arrImageItemNames;
  private List<DxfImpl> m_lstParsedDxfs;
  private List<Dictionary<string, string>> m_lstBookViews;
  private Dictionary<string, object> m_dictItemsToRemove = new Dictionary<string, object>();
  private Stream m_functionGroups;
  private FileVersion m_fileVersion = new FileVersion();
  private string m_strCalculationId = "125725";
  private Dictionary<string, string> m_preservedCaches = new Dictionary<string, string>();
  private Stream m_extensions;
  private string m_strConnectionId;
  private int m_queryTableCount = 1;
  private int m_iLastChartExIndex;

  private FileDataHolder()
  {
  }

  public FileDataHolder(WorkbookImpl book)
  {
    this.m_book = book != null ? book : throw new ArgumentNullException(nameof (book));
    this.m_archive.CreateCompressor = new ZipArchive.CompressorCreator(book.AppImplementation.CreateCompressor);
  }

  public FileDataHolder(WorkbookImpl book, string filename, string password)
    : this(book)
  {
    if (filename == null || filename.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (filename));
    this.m_archive.Open(filename);
  }

  private void RerequestPassword(ref string password, ApplicationImpl excel)
  {
    PasswordRequiredEventArgs e = new PasswordRequiredEventArgs();
    if (excel.RaiseOnWrongPassword((object) this, e))
      password = e.NewPassword;
    else
      e = (PasswordRequiredEventArgs) null;
    if (password == null || e == null || e.StopParsing)
      throw new ArgumentException("Workbook is protected and password wasn't specified.");
  }

  private void RequestPassword(ref string password, ApplicationImpl excel)
  {
    PasswordRequiredEventArgs e = new PasswordRequiredEventArgs();
    if (password == null)
    {
      if (excel.RaiseOnPasswordRequired((object) this, e))
        password = e.NewPassword;
      else
        e = (PasswordRequiredEventArgs) null;
    }
    if (password == null || e == null || e.StopParsing)
      throw new ArgumentException("Workbook is protected and password wasn't specified.");
  }

  internal int LastChartExIndex
  {
    get => this.m_iLastChartExIndex;
    set => this.m_iLastChartExIndex = value;
  }

  public WorkbookImpl Workbook => this.m_book;

  public Excel2007Parser Parser
  {
    get
    {
      if (this.m_parser == null)
        this.m_parser = new Excel2007Parser(this.m_book);
      return this.m_parser;
    }
  }

  public ZipArchiveItem this[Relation relation, string parentPath]
  {
    get
    {
      return relation == null ? (ZipArchiveItem) null : this.m_archive[FileDataHolder.CombinePath(parentPath, relation.Target)];
    }
  }

  public Excel2007Serializator Serializator
  {
    get
    {
      if (this.m_serializator == null || this.m_serializator.Version != this.m_book.Version)
      {
        switch (this.m_book.Version)
        {
          case OfficeVersion.Excel2007:
            this.m_serializator = new Excel2007Serializator(this.m_book);
            break;
          case OfficeVersion.Excel2010:
            this.m_serializator = (Excel2007Serializator) new Excel2010Serializator(this.m_book);
            break;
          case OfficeVersion.Excel2013:
            this.m_serializator = (Excel2007Serializator) new Excel2013Serializator(this.m_book);
            break;
          default:
            throw new NotImplementedException();
        }
      }
      return this.m_serializator;
    }
  }

  public List<int> XFIndexes => this.m_arrCellFormats;

  public ZipArchive Archive => this.m_archive;

  public int LastCommentIndex
  {
    get => this.m_iCommentIndex;
    set => this.m_iCommentIndex = value;
  }

  public int LastVmlIndex
  {
    get => this.m_iVmlIndex;
    set => this.m_iVmlIndex = value;
  }

  public int LastDrawingIndex
  {
    get => this.m_iDrawingIndex;
    set => this.m_iDrawingIndex = value;
  }

  public int LastImageIndex
  {
    get => this.m_iImageIndex;
    set => this.m_iImageIndex = value;
  }

  public int LastImageId
  {
    get => this.m_iImageId;
    set => this.m_iImageId = value;
  }

  public int LastChartIndex
  {
    get => this.m_iLastChartIndex;
    set => this.m_iLastChartIndex = value;
  }

  internal int LastPivotCacheIndex
  {
    get => this.m_iLastPivotCacheIndex;
    set => this.m_iLastPivotCacheIndex = value;
  }

  internal int LastPivotCacheRecordsIndex
  {
    get => this.m_iLastPivotCacheRecordsIndex;
    set => this.m_iLastPivotCacheRecordsIndex = value;
  }

  public IDictionary<string, string> DefaultContentTypes => this.m_dicDefaultTypes;

  public IDictionary<string, string> OverriddenContentTypes => this.m_dicOverriddenTypes;

  public int ParsedDxfsCount
  {
    get => this.m_lstParsedDxfs == null ? int.MinValue : this.m_lstParsedDxfs.Count;
  }

  public Dictionary<string, object> ItemsToRemove => this.m_dictItemsToRemove;

  public string CalculationId
  {
    get => this.m_strCalculationId;
    set => this.m_strCalculationId = value;
  }

  public FileVersion FileVersion => this.m_fileVersion;

  public Dictionary<string, string> PreservedCaches => this.m_preservedCaches;

  public Stream ExtensionStream
  {
    get => this.m_extensions;
    set => this.m_extensions = value;
  }

  public void AddOverriddenContentType(string fileName, string contentType)
  {
    if (fileName == null || fileName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (fileName));
    if (fileName[0] != '/')
      fileName = '/'.ToString() + fileName;
    this.OverriddenContentTypes[fileName] = contentType;
  }

  public List<DxfImpl> ParseDxfsCollection()
  {
    if (this.m_streamDxfs == null || this.m_streamDxfs.Length == 0L)
      return this.m_lstParsedDxfs;
    List<DxfImpl> dxfsCollection;
    if (this.m_lstParsedDxfs == null)
    {
      this.m_streamDxfs.Position = 0L;
      XmlReader reader = UtilityMethods.CreateReader(this.m_streamDxfs);
      if (reader.LocalName != "dxfs")
        reader.Read();
      dxfsCollection = this.Parser.ParseDxfCollection(reader);
      this.m_streamDxfs.Flush();
      this.m_lstParsedDxfs = dxfsCollection;
    }
    else
      dxfsCollection = this.m_lstParsedDxfs;
    return dxfsCollection;
  }

  public WorksheetDataHolder GetSheetData(string sheetPath)
  {
    if (sheetPath == null || sheetPath.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (sheetPath));
    throw new NotImplementedException();
  }

  public WorksheetBaseImpl GetSheet(string sheetName)
  {
    if (sheetName == null || sheetName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (sheetName));
    return this.m_book.Objects[sheetName] as WorksheetBaseImpl;
  }

  public void ParseDocument(ref List<Color> themeColors, bool parseOnDemand)
  {
    this.m_book.IsWorkbookOpening = true;
    bool throwOnUnknownNames = this.m_book.ThrowOnUnknownNames;
    this.m_book.ThrowOnUnknownNames = false;
    this.ParseContentType();
    this.m_topRelations = this.ParseRelations("_rels/.rels");
    this.m_strWorkbookPartName = this.FindItemByContent("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml");
    this.m_dictItemsToRemove.Add("_rels/.rels", (object) null);
    if (this.m_strWorkbookPartName == null && (this.FindWorkbookPartName("application/vnd.ms-excel.sheet.macroEnabled.main+xml") || this.FindWorkbookPartName("application/vnd.ms-excel.template.macroEnabled.main+xml")))
      this.m_book.HasMacros = true;
    if (this.m_strWorkbookPartName == null)
      this.FindWorkbookPartName("application/vnd.openxmlformats-officedocument.spreadsheetml.template.main+xml");
    if (this.m_strWorkbookPartName == null)
      throw new NotSupportedException("File cannot be opened - format is not supported.");
    if (this.m_strWorkbookPartName[0] == '/')
      this.m_strWorkbookPartName = UtilityMethods.RemoveFirstCharUnsafe(this.m_strWorkbookPartName);
    this.ParseDocumentProperties();
    this.ParseWorkbook(ref themeColors, parseOnDemand);
    foreach (string key in this.m_dictItemsToRemove.Keys)
      this.m_archive.RemoveItem(key);
    this.m_dictItemsToRemove.Clear();
    this.m_book.ThrowOnUnknownNames = throwOnUnknownNames;
    this.m_book.IsWorkbookOpening = false;
  }

  private bool FindWorkbookPartName(string strContentType)
  {
    this.m_strWorkbookContentType = strContentType;
    this.m_strWorkbookPartName = this.FindItemByContent(strContentType);
    return this.m_strWorkbookPartName != null;
  }

  internal OfficeSaveType GetWorkbookPartType()
  {
    OfficeSaveType workbookPartType = OfficeSaveType.SaveAsXLS;
    if (this.FindWorkbookPartName("application/vnd.ms-excel.template.macroEnabled.main+xml") || this.FindWorkbookPartName("application/vnd.openxmlformats-officedocument.spreadsheetml.template.main+xml"))
      workbookPartType = OfficeSaveType.SaveAsTemplate;
    else if (this.FindWorkbookPartName("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml") || this.FindWorkbookPartName("application/vnd.ms-excel.sheet.macroEnabled.main+xml"))
      workbookPartType = OfficeSaveType.SaveAsXLS;
    if (this.m_strWorkbookPartName[0] == '/')
      this.m_strWorkbookPartName = UtilityMethods.RemoveFirstCharUnsafe(this.m_strWorkbookPartName);
    return workbookPartType;
  }

  public void ParseContentType()
  {
    this.m_dicDefaultTypes.Clear();
    this.m_dicOverriddenTypes.Clear();
    this.Parser.ParseContentTypes(UtilityMethods.CreateReader((this.m_archive["[Content_Types].xml"] ?? throw new NotSupportedException("File cannot be opened - format is not supported")).DataStream), this.m_dicDefaultTypes, this.m_dicOverriddenTypes);
    string key = "/xl/workbook.xml";
    string str = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml";
    if (!this.m_dicOverriddenTypes.ContainsKey(key))
      this.m_dicOverriddenTypes.Add(key, str);
    this.m_dictItemsToRemove.Add("[Content_Types].xml", (object) null);
  }

  public void ParseDocumentProperties()
  {
    this.ParseArchiveItemByContentType("application/vnd.openxmlformats-package.core-properties+xml");
    this.ParseArchiveItemByContentType("application/vnd.openxmlformats-officedocument.extended-properties+xml");
    this.ParseArchiveItemByContentType("application/vnd.openxmlformats-officedocument.custom-properties+xml");
  }

  public void ParseArchiveItemByContentType(string strContentType)
  {
    string strItemName;
    if (this.GetXmlReaderByContentType(strContentType, out strItemName) == null)
      return;
    switch (strContentType)
    {
      case "application/vnd.openxmlformats-package.core-properties+xml":
      case "application/vnd.openxmlformats-officedocument.extended-properties+xml":
      case "application/vnd.openxmlformats-officedocument.custom-properties+xml":
        this.m_archive.RemoveItem(strItemName);
        break;
      default:
        throw new ArgumentException(nameof (strContentType));
    }
  }

  public XmlReader GetXmlReaderByContentType(string strContentType, out string strItemName)
  {
    string str = this.FindItemByContent(strContentType);
    if (str == null)
    {
      strItemName = string.Empty;
      return (XmlReader) null;
    }
    this.m_dicOverriddenTypes.Remove(str);
    if (str.StartsWith("/"))
      str = UtilityMethods.RemoveFirstCharUnsafe(str);
    ZipArchiveItem zipArchiveItem = this.m_archive[str];
    strItemName = zipArchiveItem.ItemName;
    Stream dataStream = zipArchiveItem.DataStream;
    dataStream.Position = 0L;
    return UtilityMethods.CreateReader(dataStream);
  }

  public void SaveDocument(string filename, OfficeSaveType saveType)
  {
  }

  [SecurityCritical]
  public void SaveDocument(OfficeSaveType saveType)
  {
    this.m_iVmlIndex = 0;
    this.m_iCommentIndex = 0;
    this.m_iDrawingIndex = 0;
    this.m_iImageIndex = 0;
    this.m_iImageId = 0;
    this.m_iExternLinkIndex = 0;
    this.m_iLastChartIndex = 0;
    this.m_iLastPivotCacheIndex = 0;
    this.m_iLastPivotCacheRecordsIndex = 0;
    this.m_book.LastPivotTableIndex = 0;
    this.SaveWorkbook(saveType);
    this.SaveDocumentProperties();
    this.SaveContentTypes();
    this.SaveTopLevelRelations();
    this.m_iLastChartExIndex = 0;
  }

  public string RegisterContentTypes(ImageFormat imageFormat)
  {
    string strExtension;
    string str = imageFormat != null ? FileDataHolder.GetPictureContentType(imageFormat, out strExtension) : throw new ArgumentNullException(nameof (imageFormat));
    this.m_dicDefaultTypes[strExtension] = str;
    return strExtension;
  }

  public static string GetPictureContentType(ImageFormat format, out string strExtension)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    string pictureContentType;
    if (format.Equals((object) ImageFormat.Bmp))
    {
      pictureContentType = "image/bmp";
      strExtension = "bmp";
    }
    else if (format.Equals((object) ImageFormat.Jpeg))
    {
      pictureContentType = "image/jpeg";
      strExtension = "jpeg";
    }
    else if (format.Equals((object) ImageFormat.Png))
    {
      pictureContentType = "image/png";
      strExtension = "png";
    }
    else if (format.Equals((object) ImageFormat.Emf))
    {
      pictureContentType = "image/x-emf";
      strExtension = "emf";
    }
    else if (format.Equals((object) ImageFormat.Gif))
    {
      pictureContentType = "image/gif";
      strExtension = "gif";
    }
    else if (format.Equals((object) ImageFormat.Tiff))
    {
      pictureContentType = "image/tiff";
      strExtension = "tiff";
    }
    else
    {
      pictureContentType = "image/png";
      strExtension = "png";
    }
    return pictureContentType;
  }

  [SecurityCritical]
  public string SaveImage(Image image, string proposedPath)
  {
    return this.SaveImage(image, image.RawFormat, proposedPath);
  }

  [SecurityCritical]
  public string SaveImage(Image image, ImageFormat imageFormat, string proposedPath)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    string extension = this.GetExtension(imageFormat);
    this.RegisterContentTypes(imageFormat);
    string str;
    if (proposedPath == null)
    {
      string pattern;
      do
      {
        ++this.m_iImageIndex;
        pattern = $"xl/media/image{this.m_iImageIndex}.";
      }
      while (this.m_archive.Find(new Regex(pattern)) != -1);
      str = pattern + extension;
    }
    else if (this.m_archive.Find(new Regex(proposedPath.Split('.')[0])) != -1)
    {
      ++this.m_iImageIndex;
      str = $"xl/media/image{this.m_iImageIndex}." + extension;
    }
    else
    {
      ++this.m_iImageIndex;
      str = proposedPath;
    }
    imageFormat = this.GetImageFormat(extension);
    ImageFormat rawFormat = image.RawFormat;
    MemoryStream newDataStream;
    if (rawFormat.Equals((object) ImageFormat.Emf) || rawFormat.Equals((object) ImageFormat.Wmf))
    {
      MemoryStream memoryStream1 = new MemoryStream();
      newDataStream = MsoMetafilePicture.SerializeMetafile(image);
      if (this.m_metafileStream.ContainsKey(str))
      {
        MemoryStream memoryStream2 = this.m_metafileStream[str];
        if (newDataStream.Length != memoryStream2.Length)
          newDataStream = memoryStream2;
      }
    }
    else
    {
      newDataStream = new MemoryStream();
      image.Save((Stream) newDataStream, imageFormat);
    }
    this.m_archive.UpdateItem(str, (Stream) newDataStream, true, FileAttributes.Archive);
    return str;
  }

  private ImageFormat GetImageFormat(string extension)
  {
    if (extension == null)
      throw new ArgumentNullException("format");
    ImageFormat imageFormat;
    switch (extension)
    {
      case "bmp":
        imageFormat = ImageFormat.Bmp;
        break;
      case "jpeg":
        imageFormat = ImageFormat.Jpeg;
        break;
      case "tiff":
        imageFormat = ImageFormat.Tiff;
        break;
      case "exif":
        imageFormat = ImageFormat.Exif;
        break;
      case "png":
        imageFormat = ImageFormat.Png;
        break;
      case "emf":
        imageFormat = ImageFormat.Emf;
        break;
      case "icon":
        imageFormat = ImageFormat.Jpeg;
        break;
      case "wmf":
        imageFormat = ImageFormat.Wmf;
        break;
      case "gif":
        imageFormat = ImageFormat.Gif;
        break;
      default:
        imageFormat = ImageFormat.Png;
        break;
    }
    return imageFormat;
  }

  public string GetImageItemName(int i) => this.m_arrImageItemNames[i];

  public string PrepareNewItem(
    string itemNameStart,
    string extension,
    string contentType,
    RelationCollection relations,
    string relationType,
    ref int itemsCounter,
    out ZipArchiveItem item)
  {
    this.m_dicDefaultTypes[extension] = contentType;
    string itemName = this.GenerateItemName(ref itemsCounter, itemNameStart, extension);
    item = this.m_archive.AddItem(itemName, (Stream) new MemoryStream(), true, FileAttributes.Archive);
    string relationId = relations.GenerateRelationId();
    Relation relation = new Relation('/'.ToString() + itemName, relationType);
    relations[relationId] = relation;
    return relationId;
  }

  private string GetExtension(ImageFormat format)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    return !format.Equals((object) ImageFormat.Bmp) ? (!format.Equals((object) ImageFormat.Tiff) ? (!format.Equals((object) ImageFormat.Exif) ? (!format.Equals((object) ImageFormat.Wmf) ? (!format.Equals((object) ImageFormat.Icon) ? (!format.Equals((object) ImageFormat.Jpeg) ? (!format.Equals((object) ImageFormat.Png) ? (!format.Equals((object) ImageFormat.Emf) ? (!format.Equals((object) ImageFormat.Gif) ? "png" : "gif") : "emf") : "png") : "jpeg") : "icon") : "wmf") : "exif") : "tiff") : "bmp";
  }

  private void ParseWorkbook(ref List<Color> themeColors, bool parseOnDemand)
  {
    string itemName = this.m_strWorkbookPartName;
    if (itemName == null || itemName.Length == 0)
      throw new ArgumentOutOfRangeException("workbookItemName");
    if (itemName[0] == '/')
      itemName = UtilityMethods.RemoveFirstCharUnsafe(itemName);
    ZipArchiveItem zipArchiveItem1 = this.m_archive[itemName];
    if (zipArchiveItem1 == null)
      throw new XmlException("Cannot locate workbook item: " + itemName);
    string correspondingRelations = FileDataHolder.GetCorrespondingRelations(itemName);
    this.m_workbookRelations = this.ParseRelations(correspondingRelations);
    this.m_dictItemsToRemove.Add(correspondingRelations, (object) null);
    Relation relationByContentType1 = this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles", out this.m_strStylesRelationId);
    if (relationByContentType1 == null)
    {
      this.Workbook.InsertDefaultFonts();
      this.Workbook.InsertDefaultValues();
    }
    Relation relationByContentType2 = this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings", out this.m_strSSTRelationId);
    Relation relationByContentType3 = this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme", out this.m_strThemeRelationId);
    string path;
    FileDataHolder.SeparateItemName(itemName, out path);
    MemoryStream functionGroups = new MemoryStream();
    this.Parser.ParseWorkbook(FileDataHolder.CreateReader(zipArchiveItem1), this.m_workbookRelations, this, path, this.m_streamStart, this.m_streamEnd, ref this.m_lstBookViews, (Stream) functionGroups);
    int fullSize = this.m_book.TabSheets.Count + 4;
    if (relationByContentType3 != null)
    {
      this.m_strThemesPartName = path + relationByContentType3.Target;
      XmlReader reader = this.CreateReader(relationByContentType3, path);
      themeColors = this.Parser.ParseThemes(reader);
      if (themeColors != null)
        this.Workbook.m_isThemeColorsParsed = true;
    }
    int curPos1 = 1;
    ITabSheets objects = (ITabSheets) this.m_book.Objects;
    ApplicationImpl appImplementation = this.m_book.AppImplementation;
    appImplementation.RaiseProgressEvent((long) curPos1, (long) (objects.Count + 4));
    if (relationByContentType1 != null)
    {
      this.m_strStylesPartName = path + relationByContentType1.Target;
      this.m_arrCellFormats = this.Parser.ParseStyles(this.CreateReader(relationByContentType1, path), ref this.m_streamDxfs);
      this.m_dictItemsToRemove.Add(this.m_strStylesPartName, (object) null);
    }
    Relation relationByContentType4 = this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/connections", out this.m_strConnectionId);
    if (relationByContentType4 != null)
    {
      this.m_connectionPartName = path + relationByContentType4.Target;
      this.CreateReader(relationByContentType4, path);
      this.m_dictItemsToRemove.Add(this.m_connectionPartName, (object) null);
      this.m_workbookRelations.RemoveByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/connections");
    }
    int curPos2 = curPos1 + 1;
    appImplementation.RaiseProgressEvent((long) curPos2, (long) fullSize);
    Dictionary<int, int> dictUpdatedSSTIndexes = (Dictionary<int, int>) null;
    if (relationByContentType2 != null)
    {
      this.m_strSSTPartName = relationByContentType2.Target[0] != '/' ? path + relationByContentType2.Target : relationByContentType2.Target;
      ZipArchiveItem zipArchiveItem2 = this.GetItem(relationByContentType2, path, out string _);
      if (zipArchiveItem2 != null)
      {
        dictUpdatedSSTIndexes = this.Parser.ParseSST(FileDataHolder.CreateReader(zipArchiveItem2), parseOnDemand);
        this.m_dictItemsToRemove.Add(this.m_strSSTPartName, (object) null);
      }
    }
    int curPos3 = curPos2 + 1;
    appImplementation.RaiseProgressEvent((long) curPos3, (long) fullSize);
    if (functionGroups.Length != 0L)
      this.m_functionGroups = (Stream) functionGroups;
    else
      functionGroups.Close();
    this.Parser.ParseWorksheets(dictUpdatedSSTIndexes, parseOnDemand);
    if (!this.Workbook.ParseOnDemand)
      this.Parser.ParsePivotTables();
    this.RemoveCalcChain();
  }

  private void ParseCustomXmlParts()
  {
    List<string> stringList1 = new List<string>();
    List<string> stringList2 = new List<string>();
    List<string> stringList3 = new List<string>();
    foreach (KeyValuePair<string, Relation> workbookRelation in this.m_workbookRelations)
    {
      if (workbookRelation.Value.Type == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/customXml" && !stringList1.Contains(workbookRelation.Key))
      {
        stringList1.Add(workbookRelation.Key);
        stringList3.Add(workbookRelation.Value.Target);
      }
    }
    foreach (string id in stringList1)
      this.m_workbookRelations.Remove(id);
    foreach (KeyValuePair<string, string> dicOverriddenType in (IEnumerable<KeyValuePair<string, string>>) this.m_dicOverriddenTypes)
    {
      if (dicOverriddenType.Value == "application/vnd.openxmlformats-officedocument.customXmlProperties+xml" && !stringList2.Contains(dicOverriddenType.Key))
        stringList2.Add(dicOverriddenType.Key);
    }
    foreach (string key in stringList2)
      this.m_dicOverriddenTypes.Remove(key);
    for (int index = 0; index < stringList3.Count; ++index)
    {
      string str1 = stringList3[index];
      string str2 = stringList2[index];
      if (str1[0] == '.')
        str1 = str1.Substring(3, str1.Length - 3);
      if (str2.StartsWith("/"))
        UtilityMethods.RemoveFirstCharUnsafe(str2);
      this.m_dictItemsToRemove.Add($"{str1.Substring(0, str1.IndexOf('/'))}/_rels/item{index + 1}.xml.rels", (object) null);
    }
  }

  internal RelationCollection ParseRelations(string itemPath)
  {
    RelationCollection relations = (RelationCollection) null;
    ZipArchiveItem zipArchiveItem = this.m_archive[itemPath];
    if (zipArchiveItem != null)
    {
      relations = this.Parser.ParseRelations(FileDataHolder.CreateReader(zipArchiveItem));
      relations.ItemPath = itemPath;
    }
    return relations;
  }

  private string FindItemByContent(string contentType)
  {
    return this.FindItemByContentInOverride(contentType) ?? this.FindItemByContentInDefault(contentType);
  }

  private string FindItemByContentInDefault(string contentType)
  {
    string contentInDefault = (string) null;
    foreach (KeyValuePair<string, string> dicDefaultType in (IEnumerable<KeyValuePair<string, string>>) this.m_dicDefaultTypes)
    {
      if (dicDefaultType.Value == contentType)
      {
        string key1 = dicDefaultType.Key;
        int index = 0;
        for (int count = this.m_archive.Count; index < count; ++index)
        {
          string key2 = this.m_archive[index].ItemName;
          if (key2[0] != '/')
            key2 = '/'.ToString() + key2;
          if (key2.EndsWith(key1) && !this.m_dicOverriddenTypes.ContainsKey(key2))
          {
            contentInDefault = key2;
            break;
          }
        }
        break;
      }
    }
    return contentInDefault;
  }

  private string FindItemByContentInOverride(string contentType)
  {
    string contentInOverride = (string) null;
    foreach (KeyValuePair<string, string> dicOverriddenType in (IEnumerable<KeyValuePair<string, string>>) this.m_dicOverriddenTypes)
    {
      if (dicOverriddenType.Value == contentType)
      {
        contentInOverride = dicOverriddenType.Key;
        break;
      }
    }
    return contentInOverride;
  }

  internal static string GetCorrespondingRelations(string itemName)
  {
    string path;
    string str = itemName != null && itemName.Length != 0 ? FileDataHolder.SeparateItemName(itemName, out path) : throw new ArgumentOutOfRangeException(nameof (itemName));
    return $"{path}_rels{(object) '/'}{str}.rels";
  }

  internal static string SeparateItemName(string itemName, out string path)
  {
    int num = itemName.LastIndexOf('/');
    path = num >= 0 ? itemName.Substring(0, num + 1) : string.Empty;
    return itemName.Substring(num + 1);
  }

  internal Image GetImage(string strFullPath)
  {
    if (strFullPath == null || strFullPath.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (strFullPath));
    Image image = (Image) null;
    ZipArchiveItem zipArchiveItem = this.m_archive[strFullPath];
    if (zipArchiveItem != null)
    {
      MemoryStream dataStream = (MemoryStream) zipArchiveItem.DataStream;
      MemoryStream memoryStream = new MemoryStream((int) dataStream.Length);
      dataStream.WriteTo((Stream) memoryStream);
      memoryStream.Position = 0L;
      image = ApplicationImpl.CreateImage((Stream) memoryStream);
      this.m_dictItemsToRemove[strFullPath] = (object) null;
      if ((image.RawFormat.Equals((object) ImageFormat.Emf) || image.RawFormat.Equals((object) ImageFormat.Wmf)) && !this.m_metafileStream.ContainsKey(strFullPath))
        this.m_metafileStream.Add(strFullPath, memoryStream);
    }
    return image;
  }

  private static XmlReader CreateReader(ZipArchiveItem item)
  {
    Stream data = item != null ? item.DataStream : throw new ArgumentNullException(nameof (item));
    if (data.CanSeek)
      data.Position = 0L;
    return UtilityMethods.CreateReader(data);
  }

  internal XmlReader CreateReader(Relation relation, string parentItemPath)
  {
    return this.CreateReader(relation, parentItemPath, out string _);
  }

  public XmlReader CreateReaderAndFixBr(
    Relation relation,
    string parentItemPath,
    out string strItemPath)
  {
    return UtilityMethods.CreateReader((Stream) new MemoryStream(Encoding.UTF8.GetBytes(new StreamReader(this.GetItem(relation, parentItemPath, out strItemPath).DataStream).ReadToEnd().Replace("<br></br>", "<br/>").Replace("<br>", "<br/>"))));
  }

  internal XmlReader CreateReader(Relation relation, string parentItemPath, out string strItemPath)
  {
    return FileDataHolder.CreateReader(this.GetItem(relation, parentItemPath, out strItemPath));
  }

  internal ZipArchiveItem GetItem(Relation relation, string parentItemPath, out string strItemPath)
  {
    string str = relation != null ? relation.Target : throw new ArgumentNullException(nameof (relation));
    if (parentItemPath != null)
    {
      str = FileDataHolder.CombinePath(parentItemPath, str);
      str.Replace('\\', '/');
    }
    ZipArchiveItem zipArchiveItem = this.m_archive[str];
    strItemPath = str;
    return zipArchiveItem;
  }

  internal byte[] GetData(Relation relation, string parentItemPath, bool removeItem)
  {
    string str = relation != null ? relation.Target : throw new ArgumentNullException(nameof (relation));
    if (parentItemPath != null)
    {
      str = FileDataHolder.CombinePath(parentItemPath, str);
      str.Replace('\\', '/');
    }
    Stream dataStream = this.m_archive[str].DataStream;
    byte[] buffer = new byte[dataStream.Length];
    dataStream.Position = 0L;
    dataStream.Read(buffer, 0, (int) dataStream.Length);
    if (removeItem)
      this.m_archive.RemoveItem(str);
    return buffer;
  }

  internal void ParseExternalLink(string relationId)
  {
    Relation workbookRelation = this.m_workbookRelations[relationId];
    string path;
    FileDataHolder.SeparateItemName(this.m_strWorkbookPartName, out path);
    string strItemPath;
    if (this.Parser.ParseExternalLink(this.CreateReader(workbookRelation, path, out strItemPath), this.ParseRelations(FileDataHolder.GetCorrespondingRelations(strItemPath))))
    {
      this.m_dictItemsToRemove.Add(strItemPath, (object) null);
      this.m_workbookRelations.Remove(relationId);
    }
    else
      this.m_book.PreservedExternalLinks.Add(relationId);
  }

  internal static string CombinePath(string startPath, string endPath)
  {
    if (startPath == null)
      throw new ArgumentOutOfRangeException(nameof (startPath));
    if (endPath == null || endPath.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (endPath));
    if (startPath.Length > 0 && startPath[startPath.Length - 1] == '/')
      startPath = startPath.Substring(0, startPath.Length - 1);
    while (endPath.StartsWith("../"))
    {
      int length = startPath.LastIndexOf('/');
      if (length >= 0)
      {
        endPath = endPath.Substring(3, endPath.Length - 3);
        startPath = startPath.Substring(0, length);
      }
      else
        break;
    }
    if (endPath.StartsWith("/"))
      return UtilityMethods.RemoveFirstCharUnsafe(endPath);
    return !(startPath != "") ? endPath : startPath + (object) '/' + endPath;
  }

  private void SaveContentTypes()
  {
    this.FillDefaultContentTypes();
    this.SaveArchiveItem("[Content_Types].xml");
  }

  private void SaveDocumentProperties()
  {
    this.SaveArchiveItemRelationContentType("docProps/app.xml", "application/vnd.openxmlformats-officedocument.extended-properties+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
    this.SaveArchiveItemRelationContentType("docProps/core.xml", "application/vnd.openxmlformats-package.core-properties+xml", "http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties");
    this.SaveArchiveItemRelationContentType("docProps/custom.xml", "application/vnd.openxmlformats-officedocument.custom-properties+xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties");
  }

  private void SaveContentTypeProperties()
  {
  }

  private void SaveArchiveItemRelationContentType(
    string partName,
    string contentType,
    string relationType)
  {
    this.m_dicOverriddenTypes["/" + partName] = contentType;
    string relationId;
    this.m_topRelations.FindRelationByContentType(relationType, out relationId);
    if (relationId == null)
      relationId = this.m_topRelations.GenerateRelationId();
    this.m_topRelations[relationId] = new Relation(partName, relationType);
    this.SaveArchiveItem(partName);
  }

  private void SaveArchiveItem(string strItemPartName)
  {
    MemoryStream memoryStream = new MemoryStream();
    UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) memoryStream)).Flush();
    ZipArchiveItem zipArchiveItem = this.m_archive[strItemPartName];
    if (zipArchiveItem != null)
      zipArchiveItem.Update((Stream) memoryStream, true);
    else
      this.m_archive.AddItem(strItemPartName, (Stream) memoryStream, true, FileAttributes.Archive);
  }

  private void FillDefaultContentTypes()
  {
    this.m_dicDefaultTypes["xml"] = "application/xml";
    this.m_dicDefaultTypes["rels"] = "application/vnd.openxmlformats-package.relationships+xml";
  }

  private void SaveTopLevelRelations()
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) memoryStream));
    this.Serializator.SerializeRelations(writer, this.m_topRelations, (WorksheetDataHolder) null);
    writer.Flush();
    ZipArchiveItem zipArchiveItem = this.m_archive["_rels/.rels"];
    if (zipArchiveItem != null)
      zipArchiveItem.Update((Stream) memoryStream, true);
    else
      this.m_archive.AddItem("_rels/.rels", (Stream) memoryStream, true, FileAttributes.Archive);
  }

  private static string GetRelationId(int relationIndex) => $"rId{relationIndex}";

  [SecurityCritical]
  private void SaveWorkbook(OfficeSaveType saveAsType)
  {
    this.m_strWorkbookContentType = this.SelectWorkbookContentType(saveAsType);
    if (this.m_topRelations == null)
    {
      this.m_topRelations = new RelationCollection();
      this.m_topRelations[FileDataHolder.GetRelationId(1)] = new Relation(this.m_strWorkbookPartName, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");
    }
    this.SaveSST();
    this.m_arrImageItemNames = this.SaveWorkbookImages();
    this.m_arrImageItemNames = (string[]) null;
  }

  private string SelectWorkbookContentType(OfficeSaveType saveType)
  {
    return !this.m_book.HasMacros ? (saveType == OfficeSaveType.SaveAsTemplate ? "application/vnd.openxmlformats-officedocument.spreadsheetml.template.main+xml" : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml") : (saveType == OfficeSaveType.SaveAsTemplate ? "application/vnd.ms-excel.template.macroEnabled.main+xml" : "application/vnd.ms-excel.sheet.macroEnabled.main+xml");
  }

  [SecurityCritical]
  private string[] SaveWorkbookImages()
  {
    List<MsofbtBSE> pictures = this.m_book.ShapesData.Pictures;
    int count1 = pictures != null ? pictures.Count : 0;
    string[] strArray = new string[count1];
    List<int> intList = new List<int>();
    int index1 = 0;
    for (int index2 = count1; index1 < index2; ++index1)
    {
      MsofbtBSE bse = pictures[index1];
      if (bse.PicturePath != null)
        strArray[index1] = this.SerializeBSE(bse);
      else
        intList.Add(index1);
    }
    int index3 = 0;
    for (int count2 = intList.Count; index3 < count2; ++index3)
    {
      int index4 = intList[index3];
      MsofbtBSE bse = pictures[index4];
      strArray[index4] = this.SerializeBSE(bse);
    }
    return strArray;
  }

  [SecurityCritical]
  private string SerializeBSE(MsofbtBSE bse)
  {
    if (bse == null)
      throw new ArgumentNullException(nameof (bse));
    if (bse.BlipType != MsoBlipType.msoblipERROR && bse.PictureRecord.PictureStream != null)
      bse.PictureRecord.Picture = Image.FromStream(bse.PictureRecord.PictureStream);
    return bse.BlipType == MsoBlipType.msoblipERROR ? (string) null : this.SaveImage(bse.PictureRecord.Picture, bse.PicturePath);
  }

  internal Dictionary<int, int> SaveStyles(ZipArchive archive, MemoryStream stream)
  {
    this.m_dicOverriddenTypes[new AddSlashPreprocessor().PreprocessName(this.m_strStylesPartName)] = "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml";
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) stream));
    Dictionary<int, int> dictionary = this.Serializator.SerializeStyles(writer, ref this.m_streamDxfs);
    writer.Flush();
    string itemName = this.m_strStylesPartName;
    if (this.m_strStylesPartName[0] == '/')
      itemName = UtilityMethods.RemoveFirstCharUnsafe(this.m_strStylesPartName);
    int num = 0;
    foreach (KeyValuePair<string, string> dicOverriddenType in (IEnumerable<KeyValuePair<string, string>>) this.m_dicOverriddenTypes)
    {
      if (dicOverriddenType.Value == "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml".ToString())
        ++num;
    }
    if (dictionary.Count > 0 && num == 1)
      archive.UpdateItem(itemName, (Stream) stream, true, FileAttributes.Archive);
    return dictionary;
  }

  private void SaveSST()
  {
    if (this.m_book.InnerSST.ActiveCount <= 0)
      return;
    this.m_dicOverriddenTypes[new AddSlashPreprocessor().PreprocessName(this.m_strSSTPartName)] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml";
    ZippedContentStream stream = new ZippedContentStream(new ZipArchive.CompressorCreator(this.m_book.AppImplementation.CreateCompressor));
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) stream));
    this.Serializator.SerializeSST(writer);
    writer.Flush();
    string itemName = this.m_strSSTPartName;
    if (this.m_strSSTPartName[0] == '/')
      itemName = UtilityMethods.RemoveFirstCharUnsafe(this.m_strSSTPartName);
    (this.m_archive[itemName] ?? this.m_archive.AddItem(itemName, (Stream) null, false, FileAttributes.Archive)).Update(stream);
  }

  private string AddRelation(
    RelationCollection relations,
    string target,
    string parentPath,
    string type,
    string relationId)
  {
    if (target[0] == '/')
      target = UtilityMethods.RemoveFirstCharUnsafe(target);
    if (target.StartsWith(parentPath))
      target = target.Substring(parentPath.Length);
    Relation relation = new Relation(target, type);
    if (relationId != null)
      relations[relationId] = relation;
    else
      relationId = relations.Add(relation);
    return relationId;
  }

  public void SaveRelations(string parentPartName, RelationCollection relations)
  {
    if (relations == null || relations.Count == 0)
      return;
    string itemName = parentPartName != null && parentPartName.Length != 0 ? FileDataHolder.GetCorrespondingRelations(parentPartName) : throw new ArgumentOutOfRangeException(nameof (parentPartName));
    MemoryStream newDataStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) newDataStream));
    this.Serializator.SerializeRelations(writer, relations, (WorksheetDataHolder) null);
    writer.Flush();
    this.m_archive.UpdateItem(itemName, (Stream) newDataStream, true, FileAttributes.Archive);
  }

  private void ReserveSheetRelations(WorkbookObjectsCollection sheets, RelationCollection relations)
  {
    int index = 0;
    for (int count = sheets.Count; index < count; ++index)
    {
      WorksheetDataHolder dataHolder = ((WorksheetBaseImpl) sheets[index]).DataHolder;
      if (dataHolder != null)
      {
        string relationId = dataHolder.RelationId;
        if (relationId != null)
          relations[relationId] = (Relation) null;
      }
    }
  }

  private void UpdateArchiveItem(WorksheetImpl sheet, string itemName)
  {
    if (sheet.m_dataHolder == null)
    {
      this.m_archive.RemoveItem(itemName);
      ZipArchiveItem zipArchiveItem = this.m_archive.AddItem(itemName, (Stream) null, false, FileAttributes.Archive);
      sheet.m_dataHolder = new WorksheetDataHolder(this, zipArchiveItem);
    }
    else
    {
      ZipArchiveItem archiveItem = sheet.m_dataHolder.ArchiveItem;
      if (archiveItem != null && !(archiveItem.ItemName != itemName))
        return;
      if (this.m_archive.Find(itemName) >= 0)
        this.m_archive.UpdateItem(itemName, (Stream) null, false, FileAttributes.Archive);
      else
        this.m_archive.AddItem(itemName, (Stream) null, false, FileAttributes.Archive);
      sheet.m_dataHolder.ArchiveItem = this.m_archive[itemName];
    }
  }

  [SecurityCritical]
  private void SaveChartsheet(ChartImpl chart, string itemName)
  {
    if (chart == null)
      throw new ArgumentNullException(nameof (chart));
    string key = itemName != null && itemName.Length != 0 ? itemName : throw new ArgumentOutOfRangeException(nameof (itemName));
    if (itemName[0] != '/')
      key = '/'.ToString() + itemName;
    this.m_dicOverriddenTypes[key] = "application/vnd.openxmlformats-officedocument.spreadsheetml.chartsheet+xml";
    if (chart.IsSaved && chart.m_dataHolder != null)
    {
      this.SerializeExistingData((WorksheetBaseImpl) chart, itemName);
    }
    else
    {
      if (chart.m_dataHolder == null)
      {
        this.m_archive.RemoveItem(itemName);
        ZipArchiveItem zipArchiveItem = this.m_archive.AddItem(itemName, (Stream) null, false, FileAttributes.Archive);
        chart.m_dataHolder = new WorksheetDataHolder(this, zipArchiveItem);
      }
      chart.m_dataHolder.SerializeChartsheet(chart);
    }
  }

  private void SerializeExistingData(WorksheetBaseImpl sheet, string itemName)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (itemName == null || itemName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (itemName));
    ZipArchiveItem archiveItem = (sheet.m_dataHolder ?? throw new ApplicationException("Cannot serialize sheet " + sheet.Name)).ArchiveItem;
    if (!(archiveItem.ItemName != itemName))
      return;
    this.m_archive.RemoveItem(archiveItem.ItemName);
    archiveItem.ItemName = itemName;
    this.m_archive.AddItem(archiveItem);
  }

  private void RemoveCalcChain()
  {
    string relationId;
    Relation relationByContentType = this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/calcChain", out relationId);
    if (relationByContentType == null)
      return;
    string path;
    FileDataHolder.SeparateItemName(this.m_strWorkbookPartName, out path);
    string str = path + relationByContentType.Target;
    this.m_archive.RemoveItem(str);
    this.m_workbookRelations.Remove(relationId);
    this.m_dicOverriddenTypes.Remove(str);
  }

  internal void RemoveRelation(string strItemName, string relationID)
  {
    this.ItemsToRemove.Add(strItemName, (object) null);
    this.m_workbookRelations.Remove(relationID);
  }

  public string SerializeExternalLink(ExternWorkbookImpl externBook)
  {
    MemoryStream newDataStream = new MemoryStream();
    StreamWriter data = new StreamWriter((Stream) newDataStream);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
    RelationCollection relations = this.Serializator.SerializeLinkItem(writer, externBook);
    writer.Flush();
    data.Flush();
    string externalLinkName = this.GenerateExternalLinkName();
    this.m_archive.UpdateItem(externalLinkName, (Stream) newDataStream, true, FileAttributes.Archive);
    this.SaveRelations(externalLinkName, relations);
    this.m_dicOverriddenTypes['/'.ToString() + externalLinkName] = "application/vnd.openxmlformats-officedocument.spreadsheetml.externalLink+xml";
    return externalLinkName;
  }

  private string GenerateExternalLinkName()
  {
    return this.GenerateItemName(ref this.m_iExternLinkIndex, "xl/externalLinks/externalLink", "xml");
  }

  private string GenerateItemName(ref int itemsCount, string pathStart, string extension)
  {
    return string.Format($"{pathStart}{{0}}.{extension}", (object) ++itemsCount);
  }

  private string GenerateItemName(ref int itemsCount, string pathFormat)
  {
    string itemName;
    do
    {
      ++itemsCount;
      itemName = string.Format(pathFormat, (object) itemsCount);
    }
    while (this.m_archive.Find(itemName) >= 0);
    return itemName;
  }

  private string GenerateQueryItemName(ref int itemsCount, string pathFormat)
  {
    string itemName;
    do
    {
      ++itemsCount;
      itemName = string.Format(pathFormat, (object) itemsCount);
    }
    while (this.m_archive.Find(itemName) >= 0);
    return itemName;
  }

  internal string GeneratePivotTableName(int lastIndex)
  {
    return $"xl/pivotTables/pivotTable{++lastIndex}.xml";
  }

  internal void CreateDataHolder(WorksheetBaseImpl tabSheet, string fileName)
  {
    if (tabSheet == null)
      throw new ArgumentNullException(nameof (tabSheet));
    if (fileName == null || fileName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (fileName));
    if (fileName[0] == '/')
      fileName = UtilityMethods.RemoveFirstCharUnsafe(fileName);
    int index = this.m_archive.Find(fileName);
    ZipArchiveItem zipArchiveItem = index >= 0 ? this.m_archive[index] : this.m_archive.AddItem(fileName, (Stream) null, false, FileAttributes.Archive);
    tabSheet.DataHolder = new WorksheetDataHolder(this, zipArchiveItem);
  }

  private string GenerateQueryTableFileName()
  {
    int itemsCount = 0;
    return this.GenerateQueryItemName(ref itemsCount, "/xl/queryTables/queryTable{0}.xml");
  }

  public string GetContentType(string strTarget)
  {
    string dicDefaultType;
    if (!this.m_dicOverriddenTypes.TryGetValue(strTarget, out dicDefaultType))
    {
      strTarget = UtilityMethods.RemoveFirstCharUnsafe(Path.GetExtension(strTarget));
      dicDefaultType = this.m_dicDefaultTypes[strTarget];
    }
    return dicDefaultType;
  }

  public void SerializeTableRelation(string ItemName, string queryTable)
  {
    string str = ItemName;
    int startIndex = str.LastIndexOf('/');
    string itemName = str.Insert(startIndex, '/'.ToString() + "_rels") + ".rels";
    MemoryStream newDataStream = new MemoryStream();
    StreamWriter data = new StreamWriter((Stream) newDataStream);
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) data);
    new Excel2007Serializator(this.m_book).SerializeRelations(writer, new RelationCollection()
    {
      new Relation(queryTable, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/queryTable")
    }, (WorksheetDataHolder) null);
    writer.Flush();
    data.Flush();
    this.m_archive.UpdateItem(itemName, (Stream) newDataStream, true, FileAttributes.Archive);
  }

  public void Serialize(string fullName, WorkbookImpl book, OfficeSaveType saveType)
  {
    if (book != this.m_book)
      throw new ArgumentOutOfRangeException(nameof (book));
    this.SaveDocument(fullName, saveType);
  }

  public void Serialize(Stream stream, WorkbookImpl book, OfficeSaveType saveType)
  {
    if (book != this.m_book)
      throw new ArgumentOutOfRangeException(nameof (book));
  }

  internal FileDataHolder Clone(WorkbookImpl newParent)
  {
    FileDataHolder fileDataHolder = (FileDataHolder) this.MemberwiseClone();
    fileDataHolder.m_book = newParent;
    fileDataHolder.m_parser = (Excel2007Parser) null;
    fileDataHolder.m_serializator = (Excel2007Serializator) null;
    if (this.m_workbookRelations != null)
      fileDataHolder.m_workbookRelations = this.m_workbookRelations.Clone();
    if (this.m_topRelations != null)
      fileDataHolder.m_topRelations = this.m_topRelations.Clone();
    fileDataHolder.m_arrImageItemNames = CloneUtils.CloneStringArray(this.m_arrImageItemNames);
    fileDataHolder.m_streamEnd = CloneUtils.CloneStream(this.m_streamEnd);
    fileDataHolder.m_streamStart = CloneUtils.CloneStream(this.m_streamStart);
    fileDataHolder.m_streamDxfs = CloneUtils.CloneStream(this.m_streamDxfs);
    fileDataHolder.m_functionGroups = CloneUtils.CloneStream(this.m_functionGroups);
    if (this.m_dictItemsToRemove != null)
      fileDataHolder.m_dictItemsToRemove = new Dictionary<string, object>((IDictionary<string, object>) this.m_dictItemsToRemove);
    if (this.m_dicDefaultTypes != null)
      fileDataHolder.m_dicDefaultTypes = (IDictionary<string, string>) new Dictionary<string, string>(this.m_dicDefaultTypes);
    if (this.m_dicOverriddenTypes != null)
      fileDataHolder.m_dicOverriddenTypes = (IDictionary<string, string>) new Dictionary<string, string>(this.m_dicOverriddenTypes);
    if (this.m_arrCellFormats != null)
      fileDataHolder.m_arrCellFormats = new List<int>((IEnumerable<int>) this.m_arrCellFormats);
    fileDataHolder.m_lstParsedDxfs = this.CloneDxfs();
    fileDataHolder.m_lstBookViews = this.CloneViews();
    fileDataHolder.m_archive = this.m_archive.Clone();
    return fileDataHolder;
  }

  private List<Dictionary<string, string>> CloneViews()
  {
    List<Dictionary<string, string>> dictionaryList;
    if (this.m_lstBookViews != null)
    {
      int count = this.m_lstBookViews.Count;
      dictionaryList = new List<Dictionary<string, string>>(count);
      for (int index = 0; index < count; ++index)
      {
        Dictionary<string, string> lstBookView = this.m_lstBookViews[index];
        dictionaryList.Add(new Dictionary<string, string>((IDictionary<string, string>) lstBookView));
      }
    }
    else
      dictionaryList = (List<Dictionary<string, string>>) null;
    return dictionaryList;
  }

  private List<DxfImpl> CloneDxfs()
  {
    List<DxfImpl> dxfImplList;
    if (this.m_lstParsedDxfs != null)
    {
      int count = this.m_lstParsedDxfs.Count;
      dxfImplList = new List<DxfImpl>(count);
      for (int index = 0; index < count; ++index)
      {
        DxfImpl lstParsedDxf = this.m_lstParsedDxfs[index];
        dxfImplList.Add(lstParsedDxf.Clone(this.m_book));
      }
    }
    else
      dxfImplList = (List<DxfImpl>) null;
    return dxfImplList;
  }

  internal void RegisterCache(string cacheId, string relationId)
  {
    this.m_preservedCaches.Add(cacheId, relationId);
  }

  public void Dispose()
  {
    if (this.m_serializator != null)
    {
      this.m_serializator.Dispose();
      this.m_serializator = (Excel2007Serializator) null;
    }
    if (this.m_archive != null)
    {
      this.m_archive.Dispose();
      this.m_archive = (ZipArchive) null;
    }
    if (this.m_parser != null)
    {
      this.m_parser.Dispose();
      this.m_parser = (Excel2007Parser) null;
    }
    if (this.m_workbookRelations != null)
    {
      this.m_workbookRelations.Dispose();
      this.m_workbookRelations = (RelationCollection) null;
    }
    this.m_functionGroups = (Stream) null;
    this.m_extensions = (Stream) null;
    this.m_streamDxfs = (Stream) null;
    this.m_streamStart = (Stream) null;
    this.m_streamEnd = (Stream) null;
    GC.SuppressFinalize((object) this);
  }

  internal void ParseDocument(Stream excelStream)
  {
    List<Color> themeColors = new List<Color>();
    this.m_archive.Open(excelStream, false);
    this.ParseDocument(ref themeColors, true);
  }
}
