// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.WorkbooksCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class WorkbooksCollection(IApplication application, object parent) : 
  CollectionBaseEx<IWorkbook>(application, parent),
  IWorkbooks,
  IEnumerable<IWorkbook>,
  IEnumerable
{
  private const string UTF16Encoding = "utf-16";
  private const RegexOptions DEF_REGEX = RegexOptions.Compiled;

  public new IWorkbook this[int Index] => this.InnerList[Index];

  public IWorkbook Create(string[] names)
  {
    if (names == null)
      throw new ArgumentNullException(nameof (names));
    if (names.Length == 0)
      throw new ArgumentException("Names array must contain at least one name.");
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, names.Length, this.Application.DefaultVersion);
    for (int Index = 0; Index < names.Length; ++Index)
      workbook.Worksheets[Index].Name = names[Index];
    this.Add((IWorkbook) workbook);
    workbook.Activate();
    this.SetAplicatioName((IWorkbook) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Create(int sheetsQuantity)
  {
    if (sheetsQuantity < 0)
      throw new ArgumentOutOfRangeException(nameof (sheetsQuantity), "Quantity of worksheets must be greater than zero.");
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, sheetsQuantity, this.Application.DefaultVersion);
    if (this.Application.DefaultVersion == ExcelVersion.Excel97to2003)
      workbook.BeginVersion = 2;
    this.Add((IWorkbook) workbook);
    workbook.Activate();
    this.SetAplicatioName((IWorkbook) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Create()
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, this.Application.DefaultVersion);
    if (this.Application.DefaultVersion == ExcelVersion.Excel97to2003)
      workbook.BeginVersion = 2;
    this.Add((IWorkbook) workbook);
    workbook.Activate();
    this.SetAplicatioName((IWorkbook) workbook);
    return (IWorkbook) workbook;
  }

  private void SetAplicatioName(IWorkbook book)
  {
    book.BuiltInDocumentProperties.ApplicationName = "Essential XlsIO";
  }

  public IWorkbook Add() => this.Add((string) null, this.Application.DefaultVersion);

  public IWorkbook Add(ExcelVersion version) => this.Add((string) null, version);

  public IWorkbook Add(string strTemplateFile)
  {
    return this.Add(strTemplateFile, ExcelParseOptions.Default);
  }

  public IWorkbook Add(string strTemplateFile, ExcelVersion version)
  {
    return this.Add(strTemplateFile, ExcelParseOptions.Default, version);
  }

  public IWorkbook Add(string strTemplateFile, ExcelParseOptions options)
  {
    ExcelVersion version = this.DetectVersion(strTemplateFile);
    return this.Add(strTemplateFile, options, version);
  }

  public IWorkbook Add(string strTemplateFile, ExcelParseOptions options, ExcelVersion version)
  {
    WorkbookImpl workbookImpl = strTemplateFile != null ? this.AppImplementation.CreateWorkbook((object) this, strTemplateFile, options, version) : this.AppImplementation.CreateWorkbook((object) this, version);
    this.Add((IWorkbook) workbookImpl);
    workbookImpl.Activate();
    workbookImpl.Worksheets[0].Activate();
    return (IWorkbook) workbookImpl;
  }

  public IWorkbook Open(string filename) => this.Open(filename, ExcelOpenType.Automatic);

  public IWorkbook Open(string filename, ExcelVersion version)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, filename, version);
    workbook.InternalSaved = true;
    this.Add((IWorkbook) workbook);
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
    ExcelVersion version)
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
    ExcelVersion version)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, stream, separator, row, column, version, fileName, encoding);
    this.Add((IWorkbook) workbook);
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

  public IWorkbook Open(Stream stream, string separator, ExcelVersion version)
  {
    return this.Open(stream, separator, 1, 1, (string) null, (Encoding) null, version);
  }

  public IWorkbook Open(string fileName, string separator) => this.Open(fileName, separator, 1, 1);

  public IWorkbook Open(Stream stream, string separator, Encoding encoding)
  {
    return this.Open(stream, separator, 1, 1, encoding);
  }

  public IWorkbook Open(Stream stream, Encoding encoding) => this.Open(stream, ",", 1, 1, encoding);

  public IWorkbook Open(string fileName, string separator, Encoding encoding)
  {
    return this.Open(fileName, separator, 1, 1, encoding);
  }

  public IWorkbook Open(string fileName, Encoding encoding)
  {
    return this.Open(fileName, ",", 1, 1, encoding);
  }

  public IWorkbook Open(string fileName, ExcelParseOptions options)
  {
    return this.Open(fileName, ExcelOpenType.Automatic, options);
  }

  public IWorkbook Open(
    string fileName,
    ExcelParseOptions options,
    bool isReadOnly,
    string password)
  {
    return this.Open(fileName, options, isReadOnly, password, ExcelOpenType.Automatic);
  }

  public IWorkbook Open(
    Stream stream,
    ExcelParseOptions options,
    bool isReadOnly,
    string password)
  {
    return this.Open(stream, options, isReadOnly, password, this.Application.DefaultVersion);
  }

  public IWorkbook Open(
    Stream stream,
    ExcelParseOptions options,
    bool isReadOnly,
    string password,
    ExcelVersion excelVersion)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, stream, options, isReadOnly, password, excelVersion);
    this.Add((IWorkbook) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Open(
    Stream stream,
    ExcelParseOptions options,
    bool isReadOnly,
    string password,
    ExcelOpenType openType)
  {
    if (openType == ExcelOpenType.Automatic)
      openType = this.AppImplementation.DetectFileFromStream(stream);
    switch (openType)
    {
      case ExcelOpenType.CSV:
        return this.Open(stream, this.Application.CSVSeparator);
      case ExcelOpenType.TSV:
        return this.Open(stream, "\t");
      case ExcelOpenType.SpreadsheetML:
        return this.OpenFromXml(stream, ExcelXmlOpenType.MSExcel);
      case ExcelOpenType.BIFF:
        return this.Open(stream, options, isReadOnly, password, ExcelVersion.Excel97to2003);
      case ExcelOpenType.SpreadsheetML2007:
        return this.Open(stream, options, isReadOnly, password, ExcelVersion.Excel2007);
      case ExcelOpenType.SpreadsheetML2010:
        return this.Open(stream, options, isReadOnly, password, ExcelVersion.Excel2010);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  public IWorkbook Open(
    string fileName,
    ExcelParseOptions options,
    bool isReadOnly,
    string password,
    ExcelVersion version)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, fileName, options, isReadOnly, password, version);
    this.Add((IWorkbook) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Open(
    string fileName,
    ExcelParseOptions options,
    bool isReadOnly,
    string password,
    ExcelOpenType openType)
  {
    if (isReadOnly)
    {
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        if (openType == ExcelOpenType.Automatic)
          openType = this.AppImplementation.DetectFileFromStream((Stream) fileStream);
      }
    }
    else
    {
      using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        if (openType == ExcelOpenType.Automatic)
          openType = this.AppImplementation.DetectFileFromStream((Stream) fileStream);
      }
    }
    if (openType == ExcelOpenType.Automatic)
      throw new ArgumentException("Cannot recognize current file type.");
    if (openType == ExcelOpenType.TSV && Path.GetExtension(fileName) == ".csv")
      openType = ExcelOpenType.CSV;
    else if (openType == ExcelOpenType.CSV && Path.GetExtension(fileName) == ".tsv")
      openType = ExcelOpenType.TSV;
    switch (openType)
    {
      case ExcelOpenType.CSV:
        return this.Open(fileName, this.Application.CSVSeparator);
      case ExcelOpenType.TSV:
        return this.Open(fileName, "\t");
      case ExcelOpenType.SpreadsheetML:
        return this.OpenFromXml(fileName, ExcelXmlOpenType.MSExcel);
      case ExcelOpenType.BIFF:
        return this.Open(fileName, options, isReadOnly, password, ExcelVersion.Excel97to2003);
      case ExcelOpenType.SpreadsheetML2007:
        return this.Open(fileName, options, isReadOnly, password, ExcelVersion.Excel2007);
      case ExcelOpenType.SpreadsheetML2010:
        return this.Open(fileName, options, isReadOnly, password, ExcelVersion.Excel2010);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  public IWorkbook Open(Stream stream) => this.Open(stream, ExcelOpenType.Automatic);

  public IWorkbook Open(Stream stream, ExcelVersion version)
  {
    return this.Open(stream, version, ExcelParseOptions.Default);
  }

  public IWorkbook Open(Stream stream, ExcelVersion version, ExcelParseOptions options)
  {
    WorkbookImpl workbook = this.AppImplementation.CreateWorkbook((object) this, stream, version, options);
    this.Add((IWorkbook) workbook);
    return (IWorkbook) workbook;
  }

  public IWorkbook Open(Stream stream, ExcelParseOptions options)
  {
    return this.Open(stream, ExcelOpenType.Automatic, options);
  }

  public IWorkbook Open(string fileName, ExcelOpenType openType)
  {
    return this.Open(fileName, openType, ExcelParseOptions.Default);
  }

  public IWorkbook Open(string fileName, ExcelOpenType openType, ExcelParseOptions options)
  {
    return this.Open(fileName, openType, this.Application.DefaultVersion, options);
  }

  public IWorkbook Open(string fileName, ExcelOpenType openType, ExcelVersion version)
  {
    return this.Open(fileName, openType, version, ExcelParseOptions.Default);
  }

  public IWorkbook Open(
    string fileName,
    ExcelOpenType openType,
    ExcelVersion version,
    ExcelParseOptions options)
  {
    if (!File.Exists(fileName))
      throw new FileNotFoundException($"File {fileName} could not be found. Please verify the file path.");
    using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
    {
      WorkbookImpl workbookImpl = (WorkbookImpl) this.Open((Stream) fileStream, openType, fileName, version, options);
      workbookImpl.FullFileName = Path.GetFullPath(fileName);
      return (IWorkbook) workbookImpl;
    }
  }

  public IWorkbook Open(Stream stream, ExcelOpenType openType, ExcelVersion version)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (openType == ExcelOpenType.Automatic)
      openType = this.AppImplementation.DetectFileFromStream(stream);
    if (openType == ExcelOpenType.Automatic)
      throw new ArgumentException("Cannot recognize current file type.");
    switch (openType)
    {
      case ExcelOpenType.CSV:
        return this.Open(stream, this.Application.CSVSeparator, version);
      case ExcelOpenType.TSV:
        return this.Open(stream, "\t", version);
      case ExcelOpenType.SpreadsheetML:
        return this.OpenFromXml(stream, ExcelXmlOpenType.MSExcel);
      case ExcelOpenType.BIFF:
        return this.Open(stream, ExcelVersion.Excel97to2003);
      case ExcelOpenType.SpreadsheetML2007:
        return this.Open(stream, ExcelVersion.Excel2007);
      case ExcelOpenType.SpreadsheetML2010:
        return this.Open(stream, ExcelVersion.Excel2010);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  public IWorkbook Open(Stream stream, ExcelOpenType openType)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (openType == ExcelOpenType.Automatic)
      openType = this.AppImplementation.DetectFileFromStream(stream);
    if (openType == ExcelOpenType.Automatic)
      throw new ArgumentException("Cannot recognize current file type.");
    switch (openType)
    {
      case ExcelOpenType.CSV:
        return this.Open(stream, this.Application.CSVSeparator);
      case ExcelOpenType.TSV:
        return this.Open(stream, "\t");
      case ExcelOpenType.SpreadsheetML:
        return this.OpenFromXml(stream, ExcelXmlOpenType.MSExcel);
      case ExcelOpenType.BIFF:
        return this.Open(stream, ExcelVersion.Excel97to2003);
      case ExcelOpenType.SpreadsheetML2007:
        return this.Open(stream, ExcelVersion.Excel2007);
      case ExcelOpenType.SpreadsheetML2010:
        return this.Open(stream, ExcelVersion.Excel2010);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  public IWorkbook Open(Stream stream, ExcelOpenType openType, ExcelParseOptions options)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (openType == ExcelOpenType.Automatic)
      openType = this.AppImplementation.DetectFileFromStream(stream);
    if (openType == ExcelOpenType.Automatic)
      throw new ArgumentException("Cannot recognize current file type.");
    switch (openType)
    {
      case ExcelOpenType.CSV:
        return this.Open(stream, this.Application.CSVSeparator);
      case ExcelOpenType.TSV:
        return this.Open(stream, "\t");
      case ExcelOpenType.SpreadsheetML:
        return this.OpenFromXml(stream, ExcelXmlOpenType.MSExcel);
      case ExcelOpenType.BIFF:
        return this.Open(stream, ExcelVersion.Excel97to2003, options);
      case ExcelOpenType.SpreadsheetML2007:
        return this.Open(stream, ExcelVersion.Excel2007, options);
      case ExcelOpenType.SpreadsheetML2010:
        return this.Open(stream, ExcelVersion.Excel2010, options);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  private IWorkbook Open(
    Stream stream,
    ExcelOpenType openType,
    string fileName,
    ExcelVersion version)
  {
    return this.Open(stream, openType, fileName, version, ExcelParseOptions.Default);
  }

  private IWorkbook Open(
    Stream stream,
    ExcelOpenType openType,
    string fileName,
    ExcelVersion version,
    ExcelParseOptions options)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (openType == ExcelOpenType.Automatic)
      openType = this.AppImplementation.DetectFileFromStream(stream);
    if (openType == ExcelOpenType.Automatic)
      throw new ArgumentException("Cannot recognize current file type.");
    if (openType == ExcelOpenType.TSV && Path.GetExtension(fileName) == ".csv")
      openType = ExcelOpenType.CSV;
    else if (openType == ExcelOpenType.CSV && Path.GetExtension(fileName) == ".tsv")
      openType = ExcelOpenType.TSV;
    switch (openType)
    {
      case ExcelOpenType.CSV:
        return this.Open(stream, this.Application.CSVSeparator, 1, 1, fileName, (Encoding) null, version);
      case ExcelOpenType.TSV:
        return this.Open(stream, "\t", 1, 1, fileName, (Encoding) null, version);
      case ExcelOpenType.SpreadsheetML:
        return this.OpenFromXml(stream, ExcelXmlOpenType.MSExcel);
      case ExcelOpenType.BIFF:
        return this.Open(stream, ExcelVersion.Excel97to2003, options);
      case ExcelOpenType.SpreadsheetML2007:
        return this.Open(stream, ExcelVersion.Excel2007, options);
      case ExcelOpenType.SpreadsheetML2010:
        return this.Open(stream, ExcelVersion.Excel2010, options);
      default:
        throw new ArgumentOutOfRangeException(nameof (openType));
    }
  }

  public IWorkbook OpenFromXml(string strPath, ExcelXmlOpenType openType)
  {
    if (!File.Exists(strPath))
      throw new FileNotFoundException("File could not be found. Please verify the file path.");
    using (FileStream fileStream = new FileStream(strPath, FileMode.Open, FileAccess.Read, FileShare.Read))
      return this.OpenFromXml((Stream) fileStream, openType);
  }

  public IWorkbook OpenFromXml(Stream stream, ExcelXmlOpenType openType)
  {
    stream = stream != null ? this.ReplaceValidXmlStream(stream) : throw new ArgumentNullException(nameof (stream));
    XmlReader xmlReader = XmlReader.Create((TextReader) new StreamReader(stream));
    string empty = string.Empty;
    XmlReader reader = (XmlReader) null;
    if (xmlReader.Read())
    {
      if (xmlReader.NodeType == XmlNodeType.XmlDeclaration && xmlReader.MoveToAttribute("encoding"))
      {
        string name = xmlReader.Value;
        stream.Position = 0L;
        reader = !(name == "utf-16") ? XmlReader.Create((TextReader) new StreamReader(stream, Encoding.GetEncoding(name))) : XmlReader.Create((TextReader) new StreamReader(stream, Encoding.Default));
      }
    }
    if (reader == null)
    {
      stream.Position = 0L;
      reader = XmlReader.Create((TextReader) new StreamReader(stream));
    }
    return this.OpenFromXml(reader, openType);
  }

  internal Stream ReplaceValidXmlStream(Stream stream)
  {
    string end = new StreamReader(stream).ReadToEnd();
    MatchCollection matchCollection = Regex.Matches(end.Replace("\r\n", string.Empty), "<[\\s]*xml([\\s]+(version|version[\\s]*=|version[\\s]*=[\\s]*\"[\\S]*\")){0,1}[\\s]*>", RegexOptions.Compiled);
    string newValue = "<?xml version=\"1.0\"?>";
    if (matchCollection != null && matchCollection.Count > 0)
      return (Stream) new MemoryStream(Encoding.UTF8.GetBytes(end.Replace(matchCollection[0].ToString(), newValue)));
    stream.Position = 0L;
    return stream;
  }

  public IWorkbook OpenFromXml(XmlReader reader, ExcelXmlOpenType openType)
  {
    return (reader != null ? this.OpenFromXmlInternal(reader, openType) : throw new ArgumentNullException(nameof (reader))) ?? throw new ApplicationException("Unable to read from XML.");
  }

  private IWorkbook OpenFromXmlInternal(XmlReader reader, ExcelXmlOpenType openType)
  {
    if (reader is XmlTextReader xmlTextReader)
      xmlTextReader.WhitespaceHandling = WhitespaceHandling.Significant;
    WorkbookImpl workbookImpl = new WorkbookImpl(this.Application, (object) this, reader, openType);
    if (workbookImpl == null)
      return (IWorkbook) workbookImpl;
    this.Add((IWorkbook) workbookImpl);
    return (IWorkbook) workbookImpl;
  }

  public IWorkbook OpenReadOnly(string strFileName)
  {
    return this.OpenReadOnly(strFileName, ExcelOpenType.Automatic, ExcelParseOptions.Default);
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

  public IWorkbook OpenReadOnly(string strFileName, ExcelParseOptions options)
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
    ExcelOpenType openType,
    ExcelParseOptions options)
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
    if (activeWorkbook == null || this.InnerList.IndexOf(activeWorkbook) < 0 || this.InnerList.Count <= 0)
      return;
    if ((activeWorkbook as WorkbookImpl).ReadOnly)
      activeWorkbook.Close(false, (string) null);
    else
      activeWorkbook.Close(true, (string) null);
  }

  public IWorkbook PasteWorkbook()
  {
    return this.AppImplementation.CreateClipboardProvider().GetBookFromClipboard((IWorkbooks) this);
  }

  private ExcelVersion DetectVersion(string strTemplateFile)
  {
    if (strTemplateFile == null || !File.Exists(strTemplateFile))
      return this.Application.DefaultVersion;
    ExcelVersion defaultVersion = this.Application.DefaultVersion;
    using (FileStream fileStream = new FileStream(strTemplateFile, FileMode.Open, FileAccess.Read, FileShare.Read))
    {
      switch (this.AppImplementation.DetectFileFromStream((Stream) fileStream))
      {
        case ExcelOpenType.BIFF:
          return ExcelVersion.Excel97to2003;
        case ExcelOpenType.SpreadsheetML2007:
          return ExcelVersion.Excel2007;
        case ExcelOpenType.SpreadsheetML2010:
          return ExcelVersion.Excel2010;
        case ExcelOpenType.Automatic:
          throw new ArgumentException("Cannot recognize current file type.");
        default:
          throw new ArgumentOutOfRangeException("openType");
      }
    }
  }
}
