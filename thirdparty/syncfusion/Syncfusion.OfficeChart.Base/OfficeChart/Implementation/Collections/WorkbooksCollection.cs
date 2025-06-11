// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.WorkbooksCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class WorkbooksCollection(IApplication application, object parent) : 
  CollectionBaseEx<object>(application, parent),
  IWorkbooks,
  IEnumerable
{
  public IWorkbook this[int Index] => (IWorkbook) this.InnerList[Index];

  public IWorkbook Create(string[] names)
  {
    if (names == null)
      throw new ArgumentNullException(nameof (names));
    if (names.Length == 0)
      throw new ArgumentException("Names array must contain at least one name.");
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, names.Length, this.Application.DefaultVersion);
    for (int Index = 0; Index < names.Length; ++Index)
      workbook.Worksheets[Index].Name = names[Index];
    this.Add((object) workbook);
    workbook.Activate();
    this.SetAplicatioName((IWorkbook) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Create(int sheetsQuantity)
  {
    if (sheetsQuantity < 0)
      throw new ArgumentOutOfRangeException(nameof (sheetsQuantity), "Quantity of worksheets must be greater than zero.");
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, sheetsQuantity, this.Application.DefaultVersion);
    if (this.Application.DefaultVersion == OfficeVersion.Excel97to2003)
      workbook.BeginVersion = 2;
    this.Add((object) workbook);
    workbook.Activate();
    this.SetAplicatioName((IWorkbook) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Create()
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, this.Application.DefaultVersion);
    if (this.Application.DefaultVersion == OfficeVersion.Excel97to2003)
      workbook.BeginVersion = 2;
    this.Add((object) workbook);
    workbook.Activate();
    this.SetAplicatioName((IWorkbook) workbook);
    return (IWorkbook) workbook;
  }

  private void SetAplicatioName(IWorkbook book)
  {
  }

  public IWorkbook Add() => this.Add((string) null, this.Application.DefaultVersion);

  public IWorkbook Add(OfficeVersion version) => this.Add((string) null, version);

  public IWorkbook Add(string strTemplateFile)
  {
    return this.Add(strTemplateFile, OfficeParseOptions.Default);
  }

  public IWorkbook Add(string strTemplateFile, OfficeVersion version)
  {
    return this.Add(strTemplateFile, OfficeParseOptions.Default, version);
  }

  public IWorkbook Add(string strTemplateFile, OfficeParseOptions options)
  {
    OfficeVersion version = this.DetectVersion(strTemplateFile);
    return this.Add(strTemplateFile, options, version);
  }

  public IWorkbook Add(string strTemplateFile, OfficeParseOptions options, OfficeVersion version)
  {
    WorkbookImpl workbookImpl = strTemplateFile != null ? this.AppImplementation.CreateWorkbook((object) this, strTemplateFile, options, version) : this.AppImplementation.CreateWorkbook((object) this, version);
    this.Add((object) workbookImpl);
    workbookImpl.Activate();
    workbookImpl.Worksheets[0].Activate();
    return (IWorkbook) workbookImpl;
  }

  public IWorkbook Open(string filename) => this.Open(filename, OfficeOpenType.Automatic);

  public IWorkbook Open(string filename, OfficeVersion version)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, filename, version);
    workbook.InternalSaved = true;
    this.Add((object) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Open(Stream stream, string separator, int row, int column)
  {
    return this.Open(stream, separator, row, column, (string) null, (Encoding) null, this.Application.DefaultVersion);
  }

  public IWorkbook Open(Stream stream, string separator, int row, int column, Encoding encoding)
  {
    return this.Open(stream, separator, row, column, (string) null, encoding, this.Application.DefaultVersion);
  }

  private IWorkbook Open(
    Stream stream,
    string separator,
    int row,
    int column,
    string fileName,
    Encoding encoding,
    OfficeVersion version)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    switch (separator)
    {
      case null:
        throw new ArgumentNullException(nameof (separator));
      case "":
        throw new ArgumentException(nameof (separator));
      default:
        return this.OpenInternal(stream, separator, row, column, fileName, encoding, version);
    }
  }

  private IWorkbook OpenInternal(
    Stream stream,
    string separator,
    int row,
    int column,
    string fileName,
    Encoding encoding,
    OfficeVersion version)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, stream, separator, row, column, version, fileName, encoding);
    this.Add((object) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Open(string fileName, string separator, int row, int column)
  {
    return this.Open(fileName, separator, row, column, (Encoding) null);
  }

  public IWorkbook Open(
    string fileName,
    string separator,
    int row,
    int column,
    Encoding encoding)
  {
    switch (fileName)
    {
      case null:
        throw new ArgumentNullException(nameof (fileName));
      case "":
        throw new ArgumentException(nameof (fileName));
      default:
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          WorkbookImpl workbookImpl = (WorkbookImpl) this.Open((Stream) fileStream, separator, row, column, fileName, encoding, this.Application.DefaultVersion);
          workbookImpl.FullFileName = Path.GetFullPath(fileName);
          workbookImpl.InternalSaved = true;
          return (IWorkbook) workbookImpl;
        }
    }
  }

  public IWorkbook Open(Stream stream, string separator)
  {
    return this.Open(stream, separator, 1, 1, (Encoding) null);
  }

  public IWorkbook Open(Stream stream, string separator, OfficeVersion version)
  {
    return this.Open(stream, separator, 1, 1, (string) null, (Encoding) null, version);
  }

  public IWorkbook Open(string fileName, string separator) => this.Open(fileName, separator, 1, 1);

  public IWorkbook Open(Stream stream, string separator, Encoding encoding)
  {
    return this.Open(stream, separator, 1, 1, encoding);
  }

  public IWorkbook Open(string fileName, string separator, Encoding encoding)
  {
    return this.Open(fileName, separator, 1, 1, encoding);
  }

  public IWorkbook Open(string fileName, OfficeParseOptions options)
  {
    return this.Open(fileName, OfficeOpenType.Automatic, options);
  }

  public IWorkbook Open(
    string fileName,
    OfficeParseOptions options,
    bool isReadOnly,
    string password)
  {
    return this.Open(fileName, options, isReadOnly, password, this.Application.DefaultVersion);
  }

  public IWorkbook Open(
    Stream stream,
    OfficeParseOptions options,
    bool isReadOnly,
    string password)
  {
    return this.Open(stream, options, isReadOnly, password, this.Application.DefaultVersion);
  }

  public IWorkbook Open(
    Stream stream,
    OfficeParseOptions options,
    bool isReadOnly,
    string password,
    OfficeVersion excelVersion)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, stream, options, isReadOnly, password, excelVersion);
    this.Add((object) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Open(
    Stream stream,
    OfficeParseOptions options,
    bool isReadOnly,
    string password,
    OfficeOpenType openType)
  {
    if (openType == OfficeOpenType.Automatic)
      openType = this.AppImplementation.DetectFileFromStream(stream);
    switch (openType)
    {
      case OfficeOpenType.CSV:
        return this.Open(stream, this.Application.CSVSeparator);
      case OfficeOpenType.SpreadsheetML:
        return this.OpenFromXml(stream, OfficeXmlOpenType.MSExcel);
      case OfficeOpenType.BIFF:
        return this.Open(stream, options, isReadOnly, password, OfficeVersion.Excel97to2003);
      case OfficeOpenType.SpreadsheetML2007:
        return this.Open(stream, options, isReadOnly, password, OfficeVersion.Excel2007);
      case OfficeOpenType.SpreadsheetML2010:
        return this.Open(stream, options, isReadOnly, password, OfficeVersion.Excel2010);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  public IWorkbook Open(
    string fileName,
    OfficeParseOptions options,
    bool isReadOnly,
    string password,
    OfficeVersion version)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, fileName, options, isReadOnly, password, version);
    this.Add((object) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Open(
    string fileName,
    OfficeParseOptions options,
    bool isReadOnly,
    string password,
    OfficeOpenType openType)
  {
    if (isReadOnly)
    {
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        if (openType == OfficeOpenType.Automatic)
          openType = this.AppImplementation.DetectFileFromStream((Stream) fileStream);
      }
    }
    else
    {
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        if (openType == OfficeOpenType.Automatic)
          openType = this.AppImplementation.DetectFileFromStream((Stream) fileStream);
      }
    }
    if (openType == OfficeOpenType.Automatic)
      throw new ArgumentException("Cannot recognize current file type.");
    switch (openType)
    {
      case OfficeOpenType.CSV:
        return this.Open(fileName, this.Application.CSVSeparator);
      case OfficeOpenType.SpreadsheetML:
        return this.OpenFromXml(fileName, OfficeXmlOpenType.MSExcel);
      case OfficeOpenType.BIFF:
        return this.Open(fileName, options, isReadOnly, password, OfficeVersion.Excel97to2003);
      case OfficeOpenType.SpreadsheetML2007:
        return this.Open(fileName, options, isReadOnly, password, OfficeVersion.Excel2007);
      case OfficeOpenType.SpreadsheetML2010:
        return this.Open(fileName, options, isReadOnly, password, OfficeVersion.Excel2010);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  public IWorkbook Open(Stream stream) => this.Open(stream, OfficeOpenType.Automatic);

  public IWorkbook Open(Stream stream, OfficeVersion version)
  {
    return this.Open(stream, version, OfficeParseOptions.Default);
  }

  public IWorkbook Open(Stream stream, OfficeVersion version, OfficeParseOptions options)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, stream, version, options);
    this.Add((object) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Open(Stream stream, OfficeParseOptions options)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, stream, options, this.Application.DefaultVersion);
    this.Add((object) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Open(string fileName, OfficeOpenType openType)
  {
    return this.Open(fileName, openType, OfficeParseOptions.Default);
  }

  public IWorkbook Open(string fileName, OfficeOpenType openType, OfficeParseOptions options)
  {
    return this.Open(fileName, openType, this.Application.DefaultVersion, options);
  }

  public IWorkbook Open(string fileName, OfficeOpenType openType, OfficeVersion version)
  {
    return this.Open(fileName, openType, version, OfficeParseOptions.Default);
  }

  public IWorkbook Open(
    string fileName,
    OfficeOpenType openType,
    OfficeVersion version,
    OfficeParseOptions options)
  {
    if (!File.Exists(fileName))
      throw new FileNotFoundException($"File {fileName} could not be found. Please verify the file path.");
    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
    {
      string currentDirectory = Environment.CurrentDirectory;
      fileName = Path.GetFullPath(fileName);
      Environment.CurrentDirectory = Path.GetDirectoryName(fileName);
      WorkbookImpl workbookImpl = (WorkbookImpl) this.Open((Stream) fileStream, openType, fileName, version, options);
      workbookImpl.FullFileName = Path.GetFullPath(fileName);
      Environment.CurrentDirectory = currentDirectory;
      return (IWorkbook) workbookImpl;
    }
  }

  public IWorkbook Open(Stream stream, OfficeOpenType openType, OfficeVersion version)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (openType == OfficeOpenType.Automatic)
      openType = this.AppImplementation.DetectFileFromStream(stream);
    if (openType == OfficeOpenType.Automatic)
      throw new ArgumentException("Cannot recognize current file type.");
    switch (openType)
    {
      case OfficeOpenType.CSV:
        return this.Open(stream, this.Application.CSVSeparator, version);
      case OfficeOpenType.SpreadsheetML:
        return this.OpenFromXml(stream, OfficeXmlOpenType.MSExcel);
      case OfficeOpenType.BIFF:
        return this.Open(stream, OfficeVersion.Excel97to2003);
      case OfficeOpenType.SpreadsheetML2007:
        return this.Open(stream, OfficeVersion.Excel2007);
      case OfficeOpenType.SpreadsheetML2010:
        return this.Open(stream, OfficeVersion.Excel2010);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  public IWorkbook Open(Stream stream, OfficeOpenType openType)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (openType == OfficeOpenType.Automatic)
      openType = this.AppImplementation.DetectFileFromStream(stream);
    if (openType == OfficeOpenType.Automatic)
      throw new ArgumentException("Cannot recognize current file type.");
    switch (openType)
    {
      case OfficeOpenType.CSV:
        return this.Open(stream, this.Application.CSVSeparator);
      case OfficeOpenType.SpreadsheetML:
        return this.OpenFromXml(stream, OfficeXmlOpenType.MSExcel);
      case OfficeOpenType.BIFF:
        return this.Open(stream, OfficeVersion.Excel97to2003);
      case OfficeOpenType.SpreadsheetML2007:
        return this.Open(stream, OfficeVersion.Excel2007);
      case OfficeOpenType.SpreadsheetML2010:
        return this.Open(stream, OfficeVersion.Excel2010);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  public IWorkbook Open(Stream stream, OfficeOpenType openType, OfficeParseOptions options)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (openType == OfficeOpenType.Automatic)
      openType = this.AppImplementation.DetectFileFromStream(stream);
    if (openType == OfficeOpenType.Automatic)
      throw new ArgumentException("Cannot recognize current file type.");
    switch (openType)
    {
      case OfficeOpenType.CSV:
        return this.Open(stream, this.Application.CSVSeparator);
      case OfficeOpenType.SpreadsheetML:
        return this.OpenFromXml(stream, OfficeXmlOpenType.MSExcel);
      case OfficeOpenType.BIFF:
        return this.Open(stream, OfficeVersion.Excel97to2003, options);
      case OfficeOpenType.SpreadsheetML2007:
        return this.Open(stream, OfficeVersion.Excel2007, options);
      case OfficeOpenType.SpreadsheetML2010:
        return this.Open(stream, OfficeVersion.Excel2010, options);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  private IWorkbook Open(
    Stream stream,
    OfficeOpenType openType,
    string fileName,
    OfficeVersion version)
  {
    return this.Open(stream, openType, fileName, version, OfficeParseOptions.Default);
  }

  private IWorkbook Open(
    Stream stream,
    OfficeOpenType openType,
    string fileName,
    OfficeVersion version,
    OfficeParseOptions options)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (openType == OfficeOpenType.Automatic)
      openType = this.AppImplementation.DetectFileFromStream(stream);
    if (openType == OfficeOpenType.Automatic)
      throw new ArgumentException("Cannot recognize current file type.");
    switch (openType)
    {
      case OfficeOpenType.CSV:
        return this.Open(stream, this.Application.CSVSeparator, 1, 1, fileName, (Encoding) null, version);
      case OfficeOpenType.SpreadsheetML:
        return this.OpenFromXml(stream, OfficeXmlOpenType.MSExcel);
      case OfficeOpenType.BIFF:
        return this.Open(stream, OfficeVersion.Excel97to2003, options);
      case OfficeOpenType.SpreadsheetML2007:
        return this.Open(stream, OfficeVersion.Excel2007, options);
      case OfficeOpenType.SpreadsheetML2010:
        return this.Open(stream, OfficeVersion.Excel2010, options);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  public IWorkbook OpenFromXml(string strPath, OfficeXmlOpenType openType)
  {
    if (!File.Exists(strPath))
      throw new FileNotFoundException("File could not be found. Please verify the file path.");
    using (FileStream fileStream = new FileStream(strPath, FileMode.Open, FileAccess.Read, FileShare.Read))
      return this.OpenFromXml((Stream) fileStream, openType);
  }

  public IWorkbook OpenFromXml(Stream stream, OfficeXmlOpenType openType)
  {
    return stream != null ? this.OpenFromXml(XmlReader.Create((TextReader) new StreamReader(stream)), openType) : throw new ArgumentNullException(nameof (stream));
  }

  public IWorkbook OpenFromXml(XmlReader reader, OfficeXmlOpenType openType)
  {
    return (reader != null ? this.OpenFromXmlInternal(reader, openType) : throw new ArgumentNullException(nameof (reader))) ?? throw new ApplicationException("Unable to read from XML.");
  }

  private IWorkbook OpenFromXmlInternal(XmlReader reader, OfficeXmlOpenType openType)
  {
    if (reader is XmlTextReader xmlTextReader)
      xmlTextReader.WhitespaceHandling = WhitespaceHandling.Significant;
    WorkbookImpl workbookImpl = new WorkbookImpl(this.Application, (object) this, reader, openType);
    if (workbookImpl == null)
      return (IWorkbook) workbookImpl;
    this.Add((object) workbookImpl);
    return (IWorkbook) workbookImpl;
  }

  public IWorkbook OpenReadOnly(string strFileName)
  {
    return this.OpenReadOnly(strFileName, OfficeOpenType.Automatic, OfficeParseOptions.Default);
  }

  public IWorkbook OpenReadOnly(string strFileName, string separator)
  {
    using (FileStream fileStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
      IWorkbook workbook = this.Open((Stream) fileStream, separator);
      (workbook as WorkbookImpl).ReadOnly = true;
      return workbook;
    }
  }

  public IWorkbook OpenReadOnly(string strFileName, OfficeParseOptions options)
  {
    using (FileStream fileStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
      IWorkbook workbook = this.Open((Stream) fileStream, options);
      (workbook as WorkbookImpl).ReadOnly = true;
      return workbook;
    }
  }

  public IWorkbook OpenReadOnly(
    string strFileName,
    OfficeOpenType openType,
    OfficeParseOptions options)
  {
    using (FileStream fileStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
      IWorkbook workbook = this.Open((Stream) fileStream, openType, options);
      (workbook as WorkbookImpl).ReadOnly = true;
      return workbook;
    }
  }

  public void Close()
  {
    IWorkbook activeWorkbook = this.Application.ActiveWorkbook;
    if (activeWorkbook == null || this.InnerList.IndexOf((object) activeWorkbook) < 0 || this.InnerList.Count <= 0)
      return;
    activeWorkbook.Close(true, (string) null);
  }

  public IWorkbook PasteWorkbook()
  {
    return this.AppImplementation.CreateClipboardProvider().GetBookFromClipboard((IWorkbooks) this);
  }

  private OfficeVersion DetectVersion(string strTemplateFile)
  {
    if (strTemplateFile == null || !File.Exists(strTemplateFile))
      return this.Application.DefaultVersion;
    OfficeVersion defaultVersion = this.Application.DefaultVersion;
    using (FileStream fileStream = new FileStream(strTemplateFile, FileMode.Open, FileAccess.Read, FileShare.Read))
    {
      switch (this.AppImplementation.DetectFileFromStream((Stream) fileStream))
      {
        case OfficeOpenType.BIFF:
          return OfficeVersion.Excel97to2003;
        case OfficeOpenType.SpreadsheetML2007:
          return OfficeVersion.Excel2007;
        case OfficeOpenType.SpreadsheetML2010:
          return OfficeVersion.Excel2010;
        case OfficeOpenType.Automatic:
          throw new ArgumentException("Cannot recognize current file type.");
        default:
          throw new ArgumentOutOfRangeException("openType");
      }
    }
  }
}
