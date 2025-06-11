// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlSerialization.FileDataHolder
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.Compression.Zip;
using Syncfusion.XlsIO.Implementation.Charts;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.PivotTables;
using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Implementation.XmlReaders;
using Syncfusion.XlsIO.Implementation.XmlReaders.PivotTables;
using Syncfusion.XlsIO.Implementation.XmlSerialization.PivotTables;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlSerialization;

public class FileDataHolder : IWorkbookSerializator, IDisposable
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
  private const string DefaultDialogsheetPathFormat = "xl/dialogsheets/sheet{0}.xml";
  private const string DefaultMacrosheetPathFormat = "xl/macrosheets/sheet{0}.xml";
  private const string DefaultIntlMacrosheetPathFormat = "xl/macrosheets/intlsheet{0}.xml";
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
  private const string XmlMapsPathName = "xl/xmlMaps.xml";
  private const string XmlMapsPartName = "xmlMaps.xml";
  private const string VbaProjectPath = "/xl/vbaProject.bin";
  private List<string> m_chartAndTableItemsToRemove;
  private List<string> m_sheetItems = new List<string>();
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
  private int m_iLastChartExIndex;
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
  private List<Dictionary<string, string>> m_lstCustomBookViews;
  private List<string> m_drawingsItemPath = new List<string>();

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

  public FileDataHolder(WorkbookImpl book, Stream stream, string password)
    : this(book)
  {
    long num = stream != null ? stream.Position : throw new ArgumentNullException(nameof (stream));
    bool flag1 = false;
    if (Syncfusion.CompoundFile.XlsIO.Net.CompoundFile.CheckHeader(stream))
    {
      flag1 = true;
      using (ICompoundFile compoundFile = book.AppImplementation.CreateCompoundFile(stream))
      {
        ICompoundStorage rootStorage = compoundFile.RootStorage;
        if (flag1 = Excel2007Decryptor.CheckEncrypted(rootStorage))
        {
          Excel2007Decryptor excel2007Decryptor = this.CheckVersion(rootStorage) == ExcelVersion.Excel2007 ? new Excel2007Decryptor() : (Excel2007Decryptor) new Excel2010Decryptor();
          excel2007Decryptor.Initialize(compoundFile.RootStorage);
          ApplicationImpl appImplementation = book.AppImplementation;
          bool flag2 = false;
          if (password == null)
            flag2 = excel2007Decryptor.CheckPassword("VelvetSweatshop");
          if (!flag2)
          {
            this.RequestPassword(ref password, appImplementation);
            while (!excel2007Decryptor.CheckPassword(password))
              this.RerequestPassword(ref password, appImplementation);
          }
          stream = excel2007Decryptor.Decrypt();
          this.m_book.m_encryptionType = ExcelEncryptionType.Standard;
        }
      }
      if (!flag1)
        throw new ApplicationException("Wrong excel version");
    }
    if (flag1)
      this.m_book.PasswordToOpen = password;
    this.m_archive.Open(stream, false);
  }

  private ExcelVersion CheckVersion(ICompoundStorage storage)
  {
    Stream stream = (Stream) storage.OpenStream("EncryptionInfo");
    byte[] buffer = new byte[4];
    int num = SecurityHelper.ReadInt32(stream, buffer);
    stream.Close();
    return num == 262148 /*0x040004*/ ? ExcelVersion.Excel2010 : ExcelVersion.Excel2007;
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
          case ExcelVersion.Excel2007:
            this.m_serializator = new Excel2007Serializator(this.m_book);
            break;
          case ExcelVersion.Excel2010:
            this.m_serializator = (Excel2007Serializator) new Excel2010Serializator(this.m_book);
            break;
          case ExcelVersion.Excel2013:
            this.m_serializator = (Excel2007Serializator) new Excel2013Serializator(this.m_book);
            break;
          case ExcelVersion.Excel2016:
            this.m_serializator = (Excel2007Serializator) new Excel2016Serializator(this.m_book);
            break;
          case ExcelVersion.Xlsx:
            this.m_serializator = (Excel2007Serializator) new XlsxSerializator(this.m_book);
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

  internal int LastChartExIndex
  {
    get => this.m_iLastChartExIndex;
    set => this.m_iLastChartExIndex = value;
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

  internal List<string> ChartAndTableItemsToRemove
  {
    get
    {
      if (this.m_chartAndTableItemsToRemove == null)
        this.m_chartAndTableItemsToRemove = new List<string>();
      return this.m_chartAndTableItemsToRemove;
    }
  }

  internal List<string> DrawingsItemPath
  {
    get => this.m_drawingsItemPath;
    set => this.m_drawingsItemPath = value;
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
    this.m_book.Loading = true;
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
    this.ParseMetaProperties();
    this.ParseCustomXmlParts();
    if (!parseOnDemand)
    {
      foreach (ZipArchiveItem zipArchiveItem in this.m_archive.Items)
      {
        if (zipArchiveItem.ItemName.Contains("xl/tables/table"))
          this.m_archive.RemoveItem(zipArchiveItem.ItemName);
        else if (zipArchiveItem.ItemName.Contains("xl/drawings/drawing") && !this.DrawingsItemPath.Contains(zipArchiveItem.ItemName))
          this.m_archive.RemoveItem(zipArchiveItem.ItemName);
        else if (zipArchiveItem.ItemName.Contains("xl/drawings/_rels/drawing") && !this.DrawingsItemPath.Contains(zipArchiveItem.ItemName))
          this.m_archive.RemoveItem(zipArchiveItem.ItemName);
        else if (zipArchiveItem.ItemName.Contains("xl/charts/chart") && !this.DrawingsItemPath.Contains(zipArchiveItem.ItemName))
          this.m_archive.RemoveItem(zipArchiveItem.ItemName);
        else if (zipArchiveItem.ItemName.Contains("xl/charts/_rels/chart") && !this.DrawingsItemPath.Contains(zipArchiveItem.ItemName))
          this.m_archive.RemoveItem(zipArchiveItem.ItemName);
      }
    }
    foreach (string key in this.m_dictItemsToRemove.Keys)
      this.m_archive.RemoveItem(key);
    this.m_dictItemsToRemove.Clear();
    this.m_book.ThrowOnUnknownNames = throwOnUnknownNames;
    this.m_book.Loading = false;
  }

  private bool FindWorkbookPartName(string strContentType)
  {
    this.m_strWorkbookContentType = strContentType;
    this.m_strWorkbookPartName = this.FindItemByContent(strContentType);
    return this.m_strWorkbookPartName != null;
  }

  internal ExcelSaveType GetWorkbookPartType()
  {
    ExcelSaveType workbookPartType = ExcelSaveType.SaveAsXLS;
    if (this.FindWorkbookPartName("application/vnd.ms-excel.template.macroEnabled.main+xml") || this.FindWorkbookPartName("application/vnd.openxmlformats-officedocument.spreadsheetml.template.main+xml"))
      workbookPartType = ExcelSaveType.SaveAsTemplate;
    else if (this.FindWorkbookPartName("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml") || this.FindWorkbookPartName("application/vnd.ms-excel.sheet.macroEnabled.main+xml"))
      workbookPartType = ExcelSaveType.SaveAsXLS;
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
    XmlReader readerByContentType = this.GetXmlReaderByContentType(strContentType, out strItemName);
    if (readerByContentType == null)
      return;
    switch (strContentType)
    {
      case "application/vnd.openxmlformats-package.core-properties+xml":
        string relationId1 = (string) null;
        this.m_topRelations.FindRelationByContentType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties", out relationId1);
        if (relationId1 != null)
          this.Parser.ParseDocumentCoreProperties(readerByContentType);
        this.m_topRelations.RemoveByContentType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties");
        break;
      case "application/vnd.openxmlformats-officedocument.extended-properties+xml":
        string relationId2 = (string) null;
        this.m_topRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties", out relationId2);
        if (relationId2 != null)
          this.Parser.ParseExtendedProperties(readerByContentType);
        this.m_topRelations.RemoveByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
        break;
      case "application/vnd.openxmlformats-officedocument.custom-properties+xml":
        string relationId3 = (string) null;
        this.m_topRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties", out relationId3);
        if (relationId3 != null)
          this.Parser.ParseCustomProperties(readerByContentType);
        this.m_topRelations.RemoveByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties");
        break;
      default:
        throw new ArgumentException(nameof (strContentType));
    }
    this.m_archive.RemoveItem(strItemName);
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
    if (zipArchiveItem != null)
    {
      strItemName = zipArchiveItem.ItemName;
      Stream dataStream = zipArchiveItem.DataStream;
      dataStream.Position = 0L;
      return UtilityMethods.CreateReader(dataStream);
    }
    strItemName = string.Empty;
    return (XmlReader) null;
  }

  public void SaveDocument(string filename, ExcelSaveType saveType)
  {
    using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
      this.SaveDocument((Stream) fileStream, saveType);
  }

  public void SaveDocument(Stream stream, ExcelSaveType saveType)
  {
    this.SaveDocument(saveType);
    if (this.m_book.PasswordToOpen != null && this.m_book.m_encryptionType != ExcelEncryptionType.None)
    {
      MemoryStream data = new MemoryStream();
      this.m_archive.Save((Stream) data, false);
      using (ICompoundFile compoundFile = this.m_book.AppImplementation.CreateCompoundFile())
      {
        Excel2007Encryptor excel2007Encryptor = this.m_book.Version == ExcelVersion.Excel2007 ? new Excel2007Encryptor() : (Excel2007Encryptor) new Excel2010Encryptor();
        data.Position = 0L;
        string password = this.m_book.PasswordToOpen ?? "VelvetSweatshop";
        excel2007Encryptor.Encrypt((Stream) data, password, compoundFile.RootStorage);
        compoundFile.Save(stream);
      }
    }
    else
      this.m_archive.Save(stream, false);
    foreach (string itemName in this.ChartAndTableItemsToRemove)
      this.m_archive.RemoveItem(itemName);
    this.ChartAndTableItemsToRemove.Clear();
  }

  public void SaveDocument(ExcelSaveType saveType)
  {
    this.m_iVmlIndex = 0;
    this.m_iCommentIndex = 0;
    this.m_iDrawingIndex = 0;
    this.m_iImageIndex = 0;
    this.m_iImageId = 0;
    this.m_iExternLinkIndex = 0;
    this.m_iLastChartIndex = 0;
    this.m_iLastChartExIndex = 0;
    this.m_iLastPivotCacheIndex = 0;
    this.m_iLastPivotCacheRecordsIndex = 0;
    this.m_book.LastPivotTableIndex = 0;
    this.SaveWorkbook(saveType);
    this.SaveDocumentProperties();
    this.SaveContentTypes();
    this.SaveTopLevelRelations();
    this.SaveContentTypeProperties();
    this.SaveXmlMaps();
  }

  public string RegisterContentTypes(ImageFormat imageFormat)
  {
    string strExtension;
    string str = imageFormat != null ? FileDataHolder.GetPictureContentType(imageFormat, out strExtension) : throw new ArgumentNullException(nameof (imageFormat));
    this.m_dicDefaultTypes[strExtension] = str;
    return strExtension;
  }

  internal string RegisterSvgContentType()
  {
    string str = "image/svg+xml";
    string key = "svg";
    this.m_dicDefaultTypes[key] = str;
    return key;
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

  public string SaveImage(Image image, string proposedPath)
  {
    return this.SaveImage(image, image.RawFormat, proposedPath);
  }

  public string SaveImage(Image image, ImageFormat imageFormat, string proposedPath)
  {
    if (image == null)
      throw new ArgumentNullException(nameof (image));
    string[] array = new string[9]
    {
      "bmp",
      "tiff",
      "exif",
      "wmf",
      "icon",
      "jpeg",
      "png",
      "emf",
      "gif"
    };
    string extension = this.GetExtension(imageFormat);
    this.RegisterContentTypes(imageFormat);
    string str1;
    if (proposedPath == null)
    {
      string str2;
      do
      {
        ++this.m_iImageIndex;
        str2 = $"xl/media/image{this.m_iImageIndex}.";
      }
      while (this.m_archive.Find(new Regex(str2.Replace(".", "\\."))) != -1);
      str1 = str2 + extension;
    }
    else if (this.m_archive.Find(new Regex(proposedPath.Split('.')[0])) != -1)
    {
      ++this.m_iImageIndex;
      str1 = $"xl/media/image{this.m_iImageIndex}." + extension;
    }
    else
    {
      ++this.m_iImageIndex;
      string[] strArray = proposedPath.Split('.');
      str1 = Array.IndexOf<string>(array, strArray[1]) != -1 ? proposedPath : strArray[0] + (object) '.' + extension;
    }
    imageFormat = this.GetImageFormat(extension);
    ImageFormat rawFormat = image.RawFormat;
    MemoryStream newDataStream;
    if (rawFormat.Equals((object) ImageFormat.Emf) || rawFormat.Equals((object) ImageFormat.Wmf))
    {
      MemoryStream memoryStream1 = new MemoryStream();
      newDataStream = MsoMetafilePicture.SerializeMetafile(image);
      if (this.m_metafileStream.ContainsKey(str1))
      {
        MemoryStream memoryStream2 = this.m_metafileStream[str1];
        if (newDataStream.Length != memoryStream2.Length)
          newDataStream = memoryStream2;
      }
    }
    else
    {
      newDataStream = new MemoryStream();
      image.Save((Stream) newDataStream, imageFormat);
    }
    this.m_archive.UpdateItem(str1, (Stream) newDataStream, true, FileAttributes.Archive);
    return str1;
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
    item = this.m_archive[itemName];
    if (item == null)
      item = this.m_archive.AddItem(itemName, (Stream) new MemoryStream(), true, FileAttributes.Archive);
    else
      this.m_archive.UpdateItem(itemName, (Stream) new MemoryStream(), true, FileAttributes.Archive);
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
    this.Workbook.PivotTableCount = this.m_workbookRelations.CalculatePivotTableRelationInWorkbook("http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotTable");
    Relation relationByContentType2 = this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings", out this.m_strSSTRelationId);
    Relation relationByContentType3 = this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme", out this.m_strThemeRelationId);
    XmlReader reader1 = (XmlReader) null;
    MemoryStream functionGroups = new MemoryStream();
    string path;
    FileDataHolder.SeparateItemName(itemName, out path);
    if (zipArchiveItem1 != null)
    {
      Stream dataStream = zipArchiveItem1.DataStream;
      dataStream.Position = 0L;
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.NameTable = (XmlNameTable) new NameTable();
      XmlParserContext inputContext = new XmlParserContext((XmlNameTable) null, (XmlNamespaceManager) new MyXmlNamespaceManager(settings.NameTable), (string) null, XmlSpace.Default);
      reader1 = XmlReader.Create(dataStream, settings, inputContext);
    }
    this.Parser.ParseWorkbook(reader1, this.m_workbookRelations, this, path, this.m_streamStart, this.m_streamEnd, ref this.m_lstBookViews, (Stream) functionGroups, ref this.m_lstCustomBookViews);
    int fullSize = this.m_book.TabSheets.Count + 4;
    if (relationByContentType3 != null)
    {
      this.m_strThemesPartName = path + relationByContentType3.Target;
      ZipArchiveItem zipArchiveItem2 = this.GetItem(relationByContentType3, path, out string _);
      if (zipArchiveItem2 != null)
      {
        XmlReader reader2 = FileDataHolder.CreateReader(zipArchiveItem2);
        themeColors = this.Parser.ParseThemes(reader2);
        if (themeColors != null)
          this.Workbook.m_isThemeColorsParsed = true;
      }
      else
        this.m_workbookRelations.Remove(this.m_strThemeRelationId);
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
      this.Parser.ParseConnections(this.CreateReader(relationByContentType4, path));
      this.m_dictItemsToRemove.Add(this.m_connectionPartName, (object) null);
      this.m_workbookRelations.RemoveByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/connections");
    }
    int curPos2 = curPos1 + 1;
    appImplementation.RaiseProgressEvent((long) curPos2, (long) fullSize);
    Dictionary<int, int> dictUpdatedSSTIndexes = (Dictionary<int, int>) null;
    if (relationByContentType2 != null)
    {
      this.m_strSSTPartName = relationByContentType2.Target[0] != '/' ? path + relationByContentType2.Target : relationByContentType2.Target;
      ZipArchiveItem zipArchiveItem3 = this.GetItem(relationByContentType2, path, out string _);
      if (zipArchiveItem3 != null)
      {
        dictUpdatedSSTIndexes = this.Parser.ParseSST(FileDataHolder.CreateReader(zipArchiveItem3), parseOnDemand);
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
    {
      this.ParsePivotCaches(path);
      this.Parser.ParsePivotTables();
      if (this.m_book.ArrNewNumberFormatIndexes != null)
      {
        this.m_book.ArrNewNumberFormatIndexes.Clear();
        this.m_book.ArrNewNumberFormatIndexes = (Dictionary<int, int>) null;
      }
    }
    string relationId = string.Empty;
    Relation relationByContentType5 = this.m_workbookRelations.FindRelationByContentType("http://schemas.microsoft.com/office/2006/relationships/vbaProject", out relationId);
    if (relationByContentType5 != null)
    {
      string strItemPath = (string) null;
      this.Workbook.MacroStorage = this.Workbook.AppImplementation.CreateCompoundFile(this.GetItem(relationByContentType5, path, out strItemPath).DataStream).RootStorage;
      this.Workbook.HasMacros = true;
      this.m_workbookRelations.RemoveByContentType("http://schemas.microsoft.com/office/2006/relationships/vbaProject");
    }
    ZipArchiveItem zipArchiveItem4 = this.m_archive["xl/xmlMaps.xml"];
    if (zipArchiveItem4 != null)
      this.Parser.ParseXmlMaps(FileDataHolder.CreateReader(zipArchiveItem4));
    this.RemoveCalcChain();
  }

  private void ParseMetaProperties()
  {
    for (int index = 1; index < this.m_archive.Count; ++index)
    {
      string itemName = $"customXml/item{index}.xml";
      if (itemName == null || itemName.Length == 0)
        throw new ArgumentOutOfRangeException("CustomXmlPartName");
      if (itemName[0] == '/')
        itemName = UtilityMethods.RemoveFirstCharUnsafe(itemName);
      ZipArchiveItem zipArchiveItem = this.m_archive[itemName];
      if (zipArchiveItem != null)
        this.Parser.ParseMetaProperties(FileDataHolder.CreateReader(zipArchiveItem), this, zipArchiveItem.DataStream, zipArchiveItem.ItemName);
    }
  }

  private void ParseCustomXmlParts()
  {
    List<string> stringList1 = new List<string>();
    List<string> stringList2 = new List<string>();
    List<string> stringList3 = new List<string>();
    List<string> schemas = (List<string>) null;
    foreach (KeyValuePair<string, Relation> workbookRelation in this.m_workbookRelations)
    {
      if (workbookRelation.Value.Type == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/customXml" && !stringList1.Contains(workbookRelation.Key))
      {
        stringList1.Add(workbookRelation.Key);
        if (!stringList3.Contains(workbookRelation.Value.Target))
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
    for (int index = 0; index < stringList3.Count && index < stringList2.Count; ++index)
    {
      string xmlName = stringList3[index];
      string propertyName = stringList2[index];
      if (xmlName[0] == '.')
        xmlName = xmlName.Substring(3, xmlName.Length - 3);
      if (propertyName.StartsWith("/"))
        propertyName = UtilityMethods.RemoveFirstCharUnsafe(propertyName);
      string key = $"{xmlName.Substring(0, xmlName.IndexOf('/'))}/_rels/item{index + 1}.xml.rels";
      ICustomXmlPartCollection customXmlparts = this.m_book.CustomXmlparts;
      string xmlItemProperties = this.ParseCustomXmlItemProperties(propertyName, ref schemas);
      this.ParseCustomXmlParts(xmlName, customXmlparts, xmlItemProperties, schemas);
      if (this.m_book.InnerContentTypeProperties.ItemName == xmlName)
        this.m_book.InnerContentTypeProperties.ItemName = $"customXml/item{(object) (index + 1)}.xml";
      this.m_dictItemsToRemove.Add(key, (object) null);
    }
  }

  private void ParseCustomXmlParts(
    string xmlName,
    ICustomXmlPartCollection customXmlParts,
    string xmlId,
    List<string> schemas)
  {
    if (xmlName == null || xmlName.Length == 0)
      throw new ArgumentOutOfRangeException("itemName");
    if (xmlId == null || xmlId.Length == 0)
      throw new ArgumentOutOfRangeException("itemName");
    ZipArchiveItem zipArchiveItem = this.m_archive[xmlName];
    if (zipArchiveItem == null)
      return;
    Stream dataStream = zipArchiveItem.DataStream;
    dataStream.Position = 0L;
    byte[] buffer = new byte[16384 /*0x4000*/];
    using (MemoryStream memoryStream = new MemoryStream())
    {
      int count;
      while ((count = dataStream.Read(buffer, 0, buffer.Length)) > 0)
        memoryStream.Write(buffer, 0, count);
      ICustomXmlPart customXmlPart = customXmlParts.Add(xmlId, memoryStream.ToArray());
      if (schemas != null)
      {
        if (schemas.Count > 0)
        {
          foreach (string schema in schemas)
            customXmlPart.Schemas.Add(schema);
        }
      }
    }
    this.m_dictItemsToRemove.Add(xmlName, (object) null);
  }

  private string ParseCustomXmlItemProperties(string propertyName, ref List<string> schemas)
  {
    string xmlItemProperties = (string) null;
    ZipArchiveItem zipArchiveItem = propertyName != null && propertyName.Length != 0 ? this.m_archive[propertyName] : throw new ArgumentOutOfRangeException(nameof (propertyName));
    if (zipArchiveItem != null)
    {
      Stream dataStream = zipArchiveItem.DataStream;
      dataStream.Position = 0L;
      xmlItemProperties = this.Parser.ParseItemProperties(UtilityMethods.CreateReader(dataStream), ref schemas);
      this.m_dictItemsToRemove.Add(propertyName, (object) null);
    }
    return xmlItemProperties;
  }

  internal void ParsePivotCaches(string strWorkbookPath)
  {
    PivotCacheCollection pivotCacheCollection = new PivotCacheCollection(this.m_book.Application, (object) this.m_book);
    string strItemName = "";
    foreach (KeyValuePair<string, string> preservedCach in this.m_preservedCaches)
    {
      string str = preservedCach.Value;
      PivotCacheImpl pivotCache = this.ParsePivotCache(strWorkbookPath, str, out strItemName);
      this.m_book.PivotCaches.Add(Convert.ToInt32(preservedCach.Key), pivotCache);
      this.ItemsToRemove.Add(strItemName, (object) null);
      this.m_workbookRelations.Remove(str);
    }
    this.m_preservedCaches.Clear();
  }

  internal PivotCacheImpl ParsePivotCache(
    string strWorkbookPath,
    string relationID,
    out string strItemName)
  {
    XmlReader reader = this.CreateReader(this.m_workbookRelations[relationID], strWorkbookPath, out strItemName);
    string path = (string) null;
    FileDataHolder.SeparateItemName(strItemName, out path);
    RelationCollection relations = this.ParseRelations(FileDataHolder.GetCorrespondingRelations(strItemName));
    PivotCacheImpl cache = new PivotCacheImpl(this.m_book.Application, (object) this.m_book);
    cache.HasCacheRecords = relations != null;
    string cacheRecordRelationID = (string) null;
    cache.preservedCacheRelations = relations;
    PivotCacheParser.ParsePivotCacheDefinition(reader, cache, (IWorkbook) this.m_book, path, relations, out cacheRecordRelationID);
    if (cacheRecordRelationID != null)
    {
      string strItemPath;
      this.GetItem(relations[cacheRecordRelationID], path, out strItemPath);
      this.ItemsToRemove.Add(strItemPath, (object) null);
      relations.Remove(cacheRecordRelationID);
      cache.HasCacheRecords = false;
    }
    return cache;
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

  internal static XmlReader CreateReader(ZipArchiveItem item)
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
    string s = new StreamReader(this.GetItem(relation, parentItemPath, out strItemPath).DataStream).ReadToEnd().Replace("<br></br>", "<br/>").Replace("<br>", "<br/>");
    for (int startIndex = s.IndexOf('<', 0); startIndex >= 0 && s.IndexOf('>', startIndex + 1) >= 0; startIndex = s.IndexOf('<', startIndex + 1))
    {
      string oldValue = s.Substring(startIndex + 1, s.IndexOf('>', startIndex) - startIndex - 1);
      string[] strArray = oldValue.Split(' ');
      string newValue = string.Empty;
      foreach (string str in strArray)
      {
        if (!newValue.Contains(" " + str) || !str.Contains("="))
          newValue = newValue + (newValue.Equals(string.Empty) ? string.Empty : " ") + str;
      }
      if (oldValue != newValue)
        s = s.Replace(oldValue, newValue);
    }
    return UtilityMethods.CreateReader((Stream) new MemoryStream(Encoding.UTF8.GetBytes(s)));
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
    byte[] buffer = new byte[0];
    if (dataStream != null)
    {
      buffer = new byte[dataStream.Length];
      dataStream.Position = 0L;
      dataStream.Read(buffer, 0, (int) dataStream.Length);
    }
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
    if (!(startPath != ""))
      return endPath;
    return !endPath.StartsWith("../") ? startPath + (object) '/' + endPath : startPath + (object) '/' + endPath.Substring(3, endPath.Length - 3);
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
    string itemName = this.m_book.InnerContentTypeProperties.ItemName;
    if (!this.m_book.InnerContentTypeProperties.IsValid || itemName == null || itemName.Length <= 0)
      return;
    MemoryStream newDataStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) newDataStream));
    this.Serializator.SerializeContentTypeProperties(writer);
    writer.Flush();
    this.m_archive[itemName]?.Update((Stream) newDataStream, true);
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
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) memoryStream));
    switch (strItemPartName)
    {
      case "docProps/app.xml":
        this.Serializator.SerializeExtendedProperties(writer);
        break;
      case "docProps/core.xml":
        this.Serializator.SerializeCoreProperties(writer);
        break;
      case "[Content_Types].xml":
        this.Serializator.SerializeContentTypes(writer, this.m_dicDefaultTypes, this.m_dicOverriddenTypes);
        break;
      case "docProps/custom.xml":
        this.Serializator.SerializeCustomProperties(writer);
        break;
      default:
        throw new ArgumentException(nameof (strItemPartName));
    }
    writer.Flush();
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

  internal static string GetRelationId(int relationIndex) => $"rId{relationIndex}";

  private void SaveWorkbook(ExcelSaveType saveAsType)
  {
    this.m_strWorkbookContentType = this.SelectWorkbookContentType(saveAsType);
    if (this.m_topRelations == null)
    {
      this.m_topRelations = new RelationCollection();
      this.m_topRelations[FileDataHolder.GetRelationId(1)] = new Relation(this.m_strWorkbookPartName, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");
    }
    Dictionary<int, int> hashNewXFIndexes = this.SaveStyles();
    Dictionary<PivotCacheImpl, string> cacheFiles = this.SavePivotCaches();
    this.SaveSST();
    this.CustomXmlParts();
    this.m_arrImageItemNames = this.SaveWorkbookImages();
    this.SaveWorkbookPart(hashNewXFIndexes, cacheFiles);
    this.Connections();
    this.m_arrImageItemNames = (string[]) null;
  }

  private string SelectWorkbookContentType(ExcelSaveType saveType)
  {
    switch (saveType)
    {
      case ExcelSaveType.SaveAsTemplate:
        return "application/vnd.openxmlformats-officedocument.spreadsheetml.template.main+xml";
      case ExcelSaveType.SaveAsMacro:
        return "application/vnd.ms-excel.sheet.macroEnabled.main+xml";
      case ExcelSaveType.SaveAsMacroTemplate:
        return "application/vnd.ms-excel.template.macroEnabled.main+xml";
      default:
        return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml";
    }
  }

  private Dictionary<PivotCacheImpl, string> SavePivotCaches()
  {
    Dictionary<PivotCacheImpl, string> dictionary = new Dictionary<PivotCacheImpl, string>();
    PivotCacheCollection pivotCaches = this.m_book.PivotCaches;
    if ((pivotCaches != null ? pivotCaches.Count : 0) > 0)
    {
      foreach (PivotCacheImpl pivotCacheImpl in pivotCaches)
      {
        string str = this.SavePivotCache(pivotCacheImpl);
        dictionary[pivotCacheImpl] = str;
      }
    }
    return dictionary;
  }

  private string SavePivotCache(PivotCacheImpl cache)
  {
    string cacheRecordFileName = (string) null;
    if (!cache.HasCacheRecords)
      cacheRecordFileName = this.SavePivotCacheRecords(cache);
    return this.SavePivotCacheDefinition(cache, cacheRecordFileName);
  }

  private string SavePivotCacheDefinition(PivotCacheImpl cache, string cacheRecordFileName)
  {
    string pivotCacheFileName = this.GeneratePivotCacheFileName(cache);
    RelationCollection relations = new RelationCollection();
    if (cache.preservedCacheRelations != null && cache.preservedCacheRelations.Count > 0)
      relations = cache.preservedCacheRelations;
    string str = (string) null;
    if (cacheRecordFileName != null)
    {
      str = relations.GenerateRelationId();
      relations[str] = new Relation('/'.ToString() + cacheRecordFileName, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/pivotCacheRecords");
    }
    if (cache.PreservedExtenalRelation != null)
      relations[cache.RelationId] = cache.PreservedExtenalRelation;
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
    PivotCacheSerializator.SerializePivotCacheDefinition(writer, cache, (IWorkbook) this.m_book, str, relations);
    writer.Flush();
    this.AddOverriddenContentType(pivotCacheFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotCacheDefinition+xml");
    this.m_archive.UpdateItem(pivotCacheFileName, (Stream) memoryStream, true, FileAttributes.Archive);
    this.SaveRelations(pivotCacheFileName, relations);
    return pivotCacheFileName;
  }

  private string SavePivotCacheRecords(PivotCacheImpl cache)
  {
    string cacheRecordsFileName = this.GeneratePivotCacheRecordsFileName(cache);
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
    PivotCacheSerializator.SerializePivotCacheRecords(writer, cache, memoryStream);
    writer.Flush();
    this.m_archive.UpdateItem(cacheRecordsFileName, (Stream) memoryStream, true, FileAttributes.Archive);
    this.AddOverriddenContentType(cacheRecordsFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.pivotCacheRecords+xml");
    return cacheRecordsFileName;
  }

  private string GeneratePivotCacheRecordsFileName(PivotCacheImpl cache)
  {
    return $"xl/pivotCache/pivotCacheRecords{++this.LastPivotCacheRecordsIndex}.xml";
  }

  private string GeneratePivotCacheFileName(PivotCacheImpl cache)
  {
    return $"xl/pivotCache/pivotCacheDefinition{++this.LastPivotCacheIndex}.xml";
  }

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

  private string SerializeBSE(MsofbtBSE bse)
  {
    if (bse == null)
      throw new ArgumentNullException(nameof (bse));
    if (bse.BlipType != MsoBlipType.msoblipERROR && bse.PictureRecord.PictureStream != null)
      bse.PictureRecord.Picture = Image.FromStream(bse.PictureRecord.PictureStream);
    return bse.BlipType == MsoBlipType.msoblipERROR ? (string) null : this.SaveImage(bse.PictureRecord.Picture, bse.PicturePath);
  }

  private void CustomXmlParts()
  {
    ICustomXmlPartCollection customXmlparts = this.m_book.CustomXmlparts;
    if (customXmlparts == null || customXmlparts.Count <= 0)
      return;
    for (int index = 0; index < customXmlparts.Count; ++index)
    {
      string xmlpartname = $"item{index + 1}.xml";
      string propertyname = $"itemProps{index + 1}.xml";
      ICustomXmlPart customXmlPart = customXmlparts[index];
      if (customXmlPart != null)
      {
        this.SerializeCustomXmlRelation(xmlpartname, propertyname);
        this.SerializeCustomXmlPart(xmlpartname, customXmlPart.Data);
        this.SerializeCustomXmlItemProperty(xmlpartname, propertyname, customXmlPart);
      }
    }
  }

  private void SerializeCustomXmlRelation(string xmlpartname, string propertyname)
  {
    string type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/customXmlProps";
    string target = propertyname;
    this.SaveRelations("customXml/" + xmlpartname, new RelationCollection()
    {
      new Relation(target, type)
    });
  }

  private void SerializeCustomXmlPart(string xmlpartname, byte[] data)
  {
    string itemName = "customXml/" + xmlpartname;
    if (data != null)
    {
      MemoryStream newDataStream = new MemoryStream(data);
      this.m_archive.UpdateItem(itemName, (Stream) newDataStream, true, FileAttributes.Archive);
    }
    else
    {
      MemoryStream newDataStream = new MemoryStream();
      this.m_archive.UpdateItem(itemName, (Stream) newDataStream, true, FileAttributes.Archive);
    }
  }

  private void SerializeCustomXmlItemProperty(
    string xmlpartname,
    string propertyname,
    ICustomXmlPart customXmlPart)
  {
    string fullName = "/customXml/" + propertyname;
    string itemName = "customXml/" + propertyname;
    this.m_dicOverriddenTypes[new AddSlashPreprocessor().PreprocessName(fullName)] = "application/vnd.openxmlformats-officedocument.customXmlProperties+xml";
    MemoryStream newDataStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) newDataStream));
    this.Serializator.SerializeCustomXmlPartProperty(writer, customXmlPart);
    writer.Flush();
    this.m_archive.UpdateItem(itemName, (Stream) newDataStream, true, FileAttributes.Archive);
  }

  private Dictionary<int, int> SaveStyles()
  {
    this.m_dicOverriddenTypes[new AddSlashPreprocessor().PreprocessName(this.m_strStylesPartName)] = "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml";
    MemoryStream newDataStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) newDataStream));
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
      this.m_archive.UpdateItem(itemName, (Stream) newDataStream, true, FileAttributes.Archive);
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

  private void SaveWorkbookPart(
    Dictionary<int, int> hashNewXFIndexes,
    Dictionary<PivotCacheImpl, string> cacheFiles)
  {
    this.m_dicOverriddenTypes[new AddSlashPreprocessor().PreprocessName(this.m_strWorkbookPartName)] = this.m_strWorkbookContentType;
    if (this.m_workbookRelations == null)
      this.m_workbookRelations = new RelationCollection();
    this.SaveSheets(this.m_workbookRelations, this.m_strWorkbookPartName, hashNewXFIndexes, cacheFiles);
    Stream stream = (Stream) new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter(stream));
    this.Serializator.SerializeWorkbook(writer, this.m_streamStart, this.m_streamEnd, this.m_lstBookViews, this.m_workbookRelations, cacheFiles, this.m_functionGroups, this.m_lstCustomBookViews);
    writer.Flush();
    this.m_archive.UpdateItem(this.m_strWorkbookPartName, stream, true, FileAttributes.Archive);
    string path;
    FileDataHolder.SeparateItemName(this.m_strWorkbookPartName, out path);
    this.m_strStylesRelationId = FileDataHolder.AddRelation(this.m_workbookRelations, this.m_strStylesPartName, path, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles", this.m_strStylesRelationId);
    if (this.m_book.InnerSST.ActiveCount > 0)
      this.m_strSSTRelationId = FileDataHolder.AddRelation(this.m_workbookRelations, this.m_strSSTPartName, path, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings", this.m_strSSTRelationId);
    if (this.m_book.CustomXmlparts != null && this.m_book.CustomXmlparts.Count > 0)
    {
      for (int index = 0; index < this.m_book.CustomXmlparts.Count; ++index)
      {
        string target = $"../customXml/item{index + 1}.xml";
        Relation relation1 = new Relation(target, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/customXml");
        bool flag = false;
        foreach (Relation relation2 in this.m_workbookRelations.DicRelations.Values)
        {
          if (relation2 != null && relation2.Target == relation1.Target && relation2.Type == relation1.Type && relation2.IsExternal == relation1.IsExternal)
            flag = true;
        }
        if (!flag)
          FileDataHolder.AddRelation(this.m_workbookRelations, target, "customXml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/customXml", (string) null);
      }
    }
    if (this.m_book.Connections != null && this.m_book.Connections.Count > 0 || this.m_book.DeletedConnections != null && this.m_book.DeletedConnections.Count > 0)
    {
      string relationId;
      this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/connections", out relationId);
      if (relationId == null)
        FileDataHolder.AddRelation(this.m_workbookRelations, "connections.xml", "connection.xml", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/connections", (string) null);
    }
    if (this.m_book.XmlMaps != null && this.m_book.XmlMaps.Count > 0)
    {
      string relationId;
      this.m_workbookRelations.FindRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/xmlMaps", out relationId);
      if (relationId == null)
        FileDataHolder.AddRelation(this.m_workbookRelations, "xl/xmlMaps.xml", "", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/xmlMaps", (string) null);
    }
    if (this.m_sheetItems.Count != 0)
    {
      List<ZipArchiveItem> zipArchiveItemList = new List<ZipArchiveItem>();
      foreach (ZipArchiveItem zipArchiveItem in this.m_archive.Items)
      {
        if ((zipArchiveItem.ItemName.StartsWith("xl/worksheets") || zipArchiveItem.ItemName.StartsWith("xl/macrosheets") || zipArchiveItem.ItemName.StartsWith("xl/dialogsheets")) && !zipArchiveItem.ItemName.EndsWith(".rels") && !this.m_sheetItems.Contains(zipArchiveItem.ItemName))
          zipArchiveItemList.Add(zipArchiveItem);
      }
      foreach (ZipArchiveItem zipArchiveItem in zipArchiveItemList)
      {
        this.m_archive.RemoveItem(zipArchiveItem.ItemName);
        string relationByTarget = this.m_workbookRelations.FindRelationByTarget(zipArchiveItem.ItemName.Substring(3));
        if (relationByTarget != null)
          this.m_workbookRelations.Remove(relationByTarget);
      }
      zipArchiveItemList.Clear();
    }
    this.m_sheetItems.Clear();
    if ((this.m_strWorkbookContentType == "application/vnd.ms-excel.sheet.macroEnabled.main+xml" || this.m_strWorkbookContentType == "application/vnd.ms-excel.template.macroEnabled.main+xml") && this.Workbook.HasMacros && (this.Workbook.Application.SkipOnSave & SkipExtRecords.Macros) != SkipExtRecords.Macros)
      this.SaveMacros();
    this.SaveRelations(this.m_strWorkbookPartName, this.m_workbookRelations);
  }

  private void SaveMacros()
  {
    ICompoundFile compoundFile = this.m_book.AppImplementation.CreateCompoundFile();
    this.m_book.SaveMacroStorage(compoundFile.RootStorage, false);
    Stream stream = (Stream) new MemoryStream();
    compoundFile.Save(stream);
    stream.Position = 0L;
    this.Archive.RemoveItem("xl/vbaProject.bin");
    this.Archive.AddItem("xl/vbaProject.bin", stream, true, FileAttributes.Archive);
    compoundFile.Flush();
    compoundFile.Dispose();
    string relationId = (string) null;
    if (this.m_workbookRelations.FindRelationByContentType("http://schemas.microsoft.com/office/2006/relationships/vbaProject", out relationId) == null)
      FileDataHolder.AddRelation(this.m_workbookRelations, "/xl/vbaProject.bin".Substring(3), "", "http://schemas.microsoft.com/office/2006/relationships/vbaProject", (string) null);
    if (this.m_dicOverriddenTypes.ContainsKey("/xl/vbaProject.bin"))
      return;
    this.m_dicOverriddenTypes.Add("/xl/vbaProject.bin", "application/vnd.ms-office.vbaProject");
  }

  internal static string AddRelation(
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

  private void SaveSheets(
    RelationCollection relations,
    string workbookItemName,
    Dictionary<int, int> hashNewXFIndexes,
    Dictionary<PivotCacheImpl, string> cacheFiles)
  {
    string path;
    FileDataHolder.SeparateItemName(workbookItemName, out path);
    WorkbookObjectsCollection objects = this.m_book.Objects;
    this.ReserveSheetRelations(objects, relations);
    int index1 = 0;
    for (int count = objects.Count; index1 < count; ++index1)
    {
      WorksheetBaseImpl sheet = (WorksheetBaseImpl) objects[index1];
      if (sheet != null)
      {
        string itemName = string.Format(sheet is WorksheetImpl ? "xl/worksheets/sheet{0}.xml" : "xl/chartsheets/sheet{0}.xml", (object) (sheet.Index + 1));
        this.SaveSheet(sheet, itemName, relations, path, hashNewXFIndexes, cacheFiles);
      }
    }
    for (int index2 = 0; index2 < this.m_book.InnerDialogs.Count; ++index2)
    {
      string itemName = $"xl/dialogsheets/sheet{index2 + 1}.xml";
      this.SaveDialogSheet(this.m_book.InnerDialogs[index2], itemName, relations, path, hashNewXFIndexes, cacheFiles);
    }
    int num1 = 0;
    int num2 = 0;
    for (int index3 = 0; index3 < this.m_book.InnerMacros.Count; ++index3)
    {
      string itemName;
      if (!this.m_book.InnerMacros[index3].Value)
      {
        itemName = $"xl/macrosheets/sheet{num1 + 1}.xml";
        ++num1;
      }
      else
      {
        itemName = $"xl/macrosheets/intlsheet{num2 + 1}.xml";
        ++num2;
      }
      this.SaveMacroSheet(this.m_book.InnerMacros[index3], itemName, relations, path, hashNewXFIndexes, cacheFiles);
    }
  }

  private void ReserveSheetRelations(WorkbookObjectsCollection sheets, RelationCollection relations)
  {
    int index = 0;
    for (int count = sheets.Count; index < count; ++index)
    {
      WorksheetBaseImpl sheet = (WorksheetBaseImpl) sheets[index];
      if (sheet != null)
      {
        WorksheetDataHolder dataHolder = sheet.DataHolder;
        if (dataHolder != null)
        {
          string relationId = dataHolder.RelationId;
          if (relationId != null)
            relations[relationId] = (Relation) null;
        }
      }
    }
  }

  private void SaveSheet(
    WorksheetBaseImpl sheet,
    string itemName,
    RelationCollection relations,
    string workbookPath,
    Dictionary<int, int> hashNewXFIndexes,
    Dictionary<PivotCacheImpl, string> cacheFiles)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (itemName == null || itemName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (itemName));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    string type = (string) null;
    if (sheet is WorksheetImpl)
    {
      type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet";
      this.SaveWorksheet((WorksheetImpl) sheet, itemName, hashNewXFIndexes, cacheFiles);
    }
    else if (sheet is ChartImpl)
    {
      type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chartsheet";
      this.SaveChartsheet((ChartImpl) sheet, itemName);
    }
    if (itemName.StartsWith(workbookPath))
      itemName = itemName.Substring(workbookPath.Length);
    string relationId = sheet.m_dataHolder.RelationId;
    if (relationId == null)
    {
      relationId = relations.GenerateRelationId();
      sheet.m_dataHolder.RelationId = relationId;
    }
    Relation relation = new Relation(itemName, type);
    relations.RemoveRelationByTarget(itemName);
    relations[relationId] = relation;
  }

  private void SaveDialogSheet(
    DialogSheet dialogSheet,
    string itemName,
    RelationCollection relations,
    string workbookPath,
    Dictionary<int, int> hashNewXFIndexes,
    Dictionary<PivotCacheImpl, string> cacheFiles)
  {
    WorksheetDataHolder worksheetDataHolder = dialogSheet.DataHolder;
    if (itemName == null || itemName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (itemName));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    string type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/dialogsheet";
    string key = itemName != null && itemName.Length != 0 ? itemName : throw new ArgumentOutOfRangeException(nameof (itemName));
    if (itemName[0] != '/')
      key = '/'.ToString() + itemName;
    this.m_dicOverriddenTypes[key] = "application/vnd.openxmlformats-officedocument.spreadsheetml.dialogsheet+xml";
    if (worksheetDataHolder == null)
    {
      this.m_archive.RemoveItem(itemName);
      worksheetDataHolder = new WorksheetDataHolder(this, this.m_archive.AddItem(itemName, (Stream) null, false, FileAttributes.Archive));
    }
    this.m_sheetItems.Add(itemName);
    worksheetDataHolder.SerializeDialogsheet(dialogSheet.PreservedStream, this.m_book);
    if (itemName.StartsWith(workbookPath))
      itemName = itemName.Substring(workbookPath.Length);
    string relationId = dialogSheet.DataHolder.RelationId;
    if (relationId == null)
    {
      relationId = relations.GenerateRelationId();
      dialogSheet.DataHolder.RelationId = relationId;
    }
    Relation relation = new Relation(itemName, type);
    relations.RemoveRelationByTarget(itemName);
    relations[relationId] = relation;
  }

  private void SaveMacroSheet(
    MacroSheet macroSheet,
    string itemName,
    RelationCollection relations,
    string workbookPath,
    Dictionary<int, int> hashNewXFIndexes,
    Dictionary<PivotCacheImpl, string> cacheFiles)
  {
    WorksheetDataHolder worksheetDataHolder = macroSheet.DataHolder;
    if (itemName == null || itemName.Length == 0)
      throw new ArgumentOutOfRangeException(nameof (itemName));
    if (relations == null)
      throw new ArgumentNullException(nameof (relations));
    string type = macroSheet.Value ? "http://schemas.microsoft.com/office/2006/relationships/xlIntlMacrosheet" : "http://schemas.microsoft.com/office/2006/relationships/xlMacrosheet";
    string key = itemName != null && itemName.Length != 0 ? itemName : throw new ArgumentOutOfRangeException(nameof (itemName));
    if (itemName[0] != '/')
      key = '/'.ToString() + itemName;
    this.m_dicOverriddenTypes[key] = macroSheet.Value ? "application/vnd.ms-excel.intlmacrosheet+xml" : "application/vnd.ms-excel.macrosheet+xml";
    if (worksheetDataHolder == null)
    {
      this.m_archive.RemoveItem(itemName);
      worksheetDataHolder = new WorksheetDataHolder(this, this.m_archive.AddItem(itemName, (Stream) null, false, FileAttributes.Archive));
    }
    this.m_sheetItems.Add(itemName);
    worksheetDataHolder.SerializeMacrosheet(macroSheet.PreservedStream, this.m_book);
    if (itemName.StartsWith(workbookPath))
      itemName = itemName.Substring(workbookPath.Length);
    string relationId = macroSheet.DataHolder.RelationId;
    if (relationId == null)
    {
      relationId = relations.GenerateRelationId();
      macroSheet.DataHolder.RelationId = relationId;
    }
    Relation relation = new Relation(itemName, type);
    relations.RemoveRelationByTarget(itemName);
    relations[relationId] = relation;
  }

  private void SaveWorksheet(
    WorksheetImpl sheet,
    string itemName,
    Dictionary<int, int> hashNewXFIndexes,
    Dictionary<PivotCacheImpl, string> cacheFiles)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    string key = itemName != null && itemName.Length != 0 ? itemName : throw new ArgumentOutOfRangeException(nameof (itemName));
    if (itemName[0] != '/')
      key = '/'.ToString() + itemName;
    this.m_dicOverriddenTypes[key] = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
    this.UpdateArchiveItem(sheet, itemName);
    sheet.m_dataHolder.SerializeWorksheet(sheet, hashNewXFIndexes, cacheFiles);
    this.m_sheetItems.Add(itemName);
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

  internal string SerializeTable(IListObject listObject)
  {
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
    new TableSerializator().Serialize(writer, listObject);
    writer.Flush();
    memoryStream.Flush();
    string tableFileName = this.GenerateTableFileName();
    if (listObject.QueryTable != null)
    {
      QueryTableImpl queryTable = listObject.QueryTable;
      string itemName = string.Format(this.m_queryTablePartName, (object) this.m_queryTableCount);
      this.SerializeQueryTable(listObject, itemName, tableFileName);
      ++this.m_queryTableCount;
      if (queryTable.ConnectionDeleted && this.checkconnection(queryTable.ExternalConnection.ConncetionId))
        this.m_book.DeletedConnections.Add((IConnection) queryTable.ExternalConnection);
    }
    this.OverriddenContentTypes['/'.ToString() + tableFileName] = "application/vnd.openxmlformats-officedocument.spreadsheetml.table+xml";
    this.ChartAndTableItemsToRemove.Add(tableFileName);
    this.m_archive.UpdateItem(tableFileName, (Stream) memoryStream, true, FileAttributes.Archive);
    return tableFileName;
  }

  private bool checkconnection(uint id)
  {
    ExternalConnectionCollection deletedConnections = this.m_book.DeletedConnections as ExternalConnectionCollection;
    for (int i = 0; i < deletedConnections.Count; ++i)
    {
      if ((int) id == (int) deletedConnections[i].ConncetionId)
        return false;
    }
    return true;
  }

  private string GenerateTableFileName()
  {
    int itemsCount = 0;
    return this.GenerateItemName(ref itemsCount, "xl/tables/table{0}.xml");
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

  public void Connections()
  {
    if ((this.m_book.Connections == null || this.m_book.Connections.Count <= 0) && (this.m_book.DeletedConnections == null || this.m_book.DeletedConnections.Count <= 0))
      return;
    this.m_dicOverriddenTypes[new AddSlashPreprocessor().PreprocessName(this.m_connectionPartName)] = "application/vnd.openxmlformats-officedocument.spreadsheetml.connections+xml";
    MemoryStream newDataStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) newDataStream));
    this.Serializator.SerializeConnections(writer);
    writer.Flush();
    string itemName = this.m_connectionPartName;
    if (this.m_connectionPartName[0] == '/')
      itemName = UtilityMethods.RemoveFirstCharUnsafe(this.m_connectionPartName);
    this.m_archive.UpdateItem(itemName, (Stream) newDataStream, true, FileAttributes.Archive);
  }

  public void SerializeQueryTable(IListObject listobject, string itemName, string tablerels)
  {
    this.m_dicOverriddenTypes[new AddSlashPreprocessor().PreprocessName(itemName)] = "application/vnd.openxmlformats-officedocument.spreadsheetml.queryTable+xml";
    MemoryStream newDataStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((TextWriter) new StreamWriter((Stream) newDataStream));
    new TableSerializator().SerializeQueryTable(listobject, writer);
    writer.Flush();
    string itemName1 = itemName;
    if (itemName[0] == '/')
      itemName1 = UtilityMethods.RemoveFirstCharUnsafe(itemName);
    this.m_archive.UpdateItem(itemName1, (Stream) newDataStream, true, FileAttributes.Archive);
    this.SerializeTableRelation(tablerels, itemName);
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

  private void SaveXmlMaps()
  {
    if (this.m_book.XmlMaps == null || this.m_book.XmlMaps.Count <= 0)
      return;
    MemoryStream memoryStream = new MemoryStream();
    XmlWriter writer = UtilityMethods.CreateWriter((Stream) memoryStream, Encoding.UTF8);
    this.m_book.XmlMaps.Serialize(writer);
    writer.Flush();
    memoryStream.Flush();
    this.m_archive.UpdateItem("xl/xmlMaps.xml", (Stream) memoryStream, true, FileAttributes.Archive);
  }

  public void Serialize(string fullName, WorkbookImpl book, ExcelSaveType saveType)
  {
    if (book != this.m_book)
      throw new ArgumentOutOfRangeException(nameof (book));
    this.SaveDocument(fullName, saveType);
  }

  public void Serialize(Stream stream, WorkbookImpl book, ExcelSaveType saveType)
  {
    if (book != this.m_book)
      throw new ArgumentOutOfRangeException(nameof (book));
    this.SaveDocument(stream, saveType);
  }

  internal FileDataHolder Clone(WorkbookImpl newParent)
  {
    FileDataHolder fileDataHolder = (FileDataHolder) this.MemberwiseClone();
    fileDataHolder.m_book = newParent;
    fileDataHolder.m_parser = (Excel2007Parser) null;
    fileDataHolder.m_serializator = (Excel2007Serializator) null;
    fileDataHolder.m_workbookRelations = this.m_workbookRelations.Clone();
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
    if (this.m_sheetItems != null)
    {
      this.m_sheetItems.Clear();
      this.m_sheetItems = (List<string>) null;
    }
    this.m_functionGroups = (Stream) null;
    this.m_extensions = (Stream) null;
    this.m_streamDxfs = (Stream) null;
    this.m_streamStart = (Stream) null;
    this.m_streamEnd = (Stream) null;
    GC.SuppressFinalize((object) this);
  }
}
